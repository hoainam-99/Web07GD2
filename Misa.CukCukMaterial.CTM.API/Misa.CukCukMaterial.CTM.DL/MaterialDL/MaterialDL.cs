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
        #region method

        /// <summary>
        /// Hàm kiểm tra trùng đơn vị chuyển đổi
        /// </summary>
        /// <param name="method">kiểu query thực hiện</param>
        /// <param name="materialID">ID nguyên vật liệu cần kiểm tra</param>
        /// <param name="unitID">ID đơn vị chuyển đổi cần kiểm tra</param>
        /// <returns>Giá trị true/false dùng để validate</returns>
        public bool CheckDuplicateMaterialUnit(Method method, Guid materialID, Guid unitID, Guid oldUnitID)
        {
            string procName = Common.Resource.ResourceVN.Proc_materialUnit_CheckDuplicate;

            var parameters = new DynamicParameters();
            parameters.Add("@$MaterialID", materialID);
            parameters.Add("@$UnitID", unitID);

            using(var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                Guid result = mySqlConnection.QueryFirstOrDefault<Guid>(procName, parameters, commandType: System.Data.CommandType.StoredProcedure);

                bool isDuplicate = false;
                switch (method)
                {
                    case Method.Add:
                        if(result != Guid.Empty)
                        {
                            isDuplicate = true;
                        }
                        break;
                    case Method.Edit:
                        if(unitID == oldUnitID)
                        {
                            if(result != unitID)
                            {
                                isDuplicate = true;
                            }
                        }
                        else
                        {
                            if(result != Guid.Empty)
                            {
                                isDuplicate = true;
                            }
                        }
                        break;
                }

                return isDuplicate;
            }
        }

        /// <summary>
        /// Hàm xử lý logic lọc và phân trang
        /// </summary>
        /// <param name="filterCondition">điều kiện lọc</param>
        /// <param name="pageNum">số trang</param>
        /// <param name="pageSize">số bản ghi trên 1 trang</param>
        /// <returns>
        ///     Một đối tượng gồm: 
        ///         + Danh sách nhân viên thỏa mãn điều kiện phân trang và tìm kiếm
        ///         + Tổng số bản ghi thỏa mãn điều kiện tìm kiếm
        /// </returns>
        /// Author: LHNAM (30/09/2022)
        public PagingData<Material> FilterRecord(string? filterCondition, int pageNum, int pageSize)
        {
            string storedProc = Common.Resource.ResourceVN.Proc_material_GetPaging;

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

        /// <summary>
        /// Hàm lấy mã nguyên vật liệu mới
        /// </summary>
        /// <returns>Mã nguyên vật liệu mới</returns>
        /// Author: LHNAM (01/10/2022)
        public string GetNewMaterialCode(string code)
        {
            string storedProd = Common.Resource.ResourceVN.Proc_GetAuto_MaterialCode;

            var paramaters = new DynamicParameters();
            paramaters.Add("@$Code", code);

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var newCode = mySqlConnection.QueryFirstOrDefault<string>(storedProd, paramaters, commandType: System.Data.CommandType.StoredProcedure);

                if(newCode != null)
                {
                    return String.Concat(code, int.Parse(newCode) + 1);
                }
                else
                {
                    return String.Concat(code, "1");
                }
            }
        }

        /// <summary>
        /// Lấy dữ liệu của 1 bản ghi bằng ID
        /// </summary>
        /// <param name="id">ID của đối tượng cần lấy dữ liệu</param>
        /// <returns>Dữ liệu của đối tượng cần lấy</returns>
        /// Author: LHNAM (03/10/2022)
        public override Material GetOneRecordByID(Guid id)
        {
            string storedProc = Common.Resource.ResourceVN.Proc_material_GetByID;

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

        /// <summary>
        /// Sửa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của bản ghi cần sửa</param>
        /// <param name="record">Thông tin của bản ghi cần sửa</param>
        /// <returns>ID của bản ghi được sửa thành công</returns>
        /// Author: LHNAM (03/10/2022)
        public override Guid InsertOneRecord(Material record)
        {
            string materialInsertProc = Common.Resource.ResourceVN.Proc_material_InsertOne;
            string materialUnitInsertProc = "Proc_materialunit_InsertOne";

            DynamicParameters materialParameters;
            List<MaterialUnit>? materialUnits;
            GetMaterialParamaters(record, out materialParameters, out materialUnits);

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

        private void GetMaterialParamaters(Material record, out DynamicParameters materialParameters, out List<MaterialUnit>? materialUnits)
        {
            materialParameters = new DynamicParameters();
            var materialProps = typeof(Material).GetProperties();
            materialUnits = new List<MaterialUnit>();
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="record"></param>
        /// <returns></returns>
        public override Guid UpdateOneRecord(Guid id, Material record)
        {
            string materialProc = Common.Resource.ResourceVN.Proc_material_UpdateOne;
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

        #endregion
    }
}
