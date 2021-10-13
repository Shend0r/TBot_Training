using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;

namespace TelegramBot_Training.Commands.Events.EnglishTraining
{
    internal class Dictionary : IStarter
    {
        private ITelegramBotClient botClient { get; set; }

        public Dictionary(ITelegramBotClient BotClient)
        {
            botClient = BotClient;
        }

        async void IStarter.Start(Conversation Chat, string EventName)
        {
            if (EventName == "e_dictionary")
            {
                string data = "Словарь :\n";

                XDocument d_words = XDocument.Load("BotData\\WordsData.xml");

                foreach (XElement word in d_words.Element("words").Elements("word"))
                {
                    data += $" {word.Attribute("rus_text").Value} - {word.Attribute("eng_text").Value} - {word.Attribute("theme").Value};\n";
                }

                await SendCommandText(data, Chat.GetId());
            }
            
        }

        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }
    }
}
