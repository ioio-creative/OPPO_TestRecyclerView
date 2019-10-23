using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;

namespace TestRecyclerView
{
    [Activity(Label = "TestRecyclerView", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
#if DEBUG
        private const bool isDebugLog = false;
#else
        private const bool isDebugLog = false;
#endif

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

            // Instantiate the MyImage album:
            mMyImageAlbum = new MyImageAlbum();

            // Create an adapter for the RecyclerView, and pass it the
            // data set (the MyImage album) to manage:
            mAdapter = new MyImageAlbumAdapter(mMyImageAlbum);

            // Register the item click handler (below) with the adapter:
            //mAdapter.ItemClick += OnItemClick;

            // Plug the adapter into the RecyclerView:
            mRecyclerView.SetAdapter(mAdapter);

            mScrollListener = new MyImageAlbumOnScrollListener(mLayoutManager as LinearLayoutManager);
            mRecyclerView.AddOnScrollListener(mScrollListener);

            // needed for recording numItemsPerScreenView during first scroll
            mRecyclerView.SmoothScrollToPosition(mMyImageAlbum.NumImages / 2);
        }

        protected override void OnStart()
        {
            base.OnStart();
            mScrollListener.OnActivityStart();
        }

        protected override void OnStop()
        {
            base.OnStop();
            mScrollListener.OnActivityStop();
        }

        public static void Log(object obj)
        {
            if (isDebugLog)
            {
                System.Diagnostics.Debug.WriteLine(obj);
            }
        }
    }    
}

