using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using System;
using System.Linq;
using TechTalk.SpecFlow;

namespace SdcaFramework.Utilities
{
    [Binding]
    public class Transformations
    {
        [StepArgumentTransformation("(appointment|collector|debt|student)")]
        internal SdcaParts TransformSdcaPartsStringToEnum(string part)
            => (SdcaParts)System.Enum.Parse(typeof(SdcaParts), part);

        [StepArgumentTransformation]
        internal CollectorCreator GetCollectorCreator(Table table)
        {
            return table.Rows.Select(row => new CollectorCreator
            {
                nickname = row["nickname"],
                fearFactor = Int32.Parse(row["fearFactor"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Collector GetCollector(Table table)
        {
            return table.Rows.Select(row => new Collector
            {
                id = Int32.Parse(row["id"]),
                nickname = row["nickname"],
                fearFactor = Int32.Parse(row["fearFactor"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal AppointmentCreator GetAppointmentCreator(Table table)
        {
            return table.Rows.Select(row => new AppointmentCreator
            {
                appointmentDate = row["appointmentDate"],
                collectorIds = row["collectorIds"].Replace(", ", ",").Split(',').ToList().Select(id => Int32.Parse(id)).ToArray(),
                debtId = Int32.Parse(row["debtId"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Appointment GetAppointment(Table table)
        {
            return table.Rows.Select(row => new Appointment
            {
                id = Int32.Parse(row["id"]),
                appointmentDate = row["appointmentDate"],
                collectorIds = row["collectorIds"].Replace(", ", ",").Split(',').ToList().Select(id => Int32.Parse(id)).ToArray(),
                debtId = Int32.Parse(row["debtId"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal DebtCreator GetDebtCreator(Table table)
        {
            return table.Rows.Select(row => new DebtCreator
            {
                amount = Double.Parse(row["amount"]),
                monthlyPercent = Double.Parse(row["monthlyPercent"]),
                studentId = Int32.Parse(row["studentId"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Debt GetDebt(Table table)
        {
            return table.Rows.Select(row => new Debt
            {
                id = Int32.Parse(row["id"]),
                amount = Double.Parse(row["amount"]),
                monthlyPercent = Double.Parse(row["monthlyPercent"]),
                studentId = Int32.Parse(row["studentId"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal StudentCreator GetStudentCreator(Table table)
        {
            return table.Rows.Select(row => new StudentCreator
            {
                age = Int32.Parse(row["age"]),
                name = row["name"],
                risk = Int32.Parse(row["risk"]),
                sex = Boolean.Parse(row["sex"])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Student GetStudent(Table table)
        {
            return table.Rows.Select(row => new Student
            {
                id = Int32.Parse(row["id"]),
                age = Int32.Parse(row["age"]),
                name = row["name"],
                risk = Int32.Parse(row["risk"]),
                sex = Boolean.Parse(row["sex"])
            }).FirstOrDefault();
        }
    }
}
