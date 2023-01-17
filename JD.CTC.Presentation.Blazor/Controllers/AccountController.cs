using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System;
using System.Threading.Tasks;
using JD.CTC.Shared.Model.Acesso;

namespace JD.CTC.Presentation.Blazor.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly NavigationManager _navigationManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,            
            RoleManager<ApplicationRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("account/autenticar")]
        public async Task<IActionResult> Autenticar()
        {
            var applicationUser = await _userManager.FindByIdAsync("1");

            var claims = await _userManager.GetClaimsAsync(applicationUser);

            if (claims != null)
            {
                claims.Add(new Claim("Nascimento", DateTime.Now.ToString("1980-08-05")));

                await _signInManager.SignInWithClaimsAsync(applicationUser, true, claims);

                return Redirect("/");
            }

            return Redirect($"/Denied/{System.Convert.ToBase64String(System.Text.Encoding.Default.GetBytes("Usuário não encontrado!"))}");
        }

        [HttpGet("account/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Redirect("/");
        }
    }
}
