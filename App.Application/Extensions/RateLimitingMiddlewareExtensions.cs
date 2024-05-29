using App.Application.Middlwares;
using Microsoft.AspNetCore.Builder;

namespace App.Application.Extensions
{
    public static class RateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRateLimiting(this IApplicationBuilder builder, int limit, TimeSpan window)
        {
            return builder.UseMiddleware<RateLimitingMiddleware>(limit, window);
        }
    }
}
