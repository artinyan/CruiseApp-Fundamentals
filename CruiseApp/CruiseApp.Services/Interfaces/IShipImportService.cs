using CruiseApp.Services.Core.DTOs;

namespace CruiseApp.Services.Core.Interfaces
{
    public interface IShipImportService
    {
        Task ImportShipAsync(string zipPath);
    }
}