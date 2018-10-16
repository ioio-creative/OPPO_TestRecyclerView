using System.Collections.Generic;
using System.Linq;

namespace TestRecyclerView
{
    public class MyImageAlbum
    {
        private static MyImage[] mBuiltInImages = new MyImage[]
        {
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00000 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00000 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00001 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00001 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00002 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00002 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00003 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00003 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00004 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00004 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00005 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00005 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00006 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00006 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00007 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00007 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00008 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00008 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00009 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00009 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00010 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00010 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00011 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00011 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00012 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00012 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00013 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00013 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00014 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00014 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00015 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00015 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00016 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00016 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00017 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00017 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00018 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00018 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00019 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00019 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00020 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00020 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00021 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00021 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00022 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00022 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00023 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00023 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00024 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00024 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00025 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00025 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00026 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00026 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00027 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00027 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00028 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00028 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00029 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00029 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00030 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00030 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00031 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00031 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00032 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00032 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00033 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00033 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00034 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00034 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00035 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00035 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00036 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00036 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00037 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00037 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00038 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00038 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00039 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00039 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00040 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00040 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00041 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00041 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00042 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00042 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00043 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00043 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00044 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00044 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00045 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00045 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00046 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00046 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00047 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00047 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00048 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00048 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00049 },
            new MyImage { mImageID = Resource.Drawable.scrollBar_new_00049 }
        };

        private List<MyImage> mImages;

        public MyImageAlbum()
        {
            mImages = new List<MyImage>();
            mImages.AddRange(mBuiltInImages);
        }

        public int NumImages
        {
            get { return mImages.Count(); }
        }

        // indexer
        public MyImage this[int i]
        {
            get { return mImages[i]; }
        }

        public void Add(MyImage img)
        {
            mImages.Add(img);
        }

        public void AddData()
        {
            mImages.AddRange(MyImageAlbum.mBuiltInImages);
        }

        public void AddRange(IEnumerable<MyImage> images)
        {
            mImages.AddRange(images);
        }
    }
}