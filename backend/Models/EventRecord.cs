using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace backend.Models
{
    public class EventRecord
    {
        private int _listingId;
        public int ListingId 
        {
            get => _listingId;
            set => _listingId = value;
        }

        private int _userId;
        public int UserId
        {
            get => _userId;
            set => _userId = value;
        }

        private string _title;
        public string Title
        {
            get => _title;
            set => _title = value;
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => _description = value;
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set => _startTime = value;
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get => _endTime;
            set => _endTime = value;
        }

        private double _locX;
        public double LocX
        {
            get => _locX;
            set
            {
                if (value >= -180.0 && value <= 180.0)
                {
                    _locX = value;
                }
            }
        }

        private double _locY;
        public double LocY
        {
            get => _locY;
            set
            {
                if (value >= -180.0 && value <= 180.0)
                {
                    _locY = value;
                }
            }
        }

    }
}