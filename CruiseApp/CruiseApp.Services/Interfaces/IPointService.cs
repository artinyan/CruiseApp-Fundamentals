using CruiseApp.Data.Models;

namespace CruiseApp.Services.Interfaces
{
    public interface IPointService
    {
        Task<IEnumerable<Point>> GetAllAsync();
    }
}

