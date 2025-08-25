using AutoMapper;
using Domain.Models.Entities;
using LMS.Shared.DTOs.AuthDtos;
using LMS.Shared.DTOs.EntityDto;

namespace LMS.Infractructure.Data;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<UserRegistrationDto, ApplicationUser>();
        CreateMap<ModuleDto, Module>().ReverseMap();
        CreateMap<ModuleCreateDto, Module>().ReverseMap();
        CreateMap<ModuleUpdateDto, Module>().ReverseMap();
        CreateMap<ModuleActivityDto, ModuleActivity>().ReverseMap();
        CreateMap<ModuleActivityCreateDto, ModuleActivity>().ReverseMap();
        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<ApplicationUser, UserUpdateDto>().ReverseMap();
        CreateMap<CourseDto, Course>().ReverseMap();
        CreateMap<CourseCreateDto, Course>().ReverseMap();
        CreateMap<DocumentDto, Document>().ReverseMap();
        CreateMap<DocumentCreateDto, Document>().ReverseMap();
        CreateMap<ActivityTypeDto, ActivityType>().ReverseMap();
    }
}
