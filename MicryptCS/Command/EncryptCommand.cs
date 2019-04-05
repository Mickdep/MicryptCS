using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS.Command
{
    public class EncryptCommand : BaseCommand
    {
        public override string Text => "Encrypt";

        public override void Execute(Crypto crypto)
        {
            var filesToEncrypt = _fileEnumerator.EnumerateFiles();
            if (filesToEncrypt.Count < 1)
            {
                Console.WriteLine("No files found to encrypt.");
                return;
            }

            string encryptionPassword = GetPasswordInput();
            Stopwatch stopWatch = Stopwatch.StartNew();
            int filesEncrypted = 0;
            Console.WriteLine();
            foreach (var filePath in filesToEncrypt)
            {
                byte[] fileContent = _fileReader.ReadFile(filePath);
                if (fileContent?.Length < 1)
                    return;

                var encryptedContent = crypto.EncryptFileContent(fileContent, encryptionPassword);
                if (encryptedContent?.Length > 0)
                {
                    _fileWriter.WriteEncryptedContentToFile(encryptedContent, filePath);
                    filesEncrypted++;
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
            Console.WriteLine("Encrypted {0}/{1} files.", filesEncrypted, filesToEncrypt.Count);
            Console.WriteLine("Time elapsed: " + stopWatch.Elapsed);
        }
    }
}
