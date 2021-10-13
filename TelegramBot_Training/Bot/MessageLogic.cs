using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot_Training.Bot
{
    internal class MessageLogic
    {
        private ITelegramBotClient botClient { get; set; }
        private Conversation Chat { get; set; }
        private Messenger messanger { get; set; }
        private Commands.Events.EnglishTraining.AddWord addWord { get; set; }
        private Commands.Events.EnglishTraining.Dictionary dictionary { get; set; }
        private Commands.Events.EnglishTraining.DeleteWord deleteWord { get; set; }
        private Commands.Events.EnglishTraining.Training training { get; set; }
        private Dictionary<long, Conversation> chatList { get; set; }
        private string eventName { get; set; }

        public MessageLogic(ITelegramBotClient botClient)
        {
            messanger = new Messenger();
            chatList = new Dictionary<long, Conversation>();
            this.botClient = botClient;
            addWord = new Commands.Events.EnglishTraining.AddWord(botClient);
            dictionary = new Commands.Events.EnglishTraining.Dictionary(botClient);
            deleteWord = new Commands.Events.EnglishTraining.DeleteWord(botClient);
            training = new Commands.Events.EnglishTraining.Training(botClient);
        }

        public async Task Response(MessageEventArgs e)
        {
            var ID = e.Message.Chat.Id;

            if (!chatList.ContainsKey(e.Message.Chat.Id))
            {
                var newChat = new Conversation(e.Message.Chat);
                chatList.Add(ID, newChat);
            }

            Chat = chatList[ID];

            Chat.AddMessage(e.Message);

            if (Commands.Data.IsCommand(Chat.GetLastMessage()) == true)
            {
                if (Commands.Data.IsTextCommand(Chat.GetLastMessage()) == true)
                {
                    if (Commands.Data.IsTextEvent(Chat.GetLastMessage()) == false)
                    {
                        await SendTextMessage(Chat);
                    }
                    else
                    {
                        eventName = Commands.Data.TextEventName(Chat.GetLastMessage());
                        ((Commands.Events.IStarter)addWord).Start(Chat, eventName);
                        ((Commands.Events.IStarter)dictionary).Start(Chat, eventName);
                        ((Commands.Events.IStarter)deleteWord).Start(Chat, eventName);
                        ((Commands.Events.IStarter)training).Start(Chat, eventName);
                        ((Commands.Events.IStoper)training).Stop(eventName);
                    }
                }

                if (Commands.Data.IsButtonCommand(Chat.GetLastMessage()) == true)
                {
                    if (Commands.Data.IsButtonEvent(Chat.GetLastMessage()) == false)
                    {
                        await SendTextWithKeyBoard(Chat, Commands.Data.ButtonDescription(Chat.GetLastMessage()), ReturnKeyBoard(Commands.Data.ButtonsID(Chat.GetLastMessage())));
                    }
                    else
                    {
                        eventName = Commands.Data.ButtonEventName(Chat.GetLastMessage());
                        await SendTextWithKeyBoard(Chat, Commands.Data.ButtonDescription(Chat.GetLastMessage()), ReturnKeyBoard(Commands.Data.ButtonsID(Chat.GetLastMessage())));
                    }
                }
            }
            else
            {
                ((Commands.Events.IStager)addWord).NextStage(Chat, eventName);
                ((Commands.Events.IStager)deleteWord).NextStage(Chat, eventName);
                training.CheckAnswer(Chat, eventName);
                training.NextStage(Chat, eventName);
            }

        }

        private async Task SendTextMessage(Conversation Chat)
        {
            var text = messanger.CreateTextMessage(Chat);

            await botClient.SendTextMessageAsync(chatId: Chat.GetId(), text: text);
        }

        /// <summary>
        /// Получает клавиатуру с кнопками
        /// </summary>
        /// <param name="ButtonsID"></param>
        /// <returns>Возвращает InlineKeyboardMarkup</returns>
        public InlineKeyboardMarkup ReturnKeyBoard(string ButtonsID)
        {
            var buttonsID = ButtonsID.Split(';');

            List<InlineKeyboardButton> buttonList = new List<InlineKeyboardButton>();

            InlineKeyboardButton button = new InlineKeyboardButton();

            foreach (string buttonID in buttonsID)
            {
                button = new InlineKeyboardButton
                {
                    Text = Commands.Data.ButtonName(buttonID),

                    CallbackData = buttonID
                };

                buttonList.Add(button);
            }

            InlineKeyboardMarkup keyboard = new InlineKeyboardMarkup(buttonList);

            return keyboard;
        }

        private async Task SendTextWithKeyBoard(Conversation Chat, string Message, InlineKeyboardMarkup Keyboard)
        {
            await botClient.SendTextMessageAsync(chatId: Chat.GetId(), text: Message, replyMarkup: Keyboard);
        }
    }
}
