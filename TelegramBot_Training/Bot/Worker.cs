using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace TelegramBot_Training.Bot
{
    internal class Worker
    {
        private ITelegramBotClient botClient;
        private MessageLogic botLogic;

        public void Inizalize()
        {
            botClient = new TelegramBotClient(Credentials.BotToken);
            botLogic = new MessageLogic(botClient);
        }

        public void Start()
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.OnCallbackQuery += Bot_Callback;
            botClient.StartReceiving();
        }

        public void Stop()
        {
            botClient.StopReceiving();
        }

        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message != null)
            {
                await botLogic.Response(e);
            }
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = Commands.Data.ButtonAnswer(e.CallbackQuery.Data);

            await botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text);
            await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }
    }
}
