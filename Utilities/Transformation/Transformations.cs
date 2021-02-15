using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;

namespace SdcaFramework.Utilities
{
    public class Transformations
    {
        internal Collector GetCollectorBasedOnCollectorCreator(CollectorCreator creator, bool IsNewCreatedOne = true)
        {
            int lastId = new CollectorSteps().LastCollectorId;
            return (new Collector
            {
                id = IsNewCreatedOne ? lastId : lastId + 1,
                nickname = creator.nickname,
                fearFactor = creator.fearFactor
            });
        }

        internal Appointment GetAppointmentBasedOnAppointmentCreator(AppointmentCreator creator, bool IsNewCreatedOne = true)
        {
            int lastId = new AppointmentSteps().LastAppointmentId;
            return (new Appointment
            {
                id = IsNewCreatedOne ? lastId : lastId + 1,
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
                id = IsNewCreatedOne ? lastId : lastId + 1,
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
                id = IsNewCreatedOne ? lastId : lastId + 1,
                age = creator.age,
                name = creator.name,
                risk = creator.risk,
                sex = creator.sex
            });
        }
    }
}
