using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS
{
    public class FileEnumerator
    {
        /// <summary>
        /// Loops through all unencrypted files and directories found in the folder.
        /// </summary>
        /// <returns>A list of filepaths</returns>
        public List<string> EnumerateFiles()
        {
            List<string> fileList = new List<string>();
            try
            {
                foreach (var filePath in Directory.GetFiles(Utils.FileFolder, "*.*", SearchOption.AllDirectories))
                {
                    if (Path.GetExtension(filePath) != Utils.EncryptionExtension)
                    {
                        fileList.Add(filePath);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("[ERROR] Unauthorized access.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("[ERROR] Directory not found.");
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Something went wrong when reading the files. Check if the directory exists");
            }
            return fileList;
        }

        /// <summary>
        /// Loops through all encrypted files and directories found in the folder
        /// </summary>
        /// <returns>A list of filepaths of encrypted files</returns>
        public List<string> EnumerateEncryptedFiles()
        {
            List<string> fileList = new List<string>();
            try
            {
                foreach (string filePath in Directory.GetFiles(Utils.FileFolder, "*.*", SearchOption.AllDirectories))
                {
                    if (Path.GetExtension(filePath) == Utils.EncryptionExtension)
                    {
                        fileList.Add(filePath);
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("[ERROR] Unauthorized access.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("[ERROR] Directory not found.");
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Something went wrong when reading the files. Check if the directory exists");
            }
            return fileList;
        }
    }
}
