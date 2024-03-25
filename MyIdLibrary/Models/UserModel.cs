using System.Security.Cryptography;
using System.Text;

namespace MyIdLibrary.Models;

public class UserModel
{
    public string Id { get; set; } = Guid.NewGuid().ToString();//ObjectIdentifier
    public string Name { get; set; }
    public string Email { get; set; }

    /// <summary>
    /// Base64 encoded RSA public key
    /// </summary>
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }

    public byte[] GetPrivateKey()
    {
        var key = SHA256.HashData(Encoding.ASCII.GetBytes(SecurityStamp));
        string subId = Id.Replace("-", "").Substring(0, 16);
        byte[] iv = Encoding.ASCII.GetBytes(subId);
        string base64PrivateKey = Crypto.SymmetricDecrypt(PrivateKey, key, iv);
        return Convert.FromBase64String(base64PrivateKey);
    }
    private static string EncryptPrivateKey(string newSecurityStamp, byte[] priKey, string id)
    {
        //key size 256 bits (32 bytes)
        byte[] key = SHA256.HashData(Encoding.ASCII.GetBytes(newSecurityStamp));
        //IV size is BlockSize / 8 = 128 / 8 = 16 bytes
        string subId = id.Replace("-", "").Substring(0, 16);
        byte[] iv = Encoding.ASCII.GetBytes(subId);

        string base64PrivateKey = Convert.ToBase64String(priKey);

        return Crypto.SymmetricEncrypt(base64PrivateKey, key, iv);
    }
    public string SecurityStamp { get; private set; }
    public void SetSecurityStamp(string newSecurityStamp)
    {
        if (string.IsNullOrWhiteSpace(SecurityStamp))
        {
            //Create user

            // 1. Create asymmetric Public Key and Private Key 
            RSA rsa = RSA.Create();

            // 2. Save Public Key in -> users.public_key 
            var pubKey = rsa.ExportRSAPublicKey();
            PublicKey = Convert.ToBase64String(pubKey);

            // 3.Symmetric encrypt Private Key with user's password + users.uuid as salt -> users.private_key(encrypted)
            var priKey = rsa.ExportRSAPrivateKey();

            PrivateKey = EncryptPrivateKey(newSecurityStamp, priKey, Id);
        }
        else if (SecurityStamp != newSecurityStamp)
        {
            //User changed password
            byte[] priKey = GetPrivateKey();

            //Symmetric encrypt private key
            PrivateKey = EncryptPrivateKey(newSecurityStamp, priKey, Id);

        }
        SecurityStamp = newSecurityStamp;
    }

    public int Status { get; set; }
    public int Perference { get; set; }
    public int IdleTimeout { get; set; }
    public DateTime LastActive { get; set; }
    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }
    public DateTime? Expiry { get; set; } 


    public static UserModel CreateTempUser()
    {
        UserModel user = new UserModel();
        string newStamp = BitConverter.ToString(RandomNumberGenerator.GetBytes(32)).Replace("-", "");
        user.SetSecurityStamp(newStamp);

        return user;
    }

    
}
