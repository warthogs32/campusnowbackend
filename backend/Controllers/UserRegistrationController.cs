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

namespace backend.Controllers
{
    [RoutePrefix("user/register")]
    public class UserRegistrationController : ApiController
    {
        private UserRepository _userRepo = new UserRepository();

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
