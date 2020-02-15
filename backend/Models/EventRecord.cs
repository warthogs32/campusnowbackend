using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace backend.Models
{
    public class EventRecord
    {
        public int id { get; }
        public String title { get; }
        public String description { get; }
        public DateTime startTime { get; }
        public DateTime endTime { get; }
        public float locX { get; }
        public float locY { get; }
        public int userId { get; }

        public EventRecord(SQLDataReader reader)
        {
            this.id = Int32.Parse(reader["ListingId"].ToString());
            this.title = reader["Title"].ToString();
            this.description = reader["Description"].ToString();
            this.startTime = DateTime.Parse(reader["StartTime"].ToString());
            this.endTime = DateTime.Parse(reader["EndTime"].toString());
            this.LocX = float.Parse(reader["LocX"].ToString());
            this.LocY = float.Parse(reader["locY"].ToString());
            this.userId = Int32.Parse(reader["UserId"].ToString());
        }

        public EventRecord(EventRecordDTO dto)
        {
            this.id = dto.id;
            this.title = dto.title;
            this.description = dto.description;
            this.startTime = dto.startTime;
            this.endTime = dto.endTime;
            this.locX = dto.locX;
            this.locY = dto.locY;
            this.userId = dto.userId;
        }

        private int LocX
        {
            set
            {
                if (value >= -180.0 && value <= 180.0)
                    locX = LocX;
                else
                    locX = 0;
            }
        }

        private int LocY
        {
            set
            {
                if (value >= -180.0 && value <= 180.0)
                    locY = LocY;
                else
                    locY = 0;
            }
        }
    }
}