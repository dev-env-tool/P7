using Serilog.Context;

namespace P7CreateRestApi.LogUserNameMiddleware
{
    public class LogUserNameMiddleware
    {
        private readonly RequestDelegate _next;
        public LogUserNameMiddleware(RequestDelegate next) => _next = next;

        public Task InvokeAsync(HttpContext context)
        {
            using (Serilog.Context.LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
            {
                return _next(context);
            }
        }
    }
}
//using Serilog.Core;
//using Serilog.Events;
//using System.Security.Claims;

//public class RequestUserIdEnricher : ILogEventEnricher
//{
//    private readonly IHttpContextAccessor _contextAccessor;

//    public RequestUserIdEnricher(IHttpContextAccessor contextAccessor)
//    {
//        _contextAccessor = contextAccessor;
//    }

//    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
//    {
//        var userId = _contextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

//        if (!string.IsNullOrEmpty(userId))
//        {
//            var property = propertyFactory.CreateProperty("UserId", userId);
//            logEvent.AddPropertyIfAbsent(property);
//        }
//    }
//}