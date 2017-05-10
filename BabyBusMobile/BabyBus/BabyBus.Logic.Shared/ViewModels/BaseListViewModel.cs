using System;
using System.Collections;


namespace BabyBus.Logic.Shared
{

    public class BaseListViewModel:BaseViewModel
    {

        /// <summary>
        /// Gets or sets the list object.
        /// ListObject just store data for a while, after call FirstLoadedEventHandler, dispose it
        /// why: because we do not want call FirstLoadEventInvoke explicite
        /// </summary>
        /// <value>The list object.</value>
        public IEnumerable ListObject
        {
            get;
            set;
        }

        public new event EventHandler<object> FirstLoadedEventHandler;

        /// <summary>
        /// base class will call this function,  never call this function by your self
        /// why: the flow is controled by base class, this is by design
        /// </summary>
        public override void FirstLoadEventInvoke()
        {
            if (FirstLoadedEventHandler != null)
            {
                FirstLoadedEventHandler(this, ListObject);

                //view model life is long, so do not store big data in the viewmodel
                ListObject = null;
            }
        }
    }
}
