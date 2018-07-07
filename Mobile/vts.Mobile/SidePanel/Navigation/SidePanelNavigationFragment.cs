using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agrimanagr.Mobile.SidePanel;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Mobile.core;
using Mobile.Common.Core;
using vts.Core.Shared.Entities.Master;
using vts.Mobile.Home;

namespace vts.Mobile.SidePanel.Navigation
{
    public class SidePanelNavigationFragment :NestedFragment<User>
    {
        private readonly List<NavigationItemsDefiniton<SidePanelNavigationFragment>> itemDefinitons = new List
            <NavigationItemsDefiniton<SidePanelNavigationFragment>>
        {
            new NavigationItemsDefiniton<SidePanelNavigationFragment>
            {
                IconId = Resource.Drawable.ic_routes,
                NameId = Resource.String.home_name_id,
                Action = fragment =>
                {
                    fragment.Activity.ShowRootFragment(typeof (HomeFragment));
                }
                //CountQuery = "SELECT COUNT(MasterId) FROM Route"
            }
        };
        private NavigationListAdapter<SidePanelNavigationFragment> navigationListAdapter;
        public override void CreateChildViews(View parent, Bundle bundle)
        {
            SetupNavigationList(parent);

        }
        private void SetupNavigationList(View parent)
        {
            var listView = parent.FindViewById<ListView>(Resource.Id.navigation_list);
            var adapter = navigationListAdapter = new NavigationListAdapter<SidePanelNavigationFragment>(Activity);
            listView.Adapter = adapter;

          
            listView.ItemClick += delegate (object sender, AdapterView.ItemClickEventArgs args)
            {
                adapter.SelectedPosition = args.Position;
                listView.InvalidateViews();
                var item = adapter.GetItem(args.Position);
                item.Action(this);
                Activity.CloseNavigationDrawer();
            };

           // Activity.ShowRootFragment();
           AddNavigationItems();
          var defaultItem = adapter.GetItem(0);
            defaultItem.Action(this);
        }
        private void AddNavigationItems()
        {
            var items = new List<NavigationItem<SidePanelNavigationFragment>>();
            itemDefinitons.ForEach(
                i => { items.Add(new NavigationItem<SidePanelNavigationFragment>(i.IconId, i.NameId, i.Action, 0)); });

            navigationListAdapter.Clear();
            navigationListAdapter.AddAll(items);
        }

    }

    public class NavigationItemsDefiniton<T>
    {
        public int IconId { get; set; }
        public int NameId { get; set; }
        public Action<T> Action { get; set; }
        public string CountQuery { get; set; }
    }
}