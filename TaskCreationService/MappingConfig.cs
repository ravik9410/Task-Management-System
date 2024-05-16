using AutoMapper;
using TaskCreationService.Models;
using TaskCreationService.Models.DTO;

namespace TaskCreationService
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterConfig()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<UserTaskDto,UserTask>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
