using App.Application.UseCases.UserCase.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ForTelegram.Services
{
    public class BSTimer : BackgroundService
    {
        private readonly TelegramBotClient _botClient;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMediator _mediator;
        public BSTimer(TelegramBotClient botClient, IServiceScopeFactory scopeFactory, IMediator mediator)
        {
            _botClient = botClient;
            _scopeFactory = scopeFactory;
            _mediator = mediator;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _scopeFactory.CreateScope();
            
            var users = await _mediator.Send(new GetAllTgUsersQuery());
            while(!stoppingToken.IsCancellationRequested)
            {
                foreach(var user in users)
                {
                    await _botClient.SendTextMessageAsync(
                    chatId: user.TgId,
                    text: "Qales",
                    cancellationToken: stoppingToken
                    
                    ); 
                }
                await Task.Delay(TimeSpan.FromSeconds(12), stoppingToken);
            }
        }
    }
}
