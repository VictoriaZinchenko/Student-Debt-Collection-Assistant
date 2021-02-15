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
        private readonly string ExpectedCollector = "expectedCollector";

        public CollectorStepsDef(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        private Collector ContextExpectedCollector => ScenarioContext.Get<Collector>(ExpectedCollector);

        [Given(@"I have added a collector with the following parameters")]
        [When(@"I add a collector with the following parameters")]
        public void AddCollectorWithParameters(CollectorCreator collector)
        {
            new CollectorSteps().CreateCollector(collector);
            Collector expectedCollector = Transformations.GetCollectorBasedOnCollectorCreator(collector);
            ScenarioContext.Set(expectedCollector, ExpectedCollector);
            Logger.Info($"\nCollector created with the following properties. " +
                $"{PropertiesDescriber.GetObjectProperties(expectedCollector)}");
        }

        [Given(@"I have modified the collector with the following parameters(?: again)?")]
        [When(@"I modify the collector with the following parameters(?: again)?")]
        public void ModifyCollectorWithFollowingParameters(Collector collector)
        {
            Logger.Info($"\nTry to modify the collector with {collector.id} id");
            new CollectorSteps().ModifyCollector(collector);
            ScenarioContext.Set(collector, "expectedModifiedCollector");
        }

        [Then(@"I can see the created collector in the list")]
        public void CheckCollectorPresenceInList()
        {
            var actualObjectsList = new List<object>();
            Collector expectedObject = ContextExpectedCollector;
            new CollectorSteps().GetListOfCollectors().ForEach(element => actualObjectsList.Add(element));
            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"(?:I check again that )?the collector data is saved correctly")]
        public void ThenCollectorIsSavedCorrectly()
        {
            Collector expectedObject = ContextExpectedCollector;
            Collector actualObject = GetCollectorDataById("last");
            Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the collector data with (.*) id is modified correctly")]
        public void ThenCollectorIsModifiedCorrectly(string id)
        {
            Collector expectedObject = ScenarioContext.Get<Collector>("expectedModifiedCollector");
            Collector actualObject = GetCollectorDataById(id);
            Assert.AreEqual(expectedObject, actualObject,
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Given(@"I have deleted a collector by (.*) id")]
        [When(@"I delete a collector by (.*) id")]
        public void DeleteCollectorById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            new CollectorSteps().DeleteCllectorById(neededId);
            SetNeededIdToContext(neededId);
            SetActualStatusCodeToContext(new CollectorSteps().GetStatusCodeGetCollectorByIdAction(neededId));
            Logger.Info($"\nCollector with {neededId} id deleted");
        }

        [Given(@"I have tried to delete the removed collector by (.*) id")]
        [When(@"I try to delete the removed collector by (.*) id")]
        public void TryToDeleteRemovedCollectorById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            Logger.Info($"\nTry to delete the removed collector by {neededId} id");
            SetNeededIdToContext(neededId);
            SetActualStatusCodeToContext(new CollectorSteps().GetStatusCodeDeleteCollectorAction(neededId));
        }

        [Then(@"I find only one collector with the following parameters")]
        public void FindCollectorByParameters(CollectorCreator expectedCollector)
        {
            int countOfObjects = new CollectorSteps().GetListOfCollectors()
                .Count(collector => collector.fearFactor.Equals(expectedCollector.fearFactor)
                && collector.nickname.Equals(expectedCollector.nickname));
            Assert.AreEqual(1, countOfObjects,
                $"There should be only one collector with the following properties. Actual count of such collectors is {countOfObjects}." +
                $"{PropertiesDescriber.GetObjectProperties(expectedCollector)}");
        }

        [Then(@"the system can't find the collector data")]
        public void ThenSystemDidNotFindCollector()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ContextActualStatusCode, AssertNotFoundMessage);
        }

        [Then(@"the system can't create the collector data")]
        public void ThenSystemDidNotCreateCollector()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ContextActualStatusCode, AssertBadRequestMessage);
        }

        [When(@"I try to add a collector with invalid parameter")]
        public void TryToAddCollectorWithInvalidParameter(Dictionary<string, object> parameters)
        {
            Logger.Info($"\nTry to add a collector with invalid parameter");
            SetActualStatusCodeToContext(new CollectorSteps().GetStatusCodeForInvalidPostAction(parameters));
        }

        public Collector GetCollectorDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.collector);
            return new CollectorSteps().GetCollectorById(neededId);
        }
    }
}
