using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using WebApi;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Persistence;
using Domain.Services.Appointments;

namespace Clinic.AcceptanceTests.Infrastructure
{
    public class ClinicWebApplicatioFactory : WebApplicationFactory<IApiMarker>
    {
        private readonly string _dbName = Guid.NewGuid().ToString();
        
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                configurationBuilder.AddInMemoryCollection(
                    new KeyValuePair<string, string>[]
                    {
                        new("UseInMemoryDatabase", "true"),
                        new("Token:ValidateLifetime", "false"),
                    });

                builder.ConfigureServices(services =>
                {
                    services.AddScoped<IDoctorEmailAddressUniquenessChecker, DoctorEmailAddressUniquenessChecker>();
                });
            });

            builder.ConfigureTestServices((services) =>
            {
                ServiceProvider serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                services.Where(d => d.ServiceType == typeof(DbContextOptions<AppointmentDbContext>))
                    .ToList()
                    .ForEach(d => services.Remove(d));

                services.AddDbContext<AppointmentDbContext>(options =>
                {
                    options.UseInMemoryDatabase(_dbName);
                    options.UseInternalServiceProvider(serviceProvider);
                });


                using IServiceScope scope = services.BuildServiceProvider().CreateScope();
                IServiceProvider scopedServices = scope.ServiceProvider;

                AppointmentDbContext db = scopedServices.GetRequiredService<AppointmentDbContext>();
                db.Database.EnsureCreated();

            });
        }
    }
}
