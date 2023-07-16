using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.ValueObjects;
using Shared.Domain;

namespace Domain.Models.Appointments.Interfaces;

public interface IAppointmentRepository : IRepository
{
    Appointment Get(Guid id);

    Appointment GetByDoctorId(AppointmentTime appointmentTime, Guid doctorId);
    Appointment GetByRoomId(AppointmentTime appointmentTime, Guid roomId);
    Appointment GetByPatientId(AppointmentTime appointmentTime, Guid patientId);
    List<Appointment> GetConflictingAppointments(AppointmentTime appointmentTime);
    int Count(Guid patientId, DateTime startTime);
    Task InsertAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<DateTime> GetFirstDoctorFreeTime(Guid doctorId, TimeSpan duration, CancellationToken cancellationToken = default);
}