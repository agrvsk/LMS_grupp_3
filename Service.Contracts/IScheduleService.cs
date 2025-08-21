using LMS.Shared.DTOs.EntityDTO;

namespace Service.Contracts;

    public interface IScheduleService
    {
        ScheduleDto GetSchedule(Guid coursId, DateTime start, DateTime end);
    }
