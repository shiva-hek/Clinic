using MediatR;

namespace Application.Appointments.Commands.CreateAppointment;

public record CreateAppointmentRequest(
    Guid DoctorId,
    Guid PatientId,
    DateTime StartTime,
    int DurationInMinutes) : IRequest;
