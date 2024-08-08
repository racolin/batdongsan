using Application.Common.Interfaces;
using System.Security.Claims;

namespace WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var value = 
                _httpContextAccessor.HttpContext?.User?.FindFirstValue("sub") ??
                _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (int.TryParse(value, out var id))
                return id;

            return null;
        }
    }

    public string? UserName => _httpContextAccessor.HttpContext?.User.Identity?.Name;
}
