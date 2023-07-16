using Application.Services;
using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Factories;
using Domain.Models.Appointments.Interfaces;
using MediatR;

namespace Application.Appointments.Commands.BookAppointment;

public class BookAppointmentCommandHandler : IRequestHandler<BookAppointmentRequest>
{
    private readonly IIdService _idService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly AppointmentFactory _appointmentFactory;

    public BookAppointmentCommandHandler(
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

    public async Task Handle(BookAppointmentRequest request, CancellationToken cancellationToken)
    {
        Appointment appointment = _appointmentFactory.Create(
            id: _idService.GenerateNewId(),
            startTime: request.StartTime,
            duration: request.Duration,
            doctorId: request.DoctorId,
            patientId: request.PatientId);

        await _unitOfWork.AppointmentRepository.InsertAsync(appointment, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);
    }
}