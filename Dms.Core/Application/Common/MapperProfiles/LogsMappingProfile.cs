using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Common.MapperProfiles
{
    public class LogsMappingProfile : Profile
    {
        public LogsMappingProfile()
        {
            CreateMap<Logger, LogDto>().ReverseMap();
        }
    }
}