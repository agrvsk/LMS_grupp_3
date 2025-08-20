using Bogus;
using LMS.Infractructure.Data;
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
            await AddCoursesAsync(5, context);
            await AddUsersAsync(20);
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

        await AddUserToDb(faker.Generate(nrOfUsers));
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
                // EF/SQL will fill ModuleId when saved
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
                // EF/SQL will fill CourseId when saved
                return modules;
            });

        courseAmount = 5; // how many courses you want
        var courses = courseFaker.Generate(courseAmount);

        // Add to context
        context.Courses.AddRange(courses);
        context.SaveChanges();
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
