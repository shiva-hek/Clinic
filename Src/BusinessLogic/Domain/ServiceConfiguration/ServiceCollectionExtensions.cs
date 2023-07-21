using Domain.Models.Appointments.Factories;
using Domain.Services.Appointments;
using Microsoft.Extensions.DependencyInjection;

namespace Domain.ServiceConfiguration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped(typeof(AppointmentFactory));
            services.AddScoped<IAppointmentDurationChecker, AppointmentDurationChecker>();
            services.AddScoped<IAppointmentNumberChecker, AppointmentNumberChecker>();
            services.AddScoped<IAppoitmentsOfPatientOverlapChecker, AppoitmentsOfPatientOverlapChecker>();
            services.AddScoped<IClinicTimeChecker, ClinicTimeChecker>();
            services.AddScoped<IDoctorTimeChecker, DoctorTimeChecker>();
            services.AddScoped<IDoctorAvailabilityChecker, DoctorAvailabilityChecker>();
            services.AddScoped<IRoomAvailabilityChecker, RoomAvailabilityChecker>();
            services.AddScoped<IAppointmentOverlapChecker, AppointmentOverlapChecker>();

            return services;
        }
    }
}
