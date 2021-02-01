Feature: Collector
As a user I want to add collector data to database, read and modify it

Scenario: Add collector and check it presence in the list
	Given I have added a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man | 1          |
	When I get the list of collectors
	Then I can see the created collector in the list

@Bug.Fail.4
Scenario: Modify collector and check the changes
	Given I have modified the collector with the following parameters
		| id | nickname    | fearFactor |
		| 1  | Fear Man777 | 1          |
	When I get a collector data by 1 id
	Then the collector data is modified correctly

Scenario: Delete collector and check its absence
	Given I have added a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man | 1          |
	When I delete a collector by last id
	Then the system can't find the collector data

Scenario: Try to delete the removed collector
	Given I have added a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man | 1          |
	When I delete a collector by last id
	And I try to delete the removed collector by this id
	Then the system can't find the collector data

	@Bug.Fail.10
	Scenario: Create collector, student, debt, appointment and check appointment connection with collector
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
    Then the appointment data with last id is connected with the following collector
		| nickname | fearFactor |
		| Fear Man | 1          |