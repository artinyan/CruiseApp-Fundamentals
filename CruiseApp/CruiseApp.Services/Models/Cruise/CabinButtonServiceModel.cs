using CruiseApp.Data.Models.Enums;

namespace CruiseApp.Services.Models.Cruise
{
    public class CabinButtonServiceModel
    {
        public int Id { get; set; }

        public string Number { get; set; } = string.Empty;

        public CabinType CabinType { get; set; }

        public bool IsAvailable { get; set; }
    }
}

//using CruiseApp.Data.Models.Enums;

//namespace CruiseApp.Services.Models.Cruise;

///// <summary>
///// For cabin buttons in Deck.cs
///// </summary>
//public class CabinButtonServiceModel
//{
//    public int Id { get; set; }

//    // 07003
//    public string Number { get; set; } = string.Empty;

//    public CabinType CabinType { get; set; }

//    public bool IsAvailable { get; set; }

//    // CSS class за цвета
//    public string CssClass =>
//        CabinType switch
//        {
//            CabinType.Interior => "cabin-blue",
//            CabinType.SeaView => "cabin-green",
//            CabinType.Balcony => "cabin-orange",
//            CabinType.Suite => "cabin-purple",
//            _ => "cabin-gray"
//        };
//}