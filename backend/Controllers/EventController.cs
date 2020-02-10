using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

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

        [Route("getEventById/")]
        public string GetEventById(int id)
        {
            return id.ToString();
        }

        // POST: api/Event
        public void Post([FromBody]string value)
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
