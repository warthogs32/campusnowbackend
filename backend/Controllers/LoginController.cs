using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.IO;
using System.Net.Http;
using System.Web;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using backend.DTOs;
using System.Web.Http;
using backend.Repositories;
using System.Web.Http.Description;
using System.Web.Http.Cors;

namespace backend.Controllers
{
    [RoutePrefix("user/login")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private LoginRepository _loginRepo = new LoginRepository();

        /// <summary>
        /// User login.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>JSON web token when authenticated, or unauthorized status code for failure.</returns>
        [Route("authenticate/")]
        [ResponseType(typeof(IHttpActionResult))]
        [HttpPost]
        public IHttpActionResult Authenticate([FromBody] LoginRequestDTO login)
        {
            var loginResponse = new LoginResponseDTO();
            LoginRequestDTO loginrequest = new LoginRequestDTO();
            loginrequest.Username = login.Username.ToLower();
            loginrequest.Password = login.Password;

            IHttpActionResult response;
            HttpResponseMessage responseMsg = new HttpResponseMessage();
            bool isUsernamePasswordValid = _loginRepo.IsUserLoginValid(loginrequest.Username, loginrequest.Password);
            // if credentials are valid
            if (isUsernamePasswordValid)
            {
                string token = createToken(loginrequest.Username);
                LoginRepository.CurrentUser.Token = token;
                //return the token
                return Ok<string>(token);
            }
            else
            {
                // if credentials are not valid send unauthorized status code in response
                loginResponse.responseMsg = new HttpResponseMessage();
                loginResponse.responseMsg.StatusCode = HttpStatusCode.Unauthorized;
                response = ResponseMessage(loginResponse.responseMsg);
                return response;
            }
        }

        /// <summary>
        /// User logout
        /// </summary>
        /// <returns>True for success, false for failure.</returns>
        [Route("logout/")]
        [ResponseType(typeof(LogoutResponseDTO))]
        [HttpGet]
        public LogoutResponseDTO Logout()
        {
            return new LogoutResponseDTO()
            { 
                status = LoginRepository.Logout(LoginRepository.CurrentUser.Token)
            };
        }

        private string createToken(string username)
        {
            //Set issued at date
            DateTime issuedAt = DateTime.Now;
            //set the time when it expires
            DateTime expires = issuedAt.AddDays(23);

            //http://stackoverflow.com/questions/18223868/how-to-encrypt-jwt-security-token
            var tokenHandler = new JwtSecurityTokenHandler();

            //create a identity and add claims to the user which we want to log in
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, username)
            });

            const string sec = "401b09eab3c013d4ca54922bb802bec8fd5318192b0a75f201d8b3727429090fb337591abd3e44453b954555b7a0812e1081c39b740293f765eae731f5a65ed1";
            var now = DateTime.UtcNow;
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(sec));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);


            //create the jwt
            var token =
                (JwtSecurityToken)
                    tokenHandler.CreateJwtSecurityToken(issuer: "http://localhost:50191", audience: "http://localhost:50191",
                        subject: claimsIdentity, notBefore: issuedAt, expires: expires, signingCredentials: signingCredentials);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }
    }
}
