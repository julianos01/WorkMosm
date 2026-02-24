using System.Net.Http.Json;
using WorkMosm.FrontEnd.Models;
using WorkMosm.FrontEnd.Services.Interfaces;

namespace WorkMosm.FrontEnd.Services
{
    public class VehicleService : IVehicleService
    {

        private readonly HttpClient _http;

        public VehicleService(HttpClient http)
        {
            _http = http;
        }

        public async Task<IReadOnlyList<VehicleDto>> GetVehiclesAsync()
        {
            return await _http.GetFromJsonAsync<IReadOnlyList<VehicleDto>>("vehicles/showvehicles") ?? [];
        }
    }
}
