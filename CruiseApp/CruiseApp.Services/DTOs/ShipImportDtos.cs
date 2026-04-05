namespace CruiseApp.Services.Core.DTOs
{
    public class ShipImportDto
    {
        public string Name { get; set; } = null!;
        public List<DeckDto> Decks { get; set; } = new();
    }

    public class DeckDto
    {
        public int Number { get; set; }
        public string Image { get; set; } = null!;
        public List<CabinDto> Cabins { get; set; } = new();
    }

    public class CabinDto
    {
        public int SequenceNumber { get; set; }
        public string Type { get; set; } = null!;
        public int PosX { get; set; }
        public int PosY { get; set; }
    }

    public class RouteImportDto
    {
        public string ShipName { get; set; } = null!;
        public List<RouteDayDto> RouteDays { get; set; } = new();
    }

    public class RouteDayDto
    {
        public string Date { get; set; } = null!;
        public string Point { get; set; } = null!;
    }
}