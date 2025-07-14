namespace LMS.MiddleWare
{
    using System.Globalization;
    using System.Net;
    using System.Text.Json;

    public class AppException : Exception
    {
        public AppException()
            : base() { }

        public AppException(string message)
            : base(message) { }

        public AppException(string message, params object[] args)
            : base(string.Format(CultureInfo.CurrentCulture, message, args)) { }
    }

    public class ErrorHandlerMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleWare> _logger;

        public ErrorHandlerMiddleWare(RequestDelegate next, ILogger<ErrorHandlerMiddleWare> logger)
        {
            _next = next;
            _logger = logger;
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

                switch (error)
                {
                    case AppException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        break;

                    case KeyNotFoundException:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;

                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        _logger.LogError(error, error.Message);
                        break;
                }

                var result = JsonSerializer.Serialize(
                    new { message = GetInnermostExceptionMessage(error) }
                );

                await response.WriteAsync(result);
            }
        }

        private static string GetInnermostExceptionMessage(Exception ex)
        {
            while (ex.InnerException != null)
                ex = ex.InnerException;

            return ex.Message;
        }
    }
}