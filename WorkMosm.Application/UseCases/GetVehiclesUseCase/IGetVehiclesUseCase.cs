namespace WorkMosm.Application.UseCases.GetVehiclesUseCase
{
    public interface IGetVehiclesUseCase
    {
        Task<IEnumerable<VehicleDtoResponse>> ExecuteAsync();
    }
}
