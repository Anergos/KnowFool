using Android.Views;

namespace HumbleFool_Project.Helper
{
    public interface IItemClickListener
    {
        void OnClick(View itemView, int position, bool isLongClick);
    }
}