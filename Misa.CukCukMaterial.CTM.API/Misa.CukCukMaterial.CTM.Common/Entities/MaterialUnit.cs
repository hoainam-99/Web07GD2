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
    public class MaterialUnit
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

        public Method Method { get; set; } = Method.Edit;

    }
}
