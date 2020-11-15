using SdcaFramework.Clients;
using SdcaFramework.ClientSteps;
using System;
using TechTalk.SpecFlow;

namespace SdcaFramework.BusinessLogic
{
    public class DebtCalculation
    {
        private double CalculateDiscount(int studentId)
        {
            Student student = new StudentSteps().GetStudentById(studentId);
            double sexDiscount = student.sex == false ? 0.9 : 1;
            double ageDiscount = 1;
            //TimeSpan targetStudentAge = DateTime.Now - student.BirthDate;
            if (student.age < 21)
            {
                ageDiscount = 0.9;
            }
            if (student.age < 18)
            {
                ageDiscount = 0.8;
            }
            return sexDiscount * ageDiscount;
        }

        public double RecalculateAmount(Debt debt)
        {
            TimeSpan passedSinceLastUpdate = GetDebtDate(DateTime.Now) - GetDebtDate(ScenarioContext.Current.Get<DateTime>("lastUpdatedDate"));
            double dailyRate = debt.monthlyPercent * CalculateDiscount(debt.studentId) / 30;
            return debt.amount * Math.Pow(1 + dailyRate, passedSinceLastUpdate.TotalDays);
        }
        public static DateTime GetDebtDate(DateTime realDate)
        {
            return realDate.Date + new TimeSpan(5, 5, 5);
        }
    }
}
