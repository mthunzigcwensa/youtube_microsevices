namespace youtube.Web.Models
{
    public class VideoDto
    {
        public int VideoId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        public string? VideoUrl { get; set; }
        public string? VideoLocalPath { get; set; }
        public IFormFile Video { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? ThumbnailLocalPath { get; set; }
        public IFormFile Thumbnail { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
