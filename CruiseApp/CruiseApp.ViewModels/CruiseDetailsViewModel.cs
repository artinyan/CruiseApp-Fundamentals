namespace CruiseApp.ViewModels
{
    public class CruiseDetailsViewModel
    {
        public int Id { get; set; }

        public string ShipName { get; set; } = string.Empty;
        public int Nights { get; set; }
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }

        public string StartPoint { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public string Destinations { get; set; } = string.Empty;

        public string StartPointImage { get; set; } = string.Empty;

        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }

        public string? Description { get; set; }
    }
}
