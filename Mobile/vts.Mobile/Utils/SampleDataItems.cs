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
using vts.Shared.Entities.Master;

namespace vts.Mobile.Utils
{
  public  class SampleDataItems
    {


      public static List<Race> SampleRaces()
      {
          var races = new List<Race>();

          return races;

      }

      public static List<PollingCentre> SamplePllingCentres()
      {
          var pollingcentres= new List<PollingCentre>();
          return pollingcentres;
      }

      public static List<PoliticalParty> SamplePoliticalParties()
      {
          var pollititcalParties= new List<PoliticalParty>();
          return pollititcalParties;
      }

      public static County SampleCounty()
      {
          return  new County();
      }

      public static Region SampleCpounty()
      {
          return  new Region();
      }

      public static Ward SampleWard()
      {
          return  new Ward();
      }


    }
}