using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.PurchaseTransactionProfile
{
    public class PurchaseTransactionId : ValueObject
    {
        public Guid Value { get; protected set; }

        public PurchaseTransactionId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(PurchaseTransactionId self) => self.Value;

        public static implicit operator PurchaseTransactionId(Guid value)
            => new PurchaseTransactionId(new SequentialGuid(value));
    }
}