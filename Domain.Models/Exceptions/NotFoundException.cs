namespace Domain.Models.Exceptions;
public abstract class NotFoundException(string Message, string Title = "Not Found") : Exception(Message)
{
    public string Title { get; }
    //public NotFoundException(string message, string title = "Not Found") : base(message)
    //{
    //    Title = title;
    //}
}
//Nedanstående måste även in i LMS.API/Extensions/ExceptionMiddlewareExtensions.cs
public sealed class NoUsersFoundException() : NotFoundException($"No users found") { }
public sealed class UserNotFoundException(string Id) : NotFoundException($"The user with id {Id} is not found") { }
public sealed class NoCoursesFoundException() : NotFoundException($"No courses found") { }
public sealed class CourseNotFoundException(Guid Id) : NotFoundException($"The course with id {Id} is not found") { }
public sealed class NoModulesFoundException() : NotFoundException($"No modules found") { }
public sealed class ModuleNotFoundException(Guid Id) : NotFoundException($"The module with id {Id} is not found") { }
public sealed class NoActivitiesFoundException() : NotFoundException($"No activities found") { }
public sealed class ActivityNotFoundException(Guid Id) : NotFoundException($"The activity with id {Id} is not found") { }
public sealed class NoDocumentsFoundException() : NotFoundException($"No documents found") { }
public sealed class DocumentNotFoundException(Guid Id) : NotFoundException($"The document with id {Id} is not found") { }
public sealed class NoSubmissionsFoundException() : NotFoundException($"No submissions found") { }
public sealed class SubmissionNotFoundException(Guid Id) : NotFoundException($"The submission with id {Id} is not found") { }





