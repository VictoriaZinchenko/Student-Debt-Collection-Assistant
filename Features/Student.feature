Feature: Student
As a user I want to add student data to database, read and modify it

@Bug.Fail.5
Scenario: Add student and check it presence in the list
	When I add a student with the following parameters
		| name     | age | sex  | risk |
		| Poor guy | 17  | true | 5    |
	Then I can see the created student in the list

@Bug.Fail.5
Scenario: Get student by id
	When I add a student with the following parameters
		| name      | age | sex   | risk |
		| Poor girl | 17  | false | 5    |
	Then the student data is saved correctly
	And I check again that the student data is saved correctly

#BUG?
Scenario Outline: Try to add student with invalid parameter
	When I try to add a student with invalid parameter
		| Parameter | Value  |
		| name      | <name> |
		| age       | <age>  |
		| sex       | <sex>  |
		| risk      | <risk> |
	Then the system can't create the collector data

	Examples:
		| name  | age | sex  | risk |
		| 123   | 17  | true | 5    |
		| Cutie | age | true | 5    |
		| Cutie | 17  | sex  | 5    |
		| Cutie | 17  | true | risk |
		| Cutie | -17 | true | 5    |
		|       | 17  | true | 5    |
		| Cutie |     | true | 5    |
		| Cutie | 17  |      | 5    |
		| Cutie | 17  | true |      |

@Bug.Fail.5
Scenario: Modify student and check the changes
	When I modify the student with the following parameters
		| id | name     | age | sex  | risk |
		| 1  | Poor guy | 17  | true | 5    |
	Then the student data with 1 id is modified correctly

Scenario: Modify student multiple times and check the result
	Given I have modified the student with the following parameters
		| id | name         | age | sex  | risk |
		| 5  | Modified guy | 17  | true | 2    |
	When I modify the student with the following parameters again
		| id | name         | age | sex  | risk |
		| 5  | Modified guy | 17  | true | 2    |
	Then I find only one student with the following parameters
		| name         | age | sex  | risk |
		| Modified guy | 17  | true | 2    |

Scenario: Delete student and check its absence
	Given I have added a student with the following parameters
		| name     | age | sex  | risk |
		| Poor guy | 17  | true | 5    |
	When I delete a student by last id
	Then the system can't find the student data

Scenario: Try to delete the removed student
	Given I have added a student with the following parameters
		| name     | age | sex  | risk |
		| Poor guy | 17  | true | 5    |
	When I delete a student by last id
	And I try to delete the removed student by this id
	Then the system can't find the student data

@Bug.Fail.5
Scenario: Create collector, student, debt, appointment and check debt connection with student
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
	Then the debt data with last id is connected with the following student
		| name     | age | sex  | risk |
		| Poor guy | 17  | true | 1    |