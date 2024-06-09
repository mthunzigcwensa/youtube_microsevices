using System.Diagnostics;
using System.Text.RegularExpressions;
using youtube.Web.Models;
using youtube.Web.Service.IService;
using youtube.Web.Utility;

namespace youtube.Web.Service
{
    public class VideoService : IVideoService
    {
        private readonly IBaseService _baseService;

        public VideoService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateVideosAsync(VideoDto video)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = video,
                Url = SD.VideoAPIBase + "/api/video",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

        public async Task<ResponseDto?> DeleteVideosAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.VideoAPIBase + "/api/video/" + id
            });
        }

        public async Task<ResponseDto?> GetAllVideosAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.VideoAPIBase + "/api/video"
            });
        }

        public async Task<ResponseDto?> GetVideoByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.VideoAPIBase + "/api/video/" + id
            });
        }

        public async Task<ResponseDto?> UpdateVideosAsync(VideoDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = productDto,
                Url = SD.VideoAPIBase + "/api/video",
                ContentType = SD.ContentType.MultipartFormData
            });
        }

    }
}
