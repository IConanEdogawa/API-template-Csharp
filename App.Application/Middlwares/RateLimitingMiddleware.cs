using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Application.Middlwares
{
    public class RateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private readonly int _limit;
        private readonly TimeSpan _window;

        public RateLimitingMiddleware(RequestDelegate next, IMemoryCache cache, int limit, TimeSpan window)
        {
            _next = next;
            _cache = cache;
            _limit = limit;
            _window = window;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var key = GenerateCacheKey(context);
            var windowStart = DateTime.UtcNow.RoundDown(_window);

            var count = _cache.GetOrCreate(key, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _window;
                return 0;
            });

            if (count >= _limit)
            {
                context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                await context.Response.WriteAsync("Rate limit exceeded. Try again later.");
                return;
            }

            _cache.Set(key, count + 1);

            await _next(context);
        }

        private string GenerateCacheKey(HttpContext context)
        {
            return $"{context.Request.Path}:{context.Connection.RemoteIpAddress}";
        }
    }

    public static class DateTimeExtensions
    {
        public static DateTime RoundDown(this DateTime dateTime, TimeSpan interval)
        {
            return new DateTime(dateTime.Ticks / interval.Ticks * interval.Ticks);
        }
    }
}
