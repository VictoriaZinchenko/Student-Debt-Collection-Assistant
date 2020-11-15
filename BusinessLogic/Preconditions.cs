using SdcaFramework.Clients;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using TechTalk.SpecFlow;

namespace SdcaFramework.Steps
{
    [Binding]
    public sealed class Preconditions
    {
        Transformations Transformations = new Transformations();

        [Given(@"I have added a(?:n)? (appointment|collector|debt|student) with the following parameters")]
        public void AddObjectWithParameters(SdcaParts part, Table table)
        {
            if (part.Equals(SdcaParts.collector))
            {
                new CollectorSteps().CreateCollector(Transformations.GetCollectorCreator(table));
                ScenarioContext.Current.Set<Collector>(
                    Transformations.GetCollectorBasedOnCollectorCreator(Transformations.GetCollectorCreator(table)), "expectedObject");
            }
            else if (part.Equals(SdcaParts.appointment))
            {
                new AppointmentSteps().CreateAppointment(Transformations.GetAppointmentCreator(table));
                ScenarioContext.Current.Set<Appointment>(
                    Transformations.GetAppointmentBasedOnAppointmentCreator(Transformations.GetAppointmentCreator(table)), "expectedObject");
            }
            else if (part.Equals(SdcaParts.debt))
            {
                new DebtSteps().CreateDebt(Transformations.GetDebtCreator(table));
                ScenarioContext.Current.Set<Debt>(
                    Transformations.GetDebtBasedOnDebtCreator(Transformations.GetDebtCreator(table)), "expectedObject");
            }
            else if (part.Equals(SdcaParts.student))
            {
                new StudentSteps().CreateStudent(Transformations.GetStudentCreator(table));
                ScenarioContext.Current.Set<Student>(
                    Transformations.GetStudentBasedOnStudentCreator(Transformations.GetStudentCreator(table)), "expectedObject");
            }
        }

        [Given(@"I have deleted a(?:n)? (appointment|collector|debt|student) by (.*) id")]
        [When(@"I delete a(?:n)? (appointment|collector|debt|student) by (.*) id")]
        public void GivenIHaveDeletedAnObjectById(SdcaParts part, string id)
        {
            int neededId = new Actions().GetNeededId(id, part);

            if (part.Equals(SdcaParts.collector))
            {
                new CollectorSteps().DeleteCllectorById(neededId);
            }
            else if (part.Equals(SdcaParts.appointment))
            {
                new AppointmentSteps().DeleteAppointmentById(neededId);
            }
            else if (part.Equals(SdcaParts.debt))
            {
                new DebtSteps().DeleteDebtById(neededId);
            }
            else if (part.Equals(SdcaParts.student))
            {
                new StudentSteps().DeleteStudentById(neededId);
            }
            ScenarioContext.Current.Set<int>(neededId, "NeededId");
        }

        [Given(@"I have modified the (collector|student) with the following parameters")]
        public void GivenIHaveModifiedTheObjectWithTheFollowingParameters(SdcaParts part, Table table)
        {
            if (part.Equals(SdcaParts.collector))
            {
                Collector collector = Transformations.GetCollector(table);
                new CollectorSteps().ModifyCollector(collector);
                ScenarioContext.Current.Set<Collector>(collector, "expectedModifiedObject");
            }
            else if (part.Equals(SdcaParts.student))
            {
                Student student = Transformations.GetStudent(table);
                new StudentSteps().ModifyStudent(student);
                ScenarioContext.Current.Set<Student>(student, "expectedModifiedObject");
            }
        }
    }
}