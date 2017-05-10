using System;
using UIKit;
using BabyBus.Logic.Shared;
using System.Collections.Generic;
using CoreGraphics;
using BabyBus.iOS;
using System.Linq;

namespace BabyBus.iOS
{
	public class ButtonTabView : UIControl
	{
		public List<ECCategory> _categoryList;

		public List<UIButton> _btnList = new List<UIButton>();

		public event EventHandler<int> CategoryChanged;

		public ButtonTabView(List<ECCategory> categoryList)
		{
			_categoryList = categoryList;

			BackgroundColor = MvxTouchColor.White;

			if (_categoryList != null && _categoryList.Count > 0) {
				avgWidth = 320 / _categoryList.Count;

				foreach (var category in _categoryList) {
					var btn = new UIButton();
					btn.SetTitle(category.Name, UIControlState.Normal);
					btn.Font = EasyLayout.SubTitleFont;
					btn.SetTitleColor(MvxTouchColor.Black1, UIControlState.Normal);
					btn.Tag = category.Id;
					btn.TouchUpInside += (sender, e) => {
						var curBtn = (UIButton)sender;
						foreach (var item in _btnList) {
							item.SetTitleColor(MvxTouchColor.Black1, UIControlState.Normal);
						}
						curBtn.SetTitleColor(MvxTouchColor.Blue1, UIControlState.Normal);
						if (CategoryChanged != null) {
							CategoryChanged.Invoke(null, (int)curBtn.Tag);
						}
					};
					_btnList.Add(btn);
				}
				if (_btnList.Count > 0) {
					_btnList.First().SetTitleColor(MvxTouchColor.Blue1, UIControlState.Normal);
				}
			}
		}



		nfloat avgWidth = 320f;

		UIView _container = new UIView();

		public override void LayoutSubviews()
		{
			base.LayoutSubviews();


			_container.AddSubviews(_btnList.ToArray());
			AddSubviews(_container);
			SetUpConstrainLayout();

			base.LayoutSubviews();
		}

		void SetUpConstrainLayout()
		{

			this.ConstrainLayout(
				() =>
				_container.Frame.Top == Frame.Top
				&& _container.Frame.Left == Frame.Left
				&& _container.Frame.Right == Frame.Right
				&& _container.Frame.Bottom == Frame.Bottom
			);

			for (int i = 0; i < _btnList.Count; i++) {
				if (i == 0) {
					this.ConstrainLayout(
						() =>
						_btnList[i].Frame.Top == _container.Frame.Top
						&& _btnList[i].Frame.Left == _container.Frame.Left
						&& _btnList[i].Frame.Width == avgWidth
					);
				} else {
					this.ConstrainLayout(
						() =>
						_btnList[i].Frame.Top == _container.Frame.Top
						&& _btnList[i].Frame.Left == _btnList[i - 1].Frame.Right
						&& _btnList[i].Frame.Width == avgWidth
					);
				}
			}
		}
	}
}

