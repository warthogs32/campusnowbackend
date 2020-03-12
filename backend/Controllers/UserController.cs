using backend.DTOs;
using backend.Models;
using backend.Repositories;
using backend.Transformers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using backend.Exceptions;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace backend.Controllers
{
    [ExcludeFromCodeCoverage]
    [RoutePrefix("api/user")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        private UserRepository _userRepo = new UserRepository();

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
            try
            {
                string newUserResponse = _userRepo.PostNewUser(UserRecordTransformer.Transform(newUserRequest.NewUserRecord));
                return new PostNewUserResponseDTO()
                {
                    Status = newUserResponse
                };
            }
            catch(RepoException e)
            {
                return new PostNewUserResponseDTO()
                {
                    Status = e.Message
                };
            }
        }

        /// <summary>
        /// Gets a user with the given userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>A UserRecord of the user of the matching id</returns>
        [HttpGet]
        [ResponseType(typeof(GetUserByIdResponseDTO))]
        [Route("getById/")]
        public GetUserByIdResponseDTO GetUserById([FromUri]GetUserByIdRequestDTO userId)
        {
            try
            {
                return new GetUserByIdResponseDTO
                {
                    UserRecord = UserRecordTransformer.Transform(_userRepo.GetUserById(userId.UserId))
                };
            }
            catch(RepoException e)
            {
                return new GetUserByIdResponseDTO()
                {
                    Status = e.Message
                };
            }
        }
    }
}
