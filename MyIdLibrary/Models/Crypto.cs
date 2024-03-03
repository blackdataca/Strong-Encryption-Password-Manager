using System.Security.Cryptography;
using System.Text;

namespace MyIdLibrary.Models;

public static class Crypto
{

    /// <summary>
    /// Encrypt - AES
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

    /// <summary>
    /// Decrypt - AES
    /// </summary>
    /// <param name="plain"></param>
    /// <param name="key">256-bit/8, 32 bytes</param>
    /// <param name="iv">IV size is BlockSize / 8 = 128 / 8 = 16 bytes</param>
    /// <returns></returns>
    public static string SymmetricDecrypt(string crypt, byte[] key, byte[] iv)
    {
        string plain = "";
        var aes = Aes.Create();
        ICryptoTransform cryptor = aes.CreateDecryptor(key, iv);
        byte[] cryptBytes = Convert.FromBase64String(crypt);
        // Create the streams used for encryption.
        using (MemoryStream ms = new MemoryStream(cryptBytes))
        {
            using (CryptoStream cs = new CryptoStream(ms, cryptor, CryptoStreamMode.Read))
            {
                using (var sCrypt = new StreamReader(cs))
                {
                    plain = sCrypt.ReadToEnd();
                }
            }
        }
        return plain;
    }

    /// <summary>
    /// Encrypt - RSA
    /// </summary>
    /// <param name="byteData"></param>
    /// <param name="pubKey"></param>
    /// <returns></returns>
    public static byte[] AsymetricEncrypt(byte[] byteData, byte[] pubKey)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportRSAPublicKey(pubKey, out int bytesRead);
            var encryptedData = rsa.Encrypt(byteData, RSAEncryptionPadding.OaepSHA256);
            return encryptedData;
        }
    }

    /// <summary>
    /// Decrypt - RSA
    /// </summary>
    /// <param name="byteData"></param>
    /// <param name="pubKey"></param>
    /// <returns></returns>
    public static byte[] AsymetricDecrypt(byte[] byteData, byte[] priKey)
    {
        using (var rsa = RSA.Create())
        {
            rsa.ImportRSAPrivateKey(priKey, out int bytesRead);
            var decryptedData = rsa.Decrypt(byteData, RSAEncryptionPadding.OaepSHA256);
            return decryptedData;
        }
    }
}
