using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts.Repositories;
using Moq;

namespace LMS.UnitTests.Setups
{
    public abstract class ServiceTestBase
    {
        protected readonly Mock<IUnitOfWork> MockUow;
        protected readonly Mock<ICourseRepository> MockCourseRepo;
        protected readonly Mock<IMapper> MockMapper;

        protected ServiceTestBase() 
        {
            MockUow = new Mock<IUnitOfWork>();
            MockCourseRepo = new Mock<ICourseRepository>();
            MockMapper = new Mock<IMapper>();

            MockUow.SetupGet(u => u.CourseRepository).Returns(MockCourseRepo.Object);
        }
    }
}
