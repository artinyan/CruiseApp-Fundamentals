namespace CruiseApp.Services.Models.Cruise;

public class CabinsServiceModel
{
    public int CruiseId { get; set; }

    public string ShipName { get; set; } = string.Empty;

    public string StartPoint { get; set; } = string.Empty;

    public DateOnly FirstDay { get; set; }

    public DateOnly LastDay { get; set; }

    public int Nights { get; set; }

    public List<CabinCardServiceModel> Cabins { get; set; } = new();
}