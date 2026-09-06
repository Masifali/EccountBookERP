using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Xml;
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Common;

public class GridLayoutService
{
	public class ButtonClass
	{
		public string Key;

		public string Caption;

		public string Text;
	}

	public static void EnsureGridLayoutMatchesDataTable(string FormName, GridEX grd, DataTable grdTable)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		FileStream fs = CommonServices.GetGridLayout(FormName, ((Control)(object)grd).Name);
		if (fs == null)
		{
			return;
		}
		try
		{
			string fileName = fs.Name;
			GridEX grdCopy = new GridEX();
			CopyGridConfiguration(grd, grdCopy);
			grd.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			List<ButtonClass> missingButtons = GetMissingButtonColumns(grdCopy, xmlDoc);
			int dataTableColumnCount = grdTable.Columns.Count;
			if (((JanusCollectionBase)grd.RootTable.Columns).Count == dataTableColumnCount && missingButtons.Count <= 0)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(grdTable, xmlDoc);
			foreach (string column2 in missingColumns)
			{
				AddMissingColumnToLayout(xmlDoc, fileName, column2, grdTable);
			}
			foreach (ButtonClass column in missingButtons)
			{
				AddMissingButtonColumnToLayout(xmlDoc, fileName, column);
			}
			if (missingColumns.Count > 0 || missingButtons.Count > 0)
			{
				ReloadLayoutFile(FormName, grd);
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private static void CopyGridConfiguration(GridEX sourceGrid, GridEX targetGrid)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		targetGrid.DataSource = sourceGrid.DataSource;
		targetGrid.RootTable = sourceGrid.RootTable;
		targetGrid.AllowAddNew = sourceGrid.AllowAddNew;
		targetGrid.AllowEdit = sourceGrid.AllowEdit;
		targetGrid.AllowDelete = sourceGrid.AllowDelete;
		foreach (GridEXColumn item in (JanusCollectionBase)sourceGrid.RootTable.Columns)
		{
			GridEXColumn column = item;
			GridEXColumn obj = targetGrid.RootTable.Columns[column.Key];
			obj.Width = column.Width;
			obj.Visible = column.Visible;
		}
	}

	public static List<ButtonClass> GetMissingButtonColumns(GridEX grd, XmlDocument xmlDoc)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		XmlNodeList xmlNodeList = xmlDoc.SelectNodes("//Columns/*");
		HashSet<string> xmlButtonColumnIDs = new HashSet<string>();
		foreach (XmlNode node in xmlNodeList)
		{
			if (node.SelectSingleNode("ButtonStyle") != null)
			{
				string Id = node.Attributes["ID"]?.InnerText;
				if (!string.IsNullOrEmpty(Id))
				{
					xmlButtonColumnIDs.Add(Id);
				}
			}
		}
		List<ButtonClass> missingButtonColumns = new List<ButtonClass>();
		foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
		{
			GridEXColumn column = item;
			if ((int)column.ButtonStyle == 5 && !string.IsNullOrEmpty(column.ButtonText) && !xmlButtonColumnIDs.Contains(column.Key))
			{
				ButtonClass b = new ButtonClass();
				b.Key = column.Key;
				b.Caption = column.Caption;
				b.Text = column.ButtonText;
				missingButtonColumns.Add(b);
			}
		}
		return missingButtonColumns;
	}

	private static void AddMissingColumnToLayout(XmlDocument xmlDoc, string fileName, string column, DataTable grdTable)
	{
		string dataTypeString = CommonServices.GetDataTypeString(grdTable.Columns[column].DataType);
		CommonServices.AddNewColumn(xmlDoc, fileName, column, dataTypeString);
	}

	private static int GetNextColumnIndex(XmlDocument xmlDoc)
	{
		XmlNodeList columnNodes = xmlDoc.SelectNodes("//Columns/*");
		if (columnNodes != null && columnNodes.Count > 0)
		{
			return int.Parse(columnNodes[columnNodes.Count - 1].Name.Replace("Column", "")) + 1;
		}
		return 1;
	}

	private static void AddMissingButtonColumnToLayout(XmlDocument xmlDoc, string fileName, ButtonClass columnName)
	{
		XmlNode columnsNode = xmlDoc.SelectSingleNode("//Columns");
		if (columnsNode != null)
		{
			XmlElement newColumn = xmlDoc.CreateElement($"Column{GetNextColumnIndex(xmlDoc)}");
			SetColumnButtonAttributes(newColumn, columnName.Key, Regex.Replace(columnName.Caption, "([A-Z0-9]+)", " $1").Trim(), columnName.Text);
			columnsNode.AppendChild(newColumn);
			xmlDoc.Save(fileName);
		}
		else
		{
			System.Windows.MessageBox.Show("Columns node not found.");
		}
	}

	private static void SetColumnButtonAttributes(XmlElement columnElement, string id, string caption, string buttonText)
	{
		int width = buttonText.Length * 7;
		width = Math.Min(150, width);
		width = Math.Max(20, width);
		columnElement.SetAttribute("ID", id);
		AppendChildElement(columnElement, "ButtonDisplayMode", "Always");
		AppendChildElement(columnElement, "ButtonText", buttonText);
		AppendChildElement(columnElement, "ButtonStyle", "ButtonCell");
		AppendChildElement(columnElement, "Caption", caption);
		AppendChildElement(columnElement, "DataMember", id);
		AppendChildElement(columnElement, "Key", id);
		AppendChildElement(columnElement, "Position", "0");
		AppendChildElement(columnElement, "Width", width.ToString());
		AppendChildElement(columnElement, "Visible", "true");
	}

	private static void AppendChildElement(XmlElement columnElement, string name, string value)
	{
		XmlElement childElement = columnElement.OwnerDocument.CreateElement(name);
		childElement.InnerText = value;
		columnElement.AppendChild(childElement);
	}

	private static void ReloadLayoutFile(string FormName, GridEX grd)
	{
		FileStream fsNew = CommonServices.GetGridLayout(FormName, ((Control)(object)grd).Name);
		if (fsNew != null)
		{
			grd.LoadLayoutFile((Stream)fsNew);
			fsNew.Close();
			fsNew.Dispose();
		}
	}
}
