using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.CukCukMaterial.CTM.BL;
using Swashbuckle.AspNetCore.Annotations;

namespace Misa.CukCukMaterial.CTM.API.NTier.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasesController<T> : ControllerBase
    {
        #region Field

        private IBaseBL<T> _baseBL;

        #endregion

        #region Constructor

        public BasesController(IBaseBL<T> baseBL)
        {
            _baseBL = baseBL;
        }

        #endregion

        #region Method

        /// <summary>
        /// API lấy tất cả dữ liệu bản ghi
        /// </summary>
        /// <returns>Tất cả dữ liệu bản ghi trong bảng</returns>
        /// Author: LHNAM (29/09/2022)
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public virtual IActionResult GetRecords()
        {
            try
            {
                var records = _baseBL.GetRecords();

                if(records != null)
                {
                    return StatusCode(200, records);
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

        /// <summary>
        /// API lấy dữ liệu bản ghi theo ID
        /// </summary>
        /// <param name="id">ID của bản ghi</param>
        /// <returns>Dữ liệu của bản ghi</returns>
        /// Author: LHNAM (29/09/2022)
        [HttpGet("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public virtual IActionResult GetOneRecordByID([FromRoute] Guid id)
        {
            try
            {
                var record = _baseBL.GetOneRecordByID(id);

                if(record != null)
                {
                    return StatusCode(200, record);
                }
                else
                {
                    return StatusCode(404);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// API xóa một bản ghi
        /// </summary>
        /// <param name="id">ID của bản ghi cần xóa</param>
        /// <returns>ID của bản ghi đã xóa</returns>
        /// Author: LHNAM (29/09/2022)
        [HttpDelete("{id}")]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(Guid))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public virtual IActionResult DeleteOneRecord([FromRoute] Guid id)
        {
            try
            {
                int affectedRow = _baseBL.DeleteOneRecord(id);

                if(affectedRow > 0)
                {
                    return StatusCode(200, id);
                }
                else
                {
                    return StatusCode(404);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// API Thêm một bản ghi
        /// </summary>
        /// <param name="record">nội dung bản ghi cần thêm</param>
        /// <returns>ID của bản ghi đã thêm</returns>
        /// Author: LHNAM (25/08/2022)
        [HttpPost]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public virtual IActionResult InsertOneRecord([FromBody] T record)
        {
            try
            {
                var recordID = _baseBL.InsertOneRecord(record);

                if(recordID != Guid.Empty)
                {
                    return StatusCode(201, recordID);
                }
                else
                {
                    return StatusCode(500, "e001");
                }
            }
            catch (ValidateException ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Data
                };

                return StatusCode(400, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// API sửa 1 bản ghi
        /// </summary>
        /// <param name="ID">ID của bản ghi</param>
        /// <param name="record">Nội dung của bản ghi</param>
        /// <returns>ID của bản ghi đã sửa</returns>
        /// Author: LHNAM (29/09/2022)
        [HttpPut("{ID}")]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public virtual IActionResult UpdateOneRecord([FromRoute] Guid id, [FromBody] T record)
        {
            try
            {
                var recordID = _baseBL.UpdateOneRecord(id, record);

                if(recordID != Guid.Empty)
                {
                    return StatusCode(200, recordID);
                }
                else
                {
                    return StatusCode(404);
                }
            }
            catch (ValidateException ex)
            {
                var res = new
                {
                    devMsg = ex.Message,
                    userMsg = ex.Data
                };

                return StatusCode(400, res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        #endregion
    }
}
