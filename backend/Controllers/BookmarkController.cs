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
using backend.DTOs;
using backend.Transformers;
using backend.Models;

namespace backend.Controllers
{
    [ExcludeFromCodeCoverage]
    [RoutePrefix("api/bookmark")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BookmarkController : ApiController
    {
        private BookmarkRepository _bookmarkRepo = new BookmarkRepository();

        /// <summary>
        /// Creates an event bookmark for a given user
        /// </summary>
        /// <param name="addBookmarkRequest"></param>
        /// <returns>True for success, false for post failure.</returns>
        [Route("addBookmark/")]
        [ResponseType(typeof(AddNewBookmarkResponseDTO))]
        [HttpPost]
        public AddNewBookmarkResponseDTO PostAddNewBookmark([FromBody]AddNewBookmarkRequestDTO addBookmarkRequest)
        {
            EventRecord eventRecord = EventRecordTransformer.Transform(addBookmarkRequest.EventRecord);
            UserRecord userRecord = UserRecordTransformer.Transform(addBookmarkRequest.UserRecord);

            bool status = _bookmarkRepo.AddNewBookmark(userRecord, eventRecord);
            return new AddNewBookmarkResponseDTO()
            {
                Status = status
            };
        }

        /// <summary>
        /// Gets all of a user's bookmarked events
        /// </summary>
        /// <param name="user"></param>
        /// <returns>List of bookmarked event records.</returns>
        [Route("getAllBookmarkedEvents/")]
        [ResponseType(typeof(GetAllEventsResponseDTO))]
        [HttpGet]
        public GetBookmarkedEventsResponseDTO GetAllBookmarkedEvents([FromBody]GetBookmarkedEventsRequestDTO user)
        {
            UserRecord userRecord = UserRecordTransformer.Transform(user.User);

            List<EventRecord> allEvents = _bookmarkRepo.GetAllBookmarkedEvents(userRecord);
            return new GetBookmarkedEventsResponseDTO()
            {
                EventRecords = allEvents.Select(x => EventRecordTransformer.Transform(x)).ToList()
            };
        }
    }
}
