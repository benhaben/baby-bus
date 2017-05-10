using System;
using System.Collections.Generic;
using System.Diagnostics;
using CoolBeans.Controls;
using Xamarin.Forms;

namespace CoolBeans.Pages.Login
{
    public class LoginHelper
    {
        public static void SetBinding(StackLayout mibaokaLayout, string entryBinding)
        {
            var t = mibaokaLayout.Children[0] as StackLayout;
            Debug.Assert(t != null, "t != null");
            t.Children[1].SetBinding(Entry.TextProperty, new Binding(entryBinding));
        }



        public static StackLayout CreateLabelDatePickerInStackLayout(string labelName, string datePickerBinding)
        {
            DatePicker datePicker = new DatePicker
            {
                Format = "D",
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Date = new DateTime(1960, 1, 1)
            };

            datePicker.SetBinding(DatePicker.DateProperty, datePickerBinding);
            var stackLayout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(15, 0, 15, 0),
                Children =
                {
                    new StackLayout
                    {
                        Spacing = 0,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = labelName,
                                Font = Font.SystemFontOfSize(NamedSize.Small),
                                TextColor = Color.White,
                                YAlign = TextAlignment.Center,
                                XAlign = TextAlignment.End,
                                HorizontalOptions = LayoutOptions.Start,
                            },
                           datePicker
                        }
                    }
                }
            };
            return stackLayout;
        }

        public static StackLayout CreateLabelEntryInStackLayout(
            string labelName
            , string entryBinding
            , bool isPassword = false
            , string entryPlaceHolder = "")
        {
            var stackLayout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(15, 0, 15, 0),
                Children =
                {
                    new StackLayout
                    {
                        Spacing = 0,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = labelName,
                                Font = Font.SystemFontOfSize(NamedSize.Small),
                                TextColor = Color.White,
                                YAlign = TextAlignment.Center,
                                XAlign = TextAlignment.End,
                                HorizontalOptions = LayoutOptions.Start,
                            },
                            new Entry
                            {
                                //HeightRequest = 10,
                                Placeholder = entryPlaceHolder,
                                HorizontalOptions = LayoutOptions.FillAndExpand,
                                IsPassword = isPassword,
                            }
                        }
                    }
                }
            };
            SetBinding(stackLayout, entryBinding);
            return stackLayout;
        }

        public static StackLayout CreateLabelPickerInStackLayout(string labelName, IList<string> citys,
            string pickBinding, string title)
        {
            //TODO:Picker is not easy to use, derive it later
            var picker = new Picker
            {
                Title = title,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            foreach (string name in citys)
            {
                picker.Items.Add(name);
            }
            picker.SetBinding(Picker.SelectedIndexProperty, pickBinding);

            var stackLayout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(15, 0, 15, 0),
                Children =
                {
                    new StackLayout
                    {
                        Spacing = 0,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            new Label
                            {
                                Text = labelName,
                                Font = Font.SystemFontOfSize(NamedSize.Small),
                                TextColor = Color.White,
                                YAlign = TextAlignment.Center,
                                XAlign = TextAlignment.End,
                                HorizontalOptions = LayoutOptions.Start,
                            },
                            picker
                        }
                    }
                }
            };
            return stackLayout;
        }

        public static ImageCell CreateImageCell(string labelName = "照片",
        string sourceProperty = "ImageSource")
        {
            var imageCell =
                new ImageCell
                {
                    Height = 100,
                    // Some differences with loading images in initial release.
                    ImageSource = ImageSource.FromFile("Icon.png"),
                    Text = "宝贝照片会显示在这里",
                };
            imageCell.SetBinding(ImageCell.ImageSourceProperty, new Binding(sourceProperty));
            //var image = new Image()
            //{
            //    HeightRequest = 200
            //};
            //image.SetBinding(Image.SourceProperty, sourceProperty);

            //var stackLayout = new StackLayout
            //{
            //    HeightRequest = 40,
            //    Spacing = 0,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    Orientation = StackOrientation.Horizontal,
            //    Padding = new Thickness(15, 0, 15, 0),
            //    Children =
            //    {
            //        new StackLayout
            //        {
            //            HeightRequest = 40,
            //            Spacing = 0,
            //            Orientation = StackOrientation.Horizontal,
            //            Children =
            //            {
            //                new Label
            //                {
            //                    Text = labelName,
            //                    Font = Font.SystemFontOfSize(NamedSize.Small),
            //                    TextColor = Color.White,
            //                    YAlign = TextAlignment.Center,
            //                    XAlign = TextAlignment.End,
            //                    HorizontalOptions = LayoutOptions.Start,
            //                },
            //                image
            //            }
            //        }
            //    }
            //};
            return imageCell;
        }

        public static StackLayout CreatePhotoModuleInStackLayout(NeedSelectImageContentPage page
            , string chooseButtonText = "宝贝照片")
        {

            var imageButton = new ImageSelectButton(page);
          
            //var chooseButton = new Button
            //{
            //    HeightRequest = 40,
            //    BorderWidth = 2,
            //    BorderRadius = 5,
            //    Font = Font.SystemFontOfSize(NamedSize.Small),
            //    Text = chooseButtonText,
            //    TextColor = Color.White,
            //};
            //chooseButton.SetBinding(Button.CommandProperty, new Binding(chooesePhotoBinding));

            //var takeButton = new Button
            //{
            //    HeightRequest = 30,
            //    BorderWidth = 2,
            //    BorderRadius = 5,
            //    Font = Font.SystemFontOfSize(NamedSize.Small),
            //    Text = takeButtonText,
            //    TextColor = Color.White
            //};
            //takeButton.SetBinding(Button.CommandProperty, new Binding(takePhotoBinding));

            var stackLayout = new StackLayout
            {
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(15, 0, 15, 0),
                Children =
                {
                    new StackLayout
                    {
                        Spacing = 0,
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            imageButton,
                            //chooseButton,
                            //takeButton,
                            new Label
                                {
                                    Text = "宝贝照片会显示在下面"
                                },
                        }
                    }
                }
            };
            return stackLayout;
        }

        public static List<string> GetSupportAreas()
        {
            var list = new List<string>
            {
                "陕西",
                "江浙沪",
                "珠三角",
                "北上广"
            };
            return list;
        }

        public static List<string> GetSupportCitys()
        {
            var list = new List<string>
            {
                "西安","宝鸡",  "咸阳", "渭南", "汉中", "安康"
            };
            return list;
        }

        public static List<string> GetSupportGardens()
        {
            var list = new List<string>
            {
                "花花幼儿园",
                "新东方幼儿园",
                "启明星幼儿园",
                "中心幼儿园"
            };
            return list;
        }

        //TODO: filter by city and garden
        public static List<string> GetSupportClassNames()
        {
            var list = new List<string>
            {
                "一班",
                "二班",
                "三班",
                "四班"
            };
            return list;
        }

        internal static IList<string> GetSupportGender()
        {
            var list = new List<string>
            {
                "女",
                "男",
            };
            return list;
        }
    } //class
} //namespace