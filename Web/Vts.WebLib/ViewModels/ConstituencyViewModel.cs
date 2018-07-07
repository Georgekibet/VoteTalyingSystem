using vts.Shared.Entities.Master;

namespace Vts.WebLib.ViewModels
{
    public class ConstituencyViewModel
    {
        public ConstituencyViewModel()
        {
        }

        public ConstituencyViewModel(Constituency constituency)
        {
            Constituency = constituency;
        }

        public Constituency Constituency { get; set; }
    }
}
