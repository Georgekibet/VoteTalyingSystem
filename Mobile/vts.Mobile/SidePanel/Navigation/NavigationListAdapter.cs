using System;

using Android.Content;
using Android.Views;
using Android.Widget;
using vts.Mobile;

namespace Agrimanagr.Mobile.SidePanel
{
    public class NavigationListAdapter<T> : ArrayAdapter<NavigationItem<T>>
    {
        private readonly Context context;

        public NavigationListAdapter(Context context)
            : base(context, Resource.Layout.side_panel_navigation_list_item)
        {
            this.context = context;
            SelectedPosition = -1;
        }

        public int SelectedPosition { get; set; }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var inflator = LayoutInflater.From(context);
            var view = inflator.Inflate(Resource.Layout.side_panel_navigation_list_item, null);

            var name = view.FindViewById<TextView>(Resource.Id.navigation_name);
            var icon = view.FindViewById<ImageView>(Resource.Id.navigation_icon);

            var item = GetItem(position);
            name.Text = context.GetString(item.NameId);
            icon.SetImageResource(item.IconId);

            if (position == SelectedPosition)
            {
                //view.Alpha = 1;
                view.SetBackgroundColor(context.Resources.GetColor(Resource.Color.color_accent));
            }

            return view;
        }
    }

    public class NavigationItem<T>
    {
        public int IconId { get; private set; }
        public int NameId { get; private set; }
        public Action<T> Action { get; private set; }
        public int Count { get; set; }

        public NavigationItem(int iconId, int nameId, Action<T> action, int count)
        {
            IconId = iconId;
            NameId = nameId;
            Action = action;
            Count = count;
        }
    }
}