namespace Service.Contracts
{
    public interface IDateValidationService
    {
        bool ValidateCourseDates(DateTime startDate, DateTime endDate);
        Task<bool> ValidateModuleActivityUppdateDatesAsync(DateTime startDate, DateTime endDate, Guid moduleId, Guid? activityId = null);
        Task<bool> ValidateModuleUppdateDatesAsync(DateTime startDate, DateTime endDate, Guid courseId, Guid? moduleId = null);
        public bool ValidateListOfDateRanges(List<(DateTime startDate, DateTime endDate)> dateRanges, DateTime minDate, DateTime maxDate);
    }
}