using SdcaFramework.Clients.Creators;
using System;

namespace SdcaFramework.Clients
{
    public sealed class Student : StudentCreator
    {
        public int id { get; set; }

        protected bool Equals(Student other)
            => id == other.id
               && name == other.name
               && age == other.age
               && sex == other.sex
               && risk == other.risk;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Student)obj);
        }

        public override int GetHashCode() => HashCode.Combine(id, name, age, sex, risk);
    }
}
