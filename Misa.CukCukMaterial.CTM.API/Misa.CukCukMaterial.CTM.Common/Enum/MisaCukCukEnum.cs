using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.Common
{
    public enum Status
    {   
        Follow = 1,      // Sử dụng
        StopFollow = 2   // Ngưng sử dụng
    }

    public enum Method
    {
        Add = 1,    // Phương thức thêm mới
        Edit = 2    // Phương thức sửa
    }

    public enum Calculation
    {
        Multiplication = 1  // Phép nhân
    }
}
