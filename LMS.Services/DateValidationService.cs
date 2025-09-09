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
    private readonly ICourseService courseService;

    public DateValidationService(IMapper mapper, IModuleService moduleService, IModuleActivityService moduleActivityService, ICourseService courseService)
    {
        this.mapper = mapper;

        this.moduleService = moduleService;
        this.moduleActivityService = moduleActivityService;
        this.courseService = courseService;
    }
    public bool ValidateCourseDates(DateTime startDate, DateTime endDate)
    {
        return validateDates(startDate, endDate);
    }
    public async Task<bool> ValidateModuleUppdateDatesAsync(DateTime startDate, DateTime endDate, Guid courseId, Guid? moduleId = null)
    {
        //string message = "";
        if (!validateDates(startDate, endDate))
        {
            //message = "Invalid date range.";
            return (false/*, message*/);
        }
        var modules = await moduleService.GetModulesByCourseIdAsync(courseId);
        if (moduleId!=null)
        {
            modules = modules.Where(m => m.Id != moduleId).ToList();
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
    public async Task<bool> ValidateModuleActivityUppdateDatesAsync(DateTime startDate, DateTime endDate, Guid moduleId, Guid? activityId = null)
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
    public bool ValidateListOfDateRanges(List<(DateTime startDate, DateTime endDate)> dateRanges, DateTime minDate, DateTime maxDate)
    {
        //string message = "";
        for (int i = 0; i < dateRanges.Count; i++)
        {
            var (startDate, endDate) = dateRanges[i];
            if (!validateDates(startDate, endDate))
            {
                //message = $"Invalid date range at index {i}.";
                return (false/*, message*/);
            }
            if (startDate < minDate || endDate > maxDate)
            {
                //message = $"Date range at index {i} is out of bounds ({minDate.ToShortDateString()} - {maxDate.ToShortDateString()}).";
                return (false /*,message*/);
            }
            if(i+1< dateRanges.Count) for (int j = i + 1; j < dateRanges.Count; j++)
            {
                var (otherStart, otherEnd) = dateRanges[j];
                if (OverlappingPeriods(startDate, endDate, otherStart, otherEnd))
                {
                    //message = $"Date range at index {i} overlaps with date range at index {j}.";
                    return (false /*,message*/);
                }
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


         if((aEnd <= bStart && aStart <= bStart) ||
                    (bEnd <= aStart && bStart <= aStart)) return false; return true;
    }
}
