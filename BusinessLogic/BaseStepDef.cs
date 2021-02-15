using NLog;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using SdcaFramework.Utilities.Enums;
using System;
using System.Net;
using TechTalk.SpecFlow;

namespace SdcaFramework.BusinessLogic
{
    [Binding]
    public class BaseStepDef
    {
        protected Logger Logger = LogManager.GetCurrentClassLogger();
        protected readonly ScenarioContext ScenarioContext;
        protected readonly Transformations Transformations = new Transformations();
        protected readonly StepArgumentTransformations StepArgumentTransformations = new StepArgumentTransformations();
        protected readonly string AssertBadRequestMessage = "Expected status code should be 'Bad Request'.";
        protected readonly string AssertNotFoundMessage = "Expected status code should be 'Not Found'.";
        private readonly string ActualStatusCode = "ActualStatusCode";

        public BaseStepDef(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext;
        }

        protected HttpStatusCode ContextActualStatusCode => ScenarioContext.Get<HttpStatusCode>(ActualStatusCode);

        protected void SetNeededIdToContext(int neededId) => ScenarioContext.Set(neededId, "NeededId");

        protected void SetActualStatusCodeToContext(HttpStatusCode statusCode) => ScenarioContext.Set(statusCode, ActualStatusCode);

        protected int GetNeededId(string id, SdcaParts part)
        {
            int neededId;
            if (id.Equals("last"))
            {
                neededId = part switch
                {
                    SdcaParts.collector => new CollectorSteps().LastCollectorId,
                    SdcaParts.appointment => new AppointmentSteps().LastAppointmentId,
                    SdcaParts.debt => new DebtSteps().LastDebtId,
                    SdcaParts.student => new StudentSteps().LastStudentId,
                    _ => throw new NotImplementedException($"The GetNeededId method has no implementation for '{part}' object")
                };
            }
            else if (id.Equals("this"))
            {
                neededId = ScenarioContext.Get<int>("NeededId");
            }
            else
            {
                neededId = int.Parse(id);
            }
            return neededId;
        }
    }
}
