using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.DTOs
{
    public class GetAllEventsResponseDTO
    {
        public IList<EventRecordDTO> EventRecords { get; set; }
    }
}