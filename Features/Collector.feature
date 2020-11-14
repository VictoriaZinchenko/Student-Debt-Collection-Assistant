Feature: Collector
As a user I want to add collector data to database, read and modify it

Scenario: Add collector and check it presence in the list
	Given I have added a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man | 1          |
	When I get the list of collectors
	Then I can see the created collector in the list

@Bug.Fail
Scenario: Modify collector and check the changes
	Given I have modified the collector with the following parameters
		| id | nickname    | fearFactor |
		| 1  | Fear Man777 | 1          |
	When I get a collector data by 1 id
	Then the collector data were modified correctly

Scenario: Delete collector and check its absence
	Given I have added a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man | 1          |
	When I delete a collector by last id
	Then the system did not find the collector data with this id

Scenario: Try to delete the removed collector
	Given I have added a collector with the following parameters
		| nickname | fearFactor |
		| Fear Man | 1          |
	When I delete a collector by last id
	Then the system did not find the collector data with this id when trying to delete it