Feature: Student
As a user I want to add student data to database, read and modify it

  @Bug.Fail
Scenario: Add student and check it presence in the list
	Given I have added a student with the following parameters
	| name     | age | sex  | risk |
	| Poor guy | 17  | true | 5    |
  When I get the list of students
  Then I can see the created student in the list

  @Bug.Fail
  Scenario: Modify student and check the changes
	Given I have modified the student with the following parameters
	| id | name | age | sex | risk |
	|1 | Poor guy | 17  | true | 5    |
  When I get a student data by 1 id
  Then the student data were modified correctly 

    @Bug.Fail
Scenario: Delete student and check its absence
	Given I have added a student with the following parameters
	| name     | age | sex  | risk |
	| Poor guy | 17  | true | 5    |
	When I delete a student by last id
	Then the system did not find the student data with this id

	@Bug.Fail
Scenario: Try to delete the removed student
	Given I have added a student with the following parameters
	| name     | age | sex  | risk |
	| Poor guy | 17  | true | 5    |
	When I delete a student by last id
	Then the system did not find the student data with this id when trying to delete it