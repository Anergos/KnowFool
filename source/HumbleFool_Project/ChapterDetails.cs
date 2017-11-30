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
using Android.Graphics;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;

namespace HumbleFool_Project
{
    [Activity(Label = "ChapterDetails")]
    public class ChapterDetails : Activity
    {
        public static string briefDesc, chapterContent, chapterNumber, chapterName;
        TextView toolbarChapterNumber, toolbarChapterName, label1, label2, label3, label4;
        EditText instructorName, briefDescription, mainContent;

        //ProgressDialog
        Android.App.ProgressDialog progress;

        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ChapterDetailsLayout);
            string chapt_id = Intent.GetStringExtra("chapter_id") ?? "Data not available";
            //string instructor_name = Intent.GetStringExtra("instructor_name") ?? "Name Not Available";
            //instructorName.Text = instructor_name;
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetTitle("Please Wait");
            progress.SetMessage("Getting Course Details");
            progress.SetCancelable(false);
            progress.Show();

            await Task.Run(() => ChapterDetailsFetcher(chapt_id));

            progress.Hide();

            FindViews();
            instructorName.Text = Convert.ToString(instructorChapterList.inst_name);
            briefDescription.Text = briefDesc;
            mainContent.Text = chapterContent;
            toolbarChapterNumber.Text = chapterNumber;
            toolbarChapterName.Text = chapterName;
        }

        private void FindViews()
        {
            toolbarChapterNumber = FindViewById<TextView>(Resource.Id.chapterDetailToolbarNumber);
            toolbarChapterName = FindViewById<TextView>(Resource.Id.chapterDetailToolbarName);
            label1 = FindViewById<TextView>(Resource.Id.chapterDetailLabel1);
            label2 = FindViewById<TextView>(Resource.Id.chapterDetailLabel2);
            label3 = FindViewById<TextView>(Resource.Id.chapterDetailLabel3);
            label4 = FindViewById<TextView>(Resource.Id.chapterDetailToolbar1);
            instructorName = FindViewById<EditText>(Resource.Id.chapterDetailInstructorName);
            briefDescription = FindViewById<EditText>(Resource.Id.chapterDetailDescription);
            mainContent = FindViewById<EditText>(Resource.Id.chapterDetailContent);

            Typeface tf = Typeface.CreateFromAsset(Assets, "CerebriSans-Regular.otf");


            //Font Implementation
            //Login Fields
            toolbarChapterNumber.SetTypeface(tf, TypefaceStyle.Bold);
            toolbarChapterName.SetTypeface(tf, TypefaceStyle.Bold);
            label1.SetTypeface(tf, TypefaceStyle.Bold);
            label2.SetTypeface(tf, TypefaceStyle.Bold);
            label3.SetTypeface(tf, TypefaceStyle.Bold);
            label4.SetTypeface(tf, TypefaceStyle.Bold);
            instructorName.SetTypeface(tf, TypefaceStyle.Normal);
            briefDescription.SetTypeface(tf, TypefaceStyle.Normal);
            mainContent.SetTypeface(tf, TypefaceStyle.Normal);

        }

        private async Task ChapterDetailsFetcher(string chapt_id)
        {
            Console.WriteLine("Inside function : " + chapt_id);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("chapt_id", chapt_id)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/course_information/chapter_content.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                Console.WriteLine("Result Content For Chapter Detail : " + resultContent);

                dynamic dynJson = JsonConvert.DeserializeObject(resultContent);
                //listData.Clear();
                foreach (var item in dynJson)
                {
                    //Console.WriteLine("{0} {1} {2}\n", item.language, item.user, item.userName);
                    //listData.Add(Convert.ToString(item.userName));
                    //listData.Add(Convert.ToString(item.user));
                    //listData.Add(new ChapterDetailData()
                    //{
                    //    instructorChapterNumber = Convert.ToString(item.chapter_count),                    //For Chapter Number
                    //    instructorChapterName = Convert.ToString(item.chapter_title)        //For Chapter Name

                    //});
                    briefDesc = Convert.ToString(item.chapter_brief_description);
                    chapterContent = Convert.ToString(item.chapter_content);
                    chapterNumber = Convert.ToString(item.chapterId);
                    chapterName = Convert.ToString(item.chapter_title);
                }

            }

        }
    }
}