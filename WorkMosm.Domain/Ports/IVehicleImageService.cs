namespace WorkMosm.Domain.Ports
{
    public interface IVehicleImageService
    {
        string ResolveImageUrl(string? imageName);
    }
}
