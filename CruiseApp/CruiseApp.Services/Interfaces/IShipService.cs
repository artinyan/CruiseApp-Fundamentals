using CruiseApp.Data.Models;

namespace CruiseApp.Services.Interfaces
{
    public interface IShipService
    {
        Task<IEnumerable<Ship>> GetAllAsync();
    }
}


