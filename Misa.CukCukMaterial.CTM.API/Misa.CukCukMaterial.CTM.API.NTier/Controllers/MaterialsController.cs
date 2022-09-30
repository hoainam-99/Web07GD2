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

        #endregion
    }
}
