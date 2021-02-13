using NUnit.Framework;
using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using SdcaFramework.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;

namespace SdcaFramework.BusinessLogic
{
    [Binding]
    public sealed class DebtStepsDef : BaseStepDef
    { 
    public DebtStepsDef(ScenarioContext scenarioContext) : base(scenarioContext)
    {

    }
    
        [When(@"I get the list of debts")]
        public void SetListOfDebtsToContext()
        {
            ScenarioContext.Set<List<Debt>>(new DebtSteps().GetListOfDebts(), "listOfDebts");
        }

        [Given(@"I have got a debt data by (.*) id")]
        [When(@"I get a debt data by (.*) id")]
        public void WhenIGetDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.debt);
            ScenarioContext.Set<Debt>(new DebtSteps().GetDebtById(neededId), "actualDebt");
        }

        [Given(@"I have added a debt with the following( invalid)* parameters")]
        [When(@"I add a debt with the following( invalid)* parameters")]
        public void AddObjectWithParameters(string invalidParameter, DebtCreator debt)
        {
            if (!string.IsNullOrEmpty(invalidParameter))
            {
                ScenarioContext.Set<HttpStatusCode>(new DebtSteps().GetResponseCreateDebtAction(debt), "ActualStatusCode");
            }
            new DebtSteps().CreateDebt(debt);
                ScenarioContext.Set<Debt>(
                Transformations.GetDebtBasedOnDebtCreator(debt), "expectedDebt");
        }

        [Then(@"I can see the created debt in the list")]
        public void ThenICanSeeThisObject()
        {
            object expectedObject = null;
            var actualObjectsList = new List<object>();

                expectedObject = ScenarioContext.Get<Debt>("expectedDebt");
                ScenarioContext.Get<List<Debt>>("listOfDebts").ForEach(element => actualObjectsList.Add(element));

            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"the debt data is saved correctly")]
        public void ThenTheDataIsSavedCorrectly()
        {
            object expectedObject = null;
            object actualObject = null;

                expectedObject = ScenarioContext.Get<Debt>("expectedDebt");
                actualObject = ScenarioContext.Get<Debt>("actualDebt");

            Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Given(@"I have deleted a debt by (.*) id")]
        [When(@"I delete a debt by (.*) id")]
        public void GivenIHaveDeletedAnObjectById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.debt);
            new DebtSteps().DeleteDebtById(neededId);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new DebtSteps().GetResponseGetDebtByIdAction(neededId),
                "ActualStatusCode");
        }

        [Given(@"I have tried to delete the removed debt by (.*) id")]
        [When(@"I try to delete the removed debt by (.*) id")]
        public void GivenIHaveDeletedAnObjectByIdmmmmmmmmmmmmmmmmmmmmmmm(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.debt);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new DebtSteps().GetResponseDeleteDebtAction(neededId),
                "ActualStatusCode");
        }

        [Then(@"the system can't find the debt data")]
        public void ThenTheSystemDidNotFindTheData()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Not Found'.");
        }

        [Then(@"the system can't create the debt data")]
        public void ThenTheSystemDidNotCreateTheData()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Bad Request'.");
        }

        [Then(@"the current amount is recalculated correctly for debt with 0 id")]
        public void ThenTheDebtAmountIsRecalculatedCorrectly()
        {
            var dateOfCreationDebtWith0Id = new DateTime(2020, 5, 30); //taken from the TeztApi framework as the database is missing
            double originalAmountDebtWith0Id = 1000;
            Debt debt = ScenarioContext.Get<Debt>("actualDebt");
            double expectedAmount = new DebtCalculation().RecalculateAmount(debt, originalAmountDebtWith0Id,
                dateOfCreationDebtWith0Id, DateTime.Today);
            Assert.AreEqual(expectedAmount, debt.amount, "The expected and actual amounts are different");
        }

        [Then(@"the debt data with (.*) id is connected with the following (debt|student|collector)")]
        public void ThenDataWithIdIsConnectedWithTheFollowingObject(string id, SdcaParts sdcaPart, Table table)
        {
            int neededId = GetNeededId(id, SdcaParts.debt);
            object expectedObject = sdcaPart switch
                {
                    SdcaParts.student => Transformations.GetStudentBasedOnStudentCreator(StepArgumentTransformations.GetStudentCreator(table)),
                    _ => null
                };
            object actualObject = sdcaPart switch
                {
                    SdcaParts.student => new StudentSteps().GetStudentById(new DebtSteps().GetDebtById(neededId).studentId),
                    _ => null
                };
            Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }
    }
}
