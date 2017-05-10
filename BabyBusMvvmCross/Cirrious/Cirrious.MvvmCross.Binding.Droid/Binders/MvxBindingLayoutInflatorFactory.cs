// MvxBindingLayoutInflatorFactory.cs
// (c) Copyright Cirrious Ltd. http://www.cirrious.com
// MvvmCross is licensed using Microsoft Public License (Ms-PL)
// Contributions and inspirations noted in readme.md and license.txt
// 
// Project Lead - Stuart Lodge, @slodge, me@slodge.com

using System.Collections.Generic;
using Android.Content;
using Android.Util;
using Android.Views;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Binding.Bindings;
using Cirrious.MvvmCross.Binding.Droid.ResourceHelpers;

namespace Cirrious.MvvmCross.Binding.Droid.Binders
{
    public class MvxBindingLayoutInflatorFactory
        : Java.Lang.Object
        , IMvxLayoutInfactorFactory
    {
        private readonly object _source;

        private IMvxAndroidViewFactory _androidViewFactory;
        private IMvxAndroidViewBinder _binder;

        public MvxBindingLayoutInflatorFactory(
            object source)
        {
            _source = source;
        }

        protected IMvxAndroidViewFactory AndroidViewFactory
        {
            get
            {
                if (_androidViewFactory == null)
                    _androidViewFactory = Mvx.Resolve<IMvxAndroidViewFactory>();
                return _androidViewFactory;
            }
        }

        protected IMvxAndroidViewBinder Binder
        {
            get
            {
                if (_binder == null)
                    _binder = Mvx.Resolve<IMvxAndroidViewBinderFactory>().Create(_source);
                return _binder;
            }
        }

        public List<IMvxUpdateableBinding> CreatedBindings
        {
            get { return Binder.CreatedBindings; }
        }

        public View OnCreateView(string name, Context context, IAttributeSet attrs)
        {
            if (name == "fragment")
            {
                // MvvmCross does not inflate Fragments - instead it returns null and lets Android inflate them.
                return null;
            }

            View view = AndroidViewFactory.CreateView(name, context, attrs);
            if (view != null)
            {
                Binder.BindView(view, context, attrs);
                //Permission Check Test
                //PermissionCheck(view, context, attrs);
            }

            return view;
        }

//        private void PermissionCheck(View view,Context context, IAttributeSet attrs)
//        {
//            using (
//               var typedArray = context.ObtainStyledAttributes(attrs,
//                                                               MvxAndroidBindingResource.Instance
//                                                                                        .PermissionStylableGroupId))
//            {
//                int numStyles = typedArray.IndexCount;
//                for (var i = 0; i < numStyles; ++i)
//                {
//                    var attributeId = typedArray.GetIndex(i);
//
//                    if (attributeId == MvxAndroidBindingResource.Instance.IsVisiableId)
//                    {
//                        var attrValue = typedArray.GetString(i);
//                        view.Visibility = ViewStates.Gone;
//                        foreach (var per in PermissionContainer.Permission)
//                        {
//                            if (attrValue == per)
//                            {
//                                view.Visibility = ViewStates.Visible;
//                                return;
//                            }
//                        }
//                    }
//                }
//                typedArray.Recycle();
//            }
//        }
    }
}