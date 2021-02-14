using NUnit.Framework;
using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using SdcaFramework.Utilities.Enums;
using System.Collections.Generic;
using System.Linq;
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

        [Given(@"I have added a collector with the following parameters")]
        [When(@"I add a collector with the following parameters")]
        public void AddObjectWithParameters(CollectorCreator collector)
        {
                new CollectorSteps().CreateCollector(collector);
                ScenarioContext.Set<Collector>(
                Transformations.GetCollectorBasedOnCollectorCreator(collector), "expectedCollector");
        }

        [Given(@"I have modified the collector with the following parameters(?: again)?")]
        [When(@"I modify the collector with the following parameters(?: again)?")]
        public void GivenIHaveModifiedTheObjectWithTheFollowingParameters(Collector collector)
        {
                new CollectorSteps().ModifyCollector(collector);
                ScenarioContext.Set<Collector>(collector, "expectedModifiedCollector");
        }

        [Then(@"I can see the created collector in the list")]
        public void ThenICanSeeThisObject()
        {
            var actualObjectsList = new List<object>();
            Collector expectedObject = ScenarioContext.Get<Collector>("expectedCollector");
            new CollectorSteps().GetListOfCollectors().ForEach(element => actualObjectsList.Add(element));
            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"(?:I check again that )?the collector data is saved correctly")]
        public void ThenTheDataIsSavedCorrectly()
        {
            Collector expectedObject = ScenarioContext.Get<Collector>("expectedCollector");
            Collector actualObject = GetCollectorDataById("last");
            Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the collector data with (.*) id is modified correctly")]
        public void ThenTheDataIsModifiedCorrectly(string id)
        {
            Collector expectedObject = ScenarioContext.Get<Collector>("expectedModifiedCollector");
            Collector actualObject = GetCollectorDataById(id);
            Assert.AreEqual(expectedObject, actualObject, 
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Given(@"I have deleted a collector by (.*) id")]
        [When(@"I delete a collector by (.*) id")]
        public void GivenIHaveDeletedAnObjectById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            new CollectorSteps().DeleteCllectorById(neededId);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new CollectorSteps().GetStatusCodeGetCollectorByIdAction(neededId),
                "ActualStatusCode");
        }

        [Given(@"I have tried to delete the removed collector by (.*) id")]
        [When(@"I try to delete the removed collector by (.*) id")]
        public void GivenIHaveDeletedAnObjectByIdmmmmmmmmmmmmmmmmmmmmmmm(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new CollectorSteps().GetStatusCodeDeleteCollectorAction(neededId),
                "ActualStatusCode");
        }

        [Then(@"I find only one collector with the following parameters")]
        public void TryToGetAnObjectByParameters(CollectorCreator expectedCollector)
        {
            int countOfObjects = new CollectorSteps().GetListOfCollectors()
                .Count(collector => collector.fearFactor.Equals(expectedCollector.fearFactor)
                && collector.nickname.Equals(expectedCollector.nickname));
            Assert.AreEqual(1, countOfObjects,
                $"There should be only one collector with the following properties. Actual count of such collectors is {countOfObjects}." +
                $"{PropertiesDescriber.GetObjectProperties(expectedCollector)}");
        }

        [Then(@"the system can't find the collector data")]
        public void ThenTheSystemDidNotFindTheData()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Not Found'.");
        }

        [Then(@"the system can't create the collector data")]
        public void ThenTheSystemDidNotCreateTheData()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Bad Request'.");
        }

        [When(@"I try to add a collector with invalid parameter")]
        public void WhenITryToAddACollectorWithInvalidParameter(Dictionary<string, object> parameters)
        {
            ScenarioContext.Set<HttpStatusCode>(new CollectorSteps().GetStatusCodeForInvalidPostAction(parameters),
                "ActualStatusCode");
        }

        public Collector GetCollectorDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            return new CollectorSteps().GetCollectorById(neededId);
        }
    }
}
