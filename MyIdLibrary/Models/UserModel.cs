using System.Security.Cryptography;
using System.Text;

namespace MyIdLibrary.Models;

public class UserModel
{
    public string Id { get; set; } //ObjectIdentifier
    public string Name { get; set; }
    public string Email { get; set; }

    /// <summary>
    /// Base64 encoded RSA public key
    /// </summary>
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }

    public byte[] GetPrivateKey()
    {
        using (var sha256 = SHA256.Create())
        {
            var key = sha256.ComputeHash(Encoding.ASCII.GetBytes(SecurityStamp));
            string subId = Id.Replace("-", "").Substring(0, 16);
            byte[] iv = Encoding.ASCII.GetBytes(subId);
            string priKey = Crypto.SymmetricDecrypt(PrivateKey, key, iv);
            return Convert.FromBase64String(priKey);
        }
    }
    public string SecurityStamp { get; private set; }
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
                //var aes = Aes.Create();
                //key size 256 bits (32 bytes)
                byte[] key = sha256.ComputeHash(Encoding.ASCII.GetBytes(newSecurityStamp));
                //IV size is BlockSize / 8 = 128 / 8 = 16 bytes
                string subId = Id.Replace("-", "").Substring(0, 16);
                byte[] iv = Encoding.ASCII.GetBytes(subId);

                var priKey = rsa.ExportRSAPrivateKey();
                string privateKey = Convert.ToBase64String(priKey);

                PrivateKey = Crypto.SymmetricEncrypt(privateKey, key, iv);
            }
        }
        else
        {
            //TODO follow User changed password
        }
        SecurityStamp = newSecurityStamp;
    }

    public int Status { get; set; }
    public int Perference { get; set; }
    public int IdleTimeout { get; set; }
    public DateTime LastActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime Expiry { get; set; }

    
}
