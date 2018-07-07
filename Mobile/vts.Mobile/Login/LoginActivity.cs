using System;
using Android.App;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.Interop;
using Mobile.Common.Core;
using Mobile.Common.Storage;
using Mobile.Common.Util;
using vts.Core.Shared.Entities.Master;
using vts.Mobile;

namespace Agrimanagr.Mobile
{
    // using Distributr.Core.Domain.Master.UserEntities;

    [Activity(Label = "VTS", MainLauncher = true, Icon = "@drawable/icon",
        Theme = "@style/Theme.AppCompat.Light.NoActionBar.FullScreen",
        LaunchMode = LaunchMode.SingleInstance,
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)
    ]
    public class LoginActivity : BaseActivity<User>
    {
        private const int ProgressLimit = 10000;
        private Button loginButton;
        /*   private Database database;
        private LoginWorkflow loginWorkFlow;*/
        private string loginButtonText;
        private ClipDrawable progress;

        public override void Created(Bundle bundle)
        {
            // loginWorkFlow = Resolve<LoginWorkflow>();
            SetupLoginButton();

            // database = Resolve<IDatabase>() as Database;

            //  var uncaughtError = database.Table<UncaughtExceptionLogEntry>().FirstOrDefault();

            /*  if (uncaughtError != null)
		    {
		        ShowCrashReport(uncaughtError);
		    }*/
        }

     /*   private void ShowCrashReport(UncaughtExceptionLogEntry uncaughtError)
        {
            var dialog = new CrashReportDialog(this);
            dialog.OnClose += delegate { Resolve<IDatabase>().DeleteAll(typeof (UncaughtExceptionLogEntry)); };
            dialog.Show(uncaughtError);
        }*/

        private void SetupLoginButton()
        {
            loginButton = FindViewById<Button>(Resource.Id.login);
            loginButtonText = loginButton.Text;

            var layer = (LayerDrawable) loginButton.Background;
            progress = (ClipDrawable) layer.GetDrawable(1);
        }

        [Export("Login")]
        public async void Login(View button)
        {
            loginButton.Enabled = false;

            var username = FindViewById<EditText>(Resource.Id.username).Text;
            var password = FindViewById<EditText>(Resource.Id.password).Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ShowErrorDialog("You must enter a username and password", null);
                return;
            }

            loginButton.Text = "Logging In..";
          //  var result = await loginWorkFlow.Login(username.Trim(), password.Trim());

            try
            {
             //   ProcessLoginResult(result);

                //Setup a dummy User


                var user = DefaultUser();
                 App.InitialiseFor(user);
                StartActivity(typeof(MainActivity));

            }
            catch (Exception exception)
            {
                ShowErrorDialog("Error! Please check your credentials again ", null);
            }
        }

        private User DefaultUser()
        {
            return new User(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709"),DateTime.Today, DateTime.Today, EntityStatus.Active
                )
            {
                Surname = "guru",
                Password = "12345678",
                 UserType = UserType.Client
            };


        }

     /*   private void ProcessLoginResult(Result<MobileUser> result)
        {
            if (result.WasSuccessful())
            {
                var user = result.Value;
                if (user.IsNewUser)
                {
                    // No local user data so we need to hold the user at the login screen until
                    // we download it - the app is pretty useless without data so no 
                    // point in progressing just yet. 
                    App.Register(this);
                    App.InitialiseFor(user);
                }
                else
                {
                    // We've already got a user in the local DB which would also include
                    // master data so we can open the app
                    App.InitialiseFor(user);
                    ResetLoginButton();
                    StartActivity(typeof (MainActivity));
                }
            }
            else
            {
                ShowErrorDialog(result.Message, result.Exception);
            }
        }*/


        [Export("Settings")]
        public void Settings(View button)
        {
            //StartActivity(typeof (LoginSettingsActivity));
            OverridePendingTransition(Resource.Animation.slide_in, Resource.Animation.slide_out);
        }

        protected override void Resumed()
        {
            if (App.Initialised())
            {
                StartActivity(typeof (MainActivity));
            }
        }

        private void ShowErrorDialog(string message, Exception exception)
        {
            ResetLoginButton();
            Dialog alert = new ErrorAlertBuilder(this).Build(message, exception,
                Resource.Style.AppTheme_AlertDialog);
            alert.Show();
        }

      
        private void ResetLoginButton()
        {
            loginButton.Enabled = true;
            loginButton.Text = loginButtonText;
            progress.SetLevel(0);
        }
    }
}