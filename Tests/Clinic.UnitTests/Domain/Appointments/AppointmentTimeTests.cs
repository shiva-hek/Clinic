using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.ValueObjects;
using Shared.Exceptions;

namespace Clinic.UnitTests.Domain.Appointments
{
    public class AppointmentTimeTests
    {
        [Fact]
        public void Constructor_ShouldTrowException_WhenStartTimeIsInThePast()
        {
            // Arrange
            DateTime pastStartTime = DateTime.Now.AddMinutes(-10);
            TimeSpan duration = TimeSpan.FromMinutes(30);

            // Act and Assert
            var exception = Assert.Throws<ApiException>(() =>new AppointmentTime(pastStartTime, duration));

            //Assert
            Assert.Equal(ErrorCode.StartTimeValidation.Code, exception.Code);
        }

        [Fact]
        public void DurationIsGreaterThanZero()
        {
            // Arrange
            DateTime validStartTime = DateTime.Now.AddMinutes(30);
            TimeSpan zeroDuration = TimeSpan.Zero;

            // Act and Assert
            var exception = Assert.Throws<ApiException>(() =>new AppointmentTime(validStartTime, zeroDuration));

            //Assert
            Assert.Equal(ErrorCode.DurationValidation.Code, exception.Code);
        }

        [Fact]
        public void OverlapsWith_OverlappingAppointments_ShouldReturnTrue()
        {
            // Arrange
            DateTime startTime1 = DateTime.Now.AddHours(2);
            TimeSpan duration1 = TimeSpan.FromMinutes(45);
            AppointmentTime appointment1 = new AppointmentTime(startTime1, duration1);

            DateTime startTime2 = DateTime.Now.AddHours(1).AddMinutes(30);
            TimeSpan duration2 = TimeSpan.FromMinutes(60);
            AppointmentTime appointment2 = new AppointmentTime(startTime2, duration2);

            // Act
            bool overlaps = appointment1.OverlapsWith(appointment2);

            // Assert
            Assert.True(overlaps);
        }

        [Fact]
        public void OverlapsWith_NonOverlappingAppointments_ShouldReturnFalse()
        {
            // Arrange
            DateTime startTime1 = DateTime.Now.AddHours(2);
            TimeSpan duration1 = TimeSpan.FromMinutes(45);
            AppointmentTime appointment1 = new AppointmentTime(startTime1, duration1);

            DateTime startTime2 = DateTime.Now.AddHours(3);
            TimeSpan duration2 = TimeSpan.FromMinutes(60);
            AppointmentTime appointment2 = new AppointmentTime(startTime2, duration2);

            // Act
            bool overlaps = appointment1.OverlapsWith(appointment2);

            // Assert
            Assert.False(overlaps);
        }
    }
}

