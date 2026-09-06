using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.pcc.Approvals
{

public class DeliveryOrderForApproval_Concrete : Form
{
	public int DocumentTypeId;

	private bool AllowEditOnVoucherNoinReports;

	private DataTable grdlst = new DataTable();

	private DataTable dtPending = new DataTable();

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private Panel panel4;

	private ToolStrip toolStrip3;

	private Label label23;

	private CtrlGrdBar ctrlGrdBar1;

	private Button btnshow;

	internal UltraCombo cmbApproveStatus;

	private Label label4;

	private Button btnApproved;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private Panel panel6;

	internal GridEX GridDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label9;

	internal GridEX grdsaleorder;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label8;

	private TabPage tabPage4;

	private Panel panel7;

	private Panel panel5;

	private Panel panel8;

	private Label label5;

	private Panel panel9;

	private Label label10;

	private Button btnPendingForView;

	private Panel panel11;

	private Label label12;

	private Button btnApprovePendingRecord;

	private Panel panel15;

	private Label label11;

	private CtrlGrdBar ctrlGrdBar4;

	internal GridEX grdPendingViewVoucher;

	private Panel panel17;

	private Label label13;

	internal GridEX GridDetailForPendingView;

	private ToolStripButton btnNew;

	private GroupBox groupBox1;

	private ToolStripButton btnShortCutKeys;

	internal UltraCombo cmbItem;

	private Label label1;

	private Label label7;

	internal UltraCombo cmbvehicle;

	internal UltraCombo cmbSupplier;

	private Label label2;

	private TextBox txttodoc;

	private Label label3;

	private TextBox txtfromdoc;

	private DateTimePicker DateFrom;

	private Label label14;

	private Label label15;

	internal UltraCombo cmbrefparty;

	private DateTimePicker DateTo;

	private Label label16;

	internal UltraCombo cmbitemtype;

	private Label label18;

	private Label label19;

	internal UltraCombo cmbitemcat;

	internal UltraCombo cmbperemeter;

	private Label label20;

	internal UltraCombo cmbparentcat;

	public DeliveryOrderForApproval_Concrete(UserAccount user)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmSaleOrderHistory_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			ComboBind();
			Datetypefill();
			ApproveStatusFill();
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
			if (!Conversion.CheckDateTimeNull(RequestedFromDate) && !Conversion.CheckDateTimeNull(RequestedToDate))
			{
				DateFrom.Value = Convert.ToDateTime(RequestedFromDate);
				DateTo.Value = Convert.ToDateTime(RequestedToDate);
			}
			else
			{
				((UltraGridBase)cmbperemeter).Rows[5].Activate();
			}
			gridHisory();
			GetPendingSaleOrder();
			cmbperemeter.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = InvDeliveryOrder.GetDataForDropDownFromDeliveryOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtpartyname = new DataTable();
			dtpartyname.Columns.Add("Id", typeof(int));
			dtpartyname.Columns.Add("name", typeof(string));
			DataTable dtRefParty = new DataTable();
			dtRefParty.Columns.Add("Id", typeof(int));
			dtRefParty.Columns.Add("name", typeof(string));
			DataTable dtItemName = new DataTable();
			dtItemName.Columns.Add("Id", typeof(int));
			dtItemName.Columns.Add("name", typeof(string));
			DataTable dtitemType = new DataTable();
			dtitemType.Columns.Add("Id", typeof(int));
			dtitemType.Columns.Add("name", typeof(string));
			DataTable dtItemCat = new DataTable();
			dtItemCat.Columns.Add("Id", typeof(int));
			dtItemCat.Columns.Add("name", typeof(string));
			DataTable dtParentCat = new DataTable();
			dtParentCat.Columns.Add("Id", typeof(int));
			dtParentCat.Columns.Add("name", typeof(string));
			DataTable dtvehicleno = new DataTable();
			dtvehicleno.Columns.Add("Id", typeof(int));
			dtvehicleno.Columns.Add("name", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Customer")
				{
					dtpartyname.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ReferencePartyName")
				{
					dtRefParty.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
				{
					dtItemName.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemType")
				{
					dtitemType.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ItemCategory")
				{
					dtItemCat.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "ParentCategory")
				{
					dtParentCat.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dt.Rows[i]["Activity"]) == "VehicleNo")
				{
					dtvehicleno.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
				}
			}
			if (dtRefParty.Rows.Count > 0)
			{
				DDL.BindDDL(dtRefParty, cmbrefparty, "Id", "name", "Ref Party", ZeroIndex: false);
				((UltraGridBase)cmbrefparty).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtpartyname.Rows.Count > 0)
			{
				DDL.BindDDL(dtpartyname, cmbSupplier, "Id", "name", "Ref Party", ZeroIndex: false);
				((UltraGridBase)cmbSupplier).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemName.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemName, cmbItem, "Id", "name", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtitemType.Rows.Count > 0)
			{
				DDL.BindDDL(dtitemType, cmbitemtype, "Id", "name", "Item Type", ZeroIndex: false);
				((UltraGridBase)cmbitemtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItemCat.Rows.Count > 0)
			{
				DDL.BindDDL(dtItemCat, cmbitemcat, "Id", "name", "Item Category", ZeroIndex: false);
				((UltraGridBase)cmbitemcat).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtParentCat.Rows.Count > 0)
			{
				DDL.BindDDL(dtParentCat, cmbparentcat, "Id", "name", "Parent Category", ZeroIndex: false);
				((UltraGridBase)cmbparentcat).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtvehicleno.Rows.Count > 0)
			{
				DDL.BindDDL(dtvehicleno, cmbvehicle, "Id", "name", "Vehicle No", ZeroIndex: false);
				((UltraGridBase)cmbvehicle).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
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
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Date Type", ZeroIndex: true);
			((UltraGridBase)cmbperemeter).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ApproveStatusFill()
	{
		try
		{
			DataTable dtStatus = new DataTable();
			dtStatus.Columns.Add("Id");
			dtStatus.Columns.Add("Label");
			dtStatus.Rows.Add(1, "Not Approved");
			dtStatus.Rows.Add(2, "Approved");
			dtStatus.Rows.Add(3, "All");
			DDL.BindDDLNew(dtStatus, cmbApproveStatus, "Id", "Label", "Status", false);
			((UltraGridBase)cmbApproveStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			txtfromdoc.Text = string.Empty;
			txttodoc.Text = string.Empty;
			((Control)(object)cmbSupplier).Text = string.Empty;
			((Control)(object)cmbparentcat).Text = string.Empty;
			((Control)(object)cmbitemcat).Text = string.Empty;
			((Control)(object)cmbitemtype).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbrefparty).Text = string.Empty;
			((Control)(object)cmbvehicle).Text = string.Empty;
			((UltraGridBase)cmbApproveStatus).Rows[0].Activate();
			Datetypefill();
			cmbperemeter.Focus();
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
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SaleOrderForApproval_Concrete_FormClosed(object sender, FormClosedEventArgs e)
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
			if (base.ActiveControl.Name == "tabControl2")
			{
				cmbperemeter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click_1(object sender, EventArgs e)
	{
		try
		{
			gridHisory();
			GetPendingSaleOrder();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void gridHisory()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			grdlst.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDocNo = Conversion.ToInt((object)txtfromdoc.Text);
			obj.ToDocNo = Conversion.ToInt((object)txttodoc.Text);
			obj.FromDate = Conversion.ToDateTime((object)DateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)DateTo.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			obj.InventoryParentCategories = Conversion.ToInt(cmbparentcat.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbitemcat.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbitemtype.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.ReferencePartyName = Conversion.ToString((object)((Control)(object)cmbrefparty).Text);
			obj.VehicleNo = Conversion.ToString((object)((Control)(object)cmbvehicle).Text);
			obj.PendingForView = 1;
			if (((Control)(object)cmbApproveStatus).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			grdlst = InvDeliveryOrder.GetDeliveryOrderForApproval(obj);
			if (grdlst.Rows.Count > 0)
			{
				DataTable dt1 = new DataTable();
				dt1.Columns.Add("InvDeliveryOrderId", typeof(int));
				dt1.Columns.Add("DocNo", typeof(int));
				dt1.Columns.Add("DocDate", typeof(DateTime));
				dt1.Columns.Add("RefrenenceNo", typeof(string));
				dt1.Columns.Add("VehicleType", typeof(string));
				dt1.Columns.Add("VehicleNo", typeof(string));
				dt1.Columns.Add("DoTotalQty", typeof(double));
				dt1.Columns.Add("EntryDate", typeof(DateTime));
				dt1.Columns.Add("EntryUserName", typeof(string));
				dt1.Columns.Add("ModifyDate", typeof(DateTime));
				dt1.Columns.Add("ModifyUserName", typeof(string));
				dt1.Columns.Add("RemarksHeader", typeof(string));
				dt1.Columns.Add("ApprovedStatus", typeof(string));
				dt1.Clear();
				int i;
				for (i = 0; i < grdlst.Rows.Count; i++)
				{
					if (!dt1.AsEnumerable().Any((DataRow rr) => rr.Field<int>("InvDeliveryOrderId") == Conversion.ToInt(grdlst.Rows[i]["InvDeliveryOrderId"])))
					{
						dt1.Rows.Add(grdlst.Rows[i]["InvDeliveryOrderId"], grdlst.Rows[i]["DocNo"], Conversion.ToDateTime(grdlst.Rows[i]["DocDate"]).ToShortDateString(), grdlst.Rows[i]["RefrenenceNo"], grdlst.Rows[i]["VehicleType"], grdlst.Rows[i]["VehicleNo"], grdlst.Rows[i]["DoTotalQty"], grdlst.Rows[i]["EntryDate"], grdlst.Rows[i]["EntryUserName"], grdlst.Rows[i]["ModifyDate"], grdlst.Rows[i]["ModifyUserName"], grdlst.Rows[i]["RemarksHeader"], grdlst.Rows[i]["IsApproved"]);
					}
				}
				grdsaleorder.DataSource = dt1;
				grdsaleorder.RetrieveStructure();
				GridSettings();
			}
			else
			{
				grdsaleorder.DataSource = null;
				GridDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdsaleorder.RootTable.Columns["InvDeliveryOrderId"].Visible = false;
			grdsaleorder.RootTable.Columns["DoTotalQty"].AggregateFunction = (AggregateFunction)2;
			grdsaleorder.RootTable.Columns["DoTotalQty"].TextAlignment = (TextAlignment)3;
			grdsaleorder.RootTable.Columns["DoTotalQty"].FormatString = "#,##0.##";
			grdsaleorder.RootTable.Columns["DoTotalQty"].TotalFormatString = "#,##0.##";
			grdsaleorder.RootTable.GroupTotals = (GroupTotals)2;
			CommonServices.GridAutoAdjustmentNew(grdsaleorder);
			grdsaleorder.RootTable.Columns.Add("Print");
			grdsaleorder.RootTable.Columns["Print"].Caption = "Slip";
			grdsaleorder.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdsaleorder.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdsaleorder.RootTable.Columns["Print"].ButtonText = "Slip";
			grdsaleorder.RootTable.Columns["Print"].Width = 60;
			grdsaleorder.RootTable.Columns["Print"].Position = 0;
			grdsaleorder.FrozenColumns = 1;
			if (((Control)(object)cmbApproveStatus).Text == "Not Approved")
			{
				grdsaleorder.RootTable.Columns.Add("IsApproved");
				grdsaleorder.RootTable.Columns["IsApproved"].Caption = "Approve";
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonStyle = (ButtonStyle)5;
				grdsaleorder.RootTable.Columns["IsApproved"].ButtonText = "Approve";
				grdsaleorder.RootTable.Columns["IsApproved"].Width = 60;
				grdsaleorder.RootTable.Columns.Add("Select");
				grdsaleorder.RootTable.Columns["Select"].ActAsSelector = true;
				grdsaleorder.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdsaleorder.RootTable.Columns["Select"].Width = 40;
				grdsaleorder.RootTable.Columns["Select"].Position = 0;
				grdsaleorder.RootTable.Columns["IsApproved"].Position = 1;
				grdsaleorder.RootTable.Columns["Print"].Position = 2;
				grdsaleorder.FrozenColumns = 3;
			}
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Print")
			{
				CommonServices.DeliveryOrderConcreteSlipCustomerWise(Conversion.ToInt((object)grdsaleorder.CurrentRow.Cells["InvDeliveryOrderId"].Value.ToString()));
			}
			else if (e.Column.Key == "IsApproved")
			{
				GridColumnApproveButtonClick();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			int Id = int.Parse(item.Cells["InvDeliveryOrderId"].Value.ToString());
			if (e.Column.Key == "NoOfAttachments" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
			}
			if (e.Column.Key == "CustomerName" && item != null && (int)item.RowType == 0)
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(grdsaleorder.CurrentRow.Cells["CustomerId"].Value), DateFrom.Value, DateTo.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsaleorder_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdsaleorder.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (item.Cells.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("InvDeliveryOrderId", typeof(int));
					dt.Columns.Add("SaleOrderId", typeof(int));
					dt.Columns.Add("SaleOrderDetailId", typeof(int));
					dt.Columns.Add("SaleOrderNo", typeof(int));
					dt.Columns.Add("SupplierCustomerId", typeof(int));
					dt.Columns.Add("CustomerName", typeof(string));
					dt.Columns.Add("ReferencePartyName", typeof(string));
					dt.Columns.Add("ReferencePartyAddress", typeof(string));
					dt.Columns.Add("ReferencePartyCellNo", typeof(string));
					dt.Columns.Add("WareHouseName", typeof(string));
					dt.Columns.Add("ItemId", typeof(int));
					dt.Columns.Add("ItemName", typeof(string));
					dt.Columns.Add("ItemAttributeVarient", typeof(string));
					dt.Columns.Add("VarientEquivalent", typeof(int));
					dt.Columns.Add("JobLot", typeof(string));
					dt.Columns.Add("DoQty", typeof(double));
					dt.Columns.Add("ItemWeight", typeof(double));
					dt.Columns.Add("DoNetWeight", typeof(double));
					dt.Columns.Add("CityName", typeof(string));
					dt.Columns.Add("RemarksDetail", typeof(string));
					DataTable resultRows = (from row in grdlst.AsEnumerable()
						where row.Field<int>("InvDeliveryOrderId") == Conversion.ToInt(item.Cells["InvDeliveryOrderId"].Value)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dt.Rows.Add(resultRows.Rows[i]["InvDeliveryOrderId"], resultRows.Rows[i]["SaleOrderId"], resultRows.Rows[i]["SaleOrderDetailId"], resultRows.Rows[i]["OrderNo"], resultRows.Rows[i]["SupplierCustomerId"], resultRows.Rows[i]["CustomerName"], resultRows.Rows[i]["ReferencePartyName"], resultRows.Rows[i]["ReferencePartyAddress"], resultRows.Rows[i]["ReferencePartyCellNo"], resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemAttributeVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["DoQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["DoNetWeight"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
					}
					GridDetail.DataSource = dt;
					GridDetail.RetrieveStructure();
					GridDetailSetting();
				}
			}
			else
			{
				GridDetail.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSetting()
	{
		try
		{
			GridDetail.RootTable.Columns["InvDeliveryOrderId"].Visible = false;
			GridDetail.RootTable.Columns["SaleOrderId"].Visible = false;
			GridDetail.RootTable.Columns["SaleOrderDetailId"].Visible = false;
			GridDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridDetail.RootTable.Columns["ItemId"].Visible = false;
			GridDetail.RootTable.Columns["SaleOrderNo"].ColumnType = (ColumnType)5;
			GridDetail.RootTable.Columns["DoQty"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["DoNetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetail.RootTable.Columns["DoQty"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["DoNetWeight"].TextAlignment = (TextAlignment)3;
			GridDetail.RootTable.Columns["DoQty"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["DoNetWeight"].FormatString = "#,##0.##";
			GridDetail.RootTable.Columns["DoQty"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			GridDetail.RootTable.Columns["DoNetWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(GridDetail);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetail_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = GridDetail.CurrentRow;
			_ = e.Column.Key == "ItemName";
			if (e.Column.Key == "SaleOrderNo")
			{
				CommonServices.SaleOrderSlipConcrete(Conversion.ToInt(item.Cells["SaleOrderId"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnApproved_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Approved" && MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.BranchesId = UserAccount.BranchesId;
					item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
					item.Id = Conversion.ToInt((object)r.Cells["InvDeliveryOrderId"].Value.ToString());
					item.ReqType = "Approve";
					item.EntryUser = UserAccount.ID;
					obj.AllApprovalLists.Add(item);
				}
				InvDeliveryOrder.UpdateDelivertOrderStatusById(obj);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridColumnApproveButtonClick()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.Id = Conversion.ToInt((object)grdsaleorder.CurrentRow.Cells["InvDeliveryOrderId"].Value.ToString());
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvDeliveryOrder.UpdateDelivertOrderStatusById(val);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPendingSaleOrder()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FromDocNo = Conversion.ToInt((object)txtfromdoc.Text);
			obj.ToDocNo = Conversion.ToInt((object)txttodoc.Text);
			obj.FromDate = Conversion.ToDateTime((object)DateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)DateTo.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplier.Value);
			obj.InventoryParentCategories = Conversion.ToInt(cmbparentcat.Value);
			obj.ItemCategoryId = Conversion.ToInt(cmbitemcat.Value);
			obj.ItemTypeId = Conversion.ToInt(cmbitemtype.Value);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.ReferencePartyName = Conversion.ToString((object)((Control)(object)cmbrefparty).Text);
			obj.VehicleNo = Conversion.ToString((object)((Control)(object)cmbvehicle).Text);
			obj.PendingForView = 2;
			if (((Control)(object)cmbApproveStatus).Text == "Approved")
			{
				obj.IsApproved = true;
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Approved")
			{
				obj.IsApproved = false;
			}
			if (((Control)(object)cmbApproveStatus).Text == "All")
			{
				obj.ApprovedFilter = "All";
			}
			dtPending = InvDeliveryOrder.GetDeliveryOrderForApproval(obj);
			if (dtPending.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("InvDeliveryOrderId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("RefrenenceNo", typeof(string));
				dt.Columns.Add("VehicleType", typeof(string));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("DoTotalQty", typeof(double));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("ApprovedStatus", typeof(string));
				dt.Clear();
				int i;
				for (i = 0; i < dtPending.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("InvDeliveryOrderId") == Conversion.ToInt(dtPending.Rows[i]["InvDeliveryOrderId"])))
					{
						dt.Rows.Add(dtPending.Rows[i]["InvDeliveryOrderId"], dtPending.Rows[i]["DocNo"], Conversion.ToDateTime(dtPending.Rows[i]["DocDate"]).ToShortDateString(), dtPending.Rows[i]["RefrenenceNo"], dtPending.Rows[i]["VehicleType"], dtPending.Rows[i]["VehicleNo"], dtPending.Rows[i]["DoTotalQty"], dtPending.Rows[i]["EntryDate"], dtPending.Rows[i]["EntryUserName"], dtPending.Rows[i]["ModifyDate"], dtPending.Rows[i]["ModifyUserName"], dtPending.Rows[i]["RemarksHeader"], dtPending.Rows[i]["IsApproved"]);
					}
				}
				grdPendingViewVoucher.DataSource = dt;
				grdPendingViewVoucher.RetrieveStructure();
				GridSettingsForPending();
			}
			else
			{
				grdPendingViewVoucher.ClearStructure();
				GridDetailForPendingView.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettingsForPending()
	{
		try
		{
			grdPendingViewVoucher.RootTable.Columns["InvDeliveryOrderId"].Visible = false;
			grdPendingViewVoucher.RootTable.Columns["DoTotalQty"].AggregateFunction = (AggregateFunction)2;
			grdPendingViewVoucher.RootTable.Columns["DoTotalQty"].TextAlignment = (TextAlignment)3;
			grdPendingViewVoucher.RootTable.Columns["DoTotalQty"].FormatString = "#,##0.##";
			grdPendingViewVoucher.RootTable.Columns["DoTotalQty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdPendingViewVoucher);
			grdPendingViewVoucher.RootTable.Columns.Add("Slip");
			grdPendingViewVoucher.RootTable.Columns["Slip"].Caption = "Slip";
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			grdPendingViewVoucher.RootTable.Columns["Slip"].ButtonText = "Slip";
			grdPendingViewVoucher.RootTable.Columns["Slip"].Width = 60;
			if (((Control)(object)cmbApproveStatus).Text == "Not Approved")
			{
				grdPendingViewVoucher.RootTable.Columns.Add("Select");
				grdPendingViewVoucher.RootTable.Columns["Select"].ActAsSelector = true;
				grdPendingViewVoucher.RootTable.Columns["Select"].UseHeaderSelector = true;
				grdPendingViewVoucher.RootTable.Columns["Select"].Width = 40;
				grdPendingViewVoucher.RootTable.Columns["Select"].Position = 0;
				grdPendingViewVoucher.RootTable.Columns.Add("Approve");
				grdPendingViewVoucher.RootTable.Columns["Approve"].Caption = "Approve";
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
				grdPendingViewVoucher.RootTable.Columns["Approve"].ButtonText = "Approve";
				grdPendingViewVoucher.RootTable.Columns["Approve"].Width = 60;
				grdPendingViewVoucher.RootTable.Columns["Approve"].Position = 1;
				grdPendingViewVoucher.RootTable.Columns["Slip"].Position = 2;
				grdPendingViewVoucher.FrozenColumns = 3;
			}
			else
			{
				grdPendingViewVoucher.RootTable.Columns["Slip"].Position = 0;
				grdPendingViewVoucher.FrozenColumns = 1;
			}
			ctrlGrdBar4_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			int Id = Convert.ToInt32(grdPendingViewVoucher.CurrentRow.Cells["InvDeliveryOrderId"].Value);
			if (e.Column.Key == "Slip")
			{
				CommonServices.DeliveryOrderConcreteSlipCustomerWise(Id);
			}
			if (e.Column.Key == "Approve")
			{
				ApproveOrder(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			Conversion.ToInt(item.Cells["InvDeliveryOrderId"].Value);
			if (e.Column.Key == "CustomerName")
			{
				CommonServices.GoToCustomerLedgerFromLinkedEvent(Conversion.ToInt(item.Cells["CustomerId"].Value), DateFrom.Value, DateTo.Value);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPendingViewVoucher_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPendingViewVoucher.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("InvDeliveryOrderId", typeof(int));
				dt.Columns.Add("SaleOrderId", typeof(int));
				dt.Columns.Add("SaleOrderDetailId", typeof(int));
				dt.Columns.Add("SaleOrderNo", typeof(int));
				dt.Columns.Add("SupplierCustomerId", typeof(int));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("ReferencePartyName", typeof(string));
				dt.Columns.Add("ReferencePartyAddress", typeof(string));
				dt.Columns.Add("ReferencePartyCellNo", typeof(string));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemAttributeVarient", typeof(string));
				dt.Columns.Add("VarientEquivalent", typeof(int));
				dt.Columns.Add("JobLot", typeof(string));
				dt.Columns.Add("DoQty", typeof(double));
				dt.Columns.Add("ItemWeight", typeof(double));
				dt.Columns.Add("DoNetWeight", typeof(double));
				dt.Columns.Add("CityName", typeof(string));
				dt.Columns.Add("RemarksDetail", typeof(string));
				DataTable resultRows = (from row in dtPending.AsEnumerable()
					where row.Field<int>("InvDeliveryOrderId") == Conversion.ToInt(item.Cells["InvDeliveryOrderId"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dt.Rows.Add(resultRows.Rows[i]["InvDeliveryOrderId"], resultRows.Rows[i]["SaleOrderId"], resultRows.Rows[i]["SaleOrderDetailId"], resultRows.Rows[i]["OrderNo"], resultRows.Rows[i]["SupplierCustomerId"], resultRows.Rows[i]["CustomerName"], resultRows.Rows[i]["ReferencePartyName"], resultRows.Rows[i]["ReferencePartyAddress"], resultRows.Rows[i]["ReferencePartyCellNo"], resultRows.Rows[i]["WareHouseName"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemAttributeVarient"], resultRows.Rows[i]["VarientEquivalent"], resultRows.Rows[i]["JobLotDescription"], resultRows.Rows[i]["DoQty"], resultRows.Rows[i]["ItemWeight"], resultRows.Rows[i]["DoNetWeight"], resultRows.Rows[i]["CityName"], resultRows.Rows[i]["RemarksDetail"]);
				}
				GridDetailForPendingView.DataSource = dt;
				GridDetailForPendingView.RetrieveStructure();
				GridDetailSettingForPendingRecord();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSettingForPendingRecord()
	{
		try
		{
			GridDetailForPendingView.RootTable.Columns["InvDeliveryOrderId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["SaleOrderId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["SaleOrderDetailId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["ItemId"].Visible = false;
			GridDetailForPendingView.RootTable.Columns["SaleOrderNo"].ColumnType = (ColumnType)5;
			GridDetailForPendingView.RootTable.Columns["DoQty"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["DoNetWeight"].AggregateFunction = (AggregateFunction)2;
			GridDetailForPendingView.RootTable.Columns["DoQty"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["DoNetWeight"].TextAlignment = (TextAlignment)3;
			GridDetailForPendingView.RootTable.Columns["DoQty"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["DoNetWeight"].FormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["DoQty"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			GridDetailForPendingView.RootTable.Columns["DoNetWeight"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(GridDetailForPendingView);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailForPendingView_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = GridDetailForPendingView.CurrentRow;
			if (e.Column.Key == "SaleOrderNo")
			{
				int OrderNo = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
				Conversion.ToInt(r.Cells["ItemId"].Value);
				CommonServices.SaleOrderSlipConcrete(OrderNo);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ApproveOrder(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.BranchesId = UserAccount.BranchesId;
				item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				item.Id = Id;
				item.ReqType = "Approve";
				item.EntryUser = UserAccount.ID;
				val.AllApprovalLists.Add(item);
				InvDeliveryOrder.UpdateDelivertOrderStatusById(val);
				MessageBox.Show("Record Approve Successfully");
				gridHisory();
				GetPendingSaleOrder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPendingForView_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grdsaleorder.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Move Order in PendingGrid?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdsaleorder.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						item.Id = Conversion.ToInt(r.Cells["InvDeliveryOrderId"].Value);
						item.ReqType = "PendingForView";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					InvDeliveryOrder.UpdateDelivertOrderStatusById(obj);
					MessageBox.Show("Record Updated Successfully");
					gridHisory();
					GetPendingSaleOrder();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprovePendingRecord_Click(object sender, EventArgs e)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (grdPendingViewVoucher.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select check box first");
			}
			if (((Control)(object)cmbApproveStatus).Text == "Not Approved")
			{
				if (MessageBox.Show("Are you sure to Approve Order?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					ApprovalList obj = new ApprovalList();
					obj.AllApprovalLists = new List<ApprovalList>();
					GridEXRow[] checkedRows = grdPendingViewVoucher.GetCheckedRows();
					foreach (GridEXRow r in checkedRows)
					{
						ApprovalList item = new ApprovalList();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.BranchesId = UserAccount.BranchesId;
						item.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
						item.Id = Conversion.ToInt(r.Cells["InvDeliveryOrderId"].Value);
						item.ReqType = "Approve";
						item.EntryUser = UserAccount.ID;
						obj.AllApprovalLists.Add(item);
					}
					InvDeliveryOrder.UpdateDelivertOrderStatusById(obj);
					MessageBox.Show("Record Approve Successfully");
					gridHisory();
					GetPendingSaleOrder();
				}
				return;
			}
			throw new Exception("Please Select Not Approved status first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridDetailForPendingView_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Control && e.KeyCode == Keys.Space)
			{
				GridEXRow r = GridDetailForPendingView.CurrentRow;
				if (r != null && (int)r.RowType == 0 && GridDetailForPendingView.CurrentColumn != null && GridDetailForPendingView.CurrentColumn.Key == "SaleOrderNo")
				{
					int printId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
					Conversion.ToInt(r.Cells["ItemId"].Value);
					CommonServices.SaleOrderSlipConcrete(printId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdsaleorder_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdsaleorder.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = int.Parse(item.Cells["InvDeliveryOrderId"].Value.ToString());
			if (grdsaleorder.CurrentColumn != null)
			{
				if (grdsaleorder.CurrentColumn.Key == "Print")
				{
					CommonServices.DeliveryOrderConcreteSlipCustomerWise(Id);
				}
				else if (grdsaleorder.CurrentColumn.Key == "IsApproved")
				{
					GridColumnApproveButtonClick();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmSaleOrderHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControl2.SelectedIndex == 0)
				{
					if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGrid")
					{
						((Control)(object)GridDetail).Focus();
					}
					else
					{
						((Control)(object)grdsaleorder).Focus();
					}
				}
				else if (Conversion.ToString(base.ActiveControl.Tag) == "MasterGridPendingView")
				{
					((Control)(object)GridDetailForPendingView).Focus();
				}
				else
				{
					((Control)(object)grdPendingViewVoucher).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.A)
			{
				btnApproved_Click(null, null);
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

	private void GridDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = GridDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && GridDetail.CurrentColumn != null)
			{
				_ = GridDetail.CurrentColumn.Key == "ItemName";
				if (GridDetail.CurrentColumn.Key == "SaleOrderNo")
				{
					CommonServices.SaleOrderSlipConcrete(Conversion.ToInt(r.Cells["SaleOrderId"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdPendingViewVoucher_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdPendingViewVoucher.CurrentRow;
			if (r != null && (int)r.RowType == 0 && grdPendingViewVoucher.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["InvDeliveryOrderId"].Value);
				if (grdPendingViewVoucher.CurrentColumn.Key == "Slip")
				{
					CommonServices.DeliveryOrderConcreteSlipCustomerWise(Id);
				}
				else if (grdPendingViewVoucher.CurrentColumn.Key == "Approve")
				{
					ApproveOrder(Id);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			dt.Rows.Add("Ctrl+S", "For Show Record");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid History");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Date Type");
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetail).Name);
			if (fs != null)
			{
				GridDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdsaleorder).Name);
			if (fs != null)
			{
				grdsaleorder.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar4_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdPendingViewVoucher).Name);
			if (fs != null)
			{
				grdPendingViewVoucher.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				DateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				DateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				DateFrom.Value = first;
				DateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				DateFrom.Value = first2;
				DateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				DateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfromdoc_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txttodoc_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Expected O, but got Unknown
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0385: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Expected O, but got Unknown
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Expected O, but got Unknown
		//IL_0462: Unknown result type (might be due to invalid IL or missing references)
		//IL_046c: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		//IL_04f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_053d: Expected O, but got Unknown
		//IL_053e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055e: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0b7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b84: Expected O, but got Unknown
		//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f5d: Expected O, but got Unknown
		//IL_0f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8b: Expected O, but got Unknown
		//IL_168b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1695: Expected O, but got Unknown
		//IL_18d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_18de: Expected O, but got Unknown
		//IL_1902: Unknown result type (might be due to invalid IL or missing references)
		//IL_190c: Expected O, but got Unknown
		GridEXLayout GridDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdsaleorder_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetailForPendingView_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdPendingViewVoucher_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.GridDetail = new GridEX();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdsaleorder = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.btnPendingForView = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.btnApproved = new System.Windows.Forms.Button();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label8 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.GridDetailForPendingView = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.label13 = new System.Windows.Forms.Label();
		this.grdPendingViewVoucher = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.btnApprovePendingRecord = new System.Windows.Forms.Button();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.ctrlGrdBar4 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.cmbItem = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbvehicle = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbSupplier = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txttodoc = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtfromdoc = new System.Windows.Forms.TextBox();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.label14 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbrefparty = new UltraCombo();
		this.DateTo = new System.Windows.Forms.DateTimePicker();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbitemtype = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbitemcat = new UltraCombo();
		this.cmbperemeter = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.cmbparentcat = new UltraCombo();
		this.cmbApproveStatus = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).BeginInit();
		this.panel8.SuspendLayout();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).BeginInit();
		this.panel17.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).BeginInit();
		this.panel11.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicle).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefparty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcat).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(987, 708);
		this.panel1.TabIndex = 0;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.tabControl2.Location = new System.Drawing.Point(0, 188);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(987, 520);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add(this.panel6);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Controls.Add(this.ctrlGrdBar3);
		this.tabPage3.Controls.Add(this.label8);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(979, 491);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Order Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label9);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(3, 303);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(973, 185);
		this.panel6.TabIndex = 29;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 22);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(973, 163);
		this.panel7.TabIndex = 28;
		this.GridDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetail).BackColor = System.Drawing.Color.White;
		GridDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetail.DesignTimeLayout = GridDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetail.FilterMode = (FilterMode)2;
		this.GridDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetail.GroupByBoxFormatStyle.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxInfoFormatStyle.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.GridDetail.GroupByBoxVisible = false;
		this.GridDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GridDetail).Name = "GridDetail";
		((ScrollableBase)this.GridDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetail).Size = new System.Drawing.Size(973, 163);
		((System.Windows.Forms.Control)(object)this.GridDetail).TabIndex = 0;
		this.GridDetail.TotalRow = (InheritableBoolean)1;
		this.GridDetail.TotalRowPosition = (TotalRowPosition)1;
		this.GridDetail.TreeLineColor = System.Drawing.Color.FromArgb(10, 110, 110);
		((ScrollableBase)this.GridDetail).VisualStyle = (VisualStyle)4;
		this.GridDetail.LinkClicked += new ColumnActionEventHandler(GridDetail_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetail_KeyDown);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(931, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = this.GridDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 22);
		this.ctrlGrdBar2.TabIndex = 25;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(973, 22);
		this.label9.TabIndex = 27;
		this.label9.Text = "Detail of Above Selected row";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel5.Controls.Add(this.panel9);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(3, 25);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(973, 278);
		this.panel5.TabIndex = 28;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdsaleorder);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 35);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(973, 243);
		this.panel9.TabIndex = 2;
		this.grdsaleorder.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdsaleorder).BackColor = System.Drawing.Color.White;
		grdsaleorder_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsaleorder.DesignTimeLayout = grdsaleorder_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdsaleorder.FilterMode = (FilterMode)2;
		this.grdsaleorder.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsaleorder.GroupByBoxFormatStyle.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxInfoFormatStyle.ForeColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.grdsaleorder.GroupByBoxVisible = false;
		this.grdsaleorder.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Name = "grdsaleorder";
		((ScrollableBase)this.grdsaleorder).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Size = new System.Drawing.Size(973, 243);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdsaleorder).Tag = "MasterGrid";
		this.grdsaleorder.TotalRow = (InheritableBoolean)1;
		this.grdsaleorder.TotalRowPosition = (TotalRowPosition)1;
		this.grdsaleorder.TreeLineColor = System.Drawing.Color.FromArgb(10, 110, 110);
		((ScrollableBase)this.grdsaleorder).VisualStyle = (VisualStyle)4;
		this.grdsaleorder.ColumnButtonClick += new ColumnActionEventHandler(grdsaleorder_ColumnButtonClick);
		this.grdsaleorder.SelectionChanged += new System.EventHandler(grdsaleorder_SelectionChanged);
		this.grdsaleorder.LinkClicked += new ColumnActionEventHandler(grdsaleorder_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdsaleorder).KeyDown += new System.Windows.Forms.KeyEventHandler(grdsaleorder_KeyDown);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add(this.btnPendingForView);
		this.panel8.Controls.Add(this.label5);
		this.panel8.Controls.Add(this.btnApproved);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(973, 35);
		this.panel8.TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.DarkBlue;
		this.label10.Location = new System.Drawing.Point(437, 9);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(315, 17);
		this.label10.TabIndex = 143;
		this.label10.Text = "Click To Transfer Selected rows In Pending for View";
		this.btnPendingForView.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnPendingForView.FlatAppearance.BorderSize = 0;
		this.btnPendingForView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPendingForView.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPendingForView.ForeColor = System.Drawing.Color.White;
		this.btnPendingForView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnPendingForView.Location = new System.Drawing.Point(757, 4);
		this.btnPendingForView.Margin = new System.Windows.Forms.Padding(2);
		this.btnPendingForView.Name = "btnPendingForView";
		this.btnPendingForView.Size = new System.Drawing.Size(134, 26);
		this.btnPendingForView.TabIndex = 1;
		this.btnPendingForView.Text = "PendingForView";
		this.btnPendingForView.UseVisualStyleBackColor = false;
		this.btnPendingForView.Click += new System.EventHandler(btnPendingForView_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.DarkBlue;
		this.label5.Location = new System.Drawing.Point(15, 9);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(222, 17);
		this.label5.TabIndex = 134;
		this.label5.Text = "Click To Approve The Selected rows";
		this.btnApproved.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApproved.FlatAppearance.BorderSize = 0;
		this.btnApproved.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApproved.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApproved.ForeColor = System.Drawing.Color.White;
		this.btnApproved.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApproved.Location = new System.Drawing.Point(242, 4);
		this.btnApproved.Margin = new System.Windows.Forms.Padding(2);
		this.btnApproved.Name = "btnApproved";
		this.btnApproved.Size = new System.Drawing.Size(68, 26);
		this.btnApproved.TabIndex = 0;
		this.btnApproved.Text = "Approve";
		this.btnApproved.UseVisualStyleBackColor = false;
		this.btnApproved.Click += new System.EventHandler(btnApproved_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(933, 4);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar3.MyGrid = this.grdsaleorder;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(43, 24);
		this.ctrlGrdBar3.TabIndex = 24;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Dock = System.Windows.Forms.DockStyle.Top;
		this.label8.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(973, 22);
		this.label8.TabIndex = 26;
		this.label8.Text = "Delivery Order Main Information";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailForPendingView);
		this.tabPage4.Controls.Add(this.panel17);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher);
		this.tabPage4.Controls.Add(this.panel11);
		this.tabPage4.Location = new System.Drawing.Point(4, 25);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(979, 491);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Pending For View";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.GridDetailForPendingView.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailForPendingView).BackColor = System.Drawing.Color.White;
		this.GridDetailForPendingView.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GridDetailForPendingView_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailForPendingView.DesignTimeLayout = GridDetailForPendingView_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailForPendingView.FilterMode = (FilterMode)2;
		this.GridDetailForPendingView.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailForPendingView.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Location = new System.Drawing.Point(3, 353);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Name = "GridDetailForPendingView";
		((ScrollableBase)this.GridDetailForPendingView).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).Size = new System.Drawing.Size(973, 135);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).TabIndex = 10;
		this.GridDetailForPendingView.TotalRow = (InheritableBoolean)1;
		this.GridDetailForPendingView.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailForPendingView).VisualStyle = (VisualStyle)4;
		this.GridDetailForPendingView.LinkClicked += new ColumnActionEventHandler(GridDetailForPendingView_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridDetailForPendingView).KeyDown += new System.Windows.Forms.KeyEventHandler(GridDetailForPendingView_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.label13);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 330);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(973, 23);
		this.panel17.TabIndex = 9;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(5, 3);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(205, 17);
		this.label13.TabIndex = 22;
		this.label13.Text = "Detail of Above Selected row";
		this.grdPendingViewVoucher.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPendingViewVoucher).BackColor = System.Drawing.Color.White;
		this.grdPendingViewVoucher.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdPendingViewVoucher_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPendingViewVoucher.DesignTimeLayout = grdPendingViewVoucher_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdPendingViewVoucher.FilterMode = (FilterMode)2;
		this.grdPendingViewVoucher.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPendingViewVoucher.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Location = new System.Drawing.Point(3, 62);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Name = "grdPendingViewVoucher";
		((ScrollableBase)this.grdPendingViewVoucher).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Size = new System.Drawing.Size(973, 268);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).Tag = "MasterGridPendingView";
		this.grdPendingViewVoucher.TotalRow = (InheritableBoolean)1;
		this.grdPendingViewVoucher.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPendingViewVoucher).VisualStyle = (VisualStyle)4;
		this.grdPendingViewVoucher.ColumnButtonClick += new ColumnActionEventHandler(grdPendingViewVoucher_ColumnButtonClick);
		this.grdPendingViewVoucher.SelectionChanged += new System.EventHandler(grdPendingViewVoucher_SelectionChanged);
		this.grdPendingViewVoucher.LinkClicked += new ColumnActionEventHandler(grdPendingViewVoucher_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdPendingViewVoucher).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPendingViewVoucher_KeyDown);
		this.panel11.Controls.Add(this.label12);
		this.panel11.Controls.Add(this.btnApprovePendingRecord);
		this.panel11.Controls.Add(this.panel15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(973, 59);
		this.panel11.TabIndex = 7;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.DarkBlue;
		this.label12.Location = new System.Drawing.Point(3, 35);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(222, 17);
		this.label12.TabIndex = 138;
		this.label12.Text = "Click To Approve The Selected rows";
		this.btnApprovePendingRecord.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnApprovePendingRecord.FlatAppearance.BorderSize = 0;
		this.btnApprovePendingRecord.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnApprovePendingRecord.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnApprovePendingRecord.ForeColor = System.Drawing.Color.White;
		this.btnApprovePendingRecord.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnApprovePendingRecord.Location = new System.Drawing.Point(233, 30);
		this.btnApprovePendingRecord.Margin = new System.Windows.Forms.Padding(2);
		this.btnApprovePendingRecord.Name = "btnApprovePendingRecord";
		this.btnApprovePendingRecord.Size = new System.Drawing.Size(83, 26);
		this.btnApprovePendingRecord.TabIndex = 0;
		this.btnApprovePendingRecord.Text = "Approve";
		this.btnApprovePendingRecord.UseVisualStyleBackColor = false;
		this.btnApprovePendingRecord.Click += new System.EventHandler(btnApprovePendingRecord_Click);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label11);
		this.panel15.Controls.Add(this.ctrlGrdBar4);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 0);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(973, 28);
		this.panel15.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(4, 4);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(230, 17);
		this.label11.TabIndex = 2;
		this.label11.Text = "Delivery Order Main Information";
		this.ctrlGrdBar4.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar4.FormName = this;
		this.ctrlGrdBar4.Location = new System.Drawing.Point(931, 3);
		this.ctrlGrdBar4.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar4.MyGrid = this.grdPendingViewVoucher;
		this.ctrlGrdBar4.Name = "ctrlGrdBar4";
		this.ctrlGrdBar4.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar4.TabIndex = 21;
		this.ctrlGrdBar4.Load += new System.EventHandler(ctrlGrdBar4_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 53);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(987, 135);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicle);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplier);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txttodoc);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtfromdoc);
		this.groupBox1.Controls.Add(this.DateFrom);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrefparty);
		this.groupBox1.Controls.Add(this.DateTo);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemtype);
		this.groupBox1.Controls.Add(this.label18);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitemcat);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbparentcat);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbApproveStatus);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, -1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(980, 127);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(439, 96);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(202, 25);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(648, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(64, 15);
		this.label1.TabIndex = 631;
		this.label1.Text = "Vehicle No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(339, 101);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(67, 15);
		this.label7.TabIndex = 613;
		this.label7.Text = "Item Name";
		this.cmbvehicle.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicle.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicle.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicle.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicle).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicle).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicle).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicle).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbvehicle.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicle).Location = new System.Drawing.Point(753, 41);
		((System.Windows.Forms.Control)(object)this.cmbvehicle).Name = "cmbvehicle";
		((System.Windows.Forms.Control)(object)this.cmbvehicle).Size = new System.Drawing.Size(217, 25);
		((System.Windows.Forms.Control)(object)this.cmbvehicle).TabIndex = 11;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(905, 96);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 26);
		this.btnshow.TabIndex = 14;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click_1);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.cmbSupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance56;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Location = new System.Drawing.Point(99, 96);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Name = "cmbSupplier";
		((System.Windows.Forms.Control)(object)this.cmbSupplier).Size = new System.Drawing.Size(238, 25);
		((System.Windows.Forms.Control)(object)this.cmbSupplier).TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(4, 46);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(85, 15);
		this.label2.TabIndex = 629;
		this.label2.Text = "DocFrom && To";
		this.txttodoc.BackColor = System.Drawing.Color.White;
		this.txttodoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttodoc.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txttodoc.Location = new System.Drawing.Point(221, 42);
		this.txttodoc.Name = "txttodoc";
		this.txttodoc.Size = new System.Drawing.Size(116, 22);
		this.txttodoc.TabIndex = 2;
		this.txttodoc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttodoc_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 101);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(69, 15);
		this.label3.TabIndex = 614;
		this.label3.Text = "Party Name";
		this.txtfromdoc.BackColor = System.Drawing.Color.White;
		this.txtfromdoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfromdoc.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdoc.Location = new System.Drawing.Point(99, 42);
		this.txtfromdoc.Name = "txtfromdoc";
		this.txtfromdoc.Size = new System.Drawing.Size(116, 22);
		this.txtfromdoc.TabIndex = 1;
		this.txtfromdoc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfromdoc_KeyPress);
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(99, 70);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(112, 22);
		this.DateFrom.TabIndex = 3;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(648, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(54, 15);
		this.label14.TabIndex = 626;
		this.label14.Text = "Ref Party";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(4, 74);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(91, 15);
		this.label15.TabIndex = 615;
		this.label15.Text = "Date From && To";
		this.cmbrefparty.AllowNull = (DefaultableBoolean)1;
		this.cmbrefparty.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrefparty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrefparty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance107).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance107).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefparty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.cmbrefparty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrefparty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrefparty).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrefparty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrefparty).Location = new System.Drawing.Point(752, 13);
		((System.Windows.Forms.Control)(object)this.cmbrefparty).Name = "cmbrefparty";
		((System.Windows.Forms.Control)(object)this.cmbrefparty).Size = new System.Drawing.Size(217, 25);
		((System.Windows.Forms.Control)(object)this.cmbrefparty).TabIndex = 10;
		this.DateTo.CustomFormat = "dd-MMM-yy";
		this.DateTo.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateTo.Location = new System.Drawing.Point(215, 70);
		this.DateTo.Name = "DateTo";
		this.DateTo.Size = new System.Drawing.Size(122, 22);
		this.DateTo.TabIndex = 4;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(339, 74);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(60, 15);
		this.label16.TabIndex = 624;
		this.label16.Text = "Item Type";
		this.cmbitemtype.AllowNull = (DefaultableBoolean)1;
		this.cmbitemtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance77).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Location = new System.Drawing.Point(439, 69);
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Name = "cmbitemtype";
		((System.Windows.Forms.Control)(object)this.cmbitemtype).Size = new System.Drawing.Size(202, 25);
		((System.Windows.Forms.Control)(object)this.cmbitemtype).TabIndex = 8;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(339, 46);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(82, 15);
		this.label18.TabIndex = 622;
		this.label18.Text = "Item Category";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(4, 18);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 618;
		this.label19.Text = "Date Type";
		this.cmbitemcat.AllowNull = (DefaultableBoolean)1;
		this.cmbitemcat.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitemcat.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitemcat.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance88).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitemcat).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbitemcat.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Location = new System.Drawing.Point(439, 41);
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Name = "cmbitemcat";
		((System.Windows.Forms.Control)(object)this.cmbitemcat).Size = new System.Drawing.Size(202, 25);
		((System.Windows.Forms.Control)(object)this.cmbitemcat).TabIndex = 7;
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance34).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance35).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(99, 13);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(238, 25);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(339, 18);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(91, 15);
		this.label20.TabIndex = 620;
		this.label20.Text = "Parent Category";
		this.cmbparentcat.AllowNull = (DefaultableBoolean)1;
		this.cmbparentcat.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbparentcat.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbparentcat.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance101).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance102).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbparentcat).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbparentcat.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Location = new System.Drawing.Point(439, 13);
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Name = "cmbparentcat";
		((System.Windows.Forms.Control)(object)this.cmbparentcat).Size = new System.Drawing.Size(202, 25);
		((System.Windows.Forms.Control)(object)this.cmbparentcat).TabIndex = 6;
		this.cmbApproveStatus.AllowNull = (DefaultableBoolean)1;
		this.cmbApproveStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbApproveStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbApproveStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbApproveStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbApproveStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Location = new System.Drawing.Point(752, 68);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Name = "cmbApproveStatus";
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.cmbApproveStatus).TabIndex = 13;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(648, 74);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(95, 15);
		this.label4.TabIndex = 138;
		this.label4.Text = "Approved Status";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(987, 53);
		this.panel2.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(10, 30);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(205, 19);
		this.label23.TabIndex = 3;
		this.label23.Text = "Delivery Order Approval";
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnShortCutKeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(987, 27);
		this.toolStrip3.TabIndex = 3;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(121, 24);
		this.btnShortCutKeys.Text = "ShortCutKeys";
		this.btnShortCutKeys.ToolTipText = "ShortCutKeys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(987, 708);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DeliveryOrderForApproval_Concrete";
		this.Text = "frmSaleOrderHistory";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(SaleOrderForApproval_Concrete_FormClosed);
		base.Load += new System.EventHandler(frmSaleOrderHistory_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmSaleOrderHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsaleorder).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailForPendingView).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPendingViewVoucher).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicle).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbrefparty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbitemcat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbparentcat).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbApproveStatus).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
