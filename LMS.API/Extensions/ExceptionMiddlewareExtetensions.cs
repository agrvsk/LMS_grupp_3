using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LMS.API.Extensions;

public static class ExceptionMiddlewareExtetensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                //              ArgumentNullException.ThrowIfNull(nameof(IExceptionHandlerFeature));

                if (contextFeature != null)
                {
                    var problemDetailsFactory = app.Services.GetRequiredService<ProblemDetailsFactory>();

                    ProblemDetails problemDetails = CreateProblemDetails(context, contextFeature.Error, problemDetailsFactory, app);
                    /*
                                        ProblemDetails problemDetails;
                                        int statusCode;
                                        switch (contextFeature.Error)
                                        {
                                            case TokenValidationException tokenValidationException:
                                                statusCode = tokenValidationException.StatusCode;
                                                problemDetails = problemDetailsFactory.CreateProblemDetails(
                                                        context,
                                                        statusCode,
                                                        detail: tokenValidationException.Message,
                                                        instance: context.Request.Path);
                                                break;
                                            default:
                                                statusCode = StatusCodes.Status500InternalServerError;
                                                problemDetails = problemDetailsFactory.CreateProblemDetails(
                                                        context,
                                                        statusCode,
                                                        title: "Internal Server Error",
                                                        detail: contextFeature.Error.Message,
                                                        instance: context.Request.Path);
                                                break;
                                        }
                    context.Response.StatusCode = statusCode;
*/
                    context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });
    }

    private static ProblemDetails CreateProblemDetails(HttpContext context, Exception error, ProblemDetailsFactory? problemDetailsFactory, WebApplication app)
    {
        return error switch
        {
            NoUsersFoundException noUsersFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: noUsersFoundException.Title,
            type: context.Request.Method,
            detail: noUsersFoundException.Message,
            instance: context.Request.Path),

            UserNotFoundException userNotFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: userNotFoundException.Title,
            type: context.Request.Method,
            detail: userNotFoundException.Message,
            instance: context.Request.Path),

            NoCoursesFoundException noCoursesFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: noCoursesFoundException.Title,
            type: context.Request.Method,
            detail: noCoursesFoundException.Message,
            instance: context.Request.Path),

            CourseNotFoundException courseNotFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: courseNotFoundException.Title,
            type: context.Request.Method,
            detail: courseNotFoundException.Message,
            instance: context.Request.Path),

            NoModulesFoundException noModulesFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: noModulesFoundException.Title,
            type: context.Request.Method,
            detail: noModulesFoundException.Message,
            instance: context.Request.Path),

            ModuleNotFoundException moduleNotFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: moduleNotFoundException.Title,
            type: context.Request.Method,
            detail: moduleNotFoundException.Message,
            instance: context.Request.Path),

            NoActivitiesFoundException noActivitiesFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: noActivitiesFoundException.Title,
            type: context.Request.Method,
            detail: noActivitiesFoundException.Message,
            instance: context.Request.Path),

            ActivityNotFoundException activityNotFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: activityNotFoundException.Title,
            type: context.Request.Method,
            detail: activityNotFoundException.Message,
            instance: context.Request.Path),

            NoDocumentsFoundException noDocumentsFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: noDocumentsFoundException.Title,
            type: context.Request.Method,
            detail: noDocumentsFoundException.Message,
            instance: context.Request.Path),

            DocumentNotFoundException documentNotFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: documentNotFoundException.Title,
            type: context.Request.Method,
            detail: documentNotFoundException.Message,
            instance: context.Request.Path),

            NoSubmissionsFoundException noSubmissionsFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: noSubmissionsFoundException.Title,
            type: context.Request.Method,
            detail: noSubmissionsFoundException.Message,
            instance: context.Request.Path),

            SubmissionNotFoundException submissionNotFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: submissionNotFoundException.Title,
            type: context.Request.Method,
            detail: submissionNotFoundException.Message,
            instance: context.Request.Path),

            NotFoundException notFoundException
            => problemDetailsFactory.CreateProblemDetails(
            context,
            StatusCodes.Status404NotFound,
            title: notFoundException.Title,
            type: context.Request.Method,
            detail: notFoundException.Message,
            instance: context.Request.Path),

            TokenValidationException tokenValidationException
            => problemDetailsFactory.CreateProblemDetails(
                context,
                statusCode: tokenValidationException.StatusCode,
                detail: tokenValidationException.Message,
                instance: context.Request.Path),

            _ => problemDetailsFactory.CreateProblemDetails(
                context,
                StatusCodes.Status500InternalServerError,
                title: "Internal Server Error",
                //              detail: error.Message,
                detail: app.Environment.IsDevelopment()
                ? error.Message
                : "Un unexpected error occured.",
                instance: context.Request.Path
                )
        };
    }


}
