﻿namespace youtube.Services.AuthAPI.Models.Dto
{
    public class RegistrationRequestDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Role { get; set; }
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicLocalPath { get; set; }
        public IFormFile? ProfilePic { get; set; }
    }
}
