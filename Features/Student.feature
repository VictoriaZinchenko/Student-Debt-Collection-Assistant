Feature: Student
As a user I want to add student data to database, read and modify it

  @Bug.Fail.5
Scenario: Add student and check it presence in the list
	Given I have added a student with the following parameters
	| name     | age | sex  | risk |
	| Poor guy | 17  | true | 5    |
  When I get the list of students
  Then I can see the created student in the list

  @Bug.Fail.5
  Scenario: Modify student and check the changes
	Given I have modified the student with the following parameters
	| id | name | age | sex | risk |
	|1 | Poor guy | 17  | true | 5    |
  When I get a student data by 1 id
  Then the student data were modified correctly 

Scenario: Delete student and check its absence
	Given I have added a student with the following parameters
	| name     | age | sex  | risk |
	| Poor guy | 17  | true | 5    |
	When I delete a student by last id
	Then the system did not find the student data with this id

Scenario: Try to delete the removed student
	Given I have added a student with the following parameters
	| name     | age | sex  | risk |
	| Poor guy | 17  | true | 5    |
	When I delete a student by last id
	Then the system did not find the student data with this id when trying to delete it

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
	And I have added an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| last         | last   | 2020-12-09T14:30:00.000000+02:00 |
	Then the debt data with last id is connected with the following student
		| name     | age | sex  | risk |
		| Poor guy | 17  | true | 1    |