using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestRecyclerView
{
    public class MyImageAlbum
    {
        private static MyImage[] mBuiltInImages;
        //private static MyImage[] mBuiltInImages = new MyImage[]
        //{
        //    new MyImage { mImageID = Resource.Drawable.scrollBar_new_00001 }
        //};

        private List<MyImage> mImages;

        public MyImageAlbum()
        {            
            // https://stackoverflow.com/questions/10261824/how-can-i-get-all-constants-of-a-type-by-reflection
            FieldInfo[] drawableFieldInfos = typeof(Resource.Drawable)
                .GetFields();

            mBuiltInImages = drawableFieldInfos
                .Where(drawableFileInfo => (drawableFileInfo.Name.IndexOf("scrollBar_new_") > -1))
                .Take(50)
                .Select(drawableFileInfo => new MyImage { mImageID = int.Parse(drawableFileInfo.GetValue(null).ToString()) })
                .ToArray();

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