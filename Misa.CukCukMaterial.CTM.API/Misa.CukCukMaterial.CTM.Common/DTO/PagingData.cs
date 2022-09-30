using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.Common
{
    /// <summary>
    /// Class định dạng dữ liệu trả về khi lọc và phân trang
    /// </summary>
    /// <typeparam name="T">Kiểu dữ liệu của đối tượng trong mảng trả về</typeparam>
    /// Author: LHNAM (29/09/2022)
    public class PagingData<T>
    {
        /// <summary>
        /// Mảng các đối tượng được trả về thỏa mãn với điều kiện phân trang
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// Số bản ghi thỏa mãn điều kiện
        /// </summary>
        public long TotalCount { get; set; }
    }
}
