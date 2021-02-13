using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace SdcaFramework.Utilities
{
    [Binding]
    public class StepArgumentTransformations
    {
        [StepArgumentTransformation("(appointment|collector|debt|student)")]
        internal SdcaParts TransformSdcaPartsStringToEnum(string part)
            => (SdcaParts)System.Enum.Parse(typeof(SdcaParts), part);

        [StepArgumentTransformation]
        internal List<string> GetList(Table table)
        {
            return table.Rows.Select(row => row.Values.First()).ToList();
        }

        [StepArgumentTransformation]
        internal CollectorCreator GetCollectorCreator(Table table)
        {
            return table.Rows.Select(row => new CollectorCreator
            {
                nickname = row["nickname"],
                fearFactor = int.Parse(row["fearFactor"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Collector GetCollector(Table table)
        {
            return table.Rows.Select(row => new Collector
            {
                id = int.Parse(row["id"]),
                nickname = row["nickname"],
                fearFactor = int.Parse(row["fearFactor"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal AppointmentCreator GetAppointmentCreator(Table table)
        {
            return table.Rows.Select(row => new AppointmentCreator
            {
                appointmentDate = row["appointmentDate"],
                collectorIds = row["collectorIds"] == "last" ? new List<int> { new CollectorSteps().LastCollectorId } :
                GetListOfIds(row["collectorIds"]),
                debtId = GetNeededId(row["debtId"], new DebtSteps().LastDebtId),
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Appointment GetAppointment(Table table)
        {
            return table.Rows.Select(row => new Appointment
            {
                id = int.Parse(row["id"]),
                appointmentDate = row["appointmentDate"],
                collectorIds = GetListOfIds(row["collectorIds"]),
                debtId = int.Parse(row["debtId"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal DebtCreator GetDebtCreator(Table table)
        {
            return table.Rows.Select(row => new DebtCreator
            {
                amount = Double.Parse(row["amount"]),
                monthlyPercent = Double.Parse(row["monthlyPercent"]),
                studentId = GetNeededId(row["studentId"], new StudentSteps().LastStudentId),
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Debt GetDebt(Table table)
        {
            return table.Rows.Select(row => new Debt
            {
                id = int.Parse(row["id"]),
                amount = Double.Parse(row["amount"]),
                monthlyPercent = Double.Parse(row["monthlyPercent"]),
                studentId = int.Parse(row["studentId"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal StudentCreator GetStudentCreator(Table table)
        {
            //try
            //{
                StudentCreator student = table.Rows.Select(row => new StudentCreator
                {
                    age = long.Parse(row["age"]),
                    name = row["name"],
                    risk = int.Parse(row["risk"]),
                    sex = Boolean.Parse(row["sex"])
                }).FirstOrDefault();
                return student;

            //}
            //catch (System.FormatException)
            //{
            //    //add log
            //}
        }

        [StepArgumentTransformation]
        internal Student GetStudent(Table table)
        {
            Student student = null;
            try
            {
                student = table.Rows.Select(row => new Student
                {
                    id = int.Parse(row["id"]),
                    age = long.Parse(row["age"]),
                    name = row["name"],
                    risk = int.Parse(row["risk"]),
                    sex = Boolean.Parse(row["sex"])
                }).FirstOrDefault();
            }
            catch (System.FormatException)
            {
                //add log
            }
            return student;
        }

        private List<int> GetListOfIds(string row) 
            => row.Replace(", ", ",").Split(',').ToList().Select(id => int.Parse(id)).ToList();

        private int GetNeededId(string row, int lastId) => row == "last" ? lastId : int.Parse(row);
    }
}
