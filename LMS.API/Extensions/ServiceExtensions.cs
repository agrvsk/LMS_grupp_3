using LMS.Infractructure.Data;
using LMS.Infractructure.Repositories;
using LMS.Presentation;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace LMS.API.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            //ToDo: Restrict access to your BlazorApp only!
            options.AddDefaultPolicy(policy =>
            {
                //..
                //..
                //..
            });

            //Can be used during development
            options.AddPolicy("AllowAll", p =>
               p.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
        });
    }

    public static void ConfigureOpenApi(this IServiceCollection services) =>
       services.AddEndpointsApiExplorer()
               .AddSwaggerGen(setup =>
               {
                   setup.EnableAnnotations();

                   setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                   {
                       In = ParameterLocation.Header,
                       Description = "Place to add JWT with Bearer",
                       Name = "Authorization",
                       Type = SecuritySchemeType.Http,
                       Scheme = "Bearer"
                   });

                   setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                   {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                   });
               });

    public static void ConfigureControllers(this IServiceCollection services)
    {
        services.AddControllers(opt =>
        {
            opt.ReturnHttpNotAcceptable = true;
            opt.Filters.Add(new ProducesAttribute("application/json"));

        })
                .AddNewtonsoftJson()
                .AddApplicationPart(typeof(AssemblyReference).Assembly);
    }

    public static void ConfigureSql(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("ApplicationDbContext") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContext' not found.")));
    }

    public static void AddRepositories(this IServiceCollection services)
    {

        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IModuleActivityRepository, ModuleActivityRepository>();
        services.AddScoped<IApplicationUserRepository, ApplicationUserRepository>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        services.AddScoped<IDocumentRepository, DocumentRepository>();
        services.AddScoped<IActivityTypeRepository, ActivityTypeRepository>();

        services.AddLazy<ICourseRepository>();
        services.AddLazy<IModuleRepository>();
        services.AddLazy<IModuleActivityRepository>();
        services.AddLazy<IApplicationUserRepository>();
        services.AddLazy<ISubmissionRepository>();
        services.AddLazy<IDocumentRepository>();
        services.AddLazy<IActivityTypeRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

    }

    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();

        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICourseService, CourseService>();
        services.AddScoped<IModuleActivityService, ModuleActivityService>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IActivityTypeService, ActivityTypeService>();
        services.AddScoped<IFileHandlerService>(provider =>
        {
            var env = provider.GetRequiredService<IWebHostEnvironment>();
            var rootPath = env.ContentRootPath;
            return new FileHandlerService(rootPath);
        });
        services.AddScoped<IScheduleService, ScheduleService>();

        services.AddScoped(provider => new Lazy<IAuthService>(() => provider.GetRequiredService<IAuthService>()));
        services.AddLazy<ICourseService>();
        services.AddLazy<IDocumentService>();
        services.AddLazy<IModuleActivityService>();
        services.AddLazy<IModuleService>();
        services.AddLazy<ISubmissionService>();
        services.AddLazy<IUserService>();
        services.AddLazy<IActivityTypeService>();
        services.AddLazy<IFileHandlerService>();
        services.AddLazy<IScheduleService>();


    }



}

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLazy<TService>(this IServiceCollection services) where TService : class
    {
        return services.AddScoped(provider => new Lazy<TService>(() => provider.GetRequiredService<TService>()));
    }
}

