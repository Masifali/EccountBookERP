using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Architecture.Model.Lab;
using Architecture.WinApp.Common;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.LabAnalysis.Helper_Methods;

public class LabAnalysisSchedule_Helper
{
	private static readonly Dictionary<string, string> ColumnMap = new Dictionary<string, string>
	{
		{ "Id", "LabAnalysisScheduleItemAndPartyWiseId" },
		{ "AnalysisGroupId", "LabAnalysisGroupId" },
		{ "AnalysisGroup", "LabAnalysisGroupDescription" },
		{ "AnalysisItemId", "LabAnalysisItemId" },
		{ "AnalysisItem", "LabAnalysisItemName" },
		{ "MinValue", "MinValue" },
		{ "MaxValue", "MaxValue" },
		{ "Active", "Active" }
	};

	public static void InitializeDetailTable(DataTable dt)
	{
		if (dt != null && dt.Columns.Count == 0)
		{
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("AnalysisGroupId", typeof(int));
			dt.Columns.Add("AnalysisGroup", typeof(string));
			dt.Columns.Add("AnalysisItemId", typeof(int));
			dt.Columns.Add("AnalysisItem", typeof(string));
			dt.Columns.Add("MinValue", typeof(double));
			dt.Columns.Add("MaxValue", typeof(double));
			dt.Columns.Add("Active", typeof(bool));
		}
	}

	private static void FillRow(DataRow dr, object source)
	{
		foreach (KeyValuePair<string, string> map in ColumnMap)
		{
			object sourceValue = GetValue(source, map.Value);
			dr[map.Key] = sourceValue ?? DBNull.Value;
		}
	}

	private static object GetValue(object source, string columnName)
	{
		if (source is DataRow row)
		{
			return row[columnName];
		}
		return source.GetType().GetProperty(columnName)?.GetValue(source);
	}

	public static void FillDetailFromList(LabAnalysisScheduleItemAndPartyWise list, DataTable dt)
	{
		List<LabAnalysisScheduleItemAndPartyWise> items = ((list != null) ? list.LabAnalysisScheduleItemAndPartyWiseList : null);
		if (items == null || items.Count == 0)
		{
			return;
		}
		dt.BeginLoadData();
		try
		{
			foreach (LabAnalysisScheduleItemAndPartyWise item in items)
			{
				DataRow dr = dt.NewRow();
				FillRow(dr, item);
				dt.Rows.Add(dr);
			}
		}
		finally
		{
			dt.EndLoadData();
		}
	}

	public static void FillDetailFromDataTable(DataTable dtData, DataTable dt)
	{
		if (dtData != null && dtData.Rows.Count == 0)
		{
			return;
		}
		dt.BeginLoadData();
		try
		{
			foreach (DataRow row in dtData.Rows)
			{
				DataRow dr = dt.NewRow();
				FillRow(dr, row);
				dt.Rows.Add(dr);
			}
		}
		finally
		{
			dt.EndLoadData();
		}
	}

	public static void DetailGridCommonSetting(GridEX gridEX, Dictionary<string, EditType> editableColumns = null)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "AnalysisGroupId", "AnalysisItemId" };
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 3, hiddenColumns, editableColumns);
			gridEX.RootTable.Columns["AnalysisGroup"].Width = 180;
			gridEX.RootTable.Columns["AnalysisItem"].Width = 180;
			gridEX.RootTable.Columns["MinValue"].Width = 90;
			gridEX.RootTable.Columns["MaxValue"].Width = 90;
			gridEX.RootTable.Columns["Active"].Width = 70;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}
}
