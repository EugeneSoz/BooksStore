using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BooksStore.App.Client.Filters
{
    /// <summary>
    /// The general exception filter.
    /// </summary>
    /// <seealso cref="ExceptionFilterAttribute" />
    public class GeneralExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ProblemDetails problemDetails;

            switch (context.Exception)
            {
                case ValidationException validationException:
                    problemDetails = GetProblemDetails(validationException);
                    break;
                default:
                    problemDetails = GetProblemDetails(context.Exception);
                    break;
            }

            problemDetails.Instance = context.HttpContext.Request.Path;
            problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);

            context.Result = new JsonResult(problemDetails);
            context.HttpContext.Response.ContentType = "application/problem+json";
            context.HttpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        }

        private static ValidationProblemDetails GetProblemDetails(ValidationException exception)
        {
            var modelState = new ModelStateDictionary();

            foreach (var memberName in exception.ValidationResult.MemberNames)
            {
                modelState.AddModelError(memberName, exception.ValidationResult.ErrorMessage);
            }

            return new ValidationProblemDetails(modelState)
            {
                Status = StatusCodes.Status400BadRequest
            };
        }

        private static ProblemDetails GetProblemDetails(Exception exception, bool printStackTrace = true)
        {
            var problemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = exception.Message,
                Status = StatusCodes.Status500InternalServerError,
                Detail = printStackTrace ? exception.ToString() : exception.Message
            };

            return problemDetails;
        }
    }
}