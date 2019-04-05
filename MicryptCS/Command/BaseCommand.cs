 using MicryptCS.FileHandling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS.Command
{
    public abstract class BaseCommand
    {

        protected readonly FileReader _fileReader;
        protected readonly FileWriter _fileWriter;
        protected readonly FileEnumerator _fileEnumerator;

        public abstract string Text { get; }
        public abstract void Execute(Crypto crypto);

        public BaseCommand()
        {
            _fileReader = new FileReader();
            _fileWriter = new FileWriter();
            _fileEnumerator = new FileEnumerator();
        }

        /// <summary>
        /// Prompts the user for a password to lock the file with.
        /// </summary>
        /// <returns>A string containing the password.</returns>
        protected string GetPasswordInput()
        {
            Console.WriteLine("Please enter a password to (un)lock the files with: ");
            var password = Console.ReadLine();
            while (string.IsNullOrEmpty(password) || password.Contains(" "))
            {
                Console.WriteLine("Password is invalid. Please try again.");
                password = Console.ReadLine();
            }

            return password;
        }
    }
}
