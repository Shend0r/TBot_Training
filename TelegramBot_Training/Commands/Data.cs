using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TelegramBot_Training.Commands
{
    internal class Data
    {
        /// <summary>
        /// Список текстовых команд
        /// </summary>
        private static List<XElement> TextCommandsList { get; set; }
        /// <summary>
        /// Сиписок "кнопочных команд"
        /// </summary>
        private static List<XElement> ButtonCommandsList { get; set; }
        /// <summary>
        /// Список кнопок
        /// </summary>
        private static List<XElement> ButtonsList { get; set; }

        /// <summary>
        /// Загружает данные из документов XML
        /// </summary>
        public static void Load()
        {
            XDocument botCommandsList = XDocument.Load("BotData\\BotCommands.xml");

            TextCommandsList = new List<XElement>();
            ButtonCommandsList = new List<XElement>();
            ButtonsList = new List<XElement>();

            foreach (XElement text_command in botCommandsList.Element("commands").Elements("text_command"))
            {
                TextCommandsList.Add(text_command);
            }

            foreach (XElement button_command in botCommandsList.Element("commands").Elements("button_command"))
            {
                ButtonCommandsList.Add(button_command);
            }

            foreach (XElement button in botCommandsList.Element("commands").Elements("buttons").Elements("button"))
            {
                ButtonsList.Add(button);
            }
        }

        /// <summary>
        /// Получение ответа текстовой команды.
        /// </summary>
        /// <param name="Trigger">Команда</param>
        /// <returns>Возвращает string.</returns>
        public static string Answer(string Trigger)
        {
            var return_answer = "";

            Load();

            foreach (XElement text_command in TextCommandsList)
            {
                if (text_command.Attribute("trigger").Value == Trigger)
                {
                    return_answer = text_command.Attribute("answer").Value;
                }
            }

            return return_answer;
        }

        /// <summary>
        /// Получает ID кнопок из команды типа button_command
        /// </summary>
        /// <param name="Trigger">Команда</param>
        /// <returns>Список ID через символ ";"</returns>
        public static string ButtonsID(string Trigger)
        {
            var return_button_data = "";

            Load();

            bool IsMatch = false;

            foreach (XElement button_command in ButtonCommandsList)
            {
                if (button_command.Attribute("trigger").Value == Trigger)
                {
                    return_button_data = button_command.Attribute("answer").Value;                    
                    IsMatch = true;
                }
            }

            return return_button_data;
        }

        /// <summary>
        /// Получает ответ кнопочной команды
        /// </summary>
        /// <param name="Trigger">Команда</param>
        /// <returns>Возвращает string.</returns>
        public static string ButtonDescription(string Trigger)
        {
            var return_description = "";

            Load();

            foreach (XElement button_command in ButtonCommandsList)
            {
                if (button_command.Attribute("trigger").Value == Trigger)
                {
                    return_description = button_command.Attribute("description").Value;
                }
            }

            return return_description;
        }

        /// <summary>
        /// Получает текст кнопки
        /// </summary>
        /// <param name="ButtonID">ID кнопки</param>
        /// <returns>Возвращает string.</returns>
        public static string ButtonName(string ButtonID)
        {
            var return_text = "";

            Load();

            foreach (XElement button_text in ButtonsList)
            {
                if (button_text.Attribute("id").Value == ButtonID)
                {
                    return_text = button_text.Attribute("text").Value;
                }
            }

            return return_text;
        }

        /// <summary>
        /// Получает ответ кнопочной команды
        /// </summary>
        /// <param name="ButtonID">ID кнопки</param>
        /// <returns>Возвращает string.</returns>
        public static string ButtonAnswer(string ButtonID)
        {
            var return_answer = "";

            Load();

            foreach (XElement button_text in ButtonsList)
            {
                if (button_text.Attribute("id").Value == ButtonID)
                {
                    return_answer = button_text.Attribute("answer").Value;
                }
            }

            return return_answer;
        }

        /// <summary>
        /// Получает ответ о принадлежности Trigger (Команды) к Текстовой команде.
        /// </summary>
        /// <param name="Trigger">Команда</param>
        /// <returns>true - если команда текстовая, false - если команда не текстовая</returns>
        public static bool IsTextCommand(string Trigger)
        {
            var return_boolean = false;

            Load();

            foreach (XElement text_command in TextCommandsList)
            {
                if (text_command.Attribute("trigger").Value == Trigger)
                {
                    return_boolean = true;
                }
            }

            return return_boolean;
        }

        /// <summary>
        /// Получает ответ о принадлежности Trigger (Команды) к "Кнопочной" команде.
        /// </summary>
        /// <param name="Trigger">Команда</param>
        /// <returns>true - если команда "кнопочная", false - если команда не "кнопочная"</returns>
        public static bool IsButtonCommand(string Trigger)
        {
            var return_boolean = false;

            Load();

            foreach (XElement button_command in ButtonCommandsList)
            {
                if (button_command.Attribute("trigger").Value == Trigger)
                {
                    return_boolean = true;
                }
            }

            return return_boolean;
        }

        /// <summary>
        /// Получает ответ о принадлежности Trigger (Команды) к командам.
        /// </summary>
        /// <param name="Trigger">Команда</param>
        /// <returns>true - если команда, false - если команда нет</returns>
        public static bool IsCommand(string Trigger)
        {
            char command_symbol = Convert.ToChar("/");
            char[] converted_command = Trigger.ToCharArray();

            if (converted_command[0] == command_symbol)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Получает ответ о том, Текстовой ли это ивент.
        /// </summary>
        /// <param name="Trigger"></param>
        /// <returns>true - текстовой ивент, false - не текстовой ивент.</returns>
        public static bool IsTextEvent(string Trigger)
        {
            Load();

            var return_boolean = false;

            foreach (XElement text_command in TextCommandsList)
            {
                if (text_command.Attribute("trigger").Value == Trigger)
                {
                    if (bool.Parse(text_command.Attribute("is_event").Value) == true)
                    {
                        return_boolean = true;
                    }
                    break;
                }
            }

            return return_boolean;
        }

        /// <summary>
        /// Получает ответ о том, "Кнопочный" ли это ивент.
        /// </summary>
        /// <param name="Trigger"></param>
        /// <returns>true - "кнопочный" ивент, false - не "кнопочный" ивент.</returns>
        public static bool IsButtonEvent(string Trigger)
        {
            Load();

            var return_boolean = false;

            foreach (XElement button in ButtonsList)
            {
                if (button.Attribute("trigger").Value == Trigger)
                {
                    if (bool.Parse(button.Attribute("is_event").Value) == true)
                    {
                        return_boolean = true;
                    }
                    break;
                }
            }

            return return_boolean;
        }

        /// <summary>
        /// Получает имя текстового ивента.
        /// </summary>
        /// <param name="Trigger"></param>
        /// <returns>Возвращает string.</returns>
        public static string TextEventName(string Trigger)
        {
            Load();

            var return_name = "";

            foreach (XElement text_command in TextCommandsList)
            {
                if (text_command.Attribute("trigger").Value == Trigger)
                {
                    if (bool.Parse(text_command.Attribute("is_event").Value) == true)
                    {
                        return_name = text_command.Attribute("event_name").Value;
                    }
                    break;
                }
            }

            return return_name;
        }

        /// <summary>
        /// Получает имя "кнопочного" ивента.
        /// </summary>
        /// <param name="Trigger"></param>
        /// <returns>Возвращает string.</returns>
        public static string ButtonEventName(string Trigger)
        {
            Load();

            var return_name = "";

            foreach (XElement button in ButtonsList)
            {
                if (button.Attribute("trigger").Value == Trigger)
                {
                    if (bool.Parse(button.Attribute("is_event").Value) == true)
                    {
                        return_name = button.Attribute("event_name").Value;
                    }
                    break;
                }
            }

            return return_name;
        }
    }
}
