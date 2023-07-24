using Clinic.AcceptanceTests.Infrastructure;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Clinic.AcceptanceTests.Hooks
{
    [Binding]
    public class SpecFlowHooks
    {
        private readonly ClinicWebApplicatioFactory _factory;
        private readonly AppointmentDbContext _dbContext;

        public SpecFlowHooks(ClinicWebApplicatioFactory factory)
        {
            _factory = factory;
            _dbContext = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<AppointmentDbContext>();
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            //_dbContext.Database.ExecuteSqlRaw("DELETE FROM Doctors");
            //_dbContext.Database.ExecuteSqlRaw("DELETE FROM Patients");
            //_dbContext.Database.ExecuteSqlRaw("DELETE FROM WeeklyAvailabilities");
            //_dbContext.Database.ExecuteSqlRaw("DELETE FROM Appointments");
        }
    }
}
