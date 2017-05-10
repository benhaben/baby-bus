using Android.App;
using Cirrious.MvvmCross.ViewModels;
using Android.Content;


namespace BabyBus.Droid.Utils
{
	public class SimpleerAlertDialog
	{
		public static void SimplAlertDialog(Context context, string title, string message, string positiveButtonText, string negativeButtonText, MvxCommand positiveCommand, MvxCommand negativeCommand)
		{
			AlertDialog.Builder buider = new AlertDialog.Builder(context);
			buider.SetTitle(title);
			buider.SetMessage(message);
			buider.SetPositiveButton(positiveButtonText, ((sender, e) => {
				if (positiveCommand != null) {
					positiveCommand.Execute();
				}
			}));
			buider.SetNegativeButton(negativeButtonText, ((sender, e) => {
				if (negativeCommand != null) {
					negativeCommand.Execute();
				}
			}));
			buider.Create();
			buider.Show();
		}
	}
}

