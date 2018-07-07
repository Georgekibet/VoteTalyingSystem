using vts.Shared.Entities.Master;

namespace Vts.WebLib.ViewModels
{
    public class PoliticalPartyViewModel
    {
        public PoliticalPartyViewModel()
        {
        }

        public PoliticalPartyViewModel(PoliticalParty politicalParty)
        {
            PoliticalParty = politicalParty;
        }

        public PoliticalParty PoliticalParty { get; set; }
    }
}