namespace CruiseApp.Services.Models.Cruise;

public class CabinsServiceModel
{
    public int CruiseId { get; set; }

    public string ShipName { get; set; } = string.Empty;

    public List<CabinCardServiceModel> Cabins { get; set; } = new();
}