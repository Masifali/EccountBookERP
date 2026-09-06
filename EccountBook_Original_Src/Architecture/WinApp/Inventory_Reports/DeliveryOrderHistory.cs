using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Inventory_Reports;

public class DeliveryOrderHistory : Form
{
	private WaitForm waitForm = new WaitForm();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	public static int DocumentTypeId;

	private DataTable dtGrid = new DataTable();

	private int[] AryColWidth;

	public int DateTypeId;

	public string DOType = "";

	public DateTime FromDate;

	public DateTime ToDate;

	public int InvoiceId;

	private IContainer components;

	private ToolStripButton btnnew;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStrip toolStrip1;

	private Panel panel2;

	private Panel panel3;

	private Panel panel4;

	private Panel panel1;

	private Label label5;

	private Label label4;

	private Label label2;

	private Button btnshow;

	private Label label7;

	private Label label1;

	private ToolStripButton btnPrint;

	private Panel panel5;

	private Label lblCaption;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	private Panel panel6;

	private Label label3;

	private ToolStripButton btnrefresh;

	private Label label6;

	private GroupBox groupBox1;

	private Label label8;

	private ToolStripButton btnShortCutKeys;

	private Label label9;

	public DateTimePicker DateTo;

	public DateTimePicker DateFrom;

	public UltraCombo cmbSupplier;

	public UltraCombo cmbItem;

	public UltraCombo cmbStatus;

	public UltraCombo cmbDeliveryType;

	public UltraCombo cmbdatetype;

	public UltraCombo CmbInvoice;

	public RadioButton RadWithSecondWeight;

	public RadioButton RadOnlyfirstWeight;

	public RadioButton RadAll;

	public RadioButton RadNotReffered;

	public RadioButton RadReffered;

	public RadioButton RadAllRef;

	public DeliveryOrderHistory(UserAccount userAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmStockWithSupplierHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			Datetypefill();
			if (DateTypeId > 0)
			{
				cmbdatetype.Value = DateTypeId;
			}
			if (!Conversion.CheckDateTimeNull(FromDate))
			{
				DateFrom.Value = FromDate;
			}
			if (!Conversion.CheckDateTimeNull(FromDate))
			{
				DateTo.Value = ToDate;
			}
			if (DOType != null && DOType != string.Empty)
			{
				((Control)(object)cmbDeliveryType).Text = DOType;
			}
			if (InvoiceId > 0)
			{
				CmbInvoice.Value = InvoiceId;
				btnshow_Click(null, null);
			}
			else
			{
				waitForm.Show();
			}
			cmbdatetype.Focus();
			StatusFill();
			ComboFill();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void Reset()
	{
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbSupplier).Text = string.Empty;
			((Control)(object)cmbdatetype).Text = string.Empty;
			((UltraGridBase)cmbStatus).Rows[0].Activate();
			cmbdatetype.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnrefresh_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show(this);
			ComboFill();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
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

	private void DeliveryOrderHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null)
			{
				if (base.ActiveControl.Name == "btnshow")
				{
					cmbdatetype.Focus();
				}
				else
				{
					cmbdatetype.Focus();
				}
			}
			else
			{
				cmbdatetype.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtcombo = InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName", typeof(string));
			DataTable dtcust = new DataTable();
			dtcust.Columns.Add("Id", typeof(int));
			dtcust.Columns.Add("CustomerName", typeof(string));
			DataTable dtDelivtype = new DataTable();
			dtDelivtype.Columns.Add("Id", typeof(int));
			dtDelivtype.Columns.Add("DeliveryType", typeof(string));
			DataTable dtInvoice = new DataTable();
			dtInvoice.Columns.Add("Id", typeof(int));
			dtInvoice.Columns.Add("InvoiceNo", typeof(string));
			int SuppId = Conversion.ToInt(cmbSupplier.Value);
			int itmId = Conversion.ToInt(cmbItem.Value);
			int deliverId = Conversion.ToInt(cmbDeliveryType.Value);
			int InvoiceId = Conversion.ToInt(CmbInvoice.Value);
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Item")
				{
					dtItem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "Customer")
				{
					dtcust.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "DeliveryOrderType")
				{
					dtDelivtype.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "InvoiceNo")
				{
					dtInvoice.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtcust.Rows.Count > 0)
			{
				DDL.BindDDL(dtcust, cmbSupplier, "Id", "CustomerName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)cmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtDelivtype.Rows.Count > 0)
			{
				DDL.BindDDL(dtDelivtype, cmbDeliveryType, "Id", "DeliveryType", "Delivery Type", ZeroIndex: true);
				((UltraGridBase)cmbDeliveryType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtDelivtype.Rows.Count > 0)
			{
				DDL.BindDDL(dtInvoice, CmbInvoice, "Id", "InvoiceNo", "InvoiceNo", ZeroIndex: true);
				((UltraGridBase)CmbInvoice).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (SuppId > 0)
			{
				if (dtcust.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SuppId))
				{
					cmbSupplier.Value = SuppId;
				}
				else
				{
					((Control)(object)cmbSupplier).Text = string.Empty;
				}
			}
			if (itmId > 0)
			{
				if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == itmId))
				{
					cmbItem.Value = itmId;
				}
				else
				{
					((Control)(object)cmbItem).Text = string.Empty;
				}
			}
			if (deliverId > 0)
			{
				if (dtDelivtype.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == deliverId))
				{
					cmbDeliveryType.Value = deliverId;
				}
				else
				{
					((Control)(object)cmbDeliveryType).Text = string.Empty;
				}
			}
			if (InvoiceId > 0)
			{
				if (dtInvoice.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == InvoiceId))
				{
					CmbInvoice.Value = InvoiceId;
				}
				else
				{
					((Control)(object)CmbInvoice).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void StatusFill()
	{
		try
		{
			DataTable dtStatus = new DataTable();
			dtStatus.Columns.Add("Id", typeof(int));
			dtStatus.Columns.Add("Label", typeof(string));
			dtStatus.Rows.Add(1, "Not Approved");
			dtStatus.Rows.Add(2, "Approved");
			dtStatus.Rows.Add(3, "All");
			DDL.BindDDLNew(dtStatus, cmbStatus, "Id", "Label", "Status", true);
			((UltraGridBase)cmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbStatus).Rows[3].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Datetypefill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbdatetype, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbdatetype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			int Id = Conversion.ToInt(cmbdatetype.Value);
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbdatetype.Value = Id;
				}
				else
				{
					((Control)(object)cmbdatetype).Text = string.Empty;
				}
			}
			((UltraGridBase)cmbdatetype).Rows[1].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbdatetype_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbdatetype.Value) == 1)
			{
				DateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbdatetype.Value) == 2)
			{
				DateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbdatetype.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				DateFrom.Value = first;
				DateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbdatetype.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				DateFrom.Value = first2;
				DateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbdatetype.Value) == 5)
			{
				DateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			waitForm.Show();
			GridFill();
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			waitForm.Close();
		}
	}

	private void GridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtGrid.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			obj.ExImInvoiceId = Conversion.ToInt(CmbInvoice.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			if (Conversion.ToInt(cmbDeliveryType.Value) > 0)
			{
				obj.Activity = Conversion.ToString((object)((Control)(object)cmbDeliveryType).Text);
			}
			obj.FromDate = Conversion.ToDateTime((object)DateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)DateTo.Value);
			obj.DocumentTypeId = DocumentTypeId;
			if (((Control)(object)cmbStatus).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbStatus).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			if (RadReffered.Checked)
			{
				obj.SkipZero = 1;
			}
			else if (RadNotReffered.Checked)
			{
				obj.SkipZero = 2;
			}
			if (RadOnlyfirstWeight.Checked)
			{
				obj.ActionId = 1;
			}
			else if (RadWithSecondWeight.Checked)
			{
				obj.ActionId = 2;
			}
			DataSet ds = DeliverySchedule.GetDeliveryOrderForApproval(obj);
			dtGrid = ds?.Tables[0];
			if (dtGrid.Rows.Count > 0)
			{
				DatatableHelper.ReplaceCompLogoColumn(dtGrid, (ds.Tables[1].Rows.Count > 0) ? ds.Tables[1] : null);
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("DocumentTypeId", typeof(int));
				dtHistory.Columns.Add("DocNo", typeof(int));
				dtHistory.Columns.Add("DocDate", typeof(DateTime));
				dtHistory.Columns.Add("D.Type", typeof(string));
				dtHistory.Columns.Add("GpId", typeof(int));
				dtHistory.Columns.Add("GpNo", typeof(int));
				dtHistory.Columns.Add("InvoiceNo", typeof(string));
				dtHistory.Columns.Add("SaleOrderId", typeof(int));
				dtHistory.Columns.Add("SaleOrderNo", typeof(string));
				dtHistory.Columns.Add("CustomerName", typeof(string));
				dtHistory.Columns.Add("ItemName", typeof(string));
				dtHistory.Columns.Add("UOM", typeof(string));
				dtHistory.Columns.Add("ItemQty", typeof(double));
				dtHistory.Columns.Add("NetWeight", typeof(double));
				dtHistory.Columns.Add("EbUnit", typeof(double));
				dtHistory.Columns.Add("EbTotal", typeof(double));
				dtHistory.Columns.Add("GrossWeight", typeof(double));
				dtHistory.Columns.Add("EntryUser", typeof(string));
				dtHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtHistory.Columns.Add("ModifyUser", typeof(string));
				dtHistory.Columns.Add("ModifyDate", typeof(string));
				dtHistory.Columns.Add("IsApproved", typeof(string));
				dtHistory.Columns.Add("Remarks", typeof(string));
				dtHistory.Columns.Add("Container", typeof(string));
				dtHistory.Columns.Add("NoOfAttachments", typeof(int));
				dtHistory.Clear();
				for (int i = 0; i < dtGrid.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["DocumentTypeId"], dtGrid.Rows[i]["DocNo"], dtGrid.Rows[i]["DocDate"], dtGrid.Rows[i]["DeliveryOrderType"], dtGrid.Rows[i]["GpId"], dtGrid.Rows[i]["GpNo"], dtGrid.Rows[i]["InvoiceNo"], dtGrid.Rows[i]["SaleOrderId"], dtGrid.Rows[i]["SaleOrderNo"], dtGrid.Rows[i]["CustomerName"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["PackUOM"], dtGrid.Rows[i]["LoadingQty"], dtGrid.Rows[i]["LoadingWeight"], dtGrid.Rows[i]["EbUnit"], dtGrid.Rows[i]["TotalPackingWeight"], dtGrid.Rows[i]["GrossWeight"], dtGrid.Rows[i]["EntryUserName"], dtGrid.Rows[i]["EntryDate"], dtGrid.Rows[i]["ModifyUserName"], dtGrid.Rows[i]["ModifyDate"], dtGrid.Rows[i]["IsApproved"], dtGrid.Rows[i]["LoadingRemarks"], dtGrid.Rows[i]["Container"], dtGrid.Rows[i]["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtHistory;
				grdHistory.RetrieveStructure();
				GridSetting();
			}
			else
			{
				grdHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["SaleOrderId"].Visible = false;
			grdHistory.RootTable.Columns["SaleOrderNo"].Visible = false;
			grdHistory.RootTable.Columns["IsApproved"].Visible = false;
			grdHistory.RootTable.Columns["GpId"].Visible = false;
			grdHistory.RootTable.Columns["GpNo"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdHistory.RootTable.Columns["D.Type"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdHistory.RootTable.Columns["InvoiceNo"].Width = Constants.InventoryConstants.InvoiceNoConstant;
			grdHistory.RootTable.Columns["SaleOrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdHistory.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdHistory.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			grdHistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdHistory.RootTable.Columns["EbUnit"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistory.RootTable.Columns["EbTotal"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistory.RootTable.Columns["GrossWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.ApprovedByConstant;
			grdHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdHistory.RootTable.Columns["Container"].Width = Constants.InventoryConstants.QtyConstant;
			grdHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdHistory.RootTable.HeaderLines = 2;
			grdHistory.RootTable.Columns["DocNo"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["DocDate"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["D.Type"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["GpNo"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["InvoiceNo"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["SaleOrderNo"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["CustomerName"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["ItemName"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["UOM"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["ItemQty"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["NetWeight"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["EbUnit"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["EbTotal"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["GrossWeight"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["EntryUser"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["EntryDate"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["ModifyUser"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["ModifyDate"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["IsApproved"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["Remarks"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["Container"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["NoOfAttachments"].HeaderAlignment = (TextAlignment)2;
			grdHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["EbUnit"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["EbTotal"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["EbTotal"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["GrossWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["GrossWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["GrossWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdHistory.RootTable.Columns["Print"].Width = 40;
			grdHistory.RootTable.Columns["Print"].Position = 0;
			grdHistory.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				if (e.Column.Key == "Print")
				{
					CommonServices.InvDeliveryOrderSlip(Conversion.ToInt(item.Cells["Id"].Value), 84);
				}
				if (e.Column.Key == "IsApproved" && MessageBox.Show("Are you sure to Approve Delivery Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList val = new ApprovalList
					{
						AllApprovalLists = new List<ApprovalList>()
					};
					ApprovalList detail = new ApprovalList();
					detail.OrganizationId = UserAccount.OrganizationId;
					detail.CompanyId = UserAccount.CompanyId;
					detail.EntryUser = UserAccount.ID;
					detail.Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
					detail.ReqType = "Approve";
					val.AllApprovalLists.Add(detail);
					InvDeliveryOrder.ApprovedRecordByDeliveryOrderId(val);
					MessageBox.Show("Record Approve Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0 && e.Column.Key == "GpNo")
			{
				CommonServices.OutwardGatePassWithWb(Conversion.ToInt(item.Cells["GpId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid == null || dtGrid.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "263-InvDeliveryOrderForApproval.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdHistory).Name);
			if (fs != null)
			{
				grdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			waitForm.Close();
			MessageBox.Show(ex.ToString());
		}
	}

	private void DeliveryOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnrefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbdatetype.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdHistory).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				cmbdatetype.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on DateType");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DateType in Delivery Order History");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdHistory.CurrentColumn != null)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				Conversion.ToInt((object)r.Cells["DocumentTypeId"].Value.ToString());
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.InvDeliveryOrderSlip(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value), 84);
				}
			}
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
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_2f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6b: Expected O, but got Unknown
		//IL_2f78: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f82: Expected O, but got Unknown
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
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnrefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.RadNotReffered = new System.Windows.Forms.RadioButton();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadWithSecondWeight = new System.Windows.Forms.RadioButton();
		this.RadOnlyfirstWeight = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.cmbdatetype = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.label9 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.CmbInvoice = new UltraCombo();
		this.cmbDeliveryType = new UltraCombo();
		this.cmbItem = new UltraCombo();
		this.cmbSupplier = new UltraCombo();
		this.cmbStatus = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.RadReffered = new System.Windows.Forms.RadioButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblCaption = new System.Windows.Forms.Label();
		this.RadAllRef = new System.Windows.Forms.RadioButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbdatetype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoice).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.toolStripSeparator1, this.btnnew, this.btnrefresh, this.btnPrint, this.btnShortCutKeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 27);
		this.toolStrip1.TabIndex = 7;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnrefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnrefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnrefresh.Name = "btnrefresh";
		this.btnrefresh.Size = new System.Drawing.Size(80, 24);
		this.btnrefresh.Text = "&Refresh";
		this.btnrefresh.Click += new System.EventHandler(btnrefresh_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "263-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 29);
		this.panel2.TabIndex = 0;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.RadNotReffered);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.RadReffered);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.RadAllRef);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 140);
		this.panel3.TabIndex = 0;
		this.RadNotReffered.AutoSize = true;
		this.RadNotReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadNotReffered.Location = new System.Drawing.Point(847, 80);
		this.RadNotReffered.Name = "RadNotReffered";
		this.RadNotReffered.Size = new System.Drawing.Size(108, 23);
		this.RadNotReffered.TabIndex = 3;
		this.RadNotReffered.Text = "Not Reffered";
		this.RadNotReffered.UseVisualStyleBackColor = true;
		this.groupBox1.Controls.Add(this.RadWithSecondWeight);
		this.groupBox1.Controls.Add(this.RadOnlyfirstWeight);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdatetype);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.DateFrom);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoice);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDeliveryType);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbStatus);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.DateTo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 30);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(836, 103);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadWithSecondWeight.AutoSize = true;
		this.RadWithSecondWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.RadWithSecondWeight.Location = new System.Drawing.Point(728, 48);
		this.RadWithSecondWeight.Name = "RadWithSecondWeight";
		this.RadWithSecondWeight.Size = new System.Drawing.Size(104, 21);
		this.RadWithSecondWeight.TabIndex = 11;
		this.RadWithSecondWeight.Text = "With 2nd Wt";
		this.RadWithSecondWeight.UseVisualStyleBackColor = true;
		this.RadOnlyfirstWeight.AutoSize = true;
		this.RadOnlyfirstWeight.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.RadOnlyfirstWeight.Location = new System.Drawing.Point(728, 31);
		this.RadOnlyfirstWeight.Name = "RadOnlyfirstWeight";
		this.RadOnlyfirstWeight.Size = new System.Drawing.Size(96, 21);
		this.RadOnlyfirstWeight.TabIndex = 10;
		this.RadOnlyfirstWeight.Text = "1st Wt Only";
		this.RadOnlyfirstWeight.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(728, 14);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(41, 21);
		this.RadAll.TabIndex = 9;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "All";
		this.RadAll.UseVisualStyleBackColor = true;
		this.cmbdatetype.AllowNull = (DefaultableBoolean)1;
		this.cmbdatetype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdatetype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdatetype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdatetype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbdatetype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdatetype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdatetype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbdatetype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdatetype).Location = new System.Drawing.Point(70, 13);
		((System.Windows.Forms.Control)(object)this.cmbdatetype).Name = "cmbdatetype";
		((System.Windows.Forms.Control)(object)this.cmbdatetype).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.cmbdatetype).TabIndex = 0;
		this.cmbdatetype.ValueChanged += new System.EventHandler(cmbdatetype_ValueChanged);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 19);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(60, 15);
		this.label8.TabIndex = 209;
		this.label8.Text = "Date Type";
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(70, 44);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(134, 23);
		this.DateFrom.TabIndex = 1;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(485, 48);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(65, 15);
		this.label9.TabIndex = 208;
		this.label9.Text = "Invoice No";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(485, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 15);
		this.label6.TabIndex = 208;
		this.label6.Text = "Delivery Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(209, 48);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(32, 15);
		this.label7.TabIndex = 200;
		this.label7.Text = "Item";
		this.CmbInvoice.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoice.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoice.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoice.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoice).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoice).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoice).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoice).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoice.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoice).Location = new System.Drawing.Point(566, 42);
		((System.Windows.Forms.Control)(object)this.CmbInvoice).Name = "CmbInvoice";
		((System.Windows.Forms.Control)(object)this.CmbInvoice).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoice).TabIndex = 7;
		this.cmbDeliveryType.AllowNull = (DefaultableBoolean)1;
		this.cmbDeliveryType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDeliveryType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDeliveryType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbDeliveryType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDeliveryType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDeliveryType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDeliveryType).Location = new System.Drawing.Point(566, 13);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryType).Name = "cmbDeliveryType";
		((System.Windows.Forms.Control)(object)this.cmbDeliveryType).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.cmbDeliveryType).TabIndex = 6;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance31).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(269, 42);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 4;
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance29).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(269, 13);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 3;
		this.cmbStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbStatus).Location = new System.Drawing.Point(269, 70);
		((System.Windows.Forms.Control)(object)this.cmbStatus).Name = "cmbStatus";
		((System.Windows.Forms.Control)(object)this.cmbStatus).Size = new System.Drawing.Size(211, 26);
		((System.Windows.Forms.Control)(object)this.cmbStatus).TabIndex = 5;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.Location = new System.Drawing.Point(660, 70);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(63, 26);
		this.btnshow.TabIndex = 8;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(209, 76);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(40, 15);
		this.label1.TabIndex = 205;
		this.label1.Text = "Status";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(209, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 15);
		this.label2.TabIndex = 201;
		this.label2.Text = "Customer";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 76);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 203;
		this.label5.Text = "To Date";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 48);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 15);
		this.label4.TabIndex = 202;
		this.label4.Text = "From Date";
		this.DateTo.CustomFormat = "dd-MMM-yy";
		this.DateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateTo.Location = new System.Drawing.Point(70, 72);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(134, 23);
		this.DateTo.TabIndex = 2;
		this.RadReffered.AutoSize = true;
		this.RadReffered.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadReffered.Location = new System.Drawing.Point(847, 62);
		this.RadReffered.Name = "RadReffered";
		this.RadReffered.Size = new System.Drawing.Size(80, 23);
		this.RadReffered.TabIndex = 2;
		this.RadReffered.Text = "Reffered";
		this.RadReffered.UseVisualStyleBackColor = true;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.lblCaption);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 28);
		this.panel5.TabIndex = 206;
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.Teal;
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(7, 5);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(201, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Delivery Order Register";
		this.RadAllRef.AutoSize = true;
		this.RadAllRef.Checked = true;
		this.RadAllRef.Font = new System.Drawing.Font("Segoe UI Semibold", 10f, System.Drawing.FontStyle.Bold);
		this.RadAllRef.Location = new System.Drawing.Point(847, 44);
		this.RadAllRef.Name = "RadAllRef";
		this.RadAllRef.Size = new System.Drawing.Size(44, 23);
		this.RadAllRef.TabIndex = 1;
		this.RadAllRef.TabStop = true;
		this.RadAllRef.Text = "All";
		this.RadAllRef.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 169);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 416);
		this.panel4.TabIndex = 2;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		this.grdHistory.DefaultFilterRowComparison = (FilterConditionOperator)7;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(964, 388);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(964, 28);
		this.panel6.TabIndex = 207;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(924, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label3.Location = new System.Drawing.Point(7, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(44, 16);
		this.label3.TabIndex = 0;
		this.label3.Text = "Detail";
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 585);
		this.panel1.TabIndex = 1;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 585);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DeliveryOrderHistory";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(DeliveryOrderHistory_FormClosing);
		base.Load += new System.EventHandler(frmStockWithSupplierHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DeliveryOrderHistory_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbdatetype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoice).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDeliveryType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbStatus).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
