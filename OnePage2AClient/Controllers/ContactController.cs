using Microsoft.AspNetCore.Mvc;
using OnePage2ABussiness.Contacts.Abstract;
using OnePage2ABussiness.Contacts.Models;
using OnePage2ABussiness.Services.Models;
using OnePage2ADataAccess.Contexts;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

namespace OnePage2AClient.Controllers
{
    public class ContactController : BaseController
    {
        private readonly IContactServices _contactService;
        private readonly IRepository<Contact> _repository;

        public ContactController(DbContext2A context, IWebHostEnvironment webHostEnvironment, IContactServices contactService, IRepository<Contact> repository) : base(context, webHostEnvironment)
        {
            _contactService = contactService;
            _repository = repository;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            int pageSize = 5;

            // Fetch data from repository and map to AddContactModel
            var contacts = await _repository.GetAllAsync();
            var contactModels = contacts.Select(contact => new AddContactModel
            {
                Id = contact.Id,
                Address = contact.Address,
                Phone = contact.Phone,
                Email = contact.Email,
                CreatedByName = contact.CreatedByName,
                CreatedAt = contact.CreatedAt
            }).ToList(); // Ensure it's a List<AddContactModel>

            // Paginate
            var paginatedContacts = contactModels
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList(); // Ensure this is also a List<AddContactModel>

            ViewBag.TotalPages = (int)Math.Ceiling(contactModels.Count / (double)pageSize);
            ViewBag.CurrentPage = page;

            // Pass paginated list to the view
            return View(paginatedContacts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddContact(AddContactModel contact)
        {
            await _contactService.AddContactAsync(contact, User.Identity.Name);
            TempData["SuccessMessage"] = "Contact başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult EditContactById(int id)
        {
            var contact = _context.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact == null)
            {
                return NotFound();
            }

            var contactServiceModel = new UpdateContactModel
            {
                Id = contact.Id,
                Phone = contact.Phone,
                Email = contact.Email,
                Address = contact.Address,
            };

            return PartialView("~/Views/Shared/Admin/Contact/_EditContactPartial.cshtml", contactServiceModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditContact(UpdateContactModel contact)
        {
            await _contactService.UpdateContactAsync(contact);
            TempData["SuccessMessage"] = "Contact başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetActiveContact(int selectedContactId)
        {
            await _contactService.SetActiveContactAsync(selectedContactId);
            TempData["SuccessMessage"] = "Aktif contact başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteContact(int id)
        {
            await _repository.DeleteAsync(id);
            TempData["SuccessMessage"] = "Contact başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }

    }
}
