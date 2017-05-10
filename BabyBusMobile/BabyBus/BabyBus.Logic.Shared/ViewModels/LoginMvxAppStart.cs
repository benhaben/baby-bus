using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;


namespace BabyBus.Logic.Shared
{
    public class LoginMvxAppStart<TViewModel>
    : MvxNavigatingObject
          , IMvxAppStart
        where TViewModel : IMvxViewModel
    {
        public void Start(object hint = null)
        {
            if (hint != null)
            {
                MvxTrace.Trace("Hint ignored in default MvxAppStart");
            }
//			build failure
            ShowViewModel<TViewModel>(requestedBy:new MvxRequestedBy());
        }
    }
}
