using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Enums;
using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Moq;
using Shared.Domain;
using Shared.Exceptions;
using System;
using System.Collections.Generic;

namespace Clinic.UnitTests.Domain.Appointments
{
    public class AppointmentTests
    {
        private Guid invalidId = Guid.Empty;
        private Guid appointmentId = Guid.NewGuid();
        private Guid doctorId = Guid.NewGuid();
        private Guid patientId = Guid.NewGuid();
        private Guid roomId = Guid.NewGuid();


        private Mock<IClinicTimeChecker> clinicTimeCheckerMock;
        private Mock<IDoctorTimeChecker> doctorTimeCheckerMock;
        private Mock<IAppointmentNumberChecker> appointmentNumberCheckerMock;
        private Mock<IAppointmentDurationChecker> appointmentDurationCheckerMock;
        private Mock<IAppoitmentsOfPatientOverlapChecker> appoitmentsOfPatientOverlapCheckerMock;
        private Mock<IDoctorAvailabilityChecker> doctorAvailabilityCheckerMock;
        private Mock<IRoomAvailabilityChecker> roomAvailabilityCheckerMock;
        private Mock<IAppointmentOverlapChecker> appointmentOverlapCheckerMock;
        private Mock<IDoctorEmailAddressUniquenessChecker> doctorEmailAddressUniquenessChecker;

        public AppointmentTests()
        {
            // Set up mocks for dependencies
            clinicTimeCheckerMock = new Mock<IClinicTimeChecker>();
            doctorTimeCheckerMock = new Mock<IDoctorTimeChecker>();
            appointmentNumberCheckerMock = new Mock<IAppointmentNumberChecker>();
            appointmentDurationCheckerMock = new Mock<IAppointmentDurationChecker>();
            appoitmentsOfPatientOverlapCheckerMock = new Mock<IAppoitmentsOfPatientOverlapChecker>();
            doctorAvailabilityCheckerMock = new Mock<IDoctorAvailabilityChecker>();
            roomAvailabilityCheckerMock = new Mock<IRoomAvailabilityChecker>();
            appointmentOverlapCheckerMock = new Mock<IAppointmentOverlapChecker>();
            doctorEmailAddressUniquenessChecker = new Mock<IDoctorEmailAddressUniquenessChecker>();

            // Set up the mock for rules
            clinicTimeCheckerMock.Setup(
                    x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            doctorTimeCheckerMock.Setup(
                x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(true);

            appointmentNumberCheckerMock.Setup(x => x.IsLessThanTwo(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(true);

            appointmentDurationCheckerMock.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            appoitmentsOfPatientOverlapCheckerMock.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            doctorAvailabilityCheckerMock.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            roomAvailabilityCheckerMock.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            appointmentOverlapCheckerMock.Setup(
                x => x.HasNoConflict(It.IsAny<AppointmentTime>())).Returns(true);
        }


        [Fact]
        public void Appointment_ShouldBeAnEntity()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(1), new TimeSpan(0, 15, 0));
            Guid Id = Guid.NewGuid();

            // Act
            var appointment = new Appointment(
                Id,
                appointmentTime,
                doctorId,
                patientId,
                roomId,
                clinicTimeCheckerMock.Object,
                doctorTimeCheckerMock.Object,
                appointmentNumberCheckerMock.Object,
                appointmentDurationCheckerMock.Object,
                appoitmentsOfPatientOverlapCheckerMock.Object,
                doctorAvailabilityCheckerMock.Object,
                roomAvailabilityCheckerMock.Object,
                appointmentOverlapCheckerMock.Object
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
                appointmentId,
                appointmentTime,
                doctorId,
                patientId,
                roomId,
                clinicTimeCheckerMock.Object,
                doctorTimeCheckerMock.Object,
                appointmentNumberCheckerMock.Object,
                appointmentDurationCheckerMock.Object,
                appoitmentsOfPatientOverlapCheckerMock.Object,
                doctorAvailabilityCheckerMock.Object,
                roomAvailabilityCheckerMock.Object,
                appointmentOverlapCheckerMock.Object
            );

            // Assert
            Assert.NotNull(appointment);
            Assert.Equal(appointmentId, appointment.Id);
            Assert.Equal(appointmentTime, appointment.AppointmentTime);
            Assert.Equal(doctorId, appointment.DoctorId);
            Assert.Equal(patientId, appointment.PatientId);
            Assert.Equal(roomId, appointment.RoomId);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenNullAppointmentTime()
        {
            //Arrange
            AppointmentTime appointmentTime = new AppointmentTime(DateTime.Now.AddMinutes(1), new TimeSpan(0, 15, 0));

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    null,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
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
                    appointmentId,
                    appointmentTime,
                    invalidId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
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
                    appointmentId,
                    appointmentTime,
                    doctorId,
                    invalidId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal("103", exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenAppointmentTimeIsOutOfClinicworkingTime()
        {
            //Arrange
            AppointmentTime OuOfRangeTime = new AppointmentTime(DateTime.Today.AddDays(1).Add(new TimeSpan(20, 0, 0)), new TimeSpan(0, 15, 0));

            IClinicTimeChecker clinicTimeChecker = new ClinicTimeChecker();

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    OuOfRangeTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeChecker,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.OutOfClinicWorkingHours.Code, exception.Code);
        }

        [Fact]
        public void Constructor_ShouldTrowException_WhenAppointmentTimeIsOutOfDoctorworkingTime()
        {
            //Arrange
            AppointmentTime OuOfRangeTime = new AppointmentTime(DateTime.Today.AddDays(1).Add(new TimeSpan(20, 0, 0)), new TimeSpan(0, 15, 0));
            EmailAddress doctorEmail = new EmailAddress("doctor1@gmail.com");
            Name doctorName = new Name("John", "Doe");

            List<WeeklyAvailability> WeeklyAvailabilities = new List<WeeklyAvailability>
            {
                new WeeklyAvailability(doctorId, OuOfRangeTime.StartTime.DayOfWeek, new TimeSpan(9, 0, 0), new TimeSpan(12, 0, 0), Guid.NewGuid()),
                new WeeklyAvailability(doctorId, DayOfWeek.Tuesday, new TimeSpan(8, 30, 0), new TimeSpan(12, 30, 0), Guid.NewGuid())
            };

            doctorEmailAddressUniquenessChecker.Setup(
                x => x.IsUnique(It.IsAny<EmailAddress>())).Returns(true);

            Doctor doctor1 = new Doctor(Guid.NewGuid(), doctorName, DoctorType.General, WeeklyAvailabilities, doctorEmail, doctorEmailAddressUniquenessChecker.Object);

            var mockRepository = new Mock<IDoctorRepository>();
            mockRepository.Setup(
                repo => repo.Get(doctor1.Id)).Returns(doctor1);

            IDoctorTimeChecker doctorTimeChecker = new DoctorTimeChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    OuOfRangeTime,
                    doctor1.Id,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeChecker,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
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
                repo => repo.Count(patientId, appointmentTime.StartTime)).Returns(2);

            IAppointmentNumberChecker appointmentNumberChecker = new AppointmentNumberChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    appointmentTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberChecker,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
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

            doctorEmailAddressUniquenessChecker.Setup(
                x => x.IsUnique(It.IsAny<EmailAddress>())).Returns(true);

            Doctor doctor1 = new Doctor(Guid.NewGuid(), doctorName, DoctorType.General, new List<WeeklyAvailability>(), doctorEmail, doctorEmailAddressUniquenessChecker.Object);

            var mockRepository = new Mock<IDoctorRepository>();
            mockRepository.Setup(
                repo => repo.Get(doctor1.Id)).Returns(doctor1);

            IAppointmentDurationChecker appointmentDurationChecker = new AppointmentDurationChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    appointmentTime,
                    doctor1.Id,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationChecker,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
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
                    appointmentId,
                    firstAppointppointmentTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
                );

            var mockRepository = new Mock<IAppointmentRepository>();

            mockRepository.Setup(
                repo => repo.GetConflictingAppointmentByPatientId(seconAppointppointmentTime, patientId)).Returns(appointment1);

            IAppoitmentsOfPatientOverlapChecker appoitmentsOfPatientOverlapChecker = new AppoitmentsOfPatientOverlapChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    seconAppointppointmentTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapChecker,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
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

            clinicTimeCheckerMock.Setup(
                      x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>())).Returns(true);

            doctorTimeCheckerMock.Setup(
                x => x.IsValid(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<Guid>())).Returns(true);

            appointmentNumberCheckerMock.Setup(
                x => x.IsLessThanTwo(It.IsAny<Guid>(), It.IsAny<DateTime>())).Returns(true);

            appointmentDurationCheckerMock.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            appoitmentsOfPatientOverlapCheckerMock.Setup(
                x => x.IsValid(It.IsAny<Guid>(), It.IsAny<AppointmentTime>())).Returns(true);

            doctorAvailabilityCheckerMock.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            roomAvailabilityCheckerMock.Setup(
                x => x.IsAvailable(It.IsAny<AppointmentTime>(), It.IsAny<Guid>())).Returns(true);

            appointmentOverlapCheckerMock.Setup(
                x => x.HasNoConflict(It.IsAny<AppointmentTime>())).Returns(true);

            Appointment appointment1 = new Appointment(
                    appointmentId,
                    firstAppointppointmentTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
                );

            var mockRepository = new Mock<IAppointmentRepository>();

            mockRepository.Setup(
                repo => repo.GetConflictingAppointments(seconAppointppointmentTime)).Returns(new List<Appointment> { appointment1 });

            IAppointmentOverlapChecker appointmentOverlapChecker = new AppointmentOverlapChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    seconAppointppointmentTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
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
                    appointmentId,
                    firstAppointppointmentTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityCheckerMock.Object,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
                );

            var mockRepository = new Mock<IAppointmentRepository>();

            mockRepository.Setup(
                repo => repo.GetConflictingAppointmentByDoctorId(seconAppointppointmentTime, doctorId)).Returns(appointment1);

            IDoctorAvailabilityChecker doctorAvailabilityChecker = new DoctorAvailabilityChecker(mockRepository.Object);

            // Act
            var exception = Assert.Throws<ApiException>(() =>
                new Appointment(
                    appointmentId,
                    seconAppointppointmentTime,
                    doctorId,
                    patientId,
                    roomId,
                    clinicTimeCheckerMock.Object,
                    doctorTimeCheckerMock.Object,
                    appointmentNumberCheckerMock.Object,
                    appointmentDurationCheckerMock.Object,
                    appoitmentsOfPatientOverlapCheckerMock.Object,
                    doctorAvailabilityChecker,
                    roomAvailabilityCheckerMock.Object,
                    appointmentOverlapCheckerMock.Object
                )
            );

            //Assert
            Assert.Equal(ErrorCode.DoctorIsBusy.Code, exception.Code);
        }
    }
}
