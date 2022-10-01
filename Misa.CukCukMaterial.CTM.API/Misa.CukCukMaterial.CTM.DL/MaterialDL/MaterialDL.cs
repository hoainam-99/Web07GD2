using Dapper;
using Misa.CukCukMaterial.CTM.Common;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString)){
                var record = mySqlConnection.QueryMultiple(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                Material material = record.Read<Material>().Single();
                material.MaterialUnit = record.Read<MaterialUnit>().ToList();

                return material;
            }
        }
    }
}
