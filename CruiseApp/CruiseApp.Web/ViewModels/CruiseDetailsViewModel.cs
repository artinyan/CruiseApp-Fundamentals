namespace CruiseApp.Web.ViewModels
{
    public class CruiseDetailsViewModel
    {
        // Identity
        public int Id { get; set; }

        // Main info
        public string ShipName { get; set; } = string.Empty;
        public int Nights { get; set; }
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }

        // Route
        public string StartPoint { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;
        public string Destinations { get; set; } = string.Empty;

        // Image
        public string StartPointImage { get; set; } = string.Empty;

        // Likes
        public bool IsLiked { get; set; }
        public int LikesCount { get; set; }

    }
}
