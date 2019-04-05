using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace MicryptCS
{
    public class Crypto
    {
        private const int SaltSizeBytes = 32;
        private const int IVSizeBytes = 16;
        private const int PBKDF2Iterations = 10000;
        private const int HMACSizeBytes = 32;
        private const int KeySizeBytes = 32;

        /// <summary>
        /// Encrypts a file's content and returns the ciphertext.
        /// </summary>
        /// <param name="fileContent">Array of bytes containing the file content</param>
        /// <param name="password">Password to derive the encryption key from</param>
        public byte[] EncryptFileContent(byte[] fileContent, string password)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                byte[] salt = GenerateRandomSalt();

                //Generate keys for encryptiong and HMAC
                var keys = GenerateKeys(password, salt);
                var encryptionKey = keys.Item1; //First 32 bytes
                var hmacKey = keys.Item2; //Last 32 bytes

                aes.Key = encryptionKey;
                aes.GenerateIV();

                byte[] cipherText = null;

                //Use memorystream because we need the ciphertext in memory for generating an HMAC.
                using (var memoryStream = new MemoryStream())
                {
                    memoryStream.Write(salt, 0, salt.Length);
                    memoryStream.Write(aes.IV, 0, aes.IV.Length);
                    //CryptoStream writes the encrypted content to memoryStream
                    using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(fileContent, 0, fileContent.Length);
                    }
                    cipherText = memoryStream.ToArray();
                }

                //Create HMAC and concat it to the complete ciphertext.
                var hmac = new HMACSHA256(hmacKey).ComputeHash(cipherText);
                cipherText = cipherText.Concat(hmac).ToArray();

                return cipherText;
            }
        }

        /// <summary>
        /// Decrypts encrypted file content and returns the plaintext.
        /// </summary>
        /// <param name="fileContent">Array of bytes containing the file content</param>
        /// <param name="password">Password to derive the encryption key from</param> 
        public byte[] DecryptFileContent(byte[] fileContent, string password)
        {
            using (var aes = new AesCryptoServiceProvider())
            {
                //Extract all necessary data
                byte[] hmac = ExtractHMAC(fileContent);
                byte[] salt = ExtractSalt(fileContent);
                byte[] IV = ExtractIV(fileContent);
                byte[] cipherText = ExtractCipherText(fileContent);

                //Generate keys for encryptiong and HMAC
                var keys = GenerateKeys(password, salt);
                var encryptionKey = keys.Item1; //First 32 bytes
                var hmacKey = keys.Item2; //Last 32 bytes

                //Calculate and compare HMAC
                var valueToHmac = salt.Concat(IV).Concat(cipherText).ToArray();
                var calculatedHmac = new HMACSHA256(hmacKey).ComputeHash(valueToHmac);
                var hmacIsValid = hmac.SequenceEqual(calculatedHmac);

                aes.Key = encryptionKey;
                aes.IV = IV;

                byte[] plainText = null;
                if (hmacIsValid)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(cipherText, 0, cipherText.Length);
                        }
                        plainText = memoryStream.ToArray();
                    }
                }

                return plainText;
            }
        }

        /// <summary>
        /// Generates a random salt using the RNGCryptoServiceProvider
        /// </summary>
        /// <returns>An array of non-zero bytes</returns>
        private byte[] GenerateRandomSalt()
        {
            //Salt of 256 bits
            byte[] salt = new byte[SaltSizeBytes];

            //Generate salt using (pseudo)random bytes
            using (var random = new RNGCryptoServiceProvider())
            {
                random.GetNonZeroBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Extracts the HMAC from the file content.
        /// </summary>
        /// <param name="fileContent">Byte array containing the file's content</param>
        /// <returns>A byte array representing the HMAC</returns>
        private byte[] ExtractHMAC(byte[] fileContent)
        {
            var offset = fileContent.Length - HMACSizeBytes;
            byte[] hmac = new byte[HMACSizeBytes];

            for (int i = 0; i < hmac.Length; i++)
            {
                hmac[i] = fileContent[offset + i];
            }

            return hmac;
        }

        /// <summary>
        /// Derives 512 bytes from a password and a salt
        /// </summary>
        /// <param name="password">Password to derive the keys from</param>
        /// <param name="salt">Random 256 bits salt</param>
        /// <returns>A Tuple of two byte arrays (Encryption key, HMAC key)</returns>
        private Tuple<byte[], byte[]> GenerateKeys(string password, byte[] salt)
        {
            //Use password derivation function PBKDF2 with 10.000 iterations (1000 is default)
            Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(password, salt, PBKDF2Iterations, HashAlgorithmName.SHA512);

            //Get 64 bytes (512 bits) from the derived key. A 256 bits key is required for AES. The other 256 bits are used as the key for the HMAC.
            byte[] key = rfc.GetBytes(KeySizeBytes * 2);
            var encryptionKey = key.Take(KeySizeBytes).ToArray();
            var hmacKey = key.Skip(KeySizeBytes).Take(KeySizeBytes).ToArray();

            return Tuple.Create(encryptionKey, hmacKey);
        }

        /// <summary>
        /// Retrieves the salt from the encrypted file
        /// </summary>
        /// <param name="fileContent">An array of bytes containing the file's content</param>
        /// <returns>An array of bytes(32) containing a salt</returns>
        private byte[] ExtractSalt(byte[] fileContent)
        {
            byte[] salt = new byte[SaltSizeBytes];
            //Get the salt from the encrypted file content
            for (int i = 0; i < salt.Length; i++)
            {
                salt[i] = fileContent[i];
            }

            return salt;
        }

        /// <summary>
        /// Retrieves the initialization vector from the encrypted file
        /// </summary>
        /// <param name="fileContent">An array of bytes containing the cipher text</param>
        /// <returns>An array of bytes(16) containing an initialization vector</returns>
        private byte[] ExtractIV(byte[] fileContent)
        {
            var offset = SaltSizeBytes;
            byte[] IV = new byte[IVSizeBytes];
            //Get the initialization vector from the encrypted file content
            for (int i = 0; i < IV.Length; i++)
            {
                IV[i] = fileContent[i + offset];
            }

            return IV;
        }

        /// <summary>
        /// Retrieves the cipher text from an encrypted file
        /// </summary>
        /// <param name="fileContent">An array of bytes containing the cipher text</param>
        /// <returns>An array of bytes containing the encrypted content</returns>
        private byte[] ExtractCipherText(byte[] fileContent)
        {
            var cipherTextSize = fileContent.Length - SaltSizeBytes - IVSizeBytes - HMACSizeBytes;
            var offset = SaltSizeBytes + IVSizeBytes;
            byte[] cipherText = new byte[cipherTextSize];

            for (int i = 0; i < cipherText.Length; i++)
            {
                cipherText[i] = fileContent[offset + i];
            }

            return cipherText;
        }

    }
}