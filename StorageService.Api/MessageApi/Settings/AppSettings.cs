namespace StorageService.Api.MessageApi.Settings;

public class AppSettings
{
    public string FilePath { get; set; }

    public AppSettingsRabbitMq RabbitMq { get; set; }
}