using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cirrious.MvvmCross.Plugins.Sqlite;

namespace BabyBus.Logic.Shared
{
    public class CityModel
    {
		public CityModel(){
		}
		public	CityModel(int id, string cityName){
			CityId = id;
			CityName = cityName;
		}
        [PrimaryKey, AutoIncrement]
        public int CityId { get; set; }

        public string CityName { get; set; }

        public string Name
        {
            get { return CityName; }
        }

        public override string ToString() {
            return Name;
        }
    }
}
