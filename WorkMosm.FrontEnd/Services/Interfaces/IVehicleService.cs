using WorkMosm.FrontEnd.Models;

namespace WorkMosm.FrontEnd.Services.Interfaces
{
    public interface IVehicleService
    {
        Task<IReadOnlyList<VehicleDto>> GetVehiclesAsync();
    }
}
