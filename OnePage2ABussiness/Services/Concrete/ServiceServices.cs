using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.Services.Abstract;
using OnePage2ABussiness.Services.Models;
using OnePage2ACore.Abstract;
using OnePage2ADataAccess.Repositories.Abstract;

namespace OnePage2ABussiness.Services.Concrete
{
    public class ServiceServices : IServiceServices
    {
        private readonly IRepository<OnePage2AEntity.Entites.Services> _repository;
        private readonly IFileService _fileService;

        public ServiceServices(IRepository<OnePage2AEntity.Entites.Services> repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }
        public async Task AddServiceAsync(AddServiceModel servicesModel, IFormFile imageFile, string createdByName)
        {
            if (imageFile != null)
            {
                servicesModel.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/services");
            }

            var servicesEntity = new OnePage2AEntity.Entites.Services
            {
                ServicesTitle = servicesModel.ServicesTitle,
                ServicesDescription = servicesModel.ServicesDescription,
                ImgUrl = servicesModel.ImgUrl,
                CreatedByName = createdByName,
                CreatedAt = DateTime.UtcNow,
                IsActive = servicesModel.IsActive
            };

            await _repository.AddAsync(servicesEntity);
        }

        public async Task UpdateServiceAsync(UpdateServiceModel servicesModel, IFormFile imageFile)
        {
            var existingService = await _repository.GetByIdAsync(servicesModel.Id);
            if (existingService == null)
            {
                throw new Exception("Service not found");
            }

            existingService.ServicesTitle = servicesModel.ServicesTitle ?? existingService.ServicesTitle;
            existingService.ServicesDescription = servicesModel.ServicesDescription ?? existingService.ServicesDescription;
            existingService.IsActive = servicesModel.IsActive;

            if (imageFile != null)
            {
                if (!string.IsNullOrEmpty(existingService.ImgUrl))
                {
                    _fileService.DeleteFile(existingService.ImgUrl);
                }

                existingService.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/services");
            }

            await _repository.UpdateAsync(existingService);
        }

        public async Task SetActiveServiceAsync(int selectedServiceId)
        {
            var servicess = await _repository.GetAllAsync();
            foreach (var services in servicess)
            {
                services.IsActive = services.Id == selectedServiceId;
                await _repository.UpdateAsync(services);
            }
        }

    }
}
