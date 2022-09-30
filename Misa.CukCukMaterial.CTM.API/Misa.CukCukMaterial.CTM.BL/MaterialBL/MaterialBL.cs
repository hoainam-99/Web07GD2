using Misa.CukCukMaterial.CTM.Common;
using Misa.CukCukMaterial.CTM.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    public class MaterialBL : BaseBL<Material>, IMaterialBL
    {
        #region Field

        private IMaterialDL _materialDL;
        List<string> Errors = new List<string>();

        #endregion

        #region Constructor
        public MaterialBL(IMaterialDL materialDL) : base(materialDL)
        {
            _materialDL = materialDL;
        }


        #endregion

        #region Method
        public PagingData<Material> FilterRecords(FilterData filterData)
        {
            int pageNum = filterData.PageNum;
            int pageSize = filterData.PageSize;

            string filterCondition = "";
            var condition = new List<string>();


            if (filterData.MaterialCode != null)
            {
                string filterStr = checkFilterString(filterData.MaterialCode);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"MaterialCode {filterStr}");
                }
            }

            if (filterData.MaterialName != null)
            {
                string filterStr = checkFilterString(filterData.MaterialName);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"MaterialName {filterStr}");
                }
            }

            if (filterData.Feature != null)
            {
                string filterStr = checkFilterString(filterData.Feature);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"Feature {filterStr}");
                }
            }

            if (filterData.Unit != null)
            {
                string filterStr = checkFilterString(filterData.Unit);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"UnitName {filterStr}");
                }
            }

            if (filterData.Category != null)
            {
                string filterStr = checkFilterString(filterData.Category);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"CategoryName {filterStr}");
                }
            }

            if (filterData.Description != null)
            {
                string filterStr = checkFilterString(filterData.Description);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"Description {filterStr}");
                }
            }

            if (filterData.Status != null)
            {
                switch (filterData.Status)
                {
                    case Status.Follow:
                        condition.Add($"Status = \'1\'");
                        break;
                    default:
                        condition.Add($"Status = \'2\'");
                        break;
                }
                
            }

            filterCondition = string.Join(" AND ", condition);

            return _materialDL.FilterRecord(filterCondition, pageNum, pageSize);
        }

        protected override void Validate(Method method, Material record)
        {
            // mã nguyên vật liệu không được phép trùng

            // trường materialCode không được bỏ trống
            if (String.IsNullOrEmpty(record.MaterialCode))
            {
                Errors.Add(Common.Resource.ResourceVN.MaterialCode_NotEmpty);
            }

            // trường materialName không được bỏ trống
            if (String.IsNullOrEmpty(record.MaterialName))
            {
                Errors.Add(Common.Resource.ResourceVN.MaterialName_NotEmpty);
            }

            // trường unitID không được bỏ trống
            if (record.UnitID == Guid.Empty)
            {
                Errors.Add(Common.Resource.ResourceVN.UnitID_NotEmpty);
            }

            if (Errors.Count > 0)
            {
                throw new ValidateException(Errors);
            }
        }

        public string checkFilterString(string filterString)
        {
            string filterStr = "";
            if (!String.IsNullOrEmpty(filterString))
            {
                // cắt lấy ký tự đầu tiên
                char filterChar = Char.Parse(filterString.Substring(0, 1));

                // cắt lấy chuỗi cần filter
                string filterValue = filterString.Substring(1);

                switch (filterChar)
                {
                    case '*':
                        filterStr = $"LIKE \'%{filterValue}%\'";
                        break;
                    case '=':
                        filterStr = $"= \'{filterValue}\'";
                        break;
                    case '+':
                        filterStr = $"LIKE \'{filterValue}%\'";
                        break;
                    case '-':
                        filterStr = $"LIKE \'%{filterValue}\'";
                        break;
                    case '!':
                        filterStr = $"NOT LIKE \'%{filterValue}%\'";
                        break;
                }
            }

            return filterStr;
        }

        #endregion
    }
}
