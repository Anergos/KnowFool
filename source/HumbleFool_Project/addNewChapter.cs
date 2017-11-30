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
using Android.Graphics;
using System.Threading.Tasks;
using System.Net.Http;

namespace HumbleFool_Project
{
    [Activity]
    public class addNewChapter : AppCompatActivity
    {
        public bool chapterAddition;
        public string selectedLanguage, user_id, errorMessage;
        public Spinner selectCourse;
        public String languageSelected;
        CoordinatorLayout rootView;
        TextView toolbar, label1, label2, label3, label4;
        EditText chapterNumber, chapterTitle, chapterDescription, chapterContent, newCourseTitle;
        Button newCourse, newChapterSubmit, existingCourse;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.addNewCourseChapter);

            FindViews();
            ClickEvents();
            user_id = Intent.GetStringExtra("user_id") ?? "Data not available";

        }

        private int LanguageCode(string langName)
        {
            Console.WriteLine("langName : " + langName);
            if (langName == "Python")
            {
                return 2;
            }
            else if (langName == "Unreal Engine 4")
            {
                return 3;
            }
            else if (langName == "C#")
            {
                return 4;
            }
            else if (langName == "Machine Learning")
            {
                return 5;
            }
            else if (langName == "PHP 7")
            {
                return 6;
            }
            else if (langName == "Windows 10 UWP")
            {
                return 7;
            }
            else
            {
                return 1;
            }
        }

        private void ClickEvents()
        {

            newCourse.Click += NewCourse_Click;
            existingCourse.Click += ExistingCourse_Click;
            newCourse.LongClick += NewCourse_LongClick;
            newChapterSubmit.Click += NewChapterSubmit_Click;
            newChapterSubmit.LongClick += NewChapterSubmit_LongClick;


            //Spinner Click Event
            languageSelected = selectCourse.SelectedItem.ToString();
            selectCourse.ItemSelected += (s,e) =>
             {
                 if(languageSelected.Equals(selectCourse.SelectedItem.ToString()))
                 {
                     //Snackbar.Make(rootView, "Please select an appropriate course.", Snackbar.LengthLong).Show();
                 }
                 else
                 {
                     var item = selectCourse.SelectedItem.ToString();
                     Toast.MakeText(this, "Item position: " + e.Parent.GetItemIdAtPosition(e.Position).ToString(), ToastLength.Long).Show();
                     Snackbar.Make(rootView, "You have selected: "+item, Snackbar.LengthLong).Show();
                 }
             };
        }

        private void ExistingCourse_Click(object sender, EventArgs e)
        {
            selectCourse.Visibility = ViewStates.Visible;
            newCourse.Visibility = ViewStates.Visible;
            newCourseTitle.Visibility = ViewStates.Gone;
            existingCourse.Visibility = ViewStates.Gone;
        }

        private void NewChapterSubmit_LongClick(object sender, View.LongClickEventArgs e)
        {
            Snackbar.Make(rootView, "Lets You(Instructor) add a new Chapter in existing Course. ", Snackbar.LengthLong).Show();
        }

        private async void NewChapterSubmit_Click(object sender, EventArgs e)
        {
            //Snackbar.Make(rootView, "Chapter Submitted. ", Snackbar.LengthLong).Show();
            string courseCode = Convert.ToString(LanguageCode(selectCourse.SelectedItem.ToString()));
            await Task.Run(() => CourseAddition(courseCode, user_id, chapterTitle.Text, chapterDescription.Text, chapterNumber.Text, chapterContent.Text));
            if (chapterAddition)
            {
                Snackbar.Make(rootView, "This chapter was added successfully.", Snackbar.LengthLong).Show();
                await Task.Delay(2000);
                base.OnBackPressed();
            }
            else
            {
                Snackbar.Make(rootView, errorMessage, Snackbar.LengthLong).Show();
            }
        }

        private void NewCourse_LongClick(object sender, View.LongClickEventArgs e)
        {
            Snackbar.Make(rootView, "Lets You (Instructor) add a new Course. ", Snackbar.LengthLong).Show();
        }

        private async void NewCourse_Click(object sender, EventArgs e)
        {
            selectCourse.Visibility = ViewStates.Gone;
            newCourse.Visibility = ViewStates.Gone;
            newCourseTitle.Visibility = ViewStates.Visible;
            existingCourse.Visibility = ViewStates.Visible;
        }

        private void FindViews()
        {
            selectCourse = FindViewById<Spinner>(Resource.Id.spinnerSelectCourse);
            rootView = FindViewById<CoordinatorLayout>(Resource.Id.linearLayout12);
            toolbar = FindViewById<TextView>(Resource.Id.newChapterToolbar);
            label1 = FindViewById<TextView>(Resource.Id.newChapterLabel1);
            label2 = FindViewById<TextView>(Resource.Id.newChapterLabel2);
            label3 = FindViewById<TextView>(Resource.Id.newChapterLabel3);
            label4 = FindViewById<TextView>(Resource.Id.newChapterLabel4);
            chapterNumber = FindViewById<EditText>(Resource.Id.newChapterNumber);
            chapterTitle = FindViewById<EditText>(Resource.Id.newChapterTitle);
            chapterDescription = FindViewById<EditText>(Resource.Id.newChapterDescription);
            chapterContent = FindViewById<EditText>(Resource.Id.newChapterContent);
            newCourse = FindViewById<Button>(Resource.Id.newCourseButton);
            existingCourse = FindViewById<Button>(Resource.Id.newExistingCourse);
            newChapterSubmit = FindViewById<Button>(Resource.Id.newChapterSubmit);
            newCourseTitle = FindViewById<EditText>(Resource.Id.newChapterCourse);

            Typeface tf = Typeface.CreateFromAsset(Assets, "CerebriSans-Regular.otf");

            toolbar.SetTypeface(tf, TypefaceStyle.Bold);
            label1.SetTypeface(tf, TypefaceStyle.Bold);
            label2.SetTypeface(tf, TypefaceStyle.Bold);
            label3.SetTypeface(tf, TypefaceStyle.Bold);
            label4.SetTypeface(tf, TypefaceStyle.Bold);
            newCourse.SetTypeface(tf, TypefaceStyle.Bold);
            existingCourse.SetTypeface(tf, TypefaceStyle.Bold);
            newChapterSubmit.SetTypeface(tf, TypefaceStyle.Bold);
            chapterNumber.SetTypeface(tf, TypefaceStyle.Normal);
            chapterTitle.SetTypeface(tf, TypefaceStyle.Normal);
            chapterDescription.SetTypeface(tf, TypefaceStyle.Normal);
            chapterContent.SetTypeface(tf, TypefaceStyle.Normal);
            newCourseTitle.SetTypeface(tf, TypefaceStyle.Normal);
        }

        private async Task CourseAddition(string language_id, string user_id, string chapter_title, string chapter_brief_desctiption, string chapter_count, string chapter_content)
        {
            //Console.WriteLine("Email Id : " + user_name);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("language_id", language_id),
                new KeyValuePair<string, string>("user_id", user_id),
                new KeyValuePair<string, string>("chapter_title", chapter_title),
                new KeyValuePair<string, string>("chapter_brief_desctiption", chapter_brief_desctiption),
                new KeyValuePair<string, string>("chapter_count", chapter_count),
                new KeyValuePair<string, string>("chapter_content", chapter_content)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/course/course_chapter_adder.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                try
                {
                    dynamic obj2 = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
                    Console.WriteLine("OBJ2 : " + obj2);
                    Console.WriteLine("error_code : " + obj2.error_code);
                    if (obj2.error_code == "1")
                    {
                        Console.WriteLine(obj2.message);
                        this.chapterAddition = false;
                        this.errorMessage = obj2.message;
                    }
                    else
                    {
                        this.chapterAddition = true;
                    }
                }
                catch (Exception)
                {
                    throw;
                    //Toast.MakeText(this, loginException.ToString(), ToastLength.Long).Show(); //Showing Bad Connection Error
                }

            }

        }
    }
}