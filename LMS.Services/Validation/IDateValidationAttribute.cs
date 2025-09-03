
namespace LMS.Services.Validation
{
    public interface IDateValidationAttribute
    {
        static abstract bool OverlappingPeriods(DateTime aStart, DateTime aEnd, DateTime bStart, DateTime bEnd);
    }
}