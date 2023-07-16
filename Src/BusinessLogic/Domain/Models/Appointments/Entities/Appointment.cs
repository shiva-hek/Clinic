using Domain.Models.Appointments.Rules;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;

namespace Domain.Models.Appointments.Entities
{
    public class Appointment : BaseEntity, IAggregateRoot
    {
        public static readonly Guid DefaultRoomId = new Guid("00000000-0000-0000-0000-000000000000");
        public AppointmentTime AppointmentTime { get; private set; }
        public Guid DoctorId { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid VisitingRoomId { get; set; } = DefaultRoomId;

        [Obsolete("Reserved for EF Core", true)]
        private Appointment()
        {
        }

        public Appointment(
            Guid id,
            AppointmentTime appointmentTime,
            Guid doctorId,
            Guid patientId,
            Guid visitingRoomId,
            IClinicTimeChecker clinicTimeChecker,
            IDoctorTimeChecker doctorTimeChecker,
            IAppointmentNumberChecker appointmentNumberChecker,
            IAppointmentDurationChecker appointmentDurationChecker,
            IAppoitmentsOfPatientOverlapChecker appoitmentsOfPatientOverlapChecker,
            IDoctorAvailabilityChecker doctorAvailabilityChecker,
            IRoomAvailabilityChecker roomAvailabilityChecker,
            IAppointmentOverlapChecker appointmentOverlapChecker
        )
        {
            AssertionConcern.AssertArgumentNotNull(appointmentTime, $"The {nameof(appointmentTime)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(doctorId, $"The {nameof(doctorId)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(patientId, $"The {nameof(patientId)} must be provided.");
            AssertionConcern.AssertArgumentNotNull(visitingRoomId, $"The {nameof(visitingRoomId)} must be provided.");
            AssertionConcern.AssertRuleNotBroken(
                new AppointmentMustBeInClinicWorkingHoursRule(appointmentTime, clinicTimeChecker));
            AssertionConcern.AssertRuleNotBroken(
                new AppointmentMustBeInDoctorWorkingHoursRule(appointmentTime, doctorId, doctorTimeChecker));
            AssertionConcern.AssertRuleNotBroken(
                new APatientMustHaveMaximumTwoAppointmentsPerDayRule(patientId, appointmentTime,
                    appointmentNumberChecker));
            AssertionConcern.AssertRuleNotBroken(
                new AppointmentDurationMustBeValidRule(appointmentTime, doctorId, appointmentDurationChecker));
            AssertionConcern.AssertRuleNotBroken(new AppointmentsOfAPatientMustNotOverLapRule(appointmentTime,
                patientId, appoitmentsOfPatientOverlapChecker));
            AssertionConcern.AssertRuleNotBroken(new DoctorMustBeAvailableRule(appointmentTime, doctorId,
                doctorAvailabilityChecker));
            AssertionConcern.AssertRuleNotBroken(new RoomMustBeAvailableRule(appointmentTime, visitingRoomId,
                roomAvailabilityChecker));
            AssertionConcern.AssertRuleNotBroken(
                new AppointmetMustNotOverlapRule(appointmentTime, appointmentOverlapChecker));


            Id = id;
            AppointmentTime = appointmentTime;
            DoctorId = doctorId;
            PatientId = patientId;
            VisitingRoomId = visitingRoomId;
        }

        public void ChangeAppointmentTime(
            DateTime startTime,
            TimeSpan duration,
            Guid doctorId,
            Guid patientId,
            Guid visitingRoomId,
            IClinicTimeChecker clinicTimeChecker,
            IDoctorTimeChecker doctorTimeChecker,
            IAppointmentNumberChecker appointmentNumberChecker,
            IAppointmentDurationChecker appointmentDurationChecker,
            IAppoitmentsOfPatientOverlapChecker appoitmentsOfPatientOverlapChecker,
            IDoctorAvailabilityChecker doctorAvailabilityChecker,
            IRoomAvailabilityChecker roomAvailabilityChecker,
            IAppointmentOverlapChecker appointmentOverlapChecker)
        {
            var appointmentTime = new AppointmentTime(startTime, duration);

            if (appointmentTime == AppointmentTime)
                return;

            AssertionConcern.AssertRuleNotBroken(
                new AppointmentMustBeInClinicWorkingHoursRule(appointmentTime, clinicTimeChecker));
            AssertionConcern.AssertRuleNotBroken(
                new AppointmentMustBeInDoctorWorkingHoursRule(appointmentTime, doctorId, doctorTimeChecker));
            AssertionConcern.AssertRuleNotBroken(
                new APatientMustHaveMaximumTwoAppointmentsPerDayRule(patientId, appointmentTime,
                    appointmentNumberChecker));
            AssertionConcern.AssertRuleNotBroken(
                new AppointmentDurationMustBeValidRule(appointmentTime, doctorId, appointmentDurationChecker));
            AssertionConcern.AssertRuleNotBroken(new AppointmentsOfAPatientMustNotOverLapRule(appointmentTime,
                patientId, appoitmentsOfPatientOverlapChecker));
            AssertionConcern.AssertRuleNotBroken(new DoctorMustBeAvailableRule(appointmentTime, doctorId,
                doctorAvailabilityChecker));
            AssertionConcern.AssertRuleNotBroken(new RoomMustBeAvailableRule(appointmentTime, visitingRoomId,
                roomAvailabilityChecker));
            AssertionConcern.AssertRuleNotBroken(
                new AppointmetMustNotOverlapRule(appointmentTime, appointmentOverlapChecker));

            AppointmentTime = appointmentTime;
        }

        public void ChangeDoctor(
            DateTime startTime,
            TimeSpan duration,
            Guid doctorId,
            IDoctorAvailabilityChecker doctorAvailabilityChecker)
        {
            var appointmentTime = new AppointmentTime(startTime, duration);

            if (doctorId == DoctorId)
                return;
            
            AssertionConcern.AssertRuleNotBroken(new DoctorMustBeAvailableRule(appointmentTime, doctorId,
                doctorAvailabilityChecker));
            
            DoctorId = doctorId;
        }

        public void ChangePatient(
            DateTime startTime,
            TimeSpan duration,
            Guid patientId,
            IAppointmentNumberChecker appointmentNumberChecker,
            IAppoitmentsOfPatientOverlapChecker appoitmentsOfPatientOverlapChecker)
        {
            var appointmentTime = new AppointmentTime(startTime, duration);
            
            if (patientId == PatientId)
                return;

            AssertionConcern.AssertRuleNotBroken(
                new APatientMustHaveMaximumTwoAppointmentsPerDayRule(patientId, appointmentTime,
                    appointmentNumberChecker));
            AssertionConcern.AssertRuleNotBroken(new AppointmentsOfAPatientMustNotOverLapRule(appointmentTime,
                patientId, appoitmentsOfPatientOverlapChecker));
            
            PatientId = patientId;
        }

        public void ChangeVisitingRoom(
            DateTime startTime,
            TimeSpan duration,
            Guid visitingRoomId,
            IRoomAvailabilityChecker roomAvailabilityChecker)
        {
            var appointmentTime = new AppointmentTime(startTime, duration);
            
            if (visitingRoomId == VisitingRoomId)
                return;

            AssertionConcern.AssertRuleNotBroken(new RoomMustBeAvailableRule(appointmentTime, visitingRoomId,
                roomAvailabilityChecker));
            
            VisitingRoomId = visitingRoomId;
        }
    }
}