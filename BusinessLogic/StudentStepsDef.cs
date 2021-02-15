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
    public sealed class StudentStepsDef : BaseStepDef
    {
        private readonly string ExpectedStudent = "expectedStudent";

        public StudentStepsDef(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        private Student ContextExpectedStudent => ScenarioContext.Get<Student>(ExpectedStudent);

        [Given(@"I have added a student with the following parameters")]
        [When(@"I add a student with the following parameters")]
        public void AddStudentWithParameters(StudentCreator student)
        {
            new StudentSteps().CreateStudent(student);
            Student expectedStudent = Transformations.GetStudentBasedOnStudentCreator(student);
            ScenarioContext.Set(expectedStudent, ExpectedStudent);
            Logger.Info($"\nStudent created with the following properties. " +
                $"{PropertiesDescriber.GetObjectProperties(expectedStudent)}");
        }

        [Given(@"I have modified the student with the following parameters(?: again)?")]
        [When(@"I modify the student with the following parameters(?: again)?")]
        public void ModifyStudentWithFollowingParameters(Student student)
        {
            Logger.Info($"\nTry to modify the student with {student.id} id");
            new StudentSteps().ModifyStudent(student);
            ScenarioContext.Set(student, "expectedModifiedStudent");
        }

        [Then(@"I can see the created student in the list")]
        public void CheckStudentPresenceInList()
        {
            var actualObjectsList = new List<object>();
            Student expectedObject = ContextExpectedStudent;
            new StudentSteps().GetListOfStudents().ForEach(element => actualObjectsList.Add(element));
            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"(?:I check again that )?the student data is saved correctly")]
        public void ThenStudentIsSavedCorrectly()
        {
            Student expectedObject = ContextExpectedStudent;
            Student actualObject = GetStudentDataById("last");
            Assert.AreEqual(expectedObject, actualObject,
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the student data with (.*) id is modified correctly")]
        public void ThenStudentIsModifiedCorrectly(string id)
        {
            Student expectedObject = ScenarioContext.Get<Student>("expectedModifiedStudent");
            Student actualObject = GetStudentDataById(id);
            Assert.AreEqual(expectedObject, actualObject, 
                PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Given(@"I have deleted a student by (.*) id")]
        [When(@"I delete a student by (.*) id")]
        public void DeleteStudentById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.student);
            new StudentSteps().DeleteStudentById(neededId);
            SetNeededIdToContext(neededId);
            SetActualStatusCodeToContext(new StudentSteps().GetStatusCodeGetStudentByIdAction(neededId));
            Logger.Info($"\nStudent with {neededId} id deleted");
        }

        [Given(@"I have tried to delete the removed student by (.*) id")]
        [When(@"I try to delete the removed student by (.*) id")]
        public void TryToDeleteReomovedStudentById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.student);
            Logger.Info($"\nTry to delete the removed student by {neededId} id");
            SetNeededIdToContext(neededId);
            SetActualStatusCodeToContext(new StudentSteps().GetStatusCodeDeleteStudentAction(neededId));
        }

        [Then(@"the system can't find the student data")]
        public void ThenSystemDidNotFindStudent()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ContextActualStatusCode, AssertNotFoundMessage);
        }

        [Then(@"the system can't create the student data")]
        public void ThenSystemDidNotCreateStudent()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ContextActualStatusCode, AssertBadRequestMessage);
        }

        [When(@"I try to add a student with invalid parameter")]
        public void TryToAddStudentWithInvalidParameter(Dictionary<string, object> parameters)
        {
            Logger.Info($"\nTry to add a student with invalid parameter");
            SetActualStatusCodeToContext(new StudentSteps().GetStatusCodeForInvalidPostAction(parameters));
        }

        [Then(@"I find only one student with the following parameters")]
        public void FindStudentByParameters(StudentCreator expectedStudent)
        {
            int countOfObjects = new StudentSteps().GetListOfStudents()
                .Count(student => student.name.Equals(expectedStudent.name)
                && student.age.Equals(expectedStudent.age)
                && student.sex.Equals(expectedStudent.sex)
                && student.risk.Equals(expectedStudent.risk));
            Assert.AreEqual(1, countOfObjects,
                $"There should be only one student with the following properties. Actual count of such students is {countOfObjects}." +
                $"{PropertiesDescriber.GetObjectProperties(expectedStudent)}");
        }

        private Student GetStudentDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.student);
            return new StudentSteps().GetStudentById(neededId);
        }
    }
}
