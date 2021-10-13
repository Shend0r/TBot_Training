using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramBot_Training.Commands.Events
{
    internal interface IStager
    {
        void NextStage(Conversation Char, string EventName);
    }
}
