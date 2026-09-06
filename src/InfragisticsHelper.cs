using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.WinApp.Helper;
using Infragistics.Shared;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{

public static class InfragisticsHelper
{
	public static void BindAndRetainSelection(UltraCombo combo, DataTable source, string valueMember, string displayMember, string headerCaption = null, bool AllColumns = true, object previousValue = null, bool insertDefaultRow = false, string defaultText = null, object defaultValue = null, bool ActivateRow = true, int activateRowIndex = 0, string previousValueColumnName = "", int displayWidth = 350)
	{
		if (combo == null || source == null || source.Rows.Count == 0)
		{
			((Control)(object)combo).Text = "";
			((UltraGridBase)combo).DataSource = null;
			return;
		}
		if (AllColumns)
		{
			BindComboAllColumns(combo, source, valueMember, displayMember, headerCaption, insertDefaultRow, defaultText, defaultValue, displayWidth);
		}
		else
		{
			BindComboTwoColumns(combo, source, valueMember, displayMember, headerCaption, insertDefaultRow, defaultText, defaultValue, displayWidth);
		}
		if (source.Rows.Count != 0)
		{
			string valueColumn = (string.IsNullOrWhiteSpace(previousValueColumnName) ? valueMember : previousValueColumnName);
			bool BindInText = !string.IsNullOrWhiteSpace(previousValueColumnName);
			RetainComboSelection(combo, source, valueColumn, previousValue, ActivateRow, activateRowIndex, BindInText);
		}
	}

	private static void BindComboAllColumns(UltraCombo combo, DataTable source, string valueMember, string displayMember, string headerCaption, bool insertDefaultRow, string defaultText, object defaultValue, int displayWidth)
	{
		DataTable bindSource = source.Copy();
		if (insertDefaultRow)
		{
			InsertDefaultRow(bindSource, valueMember, displayMember, defaultText, defaultValue);
		}
		ApplyComboBinding(combo, bindSource, valueMember, displayMember, headerCaption, displayWidth, insertDefaultRow, defaultValue);
	}

	private static void BindComboTwoColumns(UltraCombo combo, DataTable source, string valueMember, string displayMember, string headerCaption, bool insertDefaultRow, string defaultText, object defaultValue, int displayWidth)
	{
		DataTable table = new DataTable();
		table.Columns.Add(valueMember);
		table.Columns.Add(displayMember);
		foreach (DataRow row in source.Rows)
		{
			table.Rows.Add(row[valueMember], row[displayMember]);
		}
		if (insertDefaultRow)
		{
			InsertDefaultRow(table, valueMember, displayMember, defaultText, defaultValue);
		}
		ApplyComboBinding(combo, table, valueMember, displayMember, headerCaption, displayWidth, insertDefaultRow, defaultValue);
	}

	private static void ApplyComboBinding(UltraCombo combo, DataTable dataSource, string valueMember, string displayMember, string headerCaption, int displayWidth, bool setDefaultValue, object defaultValue)
	{
		((UltraGridBase)combo).DataSource = dataSource;
		((UltraDropDownBase)combo).ValueMember = valueMember;
		((UltraDropDownBase)combo).DisplayMember = displayMember;
		ColumnsCollection columns = ((UltraGridBase)combo).DisplayLayout.Bands[0].Columns;
		columns[0].Hidden = true;
		columns[1].Hidden = false;
		((HeaderBase)columns[1].Header).Caption = headerCaption ?? displayMember;
		if (displayWidth == 0)
		{
			((UltraGridBase)combo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		else
		{
			columns[1].Width = displayWidth;
		}
		if (setDefaultValue)
		{
			combo.Value = defaultValue ?? ((object)0);
		}
	}

	private static void InsertDefaultRow(DataTable table, string valueMember, string displayMember, string defaultText, object defaultValue)
	{
		DataRow newRow = table.NewRow();
		newRow[valueMember] = defaultValue ?? ((object)0);
		newRow[displayMember] = (string.IsNullOrWhiteSpace(defaultText) ? "-- Select --" : defaultText);
		table.Rows.InsertAt(newRow, 0);
	}

	private static void RetainComboSelection(UltraCombo combo, DataTable source, string valueColumn, object previousValue, bool activateRow, int activateRowIndex, bool BindInText = false)
	{
		if (combo == null || source == null || string.IsNullOrWhiteSpace(valueColumn))
		{
			((Control)(object)combo).Text = string.Empty;
			((UltraGridBase)combo).DataSource = null;
		}
		else if (previousValue == null || previousValue == DBNull.Value)
		{
			if (activateRow && ((DisposableObjectCollectionBase)((UltraGridBase)combo).Rows).Count > activateRowIndex)
			{
				((UltraGridBase)combo).Rows[activateRowIndex].Activate();
			}
			else
			{
				((Control)(object)combo).Text = string.Empty;
			}
		}
		else if (source.AsEnumerable().Any((DataRow row) => row[valueColumn] != DBNull.Value && row[valueColumn].ToString() == previousValue.ToString()))
		{
			if (BindInText)
			{
				((Control)(object)combo).Text = Conversion.ToString(previousValue);
			}
			else
			{
				combo.Value = previousValue;
			}
		}
		else if (activateRow && ((DisposableObjectCollectionBase)((UltraGridBase)combo).Rows).Count > activateRowIndex)
		{
			((UltraGridBase)combo).Rows[activateRowIndex].Activate();
		}
		else
		{
			((Control)(object)combo).Text = string.Empty;
		}
	}

	public static void ClearCombo(UltraCombo combo)
	{
		((UltraGridBase)combo).DataSource = null;
		((Control)(object)combo).Text = string.Empty;
	}

	public static void BindComboDateType(UltraCombo combo, int displayWidth = 150, object previousValue = null, bool ActivateRow = false, int activateRowIndex = 0)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Parameters");
			dt.Rows.Add(1, "This Day");
			dt.Rows.Add(2, "This Week");
			dt.Rows.Add(3, "This Month");
			dt.Rows.Add(4, "This Year");
			dt.Rows.Add(5, "Financial Year");
			BindComboAllColumns(combo, dt, "Id", "Parameters", "Date Type", insertDefaultRow: true, "Select...", 0, displayWidth);
			if (previousValue != null)
			{
				RetainComboSelection(combo, dt, "Id", previousValue, ActivateRow, activateRowIndex);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void FinancialYearMonthsComboBind(UltraCombo combo, int displayWidth = 100)
	{
		try
		{
			List<DateTime> monthListByFinancialYEarFromGlobal = GlobalVariables_Helper.GetMonthListByFinancialYEarFromGlobal();
			DataTable dt = new DataTable
			{
				Columns = { "Id", "Month" }
			};
			foreach (DateTime month in monthListByFinancialYEarFromGlobal)
			{
				dt.Rows.Add(month.ToString("MMM-yy"), month.ToString("MMM-yy"));
			}
			BindComboAllColumns(combo, dt, "Id", "Month", "Month", insertDefaultRow: false, "Select...", 0, displayWidth);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetBranchesIdsByFeature(bool BranchFeature, bool BranchFeatureConsolidated, UltraCombo combo, string ColumnNameForMultiSelect, WaitForm wf = null)
	{
		string BranchIds = "";
		try
		{
			if (BranchFeature && BranchFeatureConsolidated)
			{
				if (Conversion.ToString((object)((Control)(object)combo).Text) != string.Empty)
				{
					DataTable dt = (DataTable)((UltraGridBase)combo).DataSource;
					string[] array = (Conversion.ToString((object)((Control)(object)combo).Text) + ",").Split(',');
					foreach (string author in array)
					{
						DataRow[] dr = dt.Select(ColumnNameForMultiSelect + "='" + Conversion.ToString((object)author) + "'");
						if (dr.Length != 0)
						{
							BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
						}
					}
				}
			}
			else if (BranchFeature)
			{
				if (((UltraGridBase)combo).ActiveRow == null || Conversion.ToInt(combo.Value) == 0)
				{
					wf.Close();
					combo.Focus();
					throw new Exception("Please Select Branch first!");
				}
				BranchIds = Conversion.ToString(combo.Value);
			}
			return BranchIds;
		}
		catch (Exception ex)
		{
			BranchIds = "";
			throw new Exception(ex.Message);
		}
	}

	public static string GetSelectedIdsFromMultiSelectionCombo(string selectedText, DataTable sourceTable, string columnName, string ColumnToGetFromDt = "")
	{
		if (string.IsNullOrWhiteSpace(selectedText))
		{
			return string.Empty;
		}
		List<string> ids = new List<string>();
		string[] array = selectedText.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
		foreach (string item in array)
		{
			DataRow[] rows = sourceTable.Select(columnName + "='" + item.Trim() + "'");
			if (rows.Length != 0)
			{
				if (ColumnToGetFromDt != null && ColumnToGetFromDt != string.Empty)
				{
					ids.Add(rows[0][ColumnToGetFromDt ?? ""].ToString());
				}
				else
				{
					ids.Add(rows[0][0].ToString());
				}
			}
		}
		return string.Join(",", ids);
	}

	public static double GetColumnTotal(UltraCombo combo, string columnName)
	{
		try
		{
			if (combo == null || ((UltraGridBase)combo).DataSource == null)
			{
				return 0.0;
			}
			if (!(((UltraGridBase)combo).DataSource is DataTable dt) || !dt.Columns.Contains(columnName))
			{
				return 0.0;
			}
			object result = dt.Compute("SUM([" + columnName + "])", "");
			return (result == DBNull.Value) ? 0.0 : Conversion.ToDouble(result);
		}
		catch
		{
			return 0.0;
		}
	}

	public static double GetColumnByIdTotal(UltraCombo combo, string columnName, int id, string idColumnName)
	{
		try
		{
			if (!(((UltraGridBase)combo).DataSource is DataTable dt))
			{
				return 0.0;
			}
			string filter = $"{idColumnName} = {id}";
			object result = dt.Compute("SUM([" + columnName + "])", filter);
			return (result == DBNull.Value) ? 0.0 : Convert.ToDouble(result);
		}
		catch
		{
			return 0.0;
		}
	}

	public static void SetComboSelectedRow(UltraCombo combo, string columnName, object value)
	{
		if (combo == null || ((DisposableObjectCollectionBase)((UltraGridBase)combo).Rows).Count == 0)
		{
			return;
		}
		combo.Value = value;
		UltraGridRow selectedRow = null;
		RowEnumerator enumerator = ((UltraGridBase)combo).Rows.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				UltraGridRow row = enumerator.Current;
				if (row.Cells[columnName].Value != null && row.Cells[columnName].Value.ToString() == value.ToString())
				{
					selectedRow = row;
					break;
				}
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		if (selectedRow != null)
		{
			((UltraDropDownBase)combo).SelectedRow = selectedRow;
			((UltraGridBase)combo).ActiveRow = selectedRow;
		}
	}
}

}
