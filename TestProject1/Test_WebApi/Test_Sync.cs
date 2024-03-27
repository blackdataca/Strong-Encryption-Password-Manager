using Azure;
using MyIdLibrary.DataAccess;
using MyIdLibrary.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace TestProject1.Test_WebApi;

[TestClass]
public class Test_Sync : UnitTest_Settings
{
    [TestMethod]
    public async Task Test_SyncApi()
    {
        if (configString == "No Github DB Yet")
            return;

        DbConnection db = new(configString);
        SqlUserData userData = new(db);

        var mainForm = new MyId.MainForm();
        mainForm.IdList.Add(new IdItem() { Site = "Test", User = "Test", Password = "Test", Memo = "Test" });
        var dataString = mainForm.PrepareSyncData(null);
        using var compressedData = Crypto.CompressString(dataString);


        using HttpClient client = new HttpClient();

        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var form = new Dictionary<string, string>()
        {
            { "email" , "UnitTest@test.com" },
            { "password" , Crypto.GenerateRandomPassword() }
        };

        UserModel user = new UserModel();
        user.Name = form["email"];

        var json = JsonConvert.SerializeObject(form);
        var body = new StringContent(json, Encoding.UTF8, "application/json");

        var registerResponse = await client.PostAsync($"{webApiUrl}/register", body).ConfigureAwait(false);
        //if (registerResponse.IsSuccessStatusCode)
        //{
        var aspnetUser = await userData.FindAspNetUserIdAsync(user.Name);
        user.Id = aspnetUser.Id;
        user.SetSecurityStamp(aspnetUser.SecurityStamp);
        Assert.IsTrue(await userData.CreateUserAsync(user));
        //}
        //Ignore 400 DuplicateUserName Username 'UnitTest@test.com' is already taken.

        HttpResponseMessage tokenResponse = await client.PostAsync($"{webApiUrl}/login", body);
        string? token = null;

        if (tokenResponse.IsSuccessStatusCode)
        {
            var jsonContent = await tokenResponse.Content.ReadAsStringAsync();
            var jobj = JsonConvert.DeserializeObject<JObject>(jsonContent);
            token = jobj?["accessToken"]?.ToString();
        }
        Assert.IsNotNull(token);

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpContent httpContent = new StreamContent(compressedData);
        // Set the content type (replace "application/octet-stream" with the appropriate content type for your binary data)
        httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        httpContent.Headers.ContentEncoding.Add("gzip");
        client.DefaultRequestHeaders.AcceptEncoding.ParseAdd("gzip");
        client.DefaultRequestHeaders
          .Accept
          .Add(new MediaTypeWithQualityHeaderValue("application/json"));//ACCEPT header
        HttpResponseMessage res = await client.PutAsync("https://localhost:7283/Sync", httpContent);
        var response = await res.Content.ReadAsStringAsync();
        JObject joResponse = JObject.Parse(response);
        Assert.IsNotNull(joResponse);
        Assert.AreEqual("0", joResponse?["Error"]?.ToString());

        //TODO test sync data


        Assert.IsTrue(await userData.DeleteTempUser(user));

    }
}
