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
using Mobile.core.Dependencies;
using Mobile.Common.Core;
using vts.Core.Shared.Entities.Master;

namespace vts.Mobile
{
    [Application(Label = "VtsMobile", Theme = "@style/AppTheme", LargeHeap = true)]
    public  class VtsApplication :BaseApplication<User>
    {
        private readonly DependencyContainer container;

        public VtsApplication(IntPtr javaReference, JniHandleOwnership transfer)
            : base(typeof(Resource.Layout), typeof(Resource.Menu), javaReference, transfer)
        {
            container = BuildContainer();
        }
     
        protected override T ResolveDependency<T>()
      {
            return container.Resolve<T>();
        }

      public override bool Initialised()
      {
          //throw new NotImplementedException();
          return User!=null;
      }

      public override void InitialiseFor(User user)
      {
          User = user;
      }

      public override void Logout()
      {
          throw new NotImplementedException();
      }

      public override void OnUncaughtException(object sender, RaiseThrowableEventArgs e)
      {
          throw new NotImplementedException();
      }

        private DependencyContainer BuildContainer()
        {
            return new DependencyContainerBuilder()
                .AddModule(new ApplicationModule(this))
                .Build();
        }
    }
}