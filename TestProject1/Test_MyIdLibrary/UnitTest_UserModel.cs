using MyIdLibrary.DataAccess;
using MyIdLibrary.Models;
using Newtonsoft.Json;
using System.Text;

namespace TestProject1.Test_MyIdLibrary;
[TestClass]
public class UnitTest_UserModel : UnitTest_Settings
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

    [TestMethod]
    public async Task Test_User_CRUD()
    {
        if (configString == "No Github DB Yet")
            return;

        UserModel user = new UserModel();
        user.Name = "UnitTest";
        user.SetSecurityStamp("Randome Password");

        DbConnection db = new(configString);
        SqlUserData userData = new(db);
        Assert.IsTrue(await userData.CreateUserAsync(user));

        var anotherUser = await userData.GetUserAsync(user.Id);

        Assert.IsNotNull(anotherUser);

        Assert.AreEqual(user.Name, anotherUser.Name);

        anotherUser.Name = "New Name";
        await userData.UpdateUserAsync(anotherUser);

        user = await userData.GetUserAsync(anotherUser.Id);

        Assert.AreEqual(user.Name, anotherUser.Name);

        Assert.IsTrue(await userData.DeleteTempUser(user));
    }

}
