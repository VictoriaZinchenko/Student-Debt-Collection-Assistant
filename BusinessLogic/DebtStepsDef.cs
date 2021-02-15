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

        [Given(@"I have got a debt data by (.*) id")]
        [When(@"I get a debt data by (.*) id")]
        public void GetDebtById(string id)
        {
            Logger.Info($"\nTry to get debt by {id} id");
            GetDebtDataById(id);
        }

        [Given(@"I have added a debt with the following parameters")]
        [When(@"I add a debt with the following parameters")]
        public void AddDebtWithParameters(DebtCreator debt)
        {
            new DebtSteps().CreateDebt(debt);
            Debt expectedDebt = Transformations.GetDebtBasedOnDebtCreator(debt);
            ScenarioContext.Set<Debt>(expectedDebt, "expectedDebt");
            Logger.Info($"\nDebt created with the following properties. " +
$"{PropertiesDescriber.GetObjectProperties(expectedDebt)}");
        }

        [Then(@"I can see the created debt in the list")]
        public void CheckDebtPresenceInList()
        {
            var actualObjectsList = new List<object>();
            Debt expectedObject = ScenarioContext.Get<Debt>("expectedDebt");
            new DebtSteps().GetListOfDebts().ForEach(element => actualObjectsList.Add(element));
            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"(?:I check again that )?the debt data is saved correctly")]
        public void ThenDebtIsSavedCorrectly()
        {
            Debt expectedObject = ScenarioContext.Get<Debt>("expectedDebt");
            Debt actualObject = GetDebtDataById("last");
            Assert.AreEqual(expectedObject, actualObject,
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Given(@"I have deleted a debt by (.*) id")]
        [When(@"I delete a debt by (.*) id")]
        public void DeleteDebtById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.debt);
            new DebtSteps().DeleteDebtById(neededId);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new DebtSteps().GetStatusCodeGetDebtByIdAction(neededId),
                "ActualStatusCode");
            Logger.Info($"\nDebt with {neededId} id deleted");
        }

        [Given(@"I have tried to delete the removed debt by (.*) id")]
        [When(@"I try to delete the removed debt by (.*) id")]
        public void TryToDeleteRemovedDebtById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.debt);
            Logger.Info($"\nTry to delete the removed debt by {neededId} id");
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new DebtSteps().GetStatusCodeDeleteDebtAction(neededId),
                "ActualStatusCode");
        }

        [Then(@"the system can't find the debt data")]
        public void ThenSystemDidNotFindDebt()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Not Found'.");
        }

        [Then(@"the system can't create the debt data")]
        public void ThenSystemDidNotCreateDebt()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Bad Request'.");
        }

        [Then(@"the current amount is recalculated correctly for debt with 0 id")]
        public void ThenDebtAmountIsRecalculatedCorrectly()
        {
            var dateOfCreationDebtWith0Id = new DateTime(2020, 5, 30); //taken from the TeztApi framework as the database is missing
            double originalAmountDebtWith0Id = 1000;
            Debt debt = ScenarioContext.Get<Debt>("actualDebt");
            double expectedAmount = new DebtCalculation().RecalculateAmount(debt, originalAmountDebtWith0Id,
                dateOfCreationDebtWith0Id, DateTime.Today);
            Assert.AreEqual(expectedAmount, debt.amount, "The expected and actual amounts are different");
        }

        [Then(@"the debt data with (.*) id is connected with the following (debt|student|collector)")]
        public void ThenDebtWithIdIsConnectedWithFollowingObject(string id, SdcaParts sdcaPart, Table table)
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

        [When(@"I try to add a debt with invalid parameter")]
        public void TryToAddADebtWithInvalidParameter(Dictionary<string, object> parameters)
        {
            Logger.Info($"\nTry to add a debt with invalid parameter");
            ScenarioContext.Set<HttpStatusCode>(new DebtSteps().GetStatusCodeForInvalidPostAction(parameters),
                "ActualStatusCode");
        }

        private Debt GetDebtDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.debt);
            Debt debt = new DebtSteps().GetDebtById(neededId);
            ScenarioContext.Set<Debt>(debt, "actualDebt");
            return debt;
        }
    }
}
