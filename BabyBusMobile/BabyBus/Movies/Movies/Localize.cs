using System.Globalization;
using System.Reflection;
using System.Resources;
using BabyBus.Services;
using Cirrious.CrossCore;

namespace BabyBus {
    //if you want to get Localize string, just can use BaseViewModel["index"] or Localize.GetString
    public static class Localize {
        static CultureInfo _ci;
        static readonly IEnvironmentService Ienv;
        private static readonly ResourceManager ResManager;

        static Localize() {
            Ienv = Mvx.Resolve<IEnvironmentService>();
            ResManager = new ResourceManager("BabyBus.Resx.Resources", typeof(Localize).GetTypeInfo().Assembly);
        }

        public static string GetString(string key) {
            _ci = Ienv.GetCurrentCultureInfo();
            var result = ResManager.GetString(key, _ci);
            return result; 
        }
    }
}