using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using PruebaSignalR.Pages.Hub;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using DemoSignalR.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PruebaSignalR.Pages
{
    [BindProperties]
    public class LoginModel : PageModel
    {
        public string email { get; set; } = string.Empty;
        public string pass { get; set; } = string.Empty;

        private readonly IHubContext<SignalHub> _hubContext;

        private readonly IAuthService _authService;

        public LoginModel(IHubContext<SignalHub> hubContext, IAuthService authService)
        {
            _hubContext = hubContext;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
          return Page();
        }

        public async Task<IActionResult> OnPostLoginAsync()
        {
            ClaimsPrincipal claimsPrincipal = await _authService.Login(email,pass);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal);

            string _userId = claimsPrincipal.FindFirst("userId")?.Value ?? string.Empty;

            HttpContext.Session.SetString("UserAuth", _userId);
            HttpContext.Session.SetString("User", email);

            return RedirectToPage("Index");     
        }
    }
}
