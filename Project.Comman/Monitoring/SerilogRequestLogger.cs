using System;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;

namespace Ettad.CrossCutting.Comman.Monitoring
{
    /// <summary>
    /// Middleware for logging HTTP requests and responses with Serilog
    /// </summary>
    public class SerilogRequestLogger
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        public SerilogRequestLogger(RequestDelegate next, ILogger logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var sw = Stopwatch.StartNew();
            try
            {
                await _next(httpContext);
                sw.Stop();

                var statusCode = httpContext.Response?.StatusCode;
                var level = statusCode > 499 ? LogEventLevel.Error : LogEventLevel.Information;

                var log = level == LogEventLevel.Error ? LogForErrorContext(httpContext) : Log.Logger;
                log.Write(level, MessageTemplate, httpContext.Request.Method, GetPath(httpContext), statusCode, sw.Elapsed.TotalMilliseconds);
            }
            catch (System.Exception ex)
            {
                sw.Stop();
                LogForErrorContext(httpContext)
                    .Error(ex, MessageTemplate, httpContext.Request.Method, GetPath(httpContext), 500, sw.Elapsed.TotalMilliseconds);
                throw;
            }
        }

        private static ILogger LogForErrorContext(HttpContext httpContext)
        {
            var request = httpContext.Request;

            var logContext = Log
                .ForContext("RequestHeaders", request.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
                .ForContext("RequestHost", request.Host.Value)
                .ForContext("RequestProtocol", request.Protocol)
                .ForContext("RequestScheme", request.Scheme)
                .ForContext("RequestPath", request.Path)
                .ForContext("RequestQueryString", request.QueryString.Value)
                .ForContext("RequestMethod", request.Method);

            // Add user information if available
            if (httpContext.User?.Identity?.IsAuthenticated == true)
            {
                logContext = logContext
                    .ForContext("UserId", httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                    .ForContext("UserName", httpContext.User.Identity.Name)
                    .ForContext("OrganizationId", httpContext.User.FindFirst("OrgId")?.Value);
            }

            return logContext;
        }

        private static string GetPath(HttpContext httpContext)
        {
            return httpContext.Request.Path.ToString();
        }
    }
}

