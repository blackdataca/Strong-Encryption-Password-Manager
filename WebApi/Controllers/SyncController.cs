using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdLibrary.DataAccess;
using MyIdLibrary.Models;
using MyIdWeb.Data;
using Newtonsoft.Json;
using System.Linq.Expressions;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;


namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SyncController : ControllerBase
{

    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<SyncController> _logger;
    private readonly ISecretData _secretData;
    private readonly IUserData _userData;

    public SyncController(ILogger<SyncController> logger, ISecretData secretData, IUserData userData)
    {
        _logger = logger;
        _secretData = secretData;
        _userData = userData;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    [Authorize]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPut]
    [Authorize]
    [Produces("text/plain")]
    public async Task<string> PutAsync()
    {
        Response.Headers.Remove("Content-Encoding");
        Response.Headers.TryAdd("Content-Encoding", "gzip");

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

        //1. Get authenticated user
        var user = _userData.GetUser(userId).Result;
        if (user is null)
        {
            _logger.LogWarning($"[PutAsync] user not found: {userId}");
            return """{"Error":"Unauthorized!"}""";
        }

        _logger.LogInformation($"[PutAsync] user found: {user.Email}");

        //2. Clear all synced flags
        await _secretData.ClearSyncFlagsAsync(user);
        _logger.LogDebug($"[PutAsync] Sync flags cleared");

        //3. Sync from incoming data: Update, Add, or No Change. If server is newer, leave synced null.
        using var sr = new StreamReader(Request.Body);
        string s = await sr.ReadToEndAsync();
        dynamic? vm = JsonConvert.DeserializeObject(s);
        var returnObject = new List<SecretModel>();
        int newCnt = 0;
        int updateCnt = 0;

        if (vm is not null && vm.Count is not null)
        {
            _logger.LogInformation( $"[PutAsync] incoming {vm.Count} records");
            for (int i = 0; i < (int)vm.Count; i++)
            {
                var rec = vm.Payloads[i];
                string recId = rec.RecId;
                string payload = rec.Payload;
                IdItem recItem = null;
                try
                {
                    recItem = JsonConvert.DeserializeObject<IdItem>(payload);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex.ToString());
                }
                if (recItem is null)
                {
                    _logger.LogWarning($"{recId} Invalid payload: {payload}");
                    continue;
                }
                if (string.IsNullOrWhiteSpace(recId))
                    recId = Guid.NewGuid().ToString();

                DateTime appTime = rec.LastUpdate;
                appTime = DateTime.SpecifyKind(appTime, DateTimeKind.Utc);
                SecretModel secret = await _secretData.FindSecretAsync(recId, user);
                if (secret is not null)
                { //row exists on server
                    DateTime dbTime = DateTime.SpecifyKind(secret.Modified, DateTimeKind.Utc);
                    if (appTime > dbTime)
                    { //app is newer, update server record, mark synced
                        secret.Synced = DateTime.UtcNow;
                        secret.Modified = appTime;
                        secret.Deleted = recItem.Deleted; 
                        secret.Payload = payload;
                        updateCnt++;
                        if (await _secretData.UpdateSecret(secret, user))
                            _logger.LogDebug($"{recId} Server secret updated");
                        else
                            _logger.LogWarning($"{recId} Server secret update failed");

                        if (recItem.Images?.Count > 0)
                        {
                            //app is newer send back record, may need file upload
                            secret.Payload = payload; //send back unencrypted payload
                            returnObject.Add(secret);
                        }
                    }
                    else if (appTime == dbTime)
                    {//No update, mark synced
                        secret.Synced = DateTime.UtcNow;
                        if (!await _secretData.UpdateSecret(secret, user))
                            //_logger.LogDebug( "Server secert synced");
                        //else
                            _logger.LogWarning($"{recId} Server secret synced failed");
                    }
                    else
                    { //server is newer, will send to app

                        _logger.LogDebug($"{recId} Server {dbTime} is newer than app {appTime}");
                    }

                }
                else
                {  //row does not exist on server, create server record
                    secret = new();
                    secret.Id = Guid.NewGuid();
                    secret.RecordId = recId;
                    secret.Payload = payload;
                    secret.Modified = appTime;
                    secret.Synced = DateTime.UtcNow;
                    secret.UserIds.Add(user.Id);
                    if (await _secretData.CreateSecret(secret, user))
                    {
                        newCnt++;
                        _logger.LogDebug($"{secret.Id} Server secret created");

                        if (recItem.Images?.Count> 0)
                        {
                            secret.Payload = payload; //send back unencrypted payload
                            returnObject.Add(secret);
                        }
                    }
                    else
                        _logger.LogWarning($"{secret.Id} Server secret create failed");
                }


            }
        }

        _logger.LogInformation($"New: {newCnt} Updated: {updateCnt}");

        //4. Get all synced is null records (server is newer)

        var secrets = await _secretData.GetUserSecretsAsync(user, true);
        returnObject.AddRange(secrets);

        var res = new
        {
            Error = "0",
            Return = returnObject
        };

        string resJson = JsonConvert.SerializeObject(res);
        return resJson;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PostAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

        //1. Get authenticated user
        var user = _userData.GetUser(userId).Result;
        if (user is null)
        {
            _logger.LogWarning($"[PostAsync] user not found: {userId}");
            return Unauthorized();
        }

        _logger.LogInformation($"[PostAsync] user found: {user.Email}");
        return Ok();
    }
}
