using MicryptCS.Command;
using System;
using System.Diagnostics;
using System.IO;

namespace MicryptCS
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                CheckFileFolder(args[0]);
            }
            else
            {
                Console.WriteLine("Please supply a folder path. Usage: Micrypt [folderpath]");
                ExitApplication();
            }

            //Startup
            InitializeConsole();
            PrintBanner();
            PrintInformation();

            var commandHandler = new CommandHandler();

            //Run loop
            while (true)
            {
                commandHandler.HandleCommand();
            }
        }

        private static void CheckFileFolder(string fileFolderPath)
        {
            //Console.WriteLine(fileFolderPath);
            if (Directory.Exists(fileFolderPath))
            {
                Utils.FileFolder = fileFolderPath;
            }
            else
            {
                Console.WriteLine("The given folder path could not be found.");
                ExitApplication();
            }
        }

        private static void InitializeConsole()
        {
            Console.Title = "Micrypt";
            Console.ForegroundColor = Utils.MainColor;
        }

        private static void PrintBanner()
        {
            Console.WriteLine("------------------------------------------");
            Console.WriteLine("|   __  __ _                       _     |");
            Console.WriteLine("|  |  \\/  (_)                     | |    |");
            Console.WriteLine("|  | \\  / |_  ___ _ __ _   _ _ __ | |_   |");
            Console.WriteLine("|  | |\\/| | |/ __| '__| | | | '_ \\| __|  |");
            Console.WriteLine("|  | |  | | | (__| |  | |_| | |_) | |_   |");
            Console.WriteLine("|  |_|  |_|_|\\___|_|   \\__, | .__/ \\__|  |");
            Console.WriteLine("|                       __/ | |          |");
            Console.WriteLine("|                      |___/|_|          |");
            Console.WriteLine("------------------------------------------");
        }

        private static void PrintInformation()
        {
            Console.WriteLine("==============================================================================");
            Console.WriteLine("Encryption using AES-CBC-256 with HMAC-SHA-256.");
            Console.WriteLine("Encrypted files have the .MICRYPT extension.");
            Console.WriteLine("All files placed inside the given folder will be encrypted.");
            Console.WriteLine("Changing and/or deleting any of the files inside of this folder will result in the loss of that file.");
            Console.WriteLine("Encrypting files in: " + Utils.FileFolder);
            Console.WriteLine("==============================================================================");
        }

        private static void ExitApplication()
        {
            Environment.Exit(0);
        }
    }
}
