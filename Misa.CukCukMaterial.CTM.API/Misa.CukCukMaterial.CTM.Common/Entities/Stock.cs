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
    [Table("stock")]
    public class Stock : BaseEntity
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
    }
}
