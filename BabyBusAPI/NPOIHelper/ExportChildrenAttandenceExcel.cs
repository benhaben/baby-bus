using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using BabyBus.NPOI;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

namespace BabyBus.NPOI
{
    public class ExportChildrenAttandenceExcel
    {
        private const string UnAttendanceDays = "缺勤天数";
        static public void Execute(DataTable dtSource, ref MemoryStream excelStream)
        {
            NPOIHelper.ListColumnsName = new SortedList(new NoSort());
            NPOIHelper.ListColumnsName.Add("Year", "年份");
            NPOIHelper.ListColumnsName.Add("Month", "月份");
            NPOIHelper.ListColumnsName.Add("KindergartenName", "幼儿园名称");
            NPOIHelper.ListColumnsName.Add("ClassName", "班级名称");
            NPOIHelper.ListColumnsName.Add("ChildName", "幼儿姓名");
            NPOIHelper.ListColumnsName.Add("UnAttendanceDays", UnAttendanceDays);
            NPOIHelper.ListColumnsName.Add("Phone", "联系电话");
            for (int i = 1; i <= 31; i++)
            {
                var c = string.Format("{0}", i);
                var cv = string.Format("{0}日", i);
                NPOIHelper.ListColumnsName.Add(c, cv);
            }
            NPOIHelper.InsertCellActionEvent += NpoiHelperOnInsertCellActionEvent;
            NPOIHelper.ExportExcel(dtSource, excelStream);
        }

        private static bool NpoiHelperOnInsertCellActionEvent(HSSFWorkbook excelWorkBook, HSSFCell newCell, string columnsName, Type colType, string value)
        {
            bool ret = true;

            if (columnsName == "UnAttendanceDays")
            {
                int intV = 0;
                int.TryParse(value, out intV);
                newCell.SetCellValue(intV.ToString());
                HSSFCellStyle style = excelWorkBook.CreateCellStyle();

                if (intV <= 0)
                {
                    style.FillForegroundColor = (HSSFColor.LIGHT_GREEN.index);
                }
                else if (intV > 0 && intV < 5)
                {
                    style.FillForegroundColor = (HSSFColor.LIGHT_YELLOW.index);
                }
                else
                {
                    style.FillForegroundColor = (HSSFColor.RED.index);
                }
                style.FillPattern = (HSSFCellStyle.SOLID_FOREGROUND);
                newCell.CellStyle = style;
                ret = true;
            }
            else if (Regex.Match(columnsName, "^\\d+$").Success)
            {
                //verify 1 and 0
                int intV = 0;
                HSSFCellStyle style = excelWorkBook.CreateCellStyle();

                if (value == "1")
                {
                    int.TryParse(value, out intV);
                    newCell.SetCellValue("出勤");
                    style.FillForegroundColor = (HSSFColor.LIGHT_GREEN.index);
                    style.FillPattern = (HSSFCellStyle.SOLID_FOREGROUND);
                    newCell.CellStyle = style;
                }
                else if (value == "0")
                {
                    int.TryParse(value, out intV);
                    newCell.SetCellValue("缺勤");
                    style.FillForegroundColor = (HSSFColor.RED.index);
                    style.FillPattern = (HSSFCellStyle.SOLID_FOREGROUND);
                    newCell.CellStyle = style;
                }
              
                ret = true;
            }
            else
            {
                ret = false;
            }
            return ret;
        }
    }
}
