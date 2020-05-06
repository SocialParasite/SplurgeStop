using System;
using System.Collections.Generic;
using System.Text;
using GuidHelpers;

namespace SplurgeStop.Domain.StoreProfile
{
    public sealed class Store
    {
        public StoreId Id { get; }

        public Store()
        {
            Id = new StoreId(SequentialGuid.NewSequentialGuid());
        }
    }
}
