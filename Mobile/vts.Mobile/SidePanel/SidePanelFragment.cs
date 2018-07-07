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
using Mobile.core;
using Mobile.Common;
using Mobile.Common.Core;
using vts.Core.Shared.Entities.Master;
using vts.Mobile.SidePanel.Navigation;

namespace vts.Mobile.SidePanel
{
    public class SidePanelFragment : NestedFragment<User>
    {
        private readonly SidePanelNavigationFragment navigationFragment = new SidePanelNavigationFragment
        {
            RetainInstance = true
        };

        private List<ImageView> buttons; 
        public override void CreateChildViews(View parent, Bundle bundle)
        {
            //throw new NotImplementedException();
            buttons= new List<ImageView>();
            SetupView(parent, Resource.Id.screen_list, navigationFragment);
         /*   SetupView(parent, Resource.Id.sync, syncFragment);
            SetupView(parent, Resource.Id.reports, reportsFragment);
            SetupView(parent, Resource.Id.settings, settingsFragment);*/

            ShowView(buttons[0], navigationFragment);
        }
        private void SetupView(View parent, int viewId, BaseFragment<User> fragment)
        {
            var view = parent.FindViewById<ImageView>(viewId);
            buttons.Add(view);

            view.Click += delegate
            {
                ShowView(view, fragment);
            };
        }

        private void ShowView(ImageView selected, BaseFragment<User> fragment)
        {
            ShowNestedFragment(Resource.Id.navigation_fragment_container, fragment);
        }
    }
}