using System.Security.Claims;

namespace PatientApi.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static bool TryGetUserId(this ClaimsPrincipal user, out int userId)
    {
        userId = default;
        var raw = user.FindFirstValue(ClaimTypes.NameIdentifier);
        return !string.IsNullOrWhiteSpace(raw) && int.TryParse(raw, out userId);
    }
}
