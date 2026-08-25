//using Serilog.Context;

//namespace P7CreateRestApi.LogUserNameMiddleware
//{
//    public class LogUserNameMiddleware
//    {
//        private readonly RequestDelegate _next;
//        public LogUserNameMiddleware(RequestDelegate next) => _next = next;

//        public Task InvokeAsync(HttpContext context)
//        {
//            using (Serilog.Context.LogContext.PushProperty("CorrelationId", context.TraceIdentifier))
//            {
//                return _next(context);
//            }
//        }
//    }
//}











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


using Duende.IdentityServer.Extensions;
using Newtonsoft.Json;
using Serilog;
using Serilog.Context;
namespace P7CreateRestApi.LogUserNameMiddleware
{
    public class LogUserNameMiddleware
    {
        private readonly RequestDelegate next;

        public LogUserNameMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public Task Invoke(HttpContext context)
        {
            
            // If the user is trying to loggin, claims do not yet exist
            // If a logged in user with JWT bearer uses a controller, then claims exist claim[0] = User.Email
            if (context.User.Claims.Count() > 0)
            {
                Log.Information("UserName{UserName}", context.User.Claims.AsEnumerable().ElementAt(0));
            }
            else
                // If a logged in user without giving the JWT bearer acts then it has no claims.
                // Trying to write them will give an error message : dateTime [INF] Authorization failed. These requirements were not met:
                // DenyAnonymousAuthorizationRequirement: Requires an authenticated user.
            {
                LogContext.PushProperty("UserName", context.User.Claims.AsEnumerable());
            }
            return next(context);
        }
    }
}