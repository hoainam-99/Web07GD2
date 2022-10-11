using Misa.CukCukMaterial.CTM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.DL
{
    public interface IUnitDL : IBaseDL<Unit>
    {
        /// <summary>
        /// Hàm kiểm tra trùng tên đơn vị tính
        /// </summary>
        /// <param name="method">phương thức sử dụng</param>
        /// <param name="id">ID của đơn vị tính</param>
        /// <param name="name">Tên đơn vị tính</param>
        /// <returns>Giá trị true/false </returns>
        public bool CheckDuplicateUnitName(Method method, Guid id, string name);
    }
}
