using Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApplication.Filters
{
    public class ExceptionFilter : Attribute, IExceptionFilter
    {
        private ExceptionContext context;
        public void OnException(ExceptionContext exceptionContext)
        {
            this.context = exceptionContext;
            try
            {
                throw context.Exception;
            }
            catch (ArgumentException)
            {
                SetResult(400, "Bad Request: " + context.Exception.Message);
            }
            catch (InvalidOperationException)
            {
                SetResult(400, "Bad Request: " + context.Exception.Message);
            }
            catch (ResourceNotFoundException)
            {
                SetResult(404, "Not found: " + context.Exception.Message);
            }
            catch (Exception)
            {
                SetResult(500, "Unexpected error: " + context.Exception.Message);
            }
        }

        private void SetResult(int statusCode, string message) => 
            context.Result = new ContentResult() { StatusCode = statusCode, Content = message };
        
    }
}
