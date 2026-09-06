using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.SDT;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SDT;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Architecture.WinApp.SDT;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.SDT_Reports
{

public class frmExportDocumentTrackingReport : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGridI = new DataTable();

	private DataTable dtGridII = new DataTable();

	private DataTable deDetailGrid = new DataTable();

	private DataTable dtStatus = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Panel panel5;

	private Label label27;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btn335Register;

	private ToolStripButton btnRefresh;

	private GroupBox groupBox1;

	private BackgroundWorker backgroundWorker1;

	private ToolStripButton btnShortCutKeys;

	private Label label12;

	public DateTimePicker GRNfromdate;

	public DateTimePicker GRNToDate;

	internal UltraCombo cmbperemeter;

	private Button btnshow;

	private Panel panel7;

	private Panel panel8;

	internal GridEX grdII;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton BtnPrint;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	internal GridEX grdDetail;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar3;

	private RadioButton RadNotReadyDocs;

	private RadioButton RadReadyDocs;

	internal UltraCombo CmbDocument;

	private Label label3;

	internal UltraCombo CmbInvoiceNo;

	private Label label2;

	internal UltraCombo CmbCustomerName;

	private Label label1;

	private RadioButton RadReadyDate;

	private RadioButton RadDueDate;

	private Button BtnUpdateTemplateI;

	private Button BtnUpdateTemplateII;

	private Label label5;

	private Label label4;

	private GroupBox GroupBoxValuesToUpdateInGrid;

	private Button BtnUpdateInGrid;

	internal UltraCombo CmbDocmentStausToUpdate;

	private Label label6;

	public DateTimePicker txtReadyDateUpdateInGrid;

	private Label label10;

	private RadioButton RadAll;

	private RadioButton RadOnlyEmpty;

	internal UltraCombo CmbdocumentProvider;

	private Label label7;

	private RadioButton RadAllDocs;

	private ToolStripButton BtnColorForm;

	private GroupBox groupBox2;

	private Panel panel10;

	private RadioButton chkGrdViewI;

	private RadioButton chkGrdViewII;

	private GroupBox GroupColorAlert;

	private CheckBox ChkThirdLevel;

	private CheckBox ChkSecondLevel;

	private CheckBox ChkFirstLevel;

	private Label txtThirdLevelRowCount;

	private Label txtSecondLevelRowCount;

	private Label txtFirstLevelRowCount;

	private Panel panel11;

	private RadioButton RadAllInvoice;

	private RadioButton RadInvoiceDone;

	private RadioButton RadInvoicePending;

	private Label label8;

	internal UltraCombo ultraCombo1;

	public frmExportDocumentTrackingReport(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		grdDetail.FormattingRow += new RowLoadEventHandler(GrdDetail_FormattingRow);
		grd.FormattingRow += new RowLoadEventHandler(grd_FormattingRow);
		grdII.FormattingRow += new RowLoadEventHandler(grdII_FormattingRow);
	}

	private void frmExportDocumentTrackingReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 600);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			BtnUpdateTemplateI.Enabled = formright.DoHaveSaveRight;
			BtnUpdateTemplateII.Enabled = formright.DoHaveSaveRight;
			GroupBoxValuesToUpdateInGrid.Visible = formright.DoHaveSaveRight;
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Visible = false;
			ctrlGrdBar1.GroupExpand.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar2.mGridChooseFielder.Visible = false;
			ctrlGrdBar2.mGridSaveLayouts.Visible = false;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Visible = false;
			ctrlGrdBar2.GroupExpand.Visible = false;
			ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar3.mGridChooseFielder.Visible = false;
			ctrlGrdBar3.mGridSaveLayouts.Visible = false;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Visible = false;
			ctrlGrdBar3.GroupExpand.Visible = false;
			ctrlGrdBar3.removeLayoutToolStripMenuItem.Visible = false;
			tabControl1.SelectedIndex = 1;
			ParameterFill();
			ComboFill();
			StatusFill();
			btnshow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		try
		{
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("name", typeof(string));
			DataTable dtcustomGroup = dtCustomer.Clone();
			DataTable dtInvoice = dtCustomer.Clone();
			DataTable dtchartOfDocument = dtCustomer.Clone();
			DataTable dtdocumentProvider = dtCustomer.Clone();
			DataTable dtSalesPerson = dtCustomer.Clone();
			DataTable dt = ShipmentDocumentSchedule.GetDataForDropDownFromShipmentDocumentSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "customGroup")
					{
						dtcustomGroup.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "InvoiceNo")
					{
						dtInvoice.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "chartOfDocument")
					{
						dtchartOfDocument.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "documentProvider")
					{
						dtdocumentProvider.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "SalesPerson")
					{
						dtSalesPerson.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerName, "Id", "name", "Customer Name", ZeroIndex: false);
			DDL.BindDDL(dtInvoice, CmbInvoiceNo, "Id", "name", "Invoice No", ZeroIndex: false);
			DDL.BindDDL(dtchartOfDocument, CmbDocument, "Id", "name", "Document", ZeroIndex: false);
			DDL.BindDDL(dtdocumentProvider, CmbdocumentProvider, "Id", "name", "Document Provider", ZeroIndex: false);
			DDL.BindDDL(dtSalesPerson, ultraCombo1, "Id", "name", "Sales Person", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbDocmentStausToUpdate.Value);
			dtStatus.Clear();
			dtStatus = CommonServices.StaticColumnsService("ShipmentDocumentScheduleStatus");
			if (dtStatus.Rows.Count > 0)
			{
				DDL.BindDDL(dtStatus, CmbDocmentStausToUpdate, "Id", "type", "Document Status", ZeroIndex: true);
				if (ID > 0)
				{
					if (dtStatus.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbDocmentStausToUpdate.Value = ID;
					}
					else
					{
						((Control)(object)CmbDocmentStausToUpdate).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbDocmentStausToUpdate).Text = string.Empty;
				((UltraGridBase)CmbDocmentStausToUpdate).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbperemeter).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				GRNfromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				GRNfromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				GRNfromdate.Value = first;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				GRNfromdate.Value = first2;
				GRNToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				GRNfromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			FillGrids();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillGrids(bool Value = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			if (GRNfromdate.Checked)
			{
				if (RadDueDate.Checked)
				{
					grn.DueDateFrom = Conversion.ToDateTime((object)GRNfromdate.Value);
				}
				else if (RadReadyDate.Checked)
				{
					grn.ReadyDateFrom = Conversion.ToDateTime((object)GRNfromdate.Value);
				}
			}
			if (GRNToDate.Checked)
			{
				if (RadDueDate.Checked)
				{
					grn.DueDateTo = Conversion.ToDateTime((object)GRNToDate.Value);
				}
				else if (RadReadyDate.Checked)
				{
					grn.ReadyDateTo = Conversion.ToDateTime((object)GRNToDate.Value);
				}
			}
			grn.SupplierCustomerId = Conversion.ToInt(CmbCustomerName.Value);
			grn.SalesPersonId = Conversion.ToInt(ultraCombo1.Value);
			grn.ExImInvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			grn.RefDocumentTypeId = Conversion.ToInt(CmbDocument.Value);
			grn.DocumentProviderId = Conversion.ToInt(CmbdocumentProvider.Value);
			if (RadReadyDocs.Checked)
			{
				grn.SkipZero = 1;
			}
			if (RadNotReadyDocs.Checked)
			{
				grn.SkipZero = 2;
			}
			if (RadInvoicePending.Checked)
			{
				grn.ActionId = 2;
			}
			if (RadInvoiceDone.Checked)
			{
				grn.ActionId = 1;
			}
			if (tabControl1.SelectedIndex == 1)
			{
				GridTemplateIFill(grn, Value);
				GridTemplateIIFill(grn, Value);
			}
			else
			{
				GridDetailFill(grn, Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridTemplateIFill(ReportsParameters grn, bool Value = true)
	{
		try
		{
			if (Value)
			{
				dtGridI.Clear();
				dtGridI = ShipmentDocumentSchedule.ExportDocumentTrackingReportGroupWiseI(grn);
			}
			if (dtGridI.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("ShipmentDocumentScheduleId", typeof(int));
				dtcol.Columns.Add("Description", typeof(string));
				dtcol.Columns.Add("ChartOfDocumentId", typeof(int));
				dtcol.Columns.Add("DocumentName", typeof(string));
				dtcol.Columns.Add("ProviderName", typeof(string));
				dtcol.Columns.Add("DueDate", typeof(string));
				dtcol.Columns.Add("ReadyDate", typeof(string));
				dtcol.Columns.Add("DocumentStatus", typeof(int));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("PreviousReadyDate", typeof(string));
				dtcol.Columns.Add("PreviousDocumentStatus", typeof(int));
				dtcol.Columns.Add("LevelId", typeof(int));
				dtcol.Columns.Add("LevelName", typeof(string));
				dtcol.Columns.Add("ColorCode", typeof(string));
				dtcol.Columns.Add("DueDateDiffDays", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow r in dtGridI.Rows)
				{
					if ((!ChkFirstLevel.Checked && !ChkSecondLevel.Checked && !ChkThirdLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 31 && ChkFirstLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 32 && ChkSecondLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 33 && ChkThirdLevel.Checked))
					{
						DateTime ReadyDate = Conversion.ToDateTime(r["ReadyDate"]);
						DateTime dueDate = Conversion.ToDateTime(r["dueDate"]);
						dtcol.Rows.Add(r["shipmentDocumentScheduleId"], r["Description"], r["chartOfDocumentId"], r["documentName"], r["DocProviderName"], Conversion.CheckDateTimeNull(dueDate) ? "" : dueDate.ToShortDateString(), Conversion.CheckDateTimeNull(ReadyDate) ? "" : ReadyDate.ToShortDateString(), r["DocStatus"], r["Remarks"], Conversion.CheckDateTimeNull(ReadyDate) ? "" : ReadyDate.ToShortDateString(), r["DocStatus"], r["LevelId"], r["LevelName"], r["ColorCode"], r["DueDateDiffDays"]);
					}
				}
				grd.DataSource = dtcol;
				grd.RetrieveStructure();
				GridTemplateISetting();
			}
			else
			{
				grd.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridTemplateISetting()
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowEdit = (InheritableBoolean)1;
			grd.RootTable.Columns["shipmentDocumentScheduleId"].Visible = false;
			grd.RootTable.Columns["ChartOfDocumentId"].Visible = false;
			grd.RootTable.Columns["PreviousReadyDate"].Visible = false;
			grd.RootTable.Columns["PreviousDocumentStatus"].Visible = false;
			grd.RootTable.Columns["LevelId"].Visible = false;
			grd.RootTable.Columns["LevelName"].Visible = false;
			grd.RootTable.Columns["ColorCode"].Visible = false;
			grd.RootTable.Columns["DueDateDiffDays"].Visible = false;
			grd.RootTable.Groups.Add("Description");
			grd.RootTable.Columns["Description"].Visible = false;
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)((!item.Key.Contains("Date")) ? 1 : 6);
				item.HeaderAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["DocumentStatus"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["DocumentStatus"].HasValueList = true;
			grd.RootTable.Columns["DocumentStatus"].LimitToList = true;
			grd.RootTable.Columns["DocumentStatus"].ValueList.PopulateValueList((IEnumerable)dtStatus.DefaultView, "Id", "type");
			grd.RootTable.Columns["DocumentStatus"].EditType = (EditType)(formright.DoHaveSaveRight ? 4 : 0);
			grd.RootTable.Columns["ReadyDate"].EditType = (EditType)(formright.DoHaveSaveRight ? 2 : 0);
			grd.RootTable.Columns["DocumentName"].Width = 120;
			grd.RootTable.Columns["ProviderName"].Width = 105;
			grd.RootTable.Columns["DueDate"].Width = 80;
			grd.RootTable.Columns["ReadyDate"].Width = 80;
			grd.RootTable.Columns["DocumentStatus"].Width = 85;
			grd.RootTable.Columns["Remarks"].Width = 120;
			grd.RootTable.Columns["Description"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			_ = e.Column.Key == "Print";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				Conversion.ToInt(item.Cells["Id"].Value);
				_ = e.Column.Key == "Description";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		try
		{
			if (grd.RowCount <= 0 || (int)e.Row.RowType != 0 || e.Row.Cells.Count <= 0)
			{
				return;
			}
			object value = e.Row.Cells["DueDate"].Value;
			string colorCode = e.Row.Cells["ColorCode"].Value?.ToString();
			if (value != null && !string.IsNullOrWhiteSpace(colorCode))
			{
				if (e.Row.Cells["DueDate"].FormatStyle == null)
				{
					e.Row.Cells["DueDate"].FormatStyle = new GridEXFormatStyle();
				}
				e.Row.Cells["DueDate"].FormatStyle.ForeColor = ColorTranslator.FromHtml(colorCode);
				e.Row.Cells["DueDate"].FormatStyle.FontBold = (TriState)2;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridTemplateIIFill(ReportsParameters grn, bool Value = true)
	{
		try
		{
			if (Value)
			{
				dtGridII.Clear();
				dtGridII = ShipmentDocumentSchedule.ExportDocumentTrackingReportGroupWiseII(grn);
			}
			if (dtGridII.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("shipmentDocumentScheduleId", typeof(int));
				dtcol.Columns.Add("Description", typeof(string));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("ScheduleCode", typeof(string));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("InvoiceDate", typeof(string));
				dtcol.Columns.Add("DueDate", typeof(string));
				dtcol.Columns.Add("ReadyDate", typeof(string));
				dtcol.Columns.Add("DocumentStatus", typeof(int));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("PreviousReadyDate", typeof(string));
				dtcol.Columns.Add("PreviousDocumentStatus", typeof(int));
				dtcol.Columns.Add("LevelId", typeof(int));
				dtcol.Columns.Add("LevelName", typeof(string));
				dtcol.Columns.Add("ColorCode", typeof(string));
				dtcol.Columns.Add("DueDateDiffDays", typeof(int));
				dtcol.Rows.Clear();
				ChkFirstLevel.Text = "";
				ChkSecondLevel.Text = "";
				ChkThirdLevel.Text = "";
				int Level1 = 0;
				int Level2 = 0;
				int Level3 = 0;
				foreach (DataRow r in dtGridII.Rows)
				{
					if ((!ChkFirstLevel.Checked && !ChkSecondLevel.Checked && !ChkThirdLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 31 && ChkFirstLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 32 && ChkSecondLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 33 && ChkThirdLevel.Checked))
					{
						DateTime ReadyDate = Conversion.ToDateTime(r["ReadyDate"]);
						DateTime InvoiceDate = Conversion.ToDateTime(r["InvoiceDate"]);
						DateTime dueDate = Conversion.ToDateTime(r["dueDate"]);
						dtcol.Rows.Add(r["shipmentDocumentScheduleId"], r["Description"], r["CustomerName"], r["ScheduleCode"], r["InvoiceNo"], Conversion.CheckDateTimeNull(InvoiceDate) ? "" : InvoiceDate.ToShortDateString(), Conversion.CheckDateTimeNull(dueDate) ? "" : dueDate.ToShortDateString(), Conversion.CheckDateTimeNull(ReadyDate) ? "" : ReadyDate.ToShortDateString(), r["DocStatus"], r["Remarks"], Conversion.CheckDateTimeNull(ReadyDate) ? "" : ReadyDate.ToShortDateString(), r["DocStatus"], r["LevelId"], r["LevelName"], r["ColorCode"], r["DueDateDiffDays"]);
					}
					Level1 = ((Conversion.ToInt(r["LevelId"]) == 31) ? (Level1 + 1) : Level1);
					Level2 = ((Conversion.ToInt(r["LevelId"]) == 32) ? (Level2 + 1) : Level2);
					Level3 = ((Conversion.ToInt(r["LevelId"]) == 33) ? (Level3 + 1) : Level3);
					if (string.IsNullOrEmpty(ChkFirstLevel.Text) && Conversion.ToInt(r["LevelId"]) == 31)
					{
						ChkFirstLevel.Text = Conversion.ToString(r["LevelName"]);
						ChkFirstLevel.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r["ColorCode"]));
					}
					if (string.IsNullOrEmpty(ChkSecondLevel.Text) && Conversion.ToInt(r["LevelId"]) == 32)
					{
						ChkSecondLevel.Text = Conversion.ToString(r["LevelName"]);
						ChkSecondLevel.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r["ColorCode"]));
					}
					if (string.IsNullOrEmpty(ChkThirdLevel.Text) && Conversion.ToInt(r["LevelId"]) == 33)
					{
						ChkThirdLevel.Text = Conversion.ToString(r["LevelName"]);
						ChkThirdLevel.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r["ColorCode"]));
					}
				}
				if (string.IsNullOrEmpty(ChkFirstLevel.Text))
				{
					ChkFirstLevel.Text = "First";
				}
				if (string.IsNullOrEmpty(ChkSecondLevel.Text))
				{
					ChkSecondLevel.Text = "Second";
				}
				if (string.IsNullOrEmpty(ChkThirdLevel.Text))
				{
					ChkThirdLevel.Text = "Third";
				}
				txtFirstLevelRowCount.Text = Level1.ToString("#,0");
				txtSecondLevelRowCount.Text = Level2.ToString("#,#0");
				txtThirdLevelRowCount.Text = Level3.ToString("#,#0");
				grdII.DataSource = dtcol;
				grdII.RetrieveStructure();
				GridTemplateIISetting();
			}
			else
			{
				grdII.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridTemplateIISetting()
	{
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdII.AllowEdit = (InheritableBoolean)1;
			grdII.RootTable.Groups.Add("Description");
			grdII.RootTable.Columns["Description"].Visible = false;
			grdII.RootTable.Columns["CustomerName"].Visible = false;
			grdII.RootTable.Columns["PreviousReadyDate"].Visible = false;
			grdII.RootTable.Columns["PreviousDocumentStatus"].Visible = false;
			grdII.RootTable.Columns["shipmentDocumentScheduleId"].Visible = false;
			grdII.RootTable.Columns["LevelId"].Visible = false;
			grdII.RootTable.Columns["LevelName"].Visible = false;
			grdII.RootTable.Columns["ColorCode"].Visible = false;
			grdII.RootTable.Columns["DueDateDiffDays"].Visible = false;
			grdII.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdII.RootTable.Columns)
			{
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)((!item.Key.Contains("Date")) ? 1 : 6);
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdII.RootTable.Columns["DocumentStatus"].ColumnType = (ColumnType)0;
			grdII.RootTable.Columns["DocumentStatus"].HasValueList = true;
			grdII.RootTable.Columns["DocumentStatus"].LimitToList = true;
			grdII.RootTable.Columns["DocumentStatus"].ValueList.PopulateValueList((IEnumerable)dtStatus.DefaultView, "Id", "type");
			grdII.RootTable.Columns["DocumentStatus"].EditType = (EditType)(formright.DoHaveSaveRight ? 4 : 0);
			grdII.RootTable.Columns["ReadyDate"].EditType = (EditType)(formright.DoHaveSaveRight ? 2 : 0);
			grdII.RootTable.Columns["CustomerName"].Width = 150;
			grdII.RootTable.Columns["ScheduleCode"].Width = 120;
			grdII.RootTable.Columns["InvoiceNo"].Width = 100;
			grdII.RootTable.Columns["InvoiceDate"].Width = 80;
			grdII.RootTable.Columns["DueDate"].Width = 80;
			grdII.RootTable.Columns["ReadyDate"].Width = 80;
			grdII.RootTable.Columns["DocumentStatus"].Width = 85;
			grdII.RootTable.Columns["Remarks"].Width = 115;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdII_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		try
		{
			if (grdII.RowCount <= 0 || (int)e.Row.RowType != 0 || e.Row.Cells.Count <= 0)
			{
				return;
			}
			object value = e.Row.Cells["DueDate"].Value;
			string colorCode = e.Row.Cells["ColorCode"].Value?.ToString();
			if (value != null && !string.IsNullOrWhiteSpace(colorCode))
			{
				if (e.Row.Cells["DueDate"].FormatStyle == null)
				{
					e.Row.Cells["DueDate"].FormatStyle = new GridEXFormatStyle();
				}
				e.Row.Cells["DueDate"].FormatStyle.ForeColor = ColorTranslator.FromHtml(colorCode);
				e.Row.Cells["DueDate"].FormatStyle.FontBold = (TriState)2;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailFill(ReportsParameters grn, bool Value = true)
	{
		try
		{
			if (Value)
			{
				deDetailGrid.Clear();
				deDetailGrid = ShipmentDocumentSchedule.ExportDocumentTrackingReport(grn);
			}
			if (deDetailGrid.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("shipmentDocumentScheduleId", typeof(int));
				dtcol.Columns.Add("referenceDocumentTypeId", typeof(int));
				dtcol.Columns.Add("CustomerName", typeof(string));
				dtcol.Columns.Add("refDocumentId", typeof(int));
				dtcol.Columns.Add("ScheduleCode", typeof(string));
				dtcol.Columns.Add("ExImInvoiceId", typeof(int));
				dtcol.Columns.Add("InvoiceNo", typeof(string));
				dtcol.Columns.Add("InvoiceDate", typeof(string));
				dtcol.Columns.Add("chartOfDocumentId", typeof(int));
				dtcol.Columns.Add("DocumentName", typeof(string));
				dtcol.Columns.Add("ProviderName", typeof(string));
				dtcol.Columns.Add("DueDate", typeof(string));
				dtcol.Columns.Add("ReadyDate", typeof(string));
				dtcol.Columns.Add("DocumentStatus", typeof(string));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("Original", typeof(int));
				dtcol.Columns.Add("Duplicate", typeof(int));
				dtcol.Columns.Add("CriteriaDate", typeof(string));
				dtcol.Columns.Add("BeforeDays", typeof(int));
				dtcol.Columns.Add("AfterDays", typeof(int));
				dtcol.Columns.Add("Contract", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Columns.Add("LevelId", typeof(int));
				dtcol.Columns.Add("LevelName", typeof(string));
				dtcol.Columns.Add("ColorCode", typeof(string));
				dtcol.Columns.Add("DueDateDiffDays", typeof(int));
				dtcol.Rows.Clear();
				ChkFirstLevel.Text = "";
				ChkSecondLevel.Text = "";
				ChkThirdLevel.Text = "";
				int Level1 = 0;
				int Level2 = 0;
				int Level3 = 0;
				foreach (DataRow r in deDetailGrid.Rows)
				{
					if ((!ChkFirstLevel.Checked && !ChkSecondLevel.Checked && !ChkThirdLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 31 && ChkFirstLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 32 && ChkSecondLevel.Checked) || (Conversion.ToInt(r["LevelId"]) == 33 && ChkThirdLevel.Checked))
					{
						DateTime ReadyDate = Conversion.ToDateTime(r["ReadyDate"]);
						DateTime InvoiceDate = Conversion.ToDateTime(r["InvoiceDate"]);
						DateTime criteriaDate = Conversion.ToDateTime(r["criteriaDate"]);
						DateTime dueDate = Conversion.ToDateTime(r["dueDate"]);
						dtcol.Rows.Add(r["shipmentDocumentScheduleId"], r["referenceDocumentTypeId"], r["CustomerName"], r["refDocumentId"], r["ScheduleCode"], r["ExImInvoiceId"], r["InvoiceNo"], Conversion.CheckDateTimeNull(InvoiceDate) ? "" : InvoiceDate.ToShortDateString(), r["chartOfDocumentId"], r["documentName"], r["DocProviderName"], Conversion.CheckDateTimeNull(dueDate) ? "" : dueDate.ToShortDateString(), Conversion.CheckDateTimeNull(ReadyDate) ? "" : ReadyDate.ToShortDateString(), r["DocStatus"], r["Remarks"], r["original"], r["duplicate"], Conversion.CheckDateTimeNull(criteriaDate) ? "" : criteriaDate.ToShortDateString(), r["beforeDays"], r["afterDays"], r["LC_Contract"], r["NoOfAttachments"], r["LevelId"], r["LevelName"], r["ColorCode"], r["DueDateDiffDays"]);
					}
					Level1 = ((Conversion.ToInt(r["LevelId"]) == 31) ? (Level1 + 1) : Level1);
					Level2 = ((Conversion.ToInt(r["LevelId"]) == 32) ? (Level2 + 1) : Level2);
					Level3 = ((Conversion.ToInt(r["LevelId"]) == 33) ? (Level3 + 1) : Level3);
					if (string.IsNullOrEmpty(ChkFirstLevel.Text) && Conversion.ToInt(r["LevelId"]) == 31)
					{
						ChkFirstLevel.Text = Conversion.ToString(r["LevelName"]);
						ChkFirstLevel.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r["ColorCode"]));
					}
					if (string.IsNullOrEmpty(ChkSecondLevel.Text) && Conversion.ToInt(r["LevelId"]) == 32)
					{
						ChkSecondLevel.Text = Conversion.ToString(r["LevelName"]);
						ChkSecondLevel.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r["ColorCode"]));
					}
					if (string.IsNullOrEmpty(ChkThirdLevel.Text) && Conversion.ToInt(r["LevelId"]) == 33)
					{
						ChkThirdLevel.Text = Conversion.ToString(r["LevelName"]);
						ChkThirdLevel.ForeColor = ColorTranslator.FromHtml(Conversion.ToString(r["ColorCode"]));
					}
				}
				if (string.IsNullOrEmpty(ChkFirstLevel.Text))
				{
					ChkFirstLevel.Text = "First";
				}
				if (string.IsNullOrEmpty(ChkSecondLevel.Text))
				{
					ChkSecondLevel.Text = "Second";
				}
				if (string.IsNullOrEmpty(ChkThirdLevel.Text))
				{
					ChkThirdLevel.Text = "Third";
				}
				txtFirstLevelRowCount.Text = Level1.ToString("#,0");
				txtSecondLevelRowCount.Text = Level2.ToString("#,#0");
				txtThirdLevelRowCount.Text = Level3.ToString("#,#0");
				grdDetail.DataSource = dtcol;
				grdDetail.RetrieveStructure();
				GridDetailSetting();
			}
			else
			{
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridDetailSetting()
	{
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.RootTable.Columns["shipmentDocumentScheduleId"].Visible = false;
			grdDetail.RootTable.Columns["referenceDocumentTypeId"].Visible = false;
			grdDetail.RootTable.Columns["refDocumentId"].Visible = false;
			grdDetail.RootTable.Columns["ExImInvoiceId"].Visible = false;
			grdDetail.RootTable.Columns["chartOfDocumentId"].Visible = false;
			grdDetail.RootTable.Columns["LevelId"].Visible = false;
			grdDetail.RootTable.Columns["LevelName"].Visible = false;
			grdDetail.RootTable.Columns["ColorCode"].Visible = false;
			grdDetail.RootTable.Columns["DueDateDiffDays"].Visible = false;
			grdDetail.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdDetail.RootTable.Columns["CustomerName"].Width = 150;
			grdDetail.RootTable.Columns["ScheduleCode"].Width = 120;
			grdDetail.RootTable.Columns["InvoiceNo"].Width = 110;
			grdDetail.RootTable.Columns["InvoiceDate"].Width = 80;
			grdDetail.RootTable.Columns["DueDate"].Width = 80;
			grdDetail.RootTable.Columns["ReadyDate"].Width = 80;
			grdDetail.RootTable.Columns["DocumentStatus"].Width = 85;
			grdDetail.RootTable.Columns["Remarks"].Width = 120;
			grdDetail.RootTable.Columns["AfterDays"].Width = 60;
			grdDetail.RootTable.Columns["BeforeDays"].Width = 60;
			grdDetail.RootTable.Columns["CriteriaDate"].Width = 80;
			grdDetail.RootTable.Columns["Duplicate"].Width = 70;
			grdDetail.RootTable.Columns["Original"].Width = 65;
			grdDetail.RootTable.Columns["Contract"].Width = 120;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDetail_FormattingRow(object sender, RowLoadEventArgs e)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		try
		{
			if (grdDetail.RowCount <= 0 || (int)e.Row.RowType != 0 || e.Row.Cells.Count <= 0)
			{
				return;
			}
			object value = e.Row.Cells["DueDate"].Value;
			string colorCode = e.Row.Cells["ColorCode"].Value?.ToString();
			if (value != null && !string.IsNullOrWhiteSpace(colorCode))
			{
				if (e.Row.Cells["DueDate"].FormatStyle == null)
				{
					e.Row.Cells["DueDate"].FormatStyle = new GridEXFormatStyle();
				}
				e.Row.Cells["DueDate"].FormatStyle.ForeColor = ColorTranslator.FromHtml(colorCode);
				e.Row.Cells["DueDate"].FormatStyle.FontBold = (TriState)2;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDetail_ColorUpdate()
	{
		try
		{
			if (grdDetail.RowCount <= 0)
			{
				return;
			}
			GridEXRow[] dataRows = grdDetail.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				object dueDateValue = r.Cells["DueDate"].Value;
				string colorCode = r.Cells["ColorCode"].Value?.ToString();
				if (dueDateValue != null && !string.IsNullOrWhiteSpace(colorCode) && r.Cells["DueDate"].FormatStyle != null)
				{
					r.Cells["DueDate"].FormatStyle.ForeColor = ColorTranslator.FromHtml(colorCode);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int referenceDocumentTypeId = Conversion.ToInt(item.Cells["referenceDocumentTypeId"].Value);
			int num = Conversion.ToInt(item.Cells["refDocumentId"].Value);
			int chartOfDocumentId = Conversion.ToInt(item.Cells["chartOfDocumentId"].Value);
			DataTable dtViewAttachments = new DataTable();
			dtViewAttachments.Columns.Add("AttchmentPath");
			List<global::Architecture.Model.DMSAttachments> dMSList = DMSAttachments.ReadAttachmentsbyRefDocumentTypeIdAndAccountId(num, referenceDocumentTypeId, chartOfDocumentId);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.UploadedFileCustomName);
				}
				new AttachmentView(dtViewAttachments).Show();
			}
			else
			{
				dtViewAttachments.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grd).Name);
			if (fs != null)
			{
				grd.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdII).Name);
			if (fs != null)
			{
				grdII.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdII).Name);
			if (fs != null)
			{
				grdII.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportDocumentTrackingReport_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				BtnPrint_Click(null, null);
			}
			if (e.Alt)
			{
				switch (e.KeyCode)
				{
				case Keys.D1:
					BtnPrint_Click(null, null);
					break;
				case Keys.D2:
					btn335Register_Click(null, null);
					break;
				}
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Shoe Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+1", "For Print 841");
			dt.Rows.Add("Alt+2", "For Print 841A");
			dt.Rows.Add("Ctrl+F5", "For Focus on Combo Date");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date combo in Filters Box");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grd.CurrentColumn != null && e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				_ = grd.CurrentColumn.Key == "NoOfAttachments";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Reset()
	{
		try
		{
			GRNfromdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
			StatusFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "DataGridHistory")
			{
				GRNfromdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmExportDocumentTrackingReport_FormClosing(object sender, FormClosingEventArgs e)
	{
		try
		{
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btn335Register_Click(object sender, EventArgs e)
	{
	}

	private void BtnPrint_Click(object sender, EventArgs e)
	{
	}

	private void BtnUpdateTemplateI_Click(object sender, EventArgs e)
	{
		try
		{
			Insert(grdII);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateTemplateII_Click(object sender, EventArgs e)
	{
		try
		{
			Insert(grd);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert(GridEX grd)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		try
		{
			if (grd.RowCount == 0)
			{
				throw new Exception("Grid Record not found...");
			}
			ShipmentDocumentSchedule obj = new ShipmentDocumentSchedule();
			obj.ShipmentDocumentScheduleList = new List<ShipmentDocumentSchedule>();
			if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] dataRows = grd.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				if (Conversion.ToInt(r.Cells["DocumentStatus"].Value) != Conversion.ToInt(r.Cells["PreviousDocumentStatus"].Value) || Conversion.ToDateTime(r.Cells["ReadyDate"].Value) != Conversion.ToDateTime(r.Cells["PreviousReadyDate"].Value))
				{
					ShipmentDocumentSchedule exIm = new ShipmentDocumentSchedule();
					exIm.shipmentDocumentScheduleId = Conversion.ToInt(r.Cells["shipmentDocumentScheduleId"].Value);
					exIm.DocStatus = Conversion.ToInt(r.Cells["DocumentStatus"].Value);
					if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime(r.Cells["ReadyDate"].Value)))
					{
						exIm.ReadyDate = Conversion.ToDateTime(r.Cells["ReadyDate"].Value);
					}
					exIm.ModifyDate = DateTime.Now;
					exIm.ModifyUserId = UserAccount.ID;
					obj.ShipmentDocumentScheduleList.Add(exIm);
				}
			}
			if (obj.ShipmentDocumentScheduleList.Count == 0)
			{
				throw new Exception("No record Found For Updation");
			}
			ShipmentDocumentSchedule.UpdateDocStatusAndReadyDate(obj);
			MessageBox.Show("Update SuccessFully");
			btnshow_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnUpdateInGrid_Click(object sender, EventArgs e)
	{
		try
		{
			if (!formright.DoHaveSaveRight)
			{
				MessageBox.Show("You don't have the right to update the record.");
				return;
			}
			if (chkGrdViewI.Checked)
			{
				UpdateGrid(grdII);
			}
			if (chkGrdViewII.Checked)
			{
				UpdateGrid(grd);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateGrid(GridEX grid)
	{
		try
		{
			if (grid.RowCount <= 0)
			{
				return;
			}
			string readyDate = (txtReadyDateUpdateInGrid.Checked ? Conversion.ToDateTime((object)txtReadyDateUpdateInGrid.Value).ToShortDateString() : "");
			int status = Conversion.ToInt(CmbDocmentStausToUpdate.Value);
			bool isAllSelected = RadAll.Checked;
			GridEXRow[] dataRows = grid.GetDataRows();
			foreach (GridEXRow row in dataRows)
			{
				row.BeginEdit();
				if (isAllSelected || Conversion.CheckDateTimeNull(Conversion.ToDateTime(row.Cells["ReadyDate"].Value)))
				{
					row.Cells["ReadyDate"].Value = readyDate;
				}
				if (status > 0 && (isAllSelected || Conversion.ToInt(row.Cells["DocumentStatus"].Value) == 0))
				{
					row.Cells["DocumentStatus"].Value = status;
				}
				row.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error updating grid: " + ex.Message);
		}
	}

	private void BtnColorForm_Click(object sender, EventArgs e)
	{
		try
		{
			new frmDocDueAlertColorSchedule(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkFirstLevel_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			FillGrids(Value: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkSecondLevel_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			FillGrids(Value: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkThirdLevel_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			FillGrids(Value: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Expected O, but got Unknown
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_029a: Expected O, but got Unknown
		//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ef: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_04a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Expected O, but got Unknown
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f7: Expected O, but got Unknown
		//IL_059d: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Expected O, but got Unknown
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0c: Expected O, but got Unknown
		//IL_0d82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8c: Expected O, but got Unknown
		//IL_0d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da3: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdII_DesignTimeLayout = new GridEXLayout();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SDT_Reports.frmExportDocumentTrackingReport));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.BtnUpdateTemplateII = new System.Windows.Forms.Button();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdII = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.BtnUpdateTemplateI = new System.Windows.Forms.Button();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.GroupColorAlert = new System.Windows.Forms.GroupBox();
		this.ChkThirdLevel = new System.Windows.Forms.CheckBox();
		this.ChkSecondLevel = new System.Windows.Forms.CheckBox();
		this.ChkFirstLevel = new System.Windows.Forms.CheckBox();
		this.txtThirdLevelRowCount = new System.Windows.Forms.Label();
		this.txtSecondLevelRowCount = new System.Windows.Forms.Label();
		this.txtFirstLevelRowCount = new System.Windows.Forms.Label();
		this.GroupBoxValuesToUpdateInGrid = new System.Windows.Forms.GroupBox();
		this.panel10 = new System.Windows.Forms.Panel();
		this.chkGrdViewI = new System.Windows.Forms.RadioButton();
		this.chkGrdViewII = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.RadOnlyEmpty = new System.Windows.Forms.RadioButton();
		this.BtnUpdateInGrid = new System.Windows.Forms.Button();
		this.CmbDocmentStausToUpdate = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtReadyDateUpdateInGrid = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.panel11 = new System.Windows.Forms.Panel();
		this.RadAllInvoice = new System.Windows.Forms.RadioButton();
		this.RadInvoiceDone = new System.Windows.Forms.RadioButton();
		this.RadInvoicePending = new System.Windows.Forms.RadioButton();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.RadReadyDate = new System.Windows.Forms.RadioButton();
		this.RadDueDate = new System.Windows.Forms.RadioButton();
		this.RadAllDocs = new System.Windows.Forms.RadioButton();
		this.RadNotReadyDocs = new System.Windows.Forms.RadioButton();
		this.RadReadyDocs = new System.Windows.Forms.RadioButton();
		this.CmbInvoiceNo = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.CmbdocumentProvider = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbDocument = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbCustomerName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbperemeter = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.GRNfromdate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.GRNToDate = new System.Windows.Forms.DateTimePicker();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.BtnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.btn335Register = new System.Windows.Forms.ToolStripButton();
		this.BtnColorForm = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.ultraCombo1 = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel7.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdII).BeginInit();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		this.GroupColorAlert.SuspendLayout();
		this.GroupBoxValuesToUpdateInGrid.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocmentStausToUpdate).BeginInit();
		this.groupBox1.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbdocumentProvider).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocument).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1234, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 190);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1234, 371);
		this.tabControl1.TabIndex = 5;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 342);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Detail";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		this.grdDetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 28);
		((System.Windows.Forms.Control)(object)this.grdDetail).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1170, 311);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.LinkClicked += new ColumnActionEventHandler(grdDetail_LinkClicked);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Margin = new System.Windows.Forms.Padding(4);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1170, 25);
		this.panel6.TabIndex = 4;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1128, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar3.MyGrid = this.grdDetail;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar3.TabIndex = 3;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1226, 342);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Group Wise";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(702, 3);
		this.panel5.Margin = new System.Windows.Forms.Padding(4);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(521, 336);
		this.panel5.TabIndex = 3;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grd).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(521, 311);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.grd.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.BtnUpdateTemplateII);
		this.panel7.Controls.Add(this.ctrlGrdBar1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(4);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(521, 25);
		this.panel7.TabIndex = 2;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(5, 4);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 18);
		this.label5.TabIndex = 2;
		this.label5.Text = "View II";
		this.BtnUpdateTemplateII.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateTemplateII.BackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnUpdateTemplateII.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateII.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateTemplateII.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateTemplateII.Location = new System.Drawing.Point(448, 1);
		this.BtnUpdateTemplateII.Margin = new System.Windows.Forms.Padding(0);
		this.BtnUpdateTemplateII.Name = "BtnUpdateTemplateII";
		this.BtnUpdateTemplateII.Size = new System.Drawing.Size(27, 22);
		this.BtnUpdateTemplateII.TabIndex = 7;
		this.BtnUpdateTemplateII.UseVisualStyleBackColor = false;
		this.BtnUpdateTemplateII.Click += new System.EventHandler(BtnUpdateTemplateII_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(478, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar1.TabIndex = 3;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdII);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Margin = new System.Windows.Forms.Padding(4);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(699, 336);
		this.panel8.TabIndex = 4;
		this.grdII.AllowEdit = (InheritableBoolean)2;
		this.grdII.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdII_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdII.DesignTimeLayout = grdII_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdII).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdII.DynamicFiltering = true;
		this.grdII.FilterMode = (FilterMode)2;
		this.grdII.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdII).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdII.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdII).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdII).Margin = new System.Windows.Forms.Padding(4);
		((System.Windows.Forms.Control)(object)this.grdII).Name = "grdII";
		((ScrollableBase)this.grdII).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdII).Size = new System.Drawing.Size(699, 311);
		((System.Windows.Forms.Control)(object)this.grdII).TabIndex = 0;
		this.grdII.TotalRow = (InheritableBoolean)1;
		this.grdII.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdII).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label4);
		this.panel9.Controls.Add(this.BtnUpdateTemplateI);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Margin = new System.Windows.Forms.Padding(4);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(699, 25);
		this.panel9.TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(4, 3);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(56, 18);
		this.label4.TabIndex = 2;
		this.label4.Text = "View I";
		this.BtnUpdateTemplateI.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnUpdateTemplateI.BackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateI.Cursor = System.Windows.Forms.Cursors.Hand;
		this.BtnUpdateTemplateI.FlatAppearance.BorderColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateI.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateI.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
		this.BtnUpdateTemplateI.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateTemplateI.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateTemplateI.Location = new System.Drawing.Point(625, 1);
		this.BtnUpdateTemplateI.Margin = new System.Windows.Forms.Padding(0);
		this.BtnUpdateTemplateI.Name = "BtnUpdateTemplateI";
		this.BtnUpdateTemplateI.Size = new System.Drawing.Size(27, 22);
		this.BtnUpdateTemplateI.TabIndex = 6;
		this.BtnUpdateTemplateI.UseVisualStyleBackColor = false;
		this.BtnUpdateTemplateI.Click += new System.EventHandler(BtnUpdateTemplateI_Click);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(657, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = this.grdII;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 22);
		this.ctrlGrdBar2.TabIndex = 3;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.GroupColorAlert);
		this.panel4.Controls.Add(this.GroupBoxValuesToUpdateInGrid);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(4);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1234, 133);
		this.panel4.TabIndex = 0;
		this.GroupColorAlert.Controls.Add(this.ChkThirdLevel);
		this.GroupColorAlert.Controls.Add(this.ChkSecondLevel);
		this.GroupColorAlert.Controls.Add(this.ChkFirstLevel);
		this.GroupColorAlert.Controls.Add(this.txtThirdLevelRowCount);
		this.GroupColorAlert.Controls.Add(this.txtSecondLevelRowCount);
		this.GroupColorAlert.Controls.Add(this.txtFirstLevelRowCount);
		this.GroupColorAlert.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupColorAlert.Location = new System.Drawing.Point(782, 3);
		this.GroupColorAlert.Name = "GroupColorAlert";
		this.GroupColorAlert.Size = new System.Drawing.Size(186, 126);
		this.GroupColorAlert.TabIndex = 1;
		this.GroupColorAlert.TabStop = false;
		this.GroupColorAlert.Text = "Due Date Alert";
		this.ChkThirdLevel.AutoSize = true;
		this.ChkThirdLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkThirdLevel.Location = new System.Drawing.Point(7, 69);
		this.ChkThirdLevel.Name = "ChkThirdLevel";
		this.ChkThirdLevel.Size = new System.Drawing.Size(90, 24);
		this.ChkThirdLevel.TabIndex = 0;
		this.ChkThirdLevel.Text = "3rd Level";
		this.ChkThirdLevel.UseVisualStyleBackColor = true;
		this.ChkThirdLevel.CheckedChanged += new System.EventHandler(ChkThirdLevel_CheckedChanged);
		this.ChkSecondLevel.AutoSize = true;
		this.ChkSecondLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSecondLevel.Location = new System.Drawing.Point(7, 43);
		this.ChkSecondLevel.Name = "ChkSecondLevel";
		this.ChkSecondLevel.Size = new System.Drawing.Size(93, 24);
		this.ChkSecondLevel.TabIndex = 0;
		this.ChkSecondLevel.Text = "2nd Level";
		this.ChkSecondLevel.UseVisualStyleBackColor = true;
		this.ChkSecondLevel.CheckedChanged += new System.EventHandler(ChkSecondLevel_CheckedChanged);
		this.ChkFirstLevel.AutoSize = true;
		this.ChkFirstLevel.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkFirstLevel.Location = new System.Drawing.Point(7, 18);
		this.ChkFirstLevel.Name = "ChkFirstLevel";
		this.ChkFirstLevel.Size = new System.Drawing.Size(84, 24);
		this.ChkFirstLevel.TabIndex = 0;
		this.ChkFirstLevel.Text = "1st Level";
		this.ChkFirstLevel.UseVisualStyleBackColor = true;
		this.ChkFirstLevel.CheckedChanged += new System.EventHandler(ChkFirstLevel_CheckedChanged);
		this.txtThirdLevelRowCount.AutoSize = true;
		this.txtThirdLevelRowCount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.txtThirdLevelRowCount.Location = new System.Drawing.Point(111, 73);
		this.txtThirdLevelRowCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.txtThirdLevelRowCount.Name = "txtThirdLevelRowCount";
		this.txtThirdLevelRowCount.Size = new System.Drawing.Size(163, 16);
		this.txtThirdLevelRowCount.TabIndex = 119;
		this.txtThirdLevelRowCount.Text = "3rd Level Rows Count";
		this.txtSecondLevelRowCount.AutoSize = true;
		this.txtSecondLevelRowCount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold);
		this.txtSecondLevelRowCount.Location = new System.Drawing.Point(111, 47);
		this.txtSecondLevelRowCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.txtSecondLevelRowCount.Name = "txtSecondLevelRowCount";
		this.txtSecondLevelRowCount.Size = new System.Drawing.Size(166, 16);
		this.txtSecondLevelRowCount.TabIndex = 119;
		this.txtSecondLevelRowCount.Text = "2nd Level Rows Count";
		this.txtFirstLevelRowCount.AutoSize = true;
		this.txtFirstLevelRowCount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFirstLevelRowCount.Location = new System.Drawing.Point(111, 22);
		this.txtFirstLevelRowCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.txtFirstLevelRowCount.Name = "txtFirstLevelRowCount";
		this.txtFirstLevelRowCount.Size = new System.Drawing.Size(162, 16);
		this.txtFirstLevelRowCount.TabIndex = 119;
		this.txtFirstLevelRowCount.Text = "1st Level Rows Count";
		this.GroupBoxValuesToUpdateInGrid.Controls.Add(this.panel10);
		this.GroupBoxValuesToUpdateInGrid.Controls.Add(this.RadAll);
		this.GroupBoxValuesToUpdateInGrid.Controls.Add(this.RadOnlyEmpty);
		this.GroupBoxValuesToUpdateInGrid.Controls.Add(this.BtnUpdateInGrid);
		this.GroupBoxValuesToUpdateInGrid.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocmentStausToUpdate);
		this.GroupBoxValuesToUpdateInGrid.Controls.Add(this.label6);
		this.GroupBoxValuesToUpdateInGrid.Controls.Add(this.txtReadyDateUpdateInGrid);
		this.GroupBoxValuesToUpdateInGrid.Controls.Add(this.label10);
		this.GroupBoxValuesToUpdateInGrid.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.GroupBoxValuesToUpdateInGrid.Location = new System.Drawing.Point(968, 5);
		this.GroupBoxValuesToUpdateInGrid.Name = "GroupBoxValuesToUpdateInGrid";
		this.GroupBoxValuesToUpdateInGrid.Size = new System.Drawing.Size(263, 124);
		this.GroupBoxValuesToUpdateInGrid.TabIndex = 2;
		this.GroupBoxValuesToUpdateInGrid.TabStop = false;
		this.GroupBoxValuesToUpdateInGrid.Text = "Values To Update In Grid";
		this.GroupBoxValuesToUpdateInGrid.Visible = false;
		this.panel10.Controls.Add(this.chkGrdViewI);
		this.panel10.Controls.Add(this.chkGrdViewII);
		this.panel10.Location = new System.Drawing.Point(107, 70);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(69, 37);
		this.panel10.TabIndex = 590;
		this.chkGrdViewI.AutoSize = true;
		this.chkGrdViewI.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGrdViewI.Location = new System.Drawing.Point(3, 3);
		this.chkGrdViewI.Name = "chkGrdViewI";
		this.chkGrdViewI.Size = new System.Drawing.Size(59, 17);
		this.chkGrdViewI.TabIndex = 0;
		this.chkGrdViewI.TabStop = true;
		this.chkGrdViewI.Text = "View I";
		this.chkGrdViewI.UseVisualStyleBackColor = true;
		this.chkGrdViewII.AutoSize = true;
		this.chkGrdViewII.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGrdViewII.Location = new System.Drawing.Point(3, 18);
		this.chkGrdViewII.Name = "chkGrdViewII";
		this.chkGrdViewII.Size = new System.Drawing.Size(64, 17);
		this.chkGrdViewII.TabIndex = 1;
		this.chkGrdViewII.TabStop = true;
		this.chkGrdViewII.Text = "View II";
		this.chkGrdViewII.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAll.Location = new System.Drawing.Point(14, 71);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(72, 17);
		this.RadAll.TabIndex = 2;
		this.RadAll.Text = "All Rows";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadOnlyEmpty.AutoSize = true;
		this.RadOnlyEmpty.Checked = true;
		this.RadOnlyEmpty.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadOnlyEmpty.Location = new System.Drawing.Point(14, 87);
		this.RadOnlyEmpty.Name = "RadOnlyEmpty";
		this.RadOnlyEmpty.Size = new System.Drawing.Size(89, 17);
		this.RadOnlyEmpty.TabIndex = 3;
		this.RadOnlyEmpty.TabStop = true;
		this.RadOnlyEmpty.Text = "Empty Only";
		this.RadOnlyEmpty.UseVisualStyleBackColor = true;
		this.BtnUpdateInGrid.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.BtnUpdateInGrid.FlatAppearance.BorderSize = 0;
		this.BtnUpdateInGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateInGrid.Font = new System.Drawing.Font("Verdana", 8.75f, System.Drawing.FontStyle.Bold);
		this.BtnUpdateInGrid.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateInGrid.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.BtnUpdateInGrid.Location = new System.Drawing.Point(186, 70);
		this.BtnUpdateInGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.BtnUpdateInGrid.Name = "BtnUpdateInGrid";
		this.BtnUpdateInGrid.Size = new System.Drawing.Size(69, 37);
		this.BtnUpdateInGrid.TabIndex = 4;
		this.BtnUpdateInGrid.Text = "Update In Grid";
		this.BtnUpdateInGrid.UseVisualStyleBackColor = false;
		this.BtnUpdateInGrid.Click += new System.EventHandler(BtnUpdateInGrid_Click);
		this.CmbDocmentStausToUpdate.AllowNull = (DefaultableBoolean)1;
		this.CmbDocmentStausToUpdate.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocmentStausToUpdate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocmentStausToUpdate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocmentStausToUpdate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocmentStausToUpdate).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocmentStausToUpdate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocmentStausToUpdate).Location = new System.Drawing.Point(110, 42);
		((System.Windows.Forms.Control)(object)this.CmbDocmentStausToUpdate).Name = "CmbDocmentStausToUpdate";
		((System.Windows.Forms.Control)(object)this.CmbDocmentStausToUpdate).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocmentStausToUpdate).TabIndex = 1;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(9, 48);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(100, 15);
		this.label6.TabIndex = 588;
		this.label6.Text = "Document Status";
		this.txtReadyDateUpdateInGrid.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReadyDateUpdateInGrid.Checked = false;
		this.txtReadyDateUpdateInGrid.CustomFormat = "dd-MMM-yy";
		this.txtReadyDateUpdateInGrid.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReadyDateUpdateInGrid.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtReadyDateUpdateInGrid.Location = new System.Drawing.Point(110, 18);
		this.txtReadyDateUpdateInGrid.Margin = new System.Windows.Forms.Padding(4);
		this.txtReadyDateUpdateInGrid.Name = "txtReadyDateUpdateInGrid";
		this.txtReadyDateUpdateInGrid.ShowCheckBox = true;
		this.txtReadyDateUpdateInGrid.Size = new System.Drawing.Size(145, 22);
		this.txtReadyDateUpdateInGrid.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(9, 22);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 119;
		this.label10.Text = "Ready Date";
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.panel11);
		this.groupBox1.Controls.Add(this.groupBox2);
		this.groupBox1.Controls.Add(this.RadAllDocs);
		this.groupBox1.Controls.Add(this.RadNotReadyDocs);
		this.groupBox1.Controls.Add(this.RadReadyDocs);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbdocumentProvider);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocument);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.ultraCombo1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.GRNfromdate);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.GRNToDate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(777, 126);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.panel11.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.panel11.Controls.Add(this.RadAllInvoice);
		this.panel11.Controls.Add(this.RadInvoiceDone);
		this.panel11.Controls.Add(this.RadInvoicePending);
		this.panel11.Location = new System.Drawing.Point(663, 9);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(113, 57);
		this.panel11.TabIndex = 13;
		this.RadAllInvoice.AutoSize = true;
		this.RadAllInvoice.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAllInvoice.Location = new System.Drawing.Point(2, 4);
		this.RadAllInvoice.Name = "RadAllInvoice";
		this.RadAllInvoice.Size = new System.Drawing.Size(84, 17);
		this.RadAllInvoice.TabIndex = 0;
		this.RadAllInvoice.Text = "All Invoice";
		this.RadAllInvoice.UseVisualStyleBackColor = true;
		this.RadInvoiceDone.AutoSize = true;
		this.RadInvoiceDone.Checked = true;
		this.RadInvoiceDone.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadInvoiceDone.Location = new System.Drawing.Point(2, 35);
		this.RadInvoiceDone.Name = "RadInvoiceDone";
		this.RadInvoiceDone.Size = new System.Drawing.Size(99, 17);
		this.RadInvoiceDone.TabIndex = 2;
		this.RadInvoiceDone.TabStop = true;
		this.RadInvoiceDone.Text = "Invoice Done";
		this.RadInvoiceDone.UseVisualStyleBackColor = true;
		this.RadInvoicePending.AutoSize = true;
		this.RadInvoicePending.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadInvoicePending.Location = new System.Drawing.Point(2, 19);
		this.RadInvoicePending.Name = "RadInvoicePending";
		this.RadInvoicePending.Size = new System.Drawing.Size(115, 17);
		this.RadInvoicePending.TabIndex = 1;
		this.RadInvoicePending.Text = "Invoice Pending";
		this.RadInvoicePending.UseVisualStyleBackColor = true;
		this.groupBox2.Controls.Add(this.RadReadyDate);
		this.groupBox2.Controls.Add(this.RadDueDate);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 29);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(58, 50);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Date";
		this.RadReadyDate.AutoSize = true;
		this.RadReadyDate.Checked = true;
		this.RadReadyDate.Font = new System.Drawing.Font("Tahoma", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadReadyDate.Location = new System.Drawing.Point(3, 12);
		this.RadReadyDate.Name = "RadReadyDate";
		this.RadReadyDate.Size = new System.Drawing.Size(54, 15);
		this.RadReadyDate.TabIndex = 1;
		this.RadReadyDate.TabStop = true;
		this.RadReadyDate.Text = "Ready";
		this.RadReadyDate.UseVisualStyleBackColor = true;
		this.RadDueDate.AutoSize = true;
		this.RadDueDate.Font = new System.Drawing.Font("Tahoma", 6.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadDueDate.Location = new System.Drawing.Point(3, 27);
		this.RadDueDate.Name = "RadDueDate";
		this.RadDueDate.Size = new System.Drawing.Size(42, 15);
		this.RadDueDate.TabIndex = 0;
		this.RadDueDate.Text = "Due";
		this.RadDueDate.UseVisualStyleBackColor = true;
		this.RadAllDocs.AutoSize = true;
		this.RadAllDocs.Checked = true;
		this.RadAllDocs.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadAllDocs.Location = new System.Drawing.Point(359, 100);
		this.RadAllDocs.Name = "RadAllDocs";
		this.RadAllDocs.Size = new System.Drawing.Size(39, 17);
		this.RadAllDocs.TabIndex = 9;
		this.RadAllDocs.TabStop = true;
		this.RadAllDocs.Text = "All";
		this.RadAllDocs.UseVisualStyleBackColor = true;
		this.RadNotReadyDocs.AutoSize = true;
		this.RadNotReadyDocs.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadNotReadyDocs.Location = new System.Drawing.Point(490, 100);
		this.RadNotReadyDocs.Name = "RadNotReadyDocs";
		this.RadNotReadyDocs.Size = new System.Drawing.Size(113, 17);
		this.RadNotReadyDocs.TabIndex = 11;
		this.RadNotReadyDocs.Text = "Not Ready Docs";
		this.RadNotReadyDocs.UseVisualStyleBackColor = true;
		this.RadReadyDocs.AutoSize = true;
		this.RadReadyDocs.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.RadReadyDocs.Location = new System.Drawing.Point(402, 100);
		this.RadReadyDocs.Name = "RadReadyDocs";
		this.RadReadyDocs.Size = new System.Drawing.Size(91, 17);
		this.RadReadyDocs.TabIndex = 10;
		this.RadReadyDocs.Text = "Ready Docs";
		this.RadReadyDocs.UseVisualStyleBackColor = true;
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(422, 12);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 6;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(0, 64, 64);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(604, 96);
		this.btnshow.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(56, 24);
		this.btnshow.TabIndex = 12;
		this.btnshow.Text = "Show Records";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.CmbdocumentProvider.AllowNull = (DefaultableBoolean)1;
		this.CmbdocumentProvider.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbdocumentProvider.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbdocumentProvider.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbdocumentProvider).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbdocumentProvider).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbdocumentProvider.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbdocumentProvider).Location = new System.Drawing.Point(422, 68);
		((System.Windows.Forms.Control)(object)this.CmbdocumentProvider).Name = "CmbdocumentProvider";
		((System.Windows.Forms.Control)(object)this.CmbdocumentProvider).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbdocumentProvider).TabIndex = 8;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(357, 74);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(51, 15);
		this.label7.TabIndex = 588;
		this.label7.Text = "Provider";
		this.CmbDocument.AllowNull = (DefaultableBoolean)1;
		this.CmbDocument.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocument.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocument.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbDocument).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocument).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocument).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocument).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocument).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocument).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbDocument).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocument).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocument).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbDocument).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocument).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocument).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocument).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocument.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocument).Location = new System.Drawing.Point(422, 40);
		((System.Windows.Forms.Control)(object)this.CmbDocument).Name = "CmbDocument";
		((System.Windows.Forms.Control)(object)this.CmbDocument).Size = new System.Drawing.Size(238, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocument).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(357, 46);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(64, 15);
		this.label3.TabIndex = 588;
		this.label3.Text = "Document";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(357, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(46, 15);
		this.label2.TabIndex = 588;
		this.label2.Text = "Invoice";
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(96, 68);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 74);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(93, 15);
		this.label1.TabIndex = 588;
		this.label1.Text = "Customer Name";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(96, 12);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(3, 14);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(60, 15);
		this.label12.TabIndex = 588;
		this.label12.Text = "Date Type";
		this.GRNfromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Checked = false;
		this.GRNfromdate.CustomFormat = "dd-MMM-yy";
		this.GRNfromdate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNfromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNfromdate.Location = new System.Drawing.Point(96, 42);
		this.GRNfromdate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNfromdate.Name = "GRNfromdate";
		this.GRNfromdate.ShowCheckBox = true;
		this.GRNfromdate.Size = new System.Drawing.Size(130, 22);
		this.GRNfromdate.TabIndex = 2;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(59, 38);
		this.label27.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(35, 30);
		this.label27.TabIndex = 119;
		this.label27.Text = "From\r\n&& To";
		this.GRNToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Checked = false;
		this.GRNToDate.CustomFormat = "dd-MMM-yy";
		this.GRNToDate.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.GRNToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.GRNToDate.Location = new System.Drawing.Point(227, 42);
		this.GRNToDate.Margin = new System.Windows.Forms.Padding(4);
		this.GRNToDate.Name = "GRNToDate";
		this.GRNToDate.ShowCheckBox = true;
		this.GRNToDate.Size = new System.Drawing.Size(130, 22);
		this.GRNToDate.TabIndex = 3;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 28);
		this.panel3.Margin = new System.Windows.Forms.Padding(4);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1234, 29);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(287, 19);
		this.label23.TabIndex = 2;
		this.label23.Text = "Export Document Tracking Report";
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Margin = new System.Windows.Forms.Padding(4);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1234, 28);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnNew, this.btnRefresh, this.BtnPrint, this.btnShortCutKeys, this.btn335Register, this.BtnColorForm });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1234, 27);
		this.toolStrip3.TabIndex = 2;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.BtnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrint.Name = "BtnPrint";
		this.BtnPrint.Size = new System.Drawing.Size(92, 24);
		this.BtnPrint.Text = "841-Print";
		this.BtnPrint.Visible = false;
		this.BtnPrint.Click += new System.EventHandler(BtnPrint_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.btn335Register.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btn335Register.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btn335Register.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btn335Register.Name = "btn335Register";
		this.btn335Register.Size = new System.Drawing.Size(101, 24);
		this.btn335Register.Text = "841A-Print";
		this.btn335Register.Visible = false;
		this.btn335Register.Click += new System.EventHandler(btn335Register_Click);
		this.BtnColorForm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnColorForm.Image = Architecture.WinApp.Properties.Resources.Account_Reports;
		this.BtnColorForm.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnColorForm.Name = "BtnColorForm";
		this.BtnColorForm.Size = new System.Drawing.Size(129, 24);
		this.BtnColorForm.Text = "&Color Schedule";
		this.BtnColorForm.Click += new System.EventHandler(BtnColorForm_Click);
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(96, 96);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 5;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(3, 102);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 588;
		this.label8.Text = "Sales Person";
		base.AutoScaleDimensions = new System.Drawing.SizeF(8f, 16f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1234, 561);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4);
		base.Name = "frmExportDocumentTrackingReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmExportDocumentTrackingReport_FormClosing);
		base.Load += new System.EventHandler(frmExportDocumentTrackingReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportDocumentTrackingReport_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdII).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.GroupColorAlert.ResumeLayout(false);
		this.GroupColorAlert.PerformLayout();
		this.GroupBoxValuesToUpdateInGrid.ResumeLayout(false);
		this.GroupBoxValuesToUpdateInGrid.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocmentStausToUpdate).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbdocumentProvider).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDocument).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		base.ResumeLayout(false);
	}
}

}
