using AutoMapper;
using DailyDynamo.Shared.Models.DTO;
using DailyDynamo.Shared.Models.DTO.Employee;
using DailyDynamo.Shared.Models.DTO.WorkDiary;
using DailyDynamo.Shared.Models.Entities;

namespace DailyDynamo.Services.AutoMapperProfile
{
    public class MappingProfile : AutoMapper.Profile
    {
        public MappingProfile()
        {
            CreateMap<SignUpViewModel, Shared.Models.Entities.Profile>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid()))
                .ReverseMap();


            CreateMap<Shared.Models.Entities.Profile, ClaimDataViewModel>();


        }


    }

    public class WorkDiaryProfile : AutoMapper.Profile
    {
        public WorkDiaryProfile()
        {
            CreateMap<WorkDiaryRequest, WorkDiary>()
            .ForMember(dest => dest.ReportDate, opt => opt.MapFrom(src => DateOnly.Parse(src.ReportDate)));
            CreateMap<WorkDiary, WorkDiaryResponse>();
            CreateMap<WorkDiary, WorkDiaryGetResponse>()
                .ForMember(dsr => dsr.Name, opt => opt.MapFrom(src => src.CreatedByNavigation.FirstName + " " + src.CreatedByNavigation.LastName));
        }

    }

    public class ProfileMapperProfile : AutoMapper.Profile
    {
        public ProfileMapperProfile()
        {
            CreateMap<ProfilePatchRequest, Shared.Models.Entities.Profile>();
            CreateMap<ProfileUpdateRequest, Shared.Models.Entities.Profile>();
            CreateMap<Shared.Models.Entities.Profile, ProfileResponse>();
            CreateMap<Shared.Models.Entities.Profile, ProfilePatchResponse>();
        }

    }
}
