using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.AboutUs.Abstract;
using OnePage2ABussiness.AboutUs.Models;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

namespace OnePage2AClient.Controllers
{
    [Authorize(Roles = "admin,editor")]
    public class AboutUsController : BaseController
    {
        private readonly IAboutUsServices _aboutUsService;
        private readonly IRepository<AboutUs> _repository;

        public AboutUsController(DbContext2A context, IWebHostEnvironment webHostEnvironment, IAboutUsServices aboutUsService, IRepository<AboutUs> repository) : base(context, webHostEnvironment)
        {
            _aboutUsService = aboutUsService;
            _repository = repository;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var aboutUsEntities = await _repository.GetAllAsync();

            var aboutUsModels = aboutUsEntities.Select(aboutUs => new AddAboutUsModel
            {
                Id = aboutUs.Id,
                AboutUsDescription = aboutUs.AboutUsDescription,
                ImgUrl = aboutUs.ImgUrl,
                CreatedByName = aboutUs.CreatedByName,
                CreatedAt = aboutUs.CreatedAt
            }).ToList();

            var paginatedAboutUsModels = aboutUsModels
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.TotalPages = (int)Math.Ceiling(aboutUsModels.Count / (double)pageSize);
            ViewBag.CurrentPage = page;

            return View(paginatedAboutUsModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAboutUs(AddAboutUsModel aboutUs, IFormFile imageFile)
        {
            await _aboutUsService.AddAboutUsAsync(aboutUs, imageFile, User.Identity.Name);
            TempData["SuccessMessage"] = "Hakkımızda başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult EditAboutUsById(int id)
        {
            var aboutUs = _context.AboutUs.FirstOrDefault(a => a.Id == id);
            if (aboutUs == null)
            {
                return NotFound();
            }

            var updateAboutUsModel = new UpdateAboutUsModel
            {
                Id = aboutUs.Id,
                AboutUsDescription = aboutUs.AboutUsDescription,
                ImgUrl = aboutUs.ImgUrl,
                IsActive = aboutUs.IsActive
            };

            return PartialView("~/Views/Shared/Admin/AboutUs/_EditAboutUsPartial.cshtml", updateAboutUsModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAboutUs(UpdateAboutUsModel aboutUs, IFormFile imageFile)
        {
            await _aboutUsService.UpdateAboutUsAsync(aboutUs, imageFile);
            TempData["SuccessMessage"] = "AboutUs başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActiveAboutUs(int selectedAboutUsId)
        {
            await _aboutUsService.SetActiveAboutUsAsync(selectedAboutUsId);
            TempData["SuccessMessage"] = "Aktif aboutUs başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAboutUs(int id)
        {
            await _repository.DeleteAsync(id);
            TempData["SuccessMessage"] = "AboutUs başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
