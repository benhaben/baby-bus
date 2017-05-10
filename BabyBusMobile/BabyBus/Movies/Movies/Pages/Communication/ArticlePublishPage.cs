using System;
using CoolBeans.Controls;
using Xamarin.Forms;

namespace CoolBeans.Pages.Communication
{
    public class ArticlePublishPage : NeedSelectImageContentPage
    {
        public ArticlePublishPage()
        {
            Title = "内容发布";

            var headerLabel = new Label();
            headerLabel.Font = Font.SystemFontOfSize(24);
            headerLabel.TextColor = Device.OnPlatform(Color.Green, Color.Yellow, Color.Yellow);
            //headerLabel.SetBinding(Label.TextProperty, new Binding("Subject", stringFormat:"  {0}"));
            headerLabel.SetValue(Label.TextColorProperty, "内容发布");

            var sendButton = new Button();
            sendButton.Text = " 发送 ";
            sendButton.VerticalOptions = LayoutOptions.EndAndExpand;
            sendButton.SetBinding(Button.CommandProperty, new Binding("SendMessageCommand"));
            if (Device.OS == TargetPlatform.WinPhone)
            {
                sendButton.BackgroundColor = Color.Green;
                sendButton.BorderColor = Color.Green;
                sendButton.TextColor = Color.White; 
            }

            var imageButton = new ImageSelectButton(this, 400,400);
            imageButton.Text = " + ";
           
            var inputBox = new Editor();
            inputBox.HorizontalOptions = LayoutOptions.FillAndExpand;
            inputBox.Keyboard = Keyboard.Chat;
            inputBox.HeightRequest = 30;
            inputBox.SetBinding(Entry.TextProperty, new Binding("InputText", BindingMode.TwoWay));

            var messageList = new ListView();
            messageList.VerticalOptions = LayoutOptions.FillAndExpand;
            messageList.SetBinding(ListView.ItemsSourceProperty, new Binding("Items"));
            messageList.ItemTemplate = new DataTemplate(CreateMessageCell);

            Content = new StackLayout
                {
                    Padding = Device.OnPlatform(new Thickness(6, 6, 6, 6), new Thickness(0), new Thickness(0)),
                    Children =
                        {
                            new StackLayout
                                {
                                    Children = {inputBox, sendButton,imageButton},
                                    Orientation = StackOrientation.Horizontal,
                                    Padding = new Thickness(0, Device.OnPlatform(0, 20, 0),0,0),
                                },
                            //headerLabel,
                            messageList,
                        }
                };
        }

        private Cell CreateMessageCell()
        {
            var timestampLabel = new Label();
            timestampLabel.SetBinding(Label.TextProperty, new Binding("Timestamp", stringFormat: "[{0:HH:mm}]"));
            timestampLabel.TextColor = Color.Silver;
            timestampLabel.Font = Font.SystemFontOfSize(14);

            var authorLabel = new Label();
            authorLabel.SetBinding(Label.TextProperty, new Binding("AuthorName", stringFormat: "{0}: "));
            authorLabel.TextColor = Device.OnPlatform(Color.Blue, Color.Yellow, Color.Yellow);
            authorLabel.Font = Font.SystemFontOfSize(14);

            var messageLabel = new Label();
            messageLabel.SetBinding(Label.TextProperty, new Binding("Content"));
            messageLabel.Font = Font.SystemFontOfSize(14);

            var stack = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    Children = { authorLabel, messageLabel }
                };

            //can be on the top
            //if (Device.Idiom == TargetIdiom.Tablet)
            //{
            //    stack.Children.Insert(0, timestampLabel);
            //}

            var view = new MessageViewCell
                {
                    View = stack
                };
            return view;
        }
    }
}
