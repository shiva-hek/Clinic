using Domain.Models.Appointments.Rules;
using Domain.Models.Appointments.ValueObjects;
using Domain.Services.Appointments;
using Shared.Domain;
using Shared.Exceptions;

namespace Domain.Models.Appointments.Entities
{
    public class Appointment : BaseEntity, IAggregateRoot
    {
        public AppointmentTime AppointmentTime { get; private set; }
        public Guid DoctorId { get; private set; }
        public Guid PatientId { get; private set; }
        public Guid? RoomId { get; set; } = null;
        

        [Obsolete("Reserved for EF Core", true)]
        private Appointment()
        {
        }

        public Appointment(
            Guid id,
            AppointmentTime appointmentTime,
            Guid doctorId,
            Guid patientId,
            Guid? roomId,

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
            AssertionConcern.AssertArgumentNotNull(appointmentTime, ErrorCode.IsNull(nameof(appointmentTime)));
            AssertionConcern.AssertArgumentNotNull(doctorId, ErrorCode.IsNull(nameof(doctorId)));
            AssertionConcern.AssertArgumentNotNull(patientId, ErrorCode.IsNull(nameof(patientId)));

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

            AssertionConcern.AssertRuleNotBroken(
                new AppointmetMustNotOverlapRule(appointmentTime, appointmentOverlapChecker));
            
            if (roomId is not null)
            {
                AssertionConcern.AssertRuleNotBroken(new RoomMustBeAvailableRule(appointmentTime, (Guid)roomId,
                    roomAvailabilityChecker));
            }

            Id = id;
            AppointmentTime = appointmentTime;
            DoctorId = doctorId;
            PatientId = patientId;
            RoomId = roomId;
        }

        public void ChangeAppointmentTime(
            DateTime startTime,
            TimeSpan duration,
            Guid doctorId,
            Guid patientId,
            Guid? roomId,
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
            AssertionConcern.AssertRuleNotBroken(
                new AppointmetMustNotOverlapRule(appointmentTime, appointmentOverlapChecker));
            
            if (roomId is not null)
            {
                AssertionConcern.AssertRuleNotBroken(new RoomMustBeAvailableRule(appointmentTime, (Guid)roomId,
                    roomAvailabilityChecker));
            }

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
            Guid roomId,
            IRoomAvailabilityChecker roomAvailabilityChecker)
        {
            var appointmentTime = new AppointmentTime(startTime, duration);
            
            if (roomId == RoomId)
                return;

            AssertionConcern.AssertRuleNotBroken(new RoomMustBeAvailableRule(appointmentTime, roomId,
                roomAvailabilityChecker));
            
            RoomId = roomId;
        }
    }
}