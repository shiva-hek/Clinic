

using Domain.Models.Appointments.Enums;

namespace Clinic.AcceptanceTests.Dtos
{
    public  class DoctorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DoctorType DoctorType { get; set; } = DoctorType.General;
        public string Email { get; set; } = string.Empty;
    }
}
