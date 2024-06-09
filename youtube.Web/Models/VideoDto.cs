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

        public string TimeAgo
        {
            get
            {
                var timeSpan = DateTime.UtcNow - UploadDate;
                if (timeSpan.TotalMinutes < 1)
                    return "Just now";
                if (timeSpan.TotalMinutes < 60)
                    return $"{timeSpan.Minutes} minutes ago";
                if (timeSpan.TotalHours < 24)
                    return $"{timeSpan.Hours} hours ago";
                if (timeSpan.TotalDays < 7)
                    return $"{timeSpan.Days} days ago";
                if (timeSpan.TotalDays < 30)
                    return $"{timeSpan.Days / 7} weeks ago";
                if (timeSpan.TotalDays < 365)
                    return $"{timeSpan.Days / 30} months ago";
                return $"{timeSpan.Days / 365} years ago";
            }
        }
    }

}
