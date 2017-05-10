using System.Collections.Generic;
using Android.Support.V4.App;
using BabyBus.Droid.Views.Frags;

namespace BabyBus.Droid.Adapters
{
	public class ImageSlideAdapter : FragmentPagerAdapter
	{
		private List<int> _list;
		private List<string> _stringlist;

		public ImageSlideAdapter(FragmentManager fm)
			: base(fm)
		{
		}

		public List<int> List {
			set { _list = value; }
		}

		public List<string> ImageList {
			set { _stringlist = value; }
		}

		public override int Count {
			get { return _stringlist == null ? _list.Count : _stringlist.Count; }
		}

		public override Fragment GetItem(int position)
		{
			if (_stringlist != null) {
				return	ImageSlideFragment.NewInstance(_stringlist[position]);
			} else if (_list != null) {
				return ImageSlideFragment.NewInstance(_list[position]);
			} else {
				return null;
			}

		}
	}
}