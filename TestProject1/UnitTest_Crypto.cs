using MyIdLibrary.Models;
using System.Runtime.Versioning;
using System.Security.Cryptography;

namespace TestProject1;

[TestClass]
public class UnitTest_Crypto
{
    [TestMethod]
    public void Test_SymmetricEncryption()
    {
        // Given a message to encrypt
        var message = "Hello, world!";

       
        // Generate our encryption key
        var key = RandomNumberGenerator.GetBytes(16);

        // Generate our initialization vector (IV)
        var iv = RandomNumberGenerator.GetBytes(16);

        // Encrypt the data
        string encryptedData = Crypto.SymmetricEncrypt(message, key, iv);

        // Decrypt the data
        string decryptedData = Crypto.SymmetricDecrypt(encryptedData, key, iv);

        Assert.AreEqual(message, decryptedData);
    }

    [TestMethod]
    public void Test_AsymmetricEntryption()
    {
        // Given a message to encrypt
        string message = "Hello, world!";

        // Generate a new RSA key pair
        using (var rsa = new RSACryptoServiceProvider())
        {
            // Get the public key
            var publicKey = rsa.ExportRSAPublicKey();

            // Encrypt the data using the public key
            var encryptedData = Crypto.AsymetricEncrypt(System.Text.Encoding.UTF8.GetBytes( message), publicKey);
            Console.WriteLine(Convert.ToBase64String(encryptedData));

            var privateKey = rsa.ExportRSAPrivateKey();

            // Decrypt the data using the private key
            var decryptedData = Crypto.AsymetricDecrypt(encryptedData, privateKey);

            // Assert that the decrypted data matches the original message
            Assert.AreEqual(message, System.Text.Encoding.UTF8.GetString( decryptedData));
        }
    }

    [TestMethod]
    public void Test_UniqId()
    {
        int testSize = 1000000; //1M
        HashSet<string> uids = new();
        for (int i = 0; i < testSize; i++)
        {
            string uid = Crypto.UniqId("");
            Assert.AreEqual(23, uid.Length);
            Assert.IsTrue(uids.Add(uid));

            uid = Crypto.UniqId("12345");
            Assert.AreEqual(28, uid.Length);
            Assert.IsTrue(uids.Add(uid), $"Duplicated:{uid} found at {i}");
        }

        Assert.AreEqual(testSize * 2, uids.Count);
    }

    [TestMethod]
    public void Test_GenerateRandomPassword()
    {
        int testSize = 1000000; //1M
        HashSet<string> pwds = new();
        for (int i = 0; i < testSize; i++)
        {
            string pwd = Crypto.GenerateRandomPassword();
            Assert.IsTrue(pwd.Length >= 10, $"Length error:{pwd} found at {i}");
            Assert.IsTrue(pwds.Add(pwd), $"Duplicated:{pwd} found at {i}");
        }

        Assert.AreEqual(testSize, pwds.Count);
    }

    [TestMethod]
    [SupportedOSPlatform("windows")]
    public void Test_KeyIv()
    {
        //Save + Get Iv
        byte[] valueBefore = Crypto.GetKeyIv("Iv2022");

        byte[] valueIn = RandomNumberGenerator.GetBytes(16);
        Crypto.SaveKeyIv("Iv2022", valueIn);

        byte[] valueOut = Crypto.GetKeyIv("Iv2022");
        CollectionAssert.AreEqual(valueIn, valueOut);

        if (valueBefore is not null)
        {
            Crypto.SaveKeyIv("Iv2022", valueBefore);
            valueOut = Crypto.GetKeyIv("Iv2022");
            CollectionAssert.AreEqual(valueBefore, valueOut);
        }

        //Save + Get Salt
        byte[] saltBefore = Crypto.GetKeyIv("Salt");

        byte[] saltIn = RandomNumberGenerator.GetBytes(32);
        Crypto.SaveKeyIv("Salt", saltIn);

        byte[] saltOut = Crypto.GetKeyIv("Salt");
        CollectionAssert.AreEqual(saltIn, saltOut);

        if (saltBefore is not null)
        {
            Crypto.SaveKeyIv("Salt", saltBefore);
            saltOut = Crypto.GetKeyIv("Salt");
            CollectionAssert.AreEqual(saltBefore, saltOut);
        }

        //Save + Get Pin
        byte[] pinIn = RandomNumberGenerator.GetBytes(32);
        byte[] encPin = Crypto.SaveKeyIv("Pin", pinIn);

        byte[] pinOut = Crypto.GetKeyIv("Pin", encPin);
        CollectionAssert.AreEqual(pinIn, pinOut);
    }

    [TestMethod]
    public void Test_GenerateRandomBytes()
    {
        int testSize = 1000000; //1M
        HashSet<byte[]> rans = new();
        for (int i = 0; i < testSize; i++)
        {
            byte[] ran = Crypto.GenerateRandomBytes(32);
            Assert.AreEqual(32, ran.Length);
            Assert.IsTrue(rans.Add(ran));

            ran = Crypto.GenerateRandomBytes(64);
            Assert.AreEqual(64, ran.Length);
            Assert.IsTrue(rans.Add(ran));
        }

        Assert.AreEqual(testSize * 2, rans.Count);
    }

    [TestMethod]
    [SupportedOSPlatform("windows")]
    public void Test_FileStream()
    {
        byte[] pinPlain = RandomNumberGenerator.GetBytes(64);
        var pinCrypt = Crypto.SaveKeyIv("Pin", pinPlain);

        byte[] bytes = RandomNumberGenerator.GetBytes(1000000);
        var ms = new MemoryStream(bytes);
        string tempFileName = Path.GetTempFileName();
        string encFileNameOnly = Crypto.EncryptFileStream(tempFileName, ms, pinCrypt);

        KeyValuePair<string, string> encFile = new KeyValuePair<string, string>(encFileNameOnly, tempFileName);

        var decryptedMs = Crypto.DecryptFileStream(encFile, pinCrypt);
        var decryptedBytes = decryptedMs.ToArray();

        CollectionAssert.AreEqual(bytes, decryptedBytes);

        string encFileFullName = Path.Combine(KnownFolders.DataDir, encFileNameOnly);
        File.Delete(encFileFullName);
        File.Delete(tempFileName);
    }
}
