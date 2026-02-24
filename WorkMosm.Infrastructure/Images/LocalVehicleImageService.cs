using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WorkMosm.Domain.Ports;


namespace WorkMosm.Infrastructure.Images
{
    public class LocalVehicleImageService : IVehicleImageService
    {
        private const string ImagesUrlPath = "assets/images/vehicles";
        private const string DefaultImage = "default-car.jpg";

        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LocalVehicleImageService(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env ?? throw new ArgumentNullException(nameof(env));
            _httpContextAccessor = httpContextAccessor;
        }

        public string ResolveImageUrl(string? imageName)
        {
            var httpContext = _httpContextAccessor.HttpContext ?? throw new InvalidOperationException("No httpContext available");

            var finalImage = string.IsNullOrWhiteSpace(imageName)
                ? DefaultImage : imageName;

            var physicalPath = Path.Combine(_env.ContentRootPath, "wwwroot", ImagesUrlPath, finalImage);

            if (!File.Exists(physicalPath)) finalImage = DefaultImage;

            var imageUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/{ImagesUrlPath}/{finalImage}";

            return imageUrl;
        }
    }
}
