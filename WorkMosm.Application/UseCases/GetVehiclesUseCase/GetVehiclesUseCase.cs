using WorkMosm.Domain.Ports;

namespace WorkMosm.Application.UseCases.GetVehiclesUseCase
{
    public class GetVehiclesUseCase : IGetVehiclesUseCase
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IVehicleImageService _imageService;

        public GetVehiclesUseCase(IVehicleRepository vehicleRepository, IVehicleImageService imageService)
        {
            _vehicleRepository = vehicleRepository;
            _imageService = imageService;
        }

        public async Task<IEnumerable<VehicleDtoResponse>> ExecuteAsync()
        {
            var vehicles = await _vehicleRepository.GetAllAsync();

            return vehicles.Select(x => new VehicleDtoResponse
            {
                Id = x.Id,
                Brand = x.Brand,
                Model = x.Model,
                Year = x.Year,
                Price = x.Price,
                ImageUrl = _imageService.ResolveImageUrl(x.ImageName)
            });
        }

    }
}
