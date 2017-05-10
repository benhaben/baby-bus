using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using BabyBus.Droid.Adapters;
using BabyBus.Droid.Utils;
using BabyBus.Droid.Views.Common;
using BabyBus.Droid.Views.Common.Album;
using BabyBus.Logic.Shared;
using BabyBusSSApi.ServiceModel.Enumeration;

namespace BabyBus.Droid.Views.Communication
{
    /// <summary>
    /// Send Notice&Grow Memory View
    /// </summary>
    [Activity(Theme = "@style/CustomTheme")]
    public class SendNoticeView : ViewBase<SendNoticeViewModel>
    {
        private const int DefaultGridHeight = CustomConfig.DefaultThumbImageGridHeight;
        private SelectedImageGridAdapter adapter;
        private EditText title;
        private EditText content;
        private LinearLayout send;
        private int _times = 0;

        //Default Image Grid Height, One Column

        private GridView noScrollgridview;
        private float dp;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetCustomTitleWithBack(Resource.Layout.Page_Comm_SendNotice, GetTitleResourceId());

            NetContectStatus.registerReceiver(this);
            var netStatus = FindViewById<LinearLayout>(Resource.Id.no_net_lable);
            NetContectStatus.NetStatus(this, netStatus);

            SetTitle(GetTitleResourceId());
            title = FindViewById<EditText>(Resource.Id.notice_title_text);
            content = FindViewById<EditText>(Resource.Id.notice_content_text);
            send = FindViewById<LinearLayout>(Resource.Id.notice_sendto_layout);
            
            Init();

            dp = (float)Resources.DisplayMetrics.Density;

            ViewModel.SendImagesResultEventHandler += (successlist, failurelist) => RunOnUiThread(() =>
                {
                    if (failurelist.Count > 0)
                    {
                        _times = 0;
                        var result = string.Format("上传成功{0}张图片，失败{1}张图片。", successlist.Count, failurelist.Count);
                        new AlertDialog.Builder(this)
						.SetTitle("错误")
						.SetMessage(result)
						.SetPositiveButton("重新发送", (sender, args) =>
                            {
                                ViewModel.SendNoticeAndImage(failurelist);
                            })
						.SetNegativeButton("放弃发送", (sender, args) =>
                            {
                                this.HideInfo();
                                ImageCollection.ClearImage();
                                ViewModel.ClearDataAfterGiveUpSendNotice();

                            })
						.Show();
                    }
                    else
                    {
                        ImageCollection.ClearImage();
                    }
                });

            ViewModel.ImageHelper.SendImageProgressResultEventHandler += (uploadImageData) =>
            {
                RunOnUiThread(() =>
                    {
                        _times++;
                        AndHUD.Shared.Show(this, string.Format("正在发送第{0}张图片。", _times), -1, MaskType.None, TimeSpan.FromSeconds(50)
						, null, true, () => AndHUD.Shared.Dismiss(this));
                    });
            };
        }

        private void Init()
        {
            

            noScrollgridview = FindViewById<GridView>(Resource.Id.noScrollgridview);
            adapter = new SelectedImageGridAdapter(this);

            noScrollgridview.ItemClick += (sender, args) =>
            {
                if (args.Position == ImageCollection.BmpList.Count)
                { //Add new Image
                    ImageCollection.ClearImage();
                    var intent = new Intent(this, typeof(ImageGridView));
                    StartActivity(intent);
                }
                else
                { //Display selected Image
                    ImageCollection.Selected = args.Position;
                    var intent = new Intent(this, typeof(ImageCheckView));
                    StartActivity(intent);
                }
            };

            if (ViewModel.NoticeType == NoticeType.GrowMemory)
            {
                title.Hint = "写下这一刻的想法...";
                title.Text = DateTime.Now.ToString("D") + "宝贝们的成长记忆";
                content.Visibility = ViewStates.Gone;
                send.Visibility = ViewStates.Gone;
            }
            else
            {
                content.Hint = ViewModel.ContentHolder;
            }
        }


        protected override void OnResume()
        {
            base.OnResume();
            //Refresh Images GridView
            adapter.Update();
            noScrollgridview.Adapter = adapter;
            //ViewModel.BytesList = ImageCollection.GetBytesList();
            ViewModel.ImagesUrl = ImageCollection.PthList;
            //Autofit Grid Height
            ViewGroup.LayoutParams layoutParams = noScrollgridview.LayoutParameters;
            if (ImageCollection.Max < 4)
            { //One Column
                layoutParams.Height = Convert.ToInt32((DefaultGridHeight + 20) * dp);
                noScrollgridview.LayoutParameters = layoutParams;
            }
            else if (ImageCollection.Max < 8)
            { //Two Column
                layoutParams.Height = Convert.ToInt32((DefaultGridHeight * 2 + 25) * dp);
                noScrollgridview.LayoutParameters = layoutParams;
            }
            else
            { //Three Column
                layoutParams.Height = Convert.ToInt32((DefaultGridHeight * 3 + 30) * dp);
                noScrollgridview.LayoutParameters = layoutParams;
            }
        }

        private int GetTitleResourceId()
        {
            switch (ViewModel.NoticeType)
            {
                case NoticeType.ClassCommon:
                    return Resource.String.comm_label_sendnotice;
                case NoticeType.ClassHomework:
                    return Resource.String.comm_label_sendhomework;
                case NoticeType.GrowMemory:
                    return Resource.String.comm_label_sendphoto;
                case NoticeType.KindergartenAll:
                    return Resource.String.comm_label_kindergartenall;
                case NoticeType.KindergartenStaff:
                    return Resource.String.comm_label_kindergartenstaff;
                case NoticeType.KindergartenRecipe:
                    return Resource.String.comm_label_kindergartenrecipe;
                default:
                    return -1;
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
        }

        public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        {
            if (keyCode == Keycode.Back)
            {
                if ((ViewModel.NoticeType != NoticeType.GrowMemory && !string.IsNullOrEmpty(ViewModel.Content))
                || (ViewModel.NoticeType == NoticeType.GrowMemory && ViewModel.ImagesUrl.Count > 0))
                {
                    var dlg = new AlertDialog.Builder(this);
                    dlg.SetTitle(Resource.String.common_label_return);
                    dlg.SetMessage(Resource.String.comm_label_abortreturn);
                    dlg.SetPositiveButton(Resource.String.common_label_enter, (o, args1) =>
                        {
                            Finish();
                        });
                    dlg.SetNegativeButton(Resource.String.common_label_cancel, (o, args1) =>
                        {
                        });
                    dlg.Show();
                }
                else
                {
                    Finish();
                }
                ImageCollection.ClearImage();
                return false;
            }
            return base.OnKeyDown(keyCode, e);
		
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            NetContectStatus.unregisterReceiver(this);
        }
    }
}