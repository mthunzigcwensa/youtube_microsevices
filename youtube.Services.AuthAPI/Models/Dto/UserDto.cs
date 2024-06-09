namespace youtube.Services.AuthAPI.Models.Dto
{
    public class UserDto
    {
        public string ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicLocalPath { get; set; }
        public IFormFile? ProfilePic { get; set; }
    }
}
