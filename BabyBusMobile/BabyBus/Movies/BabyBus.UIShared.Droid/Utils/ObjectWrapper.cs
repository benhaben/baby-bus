using Android.Graphics;
using Java.Lang;

namespace BabyBus.Droid.Utils
{
    public class ObjectWrapper<T> : Object
    {
        private T _managedObject;

        public ObjectWrapper(T managedObject) {
            _managedObject = managedObject;
        }

        public T Value {
            get { return _managedObject; }
            set { _managedObject = value; }
        }
    }

}