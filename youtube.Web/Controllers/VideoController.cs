using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using youtube.Web.Models;
using youtube.Web.Service;
using youtube.Web.Service.IService;

namespace youtube.Web.Controllers
{
    public class VideoController : Controller
    {
        private readonly IVideoService _videoService;
        public VideoController(IVideoService videoService)
        {
            _videoService = videoService;
        }
        public async Task<IActionResult> VideoIndex()
        {
            List<VideoDto>? list = new();
            ResponseDto? response = await _videoService.GetAllVideosAsync();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VideoDto>>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }
            return View(list);
        }

        public async Task<IActionResult> VideoCreate()
        {
            return View();
        }

        public async Task<IActionResult> getUserData()
        {
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VideoCreate(VideoDto model)
        {
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;

            if (ModelState.IsValid)
            {
                model.UploadedBy = userId;
                ResponseDto? response = await _videoService.CreateVideosAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Video created successfully";
                    return RedirectToAction(nameof(VideoIndex));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
                
            }

            return View(model);

        }
    }
}
