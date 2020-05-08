using System;
using System.Collections.Generic;
using System.Text;
using GuidHelpers;

namespace SplurgeStop.Domain.StoreProfile
{
    public class Store
    {
        public Store()
        {
            Id = new StoreId(SequentialGuid.NewSequentialGuid());
        }
        public StoreId Id { get; }
        // store name, K-Citymarket Länsikeskus
        // chain, Kesko
        // store type, Citymarket
        // store location? Länsikeskus / Turku??? Finland!!
    }
}
