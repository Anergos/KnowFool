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
using Android.Graphics;
using Android.Support.V7.Widget;
using HumbleFool_Project.Helper;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;


// For this recycler view, Access the Adapter in "Helper > ChapterListRecyclerViewAdapter" 
// For Variables related to Row, Access the File "Helper > ChapterDetailData"

namespace HumbleFool_Project
{
    [Activity(MainLauncher = false, Theme = "@style/Theme.AppCompat.Light.NoActionBar")]
    public class instructorChapterList : AppCompatActivity
    {
        public static List<System.String> chapterID = new List<System.String>(); // should contain the userIDs of the Instructors.
        Android.Support.V7.Widget.Toolbar chapterListToolbar;
        ImageView chapterListBanner;
        TextView chapterListLabel1, chapterListLabel2;
        EditText chapterListInstructorName;
        RecyclerView chapterListRecyclerView;
        private RecyclerView.LayoutManager layoutManager;
        public bool courseExist;
        String courseCode;
        private List<ChapterDetailData> listData = new List<ChapterDetailData>();  //Populate this list to add chapter number and name

        private chapterRecycleViewAdapter recycleAdapter2;
        public static string inst_name;
        public string language_id, language_image, language_name;
        //ProgressDialog
        Android.App.ProgressDialog progress;


        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.instructorChapterListToolBar);
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetTitle("Please Wait");
            progress.SetMessage("Getting Course Details");
            progress.SetCancelable(false);
            progress.Show();
            string inst_id = Intent.GetStringExtra("instructor_id") ?? "Data not available";
            language_id = Intent.GetStringExtra("language_id") ?? "Data not available";
            inst_name = Intent.GetStringExtra("instructor_name") ?? "Name Not Available";

            try
            {
                Console.WriteLine("instructor_name : " + inst_name);
                await Task.Run(() => ChapterList(inst_id, language_id));
                await Task.Run(() => CourseInfoFetcher(language_id));
                progress.Hide();
                //InitData();
                FindViews();
                chapterListInstructorName.Text = inst_name;
            }
            catch (Exception ChapterListError)
            {
                Console.WriteLine("ChapterListError : " + ChapterListError);
                throw;
            }
        }

        //This method is USELESS... 'cause I'm populating the list in the fetching method itself :D
        //private void InitData()
        //{
        //    listData.Add(new ChapterDetailData()
        //    {
        //        instructorChapterNumber = "Chapter 01",                    //For Chapter Number
        //        instructorChapterName = "Introduction to Python 03"        //For Chapter Name

        //    });
        //}

        private void FindViews()
        {
            //ToolBar Layout
            chapterListToolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.instructorChapterListToolbar);
            chapterListBanner = FindViewById<ImageView>(Resource.Id.instructorChapterListBanner);
            chapterListInstructorName = FindViewById<EditText>(Resource.Id.chapterListInstructorName);
            chapterListLabel1 = FindViewById<TextView>(Resource.Id.chapterListLabel1);
            chapterListLabel2 = FindViewById<TextView>(Resource.Id.chapterListLabel2);


            //RecyclerView Layout
            chapterListRecyclerView = FindViewById<RecyclerView>(Resource.Id.chapterListInstructorList);
            chapterListRecyclerView.HasFixedSize = true;
            layoutManager = new LinearLayoutManager(this);
            chapterListRecyclerView.SetLayoutManager(layoutManager);
            recycleAdapter2 = new chapterRecycleViewAdapter(listData, this);
            chapterListRecyclerView.SetAdapter(recycleAdapter2);

            //Fonts
            Typeface tf = Typeface.CreateFromAsset(Assets, "CerebriSans-Regular.otf");
            chapterListLabel1.SetTypeface(tf, TypefaceStyle.Bold);
            chapterListLabel2.SetTypeface(tf, TypefaceStyle.Bold);
            chapterListInstructorName.SetTypeface(tf, TypefaceStyle.Normal);


            //Set Toolbar Title and Banner depending upon the Click of Course
            chapterListToolbar.Title = "Default ToolBar";
            //var imageBitmap = GetImageBitmapFromUrl(language_image);
            //chapterListBanner.SetImageBitmap(imageBitmap);
            chapterListToolbar.Title = language_name;
            //chapterListBanner;
            updateChapterListBanner(language_id);


            //If adding anything in this block, add after these 3 lines
            SetSupportActionBar(chapterListToolbar);
            if (SupportActionBar != null)
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        private void updateChapterListBanner(string language_id)
        {
            switch (language_id)
            {
                case "2":
                    chapterListBanner.SetImageResource(Resource.Drawable.python);
                    break;
                case "3":
                    chapterListBanner.SetImageResource(Resource.Drawable.ue4);
                    break;
                case "4":
                    chapterListBanner.SetImageResource(Resource.Drawable.clang);
                    break;
                case "5":
                    chapterListBanner.SetImageResource(Resource.Drawable.ml);
                    break;
                case "6":
                    chapterListBanner.SetImageResource(Resource.Drawable.php);
                    break;
                case "7":
                    chapterListBanner.SetImageResource(Resource.Drawable.windows);
                    break;
                default:
                    chapterListBanner.SetImageResource(Resource.Drawable.banner);
                    break;
            }
        }

        //private Bitmap GetImageBitmapFromUrl(string url)
        //{
        //    Bitmap imageBitmap = null;

        //    using (var webClient = new WebClient())
        //    {
        //        var imageBytes = webClient.DownloadData(url);
        //        if (imageBytes != null && imageBytes.Length > 0)
        //        {
        //            imageBitmap = BitmapFactory.DecodeByteArray(imageBytes, 0, imageBytes.Length);
        //        }
        //    }

        //    return imageBitmap;
        //}

        private async Task ChapterList(string instructor_id, string lang_id)
        {
            Console.WriteLine("Inside function : " + instructor_id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("instruct_id", instructor_id),
                new KeyValuePair<string, string>("lang_id", lang_id)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/course_information/course_by_instructor.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine("Result Content : " + resultContent);

                dynamic dynJson = JsonConvert.DeserializeObject(resultContent);
                listData.Clear();
                foreach (var item in dynJson)
                {
                    //Console.WriteLine("{0} {1} {2}\n", item.language, item.user, item.userName);
                    //listData.Add(Convert.ToString(item.userName));
                    //listData.Add(Convert.ToString(item.user));
                    listData.Add(new ChapterDetailData()
                    {
                        instructorChapterNumber = Convert.ToString(item.chapter_count),                    //For Chapter Number
                        instructorChapterName = Convert.ToString(item.chapter_title)        //For Chapter Name

                    });
                    chapterID.Add(Convert.ToString(item.chapterId));
                }

            }

        }

        private async Task CourseInfoFetcher(string lang_id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("lang_id", lang_id)
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
                        //this.language_image = obj2.language_image;
                    }
                }
                catch (System.Exception)
                {
                    throw;
                    //Toast.MakeText(this, loginException.ToString(), ToastLength.Long).Show(); //Showing Bad Connection Error
                }

            }

        }
    }
}