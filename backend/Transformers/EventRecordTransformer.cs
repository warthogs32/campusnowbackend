using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backend.DTOs;
using backend.Models;

namespace backend.Transformers
{
    public class EventRecordTransformer
    {
        /// <summary>
        /// Transforms an EventRecordDTO to an EventRecord.
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>EventRecord</returns>
        public static EventRecord Transform(EventRecordDTO dto)
        {
            if(dto == null)
            {
                return null;
            }
            var eventRecord = new EventRecord();
            return eventRecord;
        }

        /// <summary>
        /// Transforms an EventRecord to an EventRecordDTO.
        /// </summary>
        /// <param name="eventRecord"></param>
        /// <returns>EventRecordDTO</returns>
        public static EventRecordDTO Transform(EventRecord eventRecord)
        {
            if(eventRecord == null)
            {
                return null;
            }

            var dto = new EventRecordDTO();
            return dto;
        }
    }
}