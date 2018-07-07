using vts.Shared.Entities.Master;

namespace Vts.WebLib.ViewModels
{
    public class RegionViewModel
    {
        public RegionViewModel()
        {
        }

        public RegionViewModel(Region region)
        {
            Region = region;
        }

        public Region Region { get; set; }
    }
}
