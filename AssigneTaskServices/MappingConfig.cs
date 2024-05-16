using AutoMapper;
using AssigneTaskServices.Models;
using AssigneTaskServices.Models.DTO;

namespace AssigneTaskServices
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterConfig()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<AssignedUserTask, AssignedUserTaskDto>().ReverseMap();
            });
            return mappingConfig;
        }
    }
}
