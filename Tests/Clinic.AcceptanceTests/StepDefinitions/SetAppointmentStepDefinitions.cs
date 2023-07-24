using Clinic.AcceptanceTests.Drivers;
using Clinic.AcceptanceTests.Dtos;
using Clinic.AcceptanceTests.Context;
using TechTalk.SpecFlow.Assist;

namespace Clinic.AcceptanceTests.StepDefinitions
{
    [Binding]
    public class SetAppointmentStepDefinitions
    {
        private readonly SetAppointmentDriver _driver;
        protected readonly AppointmentContext ScenarioContext;


        public SetAppointmentStepDefinitions(SetAppointmentDriver setAppointmentDriver, AppointmentContext scenarioContext)
        {
            _driver = setAppointmentDriver;
            ScenarioContext = scenarioContext;
        }

        [Given(@"The following doctors work in the clinic")]
        public async Task GivenTheFollowingDoctorsWorkInTheClinic(Table table)
        {
            IEnumerable<DoctorDto>? doctorDtos = table.CreateSet<DoctorDto>();
            await _driver.SeedDoctors(doctorDtos);
        }

        [Given(@"doctors work in the clinic according to the schedule below")]
        public async Task GivenDoctorsWorkInTheClinicAccordingToTheScheduleBelow(Table table)
        {
            IEnumerable<WeeklyAvailibilityDto>? availibilityDtos = table.CreateSet<WeeklyAvailibilityDto>();
            await _driver.SeedWeeklyAvailability(availibilityDtos);
        }


        [Given(@"The patients listed below have registered at the clinic")]
        public async Task GivenThePatientsListedBelowHaveRegisteredAtTheClinic(Table table)
        {
            IEnumerable<PatientDto>? patientDtos = table.CreateSet<PatientDto>();
            await _driver.SeedPatients(patientDtos);
        }

        [Given(@"the following appointments have been reserved")]
        public async Task GivenTheFollowingAppointmentsHaveBeenReserved(Table table)
        {
            IEnumerable<AppointmentDto>? appointmentDtos = table.CreateSet<AppointmentDto>();
            await _driver.SeedAppointments(appointmentDtos);
        }

        [Given(@"the user is going to set an appointment as below")]
        public void GivenTheUserIsGoingToSetAnAppointmentAsBelow(Table table)
        {
            ScenarioContext.Appointment = table.CreateInstance<AppointmentDto>();
        }

        [When(@"user book appintment")]
        public async Task WhenUserBookAppintment()
        {
            await _driver.SetAppointment(ScenarioContext.Appointment);
        }

        [Then(@"user should get below result")]
        public void ThenUserShouldGetBelowResult(Table table)
        {
            ResultDto expectedResult = table.CreateInstance<ResultDto>();
            ScenarioContext.SetAppointmentResponse.Success.Should().Be(expectedResult.Success);
        }

        [When(@"user requests that an appointment be scheduled that conflicts with another appointment as below")]
        public async Task WhenAUserRequestsThatAnAppointmentBeScheduledThatConflictsWithAnotherAppointmentAsBelow(Table table)
        {
            AppointmentDto appointment = table.CreateInstance<AppointmentDto>();
            await _driver.SetAppointment(appointment);
        }

        [Then(@"user should get below Error")]
        public void ThenUserShouldGetBelowError(Table table)
        {
            ResultDto expectedResult = table.CreateInstance<ResultDto>();
            ScenarioContext.SetAppointmentResponse.Errors[0].Code.Should().Be(expectedResult.ErrorCode);
        }

        [When(@"user requests that an appointment be scheduled with a general doctor for (.*) minutes")]
        public async Task WhenAUserRequestsThatAnAppointmentBeScheduledWithaGeneralDoctorForMinutes(int p0, Table table)
        {
            AppointmentDto appointment = table.CreateInstance<AppointmentDto>();
            await _driver.SetAppointment(appointment);
        }

        [When(@"user requests that an appointment be scheduled out of doctor working hours")]
        public async Task WhenUserRequestsThatAnAppointmentBeScheduledOutOfDoctorWorkingHours(Table table)
        {
            AppointmentDto appointment = table.CreateInstance<AppointmentDto>();
            await _driver.SetAppointment(appointment);
        }

        [When(@"user book earliest appintment")]
        public async Task WhenUserBookEarliestAppintment()
        {
            await _driver.SetEarliestAppointment(ScenarioContext.Appointment);
        }

        [Then(@"the user should get below result")]
        public void ThenTheUserShouldGetBelowResult(Table table)
        {
            ResultDto expectedResult = table.CreateInstance<ResultDto>();
            ScenarioContext.SetEarliestAppointmentResponse.Success.Should().Be(expectedResult.Success);
        }

    }
}
