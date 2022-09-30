using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.Common
{
    public class FilterData
    {
        #region Field

        /// <summary>
        /// Mã nguyên vật liệu
        /// </summary>
        public String? MaterialCode { get; set; }

        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        public String? MaterialName { get; set; }

        /// <summary>
        /// Tính chất
        /// </summary>
        public String? Feature { get; set; }

        /// <summary>
        /// Đơn vị tính
        /// </summary>
        public String? Unit { get; set; }

        /// <summary>
        /// Nhóm nguyên vật liệu
        /// </summary>
        public String? Category { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public String? Description { get; set; }

        /// <summary>
        /// Trạng thái (hoạt động / ngưng hoạt động)
        /// </summary>
        public Status? Status { get; set; }

        /// <summary>
        /// Số bản ghi trong 1 trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Số trang
        /// </summary>
        public int PageNum { get; set; }

        #endregion
    }
}
