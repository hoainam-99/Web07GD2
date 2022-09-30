using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    /// <summary>
    /// Interface IBaseBL dùng để chứa các phương thức triển khai Bussiness Layer
    /// </summary>
    /// Author: LHNAM (29/09/2022)
    public interface IBaseBL<T>
    {
        /// <summary>
        /// Lấy tất cả các bản ghi
        /// </summary>
        /// <returns>Các bản ghi của 1 bảng</returns>
        /// Author: LHNAM (29/09/2022)
        public IEnumerable<T> GetRecords();

        /// <summary>
        /// Lấy dữ liệu của 1 bản ghi bằng ID
        /// </summary>
        /// <param name="id">ID của đối tượng cần lấy dữ liệu</param>
        /// <returns>Dữ liệu của đối tượng cần lấy</returns>
        /// Author: LHNAM (29/09/2022)
        public T GetOneRecordByID(Guid id);

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của đối tượng cần xóa</param>
        /// <returns>Số bản ghi bị xóa</returns>
        /// Author: LHNAM (29/09/2022)
        public int DeleteOneRecord(Guid id);

        /// <summary>
        /// Thêm 1 bản ghi 
        /// </summary>
        /// <param name="record">Thông tin của bản ghi cần thêm</param>
        /// <returns>ID của bản ghi được thêm thành công</returns>
        /// Author: LHNAM (29/09/2022)
        public Guid InsertOneRecord(T record);

        /// <summary>
        /// Sửa 1 bản ghi
        /// </summary>
        /// <param name="id">ID của bản ghi cần sửa</param>
        /// <param name="record">Thông tin của bản ghi cần sửa</param>
        /// <returns>ID của bản ghi được sửa thành công</returns>
        /// Author: LHNAM (29/09/2022)
        public Guid UpdateOneRecord(Guid id, T record);
    }
}
