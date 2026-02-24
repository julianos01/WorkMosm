using WorkMosm.Domain.Entities;

namespace WorkMosm.Domain.Ports
{
    public interface IVehicleRepository
    {
        Task<IReadOnlyList<Vehicle>> GetAllAsync();
    }
}
