using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using App.Domain.Entities.Models;
using MediatR;


namespace ForTelegram.Services
{
    public class BotUpdateHand : IUpdateHandler
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMediator _mediator;
        public BotUpdateHand(IServiceScopeFactory serviceScopeFactory, IMediator mediator)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mediator = mediator;
        }
        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var scope = _serviceScopeFactory.CreateScope();
            
            if (update.Message is not { } message)
                return;
            if (message.Text is not { } messageText)
                return;

            var chatId = message.Chat.Id;


            var user = new TgUser
            {
                Id = chatId,
                UserName = message.Chat.Username,

            };
            await _mediator.SendAsync()

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText,
                cancellationToken: cancellationToken);
        }
    }
}
