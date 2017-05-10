using Cirrious.CrossCore.Plugins;

namespace BabyBus.iOS
{
    //    public class JsonPluginBootstrap
    //        : MvxPluginBootstrapAction<Cirrious.MvvmCross.Plugins.Json.PluginLoader>
    //    {
    //    }
    public class PictureChooserPluginBootstrap
		: MvxLoaderPluginBootstrapAction<Cirrious.MvvmCross.Plugins.PictureChooser.PluginLoader, Cirrious.MvvmCross.Plugins.PictureChooser.Touch.Plugin>
    {
    }
    //		public class PictureChooserPluginBootstrap
    //		: MvxPluginBootstrapAction<Cirrious.MvvmCross.Plugins.PictureChooser.PluginLoader, PictureChooser.Plugin>
    //		{
    //		}
}