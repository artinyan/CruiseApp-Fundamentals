namespace CruiseApp.ViewModels
{
    public class CruiseListItemViewModel
    {
        public int Id { get; set; }

        public string ShipName { get; set; } = string.Empty;
        public string RouteName {  get; set; } = string.Empty;
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }

        public int Nights { get; set; }

        public string StartPoint { get; set; } = string.Empty;
        public string Destinations { get; set; } = string.Empty;
        public string EndPoint { get; set; } = string.Empty;

        public string PointImage => $"{StartPoint.ToLower()}.jpg";

        public bool IsLiked { get; set; }    // logged users only
    }
}
