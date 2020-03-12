using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public abstract class ResponseDTO
    {
		public string Status
		{
			get { return "Success"; }
			set { Status = value; }
		}

	}
}
