using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Banners.Models;
using OnePage2AClientBussines.Banners.Abstract;
using OnePage2ACore.Abstract;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;

public class BannerService : IBannerService
{
    private readonly IRepository<Banner> _repository;
    private readonly IFileService _fileService;
    public BannerService(IRepository<Banner> repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task AddBannerAsync(AddBannerModel bannerModel, IFormFile imageFile, string createdByName)
    {
        if (imageFile != null)
        {
            bannerModel.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/banners");
        }

        var bannerEntity = new Banner
        {
            Title = bannerModel.Title,
            Description = bannerModel.Description,
            ImgUrl = bannerModel.ImgUrl,
            CreatedByName = createdByName,
            CreatedAt = DateTime.UtcNow,
            IsActive = bannerModel.IsActive
        };

        await _repository.AddAsync(bannerEntity);
    }
    public async Task UpdateBannerAsync(UpdateBannerModel bannerModel, IFormFile imageFile)
    {
        var existingBanner = await _repository.GetByIdAsync(bannerModel.Id);
        if (existingBanner == null)
        {
            throw new Exception("Banner not found");
        }

        // Title ve Description null veya boş ise, alanları sıfırla
        existingBanner.Title = string.IsNullOrEmpty(bannerModel.Title) ? null : bannerModel.Title;
        existingBanner.Description = string.IsNullOrEmpty(bannerModel.Description) ? null : bannerModel.Description;

        // IsActive her durumda güncellenebilir
        existingBanner.IsActive = bannerModel.IsActive;

        // Eğer yeni bir resim dosyası gelmişse, önceki resmi sil ve yeni resmi kaydet
        if (imageFile != null)
        {
            if (!string.IsNullOrEmpty(existingBanner.ImgUrl))
            {
                _fileService.DeleteFile(existingBanner.ImgUrl);
            }

            existingBanner.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/banners");
        }

        // Banner'ı güncelle
        await _repository.UpdateAsync(existingBanner);
    }



    public async Task SetActiveBannerAsync(int selectedBannerId)
    {
        var banners = await _repository.GetAllAsync();
        foreach (var banner in banners)
        {
            banner.IsActive = banner.Id == selectedBannerId;
            await _repository.UpdateAsync(banner);
        }
    }
}
