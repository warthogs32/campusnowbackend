using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backend.Components;
using System.ComponentModel.DataAnnotations;

namespace backend.Models
{
    public class EventModel
    {
        private int _listingId;
        public int ListingId
        {
            get => _listingId;
            set
            {
                _listingId = value;
            }
        }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
            }
        }

        private DateTime _startTime;
        public DateTime StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
            }
        }

        private DateTime _endTime;
        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
            }
        }

        private Location _location;
        public Location Location
        {
            get => _location;
            set
            {
                _location = value;
            }
        }
    }
}