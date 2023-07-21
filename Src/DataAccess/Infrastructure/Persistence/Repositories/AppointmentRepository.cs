using Domain.Models.Appointments.Common;
using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.Interfaces;
using Domain.Models.Appointments.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppointmentDbContext _dbContext;

    public AppointmentRepository(AppointmentDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Appointment? Get(Guid id)
    {
        return _dbContext.Appointments.FirstOrDefault(a => a.Id == id);
    }

    public Appointment? GetConflictingAppointmentByDoctorId(AppointmentTime appointmentTime, Guid doctorId)
    {
        List<Appointment> allAppointments = _dbContext.Appointments
            .Where(a => a.DoctorId == doctorId)
            .ToList();

        Appointment? firstConflictingAppointment = allAppointments
            .FirstOrDefault(a => a.AppointmentTime.OverlapsWith(appointmentTime));

        return firstConflictingAppointment;
    }

    public Appointment? GetConflictingAppointmentByRoomId(AppointmentTime appointmentTime, Guid roomId)
    {
        List<Appointment> allAppointments = _dbContext.Appointments
            .Where(a => a.RoomId == roomId)
            .ToList();

        Appointment? firstConflictingAppointment = allAppointments
            .Where(a => a.AppointmentTime.OverlapsWith(appointmentTime))
            .FirstOrDefault();

        return firstConflictingAppointment;
    }

    public List<Appointment> GetConflictingAppointments(AppointmentTime appointmentTime)
    {
        DateTime startTimeWithBuffer = appointmentTime.StartTime.AddMinutes(-30);
        DateTime endTimeWithBuffer = appointmentTime.EndTime.AddMinutes(30);

        List<Appointment> allAppointments = _dbContext.Appointments
            .Where(a => a.AppointmentTime.StartTime < endTimeWithBuffer &&
                        a.AppointmentTime.EndTime > startTimeWithBuffer)
            .ToList();

        List<Appointment> conflictingAppointments = allAppointments
            .Where(a => a.AppointmentTime.OverlapsWith(appointmentTime))
            .ToList();

        return conflictingAppointments;
    }

    public Appointment? GetConflictingAppointmentByPatientId(AppointmentTime appointmentTime, Guid patientId)
    {
        List<Appointment> allAppointments = _dbContext.Appointments
            .Where(a => a.PatientId == patientId)
            .ToList();

        Appointment? firstConflictingAppointment = allAppointments
            .Where(a => a.AppointmentTime.OverlapsWith(appointmentTime))
            .FirstOrDefault();

        return firstConflictingAppointment;
    }

    public int Count(Guid patientId, DateTime startTime)
    {
        DateTime appointmentDate = startTime.Date;

        return _dbContext.Appointments
            .Count(a => a.PatientId == patientId &&
                        a.AppointmentTime.StartTime.Date == appointmentDate);
    }

    public async Task InsertAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await _dbContext.AddAsync(appointment, cancellationToken);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<DateTime> GetFirstDoctorFreeTime(Guid doctorId, Guid patientId, TimeSpan duration,
        List<WeeklyAvailability> doctorWeeklyAvailabilities, CancellationToken cancellationToken = default)
    {
        int numberOfMonths = AppConfig.TimeLimitationToSetAppointment;

        DateTime currentTime = DateTime.Now.AddMinutes(1);

        List<Appointment> doctorAppointments =
            await _dbContext.Appointments
                .Where(a => a.DoctorId == doctorId && a.AppointmentTime.StartTime >= currentTime)
                .OrderBy(a => a.AppointmentTime.StartTime)
                .ToListAsync(cancellationToken);

        return GetFirstFreeTime(duration, doctorWeeklyAvailabilities, numberOfMonths, doctorAppointments, patientId);
    }

    private DateTime GetFirstFreeTime(TimeSpan duration, List<WeeklyAvailability> doctorWeeklyAvailabilities,
        int numberOfMonths, List<Appointment> doctorAppointments, Guid patientId)
    {
        DateTime currentDateTime = DateTime.Now.AddMinutes(1);
        DateTime endDateTime = currentDateTime.AddMonths(numberOfMonths);
        TimeSpan timeInterval = TimeSpan.FromMinutes(15);

        // Generate a list of all possible time slots within the next 6 months
        List<DateTime> allTimeSlots = new List<DateTime>();
        while (currentDateTime < endDateTime)
        {
            foreach (var weeklyAvailability in doctorWeeklyAvailabilities)
            {
                int patientReservedAppointmetscount = _dbContext.Appointments
                    .Count(a => a.PatientId == patientId &&
                                a.AppointmentTime.StartTime.Date == currentDateTime.Date);

                DateTime startTime = new DateTime(
                    currentDateTime.Year, currentDateTime.Month, currentDateTime.Day,
                    weeklyAvailability.StartTime.Hours, weeklyAvailability.StartTime.Minutes, 0);

                DateTime endTime = new DateTime(
                    currentDateTime.Year, currentDateTime.Month, currentDateTime.Day,
                    weeklyAvailability.EndTime.Hours, weeklyAvailability.EndTime.Minutes, 0);

                if (currentDateTime.DayOfWeek == weeklyAvailability.Day &&
                    startTime.TimeOfDay >= weeklyAvailability.StartTime &&
                    endTime.TimeOfDay <= weeklyAvailability.EndTime &&
                    patientReservedAppointmetscount < 2
                   )
                {
                    DateTime availabilityStart = currentDateTime.Date.Add(weeklyAvailability.StartTime);
                    DateTime availabilityEnd = currentDateTime.Date.Add(weeklyAvailability.EndTime);

                    while (availabilityStart.Add(duration) <= availabilityEnd)
                    {
                        allTimeSlots.Add(availabilityStart);
                        availabilityStart = availabilityStart.Add(timeInterval);
                    }
                }
            }

            currentDateTime = currentDateTime.AddDays(1);
        }

        // Check each time slot to see if it overlaps with any existing appointments
        foreach (var timeSlot in allTimeSlots)
        {
            bool isTimeSlotAvailable = true;
            DateTime endTimeSlot = timeSlot.Add(duration);

            foreach (var appointment in doctorAppointments)
            {
                DateTime appointmentStart = appointment.AppointmentTime.StartTime;
                DateTime appointmentEnd = appointment.AppointmentTime.EndTime;

                if (timeSlot < appointmentEnd && endTimeSlot > appointmentStart)
                {
                    isTimeSlotAvailable = false;
                    break;
                }
            }

            if (isTimeSlotAvailable)
            {
                return timeSlot;
            }
        }

        // If no available time slot is found
        return DateTime.MinValue;
    }
}