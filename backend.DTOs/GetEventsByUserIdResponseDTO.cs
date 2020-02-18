using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class GetEventsByUserIdResponseDTO
    {
        public IList<EventRecordDTO> Events { get; set; }
    }
}
