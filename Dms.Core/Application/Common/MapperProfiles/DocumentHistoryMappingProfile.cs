using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Common.MapperProfiles
{
    public class DocumentHistoryMappingProfile : Profile
    {
        public DocumentHistoryMappingProfile()
        {
            CreateMap<DocumentHistory, DocumentHistoryDto>().ReverseMap();
        }
    }
}
