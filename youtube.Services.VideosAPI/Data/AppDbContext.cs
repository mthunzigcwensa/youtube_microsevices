using Microsoft.EntityFrameworkCore;
using youtube.Services.VideosAPI.Models;

namespace youtube.Services.VideosAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Video> Videos { get; set; }
       
    }
}
