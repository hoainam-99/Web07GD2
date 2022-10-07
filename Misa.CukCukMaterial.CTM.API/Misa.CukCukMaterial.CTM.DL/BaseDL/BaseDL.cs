using Dapper;
using Misa.CukCukMaterial.CTM.Common;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.DL
{
    /// <summary>
    /// Class BaseDL dùng để triển khai Base Data Access Layer
    /// </summary>
    /// Author: LHNAM (29/09/2022)
    public class BaseDL<T> : IBaseDL<T>
    {
        #region Method

        /// <summary>
        /// Kiểm tra trùng mã 
        /// </summary>
        /// <param name="code">Code cần kiểm tra</param>
        /// <returns>Giá trị true, false</returns>
        /// Author: LHNAM (29/09/2022)
        public bool CheckDuplicateCode(Method method, Guid id, string code)
        {
            // Chuẩn bị stored Proc 
            string className = typeof(T).Name;
            string storedProc = String.Format(Common.Resource.ResourceVN.Procedure_CheckDuplicateCode, className);

            // Chuẩn bị tham số
            var parameters = new DynamicParameters();

            parameters.Add("@$Code", code);

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

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của đối tượng cần xóa</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Author: LHNAM (29/09/2022)
        public int DeleteOneRecord(Guid id)
        {
            // Chuẩn bị Stored Procdure
            string className = typeof(T).Name;
            string storedProc = String.Format(Common.Resource.ResourceVN.Procedure_DeleteOne, className);

            // Chuẩn bị tham số
            var parameters = new DynamicParameters();

            parameters.Add($"@${className}ID", id);

            // Thực hiện proc với tham số ở trên
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                int affectedRow = mySqlConnection.Execute(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return affectedRow;
            }
        }

        /// <summary>
        /// Lấy dữ liệu của 1 bản ghi bằng ID
        /// </summary>
        /// <param name="id">ID của đối tượng cần lấy dữ liệu</param>
        /// <returns>Dữ liệu của đối tượng cần lấy</returns>
        /// Author: LHNAM (29/09/2022)
        public virtual T GetOneRecordByID(Guid id)
        {
            // Chuẩn bị stored Proc
            string className = typeof(T).Name;
            string storedProc = String.Format(Common.Resource.ResourceVN.Procedure_GetByID, className);

            // Chuẩn bị tham số
            var parameters = new DynamicParameters();

            parameters.Add($"@${className}ID", id);

            // Thực hiện gọi câu lệnh SELECT 
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                var record = mySqlConnection.QueryFirstOrDefault<T>(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                return record;
            }
        }

        /// <summary>
        /// Lấy tất cả các bản ghi
        /// </summary>
        /// <returns>Các bản ghi của 1 bảng</returns>
        /// Author: LHNAM (29/09/2022)
        public IEnumerable<T> GetRecords()
        {
            // Chuẩn bị câu lệnh SELECT 
            string className = typeof(T).Name;
            string storedProc = String.Format(Common.Resource.ResourceVN.Procedure_GetAll, className);

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi câu lệnh SELECT 
                var records = mySqlConnection.Query<T>(storedProc, commandType: System.Data.CommandType.StoredProcedure);

                return records;
            }
        }

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <param name="record">Thông tin của bản ghi cần thêm</param>
        /// <returns>ID của bản ghi được thêm thành công</returns>
        /// Author: LHNAM (29/09/2022)
        public virtual Guid InsertOneRecord(T record)
        {
            // Chuẩn bị stored Proc
            string className = EntityUtilities.GetTableName<T>();
            string storedProc = String.Format(Common.Resource.ResourceVN.Procedure_InsertOne, className);

            // Chuẩn bị tham số
            var parameters = new DynamicParameters();

            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var propName = $"@${prop.Name}";
                var propValue = prop.GetValue(record);
                parameters.Add(propName, propValue);
            }

            // Thực hiện lệnh với tham số đầu vào
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                int affectedRow = mySqlConnection.Execute(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

                var result = Guid.Empty;
                if (affectedRow > 0)
                {
                    var primaryKeyProp = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);
                    var newID = primaryKeyProp?.GetValue(record);

                    if (newID != null)
                    {
                        result = (Guid)newID;
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Sửa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của bản ghi cần sửa</param>
        /// <param name="record">Thông tin của bản ghi cần sửa</param>
        /// <returns>ID của bản ghi được sửa thành công</returns>
        /// Author: LHNAM (29/09/2022)
        public virtual Guid UpdateOneRecord(Guid id, T record)
        {
            // Chuẩn bị stored Proc
            string className = EntityUtilities.GetTableName<T>();
            string storedProc = String.Format(Common.Resource.ResourceVN.Procedure_UpdateOne, className);

            // Chuẩn bị tham số
            var parameters = new DynamicParameters();

            var props = typeof(T).GetProperties();

            foreach (var prop in props)
            {
                var propName = $"@${prop.Name}";
                var propValue = prop.GetValue(record);
                parameters.Add(propName, propValue);
            }

            // Thực hiện lệnh với tham số đầu vào
            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                int affectedRow = mySqlConnection.Execute(storedProc, parameters, commandType: System.Data.CommandType.StoredProcedure);

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
