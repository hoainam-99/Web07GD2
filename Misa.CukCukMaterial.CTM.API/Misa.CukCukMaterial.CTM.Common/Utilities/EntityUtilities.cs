using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.Common
{
    /// <summary>
    /// Hàm chung xử lý Entity
    /// </summary>
    public static class EntityUtilities
    {
        /// <summary>
        /// Lấy tên bảng của entity
        /// </summary>
        /// <typeparam name="T">Kiểu dữ liệu của entity</typeparam>
        /// <returns>Tên bảng</returns>
        /// LHNAM (25/08/2022)
        public static string GetTableName<T>()
        {
            string tableName = typeof(T).Name;
            var tableAttr = typeof(T).GetTypeInfo().GetCustomAttributes<TableAttribute>();

            if (tableAttr.Count() > 0)
            {
                tableName = tableAttr.First().Name;
            }

            return tableName;
        }
    }
}
