using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests.Abstractions;
using System.Diagnostics;

namespace Telegrambot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //This string to server console
            Console.WriteLine("I am a telegram bot!");

            //Create an object for connecting with the Telegramm API
            //Use BotFather in Telegramm to create new Bot and to obtain the token to access the HTTP API:
            var client = new TelegramBotClient("7481963222:AAG_SCwrVu5Azcs62TdciU-jy7uMvkklDHk");

            //Start telegramm client
            client.StartReceiving(Update, Error, null);

            //for do not exit
            Console.ReadLine();
        }

        // Errors processing
        private static async Task Error(ITelegramBotClient client, Exception exception, CancellationToken token)
        {

        }

        // Processing of events of change state 
        private static async Task Update(ITelegramBotClient botClient, Update update, CancellationToken token)
        {
            //get a message from update object
            var message = update.Message;

            //if input message is a text type
            if (message.Text != null)
            {
                //out the message to server console
                Console.WriteLine($"{message.Chat.FirstName}  |  {message.Text}  ");
                
                //a reaction of text "hello" in message
                if (message.Text.ToLower().Contains("hello"))
                {
                    //send to bot text message
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Hello Friend!");
                    return;
                }
            }

            //if input message is photo type file
            if (message.Photo != null)
            {
                //send to bot text message
                await botClient.SendTextMessageAsync(message.Chat.Id, "Your image is good. But better send me" +
                    " a docement!");
                return;
            }

            //if message is document type file
            if (message.Document != null)
            {
                //obtain recieved file ID from bot
                var filId = message.Document.FileId;
                //obtain file info
                var fileInfo = await botClient.GetFileAsync(filId);
                //obtain file path
                var filePath = fileInfo.FilePath;
                //obtain file name
                var fileName = message.Document.FileName;
                if (fileName.Contains(".jpg"))
                {
                    //file path for destination file
                    //const string destinationFilePath = //"../downloaded.file";
                    string destinationFilePath =
                        $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/{message.Document.FileName}";

                    //open destination file for write
                    Stream fileStream = System.IO.File.OpenWrite(destinationFilePath);

                    //wait while source file from bot write into destination file on server desktop
                    await botClient.DownloadFileAsync(filePath, fileStream);

                    //you need close to be written file on 
                    fileStream.Close();

                    //Process received file in server for some aims
                    //Process.Start(@"C:\Users\Admin\Desktop\MovieStar.exe", $@"""{destinationFilePath}""");
                    //Task.Delay(1500);

                    //open processed file
                    Stream stream = System.IO.File.OpenRead(destinationFilePath);

                    //send file to bot
                    await botClient.SendDocumentAsync(message.Chat.Id, document: InputFile.FromStream(stream, message.Document.FileName.Replace(".jpg", " (edited).jpg")),
                        caption: "This picture was improved!\n Have a pleasure.");
                    //you need to close it
                    stream.Close();
                }
                //echange files through the telegramm bot
                if (fileName.Contains(".txt"))
                {
                    //file path for destination file
                    //const string destinationFilePath = //"../downloaded.file";
                    string destinationFilePath =
                        $@"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/{message.Document.FileName}";

                    //open destination file for write
                    Stream fileStream = System.IO.File.OpenWrite(destinationFilePath);

                    //wait while source file from bot write into destination file on server desktop
                    await botClient.DownloadFileAsync(filePath, fileStream);

                    //you need close to be written file on 
                    fileStream.Close();

                    //Process received file in server for some aims
                    //Process.Start(@"C:\Users\Admin\Desktop\MovieStar.exe", $@"""{destinationFilePath}""");
                    //Task.Delay(1500);

                    //open processed file
                    Stream stream = System.IO.File.OpenRead(destinationFilePath);

                    //send file to bot
                    await botClient.SendDocumentAsync(message.Chat.Id, document: InputFile.FromStream(stream, message.Document.FileName.Replace(".txt", " (edited).txt")),
                        caption: "This your keas!\n Have a pleasure.");
                    //you need to close it
                    stream.Close();
                }
            }            
        }
    }
}
