using NUnit.Framework;
using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using SdcaFramework.Utilities.Enums;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;

namespace SdcaFramework.BusinessLogic
{
    [Binding]
    public class AppointmentStepsDef : BaseStepDef
    {
        public AppointmentStepsDef(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [When(@"I get the list of appointments")]
        public void SetListOfAppointmentsToContext()
        {
            ScenarioContext.Set<List<Appointment>>(new AppointmentSteps().GetListOfAppointments(), "listOfAppointments");
            
        }

        [Given(@"I have got an appointment data by (.*) id")]
        [When(@"I get an appointment data by (.*) id")]
        public void WhenIGetDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            ScenarioContext.Set<Appointment>(new AppointmentSteps().GetAppointmentById(neededId), "actualAppointment");
        }

        [Given(@"I have added an appointment with the following( invalid)* parameters")]
        [When(@"I add an appointment with the following( invalid)* parameters")]
        public void AddObjectWithParameters(string invalidParameter, AppointmentCreator appointment)
        {
            if (!string.IsNullOrEmpty(invalidParameter))
            {
                ScenarioContext.Set<HttpStatusCode>(new AppointmentSteps().GetResponseCreateAppointmentAction(appointment), "ActualStatusCode");
            }
            else
            {
                new AppointmentSteps().CreateAppointment(appointment);
                ScenarioContext.Set<Appointment>(
                Transformations.GetAppointmentBasedOnAppointmentCreator(appointment), "expectedAppointment");
            }
        }

        [Given(@"I have deleted an appointment by (.*) id")]
        [When(@"I delete an appointment by (.*) id")]
        public void GivenIHaveDeletedAnObjectById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            new AppointmentSteps().DeleteAppointmentById(neededId);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new AppointmentSteps().GetResponseGetAppointmentByIdAction(neededId), 
                "ActualStatusCode");
        }

        [Given(@"I have tried to delete the removed appointment by (.*) id")]
        [When(@"I try to delete the removed appointment by (.*) id")]
        public void GivenIHaveDeletedAnObjectByIdmmmmmmmmmmmmmmmmmmmmmmm(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new AppointmentSteps().GetResponseDeleteAppointmentAction(neededId),
                "ActualStatusCode");
        }

        [Then(@"the system can't find the appointment data")]
        public void ThenTheSystemCanNotFindTheData()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Not Found'.");
        }

        [Then(@"the system can't create the appointment data")]
        public void ThenTheSystemCanNotCreateTheData()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Bad Request'.");
        }

        [Then(@"I can see the created appointment in the list")]
        public void ThenICanSeeThisObject()
        {
            object expectedObject = null;
            var actualObjectsList = new List<object>();

                expectedObject = ScenarioContext.Get<Appointment>("expectedAppointment");
                ScenarioContext.Get<List<Appointment>>("listOfAppointments").ForEach(element => actualObjectsList.Add(element));

            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"the appointment data is saved correctly")]
        public void ThenTheDataIsSavedCorrectly()
        {
            object expectedObject = null;
            object actualObject = null;

                expectedObject = ScenarioContext.Get<Appointment>("expectedAppointment");
                actualObject = ScenarioContext.Get<Appointment>("actualAppointment");

            Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the appointment data with (.*) id is connected with the following (debt|student|collector)")]
        public void ThenDataWithIdIsConnectedWithTheFollowingObject(string id, SdcaParts sdcaPart, Table table)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            object expectedObject = null;
            object actualObject = null;

                expectedObject = sdcaPart switch
                {
                    SdcaParts.debt => Transformations.GetDebtBasedOnDebtCreator(StepArgumentTransformations.GetDebtCreator(table)),
                    SdcaParts.collector => Transformations.GetCollectorBasedOnCollectorCreator(StepArgumentTransformations.GetCollectorCreator(table)),
                    _ => null
                };
                actualObject = sdcaPart switch
                {
                    SdcaParts.debt => new DebtSteps().GetDebtById(new AppointmentSteps().GetAppointmentById(neededId).debtId),
                    SdcaParts.collector => new CollectorSteps().GetCollectorById(new AppointmentSteps().GetAppointmentById(neededId).collectorIds[0]),
                    _ => null
                };
            
            Assert.AreEqual(expectedObject, actualObject, 
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }
    }
}
