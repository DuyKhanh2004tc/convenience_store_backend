using System.ComponentModel.DataAnnotations;

namespace ConvenienceStore.Domain.Entities;

public class SystemSetting
{
    [Key]
    public string SettingKey { get; set; } = string.Empty;
    public string SettingValue { get; set; } = string.Empty;
    public string? Description { get; set; }
}
