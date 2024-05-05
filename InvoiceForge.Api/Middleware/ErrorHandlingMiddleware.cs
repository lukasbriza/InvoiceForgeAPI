using InvoiceForgeApi.Errors;
using InvoiceForgeApi.Helpers;
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
            catch (NotUniqueEntityError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);}
            catch (NoPossessionError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);}
            catch (NoEntityError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);}
            catch (InvalidModelError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);}
            catch (EntityReferenceError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);}

            catch (OperationError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);}
            catch (DbSetError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);}
            catch (ContextSaveError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.InternalServerError);}
            
            catch (ApiError ex) {await HandleExceptionAsync(context, ex, HttpStatusCode.BadRequest);}
            catch (Exception ex) {await HandleExceptionAsync(context, new ApiError(ex), HttpStatusCode.InternalServerError);}
        }
        private static Task HandleExceptionAsync(HttpContext context, ApiError ex, HttpStatusCode code)
        {   
            ResponseBuilder<bool> repsonseBuilder = new ResponseBuilder<bool>(false, ex);
            CustomResponse<bool> response = repsonseBuilder.Get();
            var result = JsonConvert.SerializeObject(response);

            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}
