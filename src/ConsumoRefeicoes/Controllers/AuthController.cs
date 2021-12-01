using System;
using System.Threading.Tasks;
using backend.Interfaces.Services.Security;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [ApiController]
    [Route("/auth")]
    [Authorize]
    public class AuthController : ControllerBase
    {
        protected readonly ILogin _loginService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(ILogin loginService, IHttpContextAccessor httpContextAccessor)
        {
            _loginService = loginService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Login loginData)
        {
            Response response = new Response();
            try
            {
                ResponseLogin responseLogin = await _loginService.Authenticate(loginData);

                response.Success = true;
                response.Data = responseLogin;
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("check")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
        public bool Check()
        {
            var user = _httpContextAccessor.HttpContext?.User;
            string userName = user.Identity.Name;
            if (userName == null)
            {
                return false;
            }
            return true;


        }
    }
}
