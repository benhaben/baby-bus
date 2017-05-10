using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace CoolBeans.Controls
{
    public class GridView : ContentView
    {
        public GridView()
        {
            SelectionEnabled = true;
        }


        //
        // Static Fields
        //
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(GridView), null, BindingMode.OneWay, null, null, null, null);

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create("ItemTemplate", typeof(DataTemplate), typeof(GridView), null, BindingMode.OneWay, null, null, null, null);

        public static readonly BindableProperty RowSpacingProperty = BindableProperty.Create("RowSpacing", typeof(double), typeof(GridView), (double)0, BindingMode.OneWay, null, null, null, null);

        public static readonly BindableProperty ColumnSpacingProperty = BindableProperty.Create("ColumnSpacing", typeof(double), typeof(GridView), (double)0, BindingMode.OneWay, null, null, null, null);

        public static readonly BindableProperty ItemWidthProperty = BindableProperty.Create("ItemWidth", typeof(double), typeof(GridView), (double)100, BindingMode.OneWay, null, null, null, null);

        public static readonly BindableProperty ItemHeightProperty = BindableProperty.Create("ItemHeight", typeof(double), typeof(GridView), (double)100, BindingMode.OneWay, null, null, null, null);

        //
        // Properties
        //
        public IEnumerable ItemsSource
        {
            get
            {
                return (IEnumerable)base.GetValue(GridView.ItemsSourceProperty);
            }
            set
            {
                base.SetValue(GridView.ItemsSourceProperty, value);
            }
        }

        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)base.GetValue(GridView.ItemTemplateProperty);
            }
            set
            {
                base.SetValue(GridView.ItemTemplateProperty, value);
            }
        }

        public double RowSpacing
        {
            get
            {
                return (double)base.GetValue(GridView.RowSpacingProperty);
            }
            set
            {
                base.SetValue(GridView.RowSpacingProperty, value);
            }
        }

        public double ColumnSpacing
        {
            get
            {
                return (double)base.GetValue(GridView.ColumnSpacingProperty);
            }
            set
            {
                base.SetValue(GridView.ColumnSpacingProperty, value);
            }
        }

        public double ItemWidth
        {
            get
            {
                return (double)base.GetValue(GridView.ItemWidthProperty);
            }
            set
            {
                base.SetValue(GridView.ItemWidthProperty, value);
            }
        }

        public double ItemHeight
        {
            get
            {
                return (double)base.GetValue(GridView.ItemHeightProperty);
            }
            set
            {
                base.SetValue(GridView.ItemHeightProperty, value);
            }
        }

        /// <summary>
        /// Occurs when item is selected.
        /// </summary>
        public event EventHandler<EventArgs<object>> ItemSelected;

        /// <summary>
        /// Invokes the item selected event.
        /// </summary>
        /// <param name="item">Item.</param>
        public void InvokeItemSelectedEvent(object sender, object item)
        {
            if (this.ItemSelected != null)
            {
                this.ItemSelected.Invoke(sender, new EventArgs<object>(item));
            }
        }

        public bool SelectionEnabled
        {
            get;
            set;
        }
    }
}
