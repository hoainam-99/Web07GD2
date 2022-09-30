using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Misa.CukCukMaterial.CTM.BL
{
    /// <summary>
    /// Class xử lý lỗi validate
    /// </summary>
    /// Author: LHNAM (29/09/2022)
    public class ValidateException : Exception
    {
        IDictionary ErrorMessage;

        public ValidateException(List<string> messages)
        {
            ErrorMessage = new Dictionary<string, List<string>>();
            ErrorMessage.Add("error", messages);
        }

        public override IDictionary Data => this.ErrorMessage;
    }
}
