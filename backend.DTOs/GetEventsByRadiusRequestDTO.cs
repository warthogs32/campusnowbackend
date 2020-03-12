using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class GetEventsByRadiusRequestDTO
    {
        public float Radius { get; set; }

        public float X { get; set; }

        public float Y { get; set; }
    }
}
