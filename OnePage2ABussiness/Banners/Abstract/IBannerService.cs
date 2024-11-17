using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Banners.Models;

namespace OnePage2AClientBussines.Banners.Abstract
{
    public interface IBannerService
    {
        Task AddBannerAsync(AddBannerModel bannerModel, IFormFile imageFile, string createdByName);
        Task UpdateBannerAsync(UpdateBannerModel bannerModel, IFormFile imageFile);
        Task SetActiveBannerAsync(int selectedBannerId);
    }
}
