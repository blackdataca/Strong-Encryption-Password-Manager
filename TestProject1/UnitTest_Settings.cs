using System.Configuration;

namespace TestProject1;

public class UnitTest_Settings
{
    protected string? configString;
    protected string? webApiUrl;
    public UnitTest_Settings()
    {
        ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
        string file = System.Reflection.Assembly.GetExecutingAssembly().Location;
        file = $"{file}.config";
        fileMap.ExeConfigFilename = file;
        Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

        configString = Environment.GetEnvironmentVariable("ConnectionString");
        if (string.IsNullOrWhiteSpace(configString))
            configString = config.ConnectionStrings.ConnectionStrings["Default"].ConnectionString;

        webApiUrl = Environment.GetEnvironmentVariable("webApiUrl");
        if (string.IsNullOrWhiteSpace(webApiUrl))
            webApiUrl = config.AppSettings.Settings["webApiUrl"].Value;
        if (webApiUrl.EndsWith("/"))
            webApiUrl = webApiUrl.Remove(webApiUrl.Length-1,1);
    }
}
