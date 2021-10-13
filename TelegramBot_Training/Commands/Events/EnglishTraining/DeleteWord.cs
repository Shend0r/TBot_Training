using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;

namespace TelegramBot_Training.Commands.Events.EnglishTraining
{
    internal class DeleteWord : IStarter, IStager
    {
        private ITelegramBotClient botClient { get; set; }
        private DeleteType deleteType = DeleteType.End;

        public DeleteWord(ITelegramBotClient BotClient)
        {
            botClient = BotClient;
        }

        async void IStarter.Start(Conversation Chat, string EventName)
        {
            if (EventName == "e_deleteword")
            {
                string text = "Введите слово для удаления :";

                await SendCommandText(text, Chat.GetId());

                deleteType = DeleteType.Waiting;
            }

        }

        async void IStager.NextStage(Conversation Chat, string EventName)
        {
            if (EventName == "e_deleteword" && deleteType == DeleteType.Waiting)
            {
                XDocument d_words = XDocument.Load("BotData\\WordsData.xml");

                bool IsMatch = false;
                string text = $"Слово {Chat.GetLastMessage()} было удалено из словаря.";

                foreach (XElement word in d_words.Element("words").Elements("word"))
                {
                    if (word.Attribute("rus_text").Value == Chat.GetLastMessage() || word.Attribute("eng_text").Value == Chat.GetLastMessage())
                    {
                        IsMatch = true;

                        word.Remove();

                        await SendCommandText(text, Chat.GetId());
                    }
                }

                d_words.Save("BotData\\WordsData.xml");

                Data.Load();

                if (IsMatch == false)
                {
                    text = $"Слово {Chat.GetLastMessage()} не найдено.";
                    await SendCommandText(text, Chat.GetId());
                }

                deleteType = DeleteType.End;
            }
        }

        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }
    }
}
