// using Attachment = global::Architecture.Model.Attachment;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Infragistics.Win.UltraWinGrid;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using SupplierCustomer = global::Architecture.Model.Inventory.SupplierCustomer;
using ChartofAccount = global::Architecture.Model.Accounts.ChartofAccount;
using BankReconciliation = global::Architecture.Model.Accounts.BankReconciliation;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Common
{
public class DropDownBind
{
	public void BindDDLNew(DataTable dt, UltraCombo DDL, string ValueMember, string DisplayMember, string Caption, bool? ZeroIndex)
	{
		try
		{
			DataTable table = new DataTable();
			table.Columns.Add(ValueMember);
			table.Columns.Add(DisplayMember);
			foreach (DataRow row2 in dt.Rows)
			{
				table.Rows.Add(row2[ValueMember], row2[DisplayMember]);
			}
			if (ZeroIndex == true && !table.AsEnumerable().Any((DataRow row) => row[DisplayMember].ToString() == "...Select Any Value..."))
			{
				DataRow newRow = table.NewRow();
				newRow[ValueMember] = 0;
				newRow[DisplayMember] = "...Select Any Value...";
				table.Rows.InsertAt(newRow, 0);
			}
			((UltraGridBase)DDL).DataSource = table;
			((UltraDropDownBase)DDL).DisplayMember = DisplayMember;
			((UltraDropDownBase)DDL).ValueMember = ValueMember;
			UltraGridBand obj = ((UltraGridBase)DDL).DisplayLayout.Bands[0];
			obj.Columns[0].Hidden = true;
			obj.Columns[1].Hidden = false;
			((HeaderBase)obj.Columns[1].Header).Caption = Caption;
			obj.Columns[1].Width = 350;
			if (ZeroIndex == true)
			{
				DDL.Value = 0;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	public void BindDDL(DataTable dt, UltraCombo DDL, string ValueMember, string DisplayMember, string Caption, bool ZeroIndex)
	{
		try
		{
			if (ZeroIndex && dt.AsEnumerable().All((DataRow row) => row[DisplayMember].ToString() != "...Select Any Value..."))
			{
				DataRow newRow = dt.NewRow();
				newRow[ValueMember] = 0;
				foreach (DataColumn column in dt.Columns)
				{
					if (column.DataType == typeof(string))
					{
						newRow[column.ColumnName] = "...Select Any Value...";
					}
					else if (column.DataType == typeof(int) || column.DataType == typeof(double) || column.DataType == typeof(decimal))
					{
						newRow[column.ColumnName] = 0;
					}
					else if (column.DataType == typeof(DateTime))
					{
						newRow[column.ColumnName] = DateTime.Now;
					}
					else
					{
						newRow[column.ColumnName] = DBNull.Value;
					}
				}
				dt.Rows.InsertAt(newRow, 0);
			}
			((UltraGridBase)DDL).DataSource = dt;
			((UltraDropDownBase)DDL).DisplayMember = DisplayMember;
			((UltraDropDownBase)DDL).ValueMember = ValueMember;
			UltraGridBand obj = ((UltraGridBase)DDL).DisplayLayout.Bands[0];
			obj.Columns[0].Hidden = true;
			obj.Columns[1].Hidden = false;
			((HeaderBase)obj.Columns[1].Header).Caption = Caption;
			obj.Columns[1].Width = 350;
			if (ZeroIndex)
			{
				DDL.Value = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message, "Binding Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}
}
}
