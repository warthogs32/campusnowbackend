using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using backend.Repositories;
using backend.DTOs;
using System.Web.Http.Description;
using backend.Models;
using backend.Transformers;

namespace backend.Controllers
{
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        private EventRepository _eventRepo = new EventRepository();

        /// <summary>
        /// Retrieves all events.
        /// </summary>
        /// <returns>List of all event records.</returns>
        [Route("getAllEvents/")]
        [ResponseType(typeof(GetAllEventsResponseDTO))]
        [HttpGet]
        public GetAllEventsResponseDTO GetAllEvents()
        {
            List<EventRecord> allEvents = _eventRepo.GetAllEvents();
            return new GetAllEventsResponseDTO()
            {
                EventRecords = allEvents.Select(x => EventRecordTransformer.Transform(x)).ToList()
            };
        }

        /// <summary>
        /// Retrieves event given its corresponding ID.
        /// </summary>
        /// <param name="eventIdRequest"></param>
        /// <returns>Event record with the given ID.</returns>
        [HttpGet]
        [ResponseType(typeof(GetEventsByUserIdResponseDTO))]
        [Route("getEventById/")]
        public GetEventByIdResponseDTO GetEventById([FromUri]GetEventByIdRequestDTO eventIdRequest)
        {
            int eventId = eventIdRequest.EventId;
            EventRecord retrievedEvent = _eventRepo.GetEventById(eventId);
            EventRecordDTO retrievedEventDTO = EventRecordTransformer.Transform(retrievedEvent);
            return new GetEventByIdResponseDTO()
            {
                EventRecord = retrievedEventDTO
            };
        }

        /// <summary>
        /// Posts a new event.
        /// </summary>
        /// <param name="newEvent"></param>
        /// <returns>True for success, false for post failure.</returns>
        [HttpPost]
        [Authorize]
        [ResponseType(typeof(PostNewEventResponseDTO))]
        [Route("postNewEvent/")]
        public PostNewEventResponseDTO PostNewEvent([FromBody]PostNewEventRequestDTO newEvent)
        {
            bool newEventResponse = _eventRepo.PostNewEvent(EventRecordTransformer.Transform(newEvent.NewEvent));
            return new PostNewEventResponseDTO
            {
                status = newEventResponse
            };
        }

        /// <summary>
        /// Modifies an existing event.
        /// </summary>
        /// <param name="updateEventRequest"></param>
        /// <returns>True for success, false for update failure.</returns>
        [HttpPut]
        [Authorize]
        [ResponseType(typeof(UpdateEventResponseDTO))]
        [Route("updateEvent/")]
        public UpdateEventResponseDTO Put([FromBody]UpdateEventRequestDTO updateEventRequest)
        {
            bool updateEventResponse = _eventRepo.UpdateEvent(EventRecordTransformer.Transform(updateEventRequest.UpdatedEvent));
            return new UpdateEventResponseDTO
            {
                status = updateEventResponse
            };
        }

        /// <summary>
        /// Deletes an event.
        /// </summary>
        /// <param name="deleteEventRequest"></param>
        /// <returns>True for success, false for delete failure.</returns>
        [HttpDelete]
        [Authorize]
        [ResponseType(typeof(DeleteEventResponseDTO))]
        [Route("deleteEvent/")]
        public DeleteEventResponseDTO Delete([FromBody]DeleteEventRequestDTO deleteEventRequest)
        {
            bool deleteEventResponse = _eventRepo.DeleteEvent(deleteEventRequest.EventIdToDelete);
            return new DeleteEventResponseDTO
            {
                Status = deleteEventResponse
            };
        }

        /// <summary>
        /// Lists all of a user's events.
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <returns>List of a user's events.</returns>
        [HttpGet]
        [ResponseType(typeof(GetEventsByUserIdResponseDTO))]
        [Route("getEventsByUserId/")]
        public GetEventsByUserIdResponseDTO GetEventsByUserId([FromUri]GetEventsByUserIdRequestDTO currentUserId)
        {
            List<EventRecord> userEvents = _eventRepo.GetEventsByUserId(currentUserId.UserId);
            return new GetEventsByUserIdResponseDTO
            {
                Events = userEvents.Select(x => EventRecordTransformer.Transform(x)).ToList()
            };
        }

        /// <summary>
        /// List all events within a speficied time range.
        /// </summary>
        /// <param name="timeInterval"></param>
        /// <returns>List of event records in a specified time range.</returns>
        [HttpPost]
        [ResponseType(typeof(GetEventsByTimeRangeResponseDTO))]
        [Route("getEventsByTimeRange/")]
        public GetEventsByTimeRangeResponseDTO PostGetEventsByTimeRange([FromBody]GetEventsByTimeRangeRequestDTO timeInterval)
        {
            List<EventRecord> eventsInTimeInterval = _eventRepo.GetEventsByTimeRange(timeInterval.StartTime, timeInterval.EndTime);
            return new GetEventsByTimeRangeResponseDTO
            {
                Events = eventsInTimeInterval.Select(x => EventRecordTransformer.Transform(x)).ToList()
            };
        }
    }
}
