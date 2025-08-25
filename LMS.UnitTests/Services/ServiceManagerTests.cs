using LMS.UnitTests.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.UnitTests.Services
{
    public class ServiceManagerTests : ServiceTestBase
    {

        [Fact]
        [Trait("ServiceManager", "AuthService")]
        public void AuthService_ReturnsInjectedService()
        {            
            var result = ServiceManager.AuthService;

            Assert.Same(MockAuthService.Object, result);
        }

        [Fact]
        [Trait("ServiceManager", "CourseService")]
        public void CourseService_ReturnsInjectedService()
        {            
            var result = ServiceManager.CourseService;

            Assert.Same(MockCourseService.Object, result);
        }

        [Fact]
        [Trait("ServiceManager", "DocumentService")]
        public void DocumentService_ReturnsInjectedService()
        {            
            var result = ServiceManager.DocumentService;

            Assert.Same(MockDocumentService.Object, result);
        }

        [Fact]
        [Trait("ServiceManager", "ModuleActivityService")]
        public void ModuleActivityService_ReturnsInjectedService()
        {            
            var result = ServiceManager.ModuleActivityService;

            Assert.Same(MockModuleActivityService.Object, result);
        }

        [Fact]
        [Trait("ServiceManager", "ModuleService")]
        public void ModuleService_ReturnsInjectedService()
        {            
            var result = ServiceManager.ModuleService;

            Assert.Same(MockModuleService.Object, result);
        }

        [Fact]
        [Trait("ServiceManager", "SubmissionService")]
        public void SubmissionService_ReturnsInjectedService()
        {            
            var result = ServiceManager.SubmissionService;

            Assert.Same(MockSubmissionService.Object, result);
        }

        [Fact]
        [Trait("ServiceManager", "UserService")]
        public void UserService_ReturnsInjectedService()
        {            
            var result = ServiceManager.UserService;

            Assert.Same(MockUserService.Object, result);
        }
    }

}

