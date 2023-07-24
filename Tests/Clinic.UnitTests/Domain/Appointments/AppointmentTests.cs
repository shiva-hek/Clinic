using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Enums;
using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Moq;
using Shared.Domain;
using Shared.Exceptions;

namespace Clinic.UnitTests.Domain.Appointments
{
    public class AppointmentTests
    {
        private Guid _invalidId = Guid.Empty;
        private Guid _appointmentId = Guid.NewGuid();
        private Guid _doctorId = Guid.NewGuid();
        private Guid _patientId = Guid.NewGuid();
        private Guid _roomId = Guid.NewGuid();


        private Mock<IClinicTimeChecker> _clinicTimeCheckerMock;
        private Mock<IDoctorTimeChecker> _doctorTimeCheckerMock;
        private Mock<IAppointmentNumberChecker> _appointmentNumberCheckerMock;
        private Mock<IAppointmentDurationChecker> _appointmentDurationCheckerMock;
        private Mock<IAppoitmentsOfPatientOverlapChecker> _appoitmentsOfPatientOverlapCheckerMock;
        private Mock<IDoctorAvailabilityChecker> _doctorAvailabilityCheckerMock;
        private Mock<IRoomAvailabilityChecker> _roomAvailabilityCheckerMock;
        private Mock<IAppointmentOverlapChecker> _appointmentOverlapCheckerMock;
        private Mock<IDoctorEmailAddressUniquenessChecker> _doctorEmailAddressUniquenessChecker;

        public AppointmentTests()
        {
            // Set up mocks for dependencies
            _clinicTimeCheckerMock = new Mock<IClinicTimeChecker>();
            _doctorTimeCheckerMock = new Mock<IDoctorTimeChecker>();
            _appointmentNumberCheckerMock = new Mock<IAppointmentNumberChecker>();
            _appointmentDurationCheckerMock = new Mock<IAppointmentDurationChecker>();
            _appoitmentsOfPatientOverlapCheckerMock = new Mock<IAppoitmentsOfPatientOverlapChecker>();
            _doctorAvailabilityCheckerMock = new Mock<IDoctorAvailabilityChecker>();
            _roomAvailabilityCheckerMock = new Mock<IRoomAvailabilityChecker>();
            _appointmentOverlapCheckerMock = new Mock<IAppointmentOverlapChecker>();
            _doctorEmailAddressUniquenessChecker = new Mock<IDoctorEmailAddressUniquenessChecker>();

            // Set up the mock for rules
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
        }


        [Fact]
        public void Appointment_ShouldBeAnEntity()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(1), new TimeSpan(0, 15, 0));
            Guid id = Guid.NewGuid();

            // Act
            var appointment = new Appointment(
                id,
                appointmentTime,
                _doctorId,
                _patientId,
                _roomId,
                _clinicTimeCheckerMock.Object,
                _doctorTimeCheckerMock.Object,
                _appointmentNumberCheckerMock.Object,
                _appointmentDurationCheckerMock.Object,
                _appoitmentsOfPatientOverlapCheckerMock.Object,
                _doctorAvailabilityCheckerMock.Object,
                _roomAvailabilityCheckerMock.Object,
                _appointmentOverlapCheckerMock.Object
            );

            // Assert
            Assert.IsAssignableFrom<Entity<Guid>>(appointment);
        }

        [Fact]
        public void Constructor_ShouldCreateAppointment_WhendValidParameters()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(1), new TimeSpan(0, 15, 0));

            // Act
            var appointment = new Appointment(
                _appointmentId,
                appointmentTime,
                _doctorId,
                _patientId,
                _roomId,
                _clinicTimeCheckerMock.Object,
                _doctorTimeCheckerMock.Object,
                _appointmentNumberCheckerMock.Object,
                _appointmentDurationCheckerMock.Object,
                _appoitmentsOfPatientOverlapCheckerMock.Object,
                _doctorAvailabilityCheckerMock.Object,
                _roomAvailabilityCheckerMock.Object,
                _appointmentOverlapCheckerMock.Object
            );

            // Assert
            Assert.NotNull(appointment);
            Assert.Equal(_appointmentId, appointment.Id);
            Assert.Equal(appointmentTime, appointment.AppointmentTime);
            Assert.Equal(_doctorId, appointment.DoctorId);
            Assert.Equal(_patientId, appointment.PatientId);
            Assert.Equal(_roomId, appointment.RoomId);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenNullAppointmentTime()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(1), new TimeSpan(0, 15, 0));

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    null,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal("101", exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenInvalidDoctorId()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(1), new TimeSpan(0, 15, 0));

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    appointmentTime,
                    _invalidId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal("103", exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenInvalidPatientId()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(1), new TimeSpan(0, 15, 0));

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    appointmentTime,
                    _doctorId,
                    _invalidId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal("103", exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenAppointmentTimeIsOutOfClinicworkingTime()
        {
            //Arrange
            AppointmentTime ouOfRangeTime = new AppointmentTime(DateTime.Today.AddDays(1).Add(new TimeSpan(20, 0, 0)), new TimeSpan(0, 15, 0));

            IClinicTimeChecker clinicTimeChecker = new ClinicTimeChecker();

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    ouOfRangeTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    clinicTimeChecker,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.OutOfClinicWorkingHours.Code, exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenAppointmentTimeIsOutOfDoctorworkingTime()
        {
            //Arrange
            AppointmentTime ouOfRangeTime = new AppointmentTime(DateTime.Today.AddDays(1).Add(new TimeSpan(20, 0, 0)), new TimeSpan(0, 15, 0));
            EmailAddress doctorEmail = new EmailAddress("doctor1@gmail.com");
            Name doctorName = new Name("John", "Doe");

            List<WeeklyAvailability> weeklyAvailabilities = new List<WeeklyAvailability>
            {
                new WeeklyAvailability(_doctorId, ouOfRangeTime.StartTime.DayOfWeek, new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0), Guid.NewGuid()),
                new WeeklyAvailability(_doctorId, DayOfWeek.Tuesday, new TimeSpan(8, 30, 0), new TimeSpan(12, 30, 0), Guid.NewGuid())
            };

            _doctorEmailAddressUniquenessChecker.Setup(
                x => x.IsUnique(It.IsAny<EmailAddress>())).Returns(true);

            Doctor doctor1 = new Doctor(Guid.NewGuid(), doctorName, DoctorType.General, weeklyAvailabilities, doctorEmail, _doctorEmailAddressUniquenessChecker.Object);

            var mockRepository = new Mock<IDoctorRepository>();
            mockRepository.Setup(
                repo => repo.Get(doctor1.Id)).Returns(doctor1);

            IDoctorTimeChecker doctorTimeChecker = new DoctorTimeChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    ouOfRangeTime,
                    doctor1.Id,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    doctorTimeChecker,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.OutOfDoctorWorkingHours.Code, exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhendPatientHasTwoAppintmentInADay()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(5), new TimeSpan(0, 15, 0));

            var mockRepository = new Mock<IAppointmentRepository>();
            mockRepository.Setup(
                repo => repo.Count(_patientId, appointmentTime.StartTime)).Returns(2);

            IAppointmentNumberChecker appointmentNumberChecker = new AppointmentNumberChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    appointmentTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    appointmentNumberChecker,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.NumberOfPatientAppointments.Code, exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenInvalidDuration()
        {
            //Arrange 
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Today.AddDays(1), new TimeSpan(0, 30, 0));
            EmailAddress doctorEmail = new EmailAddress("doctor1@gmail.com");
            Name doctorName = new Name("John", "Doe");

            _doctorEmailAddressUniquenessChecker.Setup(
                x => x.IsUnique(It.IsAny<EmailAddress>())).Returns(true);

            Doctor doctor1 = new Doctor(Guid.NewGuid(), doctorName, DoctorType.General, new List<WeeklyAvailability>(), doctorEmail, _doctorEmailAddressUniquenessChecker.Object);

            var mockRepository = new Mock<IDoctorRepository>();
            mockRepository.Setup(
                repo => repo.Get(doctor1.Id)).Returns(doctor1);

            IAppointmentDurationChecker appointmentDurationChecker = new AppointmentDurationChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    appointmentTime,
                    doctor1.Id,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    appointmentDurationChecker,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.InvalidDurationWithDoctorType.Code, exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenAppointmentsOfAPatientHasOverlap()
        {
            //Arrange 
            AppointmentTime firstAppointppointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(5), new TimeSpan(0, 15, 0));
            AppointmentTime seconAppointppointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(10), new TimeSpan(0, 15, 0));

            Appointment appointment1 = new Appointment(
                    _appointmentId,
                    firstAppointppointmentTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                );

            var mockRepository = new Mock<IAppointmentRepository>();

            mockRepository.Setup(
                repo => repo.GetConflictingAppointmentByPatientId(seconAppointppointmentTime, _patientId)).Returns(appointment1);

            IAppoitmentsOfPatientOverlapChecker appoitmentsOfPatientOverlapChecker = new AppoitmentsOfPatientOverlapChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    seconAppointppointmentTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapChecker,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.OverlapWithPatientAppointment.Code, exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenAppointmentHasOverlap()
        {
            //Arrange 
            AppointmentTime firstAppointppointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(5), new TimeSpan(0, 15, 0));
            AppointmentTime seconAppointppointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(10), new TimeSpan(0, 15, 0));

            _clinicTimeCheckerMock.Setup(
                      x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            _doctorTimeCheckerMock.Setup(
                x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(true);

            _appointmentNumberCheckerMock.Setup(
                x => x.IsLessThanTwo(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(true);

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

            Appointment appointment1 = new Appointment(
                    _appointmentId,
                    firstAppointppointmentTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                );

            var mockRepository = new Mock<IAppointmentRepository>();

            mockRepository.Setup(
                repo => repo.GetConflictingAppointments(seconAppointppointmentTime)).Returns(new List<Appointment> { appointment1 });

            IAppointmentOverlapChecker appointmentOverlapChecker = new AppointmentOverlapChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    seconAppointppointmentTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    appointmentOverlapChecker
                )
            );

            //Assert
            Assert.Equal(ErrorCode.Overlap.Code, exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenDoctorIsNotAvailable()
        {
            //Arrange 
            AppointmentTime firstAppointppointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(5), new TimeSpan(0, 15, 0));
            AppointmentTime seconAppointppointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(10), new TimeSpan(0, 15, 0));

            Appointment appointment1 = new Appointment(
                    _appointmentId,
                    firstAppointppointmentTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    _doctorAvailabilityCheckerMock.Object,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                );

            var mockRepository = new Mock<IAppointmentRepository>();

            mockRepository.Setup(
                repo => repo.GetConflictingAppointmentByDoctorId(seconAppointppointmentTime, _doctorId)).Returns(appointment1);

            IDoctorAvailabilityChecker doctorAvailabilityChecker = new DoctorAvailabilityChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    _appointmentId,
                    seconAppointppointmentTime,
                    _doctorId,
                    _patientId,
                    _roomId,
                    _clinicTimeCheckerMock.Object,
                    _doctorTimeCheckerMock.Object,
                    _appointmentNumberCheckerMock.Object,
                    _appointmentDurationCheckerMock.Object,
                    _appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityChecker,
                    _roomAvailabilityCheckerMock.Object,
                    _appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.DoctorIsBusy.Code, exception.Code);
        }
    }
}
