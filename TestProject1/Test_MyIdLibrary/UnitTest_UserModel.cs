using MyIdLibrary.Models;
using System.Text;

namespace TestProject1.Test_MyIdLibrary;
[TestClass]
public class UnitTest_UserModel
{

    [TestMethod]
    public void Test_NewObject()
    {
        UserModel obj = new ();
        Assert.IsTrue(Guid.TryParse(obj.Id, out var id));
        Assert.IsNull(obj.GetPrivateKey());
    }

    [TestMethod]
    public void Test_PublicPrivateKeys()
    {
        UserModel obj = new();
        string pwd = "Some Random Password";
        obj.SetSecurityStamp(pwd);
        var pubKey = obj.PublicKey;
        var priKey = obj.GetPrivateKey();
        Assert.IsNotNull(pubKey);
        Assert.IsNotNull(priKey);
        Assert.AreEqual(pwd, obj.SecurityStamp);

        string plain = "Verify public key and private key encryption";
        var crypt = Crypto.AsymetricEncrypt(Encoding.UTF8.GetBytes(plain), Convert.FromBase64String( pubKey));
        Assert.IsNotNull(crypt);
        var uncryptB = Crypto.AsymetricDecrypt(crypt, priKey);
        Assert.IsNotNull(uncryptB);

        var uncrypt = Encoding.UTF8.GetString(uncryptB);
        Assert.AreEqual(plain, uncrypt);

    }

}
