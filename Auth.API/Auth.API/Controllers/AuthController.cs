using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWT;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Auth.API.Controllers
{
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        const string secret = "c2R1SGVhbHRoRHJvbmVDb25uZWN0OnNkdTIwMTkwM0hlYWx0aERyb25l";

        [HttpGet]
        public IActionResult Get()
        {
            var token = this.HttpContext.Request.Headers.FirstOrDefault(header => header.Key == "Authorization").Value.FirstOrDefault();
            if (token == null)
                return BadRequest("Token missing");

            try
            {
                var json = new JwtBuilder()
                    .WithSecret(secret)
                    .MustVerifySignature()
                    .Decode(token);
                return Ok(json);
            }
            catch (TokenExpiredException)
            {
                return BadRequest("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                return BadRequest("Token has invalid signature");
            }
        }
    }
}
