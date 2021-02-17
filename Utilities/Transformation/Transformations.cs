using SdcaFramework.Clients;
using SdcaFramework.Clients.Creators;
using SdcaFramework.ClientSteps;

namespace SdcaFramework.Utilities
{
    public class Transformations
    {
        internal Collector GetCollectorBasedOnCollectorCreator(CollectorCreator creator)
        {
            return (new Collector
            {
                id = new CollectorSteps().LastCollectorId,
                nickname = creator.nickname,
                fearFactor = creator.fearFactor
            });
        }

        internal Appointment GetAppointmentBasedOnAppointmentCreator(AppointmentCreator creator)
        {
            return (new Appointment
            {
                id = new AppointmentSteps().LastAppointmentId,
                appointmentDate = creator.appointmentDate,
                collectorIds = creator.collectorIds,
                debtId = creator.debtId
            });
        }

        internal Debt GetDebtBasedOnDebtCreator(DebtCreator creator)
        {
            return (new Debt
            {
                id = new DebtSteps().LastDebtId,
                amount = creator.amount,
                monthlyPercent = creator.monthlyPercent,
                studentId = creator.studentId
            });
        }

        internal Student GetStudentBasedOnStudentCreator(StudentCreator creator)
        {
            return (new Student
            {
                id = new StudentSteps().LastStudentId,
                age = creator.age,
                name = creator.name,
                risk = creator.risk,
                sex = creator.sex
            });
        }
    }
}
