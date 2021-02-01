using NUnit.Framework;
using SdcaFramework.Clients;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;

namespace SdcaFramework.BusinessLogic
{
    [Binding]
    public sealed class CollectorStepsDef : BaseStepDef
    {
        public CollectorStepsDef(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [When(@"I get the list of collectors")]
        public void SetListOfCollectorsToContext()
        {
           ScenarioContext.Set<List<Collector>>(new CollectorSteps().GetListOfCollectors(), "listOfCollectors");
        }

        [Given(@"I have got a collector data by (.*) id( again)*")]
        [When(@"I get a collector data by (.*) id( again)*")]
        public void WhenIGetDataById(string id, string repeatedAction)
        {
            string key;
            int neededId = GetNeededId(id, SdcaParts.collector);
            if (!string.IsNullOrEmpty(repeatedAction))
            {
                key = "actualCollector№2";
            }
            else
            {
                key = "actualCollector";
            }
            ScenarioContext.Set<Collector>(new CollectorSteps().GetCollectorById(neededId), key);
        }

        [Given(@"I have added a collector with the following parameters")]
        public void AddObjectWithParameters(Table table)
        {
                new CollectorSteps().CreateCollector(StepArgumentTransformations.GetCollectorCreator(table));
                ScenarioContext.Set<Collector>(
                    Transformations.GetCollectorBasedOnCollectorCreator(StepArgumentTransformations.GetCollectorCreator(table)), "expectedCollector");
        }

        [Given(@"I have modified the collector with the following parameters")]
        public void GivenIHaveModifiedTheObjectWithTheFollowingParameters(Table table)
        {
                Collector collector = StepArgumentTransformations.GetCollector(table);
                new CollectorSteps().ModifyCollector(collector);
                ScenarioContext.Set<Collector>(collector, "expectedModifiedCollector");
        }

        [Then(@"I can see the created collector in the list")]
        public void ThenICanSeeThisObject()
        {
            object expectedObject = null;
            var actualObjectsList = new List<object>();

                expectedObject = ScenarioContext.Get<Collector>("expectedCollector");
                ScenarioContext.Get<List<Collector>>("listOfCollectors").ForEach(element => actualObjectsList.Add(element));
            
            Assert.Contains(expectedObject, actualObjectsList,
                AssertHelper.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"the collector data is saved correctly")]
        public void ThenTheDataIsSavedCorrectly()
        {
            object expectedObject = null;
            object actualObject = null;

                expectedObject = ScenarioContext.Get<Collector>("expectedCollector");
                actualObject = ScenarioContext.Get<Collector>("actualCollector");

            Assert.AreEqual(expectedObject, actualObject, AssertHelper.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the collector data is modified correctly")]
        public void ThenTheDataIsModifiedCorrectly()
        {
            object expectedObject = null;
            object actualObject = null;

                expectedObject = ScenarioContext.Get<Collector>("expectedModifiedCollector");
                actualObject = ScenarioContext.Get<Collector>("actualCollector");

            Assert.AreEqual(expectedObject, actualObject, AssertHelper.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Given(@"I have deleted a collector by (.*) id")]
        [When(@"I delete a collector by (.*) id")]
        public void GivenIHaveDeletedAnObjectById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            new CollectorSteps().DeleteCllectorById(neededId);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new CollectorSteps().GetResponseGetCollectorByIdAction(neededId),
                "ActualStatusCode");
        }

        [Given(@"I have tried to delete the removed collector by (.*) id")]
        [When(@"I try to delete the removed collector by (.*) id")]
        public void GivenIHaveDeletedAnObjectByIdmmmmmmmmmmmmmmmmmmmmmmm(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new CollectorSteps().GetResponseDeleteCollectorAction(neededId),
                "ActualStatusCode");
        }

        [Then(@"the system can't find the collector data")]
        public void ThenTheSystemDidNotFindTheData()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Not Found'.");
        }
    }
}
