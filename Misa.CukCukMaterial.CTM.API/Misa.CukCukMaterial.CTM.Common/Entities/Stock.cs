using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.Common
{
    /// <summary>
    /// Class Stock định dạng dữ liệu lấy ra từ bảng unit
    /// </summary>
    /// Author: LHNAM (29/09/2022)
    [Table("unit")]
    public class Stock
    {
        /// <summary>
        /// ID kho
        /// </summary>
        [Key]
        public Guid StockID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Mã kho
        /// </summary>
        public String StockCode { get; set; }

        /// <summary>
        /// Tên kho
        /// </summary>
        public String StockName { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public String? CreatedBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Người sửa
        /// </summary>
        public String? ModifiedBy { get; set; }

        /// <summary>
        /// Ngày sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
    }
}
