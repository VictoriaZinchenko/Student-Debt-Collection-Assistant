using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;
using SdcaFramework.Utilities.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TechTalk.SpecFlow;

namespace SdcaFramework.Utilities
{
    [Binding]
    public class StepArgumentTransformations
    {
        private readonly string Id = "id";
        private readonly string Nickname = "nickname";
        private readonly string FearFactor = "fearFactor";
        private readonly string AppointmentDate = "appointmentDate";
        private readonly string CollectorIds = "collectorIds";
        private readonly string Last = "last";
        private readonly string DebtId = "debtId";
        private readonly string Amount = "amount";
        private readonly string MonthlyPercent = "monthlyPercent";
        private readonly string StudentId = "studentId";
        private readonly string Age = "age";
        private readonly string Name = "name";
        private readonly string Risk = "risk";
        private readonly string Sex = "sex";

        [StepArgumentTransformation("(appointment|collector|debt|student)")]
        internal SdcaParts TransformSdcaPartsStringToEnum(string part)
            => (SdcaParts)System.Enum.Parse(typeof(SdcaParts), part);

        [StepArgumentTransformation]
        internal List<string> GetList(Table table)
            => table.Rows.Select(row => row.Values.First()).ToList();

        [StepArgumentTransformation]
        internal Dictionary<string, object> GetDictionary(Table table)
            => table.Rows.ToDictionary(row => row[0].ToString(), row => TryParseStringContent(row[1]));

        [StepArgumentTransformation]
        internal CollectorCreator GetCollectorCreator(Table table)
        {
            return table.Rows.Select(row => new CollectorCreator
            {
                nickname = row[Nickname],
                fearFactor = int.Parse(row[FearFactor])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Collector GetCollector(Table table)
        {
            return table.Rows.Select(row => new Collector
            {
                id = int.Parse(row[Id]),
                nickname = row[Nickname],
                fearFactor = int.Parse(row[FearFactor])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal AppointmentCreator GetAppointmentCreator(Table table)
        {
            return table.Rows.Select(row => new AppointmentCreator
            {
                appointmentDate = DateTime.Parse(row[AppointmentDate]),
                collectorIds = row[CollectorIds].Equals(Last) ? new List<int> { new CollectorSteps().LastCollectorId } :
                GetListOfIds(row[CollectorIds]),
                debtId = GetNeededId(row[DebtId], new DebtSteps().LastDebtId),
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Appointment GetAppointment(Table table)
        {
            return table.Rows.Select(row => new Appointment
            {
                id = int.Parse(row[Id]),
                appointmentDate = DateTime.Parse(row[AppointmentDate]),
                collectorIds = GetListOfIds(row[CollectorIds]),
                debtId = int.Parse(row[DebtId])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal DebtCreator GetDebtCreator(Table table)
        {
            return table.Rows.Select(row => new DebtCreator
            {
                amount = double.Parse(row[Amount]),
                monthlyPercent = double.Parse(row[MonthlyPercent]),
                studentId = GetNeededId(row[StudentId], new StudentSteps().LastStudentId),
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal Debt GetDebt(Table table)
        {
            return table.Rows.Select(row => new Debt
            {
                id = int.Parse(row[Id]),
                amount = double.Parse(row[Amount]),
                monthlyPercent = double.Parse(row[MonthlyPercent]),
                studentId = int.Parse(row[StudentId])
            }).FirstOrDefault();
        }

        [StepArgumentTransformation]
        internal StudentCreator GetStudentCreator(Table table)
        {
            StudentCreator student = table.Rows.Select(row => new StudentCreator
            {
                age = long.Parse(row[Age]),
                name = row[Name],
                risk = int.Parse(row[Risk]),
                sex = bool.Parse(row[Sex])
            }).FirstOrDefault();
            return student;
        }

        [StepArgumentTransformation]
        internal Student GetStudent(Table table)
        {
            return table.Rows.Select(row => new Student
            {
                id = int.Parse(row[Id]),
                age = long.Parse(row[Age]),
                name = row[Name],
                risk = int.Parse(row[Risk]),
                sex = bool.Parse(row[Sex])
            }).FirstOrDefault();

        }

        private List<int> GetListOfIds(string row)
            => row.Replace(", ", ",").Split(',').Select(id => int.Parse(id)).ToList();

        private int GetNeededId(string row, int lastId) => row.Equals(Last) ? lastId : int.Parse(row);

        private object TryParseStringContent(string value)
        {
            int intValue;
            double doubleValue;
            DateTime dateTimeValue;
            bool boolValue;
            return value switch
            {
                string _ when string.IsNullOrEmpty(value) => string.Empty,
                string _ when int.TryParse(value, out intValue) => intValue,
                string _ when double.TryParse(value, out doubleValue) => doubleValue,
                string _ when DateTime.TryParse(value, out dateTimeValue) => dateTimeValue,
                string _ when bool.TryParse(value, out boolValue) => boolValue,
                _ => value
            };
        }
    }
}
