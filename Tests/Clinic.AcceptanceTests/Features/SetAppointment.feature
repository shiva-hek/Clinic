Feature: Appointment Booking
  As a userLastname
  In order to book an appointment with a doctor
  I want to reserve an appointment slot in the clinic



Background:
	Given The following doctors work in the clinic
		| Firstname | Lastname | Id                                   | Email            | DoctorType |
		| John      | Doe      | C8F01166-025B-493D-85E8-2573B9D509E3 | john.d@gmail.com | General    |
		| Jane      | Smith    | 6142E620-DBDE-4940-8920-5CEB6CD88508 | jane.s@gmail.com | Specialist |
	And doctors work in the clinic according to the schedule below
		| Firstname | Lastname | DoctorId                             | Day     | StartTime | EndTime |
		| John      | Doe      | C8F01166-025B-493D-85E8-2573B9D509E3 | Sunday  | 9:00      | 15:00   |
		| John      | Doe      | C8F01166-025B-493D-85E8-2573B9D509E3 | Monday  | 9:00      | 15:00   |
		| John      | Doe      | C8F01166-025B-493D-85E8-2573B9D509E3 | Tuesday | 9:00      | 15:00   |
		| Jane      | Smith    | 6142E620-DBDE-4940-8920-5CEB6CD88508 | Sunday  | 10:00     | 18:00   |
		| Jane      | Smith    | 6142E620-DBDE-4940-8920-5CEB6CD88508 | Monday  | 10:00     | 18:00   |
	And The patients listed below have registered at the clinic
		| Firstname | Lastname | Id                                   | Email              | Gender |
		| Emily     | Brown    | B0150DF0-D611-4050-8B06-9F8AF8971E24 | emily.b@gmail.com  | Male   |
		| Robert    | Lee      | 981FF732-906C-4870-A915-1B46C46D89C0 | robert.l@gmail.com | Female |

	And the following appointments have been reserved
		| Id                                   | Date       | Time     | Day     | Duration | DoctorId                             | PatientId                            |
		| 3DF48275-6F07-48B5-B73C-16F751C070E1 | 2023-08-21 | 10:00:00 | Monday  | 15       | C8F01166-025B-493D-85E8-2573B9D509E3 | B0150DF0-D611-4050-8B06-9F8AF8971E24 |
		| 981FF732-906C-4870-A915-1B46C46D89C0 | 2023-08-22 | 10:00:00 | Tuesday | 15       | C8F01166-025B-493D-85E8-2573B9D509E3 | B0150DF0-D611-4050-8B06-9F8AF8971E24 |


Scenario: User sets an appointment
	Given the user is going to set an appointment as below
		| Key       | Value                                |
		| Date      | 2023-08-26                           |
		| Time      | 10:30:00                             |
		| Duration  | 15                                   |
		| DoctorId  | C8F01166-025B-493D-85E8-2573B9D509E3 |
		| PatientId | B0150DF0-D611-4050-8B06-9F8AF8971E24 |
	When user book appintment
	Then user should get below result
		| Key     | Value |
		| Success | true  |
	When user requests that an appointment be scheduled that conflicts with another appointment as below
		| Key       | Value                                |
		| Date      | 2023-08-26                           |
		| Time      | 10:35:00                             |
		| Duration  | 15                                   |
		| DoctorId  | C8F01166-025B-493D-85E8-2573B9D509E3 |
		| PatientId | B0150DF0-D611-4050-8B06-9F8AF8971E24 |
	Then user should get below Error
		| Key       | Value                                                                      |
		| Success   | true                                                                       |
		| ErrorCode | 303                                                                        |
		| Message   | The appointment time has overlap with another appointment of this patient. |
	When user requests that an appointment be scheduled with a general doctor for 30 minutes
		| Key       | Value                                |
		| Date      | 2023-08-26                           |
		| Time      | 10:30:00                             |
		| Duration  | 30                                   |
		| DoctorId  | C8F01166-025B-493D-85E8-2573B9D509E3 |
		| PatientId | B0150DF0-D611-4050-8B06-9F8AF8971E24 |
	Then user should get below Error
		| Key       | Value                                                   |
		| Success   | true                                                    |
		| ErrorCode | 302                                                     |
		| Message   | The appointment duration with this doctor is not valid. |

		
Scenario: User sets the earliest appointment
	Given the user is going to set an appointment as below
		| Key       | Value                                |
		| Date      | 2023-08-26                           |
		| Time      | 11:30:00                             |
		| Duration  | 15                                   |
		| DoctorId  | C8F01166-025B-493D-85E8-2573B9D509E3 |
		| PatientId | B0150DF0-D611-4050-8B06-9F8AF8971E24 |
	When user book earliest appintment
	Then the user should get below result
		| Key     | Value |
		| Success | true  |



	