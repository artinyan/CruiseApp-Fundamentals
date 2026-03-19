using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Services.Core.Models.Cruise
{
    public class CabinButtonServiceModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public int PosX { get; set; }
        public int PosY { get; set; }

        public CabinType CabinType { get; set; }

        public bool IsAvailable { get; set; }
    }
}