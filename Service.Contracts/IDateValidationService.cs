namespace Service.Contracts
{
    public interface IDateValidationService
    {
        bool ValidateCourseDates(DateTime startDate, DateTime endDate);
        Task<bool> ValidateModuleActivityDatesAsync(DateTime startDate, DateTime endDate, Guid moduleId, Guid? activityId = null);
        Task<bool> ValidateModuleDatesAsync(DateTime startDate, DateTime endDate, Guid courseId, Guid? moduleId = null);
    }
}