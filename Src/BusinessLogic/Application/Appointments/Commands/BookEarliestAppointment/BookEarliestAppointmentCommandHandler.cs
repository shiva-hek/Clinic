using Application.Services;
using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Factories;
using Domain.Models.Appointments.Interfaces;
using MediatR;

namespace Application.Appointments.Commands.BookEarliestAppointment;

public class BookEarliestAppointmentCommandHandler : IRequestHandler<BookEarliestAppointmentRequest>
{
    private readonly IIdService _idService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly AppointmentFactory _appointmentFactory;

    public BookEarliestAppointmentCommandHandler(
        IIdService idService,
        IUnitOfWork unitOfWork,
        IAppointmentRepository appointmentRepository,
        AppointmentFactory appointmentFactory
    )
    {
        _idService = idService;
        _unitOfWork = unitOfWork;
        _appointmentRepository = appointmentRepository;
        _appointmentFactory = appointmentFactory;
    }

    public async Task Handle(BookEarliestAppointmentRequest request, CancellationToken cancellationToken)
    {
        DateTime firsDoctorFreeTime =
            await _unitOfWork.AppointmentRepository.GetFirstDoctorFreeTime(request.DoctorId, request.Duration, cancellationToken);

        Appointment appointment = _appointmentFactory.Create(
            id: _idService.GenerateNewId(),
            startTime: firsDoctorFreeTime,
            duration: request.Duration,
            doctorId: request.DoctorId,
            patientId: request.PatientId);

        await _appointmentRepository.InsertAsync(appointment, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}