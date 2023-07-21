using Application.Services;
using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Factories;
using Domain.Models.Appointments.Interfaces;
using MediatR;

namespace Application.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentRequest>
{
    private readonly IIdService _idService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly AppointmentFactory _appointmentFactory;

    public CreateAppointmentCommandHandler(
        IIdService idService,
        IAppointmentRepository appointmentRepository,
        AppointmentFactory appointmentFactory
    )
    {
        _idService = idService;
        _appointmentRepository = appointmentRepository;
        _appointmentFactory = appointmentFactory;
    }

    public async Task Handle(CreateAppointmentRequest request, CancellationToken cancellationToken)
    {
        TimeSpan duration = TimeSpan.FromMinutes(request.DurationInMinutes);

        Appointment appointment = _appointmentFactory.Create(
            id: _idService.GenerateNewId(),
            startTime: request.StartTime,
            duration: duration,
            doctorId: request.DoctorId,
            patientId: request.PatientId);

        await _appointmentRepository.InsertAsync(appointment, cancellationToken);
    }
}