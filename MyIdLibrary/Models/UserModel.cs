using System.Security.Cryptography;
using System.Text;

namespace MyIdLibrary.Models;

public class UserModel
{
    public string Id { get; set; } //ObjectIdentifier
    public string Name { get; set; }
    public string Email { get; set; }

    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }

    public string SecurityStamp { get; private set; }
    public int Status { get; set; }
    public int Perference { get; set; }
    public int IdleTimeout { get; set; }
    public DateTime LastActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Expiry { get; set; }

    public void SetSecurityStamp(string newSecurityStamp)
    {
        if (string.IsNullOrWhiteSpace(SecurityStamp))
        {
            //Generate an asymmetric public/private key pair.  
            RSA rsa = RSA.Create();
            //Save the public key information to an RSAParameters structure.  
            var pubKey = rsa.ExportRSAPublicKey();
            PublicKey = Convert.ToBase64String(pubKey);

            //Symmetric encrypt private key
            using (var sha256 = SHA256.Create())
            {
                var aes = Aes.Create();
                //key size 256 bits (32 bytes)
                var newSecurityStampHash = sha256.ComputeHash(Encoding.ASCII.GetBytes(newSecurityStamp));
                aes.Key = newSecurityStampHash;
                //IV size is BlockSize / 8 = 128 / 8 = 16 bytes
                string subId = Id.Replace("-", "").Substring(0, 16);
                aes.IV = Encoding.ASCII.GetBytes(subId);

                var priKey = rsa.ExportRSAPrivateKey();
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(priKey);
                        }
                        PrivateKey = Convert.ToBase64String(msEncrypt.ToArray());
                    }
                }
            }
        }
        else
        {
            //TODO follow User changed password
        }
        SecurityStamp = newSecurityStamp;
    }
}
