using youtube.Web.Models;

namespace youtube.Web.Service.IService
{
    public interface IVideoService
    {
        Task<ResponseDto?> GetAllVideosAsync();
        Task<ResponseDto?> GetVideoByIdAsync(int id);
        Task<ResponseDto?> CreateVideosAsync(VideoDto productDto);
        Task<ResponseDto?> UpdateVideosAsync(VideoDto productDto);
        Task<ResponseDto?> DeleteVideosAsync(int id);
        
    }
}
