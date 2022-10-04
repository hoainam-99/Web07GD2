using Dapper;
using Misa.CukCukMaterial.CTM.Common;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Misa.CukCukMaterial.CTM.DL
{
    public class MaterialDL : BaseDL<Material>, IMaterialDL
    {
        public PagingData<Material> FilterRecord(string? filterCondition, int pageNum, int pageSize)
        {
            string storedProc = "Proc_material_GetPaging";

            var parameters = new DynamicParameters();

            parameters.Add("@$Offset", (pageNum - 1) * pageSize);
            parameters.Add("@$Limit", pageSize);
            parameters.Add("@$Sort", "ModifiedDate DESC");
            parameters.Add("@$Where", filterCondition);

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var multipleResult = mySqlConnection.QueryMultiple(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);


                return new PagingData<Material>()
                {
                    Data = multipleResult.Read<Material>().ToList(),
                    TotalCount = multipleResult.Read<long>().Single()
                };
            }
        }

        public string GetNewMaterialCode()
        {
            string func = "Select Func_Get_Auto_MaterialCode()";

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var newCode = mySqlConnection.QueryFirstOrDefault<string>(func);

                return newCode;
            }
        }

        public override Material GetOneRecordByID(Guid id)
        {
            string storedProc = "Proc_material_GetByID";

            var parameters = new DynamicParameters();

            parameters.Add("@$MaterialID", id);

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var record = mySqlConnection.QueryMultiple(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                Material material = record.Read<Material>().Single();
                material.MaterialUnit = record.Read<MaterialUnit>().ToList(); 

                foreach(var item in material.MaterialUnit)
                {
                    item.OldUnitID = item.UnitID;
                }

                return material;
            }
        }

        public override Guid InsertOneRecord(Material record)
        {
            string materialInsertProc = "Proc_material_InsertOne";
            string materialUnitInsertProc = "Proc_materialunit_InsertOne";

            var materialParameters = new DynamicParameters();

            var materialProps = typeof(Material).GetProperties();
            var materialUnits = new List<MaterialUnit>();

            foreach (var prop in materialProps)
            {
                if (prop.Name == "MaterialUnit")
                {
                    materialUnits = (List<MaterialUnit>?)prop.GetValue(record);
                    continue;
                }
                string propName = $"@${prop.Name}";
                var propValue = prop.GetValue(record);

                materialParameters.Add(propName, propValue);
            }

            using (var transaction = new TransactionScope())
            {
                int affectedRow = 0;
                using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
                {
                    affectedRow = mySqlConnection.Execute(materialInsertProc, materialParameters, commandType: System.Data.CommandType.StoredProcedure);
                    foreach (var materialUnit in materialUnits)
                    {
                        var materialUnitParameter = new DynamicParameters();
                        var materialUnitProps = typeof(MaterialUnit).GetProperties();

                        foreach (var prop in materialUnitProps)
                        {
                            string propName = $"@${prop.Name}";
                            var propValue = prop.GetValue(materialUnit);
                            if (prop.Name == "MaterialID")
                            {
                                propValue = record.MaterialID;
                            }

                            materialUnitParameter.Add(propName, propValue);
                        }

                        int materialUnitAffectedRow = mySqlConnection.Execute(materialUnitInsertProc, materialUnitParameter, commandType: System.Data.CommandType.StoredProcedure);
                    }
                }

                transaction.Complete();
                var result = Guid.Empty;
                if (affectedRow > 0)
                {
                    var primaryKeyProp = typeof(Material).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                    var newID = primaryKeyProp?.GetValue(record);

                    if (newID != null)
                    {
                        result = (Guid)newID;
                    }
                }

                return result;
            }
        }

        public override Guid UpdateOneRecord(Guid id, Material record)
        {
            string materialProc = "Proc_material_UpdateOne";
            string materialUnitInsertProc = "Proc_materialunit_InsertOne";
            string materialUnitUpdateProc = "Proc_materialunit_UpdateOne";
            string materialUnitDeleteProc = "Proc_materialunit_DeleteOne";

            var materialParameters = new DynamicParameters();

            var materialProps = typeof(Material).GetProperties();
            var materialUnits = new List<MaterialUnit>();

            foreach (var prop in materialProps)
            {
                if (prop.Name == "MaterialUnit")
                {
                    materialUnits = (List<MaterialUnit>?)prop.GetValue(record);
                    continue;
                }
                string propName = $"@${prop.Name}";
                var propValue = prop.GetValue(record);

                materialParameters.Add(propName, propValue);
            }

            using (var transaction = new TransactionScope())
            {
                int affectedRow = 0;
                using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
                {
                    affectedRow = mySqlConnection.Execute(materialProc, materialParameters, commandType: System.Data.CommandType.StoredProcedure);
                    foreach (var materialUnit in materialUnits)
                    {
                        var materialUnitParameter = new DynamicParameters();
                        var materialUnitProps = typeof(MaterialUnit).GetProperties();

                        foreach (var prop in materialUnitProps)
                        {
                            string propName = $"@${prop.Name}";
                            var propValue = prop.GetValue(materialUnit);
                            if (prop.Name == "MaterialID")
                            {
                                propValue = record.MaterialID;
                            }

                            materialUnitParameter.Add(propName, propValue);
                        }
                        int materialUnitAffectedRow = 0;
                        switch (materialUnit.Method)
                        {
                            case Method.Add:
                                materialUnitAffectedRow = mySqlConnection.Execute(materialUnitInsertProc, materialUnitParameter, commandType: System.Data.CommandType.StoredProcedure);
                                break;
                            case Method.Edit:
                                materialUnitAffectedRow = mySqlConnection.Execute(materialUnitUpdateProc, materialUnitParameter, commandType: System.Data.CommandType.StoredProcedure);
                                break;
                            case Method.Delete:
                                materialUnitAffectedRow = mySqlConnection.Execute(materialUnitDeleteProc, materialUnitParameter, commandType: System.Data.CommandType.StoredProcedure);
                                break;
                        }
                    }
                }

                transaction.Complete();
                var result = Guid.Empty;
                if (affectedRow > 0)
                {
                    result = (Guid)id;
                }

                return result;
            }
        }
    }
}
