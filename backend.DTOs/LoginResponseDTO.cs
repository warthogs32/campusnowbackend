using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public HttpResponseMessage responseMsg { get; set; }
    }
}
