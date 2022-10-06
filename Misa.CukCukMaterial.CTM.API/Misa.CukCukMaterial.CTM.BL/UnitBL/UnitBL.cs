using Misa.CukCukMaterial.CTM.Common;
using Misa.CukCukMaterial.CTM.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    public class UnitBL : BaseBL<Unit>, IUnitBL
    {
        #region Field

        private IUnitDL _unitDL;
        List<string> Errors = new List<string>();

        #endregion

        #region Constructor
        public UnitBL(IUnitDL unitDL) : base(unitDL)
        {
            _unitDL = unitDL;
        }

        #endregion

        #region Method

        protected override void Validate(Method method, Unit record)
        {
            // trường unitName không được bỏ trống
            if (String.IsNullOrEmpty(record.UnitName))
            {
                Errors.Add(Common.Resource.ResourceVN.UnitName_NotEmpty);
            }

            if (Errors.Count > 0)
            {
                throw new ValidateException(Errors);
            }
        }

        #endregion
    }
}
