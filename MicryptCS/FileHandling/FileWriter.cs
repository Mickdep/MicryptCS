using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS.FileHandling
{
    public class FileWriter
    {
        public void WriteEncryptedContentToFile(byte[] cipherText, string filePath)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(cipherText, 0, cipherText.Length);
                fileStream.Flush();
            }

            File.Move(filePath, filePath + Utils.EncryptionExtension);
        }

        public void WritePlainTextToFile(byte[] plainText, string filePath)
        {
            var originalFileName = filePath.Remove(filePath.LastIndexOf('.'), filePath.Length - filePath.LastIndexOf('.')); //Remove the last extension from the file.
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileStream.Write(plainText, 0, plainText.Length);
                fileStream.Flush();
            }

            File.Move(filePath, originalFileName);
        }
    }
}
