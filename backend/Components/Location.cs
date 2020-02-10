using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace backend.Components
{
    public class Location
    {
        private double _x;
        public double X
        {
            get => _x;
            set
            {
                _x = value;
            }
        }

        private double _y;
        public double Y
        {
            get => _y;
            set
            {
                _y = value;
            }
        }
    }
}