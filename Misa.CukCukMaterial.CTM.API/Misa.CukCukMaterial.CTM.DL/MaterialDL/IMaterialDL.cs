using Misa.CukCukMaterial.CTM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.DL
{
    public interface IMaterialDL : IBaseDL<Material>
    {
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
        public PagingData<Material> FilterRecord(string? filterCondition, int pageNum, int pageSize);

        /// <summary>
        /// Hàm lấy mã nguyên vật liệu mới
        /// </summary>
        /// <returns>Mã nguyên vật liệu mới</returns>
        /// Author: LHNAM (01/10/2022)
        public string GetNewMaterialCode();
    }
}
