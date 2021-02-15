Feature: Appointment
As a user I want to add appointment data to database, read and modify it

@Bug.Fail.10
Scenario: Add appointment and check it presence in the list
	When I add an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	Then I can see the created appointment in the list

#BUG?
Scenario Outline: Try to add appointment with invalid parameter
	When I try to add an appointment with invalid parameter
		| Parameter       | Value             |
		| collectorIds    | <collectorIds>    |
		| debtId          | <debtId>          |
		| appointmentDate | <appointmentDate> |
	Then the system can't create the collector data

	Examples:
		| collectorIds | debtId | appointmentDate                  |
		| 1            | 1      | appointmentDate                  |
		| 1            | debtId | 2020-12-09T14:30:00.000000+02:00 |
		| collectorIds | 1      | 2020-12-09T14:30:00.000000+02:00 |
		| -1           | 1      | 2020-12-09T14:30:00.000000+02:00 |
		| 1            | -1     | 2020-12-09T14:30:00.000000+02:00 |
		|              | 1      | 2020-12-09T14:30:00.000000+02:00 |
		| 1            |        | 2020-12-09T14:30:00.000000+02:00 |
		| 1            | 1      |                                  |

@Bug.Fail.10
Scenario: Get appointment by id
	When I add an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	Then the appointment data is saved correctly
	And I check again that the appointment data is saved correctly

Scenario:  Delete appointment and check its absence
	Given I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	When I delete an appointment by last id
	Then the system can't find the appointment data

Scenario: Try to delete the removed appointment
	Given I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| 1, 2         | 1      | 2020-12-09T14:30:00.000000+02:00 |
	When I delete an appointment by last id
	And I try to delete the removed appointment by this id
	Then the system can't find the appointment data

@Bug.Fail.10
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
	When I add an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| last         | last   | 2020-12-09T14:30:00.000000+02:00 |
	Then the appointment data is saved correctly