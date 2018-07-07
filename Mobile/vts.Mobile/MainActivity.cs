using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.Locations;
using Android.Nfc;
using Android.OS;
using Android.Support.V4.Content;
using Android.Support.V4.Content.Res;
using Android.Views;
using Android.Widget;
using Mobile.Common.Core;
using Mobile.Common.Util;
using vts.Core.Shared.Entities.Master;
using vts.Mobile;
using vts.Mobile.SidePanel;
using vts.Shared.Entities.Master;
using vts.Shared.Repository;
using Settings = Android.Provider.Settings;

namespace Agrimanagr.Mobile
{
    [Activity(Label = "MainActivity", WindowSoftInputMode = SoftInput.AdjustPan,
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
     
    public class MainActivity : FragmentHostActivity<User>
    {
        private readonly Criteria criteriaForLocationService = new Criteria
        {
            Accuracy = Accuracy.Fine
        };

        public override void Created(Bundle bundle)
        {
            base.Created(bundle);
            SetupNavigationFragment();
            int i = GetColor(Resource.Color.color_primary_dark);

          var colorid=       ContextCompat.GetColor(this, Resource.Color.color_primary_dark);
            Color color=new Color(colorid);

            if ((Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop))
            {
                /*
                 window.addFlags(WindowManager.LayoutParams.FLAG_DRAWS_SYSTEM_BAR_BACKGROUNDS);
                window.clearFlags(WindowManager.LayoutParams.FLAG_TRANSLUCENT_STATUS);
                    Win             */

                var window = this.Window;
                window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);
                window.ClearFlags(WindowManagerFlags.TranslucentStatus);
                Window.SetStatusBarColor(color);
            }
            
          
           var User= new User(Guid.NewGuid())
           {
               DateCreated = DateTime.Today,
               FirstName = "George",
               DateLastUpdated = DateTime.Today,
               Password = "iieiaiie",
               UserType = UserType.Client,
               Status = EntityStatus.Active,
               
               
           };

           /* var userrepo = Resolve<IUserRepository>();
            userrepo.Save(User);

            var savedusers = userrepo.GetAll();*/



        }
       

      

        private void SetupNavigationFragment()
        {
            SetupNavigationDrawer();
            var fragment = new SidePanelFragment();

            SupportFragmentManager
                .BeginTransaction()
                .Replace(Resource.Id.side_panel_fragment, fragment)
                .Commit();
        }

    

       

     

        private string GetDataStoredOnTag(NdefMessage message)
        {
            var result = "";
            var payload = message.GetRecords()[0].GetPayload();

            payload.ForEach(b => { result += (char) b; });

            return result;
        }


       

        protected override void FragmentAttached()
        {
            try
            {
                // ShowEnableGpsDialog();

            }
            catch (Exception exception)
            {
            }
        }

        

   
    }
}