using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class LoginResponseDTO : ResponseDTO
    {
        public string Token { get; set; }
        public HttpResponseMessage ResponseMsg { get; set; }
    }
}
