using SdcaFramework.Clients.Creators;
using System;
using System.Linq;

namespace SdcaFramework.Clients
{
    public sealed class Appointment : AppointmentCreator
    {
        public int id { get; set; }

        protected bool Equals(Appointment other)
            => id == other.id && debtId == other.debtId
            && Enumerable.SequenceEqual(collectorIds, other.collectorIds)
            && appointmentDate.Equals(other.appointmentDate);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Appointment)obj);
        }

        public override int GetHashCode() => HashCode.Combine(id, debtId, collectorIds, appointmentDate);
    }
}
