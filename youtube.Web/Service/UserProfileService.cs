namespace youtube.Web.Service
{
    public class UserProfileService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProfileService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetProfilePicUrl()
        {
            var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
            var profilePicUrl = claimsPrincipal?.FindFirst("ProfilePicUrl")?.Value;

            return profilePicUrl ?? string.Empty;
        }
    }
}
