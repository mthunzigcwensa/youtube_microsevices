using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using youtube.Services.AuthAPI.Models.Dto;
using youtube.Services.AuthAPI.Service.IService;

namespace youtube.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        protected ResponseDto _response;
        
        private readonly IConfiguration _configuration;


        public AuthAPIController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _response = new();
            
            _configuration = configuration;

        }
        
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromForm] RegistrationRequestDto model)
        {
            if (ModelState.IsValid)
            {
                var errorMessage = await _authService.Register(model);
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _response.IsSuccess = false;
                    _response.Message = errorMessage;
                    return BadRequest(_response);
                }

                return Ok(_response);
            }
            else
            {
                _response.IsSuccess = false;
                _response.Message = "Invalid model state";
                return BadRequest(_response);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var loginResponse = await _authService.Login(model);

            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.Message = "Username or password is incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody] RegistrationRequestDto model)
        {
            var assignRoleSuccessful = await _authService.AssignRole(model.Email, model.Role.ToUpper());
            if (!assignRoleSuccessful)
            {
                _response.IsSuccess = false;
                _response.Message = "Error encountered";
                return BadRequest(_response);
            }
            return Ok(_response);

        }
    }
}
