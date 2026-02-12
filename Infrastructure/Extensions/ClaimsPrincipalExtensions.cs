using System.Security.Claims;

namespace Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal user)
        {
            var id = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(user));
            return id;
        }
    }
}
