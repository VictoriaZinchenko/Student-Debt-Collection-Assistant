using BoDi;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;


namespace SdcaFramework.BusinessLogic
{
    [Binding]
    public class BaseStepDef
    {
        protected readonly ScenarioContext ScenarioContext;
        protected readonly Transformations Transformations = new Transformations();
        protected readonly StepArgumentTransformations StepArgumentTransformations = new StepArgumentTransformations();

        public BaseStepDef(ScenarioContext scenarioContext)
        {
            this.ScenarioContext = scenarioContext;
        }

        //[BeforeScenario]
        //public void BeforeTest()
        //{
        //    ScenarioContext = new ScenarioContext();
        //}

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
                neededId = Int32.Parse(id);
            }
            return neededId;
        }
    }
}
