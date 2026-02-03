using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formation_Ecommerce_11_2025.Application.Athentication.Dtos
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Error { get; set; } = string.Empty;

        public ResponseDto() { }

        public ResponseDto(bool isSuccess, string error = "")
        {
            IsSuccess = isSuccess;
            Error = error;
        }
    }
}
