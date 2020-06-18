using System;
using System.Collections.Generic;
using GuidHelpers;

namespace SplurgeStop.Domain.CityProfile
{
    public class CityId : ValueObject
    {
        public Guid Value { get; protected set; }

        public CityId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(CityId self) => self.Value;

        public static implicit operator CityId(Guid value)
            => new CityId(new SequentialGuid(value));
    }
}