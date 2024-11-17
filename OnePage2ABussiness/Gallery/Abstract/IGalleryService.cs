using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Gallery.Models;

namespace OnePage2ABussiness.Gallery.Abstract
{
    public interface IGalleryService
    {
        Task AddGalleryAsync(AddGalleryModel galleryModel, IFormFile[] imageFiles);
        Task EditGalleryAsync(UpdateGalleryModel galleryModel, IFormFile imageFile);
        Task SetActiveGalleryAsync(int selectedGalleryId);
    }
}
