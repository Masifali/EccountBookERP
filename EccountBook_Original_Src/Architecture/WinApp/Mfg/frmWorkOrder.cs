using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Mfg;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Mfg;

public class frmWorkOrder : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtOperationProcess = new DataTable();

	private DataTable dtCustomerDetail = new DataTable();

	private DataTable dtItemDetail = new DataTable();

	private DataTable dtCmbitem = new DataTable();

	private DataTable dtCmbCustomer = new DataTable();

	private DataTable dtCustomerOrderDetailWithItem = new DataTable();

	private DataTable dtuomGetAll = new DataTable();

	private DataTable dtHistory = new DataTable();

	private DataTable dtAccounts = new DataTable();

	private DataTable dtWorkStationOpertaion = new DataTable();

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private bool UpdateMode;

	private int RecId;

	private int updateDetailIndex = -1;

	private int updateDetailIndexItem = -1;

	private int updateDetailIndexCustomer = -1;

	private List<WorkOrderOperation> lstRemoveRecordOperations = new List<WorkOrderOperation>();

	private List<WorkOrderCustomer> lstRemoveRecordcustomerDetail = new List<WorkOrderCustomer>();

	private List<workOrderMaterial> lstRemoveRecordItemDetail = new List<workOrderMaterial>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton btnPrint;

	private TextBox txtQty;

	private Label label18;

	internal UltraCombo cmbUOM;

	private Label label44;

	internal UltraCombo cmbItem;

	private Label label7;

	private Label label4;

	internal UltraCombo combranches;

	private Label label45;

	internal UltraCombo comproject;

	private Label label46;

	private TextBox txtManualNo;

	private Label label11;

	private Label label12;

	private TabPage tabPage3;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnRegister;

	private GroupBox groupBox2;

	private Label label21;

	private Panel panel5;

	internal UltraCombo cmbBOMNo;

	private Label label37;

	private DateTimePicker txtdocdate;

	private TextBox txtdocno;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private Panel panel38;

	private Label label32;

	internal GridEX grdhistoryoperations;

	private Panel panel9;

	internal GridEX DatagridHistory;

	private Panel panel15;

	private Label label17;

	private Panel panel13;

	private GroupBox groupBox1;

	private Label label19;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label28;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label35;

	private TextBox txtFromNoHistory;

	internal UltraCombo CmbBomNoHistory;

	private Label label20;

	internal UltraCombo cmbItemhistory;

	private Label label29;

	private Button btnshow;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdOperationsDetail;

	private Panel panel14;

	private Panel panel16;

	internal UltraCombo comCompany;

	private GroupBox groupBox3;

	internal UltraCombo CmbCustomerOrderNo;

	internal UltraCombo CmbCustomerName;

	private TextBox txtCustomerOrderQty;

	private Label label10;

	private Label label38;

	private Label label22;

	private Label label25;

	private Label label23;

	private DateTimePicker txtDeadLineDateMain;

	private DateTimePicker txtEndDate;

	private DateTimePicker txtStartdate;

	internal UltraCombo cmbWorkStationOperation;

	private Button BtnCancelOp;

	private Button BtnUpdateOp;

	private Button BtnAddOp;

	private TextBox txtDaysOp;

	private Label label1;

	private TextBox txtHoursOp;

	private Label label3;

	private Label label5;

	private CheckBox ChkPreview;

	private Label label6;

	private DateTimePicker txtDeadLineDateOp;

	private TextBox txtRemarksHeader;

	private Label label8;

	private TextBox txtBatchDesc;

	private Label label13;

	private TextBox txtOpHoursPerDay;

	private Label label14;

	private Label label15;

	private Label label36;

	internal UltraCombo cmbItemUOM;

	internal UltraCombo CmbItemDetail;

	private Label label26;

	private TextBox txtQtyDetail;

	private Label label16;

	private Panel panelItem;

	private Panel panelOperation;

	private Button BtnAddItem;

	private Button BtnCancelItem;

	private Button BtnUpdateItem;

	internal GridEX grdItemDetail;

	private Panel panel7;

	private Panel panel6;

	private Panel panel4;

	internal GridEX grdCustomerDetail;

	private Button BtnAddCustomer;

	private Button BtnCancelCustomer;

	private Button BtnUpdateCustmer;

	private Label label9;

	private Label label2;

	private ToolStripButton btnSaveAs;

	private ToolStripButton btnAttachment;

	public frmWorkOrder(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmBillOfMaterial_Manufacturing_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 650);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnSaveAs.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnDelete.Visible = false;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtAccounts.Columns.Add("Id");
			dtAccounts.Columns.Add("AccountTitle");
			ItemFill();
			uomgetAll();
			AllWorkStationOpertaionFill();
			DocumentNoFill();
			dtOperationProcess.Columns.Add("Id", typeof(int));
			dtOperationProcess.Columns.Add("BOMHeaderId", typeof(int));
			dtOperationProcess.Columns.Add("BOMOperationDetailId", typeof(int));
			dtOperationProcess.Columns.Add("WorkStationOperationId", typeof(int));
			dtOperationProcess.Columns.Add("WorkStationName", typeof(string));
			dtOperationProcess.Columns.Add("OperationName", typeof(string));
			dtOperationProcess.Columns.Add("OpHours", typeof(decimal));
			dtOperationProcess.Columns.Add("OpHoursPerDay", typeof(decimal));
			dtOperationProcess.Columns.Add("OpDays", typeof(decimal));
			dtOperationProcess.Columns.Add("DeadLine", typeof(DateTime));
			grdOperationsDetail.DataSource = dtOperationProcess;
			grdOperationsDetail.RetrieveStructure();
			grdOperationsDetailSetting();
			dtItemDetail.Columns.Add("Id", typeof(int));
			dtItemDetail.Columns.Add("ItemId", typeof(int));
			dtItemDetail.Columns.Add("ItemName", typeof(string));
			dtItemDetail.Columns.Add("UOMId", typeof(int));
			dtItemDetail.Columns.Add("UOM", typeof(string));
			dtItemDetail.Columns.Add("Qty", typeof(decimal));
			grdItemDetail.DataSource = dtItemDetail;
			grdItemDetail.RetrieveStructure();
			grdItemDetailSetting();
			dtCustomerDetail.Columns.Add("Id", typeof(int));
			dtCustomerDetail.Columns.Add("CustomerId", typeof(int));
			dtCustomerDetail.Columns.Add("CustomerName", typeof(string));
			dtCustomerDetail.Columns.Add("SaleOrderId", typeof(int));
			dtCustomerDetail.Columns.Add("SaleOrder", typeof(int));
			dtCustomerDetail.Columns.Add("Qty", typeof(decimal));
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdCustomerDetailSetting();
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNoFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = WorkOrder.GenerateCode(new WorkOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1614,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			txtdocno.Text = code.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			int DetailId = Conversion.ToInt(CmbItemDetail.Value);
			dtCmbitem.Clear();
			dtCmbitem = CommonServices.ItemGetAllServiceBind();
			if (dtCmbitem.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			for (int i = 0; i < dtCmbitem.Rows.Count; i++)
			{
				dtitem.Rows.Add(dtCmbitem.Rows[i]["Id"], dtCmbitem.Rows[i]["ItemName"]);
			}
			if (dtitem.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtitem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
			DDL.BindDDL(dtitem, CmbItemDetail, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 350;
			((UltraGridBase)CmbItemDetail).DisplayLayout.Bands[0].Columns[1].Width = 350;
			if (Id > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbItem.Value = Id;
				}
				else
				{
					((Control)(object)cmbItem).Text = string.Empty;
				}
			}
			if (DetailId > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
				{
					CmbItemDetail.Value = DetailId;
				}
				else
				{
					((Control)(object)CmbItemDetail).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BomNoFill(int ItemId)
	{
		try
		{
			if (ItemId > 0)
			{
				int Id = Conversion.ToInt(cmbBOMNo.Value);
				DataTable dt = BomHeader.GetBomNoByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
				if (dt.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dt, cmbBOMNo, "Id", "BomName", "BOM Name", ZeroIndex: true);
				((UltraGridBase)cmbBOMNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbBOMNo.Value = Id;
					}
					else
					{
						((Control)(object)cmbBOMNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbBOMNo).Text = string.Empty;
				((UltraGridBase)cmbBOMNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllWorkStationOpertaionFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbWorkStationOperation.Value);
			dtWorkStationOpertaion = WorkStationOperation.GetWorkStationOpertaionByWSOId(UserAccount.OrganizationId, UserAccount.CompanyId, 0);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Description", typeof(string));
			dt.Columns.Add("OpHoursPerDay", typeof(double));
			if (dtWorkStationOpertaion.Rows.Count > 0)
			{
				dt.Clear();
				foreach (DataRow r in dtWorkStationOpertaion.Rows)
				{
					dt.Rows.Add(r["Id"], r["Description"], r["OpHoursPerDay"]);
				}
			}
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dt, cmbWorkStationOperation, "Id", "Description", "Work Station/Operation", ZeroIndex: true);
			((UltraGridBase)cmbWorkStationOperation).DisplayLayout.Bands[0].Columns[1].Width = 350;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbWorkStationOperation.Value = Id;
				}
				else
				{
					((Control)(object)cmbWorkStationOperation).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbWorkStationOperation_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbWorkStationOperation.Value) != 0)
			{
				decimal OpHoursPerDay = Conversion.ToInt(((UltraDropDownBase)cmbWorkStationOperation).SelectedRow.Cells["OpHoursPerDay"].Value);
				if (OpHoursPerDay > 0m)
				{
					txtOpHoursPerDay.Text = OpHoursPerDay.ToString();
				}
				else
				{
					txtOpHoursPerDay.Text = 0.ToString();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void uomgetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			UOMSchedule obj = new UOMSchedule();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtuomGetAll = UOMSchedule.Getall(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int CustomerId = Conversion.ToInt(CmbCustomerName.Value);
			int ItemId = Conversion.ToInt(cmbItem.Value);
			ItemUomBind(ItemId);
			BomNoFill(ItemId);
			CustomerFill(ItemId);
			int UomId = Conversion.ToInt(cmbUOM.Value);
			BindCustomerOrders(CustomerId, ItemId, UomId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemUomBind(int ItemId)
	{
		try
		{
			int Id = Conversion.ToInt(cmbUOM.Value);
			((Control)(object)cmbUOM).Text = string.Empty;
			DataTable dtuomheader = new DataTable();
			dtuomheader.Columns.Add("Id", typeof(int));
			dtuomheader.Columns.Add("ItemUOM");
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == ItemId)
				{
					dtuomheader.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["UOMDescription"]);
				}
			}
			if (dtuomheader.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtuomheader, cmbUOM, "Id", "ItemUOM", "UOM", ZeroIndex: false);
			((UltraGridBase)cmbUOM).DisplayLayout.Bands[0].Columns[1].Width = 193;
			if (Id > 0)
			{
				if (dtuomheader.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbUOM.Value = Id;
				}
				else
				{
					((Control)(object)cmbUOM).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerFill(int ItemId)
	{
		try
		{
			int Id = Conversion.ToInt(CmbCustomerName.Value);
			dtCustomerOrderDetailWithItem.Clear();
			dtCustomerOrderDetailWithItem = SaleOrder.SaleOrder_InfoByItemId(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
			if (dtCustomerOrderDetailWithItem.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CompanyName", typeof(string));
				dt.Clear();
				HashSet<int> existingCustomerIds = new HashSet<int>();
				foreach (DataRow r in dtCustomerOrderDetailWithItem.Rows)
				{
					int CustomerId = Conversion.ToInt(r["CustomerId"]);
					if (!existingCustomerIds.Contains(CustomerId))
					{
						dt.Rows.Add(CustomerId, r["CustomerName"]);
						existingCustomerIds.Add(CustomerId);
					}
				}
				if (dt.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dt, CmbCustomerName, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				((UltraGridBase)CmbCustomerName).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbCustomerName.Value = Id;
					}
					else
					{
						((Control)(object)CmbCustomerName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCustomerName).Text = string.Empty;
				((UltraGridBase)CmbCustomerName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomerName_Leave(object sender, EventArgs e)
	{
		try
		{
			int CustomerId = Conversion.ToInt(CmbCustomerName.Value);
			int ItemId = Conversion.ToInt(cmbItem.Value);
			int UomId = Conversion.ToInt(cmbUOM.Value);
			BindCustomerOrders(CustomerId, ItemId, UomId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCustomerOrders(int CustomerId, int ItemId, int UomId)
	{
		try
		{
			int Id = Conversion.ToInt(CmbCustomerOrderNo.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("OrderId", typeof(int));
			dt.Columns.Add("OrderNo", typeof(int));
			dt.Columns.Add("OrderQty", typeof(double));
			if (CustomerId > 0 && ItemId > 0)
			{
				var list = (from r in dtCustomerOrderDetailWithItem.AsEnumerable()
					where Conversion.ToInt(r["CustomerId"]) == CustomerId && Conversion.ToInt(r["ItemId"]) == ItemId
					group r by new
					{
						CustomerId = Conversion.ToInt(r["CustomerId"]),
						ItemId = Conversion.ToInt(r["ItemId"])
					} into g
					select new
					{
						OrderId = g.Select((DataRow r) => r["OrderId"]).FirstOrDefault(),
						OrderNo = g.Select((DataRow r) => r["OrderNo"]).FirstOrDefault(),
						TotalQty = g.Sum((DataRow r) => Conversion.ToDouble(r["ItemQty"]))
					}).ToList();
				dt.Clear();
				foreach (var group in list)
				{
					if (Conversion.ToInt(group.OrderId) > 0)
					{
						dt.Rows.Add(group.OrderId, group.OrderNo, group.TotalQty);
					}
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCustomerOrderNo, "OrderId", "OrderNo", "OrderNo", ZeroIndex: false);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("OrderId") == Id))
					{
						CmbCustomerOrderNo.Value = Id;
					}
					else
					{
						((Control)(object)CmbCustomerOrderNo).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbCustomerOrderNo).DataSource = null;
				((Control)(object)CmbCustomerOrderNo).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateTypeHistory_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateTypeHistory.Value) == 1)
			{
				txtFromdateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 2)
			{
				txtFromdateHistory.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 3)
			{
				DateTime now = DateTime.Now;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtFromdateHistory.Value = first;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtFromdateHistory.Value = first2;
				txtToDateHistory.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateTypeHistory.Value) == 5)
			{
				txtFromdateHistory.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtHoursOp_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDecimal((object)txtOpHoursPerDay.Text) > 0m)
			{
				decimal Days = Conversion.ToDecimal((object)txtHoursOp.Text) / Conversion.ToDecimal((object)txtOpHoursPerDay.Text);
				txtDaysOp.Text = Days.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CmbItemDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			int UomId = Conversion.ToInt(cmbItemUOM.Value);
			((Control)(object)cmbItemUOM).Text = string.Empty;
			DataTable dtUomDetail = new DataTable();
			dtUomDetail.Columns.Add("Id", typeof(int));
			dtUomDetail.Columns.Add("ItemUOM");
			for (int i = 0; i < dtuomGetAll.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtuomGetAll.Rows[i]["ItemId"]) == Conversion.ToInt(CmbItemDetail.Value))
				{
					dtUomDetail.Rows.Add(dtuomGetAll.Rows[i]["Id"], dtuomGetAll.Rows[i]["UOMDescription"]);
				}
			}
			if (dtUomDetail.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtUomDetail, cmbItemUOM, "Id", "ItemUOM", "UOM", ZeroIndex: false);
			((UltraGridBase)cmbItemUOM).DisplayLayout.Bands[0].Columns[1].Width = 193;
			if (UomId > 0)
			{
				if (dtUomDetail.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == UomId))
				{
					cmbItemUOM.Value = UomId;
				}
				else
				{
					((Control)(object)cmbItemUOM).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCustomerOrderNo_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			double Qty = 0.0;
			int OrderId = Conversion.ToInt(CmbCustomerOrderNo.Value);
			if (((UltraGridBase)CmbCustomerOrderNo).ActiveRow != null && OrderId > 0)
			{
				Qty = Conversion.ToDouble(((UltraDropDownBase)CmbCustomerOrderNo).SelectedRow.Cells["OrderQty"].Value);
			}
			txtCustomerOrderQty.Text = Qty.ToString("#,##.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text == "0")
		{
			MessageBox.Show("doc No Field Required");
			txtdocno.Focus();
			return false;
		}
		if (txtManualNo.Text.Trim() == string.Empty || txtManualNo.Text == "0")
		{
			MessageBox.Show("BOM Field Required");
			txtManualNo.Focus();
			return false;
		}
		if (((Control)(object)cmbItem).Text.Trim() == string.Empty || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show("Item Feild Required");
			cmbItem.Focus();
			return false;
		}
		if (((Control)(object)cmbUOM).Text.Trim() == string.Empty || Conversion.ToInt(cmbUOM.Value) == 0)
		{
			MessageBox.Show("IteUom Field Required");
			cmbUOM.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOperationDetail()
	{
		if (((UltraGridBase)cmbWorkStationOperation).ActiveRow == null || Conversion.ToInt(cmbWorkStationOperation.Value) == 0)
		{
			MessageBox.Show("Work Station with Operation Name Field is Required");
			cmbWorkStationOperation.Focus();
			return false;
		}
		if (txtHoursOp.Text.Trim() == string.Empty || txtHoursOp.Text.Trim() == "0")
		{
			MessageBox.Show("Hours Field is Required");
			txtHoursOp.Focus();
			return false;
		}
		if (txtDaysOp.Text.Trim() == string.Empty || txtDaysOp.Text.Trim() == "0")
		{
			MessageBox.Show("Days Field is Required");
			txtDaysOp.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationItemDetail()
	{
		if (((UltraGridBase)CmbItemDetail).ActiveRow == null || Conversion.ToInt(CmbItemDetail.Value) == 0)
		{
			MessageBox.Show("ItemName Field is Required");
			CmbItemDetail.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemUOM).ActiveRow == null || Conversion.ToInt(cmbItemUOM.Value) == 0)
		{
			MessageBox.Show("UOM Field is Required");
			cmbItemUOM.Focus();
			return false;
		}
		if (txtQtyDetail.Text.Trim() == string.Empty || txtQtyDetail.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQtyDetail.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationCustomerDetail()
	{
		if (((UltraGridBase)CmbCustomerName).ActiveRow == null || Conversion.ToInt(CmbCustomerName.Value) == 0)
		{
			MessageBox.Show("Customer Field is Required");
			CmbCustomerName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCustomerOrderNo).ActiveRow == null || Conversion.ToInt(CmbCustomerOrderNo.Value) == 0)
		{
			MessageBox.Show("Order Field is Required");
			CmbCustomerOrderNo.Focus();
			return false;
		}
		if (txtCustomerOrderQty.Text.Trim() == string.Empty || txtCustomerOrderQty.Text.Trim() == "0")
		{
			MessageBox.Show("Customer Order Qty Field is Required");
			txtCustomerOrderQty.Focus();
			return false;
		}
		return true;
	}

	private void cmbBOMNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GridFillByBomOperationDetail(Conversion.ToInt(cmbBOMNo.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillByBomOperationDetail(int BOMId)
	{
		try
		{
			DataTable BOMData = BomHeader.ReadByHeaderId_BomOperationDetail(BOMId);
			if (BOMData.Rows.Count <= 0)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < BOMData.Rows.Count; i++)
			{
				if (grdOperationsDetail.GetRows().Length != 0)
				{
					GridEXRow[] rows = grdOperationsDetail.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToInt(item.Cells["BOMOperationDetailId"].Value) == Conversion.ToInt(BOMData.Rows[i]["BOMOperationDetailId"]))
						{
							flag = true;
							break;
						}
						if (Conversion.ToInt(item.Cells["BOMHeaderId"].Value) != Conversion.ToInt(BOMData.Rows[i]["BOMHeaderId"]))
						{
							if (MessageBox.Show("Data with a different Bill of Materials (BoM) already exists. Do you want to clear that data and load new data?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
							{
								return;
							}
							dtOperationProcess.Clear();
						}
					}
				}
				if (!flag)
				{
					double hour = Conversion.ToDateTime(BOMData.Rows[i]["OperationTime"]).Hour;
					dtOperationProcess.Rows.Add(0, Conversion.ToInt(BOMData.Rows[i]["BomHeaderId"]), Conversion.ToInt(BOMData.Rows[i]["BOMOperationDetailId"]), Conversion.ToInt(BOMData.Rows[i]["WorkStationOperationId"]), Conversion.ToString(BOMData.Rows[i]["WorkStationName"]), Conversion.ToString(BOMData.Rows[i]["OperationName"]), hour, Conversion.ToDecimal(BOMData.Rows[i]["OpHoursPerDay"]), Conversion.ToInt((object)(Conversion.ToDecimal((object)hour) / Conversion.ToDecimal(BOMData.Rows[i]["OpHoursPerDay"]))), DateTime.Now);
				}
				flag = false;
			}
			grdOperationsDetail.DataSource = dtOperationProcess;
			grdOperationsDetail.RetrieveStructure();
			grdOperationsDetailSetting();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnAddOp_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationOperationDetail())
			{
				return;
			}
			int WorkStationOperationId = Conversion.ToInt(cmbWorkStationOperation.Value);
			string WorkStationName = "";
			string OperationName = "";
			if (dtWorkStationOpertaion != null && dtWorkStationOpertaion.Rows.Count > 0)
			{
				DataRow foundRow = dtWorkStationOpertaion.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == WorkStationOperationId);
				WorkStationName = ((foundRow != null) ? Conversion.ToString(foundRow["WorkStationName"]) : "");
				OperationName = ((foundRow != null) ? Conversion.ToString(foundRow["OperationName"]) : "");
			}
			dtOperationProcess.Rows.Add(0, 0, 0, cmbWorkStationOperation.Value, WorkStationName, OperationName, Conversion.ToDecimal((object)txtHoursOp.Text.Trim()), Conversion.ToDecimal((object)txtOpHoursPerDay.Text.Trim()), Conversion.ToDouble((object)txtDaysOp.Text.Trim()), Conversion.ToDateTime((object)txtDeadLineDateOp.Value));
			grdOperationsDetail.DataSource = dtOperationProcess;
			grdOperationsDetail.RetrieveStructure();
			grdOperationsDetailSetting();
			FromResetDetail();
			UpdateBOMInDetailGrid(grdOperationsDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateOp_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationOperationDetail())
			{
				return;
			}
			int WorkStationOperationId = Conversion.ToInt(cmbWorkStationOperation.Value);
			string WorkStationName = "";
			string OperationName = "";
			if (dtWorkStationOpertaion != null && dtWorkStationOpertaion.Rows.Count > 0)
			{
				DataRow foundRow = dtWorkStationOpertaion.AsEnumerable().FirstOrDefault((DataRow row) => row.Field<int>("Id") == WorkStationOperationId);
				WorkStationName = ((foundRow != null) ? Conversion.ToString(foundRow["WorkStationName"]) : "");
				OperationName = ((foundRow != null) ? Conversion.ToString(foundRow["OperationName"]) : "");
			}
			dtOperationProcess.Rows[updateDetailIndex]["WorkStationOperationId"] = Conversion.ToInt(cmbWorkStationOperation.Value);
			dtOperationProcess.Rows[updateDetailIndex]["WorkStationName"] = WorkStationName;
			dtOperationProcess.Rows[updateDetailIndex]["OperationName"] = OperationName;
			dtOperationProcess.Rows[updateDetailIndex]["OpHours"] = Conversion.ToDecimal((object)txtHoursOp.Text);
			dtOperationProcess.Rows[updateDetailIndex]["OpHoursPerDay"] = Conversion.ToDecimal((object)txtOpHoursPerDay.Text);
			dtOperationProcess.Rows[updateDetailIndex]["OpDays"] = Conversion.ToDouble((object)txtDaysOp.Text);
			dtOperationProcess.Rows[updateDetailIndex]["DeadLine"] = Conversion.ToDateTime((object)txtDeadLineDateOp.Text);
			grdOperationsDetail.DataSource = dtOperationProcess;
			FromResetDetail();
			UpdateBOMInDetailGrid(grdOperationsDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelOp_Click(object sender, EventArgs e)
	{
		try
		{
			FromResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperationsDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdOperationsDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbWorkStationOperation.Value = Conversion.ToInt(item.Cells["WorkStationOperationId"].Value);
				txtDaysOp.Text = Conversion.ToDecimal(item.Cells["OpDays"].Value).ToString("#,##.###");
				txtHoursOp.Text = Conversion.ToDecimal(item.Cells["OpHours"].Value).ToString("#,##.###");
				txtOpHoursPerDay.Text = Conversion.ToDecimal(item.Cells["OpHoursPerDay"].Value).ToString("#,##.###");
				txtDeadLineDateOp.Value = Conversion.ToDateTime(item.Cells["DeadLine"].Value);
				BtnAddOp.Visible = false;
				BtnUpdateOp.Visible = true;
				BtnCancelOp.Visible = true;
				cmbWorkStationOperation.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperationsDetailSetting()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		try
		{
			grdOperationsDetail.RootTable.Columns["Id"].Visible = false;
			grdOperationsDetail.RootTable.Columns["BOMOperationDetailId"].Visible = false;
			grdOperationsDetail.RootTable.Columns["BOMHeaderId"].Visible = false;
			grdOperationsDetail.RootTable.Columns["WorkStationOperationId"].Visible = false;
			grdOperationsDetail.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdOperationsDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			grdOperationsDetail.RootTable.Columns["WorkStationName"].Width = Constants.InventoryConstants.WorkStationName + 30;
			grdOperationsDetail.RootTable.Columns["OperationName"].Width = Constants.InventoryConstants.OperationName + 10;
			grdOperationsDetail.RootTable.Columns["OpHours"].Width = Constants.InventoryConstants.Days;
			grdOperationsDetail.RootTable.Columns["OpHoursPerDay"].Width = Constants.InventoryConstants.Days;
			grdOperationsDetail.RootTable.Columns["OpDays"].Width = Constants.InventoryConstants.Days;
			grdOperationsDetail.RootTable.Columns["DeadLine"].Width = Constants.InventoryConstants.DateConstant;
			grdOperationsDetail.RootTable.Columns.Add("Delete");
			grdOperationsDetail.RootTable.Columns["Delete"].Caption = "X";
			grdOperationsDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOperationsDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdOperationsDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdOperationsDetail.RootTable.Columns["Delete"].Width = 25;
			grdOperationsDetail.RootTable.Columns["Delete"].Position = 0;
			grdOperationsDetail.RootTable.Columns.Add("Edit");
			grdOperationsDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdOperationsDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdOperationsDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdOperationsDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdOperationsDetail.RootTable.Columns["Edit"].Width = 40;
			grdOperationsDetail.RootTable.Columns["Edit"].Position = 1;
			grdOperationsDetail.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOperationsDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdOperationsDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdOperationsDetail.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				DeleteOperationDetailRow(r);
			}
			if (e.Column.Key == "Edit")
			{
				grdOperationsDetail_DoubleClick(null, null);
			}
			grdOperationsDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteOperationDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						WorkOrderOperation d1 = new WorkOrderOperation();
						d1.WorkOrderOperationId = Conversion.ToInt(r.Cells["Id"].Value);
						d1.BOMHeaderId = Conversion.ToInt(r.Cells["BOMHeaderId"].Value);
						d1.BOMOperationDetailId = Conversion.ToInt(r.Cells["BOMOperationDetailId"].Value);
						d1.WorkStationOperationId = Conversion.ToInt(r.Cells["WorkStationOperationId"].Value);
						d1.OperationDays = Conversion.ToInt(r.Cells["OpDays"].Value);
						d1.OperationHours = Conversion.ToDecimal((object)r.Cells["OpHours"].Text);
						d1.OpHoursPerDay = Conversion.ToDecimal(r.Cells["OpHoursPerDay"].Value);
						d1.DeadLine = Conversion.ToDateTime(r.Cells["DeadLine"].Value);
						d1.Remarks = Conversion.ToString(r.Cells["Remarks"].Value);
						d1.ActionTypeId = 3;
						lstRemoveRecordOperations.Add(d1);
						dtOperationProcess.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtOperationProcess.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			throw new Exception("Please Reset the Detail first..");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateBOMInDetailGrid(GridEX grd)
	{
		try
		{
			if (grd.GetDataRows().Count() > 0)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					obj.Cells["BOMHeaderId"].Value = Conversion.ToInt(cmbBOMNo.Value);
					obj.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddItem_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationItemDetail())
			{
				dtItemDetail.Rows.Add(0, Conversion.ToInt(CmbItemDetail.Value), Conversion.ToString((object)((Control)(object)CmbItemDetail).Text.Trim()), Conversion.ToInt(cmbItemUOM.Value), Conversion.ToString((object)((Control)(object)cmbItemUOM).Text.Trim()), Conversion.ToDecimal((object)txtQtyDetail.Text.Trim()));
				grdItemDetail.DataSource = dtItemDetail;
				grdItemDetail.RetrieveStructure();
				grdItemDetailSetting();
				ResetItemDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateItem_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationItemDetail())
			{
				dtItemDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItemDetail.Value);
				dtItemDetail.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString((object)((Control)(object)CmbItemDetail).Text);
				dtItemDetail.Rows[updateDetailIndex]["UOMId"] = Conversion.ToInt(cmbItemUOM.Value);
				dtItemDetail.Rows[updateDetailIndex]["UOM"] = Conversion.ToString((object)((Control)(object)cmbItemUOM).Text);
				dtItemDetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDecimal((object)txtQtyDetail.Text);
				grdItemDetail.DataSource = dtItemDetail;
				ResetItemDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelItem_Click(object sender, EventArgs e)
	{
		try
		{
			ResetItemDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdItemDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbItemDetail.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbItemDetail_Leave(null, null);
				cmbItemUOM.Value = Conversion.ToInt(item.Cells["UOMId"].Value);
				txtQtyDetail.Text = Conversion.ToDecimal(item.Cells["Qty"].Value).ToString("#,##.###");
				BtnAddItem.Visible = false;
				BtnUpdateItem.Visible = true;
				BtnCancelItem.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemDetailSetting()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		try
		{
			grdItemDetail.RootTable.Columns["Id"].Visible = false;
			grdItemDetail.RootTable.Columns["ItemId"].Visible = false;
			grdItemDetail.RootTable.Columns["UOMId"].Visible = false;
			grdItemDetail.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdItemDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			grdItemDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdItemDetail.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grdItemDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdItemDetail.RootTable.Columns.Add("Delete");
			grdItemDetail.RootTable.Columns["Delete"].Caption = "X";
			grdItemDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdItemDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdItemDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdItemDetail.RootTable.Columns["Delete"].Width = 25;
			grdItemDetail.RootTable.Columns["Delete"].Position = 0;
			grdItemDetail.RootTable.Columns.Add("Edit");
			grdItemDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdItemDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdItemDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdItemDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdItemDetail.RootTable.Columns["Edit"].Width = 40;
			grdItemDetail.RootTable.Columns["Edit"].Position = 1;
			grdItemDetail.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdItemDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdItemDetail.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				DeleteItemDetailRow(r);
			}
			if (e.Column.Key == "Edit")
			{
				grdItemDetail_DoubleClick(null, null);
			}
			grdItemDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteItemDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexItem == -1)
			{
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						new workOrderMaterial
						{
							WorkOrderMaterialId = Conversion.ToInt(r.Cells["Id"].Value),
							ItemId = Conversion.ToInt(r.Cells["ItemId"].Value),
							uomId = Conversion.ToInt(r.Cells["UOMId"].Value),
							Qty = Conversion.ToDecimal((object)r.Cells["Qty"].Text),
							ActionTypeId = 3
						};
						dtItemDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtItemDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			throw new Exception("Please Reset the Detail first..");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationCustomerDetail())
			{
				dtCustomerDetail.Rows.Add(0, Conversion.ToInt(CmbCustomerName.Value), Conversion.ToString((object)((Control)(object)CmbCustomerName).Text.Trim()), Conversion.ToInt(CmbCustomerOrderNo.Value), Conversion.ToInt((object)((Control)(object)CmbCustomerOrderNo).Text.Trim()), Conversion.ToDecimal((object)txtCustomerOrderQty.Text.Trim()));
				grdCustomerDetail.DataSource = dtCustomerDetail;
				grdCustomerDetail.RetrieveStructure();
				grdCustomerDetailSetting();
				ResetCustomerDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateCustmer_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationCustomerDetail())
			{
				dtCustomerDetail.Rows[updateDetailIndex]["CustomerId"] = Conversion.ToInt(CmbCustomerName.Value);
				dtCustomerDetail.Rows[updateDetailIndex]["CustomerName"] = Conversion.ToString((object)((Control)(object)CmbCustomerName).Text);
				dtCustomerDetail.Rows[updateDetailIndex]["SaleOrderId"] = Conversion.ToInt(CmbCustomerOrderNo.Value);
				dtCustomerDetail.Rows[updateDetailIndex]["SaleOrder"] = Conversion.ToInt((object)((Control)(object)CmbCustomerOrderNo).Text);
				dtCustomerDetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToDecimal((object)txtCustomerOrderQty.Text);
				grdCustomerDetail.DataSource = dtCustomerDetail;
				ResetCustomerDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelCustomer_Click(object sender, EventArgs e)
	{
		try
		{
			ResetCustomerDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow Customer = grdCustomerDetail.CurrentRow;
			if (Customer != null)
			{
				updateDetailIndex = Customer.RowIndex;
				CmbCustomerName.Value = Conversion.ToInt(Customer.Cells["CustomerId"].Value);
				CmbCustomerName_Leave(null, null);
				CmbCustomerOrderNo.Value = Conversion.ToInt(Customer.Cells["SaleOrderId"].Value);
				txtQty.Text = Conversion.ToDecimal(Customer.Cells["Qty"].Value).ToString("#,##.###");
				BtnAddCustomer.Visible = false;
				BtnUpdateCustmer.Visible = true;
				BtnCancelCustomer.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetailSetting()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		try
		{
			grdCustomerDetail.RootTable.Columns["Id"].Visible = false;
			grdCustomerDetail.RootTable.Columns["CustomerId"].Visible = false;
			grdCustomerDetail.RootTable.Columns["SaleOrderId"].Visible = false;
			grdCustomerDetail.RootTable.HeaderLines = 1;
			foreach (GridEXColumn item in (JanusCollectionBase)grdCustomerDetail.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			grdCustomerDetail.RootTable.Columns["CustomerName"].Width = Constants.InventoryConstants.SupplierName;
			grdCustomerDetail.RootTable.Columns["SaleOrder"].Width = Constants.InventoryConstants.OrderNo * 2;
			grdCustomerDetail.RootTable.Columns["Qty"].Width = Constants.InventoryConstants.QtyConstant;
			grdCustomerDetail.RootTable.Columns.Add("Delete");
			grdCustomerDetail.RootTable.Columns["Delete"].Caption = "X";
			grdCustomerDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdCustomerDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdCustomerDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdCustomerDetail.RootTable.Columns["Delete"].Width = 25;
			grdCustomerDetail.RootTable.Columns["Delete"].Position = 0;
			grdCustomerDetail.RootTable.Columns.Add("Edit");
			grdCustomerDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdCustomerDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdCustomerDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdCustomerDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdCustomerDetail.RootTable.Columns["Edit"].Width = 40;
			grdCustomerDetail.RootTable.Columns["Edit"].Position = 1;
			grdCustomerDetail.FrozenColumns = 2;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
	}

	private void grdCustomerDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdCustomerDetail.CurrentRow;
			if (e.Column.Key == "Delete")
			{
				DeleteCustomerDetailRow(r);
			}
			if (e.Column.Key == "Edit")
			{
				grdCustomerDetail_DoubleClick(null, null);
			}
			grdCustomerDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteCustomerDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (updateDetailIndexCustomer == -1)
			{
				if (Conversion.ToInt((object)r.Cells["Id"].Value.ToString()) != 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						WorkOrderCustomer d = new WorkOrderCustomer();
						d.WorkOrderCustomerId = Conversion.ToInt(r.Cells["Id"].Value);
						d.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
						d.SaleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
						d.Qty = Conversion.ToDecimal((object)r.Cells["Qty"]);
						d.ActionTypeId = 3;
						lstRemoveRecordcustomerDetail.Add(d);
						dtCustomerDetail.Rows[r.RowIndex].Delete();
						r.Delete();
					}
				}
				else
				{
					dtCustomerDetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				return;
			}
			throw new Exception("Please Reset the Detail first..");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		FromReset();
	}

	private void FromReset()
	{
		try
		{
			RecId = 0;
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			lstRemoveRecordcustomerDetail.Clear();
			lstRemoveRecordItemDetail.Clear();
			lstRemoveRecordOperations.Clear();
			DocumentNoFill();
			txtdocdate.Value = DateTime.Now;
			txtManualNo.Text = string.Empty;
			((Control)(object)cmbBOMNo).Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbUOM).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtRemarksHeader.Text = string.Empty;
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbCustomerOrderNo).Text = string.Empty;
			txtCustomerOrderQty.Text = string.Empty;
			FromResetDetail();
			ResetItemDetail();
			ResetCustomerDetail();
			dtOperationProcess.Rows.Clear();
			dtCustomerDetail.Rows.Clear();
			dtItemDetail.Rows.Clear();
			grdOperationsDetail.ClearStructure();
			grdCustomerDetail.ClearStructure();
			grdItemDetail.ClearStructure();
			((Control)(object)cmbBOMNo).Enabled = true;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			txtdocdate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FromResetDetail()
	{
		try
		{
			((Control)(object)cmbWorkStationOperation).Text = string.Empty;
			txtHoursOp.Text = string.Empty;
			txtOpHoursPerDay.Text = string.Empty;
			txtDaysOp.Text = string.Empty;
			BtnAddOp.Visible = true;
			BtnCancelOp.Visible = false;
			BtnUpdateOp.Visible = false;
			cmbWorkStationOperation.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetItemDetail()
	{
		try
		{
			updateDetailIndexItem = -1;
			((Control)(object)CmbItemDetail).Text = string.Empty;
			((Control)(object)cmbItemUOM).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			BtnAddItem.Visible = true;
			BtnCancelItem.Visible = false;
			BtnUpdateItem.Visible = false;
			CmbItemDetail.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetCustomerDetail()
	{
		try
		{
			updateDetailIndexCustomer = -1;
			((Control)(object)CmbCustomerName).Text = string.Empty;
			((Control)(object)CmbCustomerOrderNo).Text = string.Empty;
			txtQtyDetail.Text = string.Empty;
			BtnAddCustomer.Visible = true;
			BtnCancelCustomer.Visible = false;
			BtnUpdateCustmer.Visible = false;
			CmbCustomerName.Focus();
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
			ItemFill();
			uomgetAll();
			int ItemId = Conversion.ToInt(cmbItem.Value);
			BomNoFill(ItemId);
			CustomerFill(ItemId);
			AllWorkStationOpertaionFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWorkOrder_FormClosed(object sender, FormClosedEventArgs e)
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

	private void txtCustomerRemarks_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "grdOperationsDetail")
			{
				cmbWorkStationOperation.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnCancelOp_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "tabControl1")
			{
				cmbWorkStationOperation.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnAddOp_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "cmbWorkStationOperation")
			{
				cmbWorkStationOperation.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Expected O, but got Unknown
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Expected O, but got Unknown
		//IL_064b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Expected O, but got Unknown
		//IL_0857: Unknown result type (might be due to invalid IL or missing references)
		//IL_085e: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdOperationsDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Operation Grid Record Not Found...\nGrid Should have atleast one record");
				return;
			}
			if (grdItemDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Item Grid Record Not Found...\nGrid Should have atleast one record");
				return;
			}
			WorkOrder Or = new WorkOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				Or.WorkOrderId = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			UpdateBOMInDetailGrid(grdOperationsDetail);
			if (Conversion.ToInt(CmbCustomerOrderNo.Value) > 0)
			{
				SaleOrder.ItemExistsAgainstSaleOrderNo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(cmbItem.Value), Conversion.ToInt(CmbCustomerOrderNo.Value));
			}
			Or.CompanyId = UserAccount.CompanyId;
			Or.BranchesId = UserAccount.BranchesId;
			Or.ProjectsId = UserAccount.BranchesId;
			Or.OrganizationId = UserAccount.OrganizationId;
			Or.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			Or.DocumentTypeId = 1614;
			Or.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			Or.DocNo = Conversion.ToInt((object)txtdocno.Text);
			Or.BomHeaderId = Conversion.ToInt(cmbBOMNo.Value);
			Or.ManualDocCode = txtManualNo.Text;
			Or.StartDate = Conversion.ToDateTime((object)txtStartdate.Value);
			Or.EndDate = Conversion.ToDateTime((object)txtEndDate.Value);
			Or.DeadLine = Conversion.ToDateTime((object)txtDeadLineDateMain.Value);
			Or.ItemId = Conversion.ToInt(cmbItem.Value);
			Or.ItemUomId = Conversion.ToInt(cmbUOM.Value);
			Or.ItemQty = Conversion.ToDecimal((object)txtQty.Text);
			Or.BatchDesc = txtBatchDesc.Text;
			Or.RemarksHeader = Conversion.ToString((object)txtRemarksHeader.Text);
			Or.EntryUserId = UserAccount.ID;
			Or.EntryDate = DateTime.Now;
			Or.ModifyDate = DateTime.Now;
			Or.ModifyUserId = UserAccount.ID;
			Or.ApprovedUserId = UserAccount.ID;
			Or.ApprovedDate = DateTime.Now;
			Or.IsApproved = false;
			Or.WorkOrderCustomerList = new List<WorkOrderCustomer>();
			Or.WorkOrderOperationList = new List<WorkOrderOperation>();
			Or.WorkOrderMaterialList = new List<workOrderMaterial>();
			Or.workOrderItemDetailList = new List<workOrderItemDetail>();
			if (lstRemoveRecordOperations.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecordOperations.Count; i++)
				{
					Or.WorkOrderOperationList.Add(lstRemoveRecordOperations[i]);
				}
			}
			GridEXRow[] rows = grdOperationsDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				if (Conversion.ToInt((object)r2.Cells["WorkStationOperationId"].Value.ToString()) != 0)
				{
					WorkOrderOperation d2 = new WorkOrderOperation();
					if (r2.Cells["WorkStationOperationId"].Text == string.Empty)
					{
						MessageBox.Show("WorkStationOperation Field Required in Grid Operations Row No: " + (r2.RowIndex + 1));
						return;
					}
					if (r2.Cells["OpDays"].Text == string.Empty)
					{
						MessageBox.Show("OpDays  Field Required in Grid Operations Row No: " + (r2.RowIndex + 1));
						return;
					}
					if (r2.Cells["OpHoursPerDay"].Text == string.Empty)
					{
						MessageBox.Show("OpHoursPerDay  Field Required in Grid Operations Row No: " + (r2.RowIndex + 1));
						return;
					}
					d2.WorkOrderOperationId = ((RecId != 0) ? Conversion.ToInt(r2.Cells["Id"].Value) : 0);
					d2.ActionTypeId = ((d2.WorkOrderOperationId <= 0) ? 1 : 2);
					d2.BOMHeaderId = Conversion.ToInt(r2.Cells["BOMHeaderId"].Value);
					d2.BOMOperationDetailId = Conversion.ToInt(r2.Cells["BOMOperationDetailId"].Value);
					d2.ItemId = 0;
					d2.ItemUomId = 0;
					d2.Qty = 0m;
					d2.WorkStationOperationId = Conversion.ToInt(r2.Cells["WorkStationOperationId"].Value);
					d2.OperationDays = Conversion.ToDecimal(r2.Cells["OpDays"].Value);
					d2.OpHoursPerDay = Conversion.ToDecimal(r2.Cells["OpHoursPerDay"].Value);
					d2.OperationHours = Conversion.ToDecimal((object)r2.Cells["OpHours"].Text);
					d2.DeadLine = Conversion.ToDateTime(r2.Cells["DeadLine"].Value);
					Or.WorkOrderOperationList.Add(d2);
				}
			}
			if (lstRemoveRecordItemDetail.Count > 0 && RecId > 0)
			{
				for (int k = 0; k < lstRemoveRecordItemDetail.Count; k++)
				{
					Or.WorkOrderMaterialList.Add(lstRemoveRecordItemDetail[k]);
				}
			}
			rows = grdItemDetail.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToInt((object)r3.Cells["ItemId"].Value.ToString()) != 0)
				{
					workOrderMaterial d3 = new workOrderMaterial();
					d3.WorkOrderMaterialId = Conversion.ToInt(r3.Cells["Id"].Value);
					d3.WorkOrderMaterialId = ((RecId != 0) ? Conversion.ToInt(r3.Cells["Id"].Value) : 0);
					d3.ActionTypeId = ((d3.WorkOrderMaterialId <= 0) ? 1 : 2);
					d3.ItemId = Conversion.ToInt(r3.Cells["ItemId"].Value);
					d3.uomId = Conversion.ToInt(r3.Cells["UOMId"].Value);
					d3.Qty = Conversion.ToDecimal((object)r3.Cells["Qty"].Text);
					if (d3.ItemId == 0)
					{
						MessageBox.Show("Item Field Required in Item_Grid Row No: " + (r3.RowIndex + 1));
						return;
					}
					if (d3.uomId == 0)
					{
						MessageBox.Show("Uom Field Required in Item_Grid Row No: " + (r3.RowIndex + 1));
						return;
					}
					if (d3.Qty == 0m)
					{
						MessageBox.Show("Qty Field Required in Item_Grid Row No: " + (r3.RowIndex + 1));
						return;
					}
					Or.WorkOrderMaterialList.Add(d3);
				}
			}
			if (lstRemoveRecordcustomerDetail.Count > 0 && RecId > 0)
			{
				for (int j = 0; j < lstRemoveRecordcustomerDetail.Count; j++)
				{
					Or.WorkOrderCustomerList.Add(lstRemoveRecordcustomerDetail[j]);
				}
			}
			rows = grdCustomerDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["CustomerId"].Value) > 0)
				{
					WorkOrderCustomer d = new WorkOrderCustomer();
					d.WorkOrderCustomerId = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					d.ActionTypeId = ((d.WorkOrderCustomerId <= 0) ? 1 : 2);
					d.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
					d.SaleOrderId = Conversion.ToInt(r.Cells["SaleOrderId"].Value);
					d.Qty = Conversion.ToDecimal(r.Cells["Qty"].Value);
					if (d.CustomerId == 0)
					{
						MessageBox.Show("Customer Field Required in Customer_Grid Row No: " + (r.RowIndex + 1));
						return;
					}
					if (d.SaleOrderId == 0)
					{
						MessageBox.Show("SaleOrder Field Required in Customer_Grid Row No: " + (r.RowIndex + 1));
						return;
					}
					if (d.Qty == 0m)
					{
						MessageBox.Show("Qty Field Required in Customer_Grid Row No: " + (r.RowIndex + 1));
						return;
					}
					Or.WorkOrderCustomerList.Add(d);
				}
			}
			Or.AttachmentsList = new List<DMSAttachments>();
			Or.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<WorkOrder>(Or, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			Or.AttachmentsValues = AttachmentsValues;
			Or.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = WorkOrder.Save(Or);
			Or.WorkOrderId = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + Or.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1614, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + Or.DocNo);
			}
			FromReset();
			if (ChkPreview.Checked && formright.DoHavePrintRights)
			{
				WorkOrderSlip(Or.WorkOrderId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			dtOperationProcess.Clear();
			grdOperationsDetail.ClearStructure();
			dtItemDetail.Clear();
			grdItemDetail.ClearStructure();
			dtCustomerDetail.Clear();
			grdCustomerDetail.ClearStructure();
			lstRemoveRecordcustomerDetail.Clear();
			lstRemoveRecordItemDetail.Clear();
			lstRemoveRecordOperations.Clear();
			RecId = Id;
			WorkOrder Or = WorkOrder.ReadById(RecId);
			if (Or == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			UpdateMode = true;
			btnsave.Visible = false;
			btnSaveAs.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			txtdocdate.Value = Or.DocDate;
			txtdocno.Text = Conversion.ToString((object)Or.DocNo);
			cmbBOMNo.Value = Or.BomHeaderId;
			if (Or.BomHeaderId > 0)
			{
				((Control)(object)cmbBOMNo).Enabled = false;
			}
			else
			{
				((Control)(object)cmbBOMNo).Enabled = true;
			}
			txtManualNo.Text = Or.ManualDocCode;
			txtStartdate.Value = Or.StartDate;
			txtEndDate.Value = Or.EndDate;
			txtDeadLineDateMain.Value = Or.DeadLine;
			txtRemarksHeader.Text = Conversion.ToString((object)Or.RemarksHeader);
			cmbItem.Value = Or.ItemId;
			cmbItem_Leave(null, null);
			cmbUOM.Value = Or.ItemUomId;
			txtQty.Text = Or.ItemQty.ToString("#,##.###");
			txtBatchDesc.Text = Conversion.ToString((object)Or.BatchDesc);
			AttachmentsValues = Or.AttachmentsValues;
			CustomAttachmentsValues = Or.CustomAttachmentsValues;
			if (Or.WorkOrderOperationList.Count > 0)
			{
				for (int k = 0; k < Or.WorkOrderOperationList.Count; k++)
				{
					dtOperationProcess.Rows.Add(Or.WorkOrderOperationList[k].WorkOrderOperationId, Or.WorkOrderOperationList[k].BOMHeaderId, Or.WorkOrderOperationList[k].BOMOperationDetailId, Or.WorkOrderOperationList[k].WorkStationOperationId, Or.WorkOrderOperationList[k].WorkStationName, Or.WorkOrderOperationList[k].OperationName, Or.WorkOrderOperationList[k].OperationHours, Or.WorkOrderOperationList[k].OpHoursPerDay, Or.WorkOrderOperationList[k].OperationDays, Or.WorkOrderOperationList[k].DeadLine);
				}
			}
			else
			{
				grdOperationsDetail.ClearStructure();
			}
			grdOperationsDetail.DataSource = dtOperationProcess;
			grdOperationsDetail.RetrieveStructure();
			grdOperationsDetailSetting();
			if (Or.WorkOrderMaterialList.Count > 0)
			{
				for (int j = 0; j < Or.WorkOrderMaterialList.Count; j++)
				{
					dtItemDetail.Rows.Add(Or.WorkOrderMaterialList[j].WorkOrderMaterialId, Or.WorkOrderMaterialList[j].ItemId, Or.WorkOrderMaterialList[j].ItemName, Or.WorkOrderMaterialList[j].uomId, Or.WorkOrderMaterialList[j].PackUom, Or.WorkOrderMaterialList[j].Qty);
				}
			}
			else
			{
				grdItemDetail.ClearStructure();
			}
			grdItemDetail.DataSource = dtItemDetail;
			grdItemDetail.RetrieveStructure();
			grdItemDetailSetting();
			if (Or.WorkOrderCustomerList.Count > 0)
			{
				for (int i = 0; i < Or.WorkOrderCustomerList.Count; i++)
				{
					dtCustomerDetail.Rows.Add(Or.WorkOrderCustomerList[i].WorkOrderCustomerId, Or.WorkOrderCustomerList[i].CustomerId, Or.WorkOrderCustomerList[i].CustomerName, Or.WorkOrderCustomerList[i].SaleOrderId, Or.WorkOrderCustomerList[i].SaleOrderNo, Or.WorkOrderCustomerList[i].Qty);
				}
			}
			else
			{
				grdCustomerDetail.ClearStructure();
			}
			grdCustomerDetail.DataSource = dtCustomerDetail;
			grdCustomerDetail.RetrieveStructure();
			grdCustomerDetailSetting();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					WorkOrder.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					FromReset();
				}
			}
			else
			{
				MessageBox.Show("No record found to Delete");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyBOM_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtQtyDetail_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			WorkOrderSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkOrderSlip(int PrintId)
	{
		try
		{
			CommonServices.WorkOrder_Slip1614(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllCombobind()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		try
		{
			int Idname = Conversion.ToInt(CmbBomNoHistory.Value);
			int Iditem = Conversion.ToInt(cmbItemhistory.Value);
			DataTable dtcombo = WorkOrder.GetDataForDropDownFromWorkOrder(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtcombo.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtBOMName = new DataTable();
			dtBOMName.Columns.Add("Id", typeof(int));
			dtBOMName.Columns.Add("BOMName");
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName");
			for (int i = 0; i < dtcombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "BOMName")
				{
					dtBOMName.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ItemHeader")
				{
					dtitem.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
				}
			}
			if (dtBOMName.Rows.Count > 0)
			{
				DDL.BindDDL(dtBOMName, CmbBomNoHistory, "Id", "BOMName", "BOM Name", ZeroIndex: true);
				((UltraGridBase)CmbBomNoHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Idname > 0)
				{
					if (dtBOMName.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idname))
					{
						CmbBomNoHistory.Value = Idname;
					}
					else
					{
						((Control)(object)CmbBomNoHistory).Text = string.Empty;
					}
				}
			}
			if (dtitem.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtitem, cmbItemhistory, "Id", "ItemName", "Item Name", ZeroIndex: true);
			((UltraGridBase)cmbItemhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Iditem > 0)
			{
				if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Iditem))
				{
					cmbItemhistory.Value = Iditem;
				}
				else
				{
					((Control)(object)cmbItemhistory).Text = string.Empty;
				}
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
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: true);
			((UltraGridBase)cmbDateTypeHistory).Rows[2].Activate();
			((UltraGridBase)cmbDateTypeHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			int num = Conversion.ToInt((object)((DisposableObjectCollectionBase)((UltraGridBase)CmbBomNoHistory).Rows).Count);
			int IditemHis = Conversion.ToInt((object)((DisposableObjectCollectionBase)((UltraGridBase)cmbItemhistory).Rows).Count);
			if (num == 0 && IditemHis == 0)
			{
				Datetypefill();
				AllCombobind();
			}
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			dtHistory.Clear();
			ReportsParameters header = new ReportsParameters();
			header.CompanyId = UserAccount.CompanyId;
			header.OrganizationId = UserAccount.OrganizationId;
			header.BranchesId = UserAccount.BranchesId;
			header.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			header.DocumentTypeId = 1614;
			header.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text.Trim());
			header.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text.Trim());
			header.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			header.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			header.Id = Conversion.ToInt(CmbBomNoHistory.Value);
			header.ItemId = Conversion.ToInt(cmbItemhistory.Value);
			header.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtHistory = WorkOrder.FormHistory(header);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocNo", typeof(string));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("ManualDocCode", typeof(string));
				dtcol.Columns.Add("BomNo", typeof(string));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("Uom", typeof(string));
				dtcol.Columns.Add("ItemQty", typeof(decimal));
				dtcol.Columns.Add("StartDate", typeof(DateTime));
				dtcol.Columns.Add("EndDate", typeof(DateTime));
				dtcol.Columns.Add("DeadLine", typeof(string));
				dtcol.Columns.Add("BatchDesc", typeof(string));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				dtcol.Columns.Add("EntryUserName", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUserName", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("IsApproved", typeof(bool));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Rows.Clear();
				foreach (DataRow row in dtHistory.Rows)
				{
					dtcol.Rows.Add(row["RecordNo"], row["Id"], row["DocumentTypeId"], row["DocNo"], row["DocDate"], row["ManualDocCode"], row["BomHeaderNo"], row["ItemName"], row["Uom"], row["ItemQty"], row["StartDate"], row["EndDate"], row["DeadLine"], row["BatchDesc"], row["RemarksHeader"], row["EntryUserName"], row["EntryDate"], row["ModifyUserName"], row["ModifyDate"], row["IsApproved"], row["NoOfAttachments"]);
				}
				DatagridHistory.DataSource = dtcol;
				DatagridHistory.RetrieveStructure();
				HistoryGridSetting();
			}
			else
			{
				DatagridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSetting()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)DatagridHistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			DatagridHistory.RootTable.Columns["RecordNo"].Width = Constants.InventoryConstants.RecordNo;
			DatagridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["ManualDocCode"].Width = Constants.InventoryConstants.RecordNo;
			DatagridHistory.RootTable.Columns["BomNo"].Width = Constants.InventoryConstants.InvoiceNoConstant;
			DatagridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DatagridHistory.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			DatagridHistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			DatagridHistory.RootTable.Columns["StartDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["EndDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["DeadLine"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["BatchDesc"].Width = Constants.InventoryConstants.OtherDescription;
			DatagridHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			DatagridHistory.RootTable.Columns["EntryUserName"].Width = Constants.InventoryConstants.EntryUserConstants;
			DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["ModifyUserName"].Width = Constants.InventoryConstants.ModifyUserConstants;
			DatagridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["IsApproved"].Width = Constants.InventoryConstants.IsApproved;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DatagridHistory.RootTable.Columns.Add("Print");
			DatagridHistory.RootTable.Columns["Print"].Key = "Print";
			DatagridHistory.RootTable.Columns["Print"].Caption = "Print";
			DatagridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DatagridHistory.RootTable.Columns["Print"].Width = 50;
			DatagridHistory.RootTable.Columns["Print"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Key = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			DatagridHistory.RootTable.Columns["Edit"].Position = 1;
			DatagridHistory.RootTable.Columns.Add("SaveAs");
			DatagridHistory.RootTable.Columns["SaveAs"].Key = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			DatagridHistory.RootTable.Columns["SaveAs"].Width = 60;
			DatagridHistory.RootTable.Columns["SaveAs"].Position = 2;
			DatagridHistory.FrozenColumns = 3;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				ReadById(Conversion.ToInt((object)DatagridHistory.CurrentRow.Cells["Id"].Value.ToString()));
				return;
			}
			throw new Exception("ypu don't have updae rights...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (e.Column.Key == "Print")
			{
				WorkOrderSlip(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Edit")
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "SaveAs")
			{
				DataGridHistory_SaveAs(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(r.Cells["Id"].Value), 1614);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SaveAs(int CurrId)
	{
		try
		{
			ReadById(CurrId);
			btnsave.Visible = false;
			btnDelete.Visible = false;
			btnUpdate.Visible = false;
			btnSaveAs.Visible = true;
			txtdocdate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				DatagridHistoryDetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistoryDetailbind(int Id)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		try
		{
			grdhistoryoperations.ClearStructure();
			WorkOrder Or = new WorkOrder();
			Or = WorkOrder.ReadById(Id);
			if (Or == null)
			{
				return;
			}
			DataTable dtOperationProcessHistory = new DataTable();
			dtOperationProcessHistory.Columns.Add("Id", typeof(int));
			dtOperationProcessHistory.Columns.Add("WorkStationName", typeof(string));
			dtOperationProcessHistory.Columns.Add("OperationName", typeof(string));
			dtOperationProcessHistory.Columns.Add("OpHours", typeof(decimal));
			dtOperationProcessHistory.Columns.Add("OpHoursPerDay", typeof(decimal));
			dtOperationProcessHistory.Columns.Add("OpDays", typeof(int));
			dtOperationProcessHistory.Columns.Add("DeadLine", typeof(DateTime));
			if (Or.WorkOrderOperationList.Count > 0)
			{
				for (int i = 0; i < Or.WorkOrderOperationList.Count; i++)
				{
					dtOperationProcessHistory.Rows.Add(Or.WorkOrderOperationList[i].WorkOrderOperationId, Or.WorkOrderOperationList[i].WorkStationName, Or.WorkOrderOperationList[i].OperationName, Or.WorkOrderOperationList[i].OperationHours, Or.WorkOrderOperationList[i].OpHoursPerDay, Or.WorkOrderOperationList[i].OperationDays, Or.WorkOrderOperationList[i].DeadLine);
				}
			}
			else
			{
				grdhistoryoperations.ClearStructure();
			}
			grdhistoryoperations.DataSource = dtOperationProcessHistory;
			grdhistoryoperations.RetrieveStructure();
			grdHistoryOperationSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistoryOperationSetting()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		try
		{
			grdhistoryoperations.RootTable.Columns["Id"].Visible = false;
			grdhistoryoperations.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistoryoperations.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)1;
				CommonServices.GridColumnSettings(item);
			}
			grdhistoryoperations.RootTable.Columns["WorkStationName"].Width = Constants.InventoryConstants.WorkStationName;
			grdhistoryoperations.RootTable.Columns["OperationName"].Width = Constants.InventoryConstants.OperationName;
			grdhistoryoperations.RootTable.Columns["OpHours"].Width = Constants.InventoryConstants.Days;
			grdhistoryoperations.RootTable.Columns["OpHoursPerDay"].Width = Constants.InventoryConstants.Days;
			grdhistoryoperations.RootTable.Columns["OpDays"].Width = Constants.InventoryConstants.Days;
			grdhistoryoperations.RootTable.Columns["DeadLine"].Width = Constants.InventoryConstants.DateConstant;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnHistoryRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)cmbDateTypeHistory).Text = string.Empty;
			((Control)(object)cmbItemhistory).Text = string.Empty;
			((Control)(object)CmbBomNoHistory).Text = string.Empty;
			grdhistoryoperations.ClearStructure();
			cmbDateTypeHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmBillOfMaterial_Manufacturing_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtdocdate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DatagridHistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete)
				{
					if (formright.DoHaveCanDelete && btnDelete.Enabled)
					{
						btnDelete_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnPrint.Visible && btnPrint.Enabled)
					{
						btnPrint_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				else if (e.Control && e.KeyCode == Keys.F12)
				{
					if (btnSaveAs.Visible && btnSaveAs.Enabled)
					{
						btnSaveAs_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdOperationsDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtdocdate.Focus();
				}
				else if (e.Control && e.KeyCode != Keys.Right)
				{
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdhistoryoperations).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)DatagridHistory).Focus();
				}
				else if (e.Control)
				{
					_ = e.KeyCode;
					_ = 39;
				}
			}
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
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

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void grdOperationsDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOperationsDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdOperationsDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdOperationsDetail.CurrentColumn.Key == "Delete")
				{
					DeleteOperationDetailRow(r);
				}
				if (grdOperationsDetail.CurrentColumn.Key == "Edit")
				{
					grdOperationsDetail_DoubleClick(null, null);
				}
				grdOperationsDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteOperationDetailRow(r);
				grdOperationsDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdOperationsDetail_DoubleClick(null, null);
			}
			if (e.KeyCode == Keys.F1 && grdOperationsDetail.CurrentColumn != null)
			{
				_ = grdOperationsDetail.CurrentColumn.Key == "OverHeadAccountId";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdItemDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdItemDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdItemDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdItemDetail.CurrentColumn.Key == "Delete")
				{
					DeleteItemDetailRow(r);
				}
				if (grdItemDetail.CurrentColumn.Key == "Edit")
				{
					grdItemDetail_DoubleClick(null, null);
				}
				grdItemDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteItemDetailRow(r);
				grdItemDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdItemDetail_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdCustomerDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdCustomerDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdCustomerDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdCustomerDetail.CurrentColumn.Key == "Delete")
				{
					DeleteCustomerDetailRow(r);
				}
				if (grdCustomerDetail.CurrentColumn.Key == "Edit")
				{
					grdCustomerDetail_DoubleClick(null, null);
				}
				grdCustomerDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteCustomerDetailRow(r);
				grdCustomerDetail.UpdateData();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdCustomerDetail_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DatagridHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn.Key == "Edit")
				{
					if (!formright.DoHaveUpdateRights)
					{
						throw new Exception("you don't have update rights...");
					}
					ReadById(Conversion.ToInt((object)CurrId));
				}
				if (DatagridHistory.CurrentColumn.Key == "Print")
				{
					WorkOrderSlip(CurrId);
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1614);
				}
				if (DatagridHistory.CurrentColumn.Key == "SaveAs")
				{
					DataGridHistory_SaveAs(CurrId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				if (!formright.DoHaveUpdateRights)
				{
					throw new Exception("you don't have update rights...");
				}
				ReadById(Conversion.ToInt((object)CurrId));
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
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
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f9: Expected O, but got Unknown
		//IL_0510: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Expected O, but got Unknown
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_0644: Unknown result type (might be due to invalid IL or missing references)
		//IL_064e: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066f: Expected O, but got Unknown
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Expected O, but got Unknown
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f3: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0714: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f0: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0806: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092f: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0945: Expected O, but got Unknown
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0966: Expected O, but got Unknown
		//IL_0f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fa8: Expected O, but got Unknown
		//IL_0fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fbf: Expected O, but got Unknown
		//IL_1fb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fbf: Expected O, but got Unknown
		//IL_1fcc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd6: Expected O, but got Unknown
		//IL_2fae: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb8: Expected O, but got Unknown
		//IL_2fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fcf: Expected O, but got Unknown
		//IL_6f5d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f67: Expected O, but got Unknown
		//IL_6f8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f95: Expected O, but got Unknown
		GridEXLayout grdItemDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		GridEXLayout grdOperationsDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance101 = new Appearance();
		GridEXLayout grdCustomerDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistoryoperations_DesignTimeLayout = new GridEXLayout();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panelItem = new System.Windows.Forms.Panel();
		this.grdItemDetail = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.CmbItemDetail = new UltraCombo();
		this.BtnAddItem = new System.Windows.Forms.Button();
		this.txtQtyDetail = new System.Windows.Forms.TextBox();
		this.BtnCancelItem = new System.Windows.Forms.Button();
		this.BtnUpdateItem = new System.Windows.Forms.Button();
		this.label26 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.cmbItemUOM = new UltraCombo();
		this.panelOperation = new System.Windows.Forms.Panel();
		this.grdOperationsDetail = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.panel16 = new System.Windows.Forms.Panel();
		this.BtnAddOp = new System.Windows.Forms.Button();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbWorkStationOperation = new UltraCombo();
		this.BtnCancelOp = new System.Windows.Forms.Button();
		this.BtnUpdateOp = new System.Windows.Forms.Button();
		this.txtDeadLineDateOp = new System.Windows.Forms.DateTimePicker();
		this.label14 = new System.Windows.Forms.Label();
		this.txtDaysOp = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtOpHoursPerDay = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtHoursOp = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdCustomerDetail = new GridEX();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.BtnAddCustomer = new System.Windows.Forms.Button();
		this.CmbCustomerOrderNo = new UltraCombo();
		this.BtnCancelCustomer = new System.Windows.Forms.Button();
		this.BtnUpdateCustmer = new System.Windows.Forms.Button();
		this.CmbCustomerName = new UltraCombo();
		this.txtCustomerOrderQty = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.comCompany = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtManualNo = new System.Windows.Forms.TextBox();
		this.cmbBOMNo = new UltraCombo();
		this.txtBatchDesc = new System.Windows.Forms.TextBox();
		this.cmbItem = new UltraCombo();
		this.cmbUOM = new UltraCombo();
		this.txtEndDate = new System.Windows.Forms.DateTimePicker();
		this.txtStartdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtDeadLineDateMain = new System.Windows.Forms.DateTimePicker();
		this.txtRemarksHeader = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.combranches = new UltraCombo();
		this.label45 = new System.Windows.Forms.Label();
		this.comproject = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.label21 = new System.Windows.Forms.Label();
		this.ChkPreview = new System.Windows.Forms.CheckBox();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.DatagridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.grdhistoryoperations = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label17 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.cmbItemhistory = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbBomNoHistory = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label28 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label35 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel38 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.label32 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panelItem.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdItemDetail).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).BeginInit();
		this.panelOperation.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOperationsDetail).BeginInit();
		this.panel16.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWorkStationOperation).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryoperations).BeginInit();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel38.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1086, 561);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1086, 561);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1078, 532);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panelItem);
		this.panel5.Controls.Add(this.panelOperation);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 246);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1072, 283);
		this.panel5.TabIndex = 1;
		this.panelItem.Controls.Add((System.Windows.Forms.Control)(object)this.grdItemDetail);
		this.panelItem.Controls.Add(this.panel7);
		this.panelItem.Controls.Add(this.panel6);
		this.panelItem.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panelItem.Location = new System.Drawing.Point(636, 0);
		this.panelItem.Name = "panelItem";
		this.panelItem.Size = new System.Drawing.Size(436, 283);
		this.panelItem.TabIndex = 6;
		this.grdItemDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdItemDetail).BackColor = System.Drawing.Color.White;
		grdItemDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdItemDetail.DesignTimeLayout = grdItemDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdItemDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdItemDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdItemDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdItemDetail).Location = new System.Drawing.Point(0, 78);
		((System.Windows.Forms.Control)(object)this.grdItemDetail).Name = "grdItemDetail";
		((ScrollableBase)this.grdItemDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdItemDetail).Size = new System.Drawing.Size(436, 205);
		((System.Windows.Forms.Control)(object)this.grdItemDetail).TabIndex = 1;
		this.grdItemDetail.TotalRow = (InheritableBoolean)1;
		this.grdItemDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdItemDetail).VisualStyle = (VisualStyle)4;
		this.grdItemDetail.CellUpdated += new ColumnActionEventHandler(grdItemDetail_CellUpdated);
		this.grdItemDetail.ColumnButtonClick += new ColumnActionEventHandler(grdItemDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdItemDetail).DoubleClick += new System.EventHandler(grdItemDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdItemDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdItemDetail_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 55);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(436, 23);
		this.panel7.TabIndex = 146;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemDetail);
		this.panel6.Controls.Add(this.BtnAddItem);
		this.panel6.Controls.Add(this.txtQtyDetail);
		this.panel6.Controls.Add(this.BtnCancelItem);
		this.panel6.Controls.Add(this.BtnUpdateItem);
		this.panel6.Controls.Add(this.label26);
		this.panel6.Controls.Add(this.label16);
		this.panel6.Controls.Add(this.label36);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOM);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(436, 55);
		this.panel6.TabIndex = 0;
		this.CmbItemDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbItemDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance10).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Location = new System.Drawing.Point(3, 25);
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Name = "CmbItemDetail";
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Size = new System.Drawing.Size(195, 25);
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbItemDetail).Leave += new System.EventHandler(CmbItemDetail_Leave);
		this.BtnAddItem.BackColor = System.Drawing.Color.Teal;
		this.BtnAddItem.FlatAppearance.BorderSize = 0;
		this.BtnAddItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddItem.ForeColor = System.Drawing.Color.White;
		this.BtnAddItem.Location = new System.Drawing.Point(360, 28);
		this.BtnAddItem.Name = "BtnAddItem";
		this.BtnAddItem.Size = new System.Drawing.Size(39, 23);
		this.BtnAddItem.TabIndex = 3;
		this.BtnAddItem.Text = "+";
		this.BtnAddItem.UseVisualStyleBackColor = false;
		this.BtnAddItem.Click += new System.EventHandler(BtnAddItem_Click);
		this.txtQtyDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQtyDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQtyDetail.Location = new System.Drawing.Point(268, 26);
		this.txtQtyDetail.Name = "txtQtyDetail";
		this.txtQtyDetail.Size = new System.Drawing.Size(90, 23);
		this.txtQtyDetail.TabIndex = 2;
		this.txtQtyDetail.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQtyDetail_KeyPress);
		this.BtnCancelItem.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelItem.FlatAppearance.BorderSize = 0;
		this.BtnCancelItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelItem.ForeColor = System.Drawing.Color.White;
		this.BtnCancelItem.Location = new System.Drawing.Point(360, 3);
		this.BtnCancelItem.Name = "BtnCancelItem";
		this.BtnCancelItem.Size = new System.Drawing.Size(64, 23);
		this.BtnCancelItem.TabIndex = 5;
		this.BtnCancelItem.Text = "Cancel";
		this.BtnCancelItem.UseVisualStyleBackColor = false;
		this.BtnCancelItem.Visible = false;
		this.BtnCancelItem.Click += new System.EventHandler(BtnCancelItem_Click);
		this.BtnUpdateItem.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateItem.FlatAppearance.BorderSize = 0;
		this.BtnUpdateItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateItem.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateItem.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateItem.Location = new System.Drawing.Point(360, 28);
		this.BtnUpdateItem.Name = "BtnUpdateItem";
		this.BtnUpdateItem.Size = new System.Drawing.Size(64, 23);
		this.BtnUpdateItem.TabIndex = 141;
		this.BtnUpdateItem.Text = "Update";
		this.BtnUpdateItem.UseVisualStyleBackColor = false;
		this.BtnUpdateItem.Visible = false;
		this.BtnUpdateItem.Click += new System.EventHandler(BtnUpdateItem_Click);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(199, 5);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(35, 15);
		this.label26.TabIndex = 140;
		this.label26.Text = "UOM";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(268, 5);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(26, 15);
		this.label16.TabIndex = 13;
		this.label16.Text = "Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(3, 6);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(67, 15);
		this.label36.TabIndex = 139;
		this.label36.Text = "Item Name";
		this.cmbItemUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance157).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance157).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance157;
		((AppearanceBase)appearance159).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance159;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance166).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Location = new System.Drawing.Point(200, 25);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Name = "cmbItemUOM";
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Size = new System.Drawing.Size(66, 25);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).TabIndex = 1;
		this.panelOperation.Controls.Add((System.Windows.Forms.Control)(object)this.grdOperationsDetail);
		this.panelOperation.Controls.Add(this.panel14);
		this.panelOperation.Controls.Add(this.panel16);
		this.panelOperation.Dock = System.Windows.Forms.DockStyle.Left;
		this.panelOperation.Location = new System.Drawing.Point(0, 0);
		this.panelOperation.Name = "panelOperation";
		this.panelOperation.Size = new System.Drawing.Size(636, 283);
		this.panelOperation.TabIndex = 5;
		this.grdOperationsDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdOperationsDetail).BackColor = System.Drawing.Color.White;
		grdOperationsDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOperationsDetail.DesignTimeLayout = grdOperationsDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOperationsDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Location = new System.Drawing.Point(0, 78);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Name = "grdOperationsDetail";
		((ScrollableBase)this.grdOperationsDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).Size = new System.Drawing.Size(636, 205);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).TabIndex = 1;
		this.grdOperationsDetail.TotalRow = (InheritableBoolean)1;
		this.grdOperationsDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOperationsDetail).VisualStyle = (VisualStyle)4;
		this.grdOperationsDetail.CellUpdated += new ColumnActionEventHandler(grdOperationsDetail_CellUpdated);
		this.grdOperationsDetail.ColumnButtonClick += new ColumnActionEventHandler(grdOperationsDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).DoubleClick += new System.EventHandler(grdOperationsDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdOperationsDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOperationsDetail_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 55);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(636, 23);
		this.panel14.TabIndex = 4;
		this.panel16.BackColor = System.Drawing.Color.White;
		this.panel16.Controls.Add(this.BtnAddOp);
		this.panel16.Controls.Add(this.label5);
		this.panel16.Controls.Add(this.label6);
		this.panel16.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation);
		this.panel16.Controls.Add(this.BtnCancelOp);
		this.panel16.Controls.Add(this.BtnUpdateOp);
		this.panel16.Controls.Add(this.txtDeadLineDateOp);
		this.panel16.Controls.Add(this.label14);
		this.panel16.Controls.Add(this.txtDaysOp);
		this.panel16.Controls.Add(this.label1);
		this.panel16.Controls.Add(this.txtOpHoursPerDay);
		this.panel16.Controls.Add(this.label3);
		this.panel16.Controls.Add(this.txtHoursOp);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(636, 55);
		this.panel16.TabIndex = 0;
		this.BtnAddOp.BackColor = System.Drawing.Color.Teal;
		this.BtnAddOp.FlatAppearance.BorderSize = 0;
		this.BtnAddOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddOp.ForeColor = System.Drawing.Color.White;
		this.BtnAddOp.Location = new System.Drawing.Point(568, 26);
		this.BtnAddOp.Name = "BtnAddOp";
		this.BtnAddOp.Size = new System.Drawing.Size(39, 22);
		this.BtnAddOp.TabIndex = 6;
		this.BtnAddOp.Text = "+";
		this.BtnAddOp.UseVisualStyleBackColor = false;
		this.BtnAddOp.Click += new System.EventHandler(BtnAddOp_Click);
		this.BtnAddOp.Leave += new System.EventHandler(BtnAddOp_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(281, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(39, 15);
		this.label5.TabIndex = 13;
		this.label5.Text = "Hours";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(455, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 15);
		this.label6.TabIndex = 138;
		this.label6.Text = "Dead Line";
		this.cmbWorkStationOperation.AllowNull = (DefaultableBoolean)1;
		this.cmbWorkStationOperation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWorkStationOperation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWorkStationOperation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance62).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance84).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWorkStationOperation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWorkStationOperation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Location = new System.Drawing.Point(4, 24);
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Name = "cmbWorkStationOperation";
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Size = new System.Drawing.Size(275, 26);
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbWorkStationOperation).Leave += new System.EventHandler(cmbWorkStationOperation_Leave);
		this.BtnCancelOp.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelOp.FlatAppearance.BorderSize = 0;
		this.BtnCancelOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelOp.ForeColor = System.Drawing.Color.White;
		this.BtnCancelOp.Location = new System.Drawing.Point(568, 4);
		this.BtnCancelOp.Name = "BtnCancelOp";
		this.BtnCancelOp.Size = new System.Drawing.Size(64, 20);
		this.BtnCancelOp.TabIndex = 8;
		this.BtnCancelOp.Text = "Cancel";
		this.BtnCancelOp.UseVisualStyleBackColor = false;
		this.BtnCancelOp.Visible = false;
		this.BtnCancelOp.Click += new System.EventHandler(BtnCancelOp_Click);
		this.BtnCancelOp.Leave += new System.EventHandler(BtnCancelOp_Leave);
		this.BtnUpdateOp.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateOp.FlatAppearance.BorderSize = 0;
		this.BtnUpdateOp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateOp.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateOp.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateOp.Location = new System.Drawing.Point(568, 26);
		this.BtnUpdateOp.Name = "BtnUpdateOp";
		this.BtnUpdateOp.Size = new System.Drawing.Size(64, 22);
		this.BtnUpdateOp.TabIndex = 7;
		this.BtnUpdateOp.Text = "Update";
		this.BtnUpdateOp.UseVisualStyleBackColor = false;
		this.BtnUpdateOp.Visible = false;
		this.BtnUpdateOp.Click += new System.EventHandler(BtnUpdateOp_Click);
		this.txtDeadLineDateOp.CustomFormat = "dd-MMM-yy";
		this.txtDeadLineDateOp.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDeadLineDateOp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDeadLineDateOp.Location = new System.Drawing.Point(455, 26);
		this.txtDeadLineDateOp.Name = "txtDeadLineDateOp";
		this.txtDeadLineDateOp.Size = new System.Drawing.Size(107, 22);
		this.txtDeadLineDateOp.TabIndex = 5;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(329, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(65, 15);
		this.label14.TabIndex = 13;
		this.label14.Text = "Hours/Day";
		this.txtDaysOp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDaysOp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDaysOp.Location = new System.Drawing.Point(398, 26);
		this.txtDaysOp.Name = "txtDaysOp";
		this.txtDaysOp.Size = new System.Drawing.Size(56, 23);
		this.txtDaysOp.TabIndex = 4;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(398, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(33, 15);
		this.label1.TabIndex = 25;
		this.label1.Text = "Days";
		this.txtOpHoursPerDay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOpHoursPerDay.Enabled = false;
		this.txtOpHoursPerDay.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOpHoursPerDay.Location = new System.Drawing.Point(329, 26);
		this.txtOpHoursPerDay.Name = "txtOpHoursPerDay";
		this.txtOpHoursPerDay.Size = new System.Drawing.Size(68, 23);
		this.txtOpHoursPerDay.TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(4, 5);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(145, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "Work Stations/Operations";
		this.txtHoursOp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtHoursOp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtHoursOp.Location = new System.Drawing.Point(281, 26);
		this.txtHoursOp.Name = "txtHoursOp";
		this.txtHoursOp.Size = new System.Drawing.Size(46, 23);
		this.txtHoursOp.TabIndex = 2;
		this.txtHoursOp.TextChanged += new System.EventHandler(txtHoursOp_TextChanged);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comCompany);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.groupBox2);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel2.Controls.Add(this.label45);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel2.Controls.Add(this.label46);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 58);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1072, 188);
		this.panel2.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdCustomerDetail);
		this.panel4.Controls.Add(this.groupBox3);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(502, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(566, 188);
		this.panel4.TabIndex = 129;
		this.grdCustomerDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdCustomerDetail).BackColor = System.Drawing.Color.White;
		grdCustomerDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdCustomerDetail.DesignTimeLayout = grdCustomerDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCustomerDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Location = new System.Drawing.Point(0, 60);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Name = "grdCustomerDetail";
		((ScrollableBase)this.grdCustomerDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).Size = new System.Drawing.Size(566, 128);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).TabIndex = 1;
		this.grdCustomerDetail.TotalRow = (InheritableBoolean)1;
		this.grdCustomerDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCustomerDetail).VisualStyle = (VisualStyle)4;
		this.grdCustomerDetail.CellUpdated += new ColumnActionEventHandler(grdCustomerDetail_CellUpdated);
		this.grdCustomerDetail.ColumnButtonClick += new ColumnActionEventHandler(grdCustomerDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).DoubleClick += new System.EventHandler(grdCustomerDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdCustomerDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdCustomerDetail_KeyDown);
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.BtnAddCustomer);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo);
		this.groupBox3.Controls.Add(this.BtnCancelCustomer);
		this.groupBox3.Controls.Add(this.BtnUpdateCustmer);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerName);
		this.groupBox3.Controls.Add(this.txtCustomerOrderQty);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox3.Location = new System.Drawing.Point(0, 0);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(566, 60);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Customer Info";
		this.BtnAddCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnAddCustomer.FlatAppearance.BorderSize = 0;
		this.BtnAddCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnAddCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnAddCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnAddCustomer.Location = new System.Drawing.Point(498, 32);
		this.BtnAddCustomer.Name = "BtnAddCustomer";
		this.BtnAddCustomer.Size = new System.Drawing.Size(39, 23);
		this.BtnAddCustomer.TabIndex = 3;
		this.BtnAddCustomer.Text = "+";
		this.BtnAddCustomer.UseVisualStyleBackColor = false;
		this.BtnAddCustomer.Click += new System.EventHandler(BtnAddCustomer_Click);
		this.CmbCustomerOrderNo.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance42).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Location = new System.Drawing.Point(290, 30);
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Name = "CmbCustomerOrderNo";
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).Size = new System.Drawing.Size(106, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerOrderNo).TabIndex = 1;
		this.CmbCustomerOrderNo.ValueChanged += new System.EventHandler(CmbCustomerOrderNo_ValueChanged);
		this.BtnCancelCustomer.BackColor = System.Drawing.Color.Teal;
		this.BtnCancelCustomer.FlatAppearance.BorderSize = 0;
		this.BtnCancelCustomer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnCancelCustomer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnCancelCustomer.ForeColor = System.Drawing.Color.White;
		this.BtnCancelCustomer.Location = new System.Drawing.Point(498, 8);
		this.BtnCancelCustomer.Name = "BtnCancelCustomer";
		this.BtnCancelCustomer.Size = new System.Drawing.Size(64, 23);
		this.BtnCancelCustomer.TabIndex = 4;
		this.BtnCancelCustomer.Text = "Cancel";
		this.BtnCancelCustomer.UseVisualStyleBackColor = false;
		this.BtnCancelCustomer.Visible = false;
		this.BtnCancelCustomer.Click += new System.EventHandler(BtnCancelCustomer_Click);
		this.BtnUpdateCustmer.BackColor = System.Drawing.Color.Teal;
		this.BtnUpdateCustmer.FlatAppearance.BorderSize = 0;
		this.BtnUpdateCustmer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnUpdateCustmer.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnUpdateCustmer.ForeColor = System.Drawing.Color.White;
		this.BtnUpdateCustmer.Location = new System.Drawing.Point(498, 32);
		this.BtnUpdateCustmer.Name = "BtnUpdateCustmer";
		this.BtnUpdateCustmer.Size = new System.Drawing.Size(64, 23);
		this.BtnUpdateCustmer.TabIndex = 141;
		this.BtnUpdateCustmer.Text = "Update";
		this.BtnUpdateCustmer.UseVisualStyleBackColor = false;
		this.BtnUpdateCustmer.Visible = false;
		this.BtnUpdateCustmer.Click += new System.EventHandler(BtnUpdateCustmer_Click);
		this.CmbCustomerName.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance50;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance55).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance56).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance57).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Location = new System.Drawing.Point(5, 30);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Name = "CmbCustomerName";
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Size = new System.Drawing.Size(282, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCustomerName).Leave += new System.EventHandler(CmbCustomerName_Leave);
		this.txtCustomerOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCustomerOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCustomerOrderQty.Location = new System.Drawing.Point(398, 32);
		this.txtCustomerOrderQty.Name = "txtCustomerOrderQty";
		this.txtCustomerOrderQty.Size = new System.Drawing.Size(98, 23);
		this.txtCustomerOrderQty.TabIndex = 2;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(290, 13);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(81, 15);
		this.label10.TabIndex = 19;
		this.label10.Text = "Sale Order No";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(398, 13);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(5, 13);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(93, 15);
		this.label22.TabIndex = 9;
		this.label22.Text = "Customer Name";
		this.comCompany.AllowNull = (DefaultableBoolean)1;
		this.comCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.comCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.comCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance144).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance144).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance145;
		((SpecialBoxBase)((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance148).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance151).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).TextHAlignAsString = "Left";
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comCompany).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comCompany.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comCompany).Location = new System.Drawing.Point(1240, 15);
		((System.Windows.Forms.Control)(object)this.comCompany).Name = "comCompany";
		((System.Windows.Forms.Control)(object)this.comCompany).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.comCompany).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comCompany).Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(1170, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(63, 17);
		this.label4.TabIndex = 126;
		this.label4.Text = "Company";
		this.label4.Visible = false;
		this.groupBox2.Controls.Add(this.txtManualNo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBOMNo);
		this.groupBox2.Controls.Add(this.txtBatchDesc);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox2.Controls.Add(this.txtEndDate);
		this.groupBox2.Controls.Add(this.txtStartdate);
		this.groupBox2.Controls.Add(this.txtdocdate);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label23);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Controls.Add(this.txtDeadLineDateMain);
		this.groupBox2.Controls.Add(this.txtRemarksHeader);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtQty);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(0, 0);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(502, 188);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main Information";
		this.txtManualNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNo.Location = new System.Drawing.Point(242, 16);
		this.txtManualNo.Name = "txtManualNo";
		this.txtManualNo.Size = new System.Drawing.Size(254, 23);
		this.txtManualNo.TabIndex = 6;
		this.cmbBOMNo.AllowNull = (DefaultableBoolean)1;
		this.cmbBOMNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBOMNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBOMNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance69).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance70).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBOMNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBOMNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Location = new System.Drawing.Point(242, 69);
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Name = "cmbBOMNo";
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbBOMNo).Leave += new System.EventHandler(cmbBOMNo_Leave);
		this.txtBatchDesc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBatchDesc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBatchDesc.Location = new System.Drawing.Point(242, 125);
		this.txtBatchDesc.Name = "txtBatchDesc";
		this.txtBatchDesc.Size = new System.Drawing.Size(254, 23);
		this.txtBatchDesc.TabIndex = 11;
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance82).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance83).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(242, 41);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(254, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(242, 97);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(89, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 9;
		this.txtEndDate.CustomFormat = "dd-MMM-yy";
		this.txtEndDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEndDate.Location = new System.Drawing.Point(64, 99);
		this.txtEndDate.Name = "txtEndDate";
		this.txtEndDate.Size = new System.Drawing.Size(106, 22);
		this.txtEndDate.TabIndex = 3;
		this.txtStartdate.CustomFormat = "dd-MMM-yy";
		this.txtStartdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtStartdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtStartdate.Location = new System.Drawing.Point(64, 71);
		this.txtStartdate.Name = "txtStartdate";
		this.txtStartdate.Size = new System.Drawing.Size(106, 22);
		this.txtStartdate.TabIndex = 2;
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(64, 43);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(106, 22);
		this.txtdocdate.TabIndex = 1;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(64, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(106, 23);
		this.txtdocno.TabIndex = 0;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 129);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(60, 15);
		this.label25.TabIndex = 138;
		this.label25.Text = "Dead Line";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(3, 103);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(55, 15);
		this.label9.TabIndex = 138;
		this.label9.Text = "End Date";
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(3, 75);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(60, 15);
		this.label23.TabIndex = 138;
		this.label23.Text = "Start Date";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(3, 47);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 138;
		this.label2.Text = "Doc Date";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(3, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(48, 15);
		this.label37.TabIndex = 138;
		this.label37.Text = "Doc No";
		this.txtDeadLineDateMain.CustomFormat = "dd-MMM-yy";
		this.txtDeadLineDateMain.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDeadLineDateMain.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDeadLineDateMain.Location = new System.Drawing.Point(64, 125);
		this.txtDeadLineDateMain.Name = "txtDeadLineDateMain";
		this.txtDeadLineDateMain.Size = new System.Drawing.Size(106, 22);
		this.txtDeadLineDateMain.TabIndex = 4;
		this.txtRemarksHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHeader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksHeader.Location = new System.Drawing.Point(64, 150);
		this.txtRemarksHeader.Multiline = true;
		this.txtRemarksHeader.Name = "txtRemarksHeader";
		this.txtRemarksHeader.Size = new System.Drawing.Size(432, 34);
		this.txtRemarksHeader.TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(175, 75);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(53, 15);
		this.label11.TabIndex = 130;
		this.label11.Text = "BOM No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(3, 154);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(52, 15);
		this.label8.TabIndex = 131;
		this.label8.Text = "Remarks";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(175, 20);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(70, 15);
		this.label12.TabIndex = 131;
		this.label12.Text = "W.Order No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(175, 47);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(32, 15);
		this.label7.TabIndex = 85;
		this.label7.Text = "Item";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtQty.Location = new System.Drawing.Point(357, 99);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(139, 23);
		this.txtQty.TabIndex = 10;
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(331, 103);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(26, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Qty";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(175, 129);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(66, 15);
		this.label13.TabIndex = 87;
		this.label13.Text = "Batch Desc";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(175, 103);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 87;
		this.label44.Text = "UOM";
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance131).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance131).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance131;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance132;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance133).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance133).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance103).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1240, 45);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1186, 50);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(47, 17);
		this.label45.TabIndex = 127;
		this.label45.Text = "Branch";
		this.label45.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance138;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance24).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1240, 78);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(58, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(1186, 83);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(48, 17);
		this.label46.TabIndex = 128;
		this.label46.Text = "Project";
		this.label46.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Controls.Add(this.label21);
		this.panel3.Controls.Add(this.ChkPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1072, 55);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnSaveAs, this.btnUpdate, this.btnDelete, this.toolStripSeparator, this.btnAttachment, this.btnPrint, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1072, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(100, 24);
		this.btnPrint.Text = "1614-&Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.White;
		this.label21.Location = new System.Drawing.Point(7, 32);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(103, 19);
		this.label21.TabIndex = 126;
		this.label21.Text = "Work Order";
		this.ChkPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPreview.AutoSize = true;
		this.ChkPreview.Checked = true;
		this.ChkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPreview.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPreview.Location = new System.Drawing.Point(991, 30);
		this.ChkPreview.Name = "ChkPreview";
		this.ChkPreview.Size = new System.Drawing.Size(78, 20);
		this.ChkPreview.TabIndex = 12;
		this.ChkPreview.Text = "Preview";
		this.ChkPreview.UseVisualStyleBackColor = true;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistoryoperations);
		this.tabPage3.Controls.Add(this.panel15);
		this.tabPage3.Controls.Add(this.panel13);
		this.tabPage3.Controls.Add(this.panel38);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1078, 532);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "History";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(0, 160);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1078, 155);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 1;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(0, 315);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1078, 24);
		this.panel9.TabIndex = 51;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(4, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(300, 16);
		this.label15.TabIndex = 129;
		this.label15.Text = "Operations Detail of the Above Selected Row";
		this.grdhistoryoperations.AllowEdit = (InheritableBoolean)2;
		grdhistoryoperations_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistoryoperations.DesignTimeLayout = grdhistoryoperations_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdhistoryoperations.DynamicFiltering = true;
		this.grdhistoryoperations.FilterMode = (FilterMode)2;
		this.grdhistoryoperations.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistoryoperations.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Location = new System.Drawing.Point(0, 339);
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Name = "grdhistoryoperations";
		((ScrollableBase)this.grdhistoryoperations).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).Size = new System.Drawing.Size(1078, 193);
		((System.Windows.Forms.Control)(object)this.grdhistoryoperations).TabIndex = 2;
		this.grdhistoryoperations.TotalRow = (InheritableBoolean)1;
		this.grdhistoryoperations.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistoryoperations).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar1);
		this.panel15.Controls.Add(this.label17);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 137);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1078, 23);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = null;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1031, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(9, 6, 9, 6);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 22);
		this.ctrlGrdBar1.TabIndex = 128;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(8, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(112, 16);
		this.label17.TabIndex = 127;
		this.label17.Text = "Filtered Records";
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add(this.groupBox1);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 52);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1078, 85);
		this.panel13.TabIndex = 0;
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemhistory);
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBomNoHistory);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox1.Controls.Add(this.txtToDateHistory);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.txtToDocNoHistory);
		this.groupBox1.Controls.Add(this.txtFromdateHistory);
		this.groupBox1.Controls.Add(this.label35);
		this.groupBox1.Controls.Add(this.txtFromNoHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(894, 77);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(821, 45);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 7;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.cmbItemhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance141;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance142;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance112).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Location = new System.Drawing.Point(682, 16);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Name = "cmbItemhistory";
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemhistory).TabIndex = 6;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(621, 22);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(32, 15);
		this.label29.TabIndex = 180;
		this.label29.Text = "Item";
		this.CmbBomNoHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbBomNoHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBomNoHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBomNoHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance150).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBomNoHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBomNoHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Location = new System.Drawing.Point(417, 44);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Name = "CmbBomNoHistory";
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBomNoHistory).TabIndex = 5;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(325, 50);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(56, 15);
		this.label20.TabIndex = 176;
		this.label20.Text = "BOM No.";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(5, 22);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(60, 15);
		this.label19.TabIndex = 174;
		this.label19.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(97, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(210, 46);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(5, 50);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(91, 15);
		this.label28.TabIndex = 171;
		this.label28.Text = "Date From && To";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(518, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(99, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(97, 46);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(325, 22);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(88, 15);
		this.label35.TabIndex = 173;
		this.label35.Text = "Doc From && To";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(417, 18);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(99, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel38.BackColor = System.Drawing.Color.Teal;
		this.panel38.Controls.Add(this.toolStrip2);
		this.panel38.Controls.Add(this.label32);
		this.panel38.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel38.Location = new System.Drawing.Point(0, 0);
		this.panel38.Name = "panel38";
		this.panel38.Size = new System.Drawing.Size(1078, 52);
		this.panel38.TabIndex = 48;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.toolStripSeparator1, this.btnRegister });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1078, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnHistoryRefresh_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnRegister.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(61, 24);
		this.btnRegister.Text = "&Print";
		this.btnRegister.Visible = false;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(5, 31);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(176, 19);
		this.label32.TabIndex = 127;
		this.label32.Text = "Work Order History  ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1086, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmWorkOrder";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmWorkOrder_FormClosed);
		base.Load += new System.EventHandler(frmBillOfMaterial_Manufacturing_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmBillOfMaterial_Manufacturing_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panelItem.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdItemDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).EndInit();
		this.panelOperation.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOperationsDetail).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWorkStationOperation).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCustomerDetail).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comCompany).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBOMNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistoryoperations).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBomNoHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel38.ResumeLayout(false);
		this.panel38.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
