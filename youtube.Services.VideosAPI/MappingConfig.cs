using AutoMapper;
using youtube.Services.VideosAPI.Models;
using youtube.Services.VideosAPI.Models.Dto;
namespace onlineStore.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<VideoDto, Video>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
