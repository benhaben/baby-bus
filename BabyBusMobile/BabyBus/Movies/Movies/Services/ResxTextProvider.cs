using System.Globalization;
using System.Resources;
using System.Threading;
using Cirrious.MvvmCross.Localization;
using Cirrious.CrossCore;

namespace BabyBus.Services
{
	//if you want to use mvxlangbind, you should use this class.
	//but it's ok to use mvxbind and Viewmodel["index"]
	//The class is not use current
	public class ResxTextProvider : IMvxTextProvider
	{
		private readonly ResourceManager _resourceManager;

		public ResxTextProvider (ResourceManager resourceManager)
		{
			_resourceManager = resourceManager;
			var env = Mvx.Resolve<IEnvironmentService> ();
			CurrentLanguage = env.GetCurrentCultureInfo ();
//				Thread.CurrentThread.CurrentUICulture;
		}

		public CultureInfo CurrentLanguage { get; set; }

		public string GetText (string namespaceKey, string typeKey, string name)
		{
			string resolvedKey = name;

			if (!string.IsNullOrEmpty (typeKey)) {
				resolvedKey = string.Format ("{0}.{1}", typeKey, resolvedKey);
			}

			if (!string.IsNullOrEmpty (namespaceKey)) {
				resolvedKey = string.Format ("{0}.{1}", namespaceKey, resolvedKey);
			}

			return _resourceManager.GetString (resolvedKey, CurrentLanguage);
		}

		public string GetText (string namespaceKey, string typeKey, string name, params object[] formatArgs)
		{
			string baseText = GetText (namespaceKey, typeKey, name);

			if (string.IsNullOrEmpty (baseText)) {
				return baseText;
			}

			return string.Format (baseText, formatArgs);
		}
	}
}