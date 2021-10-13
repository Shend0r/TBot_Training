using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;

namespace TelegramBot_Training.Commands.Events.EnglishTraining
{
    internal class Training : IStarter, ICheckAnswer, IStoper
    {
        private ITelegramBotClient botClient { get; set; }
        private TrainingType trainingType = TrainingType.End;
        private XDocument d_words { get; set; }
        private List<XElement> WordsList = new List<XElement>();
        private int randomValue { get; set; }

        public Training(ITelegramBotClient BotClient)
        {
            botClient = BotClient;
        }

        async void IStarter.Start(Conversation Chat, string EventName)
        {
            if (EventName == "e_rus_to_eng")
            {
                trainingType = TrainingType.RusToEng;
            }

            if (EventName == "e_eng_to_rus")
            {
                trainingType = TrainingType.EngToRus;
            }
            
            WordsList.RemoveAll(x => x != null);

            d_words = XDocument.Load("BotData\\WordsData.xml");

            foreach (XElement word in d_words.Element("words").Elements("word"))
            {
                WordsList.Add(word);
            }

            NextStage(Chat, EventName);
        }

        public async void NextStage(Conversation Chat, string EventName) // Без ISteger тк выше объявляется этот метод и что бы по 100 раз класс не объявлять, оставил так.
        {
            Random random = new Random();
            randomValue = random.Next(0, WordsList.Count);

            if (EventName == "e_rus_to_eng" && trainingType == TrainingType.RusToEng)
            {
                await SendCommandText("Напишите перевод слова : " + WordsList[randomValue].Attribute("rus_text").Value, Chat.GetId());
            }

            if (EventName == "e_eng_to_rus" && trainingType == TrainingType.EngToRus)
            {
                await SendCommandText("Напишите перевод слова : " + WordsList[randomValue].Attribute("eng_text").Value, Chat.GetId());
            }
        }

        public async void CheckAnswer(Conversation Chat, string EventName)
        {
            if (Chat.GetLastMessage() != "/rus_to_eng" || Chat.GetLastMessage() != "/eng_to_rus")
            {
                if (EventName == "e_rus_to_eng" && trainingType == TrainingType.RusToEng)
                {
                    if (WordsList[randomValue].Attribute("eng_text").Value == Chat.GetLastMessage())
                    {
                        await SendCommandText("Правильно!", Chat.GetId());
                    }
                    else
                    {
                        await SendCommandText($"Не правильно! Перевод слова {WordsList[randomValue].Attribute("rus_text").Value} - {WordsList[randomValue].Attribute("eng_text").Value}", Chat.GetId());
                    }
                }

                if (EventName == "e_eng_to_rus" && trainingType == TrainingType.EngToRus)
                {
                    if (WordsList[randomValue].Attribute("rus_text").Value == Chat.GetLastMessage())
                    {
                        await SendCommandText("Правильно!", Chat.GetId());
                    }
                    else
                    {
                        await SendCommandText($"Не правильно! Перевод слова {WordsList[randomValue].Attribute("eng_text").Value} - {WordsList[randomValue].Attribute("rus_text").Value}", Chat.GetId());
                    }
                }
            }
        }

        void IStoper.Stop(string EventName)
        {
            if (EventName == "e_stoptraining")
            {
                trainingType = TrainingType.End;
            }
        }

        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chatId: chat, text: text);
        }
    }
}
