using System;
using System.Collections.Generic;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.PurchaseTransactionProfile
{
    /// <summary></summary>
    public class PurchaseDate : ValueObject
    {
        /// <summary>Gets or sets the PurchaseDate value (DateTime).</summary>
        /// <value>The PurchaseDate value.</value>
        public DateTime Value { get; protected set; }

        /// <summary>Initializes a new instance of the <see cref="PurchaseDate" /> class.</summary>
        /// <param name="date">The purchase date.</param>
        public PurchaseDate(DateTime date)
        {
            if (date == default)
                throw new ArgumentException("Invalid date!", nameof(date));

            Value = date;
        }

        /// <summary>Gets the equality components.</summary>
        /// <returns></returns>
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator PurchaseDate(DateTime value)
            => new PurchaseDate(value);

        public static PurchaseDate Now
            => new PurchaseDate(DateTime.Now);
    }
}