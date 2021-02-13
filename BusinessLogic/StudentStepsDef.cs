using NUnit.Framework;
using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using SdcaFramework.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TechTalk.SpecFlow;

namespace SdcaFramework.BusinessLogic
{
    [Binding]
    public sealed class StudentStepsDef : BaseStepDef
    {
        public StudentStepsDef(ScenarioContext scenarioContext) : base(scenarioContext)
        {

        }

        [When(@"I get the list of students")]
        public void SetListOfStudentsToContext()
        {
            ScenarioContext.Set<List<Student>>(new StudentSteps().GetListOfStudents(), "listOfStudents");
        }

        [Given(@"I have got a student data by (.*) id")]
        [When(@"I get a student data by (.*) id")]
        public void WhenIGetDataById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.student);
            ScenarioContext.Set<Student>(new StudentSteps().GetStudentById(neededId), "actualStudent");
        }

        [Given(@"I have added a student with the following( invalid)* parameters")]
        [When(@"I add a student with the following( invalid)* parameters")]
        public void AddObjectWithParameters(string invalidParameter, StudentCreator student)
        {
            if (!string.IsNullOrEmpty(invalidParameter))
            {
                ScenarioContext.Set<HttpStatusCode>(new StudentSteps().GetResponseCreateStudentAction(student), "ActualStatusCode");
            }
            new StudentSteps().CreateStudent(student);
                ScenarioContext.Set<Student>(
                Transformations.GetStudentBasedOnStudentCreator(student), "expectedStudent");
        }

        [Given(@"I have modified the student with the following parameters")]
        public void GivenIHaveModifiedTheObjectWithTheFollowingParameters(Student student)
        {
                new StudentSteps().ModifyStudent(student);
                ScenarioContext.Set<Student>(student, "expectedModifiedStudent");
        }

        [Then(@"I can see the created student in the list")]
        public void ThenICanSeeThisObject()
        {
            object expectedObject = null;
            var actualObjectsList = new List<object>();

                expectedObject = ScenarioContext.Get<Student>("expectedStudent");
                ScenarioContext.Get<List<Student>>("listOfStudents").ForEach(element => actualObjectsList.Add(element));
            
            Assert.Contains(expectedObject, actualObjectsList,
                PropertiesDescriber.GetActualObjectsListAndExpectedObjectProperties(expectedObject, actualObjectsList));
        }

        [Then(@"the student data is saved correctly")]
        public void ThenTheDataIsSavedCorrectly()
        {
            object expectedObject = null;
            object actualObject = null;

                expectedObject = ScenarioContext.Get<Student>("expectedStudent");
                actualObject = ScenarioContext.Get<Student>("actualStudent");
            
            Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Then(@"the student data is modified correctly")]
        public void ThenTheDataIsModifiedCorrectly()
        {
            object expectedObject = null;
            object actualObject = null;

                expectedObject = ScenarioContext.Get<Student>("expectedModifiedStudent");
                actualObject = ScenarioContext.Get<Student>("actualStudent");
            
            Assert.AreEqual(expectedObject, actualObject, PropertiesDescriber.GetActualAndExpectedObjectsProperties(expectedObject, actualObject));
        }

        [Given(@"I have deleted a student by (.*) id")]
        [When(@"I delete a student by (.*) id")]
        public void GivenIHaveDeletedAnObjectById(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.student);
            new StudentSteps().DeleteStudentById(neededId);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new StudentSteps().GetResponseGetStudentByIdAction(neededId),
                "ActualStatusCode");
        }

        [Given(@"I have tried to delete the removed student by (.*) id")]
        [When(@"I try to delete the removed student by (.*) id")]
        public void GivenIHaveDeletedAnObjectByIdmmmmmmmmmmmmmmmmmmmmmmm(string id)
        {
            int neededId = GetNeededId(id, SdcaParts.student);
            ScenarioContext.Set<int>(neededId, "NeededId");
            ScenarioContext.Set<HttpStatusCode>(new StudentSteps().GetResponseDeleteStudentAction(neededId),
                "ActualStatusCode");
        }

        [Then(@"the system can't find the student data")]
        public void ThenTheSystemDidNotFindTheData()
        {
            Assert.AreEqual(HttpStatusCode.NotFound,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Not Found'.");
        }

        [Then(@"the system can't create the student data")]
        public void ThenTheSystemDidNotCreateTheData()
        {
            Assert.AreEqual(HttpStatusCode.BadRequest,
                ScenarioContext.Get<HttpStatusCode>("ActualStatusCode"), "Expected status code should be 'Bad Request'.");
        }
    }
}
