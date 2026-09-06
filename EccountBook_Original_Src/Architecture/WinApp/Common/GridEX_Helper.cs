using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Common;

public class GridEX_Helper
{
	public static void SetEditableGridFields(GridEX grid, HashSet<string> editableFields)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		try
		{
			foreach (GridEXColumn item in (JanusCollectionBase)grid.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.FilterEditType = (FilterEditType)((!(column.Type == typeof(DateTime))) ? 1 : 6);
				column.EditType = (EditType)(editableFields.Contains(column.Key) ? 1 : 0);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void SetToolTip(GridEX grid)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		GridEXRow[] dataRows = grid.GetDataRows();
		for (int i = 0; i < dataRows.Length; i++)
		{
			foreach (GridEXCell item in (IEnumerable)dataRows[i].Cells)
			{
				GridEXCell cell = item;
				if (!string.IsNullOrEmpty(cell.Text))
				{
					int approxCharFit = cell.Column.Width / 7;
					if (cell.Text.Length > approxCharFit)
					{
						cell.ToolTipText = cell.Text;
					}
					else
					{
						cell.ToolTipText = null;
					}
				}
			}
		}
	}

	public static Dictionary<int, double> GetSummedValuesByGroupKey(GridEX grid, string groupByColumnName, string valueColumnName, string filterColumnName = null, string filterColumnValue = null)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Dictionary<int, double> result = new Dictionary<int, double>();
		GridEXRow[] rows = grid.GetRows();
		foreach (GridEXRow row in rows)
		{
			if ((int)row.RowType == 0 && (string.IsNullOrEmpty(filterColumnValue) || string.IsNullOrEmpty(filterColumnName) || !(Conversion.ToString(row.Cells[filterColumnName].Value) != filterColumnValue)))
			{
				int groupKey = Conversion.ToInt(row.Cells[groupByColumnName].Value);
				double value = Conversion.ToDouble(row.Cells[valueColumnName].Value);
				if (!result.ContainsKey(groupKey))
				{
					result[groupKey] = 0.0;
				}
				result[groupKey] += value;
			}
		}
		return result;
	}

	public static void GridColumnSettings(GridEXColumn column, int DecimalCount = 0, int defaultWidth = 100, List<string> skipColumns = null, bool BracketsForamt = false, List<string> ExcludeColumnsFromTotals = null, bool ExcludeAllColumnsFromTotals = false)
	{
		try
		{
			string key = column.Key;
			key.ToLowerInvariant();
			Type colType = column.Type;
			if (skipColumns != null && skipColumns.Contains(key))
			{
				return;
			}
			bool DontSum = false;
			if (ExcludeAllColumnsFromTotals || (ExcludeColumnsFromTotals != null && ExcludeColumnsFromTotals.Contains(key)))
			{
				DontSum = true;
			}
			if (IsColumnNumeric(colType))
			{
				column.TextAlignment = (TextAlignment)3;
				if (key.Contains("Fcy") && key.Contains("Amount"))
				{
					if (!DontSum)
					{
						column.AggregateFunction = (AggregateFunction)2;
					}
					column.FormatString = (BracketsForamt ? clsGlobalVariables.stringFormatbothForFcy : clsGlobalVariables.stringFormatsingleForFcy);
					column.TotalFormatString = (BracketsForamt ? clsGlobalVariables.stringFormatbothForFcy : clsGlobalVariables.stringFormatsingleForFcy);
				}
				else if (key.Contains("Amount") || key.Contains("Freight") || key.Contains("NetPaid"))
				{
					if (!DontSum)
					{
						column.AggregateFunction = (AggregateFunction)2;
					}
					column.FormatString = (BracketsForamt ? clsGlobalVariables.stringFormatboth : clsGlobalVariables.stringFormatsingle);
					column.TotalFormatString = (BracketsForamt ? clsGlobalVariables.stringFormatboth : clsGlobalVariables.stringFormatsingle);
				}
				else if (key.Contains("Rate") && key.Contains("Fcy"))
				{
					column.FormatString = clsGlobalVariables.DecimalFCYRateFormate;
				}
				else if (key.Contains("Rate") || key.Contains("StockInHand"))
				{
					column.FormatString = clsGlobalVariables.DecimalRateFormate;
				}
				else if (key.Contains("UomQty"))
				{
					if (DecimalCount == 0)
					{
						DecimalCount = 2;
					}
					string format = "#,##0." + new string('#', DecimalCount);
					if (!DontSum)
					{
						column.AggregateFunction = (AggregateFunction)2;
					}
					column.FormatString = format;
					column.TotalFormatString = format;
				}
				else if (key.Contains("Uom") || key.Contains("PackUnit") || key.Contains("PackUom"))
				{
					string format2 = "#,##0." + new string('#', DecimalCount);
					column.FormatString = format2;
				}
				else if (key.Contains("Cash") || key.Contains("Bank") || key.Contains("Credit") || key.Contains("Debit") || key.Contains("Opening") || key.Contains("Closing") || key.Contains("Receipt") || key.Contains("Payment") || key.Contains("Profit"))
				{
					if (DecimalCount == 0)
					{
						DecimalCount = 2;
					}
					string format3 = "#,#;(#,#.##);0";
					if (!DontSum)
					{
						column.AggregateFunction = (AggregateFunction)2;
					}
					column.FormatString = format3;
					column.TotalFormatString = format3;
				}
				else
				{
					if (DecimalCount == 0)
					{
						DecimalCount = 2;
					}
					string format4 = "#,##0." + new string('#', DecimalCount);
					if (!DontSum)
					{
						column.AggregateFunction = (AggregateFunction)2;
					}
					column.FormatString = format4;
					column.TotalFormatString = format4;
				}
			}
			if (colType == typeof(int))
			{
				column.TextAlignment = (TextAlignment)2;
			}
			column.Width = GetDefaultWidth(colType, key);
		}
		catch (Exception ex)
		{
			throw new Exception("Error in GridColumnSettings: " + ex.Message, ex);
		}
	}

	private static int GetDefaultWidth(Type colType, string key)
	{
		string keyLower = key.ToLowerInvariant();
		if (colType == typeof(int))
		{
			return 60;
		}
		if (colType == typeof(DateTime))
		{
			return 80;
		}
		if (colType == typeof(float) || colType == typeof(double) || colType == typeof(decimal))
		{
			return 90;
		}
		if (colType == typeof(string))
		{
			if (keyLower.Contains("city"))
			{
				return Constants.InventoryConstants.CityName;
			}
			if (keyLower.Contains("term"))
			{
				return Constants.InventoryConstants.TermConstant;
			}
			if (keyLower.Contains("uom"))
			{
				return Constants.InventoryConstants.UomConstant;
			}
			if (keyLower.Contains("AccountTitle"))
			{
				return 150;
			}
			if (keyLower.Contains("Wip"))
			{
				return 150;
			}
			if (keyLower.Contains("name") || keyLower.Contains("description"))
			{
				return 150;
			}
		}
		return 100;
	}

	public static bool IsColumnNumeric(Type type)
	{
		if (!(type == typeof(float)) && !(type == typeof(double)))
		{
			return type == typeof(decimal);
		}
		return true;
	}

	public static void GridWrappingAndColumnSettings(GridEX grid, int headerSize, int decimalCount = 0, List<string> hiddenColumns = null, Dictionary<string, EditType> editableColumns = null, List<string> wrapColumns = null, bool ColumnSetting = true, bool BracketsForamt = false, List<string> ExcludeColumnsFromTotals = null, bool ExcludeAllColumnsFromTotals = false)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grid.DefaultFilterRowComparison = (FilterConditionOperator)7;
			grid.RootTable.HeaderLines = headerSize;
			GridEXColumnCollection columns = grid.RootTable.Columns;
			if (editableColumns != null && editableColumns.Count > 0)
			{
				grid.AllowEdit = (InheritableBoolean)1;
			}
			if (wrapColumns != null && wrapColumns.Count > 0)
			{
				grid.RootTable.RowHeight = 30;
			}
			foreach (GridEXColumn item in (JanusCollectionBase)columns)
			{
				GridEXColumn col = item;
				string colName = col.Key;
				if (hiddenColumns != null && hiddenColumns.Contains(colName))
				{
					col.Visible = false;
				}
				if (wrapColumns != null && wrapColumns.Contains(colName))
				{
					col.WordWrap = true;
				}
				if (editableColumns != null && editableColumns.Count > 0)
				{
					col.EditType = (EditType)((editableColumns != null && editableColumns.ContainsKey(colName)) ? ((int)editableColumns[colName]) : 0);
				}
				else
				{
					col.EditType = (EditType)0;
				}
				col.FilterEditType = (FilterEditType)((!(col.Type == typeof(DateTime))) ? 1 : 6);
				if (col.Type == typeof(DateTime))
				{
					col.DefaultFilterRowComparison = (FilterConditionOperator)1;
				}
				col.HeaderAlignment = (TextAlignment)2;
				if (ColumnSetting)
				{
					GridColumnSettings(col, decimalCount, 100, null, BracketsForamt, ExcludeColumnsFromTotals, ExcludeAllColumnsFromTotals);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static double GetColumnAvg(GridEX grid, string columnName)
	{
		if (grid.RowCount <= 0)
		{
			return 0.0;
		}
		return Conversion.ToDouble(grid.GetTotal(grid.RootTable.Columns[columnName], (AggregateFunction)3));
	}

	public static double GetColumnSum(GridEX grid, string columnName)
	{
		if (grid.RowCount <= 0)
		{
			return 0.0;
		}
		return Conversion.ToDouble(grid.GetTotal(grid.RootTable.Columns[columnName], (AggregateFunction)2));
	}

	public static double GetColumnSumWithWhere(GridEX grid, string columnName, string conditionColumnName, string conditionValue, bool CheckValue = false)
	{
		double total = 0.0;
		if (grid == null || string.IsNullOrWhiteSpace(columnName) || string.IsNullOrWhiteSpace(conditionColumnName))
		{
			return total;
		}
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow obj in dataRows)
		{
			GridEXCell cell = obj.Cells[conditionColumnName];
			GridEXCell targetCell = obj.Cells[columnName];
			if (cell == null || targetCell == null)
			{
				continue;
			}
			bool conditionMatched = false;
			if (CheckValue)
			{
				if (int.TryParse(cell.Value?.ToString(), out var cellVal) && int.TryParse(conditionValue, out var condVal))
				{
					conditionMatched = cellVal == condVal;
				}
			}
			else if (!string.IsNullOrEmpty(cell.Text))
			{
				conditionMatched = string.Equals(cell.Text.Trim(), conditionValue.Trim(), StringComparison.OrdinalIgnoreCase);
			}
			if (conditionMatched && double.TryParse(targetCell.Value?.ToString(), out var numericValue))
			{
				total += numericValue;
			}
		}
		return total;
	}

	public static double GetColumnSumWithoutIndex(GridEX grid, string columnName, int RowIndex, string conditionColumnName = null, string conditionValue = null)
	{
		if (grid == null)
		{
			return 0.0;
		}
		if (string.IsNullOrWhiteSpace(columnName))
		{
			return 0.0;
		}
		double total = 0.0;
		GridEXRow[] rows = grid.GetRows();
		foreach (GridEXRow row in rows)
		{
			if (row.RowIndex == RowIndex)
			{
				continue;
			}
			if (!string.IsNullOrWhiteSpace(conditionColumnName))
			{
				GridEXCell obj = row.Cells[conditionColumnName];
				if (((obj == null) ? null : obj.Value?.ToString()) != conditionValue)
				{
					continue;
				}
			}
			double num = total;
			GridEXCell obj2 = row.Cells[columnName];
			total = num + Conversion.ToDouble((obj2 != null) ? obj2.Value : null);
		}
		return total;
	}

	public static double GetColumnSumWithWhere(GridEX grid, string columnName, Dictionary<string, string> conditions)
	{
		if (grid == null)
		{
			throw new ArgumentNullException("grid");
		}
		if (conditions == null)
		{
			throw new ArgumentNullException("conditions");
		}
		double total = 0.0;
		GridEXRow[] rows = grid.GetRows();
		foreach (GridEXRow row in rows)
		{
			bool match = true;
			foreach (KeyValuePair<string, string> condition in conditions)
			{
				if (row.Cells[condition.Key].Text != condition.Value)
				{
					match = false;
					break;
				}
			}
			if (match)
			{
				total += Conversion.ToDouble(row.Cells[columnName].Value);
			}
		}
		return total;
	}

	public static void GridAutoAdjustmentNew(GridEX grd)
	{
		try
		{
			for (int index = 0; index < ((JanusCollectionBase)grd.RootTable.Columns).Count; index++)
			{
				grd.RootTable.Columns[index].AutoSize();
			}
		}
		catch (Exception ex)
		{
			throw new Exception("GridAutoAdjustment Error: " + ex.Message);
		}
	}

	public static bool IsNumeric(UpdatingCellEventArgs e)
	{
		try
		{
			decimal result;
			return decimal.TryParse(e.Value.ToString(), out result);
		}
		catch (Exception ex)
		{
			throw new Exception("IsNumeric Error: " + ex.Message);
		}
	}

	public static bool IsPositiveNumeric(UpdatingCellEventArgs e)
	{
		try
		{
			if (decimal.TryParse(e.Value.ToString(), out var result) && result >= 0m)
			{
				return true;
			}
			e.Value = 0;
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception("IsPositiveNumeric Error: " + ex.Message);
		}
	}

	public static void HideColumns(GridEX grid, List<string> columnsToHide)
	{
		foreach (string columnKey in columnsToHide)
		{
			if (grid.RootTable.Columns.Contains(columnKey))
			{
				grid.RootTable.Columns[columnKey].Visible = false;
			}
		}
	}

	public static void SortGrid(GridEX grid, string columnKey, bool ascending = true)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		if (grid.RootTable.Columns.Contains(columnKey))
		{
			SortOrder sortOrder = (SortOrder)(ascending ? 1 : 2);
			((JanusCollectionBase)grid.RootTable.SortKeys).Clear();
			grid.RootTable.SortKeys.Add(columnKey, sortOrder);
		}
	}

	public static void ComboBind(GridEXColumn column, DataTable dt, string valueMember, string displayMember, string caption, int width = 120, bool limitToList = true, bool editType = true, int position = -1)
	{
		if (column == null)
		{
			return;
		}
		column.Caption = caption;
		if (dt != null && dt.Rows.Count != 0 && dt.Columns.Contains(valueMember) && dt.Columns.Contains(displayMember))
		{
			column.ColumnType = (ColumnType)0;
			column.HasValueList = true;
			column.EditType = (EditType)(editType ? 4 : 0);
			column.LimitToList = limitToList;
			column.Width = width;
			column.ValueList.PopulateValueList((IEnumerable)dt.DefaultView, valueMember, displayMember);
			GridEX grid = column.GridEX;
			if (grid != null && position >= 0 && position < ((JanusCollectionBase)grid.RootTable.Columns).Count)
			{
				column.Position = position;
			}
		}
	}

	public static void AddButton(GridEX grid, string columnName, string caption, int width, int position = -1, string ColumnCaption = null)
	{
		if (((grid != null) ? grid.RootTable : null) == null)
		{
			throw new ArgumentException("Grid or RootTable is null.");
		}
		if (!grid.RootTable.Columns.Contains(columnName))
		{
			GridEXColumn col = grid.RootTable.Columns.Add(columnName);
			col.Key = columnName;
			col.Caption = caption;
			col.ButtonDisplayMode = (CellButtonDisplayMode)3;
			col.ButtonStyle = (ButtonStyle)5;
			col.ButtonText = (string.IsNullOrEmpty(ColumnCaption) ? caption : ColumnCaption);
			col.Width = width;
			if (position >= 0 && position < ((JanusCollectionBase)grid.RootTable.Columns).Count)
			{
				col.Position = position;
			}
		}
	}

	public static void AddSelectButton(GridEX grid, string columnName = "Select", int width = 30, int position = -1)
	{
		if (((grid != null) ? grid.RootTable : null) == null)
		{
			throw new ArgumentException("Grid or RootTable is null.");
		}
		if (!grid.RootTable.Columns.Contains(columnName))
		{
			GridEXColumn col = grid.RootTable.Columns.Add(columnName);
			col.ActAsSelector = true;
			col.UseHeaderSelector = true;
			col.Width = width;
			if (position >= 0 && position < ((JanusCollectionBase)grid.RootTable.Columns).Count)
			{
				col.Position = position;
			}
		}
	}

	public static void HandleOverheadCellUpdated(object sender, ColumnActionEventArgs e, GridEX grid)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (grid != null && grid.CurrentRow != null && (int)grid.CurrentRow.RowType == 0)
			{
				GridEXRow item = grid.CurrentRow;
				string columnKey = e.Column.Key;
				double qty = Conversion.ToDouble(item.Cells["Qty"].Value);
				double rate = Conversion.ToDouble(item.Cells["Rate"].Value);
				switch (columnKey)
				{
				case "Qty":
				case "Rate":
				{
					double amount = qty * rate;
					item.Cells["Amount"].Value = amount;
					break;
				}
				case "Amount":
				{
					double amount = Conversion.ToDouble(item.Cells["Amount"].Value);
					item.Cells["Qty"].Value = 0;
					item.Cells["Rate"].Value = 0;
					break;
				}
				}
				grid.UpdateData();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool IsAnyCellValueGreaterThanZero(GridEX grid, string columnName)
	{
		try
		{
			if (grid == null)
			{
				return false;
			}
			GridEXRow[] dataRows = grid.GetDataRows();
			for (int i = 0; i < dataRows.Length; i++)
			{
				if (Conversion.ToDouble(dataRows[i].Cells[columnName].Value) > 0.0)
				{
					return true;
				}
			}
			return false;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool IsAnyCellValueHaveSameValue(GridEX grid, string columnName, object value, int RowIndex = 0)
	{
		if (grid == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(columnName))
		{
			throw new ArgumentNullException("columnName");
		}
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow row in dataRows)
		{
			GridEXCell obj = row.Cells[columnName];
			object cellValue = ((obj != null) ? obj.Value : null);
			if ((RowIndex == -1 || row.RowIndex != RowIndex) && cellValue == null && value == null)
			{
				return false;
			}
			if (cellValue != null && decimal.TryParse(cellValue.ToString(), out var cellNumber) && decimal.TryParse(value?.ToString(), out var valueNumber) && cellNumber == valueNumber)
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsOnlyThisValueExists(GridEX grid, string columnName, object value, int excludeRowIndex = -1)
	{
		if (grid == null)
		{
			return false;
		}
		if (string.IsNullOrEmpty(columnName))
		{
			return false;
		}
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow row in dataRows)
		{
			if (excludeRowIndex == -1 || row.RowIndex != excludeRowIndex)
			{
				GridEXCell obj = row.Cells[columnName];
				object cellValue = ((obj != null) ? obj.Value : null);
				if ((cellValue == null && value != null) || (cellValue != null && !cellValue.Equals(value)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool IsAnyCellValueHaveSameValueExcept(GridEX grid, string columnName, object value, string exceptColumnName, object exceptValue)
	{
		if (grid == null)
		{
			throw new ArgumentNullException("grid");
		}
		if (string.IsNullOrEmpty(columnName))
		{
			throw new ArgumentNullException("columnName");
		}
		if (string.IsNullOrEmpty(exceptColumnName))
		{
			throw new ArgumentNullException("exceptColumnName");
		}
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow row in dataRows)
		{
			GridEXCell obj = row.Cells[exceptColumnName];
			object exceptColumnValue = ((obj != null) ? obj.Value : null);
			if ((exceptColumnValue != null || exceptValue != null) && (exceptColumnValue == null || !exceptColumnValue.Equals(exceptValue)))
			{
				GridEXCell obj2 = row.Cells[columnName];
				object cellValue = ((obj2 != null) ? obj2.Value : null);
				if ((cellValue == null && value == null) || (cellValue != null && cellValue.Equals(value)))
				{
					return true;
				}
			}
		}
		return false;
	}

	public static GridEXRow GetRowByColumnNameAndValue(GridEX grid, string IdcolumnName, int IdColumnValue)
	{
		try
		{
			GridEXRow r = null;
			GridEXRow[] dataRows = grid.GetDataRows();
			foreach (GridEXRow row in dataRows)
			{
				if (Conversion.ToInt(row.Cells[IdcolumnName].Value) == IdColumnValue)
				{
					r = row;
					break;
				}
			}
			return r;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static string GetFirstOrDefaultValueOfColumn(GridEX grid, string columnName, string valueColumnName = "", int valueColumnId = 0)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (grid == null || string.IsNullOrWhiteSpace(columnName))
		{
			return string.Empty;
		}
		try
		{
			GridEXRow[] rows = grid.GetRows();
			foreach (GridEXRow row in rows)
			{
				if ((int)row.RowType != 0)
				{
					continue;
				}
				object cellValue = row.Cells[columnName].Value;
				if (valueColumnId == 0)
				{
					if (cellValue != null)
					{
						return cellValue.ToString();
					}
				}
				else if (!string.IsNullOrWhiteSpace(valueColumnName))
				{
					object valueCell = row.Cells[valueColumnName].Value;
					if (valueCell != null && int.TryParse(valueCell.ToString(), out var id) && id == valueColumnId && cellValue != null)
					{
						return cellValue.ToString();
					}
				}
			}
			return string.Empty;
		}
		catch (Exception ex)
		{
			throw new Exception("Error in GetFirstOrDefaultValueOfColumn: " + ex.Message, ex);
		}
	}

	public static string GetAllValuesOfColumn<T>(GridEX grid, string columnName, Func<T, bool> valueFilter = null, Func<GridEXRow, bool> rowFilter = null, bool distinct = false, bool ignoreNullOrEmpty = true, string separator = ",")
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			List<T> values = new List<T>();
			GridEXRow[] rows = grid.GetRows();
			foreach (GridEXRow row in rows)
			{
				if ((int)row.RowType != 0 || (rowFilter != null && !rowFilter(row)))
				{
					continue;
				}
				GridEXCell cell = row.Cells[columnName];
				if (cell != null && cell.Value != null)
				{
					object cellValue = cell.Value;
					T value;
					try
					{
						value = ((!(cellValue is T)) ? ((T)Convert.ChangeType(cellValue, typeof(T))) : ((T)cellValue));
					}
					catch
					{
						continue;
					}
					if ((!ignoreNullOrEmpty || !string.IsNullOrWhiteSpace(Convert.ToString(value))) && (valueFilter == null || valueFilter(value)))
					{
						values.Add(value);
					}
				}
			}
			if (distinct)
			{
				values = values.Distinct().ToList();
			}
			return string.Join(separator, values);
		}
		catch (Exception ex)
		{
			throw new Exception("Error extracting column values: " + ex.Message, ex);
		}
	}

	public static void SetColumnValueForAllRows(GridEX grid, string columnName, double Value)
	{
		try
		{
			GridEXRow[] rows = grid.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				rows[i].Cells[columnName].Value = Value;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void OpenComboSelectionPopUpAndSetGridRow(DataTable dt, string valueMember, string displayMember, GridEXRow row, string GridIdColumn, string GridNameColumn = null, bool bothNameAndId = false)
	{
		try
		{
			GrdPopUp grdPopUp = new GrdPopUp(dt, valueMember, displayMember);
			grdPopUp.StartPosition = FormStartPosition.CenterScreen;
			grdPopUp.ShowDialog();
			grdPopUp.BringToFront();
			int selectedId = Conversion.ToInt((object)grdPopUp.ReturnId);
			string selectedName = Conversion.ToString((object)grdPopUp.ReturnName);
			row.Cells[GridIdColumn].Value = selectedId;
			if (bothNameAndId)
			{
				row.Cells[GridNameColumn].Value = selectedName;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static bool IsValidRow(GridEXRow row)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		if (row != null && (int)row.RowType == 0 && row.GridEX != null)
		{
			return row.GridEX.CurrentColumn != null;
		}
		return false;
	}

	public static void ChangeColorOfColumn(GridEX grd, string ColumnName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			GridEXColumn column = new GridEXColumn();
			column = grd.RootTable.Columns[ColumnName];
			GridEXFormatCondition fc = new GridEXFormatCondition(column, (ConditionOperator)2, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Red;
			grd.RootTable.FormatConditions.Add(fc);
			column = grd.RootTable.Columns[ColumnName];
			fc = new GridEXFormatCondition(column, (ConditionOperator)3, (object)0);
			fc.TargetColumn = column;
			fc.FormatStyle.FontBold = (TriState)2;
			fc.FormatStyle.ForeColor = Color.Green;
			grd.RootTable.FormatConditions.Add(fc);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AddConditionalFormatting(GridEX grid, string columnName, List<(ConditionOperator op, object value, Color foreColor, bool bold)> conditions)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		if (grid == null || string.IsNullOrEmpty(columnName))
		{
			return;
		}
		if (!grid.RootTable.Columns.Contains(columnName))
		{
			throw new ArgumentException("Column '" + columnName + "' not found.", "columnName");
		}
		try
		{
			GridEXColumn column = grid.RootTable.Columns[columnName];
			foreach (var condition in conditions)
			{
				GridEXFormatCondition val = new GridEXFormatCondition(column, condition.op, condition.value)
				{
					TargetColumn = column
				};
				val.FormatStyle.ForeColor = condition.foreColor;
				val.FormatStyle.FontBold = (TriState)((!condition.bold) ? 1 : 2);
				GridEXFormatCondition formatCondition = val;
				grid.RootTable.FormatConditions.Add(formatCondition);
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Error applying conditional formatting: " + ex.Message, ex);
		}
	}

	public static void SetColumnWidth(GridEX grid, string columnName, int width)
	{
		if (grid.RootTable.Columns.Contains(columnName))
		{
			grid.RootTable.Columns[columnName].Width = width;
		}
	}

	public static void SetColumnVisible(GridEX gridEX, string columnName, bool visible)
	{
		if (gridEX.RootTable.Columns.Contains(columnName))
		{
			gridEX.RootTable.Columns[columnName].Visible = visible;
		}
	}

	public static void CheckAllRows(GridEX grid)
	{
		try
		{
			if (grid != null && grid.GetRows().Length != 0)
			{
				GridEXRow[] rows = grid.GetRows();
				for (int i = 0; i < rows.Length; i++)
				{
					rows[i].CheckState = (RowCheckState)0;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public static void AssignGlAccountInGlGrid(bool SubsidiaryFeature, GridEX grid, UltraCombo PartyComboHavingGlId, string GlColumn, DataTable dtAccountlst, string SupplierCustomerIdColumnName, string GlAccountColumnName = "GlAccountId")
	{
		int selectedSuppId = Conversion.ToInt(PartyComboHavingGlId.Value);
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow item in dataRows)
		{
			item.BeginEdit();
			int selectedGlOrSuppId = Conversion.ToInt(item.Cells[SupplierCustomerIdColumnName].Value);
			int glAccountId = 0;
			if (SubsidiaryFeature)
			{
				glAccountId = (from r in dtAccountlst.AsEnumerable()
					where r.Field<int>("SupplierCustomerId") == selectedGlOrSuppId
					select r.Field<int>("Id")).FirstOrDefault();
				item.Cells[GlAccountColumnName].Value = glAccountId;
				if (((UltraDropDownBase)PartyComboHavingGlId).SelectedRow != null && selectedSuppId > 0 && selectedSuppId == selectedGlOrSuppId)
				{
					item.Cells[SupplierCustomerIdColumnName].Value = 0;
					item.Cells[GlAccountColumnName].Value = 0;
				}
			}
			else
			{
				glAccountId = (from r in dtAccountlst.AsEnumerable()
					where r.Field<int>("Id") == selectedGlOrSuppId
					select r.Field<int>("Id")).FirstOrDefault();
				item.Cells[GlAccountColumnName].Value = glAccountId;
				if (((UltraDropDownBase)PartyComboHavingGlId).SelectedRow != null && selectedSuppId > 0 && Conversion.ToInt(((UltraDropDownBase)PartyComboHavingGlId).SelectedRow.Cells[GlColumn].Value) == glAccountId)
				{
					item.Cells[SupplierCustomerIdColumnName].Value = 0;
					item.Cells[GlAccountColumnName].Value = 0;
				}
			}
			item.EndEdit();
		}
	}

	public static void AssignGlAccountInFreightGrid(GridEX grid, UltraCombo PartyComboHavingGlId, DataTable dtAccountlst, string SupplierCustomerIdColumnName, string GlAccountColumnName = "GlAccountId")
	{
		Conversion.ToInt(PartyComboHavingGlId.Value);
		GridEXRow[] dataRows = grid.GetDataRows();
		foreach (GridEXRow item in dataRows)
		{
			item.BeginEdit();
			int selectedAccountId = Conversion.ToInt(item.Cells[SupplierCustomerIdColumnName].Value);
			int glAccountId = 0;
			if (CommonServices.GetERPFeatureById(4))
			{
				glAccountId = (from r in dtAccountlst.AsEnumerable()
					where r.Field<int>("SupplierCustomerId") == selectedAccountId
					select r.Field<int>("Id")).FirstOrDefault();
				item.Cells[GlAccountColumnName].Value = glAccountId;
			}
			else
			{
				glAccountId = (from r in dtAccountlst.AsEnumerable()
					where r.Field<int>("Id") == selectedAccountId
					select r.Field<int>("Id")).FirstOrDefault();
			}
			item.Cells[GlAccountColumnName].Value = glAccountId;
			item.EndEdit();
		}
	}
}
