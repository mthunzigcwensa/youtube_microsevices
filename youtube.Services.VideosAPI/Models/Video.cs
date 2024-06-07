using System.ComponentModel.DataAnnotations;

namespace youtube.Services.VideosAPI.Models
{
    public class Video
    {
        [Key]
        public int VideoId { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string UploadedBy { get; set; }
        [Required]
        public string VideoUrl { get; set; }
        public string? VideoLocalPath { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? ThumbnailLocalPath { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
