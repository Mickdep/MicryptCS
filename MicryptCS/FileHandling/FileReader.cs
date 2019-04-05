using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS
{
    public class FileReader
    {
        /// <summary>
        /// Exposes a stream for the specified file.
        /// </summary>
        /// <returns>FileStream of the specified file</returns>
        public byte[] ReadFile(string fileLocation)
        {
            byte[] fileBytes = null;
            try
            {
                fileBytes = File.ReadAllBytes(fileLocation);
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("[ERROR] Unauthorized access.");
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine("[ERROR] Directory not found.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[ERROR] File not found.");
            }
            catch (Exception exception)
            {
                Console.WriteLine("[ERROR] Something went wrong when reading the file: " + exception.Message);
            }
            return fileBytes;
        }
    }
}
