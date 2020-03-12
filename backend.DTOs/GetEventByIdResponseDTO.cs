using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.DTOs
{
    public class GetEventByIdResponseDTO : ResponseDTO
    {
        public EventRecordDTO EventRecord { get; set; }
    }
}