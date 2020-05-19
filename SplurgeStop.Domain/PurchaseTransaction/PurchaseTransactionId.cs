using System;
using System.Collections.Generic;
using GuidHelpers;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class PurchaseTransactionId : ValueObject
    {
        public Guid Value { get; protected set; }

        internal PurchaseTransactionId(Guid id)
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