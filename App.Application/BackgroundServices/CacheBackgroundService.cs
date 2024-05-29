using App.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

public class CacheBackgroundService : BackgroundService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IAppDbContext _appDbContext;

    public CacheBackgroundService(IMemoryCache memoryCache, IAppDbContext appDbContext)
    {
        _memoryCache = memoryCache;
        _appDbContext = appDbContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await RefreshCache();
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
        }
    }

    private async Task RefreshCache()
    {
        try
        {
            // Получите данные из базы данных
            var data = await _appDbContext.Users.ToListAsync();

            // Обновите данные в кэше
            _memoryCache.Set("CachedData", data, TimeSpan.FromMinutes(10)); // Установите время жизни кэша на 10 минут
        }
        catch (Exception ex)
        {
            // Обработайте ошибку
            Console.WriteLine($"An error occurred while refreshing cache: {ex.Message}");
        }
    }
}
