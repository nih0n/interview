using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Solution.Application.Services;
using System;

namespace Solution.API.Controllers
{
    [Route("auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtTokenGenerator _tokenGenerator;

        public AuthenticationController(IJwtTokenGenerator tokenGenerator) => _tokenGenerator = tokenGenerator;

        [HttpPost]
        public IActionResult Authenticate()
        {
            try
            {
                var token = _tokenGenerator.GenerateToken();

                return Ok(token);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
