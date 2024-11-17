using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.AboutUs.Models;

namespace OnePage2ABussiness.AboutUs.Abstract
{
    public interface IAboutUsServices
    {
        Task AddAboutUsAsync(AddAboutUsModel aboutUsModel, IFormFile imageFile, string createdByName);
        Task UpdateAboutUsAsync(UpdateAboutUsModel aboutUsModel, IFormFile imageFile);
        Task SetActiveAboutUsAsync(int selectedAboutUsId);
    }
}
