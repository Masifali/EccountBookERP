using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.WeightBridge;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
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
namespace Architecture.WinApp.Inventory_Definition
{

public class GatePassGeneralOutward : Form
{
	private int Id;

	private int Qty;

	public int POId;

	private int DeliveryOrderId;

	private bool UpdateMode;

	private bool Approve;

	private DataTable table = new DataTable();

	private DataTable tablegrd = new DataTable();

	public DataTable DtHistory = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private Panel panel6;

	private Label label15;

	private Panel panel4;

	private TextBox txtvehicleno;

	private Panel panel9;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label17;

	private CheckBox ChkBox;

	internal UltraCombo CmbWarehouse;

	internal UltraCombo cmbvehicletype;

	private Label label6;

	internal UltraCombo CmbGatePassType;

	private Label label10;

	internal UltraCombo CmbWeighBridge;

	private TextBox txtDeliveryOrderNo;

	private TextBox txtFactoryWeight;

	internal UltraCombo CmbStatus;

	private TextBox txtBalQty;

	private TextBox txtRecQty;

	private TextBox txtOrderQty;

	private TextBox txtApprovedBy;

	private Label lblApprovedBy;

	private TextBox txtDeliveryOrderWeight;

	private Label label13;

	private TextBox txtDifferenceWeight;

	private Label lblDiffWt;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnWeighBridgeLookUp;

	private Label txtManualNoCaption;

	private Label label19;

	private Label label20;

	private Label label21;

	private Label label24;

	private TextBox txtqty;

	private TextBox txtManualNo;

	private Label label3;

	private Label label11;

	private Label lblFactoryWt;

	private Label label26;

	private Label label28;

	private Label label29;

	private Label label4;

	private Label label2;

	private Label label1;

	private Label label8;

	private Label label23;

	private Label label7;

	private DateTimePicker txtgpdate;

	private TextBox txtgpno;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnshortcut;

	private TextBox txtremarks;

	private Panel panel13;

	private Panel panel12;

	private Label label12;

	private Panel panel11;

	private Label label9;

	internal UltraCombo txtPartyName;

	internal UltraCombo txtVarietyName;

	private TabPage tabPage2;

	private Panel panel7;

	private Label label5;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdhistory;

	private Panel panel5;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNewHistory;

	private Panel panel8;

	private Panel panel10;

	private Panel panel14;

	private GroupBox groupBox3;

	private RadioButton rdapproved;

	private RadioButton rdpqtymodify;

	private RadioButton rdpqtydoc;

	private RadioButton rdpqtyentry;

	private Button btnshow;

	private Label label60;

	private Label label61;

	private Label label62;

	private Label label63;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton toolStripButton1;

	private Label label16;

	internal UltraCombo cmbPartyHistory;

	public GatePassGeneralOutward(UserAccount pUserAccount)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void InwardGatePass_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMargin = new Size(1093, 788);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnPrint.Enabled = formrights.DoHavePrintRights;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			DtHistory.Columns.Add("Id", typeof(int));
			DtHistory.Columns.Add("GpType", typeof(string));
			DtHistory.Columns.Add("GpDate", typeof(DateTime));
			DtHistory.Columns.Add("GpNo", typeof(int));
			DtHistory.Columns.Add("DeliverOrderNo", typeof(string));
			DtHistory.Columns.Add("ManualNo", typeof(string));
			DtHistory.Columns.Add("VehicleType", typeof(string));
			DtHistory.Columns.Add("VehicleNo", typeof(string));
			DtHistory.Columns.Add("PartyName", typeof(string));
			DtHistory.Columns.Add("ItemName", typeof(string));
			DtHistory.Columns.Add("ItemQty", typeof(double));
			DtHistory.Columns.Add("Warehouse", typeof(string));
			DtHistory.Columns.Add("WBStatus", typeof(string));
			DtHistory.Columns.Add("FactoryWeight", typeof(double));
			DtHistory.Columns.Add("EntryUser", typeof(string));
			DtHistory.Columns.Add("EntryDate", typeof(DateTime));
			DtHistory.Columns.Add("ModifyUser", typeof(string));
			DtHistory.Columns.Add("ModifyDate", typeof(DateTime));
			DtHistory.Columns.Add("ApprovedUser", typeof(string));
			DtHistory.Columns.Add("ApprovedDate", typeof(DateTime));
			DtHistory.Columns.Add("NoOfAttachments", typeof(int));
			DtHistory.Columns.Add("Remarks", typeof(string));
			btnupdate.Visible = false;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			CmbGatePassType.Focus();
			ChkBox.Checked = true;
			WarehouseFill();
			vehicleTypefill();
			DocumentNoFill();
			GatePassType();
			WeighBridgeRoute();
			LookUpBind();
			Status();
			FactoryWeight();
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
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			int GpTypeCode = GatePassGeneral.GenerateCode(new GatePassGeneral
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 92,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (GpTypeCode > 0)
			{
				txtgpno.Text = GpTypeCode.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbgptype_Leave_1(object sender, EventArgs e)
	{
		DocumentNoFill();
	}

	private void vehicleTypefill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbvehicletype.Value);
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbvehicletype, "Id", "VehicleDescription", "VehicleDescription", true);
				((UltraGridBase)cmbvehicletype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					cmbvehicletype.Value = Id;
				}
				else
				{
					((Control)(object)cmbvehicletype).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassType()
	{
		try
		{
			int Id = Conversion.ToInt(CmbGatePassType.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Type", typeof(string));
			dt.Rows.Add(1, "General");
			dt.Rows.Add(2, "Store");
			DDL.BindDDL(dt, CmbGatePassType, "Id", "Type", "GatePassType", ZeroIndex: false);
			((UltraGridBase)CmbGatePassType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbGatePassType.Value = Id;
				}
				else
				{
					((Control)(object)CmbGatePassType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbWarehouse.Value);
			DataTable dt = CommonServices.getActiveWareHouse();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbWarehouse, "Id", "WareHouseName", "Godown Name", true);
				((UltraGridBase)CmbWarehouse).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbWarehouse).Rows[0].Activate();
			}
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbWarehouse.Value = Id;
				}
				else
				{
					((Control)(object)CmbWarehouse).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FactoryWeight()
	{
		try
		{
			if (((Control)(object)CmbWeighBridge).Text == "Non-Weighable")
			{
				txtFactoryWeight.Visible = false;
				lblFactoryWt.Visible = false;
				lblDiffWt.Visible = false;
				txtDifferenceWeight.Visible = false;
			}
			else
			{
				txtFactoryWeight.Visible = true;
				lblFactoryWt.Visible = true;
				lblDiffWt.Visible = true;
				txtDifferenceWeight.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LookUpBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = WbPartyandItemsDefine.GetAll(new WbPartyandItemsDefine
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtParty = new DataTable();
			dtParty.Columns.Add("Id", typeof(int));
			dtParty.Columns.Add("PartyName", typeof(string));
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName", typeof(string));
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (Conversion.ToString(dt.Rows[i]["WbType"]) == "PartyName")
				{
					dtParty.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["PartyName"]);
				}
				else
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"]);
				}
			}
			((UltraGridBase)txtPartyName).DataSource = dtParty;
			((UltraDropDownBase)txtPartyName).ValueMember = "Id";
			((UltraDropDownBase)txtPartyName).DisplayMember = "PartyName";
			((UltraGridBase)txtPartyName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)txtPartyName).DisplayLayout.Bands[0].Columns["Id"].Hidden = true;
			((UltraGridBase)cmbPartyHistory).DataSource = dtParty;
			((UltraDropDownBase)cmbPartyHistory).ValueMember = "Id";
			((UltraDropDownBase)cmbPartyHistory).DisplayMember = "PartyName";
			((UltraGridBase)cmbPartyHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)cmbPartyHistory).DisplayLayout.Bands[0].Columns["Id"].Hidden = true;
			((UltraGridBase)txtVarietyName).DataSource = dtItem;
			((UltraDropDownBase)txtVarietyName).ValueMember = "Id";
			((UltraDropDownBase)txtVarietyName).DisplayMember = "ItemName";
			((UltraGridBase)txtVarietyName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)txtVarietyName).DisplayLayout.Bands[0].Columns["Id"].Hidden = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WeighBridgeRoute()
	{
		try
		{
			int Id = Conversion.ToInt(CmbWeighBridge.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Type", typeof(string));
			dt.Rows.Add(1, "Weighable");
			dt.Rows.Add(2, "Non-Weighable");
			DDL.BindDDLNew(dt, CmbWeighBridge, "Id", "Type", "WeighBridge", true);
			((UltraGridBase)CmbWeighBridge).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbWeighBridge.Value = Id;
				}
				else
				{
					((Control)(object)CmbWeighBridge).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Status()
	{
		try
		{
			int Id = Conversion.ToInt(CmbStatus.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Status", typeof(string));
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Accepted");
			dt.Rows.Add(3, "Rejected");
			DDL.BindDDLNew(dt, CmbStatus, "Id", "Status", "Status", true);
			((UltraGridBase)CmbStatus).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbStatus.Value = Id;
				}
				else
				{
					((Control)(object)CmbStatus).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)CmbGatePassType).Text.Trim() == "" || Conversion.ToInt(CmbGatePassType.Value) == 0)
		{
			MessageBox.Show("Please Select CmbGatePassType");
			CmbGatePassType.Focus();
			return false;
		}
		if (txtgpno.Text.Trim() == "" || txtgpno.Text.Trim() == "0")
		{
			MessageBox.Show("Gp No Filed Required");
			txtgpno.Focus();
			return false;
		}
		if (txtvehicleno.Text.Trim() == "")
		{
			MessageBox.Show("Please Select Vehicle No");
			txtvehicleno.Focus();
			return false;
		}
		if (((Control)(object)cmbvehicletype).Text.Trim() == "" || Conversion.ToInt(cmbvehicletype.Value) == 0)
		{
			MessageBox.Show("Please Select Vehicle Type");
			cmbvehicletype.Focus();
			return false;
		}
		if (((Control)(object)CmbGatePassType).Text != "General" && (txtDeliveryOrderNo.Text.Trim() == "" || txtDeliveryOrderNo.Text.Trim() == "0"))
		{
			MessageBox.Show("DeliveryOrder No Field Required");
			txtDeliveryOrderNo.Focus();
			return false;
		}
		if (((Control)(object)CmbStatus).Text.Trim() == string.Empty || Conversion.ToInt(CmbStatus.Value) == 0)
		{
			MessageBox.Show("Please Select Status");
			CmbStatus.Focus();
			return false;
		}
		if (((Control)(object)CmbWeighBridge).Text.Trim() == string.Empty || Conversion.ToInt(CmbWeighBridge.Value) == 0)
		{
			MessageBox.Show("Please Select WeighBridge");
			CmbWeighBridge.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || Conversion.ToString((object)txtqty.Text.Trim()) == "0")
		{
			MessageBox.Show("Qty Field Required");
			txtqty.Focus();
			return false;
		}
		return true;
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(Id);
			GetWeighBridgeWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Expected O, but got Unknown
		try
		{
			GatePassGeneral gatePass = new GatePassGeneral();
			gatePass = GatePassGeneral.GetByID(ID);
			tabControl1.SelectedIndex = 0;
			txtgpdate.Value = gatePass.GpDate;
			txtgpno.Text = gatePass.GpSrNo.ToString();
			Qty = Conversion.ToInt((object)gatePass.NoOfPackages);
			txtqty.Text = Conversion.ToString((object)gatePass.NoOfPackages);
			txtremarks.Text = Conversion.ToString((object)gatePass.OtherRemarks);
			((Control)(object)CmbGatePassType).Text = Conversion.ToString((object)gatePass.GatepassType);
			txtvehicleno.Text = Conversion.ToString((object)gatePass.VehicleNo);
			((Control)(object)cmbvehicletype).Text = Conversion.ToString((object)gatePass.VehicleType);
			((Control)(object)txtVarietyName).Text = Conversion.ToString((object)gatePass.VarietyName);
			((Control)(object)txtPartyName).Text = Conversion.ToString((object)gatePass.OtherSupCust);
			CmbWarehouse.Value = gatePass.WarehouseId;
			((Control)(object)CmbWeighBridge).Text = Conversion.ToString((object)gatePass.Status);
			txtDeliveryOrderNo.Text = Conversion.ToString((object)gatePass.SupplierContractCode);
			txtManualNo.Text = Conversion.ToString((object)gatePass.ManualNo);
			((Control)(object)CmbStatus).Text = Conversion.ToString((object)gatePass.DocAttachment);
			DeliveryOrderId = gatePass.PurchaseOrderId;
			txtFactoryWeight.Text = Conversion.ToString((object)gatePass.FactoryWeight);
			if (gatePass.FactoryWeight > 0.0)
			{
				((Control)(object)CmbWeighBridge).Enabled = false;
			}
			txtDeliveryOrderWeight.Text = Conversion.ToString((object)gatePass.SupplierWeight);
			txtDifferenceWeight.Text = Conversion.ToString((object)gatePass.DifferenceWeight);
			Approve = gatePass.IsApproved;
			txtApprovedBy.Text = gatePass.ApprovedUserName;
			FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			if (((Control)(object)CmbGatePassType).Text == "Store")
			{
				DataTable dt = InvDeliveryOrder.ReadIdByDeliveryOrderId(new InvDeliveryOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 85,
					DocNo = Conversion.ToInt((object)txtDeliveryOrderNo.Text.Trim())
				});
				if (dt.Rows.Count <= 0)
				{
					DeliveryOrderId = 0;
					txtDeliveryOrderNo.Text = string.Empty;
					MessageBox.Show("Record Not Found against this Number");
					return;
				}
				DeliveryOrderId = Conversion.ToInt(dt.Rows[0]["Id"]);
				txtremarks.Text = Conversion.ToString(dt.Rows[0]["LoadingInstructions"]);
				txtDeliveryOrderWeight.Text = Conversion.ToString(dt.Rows[0]["NetWeight"]);
				DifferenceWeight();
				BalanceQty();
			}
			((Control)(object)CmbGatePassType).Enabled = false;
			txtDeliveryOrderNo.Enabled = false;
			btnupdate.Visible = true;
			btnsave.Visible = false;
			UpdateMode = true;
			FactoryWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			GatePassGeneral gatePass = new GatePassGeneral();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				gatePass.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (txtFactoryWeight.Text.Trim() != string.Empty && txtFactoryWeight.Text.Trim() != null && txtFactoryWeight.Text.Trim() != "0")
			{
				gatePass.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
			}
			else
			{
				gatePass.FactoryWeight = 0.0;
			}
			gatePass.DocAttachment = ((Control)(object)CmbStatus).Text.Trim().ToString();
			gatePass.OutDateTimeStamp = DateTime.Now;
			gatePass.InDateTimeStamp = DateTime.Now;
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.BranchesId = UserAccount.BranchesId;
			gatePass.DocumentTypeId = 92;
			gatePass.EntryDate = DateTime.Today;
			gatePass.EntryUser = UserAccount.ID;
			gatePass.GatepassType = ((Control)(object)CmbGatePassType).Text.Trim();
			gatePass.GpDate = txtgpdate.Value;
			gatePass.GpSrNo = int.Parse(txtgpno.Text);
			gatePass.ModifyDate = DateTime.Today;
			gatePass.ModifyUser = UserAccount.ID;
			gatePass.NoOfPackages = int.Parse(txtqty.Text.Trim());
			gatePass.OrganizationId = UserAccount.OrganizationId;
			gatePass.OtherRemarks = txtremarks.Text.Trim();
			gatePass.RefDocumentTypeId = 92;
			gatePass.VehicleNo = txtvehicleno.Text.Trim();
			gatePass.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
			gatePass.VarietyName = ((Control)(object)txtVarietyName).Text.Trim();
			gatePass.ManualNo = txtManualNo.Text.Trim();
			gatePass.OtherSupCust = ((Control)(object)txtPartyName).Text.Trim();
			gatePass.Status = ((Control)(object)CmbWeighBridge).Text.Trim();
			gatePass.SupplierContractCode = txtDeliveryOrderNo.Text.Trim();
			gatePass.PurchaseOrderId = DeliveryOrderId;
			gatePass.WarehouseId = Conversion.ToInt(CmbWarehouse.Value);
			gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gatePass.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			gatePass.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<GatePassGeneral>(gatePass, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = GatePassGeneral.Save(gatePass);
			if (Id > 0)
			{
				MessageBox.Show("Record Update Successfully[" + gatePass.GpSrNo + "]");
				FormHelper.DeleteAllAttachmentsFromDb(AT, Id, 92, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save Successfully[" + gatePass.GpSrNo + "]");
			}
			Reset();
			if (ChkBox.Checked)
			{
				CommonServices.GatePassGeneralInward254(success, 92);
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
			Id = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
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

	private DataTable GetFormHistory(string Status = null)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters gatePassGeneral = new ReportsParameters();
			gatePassGeneral.OrganizationId = UserAccount.OrganizationId;
			gatePassGeneral.CompanyId = UserAccount.CompanyId;
			gatePassGeneral.BranchesId = UserAccount.BranchesId;
			gatePassGeneral.DocumentTypeId = 92;
			gatePassGeneral.Status = Status;
			gatePassGeneral.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (rdpqtydoc.Checked)
			{
				if (FromDateHistory.Checked)
				{
					gatePassGeneral.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					gatePassGeneral.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdpqtyentry.Checked)
			{
				if (FromDateHistory.Checked)
				{
					gatePassGeneral.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					gatePassGeneral.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdpqtymodify.Checked)
			{
				if (FromDateHistory.Checked)
				{
					gatePassGeneral.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					gatePassGeneral.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproved.Checked)
			{
				if (FromDateHistory.Checked)
				{
					gatePassGeneral.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					gatePassGeneral.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			gatePassGeneral.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			gatePassGeneral.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			gatePassGeneral.SupplierCustomerId = Conversion.ToInt(cmbPartyHistory.Value);
			DataTable dt = GatePassGeneral.ReadAll(gatePassGeneral);
			if (dt.Rows.Count > 0 && dt != null)
			{
				DtHistory.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DtHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["gatePassType"], Conversion.ToDateTime(dt.Rows[i]["GpDate"]), dt.Rows[i]["GpSrNo"], dt.Rows[i]["DeliveryOrderNo"], dt.Rows[i]["ManualNo"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["PartyName"], dt.Rows[i]["VarietyName"], dt.Rows[i]["ItemQty"], dt.Rows[i]["WarehouseName"], dt.Rows[i]["Status"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["EntryUser"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUser"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["ApprovedUser"], dt.Rows[i]["PostDate"], dt.Rows[i]["NoOfAttachments"], dt.Rows[i]["OtherRemarks"]);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return DtHistory;
	}

	private void grdfrmfill()
	{
		try
		{
			grd.ClearStructure();
			DataTable dt = GetFormHistory("Open");
			grd.DataSource = dt;
			grd.RetrieveStructure();
			grdfrmsetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmsetting()
	{
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["GpType"].Width = Constants.InventoryConstants.GpType;
			grd.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grd.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grd.RootTable.Columns["DeliverOrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grd.RootTable.Columns["ManualNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grd.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant;
			grd.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grd.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 20;
			grd.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["WBStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grd.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grd.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grd.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant + 20;
			grd.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 30;
			grd.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FactoryWeight"].FormatString = "#,#";
			grd.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,#";
			grd.RootTable.Columns.Add("View");
			grd.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["View"].ButtonText = "View";
			grd.RootTable.Columns["View"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void FromHistoryFill()
	{
		try
		{
			GetFormHistory();
			grdhistory.ClearStructure();
			grdhistory.DataSource = DtHistory;
			grdhistory.RetrieveStructure();
			grdHistorySetting();
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void grdHistorySetting()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistory.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdhistory.RootTable.Columns["GpType"].Width = Constants.InventoryConstants.GpType;
			grdhistory.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["GpNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdhistory.RootTable.Columns["DeliverOrderNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["ManualNo"].Width = Constants.InventoryConstants.ManualBillNo;
			grdhistory.RootTable.Columns["VehicleType"].Width = Constants.InventoryConstants.VehicleTypeConstant;
			grdhistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdhistory.RootTable.Columns["PartyName"].Width = Constants.InventoryConstants.SupplierName + 20;
			grdhistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 20;
			grdhistory.RootTable.Columns["ItemQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdhistory.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdhistory.RootTable.Columns["WBStatus"].Width = Constants.InventoryConstants.StatusConstant;
			grdhistory.RootTable.Columns["FactoryWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["ApprovedUser"].Width = Constants.InventoryConstants.ApprovedByConstant + 20;
			grdhistory.RootTable.Columns["ApprovedDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdhistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants + 30;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grdhistory.RootTable.Columns["FactoryWeight"].AggregateFunction = (AggregateFunction)2;
			grdhistory.RootTable.Columns["FactoryWeight"].TextAlignment = (TextAlignment)3;
			grdhistory.RootTable.Columns["FactoryWeight"].FormatString = "#,#";
			grdhistory.RootTable.Columns["FactoryWeight"].TotalFormatString = "#,#";
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar2_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
				GetWeighBridgeWeight();
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GatePassGeneralInward254(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 92);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetWeighBridgeWeight()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		try
		{
			DataTable lstWb = WbTransactions.GetNetWeightFromWbTransactions(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferenceDocTypeId = 92,
				ReferenceDocNoId = Id
			});
			if (lstWb.Rows.Count > 0)
			{
				double NetWeight = 0.0;
				for (int i = 0; i < lstWb.Rows.Count; i++)
				{
					NetWeight += Convert.ToDouble(lstWb.Rows[i]["NetWbWeight"]);
				}
				txtFactoryWeight.Text = NetWeight.ToString();
				if (Conversion.ToDouble((object)txtFactoryWeight.Text.Trim()) > 0.0)
				{
					((Control)(object)CmbWeighBridge).Enabled = false;
				}
			}
			else
			{
				txtFactoryWeight.Text = "0";
			}
			BalanceQty();
			FactoryWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			ReadById(Id);
			GetWeighBridgeWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 92);
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
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			POId = 0;
			Qty = 0;
			Id = 0;
			DeliveryOrderId = 0;
			UpdateMode = false;
			Approve = false;
			txtApprovedBy.Clear();
			txtBalQty.Text = string.Empty;
			txtDeliveryOrderNo.Text = string.Empty;
			txtFactoryWeight.Text = "0";
			txtDeliveryOrderWeight.Text = "0";
			txtDifferenceWeight.Text = "0";
			((Control)(object)cmbvehicletype).Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtManualNo.Text = string.Empty;
			txtqty.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbWarehouse).Text = string.Empty;
			table.Clear();
			grd.DataSource = table;
			grdfrmfill();
			LookUpBind();
			((Control)(object)txtPartyName).Text = string.Empty;
			((Control)(object)txtVarietyName).Text = string.Empty;
			btnsave.Visible = true;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			((Control)(object)CmbWeighBridge).Enabled = true;
			((Control)(object)CmbGatePassType).Enabled = true;
			txtDeliveryOrderNo.Enabled = true;
			txtgpdate.Focus();
			DocumentNoFill();
			AT = new Attachment();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgpdate_Leave(object sender, EventArgs e)
	{
		grdfrmfill();
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
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
			throw;
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "View")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					CommonServices.GatePassGeneralInward254(Conversion.ToInt(item.Cells["Id"].Value), 92);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InwardGatePass_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
			if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				for (int i = 0; i < checkedRows.Length; i++)
				{
					int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
				}
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && btnsave.Visible && btnsave.Enabled)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					CmbGatePassType.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdhistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U && tabControl1.SelectedIndex == 0)
			{
				btnupdate_Click(null, null);
			}
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P && tabControl1.SelectedIndex == 0)
			{
				btnPrint_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbGatePassType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbGatePassType.Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControl1.SelectedIndex == 1)
			{
				Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on GatePass Type");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Detail Box");
			dt.Rows.Add("Ctrl+ArrowRight", "To change Grid Tab Control,");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcut_Click(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 0)
		{
			grdfrmfill();
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load_1(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			if (!Approve)
			{
				throw new Exception("GatePass Not Approve");
			}
			CommonServices.GatePassGeneralInward254(Id, 92);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtgpno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		vehicleTypefill();
		WarehouseFill();
		LookUpBind();
	}

	private void BalanceQty()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			DataTable dt = InvDeliveryOrder.TotalReceivedQtyByDeliveryOrderStockTransfer(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				OrderId = DeliveryOrderId
			});
			if (dt.Rows.Count > 0)
			{
				txtBalQty.Text = Conversion.ToString(dt.Rows[0]["BalQty"]);
			}
			else
			{
				txtBalQty.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void textBox1_Leave(object sender, EventArgs e)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Expected O, but got Unknown
		try
		{
			if (!btnsave.Enabled || !btnsave.Visible)
			{
				return;
			}
			if (((Control)(object)CmbGatePassType).Text == "Store")
			{
				DataTable dt = InvDeliveryOrder.ReadIdByDeliveryOrderId(new InvDeliveryOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 85,
					DocNo = Conversion.ToInt((object)txtDeliveryOrderNo.Text.Trim())
				});
				if (dt.Rows.Count > 0)
				{
					DeliveryOrderId = Conversion.ToInt(dt.Rows[0]["Id"]);
					txtremarks.Text = Conversion.ToString(dt.Rows[0]["LoadingInstructions"]);
					txtDeliveryOrderWeight.Text = Conversion.ToString(dt.Rows[0]["NetWeight"]);
					((Control)(object)cmbvehicletype).Text = Conversion.ToString(dt.Rows[0]["VehicleType"]);
					txtvehicleno.Text = Conversion.ToString(dt.Rows[0]["VehicleNo"]);
					DifferenceWeight();
					BalanceQty();
				}
				else
				{
					DeliveryOrderId = 0;
					MessageBox.Show("Record Not Found against this Number");
					Reset();
				}
			}
			else
			{
				DeliveryOrderId = 0;
				txtDeliveryOrderNo.Text = "0";
				txtDeliveryOrderWeight.Text = "0";
				((Control)(object)txtVarietyName).Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DifferenceWeight()
	{
		try
		{
			double FactoryWeight = 0.0;
			if (txtDeliveryOrderNo.Text != string.Empty && txtFactoryWeight.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtDeliveryOrderWeight.Text.Trim());
				FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text.Trim());
				txtDifferenceWeight.Text = (num - FactoryWeight).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnWeighBridgeLookUp_Click(object sender, EventArgs e)
	{
		new WeighBridgeGeneralLookups(UserAccount).Show();
	}

	private void CmbWeighBridge_Leave(object sender, EventArgs e)
	{
		FactoryWeight();
	}

	private void GatePassGeneralOutward_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtDeliveryOrderWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFactoryWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDifferenceWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBalQty_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				Id = Conversion.ToInt((object)grd.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
				GetWeighBridgeWeight();
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Print")
				{
					CommonServices.GatePassGeneralInward254(Conversion.ToInt(item.Cells["Id"].Value), 92);
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grdhistory_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit")
				{
					Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(Id);
					GetWeighBridgeWeight();
				}
				if (grdhistory.CurrentColumn.Key == "Print")
				{
					CommonServices.GatePassGeneralInward254(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value), 92);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdhistory_DoubleClick(null, null);
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
			FromHistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Clear();
			txtToDocNoHistory.Clear();
			grdhistory.DataSource = null;
			rdpqtydoc.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			LookUpBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0291: Expected O, but got Unknown
		//IL_0291: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Expected O, but got Unknown
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0334: Expected O, but got Unknown
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Expected O, but got Unknown
		//IL_048a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Expected O, but got Unknown
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_05df: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e9: Expected O, but got Unknown
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca8: Expected O, but got Unknown
		//IL_5094: Unknown result type (might be due to invalid IL or missing references)
		//IL_509e: Expected O, but got Unknown
		//IL_50ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_50b5: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
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
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Definition.GatePassGeneralOutward));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label12 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.txtPartyName = new UltraCombo();
		this.txtVarietyName = new UltraCombo();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtgpno = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.lblFactoryWt = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label29 = new System.Windows.Forms.Label();
		this.txtManualNo = new System.Windows.Forms.TextBox();
		this.txtManualNoCaption = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtDifferenceWeight = new System.Windows.Forms.TextBox();
		this.lblDiffWt = new System.Windows.Forms.Label();
		this.txtDeliveryOrderWeight = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtApprovedBy = new System.Windows.Forms.TextBox();
		this.lblApprovedBy = new System.Windows.Forms.Label();
		this.txtBalQty = new System.Windows.Forms.TextBox();
		this.txtRecQty = new System.Windows.Forms.TextBox();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.CmbStatus = new UltraCombo();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.txtDeliveryOrderNo = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbWeighBridge = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbGatePassType = new UltraCombo();
		this.cmbvehicletype = new UltraCombo();
		this.CmbWarehouse = new UltraCombo();
		this.label17 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnWeighBridgeLookUp = new System.Windows.Forms.ToolStripButton();
		this.btnshortcut = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbPartyHistory = new UltraCombo();
		this.rdapproved = new System.Windows.Forms.RadioButton();
		this.rdpqtymodify = new System.Windows.Forms.RadioButton();
		this.rdpqtydoc = new System.Windows.Forms.RadioButton();
		this.rdpqtyentry = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label60 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel9.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel12.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.txtPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtVarietyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridge).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel11.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel10.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(855, 510);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(855, 510);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel9);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(847, 481);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.panel12);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 268);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(843, 211);
		this.panel9.TabIndex = 3;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 25);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(843, 186);
		this.panel13.TabIndex = 48;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.FrozenColumns = 1;
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(843, 186);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Controls.Add(this.label12);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(843, 25);
		this.panel12.TabIndex = 47;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1130, -2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(60, 31);
		this.ctrlGrdBar1.TabIndex = 59;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.Teal;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label12.Location = new System.Drawing.Point(4, 3);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(0, 19);
		this.label12.TabIndex = 0;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.txtPartyName);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.txtVarietyName);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add(this.label23);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtgpdate);
		this.panel4.Controls.Add(this.txtgpno);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.lblFactoryWt);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add(this.label28);
		this.panel4.Controls.Add(this.label29);
		this.panel4.Controls.Add(this.txtManualNo);
		this.panel4.Controls.Add(this.txtManualNoCaption);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add(this.txtDifferenceWeight);
		this.panel4.Controls.Add(this.lblDiffWt);
		this.panel4.Controls.Add(this.txtDeliveryOrderWeight);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.txtApprovedBy);
		this.panel4.Controls.Add(this.lblApprovedBy);
		this.panel4.Controls.Add(this.txtBalQty);
		this.panel4.Controls.Add(this.txtRecQty);
		this.panel4.Controls.Add(this.txtOrderQty);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel4.Controls.Add(this.txtFactoryWeight);
		this.panel4.Controls.Add(this.txtDeliveryOrderNo);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWeighBridge);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGatePassType);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtqty);
		this.panel4.Controls.Add(this.txtvehicleno);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 56);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(843, 212);
		this.panel4.TabIndex = 1;
		this.txtPartyName.AllowNull = (DefaultableBoolean)1;
		this.txtPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.txtPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.txtPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance78).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.txtPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtPartyName).Location = new System.Drawing.Point(116, 153);
		((System.Windows.Forms.Control)(object)this.txtPartyName).Name = "txtPartyName";
		((System.Windows.Forms.Control)(object)this.txtPartyName).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.txtPartyName).TabIndex = 8;
		this.txtVarietyName.AllowNull = (DefaultableBoolean)1;
		this.txtVarietyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtVarietyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtVarietyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtVarietyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance86;
		((SpecialBoxBase)((UltraGridBase)this.txtVarietyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtVarietyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtVarietyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarietyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtVarietyName).Location = new System.Drawing.Point(513, 39);
		((System.Windows.Forms.Control)(object)this.txtVarietyName).Name = "txtVarietyName";
		((System.Windows.Forms.Control)(object)this.txtVarietyName).Size = new System.Drawing.Size(320, 26);
		((System.Windows.Forms.Control)(object)this.txtVarietyName).TabIndex = 11;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(116, 182);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(717, 23);
		this.txtremarks.TabIndex = 9;
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.White;
		this.label23.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label23.Location = new System.Drawing.Point(6, 44);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(85, 15);
		this.label23.TabIndex = 117;
		this.label23.Text = "Gp Date && No";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(95, 41);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(15, 20);
		this.label7.TabIndex = 116;
		this.label7.Text = "*";
		this.txtgpdate.CustomFormat = "dd-MMM-yyyy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(116, 40);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(134, 23);
		this.txtgpdate.TabIndex = 1;
		this.txtgpno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgpno.Enabled = false;
		this.txtgpno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpno.Location = new System.Drawing.Point(256, 39);
		this.txtgpno.Name = "txtgpno";
		this.txtgpno.ReadOnly = true;
		this.txtgpno.Size = new System.Drawing.Size(129, 23);
		this.txtgpno.TabIndex = 2;
		this.txtgpno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(253, 73);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(68, 15);
		this.label8.TabIndex = 113;
		this.label8.Text = "DO Bal Qty";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(492, 129);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(15, 20);
		this.label4.TabIndex = 112;
		this.label4.Text = "*";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(492, 102);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(15, 20);
		this.label2.TabIndex = 111;
		this.label2.Text = "*";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(492, 69);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(15, 20);
		this.label1.TabIndex = 110;
		this.label1.Text = "*";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(408, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(70, 15);
		this.label3.TabIndex = 109;
		this.label3.Text = "Warehouse";
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(6, 73);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(93, 15);
		this.label11.TabIndex = 108;
		this.label11.Text = "Delivery Order ";
		this.lblFactoryWt.AutoSize = true;
		this.lblFactoryWt.BackColor = System.Drawing.Color.White;
		this.lblFactoryWt.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.lblFactoryWt.Location = new System.Drawing.Point(642, 102);
		this.lblFactoryWt.Name = "lblFactoryWt";
		this.lblFactoryWt.Size = new System.Drawing.Size(89, 15);
		this.lblFactoryWt.TabIndex = 107;
		this.lblFactoryWt.Text = "FactoryWeight";
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.White;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(408, 129);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(61, 15);
		this.label26.TabIndex = 106;
		this.label26.Text = "Gp Status";
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.White;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(408, 73);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(57, 15);
		this.label28.TabIndex = 105;
		this.label28.Text = "Item Qty";
		this.label29.AutoSize = true;
		this.label29.BackColor = System.Drawing.Color.White;
		this.label29.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(408, 44);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(70, 15);
		this.label29.TabIndex = 103;
		this.label29.Text = "Item Name";
		this.txtManualNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtManualNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtManualNo.Location = new System.Drawing.Point(116, 97);
		this.txtManualNo.Name = "txtManualNo";
		this.txtManualNo.Size = new System.Drawing.Size(269, 23);
		this.txtManualNo.TabIndex = 5;
		this.txtManualNoCaption.AutoSize = true;
		this.txtManualNoCaption.BackColor = System.Drawing.Color.White;
		this.txtManualNoCaption.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.txtManualNoCaption.Location = new System.Drawing.Point(6, 102);
		this.txtManualNoCaption.Name = "txtManualNoCaption";
		this.txtManualNoCaption.Size = new System.Drawing.Size(66, 15);
		this.txtManualNoCaption.TabIndex = 99;
		this.txtManualNoCaption.Text = "Manual No";
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(6, 15);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(89, 15);
		this.label19.TabIndex = 98;
		this.label19.Text = "Gate Pass Type";
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.White;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(6, 185);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(56, 15);
		this.label20.TabIndex = 96;
		this.label20.Text = "Remarks";
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.White;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(6, 160);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(72, 15);
		this.label21.TabIndex = 95;
		this.label21.Text = "Party Name";
		this.label24.AutoSize = true;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label24.Location = new System.Drawing.Point(6, 129);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(108, 15);
		this.label24.TabIndex = 94;
		this.label24.Text = "Vehicle Type && No";
		this.txtDifferenceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDifferenceWeight.Enabled = false;
		this.txtDifferenceWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDifferenceWeight.Location = new System.Drawing.Point(733, 124);
		this.txtDifferenceWeight.Name = "txtDifferenceWeight";
		this.txtDifferenceWeight.ReadOnly = true;
		this.txtDifferenceWeight.Size = new System.Drawing.Size(100, 23);
		this.txtDifferenceWeight.TabIndex = 17;
		this.txtDifferenceWeight.TabStop = false;
		this.txtDifferenceWeight.Text = "0";
		this.txtDifferenceWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDifferenceWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDifferenceWeight_KeyPress);
		this.lblDiffWt.AutoSize = true;
		this.lblDiffWt.BackColor = System.Drawing.Color.White;
		this.lblDiffWt.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.lblDiffWt.Location = new System.Drawing.Point(642, 129);
		this.lblDiffWt.Name = "lblDiffWt";
		this.lblDiffWt.Size = new System.Drawing.Size(73, 15);
		this.lblDiffWt.TabIndex = 93;
		this.lblDiffWt.Text = "Diff Weight";
		this.txtDeliveryOrderWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryOrderWeight.Enabled = false;
		this.txtDeliveryOrderWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryOrderWeight.Location = new System.Drawing.Point(733, 68);
		this.txtDeliveryOrderWeight.Name = "txtDeliveryOrderWeight";
		this.txtDeliveryOrderWeight.ReadOnly = true;
		this.txtDeliveryOrderWeight.Size = new System.Drawing.Size(100, 23);
		this.txtDeliveryOrderWeight.TabIndex = 13;
		this.txtDeliveryOrderWeight.TabStop = false;
		this.txtDeliveryOrderWeight.Text = "0";
		this.txtDeliveryOrderWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDeliveryOrderWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDeliveryOrderWeight_KeyPress);
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(642, 73);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(67, 15);
		this.label13.TabIndex = 91;
		this.label13.Text = "Do Weight";
		this.txtApprovedBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtApprovedBy.Enabled = false;
		this.txtApprovedBy.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtApprovedBy.Location = new System.Drawing.Point(513, 155);
		this.txtApprovedBy.Name = "txtApprovedBy";
		this.txtApprovedBy.ReadOnly = true;
		this.txtApprovedBy.Size = new System.Drawing.Size(320, 23);
		this.txtApprovedBy.TabIndex = 18;
		this.lblApprovedBy.AutoSize = true;
		this.lblApprovedBy.BackColor = System.Drawing.Color.White;
		this.lblApprovedBy.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.lblApprovedBy.Location = new System.Drawing.Point(408, 160);
		this.lblApprovedBy.Name = "lblApprovedBy";
		this.lblApprovedBy.Size = new System.Drawing.Size(76, 15);
		this.lblApprovedBy.TabIndex = 88;
		this.lblApprovedBy.Text = "ApprovedBy";
		this.txtBalQty.Enabled = false;
		this.txtBalQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBalQty.Location = new System.Drawing.Point(324, 70);
		this.txtBalQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtBalQty.Name = "txtBalQty";
		this.txtBalQty.ReadOnly = true;
		this.txtBalQty.Size = new System.Drawing.Size(61, 23);
		this.txtBalQty.TabIndex = 4;
		this.txtBalQty.TabStop = false;
		this.txtBalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtBalQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtBalQty_KeyPress);
		this.txtRecQty.Location = new System.Drawing.Point(1154, 39);
		this.txtRecQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtRecQty.Name = "txtRecQty";
		this.txtRecQty.Size = new System.Drawing.Size(44, 23);
		this.txtRecQty.TabIndex = 86;
		this.txtRecQty.Visible = false;
		this.txtOrderQty.Location = new System.Drawing.Point(1154, 16);
		this.txtOrderQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.Size = new System.Drawing.Size(44, 23);
		this.txtOrderQty.TabIndex = 85;
		this.txtOrderQty.Visible = false;
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(513, 123);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(126, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 16;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Enabled = false;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFactoryWeight.Location = new System.Drawing.Point(733, 97);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.ReadOnly = true;
		this.txtFactoryWeight.Size = new System.Drawing.Size(100, 23);
		this.txtFactoryWeight.TabIndex = 15;
		this.txtFactoryWeight.TabStop = false;
		this.txtFactoryWeight.Text = "0";
		this.txtFactoryWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFactoryWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFactoryWeight_KeyPress);
		this.txtDeliveryOrderNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliveryOrderNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliveryOrderNo.Location = new System.Drawing.Point(116, 68);
		this.txtDeliveryOrderNo.Name = "txtDeliveryOrderNo";
		this.txtDeliveryOrderNo.Size = new System.Drawing.Size(134, 23);
		this.txtDeliveryOrderNo.TabIndex = 3;
		this.txtDeliveryOrderNo.Text = "0";
		this.txtDeliveryOrderNo.Leave += new System.EventHandler(textBox1_Leave);
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(408, 102);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(65, 15);
		this.label10.TabIndex = 78;
		this.label10.Text = "WB Status";
		this.CmbWeighBridge.AllowNull = (DefaultableBoolean)1;
		this.CmbWeighBridge.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWeighBridge.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWeighBridge.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWeighBridge).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWeighBridge.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Location = new System.Drawing.Point(513, 96);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Name = "CmbWeighBridge";
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Size = new System.Drawing.Size(126, 26);
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.CmbWeighBridge).Leave += new System.EventHandler(CmbWeighBridge_Leave);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.Red;
		this.label6.Location = new System.Drawing.Point(98, 12);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(16, 16);
		this.label6.TabIndex = 71;
		this.label6.Text = "*";
		this.CmbGatePassType.AllowNull = (DefaultableBoolean)1;
		this.CmbGatePassType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGatePassType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGatePassType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGatePassType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGatePassType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Location = new System.Drawing.Point(116, 9);
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Name = "CmbGatePassType";
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.CmbGatePassType).TabIndex = 0;
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(116, 123);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(134, 26);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 6;
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance73;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance79).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance82).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(513, 9);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(320, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 10;
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(99, 126);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(16, 16);
		this.label17.TabIndex = 63;
		this.label17.Text = "*";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(513, 68);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(126, 23);
		this.txtqty.TabIndex = 12;
		this.txtqty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(256, 124);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(129, 23);
		this.txtvehicleno.TabIndex = 7;
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(843, 25);
		this.panel3.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(843, 25);
		this.panel6.TabIndex = 46;
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label9);
		this.panel11.Controls.Add(this.ChkBox);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(843, 25);
		this.panel11.TabIndex = 47;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label9.Location = new System.Drawing.Point(4, 3);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(234, 19);
		this.label9.TabIndex = 0;
		this.label9.Text = "Outward General Gate Pass";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Checked = true;
		this.ChkBox.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBox.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(730, 2);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(108, 18);
		this.ChkBox.TabIndex = 13;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(4, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(234, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Outward General Gate Pass";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(843, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnupdate, this.btnsave, this.btnPrint, this.btnAttachment, this.btnWeighBridgeLookUp, this.btnshortcut });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(843, 27);
		this.toolStrip1.TabIndex = 58;
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
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "254-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnWeighBridgeLookUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnWeighBridgeLookUp.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnWeighBridgeLookUp.Name = "btnWeighBridgeLookUp";
		this.btnWeighBridgeLookUp.Size = new System.Drawing.Size(103, 24);
		this.btnWeighBridgeLookUp.Text = "Lookup Define";
		this.btnWeighBridgeLookUp.Click += new System.EventHandler(btnWeighBridgeLookUp_Click);
		this.btnshortcut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcut.Name = "btnshortcut";
		this.btnshortcut.Size = new System.Drawing.Size(126, 24);
		this.btnshortcut.Text = "ShortCut Keys";
		this.btnshortcut.Click += new System.EventHandler(btnshortcut_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(847, 481);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel14);
		this.panel8.Controls.Add(this.panel10);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 57);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(843, 422);
		this.panel8.TabIndex = 47;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 73);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(843, 349);
		this.panel14.TabIndex = 47;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.FrozenColumns = 2;
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(843, 349);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdhistory_KeyDown);
		this.panel10.Controls.Add(this.groupBox3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(843, 73);
		this.panel10.TabIndex = 46;
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPartyHistory);
		this.groupBox3.Controls.Add(this.rdapproved);
		this.groupBox3.Controls.Add(this.rdpqtymodify);
		this.groupBox3.Controls.Add(this.rdpqtydoc);
		this.groupBox3.Controls.Add(this.rdpqtyentry);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.label61);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(6, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(967, 69);
		this.groupBox3.TabIndex = 25;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(478, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(69, 15);
		this.label16.TabIndex = 146;
		this.label16.Text = "Party Name";
		this.cmbPartyHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbPartyHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPartyHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPartyHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPartyHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPartyHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Location = new System.Drawing.Point(481, 36);
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Name = "cmbPartyHistory";
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbPartyHistory).TabIndex = 145;
		this.rdapproved.AutoSize = true;
		this.rdapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproved.Location = new System.Drawing.Point(858, 45);
		this.rdapproved.Name = "rdapproved";
		this.rdapproved.Size = new System.Drawing.Size(105, 19);
		this.rdapproved.TabIndex = 142;
		this.rdapproved.Text = "Approved Date";
		this.rdapproved.UseVisualStyleBackColor = true;
		this.rdpqtymodify.AutoSize = true;
		this.rdpqtymodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtymodify.Location = new System.Drawing.Point(858, 22);
		this.rdpqtymodify.Name = "rdpqtymodify";
		this.rdpqtymodify.Size = new System.Drawing.Size(91, 19);
		this.rdpqtymodify.TabIndex = 8;
		this.rdpqtymodify.Text = "Modify Date";
		this.rdpqtymodify.UseVisualStyleBackColor = true;
		this.rdpqtydoc.AutoSize = true;
		this.rdpqtydoc.Checked = true;
		this.rdpqtydoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtydoc.Location = new System.Drawing.Point(774, 22);
		this.rdpqtydoc.Name = "rdpqtydoc";
		this.rdpqtydoc.Size = new System.Drawing.Size(75, 19);
		this.rdpqtydoc.TabIndex = 6;
		this.rdpqtydoc.TabStop = true;
		this.rdpqtydoc.Text = "Doc Date";
		this.rdpqtydoc.UseVisualStyleBackColor = true;
		this.rdpqtyentry.AutoSize = true;
		this.rdpqtyentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdpqtyentry.Location = new System.Drawing.Point(775, 45);
		this.rdpqtyentry.Name = "rdpqtyentry";
		this.rdpqtyentry.Size = new System.Drawing.Size(80, 19);
		this.rdpqtyentry.TabIndex = 7;
		this.rdpqtyentry.Text = "Entry Date";
		this.rdpqtyentry.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(715, 36);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(53, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(376, 19);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(64, 15);
		this.label60.TabIndex = 141;
		this.label60.Text = "To Doc No";
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(274, 19);
		this.label61.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(79, 15);
		this.label61.TabIndex = 140;
		this.label61.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(139, 19);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(5, 19);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(376, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(274, 38);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(100, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(139, 38);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(5, 38);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(131, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label5);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 26);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(843, 31);
		this.panel7.TabIndex = 46;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 19);
		this.label5.TabIndex = 61;
		this.label5.Text = "History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(795, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(39, 21);
		this.ctrlGrdBar2.TabIndex = 60;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load_1);
		this.panel5.Controls.Add(this.toolStrip2);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(2, 2);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(843, 24);
		this.panel5.TabIndex = 0;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNewHistory, this.toolStripButton1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(843, 25);
		this.toolStrip2.TabIndex = 2;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(64, 22);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(76, 22);
		this.toolStripButton1.Text = "Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(855, 510);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GatePassGeneralOutward";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GatePassGeneralOutward_FormClosing);
		base.Load += new System.EventHandler(InwardGatePass_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InwardGatePass_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.txtPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtVarietyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWeighBridge).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGatePassType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel10.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbPartyHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
