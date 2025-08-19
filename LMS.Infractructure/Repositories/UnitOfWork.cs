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

    public ICourseRepository CourseRepository => courseRepository.Value;
    public IModuleRepository ModuleRepository => moduleRepository.Value;
    public IModuleActivityRepository ModuleActivityRepository => moduleActivityRepository.Value;
    public IApplicationUserRepository ApplicationUserRepository => applicationUserRepository.Value;
    public ISubmissionRepository SubmissionRepository => submissionRepository.Value;
    public IDocumentRepository DocumentRepository => documentRepository.Value;


    public UnitOfWork(ApplicationDbContext context
            , Lazy<ICourseRepository> aCourseRepository
            , Lazy<IModuleRepository> aModuleRepository
            , Lazy<IModuleActivityRepository> aModuleActivityRepository
            , Lazy<IApplicationUserRepository> aApplicationUserRepository
            , Lazy<ISubmissionRepository> aSsubmissionRepository
            , Lazy<IDocumentRepository> aDocumentRepository
        )
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        courseRepository = aCourseRepository;
        moduleRepository = aModuleRepository;
        moduleActivityRepository = aModuleActivityRepository;
        applicationUserRepository = aApplicationUserRepository;
        submissionRepository = aSsubmissionRepository;
        documentRepository = aDocumentRepository;
    }

    public async Task CompleteAsync() => await context.SaveChangesAsync();
}
