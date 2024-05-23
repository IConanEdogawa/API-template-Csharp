using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace App.Application.BackgroundServices
{
    public class Greeting : BackgroundService
    {
        private readonly TelegramBotClient _botClient;
        private readonly ILogger<Greeting> _logger;

        public Greeting(TelegramBotClient botClient, ILogger<Greeting> logger)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _botClient.SendTextMessageAsync(chatId: 5945913071, text: "Assalomu alaykum ! hayrli kun.", cancellationToken: stoppingToken);
        }
    }
}
