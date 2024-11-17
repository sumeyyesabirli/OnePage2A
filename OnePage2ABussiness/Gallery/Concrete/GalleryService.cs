using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Gallery.Abstract;
using OnePage2ABussiness.Gallery.Models;
using OnePage2ADataAccess.Repositories.Abstract;
using OnePage2AEntity.Entites;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace OnePage2ABussiness.Gallery
{
    public class GalleryService : IGalleryService
    {
        private readonly IRepository<OnePage2AEntity.Entites.Gallery> _repository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public GalleryService(IRepository<OnePage2AEntity.Entites.Gallery> repository, IWebHostEnvironment webHostEnvironment)
        {
            _repository = repository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task AddGalleryAsync(AddGalleryModel galleryModel, IFormFile[] imageFiles)
        {
            if (imageFiles == null || imageFiles.Length == 0)
            {
                throw new Exception("En az bir fotoğraf yüklenmelidir.");
            }

            foreach (var imageFile in imageFiles)
            {
                if (imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "gallery");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    var gallery = new OnePage2AEntity.Entites.Gallery
                    {
                        ImgUrl = "/images/gallery/" + uniqueFileName,
                        CreatedByName = galleryModel.CreatedByName,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = false // Set default value or based on your requirement
                    };

                    await _repository.AddAsync(gallery);
                }
            }
        }

        public async Task EditGalleryAsync(UpdateGalleryModel galleryModel, IFormFile imageFile)
        {
            var existingGallery = await _repository.GetByIdAsync(galleryModel.Id);
            if (existingGallery == null)
            {
                throw new Exception("Galeri bulunamadı.");
            }

            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "gallery");
                var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(imageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                existingGallery.ImgUrl = "/images/gallery/" + uniqueFileName;
            }

            existingGallery.IsActive = galleryModel.IsActive;
            existingGallery.CreatedByName = galleryModel.CreatedByName;
            existingGallery.CreatedAt = galleryModel.CreatedAt;

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
}
