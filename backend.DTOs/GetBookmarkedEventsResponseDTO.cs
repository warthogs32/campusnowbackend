﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class GetBookmarkedEventsResponseDTO
    {
        public IList<EventRecordDTO> EventRecords { get; set; }
    }
}
