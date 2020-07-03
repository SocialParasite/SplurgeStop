using System;
using System.Collections.Generic;
using GuidHelpers;

namespace SplurgeStop.Domain.ProductProfile.SizeProfile
{
    public class SizeId : ValueObject
    {
        public Guid Value { get; protected set; }

        public SizeId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(SizeId self) => self.Value;

        public static implicit operator SizeId(Guid value)
            => new SizeId(new SequentialGuid(value));
    }
}