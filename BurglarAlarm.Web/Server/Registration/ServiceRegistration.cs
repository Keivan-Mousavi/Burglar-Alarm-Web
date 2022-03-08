using BurglarAlarm.ExternalService;
using BurglarAlarm.ExternalService.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace BurglarAlarm.Web.Server.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<INotificationExternalService, NotificationExternalService>();

            return services;
        }
    }
}
