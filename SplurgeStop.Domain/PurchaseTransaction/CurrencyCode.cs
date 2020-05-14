using System;
using System.Collections.Generic;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class CurrencyCode : ValueObject
    {
        public string Value { get; protected set; }

        public CurrencyCode(string code)
        {
            if (code == null)
                throw new ArgumentNullException("Invalid Currency code!", nameof(code));

            if (code.Length != 3)
                throw new ArgumentException("Invalid Currency code! Currency code should be three characters long.", nameof(code));

            Value = code;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator CurrencyCode(string value)
            => new CurrencyCode(value);
    }
}