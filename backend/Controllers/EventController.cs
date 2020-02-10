using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using backend.Repositories;
using backend.Models;

namespace backend.Controllers
{
    [RoutePrefix("Event")]
    public class EventController : ApiController
    {
        [Route("getAllEvents/")]
        [HttpGet]
        public IEnumerable<string> GetAllEvents()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet]
        [Route("getEventById/")]
        public EventModel GetEventById(int id)
        {
            EventRepository eventRepo = new EventRepository();
            EventModel retrievedEvent = eventRepo.GetEventById(id);
            return retrievedEvent;
        }

        [HttpPost]
        public void PostNewEvent([FromBody]string value)
        {
        }

        // PUT: api/Event/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Event/5
        public void Delete(int id)
        {
        }
    }
}
