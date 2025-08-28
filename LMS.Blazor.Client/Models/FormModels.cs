using LMS.Shared.DTOs.EntityDto;
using System.ComponentModel.DataAnnotations;

namespace LMS.Blazor.Client.Models
{
    public class CourseFormModel
    {
        public CourseCreateDto CourseDto;

        private List<ModuleFormModel> _modules = new List<ModuleFormModel>();
        [ValidateComplexType]
        public List<ModuleFormModel> Modules
        {
            get => _modules;
            set => _modules = value;
        }
        public CourseFormModel()
        {
            CourseDto = new CourseCreateDto();
            _modules = CourseDto.Modules.Select(m => new ModuleFormModel(m)).ToList();
        }
        public CourseFormModel(CourseCreateDto courseDto)
        {
            CourseDto = courseDto;
            _modules = CourseDto.Modules.Select(m => new ModuleFormModel(m)).ToList();
        }
        public CourseCreateDto ToDto()
        {
            CourseDto.Modules = _modules.Select(m => m.ToDto()).ToList();
            return CourseDto;
        }
        [Required(ErrorMessage = "Please enter a course name")]
        [MaxLength(100, ErrorMessage = "Course name must be shorter than 100 characters")]
        public string Name
        {
            get => CourseDto.Name;
            set => CourseDto.Name = value;
        }
        public string? Description
        {
            get => CourseDto.Description;
            set => CourseDto.Description = value;
        }
        [Required]
        public DateTime StartDate
        {
            get => CourseDto.StartDate;
            set => CourseDto.StartDate = value;
        }
        [Required]
        public DateTime EndDate
        {
            get => CourseDto.EndDate;
            set => CourseDto.EndDate = value;
        }
    }
    public class ModuleFormModel
    {
        public ModuleCreateDto ModuleDto;

        public List<ModuleActivityFormModel> _moduleActivities = new List<ModuleActivityFormModel>();
        [ValidateComplexType]
        public List<ModuleActivityFormModel> ModuleActivities
        {
            get => _moduleActivities;
            set => _moduleActivities = value;
        }
        public ModuleFormModel()
        {
            ModuleDto = new ModuleCreateDto();
            _moduleActivities = ModuleDto.ModuleActivities.Select(a => new ModuleActivityFormModel(a)).ToList();
        }
        public ModuleFormModel(ModuleCreateDto moduleDto)
        {
            ModuleDto = moduleDto;
            _moduleActivities = ModuleDto.ModuleActivities.Select(a => new ModuleActivityFormModel(a)).ToList();
        }
        public ModuleCreateDto ToDto()
        {
            ModuleDto.ModuleActivities = _moduleActivities.Select(a => a.ModuleActivityDto).ToList();
            return ModuleDto;
        }
        [Required(ErrorMessage = "Please enter a module name")]
        [MaxLength(100, ErrorMessage = "Module name must be shorter than 100 characters")]
        public string Name
        {
            get => ModuleDto.Name;
            set => ModuleDto.Name = value;
        }
        public string? Description
        {
            get => ModuleDto.Description;
            set => ModuleDto.Description = value;
        }
        [Required]
        public DateTime StartDate
        {
            get => ModuleDto.StartDate;
            set => ModuleDto.StartDate = value;
        }
        [Required]
        public DateTime EndDate
        {
            get => ModuleDto.EndDate;
            set => ModuleDto.EndDate = value;
        }
        public Guid CourseId
        {
            get => ModuleDto.CourseId;
            set => ModuleDto.CourseId = value;
        }
    }
    public class ModuleActivityFormModel
    {
        public ModuleActivityCreateDto ModuleActivityDto;
        public ModuleActivityFormModel()
        {
            ModuleActivityDto = new ModuleActivityCreateDto();
        }
        public ModuleActivityFormModel(ModuleActivityCreateDto moduleActivityDto)
        {
            ModuleActivityDto = moduleActivityDto;
        }
        [Required(ErrorMessage = "Please enter an activity name")]
        [MaxLength(100, ErrorMessage = "Activity name must be shorter than 100 characters")]
        public string Name
        {
            get => ModuleActivityDto.Name;
            set => ModuleActivityDto.Name = value;
        }
        public string? Description
        {
            get => ModuleActivityDto.Description;
            set => ModuleActivityDto.Description = value;
        }
        [Required]
        public DateTime StartDate
        {
            get => ModuleActivityDto.StartDate;
            set => ModuleActivityDto.StartDate = value;
        }
        [Required]
        public DateTime EndDate
        {
            get => ModuleActivityDto.EndDate;
            set => ModuleActivityDto.EndDate = value;
        }
        [Required(ErrorMessage = "Please select an activity type")]
        public int? ActivityTypeId
        {
            get => ModuleActivityDto.ActivityTypeId;
            set => ModuleActivityDto.ActivityTypeId = value;
        }
        public Guid ModuleId
        {
            get => ModuleActivityDto.ModuleId;
            set => ModuleActivityDto.ModuleId = value;
        }
        public string StartTimeString
        {
            get => StartDate.ToString("HH:mm");
            set
            {
                if (TimeSpan.TryParse(value, out var ts))
                {
                    StartDate = StartDate.Date + ts;
                }
            }
        }
        public string EndTimeString
        {
            get => EndDate.ToString("HH:mm");
            set
            {
                if (TimeSpan.TryParse(value, out var ts))
                {
                    EndDate = EndDate.Date + ts;
                }
            }
        }
    }
}
