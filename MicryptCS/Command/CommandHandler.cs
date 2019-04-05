using MicryptCS.FileHandling;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS.Command
{
    public class CommandHandler
    {

        private readonly Crypto _crypto;
        private Dictionary<int, BaseCommand> _commands;

        public CommandHandler()
        {
            InitializeCommands();
            PrintCommands();

            _crypto = new Crypto();
        }

        /// <summary
        /// Handles commands from the user
        /// </summary>
        public void HandleCommand()
        {
            var input = Console.ReadLine();
            while (!CommandIsValid(input))
            {
                Console.WriteLine("Command unavailable. Try again.");
                input = Console.ReadLine();
            }

            //If this code is reached the command is assumed to be valid.
            int commandId = Convert.ToInt32(input);
            if (_commands.TryGetValue(commandId, out BaseCommand command))
                command.Execute(_crypto);
            else
                Console.WriteLine("Command unavailable. Try again.");

            PrintCommands();
        }

        /// <summary>
        /// Initializes the list of commands available.
        /// </summary>
        private void InitializeCommands()
        {
            _commands = new Dictionary<int, BaseCommand> {
                { 1, new EncryptCommand() },
                { 2, new DecryptCommand() },
                { 3, new QuitCommand() }
            };
        }

        /// <summary>
        /// Prints the commands to the console.
        /// </summary>
        private void PrintCommands()
        {
            Console.WriteLine();
            Console.WriteLine("Available commands: ");
            foreach (var command in _commands)
            {
                Console.WriteLine($"{command.Key}) {command.Value.Text}");
            }
        }

        /// <summary>
        /// Checks if a command is valid
        /// </summary>
        /// <param name="input">Contains the command</param>
        /// <returns>Boolean value that indicates whether the command is valid or not</returns>
        private bool CommandIsValid(string input)
        {
            //If string is empty or contains whitespace: return
            if (string.IsNullOrEmpty(input) || input.Contains(" "))
            {
                return false;
            }

            return int.TryParse(input, out int command_value);
        }
    }
}

