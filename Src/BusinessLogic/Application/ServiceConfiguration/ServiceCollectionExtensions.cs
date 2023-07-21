using Application.Appointments.Commands.CreateAppointment;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.ServiceConfiguration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IIdService, IdService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateAppointmentRequest).Assembly));
            return services;
        }
    }
}
