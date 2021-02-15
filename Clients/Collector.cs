using SdcaFramework.Clients.Creators;
using System;

namespace SdcaFramework.Clients
{
    public sealed class Collector : CollectorCreator
    {
        public int id { get; set; }

        protected bool Equals(Collector other)
            => id == other.id
              && nickname == other.nickname
              && fearFactor == other.fearFactor;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Collector)obj);
        }

        public override int GetHashCode() => HashCode.Combine(id, nickname, fearFactor);
    }
}
