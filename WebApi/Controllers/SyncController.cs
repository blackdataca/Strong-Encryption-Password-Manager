using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyIdLibrary.DataAccess;
using MyIdLibrary.Models;
using MyIdWeb.Data;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;


namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SyncController : ControllerBase
{
    private UserModel loggedInUser = new();


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

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // will give the user's userId

        var user = _userData.GetUser(userId);
        if (user?.Result is null)
        {
            return """{"Error":"Unauthorized!"}""";
        }
        using var sr = new StreamReader(Request.Body);
        
        string json = await sr.ReadToEndAsync();

        return """{"Error":"0"}"""; ;
    }
}
