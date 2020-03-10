using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class GetCurrentUserResponseDTO
    {
        public UserRecordDTO CurrentUser { get; set; }
    }
}
