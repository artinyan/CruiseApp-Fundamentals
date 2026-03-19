using CruiseApp.Data.Models;

namespace CruiseApp.Services.Core.Interfaces
{
    public interface IShipService
    {
        Task<IEnumerable<Ship>> GetAllAsync();
    }
}


