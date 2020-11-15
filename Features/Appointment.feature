Feature: Appointment
As a user I want to add appointment data to database, read and modify it

Scenario: Add appointment and check it presence in the list
	Given I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	When I get the list of appointments
	Then I can see the created appointment in the list

Scenario: Get appointment by id
	Given I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	When I get an appointment data by last id
	Then the appointment data were saved correctly

Scenario:  Delete appointment and check its absence
	Given I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	When I delete an appointment by last id
	Then the system did not find the appointment data with this id

Scenario: Try to delete the removed appointment
	Given I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	When I delete an appointment by last id
	Then the system did not find the appointment data with this id when trying to delete it

Scenario: Create collector, student, debt, appointment and check created appointment
	Given I have added a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man | 1          |
	And I have added a student with the following parameters
		| name     | age | sex  | risk |
		| Poor guy | 17  | true | 1    |
	And I have added a debt with the following parameters
		| studentId | amount | monthlyPercent |
		| last      | 170    | 10             |
	And I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| last         | last   | 2020-12-09T14:30:00.000000+02:00 |
	When I get an appointment data by last id
	Then the appointment data were saved correctly