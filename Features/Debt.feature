Feature: Debt
As a user I want to add debt data to database, read and modify it
I want to get automatically recalculated current debt amount based on current date and monthly rate

Scenario: Add debt and check it presence in the list
	When I add a debt with the following parameters
		| studentId | amount | monthlyPercent |
		| 1         | 170    | 10             |
	Then I can see the created debt in the list

#BUG?
Scenario Outline: Try to add debt with invalid parameter
	When I try to add a debt with invalid parameter
		| Parameter      | Value            |
		| studentId      | <studentId>      |
		| amount         | <amount>         |
		| monthlyPercent | <monthlyPercent> |
	Then the system can't create the collector data

	Examples:
		| studentId | amount | monthlyPercent |
		| 0         | amount | 5              |
		| studentId | 100    | 5              |
		| 0         | 100    | monthlyPercent |
		| -2        | 100    | 5              |
		| 0         | -100   | 5              |
		| 0         | 100    | -5             |
		|           | 100    | 5              |
		| 0         |        | 5              |
		| 0         | 100    |                |

Scenario: Get debt by id
	When I add a debt with the following parameters
		| studentId | amount | monthlyPercent |
		| 1         | 170    | 10             |
	Then the debt data is saved correctly
	And I check again that the debt data is saved correctly

Scenario: Delete debt and check its absence
	Given I have added a debt with the following parameters
		| studentId | amount | monthlyPercent |
		| 1         | 170    | 10             |
	When I delete a debt by last id
	Then the system can't find the debt data

Scenario: Try to delete the removed debt
	Given I have added a debt with the following parameters
		| studentId | amount | monthlyPercent |
		| 1         | 170    | 10             |
	When I delete a debt by last id
	And I try to delete the removed debt by this id
	Then the system can't find the debt data

#Bug?
Scenario: Check recalculated debt amount
	When I get a debt data by 0 id
	Then the current amount is recalculated correctly for debt with 0 id

Scenario: Create collector, student, debt, appointment and check appointment connection with debt
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
	Then the appointment data with last id is connected with the following debt
		| studentId | amount | monthlyPercent |
		| last      | 170    | 10             |