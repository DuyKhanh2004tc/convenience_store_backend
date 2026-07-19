namespace ConvenienceStore.Application.Contracts.External;

public interface IFcmService
{
    Task SendPushNotificationAsync(string topic, string title, string body, Dictionary<string, string>? data = null);
}
