using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Fragment = Android.Support.V4.App.Fragment;
using FragmentManager = Android.Support.V4.App.FragmentManager;

namespace BabyBus.Droid.Adapters
{
    public class PhotoPagerAdapter : FragmentPagerAdapter {
        private List<string> _photoNameList;

        public List<string> PhotoNameList {
            set {
                if (value == null) {
                    _photoNameList = new List<string>();
                }
                else {
                    _photoNameList = value;
                }

            }
        } 

        public PhotoPagerAdapter(FragmentManager fm) : base(fm) {
        }

        public override int Count {
			get { return _photoNameList.Count; }
        }

        public override Fragment GetItem(int position) {
            return PhotoFragment.NewInstance(_photoNameList[position]);
        }
    }
}