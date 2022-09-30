using Misa.CukCukMaterial.CTM.Common;
using Misa.CukCukMaterial.CTM.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    public class StockBL : BaseBL<Stock>, IStockBL
    {
        #region Field

        private IStockDL _stockDL;

        #endregion

        #region Constructor
        public StockBL(IStockDL stockDL) : base(stockDL)
        {
            _stockDL = stockDL;
        }

        #endregion
    }
}
