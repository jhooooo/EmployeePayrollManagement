using Microsoft.AspNetCore.Http;
using Sprout.Exam.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sprout.Exam.Common
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        public ErrorHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var responseModel = ApiResponse<string>.Fail(error.Message);
                switch (error)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;
                    case KeyNotFoundException e:
                        // not found error
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }
                var result = JsonSerializer.Serialize(responseModel);
                await response.WriteAsync(result);
            }
        }
    }
}


//public class ErrorHandlerMiddleware
//{
//    private readonly RequestDelegate _next;

//    public ErrorHandlerMiddleware(RequestDelegate next)
//    {
//        _next = next;
//    }

//    public async Task Invoke(HttpContext context)
//    {
//        try
//        {
//            await _next(context);
//        }
//        catch (Exception error)
//        {
//            var response = context.Response;
//            response.ContentType = "application/json";

//            switch (error)
//            {
//                case AppException e:
//                    // custom application error
//                    response.StatusCode = (int)HttpStatusCode.BadRequest;
//                    break;
//                case KeyNotFoundException e:
//                    // not found error
//                    response.StatusCode = (int)HttpStatusCode.NotFound;
//                    break;
//                default:
//                    // unhandled error
//                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
//                    break;
//            }

//            var result = JsonSerializer.Serialize(new { message = error?.Message });
//            await response.WriteAsync(result);
//        }
//    }
//}