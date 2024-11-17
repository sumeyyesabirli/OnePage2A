using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Services.Models;

namespace OnePage2ABussiness.Services.Abstract
{
    public interface IServiceServices
    {
        Task AddServiceAsync(AddServiceModel serviceModel, IFormFile imageFile, string createdByName);
        Task UpdateServiceAsync(UpdateServiceModel serviceModel, IFormFile imageFile);
        Task SetActiveServiceAsync(int selectedServicesId);
    }
}
