Feature: Collector
As a user I want to add collector data to database, read and modify it

Scenario: Add collector and check it presence in the list
	When I add a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man46 | 1          |
	Then I can see the created collector in the list

	  Scenario: Get collector by id
 When I add a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man4 | 1          |
  Then the collector data is saved correctly 
  And I check again that the collector data is saved correctly 

	#BUG?
Scenario Outline: Try to add collector with invalid parameter
When I try to add a collector with invalid parameter
	| Parameter  | Value        |
	| nickname   | <nickname>   |
	| fearFactor | <fearFactor> |
Then the system can't create the collector data

Examples: 
| nickname  | fearFactor   |
| Angry guy | angry factor |
| 1234      | 5            |
| Angry guy | -1           |
|           |    1          |
| Angry guy |  |
|       | 5            |

@Bug.Fail.4
Scenario: Modify collector and check the changes
When I modify the collector with the following parameters
		| id | nickname    | fearFactor |
		| 1  | Fear Man777 | 1          |
Then the collector data with 1 id is modified correctly

Scenario: Modify collector multiple times and check the result
Given I have modified the collector with the following parameters
		| id | nickname    | fearFactor |
		| 5  | Modified Man | 1          |
When I modify the collector with the following parameters again
		| id | nickname    | fearFactor |
		| 5  | Modified Man | 1          |
Then I find only one collector with the following parameters
		| nickname | fearFactor |
		| Modified Man | 1          |

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
	When I add an appointment with the following parameters
		| collectorIds | debtId | appointmentDate                  |
		| last         | last   | 2020-12-09T14:30:00.000000+02:00 |
    Then the appointment data with last id is connected with the following collector
		| nickname | fearFactor |
		| Fear Man | 1          |