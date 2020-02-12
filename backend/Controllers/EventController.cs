using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using backend.Repositories;
using backend.DTOs;

namespace backend.Controllers
{
    [RoutePrefix("api/event")]
    public class EventController : ApiController
    {
        private EventRepository _eventRepo = new EventRepository();

        [Route("getAllEvents/")]
        [HttpGet]
        public IHttpActionResult GetAllEvents()
        {
            GetAllEventsResponseDTO allEvents = _eventRepo.GetAllEvents();
            return Ok(allEvents);
        }

        [HttpGet]
        [Route("getEventById/")]
        public IHttpActionResult GetEventById([FromBody]GetEventByIdRequestDTO eventIdRequest)
        {
            GetEventByIdResponseDTO retrievedEvent = _eventRepo.GetEventById(eventIdRequest);
            return Ok(retrievedEvent);
        }

        [HttpPost]
        [Route("postNewEvent/")]
        public IHttpActionResult PostNewEvent([FromBody]PostNewEventRequestDTO newEvent)
        {
            PostNewEventResponseDTO newEventResponse = _eventRepo.PostNewEvent(newEvent);
            return Ok(newEventResponse);
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
