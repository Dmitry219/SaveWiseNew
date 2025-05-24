namespace Middleware
{
    public class DiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerService _logger;

        public DiLoggingMiddleware(RequestDelegate next, ILoggerService logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            _logger.Log($"Входящий запрос: {context.Request.Method} {context.Request.Path}");
            await _next(context);
        }
    }
}
