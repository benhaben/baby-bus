using System;
using System.Data.SqlClient;
using System.Data;
using StarTech.NPOI;
using System.Collections;

namespace TestExcel {
    class MainClass {
        public static void Main(string[] args) {
            Console.WriteLine("Hello World!");

            DataTable dt = new DataTable();
            //Method 1
            dt.Columns.Add("column0", System.Type.GetType("System.String"));
            //Method 2
            DataColumn dc = new DataColumn("column1", System.Type.GetType("System.Boolean"));
            dt.Columns.Add(dc); 
           
            dt.Columns.Add("column3", System.Type.GetType("System.String"));

            //Initialize the row
            DataRow dr = dt.NewRow();
            dr["column0"] = "AX";
            dr["column1"] = true;
            dr["column3"] = "true";
            dt.Rows.Add(dr);
            StarTech.NPOI.NPOIHelper.ListColumnsName = new SortedList(new StarTech.NPOI.NoSort());
            StarTech.NPOI.NPOIHelper.ListColumnsName.Add("column0", "姓名");
            StarTech.NPOI.NPOIHelper.ListColumnsName.Add("column1", "账号");
            StarTech.NPOI.NPOIHelper.ListColumnsName.Add("column3", "钱多多");
            NPOIHelper.ExportExcel(dt, "./test.xls");
        }
    }
}

//StarTech.NPOI.NPOIHelper.ListColumnsName = new SortedList(new StarTech.NPOI.NoSort());
//       StarTech.NPOI.NPOIHelper.ListColumnsName.Add("MemberName", "姓名");
//       StarTech.NPOI.NPOIHelper.ListColumnsName.Add("username", "账号");
//       StarTech.NPOI.NPOIHelper.ListColumnsName.Add("starttime", "登陆时间");
//       StarTech.NPOI.NPOIHelper.ListColumnsName.Add("lasttime", "在线到期时间");
//       StarTech.NPOI.NPOIHelper.ListColumnsName.Add("state", "状态");
//       Response.Clear();
//        Response.BufferOutput = false;
//        Response.ContentEncoding = System.Text.Encoding.UTF8;
//        string filename = HttpUtility.UrlEncode(DateTime.Now.ToString("在线用户yyyyMMdd"));
//        Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
//        Response.ContentType = "application/ms-excel";
//        StarTech.NPOI.NPOIHelper.ExportExcel(dtSource, Response.OutputStream);
//        Response.Close()

