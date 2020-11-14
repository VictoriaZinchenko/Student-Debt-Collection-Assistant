using System;

namespace SdcaFramework.Clients
{
    public class Debt
    {
        public int id { get; set; }
        public int studentId { get; set; }
        public double amount { get; set; }
        public double monthlyPercent { get; set; }

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
