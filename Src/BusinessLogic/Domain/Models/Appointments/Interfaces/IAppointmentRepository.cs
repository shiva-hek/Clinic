using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Models.Appointments.Interfaces;

public interface IAppointmentRepository : IRepository<Appointment>
{

    Appointment? GetConflictingAppointmentByDoctorId(AppointmentTime appointmentTime, Guid doctorId);

    Appointment? GetConflictingAppointmentByRoomId(AppointmentTime appointmentTime, Guid roomId);

    List<Appointment>? GetConflictingAppointments(AppointmentTime appointmentTime);

    Appointment? GetConflictingAppointmentByPatientId(AppointmentTime appointmentTime, Guid patientId);

    int Count(Guid patientId, DateTime startTime);

    Task InsertAsync(Appointment appointment, CancellationToken cancellationToken = default);

    Task<DateTime> GetFirstDoctorFreeTime(Guid doctorId, Guid patientId, TimeSpan duration,
        List<WeeklyAvailability> doctorWeeklyAvailabilities, CancellationToken cancellationToken = default);
}