Feature: Debt
As a user I want to add debt data to database, read and modify it
I want to get automatically recalculated current debt amount based on current date and monthly rate

Scenario: Add debt and check it presence in the list
	Given I have added a debt with the following parameters
	| studentId | amount | monthlyPercent |
	| 1         | 170    | 10             |
  When I get the list of debts
  Then I can see the created debt in the list

  Scenario: Get debt by id
	Given I have added a debt with the following parameters
	| studentId | amount | monthlyPercent |
	| 1         | 170    | 10             |
  When I get a debt data by last id
  Then the debt data were saved correctly 

  Scenario: Delete debt and check its absence
	Given I have added a debt with the following parameters
	| studentId | amount | monthlyPercent |
	| 1         | 170    | 10             |
	When I delete a debt by last id
	Then the system did not find the debt data with this id

 Scenario: Try to delete the removed debt
	Given I have added a debt with the following parameters
	| studentId | amount | monthlyPercent |
	| 1         | 170    | 10             |
	When I delete a debt by last id
	Then the system did not find the debt data with this id when trying to delete it 

	 Scenario: llll
	Given I have got a debt data by 0 id
	When I get a debt data by 0 id again
	Then the debt amount is recalculated correctly



