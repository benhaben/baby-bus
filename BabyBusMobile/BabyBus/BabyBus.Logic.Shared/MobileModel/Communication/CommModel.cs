using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BabyBus.Logic.Shared
{
    public class CommModel<T>
    {
        public List<T> Items { get; set; }

        public string NextPageLink { get; set; }

        public long? Count { get; set; }

    }
}
