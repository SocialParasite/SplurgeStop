using System;
using System.Collections.Generic;
using GuidHelpers;

namespace SplurgeStop.Domain.ProductProfile
{
    public class ProductId : ValueObject
    {
        public Guid Value { get; protected set; }

        internal ProductId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator ProductId(Guid value)
            => new ProductId(new SequentialGuid(value));
    }
}