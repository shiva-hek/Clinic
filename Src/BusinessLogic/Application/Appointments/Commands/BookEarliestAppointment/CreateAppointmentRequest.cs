using MediatR;

namespace Application.Appointments.Commands.BookEarliestAppointment;

public record BookEarliestAppointmentRequest (
    Guid DoctorId,
    Guid PatientId,
    TimeSpan Duration) : IRequest;