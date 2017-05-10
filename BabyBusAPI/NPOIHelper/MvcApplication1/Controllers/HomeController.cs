using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BabyBus.NPOI;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            ResponseExcel();
            return View();
        }
        public void ResponseExcel()
        {
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
           NPOIHelper.ListColumnsName = new SortedList(new NoSort());
           NPOIHelper.ListColumnsName.Add("column0", "姓名");
           NPOIHelper.ListColumnsName.Add("column1", "账号");
           NPOIHelper.ListColumnsName.Add("column3", "钱多多");


            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            string filename = HttpUtility.UrlEncode(DateTime.Now.ToString("在线用户yyyyMMdd"));
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
            Response.ContentType = "application/ms-excel";
           NPOIHelper.ExportExcel(dt, Response.OutputStream);
            Response.Close();


        }
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
