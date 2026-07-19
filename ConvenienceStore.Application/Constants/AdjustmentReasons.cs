namespace ConvenienceStore.Application.Constants;

public static class AdjustmentReasons
{
    public const string Damaged = "Damaged";
    public const string Expired = "Expired";
    public const string Lost = "Lost";
    public const string Found = "Found";
    public const string Other = "Other";

    public static readonly string[] ValidReasons = { Damaged, Expired, Lost, Found, Other };

    public static bool IsValid(string reason)
    {
        return ValidReasons.Contains(reason);
    }
}
