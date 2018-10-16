using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using System;
using System.Timers;

namespace TestRecyclerView
{
    [Activity(Label = "TestRecyclerView", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // RecyclerView instance that displays the photo album:
        private RecyclerView mRecyclerView;

        // Layout manager that lays out each card in the RecyclerView:
        private RecyclerView.LayoutManager mLayoutManager;

        // Adapter that accesses the data set (a photo album):
        private MyImageAlbumAdapter mAdapter;

        // MyImage album that is managed by the adapter:
        private MyImageAlbum mMyImageAlbum;

        private MyImageAlbumOnScrollListener mScrollListener;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // http://stackoverflow.com/questions/2591036/how-to-hide-the-title-bar-for-an-activity-in-xml-with-existing-custom-theme
            //Remove title bar
            this.RequestWindowFeature(WindowFeatures.NoTitle);

            //Remove notification bar
            this.Window.SetFlags(WindowManagerFlags.Fullscreen, WindowManagerFlags.Fullscreen);

            //set content view AFTER ABOVE sequence (to avoid crash)
            SetContentView(Resource.Layout.Main);

            // Instantiate the MyImage album:
            mMyImageAlbum = new MyImageAlbum();
            while (mMyImageAlbum.NumImages < 500 * 25)
            {
                mMyImageAlbum.AddData();
            }

            // Get our RecyclerView layout:
            mRecyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            //............................................................
            // Layout Manager Setup:

            // Use the built-in linear layout manager:
            mLayoutManager = new LinearLayoutManager(this);

            // Or use the built-in grid layout manager (two horizontal rows):
            // mLayoutManager = new GridLayoutManager
            //        (this, 2, GridLayoutManager.Horizontal, false);

            // Plug the layout manager into the RecyclerView:
            mRecyclerView.SetLayoutManager(mLayoutManager);

            //............................................................
            // Adapter Setup:

            // Create an adapter for the RecyclerView, and pass it the
            // data set (the MyImage album) to manage:
            mAdapter = new MyImageAlbumAdapter(mMyImageAlbum);

            // Register the item click handler (below) with the adapter:
            //mAdapter.ItemClick += OnItemClick;

            // Plug the adapter into the RecyclerView:
            mRecyclerView.SetAdapter(mAdapter);

            mScrollListener = new MyImageAlbumOnScrollListener((LinearLayoutManager)mLayoutManager);
            mRecyclerView.AddOnScrollListener(mScrollListener);

            mRecyclerView.SmoothScrollToPosition(mMyImageAlbum.NumImages / 2);
        }
      
        public static void Log(object obj)
        {
            System.Diagnostics.Debug.WriteLine(obj);
        }
    }    
}

