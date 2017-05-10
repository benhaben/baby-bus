using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkTest
{
    /// <summary>
    /// 使用拖拽的方式自动生成model
    /// </summary>
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var db = new babybus_testEntities())
            {
                var sum = new SelectUserInformationViewWithId();
                //db.SelectUserInformationViewWithId.SelectMany()
                var info = from i in db.SelectUserInformationViewWithId
                    select i;
                sum = info.FirstOrDefault();
                Console.WriteLine(sum.ChildName);
                //db.Database.SqlQuery()
            }
        }
    }
}
