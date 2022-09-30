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

        #endregion

        #region Constructor
        public UnitBL(IUnitDL unitDL) : base(unitDL)
        {
            _unitDL = unitDL;
        }

        #endregion
    }
}
