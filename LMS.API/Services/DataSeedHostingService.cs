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
    private Guid CourseId1 = Guid.Parse("11111111-1111-1111-1111-111111111111");
    private Guid CourseId2 = Guid.Parse("22222222-2222-2222-2222-222222222222");
    private Guid CourseId3 = Guid.Parse("33333333-3333-3333-3333-333333333333");

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
            await AddCoursesAsync(5, context);
            await AddDemoUsersAsync();
            await AddUsersAsync(30);

            //await AddDocumentsAsync(context);


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
            var random = new Random();
            var courseIds = new List<Guid> { CourseId1, CourseId2, CourseId3 };
            var roleResult = await userManager.AddToRoleAsync(user, StudentRole);
            if (!roleResult.Succeeded)
                throw new Exception(string.Join("\n", roleResult.Errors.Select(e => e.Description)));
            user.CourseId = courseIds[random.Next(courseIds.Count)];
            await userManager.UpdateAsync(user);
        }

    }

    private async Task AddCoursesAsync(int courseAmount, ApplicationDbContext context)
    {

        if (!context.ActivityTypes.Any())
        {
            var activityTypes = new List<ActivityType>
    {
        new ActivityType { Name = "Föreläsning" },
        new ActivityType { Name = "Seminarie" },
        new ActivityType { Name = "Prov" },
        new ActivityType { Name = "Projekt" },
        new ActivityType { Name = "Redovisning" }
    };

            context.ActivityTypes.AddRange(activityTypes);
            context.SaveChanges();
        }

        // Fetch them back with their database-generated IDs
        var activityTypesDict = context.ActivityTypes.ToDictionary(at => at.Name, at => at.Id);

        
        // Hollow Knight: Silksong themed course
        var silksongCourse = new Course
        {
            Id = CourseId1,
            Name = "The Lore and Design of Hollow Knight: Silksong",
            Description = "A deep dive into the themes, mechanics, and artistry behind Team Cherry's upcoming masterpiece.",
            StartDate = new DateTime(2025, 9, 15),
            EndDate = new DateTime(2026, 1, 31),
            Modules = new List<Module>
    {
        new Module
        {
            Id = Guid.NewGuid(),
            Name = "Module 1: The World of Pharloom",
            Description = "Exploring the mysterious kingdom and its lore.",
            StartDate = new DateTime(2025, 9, 15),
            EndDate = new DateTime(2025, 10, 15),
            ModuleActivities = new List<ModuleActivity>
            {
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Lecture: Introduction to Pharloom",
                    Description = "A lecture on the regions, lore, and atmosphere of Silksong.",
                    StartDate = new DateTime(2025, 9, 16, 10, 0, 0),
                    EndDate = new DateTime(2025, 9, 16, 12, 0, 0),
                    ActivityTypeId = activityTypesDict["Föreläsning"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Essay on Pharloom's Lore",
                            Description = "Write a short essay discussing the parallels between Pharloom and Hallownest.",
                            DueDate = new DateTime(2025, 9, 23)
                        }
                    }
                },
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Seminar: NPCs of Pharloom",
                    Description = "Group discussion on the new characters and their roles.",
                    StartDate = new DateTime(2025, 9, 20, 14, 0, 0),
                    EndDate = new DateTime(2025, 9, 20, 16, 0, 0),
                    ActivityTypeId = activityTypesDict["Seminarie"]
                }
            }
        },
        new Module
        {
            Id = Guid.NewGuid(),
            Name = "Module 2: Hornet as a Protagonist",
            Description = "Analyzing gameplay changes and narrative perspective.",
            StartDate = new DateTime(2025, 10, 16),
            EndDate = new DateTime(2025, 11, 15),
            ModuleActivities = new List<ModuleActivity>
            {
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Lecture: Hornet’s Combat Mechanics",
                    Description = "Detailed breakdown of Hornet’s acrobatic playstyle.",
                    StartDate = new DateTime(2025, 10, 17, 9, 0, 0),
                    EndDate = new DateTime(2025, 10, 17, 11, 0, 0),
                    ActivityTypeId = activityTypesDict["Föreläsning"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Combat Design Analysis",
                            Description = "Compare Hornet’s combat style to the Knight’s from Hollow Knight.",
                            DueDate = new DateTime(2025, 10, 24)
                        }
                    }
                },
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Project: Designing a New Tool for Hornet",
                    Description = "Students create a concept for a new gameplay tool or weapon for Hornet.",
                    StartDate = new DateTime(2025, 10, 25, 10, 0, 0),
                    EndDate = new DateTime(2025, 11, 1, 12, 0, 0),
                    ActivityTypeId = activityTypesDict["Projekt"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Hornet Tool Concept",
                            Description = "Submit a design document for your tool idea.",
                            DueDate = new DateTime(2025, 11, 5)
                        }
                    }
                }
            }
        },
        new Module
        {
            Id = Guid.NewGuid(),
            Name = "Module 3: Bosses and Challenges",
            Description = "Exploring the design of encounters and player progression.",
            StartDate = new DateTime(2025, 11, 16),
            EndDate = new DateTime(2025, 12, 20),
            ModuleActivities = new List<ModuleActivity>
            {
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Seminar: The Philosophy of Boss Fights",
                    Description = "Discuss the themes and mechanics behind Silksong’s bosses.",
                    StartDate = new DateTime(2025, 11, 20, 13, 0, 0),
                    EndDate = new DateTime(2025, 11, 20, 15, 0, 0),
                    ActivityTypeId = activityTypesDict["Seminarie"]
                },
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Exam: Boss Pattern Recognition",
                    Description = "Written exam on boss mechanics and encounter design.",
                    StartDate = new DateTime(2025, 12, 10, 9, 0, 0),
                    EndDate = new DateTime(2025, 12, 10, 11, 0, 0),
                    ActivityTypeId = activityTypesDict["Prov"]
                }
            }
        }
    }
        };

        // ✅ Add the course with everything under it
        context.Courses.Add(silksongCourse);

        // === Solo Leveling Course ===
        var course2 = new Course
        {
            Id = CourseId2,
            Name = "Solo Leveling: Rise of the Shadow Monarch",
            Description = "Explore the epic journey of Sung Jin-Woo from E-rank hunter to Shadow Monarch.",
            StartDate = new DateTime(2025, 9, 1),
            EndDate = new DateTime(2026, 6, 30), // extends into 2026
            Modules = new List<Module>
    {
        new Module
        {
            Id = Guid.NewGuid(),
            Name = "Awakening",
            Description = "Introduction to the world of hunters and Jin-Woo’s first dungeon.",
            StartDate = new DateTime(2025, 9, 1),
            EndDate = new DateTime(2025, 12, 20),
            ModuleActivities = new List<ModuleActivity>
            {
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Double Dungeon Incident",
                    Description = "Covering the fateful dungeon that changes everything.",
                    StartDate = new DateTime(2025, 9, 10, 10, 0, 0),
                    EndDate = new DateTime(2025, 9, 10, 12, 0, 0),
                    ActivityTypeId = activityTypesDict["Föreläsning"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Dungeon Survival Report",
                            Description = "Write a detailed analysis of strategies to survive the double dungeon.",
                            DueDate = new DateTime(2025, 12, 15)
                        }
                    }
                },
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Hunter System Explained",
                    Description = "Discussion on how Jin-Woo’s system works.",
                    StartDate = new DateTime(2025, 11, 5, 14, 0, 0),
                    EndDate = new DateTime(2025, 11, 5, 16, 0, 0),
                    ActivityTypeId = activityTypesDict["Seminarie"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "System Mechanics Essay",
                            Description = "Essay on how the system’s quests and rewards shape Jin-Woo’s growth.",
                            DueDate = new DateTime(2026, 1, 10)
                        }
                    }
                }
            }
        },
        new Module
        {
            Id = Guid.NewGuid(),
            Name = "Rise of the Monarch",
            Description = "Sung Jin-Woo’s growth and battles against monarchs.",
            StartDate = new DateTime(2026, 1, 5),
            EndDate = new DateTime(2026, 6, 30),
            ModuleActivities = new List<ModuleActivity>
            {
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Shadow Army Training",
                    Description = "Practical project: leading your own shadow army.",
                    StartDate = new DateTime(2026, 2, 15, 13, 0, 0),
                    EndDate = new DateTime(2026, 2, 15, 15, 0, 0),
                    ActivityTypeId = activityTypesDict["Projekt"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Build Your Shadow Army",
                            Description = "Submit a design of your own shadow army composition and tactics.",
                            DueDate = new DateTime(2026, 4, 30)
                        }
                    }
                },
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Final Monarch War",
                    Description = "Review and test knowledge of the monarch conflict.",
                    StartDate = new DateTime(2026, 6, 15, 9, 0, 0),
                    EndDate = new DateTime(2026, 6, 15, 11, 0, 0),
                    ActivityTypeId = activityTypesDict["Prov"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Final Battle Exam",
                            Description = "Comprehensive test on the events and strategies of the final war.",
                            DueDate = new DateTime(2026, 6, 20)
                        }
                    }
                }
            }
        }
    }
        };

        // === Dogs Course ===
        var course3 = new Course
        {
            Id = CourseId3,
            Name = "The Wonderful World of Dogs",
            Description = "Learn about dogs, their history, breeds, and training.",
            StartDate = new DateTime(2025, 3, 1),
            EndDate = new DateTime(2025, 12, 31),
            Modules = new List<Module>
    {
        new Module
        {
            Id = Guid.NewGuid(),
            Name = "Dog History and Evolution",
            Description = "How dogs became man’s best friend.",
            StartDate = new DateTime(2025, 3, 1),
            EndDate = new DateTime(2025, 5, 15),
            ModuleActivities = new List<ModuleActivity>
            {
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Origins of Domestication",
                    Description = "Lecture on the first wolves becoming dogs.",
                    StartDate = new DateTime(2025, 3, 10, 10, 0, 0),
                    EndDate = new DateTime(2025, 3, 10, 12, 0, 0),
                    ActivityTypeId = activityTypesDict["Föreläsning"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Domestication Timeline",
                            Description = "Create a timeline of key events in dog domestication.",
                            DueDate = new DateTime(2025, 5, 1)
                        }
                    }
                },
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Dog Breeds Overview",
                    Description = "Seminar on the diversity of breeds.",
                    StartDate = new DateTime(2025, 4, 5, 14, 0, 0),
                    EndDate = new DateTime(2025, 4, 5, 16, 0, 0),
                    ActivityTypeId = activityTypesDict["Seminarie"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Breed Profile",
                            Description = "Choose one breed and present its traits, history, and uses.",
                            DueDate = new DateTime(2025, 6, 1)
                        }
                    }
                }
            }
        },
        new Module
        {
            Id = Guid.NewGuid(),
            Name = "Training and Care",
            Description = "How to train and care for dogs properly.",
            StartDate = new DateTime(2025, 6, 1),
            EndDate = new DateTime(2025, 12, 31),
            ModuleActivities = new List<ModuleActivity>
            {
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Obedience Training",
                    Description = "Hands-on project teaching dogs basic commands.",
                    StartDate = new DateTime(2025, 7, 15, 9, 0, 0),
                    EndDate = new DateTime(2025, 7, 15, 11, 0, 0),
                    ActivityTypeId = activityTypesDict["Projekt"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Training Logbook",
                            Description = "Maintain a logbook of your dog’s progress.",
                            DueDate = new DateTime(2025, 10, 15)
                        }
                    }
                },
                new ModuleActivity
                {
                    Id = Guid.NewGuid(),
                    Name = "Dog Show Presentation",
                    Description = "Students present their dogs’ skills.",
                    StartDate = new DateTime(2025, 12, 10, 13, 0, 0),
                    EndDate = new DateTime(2025, 12, 10, 15, 0, 0),
                    ActivityTypeId = activityTypesDict["Redovisning"],
                    Assignments = new List<Assignment>
                    {
                        new Assignment
                        {
                            Id = Guid.NewGuid(),
                            Name = "Showcase Report",
                            Description = "Submit a report on your presentation experience.",
                            DueDate = new DateTime(2025, 12, 20)
                        }
                    }
                }
            }
        }
    }
        };

        // Add to context
        context.Courses.AddRange(course2, course3);


        //        // ModuleActivity faker
        //        var moduleActivityFaker = new Faker<ModuleActivity>("sv")
        //            .RuleFor(ma => ma.Name, f => f.Commerce.ProductName())
        //            .RuleFor(ma => ma.Description, f => f.Lorem.Sentence(6))
        //            .RuleFor(ma => ma.StartDate, f => f.Date.FutureOffset(1).DateTime)
        //            .RuleFor(ma => ma.EndDate, (f, ma) => ma.StartDate.AddDays(f.Random.Int(1, 30)))
        //            .RuleFor(ma => ma.Type, f => f.PickRandom(activityTypes));

        //        // Module faker
        //        var moduleFaker = new Faker<Module>("sv")
        //            .RuleFor(m => m.Name, f => f.Commerce.Department())
        //            .RuleFor(m => m.Description, f => f.Lorem.Sentence(8))
        //            .RuleFor(m => m.StartDate, f => f.Date.FutureOffset(1).DateTime)
        //            .RuleFor(m => m.EndDate, (f, m) => m.StartDate.AddMonths(f.Random.Int(1, 6)))
        //            .RuleFor(m => m.ModuleActivities, (f, m) =>
        //            {
        //                var activities = moduleActivityFaker.Generate(f.Random.Int(2, 5));

        //                return activities;
        //            });

        //        // Course faker
        //        var courseFaker = new Faker<Course>("sv")
        //            .RuleFor(c => c.Name, f => f.Company.CompanyName())
        //            .RuleFor(c => c.Description, f => f.Lorem.Sentence(10))
        //            .RuleFor(c => c.StartDate, f => f.Date.FutureOffset(1).DateTime)
        //            .RuleFor(c => c.EndDate, (f, c) => c.StartDate.AddMonths(f.Random.Int(3, 12)))
        //            .RuleFor(c => c.Modules, (f, c) =>
        //            {
        //                var modules = moduleFaker.Generate(f.Random.Int(2, 4));

        //                return modules;
        //            })
        //            .RuleFor(c => c.Students, (f, c) =>
        //            {
        //                var students = context.Users
        //                    .OrderBy(_ => Guid.NewGuid()) // Randomize order
        //                    .Take(f.Random.Int(2, 4)) // Random number of students
        //                    .ToList();
        //                return students;
        //            });


        //        var courses = courseFaker.Generate(courseAmount);


        //        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();
    }

    private async Task AddDocumentsAsync(ApplicationDbContext context)
    {
        Random rnd = new();
        // Document faker
        var documentFaker = new Faker<Document>("sv")
            .RuleFor(d => d.Name, f => f.System.FileName())
            .RuleFor(d => d.FileType, f => f.System.CommonFileType())
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
            .RuleFor(s => s.SubmissionDocument, (f, s) =>
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
                //ApplicationUserId = student.Id,
                Id = Guid.NewGuid()
            };
            doc.ParentId = submission.Id;
            submission.SubmissionDocument = doc;

            allSubmissions.Add(submission);
        }

        //context.Submissions.AddRange(allSubmissions);
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
