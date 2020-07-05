using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.ProductProfile
{
    public class ProductId : ValueObject
    {
        public Guid Value { get; protected set; }

        public ProductId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(ProductId self) => self.Value;

        public static implicit operator ProductId(Guid value)
            => new ProductId(new SequentialGuid(value));
    }
}