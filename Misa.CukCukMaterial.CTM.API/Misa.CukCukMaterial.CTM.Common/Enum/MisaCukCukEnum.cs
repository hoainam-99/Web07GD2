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
        Follow = 1,      // Sử dụng
        StopFollow = 2   // Ngưng sử dụng
    }

    /// <summary>
    /// Phương thức 
    /// </summary>
    public enum Method
    {
        Add = 1,        // Phương thức thêm mới
        Edit = 2,       // Phương thức sửa
        Delete = 3,     // Phương thức xóa
    }

    /// <summary>
    /// Phép tính
    /// </summary>
    public enum Calculation
    {
        Multiplication = 1, // Phép nhân
        Division = 2,       // Phép chia
    }
}
