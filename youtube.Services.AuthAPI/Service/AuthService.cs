using Microsoft.AspNetCore.Identity;
using youtube.Services.AuthAPI.Data;
using youtube.Services.AuthAPI.Models.Dto;
using youtube.Services.AuthAPI.Models;
using youtube.Services.AuthAPI.Service.IService;

namespace youtube.Services.AuthAPI.Service
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> AssignRole(string email, string roleName)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
            if (user != null)
            {
                if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
                {
                    //create role if it does not exist
                    _roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
                await _userManager.AddToRoleAsync(user, roleName);
                return true;
            }
            return false;

        }


        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = _db.ApplicationUsers.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);

            if (user == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found , Generate JWT Token
            var roles = await _userManager.GetRolesAsync(user);
            var token = _jwtTokenGenerator.GenerateToken(user, roles);
            UserDto userDTO = new()
            {
                Email = user.Email,
                ID = user.Id,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber
            };

            LoginResponseDto loginResponseDto = new LoginResponseDto()
            {
                User = userDTO,
                Token = token
            };

            return loginResponseDto;
        }

        public async Task<string> Register(RegistrationRequestDto registrationRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registrationRequestDto.Email,
                Email = registrationRequestDto.Email,
                NormalizedEmail = registrationRequestDto.Email.ToUpper(),
                Name = registrationRequestDto.Name,
                PhoneNumber = registrationRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registrationRequestDto.Password);
                if (result.Succeeded)
                {
                    // Save profile picture if provided
                    if (registrationRequestDto.ProfilePic != null && registrationRequestDto.ProfilePic.Length > 0)
                    {
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profilePics");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + registrationRequestDto.ProfilePic.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await registrationRequestDto.ProfilePic.CopyToAsync(fileStream);
                        }

                        // Update user with profile picture details
                        user.ProfilePicLocalPath = filePath;
                        user.ProfilePicUrl = Path.Combine("/profilePics", uniqueFileName);
                    }

                    // Update user in the database
                    await _userManager.UpdateAsync(user);

                    // Retrieve the updated user
                    var userToReturn = _db.ApplicationUsers.First(u => u.UserName == registrationRequestDto.Email);

                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        ID = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber,
                        ProfilePicUrl = userToReturn.ProfilePicUrl, // Add profile pic URL to DTO
                        ProfilePicLocalPath = userToReturn.ProfilePicLocalPath // Add profile pic local path to DTO
                    };

                    // Return the user DTO or whatever you need
                    return ""; // You might return a token or user ID here instead
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return "Error Encountered";
        }
    }
}
