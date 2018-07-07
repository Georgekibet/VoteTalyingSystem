using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Pitt.Library.Fresh;
using Mobile.Common;
using vts.Core.Shared.Entities.Master;

namespace vts.Mobile.Home
{
   public class HomeFragment : BaseFragment<User>
    {
       public override void CreateChildViews(View parent, Bundle bundle)
       {
            //throw new NotImplementedException();
            FreshDownloadView progressbar = (FreshDownloadView)parent.FindViewById(Resource.Id.pitt);
            // progressbar.setProgressColor();
            progressbar.UpDateProgress(50);
            SetTitle("Results summary");

        }
    }
}