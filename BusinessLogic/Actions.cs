using SdcaFramework.Clients;
using SdcaFramework.ClientSteps;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace SdcaFramework.Steps
{
    [Binding]
    public sealed class Actions
    {
        [When(@"I get the list of (appointment|collector|debt|student)s")]
        public void WhenIViewTheListOfObjects(SdcaParts part)
        {
            if (part.Equals(SdcaParts.collector))
            {
                ScenarioContext.Current.Set<List<Collector>>(new CollectorSteps().GetListOfCollectors(), "listOfObjects");
            }
            else if (part.Equals(SdcaParts.appointment))
            {
                ScenarioContext.Current.Set<List<Appointment>>(new AppointmentSteps().GetListOfAppointments(), "listOfObjects");
            }
            else if (part.Equals(SdcaParts.debt))
            {
                ScenarioContext.Current.Set<List<Debt>>(new DebtSteps().GetListOfDebts(), "listOfObjects");
            }
            else if (part.Equals(SdcaParts.student))
            {
                ScenarioContext.Current.Set<List<Student>>(new StudentSteps().GetListOfStudents(), "listOfObjects");
            }
        }

        [Given(@"I have got a(?:n)? (appointment|collector|debt|student) data by (.*) id( again)?")]
        [When(@"I get a(?:n)? (appointment|collector|debt|student) data by (.*) id( again)?")]
        public void WhenIGetADataById(SdcaParts part, string id)
        {
            int neededId = GetNeededId(id, part);

            if (part.Equals(SdcaParts.collector))
            {
                ScenarioContext.Current.Set<Collector>(new CollectorSteps().GetCollectorById(neededId), "actualObject");
            }
            else if (part.Equals(SdcaParts.appointment))
            {
                ScenarioContext.Current.Set<Appointment>(new AppointmentSteps().GetAppointmentById(neededId), "actualObject");
            }
            else if (part.Equals(SdcaParts.debt))
            {
                ScenarioContext.Current.Set<Debt>(new DebtSteps().GetDebtById(neededId), "actualObject");
            }
            else if (part.Equals(SdcaParts.student))
            {
                ScenarioContext.Current.Set<Student>(new StudentSteps().GetStudentById(neededId), "actualObject");
            }
        }

        public int GetNeededId(string id, SdcaParts part)
        {
            int neededId = -1;
            if (id.Equals("last"))
            {
                neededId = part switch
                {
                    SdcaParts.collector => new CollectorSteps().LastCollectorId,
                    SdcaParts.appointment => new AppointmentSteps().LastAppointmentId,
                    SdcaParts.debt => new DebtSteps().LastDebtId,
                    SdcaParts.student => new StudentSteps().LastStudentId
                };
            }
            else if (id.Equals("this"))
            {
                neededId = ScenarioContext.Current.Get<int>("NeededId");
            }
            else
            {
                neededId = Int32.Parse(id);
            }
            return neededId;
        }
    }
}
