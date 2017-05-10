using System.Collections.Generic;
using Android.OS;
using Android.Support.V4.App;
using BabyBus.Droid.Views.Frags;
using BabyBus.Services;

namespace BabyBus.Droid.Adapters {
    public class ImageSlideAdapter : FragmentPagerAdapter {
        private List<string> _list;

        public ImageSlideAdapter(FragmentManager fm) : base(fm) {
        }

        public List<string> List {
            set { _list = value; }
        }


        public override int Count {
            get { return _list.Count; }
        }

        public override Fragment GetItem(int position) {
            return ImageSlideFragment.NewInstance(_list[position]);
        }
    }
}