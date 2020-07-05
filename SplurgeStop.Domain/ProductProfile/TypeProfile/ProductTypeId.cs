using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.ProductProfile.TypeProfile
{
    public class ProductTypeId : ValueObject
    {
        public Guid Value { get; protected set; }

        public ProductTypeId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(ProductTypeId self) => self.Value;

        public static implicit operator ProductTypeId(Guid value)
            => new ProductTypeId(new SequentialGuid(value));
    }
}