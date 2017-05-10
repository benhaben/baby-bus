using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using BabyBus.Logic.Shared;

namespace BabyBus.Droid.Views.Frags.Dialog
{
	[Activity(Label = "GenderListFragement")]
	public class GenderListFragement : MvxDialogFragment
	{
		public IList<GenderModel> Genders { get; set; }

		public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
		{
			EnsureBindingContextSet(savedInstanceState);

			var view = this.BindingInflate(Resource.Layout.Dialog_Genders, null);

			var dialog = new AlertDialog.Builder(Activity);
			dialog.SetTitle(Resources.GetString(Resource.String.mine_label_childgender));
			dialog.SetView(view);
			dialog.SetNegativeButton(Resources.GetString(Resource.String.common_label_return), (s, a) => {
			});
			dialog.SetPositiveButton(Resources.GetString(Resource.String.common_label_enter), (s, a) => 
				((InfoViewModel)ViewModel).UpdateGenderCommand.Execute());
			return dialog.Create();
		}
	}
}