using Application.Services;
using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Factories;
using Domain.Models.Appointments.Interfaces;
using MediatR;

namespace Application.Appointments.Commands.CreateEarliestAppointment;

public class CreateEarliestAppointmentCommandHandler : IRequestHandler<CreateEarliestAppointmentRequest, CreateEarliestAppointmentResultDto>
{
    private readonly IIdService _idService;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IWeeklyAvailabilityRepository _weeklyAvailabilityRepository;
    private readonly AppointmentFactory _appointmentFactory;

    public CreateEarliestAppointmentCommandHandler(
        IIdService idService,
        IAppointmentRepository appointmentRepository,
        IWeeklyAvailabilityRepository weeklyAvailabilityRepository,
        AppointmentFactory appointmentFactory
    )
    {
        _idService = idService;
        _appointmentRepository = appointmentRepository;
        _weeklyAvailabilityRepository = weeklyAvailabilityRepository;
        _appointmentFactory = appointmentFactory;
    }

    public async Task<CreateEarliestAppointmentResultDto> Handle(CreateEarliestAppointmentRequest request, CancellationToken cancellationToken)
    {
        TimeSpan duration = TimeSpan.FromMinutes(request.DurationInMinutes);

        List<WeeklyAvailability> doctorWorkingTimes =
            await _weeklyAvailabilityRepository.GetDoctorWorkingTime(request.DoctorId);

        DateTime firsDoctorFreeTime =
            await _appointmentRepository.GetFirstDoctorFreeTime(request.DoctorId,request.PatientId, duration,
                doctorWorkingTimes, cancellationToken);

        Appointment appointment = _appointmentFactory.Create(
            id: _idService.GenerateNewId(),
            startTime: firsDoctorFreeTime,
            duration: duration,
            doctorId: request.DoctorId,
            patientId: request.PatientId);

        await _appointmentRepository.InsertAsync(appointment, cancellationToken);

        CreateEarliestAppointmentResultDto resultDto = new CreateEarliestAppointmentResultDto
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            StartTime = firsDoctorFreeTime
        };

        return resultDto;
    }
}