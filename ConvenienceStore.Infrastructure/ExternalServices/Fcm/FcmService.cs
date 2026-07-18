using ConvenienceStore.Application.Contracts.External;
using Microsoft.Extensions.Logging;

namespace ConvenienceStore.Infrastructure.ExternalServices.Fcm;

public class FcmService : IFcmService
{
    private readonly ILogger<FcmService> _logger;

    public FcmService(ILogger<FcmService> logger)
    {
        _logger = logger;
    }

    public Task SendPushNotificationAsync(string topic, string title, string body, Dictionary<string, string>? data = null)
    {
        // Mock FCM implementation
        _logger.LogInformation("FCM Push Sent -> Topic: {Topic}, Title: {Title}, Body: {Body}", topic, title, body);
        return Task.CompletedTask;
    }
}
