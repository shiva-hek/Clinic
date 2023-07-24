using Application.Appointments.Commands.CreateAppointment;
using Application.Appointments.Commands.CreateEarliestAppointment;
using Clinic.AcceptanceTests.Dtos;
using Shared.Model;

namespace Clinic.AcceptanceTests.Context
{
    public class AppointmentContext
    {
        public AppointmentDto Appointment { get; set; } = null!;
        public ApiResponse<CreateAppointmentResultDto> SetAppointmentResponse { get; set; } = null!;
        public ApiResponse<CreateEarliestAppointmentResultDto> SetEarliestAppointmentResponse { get; set; } = null!;
    }
}
