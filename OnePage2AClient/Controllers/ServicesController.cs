using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.Services.Abstract;
using OnePage2ABussiness.Services.Models;
using OnePage2AClient.Controllers;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

public class ServicesController : BaseController
{
    private readonly IServiceServices _serviceService;
    private readonly IRepository<Services> _repository;

    public ServicesController(DbContext2A context, IWebHostEnvironment webHostEnvironment, IServiceServices serviceService, IRepository<Services> repository) : base(context, webHostEnvironment)
    {
        _serviceService = serviceService;
        _repository = repository;
    }
    public async Task<IActionResult> Index(int page = 1)
    {
        int pageSize = 5;
        var services = await _repository.GetAllAsync();
        var totalItems = await _repository.CountAsync();

        ViewBag.TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
        ViewBag.CurrentPage = page;

        return View(services.Skip((page - 1) * pageSize).Take(pageSize));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddServices(AddServiceModel service, IFormFile imageFile)
    {
        await _serviceService.AddServiceAsync(service, imageFile, User.Identity.Name);
        TempData["SuccessMessage"] = "Services başarıyla eklendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditServices(UpdateServiceModel service, IFormFile imageFile)
    {
        await _serviceService.UpdateServiceAsync(service, imageFile);
        TempData["SuccessMessage"] = "Services başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SetActiveServices(int selectedServicesId)
    {
        await _serviceService.SetActiveServiceAsync(selectedServicesId);
        TempData["SuccessMessage"] = "Aktif service başarıyla güncellendi.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> DeleteServices(int id)
    {
        await _repository.DeleteAsync(id);
        TempData["SuccessMessage"] = "Services başarıyla silindi.";
        return RedirectToAction(nameof(Index));
    }
}
