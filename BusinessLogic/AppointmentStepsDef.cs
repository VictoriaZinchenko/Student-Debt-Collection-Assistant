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
        private readonly string ExpectedAppointment = "expectedAppointment";

        public AppointmentStepsDef(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        private Appointment ContextExpectedAppointment => ScenarioContext.Get<Appointment>(ExpectedAppointment);

        [Given(@"I have added an appointment with the following parameters")]
        [When(@"I add an appointment with the following parameters")]
        public void AddAppointmentWithParameters(AppointmentCreator appointment)
        {
            new AppointmentSteps().CreateAppointment(appointment);
            Appointment expectedAppointment = Transformations.GetAppointmentBasedOnAppointmentCreator(appointment);
            ScenarioContext.Set(expectedAppointment, ExpectedAppointment);
            Logger.Info($"\nAppointment created with the following properties. " +
                $"{PropertiesDescriber.GetObjectProperties(expectedAppointment)}");
        }

        [Given(@"I have deleted an appointment by (.*) id")]
        [When(@"I delete an appointment by (.*) id")]
        public void DeleteAppointmentById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            new AppointmentSteps().DeleteAppointmentById(neededId);
            SetNeededIdToContext(neededId);
            SetActualStatusCodeToContext(new AppointmentSteps().GetStatusCodeGetAppointmentByIdAction(neededId));
            Logger.Info($"\nAppointment with {neededId} id deleted");
        }

        [Given(@"I have tried to delete the removed appointment by (.*) id")]
        [When(@"I try to delete the removed appointment by (.*) id")]
        public void TryToDeleteRemovedAppointmentById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            Logger.Info($"\nTry to delete the removed appointment by {neededId} id");
            SetNeededIdToContext(neededId);
            SetActualStatusCodeToContext(new AppointmentSteps().GetStatusCodeDeleteAppointmentAction(neededId));
        }

        [Then(@"the system can't find the appointment data")]
        public void ThenSystemCanNotFindAppointmentData()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ContextActualStatusCode, AssertNotFoundMessage);
        }

        [Then(@"the system can't create the appointment data")]
        public void ThenSystemCanNotCreateAppointmentData()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ContextActualStatusCode, AssertBadRequestMessage);
        }

        [Then(@"I can see the created appointment in the list")]
        public void CheckAppointmentPresenceInList()
        {
            var actualObjectsList = new List<object>();
            Appointment expectedObject = ContextExpectedAppointment;
            new AppointmentSteps().GetListOfAppointments().ForEach(element => actualObjectsList.Add(element));
            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"(?:I check again that )?the appointment data is saved correctly")]
        public void ThenAppointmentDataIsSavedCorrectly()
        {
            Appointment actualObject = GetAppointmentDataById("last");
            Appointment expectedObject = ContextExpectedAppointment;
            Assert.AreEqual(expectedObject, actualObject,
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the appointment data with (.*) id is connected with the following (debt|student|collector)")]
        public void ThenAppointmentDataWithIdIsConnectedWithFollowingObject(string id, SdcaParts sdcaPart, Table table)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            object expectedObject = sdcaPart switch
            {
                SdcaParts.debt => Transformations.GetDebtBasedOnDebtCreator(StepArgumentTransformations.GetDebtCreator(table)),
                SdcaParts.collector => Transformations.GetCollectorBasedOnCollectorCreator(StepArgumentTransformations.GetCollectorCreator(table)),
                _ => null
            };
            Appointment appointment = new AppointmentSteps().GetAppointmentById(neededId);
            object actualObject = sdcaPart switch
            {
                SdcaParts.debt => new DebtSteps().GetDebtById(appointment.debtId),
                SdcaParts.collector => new CollectorSteps().GetCollectorById(appointment.collectorIds[0]),
                _ => null
            };

            Assert.AreEqual(expectedObject, actualObject,
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [When(@"I try to add an appointment with invalid parameter")]
        public void TryToAddAppointmentWithInvalidParameter(Dictionary<string, object> parameters)
        {
            Logger.Info($"\nTry to add an appointment with invalid parameter");
            SetActualStatusCodeToContext(new AppointmentSteps().GetStatusCodeForInvalidPostAction(parameters));
        }

        private Appointment GetAppointmentDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.appointment);
            Appointment appointment = new AppointmentSteps().GetAppointmentById(neededId);
            ScenarioContext.Set(appointment, "actualAppointment");
            return appointment;
        }
    }
}
