using Microsoft.Extensions.DependencyInjection;
using OnePage2ACore.Abstract;
using OnePage2ACore.Concrete;

namespace OnePage2ACore
{
    public static class ServiceRegistration
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
        }

    }
}
