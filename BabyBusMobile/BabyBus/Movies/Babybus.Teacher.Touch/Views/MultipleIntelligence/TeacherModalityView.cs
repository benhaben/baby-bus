using BabyBus.iOS;
using UIKit;
using System;
using BabyBus.Logic.Shared;

namespace BabyBus.Views.MultipleIntelligence
{
    public class TeacherModalityView : MvxBabyBusBaseAutoLayoutViewController
    {
        TeacherModalityViewModel _baseViewModel;

        string[] imageNames =
            {
                "modality_1.png",
                "modality_2.png",
                "modality_3.png",
                "modality_4.png",
                "modality_5.png",
                "modality_6.png",
                "modality_7.png",
                "modality_8.png"
            };

        string[] titles =
            {
                "语言言语智力",
                "数理逻辑智力",
                "音乐节奏智力",
                "人际交往智力",
                "视觉空间智力",
                "身体动觉智力",
                "自知自省智力",
                "自然观察智力"
            };

        TeacherModalityButtonView[] _buttons = new TeacherModalityButtonView[8];

        public TeacherModalityButtonView[] Buttons
        {
            get
            { 
                if (_buttons[0] == null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        var button = new TeacherModalityButtonView(UIImage.FromBundle(imageNames[i]), titles[i]);
                        button.Tag = i;
                        button.TouchUpInside += (sender, e) =>
                        {
                            var index = (int)((TeacherModalityButtonView)sender).Tag;
                            //var item = _baseViewModel.TestModality[index];
                            _baseViewModel.ShowDetailCommand(index + 1);
                        };
                        _buttons[i] = button;
                    }

                }
                return _buttons;
            }
        }

        public override void OnViewDidLoad()
        {
            base.OnViewDidLoad();

            _baseViewModel = ViewModel as TeacherModalityViewModel;

            _baseViewModel.FirstLoadedEventHandler += (sender, e) => InvokeOnMainThread(() =>
                {
                    var data = _baseViewModel.TestModality;
                    for (int i = 0; i < 8; i++)
                    {
                        var modality = data[i];
                        Buttons[i].SetStatusLabel(modality.Total, modality.Completed);
                    }
                });

            _baseViewModel.MasterMessageChange += (sender, e) => InvokeOnMainThread(() =>
                {
                    for (int i = 0; i < Buttons.Length; i++)
                    {
                        var item = _baseViewModel.TestModality[i];
                        Buttons[i].SetStatusLabel(item.Total, item.Completed);
                    }
                });
        }

        public override void PrepareViewHierarchy()
        {
            UIView[] v = Buttons;
            Container.AddSubviews(v);

            base.PrepareViewHierarchy();
        }

        public override void SetUpConstrainLayout()
        {
            base.SetUpConstrainLayout();

            nfloat height = 160;
            nfloat leftMargin = 40f;
            nfloat middleMargin = 30f;

            View.ConstrainLayout(
                () =>
				Buttons[0].Frame.Top == Container.Frame.Top + EasyLayout.MarginNormal
                && Buttons[0].Frame.Left == Container.Frame.Left + leftMargin

                && Buttons[1].Frame.Top == Container.Frame.Top + EasyLayout.MarginNormal
                && Buttons[1].Frame.Left == Buttons[0].Frame.Right + middleMargin
                && Buttons[1].Frame.Right == Container.Frame.Right + EasyLayout.MarginLarge

                && Buttons[2].Frame.Top == Buttons[0].Frame.Bottom
                && Buttons[2].Frame.Left == Container.Frame.Left + leftMargin

                && Buttons[3].Frame.Top == Buttons[1].Frame.Bottom
                && Buttons[3].Frame.Left == Buttons[2].Frame.Right + middleMargin
                && Buttons[3].Frame.Right == Container.Frame.Right + EasyLayout.MarginLarge

                && Buttons[4].Frame.Top == Buttons[2].Frame.Bottom
                && Buttons[4].Frame.Left == Container.Frame.Left + leftMargin

                && Buttons[5].Frame.Top == Buttons[3].Frame.Bottom
                && Buttons[5].Frame.Left == Buttons[4].Frame.Right + middleMargin
                && Buttons[5].Frame.Right == Container.Frame.Right + EasyLayout.MarginLarge

                && Buttons[6].Frame.Top == Buttons[4].Frame.Bottom
                && Buttons[6].Frame.Left == Container.Frame.Left + leftMargin

                && Buttons[7].Frame.Top == Buttons[5].Frame.Bottom
                && Buttons[7].Frame.Left == Buttons[6].Frame.Right + middleMargin
                && Buttons[7].Frame.Right == Container.Frame.Right + EasyLayout.MarginLarge
                && Buttons[7].Frame.Bottom == Container.Frame.Bottom + EasyLayout.MarginNormal
            );
        }
    }
}

