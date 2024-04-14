using InvoiceForgeApi.DTO;
using InvoiceForgeApi.Models.Enum;
using Newtonsoft.Json;
using System.Net;

namespace InvoiceForgeApi.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ApiError ex)
            {
                await HandleEsceptionAsync(context, ex);
            }
            catch (Exception ex)
            {
                await HandleEsceptionAsync(context, new ApiError(ex));
            }
        }
        private static Task HandleEsceptionAsync(HttpContext context, ApiError ex)
        {
            Console.Write(ex.StackTrace);
            
            var code = ex.Code == ErrorCodes.U_E ? HttpStatusCode.InternalServerError : HttpStatusCode.BadRequest;
            var result = JsonConvert.SerializeObject(new {
                code = ex.Code.ToString(),
                error = ex.Message, 
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }
}
