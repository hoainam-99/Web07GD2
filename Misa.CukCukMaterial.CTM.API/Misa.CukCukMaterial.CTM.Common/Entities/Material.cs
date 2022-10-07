using Misa.CukCukMaterial.CTM.Common;
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
    /// Class Material định dạng đối tượng lấy từ bảng material
    /// </summary>
    /// Author: LHNAM (29/09/2022)
    [Table("material")]
    public class Material : BaseEntity
    {
        /// <summary>
        /// ID nguyên vật liệu
        /// </summary>
        [Key]
        public Guid MaterialID { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Mã nguyên vật liệu
        /// </summary>
        public String MaterialCode { get; set; }

        /// <summary>
        /// Tên nguyên vật liệu
        /// </summary>
        public String MaterialName { get; set; }

        /// <summary>
        /// Hạn sử dụng
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// ID đơn vị
        /// </summary>
        public Guid UnitID { get; set; }

        /// <summary>
        /// Tên đơn vị
        /// </summary>
        public String UnitName { get; set; }

        /// <summary>
        /// ID nhóm nguyên vật liệu
        /// </summary>
        public Guid? CategoryID { get; set; }

        /// <summary>
        /// Tên nhóm nguyên vật liệu
        /// </summary>
        public String? CategoryName { get; set; }

        /// <summary>
        /// ID kho
        /// </summary>
        public Guid? StockID { get; set; }

        /// <summary>
        /// Tên kho
        /// </summary>
        public String? StockName { get; set; }

        /// <summary>
        /// Tính chất
        /// </summary>
        public String? Feature { get; set; }

        /// <summary>
        /// Số lượng tồn kho tối thiểu
        /// </summary>
        public Double? InventoryNumber { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public String? Description { get; set; }

        /// <summary>
        /// Trạng thái (sử dụng, ngưng sử dụng)
        /// </summary>
        public Status? Status { get; set; }

        /// <summary>
        /// danh sách đơn vị chuyển đổi của material
        /// </summary>
        public List<MaterialUnit>? MaterialUnit { get; set; }
    }
}
