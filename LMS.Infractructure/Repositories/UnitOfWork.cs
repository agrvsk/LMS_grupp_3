using Domain.Contracts.Repositories;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext context;

    private readonly Lazy<ICourseRepository> courseRepository;
    private readonly Lazy<IModuleRepository> moduleRepository;
    private readonly Lazy<IModuleActivityRepository> moduleActivityRepository;
    private readonly Lazy<IApplicationUserRepository> applicationUserRepository;
    private readonly Lazy<ISubmissionRepository> submissionRepository;
    private readonly Lazy<IDocumentRepository> documentRepository;
    private readonly Lazy<IActivityTypeRepository> activityTypeRepository;

    public ICourseRepository CourseRepository { get => courseRepository.Value;  }
    public IModuleRepository ModuleRepository { get => moduleRepository.Value;  }
    public IModuleActivityRepository ModuleActivityRepository
    {
        get => moduleActivityRepository.Value;
    }
    public IApplicationUserRepository ApplicationUserRepository
    {
        get => applicationUserRepository.Value;
    }
    public ISubmissionRepository SubmissionRepository
    {
        get => submissionRepository.Value;
    }
    public IDocumentRepository DocumentRepository
    {
        get => documentRepository.Value;
    }
    public IActivityTypeRepository ActivityTypeRepository
    {
        get => activityTypeRepository.Value;
    }


    public UnitOfWork(ApplicationDbContext context

            , Lazy<ICourseRepository> aCourseRepository
            , Lazy<IModuleRepository> aModuleRepository
            , Lazy<IModuleActivityRepository> aModuleActivityRepository
            , Lazy<IApplicationUserRepository> aApplicationUserRepository
            , Lazy<ISubmissionRepository> aSsubmissionRepository
            , Lazy<IDocumentRepository> aDocumentRepository
            , Lazy<IActivityTypeRepository> aActivityTypeRepository

        )
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
/*
        courseRepository = new Lazy<ICourseRepository>(() => new CourseRepository(context));
        moduleRepository = new Lazy<IModuleRepository>(() => new ModuleRepository(context));
        moduleActivityRepository = new Lazy<IModuleActivityRepository>(() => new ModuleActivityRepository(context));
        applicationUserRepository = new Lazy<IApplicationUserRepository>(() => new ApplicationUserRepository(context));
        submissionRepository = new Lazy<ISubmissionRepository>(() => new SubmissionRepository(context));
        documentRepository = new Lazy<IDocumentRepository>(() => new DocumentRepository(context));
*/


        courseRepository = aCourseRepository;
        moduleRepository = aModuleRepository;
        moduleActivityRepository = aModuleActivityRepository;
        applicationUserRepository = aApplicationUserRepository;
        submissionRepository = aSsubmissionRepository;
        documentRepository = aDocumentRepository;
        activityTypeRepository = aActivityTypeRepository;

    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
