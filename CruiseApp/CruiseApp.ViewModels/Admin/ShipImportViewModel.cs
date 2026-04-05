namespace CruiseApp.ViewModels.Admin
{
    public class ShipImportViewModel
    {
        public List<string> Files { get; set; } = new();
        public string? SelectedFile { get; set; }
    }
}
