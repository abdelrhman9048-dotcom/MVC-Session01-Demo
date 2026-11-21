using System.Diagnostics;

namespace Session01_Demo.Middleware
{
    // Rules
    /*
        1] RequestDelegate next [Inject ctor]

        2] Method name InvokeAsync , Invoke [HttpContext context]

        3] Call await next(context)

        4] Register with app.UseMiddleware<RequestTimingMiddleware>()
    */
    public class RequestTimeMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stop = Stopwatch.StartNew();
            await _next.Invoke(context);

            stop.Stop();
            Console.WriteLine($"{context.Request.Path} --> {stop.ElapsedMilliseconds} ms");
        }
}
