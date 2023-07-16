using MediatR;

namespace Application.Appointments.Commands.BookAppointment;

public record BookAppointmentRequest(
    Guid DoctorId,
    Guid PatientId,
    DateTime StartTime,
    TimeSpan Duration) : IRequest;
