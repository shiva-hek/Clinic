using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;

namespace Domain.Services.Appointments;

public class RoomAvailabilityChecker:IRoomAvailabilityChecker
{
    private readonly IAppointmentRepository _appointmentRepository;

    public RoomAvailabilityChecker(IAppointmentRepository appointmentRepository)
    {
        _appointmentRepository = appointmentRepository;
    }

    public bool IsAvailable(AppointmentTime appointmentTime, Guid roomId)
    {
        return _appointmentRepository.GetConflictingAppointmentByRoomId(appointmentTime, roomId) == null;
    }
}