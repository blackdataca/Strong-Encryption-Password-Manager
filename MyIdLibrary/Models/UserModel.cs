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
            //Create user
            // 1. Create asymmetric Public Key and Private Key 
            RSA rsa = RSA.Create();

            // 2. Save Public Key in -> users.public_key 
            var pubKey = rsa.ExportRSAPublicKey();
            PublicKey = Convert.ToBase64String(pubKey);

            // 3.Symmetric encrypt Private Key with user's password + users.uuid as salt -> users.private_key(encrypted)
            var priKey = rsa.ExportRSAPrivateKey();

            PrivateKey = EncryptPrivateKey(newSecurityStamp, priKey,Id);
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

    private static string EncryptPrivateKey(string newSecurityStamp, byte[] priKey, string id)
    {
        //var aes = Aes.Create();
        //key size 256 bits (32 bytes)
        byte[] key = SHA256.HashData(Encoding.ASCII.GetBytes(newSecurityStamp));
        //IV size is BlockSize / 8 = 128 / 8 = 16 bytes
        string subId = id.Replace("-", "").Substring(0, 16);
        byte[] iv = Encoding.ASCII.GetBytes(subId);

        string privateKey = Convert.ToBase64String(priKey);

        return Crypto.SymmetricEncrypt(privateKey, key, iv);
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

        // 1. Create asymmetric Public Key and Private Key 
        RSA rsa = RSA.Create();

        // 2. Save Public Key in -> users.public_key 
        var pubKey = rsa.ExportRSAPublicKey();
        user.PublicKey = Convert.ToBase64String(pubKey);

        // 3.Symmetric encrypt Private Key with user's password + users.uuid as salt -> users.private_key(encrypted)
        var priKey = rsa.ExportRSAPrivateKey();
        user.SecurityStamp = BitConverter.ToString(RandomNumberGenerator.GetBytes(32)).Replace("-","");
        user.Id = Guid.NewGuid().ToString();
       
        user.PrivateKey = EncryptPrivateKey(user.SecurityStamp, priKey, user.Id);
        return user;
    }
}
