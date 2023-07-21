using Domain.Models.Appointments.Entities;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;

namespace Domain.Models.Appointments.Factories;

public class AppointmentFactory
{
    private readonly IAppointmentDurationChecker _appointmentDurationChecker;
    private readonly IAppointmentNumberChecker _appointmentNumberChecker;
    private readonly IAppoitmentsOfPatientOverlapChecker _appoitmentsOfPatientOverlapChecker;
    private readonly IClinicTimeChecker _clinicTimeChecker;
    private readonly IDoctorTimeChecker _doctorTimeChecker;
    private readonly IDoctorAvailabilityChecker _doctorAvailabilityChecker;
    private readonly IRoomAvailabilityChecker _roomAvailabilityChecker;
    private readonly IAppointmentOverlapChecker _appointmentOverlapChecker;

    public AppointmentFactory(
        IAppointmentDurationChecker appointmentDurationChecker,
        IAppointmentNumberChecker appointmentNumberChecker,
        IAppoitmentsOfPatientOverlapChecker appoitmentsOfPatientOverlapChecker,
        IClinicTimeChecker clinicTimeChecker,
        IDoctorTimeChecker doctorTimeChecker,
        IDoctorAvailabilityChecker doctorAvailabilityChecker,
        IRoomAvailabilityChecker roomAvailabilityChecker,
        IAppointmentOverlapChecker appointmentOverlapChecker)
    {
        this._appointmentDurationChecker = appointmentDurationChecker;
        this._appointmentNumberChecker = appointmentNumberChecker;
        this._appoitmentsOfPatientOverlapChecker = appoitmentsOfPatientOverlapChecker;
        this._clinicTimeChecker = clinicTimeChecker;
        this._doctorTimeChecker = doctorTimeChecker;
        this._doctorAvailabilityChecker = doctorAvailabilityChecker;
        this._roomAvailabilityChecker = roomAvailabilityChecker;
        this._appointmentOverlapChecker = appointmentOverlapChecker;
    }

    public Appointment Create(
        Guid id,
        DateTime startTime,
        TimeSpan duration,
        Guid doctorId,
        Guid patientId,
        Guid? visitingRoomId = null
    )
    {
        var appoitmentTime = new AppointmentTime(startTime, duration);

        return new Appointment(
            id,
            appoitmentTime,
            doctorId,
            patientId,
            visitingRoomId,
            _clinicTimeChecker,
            _doctorTimeChecker,
            _appointmentNumberChecker,
            _appointmentDurationChecker,
            _appoitmentsOfPatientOverlapChecker,
            _doctorAvailabilityChecker,
            _roomAvailabilityChecker,
            _appointmentOverlapChecker
        );
    }
}