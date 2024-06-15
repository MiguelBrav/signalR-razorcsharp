using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace DemoSignalR.Interfaces;

public interface IAuthService
{
    public Task<ClaimsPrincipal> Login(string email, string password);
}
