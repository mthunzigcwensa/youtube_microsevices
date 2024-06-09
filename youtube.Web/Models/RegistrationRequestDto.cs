﻿using System.ComponentModel.DataAnnotations;

namespace youtube.Web.Models
{
    public class RegistrationRequestDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Password { get; set; }
        public string? Role { get; set; }      
        public string? ProfilePicUrl { get; set; }
        public string? ProfilePicLocalPath { get; set; }
        public IFormFile? ProfilePic { get; set; }
    }
}
