using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot_Training.Commands.Events
{
    internal interface IEvent
    {
        void Start(Conversation chat, string EventName);
    }
}
