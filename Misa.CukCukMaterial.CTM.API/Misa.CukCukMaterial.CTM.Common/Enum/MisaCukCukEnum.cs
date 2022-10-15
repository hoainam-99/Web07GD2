using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.Common
{
    /// <summary>
    /// Class enum trạng thái theo dõi nguyên vật liệu
    /// </summary>
    public enum Status
    {
        /// <summary>
        /// Sử dụng
        /// </summary>
        Follow = 1,

        /// <summary>
        /// Ngưng sử dụng
        /// </summary>
        StopFollow = 2    
    }

    /// <summary>
    /// Phương thức 
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// Phương thức thêm mới
        /// </summary>
        Add = 1,

        /// <summary>
        /// Phương thức sửa
        /// </summary>
        Edit = 2,     

        /// <summary>
        /// Phương thức xóa
        /// </summary>
        Delete = 3,     
    }

    /// <summary>
    /// Phép tính
    /// </summary>
    public enum Calculation
    {
        /// <summary>
        /// Phép nhân
        /// </summary>
        Multiplication = 1,  

        /// <summary>
        /// Phép chia
        /// </summary>
        Division = 2,       
    }
}
