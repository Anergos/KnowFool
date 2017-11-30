using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using System;
using Android.Graphics;
using Android.Content;
using Android.Support.V7.App;
using Android.Support.Design.Widget;

namespace HumbleFool_Project
{
    [Activity(MainLauncher = true)]
    public class firstScreen : AppCompatActivity
    {
        ImageButton proceed;
        private FloatingActionButton fab;

        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.firstScreenLayout);
            FindViews();
            ClickEvents();
        }

        private void FindViews()
        {
            proceed = FindViewById<ImageButton>(Resource.Id.firstScreen_proceed);
            //fab = FindViewById<FloatingActionButton>(Resource.Id.fab_addNewChapter);

        }

        private void ClickEvents()
        {
            proceed.Click += StudentSide_Click;
            //fab.Click += Fab_Click;
            //fab.LongClick += Fab_LongClick;
        }

        private void StudentSide_Click(object sender, EventArgs e)
        {
            var intentLoginScreen = new Intent(this, typeof(loginScreen));
            StartActivity(intentLoginScreen);
        }
    }
}

