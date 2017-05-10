using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CoolBeans.Controls
{
    public class MyTableView : TableView
    {
        private readonly Dictionary<string,TableSection> _tableSections;

        public 
        MyTableView(string title, TableIntent intent = TableIntent.Form)
        {
            Intent = intent;
            this.Root = new TableRoot(title);
            _tableSections = new Dictionary<string, TableSection>();
        }

        public 
        void CreateTableSection(string sectionTitle)
        {
            var tableSection = new TableSection(sectionTitle);
            _tableSections.Add(sectionTitle,tableSection);
            Root.Add(tableSection);
        }


        public 
        void AddStackLayoutInTableSection(string sectionTitle, StackLayout stackLayout)
        {
            _tableSections[sectionTitle].Add( new ViewCell
            {
                Height = 20,
                View = stackLayout
            
            });
        }

        public 
        void AddCellInTableSection(string sectionTitle, Cell view)
        {
            _tableSections[sectionTitle].Add(view);
        }
    }
}
