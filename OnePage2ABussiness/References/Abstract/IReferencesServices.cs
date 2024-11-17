using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.References.Models;

namespace OnePage2ABussiness.References.Abstract
{
    public interface IReferencesServices
    {
        Task AddReferencesAsync(AddReferencesModel referencesModel, IFormFile imageFile, string createdByName);
        Task UpdateReferencesAsync(UpdateReferencesModel referencesModel, IFormFile imageFile);
        Task SetActiveReferencesAsync(int selectedReferencesId);
    }
}
