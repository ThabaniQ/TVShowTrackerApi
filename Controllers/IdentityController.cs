using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TVShowTracker.Contracts.Request;
using TVShowTracker.Domain;
using TVShowTracker.Model;
using TVShowTracker.Repository.Interface;

namespace TVShowTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> UserRegistration([FromBody] UserRegistrationDTO userRegistration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(errors => errors.Errors.Select(x => x.ErrorMessage))
                });
            }

            var authResponse = await _identityService.RegisterAsync(userRegistration);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            var authResponse = await _identityService.LoginAsync(userLogin);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token
            });
        }
    }
}
