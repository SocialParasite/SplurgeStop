using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.LocationProfile
{
    public class LocationId : ValueObject
    {
        public Guid Value { get; protected set; }

        public LocationId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(LocationId self) => self.Value;

        public static implicit operator LocationId(Guid value)
            => new LocationId(new SequentialGuid(value));
    }
}