using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.Banners.Models;
using OnePage2ABussiness.Services.Models;
using OnePage2AClient.Controllers;
using OnePage2AClientBussines.Banners.Abstract;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

public class BannersController : BaseController
{
    private readonly IBannerService _bannerService;
    private readonly IRepository<Banner> _repository;

    public BannersController(DbContext2A context,IWebHostEnvironment webHostEnvironment,IBannerService bannerService,IRepository<Banner> repository) : base(context, webHostEnvironment) 
    {
        _bannerService = bannerService;
        _repository = repository;
    }
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 5;
        var banners = await _repository.GetAllAsync();

        // Model dönüşümü
        var bannerModels = banners.Select(b => new AddBannerModel
        {
            Id = b.Id,
            Title = b.Title,
            Description = b.Description,
            ImgUrl = b.ImgUrl,
            CreatedByName = b.CreatedByName,
            IsActive = b.IsActive
        }).ToList();

        // Sayfalama bilgileri
        ViewBag.TotalPages = (int)Math.Ceiling(banners.Count() / (double)pageSize);
        ViewBag.CurrentPage = page;

        // Sayfaya model gönderimi
        return View(bannerModels.Skip((page - 1) * pageSize).Take(pageSize).ToList());
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBanner(AddBannerModel banner, IFormFile imageFile)
    {
        await _bannerService.AddBannerAsync(banner, imageFile, User.Identity.Name);
        TempData["SuccessMessage"] = "Banner başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> EditBannerById(int id)
    {
        var banner = _context.Banners.FirstOrDefault(b => b.Id == id);
        if (banner == null)
        {
            return NotFound();
        }

        // UpdateBannerModel'e dönüştürme
        var updateBannerModel = new UpdateBannerModel
        {
            Id = banner.Id,
            Title = banner.Title,
            Description = banner.Description,
            ImgUrl = banner.ImgUrl,
            IsActive = banner.IsActive
        };

        // PartialView ile dönüş
        return PartialView("~/Views/Shared/Admin/Banners/_EditBannerPartial.cshtml", updateBannerModel);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditBanner(UpdateBannerModel banner, IFormFile imageFile)
    {
        await _bannerService.UpdateBannerAsync(banner, imageFile);
        TempData["SuccessMessage"] = "Banner başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActiveBanner(int selectedBannerId)
    {
        await _bannerService.SetActiveBannerAsync(selectedBannerId);
        TempData["SuccessMessage"] = "Aktif banner başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteBanner(int id)
    {
        await _repository.DeleteAsync(id);
        TempData["SuccessMessage"] = "Banner başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }
}
