using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;

namespace TelegramBot_Training.Commands.Events.EnglishTraining
{
    internal class AddWord : IStarter, IStager
    {
        private ITelegramBotClient botClient { get; set; }
        private Word temporaryData { get; set; }
        private AddingType addingType { get; set; }

        public AddWord(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            temporaryData = new Word();
        }

        async void IStarter.Start(Conversation chat, string EventName)
        {
            if (EventName == "e_addword")
            {
                var text = "Введите русское значение слова : ";

                await SendCommandText(text, chat.GetId());

                addingType = AddingType.Russian;
            }
        }

        async void IStager.NextStage(Conversation chat, string EventName)
        {
            if (EventName == "e_addword" && addingType != AddingType.End)
            {
                var text = "";

                switch (addingType)
                {
                    case AddingType.Russian:
                        temporaryData.Russian = chat.GetLastMessage();

                        text = "Введите английское значение слова : ";

                        await SendCommandText(text, chat.GetId());

                        addingType = AddingType.English;
                        break;

                    case AddingType.English:
                        temporaryData.English = chat.GetLastMessage();

                        text = "Введите тему : ";

                        await SendCommandText(text, chat.GetId());

                        addingType = AddingType.Theme;
                        break;

                    case AddingType.Theme:
                        temporaryData.Theme = chat.GetLastMessage();

                        SaveWord(chat, temporaryData.Russian, temporaryData.English, temporaryData.Theme);

                        addingType = AddingType.End;
                        break;
                }
            }
        }

        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }

        private async void SaveWord(Conversation chat, string RusText, string EngText, string Theme)
        {

            XDocument words_data = XDocument.Load("BotData\\WordsData.xml");
            XElement words = words_data.Element("words");
            XElement word = new XElement("word", new XAttribute("rus_text", RusText), new XAttribute("eng_text", EngText), new XAttribute("theme", Theme));

            bool IsMatch = false;

            foreach (XElement e_word in words_data.Element("words").Elements("word"))
            {
                if (e_word.Attribute("rus_text").Value == RusText)
                {
                    IsMatch = true;

                    await SendCommandText($"Слово {RusText} уже было добавлено в словарь.", chat.GetId());

                    break;
                }
            }

            if (IsMatch == false)
            {
                words.Add(word);
                words_data.Save($"BotData\\WordsData.xml");
                Data.Load();
            }
        }
    }
}
