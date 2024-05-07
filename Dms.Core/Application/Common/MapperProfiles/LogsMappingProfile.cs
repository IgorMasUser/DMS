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
             //.ForMember(dto => dto.Id, opt => opt.MapFrom(src => src.Id))
             //.ForMember(dto => dto.Message, opt => opt.MapFrom(src => src.Message))
             //.ForMember(dto => dto.MessageTemplate, opt => opt.MapFrom(src => src.MessageTemplate))
             //.ForMember(dto => dto.Level, opt => opt.MapFrom(src => src.Level))
             //.ForMember(dto => dto.TimeStamp, opt => opt.MapFrom(src => src.TimeStamp))
             //.ForMember(dto => dto.Exception, opt => opt.MapFrom(src => src.Exception))
             //.ForMember(dto => dto.Properties, opt => opt.MapFrom(src => src.Properties))
             //.ForMember(dto => dto.LogEvent, opt => opt.MapFrom(src => src.LogEvent))
             //.ForMember(dto => dto.CorrelationId, opt => opt.MapFrom(src => src.CorrelationId));
        }
    }
}