using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace Architecture.WinApp.Helper;

public static class ExcelHelper
{
	public static DataTable ImportExcel(string filePath)
	{
		try
		{
			DataTable dtexcel = new DataTable();
			string extension = Path.GetExtension(filePath);
			string HDR = (true ? "Yes" : "No");
			string strConn = ((!(extension == ".xlsx")) ? ("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + filePath + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\"") : ("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\""));
			using (OleDbConnection conn = new OleDbConnection(strConn))
			{
				conn.Open();
				string sheet = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[4] { null, null, null, "TABLE" }).Rows[0]["TABLE_NAME"].ToString();
				if (!sheet.EndsWith("_"))
				{
					new OleDbDataAdapter("SELECT  * FROM [" + sheet + "]", conn).Fill(dtexcel);
				}
			}
			return dtexcel;
		}
		catch (Exception ex)
		{
			throw ex;
		}
	}

	public static void ExportFile(DataTable dt, string savepath, string fileName)
	{
		Application xlApp = null;
		Workbook xlWorkBook = null;
		Worksheet xlWorkSheet = null;
		object misValue = Missing.Value;
		try
		{
			object data = null;
			int i = 0;
			int j = 0;
			xlApp = (Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
			xlWorkBook = xlApp.Workbooks.Add(misValue);
			xlWorkSheet = (Worksheet)(dynamic)xlWorkBook.Worksheets.get_Item((object)1);
			int c = 1;
			foreach (DataColumn column in dt.Columns)
			{
				data = column.ColumnName;
				xlWorkSheet.Cells[1, c] = data;
				c++;
			}
			int g = 2;
			for (i = 0; i <= dt.Rows.Count - 1; i++)
			{
				int h = 1;
				for (j = 0; j <= dt.Columns.Count - 1; j++)
				{
					data = dt.Rows[i].ItemArray[j];
					xlWorkSheet.Cells[g, h] = "'" + data;
					h++;
				}
				g++;
			}
			xlWorkBook.SaveAs(savepath + "\\" + fileName + ".xls", XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
			xlWorkBook.Close(true, misValue, misValue);
			xlApp.Quit();
			releaseObject(xlWorkSheet);
			releaseObject(xlWorkBook);
			releaseObject(xlApp);
		}
		catch (Exception ex)
		{
			releaseObject(xlWorkSheet);
			releaseObject(xlWorkBook);
			releaseObject(xlApp);
			throw ex;
		}
	}

	public static void ExportExcel(DataTable dtData, string savepath, string fileName)
	{
		Workbook wb = null;
		Worksheet ws = null;
		wb = ((Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")))).Workbooks.Add(Type.Missing);
		ws = (Worksheet)(dynamic)wb.ActiveSheet;
		for (int Idx2 = 0; Idx2 < dtData.Columns.Count; Idx2++)
		{
			_ = dtData.Columns[Idx2].DataType;
			((_Worksheet)ws).get_Range((object)"A1", Type.Missing).get_Offset((object)0, (object)Idx2).set_Value(Type.Missing, (object)dtData.Columns[Idx2].ColumnName);
		}
		ws.Cells.EntireColumn.NumberFormat = "@";
		for (int Idx = 0; Idx < dtData.Rows.Count; Idx++)
		{
			((_Worksheet)ws).get_Range((object)"A2", Type.Missing).get_Offset((object)Idx, Type.Missing).get_Resize((object)1, (object)dtData.Columns.Count)
				.set_Value(Type.Missing, (object)dtData.Rows[Idx].ItemArray);
		}
		wb.Activate();
		wb.SaveAs(savepath + "\\" + fileName + ".xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		wb.Close(Type.Missing, Type.Missing, Type.Missing);
	}

	public static IEnumerable<int> ExportExcelWithProgress(DataTable dtData, string savepath, string fileName)
	{
		Application excel = (Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
		Workbook wb = excel.Workbooks.Add(Type.Missing);
		Worksheet ws = (Worksheet)(dynamic)wb.ActiveSheet;
		for (int Idx2 = 0; Idx2 < dtData.Columns.Count; Idx2++)
		{
			((_Worksheet)ws).get_Range((object)"A1", Type.Missing).get_Offset((object)0, (object)Idx2).set_Value(Type.Missing, (object)dtData.Columns[Idx2].ColumnName);
		}
		for (int Idx = 0; Idx < dtData.Rows.Count; Idx++)
		{
			((_Worksheet)ws).get_Range((object)"A2", Type.Missing).get_Offset((object)Idx, Type.Missing).get_Resize((object)1, (object)dtData.Columns.Count)
				.set_Value(Type.Missing, (object)dtData.Rows[Idx].ItemArray);
			yield return Idx;
		}
		wb.Activate();
		wb.SaveAs(savepath + "\\" + fileName + ".xlsx", Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, XlSaveAsAccessMode.xlNoChange, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
		wb.Close(Type.Missing, Type.Missing, Type.Missing);
	}

	public static IEnumerable<int> ExportFiel(DataTable dt, string savepath, string fileName, string str)
	{
		object misValue = Missing.Value;
		Application xlApp = (Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
		Workbook xlWorkBook = xlApp.Workbooks.Add(misValue);
		Worksheet xlWorkSheet = (Worksheet)(dynamic)xlWorkBook.Worksheets.get_Item((object)1);
		int c = 1;
		foreach (DataColumn column in dt.Columns)
		{
			string data = column.ColumnName.ToString();
			xlWorkSheet.Cells[1, c] = data;
			c++;
		}
		int g = 2;
		for (int i = 0; i <= dt.Rows.Count - 1; i++)
		{
			int h = 1;
			for (int j = 0; j <= dt.Columns.Count - 1; j++)
			{
				string data = dt.Rows[i].ItemArray[j].ToString();
				xlWorkSheet.Cells[g, h] = data;
				h++;
			}
			g++;
			yield return i;
		}
		xlWorkBook.SaveAs(savepath + "\\" + fileName + ".xls", XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
		xlWorkBook.Close(true, misValue, misValue);
		xlApp.Quit();
		releaseObject(xlWorkSheet);
		releaseObject(xlWorkBook);
		releaseObject(xlApp);
	}

	public static void ExportFixedColumn(DataTable dt, string savepath, string fileName)
	{
		Application xlApp = null;
		Workbook xlWorkBook = null;
		Worksheet xlWorkSheet = null;
		object misValue = Missing.Value;
		try
		{
			string data = null;
			int i = 0;
			xlApp = (Application)Activator.CreateInstance(Marshal.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
			xlWorkBook = xlApp.Workbooks.Add(misValue);
			xlWorkSheet = (Worksheet)(dynamic)xlWorkBook.Worksheets.get_Item((object)1);
			int c = 1;
			foreach (DataColumn column in dt.Columns)
			{
				data = column.ColumnName.ToString();
				xlWorkSheet.Cells[1, c] = data;
				c++;
			}
			int g = 2;
			for (i = 0; i <= dt.Rows.Count - 1; i++)
			{
				xlWorkSheet.Cells[g, 0] = dt.Rows[i].ItemArray[0].ToString();
				xlWorkSheet.Cells[g, 1] = dt.Rows[i].ItemArray[1].ToString();
				xlWorkSheet.Cells[g, 2] = dt.Rows[i].ItemArray[2].ToString();
				xlWorkSheet.Cells[g, 3] = dt.Rows[i].ItemArray[3].ToString();
				xlWorkSheet.Cells[g, 4] = dt.Rows[i].ItemArray[4].ToString();
				xlWorkSheet.Cells[g, 5] = dt.Rows[i].ItemArray[5].ToString();
				g++;
			}
			xlWorkBook.SaveAs(savepath + "\\" + fileName + ".xls", XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
			xlWorkBook.Close(true, misValue, misValue);
			xlApp.Quit();
			releaseObject(xlWorkSheet);
			releaseObject(xlWorkBook);
			releaseObject(xlApp);
		}
		catch (Exception ex)
		{
			releaseObject(xlWorkSheet);
			releaseObject(xlWorkBook);
			releaseObject(xlApp);
			throw ex;
		}
	}

	private static void releaseObject(object obj)
	{
		try
		{
			Marshal.ReleaseComObject(obj);
			obj = null;
		}
		catch (Exception ex)
		{
			throw ex;
		}
		finally
		{
			GC.Collect();
		}
	}
}
