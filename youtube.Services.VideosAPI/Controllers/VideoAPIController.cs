using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using youtube.Services.VideosAPI.Data;
using youtube.Services.VideosAPI.Models;
using youtube.Services.VideosAPI.Models.Dto;

namespace youtube.Services.VideosAPI.Controllers
{
    [Route("api/video")]
    [ApiController]
    public class VideoAPIController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private IMapper _mapper;
        public VideoAPIController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<Video> objList = _db.Videos.ToList();
                _response.Result = _mapper.Map<IEnumerable<VideoDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }




        [Authorize]
        [HttpGet("GetVideo/{id}")]
        public ResponseDto Get(string id)
        {
            var _response = new ResponseDto();

            try
            {
                var videosList = _db.Videos.Where(u => u.UploadedBy == id).ToList();
                _response.Result = _mapper.Map<IEnumerable<VideoDto>>(videosList);
                _response.IsSuccess = true;
                _response.Message = "Videos retrieved successfully";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }


        [HttpPost]
       // [Authorize]
        public ResponseDto Post(VideoDto videoDto)
        {
            try
            {
                Video video = _mapper.Map<Video>(videoDto);
                _db.Videos.Add(video);
                _db.SaveChanges();

                if (videoDto.Thumbnail != null)
                {
                    string fileName = video.VideoId + Path.GetExtension(videoDto.Thumbnail.FileName);
                    string filePath = @"wwwroot\Thumbnails\" + fileName;

                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    FileInfo file = new FileInfo(directoryLocation);
                    if (file.Exists)
                    {
                        file.Delete();
                    }

                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        videoDto.Thumbnail.CopyTo(fileStream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    video.ThumbnailUrl = baseUrl + "/ProductImages/" + fileName;
                    video.ThumbnailLocalPath = filePath;

                }

                if (videoDto.Video != null)
                {
                    // Generate the file name using the VideoId and the original file extension
                    string fileName = video.VideoId + Path.GetExtension(videoDto.Video.FileName);
                    // Define the file path to save the video in the "videos" directory
                    string filePath = Path.Combine("wwwroot", "videos", fileName);

                    // Get the absolute path for the file
                    var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    FileInfo file = new FileInfo(directoryLocation);
                    if (file.Exists)
                    {
                        // Delete the existing file if it exists
                        file.Delete();
                    }

                    // Create a new file stream to save the uploaded video
                    using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                    {
                        videoDto.Video.CopyTo(fileStream);
                    }

                    // Generate the base URL for the video
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    // Set the VideoUrl property to the public URL of the uploaded video
                    video.VideoUrl = baseUrl + "/videos/" + fileName;
                    // Set the VideoLocalPath property to the local path of the uploaded video
                    video.VideoLocalPath = filePath;
                }


                _db.Videos.Update(video);
                _db.SaveChanges();
                _response.Result = _mapper.Map<VideoDto>(video);


            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpPut]
        //[Authorize]
        public ResponseDto Put(VideoDto videoDto)
        {

            try
            {
                Video video = _mapper.Map<Video>(videoDto);
                if (videoDto.Thumbnail != null)
                {
                    if (!string.IsNullOrEmpty(video.ThumbnailLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), video.ThumbnailLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }

                        string fileName = video.VideoId + Path.GetExtension(videoDto.Thumbnail.FileName);
                        string filePath = @"wwwroot\ProductImages\" + fileName;
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            videoDto.Thumbnail.CopyTo(fileStream);
                        }
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        video.ThumbnailUrl = baseUrl + "/ProductImages/" + fileName;
                        video.ThumbnailUrl = filePath;
                    }

                    if (videoDto.Video != null)
                    {
                        // Check if there is an existing video file to delete
                        if (!string.IsNullOrEmpty(video.VideoLocalPath))
                        {
                            var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), video.VideoLocalPath);
                            FileInfo file = new FileInfo(oldFilePathDirectory);
                            if (file.Exists)
                            {
                                file.Delete();
                            }
                        }

                        // Generate the new file name using the VideoId and the file extension of the uploaded video
                        string fileName = video.VideoId + Path.GetExtension(videoDto.Video.FileName);
                        // Define the file path to save the video in the "videos" directory
                        string filePath = Path.Combine("wwwroot", "videos", fileName);

                        // Get the absolute path for the file
                        var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                        // Save the uploaded video to the specified path using a FileStream
                        using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                        {
                            videoDto.Video.CopyTo(fileStream);
                        }

                        // Generate the base URL for the video
                        var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                        // Set the VideoUrl property to the public URL of the uploaded video
                        video.VideoUrl = baseUrl + "/videos/" + fileName;
                        // Set the VideoLocalPath property to the local path of the uploaded video
                        video.VideoLocalPath = filePath;
                    }

                    _db.Videos.Update(video);
                    _db.SaveChanges();

                    _response.Result = _mapper.Map<VideoDto>(video);

                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;

        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize]
        public ResponseDto Delete(int id)
        {
            try
            {
                Video obj = _db.Videos.First(u => u.VideoId == id);
                if (!string.IsNullOrEmpty(obj.ThumbnailLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), obj.ThumbnailLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }

                if (!string.IsNullOrEmpty(obj.VideoLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), obj.VideoLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                _db.Videos.Remove(obj);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

    }
}
