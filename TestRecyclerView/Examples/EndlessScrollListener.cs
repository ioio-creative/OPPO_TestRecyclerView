using Android.Support.V7.Widget;
using System.Linq;

namespace TestRecyclerView
{
    // https://gist.github.com/nesquena/d09dc68ff07e845cc622
    public abstract class EndlessRecyclerViewScrollListener : RecyclerView.OnScrollListener
    {
        // The minimum amount of items to have below your current scroll position
        // before loading more.
        private int visibleThreshold = 10;
        // The current offset index of data you have loaded
        private int currentPage = 0;
        // The total number of items in the dataset after the last load
        private int previousTotalItemCount = 0;
        // True if we are still waiting for the last set of data to load.
        private bool loading = true;
        // Sets the starting page index
        private int startingPageIndex = 0;

        RecyclerView.LayoutManager mLayoutManager;

        public EndlessRecyclerViewScrollListener(LinearLayoutManager layoutManager)
        {
            this.mLayoutManager = layoutManager;
        }

        public EndlessRecyclerViewScrollListener(GridLayoutManager layoutManager)
        {
            this.mLayoutManager = layoutManager;
            visibleThreshold = visibleThreshold * layoutManager.SpanCount;
        }

        public EndlessRecyclerViewScrollListener(StaggeredGridLayoutManager layoutManager)
        {
            this.mLayoutManager = layoutManager;
            visibleThreshold = visibleThreshold * layoutManager.SpanCount;
        }

        public int GetLastVisibleItem(int[] lastVisibleItemPositions)
        {
            //int maxSize = 0;
            //for (int i = 0; i < lastVisibleItemPositions.Count(); i++)
            //{
            //    if (i == 0)
            //    {
            //        maxSize = lastVisibleItemPositions[i];
            //    }
            //    else if (lastVisibleItemPositions[i] > maxSize)
            //    {
            //        maxSize = lastVisibleItemPositions[i];
            //    }
            //}
            //return maxSize;
            return (lastVisibleItemPositions.Count() == 0) ? 0 : lastVisibleItemPositions.Max();
        }

        // This happens many times a second during a scroll, so be wary of the code you place here.
        // We are given a few useful parameters to help us work out if we need to load some more data,
        // but first we check if we are waiting for the previous load to finish.
        public override void OnScrolled(RecyclerView view, int dx, int dy)
        {
            int lastVisibleItemPosition = 0;
            int totalItemCount = mLayoutManager.ItemCount;

            if (mLayoutManager is StaggeredGridLayoutManager)
            {
                int[] lastVisibleItemPositions = ((StaggeredGridLayoutManager)mLayoutManager).FindLastVisibleItemPositions(null);
                // get maximum element within the list
                lastVisibleItemPosition = GetLastVisibleItem(lastVisibleItemPositions);
            }
            else if (mLayoutManager is GridLayoutManager)
            {
                lastVisibleItemPosition = ((GridLayoutManager)mLayoutManager).FindLastVisibleItemPosition();
            }
            else if (mLayoutManager is LinearLayoutManager)
            {
                lastVisibleItemPosition = ((LinearLayoutManager)mLayoutManager).FindLastVisibleItemPosition();
            }

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
        }

        // Call this method whenever performing new searches
        public void ResetState()
        {
            this.currentPage = this.startingPageIndex;
            this.previousTotalItemCount = 0;
            this.loading = true;
        }

        // Defines the process for actually loading more data based on page
        public abstract void OnLoadMore(int page, int totalItemsCount, RecyclerView view);
    }
}