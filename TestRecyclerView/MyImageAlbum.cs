using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace TestRecyclerView
{
    public class MyImageAlbum
    {
        private const int initialMinNumImagesRequired = (2562 / 6) * 3;

        private MyImage[] mBuiltInImages;
        //private static MyImage[] mBuiltInImages = new MyImage[]
        //{
        //    new MyImage { mImageID = Resource.Drawable.scrollBar_new_00001 }
        //};

        private readonly List<MyImage> mImages;

        public MyImageAlbum() : this(initialMinNumImagesRequired) { }

        public MyImageAlbum(int initialMinNumImages)
        {            
            // https://stackoverflow.com/questions/10261824/how-can-i-get-all-constants-of-a-type-by-reflection
            FieldInfo[] drawableFieldInfos = typeof(Resource.Drawable)
                .GetFields();

            mBuiltInImages = drawableFieldInfos
                .Where(drawableFileInfo => (drawableFileInfo.Name.IndexOf("scrollBar_new_") > -1))
                //.Take(120)
                .Select(drawableFileInfo => new MyImage { mImageID = int.Parse(drawableFileInfo.GetValue(null).ToString()) })
                .ToArray();            

            mImages = new List<MyImage>(mBuiltInImages);

            // at least add one more set
            // may be useful for implementing infinite scroll in both top and bottom ends
            AddData();

            while (NumImages < initialMinNumImages)
            {
                AddData();
            }
        }    

        public int NumImages
        {
            get { return mImages.Count; }
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
            mImages.AddRange(mBuiltInImages);
        }

        public void AddRange(IEnumerable<MyImage> images)
        {
            mImages.AddRange(images);
        }
    }
}