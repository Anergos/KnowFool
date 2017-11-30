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
using System.IO;
using Android.Support.V7.App;
using Android.Support.Design.Widget;
using System.Threading;

namespace HumbleFool_Project
{
    [Activity]
    public class loginScreen : AppCompatActivity
    {
        // Our Global Variables
        public string message, user_name_string, user_email, user_full_name, login_result_data, signup_failure_message, login_failure_message, users_id_login;
        public int user_account_status, user_role;
        public bool user_name, user_sign_up, user_signup_email, user_login;// If user name is available then True, else False.
        Button loginButton, signupButton; //Use these for Login and Signup 
        EditText login_UserName, login_Password;
        EditText signup_FirstName, signup_LastName, signup_UserName, signup_Email, signup_Password;
        RadioGroup signup_Role;
        RadioButton signup_Student, signup_Instructor;
        CoordinatorLayout rootView;
        int user_Role; //   0 -> Instructor and 1 -> Student 
                       //  (result is obtained from selecting radio buttons and then clicking on "signup" in signup layout)


        //Do not use these (v) button as a SignupButton or LoginButton, this is just for initiating SignupLayout and LoginLayout
        Button signupLayoutStart;
        Button loginLayoutStart;
        LinearLayout loginButtonLayout, signupButtonLayout;
        LinearLayout loginLayout;
		ScrollView signupLayout;
        Intent intentMainScreen;

        //ProgressDialog
        ProgressDialog progress;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.loginLayout);


            FindViews();
            ClickEvents();

            //Defines the settings for progress bar
            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            //progress.SetMessage("Checking Your Credentials...");
            progress.SetCancelable(false);

            signup_UserName.TextChanged += async (sender, e) =>
            {
                if (signup_UserName.Text.Length > 4)
                {
                    //Toast.MakeText(this, "Editing...", ToastLength.Short).Show();
                    await Task.Run(() => UserNameChecker(signup_UserName.Text));
                }
            };
            signup_UserName.AfterTextChanged += async (sender, e) =>
            {
                if (signup_UserName.Text.Length > 4)
                {
                    //Toast.MakeText(this, "Editing...", ToastLength.Short).Show();
                    await Task.Run(() => UserNameChecker(signup_UserName.Text));
                }
            };

        }

        private void FindViews()
        {
            rootView = FindViewById<CoordinatorLayout>(Resource.Id.linearLayout12);
            //Login Fields
            loginButton = FindViewById<Button>(Resource.Id.loginScreen_LoginButton);
            signupLayoutStart = FindViewById<Button>(Resource.Id.SignupScreen_SignupLayout);
            signupButton = FindViewById<Button>(Resource.Id.SignupScreen_SignupButton);
            loginLayoutStart = FindViewById<Button>(Resource.Id.loginScreen_LoginLayout);
            login_UserName = FindViewById<EditText>(Resource.Id.loginScreen_Username);
            login_Password = FindViewById<EditText>(Resource.Id.loginScreen_Password);
            loginButtonLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout_LogInButton);
            loginLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout_Login);

            //Signup Fields
            signup_FirstName = FindViewById<EditText>(Resource.Id.SignupScreen_FirstName);
            signup_LastName = FindViewById<EditText>(Resource.Id.SignupScreen_LastName);
            signup_UserName = FindViewById<EditText>(Resource.Id.SignupScreen_UserName);
            signup_Email = FindViewById<EditText>(Resource.Id.SignupScreen_Email);
            signup_Password = FindViewById<EditText>(Resource.Id.SignupScreen_Password);
            signup_Role = FindViewById<RadioGroup>(Resource.Id.radioGroupRole);
            signup_Student = FindViewById<RadioButton>(Resource.Id.radioButton_Student);
            signup_Instructor = FindViewById<RadioButton>(Resource.Id.radioButton_Instructor);
            signupButtonLayout = FindViewById<LinearLayout>(Resource.Id.linearLayout_SignupButton);
            signupLayout = FindViewById<ScrollView>(Resource.Id.linearLayout_Signup);

            //Custom Font Declaration
            Typeface tf = Typeface.CreateFromAsset(Assets, "CerebriSans-Regular.otf");


            //Font Implementation
            //Login Fields
            loginButton.SetTypeface(tf, TypefaceStyle.Bold);
            signupLayoutStart.SetTypeface(tf, TypefaceStyle.Bold);
            login_UserName.SetTypeface(tf, TypefaceStyle.Normal);
            login_Password.SetTypeface(tf, TypefaceStyle.Normal);
            loginLayoutStart.SetTypeface(tf, TypefaceStyle.Bold);

            //Signup Fields
            signup_FirstName.SetTypeface(tf, TypefaceStyle.Normal);
            signup_LastName.SetTypeface(tf, TypefaceStyle.Normal);
            signup_UserName.SetTypeface(tf, TypefaceStyle.Normal);
            signup_Email.SetTypeface(tf, TypefaceStyle.Normal);
            signup_Password.SetTypeface(tf, TypefaceStyle.Normal);
            signup_Student.SetTypeface(tf, TypefaceStyle.Bold);
            signup_Instructor.SetTypeface(tf, TypefaceStyle.Bold);
            signupButton.SetTypeface(tf, TypefaceStyle.Bold);
        }

        private void ClickEvents()
        {
            loginButton.Click += LoginButton_Click;
            signupButton.Click += SignupButton_Click;
            signupLayoutStart.Click += SignupLayoutStart_Click;
            loginLayoutStart.Click += LoginLayoutStart_Click;
        }


        //Layout Initiate
        private void LoginLayoutStart_Click(object sender, EventArgs e)
        {
            loginLayout.Visibility = ViewStates.Visible;
            loginButtonLayout.Visibility = ViewStates.Visible;
            signupLayout.Visibility = ViewStates.Gone;
            signupButtonLayout.Visibility = ViewStates.Gone;
        }

        //Layout Initiate
        private void SignupLayoutStart_Click(object sender, EventArgs e)
        {
            loginLayout.Visibility = ViewStates.Gone;
            loginButtonLayout.Visibility = ViewStates.Gone;
            signupLayout.Visibility = ViewStates.Visible;
            signupButtonLayout.Visibility = ViewStates.Visible;
        }




        //Sign up Button  Functionality
        private async void SignupButton_Click(object sender, EventArgs e)
        {
            //Select the Radio Button and Click on Signup button to assign Value
            if (signup_Student.Checked)
            {
                user_Role = 1;
                //Toast.MakeText(this, "Student", ToastLength.Short).Show();
            }
            else if(signup_Instructor.Checked)
            {
                user_Role = 0;
                //Toast.MakeText(this, "Teacher", ToastLength.Short).Show();
            }

            //Toast.MakeText(this, "Sign up Button Clicked", ToastLength.Short).Show();
            if (string.IsNullOrEmpty(signup_FirstName.Text) || string.IsNullOrEmpty(signup_LastName.Text) || string.IsNullOrEmpty(signup_UserName.Text) || string.IsNullOrEmpty(signup_Password.Text) || string.IsNullOrEmpty(signup_Email.Text))
            {
                //Toast.MakeText(this, "Please make sure to fill all the fields.", ToastLength.Short).Show();
                Snackbar.Make(rootView, "Please make sure to fill all the fields.", Snackbar.LengthLong).Show();

            }
            else
            {
                if (signup_UserName.Text.Length <= 3)
                {
                    //Toast.MakeText(this, "Username should be at least 4 characters long.", ToastLength.Short).Show();
                    Snackbar.Make(rootView, "Username should be at least 4 characters long.", Snackbar.LengthLong).Show();
                }
                else if (signup_UserName.Text.Length > 20)
                {
                    //Toast.MakeText(this, "Username cannot be longer than 20 characters.", ToastLength.Short).Show();
                    Snackbar.Make(rootView, "Username cannot be longer than 20 characters.", Snackbar.LengthLong).Show();
                }
                else
                {

                    if (user_name)
                    {
                        if (Android.Util.Patterns.EmailAddress.Matcher(signup_Email.Text).Matches())
                        {
                            progress.SetMessage("Checking Your Credentials...");
                            progress.Show();
                            await Task.Run(() => UserSignUp(signup_UserName.Text, signup_Email.Text, signup_FirstName.Text + " " + signup_LastName.Text, signup_Password.Text, user_Role.ToString()));
                            progress.Hide();

                            if (user_sign_up)
                            {
                                if (user_signup_email)
                                {
                                    //Toast.MakeText(this, "Verification Email has been sent. Please verify it.", ToastLength.Short).Show();
                                    Snackbar.Make(rootView, "Verification Email has been sent. Please verify it.", Snackbar.LengthLong).Show();
                                    LoginLayoutStart_Click(sender,e);
                                    base.OnRestart();
                                }
                                else if (!user_signup_email)
                                {
                                    Snackbar.Make(rootView, "Could not send verification email. Please contact administrator.", Snackbar.LengthLong).Show();
                                    //Toast.MakeText(this, "Could not send verification email. Please contact administrator.", ToastLength.Short).Show();
                                }
                            }
                            else
                            {
                                //Toast.MakeText(this, signup_failure_message, ToastLength.Short).Show();
                                Snackbar.Make(rootView, signup_failure_message, Snackbar.LengthLong).Show();

                            }
                        }
                    }
                    else
                    {
                        //Toast.MakeText(this, "Username is already taken.", ToastLength.Short).Show();
                        Snackbar.Make(rootView, "Username is already taken.", Snackbar.LengthLong).Show();
                        progress.Hide();
                    }
                }
            }
            //intentMainScreen = new Intent(this, typeof(mainScreen));
            //StartActivity(intentMainScreen);
        }

        //Login Button Functionality
        private async void LoginButton_Click(object sender, EventArgs e)
        {
            progress.SetMessage("Checking Your Credentials...");
            progress.Show();

            if (string.IsNullOrEmpty(login_UserName.Text) || string.IsNullOrEmpty(login_Password.Text))
            {
                //Toast.MakeText(this, "Please make sure to fill all the fields.", ToastLength.Short).Show();
                Snackbar.Make(rootView, "Please make sure to fill all the fields.", Snackbar.LengthLong).Show();
                progress.Hide();
            }
            else
            {
                if (login_UserName.Text.Length <= 3 || login_UserName.Text.Length > 20)
                {
                    //Toast.MakeText(this, "Please make sure your username is correct.", ToastLength.Short).Show();
                    Snackbar.Make(rootView, "Please make sure your username is correct.", Snackbar.LengthLong).Show();
                    progress.Hide();
                }
                else
                {
                    await Task.Run(() => UserLogin(login_UserName.Text, login_Password.Text));
                    //progress.Hide();
                    
                    if (user_login)
                    {
                        if (user_account_status == 0)
                        {
                            if (user_role == 0) // Instructor
                            {
                                try
                                {
                                    SavetoSd();
                                }
                                catch (Exception NoWritePermission)
                                {
                                    Console.WriteLine("NoWritePermission : " + NoWritePermission);
                                    //throw;
                                }
                                progress.Hide();
                                intentMainScreen = new Intent(this, typeof(mainScreen));
                                intentMainScreen.PutExtra("user_id", users_id_login);
                                intentMainScreen.PutExtra("user_role", user_role.ToString()); //.ToString() because... we're passing strings. Otherwise, it won't go, 'cause INTEGER!
                                StartActivity(intentMainScreen);
                            }
                            if (user_role == 1) // Learner
                            {
                                try
                                {
                                    SavetoSd();
                                }
                                catch (Exception NoWritePermission)
                                {
                                    Console.WriteLine("NoWritePermission : " + NoWritePermission);
                                    //throw;
                                }
                                progress.Hide();
                                intentMainScreen = new Intent(this, typeof(mainScreen));
                                intentMainScreen.PutExtra("user_id", users_id_login);
                                intentMainScreen.PutExtra("user_role", user_role.ToString());
                                StartActivity(intentMainScreen);
                            }
                        }
                        else if (user_account_status == 1)
                        {
                            progress.Hide();
                            //Toast.MakeText(this, "You have not verified yourself. Please verify yourself first.", ToastLength.Short).Show();
                            Snackbar.Make(rootView, "You have not verified yourself.Please verify yourself first.", Snackbar.LengthLong).Show();
                        }
                        else if (user_account_status == 2)
                        {
                            progress.Hide();
                            //Toast.MakeText(this, "Your Account Has Been BLOCKED. Please contact administrator.", ToastLength.Short).Show();
                            Snackbar.Make(rootView, "Your Account Has Been BLOCKED. Please contact administrator.", Snackbar.LengthLong).Show();
                        }
                    }
                    else
                    {
                        progress.Hide();
                        Snackbar.Make(rootView, "Please Check Your Login Information Again!.", Snackbar.LengthLong).Show();
                    }
                }
            }
        }




        //Back Button Functionality
        public override void OnBackPressed()
        {
            if (signupLayout.Visibility == ViewStates.Visible)
            {
                loginLayout.Visibility = ViewStates.Visible;
                loginButtonLayout.Visibility = ViewStates.Visible;
                signupLayout.Visibility = ViewStates.Gone;
                signupButtonLayout.Visibility = ViewStates.Gone;
            }
            else if(loginLayout.Visibility == ViewStates.Visible)
            {
                base.OnBackPressed();
            }
        }

        private async Task UserNameChecker(string user_name)
        {
            Console.WriteLine("Email Id : " + user_name);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("user_name", user_name)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/user_account/user_name_check.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                try
                {
                    dynamic obj2 = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
                    Console.WriteLine("OBJ2 : " + obj2);
                    Console.WriteLine("error_code : " + obj2.error_code);
                    if (obj2.error_code == "1")
                    {
                        Console.WriteLine(obj2.message);
                        this.user_name = false;
                    }
                    else
                    {
                        this.user_name = true;
                    }
                }
                catch (Exception)
                {
                    throw;
                    //Toast.MakeText(this, loginException.ToString(), ToastLength.Long).Show(); //Showing Bad Connection Error
                }

            }

        }

        private async Task UserSignUp(string user_name, string user_email, string user_fullname, string user_password, string user_role)
        {
            Console.WriteLine("Email Id : " + user_name);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("user_name", user_name),
                new KeyValuePair<string, string>("user_email", user_email),
                new KeyValuePair<string, string>("user_fullname", user_fullname),
                new KeyValuePair<string, string>("user_password", user_password),
                new KeyValuePair<string, string>("user_role", user_role)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/user_account/sign_up.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();

                try
                {
                    dynamic obj2 = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
                    Console.WriteLine("OBJ2 : " + obj2);
                    Console.WriteLine("error_code : " + obj2.error_code);
                    if (obj2.error_code == "1")
                    {
                        Console.WriteLine(obj2.message);
                        this.user_sign_up = false;
                        this.signup_failure_message = obj2.message;
                    }
                    else
                    {
                        this.user_sign_up = true;

                        if (obj2.email_status ==  "1")
                        {
                            this.user_signup_email = true;
                        }
                        else
                        {
                            this.user_signup_email = false;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                    //Toast.MakeText(this, loginException.ToString(), ToastLength.Long).Show(); //Showing Bad Connection Error
                }

            }

        }

        private async Task UserLogin(string user_name, string user_password)
        {
            System.Console.WriteLine("Email Id : " + user_name);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://knowpool.tk");

                var content = new FormUrlEncodedContent(new[]
                {

                new KeyValuePair<string, string>("user_name", user_name),
                new KeyValuePair<string, string>("user_password", user_password)
            });
                var result = await client.PostAsync("/myAPI/HumbleFool/api/user_account/sign_in.php", content);
                string resultContent = await result.Content.ReadAsStringAsync();
                login_result_data = resultContent;

                try
                {
                    dynamic obj2 = Newtonsoft.Json.Linq.JObject.Parse(resultContent);
                    Console.WriteLine("OBJ2 : " + obj2);
                    Console.WriteLine("error_code : " + obj2.error_code);
                    if (obj2.error_code == "1")
                    {
                        Console.WriteLine(obj2.message);
                        this.user_login = false;
                        this.login_failure_message = obj2.message;
                    }
                    else if (obj2.error_code == "0")
                    {
                        this.user_login = true;
                        this.login_failure_message = "None";
                        this.user_name_string = obj2.user_name;
                        this.user_email = obj2.user_email;
                        this.user_full_name = obj2.user_full_name;
                        this.user_account_status = obj2.user_account_status;
                        this.user_role = obj2.user_role;
                        this.users_id_login = obj2.user_id;
                    }
                    else
                    {
                        this.login_failure_message = obj2.message;
                        this.user_login = false;
                    }
                }
                catch (Exception)
                {
                    throw;
                    //Toast.MakeText(this, loginException.ToString(), ToastLength.Long).Show(); //Showing Bad Connection Error
                }

            }

        }

        private void SavetoSd()
        {
            //var sdCardPath = Android.OS.Environment.ExternalStorageDirectory.Path;
            //var filePath = System.IO.Path.Combine(sdCardPath, "iootext.txt");
            //if (!System.IO.File.Exists(filePath))
            //{
            //    using (System.IO.StreamWriter write = new System.IO.StreamWriter(filePath, true))
            //    {
            //        write.Write(etSipServer.ToString());
            //    }
            //}
            var dirPath = this.FilesDir + "/KnowPool";
            var exists = Directory.Exists(dirPath);
            var filepath = dirPath + "/cache_data.txt";
            Console.WriteLine("Directory Path : " + filepath);
            if (!exists)
            {
                Directory.CreateDirectory(dirPath);
                if (!System.IO.File.Exists(filepath))
                {
                    var newfile = new Java.IO.File(dirPath, "cache_data.txt");
                    using (Java.IO.FileOutputStream outfile = new Java.IO.FileOutputStream(newfile))
                    {
                        //string line = "The very first line!";
                        outfile.Write(System.Text.Encoding.ASCII.GetBytes(login_result_data));
                        outfile.Flush();
                        outfile.Close();
                    }
                }
            }
            else
            {
                if (!System.IO.File.Exists(filepath))
                {
                    var newfile = new Java.IO.File(dirPath, "cache_data.txt");
                    using (Java.IO.FileOutputStream outfile = new Java.IO.FileOutputStream(newfile))
                    {
                        //string line = "The very first line!";
                        outfile.Write(System.Text.Encoding.ASCII.GetBytes(login_result_data));
                        outfile.Flush();
                        outfile.Close();
                    }
                }
                else
                {
                    using (StreamWriter objStreamWriter = new StreamWriter(filepath, true))
                    {
                        objStreamWriter.WriteLine(login_result_data);
                        objStreamWriter.Flush();
                        objStreamWriter.Close();
                    }
                }
            }
        }
    }
}