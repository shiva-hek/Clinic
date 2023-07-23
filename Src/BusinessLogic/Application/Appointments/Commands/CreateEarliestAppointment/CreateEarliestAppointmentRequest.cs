using Application.Appointments.Commands.CreateAppointment;
using MediatR;

namespace Application.Appointments.Commands.CreateEarliestAppointment;

public record CreateEarliestAppointmentRequest(
    Guid DoctorId,
    Guid PatientId,
    int DurationInMinutes) : IRequest<CreateEarliestAppointmentResultDto>;