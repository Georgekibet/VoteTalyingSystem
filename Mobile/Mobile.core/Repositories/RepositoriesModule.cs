using StructureMap.Configuration.DSL;
using vts.Shared.Repository;

namespace Mobile.core.Repositories
{
    public class RepositoriesModule : Registry
    {
        public RepositoriesModule()
        {
            For<IUserRepository>().Use<UserRepository>();
            For<IConstituencyRepository>().Use<ConstituencyRepository>();
            For<ICountyRepository>().Use<CountyRepository>();
            For<IElectionRepository>().Use<ElectionRepository>();
            For<IPoliticalPartyRepository>().Use<PoliticalPartyRepository>();
            For<IPollingCentreRepository>().Use<PollingCentreRepository>();
            For<IRaceRepository>().Use<RaceRepository>();
            For<IRegionRepository>().Use<RegionRepository>();
            For<ISettingsRepository>().Use<SettingsRepository>();
            For<IUserRepository>().Use<UserRepository>();
            For<IWardRepository>().Use<WardRepository>();
        }
    }
}