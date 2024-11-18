using OnePage2ABussiness.Contacts.Abstract;
using OnePage2ABussiness.Contacts.Models;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

namespace OnePage2ABussiness.Contacts.Concrete
{
    public class ContactServices : IContactServices
    {
        private readonly IRepository<Contact> _repository;
        public ContactServices(IRepository<Contact> repository)
        {
            _repository = repository;
        }

        public async Task AddContactAsync(AddContactModel bannerModel, string createdByName)
        {

            var bannerEntity = new Contact
            {
                Address = bannerModel.Address,
                Phone = bannerModel.Phone,
                Email = bannerModel.Email,
                CreatedByName = createdByName,
                CreatedAt = DateTime.UtcNow,
                IsActive = bannerModel.IsActive
            };

            await _repository.AddAsync(bannerEntity);
        }

        public async Task UpdateContactAsync(UpdateContactModel bannerModel)
        {
            var existingContact = await _repository.GetByIdAsync(bannerModel.Id);
            if (existingContact == null)
            {
                throw new Exception("Contact not found");
            }

            existingContact.Email = bannerModel.Email ?? existingContact.Email;
            existingContact.Address = bannerModel.Address ?? existingContact.Address;
            existingContact.Phone = bannerModel.Phone ?? existingContact.Phone;
            existingContact.Email = bannerModel.Email ?? existingContact.Email;
            existingContact.IsActive = bannerModel.IsActive;

            await _repository.UpdateAsync(existingContact);
        }

        public async Task SetActiveContactAsync(int selectedContactId)
        {
            var banners = await _repository.GetAllAsync();
            foreach (var banner in banners)
            {
                banner.IsActive = banner.Id == selectedContactId;
                await _repository.UpdateAsync(banner);
            }
        }
    }
}
