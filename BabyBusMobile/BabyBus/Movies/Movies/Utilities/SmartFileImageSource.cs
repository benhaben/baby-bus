using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Xamarin.Forms;

namespace BabyBus.Utilities
{
	public class FileImageSource{}
    //TODO: make this class really smart, check picture automaticaly
    public  class SmartFileImageSource
    {
        private FileImageSource _small;
        private FileImageSource _medium;
        private FileImageSource _high;

        public SmartFileImageSource(FileImageSource high, FileImageSource medium, FileImageSource small)
        {
            _small = small;
            _medium = medium;
            _high = high;
        }

        public FileImageSource Small
        {
            get { return _small; }
            set { _small = value; }
        }

        public FileImageSource Medium
        {
            get { return _medium; }
            set { _medium = value; }
        }

        public FileImageSource High
        {
            get { return _high; }
            set { _high = value; }
        }

        public SmartFileImageSourceQulity Qulity { get; set; }
    }

    public enum SmartFileImageSourceQulity
    {
        Small,
        Medium,
        High
    }
   
}
