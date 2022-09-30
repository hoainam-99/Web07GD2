using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Misa.CukCukMaterial.CTM.BL;
using Misa.CukCukMaterial.CTM.Common;
using System.Runtime.CompilerServices;

namespace Misa.CukCukMaterial.CTM.API.NTier.Controllers
{
    public class StocksController : BasesController<Stock>
    {
        #region Field

        private IStockBL _stockBL;

        #endregion

        #region Constructor
        public StocksController(IStockBL stockBL) : base(stockBL)
        {
            _stockBL = stockBL;
        }

        #endregion
    }
}
