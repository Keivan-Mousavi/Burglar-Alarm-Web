using BurglarAlarm.Service;
using BurglarAlarm.Service.Contract;
using Microsoft.Extensions.DependencyInjection;

namespace BurglarAlarm.Web.Server.Registration
{
    public static class ExternalServiceRegistration
    {
        public static IServiceCollection AddExternalServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();

            return services;
        }
    }
}
