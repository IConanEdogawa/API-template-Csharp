using App.Application;
using App.Application.Extensions;
using Microsoft.Extensions.Caching.Memory;
using App.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;
using System;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register your custom services
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddSingleton<ITelegramBotClient>(provider =>
        {
            var botToken = builder.Configuration["AppSettings:TelegramBot:Token"];
            return new TelegramBotClient(botToken!);
        });

        // Add memory cache for rate limiting
        builder.Services.AddMemoryCache();
        

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            // Use HSTS in production
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        // Add rate limiting middleware before authorization middleware
        app.UseRateLimiting(limit: 5, window: TimeSpan.FromMinutes(1));

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
