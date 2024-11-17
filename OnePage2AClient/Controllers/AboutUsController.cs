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

            // Fetch all AboutUs entities from the repository
            var aboutUsEntities = await _repository.GetAllAsync();

            // Map the entities to the view model
            var aboutUsModels = aboutUsEntities.Select(aboutUs => new AddAboutUsModel
            {
                Id = aboutUs.Id,
                AboutUsDescription = aboutUs.AboutUsDescription,
                ImgUrl = aboutUs.ImgUrl,
                CreatedByName = aboutUs.CreatedByName,
                CreatedAt = aboutUs.CreatedAt
            }).ToList();

            // Apply pagination
            var paginatedAboutUsModels = aboutUsModels
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Set ViewBag properties for pagination
            ViewBag.TotalPages = (int)Math.Ceiling(aboutUsModels.Count / (double)pageSize);
            ViewBag.CurrentPage = page;

            // Pass the paginated view model to the view
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
