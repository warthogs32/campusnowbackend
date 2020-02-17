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
        [Authorize]
        [ResponseType(typeof(GetEventByIdResponseDTO))]
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

        [HttpPut]
        [Route("updateEvent/")]
        public void Put([FromBody]UpdateEventRequestDTO updateEventREquest)
        {
        }

        [HttpDelete]
        [Route("deleteEvent/")]
        public void Delete([FromBody]DeleteEventRequestDTO deleteEventRequest)
        {
        }
    }
}
