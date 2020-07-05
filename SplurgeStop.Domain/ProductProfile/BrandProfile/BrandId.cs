using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.ProductProfile.BrandProfile
{
    public class BrandId : ValueObject
    {
        public Guid Value { get; protected set; }

        public BrandId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(BrandId self) => self.Value;

        public static implicit operator BrandId(Guid value)
            => new BrandId(new SequentialGuid(value));
    }
}