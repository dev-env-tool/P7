using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.ModelBinding;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

//namespace P7CreateRestApi.Filters
//{
//    public class AsyncActionFilter : Attribute, IActionFilter
//    {

//        public bool AllowMultiple => false;

//        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(
//        HttpActionContext actionContext,
//        CancellationToken cancellationToken,
//        Func<Task<HttpResponseMessage>> continuation)
//        {
//            // 1. Pre-processing logic (Before Action Executes)
//            Console.WriteLine("Pre-action processing...");

//            // 2. Short-circuit if needed
//            if (actionContext.Response != null)
//            {
//                return actionContext.Response;
//            }

//            // 3. Execute the rest of the pipeline
//            HttpResponseMessage response = await continuation();

//            // 4. Post-processing logic (After Action Executes)
//            Console.WriteLine("Post-action processing...");

//            return response;
//        }
//        //public override void ExecuteActionFilterAsync(HttpActionContext actionContext)
//        //{
//        //    if (actionContext.ModelState.IsValid == false)
//        //    {
//        //        actionContext.Response = actionContext.Request.CreateErrorResponse(
//        //            HttpStatusCode.BadRequest, actionContext.ModelState);
//        //    }
//        //}
//    }
//}





// IAsyncActionFilter implicitly inherits from IFilterMetadata
namespace P7CreateRestApi.Filters
{
    public class AsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            // 1. Pre-processing logic (Before Action Executes)
            // You can check or mutate context.ActionArguments here
            if (context.ModelState.Count > 0)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
            else 
            { 
                // 2. Execute the action (and downstream filters)
                ActionExecutedContext resultContext = await next();
            }
            // 3. Post-processing logic (After Action Executes)
            // You can inspect or mutate resultContext.Result here
            
        }
    }
}