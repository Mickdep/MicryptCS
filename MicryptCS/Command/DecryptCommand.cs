using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS.Command
{
    public class DecryptCommand : BaseCommand
    {
        public override string Text => "Decrypt";

        public override void Execute(Crypto crypto)
        {
            var filesToDecrypt = _fileEnumerator.EnumerateEncryptedFiles();
            if (filesToDecrypt.Count < 1)
            {
                Console.WriteLine("No files found to decrypt.");
                return;
            }

            string decryptionPassword = GetPasswordInput();
            Stopwatch stopWatch = Stopwatch.StartNew();
            int filesDecrypted = 0;
            Console.WriteLine();
            foreach (var filePath in filesToDecrypt)
            {
                byte[] fileContent = _fileReader.ReadFile(filePath);
                if (fileContent?.Length < 1)
                    return;

                var plainText = crypto.DecryptFileContent(fileContent, decryptionPassword);
                if (plainText?.Length > 0)
                {
                    _fileWriter.WritePlainTextToFile(plainText, filePath);
                    filesDecrypted++;
                    Utils.PrintConsoleSuccessTag();
                    Console.WriteLine("[" + filePath + "]");
                }
                else
                {
                    Utils.PrintConsoleFailTag();
                    Console.WriteLine("[" + filePath + "]");
                }
            }
            stopWatch.Stop();
            Console.WriteLine();
            Console.WriteLine("Decrypted {0}/{1} files.", filesDecrypted, filesToDecrypt.Count);
            Console.WriteLine("Time elapsed: " + stopWatch.Elapsed);
        }
    }
}
