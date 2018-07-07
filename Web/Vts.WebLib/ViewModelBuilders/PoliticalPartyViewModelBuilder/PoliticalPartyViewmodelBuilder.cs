using System;
using System.Collections.Generic;
using System.Linq;
using vts.Shared.Repository;
using Vts.WebLib.ViewModels;

namespace Vts.WebLib.ViewModelBuilders.PoliticalPartyViewModelBuilder
{
    public class PoliticalPartyViewModelBuilder : IPoliticalPartyViewModelBuilder
    {
        private IPoliticalPartyRepository _politicalPartyRepository;

        public PoliticalPartyViewModelBuilder(IPoliticalPartyRepository politicalPartyRepository)
        {
            _politicalPartyRepository = politicalPartyRepository;
        }

        public List<PoliticalPartyViewModel> GetAll(bool inactive = false)
        {
            return _politicalPartyRepository.GetAll(inactive).ToList()
                .Select(party => new PoliticalPartyViewModel(party)) as List<PoliticalPartyViewModel>;
        }

        public PoliticalPartyViewModel Get(Guid id)
        {
            return new PoliticalPartyViewModel(_politicalPartyRepository.GetById(id));
        }

        public void Save(PoliticalPartyViewModel politicalPartyViewModel)
        {
            _politicalPartyRepository.Save(politicalPartyViewModel.PoliticalParty);
        }

        public void SetInactive(PoliticalPartyViewModel politicalPartyViewModel)
        {
            var party = _politicalPartyRepository.GetById(politicalPartyViewModel.PoliticalParty.Id);
            _politicalPartyRepository.SetInactive(party);
        }

        public void SetActive(PoliticalPartyViewModel politicalPartyViewModel)
        {
            var party = _politicalPartyRepository.GetById(politicalPartyViewModel.PoliticalParty.Id);
            _politicalPartyRepository.SetActive(party);
        }

        public void SetAsDeleted(PoliticalPartyViewModel politicalPartyViewModel)
        {
            var party = _politicalPartyRepository.GetById(politicalPartyViewModel.PoliticalParty.Id);
            _politicalPartyRepository.SetAsDeleted(party);
        }
    }
}