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
    public class UnitDL : BaseDL<Unit>, IUnitDL
    {
        /// <summary>
        /// Hàm kiểm tra trùng tên đơn vị tính
        /// </summary>
        /// <param name="method">phương thức sử dụng</param>
        /// <param name="id">ID của đơn vị tính</param>
        /// <param name="name">Tên đơn vị tính</param>
        /// <returns>Giá trị true/false </returns>
        public bool CheckDuplicateUnitName(Method method, Guid id, string name)
        {
            string storedProc = Common.Resource.ResourceVN.Proc_unit_CheckDuplicateName;

            var parameters = new DynamicParameters();
            parameters.Add("@$UnitName", name);

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var result = mySqlConnection.QueryFirstOrDefault<Guid>(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                bool isDuplicate = false;
                switch (method)
                {
                    case Method.Add:
                        if (result != Guid.Empty)
                        {
                            isDuplicate = true;
                        }
                        break;
                    case Method.Edit:
                        if (result != id)
                        {
                            isDuplicate = true;
                        }
                        break;
                }

                return isDuplicate;
            }
        }
    }
}
