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

        [When(@"I get the list of collectors")]
        public void SetListOfCollectorsToContext()
        {
           ScenarioContext.Set<List<Collector>>(new CollectorSteps().GetListOfCollectors(), "listOfCollectors");
        }

        [Given(@"I have added a collector with the following parameters")]
        [When(@"I add a collector with the following parameters")]
        public void AddObjectWithParameters(CollectorCreator collector)
        {
                new CollectorSteps().CreateCollector(collector);
                ScenarioContext.Set<Collector>(
                Transformations.GetCollectorBasedOnCollectorCreator(collector), "expectedCollector");
        }

        [Given(@"I have modified the collector with the following parameters")]
        [When(@"I modify the collector with the following parameters")]
        public void GivenIHaveModifiedTheObjectWithTheFollowingParameters(Collector collector)
        {
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
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        //[Then(@"the collector data is saved correctly")]
        //public void ThenTheDataIsSavedCorrectly()
        //{
        //    object expectedObject = null;
        //    object actualObject = null;

        //        expectedObject = ScenarioContext.Get<Collector>("expectedCollector");
        //        actualObject = ScenarioContext.Get<Collector>("actualCollector");

        //    Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        //}

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

        //[When(@"I find a collector by the parameters")]
        //public void GivenITryToGetAnObjectByParameters(CollectorCreator expectedCollector)
        //{
        //    int countOfObject = new CollectorSteps().GetListOfCollectors()
        //        .Count(collector => collector.fearFactor.Equals(expectedCollector.fearFactor) 
        //        && collector.nickname.Equals(expectedCollector.nickname));
        //    HttpStatusCode statusCode = countOfObject switch
        //    {
        //        0 => HttpStatusCode.NotFound,
        //        _ => HttpStatusCode.OK
        //    };
        //    ScenarioContext.Set<HttpStatusCode>(statusCode,"ActualStatusCode");
        //}

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
