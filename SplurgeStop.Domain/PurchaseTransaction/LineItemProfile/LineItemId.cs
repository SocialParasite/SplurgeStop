using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.PurchaseTransaction.LineItemProfile
{
    public class LineItemId : ValueObject
    {
        public Guid Value { get; protected set; }

        internal LineItemId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(LineItemId self)
            => self.Value;

        public static implicit operator LineItemId(Guid value)
            => new LineItemId(new SequentialGuid(value));
    }
}