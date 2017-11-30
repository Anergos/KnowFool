using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.Support.V7.Widget;
using System;
using Android.Graphics;

namespace HumbleFool_Project.Helper
{
    public class ChapterListRecyclerViewHolder : RecyclerView.ViewHolder, View.IOnClickListener, View.IOnLongClickListener
    {

        //Variables which corresponds to Custom Row AXML defined by name "ChapterListLayout"
        public TextView instructorChapterName { get; set; }
        public TextView instructorChapterNumber { get; set; }

        private IItemClickListener itemClickListener;       //Click Listener

        // Binds Variables defined above with UI Elements.
        public ChapterListRecyclerViewHolder(View itemView2) : base(itemView2)
        {
            instructorChapterName = itemView2.FindViewById<TextView>(Resource.Id.instructorChapterName);
            instructorChapterNumber = itemView2.FindViewById<TextView>(Resource.Id.instructorChapterNumber);

            itemView2.SetOnClickListener(this);
            itemView2.SetOnLongClickListener(this);
        }


        // Click Events but Overriden Implementation given below
        public void SetItemClickListener(IItemClickListener itemClickListener)
        {
            this.itemClickListener = itemClickListener;
        }

        public void OnClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, false);
        }

        public bool OnLongClick(View v)
        {
            itemClickListener.OnClick(v, AdapterPosition, true);
            return true;
        }
    }

    // List that populates the RecyclerView by using given in List in "instructorChapterList.cs"
    class chapterRecycleViewAdapter : RecyclerView.Adapter, IItemClickListener
    {
        private List<ChapterDetailData> instructorChapterListData = new List<ChapterDetailData>();
        private Context context;

        ChapterListRecyclerViewHolder viewHolder;

        public chapterRecycleViewAdapter()
        {

        }

        public chapterRecycleViewAdapter(List<ChapterDetailData> listData, Context context)
        {
            this.instructorChapterListData = listData;
            this.context = context;
        }


        // Returns Count of Total Items.
        public override int ItemCount
        {
            get
            {
                return instructorChapterListData.Count;
            }
        }

        //  Assigns Data to UI Elements and Populate them
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            viewHolder = holder as ChapterListRecyclerViewHolder;
            viewHolder.instructorChapterName.Text = instructorChapterListData[position].instructorChapterName;
            viewHolder.instructorChapterNumber.Text = instructorChapterListData[position].instructorChapterNumber;
            Toast.MakeText(context, "Total Items: " + instructorChapterListData.Count, ToastLength.Short).Show();
            viewHolder.SetItemClickListener(this);
        }


        //  Click Events.
        public void OnClick(View itemView, int position, bool isLongClick)
        {
            if (isLongClick) //For Long Click Events, not of our use , but meh!
            {
                Toast.MakeText(context, "Long Click on Item : " + position, ToastLength.Short).Show();
            }
            else // Normal Clicks, useful!
            {
                // "position" will identify the location of populated items, list starts with 'O' 
                //Toast.MakeText(context, "Click on Item : " + position, ToastLength.Short).Show();
                //use above mentioned 'context' instead of 'this' to provide a context.

                //Normal Intent
                var intent = new Intent(context, typeof(ChapterDetails));
                intent.PutExtra("chapter_id", instructorChapterList.chapterID[position]);
                //intent.PutExtra("instructor_name", instructorChapterList.inst_name);

                //ViewHolder is your recyclerView Holder and 'chapterName' is used to provide a explicit context for activity. 
                //you used it in hackocracy in https://goo.gl/DH3FsE , at this exact file somewhere around Line 190. 
                //Check it for reference.
                viewHolder.instructorChapterName.Context.StartActivity(intent);

                //Toast.MakeText(context, "Click Working, Do a Long Click to do Long Click", ToastLength.Short).Show();

                //Rest of property for recyclerview , I no understand so pls don't ask me! CYKA BLYAT!!!
            }
        }

        // Returns the Value.
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            LayoutInflater inflater = LayoutInflater.From(parent.Context);
            View itemView = inflater.Inflate(Resource.Layout.chapterListLayout, parent, false);
            return new ChapterListRecyclerViewHolder(itemView);
        }
    }
}