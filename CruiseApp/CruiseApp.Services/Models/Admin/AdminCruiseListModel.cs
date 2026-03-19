namespace CruiseApp.Services.Core.Models.Admin
{
    public class AdminCruiseListModel
    {
        public int Id { get; set; }

        public string ShipName { get; set; } = string.Empty;

        public DateOnly FirstDay { get; set; }

        public DateOnly LastDay { get; set; }

        public int CruiseLength { get; set; }
    }
}
