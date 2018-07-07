using System;
using vts.Core.Shared.Entities.Master;
using vts.Data.Context;
using vts.Shared.Entities.Master;

namespace Vts.Core.Tests
{
    public class BaseFixture
    {
        public static string Connection = "VtsContext";

        protected readonly Random Random = new Random();

        protected ContextConnection ContextConnection()
        {
            return new ContextConnection(Connection);
        }

        protected int GetRandomInt(int min, int max)
        {
            Random rnd = new Random();
            return rnd.Next(min, max);
        }

        protected Race CreateRace()
        {
            var raceEntity = new Race(Guid.NewGuid())
            {
                Name = "General",
                RaceType = RaceType.Presidential,
                Status = EntityStatus.Active,
            };
            return raceEntity;
        }

        protected PoliticalParty CreatePoliticalParty()
        {
            var politicalParty = new PoliticalParty(Guid.NewGuid())
            {
                Code = "001",
                Name = "Democratic",
                Acronym = "DMC",
                DateRegistered = DateTime.Now.AddYears(-5),
                Status = EntityStatus.Active,
            };
            return politicalParty;
        }

        protected Region CreateRegion()
        {
            var region = new Region(Guid.NewGuid())
            {
                Code = "P01".RandStr(),
                Name = "Coast".RandStr(),
                Status = EntityStatus.Active,
            };
            return region;
        }

        protected County CreateCounty(RegionRef region)
        {
            var county = new County(Guid.NewGuid())
            {
                Code = "001".RandStr(),
                Name = "Mombasa".RandStr(),
                Region = region,
                TotalRegisteredVoters = 100000,
                Status = EntityStatus.Active,
            };
            return county;
        }

        protected Constituency CreateConstituency(CountyRef county)
        {
            var constituencyEntity = new Constituency(Guid.NewGuid())
            {
                Name = "default".RandStr(),
                County = county,
                TotalRegisteredVoters = 40000,
                PopulationOver18 = 80000,
                Area = 54786,
                NumberOfWards = 100,
                NumberOfPollingStations = 200,
                Status = EntityStatus.Active
            };
            return constituencyEntity;
        }

        protected Ward CreateWard(ConstituencyRef constituency)
        {
            var ward = new Ward(Guid.NewGuid())
            {
                Code = "001".RandStr(),
                Name = "xyz".RandStr(),
                Constituency = constituency,
                Status = EntityStatus.Active,
            };
            return ward;
        }
    }
}