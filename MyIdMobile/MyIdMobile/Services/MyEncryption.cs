using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static System.Net.Mime.MediaTypeNames;

namespace MyIdMobile.Services
{


    public class MyEncryption
    {
        public static string EncryptString(string plainText, string key, string iv)
        {
            var cipherBytes = EncryptData(Encoding.ASCII.GetBytes(plainText), Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(iv));
            // Convert the encrypted byte array to a base64 encoded string
            string cipherText = Convert.ToBase64String(cipherBytes, 0, cipherBytes.Length);

            // Return the encrypted data as a string
            return cipherText;

        }

        public static byte[] EncryptData(byte[] plainBytes, byte[] key, byte[] iv)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            //encryptor.Padding = PaddingMode.Zeros;
            byte[] key32;
            using (var shaManaged = new SHA256Managed())
            {
                key32 = shaManaged.ComputeHash(key);
            }

            // Set key and IV
            encryptor.Key = key32;

            byte[] iv16 = Md5(iv);


            encryptor.IV = iv16;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesEncryptor = encryptor.CreateEncryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesEncryptor, CryptoStreamMode.Write);

            // Convert the plainText string into a byte array
            //byte[] plainBytes = Encoding.ASCII.GetBytes(plainText);

            // Encrypt the input plaintext string
            cryptoStream.Write(plainBytes, 0, plainBytes.Length);

            // Complete the encryption process
            cryptoStream.FlushFinalBlock();

            // Convert the encrypted data from a MemoryStream to a byte array
            byte[] cipherBytes = memoryStream.ToArray();

            // Close both the MemoryStream and the CryptoStream
            memoryStream.Close();
            cryptoStream.Close();

            return cipherBytes;
        }


        public static string DecryptString(string cipherText, string key, string iv)
        {
            byte[] encData =  Convert.FromBase64String(cipherText);
            byte[] decData = DecryptData(encData, Encoding.ASCII.GetBytes(key), Encoding.ASCII.GetBytes(iv));
            return Encoding.ASCII.GetString(decData);
        }

        public static byte[] DecryptData(byte[] cipherBytes, byte[] key, byte[] iv)
        {
            // Instantiate a new Aes object to perform string symmetric encryption
            Aes encryptor = Aes.Create();

            encryptor.Mode = CipherMode.CBC;
            //encryptor.KeySize = 256;
            //encryptor.BlockSize = 128;
            //encryptor.Padding = PaddingMode.Zeros;
            byte[] key32;
            using (var shaManaged = new SHA256Managed())
            {
                key32 = shaManaged.ComputeHash(key);
            }

            // Set key and IV
            encryptor.Key = key32;

            byte[] iv16 =  MyEncryption.Md5(iv);


            encryptor.IV = iv16;

            // Instantiate a new MemoryStream object to contain the encrypted bytes
            MemoryStream memoryStream = new MemoryStream();

            // Instantiate a new encryptor from our Aes object
            ICryptoTransform aesDecryptor = encryptor.CreateDecryptor();

            // Instantiate a new CryptoStream object to process the data and write it to the 
            // memory stream
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aesDecryptor, CryptoStreamMode.Write);

            // Will contain decrypted plaintext
            //string plainText = String.Empty;
            byte[] plainBytes;
            try
            {
                // Convert the ciphertext string into a byte array
               // byte[] cipherBytes = Convert.FromBase64String(cipherText);

                // Decrypt the input ciphertext string
                cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                // Complete the decryption process
                cryptoStream.FlushFinalBlock();

                // Convert the decrypted data from a MemoryStream to a byte array
                 plainBytes = memoryStream.ToArray();

                // Convert the decrypted byte array to string
                //plainText = Encoding.ASCII.GetString(plainBytes, 0, plainBytes.Length);
            }
            finally
            {
                // Close both the MemoryStream and the CryptoStream
                memoryStream.Close();
                cryptoStream.Close();
            }

            // Return the decrypted data as a string
            return plainBytes;
        }

        public static string UniqId(string prefix, bool more_entropy)
        {
            if (string.IsNullOrEmpty(prefix))
                prefix = string.Empty;

            if (!more_entropy)
            {
                return (prefix + System.Guid.NewGuid().ToString().Replace("-", "")).Substring(0, 13);
            }
            else
            {
                return (prefix + System.Guid.NewGuid().ToString().Replace("-", "").Substring(0, 14)) + "." + System.Guid.NewGuid().ToString().Substring(0, 8);
            }
        }


        public static string MyHash(string input)
        {
            using (SHA512 shaManaged = new SHA512Managed())
            {
                byte[] hash = shaManaged.ComputeHash(Encoding.UTF8.GetBytes(input));
                string hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();

                return hashString;
            }
        }

        public static byte[] Md5(byte[] inputBytes)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                return hashBytes;

                //return Convert.ToHexString(hashBytes); // .NET 5 +

                // Convert the byte array to hexadecimal string prior to .NET 5
                // StringBuilder sb = new System.Text.StringBuilder();
                // for (int i = 0; i < hashBytes.Length; i++)
                // {
                //     sb.Append(hashBytes[i].ToString("X2"));
                // }
                // return sb.ToString();
            }
        }

        public static byte[] MyHash(byte[] input)
        {
            using (SHA512 shaManaged = new SHA512Managed())
            {
                byte[] hash = shaManaged.ComputeHash(input);
                return hash;
            }
        }

        public static string Bin2Hex(byte[] input)
        {
            return BitConverter.ToString(input).Replace("-", "").ToLower();
        }

        public static byte[] Hex2Bin(string hexData)
        {
            byte[] binData = new byte[hexData.Length / 2];
            for (int i = 0; i < hexData.Length; i += 2)
            {
                binData[i / 2] = Convert.ToByte(hexData.Substring(i, 2), 16);
            }
            return binData;
        }

        public static string UcFirst(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            char firstChar = input[0];
            string restOfTheString = input.Substring(1);

            // Convert the first character to upper case and concatenate with the rest of the string.
            return char.ToUpper(firstChar) + restOfTheString;
        }

        public static async Task SaveKeyIvAsync(string type, byte[] value)
        {

            switch (type)
            {
               
                case "Iv2022": //16
               
                case "Salt": //32
                    //Registry.SetValue("HKEY_CURRENT_USER\\Software\\MyId", type, value);
                    string hex = Bin2Hex(value);
                    await SecureStorage.SetAsync(type, hex);
                    break;
                
                case "Pin":
                    byte[] encPin = EncryptData(value, Encoding.ASCII.GetBytes("key"), Encoding.ASCII.GetBytes("iv"));
                    string encPinText = Bin2Hex(encPin);
                    await SecureStorage.SetAsync(type, encPinText);
                    
                    break;
                default:
                    throw new NotImplementedException($"{type}: {value}");

            }
            
        }

        private static byte[] GenerateRandomBytes(int size)
        {
            byte[] data = new byte[size];

            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            for (int i = 0; i < 10; i++)
            {
                // Fille the buffer with the generated data
                rng.GetBytes(data);
            }

            return data;
        }

        public static async Task CreateNewKeyAsync(byte[] masterPin)
        {
            byte[] salt = GenerateRandomBytes(32);
            await SaveKeyIvAsync("Salt", salt);
            var key = new Rfc2898DeriveBytes(masterPin, salt, 50000);

            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                myRijndael.KeySize = 256;
                myRijndael.BlockSize = 128;
                myRijndael.Padding = PaddingMode.PKCS7;
                //Cipher modes: http://security.stackexchange.com/questions/52665/which-is-the-best-cipher-mode-and-padding-mode-for-aes-encryption
                myRijndael.Mode = CipherMode.CFB;

                myRijndael.Key = key.GetBytes(32);//256 bits = 32 bytes

                myRijndael.GenerateIV();

                await SaveKeyIvAsync("Iv2022", myRijndael.IV); //128 blocksize / 8 = 16

               


            }
        }

        public static async Task<byte[]> GetKeyIvAsync(string type)
        {
            switch (type)
            {
                
                case "Iv2022": //16
 
                case "Salt":
                    {
                        string s = await SecureStorage.GetAsync(type);
                        byte[] data = Hex2Bin(s);
                        return data;
                    }
               
                case "Pin":
                    string value = await SecureStorage.GetAsync(type);
                    byte[] encPin = Hex2Bin(value);
                    return DecryptData(encPin, Encoding.ASCII.GetBytes( "key"), Encoding.ASCII.GetBytes("iv"));
                //return ProtectedData.Unprotect(_pinEnc, null, DataProtectionScope.CurrentUser);

                default:
                    throw new NotImplementedException($"[GetKeyIvAsync] {type}");
            }
        }


        public static byte[] ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }
            return bytes;
        }


    }

    

}