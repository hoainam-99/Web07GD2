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

        /// <summary>
        /// Hàm xử lý logic lọc và phân trang
        /// </summary>
        /// <param name="filterData">đối tượng lọc và phân trang</param>
        /// <returns>
        ///     Một đối tượng gồm: 
        ///         + Danh sách nhân viên thỏa mãn điều kiện phân trang và tìm kiếm
        ///         + Tổng số bản ghi thỏa mãn điều kiện tìm kiếm
        /// </returns>
        /// Author: LHNAM (30/09/2022)
        public PagingData<Material> FilterRecords(FilterData filterData)
        {
            int pageNum = filterData.PageNum;
            int pageSize = filterData.PageSize;

            string filterCondition = "";
            var condition = new List<string>();


            if (filterData.MaterialCode != null)
            {
                string filterStr = CheckFilterString(filterData.MaterialCode);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"MaterialCode {filterStr}");
                }
            }

            if (filterData.MaterialName != null)
            {
                string filterStr = CheckFilterString(filterData.MaterialName);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"MaterialName {filterStr}");
                }
            }

            if (filterData.Feature != null)
            {
                string filterStr = CheckFilterString(filterData.Feature);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"Feature {filterStr}");
                }
            }

            if (filterData.Unit != null)
            {
                string filterStr = CheckFilterString(filterData.Unit);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"UnitName {filterStr}");
                }
            }

            if (filterData.Category != null)
            {
                string filterStr = CheckFilterString(filterData.Category);
                if (!String.IsNullOrEmpty(filterStr))
                {
                    condition.Add($"CategoryName {filterStr}");
                }
            }

            if (filterData.Description != null)
            {
                string filterStr = CheckFilterString(filterData.Description);
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

        /// <summary>
        /// Hàm validate dữ liệu
        /// </summary>
        /// <param name="method">Phương thức được gọi đến</param>
        /// <param name="record">Bản ghi cần validate</param>
        /// Author: LHNAM (29/09/2022)
        protected override void Validate(Method method, Material record)
        {
            if (record.MaterialUnit != null && record.MaterialUnit.Count() > 0)
            {
                foreach (var item in record.MaterialUnit)
                {
                    // Mã đơn vị chuyển đổi không được bỏ trống
                    if (item.UnitID == Guid.Empty)
                    {
                        Errors.Add(Common.Resource.ResourceVN.ConversionUnit_NotEmpty);
                    }
                    // Mã đơn vị chuyển đổi không được trùng với đơn vị tính
                    else if (item.UnitID == record.UnitID)
                    {
                        Errors.Add(Common.Resource.ResourceVN.ConversionUnit_And_Unit_NotSame);
                    }
                    else if (_materialDL.CheckDuplicateMaterialUnit(item.Method, record.MaterialID, item.UnitID, item.OldUnitID))
                    {
                        Errors.Add(Common.Resource.ResourceVN.ConversionUnit_Duplicate);
                    }
                }
            }


            // mã nguyên vật liệu không được phép trùng
            if (_materialDL.CheckDuplicateCode(method, record.MaterialID, record.MaterialCode))
            {
                Errors.Add(Common.Resource.ResourceVN.MaterialCode_Duplicate);
            }

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

        /// <summary>
        /// Hàm xử lý lọc
        /// </summary>
        /// <param name="filterString">Chuỗi cần lọc</param>
        /// <returns>Chuỗi để lọc sau khi xử lý</returns>
        public string CheckFilterString(string filterString)
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

        /// <summary>
        /// Hàm lấy mã nguyên vật liệu mới
        /// </summary>
        /// <returns>Mã nguyên vật liệu mới</returns>
        /// Author: LHNAM (01/10/2022)
        public string GetNewMaterialCode(string code)
        {
            return _materialDL.GetNewMaterialCode(code);
        }

        #endregion
    }
}
