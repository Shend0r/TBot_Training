// See https://aka.ms/new-console-template for more information

var telegramBot = new TelegramBot_Training.Bot.Worker();

telegramBot.Inizalize();
telegramBot.Start();

Console.WriteLine("Напишите /stop для прекращения работы");

string command;

do
{
    command = Console.ReadLine();

} while (command != "/stop");

telegramBot.Stop();
