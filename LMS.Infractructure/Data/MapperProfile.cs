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
        CreateMap<ModuleCreateDto, ModuleDto>().ReverseMap();
        CreateMap<ModuleUpdateDto, Module>().ReverseMap();
        CreateMap<ModuleUpdateDto, ModuleDto>().ReverseMap();

        CreateMap<ModuleActivityDto, ModuleActivity>().ReverseMap();
        CreateMap<ModuleActivityCreateDto, ModuleActivity>()
    .ForMember(dest => dest.Assignments, opt => opt.MapFrom(src => src.Assignments));
        CreateMap<ModuleActivityCreateDto, ModuleActivityDto>().ReverseMap();
        CreateMap<ModuleActivityUpdateDto, ModuleActivity>().ReverseMap();
        CreateMap<ModuleActivityUpdateDto, ModuleActivityDto>().ReverseMap();

        CreateMap<ApplicationUser, UserDto>().ReverseMap();
        CreateMap<ApplicationUser, UserUpdateDto>().ReverseMap();

        CreateMap<CourseDto, Course>().ReverseMap();
        CreateMap<CourseUpdateDto, Course>().ReverseMap();
        CreateMap<CourseUpdateDto, CourseDto>().ReverseMap();
        CreateMap<CourseCreateDto, Course>().ReverseMap();
        CreateMap<CourseCreateDto, CourseDto>().ReverseMap();

        CreateMap<DocumentDto, Document>().ReverseMap();
        CreateMap<DocumentCreateDto, Document>().ReverseMap();

        CreateMap<SubmissionCreateDto, Submission>().ReverseMap();
        CreateMap<SubmissionDto, Submission>();//.ReverseMap();
        CreateMap<ActivityTypeDto, ActivityType>().ReverseMap();

        CreateMap<Submission, SubmissionDto>()
        .ForMember(dest => dest.DocumentDto, opt => opt.MapFrom(src => src.SubmissionDocument));


        CreateMap<AssignmentCreateDto, Assignment>()
    .ForMember(dest => dest.Documents, opt => opt.Ignore());
        CreateMap<AssignmentDto, Assignment>().ReverseMap();
    }
}
