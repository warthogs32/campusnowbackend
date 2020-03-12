using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using backend.Repositories;
using System.Net.Http.Headers;
using backend.DTOs;
using System.Web.Http.Description;
using backend.Models;
using System.Web.Http.Cors;
using backend.Exceptions;
using backend.Transformers;
using System.Web;
using System.Diagnostics.CodeAnalysis;

namespace backend.Controllers
{
    [ExcludeFromCodeCoverage]
    [RoutePrefix("api/event")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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
            try
            {
                List<EventRecord> allEvents = _eventRepo.GetAllEvents();
                return new GetAllEventsResponseDTO()
                {
                    EventRecords = allEvents.Select(x => EventRecordTransformer.Transform(x)).ToList()
                };
            }
            catch(RepoException e)
            {
                return new GetAllEventsResponseDTO()
                {
                    Status = e.Message
                };
            }
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
            try
            {
                EventRecord retrievedEvent = _eventRepo.GetEventById(eventId);
                EventRecordDTO retrievedEventDTO = EventRecordTransformer.Transform(retrievedEvent);
                return new GetEventByIdResponseDTO()
                {
                    EventRecord = retrievedEventDTO
                };
            }
            catch(RepoException e)
            {
                return new GetEventByIdResponseDTO()
                {
                    Status = e.Message
                };
            }
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
            //context.Request.Headers["Authorization"];
            try
            {
                string newEventResponse = _eventRepo.PostNewEvent(EventRecordTransformer.Transform(newEvent.NewEvent));
                return new PostNewEventResponseDTO
                {
                    Status = newEventResponse
                };
            }
            catch(RepoException e)
            {
                return new PostNewEventResponseDTO()
                {
                    Status = e.Message
                };
            }
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
        public UpdateEventResponseDTO PutUpdateEvent([FromBody]UpdateEventRequestDTO updateEventRequest)
        {
            try
            {
                string updateEventResponse = _eventRepo.UpdateEvent(EventRecordTransformer.Transform(updateEventRequest.UpdatedEvent));
                return new UpdateEventResponseDTO
                {
                    Status = updateEventResponse
                };
            }
            catch(RepoException e)
            {
                return new UpdateEventResponseDTO()
                {
                    Status = e.Message
                };
            }
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
        public DeleteEventResponseDTO DeleteEvent([FromBody]DeleteEventRequestDTO deleteEventRequest)
        {
            try
            {
                string deleteEventResponse = _eventRepo.DeleteEvent(deleteEventRequest.EventIdToDelete);
                return new DeleteEventResponseDTO
                {
                    Status = deleteEventResponse
                };
            }
            catch(RepoException e)
            {
                return new DeleteEventResponseDTO()
                {
                    Status = e.Message
                };
            }
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
            try
            {
                List<EventRecord> userEvents = _eventRepo.GetEventsByUserId(currentUserId.UserId);
                return new GetEventsByUserIdResponseDTO()
                {
                    Events = userEvents.Select(x => EventRecordTransformer.Transform(x)).ToList()             
                };
            }
            catch(RepoException e)
            {
                return new GetEventsByUserIdResponseDTO()
                {
                    Status = e.Message
                };
            }
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
            try
            {
                List<EventRecord> eventsInTimeInterval = _eventRepo.GetEventsByTimeRange(timeInterval.StartTime, timeInterval.EndTime);
                return new GetEventsByTimeRangeResponseDTO
                {
                    Events = eventsInTimeInterval.Select(x => EventRecordTransformer.Transform(x)).ToList()
                };
            }
            catch(RepoException e)
            {
                return new GetEventsByTimeRangeResponseDTO()
                {
                    Status = e.Message
                };
            }
        }

        /// <summary>
        /// Retrieves all within a given distance from given coordinates
        /// </summary>
        /// <param name="region"></param>
        /// <returns>List of event records with the radius of (X,Y).</returns>
        [Route("getEventsInRadius/")]
        [ResponseType(typeof(GetAllEventsResponseDTO))]
        [HttpGet]
        public GetEventsByRadiusResponseDTO GetEventsByRadius([FromBody]GetEventsByRadiusRequestDTO region)
        {
            try
            {
                List<EventRecord> events = _eventRepo.GetEventsByRadius(region.X, region.Y, region.Radius);
                return new GetEventsByRadiusResponseDTO()
                {
                    EventRecords = events.Select(x => EventRecordTransformer.Transform(x)).ToList()
                };
            }
            catch (RepoException e)
            {
                return new GetEventsByRadiusResponseDTO()
                {
                    Status = e.Message
                };
            }
        }
    }
}
