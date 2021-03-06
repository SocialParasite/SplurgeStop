﻿using System;
using System.Collections.Generic;
using GuidHelpers;
using SplurgeStop.Domain.Shared;

namespace SplurgeStop.Domain.StoreProfile
{
    public class StoreId : ValueObject
    {
        public Guid Value { get; protected set; }

        public StoreId(Guid id)
        {
            if (id == default)
                throw new ArgumentException("Invalid id!", nameof(id));

            Value = id;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static implicit operator Guid(StoreId self) => self.Value;

        public static implicit operator StoreId(Guid value)
            => new StoreId(new SequentialGuid(value));
    }
}