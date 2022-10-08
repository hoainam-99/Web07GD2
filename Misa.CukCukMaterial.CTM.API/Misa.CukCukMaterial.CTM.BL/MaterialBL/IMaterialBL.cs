using Misa.CukCukMaterial.CTM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    public interface IMaterialBL : IBaseBL<Material>
    {
        /// <summary>
        /// Hàm xử lý logic lọc và phân trang
        /// </summary>
        /// <param name="filterData">đối tượng lọc và phân trang</param>
        /// <returns>
        ///     Một đối tượng gồm: 
        ///         + Danh sách nhân viên thỏa mãn điều kiện phân trang và tìm kiếm
        ///         + Tổng số bản ghi thỏa mãn điều kiện tìm kiếm
        /// </returns>
        /// Author: LHNAM (30/09/2022)
        public PagingData<Material> FilterRecords(FilterData filterData);

        /// <summary>
        /// Hàm lấy mã nguyên vật liệu mới
        /// </summary>
        /// <returns>Mã nguyên vật liệu mới</returns>
        /// Author: LHNAM (01/10/2022)
        public string GetNewMaterialCode(string code);
    }
}
