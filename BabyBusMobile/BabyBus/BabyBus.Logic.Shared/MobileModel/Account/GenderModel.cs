using System;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
	public class GenderModel
	{
		public GenderModel()
		{
		}

		public static GenderModel CreateMan()
		{
			return new GenderModel(1, "男");
		}

		public static GenderModel CreateWoman()
		{
			return  new GenderModel(2, "女");
		}

		public	GenderModel(int id, string gender)
		{
			Id = id;
			Gender = gender;
			IsSelected = false;
		}

		public int Id { get; set; }

		public string Gender { get; set; }

		private bool _isSelected;

		public bool IsSelected {
			get { return _isSelected; }
			set {
				_isSelected = value;
			}
		}

		//TODO:IOS need a property name to show value, refine here in the future
		//see RadioRootElement::EnhanceRadioSelected
		[Ignore]
		public string Name { get { return Gender; } }


		public override string ToString()
		{
			return Gender;
		}
	}
}

