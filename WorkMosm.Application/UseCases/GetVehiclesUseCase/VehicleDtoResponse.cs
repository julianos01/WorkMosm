namespace WorkMosm.Application.UseCases.GetVehiclesUseCase
{
    public class VehicleDtoResponse
    {
        public Guid Id { get; set; }

        public string Brand { get; set; } = null!;
        public string Model { get; set; } = null!;
        public string Year { get; set; } = null!;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = null!;
    }
}
