using DemoSignalR.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace DemoSignalR.Services;

public class AuthService : IAuthService
{
    public async Task<ClaimsPrincipal> Login(string email, string password)
    {
        Guid guid = Guid.NewGuid();
        string guidString = guid.ToString();
        var claims = new List<Claim> {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Name, email),
                    new Claim(ClaimTypes.Role,  "User"),
                    new Claim("userId",guidString)
                    };

        var identity = new ClaimsIdentity(claims, "auth");
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

        return claimsPrincipal;
    }
}
