using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TelegramBot_Training
{
    internal class Conversation
    {
        private Chat telegramChat { get; set; }
        private List<Message> telegramMessages { get; set; }

        public Conversation(Chat Chat)
        {
            telegramChat = Chat;
            telegramMessages = new List<Message>();
        }

        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach (var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public long GetId() => telegramChat.Id;
        public string GetLastMessage() => telegramMessages[telegramMessages.Count - 1].Text;
    }
}
