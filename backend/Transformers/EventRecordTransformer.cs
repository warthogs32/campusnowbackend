using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using backend.DTOs;
using backend.Models;
using System.Diagnostics.CodeAnalysis;

namespace backend.Transformers
{
    [ExcludeFromCodeCoverage]
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
            var eventRecord = new EventRecord()
            { 
                ListingId = dto.ListingId,
                UserId = dto.UserId,
                Title = dto.Title,
                Description = dto.Description,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                LocX = dto.LocX,
                LocY = dto.LocY
            };
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

            var dto = new EventRecordDTO() 
            {
                ListingId = eventRecord.ListingId,
                UserId = eventRecord.UserId,
                Title = eventRecord.Title,
                Description = eventRecord.Description,
                StartTime = eventRecord.StartTime,
                EndTime = eventRecord.EndTime,
                LocX = eventRecord.LocX,
                LocY = eventRecord.LocY
            };
            return dto;
        }
    }
}