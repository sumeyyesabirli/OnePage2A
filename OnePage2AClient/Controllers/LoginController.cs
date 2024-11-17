using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.Login.Abstract;
using OnePage2ABussiness.Login.Model;
using OnePage2AClient.Models;

namespace OnePage2AClient.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginServices _loginServices;

        public LoginController(ILoginServices loginServices)
        {
            _loginServices = loginServices;
        }

        // GET: Login/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                var loginModel = new AddLoginModel
                {
                    Name = model.Name,
                    Password = model.Password
                };

                var user = await _loginServices.AuthenticateAsync(loginModel, model.Password);

                // Kullanıcıyı cookie ile oturum aç
                var claims = new List<System.Security.Claims.Claim>
        {
            new(System.Security.Claims.ClaimTypes.Name, user.Name),
            new(System.Security.Claims.ClaimTypes.Email, user.Email),
            new(System.Security.Claims.ClaimTypes.Role, user.Role)
        };

                var claimsIdentity = new System.Security.Claims.ClaimsIdentity(
                    claims, Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new System.Security.Claims.ClaimsPrincipal(claimsIdentity)
                );

                return RedirectToAction("Index", "Banners");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index", model);
            }
        }


        // POST: Login/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Login");
        }

        // GET: Login/ForgotPassword
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Login/ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _loginServices.ResetPasswordAsync(model.Email);
                TempData["Message"] = "Şifre sıfırlama linki e-posta adresinize gönderildi.";
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        // GET: Login/ForgotPasswordConfirmation
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        // GET: Login/ResetPassword
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Geçersiz token.");
            }

            var model = new ResetPasswordViewModel { Token = token };
            return View(model);
        }

        // POST: Login/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                await _loginServices.ChangePasswordAsync(model.Token, model.NewPassword);
                TempData["Message"] = "Şifreniz başarıyla sıfırlandı.";
                return RedirectToAction("Index", "Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
