using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Helper;

public static class GridHelper
{
	public static void GetDataTableData(DataGridView dgv, DataTable dt)
	{
		string gridColName = string.Empty;
		_ = string.Empty;
		_ = string.Empty;
		int li = 0;
		try
		{
			foreach (DataRow row in dt.Rows)
			{
				foreach (DataColumn col in dt.Columns)
				{
					gridColName = col.ColumnName;
					if (dgv.Columns.Contains(gridColName))
					{
						dgv.Columns[gridColName].ValueType.ToString();
						row[col.ColumnName].GetType().ToString();
						dgv[gridColName, li].Value = row[col.ColumnName].ToString();
					}
				}
				li++;
			}
		}
		catch (Exception ex)
		{
			throw new Exception("Error in " + dgv.Columns[gridColName].ValueType.ToString() + " AND COLOUMN IS " + gridColName + ex.Message);
		}
	}

	public static List<T> SetCollectionData<T>(DataGridView dg, string MandatoryColName)
	{
		List<T> lst = new List<T>();
		foreach (DataGridViewRow gr in (IEnumerable)dg.Rows)
		{
			if (dg[MandatoryColName, gr.Index].Value == null || dg[MandatoryColName, gr.Index].Value.ToString() == string.Empty || !CheckSelectCol(dg, gr.Index))
			{
				continue;
			}
			T obj = Activator.CreateInstance<T>();
			PropertyInfo[] array = (from x in typeof(T).GetProperties()
				where !x.GetMethod.IsVirtual
				select x).ToArray();
			foreach (PropertyInfo item in array)
			{
				if (!dg.Columns.Contains(item.Name))
				{
					continue;
				}
				Type type = ((Nullable.GetUnderlyingType(item.PropertyType) == null) ? item.PropertyType : Nullable.GetUnderlyingType(item.PropertyType));
				_ = item.Name;
				object GetValue = dg[item.Name, gr.Index].Value;
				if (GetValue == null || GetValue.ToString() == string.Empty)
				{
					if (typeof(string) == type)
					{
						item.SetValue(obj, string.Empty, null);
					}
				}
				else if (typeof(bool) == type && GetValue.ToString() == "0")
				{
					item.SetValue(obj, false, null);
				}
				else
				{
					item.SetValue(obj, Convert.ChangeType(GetValue, type), null);
				}
			}
			lst.Add(obj);
		}
		return lst;
	}

	public static T SetCollectionDataByRow<T>(DataGridView dg, DataGridViewRow gr)
	{
		T obj = Activator.CreateInstance<T>();
		PropertyInfo[] array = (from x in typeof(T).GetProperties()
			where !x.GetMethod.IsVirtual
			select x).ToArray();
		foreach (PropertyInfo item in array)
		{
			if (!dg.Columns.Contains(item.Name))
			{
				continue;
			}
			Type type = ((Nullable.GetUnderlyingType(item.PropertyType) == null) ? item.PropertyType : Nullable.GetUnderlyingType(item.PropertyType));
			_ = item.Name;
			object GetValue = dg[item.Name, gr.Index].Value;
			if (GetValue == null || GetValue.ToString() == string.Empty)
			{
				if (typeof(string) == type)
				{
					item.SetValue(obj, string.Empty, null);
				}
			}
			else if (typeof(bool) == type && GetValue.ToString() == "0")
			{
				item.SetValue(obj, false, null);
			}
			else
			{
				item.SetValue(obj, Convert.ChangeType(GetValue, type), null);
			}
		}
		return obj;
	}

	public static bool CheckSelectCol(DataGridView dg, int RowIndex)
	{
		bool ReturnStatus = true;
		if (dg.Columns.Contains("Select"))
		{
			ReturnStatus = (Convert.ToBoolean(dg["Select", RowIndex].Value) ? true : false);
		}
		return ReturnStatus;
	}

	public static void GetCollectionData<T>(DataGridView dg, List<T> lst)
	{
		try
		{
			int Count = 0;
			Type typeFromHandle = typeof(T);
			string getItemName = string.Empty;
			Type getItemType = null;
			PropertyInfo[] pi = (from x in typeFromHandle.GetProperties()
				where !x.GetMethod.IsVirtual
				select x).ToArray();
			foreach (T obj in lst)
			{
				PropertyInfo[] array = pi;
				foreach (PropertyInfo obj2 in array)
				{
					getItemName = obj2.Name;
					getItemType = obj2.PropertyType;
					object getValue = obj2.GetValue(obj);
					if (dg.Columns.Contains(getItemName) && getValue != DBNull.Value)
					{
						Convert.ChangeType(getValue, getItemType);
						dg[getItemName, Count].Value = getValue;
					}
				}
				Count++;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static void Grid_Structure(DataGridView dg, Grid objGrid)
	{
		int setTotalWidthofColumns = 0;
		dg.Name = objGrid.GridName;
		dg.Columns.Clear();
		dg.AllowUserToAddRows = objGrid.IsAllowUserToAddRow;
		dg.AllowUserToDeleteRows = objGrid.IsAllowUserToDeleteRow;
		dg.AllowUserToOrderColumns = false;
		dg.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
		dg.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);
		dg.ReadOnly = false;
		dg.EnableHeadersVisualStyles = false;
		dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
		dg.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
		dg.ColumnHeadersHeight *= 2;
		dg.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
		dg.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;
		dg.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
		dg.DefaultCellStyle.ForeColor = Color.Black;
		dg.DefaultCellStyle.Font = new Font("Tahoma", 8.5f);
		dg.RowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#FFFFFF");
		dg.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#f3fbfc");
		if (objGrid.IsReadOnly)
		{
			dg.TabStop = false;
			dg.ReadOnly = true;
			dg.RowHeadersVisible = false;
			dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		}
		else if (objGrid.IsLookUpGrid)
		{
			dg.ReadOnly = true;
			dg.RowHeadersVisible = false;
			dg.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
		}
		if (objGrid.GridDetails == null)
		{
			throw new Exception("Grid Columns Detail not found");
		}
		List<GridDetail> lstGridDetail = objGrid.GridDetails.ToList();
		if (lstGridDetail == null)
		{
			throw new Exception("Grid Columns Detail not found");
		}
		lstGridDetail.FindLast((GridDetail x) => x.IsShow);
		foreach (GridDetail item in lstGridDetail)
		{
			int getColWidth = Convert.ToInt32(item.ColWidth);
			if (item.IsShow)
			{
				setTotalWidthofColumns += getColWidth;
			}
			if (item.ControlType == "CheckBoxColumn")
			{
				DataGridViewCheckBoxColumn dch = new DataGridViewCheckBoxColumn();
				dch.HeaderText = item.HeaderText;
				dch.Width = getColWidth;
				dch.Name = item.DataPropertyName;
				dch.ValueType = typeof(bool);
				dch.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				dch.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				if (item.IsShow && item.IsSelectAll)
				{
					dch.HeaderText = "";
					dg.Columns.Add(dch);
					CheckBox chk = new CheckBox();
					chk.Checked = true;
					chk.Name = item.DataPropertyName;
					chk.AutoSize = true;
					chk.BackColor = Color.Black;
					chk.Tag = dg.Name;
					Rectangle rect = dg.GetCellDisplayRectangle(0, -1, cutOverflow: true);
					chk.Size = new Size(getColWidth - 2, 20);
					rect.Offset(setTotalWidthofColumns - 3, 2);
					chk.Location = rect.Location;
					dg.Controls.Add(chk);
				}
				else
				{
					dg.Columns.Add(dch);
				}
				if (string.Equals(item.DataPropertyName, "SelectCol") && item.IsShow)
				{
					dg.AllowUserToDeleteRows = false;
				}
				if (!item.IsShow)
				{
					dch.Visible = false;
				}
				if (!item.IsResizeable)
				{
					dch.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
					dch.Resizable = DataGridViewTriState.False;
				}
				if (item.IsReadOnly)
				{
					dch.ReadOnly = true;
					dg.Columns[item.DataPropertyName].DefaultCellStyle.BackColor = Color.LightGray;
				}
				continue;
			}
			if (item.ControlType == "ButtonColumn")
			{
				DataGridViewButtonColumn grdTextBoxCol = new DataGridViewButtonColumn();
				grdTextBoxCol.DataPropertyName = item.DataPropertyName;
				grdTextBoxCol.ValueType = typeof(string);
				grdTextBoxCol.Name = item.DataPropertyName;
				grdTextBoxCol.HeaderText = item.HeaderText;
				grdTextBoxCol.Width = getColWidth;
				grdTextBoxCol.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
				grdTextBoxCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
				dg.Columns.Add(grdTextBoxCol);
				continue;
			}
			if (item.ControlType == "ComboBoxColumn")
			{
				DataGridViewComboBoxColumn grdComboBoxCol = new DataGridViewComboBoxColumn();
				grdComboBoxCol.DataPropertyName = item.DataPropertyName;
				grdComboBoxCol.ValueType = typeof(string);
				grdComboBoxCol.Name = item.DataPropertyName;
				grdComboBoxCol.HeaderText = item.HeaderText;
				grdComboBoxCol.Width = item.ColWidth;
				grdComboBoxCol.FlatStyle = FlatStyle.Flat;
				grdComboBoxCol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
				ComboBoxHelper.GridComboBoxData(item.DataPropertyName, grdComboBoxCol);
				dg.Columns.Add(grdComboBoxCol);
				continue;
			}
			DataGridViewTextBoxColumn grdTextBoxCol2 = new DataGridViewTextBoxColumn();
			grdTextBoxCol2.DataPropertyName = item.DataPropertyName;
			grdTextBoxCol2.ValueType = typeof(string);
			grdTextBoxCol2.Name = item.DataPropertyName;
			grdTextBoxCol2.HeaderText = item.HeaderText;
			grdTextBoxCol2.Width = getColWidth;
			grdTextBoxCol2.MaxInputLength = item.MaxInputLength;
			grdTextBoxCol2.DefaultCellStyle.Format = item.CellFormat;
			grdTextBoxCol2.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
			grdTextBoxCol2.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
			dg.Columns.Add(grdTextBoxCol2);
			if (item.ValueType == "Int16")
			{
				grdTextBoxCol2.ValueType = typeof(short);
				dg.Columns[item.DataPropertyName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}
			else if (item.ValueType == "Int32")
			{
				grdTextBoxCol2.ValueType = typeof(int);
				dg.Columns[item.DataPropertyName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}
			else if (item.ValueType == "Int64")
			{
				grdTextBoxCol2.ValueType = typeof(long);
				dg.Columns[item.DataPropertyName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}
			else if (item.ValueType == "Decimal")
			{
				grdTextBoxCol2.ValueType = typeof(decimal);
				dg.Columns[item.DataPropertyName].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
			}
			else if (item.ValueType == "DateTime")
			{
				grdTextBoxCol2.ValueType = typeof(DateTime);
			}
			else if (item.ValueType == "TimeSpan")
			{
				grdTextBoxCol2.ValueType = typeof(TimeSpan);
			}
			else if (item.ValueType == "Byte")
			{
				grdTextBoxCol2.ValueType = typeof(byte[]);
			}
			if (!item.IsResizeable)
			{
				grdTextBoxCol2.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
				grdTextBoxCol2.Resizable = DataGridViewTriState.False;
			}
			if (!item.IsShow)
			{
				grdTextBoxCol2.Visible = false;
			}
			if (item.IsReadOnly)
			{
				grdTextBoxCol2.ReadOnly = true;
				dg.Columns[item.DataPropertyName].DefaultCellStyle.BackColor = Color.LightGray;
			}
		}
		GridDetail lastColumn = lstGridDetail.FindLast((GridDetail x) => x.IsShow && x.ValueType != null);
		dg.Columns[lastColumn.DataPropertyName].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
	}

	public static void Grid_Reset(DataGridView dg, int InitRows = 100)
	{
		if (dg != null)
		{
			dg.Rows.Clear();
			if (dg.Columns.Count >= 0)
			{
				dg.Rows.Add(InitRows);
			}
		}
	}

	public static void Grid_ReadOnlyColoumn(DataGridView dtGrid, List<GridDetail> lstGridDetail)
	{
		try
		{
			if (dtGrid == null)
			{
				return;
			}
			foreach (GridDetail item in lstGridDetail)
			{
				if (item.IsReadOnly)
				{
					for (int i = 0; i < dtGrid.Rows.Count; i++)
					{
						dtGrid.Rows[i].Cells[item.DataPropertyName].Style.BackColor = Color.AliceBlue;
					}
				}
			}
			dtGrid.ClearSelection();
		}
		catch
		{
			throw;
		}
	}

	public static int Get_MaxLineId(DataGridView dg, int InitialRow, string MandColumName, string LineColumName)
	{
		int getMaxLineID = InitialRow;
		for (int i = 0; i <= dg.Rows.Count - 1; i++)
		{
			if (dg[MandColumName, i].Value != null && Convert.ToInt32(dg[LineColumName, i].Value) > getMaxLineID)
			{
				getMaxLineID = Convert.ToInt32(dg[LineColumName, i].Value);
			}
		}
		return getMaxLineID + 1;
	}

	public static object Grid_FindLastColumnData(DataGridView dg, string checkColumName, string getColumNameData)
	{
		object getLastRecord = null;
		for (int i = dg.Rows.Count - 1; i >= 0; i--)
		{
			if (Convert.ToInt32(dg[checkColumName, i].Value) != 0)
			{
				getLastRecord = dg[getColumNameData, i].Value;
				break;
			}
		}
		return getLastRecord;
	}

	public static int Grid_GetColumnCount(DataGridView dg)
	{
		int getColumnsCount = 0;
		foreach (DataGridViewColumn column in dg.Columns)
		{
			if (column.Visible)
			{
				getColumnsCount++;
			}
		}
		return getColumnsCount;
	}

	public static double Grid_SumOf(DataGridView dg, string SumOfColumnName, string MandColumName = "LineSno", bool isSelectCol = false)
	{
		double calcTotalAmount = 0.0;
		bool getSelectColExist = false;
		bool checkSelectColValid = false;
		if (isSelectCol)
		{
			getSelectColExist = dg.Columns.Contains("SelectCol");
		}
		for (int i = 0; i <= dg.RowCount - 1; i++)
		{
			if (getSelectColExist)
			{
				checkSelectColValid = (Convert.ToBoolean(dg["SelectCol", i].Value) ? true : false);
			}
			if ((!getSelectColExist || checkSelectColValid) && dg[MandColumName, i].Value != null && dg[MandColumName, i].Value.ToString() != string.Empty)
			{
				calcTotalAmount += Convert.ToDouble(dg[SumOfColumnName, i].Value);
			}
		}
		return calcTotalAmount;
	}

	public static int Grid_GetDuplicateRecord(DataGridView dg, string GridCheckColumName, object GridCheckDataValue, string MandColumName = "GridSno")
	{
		int getCountRecord = 0;
		if (GridCheckDataValue == null)
		{
			return 0;
		}
		if (!dg.Columns.Contains(GridCheckColumName))
		{
			throw new Exception("Check Grid Column name not exist");
		}
		for (int i = dg.Rows.Count - 1; i >= 0; i--)
		{
			if (Convert.ToInt32(dg[MandColumName, i].Value) != 0 && dg[GridCheckColumName, i].Value.ToString() == GridCheckDataValue.ToString())
			{
				getCountRecord++;
			}
		}
		return getCountRecord;
	}

	public static int Grid_FindRowIndexOfData(DataGridView dg, string checkColumName, object findColumValue)
	{
		int getLastIndex = -1;
		if (findColumValue == null)
		{
			return -1;
		}
		for (int i = dg.Rows.Count - 1; i >= 0; i--)
		{
			if (Convert.ToInt32(dg[checkColumName, i].Value) != 0 && dg[checkColumName, i].Value.ToString() == findColumValue.ToString())
			{
				getLastIndex = i;
				break;
			}
		}
		return getLastIndex;
	}

	public static int Grid_FindLastRowIndex(DataGridView dg, string checkColumName = "LineSno")
	{
		int getLastIndex = -1;
		for (int i = dg.Rows.Count - 1; i >= 0; i--)
		{
			if (dg[checkColumName, i].Value != null)
			{
				getLastIndex = i;
				break;
			}
		}
		return getLastIndex + 1;
	}

	public static void Fill_RowData(DataGridView dg, DataTable dt, int RowIndex)
	{
		if (dt == null)
		{
			return;
		}
		foreach (DataColumn dc in dt.Columns)
		{
			if (dg.Columns.Contains(dc.ColumnName))
			{
				dg[dc.ColumnName, RowIndex].Value = dt.Rows[0][dc.ColumnName];
			}
		}
	}

	public static void Reset_RowData(DataGridView dg, DataTable dt, int RowIndex)
	{
		if (dt == null)
		{
			return;
		}
		foreach (DataColumn dc in dt.Columns)
		{
			if (dg.Columns.Contains(dc.ColumnName))
			{
				dg[dc.ColumnName, RowIndex].Value = null;
			}
		}
	}

	public static void Reset_ColumnData(DataGridView dg, string ResetColumnName)
	{
		if (dg != null)
		{
			for (int i = 0; i <= dg.Rows.Count - 1; i++)
			{
				dg[ResetColumnName, i].Value = string.Empty;
			}
		}
	}

	public static void Reset_RowSno(DataGridView dg, string MandColumName)
	{
		int gridsno = 1;
		for (int i = 0; i <= dg.Rows.Count - 1; i++)
		{
			if (dg[MandColumName, i].Value != null && dg.Rows[i].Visible)
			{
				dg["LineSno", i].Value = gridsno;
				gridsno++;
			}
		}
	}

	public static double GetDoubleValue(GridEXRow row, string colName)
	{
		return Conversion.ToDouble(row.Cells[colName].Value);
	}
}
