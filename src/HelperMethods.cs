using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

public class HelperMethods
{
	public static void BulkCopyData(SqlConnection connection, string destinationTable, DataTable data, SqlTransaction transaction)
	{
		if (data != null && data.Rows.Count > 0)
		{
			using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
			{
				bulkCopy.DestinationTableName = destinationTable;
				bulkCopy.WriteToServer(data);
			}
		}
	}

	public static DataTable FilterTable(DataTable table, string foreignKeyColumn, HashSet<object> foreignKeys)
	{
		EnumerableRowCollection<DataRow> filteredRows = from row in table.AsEnumerable()
			where foreignKeys.Contains(row[foreignKeyColumn])
			select row;
		if (!filteredRows.Any())
		{
			return table.Clone();
		}
		return filteredRows.CopyToDataTable();
	}

	public static DataTable GetBatch(DataTable table, int batch, int batchSize)
	{
		return table.AsEnumerable().Skip(batch * batchSize).Take(batchSize)
			.CopyToDataTable();
	}

	public static DataTable FilterDataByActivity(DataTable source, string activityType)
	{
		EnumerableRowCollection<DataRow> filteredRows = from row in source.AsEnumerable()
			where row.Field<string>("ActivityType") == activityType
			select row;
		if (!filteredRows.Any())
		{
			return source.Clone();
		}
		return filteredRows.CopyToDataTable();
	}

	public static void BindComboBox(DataTable data, UltraCombo combo, string placeholder, int columnWidth = -1)
	{
		if (data.Rows.Count > 0)
		{
			new DropDownBind().BindDDL(data, combo, "Id", "name", placeholder, ZeroIndex: false);
			((UltraGridBase)combo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
			((UltraGridBase)combo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (columnWidth > 0)
			{
				((UltraGridBase)combo).DisplayLayout.Bands[0].Columns[1].Width = columnWidth;
			}
		}
	}

	public static DataTable CreateDataTable(string column1, string column2)
	{
		return new DataTable
		{
			Columns = 
			{
				{
					column1,
					typeof(int)
				},
				{
					column2,
					typeof(string)
				}
			}
		};
	}

	public static DataTable GetDistinctDataTable(DataTable sourceDt, string idColumnName, string textColumnName)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add(idColumnName, typeof(int));
		dt.Columns.Add(textColumnName, typeof(string));
		if (sourceDt == null || sourceDt.Rows.Count == 0)
		{
			return dt;
		}
		if (!sourceDt.Columns.Contains(idColumnName) || !sourceDt.Columns.Contains(textColumnName))
		{
			return dt;
		}
		foreach (var item in (from r in sourceDt.AsEnumerable()
			where !r.IsNull(idColumnName)
			select new
			{
				Id = r.Field<int?>(idColumnName),
				Text = r.Field<string>(textColumnName)
			} into x
			where x.Id.HasValue
			select x).Distinct().ToList())
		{
			dt.Rows.Add(item.Id.Value, item.Text);
		}
		return dt;
	}

	public static DataTable SelectAndOptionallyRenameColumns(DataTable sourceTable, Dictionary<string, string> columnMappings)
	{
		try
		{
			if (columnMappings == null || columnMappings.Count == 0)
			{
				throw new ArgumentException("columnMappings must contain at least one entry.");
			}
			DataTable result = new DataTable();
			foreach (KeyValuePair<string, string> mapping in columnMappings)
			{
				string originalName = mapping.Key;
				string newName = (string.IsNullOrWhiteSpace(mapping.Value) ? originalName : mapping.Value);
				if (!sourceTable.Columns.Contains(originalName))
				{
					throw new ArgumentException("Column '" + originalName + "' does not exist in the source table.");
				}
				result.Columns.Add(newName, sourceTable.Columns[originalName].DataType);
			}
			foreach (DataRow row in sourceTable.Rows)
			{
				object[] values = columnMappings.Select(delegate(KeyValuePair<string, string> m)
				{
					object obj = row[m.Key];
					return (obj is string text) ? text.Replace("\r", " ").Replace("\n", " ").Trim() : obj;
				}).ToArray();
				result.Rows.Add(values);
			}
			return result;
		}
		catch (Exception ex)
		{
			throw new Exception("Error selecting and optionally renaming columns: " + ex.Message, ex);
		}
	}
}

}
