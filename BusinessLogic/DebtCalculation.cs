using SdcaFramework.Clients;
using SdcaFramework.ClientSteps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SdcaFramework.BusinessLogic
{
    public class DebtCalculation
    {
        public double RecalculateAmount(Debt debt, double originalAmount, DateTime dateOfLastUpdate, DateTime targetDate)
        {
            double result = originalAmount;
            double dailyRate;
            double monthlyPercent = debt.monthlyPercent / 100; //convert interest
            List<DateTime> notUpdatedPeriodOfTime = GetListOfDates(dateOfLastUpdate, targetDate);
            double discount = CalculateDiscount(debt.studentId);
            foreach (DateTime date in notUpdatedPeriodOfTime)
            {
                dailyRate = (monthlyPercent / DateTime.DaysInMonth(date.Year, date.Month)) * discount;
                result += dailyRate;
            }
            return result;
        }

        private double CalculateDiscount(int studentId)
        {
            Student student = new StudentSteps().GetStudentById(studentId);
            double sexDiscount = student.sex ? 1 : 0.9;
            double ageDiscount = student.age switch
            {
                long age when age < 21 => 0.9,
                long age when age < 18 => 0.8,
                _ => 1
            };
            return sexDiscount * ageDiscount;
        }

        private List<DateTime> GetListOfDates(DateTime dateOfLastUpdate, DateTime targetDate)
        {
            var periodOfTime = Enumerable.Range(0, (int)(targetDate - dateOfLastUpdate).TotalDays + 1)
                        .Select(number => dateOfLastUpdate.AddDays(number))
                        .ToList();
            periodOfTime.Remove(periodOfTime.Last()); //exclude today
            return periodOfTime;
        }
    }
}
