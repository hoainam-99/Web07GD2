using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.CukCukMaterial.CTM.BL;
using Misa.CukCukMaterial.CTM.Common;

namespace Misa.CukCukMaterial.CTM.API.NTier.Controllers
{
    public class CategoriesController : BasesController<Category>
    {
        #region Field

        private ICategoryBL _categoryBL;

        #endregion

        #region Constructor

        public CategoriesController(ICategoryBL categoryBL) : base(categoryBL)
        {
            _categoryBL = categoryBL;
        }

        #endregion
    }
}
