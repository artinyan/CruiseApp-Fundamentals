namespace CruiseApp.Web.ViewModels
{
    /// <summary>
    /// ViewModel for showing cruise.
    /// </summary>
    public class CruiseViewModel
    {
        public int Id { get; set; }
        public string ShipName { get; set; } = null!;
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }
        public int CruiseLength { get; set; }
        public bool CanLike { get; set; }
        public int LikesCount { get; set; }
    }
}
