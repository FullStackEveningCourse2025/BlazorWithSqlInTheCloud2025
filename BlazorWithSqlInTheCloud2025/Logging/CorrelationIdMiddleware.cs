using Serilog.Context;
using System.Diagnostics;

namespace BlazorWithSqlInTheCloud2025.Logging
{
    public class CorrelationIdMiddleware
    {
        private const string HeaderName = "X-Correlation-Id";
        private readonly RequestDelegate _next;


        public CorrelationIdMiddleware(RequestDelegate next) => _next = next;


        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers[HeaderName].FirstOrDefault()
            ?? context.TraceIdentifier
            ?? Activity.Current?.Id
            ?? Guid.NewGuid().ToString("n");


            // add to response so clients can see it
            context.Response.Headers[HeaderName] = correlationId;


            // add to Serilog LogContext
            using (LogContext.PushProperty("CorrelationId", correlationId))
            using (LogContext.PushProperty("UserName", context.User?.Identity?.Name ?? "Anonymous"))
            {
                await _next(context);
            }
        }
    }
}
