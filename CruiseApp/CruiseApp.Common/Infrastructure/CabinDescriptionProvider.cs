using System.Text.Json;
using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Common.Infrastructure;

public static class CabinDescriptionProvider
{
    private static Dictionary<string, Dictionary<CabinType, string>> data
        = new();

    public static void Load(string filePath)
    {
        var json = File.ReadAllText(filePath);

        var raw = JsonSerializer.Deserialize<
            Dictionary<string, Dictionary<string, string>>>(json);

        data = raw!.ToDictionary(
            s => s.Key.ToLower(),
            s => s.Value.ToDictionary(
                c => Enum.Parse<CabinType>(c.Key),
                c => c.Value));
    }

    public static string Get(string shipName, CabinType type)
    {
        var ship = shipName.Replace(" ", "").ToLower();

        if (data.TryGetValue(ship, out var cabins) &&
            cabins.TryGetValue(type, out var description))
        {
            return description;
        }

        return "Comfortable cabin designed for a relaxing cruise experience.";
    }
}
