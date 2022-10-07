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
    /// Class Category định dạng dữ liệu lấy từ bảng category
    /// </summary>
    /// Author: LHNAM (29/09/2022)
    [Table("category")]
    public class Category : BaseEntity
    {
        /// <summary>
        /// ID nhóm nguyên vật liệu
        /// </summary>
        [Key]
        public Guid CategoryID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Mã nhóm nguyên vật liệu
        /// </summary>
        public String CategoryCode { get; set; }

        /// <summary>
        /// Tên nhóm nguyên vật liệu
        /// </summary>
        public String CategoryName { get; set; }
    }
}
