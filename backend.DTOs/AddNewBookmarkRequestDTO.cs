using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace backend.DTOs
{
    public class AddNewBookmarkRequestDTO
    {
        public EventRecordDTO EventRecord { get; set; }
        public UserRecordDTO UserRecord { get; set; }
    }
}
