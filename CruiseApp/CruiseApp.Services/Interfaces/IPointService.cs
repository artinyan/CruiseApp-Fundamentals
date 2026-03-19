using CruiseApp.Data.Models;

namespace CruiseApp.Services.Core.Interfaces
{
    public interface IPointService
    {
        Task<IEnumerable<Point>> GetAllAsync();
    }
}

