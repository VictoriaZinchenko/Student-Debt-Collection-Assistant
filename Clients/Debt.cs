using SdcaFramework.Clients.Creators;
using System;

namespace SdcaFramework.Clients
{
    public sealed class Debt : DebtCreator
    {
        public int id { get; set; }

        protected bool Equals(Debt other)
             => id == other.id
                && studentId == other.studentId
                && amount == other.amount
                && monthlyPercent == other.monthlyPercent;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Debt)obj);
        }

        public override int GetHashCode() => HashCode.Combine(id, studentId, amount, monthlyPercent);
    }
}
