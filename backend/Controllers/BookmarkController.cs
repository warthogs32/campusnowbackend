using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http.Description;
using backend.Repositories;
using backend.Exceptions;
using backend.DTOs;
using backend.Transformers;
using backend.Models;

namespace backend.Controllers
{
    [RoutePrefix("api/bookmark")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BookmarkController : ApiController
    {
        private BookmarkRepository _bookmarkRepo = new BookmarkRepository();

        /// <summary>
        /// Creates an event bookmark for current user
        /// </summary>
        /// <param name="addBookmarkRequest"></param>
        /// <returns>True for success, false for post failure.</returns>
        [Route("addBookmark/")]
        [ResponseType(typeof(AddNewBookmarkResponseDTO))]
        [Authorize]
        [HttpPost]
        public AddNewBookmarkResponseDTO PostAddNewBookmark([FromBody]AddNewBookmarkRequestDTO addBookmarkRequest)
        {
            try
            {
                EventRecord eventRecord = EventRecordTransformer.Transform(addBookmarkRequest.EventRecord);
                string addNewBookmarkResponse = _bookmarkRepo.AddNewBookmark(eventRecord);
                return new AddNewBookmarkResponseDTO()
                {
                    Status = addNewBookmarkResponse
                };
            }
            catch(RepoException e)
            {
                return new AddNewBookmarkResponseDTO()
                {
                    Status = e.Message
                };
            }
        }

        /// <summary>
        /// Gets all of current user's bookmarked events.
        /// </summary>
        /// <returns>List of bookmarked event records.</returns>
        [Route("getAllBookmarkedEvents/")]
        [ResponseType(typeof(GetAllEventsResponseDTO))]
        [Authorize]
        [HttpGet]
        public GetBookmarkedEventsResponseDTO GetAllBookmarkedEvents()
        {
            try
            {
                List<EventRecord> allEvents = _bookmarkRepo.GetAllBookmarkedEvents();
                return new GetBookmarkedEventsResponseDTO()
                {
                    EventRecords = allEvents.Select(x => EventRecordTransformer.Transform(x)).ToList()
                };
            }
            catch(RepoException e)
            {
                return new GetBookmarkedEventsResponseDTO()
                {
                    Status = e.Message
                };
            }
        }
    }
}
