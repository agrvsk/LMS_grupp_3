using AutoMapper;
using LMS.Shared.DTOs.EntityDto;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Services;

public class DateValidationService : IDateValidationService
{
    private readonly IMapper mapper;

    private readonly IModuleService moduleService;
    private readonly IModuleActivityService moduleActivityService;

    public DateValidationService(IMapper mapper, IModuleService moduleService, IModuleActivityService moduleActivityService)
    {
        this.mapper = mapper;

        this.moduleService = moduleService;
        this.moduleActivityService = moduleActivityService;
    }
    public bool ValidateCourseDates(DateTime startDate, DateTime endDate)
    {
        return validateDates(startDate, endDate);
    }
    public async Task<bool> ValidateModuleDatesAsync(DateTime startDate, DateTime endDate, Guid courseId, Guid? moduleId = null)
    {
        //string message = "";
        if (!validateDates(startDate, endDate))
        {
            //message = "Invalid date range.";
            return (false/*, message*/);
        }
        var modules = await moduleService.GetModulesByCourseIdAsync(courseId);
        if (moduleId.HasValue)
        {
            modules = modules.Where(m => m.Id != moduleId.Value).ToList();
        }
        foreach (var module in modules)
        {
            if (OverlappingPeriods(startDate, endDate, module.StartDate, module.EndDate))
            {
                //message = $"Module dates overlap with existing module '{module.Title}' ({module.StartDate.ToShortDateString()} - {module.EndDate.ToShortDateString()}).";
                return (false /*,message*/);
            }
        }
        return (true/*, message*/);
    }
    public async Task<bool> ValidateModuleActivityDatesAsync(DateTime startDate, DateTime endDate, Guid moduleId, Guid? activityId = null)
    {
        //string message = "";
        if (!validateDates(startDate, endDate))
        {
            //message = "Invalid date range.";
            return (false/*, message*/);
        }
        var activities = await moduleActivityService.GetModuleActivitiesByModuleIdAsync(moduleId);
        if (activityId.HasValue)
        {
            activities = activities.Where(a => a.Id != activityId.Value).ToList();
        }
        foreach (var activity in activities)
        {
            if (OverlappingPeriods(startDate, endDate, activity.StartDate, activity.EndDate))
            {
                //message = $"Activity dates overlap with existing activity '{activity.Title}' ({activity.StartDate.ToShortDateString()} - {activity.EndDate.ToShortDateString()}).";
                return (false /*,message*/);
            }
        }
        return (true/*, message*/);
    }
    bool validateDates(DateTime startDate, DateTime endDate)
    {
        if (startDate == default || endDate == default)
        {
            //message = "Start date and end date must be provided.";
            return false;
        }
        if (startDate > endDate)
        {
            //message = "Start date cannot be after end date.";
            return false;
        }
        return true;
    }
    private bool OverlappingPeriods(DateTime aStart, DateTime aEnd,
                                      DateTime bStart, DateTime bEnd)
    {


        return ((aEnd < bStart && aStart < bStart) ||
                    (bEnd < aStart && bStart < aStart));
    }
}
