using System;
using System.Linq;
using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Dialog.Touch;
using CrossUI.Touch.Dialog.Elements;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;
using MonoTouch.UIKit;
using CoolBeans.ViewModels.Login;
using CoolBeans.iOS.BindableElements;
using CoolBeans.Models;

namespace CoolBeans.iOS
{
	/// <summary>
	/// setting for app
	/// </summary>
	public class MineView : MvxDialogViewController
	{

		public MineView ()
			: base (UITableViewStyle.Grouped,
				null,
				false)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning ();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			UIBarButtonItem backItem = new UIBarButtonItem ();
			backItem.Title = UIConstants.RETURN_PREVIOUS_PAGE;
			this.NavigationItem.BackBarButtonItem = backItem;

			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender,args) => {
					Console.WriteLine ("RegisterCommand Execute start ");
					var vm = this.ViewModel as RegisterDetailViewModel;
					vm.RegisterCommand.Execute();
					Console.WriteLine ("RegisterCommand Execute finish");
				})
				, true);

			var bindings = this.CreateInlineBindingTarget<RegisterDetailViewModel> ();
			var birthday = new DateTime (2011, 1, 1);

			StringElement ele  = new StringElement ("Login", delegate{ 
				Console.WriteLine ("RegisterCommand Execute start ");
				var vm = this.ViewModel as RegisterDetailViewModel;
				vm.RegisterCommand.Execute();
				Console.WriteLine ("RegisterCommand Execute finish");
			}) ;

			ele.Alignment = UITextAlignment.Center;
			var rdvm = ViewModel as RegisterDetailViewModel;
			// Perform any additional setup after loading the view, typically from a nib.
			Root = new RootElement ("The Dialog") {
				new Section("绑定信息")
				{
					new EntryElement("密保卡").Bind(this, "Value Mibaoka"),
					new RadioRootElement<CityModel> ("选择城市", new RadioGroup ("选择城市", 0), rdvm.Cities) {
						new BindableSection<CityBindableRadioElement> ()
							.Bind (bindings, element => element.ItemsSource, vm => vm.Cities),
						}.Bind (bindings, e => e.EnhanceRadioSelected, vm => vm.City) as Element,

					new RadioRootElement <KindergartenModel>("选择幼儿园", new RadioGroup ("选择幼儿园", 0), rdvm.Kindergartens) {
						new BindableSection<KindergartenBindableRadioElement> ()
							.Bind (bindings, element => element.ItemsSource, vm => vm.Kindergartens),
						}.Bind (bindings, e => e.EnhanceRadioSelected, vm => vm.Kindergarten) as Element,

					new RadioRootElement <KindergartenClassModel>("选择班级", new RadioGroup ("选择班级", 0), rdvm.KindergartenClasses) {
						new BindableSection<KindergartenBindableRadioElement> ()
							.Bind (bindings, element => element.ItemsSource, vm => vm.KindergartenClasses),
						}.Bind (bindings, e => e.EnhanceRadioSelected, vm => vm.KindergartenClass) as Element,
				},
				new Section("注册信息")
				{
					new EntryElement("姓名").Bind(bindings, e => e.Value, vm => vm.ChildName),
					new EntryElement("简介").Bind(bindings, e => e.Value, vm => vm.Description),
					new RadioRootElement<GenderModel> ("选择性别", new RadioGroup ("选择性别", 0),rdvm.Genders) {
						new BindableSection<GenderBindableRadioElement> ()
							.Bind (bindings, element => element.ItemsSource, vm => vm.Genders),
						}.Bind (bindings, e => e.RadioSelected, vm => vm.Gender) as Element,

					new DateElement("生日", birthday).Bind(bindings, e => e.Value, vm => vm.Birthday),

					new PhotoElement(this)
						.Bind(bindings, e => e.Name, vm => vm.ChildName)
						.Bind(bindings, e => e.DescriptionOfChild, vm => vm.Description)
						.Bind(bindings, e => e.ChildPhotoBuffer, vm => vm.Bytes)
					},
				new Section(""){
					ele
				}
			};


		}

	}
}

