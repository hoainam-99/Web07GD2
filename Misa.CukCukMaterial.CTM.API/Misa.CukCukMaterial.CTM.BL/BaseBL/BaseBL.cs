using Misa.CukCukMaterial.CTM.Common;
using Misa.CukCukMaterial.CTM.DL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    public class BaseBL<T> : IBaseBL<T>
    {
        #region Field

        private IBaseDL<T> _baseDL;

        #endregion

        #region Constructor

        public BaseBL(IBaseDL<T> baseDL)
        {
            _baseDL = baseDL;
        }

        #endregion

        #region Method

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của đối tượng cần xóa</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Author: LHNAM (29/09/2022)
        public int DeleteOneRecord(Guid id)
        {
            return _baseDL.DeleteOneRecord(id);
        }

        /// <summary>
        /// Lấy dữ liệu của 1 bản ghi bằng ID
        /// </summary>
        /// <param name="id">ID của đối tượng cần lấy dữ liệu</param>
        /// <returns>Dữ liệu của đối tượng cần lấy</returns>
        /// Author: LHNAM (29/09/2022)
        public T GetOneRecordByID(Guid id)
        {
            return _baseDL.GetOneRecordByID(id);
        }

        /// <summary>
        /// Lấy tất cả các bản ghi
        /// </summary>
        /// <returns>Các bản ghi của 1 bảng</returns>
        /// Author: LHNAM (29/09/2022)
        public virtual IEnumerable<T> GetRecords()
        {
            return _baseDL.GetRecords();
        }

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <param name="record">Thông tin của bản ghi cần thêm</param>
        /// <returns>ID của bản ghi được thêm thành công</returns>
        /// Author: LHNAM (29/09/2022)
        public virtual Guid InsertOneRecord(T record)
        {
            // Validate dữ liệu
            Validate(Method.Add, record);

            return _baseDL.InsertOneRecord(record);
        }

        /// <summary>
        /// Sửa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của bản ghi cần sửa</param>
        /// <param name="record">Thông tin của bản ghi cần sửa</param>
        /// <returns>ID của bản ghi được sửa thành công</returns>
        /// Author: LHNAM (29/09/2022)
        public virtual Guid UpdateOneRecord(Guid id, T record)
        {
            string className = typeof(T).Name;
            var primaryKeyProp = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);

            if (primaryKeyProp != null)
            {
                primaryKeyProp.SetValue(record, id);
            }

            // Validate dữ liệu
            Validate(Method.Edit, record);

            return _baseDL.UpdateOneRecord(id, record);
        }

        /// <summary>
        /// Hàm validate dữ liệu
        /// </summary>
        /// <param name="method">Phương thức được gọi đến</param>
        /// <param name="record">Bản ghi cần validate</param>
        /// Author: LHNAM (29/09/2022)
        protected virtual void Validate(Method method, T record)
        {

        }

        #endregion
    }
}
