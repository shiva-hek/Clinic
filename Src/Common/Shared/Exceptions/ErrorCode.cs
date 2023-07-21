namespace Shared.Exceptions
{
    public class ErrorCode
    {
        public static ErrorCode IsNull(object propery) =>
            new ErrorCode("101", $"The {propery} must be provided.");

        public static ErrorCode IsEmpty(object propery) =>
            new ErrorCode("102", $"The {propery} must be provided.");

        public static ErrorCode FirstnameLength =>
        new ErrorCode("201", "Maximum length of FirstName is 20 characters");

        public static ErrorCode LastnameLength =>
        new ErrorCode("202", "Maximum length of Lastname is 30 characters");

        public static ErrorCode InvalidStatrtTimeInWeeklyAvailability() =>
           new ErrorCode("203", "Start time cannot be later than end time.");

        public static ErrorCode StartTimeValidation =>
            new ErrorCode("205", "Appointment Time must be more than now");

        public static ErrorCode DurationValidation =>
            new ErrorCode("206", "Duration must be greater than zero.");

        public static ErrorCode EmailLength =>
         new ErrorCode("207", "Maximum length of email is 50 characters");

        public static ErrorCode InvalidEmail =>
            new ErrorCode("208", "The emal format is invalid.");

        public static ErrorCode OutOfClinicWorkingHours =>
            new ErrorCode("209", "The appointment time is out of clinic working hours.");

        public static ErrorCode OutOfDoctorWorkingHours =>
            new ErrorCode("210", "The appointment time is out of this doctor working hours.");

        public static ErrorCode NumberOfPatientAppointments =>
           new ErrorCode("301", "The patient can not have more appointments for this day.");

        public static ErrorCode InvalidDurationWithDoctorType =>
          new ErrorCode("302", "The appointment duration with this doctor is not valid.");

        public static ErrorCode OverlapWithPatientAppointment =>
            new ErrorCode("303", "The appointment time has overlap with another appointment of this patient.");

        public static ErrorCode Overlap =>
            new ErrorCode("304", "There is a conflicting appointment with this one.");

        public static ErrorCode DoctorIsBusy =>
            new ErrorCode("305", "The doctor is scheduled for another appointment.");

        public static ErrorCode DoctorDuplicateEmail =>
           new ErrorCode("306", "The email address is using by anothr doctor.");

        public static ErrorCode PatientDuplicateEmail =>
          new ErrorCode("307", "The email address is using by anothr patient.");

        public static ErrorCode UnavailableRoom =>
          new ErrorCode("308", "The room is scheduled for another appointment.");

        public string Code { get; private set; }
        public string Description { get; private set; }

        public ErrorCode(string code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}
