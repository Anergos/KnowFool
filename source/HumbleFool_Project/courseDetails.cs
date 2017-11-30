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
using Java.Lang;
using HumbleFool_Project.Helper;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net;
using Android.Graphics;
using Newtonsoft.Json;


// For this recycler view, Access the Adapter in "Helper > RecycleViewAdapter" 
// For Variables related to Row, Access the File "Helper > Data"

namespace HumbleFool_Project
{
    [Activity(MainLauncher = false, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class courseDetails : AppCompatActivity
    {
        public bool courseExist;
        public string language_name, language_description, language_usage, language_image;

        ImageView courseDetailBanner;
        EditText courseDetailBannerDescription, courseDetailBannerUsage;
        Android.Support.V7.Widget.Toolbar toolbar;
        RecyclerView recyclerView;
        TextView lables1, lables2, lables3;
        private RecyclerView.LayoutManager layoutManager;
        private List<Data> listData = new List<Data>();
        private RecycleViewAdapter recycleAdapter1;
        public static List<System.String> listSample = new List<System.String>(); // Contains the instructor's names.
        public static List<System.String> userID = new List<System.String>(); // should contain the userIDs of the Instructors.
        public static System.String courseCode;

        //ProgressDialog
        Android.App.ProgressDialog progress;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.courseDetailToolBar);

            courseCode = Intent.GetStringExtra("courseCode") ?? "Data not available";

            //Defines the settings for progress bar
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetTitle("Please Wait");
            progress.SetMessage("Getting Course Details");
            progress.SetCancelable(false);
            progress.Show();
            try
            {
                await Task.Run(() => CourseListFetcher(courseCode));
                
                await Task.Run(() => InstructorListFetcher(courseCode));
                progress.Hide();

                //listSample.Add("Getting Instructors");

                InitData();

                FindViews();
                //foreach (var item in listSample)
                //{
                //    Console.WriteLine("ListSample : " + item);
                //}
                //foreach (var item in userID)
                //{
                //    Console.WriteLine("UserIDs : " + item);
                //}
            }
            catch (System.Exception something)
            {
                progress.Hide();
                Console.WriteLine("Something : " + something);
                throw;
            }

        }

        // RecyclerView List Populate
        private void InitData()
        {
            foreach (var i in listSample)
            {
                listData.Add(new Data()
                {
                    courseDetailsListChapters = i
                });
            }
        }

        private Bitmap GetImageBitmapFromUrl(string url)
        {
            Bitmap imageBitmap = null;

            using (var webClient = new WebClient())
            {
                var imageBytes = webClient.DownloadData(url);
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
                }
            }

            return imageBitmap;
        }

        private void FindViews()
        {
            toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.courseDetailToolBar);
            courseDetailBanner = FindViewById<ImageView>(Resource.Id.courseDetailBanner);
            courseDetailBannerDescription = FindViewById<EditText>(Resource.Id.courseDetailDescription);
            courseDetailBannerUsage = FindViewById<EditText>(Resource.Id.courseDetailUsage);
            recyclerView = FindViewById<RecyclerView>(Resource.Id.courseDetailsListChapters);
            recyclerView.HasFixedSize = true;
            layoutManager = new LinearLayoutManager(this);
            recyclerView.SetLayoutManager(layoutManager);
            recycleAdapter1 = new RecycleViewAdapter(listData,this);
            recyclerView.SetAdapter(recycleAdapter1);

            lables1 = FindViewById<TextView>(Resource.Id.label1);
            lables2 = FindViewById<TextView>(Resource.Id.label2);
            lables3 = FindViewById<TextView>(Resource.Id.label3);
            Typeface tf = Typeface.CreateFromAsset(Assets, "CerebriSans-Regular.otf");
            lables1.SetTypeface(tf, TypefaceStyle.Bold);
            lables2.SetTypeface(tf, TypefaceStyle.Bold);
            lables3.SetTypeface(tf, TypefaceStyle.Bold);
            courseDetailBannerDescription.SetTypeface(tf, TypefaceStyle.Normal);
            courseDetailBannerUsage.SetTypeface(tf, TypefaceStyle.Normal);

            //Sets the Banner and Title of Course details
            var imageBitmap = GetImageBitmapFromUrl(language_image);
            courseDetailBanner.SetImageBitmap(imageBitmap);
            toolbar.Title = language_name;
            courseDetailBannerDescription.Text = language_description;
            courseDetailBannerUsage.Text = language_usage;

            //If adding anything in this block, add after these 3 lines
            SetSupportActionBar(toolbar);
            if (SupportActionBar != null)
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        private async Task CourseListFetcher(string courseId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("lang_id", courseId)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/course_information/course_information.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                
                try
                {
                    dynamic obj2 = Newtonsoft.Json.Linq.JObject.Parse(resultContent);

                    if (obj2.error_code == "1")
                    {
                        Console.WriteLine("Kinda Here");
                        Console.WriteLine(obj2.message);
                        this.courseExist = false;
                    }
                    else
                    {
                        this.courseExist = true;
                        this.language_name = obj2.language_name;
                        this.language_description = obj2.language_description;
                        this.language_usage = obj2.language_usage;
                        this.language_image = obj2.language_image;
                    }
                }
                catch (System.Exception)
                {
                    throw;
                    //Toast.MakeText(this, loginException.ToString(), ToastLength.Long).Show(); //Showing Bad Connection Error
                }

            }

        }

        private async Task InstructorListFetcher(string courseId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("lang_id", courseId)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/course_information/course_instructor.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                dynamic dynJson = JsonConvert.DeserializeObject(resultContent);
                listSample.Clear();
                foreach (var item in dynJson)
                {
                    //Console.WriteLine("{0} {1} {2}\n", item.language, item.user, item.userName);
                    listSample.Add(Convert.ToString(item.userName));
                    userID.Add(Convert.ToString(item.user));
                }

            }

        }
    }
}