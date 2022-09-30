using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.CukCukMaterial.CTM.BL;
using Misa.CukCukMaterial.CTM.Common;

namespace Misa.CukCukMaterial.CTM.API.NTier.Controllers
{
    public class UnitsController : BasesController<Unit>
    {
        #region Field

        private IUnitBL _unitBL;

        #endregion

        #region Constructor
        public UnitsController(IUnitBL unitBL) : base(unitBL)
        {
            _unitBL = unitBL;
        }

        #endregion
    }
}
