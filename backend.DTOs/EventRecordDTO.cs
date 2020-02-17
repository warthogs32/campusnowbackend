using System;
using System.Collections.Generic;
using System.Linq;

namespace backend.DTOs
{
    public class EventRecordDTO
    {
        public int ListingId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double LocX { get; set; }
        public double LocY { get; set; }
    }
}