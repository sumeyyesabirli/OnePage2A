using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Contacts.Models;

namespace OnePage2ABussiness.Contacts.Abstract
{
    public interface IContactServices
    {
        Task AddContactAsync(AddContactModel aboutUsModel,string createdByName);
        Task UpdateContactAsync(UpdateContactModel aboutUsModel);
        Task SetActiveContactAsync(int selectedContactId);
    }
}
