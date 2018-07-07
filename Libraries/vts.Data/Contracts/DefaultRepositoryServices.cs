using vts.Core.Contracts;
using vts.Core.Repository;
using vts.Data.Repository.MasterData;
using vts.Data.Repository.Transactional;
using vts.Shared.Repository;

namespace vts.Data.Contracts
{
    public class DefaultRepositoryServices : IDefaultServicesList
    {
        public DefaultRepositoryServices()
        {
            Services = new DefaultServices()

                //MasterData

                .Add<IUserRepository, UserRepository>()
                .Add<ICandidateRepository, CandidateRepository>()
                .Add<IConstituencyRepository, ConstituencyRepository>()
                .Add<ICountyRepository, CountyRepository>()
                .Add<IElectionRepository,ElectionRepository>()
                .Add<IPoliticalPartyRepository, PoliticalPartyRepository>()
                .Add<IPollingCentreRepository, PollingCentreRepository>()
                .Add<IRaceRepository, RaceRepository>()
                .Add<IRegionRepository, RegionRepository>()
                .Add<ISettingsRepository, SettingsRepository>()
                .Add<IWardRepository, WardRepository>()
                
                //Results
                .Add<IPresidentialResultRepository, PresidentialResultRepository>()
                .Add<IGubernatorialResultRepository, GubernatorialResultRepository>()
                .Add<ISenatorialResultRepository, SenatorialResultRepository>()
                .Add<IMpResultRepository, MpResultRepository>()
                .Add<IWomenRepResultRepository, WomenRepResultRepository>()
                .Add<IMcaResultRepository, McaResultRepository>()
                .Add<IReferendumResultRepository, ReferendumResultRepository>()
                ;
        }

        public DefaultServices Services { get; set; }
    }
}
