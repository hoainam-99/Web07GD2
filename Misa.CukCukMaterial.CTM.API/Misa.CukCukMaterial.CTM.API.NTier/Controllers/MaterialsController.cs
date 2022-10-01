using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.CukCukMaterial.CTM.BL;
using Misa.CukCukMaterial.CTM.Common;
using Swashbuckle.AspNetCore.Annotations;

namespace Misa.CukCukMaterial.CTM.API.NTier.Controllers
{
    public class MaterialsController : BasesController<Material>
    {
        #region Field

        private IMaterialBL _materialBL;

        #endregion

        #region Constructor
        public MaterialsController(IMaterialBL materialBL) : base(materialBL)
        {
            _materialBL = materialBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy bản ghi phù hợp với điều kiện phân trang và lọc trong bảng material
        /// </summary>
        /// <param name="materialCode">Mã nguyên vật liệu</param>
        /// <param name="materialName">Tên nguyên vật liệu</param>
        /// <param name="feature">tính chất</param>
        /// <param name="unit">Đơn vị tính</param>
        /// <param name="category">Nhóm nguyên vật liệu</param>
        /// <param name="description">Mô tả</param>
        /// <param name="status">Trạng thái theo dõi / ngưng theo dõi</param>
        /// <param name="pageSize">Số bản ghi trên 1 trang</param>
        /// <param name="pageNum">Số trang</param>
        /// <returns>
        ///     Một đối tượng gồm: 
        ///         + Danh sách nhân viên thỏa mãn điều kiện phân trang và tìm kiếm
        ///         + Tổng số bản ghi thỏa mãn điều kiện tìm kiếm
        /// </returns>
        /// Author: LHNAM (30/09/2022)
        [HttpGet("Filter")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult FilterRecords(
                [FromQuery] string? materialCode,
                [FromQuery] string? materialName,
                [FromQuery] string? feature,
                [FromQuery] string? unit,
                [FromQuery] string? category,
                [FromQuery] string? description,
                [FromQuery] Status? status,
                [FromQuery] int pageSize = 10, 
                [FromQuery] int pageNum = 1)
        {
            try
            {
                var filterData = new FilterData { MaterialCode = materialCode, MaterialName = materialName, Feature = feature, Unit = unit, Category = category, Description = description, Status = status, PageSize = pageSize, PageNum = pageNum };
                var multipleResult = _materialBL.FilterRecords(filterData);

                if(multipleResult != null)
                {
                    return StatusCode(200, multipleResult);
                }
                else
                {
                    return StatusCode(500, "e002");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("new-code")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public IActionResult GetNewMaterialCode()
        {
            try
            {
                string newCode = _materialBL.GetNewMaterialCode();

                if (!String.IsNullOrEmpty(newCode))
                {
                    return StatusCode(200, newCode);
                }
                else
                {
                    return StatusCode(500, "e001");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #endregion
    }
}
