using Microsoft.EntityFrameworkCore;
using WorkMosm.Domain.Entities;
using WorkMosm.Domain.Ports;

namespace WorkMosm.Infrastructure.Persistence.Vehicles
{
    public class EfVehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _appDbContext;

        public EfVehicleRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<IReadOnlyList<Vehicle>> GetAllAsync()
        {
            return await _appDbContext.Vehicles.
                AsNoTracking().
                ToListAsync();
        }
    }
}
