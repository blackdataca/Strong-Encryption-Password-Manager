using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using MyIdLibrary.DataAccess;
using MyIdLibrary.Models;
using System.Configuration;
using System.Net.Sockets;

namespace TestProject1.Test_MyIdLibrary;
[TestClass]
public class UnitTest_SqlSecretData
{
    private string? configString;

    public UnitTest_SqlSecretData()
    {
        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
        string file = System.Reflection.Assembly.GetExecutingAssembly().Location;
        file = $"{file}.config";
        fileMap.ExeConfigFilename = file;
        System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

        configString = Environment.GetEnvironmentVariable("ConnectionString");
        if (string.IsNullOrWhiteSpace(configString))
            configString = config.ConnectionStrings.ConnectionStrings["Default"].ConnectionString;
    }
    [TestMethod]
    public async Task Test_CreateDeleteSecret()
    {
        if (configString == "No Github DB Yet")
            return;

        SecretModel secret = new();
        UserModel user = new();
        user.Name="UnitTest";
        user.SetSecurityStamp("Randome Password");
        DbConnection db = new(configString);
        SqlUserData userData = new(db);
        await userData.CreateUserAsync(user); 
        SqlSecretData secretData = new(db);
        Assert.IsTrue( await secretData.CreateSecretAsync(secret, user));

        await secretData.DeleteSecret(secret, user);

        await userData.DeleteTempUser(user);
    }
}
