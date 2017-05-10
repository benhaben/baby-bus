using System;
using UIKit;
using CrossUI.Touch.Dialog.Elements;
using System.Collections.Generic;
using Cirrious.CrossCore;

namespace BabyBus.iOS
{
    public class RadioRootElement <T> 
        : RootElement
    {

        private IList<T> _items;

        public IList<T> Items
        {
            get
            { 
                if (_items == null)
                {
                    _items = new List<T>();
                }
                return _items;
            }
            set
            { 
                _items = value;
            }
        }

        /// <summary>
        /// Initializes a RootElement that renders the summary based on the radio settings of the contained elements. 
        /// </summary>
        /// <param name="caption">
        /// The caption to ender
        /// </param>
        /// <param name="group">
        /// The group that contains the checkbox or radio information.  This is used to display
        /// the summary information when a RootElement is rendered inside a section.
        /// </param>
        public RadioRootElement(string caption, Group group)
            : base(caption, group)
        {
        }

        private T _currentItem;

        public  T EnhanceRadioSelected
        {
            get
            {
                var radio = Group as RadioGroup;
                if (radio != null)
                    return _items[radio.Selected];
                return default(T);
            }
            set
            {
                _currentItem = value;
                try
                {
                    var radioType = _currentItem.GetType();
                    var p = radioType.GetProperty("Name");
                    string name = "";
                    if (p != null)
                    {
                        name = radioType.InvokeMember("Name", System.Reflection.BindingFlags.GetProperty, null, _currentItem, null) as string;
                    }
                    Mvx.Trace(name);
                    this.CurrentAttachedCell.DetailTextLabel.Text = name;
                }
                catch (Exception e)
                {
                    Mvx.Trace(e.Message);
                }
//                try {
//                    var k = _currentItem as KindergartenModel;
//                    if (k != null) {
//                        this.CurrentAttachedCell.DetailTextLabel.Text = k.Name;
//                    }
//
//                    var kc = _currentItem as KindergartenClassModel;
//                    if (kc != null) {
//                        this.CurrentAttachedCell.DetailTextLabel.Text = kc.Name;
//                    }
//                } catch (Exception e) {
//                    Mvx.Trace(e.Message);
//                }
            }
        }

        protected override void UpdateCellDisplay(UITableViewCell cell)
        {
            base.UpdateCellDisplay(cell);
        }

        public override int RadioSelected
        {
            get
            {
                var radio = Group as RadioGroup;
                if (radio != null)
                    return radio.Selected;
                return -1;
            }
            set
            {
                #warning More needed here for two way binding!
                var radio = Group as RadioGroup;
                if (radio != null)
                {
                    radio.Selected = value;
                    //TODO: enhance here later
//                    EnhanceRadioSelected = _items[radio.Selected];
                }
                base.RadioSelected = value;
                //              var handler = RadioSelectedChanged;
                //              if (handler != null)
                //                  RadioSelectedChanged (this, EventArgs.Empty);
            }
        }

    }
}

