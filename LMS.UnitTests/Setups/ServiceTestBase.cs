using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Configurations;
using Domain.Models.Entities;
using LMS.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Setups
{
    public abstract class ServiceTestBase
    {
        // Common Mocks
        protected readonly Mock<IMapper> MockMapper;
        protected readonly Mock<IUnitOfWork> MockUow;

        // Repository Mocks
        protected readonly Mock<IApplicationUserRepository> MockApplicationUserRepo;
        protected readonly Mock<ICourseRepository> MockCourseRepo;
        protected readonly Mock<IDocumentRepository> MockDocumentRepo;
        protected readonly Mock<IModuleRepository> MockModuleRepo;
        protected readonly Mock<IModuleActivityRepository> MockModuleActivityRepo;
        protected readonly Mock<ISubmissionRepository> MockSubmissionRepo;

        // UserManager Mock
        protected readonly Mock<UserManager<ApplicationUser>> MockUserManager;
        protected readonly Mock<RoleManager<IdentityRole>> MockRoleManager;

        protected readonly IOptions<JwtSettings> JwtOptions;

        // Service Mocks
        protected readonly Mock<IAuthService> MockAuthService;
        protected readonly Mock<ICourseService> MockCourseService;
        protected readonly Mock<IDocumentService> MockDocumentService;
        protected readonly Mock<IModuleActivityService> MockModuleActivityService;
        protected readonly Mock<IModuleService> MockModuleService;
        protected readonly Mock<ISubmissionService> MockSubmissionService;
        protected readonly Mock<IUserService> MockUserService;


        protected ServiceManager ServiceManager;

        protected ServiceTestBase()
        {
            // Initialize common mocks
            MockMapper = new Mock<IMapper>();
            MockUow = new Mock<IUnitOfWork>();

            // Initialize repository mocks
            MockApplicationUserRepo = new Mock<IApplicationUserRepository>();
            MockCourseRepo = new Mock<ICourseRepository>();
            MockDocumentRepo = new Mock<IDocumentRepository>();
            MockModuleActivityRepo = new Mock<IModuleActivityRepository>();
            MockModuleRepo = new Mock<IModuleRepository>();
            MockSubmissionRepo = new Mock<ISubmissionRepository>();

            // Setup the UnitOfWork to return the repository mocks
            MockUow.SetupGet(u => u.ApplicationUserRepository).Returns(MockApplicationUserRepo.Object);
            MockUow.SetupGet(u => u.CourseRepository).Returns(MockCourseRepo.Object);
            MockUow.SetupGet(u => u.DocumentRepository).Returns(MockDocumentRepo.Object);
            MockUow.SetupGet(u => u.ModuleActivityRepository).Returns(MockModuleActivityRepo.Object);
            MockUow.SetupGet(u => u.ModuleRepository).Returns(MockModuleRepo.Object);
            MockUow.SetupGet(u => u.SubmissionRepository).Returns(MockSubmissionRepo.Object);

            // Setup UnitOfWork CompleteAsync to return a completed task
            MockUow.Setup(u => u.CompleteAsync()).Returns(Task.CompletedTask);

            // Identity Mocks
            var store = new Mock<IUserStore<ApplicationUser>>();
            MockUserManager = new Mock<UserManager<ApplicationUser>>
                (
                store.Object,   // IUserStore<ApplicationUser>
                null!,          // IOptions<IdentityOptions>
                null!,          // IPasswordHasher<ApplicationUser>
                null!,          // IEnumerable<IUserValidator<ApplicationUser>>
                null!,          // IEnumerable<IPasswordValidator<ApplicationUser>>
                null!,          // ILookupNormalizer
                null!,          // IdentityErrorDescriber
                null!,          // IServiceProvider
                null!           // ILogger<UserManager<ApplicationUser>>
                );

            var roleStore = new Mock<IRoleStore<IdentityRole>>();
            MockRoleManager = new Mock<RoleManager<IdentityRole>>
                (
                roleStore.Object, // IRoleStore<IdentityRole>
                null!,            // IEnumerable<IRoleValidator<IdentityRole>>
                null!,            // ILookupNormalizer
                null!,            // IdentityErrorDescriber
                null!             // ILogger<RoleManager<IdentityRole>>
                );

            // JWT Options
            JwtOptions = Options.Create(new JwtSettings
            {
                SecretKey = "SecretKeyForTestingPurposesOnly!",
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                Expires = 60
            });


            // Initialize all service mocks
            MockAuthService = new Mock<IAuthService>();
            MockCourseService = new Mock<ICourseService>();
            MockDocumentService = new Mock<IDocumentService>();
            MockModuleActivityService = new Mock<IModuleActivityService>();
            MockModuleService = new Mock<IModuleService>();
            MockSubmissionService = new Mock<ISubmissionService>();
            MockUserService = new Mock<IUserService>();

            // Initialize the ServiceManager with the service mocks
            ServiceManager = new ServiceManager(
                new Lazy<IAuthService>(() => MockAuthService.Object),
                new Lazy<ICourseService>(() => MockCourseService.Object),
                new Lazy<IDocumentService>(() => MockDocumentService.Object),
                new Lazy<IModuleActivityService>(() => MockModuleActivityService.Object),
                new Lazy<IModuleService>(() => MockModuleService.Object),
                new Lazy<ISubmissionService>(() => MockSubmissionService.Object),
                new Lazy<IUserService>(() => MockUserService.Object)
                );

        }
        protected ApplicationUser CreateTestUser(string username = "testuser") =>
            new ApplicationUser { UserName = username, Id = Guid.NewGuid().ToString() };

    }
}
