using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;
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
                collectorIds = row["collectorIds"] == "last" ? new int[] { new CollectorSteps().LastCollectorId } :
                row["collectorIds"].Replace(", ", ",").Split(',').ToList().Select(id => Int32.Parse(id)).ToArray(),
                debtId = row["debtId"] == "last" ? new DebtSteps().LastDebtId : Int32.Parse(row["debtId"]),
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
                studentId = row["studentId"] == "last" ? new StudentSteps().LastStudentId : Int32.Parse(row["studentId"]),
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
                age = Int64.Parse(row["age"]),
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
                age = Int64.Parse(row["age"]),
                name = row["name"],
                risk = Int32.Parse(row["risk"]),
                sex = Boolean.Parse(row["sex"])
            }).FirstOrDefault();
        }

        internal Collector GetCollectorBasedOnCollectorCreator(CollectorCreator creator, bool IsNewCreatedOne = true)
        {
            int lastId = new CollectorSteps().LastCollectorId;
            return (new Collector
            {
                id = IsNewCreatedOne == true ? lastId : lastId + 1,
                nickname = creator.nickname,
                fearFactor = creator.fearFactor
            });
        }

        internal Appointment GetAppointmentBasedOnAppointmentCreator(AppointmentCreator creator, bool IsNewCreatedOne = true)
        {
            int lastId = new AppointmentSteps().LastAppointmentId;
            return (new Appointment
            {
                id = IsNewCreatedOne == true ? lastId : lastId + 1,
                appointmentDate = creator.appointmentDate,
                collectorIds = creator.collectorIds,
                debtId = creator.debtId
            });
        }

        internal Debt GetDebtBasedOnDebtCreator(DebtCreator creator, bool IsNewCreatedOne = true)
        {
            int lastId = new DebtSteps().LastDebtId;
            return (new Debt
            {
                id = IsNewCreatedOne == true ? lastId : lastId + 1,
                amount = creator.amount,
                monthlyPercent = creator.monthlyPercent,
                studentId = creator.studentId
            });
        }

        internal Student GetStudentBasedOnStudentCreator(StudentCreator creator, bool IsNewCreatedOne = true)
        {
            int lastId = new StudentSteps().LastStudentId;
            return (new Student
            {
                id = IsNewCreatedOne == true ? lastId : lastId + 1,
                age = creator.age,
                name = creator.name,
                risk = creator.risk,
                sex = creator.sex
            });
        }
    }
}
