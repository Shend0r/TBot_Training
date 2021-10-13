using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot_Training
{
    internal class Messenger
    {
        public string CreateTextMessage(Conversation Chat)
        {
            return Commands.Data.Answer(Chat.GetLastMessage());
        }
    }
}
