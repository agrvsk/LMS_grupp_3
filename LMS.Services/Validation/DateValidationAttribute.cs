using Domain.Models.Entities;
using LMS.Shared.DTOs.EntityDto;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services.Validation;

public class DateValidationAttribute : ValidationAttribute, IDateValidationAttribute
{
    //private readonly ServiceManager serviceManager;

    //public DateValidationAttribute(ServiceManager serviceManager)
    //{
    //    this.serviceManager = serviceManager;
    //}
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        ServiceManager serviceManager = (ServiceManager)validationContext.GetService(typeof(ServiceManager));
        AutoMapper.IMapper mapper = (AutoMapper.IMapper)validationContext.GetService(typeof(AutoMapper.IMapper));
        DateTime min;
        DateTime max;
        bool success = true;
        string message = "";
        object objectInstance = validationContext.ObjectInstance;
        if (objectInstance is ModuleCreateDto)
        {
            objectInstance = mapper.Map<ModuleDto>(objectInstance);
        }
        if (objectInstance is ModuleActivityCreateDto)
        {
            objectInstance = mapper.Map<ModuleActivityDto>(objectInstance);
        }
        if (objectInstance is CourseCreateDto)
        {
            objectInstance = mapper.Map<CourseDto>(objectInstance);
        }
        if (objectInstance is ModuleUpdateDto)
        {
            objectInstance = mapper.Map<ModuleDto>(objectInstance);
        }
        if (objectInstance is ModuleActivityUpdateDto)
        {
            objectInstance = mapper.Map<ModuleActivityDto>(objectInstance);
        }
        if (objectInstance is CourseUpdateDto)
        {
            objectInstance = mapper.Map<CourseDto>(objectInstance);
        }


        if (!(value is DateTime dateTime))
        {
            return new ValidationResult("Invalid date format.");

        }

        if (objectInstance is CourseDto)
        {
            var course = (CourseDto)objectInstance;
            if (!validateDates(course.StartDate, course.EndDate)) return new ValidationResult($"{message}");
            return ValidationResult.Success;
        }

        if (objectInstance is ModuleDto)
        {
            var module = (ModuleDto)objectInstance;
            min = module.StartDate;
            max = module.EndDate;

            if (!validateDates(min, max)) return new ValidationResult($"{message}");
            List<ModuleDto> modules = serviceManager.ModuleService.GetModulesByCourseIdAsync(module.CourseId).Result;
            foreach (var mod in modules)
            {
                if (mod.Id != module.Id) // Exclude the current module from the check
                {
                    success = OverlappingPeriods(min, max, mod.StartDate, mod.EndDate);
                    if (!success)
                    {
                        return new ValidationResult($"Module dates overlap with another module: {mod.Name}");
                    }
                }
            }
            return ValidationResult.Success;
            //success = OverlappingPeriods(min, max, courseSession.Course.StartDate, courseSession.Course.EndDate);
        }
        else if (objectInstance is ModuleActivityDto)
        {

            var moduleActivity = (ModuleActivityDto)objectInstance;
            min = moduleActivity.StartDate;
            max = moduleActivity.EndDate;
            if (!validateDates(min, max)) return new ValidationResult($"{message}");
            List<ModuleActivityDto> activities = serviceManager.ModuleActivityService.GetModuleActivitiesByModuleIdAsync(moduleActivity.ModuleId).Result;
            foreach (var activity in activities)
            {
                if (activity.Id != moduleActivity.Id) // Exclude the current activity from the check
                {
                    success = OverlappingPeriods(min, max, activity.StartDate, activity.EndDate);
                    if (!success)
                    {
                        return new ValidationResult($"Activity dates overlap with another activity: {activity.Name}");
                    }
                }
            }
            return ValidationResult.Success;
        }
        else
        {
            return new ValidationResult("Invalid validation context.");
        }

        return new ValidationResult("error");
        bool validateDates(DateTime startDate, DateTime endDate)
        {
            if (startDate == default || endDate == default)
            {
                message = "Start date and end date must be provided.";
                return false;
            }
            if (startDate > endDate)
            {
                message = "Start date cannot be after end date.";
                return false;
            }
            return true;
        }
    }
    public static bool OverlappingPeriods(DateTime aStart, DateTime aEnd,
                                      DateTime bStart, DateTime bEnd)
    {


        return !((aEnd < bStart && aStart < bStart) ||
                    (bEnd < aStart && bStart < aStart));
    }
}
