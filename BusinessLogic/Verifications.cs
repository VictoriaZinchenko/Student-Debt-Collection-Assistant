using NUnit.Framework;
using SdcaFramework.Clients;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Net;
using TechTalk.SpecFlow;

namespace SdcaFramework.Steps
{
    [Binding]
    public sealed class Verifications
    {
        [Then(@"I can see the created (appointment|collector|debt|student) in the list")]
        [Obsolete]
        public void ThenICanSeeThisObject(SdcaParts part)
        {
            object expectedObject = null;
            var actualObjectsList = new List<object>();
            if (part.Equals(SdcaParts.collector))
            {
                expectedObject = ScenarioContext.Current.Get<Collector>("expectedObject");
                ScenarioContext.Current.Get<List<Collector>>("listOfObjects").ForEach(element => actualObjectsList.Add(element));
            }
            else if (part.Equals(SdcaParts.appointment))
            {
                expectedObject = ScenarioContext.Current.Get<Appointment>("expectedObject");
                ScenarioContext.Current.Get<List<Appointment>>("listOfObjects").ForEach(element => actualObjectsList.Add(element));
            }
            else if (part.Equals(SdcaParts.debt))
            {
                expectedObject = ScenarioContext.Current.Get<Debt>("expectedObject");
                ScenarioContext.Current.Get<List<Debt>>("listOfObjects").ForEach(element => actualObjectsList.Add(element));
            }
            else if (part.Equals(SdcaParts.student))
            {
                expectedObject = ScenarioContext.Current.Get<Student>("expectedObject");
                ScenarioContext.Current.Get<List<Student>>("listOfObjects").ForEach(element => actualObjectsList.Add(element));
            }
            Assert.Contains(expectedObject, actualObjectsList, 
                new AssertHelper().GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"the (appointment|collector|debt|student) data were saved correctly")]
        public void ThenTheDataWereSavedCorrectly(SdcaParts part)
        {
            object expectedObject = null;
            object actualObject = null;
            if (part.Equals(SdcaParts.collector))
            {
                expectedObject = ScenarioContext.Current.Get<Collector>("expectedObject");
                actualObject = ScenarioContext.Current.Get<Collector>("actualObject");
            }
            else if (part.Equals(SdcaParts.appointment))
            {
                expectedObject = ScenarioContext.Current.Get<Appointment>("expectedObject");
                actualObject = ScenarioContext.Current.Get<Appointment>("actualObject");
            }
            else if (part.Equals(SdcaParts.debt))
            {
                expectedObject = ScenarioContext.Current.Get<Debt>("expectedObject");
                actualObject = ScenarioContext.Current.Get<Debt>("actualObject");
            }
            else if (part.Equals(SdcaParts.student))
            {
                expectedObject = ScenarioContext.Current.Get<Student>("expectedObject");
                actualObject = ScenarioContext.Current.Get<Student>("actualObject");
            }
            Assert.AreEqual(expectedObject, actualObject, new AssertHelper().GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the (collector|student) data were modified correctly")]
        public void ThenTheDataWereModifiedCorrectly(SdcaParts part)
        {
            object expectedObject = null;
            object actualObject = null;
            if (part.Equals(SdcaParts.collector))
            {
                expectedObject = ScenarioContext.Current.Get<Collector>("expectedModifiedObject");
                actualObject = ScenarioContext.Current.Get<Collector>("actualObject");
            }
            else if (part.Equals(SdcaParts.student))
            {
                expectedObject = ScenarioContext.Current.Get<Student>("expectedModifiedObject");
                actualObject = ScenarioContext.Current.Get<Student>("actualObject");
            }
            Assert.AreEqual(expectedObject, actualObject, new AssertHelper().GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the system did not find the (appointment|collector|debt|student) data with (.*) id( when trying to delete it)*")]
        public void ThenTheSystemDidNotFindTheData(SdcaParts part, string id, string deleteAction)
        {
            int neededId = new Actions().GetNeededId(id, part);
            HttpStatusCode actualCode = 0;
            if (deleteAction == null)
            {
                actualCode = part switch
                {
                    SdcaParts.collector => new CollectorSteps().GetResponseGetCollectorByIdAction(neededId),
                    SdcaParts.appointment => new AppointmentSteps().GetResponseGetAppointmentByIdAction(neededId),
                    SdcaParts.debt => new DebtSteps().GetResponseGetDebtByIdAction(neededId),
                    SdcaParts.student => new StudentSteps().GetResponseGetStudentByIdAction(neededId)
                };
            }
            else
            {
                actualCode = part switch
                {
                    SdcaParts.collector => new CollectorSteps().GetResponseDeleteCollectorAction(neededId),
                    SdcaParts.appointment => new AppointmentSteps().GetResponseDeleteAppointmentAction(neededId),
                    SdcaParts.debt => new DebtSteps().GetResponseDeleteDebtAction(neededId),
                    SdcaParts.student => new StudentSteps().GetResponseDeleteStudentAction(neededId)
                };
            }
            Assert.AreEqual(actualCode, HttpStatusCode.NotFound, "Expected status code should be 'Not Found'.");
        }

        [Then(@"the debt amount is recalculated correctly")]
        public void ThenTheDebtAmountIsRecalculatedCorrectly()
        {
            ScenarioContext.Current.Pending();
        }
    }
}