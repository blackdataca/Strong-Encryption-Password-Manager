using System.Security.Cryptography;
using System.Text;

namespace MyIdLibrary.Models;

public static class Crypto
{

    /// <summary>
    /// 
    /// </summary>
    /// <param name="plain"></param>
    /// <param name="key">256-bit/8, 32 bytes</param>
    /// <param name="iv">IV size is BlockSize / 8 = 128 / 8 = 16 bytes</param>
    /// <returns></returns>
    public static string SymmetricEncrypt(string plain, byte[] key, byte[] iv)
    {
        string crypt = "";
        var aes = Aes.Create();
        ICryptoTransform encryptor = aes.CreateEncryptor(key, iv);

        // Create the streams used for encryption.
        using (MemoryStream msEncrypt = new MemoryStream())
        {
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plain);
                }
                crypt = Convert.ToBase64String(msEncrypt.ToArray());
            }
        }
        return crypt;
    }

    public static byte[] AsymetricEncrypt(byte[] byteData, byte[] pubKey)
    {
        using (var rsa = RSA.Create())
        {
            int bytesRead;
            rsa.ImportRSAPublicKey(pubKey, out bytesRead);
            var encryptedData = rsa.Encrypt(byteData, RSAEncryptionPadding.OaepSHA256);
            return encryptedData;
            //return Convert.ToBase64String(encryptedData);
        }
    }
}
