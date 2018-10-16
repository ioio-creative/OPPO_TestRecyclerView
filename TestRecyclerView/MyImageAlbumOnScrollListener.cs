using Android.Support.V7.Widget;
using Java.Lang;
using System.Diagnostics;

namespace TestRecyclerView
{
    public class MyImageAlbumOnScrollListener : RecyclerView.OnScrollListener
    {
        private const int maxSpeedRange = 1000;
        private const int minSpeedRange = -maxSpeedRange;
        private const int maxSpeedLevel = 10;
        private const int halfMaxSpeedLevel = maxSpeedLevel / 2;
        private const int minSpeedLevel = 0;

        private Server tcpServer;
        private Stopwatch stopWatch = new Stopwatch();
        private int currentLastVisibleItemPosition = -1;
        private float currentScrollSpeed = minSpeedLevel - 1;

        // The minimum amount of items to have below your current scroll position
        // before loading more.
        private const int visibleThreshold = 20;
        // The current offset index of data you have loaded
        private int currentPage = 0;
        // The total number of items in the dataset after the last load
        private int previousTotalItemCount = 0;
        // True if we are still waiting for the last set of data to load.
        private bool loading = true;
        // Sets the starting page index
        private int startingPageIndex = 0;        

        RecyclerView.LayoutManager mLayoutManager;

        public MyImageAlbumOnScrollListener(LinearLayoutManager layoutManager)
        {
            this.mLayoutManager = layoutManager;
            StartTcpServer();
        }

        // This happens many times a second during a scroll, so be wary of the code you place here.
        // We are given a few useful parameters to help us work out if we need to load some more data,
        // but first we check if we are waiting for the previous load to finish.
        public override void OnScrolled(RecyclerView view, int dx, int dy)
        {
            int totalItemCount = mLayoutManager.ItemCount;
            int lastVisibleItemPosition = GetLastVisibleItemPosition();

            //MainActivity.Log(lastVisibleItemPosition.ToString());
            //tcpServer.Send(lastVisibleItemPosition.ToString());            

            // If the total item count is zero and the previous isn't, assume the
            // list is invalidated and should be reset back to initial state
            if (totalItemCount < previousTotalItemCount)
            {
                this.currentPage = this.startingPageIndex;
                this.previousTotalItemCount = totalItemCount;
                if (totalItemCount == 0)
                {
                    this.loading = true;
                }
            }
            // If it’s still loading, we check to see if the dataset count has
            // changed, if so we conclude it has finished loading and update the current page
            // number and total item count.
            if (loading && (totalItemCount > previousTotalItemCount))
            {
                loading = false;
                previousTotalItemCount = totalItemCount;
            }

            // If it isn’t currently loading, we check to see if we have breached
            // the visibleThreshold and need to reload more data.
            // If we do need to reload some more data, we execute onLoadMore to fetch the data.
            // threshold should reflect how many total columns there are too
            if (!loading && (lastVisibleItemPosition + visibleThreshold) > totalItemCount)
            {
                currentPage++;
                OnLoadMore(currentPage, totalItemCount, view);
                loading = true;
            }            

            //float scrollSpeed;
            //switch (view.ScrollState)
            //{
            //    case RecyclerView.ScrollStateDragging:
            //        //MainActivity.Log("ScrollStateDragging");
            //        if (stopWatch.IsRunning)
            //        {
            //            scrollSpeed = CalculateScrollSpeed();
            //            int intScrollSpeed = Math.Round(scrollSpeed);

            //            if (Math.Round(this.currentScrollSpeed) != intScrollSpeed)
            //            {
            //                string strScrollSpeed = intScrollSpeed.ToString();
            //                tcpServer.Send(strScrollSpeed);
            //                MainActivity.Log("Speed: " + strScrollSpeed);
            //            }

            //            this.currentScrollSpeed = scrollSpeed;
            //        }
            //        else
            //        {
            //            this.currentLastVisibleItemPosition = GetLastVisibleItemPosition();
            //        }
            //        stopWatch.Restart();
            //        break;
            //    case RecyclerView.ScrollStateSettling:
            //        //MainActivity.Log("ScrollStateSettling");
            //        //scrollSpeed = CalculateAndSendScrollSpeed();
            //        //stopWatch.Restart();
            //        stopWatch.Stop();
            //        this.currentScrollSpeed = minSpeedLevel - 1;
            //        break;
            //    case RecyclerView.ScrollStateIdle:
            //        //MainActivity.Log("ScrollStateIdle");
            //        //scrollSpeed = CalculateAndSendScrollSpeed();
            //        stopWatch.Stop();
            //        this.currentScrollSpeed = minSpeedLevel - 1;
            //        break;
            //}

            switch (view.ScrollState)
            {
                case RecyclerView.ScrollStateDragging:
                    //MainActivity.Log("ScrollStateDragging");

                    if (currentLastVisibleItemPosition != -1)
                    {
                        int diffInLastVisibleItemPositon = lastVisibleItemPosition - currentLastVisibleItemPosition;
                        currentLastVisibleItemPosition = lastVisibleItemPosition;
                        MainActivity.Log("Diff: " + diffInLastVisibleItemPositon);
                        tcpServer.Send("d " + diffInLastVisibleItemPositon.ToString());
                    }

                    break;
                case RecyclerView.ScrollStateSettling:
                    //MainActivity.Log("ScrollStateSettling");                    
                    break;
                case RecyclerView.ScrollStateIdle:
                    //MainActivity.Log("ScrollStateIdle");                    
                    break;
            }

            float scrollSpeed;
            if (stopWatch.IsRunning)
            {
                scrollSpeed = CalculateScrollSpeed();
                int intScrollSpeed = Math.Round(scrollSpeed);

                if (Math.Round(this.currentScrollSpeed) != intScrollSpeed)
                {
                    string strScrollSpeed = intScrollSpeed.ToString();
                    tcpServer.Send("s " + strScrollSpeed);
                    MainActivity.Log("Speed: " + strScrollSpeed);
                }

                this.currentScrollSpeed = scrollSpeed;
            }
            else
            {
                this.currentLastVisibleItemPosition = GetLastVisibleItemPosition();
            }
            stopWatch.Restart();
        }

        //public override void OnScrollStateChanged(RecyclerView recyclerView, int newState)
        //{
        //    base.OnScrollStateChanged(recyclerView, newState);

        //    float scrollSpeed;
            
        //    switch (newState)
        //    {
        //        case RecyclerView.ScrollStateDragging:
        //            MainActivity.Log("ScrollStateDragging");
        //            this.currentLastVisibleItemPosition = GetLastVisibleItemPosition();
        //            stopWatch.Restart();
        //            break;
        //        case RecyclerView.ScrollStateSettling:
        //            MainActivity.Log("ScrollStateSettling");
        //            scrollSpeed = CalculateScrollSpeed();
        //            MainActivity.Log(scrollSpeed);
        //            stopWatch.Restart();
        //            break;
        //        case RecyclerView.ScrollStateIdle:
        //            MainActivity.Log("ScrollStateIdle");
        //            scrollSpeed = CalculateScrollSpeed();
        //            MainActivity.Log(scrollSpeed);
        //            stopWatch.Stop();
        //            break;
        //    }
        //}

        // Call this method whenever performing new searches
        public void ResetState()
        {
            this.currentPage = this.startingPageIndex;
            this.previousTotalItemCount = 0;
            this.loading = true;
        }

        // https://gist.github.com/rogerhu/17aca6ad4dbdb3fa5892
        public void OnLoadMore(int page, int totalItemsCount, RecyclerView view)
        {
            MyImageAlbumAdapter adapter = view.GetAdapter() as MyImageAlbumAdapter;
            int curSize = adapter.ItemCount;
            adapter.LoadMoreDataToImageAlbum();

            view.Post(new Runnable(() =>
            {
                adapter.NotifyItemRangeInserted(curSize, adapter.ItemCount - 1);
            }));
        }

        private int GetLastVisibleItemPosition()
        {
            return ((LinearLayoutManager)mLayoutManager).FindLastVisibleItemPosition();
        }

        private float CalculateScrollSpeed()
        {
            stopWatch.Stop();
            int lastVisibleItemPosition = GetLastVisibleItemPosition();
            float scrollSpeed = ((float)(lastVisibleItemPosition - this.currentLastVisibleItemPosition)) * 10000 / stopWatch.ElapsedMilliseconds;            

            float mappedScrollSpeed = LinearMapRange(scrollSpeed, minSpeedRange, maxSpeedRange, minSpeedLevel, maxSpeedLevel);
            //float mappedScrollSpeed = LogisticMapRange(scrollSpeed, -minSpeedRange, maxSpeedRange, minSpeedLevel, maxSpeedLevel);

            // Do not send zero speed?
            //if (Math.Round(mappedScrollSpeed) == halfMaxSpeedLevel)
            //{
            //    mappedScrollSpeed = (scrollSpeed > 0) ? mappedScrollSpeed + 1 : mappedScrollSpeed - 1;
            //}

            //MainActivity.Log(scrollSpeed + ", " + mappedScrollSpeed + ", " + Math.Round(mappedScrollSpeed));

            this.currentLastVisibleItemPosition = lastVisibleItemPosition;
            
            return mappedScrollSpeed;
        }

        private float LinearMapRange(float oldValue, float oldLow, float oldHigh, float newLow, float newHigh)
        {
            if (oldValue > oldHigh)
            {
                return newHigh;
            }

            if (oldValue < oldLow)
            {
                return newLow;
            }

            return newLow + (oldValue - oldLow) * (newHigh - newLow) / (oldHigh - oldLow);
        }

        //private float LogisticMapRange(float oldValue, float oldLow, float oldHigh, float newLow, float newHigh)
        //{
        //    if (oldValue > oldHigh)
        //    {
        //        return oldHigh;
        //    }

        //    if (oldValue < oldLow)
        //    {
        //        return oldLow;
        //    }

        //    return 10f / (1f + (float)Math.Exp(-oldValue));
        //}

        private void StartTcpServer()
        {
            tcpServer = new Server();
            tcpServer.StartServer();
        }
    }
}