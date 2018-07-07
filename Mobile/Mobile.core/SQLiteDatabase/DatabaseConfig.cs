using System;
using System.Collections.Generic;
using System.Linq;
using vts.Core.Shared.Entities.Master;
using vts.Shared.Entities.Master;

namespace Agrimanagr.Core.Storage
{
    public class DatabaseConfig
    {
        //Types with data that are cleaned when a new user logs in
        private static readonly List<Type> TransientTypes = new List<Type>
        {
            typeof (User),
            typeof (Constituency),
            typeof (County),
            typeof (Election),
            typeof (PoliticalParty),
            typeof (PollingCentre),
            typeof (Race),
            typeof (Region),
            typeof (Settings),
            typeof (Ward)
        };

        //Types that are not cleaned
        private static readonly List<Type> PermenantTypes = new List<Type>();

        public static IEnumerable<Type> GetPersistentTypes()
        {
            return TransientTypes.Concat(PermenantTypes);
        }

        public static IEnumerable<Type> GetTransientTypes()
        {
            return TransientTypes;
        }
    }
}