using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.References.Abstract;
using OnePage2ABussiness.References.Models;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

namespace OnePage2AClient.Controllers
{
    public class ReferencesController : BaseController
    {
        private readonly IReferencesServices _referencesService;
        private readonly IRepository<References> _repository;

        public ReferencesController(DbContext2A context, IWebHostEnvironment webHostEnvironment, IReferencesServices referencesService, IRepository<References> repository) : base(context, webHostEnvironment)
        {
            _referencesService = referencesService;
            _repository = repository;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;
            var references = await _repository.GetAllAsync();
            var totalItems = await _repository.CountAsync();

            ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            ViewBag.CurrentPage = page;

            // References türünü AddReferencesModel'e dönüştür
            var referencesModel = references.Skip((page - 1) * pageSize)
                                             .Take(pageSize)
                                             .Select(r => new AddReferencesModel
                                             {
                                                 Id = r.Id,
                                                 ReferemcesTitle = r.ReferemcesTitle,
                                                 ImgUrl = r.ImgUrl
                                             }).ToList();

            return View(referencesModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddReferences(AddReferencesModel references, IFormFile imageFile)
        {
            await _referencesService.AddReferencesAsync(references, imageFile, User.Identity.Name);
            TempData["SuccessMessage"] = "References başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReferences(UpdateReferencesModel references, IFormFile imageFile)
        {
            await _referencesService.UpdateReferencesAsync(references, imageFile);
            TempData["SuccessMessage"] = "References başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActiveReferences(int selectedReferencesId)
        {
            await _referencesService.SetActiveReferencesAsync(selectedReferencesId);
            TempData["SuccessMessage"] = "Aktif references başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteReferences(int id)
        {
            await _repository.DeleteAsync(id);
            TempData["SuccessMessage"] = "References başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
