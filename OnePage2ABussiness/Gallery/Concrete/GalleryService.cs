using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Gallery.Abstract;
using OnePage2ABussiness.Gallery.Models;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;
using OnePage2ACore.Abstract;

public class GalleryService : IGalleryService
{
    private readonly IRepository<Gallery> _repository;
    private readonly IFileService _fileService;

    public GalleryService(IRepository<Gallery> repository, IFileService fileService)
    {
        _repository = repository;
        _fileService = fileService;
    }

    public async Task AddGalleryAsync(AddGalleryModel galleryModel, IFormFile[] imageFiles)
    {
        foreach (var imageFile in imageFiles)
        {
            if (imageFile != null)
            {
                galleryModel.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/gallery");
            }

            var galleryEntity = new Gallery
            {
                ImgUrl = galleryModel.ImgUrl,
                CreatedByName = galleryModel.CreatedByName,
                CreatedAt = galleryModel.CreatedAt,
                IsActive = galleryModel.IsActive
            };

            await _repository.AddAsync(galleryEntity);
        }
    }


    public async Task EditGalleryAsync(UpdateGalleryModel galleryModel, IFormFile imageFile)
    {
        var existingGallery = await _repository.GetByIdAsync(galleryModel.Id);
        if (existingGallery == null)
        {
            throw new Exception("Galeri bulunamadı.");
        }

        if (imageFile != null)
        {
            if (!string.IsNullOrEmpty(existingGallery.ImgUrl))
            {
                _fileService.DeleteFile(existingGallery.ImgUrl);
            }

            existingGallery.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/gallery");
        }

        existingGallery.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(existingGallery);
    }

    public async Task SetActiveGalleryAsync(int selectedGalleryId)
    {
        var galleries = await _repository.GetAllAsync();
        foreach (var gallery in galleries)
        {
            gallery.IsActive = gallery.Id == selectedGalleryId;
            await _repository.UpdateAsync(gallery);
        }
    }
}
