using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using backend.DTOs;
using backend.Repositories;
using System.Web.Http.Description;
using backend.Models;
using backend.Transformers;
using System.Web.Http.Cors;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Diagnostics.CodeAnalysis;
using backend.Repositories;
using backend.DTOs;
using backend.Models;
using backend.Transformers;

namespace backend.Controllers
{
    [ExcludeFromCodeCoverage]
    [RoutePrefix("api/user")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private UserRepository _userRepo = new UserRepository();

        /// <summary>
        /// Creates an event bookmark for a given user
        /// </summary>
        /// <param name="addBookmarkRequest"></param>
        /// <returns>True for success, false for post failure.</returns>
        [Route("addBookmark/")]
        [ResponseType(typeof(AddNewBookmarkResponseDTO))]
        [HttpPost]
        public AddNewBookmarkResponseDTO AddNewBookmark([FromBody]AddNewBookmarkRequestDTO addBookmarkRequest)
        {
            EventRecord eventRecord = EventRecordTransformer.Transform(addBookmarkRequest.EventRecord);
            UserRecord userRecord = UserRecordTransformer.Transform(addBookmarkRequest.UserRecord);

            bool status = _userRepo.AddBookmark(userRecord, eventRecord);
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
        [Route("getAllEvents/")]
        [ResponseType(typeof(GetAllEventsResponseDTO))]
        [HttpGet]
        public GetBookmarkedEventsResponseDTO GetAllEvents([FromBody]GetBookmarkedEventsRequestDTO user)
        {
            UserRecord userRecord = UserRecordTransformer.Transform(user.User);

            List<EventRecord> allEvents = _userRepo.GetBookmarkedEvents(userRecord);
            return new GetBookmarkedEventsResponseDTO()
            {
                EventRecords = allEvents.Select(x => EventRecordTransformer.Transform(x)).ToList()
            };
        }
        /// <summary>
        /// Register a new user.
        /// </summary>
        /// <param name="newUserRequest"></param>
        /// <returns>True for success, false for failure.</returns>
        [HttpPost]
        [ResponseType(typeof(PostNewUserResponseDTO))]
        [Route("newUser/")]
        public PostNewUserResponseDTO PostNewUser([FromBody]PostNewUserRequestDTO newUserRequest)
        {
            bool newUserResponse = _userRepo.PostNewUser(UserRecordTransformer.Transform(newUserRequest.NewUserRecord));
            return new PostNewUserResponseDTO()
            {
                Status = newUserResponse
            };
        }
    }
}
