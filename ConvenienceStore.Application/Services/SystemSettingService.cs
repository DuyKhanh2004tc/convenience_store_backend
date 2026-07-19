using ConvenienceStore.Application.Contracts.Persistence;
using ConvenienceStore.Application.DTOs;
using ConvenienceStore.Application.Interfaces;
using ConvenienceStore.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConvenienceStore.Application.Services
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly IGenericRepository<SystemSetting> _settingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public SystemSettingService(IGenericRepository<SystemSetting> settingRepository, IUnitOfWork unitOfWork)
        {
            _settingRepository = settingRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<SystemSettingDto>> GetAllSettingsAsync()
        {
            var settings = await _settingRepository.GetAllAsync();
            return settings.Select(s => new SystemSettingDto
            {
                SettingKey = s.SettingKey,
                SettingValue = s.SettingValue,
                Description = s.Description
            }).ToList();
        }

        public async Task<SystemSettingDto?> GetSettingAsync(string key)
        {
            var settings = await _settingRepository.GetAsync(s => s.SettingKey == key);
            var setting = settings.FirstOrDefault();
            if (setting == null) return null;

            return new SystemSettingDto
            {
                SettingKey = setting.SettingKey,
                SettingValue = setting.SettingValue,
                Description = setting.Description
            };
        }

        public async Task UpdateSettingsAsync(Dictionary<string, string> settingsToUpdate)
        {
            foreach (var kvp in settingsToUpdate)
            {
                var existingSettings = await _settingRepository.GetAsync(s => s.SettingKey == kvp.Key);
                var existingSetting = existingSettings.FirstOrDefault();
                
                if (existingSetting != null)
                {
                    existingSetting.SettingValue = kvp.Value;
                    _settingRepository.Update(existingSetting);
                }
                else
                {
                    await _settingRepository.AddAsync(new SystemSetting
                    {
                        SettingKey = kvp.Key,
                        SettingValue = kvp.Value
                    });
                }
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
