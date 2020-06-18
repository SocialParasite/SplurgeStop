using System;
using System.Collections.Generic;
using GuidHelpers;

namespace SplurgeStop.Domain.CountryProfile
{
    public class CountryId : ValueObject
    {
        public Guid Value { get; protected set; }

        public CountryId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(CountryId self) => self.Value;

        public static implicit operator CountryId(Guid value)
            => new CountryId(new SequentialGuid(value));
    }
}