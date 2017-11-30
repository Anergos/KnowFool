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
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using Android.Support.V7.Widget;
using Android.Graphics;
using System.Threading.Tasks;

namespace HumbleFool_Project
{
    [Activity(Label =" Featured Courses" , MainLauncher =false)]
    public class mainScreen : AppCompatActivity
    {
        public string user_id, user_role;

        public CoordinatorLayout rootLayout;
        Button pythonShare, pythonView;
        Button unrealShare, unrealView;
        Button clangShare, clangView;
        Button phpShare, phpView;
        Button mlShare, mlView;
        Button windowsShare, windowsView;
        ImageView imagePython, imageUnreal, imageClang, imagePHP, imageML, imageWindows;
        RelativeLayout listAll2;
        TextView listAll3, courseInfo;
        ImageButton listAll;
        String courseClick;
        private FloatingActionButton fab_addCourse;

        //ProgressDialog
        ProgressDialog progress;

        public async override void OnBackPressed()
        {
            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetTitle("Logging Out");
            progress.SetMessage("Deleting Cache...");
            progress.SetCancelable(false);
            progress.Show();
            await Task.Delay(1500);
            progress.Hide();
            base.OnBackPressed();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.mainScreenLayout);
            user_id = Intent.GetStringExtra("user_id") ?? "Data not available";
            user_role = Intent.GetStringExtra("user_role") ?? "Data not available";

            Console.WriteLine("user_role in mainScreen : " + user_role);

            FindViews();
            //The learner/student shouldn't see the "Add a new course button".
            if (user_role == "1") //Learner
            {
                Console.WriteLine("user_role in code : " + user_role);
                fab_addCourse.Visibility = ViewStates.Gone;
            }
            ClickEvents();

        }

        private void ClickEvents()
        {
            listAll.Click += UnderDevelopmentSnackBar;
            listAll2.Click += UnderDevelopmentSnackBar;
            listAll3.Click += UnderDevelopmentSnackBar;

            pythonShare.Click += UnderDevelopmentSnackBar;
            pythonView.Click += PythonView_Click;
            imagePython.Click += PythonView_Click;

            unrealShare.Click += UnderDevelopmentSnackBar;
            unrealView.Click += UnrealView_Click;
            imageUnreal.Click += UnrealView_Click;

            clangShare.Click += UnderDevelopmentSnackBar;
            clangView.Click += ClangView_Click;
            imageClang.Click += ClangView_Click;

            mlShare.Click += UnderDevelopmentSnackBar;
            mlView.Click += MlView_Click;
            imageML.Click += MlView_Click;

            phpShare.Click += UnderDevelopmentSnackBar;
            phpView.Click += PhpView_Click;
            imagePHP.Click += PhpView_Click;

            windowsShare.Click += UnderDevelopmentSnackBar;
            windowsView.Click += WindowsView_Click;
            imageWindows.Click += WindowsView_Click;

            fab_addCourse.Click += Fab_addCourse_Click;
            fab_addCourse.LongClick += Fab_addCourse_LongClick;

        }

        private void Fab_addCourse_LongClick(object sender, View.LongClickEventArgs e)
        {
            Snackbar.Make(rootLayout, "Add a new Tutorial (For Instructors Only. ", Snackbar.LengthLong).Show();
        }

        private void Fab_addCourse_Click(object sender, EventArgs e)
        {
            var intentAddNewCourse = new Intent(this, typeof(addNewChapter));
            intentAddNewCourse.PutExtra("user_id", user_id);
            StartActivity(intentAddNewCourse);
        }

        private void WindowsView_Click(object sender, EventArgs e)
        {
            courseClick = "7";
            var intentWindows = new Intent(this, typeof(courseDetails));
            //intentWindows.PutExtra("courseCode", this.courseClick);
            intentWindows.PutExtra("courseCode", this.courseClick);
            StartActivity(intentWindows);
        }

        private void PhpView_Click(object sender, EventArgs e)
        {
            courseClick = "6";
            var intentPHP = new Intent(this, typeof(courseDetails));
            intentPHP.PutExtra("courseCode", this.courseClick);
            StartActivity(intentPHP);
        }

        private void MlView_Click(object sender, EventArgs e)
        {
            courseClick = "5";
            var intentML = new Intent(this, typeof(courseDetails));
            intentML.PutExtra("courseCode", this.courseClick);
            StartActivity(intentML);
        }

        private void ClangView_Click(object sender, EventArgs e)
        {
            courseClick = "4";
            var intentClang = new Intent(this, typeof(courseDetails));
            intentClang.PutExtra("courseCode", this.courseClick);
            StartActivity(intentClang);
        }

        private void UnrealView_Click(object sender, EventArgs e)
        {
            courseClick = "3";
            var intentUnreal = new Intent(this, typeof(courseDetails));
            intentUnreal.PutExtra("courseCode", this.courseClick);
            StartActivity(intentUnreal);
        }

        private void PythonView_Click(object sender, EventArgs e)
        {
            //courseClick = "Python";
            courseClick = "2";
            var intentPython = new Intent(this, typeof(courseDetails));
            intentPython.PutExtra("courseCode", "2");
            StartActivity(intentPython);
        }

        private void UnderDevelopmentSnackBar(object sender, EventArgs e)
        {
            Snackbar.Make(rootLayout, "This feature is under Development.", Snackbar.LengthLong).Show();
        }

        private void FindViews()
        {
            pythonShare = FindViewById<Button>(Resource.Id.pythonShare);
            pythonView = FindViewById<Button>(Resource.Id.pythonView);
            imagePython = FindViewById<ImageView>(Resource.Id.imagePython);

            unrealShare = FindViewById<Button>(Resource.Id.unrealShare);
            unrealView = FindViewById<Button>(Resource.Id.unrealView);
            imageUnreal = FindViewById<ImageView>(Resource.Id.imageUnreal);

            clangShare = FindViewById<Button>(Resource.Id.clangShare);
            clangView = FindViewById<Button>(Resource.Id.clangView);
            imageClang = FindViewById<ImageView>(Resource.Id.imageClang);

            phpShare = FindViewById<Button>(Resource.Id.phpShare);
            phpView = FindViewById<Button>(Resource.Id.phpView);
            imagePHP = FindViewById<ImageView>(Resource.Id.imagePHP);

            mlShare = FindViewById<Button>(Resource.Id.mlShare);
            mlView = FindViewById<Button>(Resource.Id.mlView);
            imageML = FindViewById<ImageView>(Resource.Id.imageML);

            windowsShare = FindViewById<Button>(Resource.Id.windowsShare);
            windowsView = FindViewById<Button>(Resource.Id.windowsView);
            imageWindows = FindViewById<ImageView>(Resource.Id.imageWindows);

            rootLayout = FindViewById<CoordinatorLayout>(Resource.Id.rootLayout);

            courseInfo = FindViewById<TextView>(Resource.Id.textView2);

            listAll = FindViewById<ImageButton>(Resource.Id.listallCourses);
            listAll2 = FindViewById<RelativeLayout>(Resource.Id.listallCourses2);
            listAll3 = FindViewById<TextView>(Resource.Id.listallCourses3);

            Typeface tf = Typeface.CreateFromAsset(Assets, "CerebriSans-Regular.otf");

            fab_addCourse = FindViewById<FloatingActionButton>(Resource.Id.fab_addNewChapter);

            pythonShare.SetTypeface(tf, TypefaceStyle.Bold);
            pythonView.SetTypeface(tf, TypefaceStyle.Bold);
            unrealShare.SetTypeface(tf, TypefaceStyle.Bold);
            unrealView.SetTypeface(tf, TypefaceStyle.Bold);
            mlShare.SetTypeface(tf, TypefaceStyle.Bold);
            mlView.SetTypeface(tf, TypefaceStyle.Bold);
            clangShare.SetTypeface(tf, TypefaceStyle.Bold);
            clangView.SetTypeface(tf, TypefaceStyle.Bold);
            phpShare.SetTypeface(tf, TypefaceStyle.Bold);
            phpView.SetTypeface(tf, TypefaceStyle.Bold);
            windowsShare.SetTypeface(tf, TypefaceStyle.Bold);
            windowsView.SetTypeface(tf, TypefaceStyle.Bold);
            courseInfo.SetTypeface(tf, TypefaceStyle.Bold);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Snackbar.Make(rootLayout, "This is a SnackBar", Snackbar.LengthLong).SetAction("Button is here", v => Toast.MakeText(this, "Button was clicked", ToastLength.Short).Show()).Show();
        }
    }
}