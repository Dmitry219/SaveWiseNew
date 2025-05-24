namespace Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Логируем запрос
            var request = context.Request;
            Console.WriteLine($"Запрос: {request.Method} {request.Path}");

            // Сохраняем оригинальный Response.Body для чтения
            var originalBody = request.HttpContext.Response.Body;
            using (var newBody = new MemoryStream())
            {
                request.HttpContext.Response.Body = newBody;

                // Продолжаем конвейер
                await _next(context);

                // Логируем ответ
                newBody.Seek(0, SeekOrigin.Begin);
                var responseBody = await new StreamReader(newBody).ReadToEndAsync();
                Console.WriteLine($"Ответ: {context.Response.StatusCode}, Тело: {responseBody}");

                // Восстанавливаем оригинальный Response.Body
                newBody.Seek(0, SeekOrigin.Begin);
                await newBody.CopyToAsync(originalBody);
                context.Response.Body = originalBody;
            }
        }
    }
}
