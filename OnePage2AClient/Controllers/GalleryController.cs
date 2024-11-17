using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.Gallery.Abstract;
using OnePage2ABussiness.Gallery.Models;
using OnePage2AClient.Controllers;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

public class GalleryController : BaseController
{
    private readonly IGalleryService _galleryService;
    private readonly IRepository<Gallery> _repository;

    public GalleryController(DbContext2A context, IWebHostEnvironment webHostEnvironment, IGalleryService galleryService, IRepository<Gallery> repository) : base(context, webHostEnvironment)
    {
        _galleryService = galleryService;
        _repository = repository;
    }
    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 5;
        var galleries = await _repository.GetAllAsync();
        var totalItems = await _repository.CountAsync();

        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        ViewBag.CurrentPage = page;

        var galleryModels = galleries.Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .Select(g => new AddGalleryModel
                                      {
                                          Id = g.Id,
                                          ImgUrl = g.ImgUrl,
                                          CreatedByName = g.CreatedByName,
                                          CreatedAt = g.CreatedAt,
                                          IsActive = g.IsActive
                                      }).ToList();

        return View(galleryModels);
    }

    [HttpPost]
    public async Task<IActionResult> AddGallery(string createdByName, IFormFile[] imageFiles)
    {
        try
        {
            var galleryModel = new AddGalleryModel
            {
                CreatedByName = createdByName,
                CreatedAt = DateTime.UtcNow,
                IsActive = false
            };

            await _galleryService.AddGalleryAsync(galleryModel, imageFiles);
            TempData["SuccessMessage"] = "Galeri başarıyla eklendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> EditGallery(UpdateGalleryModel galleryModel, IFormFile imageFile)
    {
        try
        {
            await _galleryService.EditGalleryAsync(galleryModel, imageFile);
            TempData["SuccessMessage"] = "Galeri başarıyla güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> SetActiveGallery(int selectedGalleryId)
    {
        try
        {
            await _galleryService.SetActiveGalleryAsync(selectedGalleryId);
            TempData["SuccessMessage"] = "Aktif galeri başarıyla güncellendi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteGallery(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Galeri başarıyla silindi.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}
