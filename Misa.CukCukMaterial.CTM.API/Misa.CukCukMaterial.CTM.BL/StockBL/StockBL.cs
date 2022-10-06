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
        List<string> Errors = new List<string>();

        #endregion

        #region Constructor
        public StockBL(IStockDL stockDL) : base(stockDL)
        {
            _stockDL = stockDL;
        }

        #endregion

        #region Method

        protected override void Validate(Method method, Stock record)
        {
            // mã kho không được phép trùng
            if (_stockDL.CheckDuplicateCode(method, record.StockID, record.StockCode))
            {
                Errors.Add(Common.Resource.ResourceVN.StockCode_Duplicate);
            }

            // trường stockCode không được bỏ trống
            if (String.IsNullOrEmpty(record.StockCode))
            {
                Errors.Add(Common.Resource.ResourceVN.StockCode_NotEmpty);
            }

            // trường stockName không được bỏ trống
            if (String.IsNullOrEmpty(record.StockName))
            {
                Errors.Add(Common.Resource.ResourceVN.StockName_NotEmpty);
            }

            if (Errors.Count > 0)
            {
                throw new ValidateException(Errors);
            }
        }

        #endregion
    }
}
