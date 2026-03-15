using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Services.Models.Cruise
{
    public class CabinButtonServiceModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;
        public string SequenceNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public CabinType CabinType { get; set; }

        public bool IsAvailable { get; set; }
    }
}