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
using Android.Support.V7.Widget;
using Android.Graphics;
using Android;

namespace HumbleFool_Project.Helper
{
    class RecycleViewHolder: RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
    {
        public TextView chapterName { get; set; }
        public LinearLayout defaultRow { get; set; }
        public LinearLayout noInstructor { get; set; }
        private IItemClickListener itemClickListener;

        public RecycleViewHolder(View itemView):base(itemView)
        {
            chapterName = itemView.FindViewById<TextView>(Resource.Id.courseChapter);
            defaultRow = itemView.FindViewById<LinearLayout>(Resource.Id.instructorDefaultRow);
            noInstructor = itemView.FindViewById<LinearLayout>(Resource.Id.instructorNoInstructorRow);
            itemView.SetOnClickListener(this);
            itemView.SetOnLongClickListener(this);
        }

        public void SetItemClickListener(IItemClickListener itemClickListener)
        {
            this.itemClickListener = itemClickListener;
        }

        public void OnClick(View v)
        {
            try
            {
                itemClickListener.OnClick(v, AdapterPosition, false);
            }
            catch(Exception e)
            {
                Toast.MakeText(ItemView.Context, "No Instructor Available. ", ToastLength.Long).Show();
            }
        }

        public bool OnLongClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, true);
            return true;
        }
    }
    class RecycleViewAdapter : RecyclerView.Adapter, IItemClickListener
    {
        private List<Data> listData = new List<Data>();
        private Context context;
        RecycleViewHolder viewHolder;

        public RecycleViewAdapter()
        {
        }

        public RecycleViewAdapter(List<Data> listData, Context context)
        {
            this.listData = listData;
            this.context = context;
        }

        public override int ItemCount
        {
            get
            {
                return listData.Count;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            viewHolder = holder as RecycleViewHolder;
            viewHolder.chapterName.Text = listData[position].courseDetailsListChapters;
            var length = viewHolder.chapterName.Text.ToString();
            if (length.Length == 0)
            {
                viewHolder.noInstructor.Visibility = ViewStates.Visible;
                viewHolder.defaultRow.Visibility = ViewStates.Gone;
            }
            else
            {
                viewHolder.noInstructor.Visibility = ViewStates.Gone;
                viewHolder.defaultRow.Visibility = ViewStates.Visible;
                viewHolder.SetItemClickListener(this);
            }
        }

        public void OnClick(View itemView, int position, bool isLongClick)
        {
            if (isLongClick) //For Long Click Events, not of our use , but meh!
            {
                Toast.MakeText(context, "Long Click on Item : " + position, ToastLength.Short).Show();
            }
            else // Normal Clicks, useful!
            {
                //Normal Intent
                var intent = new Intent(context, typeof(instructorChapterList));

                //ViewHolder is your recyclerView Holder and 'chapterName' is used to provide a explicit context for activity. 
                //you used it in hackocracy in https://goo.gl/DH3FsE , at this exact file somewhere around Line 190. 
                //Check it for reference.

                Console.WriteLine("RecyclerView : " + courseDetails.userID[position]);
                intent.PutExtra("instructor_id", courseDetails.userID[position]);
                intent.PutExtra("instructor_name", courseDetails.listSample[position]);
                intent.PutExtra("language_id", courseDetails.courseCode);
                viewHolder.chapterName.Context.StartActivity(intent);

                //Rest of property for recyclerview , I no understand so pls don't ask me! CYKA BLYAT!!!
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.courseDetailsInstructorsListLayout, parent, false);
            return new RecycleViewHolder(itemView);
        }
    }
}