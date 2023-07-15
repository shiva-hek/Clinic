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
    List<Appointment> GetconflictingAppointments(AppointmentTime appointmentTime);
    int Count(Guid patientId, DateTime startTime);
    Task InsertAsync(Appointment appointment);
    Task<DateTime> GetfirstFreeTime(Guid doctorId, Guid patientId, TimeSpan duration);
}