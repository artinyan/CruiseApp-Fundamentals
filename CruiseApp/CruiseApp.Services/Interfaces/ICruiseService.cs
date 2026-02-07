namespace CruiseApp.Services.Interfaces
{
    public interface ICruiseService
    {
        Task<int> CreateCruiseAsync(
            int shipId,
            DateOnly firstDay,
            DateOnly lastDay);
    }
}