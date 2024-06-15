using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using System.Security.Claims;
using youtube.Web.Models;
using youtube.Web.Service.IService;
using youtube.Web.Utility;

namespace youtube.Web.Service
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IBaseService baseService, IHttpContextAccessor httpContextAccessor)
        {
            _baseService = baseService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ResponseDto?> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/AssignRole"
            });
        }

        public async Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto)
        {
            var response = await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);

            if (response != null && response.IsSuccess)
            {
                var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(response.Result.ToString());
                if (loginResponse != null && loginResponse.User != null)
                {
                    // Add profile picture URL to claims
                    var claims = new List<Claim>
                {
                    new Claim("ProfilePicUrl", loginResponse.User.ProfilePicUrl ?? string.Empty),
                    new Claim(ClaimTypes.Name, loginResponse.User.Name),
                    new Claim(ClaimTypes.Email, loginResponse.User.Email),
                    
                };

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    // Use IHttpContextAccessor to access HttpContext
                    await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                }
            }

            return response;
        }



        public async Task<ResponseDto?> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = registrationRequestDto,
                Url = SD.AuthAPIBase + "/api/auth/register",
                ContentType = SD.ContentType.MultipartFormData
            }, withBearer: false);
        }
    }
}
