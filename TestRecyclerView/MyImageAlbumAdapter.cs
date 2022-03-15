using AndroidX.RecyclerView.Widget;
using Android.Views;

namespace TestRecyclerView
{
    //----------------------------------------------------------------------
    // ADAPTER

    // Adapter to connect the data set (photo album) to the RecyclerView: 
    public class MyImageAlbumAdapter : RecyclerView.Adapter
    {
        // Event handler for item clicks:
        //public event EventHandler<int> ItemClick;

        // Underlying data set (a MyImage album):
        private readonly MyImageAlbum mMyImageAlbum;

        // Load the adapter with the data set (photo album) at construction time:
        public MyImageAlbumAdapter(MyImageAlbum anImageAlbum)
        {
            mMyImageAlbum = anImageAlbum;
        }

        // Create a new photo CardView (invoked by the layout manager): 
        public override RecyclerView.ViewHolder
            OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            // Inflate the CardView for the photo:
            View itemView = LayoutInflater.From(parent.Context).
                        Inflate(Resource.Layout.ImageCardView, parent, false);

            // Create a ViewHolder to find and hold these view references, and 
            // register OnClick with the view holder:
            //MyImageViewHolder vh = new MyImageViewHolder(itemView, OnClick);
            MyImageViewHolder vh = new MyImageViewHolder(itemView);
            return vh;
        }

        // Fill in the contents of the photo card (invoked by the layout manager):
        public override void
            OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyImageViewHolder vh = holder as MyImageViewHolder;

            // Set the ImageView and TextView in this ViewHolder's CardView 
            // from this position in the MyImage album:
            vh.Image.SetImageResource(mMyImageAlbum[position].ImageID);
        }

        // Return the number of photos available in the photo album:
        public override int ItemCount
        {
            get { return mMyImageAlbum.NumImages; }
        }

        // Raise an event when the item-click takes place:
        //void OnClick(int position)
        //{
        //    if (ItemClick != null)
        //        ItemClick(this, position);
        //}
        
        public void LoadMoreDataToImageAlbum()
        {            
            mMyImageAlbum.AddData();
        }
    }
}