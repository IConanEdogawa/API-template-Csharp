using App.Application;
using App.Application.BackgroundServices;
using App.Infrastructure;
using ForTelegram.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Polling;

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

        //builder.Services.AddHostedService<BS>();
        builder.Services.AddSingleton<IUpdateHandler, BotUpdateHand>();

        builder.Services.AddHostedService<Greeting>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
        else
        {
            // Use HSTS in production
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
