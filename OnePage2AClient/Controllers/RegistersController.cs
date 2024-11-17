using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.Register.Abstract;
using OnePage2ABussiness.Register.Models;

namespace OnePage2AClient.Controllers
{
    [Authorize(Roles = "admin")]
    public class RegistersController : Controller
    {
        private readonly IRegisterService _registerService;

        public RegistersController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(AddRegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var createdByName = User.Identity.Name;
                await _registerService.AddUserAsync(model, model.Password, createdByName);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }
    }
}
