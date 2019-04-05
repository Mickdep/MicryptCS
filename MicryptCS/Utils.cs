using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS
{
    public class Utils
    {
        public const string EncryptionExtension = ".MICRYPT";
        public const ConsoleColor MainColor = ConsoleColor.Yellow;
        public const ConsoleColor SuccessColor = ConsoleColor.Green;
        public const ConsoleColor FailColor = ConsoleColor.Red;
        public static string FileFolder = "";

        public static void PrintConsoleFailTag()
        {
            Console.Write("[");
            Console.ForegroundColor = FailColor;
            Console.Write("FAIL");
            Console.ForegroundColor = MainColor;
            Console.Write("] ");
        }

        public static void PrintConsoleSuccessTag()
        {
            Console.Write("[");
            Console.ForegroundColor = SuccessColor;
            Console.Write("OK");
            Console.ForegroundColor = MainColor;
            Console.Write("] ");
        }
    }
}
