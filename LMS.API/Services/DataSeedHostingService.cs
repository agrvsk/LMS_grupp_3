using Bogus;
using Domain.Models.Entities;
using LMS.Infractructure.Data;
using LMS.Infractructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LMS.API.Services;

//Add in secret.json
//{
//   "password" :  "YourSecretPasswordHere"
//}
public class DataSeedHostingService : IHostedService
{
    private readonly IServiceProvider serviceProvider;
    private readonly IConfiguration configuration;
    private readonly ILogger<DataSeedHostingService> logger;
    private UserManager<ApplicationUser> userManager = null!;
    private RoleManager<IdentityRole> roleManager = null!;
    private const string TeacherRole = "Teacher";
    private const string StudentRole = "Student";

    public DataSeedHostingService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DataSeedHostingService> logger)
    {
        this.serviceProvider = serviceProvider;
        this.configuration = configuration;
        this.logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
        if (!env.IsDevelopment()) return;

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (await context.Users.AnyAsync(cancellationToken)) return;

        userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        ArgumentNullException.ThrowIfNull(roleManager, nameof(roleManager));
        ArgumentNullException.ThrowIfNull(userManager, nameof(userManager));

        try
        {
            await AddRolesAsync([TeacherRole, StudentRole]);
            await AddDemoUsersAsync();
            await AddUsersAsync(20);
            await AddCoursesAsync(5, context);
            await AddDocumentsAsync(context);


            logger.LogInformation("Seed complete");
        }
        catch (Exception ex)
        {
            logger.LogError($"Data seed fail with error: {ex.Message}");
            throw;
        }
    }

    private async Task AddRolesAsync(string[] rolenames)
    {
        foreach (string rolename in rolenames)
        {
            if (await roleManager.RoleExistsAsync(rolename)) continue;
            var role = new IdentityRole { Name = rolename };
            var res = await roleManager.CreateAsync(role);

            if (!res.Succeeded) throw new Exception(string.Join("\n", res.Errors));
        }
    }

    private async Task AddDemoUsersAsync()
    {
        var teacher = new ApplicationUser
        {
            UserName = "teacher@test.com",
            Email = "teacher@test.com"
        };

        var student = new ApplicationUser
        {
            UserName = "student@test.com",
            Email = "student@test.com"
        };

        await AddUserToDb([teacher, student]);

        var teacherRoleResult = await userManager.AddToRoleAsync(teacher, TeacherRole);
        if (!teacherRoleResult.Succeeded) throw new Exception(string.Join("\n", teacherRoleResult.Errors));

        var studentRoleResult = await userManager.AddToRoleAsync(student, StudentRole);
        if (!studentRoleResult.Succeeded) throw new Exception(string.Join("\n", studentRoleResult.Errors));
    }

    private async Task AddUsersAsync(int nrOfUsers)
    {
        var faker = new Faker<ApplicationUser>("sv").Rules((f, e) =>
        {
            e.Email = f.Person.Email;
            e.UserName = f.Person.Email;
        });

        var users = faker.Generate(nrOfUsers);
        await AddUserToDb(users);

        foreach (var user in users)
        {
            var roleResult = await userManager.AddToRoleAsync(user, StudentRole);
            if (!roleResult.Succeeded)
                throw new Exception(string.Join("\n", roleResult.Errors.Select(e => e.Description)));
        }

    }

    private async Task AddCoursesAsync(int courseAmount, ApplicationDbContext context)
    {
        // ActivityType faker
        var activityTypeFaker = new Faker<ActivityType>("sv")
            .RuleFor(a => a.Name, f => f.PickRandom(new[] { "Lecture", "Seminar", "Assignment", "Exam", "Project" }));

        var activityTypes = activityTypeFaker.Generate(5);

        // ModuleActivity faker
        var moduleActivityFaker = new Faker<ModuleActivity>("sv")
            .RuleFor(ma => ma.Name, f => f.Commerce.ProductName())
            .RuleFor(ma => ma.Description, f => f.Lorem.Sentence(6))
            .RuleFor(ma => ma.StartDate, f => f.Date.FutureOffset(1).DateTime)
            .RuleFor(ma => ma.EndDate, (f, ma) => ma.StartDate.AddDays(f.Random.Int(1, 30)))
            .RuleFor(ma => ma.Type, f => f.PickRandom(activityTypes));

        // Module faker
        var moduleFaker = new Faker<Module>("sv")
            .RuleFor(m => m.Name, f => f.Commerce.Department())
            .RuleFor(m => m.Description, f => f.Lorem.Sentence(8))
            .RuleFor(m => m.StartDate, f => f.Date.FutureOffset(1).DateTime)
            .RuleFor(m => m.EndDate, (f, m) => m.StartDate.AddMonths(f.Random.Int(1, 6)))
            .RuleFor(m => m.ModuleActivities, (f, m) =>
            {
                var activities = moduleActivityFaker.Generate(f.Random.Int(2, 5));

                return activities;
            });

        // Course faker
        var courseFaker = new Faker<Course>("sv")
            .RuleFor(c => c.Name, f => f.Company.CompanyName())
            .RuleFor(c => c.Description, f => f.Lorem.Sentence(10))
            .RuleFor(c => c.StartDate, f => f.Date.FutureOffset(1).DateTime)
            .RuleFor(c => c.EndDate, (f, c) => c.StartDate.AddMonths(f.Random.Int(3, 12)))
            .RuleFor(c => c.Modules, (f, c) =>
            {
                var modules = moduleFaker.Generate(f.Random.Int(2, 4));

                return modules;
            })
            .RuleFor(c => c.Students, (f, c) =>
            {
                var students = context.Users
                    .OrderBy(_ => Guid.NewGuid()) // Randomize order
                    .Take(f.Random.Int(2, 4)) // Random number of students
                    .ToList();
                return students;
            });


        var courses = courseFaker.Generate(courseAmount);


        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();
    }

    private async Task AddDocumentsAsync(ApplicationDbContext context)
    {
        Random rnd = new();
        // Document faker
        var documentFaker = new Faker<Document>("sv")
            .RuleFor(d => d.Name, f => f.System.FileName())
            .RuleFor(d => d.Description, f => f.Lorem.Sentence(6))
            .RuleFor(d => d.UploadDate, f => f.Date.Recent(90)) // last 3 months
            .RuleFor(d => d.FilePath, f => $"/uploads/{f.System.FileName()}")
            .RuleFor(d => d.ParentType, f => "submission")
            .RuleFor(d => d.Uploader, f => new ApplicationUser
            {
                UserName = f.Internet.UserName(),
                Email = f.Internet.Email(),
            });

        // Submission faker
        var submissionFaker = new Faker<Submission>("sv")
            .RuleFor(s => s.SubmissionDate, f => f.Date.Recent(30)) // last 30 days
            .RuleFor(s => s.SubmissionDocument, (f,s) =>
            {
                var doc = documentFaker.Generate();
                doc.UploadDate = s.SubmissionDate;
                return doc;
            });

        // Attach documents to courses
        foreach (var course in context.Courses)
        {
            var docs = documentFaker.Generate(rnd.Next(2, 4));
            foreach (var doc in docs)
            {
                doc.ParentType = "course";
                doc.ParentId = course.Id;
            }
            context.Documents.AddRange(docs);
        }

        // Attach documents to modules
        foreach (var module in context.Modules)
        {
            var docs = documentFaker.Generate(rnd.Next(2, 4));
            foreach (var doc in docs)
            {
                doc.ParentType = "module";
                doc.ParentId = module.Id;
            }
            context.Documents.AddRange(docs);
        }

        // Attach documents to activities
        foreach (var activity in context.Activities)
        {
            var docs = documentFaker.Generate(rnd.Next(2, 4));
            foreach (var doc in docs)
            {
                doc.ParentType = "activity";
                doc.ParentId = activity.Id;
            }
            context.Documents.AddRange(docs);
        }

        // Create submissions
        var allSubmissions = new List<Submission>();
        ApplicationUserRepository repository = new(context, userManager);
        var students = repository.GetUsersByRoleAsync(StudentRole).Result;
        foreach (var student in students)
        {
            var doc = documentFaker.Generate();
            doc.Id = Guid.NewGuid();
            var submission = new Submission
            {
                SubmissionDocument = doc,
                SubmissionDate = doc.UploadDate,
                DocumentId = doc.Id,
                ApplicationUserId = student.Id,
                Id = Guid.NewGuid()
            };
            doc.ParentId = submission.Id;
            submission.SubmissionDocument = doc;

            allSubmissions.Add(submission);
        }

        context.Submissions.AddRange(allSubmissions);
        await context.SaveChangesAsync();
    }

    private async Task AddUserToDb(IEnumerable<ApplicationUser> users)
    {
        var passWord = configuration["password"];
        ArgumentNullException.ThrowIfNull(passWord, nameof(passWord));

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, passWord);
            if (!result.Succeeded) throw new Exception(string.Join("\n", result.Errors));
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}
