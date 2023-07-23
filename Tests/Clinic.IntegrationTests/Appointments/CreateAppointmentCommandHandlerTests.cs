using Application.Appointments.Commands.CreateAppointment;
using Application.Appointments.Commands.CreateEarliestAppointment;
using Application.Services;
using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Factories;
using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using FluentAssertions;
using Moq;

namespace Clinic.IntegrationTests.Appointments
{
    public class CreateAppointmentCommandHandlerTests
    {
        private CreateAppointmentCommandHandler _setHandler;
        private CreateEarliestAppointmentCommandHandler _setEarliestHandler;
        private AppointmentFactory _appointmentFactory;

        private Mock<IIdService> _idServiceMock;
        private Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private Mock<IWeeklyAvailabilityRepository> _weeklyAvailabilityRepository;
        private Mock<IAppointmentDurationChecker> _appointmentDurationCheckerMock;
        private Mock<IAppointmentNumberChecker> _appointmentNumberCheckerMock;
        private Mock<IAppoitmentsOfPatientOverlapChecker> _appoitmentsOfPatientOverlapCheckerMock;
        private Mock<IClinicTimeChecker> _clinicTimeCheckerMock;
        private Mock<IDoctorTimeChecker> _doctorTimeCheckerMock;
        private Mock<IDoctorAvailabilityChecker> _doctorAvailabilityCheckerMock;
        private Mock<IRoomAvailabilityChecker> _roomAvailabilityCheckerMock;
        private Mock<IAppointmentOverlapChecker> _appointmentOverlapCheckerMock;
        

        public CreateAppointmentCommandHandlerTests()
        {
            _idServiceMock = new Mock<IIdService>();
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _weeklyAvailabilityRepository = new Mock<IWeeklyAvailabilityRepository>();

            MockFactory();

            _setHandler = new CreateAppointmentCommandHandler(
                _idServiceMock.Object,
                _appointmentRepositoryMock.Object,
                _appointmentFactory
            );

            _setEarliestHandler = new CreateEarliestAppointmentCommandHandler(
               _idServiceMock.Object,
               _appointmentRepositoryMock.Object,
               _weeklyAvailabilityRepository.Object,
               _appointmentFactory
           );
        }

        [Fact]
        public async Task Handle_ShouldSetAppointment_WhenRequestIsValid()
        {
            // Arrange
            CreateAppointmentRequest request = new(
                DoctorId: Guid.NewGuid(),
                PatientId: Guid.NewGuid(),
                StartTime: DateTime.Today.AddDays(1).Add(new TimeSpan(12, 0, 0)),
                DurationInMinutes: 15);

            var generatedId = Guid.NewGuid();
            _idServiceMock.Setup(service => service.GenerateNewId()).Returns(generatedId);

            // Act
            var result = await _setHandler.Handle(request, CancellationToken.None);


            result.Should().BeEquivalentTo(new CreateAppointmentResultDto
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                StartTime = request.StartTime
            });
        }

        [Fact]
        public async Task Handle_ShouldSetEarliestAppointment_WhenRequestIsValid()
        {
            // Arrange
            CreateEarliestAppointmentRequest request = new(
                DoctorId: Guid.NewGuid(),
                PatientId: Guid.NewGuid(),
                DurationInMinutes: 15);


            DateTime firstDoctorFreeTime = SetupMockRepositories(request.DoctorId, request.PatientId, request.DurationInMinutes);

            // Act
            var result = await _setEarliestHandler.Handle(request, CancellationToken.None);

            //assert
            result.Should().BeEquivalentTo(new CreateAppointmentResultDto
            {
                DoctorId = request.DoctorId,
                PatientId = request.PatientId,
                StartTime = firstDoctorFreeTime
            });
        }

        private void MockFactory()
        {
            _appointmentDurationCheckerMock = new Mock<IAppointmentDurationChecker>();
            _appointmentNumberCheckerMock = new Mock<IAppointmentNumberChecker>();
            _appoitmentsOfPatientOverlapCheckerMock = new Mock<IAppoitmentsOfPatientOverlapChecker>();
            _clinicTimeCheckerMock = new Mock<IClinicTimeChecker>();
            _doctorTimeCheckerMock = new Mock<IDoctorTimeChecker>();
            _doctorAvailabilityCheckerMock = new Mock<IDoctorAvailabilityChecker>();
            _roomAvailabilityCheckerMock = new Mock<IRoomAvailabilityChecker>();
            _appointmentOverlapCheckerMock = new Mock<IAppointmentOverlapChecker>();

            _clinicTimeCheckerMock.Setup(
                    x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            _doctorTimeCheckerMock.Setup(
                x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(true);

            _appointmentNumberCheckerMock.Setup(x => x.IsLessThanTwo(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(true);

            _appointmentDurationCheckerMock.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            _appoitmentsOfPatientOverlapCheckerMock.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            _doctorAvailabilityCheckerMock.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            _roomAvailabilityCheckerMock.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            _appointmentOverlapCheckerMock.Setup(
                x => x.HasNoConflict(It.IsAny<AppointmentTime>())).Returns(true);

            _appointmentFactory = new AppointmentFactory(
               _appointmentDurationCheckerMock.Object,
               _appointmentNumberCheckerMock.Object,
               _appoitmentsOfPatientOverlapCheckerMock.Object,
               _clinicTimeCheckerMock.Object,
               _doctorTimeCheckerMock.Object,
               _doctorAvailabilityCheckerMock.Object,
               _roomAvailabilityCheckerMock.Object,
               _appointmentOverlapCheckerMock.Object
           );
        }

        private DateTime SetupMockRepositories(Guid doctorId, Guid patientId, int duration)
        {
            Guid generatedId = Guid.NewGuid();
            _idServiceMock.Setup(service => service.GenerateNewId()).Returns(generatedId);

            List<WeeklyAvailability> doctorWorkingTimes = new List<WeeklyAvailability>
            {
                new WeeklyAvailability(doctorId, DayOfWeek.Monday, new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0), Guid.NewGuid()),
                new WeeklyAvailability(doctorId, DayOfWeek.Tuesday, new TimeSpan(8, 30, 0), new TimeSpan(12, 30, 0), Guid.NewGuid())
            };

            _weeklyAvailabilityRepository
                .Setup(repo => repo.GetDoctorWorkingTime(doctorId))
                .ReturnsAsync(doctorWorkingTimes);

            var firstDoctorFreeTime = DateTime.UtcNow.AddDays(1);

            _appointmentRepositoryMock
                .Setup(repo => repo.GetFirstDoctorFreeTime(
                    doctorId,
                    patientId,
                    TimeSpan.FromMinutes(duration),
                    doctorWorkingTimes,
                    CancellationToken.None
                ))
                .ReturnsAsync(firstDoctorFreeTime);

            return firstDoctorFreeTime;
        }
    }
}
