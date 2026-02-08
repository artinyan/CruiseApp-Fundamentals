namespace CruiseApp.Web.ViewModels
{
    public class CruiseListItemViewModel
    {
        // ============================
        // Identity
        // ============================
        public int Id { get; set; }

        // ============================
        // Display data
        // ============================
        public string ShipName { get; set; } = string.Empty;
        public string RouteName {  get; set; } = string.Empty;
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }

        /// <summary>
        /// Nights count (hotel / cruise standard)
        /// </summary>
        public int Nights { get; set; }

        // ============================
        // Ports
        // ============================
        public string StartPoint { get; set; } = string.Empty;
        public string Destinations { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;

        public string PointImage => $"{StartPoint.ToLower()}.jpg";

        // ============================
        // Future extensions
        // ============================

        public bool IsLiked { get; set; }    // logged users only
    }
}
