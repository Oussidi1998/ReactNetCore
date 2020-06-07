using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TP2_NET_CORE_2.Models;

namespace TP2_NET_CORE_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly IConfiguration _config;
        public readonly IEnumerable<User> _users = new List<User>
        {
            new User{Id=1, Username="Oussidi",Password="password",Role="admin" },
            new User{Id=2, Username="Roudani",Password="password",Role="user" },
        };

        public UsersController(IConfiguration config)
        {
            _config = config;
        }

        //// GET: api/Users
        [HttpGet]
        [Authorize]
        public string Get()
        {
            Console.WriteLine("Get users data action was called");

            var userData = HttpContext.User.Claims.Where(x => x.Type == "Id").SingleOrDefault();

            return "User with ID " + userData.Value + " is connected";
        }

        [HttpGet] 
        [Authorize]
        public IActionResult Gestion()
        {

            Debug.WriteLine("Gestion action was called");

            var userData = HttpContext.User.Claims.Where(x => x.Type == "Id").SingleOrDefault();

            var user = _users.Where(x => x.Id == Int32.Parse(userData.Value)).SingleOrDefault();

            if (user!=null)
            {
                return Ok(user);
            }
            else
            {
                return Unauthorized();
            }

           
        }

        // POST: api/Users
        [HttpPost]
        public IActionResult Post([FromBody] User user)
        {
            Debug.WriteLine("Post action was called");

            if (user != null)
            {
                var CreateUserToken = GenerateToken(user);

                if (CreateUserToken != null)
                {
                    return Ok(CreateUserToken);
                }

            }
            return Unauthorized();

            // return CreateUserToken != null ? Ok(CreateUserToken) : Unauthorized();
        }

       

        private string GenerateToken(User user)
        {
            Debug.WriteLine("generateToken func was called");


            // get the user data
            var getUser = _users.Where(x => x.Username == user.Username && x.Password == user.Password).SingleOrDefault();
            if (getUser == null)
                return null;

            // prepare the settings for generating token
            var key = Convert.FromBase64String(_config["Jwt:key"]);
            var expireOn = int.Parse(_config["Jwt:ExpiryDUration"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.Now,
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddMinutes(expireOn),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(
                    new List<Claim>
                    {
                        new Claim("Id", getUser.Id.ToString()),
                        new Claim("Username", getUser.Username.ToString()),
                        new Claim("Role", getUser.Role.ToString()),
                    })
            };


            // create the token based on settings above
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);


            return tokenHandler.WriteToken(jwtToken);

        }
    }
}
