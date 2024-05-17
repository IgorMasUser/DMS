using AutoMapper;
using Dms.Core.Application.Common.DTOs;
using Dms.Core.Domain.Entities;

namespace Dms.Core.Application.Common.MapperProfiles
{
    public class FileDataMappingProfile : Profile
    {
        public FileDataMappingProfile()
        {
            CreateMap<FileData, FileDataDto>()
              .AfterMap((s, d, ctx) =>
                {
                    d.DocumentHistory = s.DocumentHistory is not null
                    ? ctx.Mapper.Map<List<DocumentHistoryDto>>(s.DocumentHistory)
                    : new();
                });

            CreateMap<FileDataDto, FileData>(MemberList.None)
              .ForMember(d => d.DocumentHistory, opt => opt.Ignore());
        }
    }
}
