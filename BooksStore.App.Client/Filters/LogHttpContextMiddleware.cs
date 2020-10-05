using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BooksStore.Domain.Contracts.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace BooksStore.App.Client.Filters
{
    internal class LogHttpContextMiddleware
    {
        private readonly ILogger _logger;

        private readonly RequestDelegate _next;

        public LogHttpContextMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            //_logger = loggerFactory.GetLogger("RequestStatLogger");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestBody = await ReadRequest(context.Request);
            string responseBody = null;

            var bodyStream = context.Response.Body;

            var stopwatch = Stopwatch.StartNew();
            try
            {
                using (var memStream = new MemoryStream())
                {
                    context.Response.Body = memStream;

                    await _next(context);

                    memStream.Seek(0, SeekOrigin.Begin);
                    responseBody = await new StreamReader(memStream, Encoding.UTF8, true).ReadToEndAsync();

                    memStream.Seek(0, SeekOrigin.Begin);
                    await memStream.CopyToAsync(bodyStream);
                }
            }
            finally
            {
                stopwatch.Stop();
                context.Response.Body = bodyStream;

                var data = new ExtendedMonitoringMessage
                {
                    Identity = context.User.Identity?.Name ?? "anonymous",
                    Route = context.Request.Path,
                    RequestUrl = context.Request.GetDisplayUrl(),
                    ActionParameters = requestBody,
                    RequestorIp = context.Connection.RemoteIpAddress.ToString(),
                    ElapsedMilliseconds = stopwatch.ElapsedMilliseconds
                };

                if (context.Response.StatusCode < StatusCodes.Status400BadRequest)
                {
                    data.ResultType = ResultType.Success;
                }
                else if (context.Response.StatusCode >= StatusCodes.Status400BadRequest &&
                         context.Response.StatusCode < StatusCodes.Status500InternalServerError)
                {
                    data.ResultType = ResultType.ValidationError;
                    data.Error = responseBody;
                }
                else if (context.Response.StatusCode >= StatusCodes.Status500InternalServerError)
                {
                    data.ResultType = ResultType.Error;
                    data.Error = responseBody;
                }

                var logLevel = data.ResultType == ResultType.Error ? LogLevel.Error : LogLevel.Trace;
                //_logger.LogWithData(logLevel, data, $"request from {data.RequestorIp} to {data.RequestUrl}");
            }
        }

        private static async Task<string> ReadRequest(HttpRequest request)
        {
            //request.EnableRewind();
            try
            {
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            finally
            {
                request.Body.Seek(0, SeekOrigin.Begin);
            }
        }
    }
}