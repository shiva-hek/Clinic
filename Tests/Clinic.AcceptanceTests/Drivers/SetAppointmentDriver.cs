using Clinic.AcceptanceTests.Dtos;
using Clinic.AcceptanceTests.Infrastructure;
using Domain.Models.Appointments.Entities;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Moq;
using Domain.Models.Appointments.Enums;
using Shared.Model;
using System.Net.Http.Json;
using Application.Appointments.Commands.CreateAppointment;
using Shared.Exceptions;
using Application.Appointments.Commands.CreateEarliestAppointment;
using Clinic.AcceptanceTests.Context;

namespace Clinic.AcceptanceTests.Drivers
{
    public class SetAppointmentDriver : IClassFixture<ClinicWebApplicatioFactory>
    {
        private readonly AppointmentDbContext _dbContext;
        private readonly ClinicWebApplicatioFactory _factory;
        protected readonly AppointmentContext ScenarioContext;
        private readonly HttpClient _httpClient;

        public SetAppointmentDriver(AppointmentContext scenarioContext, ClinicWebApplicatioFactory factory)
        {
            ScenarioContext = scenarioContext;
            _factory = factory;
            IServiceScope scope = _factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<AppointmentDbContext>();
            _httpClient = factory.CreateClient();
        }

        public async Task SeedDoctors(IEnumerable<DoctorDto> dtos)
        {
            IEnumerable<Doctor> doctors = GetDoctors(dtos);
            await _dbContext.Doctors.AddRangeAsync(doctors);
            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<Doctor> GetDoctors(IEnumerable<DoctorDto> dtos)
        {
            Mock<IDoctorEmailAddressUniquenessChecker> emailUniquenessChecker = new Mock<IDoctorEmailAddressUniquenessChecker>();
            emailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<EmailAddress>())).Returns(true);
            return dtos.Select(dto =>
                new Doctor(dto.Id, new Name(dto.FirstName, dto.LastName), dto.DoctorType,
                           new List<WeeklyAvailability>(), new EmailAddress(dto.Email), emailUniquenessChecker.Object)
            );
        }

        public async Task SeedPatients(IEnumerable<PatientDto> dtos)
        {
            IEnumerable<Patient> patients = GetPatients(dtos);
            await _dbContext.Patients.AddRangeAsync(patients);
            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<Patient> GetPatients(IEnumerable<PatientDto> dtos)
        {
            Mock<IPatientEmailAddressUniquenessChecker> emailUniquenessChecker = new Mock<IPatientEmailAddressUniquenessChecker>();
            emailUniquenessChecker.Setup(x => x.IsUnique(It.IsAny<EmailAddress>())).Returns(true);
            return dtos.Select(dto =>
                new Patient(dto.Id, new Name(dto.FirstName, dto.LastName), (Gender)Enum.Parse(typeof(Gender), dto.Gender),
                            new EmailAddress(dto.Email), emailUniquenessChecker.Object)
            );
        }

        public async Task SeedWeeklyAvailability(IEnumerable<WeeklyAvailibilityDto> dtos)
        {
            IEnumerable<WeeklyAvailability> weeklyAvailabilities = GetWeeklyAvailabilities(dtos);
            await _dbContext.WeeklyAvailabilities.AddRangeAsync(weeklyAvailabilities);
            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<WeeklyAvailability> GetWeeklyAvailabilities(IEnumerable<WeeklyAvailibilityDto> dtos)
        {
            return dtos.Select(dto =>
                new WeeklyAvailability(Guid.NewGuid(), ParseDayOfWeek(dto.Day), ParseTime(dto.StartTime), ParseTime(dto.EndTime), dto.DoctorId)
            );
        }

        private DayOfWeek ParseDayOfWeek(string day)
        {
            if (Enum.TryParse(day, out DayOfWeek result))
            {
                return result;
            }

            throw new ArgumentException("Invalid DayOfWeek value: " + day);
        }

        private TimeSpan ParseTime(string time)
        {
            if (TimeSpan.TryParse(time, out TimeSpan result))
            {
                return result;
            }

            throw new ArgumentException("Invalid TimeSpan value: " + time);
        }

        public async Task SeedAppointments(IEnumerable<AppointmentDto> dtos)
        {
            IEnumerable<Appointment> appointments = GetAppointments(dtos);
            await _dbContext.Appointments.AddRangeAsync(appointments);
            await _dbContext.SaveChangesAsync();
        }

        private IEnumerable<Appointment> GetAppointments(IEnumerable<AppointmentDto> dtos)
        {
            Mock<IClinicTimeChecker> clinicTimeChecker = new Mock<IClinicTimeChecker>();
            Mock<IDoctorTimeChecker> doctorTimeChecker = new Mock<IDoctorTimeChecker>();
            Mock<IAppointmentNumberChecker> appointmentNumberChecker = new Mock<IAppointmentNumberChecker>();
            Mock<IAppointmentDurationChecker> appointmentDurationChecker = new Mock<IAppointmentDurationChecker>();
            Mock<IAppoitmentsOfPatientOverlapChecker> appoitmentsOfPatientOverlapChecker = new Mock<IAppoitmentsOfPatientOverlapChecker>();
            Mock<IDoctorAvailabilityChecker> doctorAvailabilityChecker = new Mock<IDoctorAvailabilityChecker>();
            Mock<IRoomAvailabilityChecker> roomAvailabilityChecker = new Mock<IRoomAvailabilityChecker>();
            Mock<IAppointmentOverlapChecker> appointmentOverlapChecker = new Mock<IAppointmentOverlapChecker>();

            // Set up the mock for rules
            clinicTimeChecker.Setup(
                    x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            doctorTimeChecker.Setup(
                x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(true);

            appointmentNumberChecker.Setup(x => x.IsLessThanTwo(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(true);

            appointmentDurationChecker.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            appoitmentsOfPatientOverlapChecker.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            doctorAvailabilityChecker.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            roomAvailabilityChecker.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            appointmentOverlapChecker.Setup(
                x => x.HasNoConflict(It.IsAny<AppointmentTime>())).Returns(true);

            return dtos.Select(dto =>
                new Appointment(
                    Guid.NewGuid(),
                    new AppointmentTime(ConvertToDateTime(dto.Date, dto.Time), TimeSpan.FromMinutes(dto.Duration)),
                    dto.DoctorId,
                    dto.PatientId,
                    null,
                    clinicTimeChecker.Object,
                    doctorTimeChecker.Object,
                    appointmentNumberChecker.Object,
                    appointmentDurationChecker.Object,
                    appoitmentsOfPatientOverlapChecker.Object,
                    doctorAvailabilityChecker.Object,
                    roomAvailabilityChecker.Object,
                    appointmentOverlapChecker.Object
                )
            );
        }

        public async Task SetAppointment(AppointmentDto appointment)
        {
            ApiResponse<CreateAppointmentResultDto> result = new();

            try
            {
                CreateAppointmentRequest request = new CreateAppointmentRequest(
                    appointment.DoctorId,
                    appointment.PatientId,
                    ConvertToDateTime(appointment.Date, appointment.Time),
                    appointment.Duration);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/appointment/set-appointment", request);
                result = (await response.Content.ReadFromJsonAsync<ApiResponse<CreateAppointmentResultDto>>())!;
            }
            catch (ApiException ex)
            {
                result.Errors.Add(new Error { Message = ex.Message, Code = ex.Code });
            }
            ScenarioContext.SetAppointmentResponse = result;
        }

        public async Task SetEarliestAppointment(AppointmentDto appointment)
        {
            ApiResponse<CreateEarliestAppointmentResultDto> result = new();

            try
            {
                CreateEarliestAppointmentRequest request = new CreateEarliestAppointmentRequest(
                    appointment.DoctorId,
                    appointment.PatientId,
                    appointment.Duration);

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/appointment/set-earliest-appointment", request);
                result = await response.Content.ReadFromJsonAsync<ApiResponse<CreateEarliestAppointmentResultDto>>();
            }
            catch (ApiException ex)
            {
                result.Errors.Add(new Error { Message = ex.Message, Code = ex.Code });
            }
            ScenarioContext.SetEarliestAppointmentResponse = result;
        }

        private DateTime ConvertToDateTime(string dateString, string timeString)
        {
            string dateTimeString = $"{dateString} {timeString}";
            return DateTime.Parse(dateTimeString); // Modify as needed based on input format.
        }

    }
}
