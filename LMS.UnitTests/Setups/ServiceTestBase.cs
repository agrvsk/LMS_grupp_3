using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace LMS.UnitTests.Setups
{
    public abstract class ServiceTestBase
    {
        protected readonly Mock<IMapper> MockMapper;
        protected readonly Mock<IUnitOfWork> MockUow;

        protected readonly Mock<ICourseRepository> MockCourseRepo;
        protected readonly Mock<IModuleRepository> MockModuleRepo;
        protected readonly Mock<IModuleActivityRepository> MockModuleActivityRepo;
        protected readonly Mock<IDocumentRepository> MockDocumentRepo;

        protected readonly Mock<UserManager<ApplicationUser>> MockUserManager;



        protected ServiceTestBase()
        {
            MockMapper = new Mock<IMapper>();
            MockUow = new Mock<IUnitOfWork>();

            // Initialize all repository mocks
            MockModuleActivityRepo = new Mock<IModuleActivityRepository>();
            MockModuleRepo = new Mock<IModuleRepository>();
            MockCourseRepo = new Mock<ICourseRepository>();
            MockDocumentRepo = new Mock<IDocumentRepository>();

            // Setup the UnitOfWork to return the repository mocks
            MockUow.SetupGet(u => u.ModuleActivityRepository).Returns(MockModuleActivityRepo.Object);
            MockUow.SetupGet(u => u.ModuleRepository).Returns(MockModuleRepo.Object);
            MockUow.SetupGet(u => u.CourseRepository).Returns(MockCourseRepo.Object);
            MockUow.SetupGet(u => u.DocumentRepository).Returns(MockDocumentRepo.Object);

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
        }
    }
}
