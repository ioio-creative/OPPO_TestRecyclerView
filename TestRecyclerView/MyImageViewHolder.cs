using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

namespace TestRecyclerView
{
    //----------------------------------------------------------------------
    // VIEW HOLDER

    // Implement the ViewHolder pattern: each ViewHolder holds references
    // to the UI components (ImageView and TextView) within the CardView 
    // that is displayed in a row of the RecyclerView:
    public class MyImageViewHolder : RecyclerView.ViewHolder
    {
        public ImageView Image { get; private set; }

        // Get references to the views defined in the CardView layout.
        //public MyImageViewHolder(View itemView, Action<int> listener)
        //    : base(itemView)
        //{
        //    // Locate and cache view references:
        //    Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);

        //    // Detect user clicks on the item view and report which item
        //    // was clicked (by position) to the listener:
        //    itemView.Click += (sender, e) => listener(base.Position);
        //}

        // Get references to the views defined in the CardView layout.
        public MyImageViewHolder(View itemView)
            : base(itemView)
        {
            // Locate and cache view references:
            Image = itemView.FindViewById<ImageView>(Resource.Id.imageView);

            // Detect user clicks on the item view and report which item
            // was clicked (by position) to the listener:
            //itemView.Click += (sender, e) => listener(base.Position);
        }
    }
}