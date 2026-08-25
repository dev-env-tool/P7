//using System;
//using MediatR;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Serilog.Context;


//namespace P7CreateRestApi.LogUserNameMiddleware;

//internal sealed class RequestLoggingPipelineBehavior<TRequest, TResponse>
//    : IPipelineBehavior<TRequest, TResponse>
//    where TRequest : class
//    where TResponse : IAsyncResult
//{
//    private readonly ILogger<RequestLoggingPipelineBehavior<TRequest, TResponse>> _logger;
//    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
//    {
//        string requestName = typeof(TRequest).Name;

//        _logger.LogInformation("The request : {RequestName} is running", requestName);

//        TResponse result = await next();

//        if (result.IsCompleted)
//        {
//            _logger.LogInformation("The request : {requestName} was completed", requestName);
//        }
//        else
//        {
//            //using (LogContext.PushProperty("Error", result.AsyncState, true))
//            //{ 
//            //    _logger.LogError("The request : {requestName} was completed with error {Error}", requestName);
//            //}
//            _logger.LogError("The request : {requestName} was completed with error {Error}", requestName);


//        }
//        return result;
//    }
//}






