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
        public bool CheckDuplicateCode(Method method, Guid id, string code)
        {
            // Chuẩn bị stored Proc 
            string className = typeof(T).Name;
            string storedProc = $"Proc_{className}_CheckDuplicateCode";

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

        public int DeleteOneRecord(Guid id)
        {
            // Chuẩn bị Stored Procdure
            string className = typeof(T).Name;
            string storedProc = $"Proc_{className}_DeleteOne";

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

        public virtual T GetOneRecordByID(Guid id)
        {
            // Chuẩn bị stored Proc
            string className = typeof(T).Name;
            string storedProc = $"Proc_{className}_GetByID";

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

        public IEnumerable<T> GetRecords()
        {
            // Chuẩn bị câu lệnh SELECT 
            string className = typeof(T).Name;
            string storedProc = $"Proc_{className}_GetAll";

            using (var mySqlConnection = new MySqlConnection(DatabaseContext.ConnectionString))
            {
                // Thực hiện gọi câu lệnh SELECT 
                var records = mySqlConnection.Query<T>(storedProc, commandType: System.Data.CommandType.StoredProcedure);

                return records;
            }
        }

        public virtual Guid InsertOneRecord(T record)
        {
            // Chuẩn bị stored Proc
            string className = EntityUntilities.GetTableName<T>();
            string storedProc = $"Proc_{className}_InsertOne";

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

        public virtual Guid UpdateOneRecord(Guid id, T record)
        {
            // Chuẩn bị stored Proc
            string className = EntityUntilities.GetTableName<T>();
            string storedProc = $"Proc_{className}_UpdateOne";

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
