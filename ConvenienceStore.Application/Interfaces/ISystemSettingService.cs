using ConvenienceStore.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConvenienceStore.Application.Interfaces
{
    public interface ISystemSettingService
    {
        Task<List<SystemSettingDto>> GetAllSettingsAsync();
        Task<SystemSettingDto?> GetSettingAsync(string key);
        Task UpdateSettingsAsync(Dictionary<string, string> settings);
    }
}
