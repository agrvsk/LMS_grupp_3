namespace Domain.Contracts.Repositories;

public interface IUnitOfWork
{
    public ICourseRepository CourseRepository { get;  }
    public IModuleRepository ModuleRepository { get;  }
    public IModuleActivityRepository ModuleActivityRepository { get;  }
    public IApplicationUserRepository ApplicationUserRepository { get;  }
    public ISubmissionRepository SubmissionRepository { get;  }
    public IDocumentRepository DocumentRepository { get; }

    Task CompleteAsync();
}