using Microsoft.AspNetCore.Http;

namespace IndexTableMiddleware
{
    public class TrackingMiddleware
    {
        private readonly RequestDelegate _next;
        public TrackingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _ = AnalyzeIncomingRequest(context);
            await _next(context);
        }

        private Task AnalyzeIncomingRequest(HttpContext context)
        {
            var sa = shouldAnalyze();
            if (!sa) return Task.CompletedTask;

            return Task.CompletedTask;

            bool shouldAnalyze()
            {
                return false;
            }
        }
    }
}