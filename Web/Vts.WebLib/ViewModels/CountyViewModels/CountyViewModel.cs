using vts.Shared.Entities.Master;

namespace Vts.WebLib.ViewModels.CountyViewModels
{
    public class CountyViewModel
    {
        public CountyViewModel()
        {
        }

        public CountyViewModel(County county)
        {
            County = county;
        }

        public County County { get; set; }
    }
}