using NUnit.Framework;
using SdcaFramework.Clients;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
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

        [Given(@"I have got a debt data by (.*) id( again)*")]
        [When(@"I get a debt data by (.*) id( again)*")]
        public void WhenIGetDataById(string id, string repeatedAction)
        {
            string key;
            int neededId = GetNeededId(id, SdcaParts.debt);
            if (!string.IsNullOrEmpty(repeatedAction))
            {
                key = "actualDebt№2";
            }
            else
            {
                key = "actualDebt№1";
            }
            ScenarioContext.Set<Debt>(new DebtSteps().GetDebtById(neededId), key);
            ScenarioContext.Set<DateTime>(DateTime.Now, "lastUpdatedDate");
        }

        [Given(@"I have added a debt with the following parameters")]
        public void AddObjectWithParameters(Table table)
        {
                new DebtSteps().CreateDebt(StepArgumentTransformations.GetDebtCreator(table));
                ScenarioContext.Set<Debt>(
                    Transformations.GetDebtBasedOnDebtCreator(StepArgumentTransformations.GetDebtCreator(table)), "expectedDebt");
        }

        [Then(@"I can see the created debt in the list")]
        public void ThenICanSeeThisObject(SdcaParts part)
        {
            object expectedObject = null;
            var actualObjectsList = new List<object>();

                expectedObject = ScenarioContext.Get<Debt>("expectedDebt");
                ScenarioContext.Get<List<Debt>>("listOfDebts").ForEach(element => actualObjectsList.Add(element));

            Assert.Contains(expectedObject, actualObjectsList,
                AssertHelper.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"the debt data is saved correctly")]
        public void ThenTheDataIsSavedCorrectly()
        {
            object expectedObject = null;
            object actualObject = null;

                expectedObject = ScenarioContext.Get<Debt>("expectedDebt");
                actualObject = ScenarioContext.Get<Debt>("actualDebt");

            Assert.AreEqual(expectedObject, actualObject, AssertHelper.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
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

        [Then(@"the debt amount is recalculated correctly")]
        public void ThenTheDebtAmountIsRecalculatedCorrectly()
        {
            var expectedAmount = new DebtCalculation().RecalculateAmount(ScenarioContext.Get<Debt>("actualDebt"), ScenarioContext.Get<DateTime>("lastUpdatedDate"));
            Assert.AreEqual(expectedAmount, ScenarioContext.Get<Debt>("actualDebt№2").amount, "The expected and actual amount are different");
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
            Assert.AreEqual(expectedObject, actualObject, AssertHelper.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }
    }
}
