using AndroidX.RecyclerView.Widget;
using Java.Lang;
using System.Diagnostics;
using System.Timers;

namespace TestRecyclerView
{
    public class MyImageAlbumOnScrollListener : RecyclerView.OnScrollListener
    {
        //private const int maxSpeedRange = 1000;
        //private const int minSpeedRange = -maxSpeedRange;
        //private const int maxSpeedLevel = 10;
        //private const int halfMaxSpeedLevel = maxSpeedLevel / 2;
        private const int minSpeedLevel = 0;        

        private Server tcpServer;
        private Stopwatch stopWatch;
        private int currentLastVisibleItemPosition = -1;
        private float currentScrollSpeed = minSpeedLevel - 1;

        // The minimum amount of items to have below your current scroll position
        // before loading more.
        private const float visibleThresholdRatioPerScreenView = 0.5f;
        private int visibleThreshold
        {
            get { return (int)(visibleThresholdRatioPerScreenView * numItemsPerScreenView); }
        }
        private int numItemsPerScreenView = 0;        
        private bool isFirstScroll = true;
        // The current offset index of data you have loaded
        private int currentPage = 0;
        // The total number of items in the dataset after the last load
        private int previousTotalItemCount = 0;
        // True if we are still waiting for the last set of data to load.
        private bool loading = true;
        // Sets the starting page index
        private const int startingPageIndex = 0;

        // TODO:
        private int onScrolledCounter = 0;
        private int onScrolledCounterValueWhenTopReached = -1;
        private const int numberOfTimesToSkipBeforeCheckingTopReachedCondition = 30;
        private const int numberOfTimesToSkipSendingAfterTopReached = 10;


        #region sendTimer

        private const int sendTimerIntervalInMillis = 50;  // depends on tcp client's framerate
        private Timer sendTimer;
        private bool isInSendWindow = false;

        #endregion


        //private readonly RecyclerView.LayoutManager mLayoutManager;
        private readonly LinearLayoutManager mLayoutManager;

        public MyImageAlbumOnScrollListener(LinearLayoutManager layoutManager) : base()
        {
            mLayoutManager = layoutManager;

            stopWatch = new Stopwatch();
            tcpServer = new Server();
        }

        // This happens many times a second during a scroll, so be wary of the code you place here.
        // We are given a few useful parameters to help us work out if we need to load some more data,
        // but first we check if we are waiting for the previous load to finish.
        public override void OnScrolled(RecyclerView view, int dx, int dy)
        {            
            int totalItemCount = mLayoutManager.ItemCount;            
            int lastVisibleItemPosition = GetLastVisibleItemPosition();
            
            MainActivity.Log($"onScrolledCounter: {onScrolledCounter}");
            MainActivity.Log($"lastVisibleItemPosition: {lastVisibleItemPosition}");

            //MainActivity.Log(lastVisibleItemPosition);
            //Send(lastVisibleItemPosition.ToString());

            // first OnScrolled
            if (isFirstScroll)
            {
                isFirstScroll = false;
                numItemsPerScreenView = lastVisibleItemPosition;
                MainActivity.Log($"numItemsPerScreenView: {numItemsPerScreenView}");
            }

            MainActivity.Log($"onScrolledCounter: {onScrolledCounter}");            

            // check if top is reached, "top-less" effect
            if (!isFirstScroll &&
                onScrolledCounter > numberOfTimesToSkipBeforeCheckingTopReachedCondition &&
                GetFirstVisibleItemPosition() < numItemsPerScreenView + visibleThreshold)
            {
                MainActivity.Log("Top");
                onScrolledCounterValueWhenTopReached = onScrolledCounter;
                int totalScreenViewCount = numItemsPerScreenView != 0 ? totalItemCount / numItemsPerScreenView : 0;
                MainActivity.Log($"totalScreenViewCount: {totalScreenViewCount}");
                view.Post(new Runnable(() =>
                {
                    // TODO: some magic numbers here.
                    //view.ScrollBy(0, (int)(6 * numItemsPerScreenView * 7.0675));
                    view.ScrollBy(0, (int)(6 * numItemsPerScreenView * 9));
                }));
            }

            // If the total item count is zero and the previous isn't, assume the
            // list is invalidated and should be reset back to initial state
            if (totalItemCount < previousTotalItemCount)
            {
                currentPage = startingPageIndex;
                previousTotalItemCount = totalItemCount;
                if (totalItemCount == 0)
                {
                    loading = true;
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

            switch (view.ScrollState)
            {
                case RecyclerView.ScrollStateDragging:
                    //MainActivity.Log("ScrollStateDragging");
                    if (currentLastVisibleItemPosition != -1)
                    {
                        int diffInLastVisibleItemPositon = lastVisibleItemPosition - currentLastVisibleItemPosition;
                        currentLastVisibleItemPosition = lastVisibleItemPosition;                        
                        Send("d " + diffInLastVisibleItemPositon.ToString());
                        MainActivity.Log("d: " + diffInLastVisibleItemPositon);                        
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
                
                string strScrollSpeed = intScrollSpeed.ToString();
                Send("s " + strScrollSpeed);
                MainActivity.Log("s: " + strScrollSpeed);
     
                currentScrollSpeed = scrollSpeed;                
            }
            else
            {
                currentLastVisibleItemPosition = GetLastVisibleItemPosition();
            }
            stopWatch.Restart();


            onScrolledCounter++;
        } 

        // Call this method whenever performing new searches
        public void ResetState()
        {
            currentPage = startingPageIndex;
            previousTotalItemCount = 0;
            loading = true;
        }

        // https://gist.github.com/rogerhu/17aca6ad4dbdb3fa5892        
        private void OnLoadMore(int page, int totalItemsCount, RecyclerView view)
        {
            MyImageAlbumAdapter adapter = view.GetAdapter() as MyImageAlbumAdapter;
            int curSize = adapter.ItemCount;
            adapter.LoadMoreDataToImageAlbum();

            view.Post(new Runnable(() =>
            {
                adapter.NotifyItemRangeInserted(curSize, adapter.ItemCount - 1);
            }));
        }

        private int GetFirstVisibleItemPosition()
        {
            return mLayoutManager.FindFirstVisibleItemPosition();
        }

        private int GetFirstCompletelyVisibleItemPosition()
        {
            return mLayoutManager.FindFirstCompletelyVisibleItemPosition();
        }     
        
        private int GetLastVisibleItemPosition()
        {
            //return (mLayoutManager as LinearLayoutManager).FindLastVisibleItemPosition();
            return mLayoutManager.FindLastVisibleItemPosition();
        }

        private int GetLastCompletelyVisibleItemPosition()
        {
            return mLayoutManager.FindLastCompletelyVisibleItemPosition();
        }

        private float CalculateScrollSpeed()
        {
            stopWatch.Stop();
            int lastVisibleItemPosition = GetLastVisibleItemPosition();
            float scrollSpeed = ((float)(lastVisibleItemPosition - this.currentLastVisibleItemPosition)) * 10000 / stopWatch.ElapsedMilliseconds;

            float mappedScrollSpeed = scrollSpeed;
            //float mappedScrollSpeed = LinearMapRange(scrollSpeed, minSpeedRange, maxSpeedRange, minSpeedLevel, maxSpeedLevel);
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



        #region sendServer

        private void StartTcpServer()
        {            
            tcpServer.StartServer();
        }

        private void Send(string msg)
        {
            Send(msg, false);
        }

        private void Send(string msg, bool isForceSend)
        {
            if (
                isForceSend || 
                (isInSendWindow && (onScrolledCounter > onScrolledCounterValueWhenTopReached + numberOfTimesToSkipSendingAfterTopReached))
               )
            {
                isInSendWindow = false;
                tcpServer.Send(msg);
                MainActivity.Log("Sent");
            }
        }

        private void StopTcpServer()
        {
            tcpServer.StopServer();
        }

        #endregion


        #region sendTimer

        private void SetSendTimer()
        {
            sendTimer = new Timer(sendTimerIntervalInMillis);
            sendTimer.Elapsed += OnSendTimerTimedEvent;
            sendTimer.AutoReset = true;
            sendTimer.Enabled = true;
        }

        private void UnsetSendTimer()
        {
            sendTimer.Dispose();
        }

        private void OnSendTimerTimedEvent(object sender, ElapsedEventArgs e)
        {
            isInSendWindow = true;
        }

        #endregion


        #region activity lifecycle

        // initialise resources
        public void OnActivityStart()
        {
            //stopWatch = new Stopwatch();
            SetSendTimer();
            StartTcpServer();
        }

        // clear resources
        public void OnActivityStop()
        {
            StopTcpServer();
            UnsetSendTimer();
            /**
             * !!!Important!!!
             * setting stopWatch = null 
             * may result in null reference error in OnScrolled() event handler
             */
            //stopWatch = null;                        
        }

        #endregion
    }
}