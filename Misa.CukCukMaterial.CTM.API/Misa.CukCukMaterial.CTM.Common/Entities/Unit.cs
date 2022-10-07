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
    /// Class Unit định dạng dữ liệu lấy từ bảng Unit
    /// </summary>
    /// Author: LHNAM (29/09/2022)
    [Table("unit")]
    public class Unit : BaseEntity
    {
        /// <summary>
        /// ID đơn vị
        /// </summary>
        [Key]
        public Guid UnitID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public String UnitName { get; set; }
    }
}
