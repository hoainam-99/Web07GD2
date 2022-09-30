using Misa.CukCukMaterial.CTM.Common;
using Misa.CukCukMaterial.CTM.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    public class CategoryBL : BaseBL<Category>, ICategoryBL
    {
        #region Field

        private ICategoryDL _categoryDL;

        #endregion

        #region Constructor
        public CategoryBL(ICategoryDL categoryDL) : base(categoryDL)
        {
            _categoryDL = categoryDL;
        }

        #endregion
    }
}
