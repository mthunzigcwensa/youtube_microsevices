using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using youtube.Web.Models;
using youtube.Web.Service.IService;

namespace youtube.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IVideoService _videoService;


        public HomeController(IVideoService videoService)
        {
            _videoService = videoService;
        }

        public async Task<IActionResult> IndexAsync()
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

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
