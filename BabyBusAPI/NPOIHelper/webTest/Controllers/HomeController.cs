using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;

using System.Data;
using System.IO;
using System.Collections;

namespace Controllers.Controllers {
    public class HomeController : Controller {
        public ActionResult Index() {
            var mvcName = typeof(Controller).Assembly.GetName();
            var isMono = Type.GetType("Mono.Runtime") != null;

            ViewData["Version"] = mvcName.Version.Major + "." + mvcName.Version.Minor;
            ViewData["Runtime"] = isMono ? "Mono" : ".NET";
            ResponseExcel();

            return View();
        }
        //        public

        //        public void ResponseExcel(DataTable dt)
        //        {
        //            string excelname = System.DateTime.Now.ToString().Replace(":", "").Replace("-", "").Replace(" ", "");
        //            string filePath = System.Web.HttpContext.Current.Server.MapPath("ReadExcel") + "\\" + excelname + ".xls";
        //            MemoryStream ms = RenderDataTableToExcel(dt) as MemoryStream;
        //            FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        //            byte[] data = ms.ToArray();
        //            fs.Write(data, 0, data.Length);
        //            fs.Flush();
        //            fs.Close();
        //            data = null;
        //            ms = null;
        //            fs = null;
        //            #region 导出到客户端
        //            Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
        //            Response.AppendHeader("content-disposition", "attachment;filename=" + System.Web.HttpUtility.UrlEncode(excelname, System.Text.Encoding.UTF8) + ".xls");
        //            Response.ContentType = "Application/excel";
        //            Response.WriteFile(filePath);
        //            Response.End();
        //            #endregion
        //        }
        public  void ResponseExcel() {
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


            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            string filename = HttpUtility.UrlEncode(DateTime.Now.ToString("在线用户yyyyMMdd"));
            Response.AddHeader("Content-Disposition", "attachment;filename=" + filename + ".xls");
            Response.ContentType = "application/ms-excel";
            StarTech.NPOI.NPOIHelper.ExportExcel(dt, Response.OutputStream);
            Response.Close();

           
        }
    }
}
