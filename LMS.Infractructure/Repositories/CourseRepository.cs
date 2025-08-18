using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts.Repositories;
using Domain.Models.Entities;
using LMS.Infractructure.Data;

namespace LMS.Infractructure.Repositories;

public class CourseRepository : RepositoryBase<Course>, ICourseRepository
{
    public CourseRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Course?> GetCourseById(string courseId)
    {
        return (await FindByConditionAsync(c => c.Id == courseId, trackChanges: false)).SingleOrDefault();
    }
    public async Task<List<Course>> GetAllCourses()
    {
        return (await FindAllAsync(trackChanges: false)).ToList();
    }
}
