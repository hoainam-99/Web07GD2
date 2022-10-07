using Misa.CukCukMaterial.CTM.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.Common
{
    [Table("materialunit")]
    public class MaterialUnit : BaseEntity
    {
        /// <summary>
        /// Mã nguyên vật liệu
        /// </summary>
        public Guid MaterialID { get; set; }

        /// <summary>
        /// Mã đơn vị
        /// </summary>
        public Guid UnitID { get; set; }

        /// <summary>
        /// Mã đơn vị cũ
        /// </summary>
        public Guid OldUnitID { get; set; }

        /// <summary>
        /// Phép tính
        /// </summary>
        public Calculation Calculation { get; set; }

        /// <summary>
        /// Tỉ lệ chuyển đổi
        /// </summary>
        public Double ConversionRate { get; set; }

        /// <summary>
        /// Phương thức của bản ghi
        /// </summary>
        public Method Method { get; set; } = Method.Edit;

    }
}
