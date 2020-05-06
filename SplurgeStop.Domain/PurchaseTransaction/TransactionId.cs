using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using GuidHelpers;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class TransactionId : ValueObject
    {
        public Guid Value { get; protected set; }

        public TransactionId(Guid id)
        {
            //Contract.Requires(id != default);
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        //public static implicit operator SequentialGuid(TransactionId id)
        //    => new SequentialGuid(id.Value);

        public static implicit operator Guid(TransactionId self) => self.Value;

        //public static implicit operator TransactionId(SequentialGuid id)
        //    => new TransactionId(id);

        public static implicit operator TransactionId(Guid value)
            => new TransactionId(new SequentialGuid(value));
    }
}