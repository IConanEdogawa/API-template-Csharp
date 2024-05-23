using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace App.Application.BackgroundServices
{
    public class Greating : BackgroundService
    {
        private readonly TelegramBotClient _botClient;

        public Greating(TelegramBotClient botClient)
        {
            _botClient = botClient;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();

            return Task.CompletedTask;
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            // Обработка входящих сообщений
            if (message.Text != null)
            {
                // Обработка сообщения и отправка ответа
                await ProcessMessageAsync(message.Chat.Id, message.Text);
            }
        }

        private async Task ProcessMessageAsync(long chatId, string message)
        {
            // Реализация логики бота здесь
            // Например, проверка определенных команд или ключевых слов
            if (message.Contains("/start"))
            {
                await _botClient.SendTextMessageAsync(chatId, "Добро пожаловать в бота!");
            }
            else if (message.Contains("/register"))
            {
                // Логика регистрации
                // Например: await RegisterUserAsync(chatId);
            }
            else if (message.Contains("/profile"))
            {
                // Логика просмотра профиля
                // Например: await ViewProfileAsync(chatId);
            }
            else
            {
                // Ответ по умолчанию для нераспознанных команд
                await _botClient.SendTextMessageAsync(chatId, "Извините, я не понял эту команду.");
            }
        }
    }
}
