namespace CruiseApp.Web.ViewModels.Admin
{
    public class AdminCruiseListViewModel
    {
        public int Id { get; set; }
        public string ShipName { get; set; } = null!;
        public DateOnly FirstDay { get; set; }
        public DateOnly LastDay { get; set; }
        public int CruiseLength { get; set; }
        public int Nights => CruiseLength;
    }
}
