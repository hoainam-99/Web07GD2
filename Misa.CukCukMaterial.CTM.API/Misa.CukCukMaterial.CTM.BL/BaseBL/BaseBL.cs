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
        
        public int DeleteOneRecord(Guid id)
        {
            return _baseDL.DeleteOneRecord(id);
        }

        public T GetOneRecordByID(Guid id)
        {
            return _baseDL.GetOneRecordByID(id);
        }

        public virtual IEnumerable<T> GetRecords()
        {
            return _baseDL.GetRecords();
        }

        public virtual Guid InsertOneRecord(T record)
        {
            return _baseDL.InsertOneRecord(record);
        }

        public virtual Guid UpdateOneRecord(Guid id, T record)
        {
            string className = typeof(T).Name;
            var primaryKeyProp = typeof(T).GetProperties().FirstOrDefault(prop => prop.GetCustomAttributes(typeof(KeyAttribute), true).Count() > 0);

            if (primaryKeyProp != null)
            {
                primaryKeyProp.SetValue(record, id);
            }

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
