namespace Service.Contracts;
public interface IServiceManager
{
    IAuthService AuthService { get; }

    ICourseService CourseService { get; }
    IDocumentService DocumentService { get; }
    IModuleActivityService ModuleActivityService { get; }
    IModuleService ModuleService { get; }
    ISubmissionService SubmissionService { get; }
    IUserService UserService { get; }

}