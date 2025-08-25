using Service.Contracts;

namespace LMS.Services;

public class ServiceManager : IServiceManager
{
    private Lazy<IAuthService> authService;
    private Lazy<ICourseService> courseService;
    private Lazy<IDocumentService> documentService;
    private Lazy<IModuleActivityService> moduleActivityService;
    private Lazy<IModuleService> moduleService;
    private Lazy<ISubmissionService> submissionService;
    private Lazy<IUserService> userService;
    private Lazy<IActivityTypeService> activityTypeService;

    public IAuthService AuthService => authService.Value;

    public ICourseService CourseService => courseService.Value;

    public IDocumentService DocumentService => documentService.Value;

    public IModuleActivityService ModuleActivityService => moduleActivityService.Value;

    public IModuleService ModuleService => moduleService.Value;

    public ISubmissionService SubmissionService => submissionService.Value;

    public IUserService UserService => userService.Value;

    public IActivityTypeService ActivityTypeService => activityTypeService.Value;

    public ServiceManager(Lazy<IAuthService> authService
        , Lazy<ICourseService> courseService
        , Lazy<IDocumentService> documentService
        , Lazy<IModuleActivityService> moduleActivityService
        , Lazy<IModuleService> moduleService
        , Lazy<ISubmissionService> submissionService
        , Lazy<IUserService> userService
        , Lazy<IActivityTypeService> activityTypeService

        )
    {
        this.authService = authService;
        this.courseService = courseService;
        this.documentService = documentService;
        this.moduleActivityService = moduleActivityService;
        this.moduleService = moduleService;
        this.submissionService = submissionService;
        this.userService = userService;
        this.activityTypeService = activityTypeService;

    }
}
