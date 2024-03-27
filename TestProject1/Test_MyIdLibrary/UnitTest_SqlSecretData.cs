using MyIdLibrary.DataAccess;
using MyIdLibrary.Models;
using Newtonsoft.Json;
using System.Configuration;

namespace TestProject1.Test_MyIdLibrary;
[TestClass]
public class UnitTest_SqlSecretData  : UnitTest_Settings
{
    [TestMethod]
    public async Task Test_Secret_CRUD()
    {
        if (configString == "No Github DB Yet")
            return;

        SecretModel secret = new();
        IdItem idItem = new() { Site = "Test Site", User = "Test User", Password = "Test Password", Memo = "Test Memo" };
        secret.Payload = JsonConvert.SerializeObject(idItem);
 
        UserModel user = new();
        user.Name="UnitTest";
        user.SetSecurityStamp("Randome Password");

        DbConnection db = new(configString);
        SqlUserData userData = new(db);
        Assert.IsTrue(await userData.CreateUserAsync(user)); 

        SqlSecretData secretData = new(db);
        Assert.IsTrue( await secretData.CreateSecretAsync(secret, user));
        
        var secretRead = await secretData.ReadSecretAsync(secret.Id.ToString(), user);
        Assert.AreEqual(secret.Payload, secretRead.Payload);

        Assert.IsTrue(await secretData.UpdateSecretAsync(secret, user));

        Assert.IsTrue(await secretData.ClearSyncFlagsAsync(user));

        Assert.IsTrue(await secretData.DeleteSecretAsync(true, secret, user));

        Assert.IsTrue(await userData.DeleteTempUser(user));
    }

    [TestMethod]
    public async Task Test_CreateSharedSecret_CRUD()
    {
        if (configString == "No Github DB Yet")
            return;

        UserModel tempUser = new();
        tempUser.Name = "my user";
        tempUser.SetSecurityStamp("Randome Password");

        DbConnection db = new(configString);
        SqlUserData userData = new(db);
        Assert.IsTrue(await userData.CreateUserAsync(tempUser));

        UserModel newUser = new();
        newUser.Name = "new user";
        newUser.SetSecurityStamp("Randome Password");

        Assert.IsTrue(await userData.CreateUserAsync(newUser));

        SecretModel secret = new();
        IdItem idItem = new() { Site = "Test Site", User = "Test User", Password = "Test Password", Memo = "Test Memo" };
        secret.Payload = JsonConvert.SerializeObject(idItem);
        SqlSecretData secretData = new(db);
        Assert.IsTrue(await secretData.CreateSecretAsync(secret, tempUser));

        Assert.IsTrue( await secretData.CreateSharedSecretAsync(secret, tempUser, newUser));

        Assert.IsTrue(await secretData.DeleteSecretAsync(true, secret, newUser)); //remove secrets_users link

        Assert.IsTrue(await secretData.DeleteSecretAsync(true, secret, tempUser)); //mark for deletion

        Assert.IsTrue(await userData.DeleteTempUser(tempUser));

        Assert.IsFalse(await secretData.DeleteSecretAsync(true, secret, newUser)); //secrets_users link already removed 

        Assert.IsTrue(await userData.DeleteTempUser(newUser));
    }
}
