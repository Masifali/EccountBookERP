// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.WinApp.Account_Definition;
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
namespace Architecture.WinApp.Paddy_Purchase
{

public class frmGatePassInwardForGhallaMandi : Form
{
	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public int POId;

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private bool UpdateMode;

	private int updateDetailIndex;

	private UserAccount UserAccount = new UserAccount();

	private int Id;

	private DataTable dtDetail = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtcust = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private Rightsobjects formright = new Rightsobjects();

	private byte[] GpQrCode;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel3;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrint;

	private ToolStripButton btnAttachment;

	private Panel panel6;

	private Label label15;

	private Panel panel4;

	private DateTimePicker txtouttime;

	private DateTimePicker txtintime;

	private DateTimePicker txtgpdate;

	private TextBox txtbiltyno;

	private TextBox txtremarks;

	private TextBox txtfreight;

	private TextBox txtvehicleno;

	private TextBox txtgpno;

	private Label label13;

	private Label label14;

	private Label label10;

	private Label label12;

	private Label label21;

	private Label label1;

	private Label label9;

	private Label label20;

	private Label label8;

	private Label label2;

	private Label label16;

	private Panel panel5;

	private Panel panel7;

	private Panel panel9;

	internal GridEX grd;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label17;

	private TextBox txtqty;

	private Label label22;

	private Label label25;

	private Label label31;

	internal UltraCombo CmbVariety;

	internal UltraCombo CmbGhallaMandi;

	private Label label32;

	private ToolStripButton btnRefresh;

	private TextBox txtBalQty;

	private TextBox txtRecQty;

	private TextBox txtOrderQty;

	private CheckBox ChkBox;

	private ToolStripButton toolStripButton4;

	internal UltraCombo CmbLoadingContractor;

	internal UltraCombo cmbvehicletype;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnDelete;

	internal UltraCombo cmbWeighBridge;

	private Label label18;

	private ToolStripButton btndefvehicle;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private CtrlGrdBar ctrlGrdBar3;

	private TextBox txtWeight;

	private TextBox txtPackUnit;

	private Label label42;

	private Label label38;

	private PictureBox pictureBox1;

	private TextBox txtDriverName;

	private Label label48;

	private TextBox txtDriverMobile;

	private Label label50;

	internal UltraCombo CmbCarriageContractor;

	private Label label46;

	private TabPage tabPage5;

	private Panel panel10;

	internal GridEX grdGpDetail;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button Add;

	private Label label51;

	private Label label40;

	private Label label37;

	private Label label7;

	private Label label52;

	private Label label54;

	private Label label53;

	internal UltraCombo CmbPackingType;

	private Label label3;

	private Label label11;

	private TextBox txtWorkingReportNo;

	private Label label6;

	private Label label5;

	private Label label4;

	private Panel panel11;

	internal GridEX grdhistorydetail;

	private Panel panel8;

	internal GridEX grdhistory;

	private ToolStripLabel toolStripLabel1;

	private ToolStripSeparator toolStripSeparator1;

	public frmGatePassInwardForGhallaMandi(UserAccount pUserAccount)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void gpnofill()
	{
		try
		{
			int Code = CommonServices.GatePassInwardGenerateCode(51);
			if (Code > 0)
			{
				txtgpno.Text = Conversion.ToString((object)Code);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("PackingType");
			dt.Rows.Add(1, "JUTE BAGS");
			dt.Rows.Add(2, "PP BAGS");
			dt.Rows.Add(5, "OPEN BULK");
			DDL.BindDDL(dt, CmbPackingType, "Id", "PackingType", "PackingType", ZeroIndex: false);
			((UltraGridBase)CmbPackingType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void vehicleTypefill()
	{
		try
		{
			DataTable dt = VehicleType.GetAll();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbvehicletype, "Id", "VehicleDescription", "Vehicle Name", ZeroIndex: false);
				((UltraGridBase)cmbvehicletype).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GhallaMandiBind()
	{
		try
		{
			DataTable dt = CommonServices.GetGhallaMandi();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbGhallaMandi, "Id", "GhallaMandi", "Ghalla_Mandi", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingContractorBind()
	{
		try
		{
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("6");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbLoadingContractor, "Id", "CompanyName", "Loading Contractor", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CarriageContractorBind()
	{
		try
		{
			DataTable dt = CommonServices.GetSupplierustomerByCustomerGroupId("17");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbCarriageContractor, "Id", "CompanyName", "Carriage Contractor", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemNameFill()
	{
		try
		{
			DataTable dtItem = CommonServices.ReadAllItems();
			if (dtItem.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("ItemName");
			for (int i = 0; i < dtItem.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtItem.Rows[i]["InventoryParentCategoriesId"]) == 1)
				{
					dt.Rows.Add(dtItem.Rows[i]["Id"], dtItem.Rows[i]["ItemName"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbVariety, "Id", "ItemName", "Item Name", ZeroIndex: false);
				((UltraGridBase)CmbVariety).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbWeighBridgeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Auto");
			dt.Rows.Add(2, "Mannual");
			DDL.BindDDL(dt, cmbWeighBridge, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)cmbWeighBridge).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (txtgpno.Text.Trim() == string.Empty || txtgpno.Text.Trim() == "0" || txtgpno.Text.Trim() == null)
		{
			MessageBox.Show("GatePassNo Field Required");
			txtgpno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbGhallaMandi).ActiveRow == null)
		{
			MessageBox.Show("Ghalla_Mandi Field Required");
			CmbGhallaMandi.Focus();
			return false;
		}
		if (((UltraGridBase)CmbLoadingContractor).ActiveRow == null)
		{
			MessageBox.Show("Loading Contractor Field Required");
			CmbLoadingContractor.Focus();
			return false;
		}
		if (((UltraGridBase)CmbCarriageContractor).ActiveRow == null)
		{
			MessageBox.Show("Carriage Contractor Field Required");
			CmbCarriageContractor.Focus();
			return false;
		}
		if (txtvehicleno.Text.Trim() == string.Empty)
		{
			MessageBox.Show("VehicleNo Field Required");
			txtvehicleno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbvehicletype).ActiveRow == null)
		{
			MessageBox.Show("VehicleType Field Required");
			cmbvehicletype.Focus();
			return false;
		}
		if (txtWorkingReportNo.Text.Trim() == string.Empty || txtWorkingReportNo.Text.Trim() == "0" || txtWorkingReportNo.Text.Trim() == null)
		{
			MessageBox.Show("Working Report No Field Required");
			txtWorkingReportNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWeighBridge).ActiveRow == null)
		{
			MessageBox.Show("WeighBridge Status Field Required");
			cmbWeighBridge.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbVariety).ActiveRow == null)
		{
			MessageBox.Show("ItemName Field is Required");
			CmbVariety.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtPackUnit.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("PackUnit Field is Required");
			txtPackUnit.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtqty.Text.Trim()) <= 0)
		{
			MessageBox.Show("ItemQty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtWeight.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Weight Field is Required");
			txtWeight.Focus();
			return false;
		}
		return true;
	}

	public void getUpdate(int ID)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		try
		{
			btnupdate.Visible = true;
			btnsave.Visible = false;
			UpdateMode = true;
			GatePassInward gatePass = new GatePassInward();
			Id = Conversion.ToInt((object)ID);
			gatePass = GatePassInward.GetByID(Id);
			if (gatePass != null)
			{
				tabControl1.SelectedIndex = 0;
				tabControl2.SelectedIndex = 0;
				txtbiltyno.Text = gatePass.BiltyNo;
				CmbLoadingContractor.Value = gatePass.LoadingContractorId;
				CmbGhallaMandi.Value = gatePass.GhallaMandiId;
				CmbCarriageContractor.Value = gatePass.CarriageContractorId;
				txtWorkingReportNo.Text = gatePass.WorkingReportNo.ToString();
				txtfreight.Text = gatePass.Freight.ToString();
				((Control)(object)cmbWeighBridge).Text = Conversion.ToString((object)gatePass.DocAttachment);
				txtgpdate.Value = gatePass.GpDate;
				txtgpno.Text = Conversion.ToString((object)gatePass.GpSrNo);
				txtremarks.Text = gatePass.OtherRemarks;
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)gatePass.InDateTimeStamp)))
				{
					txtintime.Value = Conversion.ToDateTime((object)gatePass.InDateTimeStamp);
				}
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)gatePass.OutDateTimeStamp)))
				{
					txtouttime.Value = Conversion.ToDateTime((object)gatePass.OutDateTimeStamp);
				}
				else
				{
					txtouttime.Value = DateTime.Now;
				}
				GpQrCode = Conversion.ToByteArray((object)gatePass.GpQRCode);
				txtvehicleno.Text = gatePass.VehicleNo;
				((Control)(object)cmbvehicletype).Text = gatePass.VehicleType;
				txtDriverName.Text = Conversion.ToString((object)gatePass.DriverName);
				txtDriverMobile.Text = Conversion.ToString((object)gatePass.DriverMobileNo);
				dtDetail.Rows.Clear();
				for (int i = 0; i < gatePass.gatePassInwardDetails.Count; i++)
				{
					dtDetail.Rows.Add(gatePass.gatePassInwardDetails[i].ItemId, gatePass.gatePassInwardDetails[i].ItemName, gatePass.gatePassInwardDetails[i].ItemQty, gatePass.gatePassInwardDetails[i].PackingTypeId, gatePass.gatePassInwardDetails[i].PackingType, gatePass.gatePassInwardDetails[i].PackUnit, gatePass.gatePassInwardDetails[i].Weight);
				}
				grdGpDetail.DataSource = dtDetail;
				grdGpDetail.RetrieveStructure();
				grdGpDetailSetting();
				List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
				AT = new Attachment();
				{
					foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
					{
						TempAttachments TA = new TempAttachments();
						TA.Id = 0;
						TA.Path = Attachment.Attachment;
						AT.lst.Add(TA);
					}
					return;
				}
			}
			Reset();
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		try
		{
			if (!formvalidation())
			{
				return;
			}
			GatePassInward obj = new GatePassInward();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.GpDate = txtgpdate.Value;
			obj.GpSrNo = Conversion.ToInt((object)txtgpno.Text.Trim());
			obj.OtherSupCust = "Market Purchase";
			obj.GatepassType = "Paddy";
			obj.DocumentTypeId = 51;
			obj.RefDocumentTypeId = 111;
			obj.BiltyNo = txtbiltyno.Text.Trim();
			obj.Freight = Conversion.ToDouble((object)txtfreight.Text);
			obj.NetPaid = obj.Freight;
			obj.InDateTimeStamp = DateTime.Now;
			obj.OutDateTimeStamp = DateTime.Now;
			obj.OtherRemarks = txtremarks.Text.Trim();
			obj.VehicleNo = txtvehicleno.Text.Trim();
			obj.VehicleType = ((Control)(object)cmbvehicletype).Text.Trim();
			obj.LoadingContractorId = Conversion.ToInt(CmbLoadingContractor.Value);
			obj.GhallaMandiId = Conversion.ToInt(CmbGhallaMandi.Value);
			obj.CarriageContractorId = Conversion.ToInt(CmbCarriageContractor.Value);
			obj.WorkingReportNo = Conversion.ToInt((object)txtWorkingReportNo.Text);
			obj.Status = "Open";
			obj.GpQRCode = Conversion.ToByteArray((object)GpQrCode);
			obj.DriverName = Conversion.ToString((object)txtDriverName.Text);
			obj.DriverMobileNo = Conversion.ToString((object)txtDriverMobile.Text);
			obj.DocAttachment = Conversion.ToString((object)((Control)(object)cmbWeighBridge).Text);
			obj.PurchaseOrderId = obj.GpSrNo;
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.gatePassInwardDetails = new List<GatePassInwardDetail>();
			if (grdGpDetail.GetRows().Length == 0)
			{
				throw new Exception("Grid detail record not found");
			}
			int Qty = 0;
			double CompareWeight = 0.0;
			double PackUnit = 0.0;
			PackUnit = Conversion.ToDouble(grdGpDetail.GetTotal(grdGpDetail.RootTable.Columns["PackUnit"], (AggregateFunction)3));
			GridEXRow[] rows = grdGpDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				GatePassInwardDetail objdt = new GatePassInwardDetail();
				objdt.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				objdt.PackingTypeId = Conversion.ToInt(item.Cells["PackingTypeId"].Value);
				objdt.ItemQty = Conversion.ToDouble(item.Cells["ItemQty"].Value);
				objdt.PackUnit = Conversion.ToDouble(item.Cells["PackUnit"].Value);
				objdt.Weight = Conversion.ToDouble(item.Cells["Weight"].Value);
				CompareWeight += objdt.Weight;
				Qty += Conversion.ToInt((object)objdt.ItemQty);
				obj.gatePassInwardDetails.Add(objdt);
			}
			obj.WeightComparedToPoWt = CompareWeight;
			obj.SupplierWeight = CompareWeight;
			obj.NoOfPackages = Qty;
			obj.PackUnit = PackUnit;
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
			}
			int success = GatePassInward.Save(obj);
			if (Id > 0)
			{
				MessageBox.Show("Record Update Successfully[" + obj.GpSrNo + "]");
			}
			else
			{
				MessageBox.Show("Record Save Successfully[" + obj.GpSrNo + "]");
			}
			Reset();
			if (ChkBox.Checked)
			{
				GenerateReport(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InwardGatePass_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnPrint.Enabled = formright.DoHavePrintRights;
			btnupdate.Enabled = formright.DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			dtViewAttachments.Columns.Clear();
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtDetail.Columns.Add("ItemId");
			dtDetail.Columns.Add("ItemName");
			dtDetail.Columns.Add("ItemQty", typeof(double));
			dtDetail.Columns.Add("PackingTypeId");
			dtDetail.Columns.Add("PackingType");
			dtDetail.Columns.Add("PackUnit", typeof(double));
			dtDetail.Columns.Add("Weight", typeof(double));
			grdGpDetail.DataSource = dtDetail;
			grdGpDetail.RetrieveStructure();
			grdGpDetailSetting();
			GhallaMandiBind();
			LoadingContractorBind();
			CarriageContractorBind();
			gpnofill();
			ItemNameFill();
			vehicleTypefill();
			cmbWeighBridgeFill();
			PackingTypeBind();
			grdfrmfill();
			btnupdate.Visible = false;
			txtintime.Value = DateTime.Now;
			txtouttime.Value = DateTime.Now;
			btnsave.Visible = true;
			txtPackUnit.Enabled = true;
			btnupdate.Visible = false;
			txtgpdate.Focus();
			ChkBox.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistory(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 51;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.NoOfRecords = NoOfRecords;
			obj.RefDocumentTypeId = 111;
			DataTable dt = GatePassInward.GatepassHistoryForGhallaMandi(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtHistory = new DataTable();
				dtHistory.Columns.Add("Id");
				dtHistory.Columns.Add("RecordNo");
				dtHistory.Columns.Add("GpDate");
				dtHistory.Columns.Add("GpNo");
				dtHistory.Columns.Add("WorkingReportNo");
				dtHistory.Columns.Add("GhallaMandi");
				dtHistory.Columns.Add("LoadingContractor");
				dtHistory.Columns.Add("VehicleType");
				dtHistory.Columns.Add("VehicleNo");
				dtHistory.Columns.Add("BiltyNo");
				dtHistory.Columns.Add("ItemQty");
				dtHistory.Columns.Add("Remarks");
				dtHistory.Columns.Add("InTime");
				dtHistory.Columns.Add("OutTime");
				dtHistory.Columns.Add("UserName");
				dtHistory.Columns.Add("CarriageContractor");
				dtHistory.Columns.Add("Freight");
				dtHistory.Columns.Add("Attachments");
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtHistory.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["RecordNo"], Conversion.ToDateTime(dt.Rows[i]["GpDate"]).ToShortDateString(), dt.Rows[i]["GpSrNo"], dt.Rows[i]["WorkingReportNo"], dt.Rows[i]["GhallaMandi"], dt.Rows[i]["LoadingContractor"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["ItemQty"], dt.Rows[i]["OtherRemarks"], Conversion.ToDateTime(dt.Rows[i]["InTime"]).ToLongTimeString(), Conversion.ToDateTime(dt.Rows[i]["OutTime"]).ToLongTimeString(), dt.Rows[i]["UserName"], dt.Rows[i]["CarriageContractro"], dt.Rows[i]["Freight"], dt.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtHistory;
				grdhistory.RetrieveStructure();
				grdhistorysetting();
			}
			else
			{
				grdhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistorysetting()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["RecordNo"].Visible = false;
			grdhistory.RootTable.Columns["Remarks"].Width = 250;
			grdhistory.RootTable.Columns["Attachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].Width = 50;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].Width = 50;
			grdhistory.RootTable.Columns["Print"].Position = 0;
			grdhistory.RootTable.Columns["Edit"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		try
		{
			DataTable dt = GatePassInward.GetOpenGatePassForGhallaMandiGpDetail(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 51,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				RefDocumentTypeId = 111
			});
			if (dt.Rows.Count > 0)
			{
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdfrmsetting();
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrmsetting()
	{
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grd.RootTable.Columns.Add("Print");
			grd.RootTable.Columns["Print"].Caption = "Print";
			grd.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Print"].ButtonText = "Print";
			grd.RootTable.Columns["Print"].Width = 50;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Print"].Position = 0;
			grd.RootTable.Columns["Edit"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdGpDetailSetting()
	{
		try
		{
			grdGpDetail.RootTable.Columns["ItemId"].Visible = false;
			grdGpDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdGpDetail.RootTable.Columns["ItemName"].Width = 350;
			grdGpDetail.RootTable.Columns["PackingType"].Width = 150;
			grdGpDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdGpDetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdGpDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdGpDetail.RootTable.Columns["PackUnit"].TextAlignment = (TextAlignment)3;
			grdGpDetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdGpDetail.RootTable.Columns["ItemQty"].FormatString = "#,#";
			grdGpDetail.RootTable.Columns["PackUnit"].FormatString = "#,#";
			grdGpDetail.RootTable.Columns["Weight"].FormatString = "#,#";
			grdGpDetail.RootTable.Columns["ItemQty"].TotalFormatString = "#,#";
			grdGpDetail.RootTable.Columns["Weight"].TotalFormatString = "#,#";
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
			dtDetail.Rows.Clear();
			grdGpDetail.ClearStructure();
			tabControl2.SelectedIndex = 0;
			POId = 0;
			txtPackUnit.Enabled = true;
			txtBalQty.Text = string.Empty;
			txtvehicleno.Text = string.Empty;
			txtbiltyno.Text = string.Empty;
			((UltraGridBase)cmbWeighBridge).Rows[0].Activate();
			((Control)(object)CmbVariety).Text = string.Empty;
			((Control)(object)CmbGhallaMandi).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtfreight.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((Control)(object)CmbLoadingContractor).Text = string.Empty;
			((Control)(object)CmbCarriageContractor).Text = string.Empty;
			txtWorkingReportNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtDriverMobile.Text = string.Empty;
			table.Rows.Clear();
			grd.DataSource = table;
			gpnofill();
			grdfrmfill();
			txtintime.Value = DateTime.Now;
			txtouttime.Value = DateTime.Now;
			txtgpdate.Focus();
			((Control)(object)cmbWeighBridge).Enabled = true;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			UpdateMode = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void InwardGatePass_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.L && tabControl1.SelectedIndex == 1)
			{
				btnLoadAll_Click(null, null);
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				btnsave_Click(null, null);
			}
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
			if (btnPrint.Visible && btnPrint.Enabled && e.Control && e.KeyCode == Keys.P)
			{
				btnPrint_Click(null, null);
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
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if (tabControl1.SelectedIndex == 0 && e.Control && e.KeyCode == Keys.F5)
			{
				txtgpdate.Focus();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.Left)
				{
					tabControl2.SelectedIndex = 0;
				}
				if (e.Control && e.KeyCode == Keys.Right)
				{
					tabControl2.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grd).Focus();
			}
			if (e.Alt && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
				txtgpdate.Focus();
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			gridhistory(50);
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
		Close();
	}

	private void GenerateReport(int PrintId)
	{
		try
		{
			CommonServices.GatePassInwardWithDetailAndLabReport(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(Id);
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
			GhallaMandiBind();
			ItemNameFill();
			vehicleTypefill();
			LoadingContractorBind();
			CarriageContractorBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton4_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@Id", Id.ToString());
			val.RptPerameter("@organizationId", UserAccount.OrganizationId.ToString());
			val.RptPerameter("@CompanyId", UserAccount.CompanyId.ToString());
			val.RptPerameter("@FromDate", (string)null);
			val.RptPerameter("@ToDate", txtgpdate.Value.ToString());
			val.RptPerameter("@GpSrNoF", (string)null);
			val.RptPerameter("@GpSrNoT", (string)null);
			val.RptPerameter("@OrderNo", (string)null);
			val.RptPerameter("@DocumentTypeId", "51");
			val.RptPerameter("@Status", (string)null);
			val.RptPerameter("@SupplierCustomerId", (string)null);
			val.RptPerameter("@IsApproved", (string)null);
			val.RptPerameter("@GatepassInwardId", Id.ToString());
			val.RptPerameter("@ReferenceDocNoId", Id.ToString());
			val.RptPerameter("@InwardGatepassId", Id.ToString());
			val.ShowReport("252-RptInwardGatePassSlip-GP-QC-WB-CWB.rpt");
		}
		catch (Exception)
		{
			MessageBox.Show("Record Not Found");
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Attachments"))
			{
				return;
			}
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
	{
		try
		{
			gridhistory();
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
				Id = Conversion.ToInt(grdhistory.CurrentRow.Cells[1].Value);
				getUpdate(Id);
			}
			if (e.Column.Key == "Print")
			{
				try
				{
					GenerateReport(Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value));
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Id = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
				getUpdate(Id);
			}
			if (e.Column.Key == "Print")
			{
				try
				{
					GenerateReport(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value));
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					return;
				}
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void grdhistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
			getUpdate(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
			getUpdate(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierWeight_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtvehicleno_TextChanged(object sender, EventArgs e)
	{
		txtvehicleno.SelectionStart = txtvehicleno.Text.Length;
		txtvehicleno.Text = txtvehicleno.Text.ToUpper();
	}

	private void CmbOrderno_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void btndefvehicle_Click(object sender, EventArgs e)
	{
		try
		{
			new VehicleType(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		WeightComparedPoWtCalculation();
	}

	private void WeightComparedPoWtCalculation()
	{
		try
		{
			double Qty = Conversion.ToDouble((object)txtqty.Text.Trim());
			double PackUnit = Conversion.ToDouble((object)txtPackUnit.Text.Trim());
			if (Qty > 0.0 && PackUnit > 0.0)
			{
				txtWeight.Text = (Qty * PackUnit).ToString();
			}
			else
			{
				txtWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtPackUnit_TextChanged(object sender, EventArgs e)
	{
		WeightComparedPoWtCalculation();
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtDetail.Rows.Add(CmbVariety.Value, ((Control)(object)CmbVariety).Text, txtqty.Text.Trim(), CmbPackingType.Value, ((Control)(object)CmbPackingType).Text.Trim(), txtPackUnit.Text.Trim(), txtWeight.Text.Trim());
				grdGpDetail.DataSource = dtDetail;
				grdGpDetail.RetrieveStructure();
				grdGpDetailSetting();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			CmbVariety.Focus();
			((Control)(object)CmbVariety).Text = string.Empty;
			txtqty.Text = string.Empty;
			((Control)(object)CmbPackingType).Text = string.Empty;
			txtPackUnit.Text = string.Empty;
			txtWeight.Text = string.Empty;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdGpDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdGpDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbVariety.Value = item.Cells["ItemId"].Value.ToString();
				txtqty.Text = item.Cells["ItemQty"].Value.ToString();
				CmbPackingType.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtPackUnit.Text = item.Cells["PackUnit"].Value.ToString();
				txtWeight.Text = item.Cells["Weight"].Value.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbVariety.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtDetail.Rows[updateDetailIndex]["ItemId"] = CmbVariety.Value;
				dtDetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbVariety).Text;
				dtDetail.Rows[updateDetailIndex]["PackingTypeId"] = CmbPackingType.Value;
				dtDetail.Rows[updateDetailIndex]["PackingType"] = ((Control)(object)CmbPackingType).Text;
				dtDetail.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDouble((object)txtqty.Text);
				dtDetail.Rows[updateDetailIndex]["PackUnit"] = Conversion.ToDouble((object)txtPackUnit.Text);
				dtDetail.Rows[updateDetailIndex]["Weight"] = Conversion.ToDouble((object)txtWeight.Text);
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
			}
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

	private void txtPackUnit_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FillHistoryDetail(int Id)
	{
		try
		{
			DataTable dt = GatePassInward.ReadByHeaderId(Id);
			DataTable dtHistoryDetail = new DataTable();
			dtHistoryDetail.Columns.Add("ItemName");
			dtHistoryDetail.Columns.Add("PackingType");
			dtHistoryDetail.Columns.Add("PackSize");
			dtHistoryDetail.Columns.Add("Qty", typeof(double));
			dtHistoryDetail.Columns.Add("Weight", typeof(double));
			dtHistoryDetail.Rows.Clear();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				dtHistoryDetail.Rows.Add(dt.Rows[i]["ItemName"], dt.Rows[i]["PackTypecode"], dt.Rows[i]["PackUnit"], dt.Rows[i]["ItemQty"], dt.Rows[i]["Weight"]);
			}
			if (dtHistoryDetail.Rows.Count > 0)
			{
				grdhistorydetail.DataSource = dtHistoryDetail;
				grdhistorydetail.RetrieveStructure();
				HistoryDetailSetting();
			}
			else
			{
				grdhistorydetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryDetailSetting()
	{
		try
		{
			grdhistorydetail.RootTable.Columns["ItemName"].Width = 300;
			grdhistorydetail.RootTable.Columns["PackingType"].Width = 100;
			grdhistorydetail.RootTable.Columns["PackSize"].Width = 80;
			grdhistorydetail.RootTable.Columns["Qty"].Width = 80;
			grdhistorydetail.RootTable.Columns["Weight"].Width = 80;
			grdhistorydetail.RootTable.Columns["Weight"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
			grdhistorydetail.RootTable.Columns["Weight"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["Weight"].TotalFormatString = "#,##0.##";
			grdhistorydetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grdhistorydetail.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grdhistorydetail.RootTable.Columns["Qty"].FormatString = "0,0";
			grdhistorydetail.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			if (grdhistory.CurrentRow.Cells.Count > 0)
			{
				Id = Conversion.ToInt(grdhistory.CurrentRow.Cells["Id"].Value);
				FillHistoryDetail(Id);
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
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Expected O, but got Unknown
		//IL_0347: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Expected O, but got Unknown
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0417: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_045a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0464: Expected O, but got Unknown
		//IL_0465: Unknown result type (might be due to invalid IL or missing references)
		//IL_046f: Expected O, but got Unknown
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c1: Expected O, but got Unknown
		//IL_0f05: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f0f: Expected O, but got Unknown
		//IL_5fad: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fb7: Expected O, but got Unknown
		//IL_5fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fe5: Expected O, but got Unknown
		GridEXLayout grdGpDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance73 = new Appearance();
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
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
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
		Appearance appearance26 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
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
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Paddy_Purchase.frmGatePassInwardForGhallaMandi));
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistorydetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel9 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage5 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdGpDetail = new GridEX();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label11 = new System.Windows.Forms.Label();
		this.txtWorkingReportNo = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.label51 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.Add = new System.Windows.Forms.Button();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.txtDriverMobile = new System.Windows.Forms.TextBox();
		this.label50 = new System.Windows.Forms.Label();
		this.CmbCarriageContractor = new UltraCombo();
		this.label46 = new System.Windows.Forms.Label();
		this.pictureBox1 = new System.Windows.Forms.PictureBox();
		this.cmbWeighBridge = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbLoadingContractor = new UltraCombo();
		this.cmbvehicletype = new UltraCombo();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.txtBalQty = new System.Windows.Forms.TextBox();
		this.txtRecQty = new System.Windows.Forms.TextBox();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.CmbGhallaMandi = new UltraCombo();
		this.CmbVariety = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.txtouttime = new System.Windows.Forms.DateTimePicker();
		this.txtintime = new System.Windows.Forms.DateTimePicker();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtbiltyno = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtWeight = new System.Windows.Forms.TextBox();
		this.txtPackUnit = new System.Windows.Forms.TextBox();
		this.txtfreight = new System.Windows.Forms.TextBox();
		this.txtvehicleno = new System.Windows.Forms.TextBox();
		this.txtgpno = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btndefvehicle = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdhistorydetail = new GridEX();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel9.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage5.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGpDetail).BeginInit();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCarriageContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWeighBridge).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGhallaMandi).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).BeginInit();
		this.panel3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1201, 735);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1201, 735);
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
		this.tabPage1.Size = new System.Drawing.Size(1193, 709);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel9.Controls.Add(this.tabControl2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(2, 314);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1189, 393);
		this.panel9.TabIndex = 3;
		this.tabControl2.Controls.Add(this.tabPage5);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1189, 393);
		this.tabControl2.TabIndex = 0;
		this.tabPage5.Controls.Add(this.panel10);
		this.tabPage5.Location = new System.Drawing.Point(4, 22);
		this.tabPage5.Name = "tabPage5";
		this.tabPage5.Size = new System.Drawing.Size(1181, 367);
		this.tabPage5.TabIndex = 2;
		this.tabPage5.Text = "GatePass Detail";
		this.tabPage5.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdGpDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1181, 367);
		this.panel10.TabIndex = 0;
		this.grdGpDetail.AllowDelete = (InheritableBoolean)1;
		this.grdGpDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGpDetail).BackColor = System.Drawing.Color.White;
		grdGpDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGpDetail.DesignTimeLayout = grdGpDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGpDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGpDetail.DynamicFiltering = true;
		this.grdGpDetail.FilterMode = (FilterMode)2;
		this.grdGpDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGpDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGpDetail.GroupByBoxVisible = false;
		this.grdGpDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGpDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGpDetail).Name = "grdGpDetail";
		((ScrollableBase)this.grdGpDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGpDetail).Size = new System.Drawing.Size(1181, 367);
		((System.Windows.Forms.Control)(object)this.grdGpDetail).TabIndex = 0;
		this.grdGpDetail.TotalRow = (InheritableBoolean)1;
		this.grdGpDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGpDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdGpDetail).DoubleClick += new System.EventHandler(grdGpDetail_DoubleClick);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1181, 367);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Open GatePass Information";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.DynamicFiltering = true;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1175, 361);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtWorkingReportNo);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.label54);
		this.panel4.Controls.Add(this.label53);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.panel4.Controls.Add(this.label52);
		this.panel4.Controls.Add(this.label51);
		this.panel4.Controls.Add(this.label40);
		this.panel4.Controls.Add(this.label37);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.btnCancelUpdateDetial);
		this.panel4.Controls.Add(this.btnUpdateDetail);
		this.panel4.Controls.Add(this.Add);
		this.panel4.Controls.Add(this.txtDriverName);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.txtDriverMobile);
		this.panel4.Controls.Add(this.label50);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCarriageContractor);
		this.panel4.Controls.Add(this.label46);
		this.panel4.Controls.Add(this.pictureBox1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWeighBridge);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbLoadingContractor);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbvehicletype);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Controls.Add(this.txtBalQty);
		this.panel4.Controls.Add(this.txtRecQty);
		this.panel4.Controls.Add(this.txtOrderQty);
		this.panel4.Controls.Add(this.label32);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGhallaMandi);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVariety);
		this.panel4.Controls.Add(this.label31);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.txtouttime);
		this.panel4.Controls.Add(this.txtintime);
		this.panel4.Controls.Add(this.txtgpdate);
		this.panel4.Controls.Add(this.txtqty);
		this.panel4.Controls.Add(this.txtbiltyno);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add(this.txtWeight);
		this.panel4.Controls.Add(this.txtPackUnit);
		this.panel4.Controls.Add(this.txtfreight);
		this.panel4.Controls.Add(this.txtvehicleno);
		this.panel4.Controls.Add(this.txtgpno);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label22);
		this.panel4.Controls.Add(this.label14);
		this.panel4.Controls.Add(this.label42);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label38);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label16);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(2, 67);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1189, 247);
		this.panel4.TabIndex = 0;
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.Red;
		this.label11.Location = new System.Drawing.Point(437, 61);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(14, 16);
		this.label11.TabIndex = 7;
		this.label11.Text = "*";
		this.txtWorkingReportNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWorkingReportNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWorkingReportNo.Location = new System.Drawing.Point(453, 58);
		this.txtWorkingReportNo.Name = "txtWorkingReportNo";
		this.txtWorkingReportNo.Size = new System.Drawing.Size(142, 22);
		this.txtWorkingReportNo.TabIndex = 9;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(339, 62);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(101, 15);
		this.label6.TabIndex = 121;
		this.label6.Text = "Working Report #";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Red;
		this.label5.Location = new System.Drawing.Point(121, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(14, 16);
		this.label5.TabIndex = 0;
		this.label5.Text = "*";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(121, 34);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 16);
		this.label4.TabIndex = 1;
		this.label4.Text = "*";
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(121, 115);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 16);
		this.label3.TabIndex = 4;
		this.label3.Text = "*";
		this.label54.AutoSize = true;
		this.label54.BackColor = System.Drawing.Color.White;
		this.label54.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold);
		this.label54.ForeColor = System.Drawing.Color.Red;
		this.label54.Location = new System.Drawing.Point(408, 192);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(14, 16);
		this.label54.TabIndex = 116;
		this.label54.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(325, 192);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(75, 15);
		this.label53.TabIndex = 115;
		this.label53.Text = "Packing type";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(324, 213);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 19;
		this.label52.AutoSize = true;
		this.label52.BackColor = System.Drawing.Color.White;
		this.label52.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(671, 61);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 16);
		this.label52.TabIndex = 12;
		this.label52.Text = "*";
		this.label51.AutoSize = true;
		this.label51.BackColor = System.Drawing.Color.White;
		this.label51.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold);
		this.label51.ForeColor = System.Drawing.Color.Red;
		this.label51.Location = new System.Drawing.Point(511, 192);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(14, 16);
		this.label51.TabIndex = 113;
		this.label51.Text = "*";
		this.label40.AutoSize = true;
		this.label40.BackColor = System.Drawing.Color.White;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold);
		this.label40.ForeColor = System.Drawing.Color.Red;
		this.label40.Location = new System.Drawing.Point(255, 192);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(14, 16);
		this.label40.TabIndex = 112;
		this.label40.Text = "*";
		this.label37.AutoSize = true;
		this.label37.BackColor = System.Drawing.Color.White;
		this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.ForeColor = System.Drawing.Color.Red;
		this.label37.Location = new System.Drawing.Point(89, 192);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(14, 16);
		this.label37.TabIndex = 111;
		this.label37.Text = "*";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.Red;
		this.label7.Location = new System.Drawing.Point(121, 88);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(14, 16);
		this.label7.TabIndex = 3;
		this.label7.Text = "*";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(727, 212);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 24;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(659, 212);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 26);
		this.btnUpdateDetail.TabIndex = 23;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.Add.BackColor = System.Drawing.Color.White;
		this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.Location = new System.Drawing.Point(629, 213);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(29, 25);
		this.Add.TabIndex = 22;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(691, 5);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(119, 22);
		this.txtDriverName.TabIndex = 12;
		this.label48.AutoSize = true;
		this.label48.BackColor = System.Drawing.Color.White;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(603, 9);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(74, 15);
		this.label48.TabIndex = 106;
		this.label48.Text = "Driver Name";
		this.txtDriverMobile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverMobile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverMobile.Location = new System.Drawing.Point(691, 31);
		this.txtDriverMobile.Name = "txtDriverMobile";
		this.txtDriverMobile.Size = new System.Drawing.Size(119, 22);
		this.txtDriverMobile.TabIndex = 13;
		this.label50.AutoSize = true;
		this.label50.BackColor = System.Drawing.Color.White;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(603, 35);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(68, 15);
		this.label50.TabIndex = 103;
		this.label50.Text = "Driver Cell#";
		this.CmbCarriageContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbCarriageContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCarriageContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCarriageContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCarriageContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCarriageContractor).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCarriageContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCarriageContractor).Location = new System.Drawing.Point(140, 111);
		((System.Windows.Forms.Control)(object)this.CmbCarriageContractor).Name = "CmbCarriageContractor";
		((System.Windows.Forms.Control)(object)this.CmbCarriageContractor).Size = new System.Drawing.Size(193, 25);
		((System.Windows.Forms.Control)(object)this.CmbCarriageContractor).TabIndex = 6;
		this.label46.AutoSize = true;
		this.label46.BackColor = System.Drawing.Color.White;
		this.label46.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(8, 116);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(109, 15);
		this.label46.TabIndex = 98;
		this.label46.Text = "Carriage Contractor";
		this.pictureBox1.Location = new System.Drawing.Point(1195, 6);
		this.pictureBox1.Name = "pictureBox1";
		this.pictureBox1.Size = new System.Drawing.Size(120, 102);
		this.pictureBox1.TabIndex = 90;
		this.pictureBox1.TabStop = false;
		this.pictureBox1.Visible = false;
		this.cmbWeighBridge.AllowNull = (DefaultableBoolean)1;
		this.cmbWeighBridge.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWeighBridge.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWeighBridge.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWeighBridge.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Location = new System.Drawing.Point(691, 56);
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Name = "cmbWeighBridge";
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Size = new System.Drawing.Size(119, 26);
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).TabIndex = 14;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(603, 62);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(62, 15);
		this.label18.TabIndex = 73;
		this.label18.Text = "WB Status";
		this.CmbLoadingContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLoadingContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLoadingContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLoadingContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance22).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance24).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLoadingContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLoadingContractor).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbLoadingContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLoadingContractor).Location = new System.Drawing.Point(140, 84);
		((System.Windows.Forms.Control)(object)this.CmbLoadingContractor).Name = "CmbLoadingContractor";
		((System.Windows.Forms.Control)(object)this.CmbLoadingContractor).Size = new System.Drawing.Size(193, 25);
		((System.Windows.Forms.Control)(object)this.CmbLoadingContractor).TabIndex = 5;
		this.cmbvehicletype.AllowNull = (DefaultableBoolean)1;
		this.cmbvehicletype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbvehicletype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbvehicletype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance63;
		((SpecialBoxBase)((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbvehicletype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbvehicletype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Location = new System.Drawing.Point(453, 4);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Name = "cmbvehicletype";
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).Size = new System.Drawing.Size(142, 25);
		((System.Windows.Forms.Control)(object)this.cmbvehicletype).TabIndex = 7;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.Location = new System.Drawing.Point(815, 145);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(67, 19);
		this.ChkBox.TabIndex = 18;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.txtBalQty.Location = new System.Drawing.Point(1352, 6);
		this.txtBalQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtBalQty.Name = "txtBalQty";
		this.txtBalQty.ReadOnly = true;
		this.txtBalQty.Size = new System.Drawing.Size(44, 21);
		this.txtBalQty.TabIndex = 71;
		this.txtBalQty.TabStop = false;
		this.txtBalQty.Visible = false;
		this.txtRecQty.Location = new System.Drawing.Point(1352, 87);
		this.txtRecQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtRecQty.Name = "txtRecQty";
		this.txtRecQty.Size = new System.Drawing.Size(44, 21);
		this.txtRecQty.TabIndex = 71;
		this.txtRecQty.Visible = false;
		this.txtOrderQty.Location = new System.Drawing.Point(1352, 64);
		this.txtOrderQty.Margin = new System.Windows.Forms.Padding(2);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.Size = new System.Drawing.Size(44, 21);
		this.txtOrderQty.TabIndex = 69;
		this.txtOrderQty.Visible = false;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.White;
		this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.ForeColor = System.Drawing.Color.Red;
		this.label32.Location = new System.Drawing.Point(434, 8);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(14, 16);
		this.label32.TabIndex = 68;
		this.label32.Text = "*";
		this.CmbGhallaMandi.AllowNull = (DefaultableBoolean)1;
		this.CmbGhallaMandi.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGhallaMandi.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGhallaMandi.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Appearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance29;
		((SpecialBoxBase)((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGhallaMandi).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGhallaMandi).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbGhallaMandi.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGhallaMandi).Location = new System.Drawing.Point(140, 57);
		((System.Windows.Forms.Control)(object)this.CmbGhallaMandi).Name = "CmbGhallaMandi";
		((System.Windows.Forms.Control)(object)this.CmbGhallaMandi).Size = new System.Drawing.Size(193, 25);
		((System.Windows.Forms.Control)(object)this.CmbGhallaMandi).TabIndex = 2;
		this.CmbVariety.AllowNull = (DefaultableBoolean)1;
		this.CmbVariety.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVariety.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVariety.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbVariety).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance90;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance95).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVariety.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Location = new System.Drawing.Point(6, 212);
		((System.Windows.Forms.Control)(object)this.CmbVariety).Name = "CmbVariety";
		((System.Windows.Forms.Control)(object)this.CmbVariety).Size = new System.Drawing.Size(220, 25);
		((System.Windows.Forms.Control)(object)this.CmbVariety).TabIndex = 17;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.ForeColor = System.Drawing.Color.Red;
		this.label31.Location = new System.Drawing.Point(121, 61);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(14, 16);
		this.label31.TabIndex = 67;
		this.label31.Text = "*";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.Red;
		this.label17.Location = new System.Drawing.Point(434, 34);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(14, 16);
		this.label17.TabIndex = 6;
		this.label17.Text = "*";
		this.txtouttime.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtouttime.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtouttime.CustomFormat = "HH:mm";
		this.txtouttime.Enabled = false;
		this.txtouttime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtouttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtouttime.Location = new System.Drawing.Point(691, 112);
		this.txtouttime.Name = "txtouttime";
		this.txtouttime.Size = new System.Drawing.Size(119, 22);
		this.txtouttime.TabIndex = 16;
		this.txtouttime.TabStop = false;
		this.txtouttime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.txtintime.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtintime.CustomFormat = "HH:mm";
		this.txtintime.Enabled = false;
		this.txtintime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtintime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtintime.Location = new System.Drawing.Point(691, 85);
		this.txtintime.Name = "txtintime";
		this.txtintime.Size = new System.Drawing.Size(119, 22);
		this.txtintime.TabIndex = 15;
		this.txtintime.TabStop = false;
		this.txtintime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.txtgpdate.CustomFormat = "dd-MMM-yyyy";
		this.txtgpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(140, 31);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(193, 22);
		this.txtgpdate.TabIndex = 1;
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(229, 213);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(93, 24);
		this.txtqty.TabIndex = 18;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtbiltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbiltyno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbiltyno.Location = new System.Drawing.Point(453, 85);
		this.txtbiltyno.Name = "txtbiltyno";
		this.txtbiltyno.Size = new System.Drawing.Size(142, 22);
		this.txtbiltyno.TabIndex = 10;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(140, 140);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(670, 26);
		this.txtremarks.TabIndex = 17;
		this.txtWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeight.Location = new System.Drawing.Point(533, 213);
		this.txtWeight.Name = "txtWeight";
		this.txtWeight.ReadOnly = true;
		this.txtWeight.Size = new System.Drawing.Size(93, 24);
		this.txtWeight.TabIndex = 21;
		this.txtPackUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackUnit.Location = new System.Drawing.Point(449, 213);
		this.txtPackUnit.Name = "txtPackUnit";
		this.txtPackUnit.Size = new System.Drawing.Size(80, 24);
		this.txtPackUnit.TabIndex = 20;
		this.txtPackUnit.TextChanged += new System.EventHandler(txtPackUnit_TextChanged);
		this.txtPackUnit.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtPackUnit_KeyPress);
		this.txtfreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfreight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfreight.Location = new System.Drawing.Point(453, 112);
		this.txtfreight.Name = "txtfreight";
		this.txtfreight.Size = new System.Drawing.Size(142, 22);
		this.txtfreight.TabIndex = 11;
		this.txtvehicleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehicleno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehicleno.Location = new System.Drawing.Point(453, 31);
		this.txtvehicleno.Name = "txtvehicleno";
		this.txtvehicleno.Size = new System.Drawing.Size(142, 22);
		this.txtvehicleno.TabIndex = 8;
		this.txtvehicleno.TextChanged += new System.EventHandler(txtvehicleno_TextChanged);
		this.txtgpno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgpno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpno.Location = new System.Drawing.Point(140, 5);
		this.txtgpno.Name = "txtgpno";
		this.txtgpno.ReadOnly = true;
		this.txtgpno.Size = new System.Drawing.Size(193, 22);
		this.txtgpno.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(603, 116);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(54, 15);
		this.label13.TabIndex = 58;
		this.label13.Text = "OutTime";
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.White;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(226, 192);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(26, 15);
		this.label22.TabIndex = 56;
		this.label22.Text = "Qty";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(9, 146);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(52, 15);
		this.label14.TabIndex = 53;
		this.label14.Text = "Remarks";
		this.label42.AutoSize = true;
		this.label42.BackColor = System.Drawing.Color.White;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(535, 192);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(46, 15);
		this.label42.TabIndex = 54;
		this.label42.Text = "Weight";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(339, 89);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(49, 15);
		this.label10.TabIndex = 56;
		this.label10.Text = "Bilty No";
		this.label38.AutoSize = true;
		this.label38.BackColor = System.Drawing.Color.White;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(446, 192);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(57, 15);
		this.label38.TabIndex = 54;
		this.label38.Text = "Pack Unit";
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(603, 89);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(45, 15);
		this.label12.TabIndex = 12;
		this.label12.Text = "InTime";
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.White;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(339, 116);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(70, 15);
		this.label21.TabIndex = 54;
		this.label21.Text = "Bilty Freight";
		this.label25.AutoSize = true;
		this.label25.BackColor = System.Drawing.Color.White;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(3, 192);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(78, 15);
		this.label25.TabIndex = 50;
		this.label25.Text = "Variety Name";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 35);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(50, 15);
		this.label1.TabIndex = 60;
		this.label1.Text = "GP Date";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(339, 35);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(64, 15);
		this.label9.TabIndex = 52;
		this.label9.Text = "Vehicle No";
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.White;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(8, 89);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(109, 15);
		this.label20.TabIndex = 50;
		this.label20.Text = "Loading Contractor";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(339, 9);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(73, 15);
		this.label8.TabIndex = 48;
		this.label8.Text = "Vehicle Type";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 62);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(77, 15);
		this.label2.TabIndex = 47;
		this.label2.Text = "Ghalla Mandi";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(8, 9);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(41, 15);
		this.label16.TabIndex = 61;
		this.label16.Text = "GP No";
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 31);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1189, 36);
		this.panel3.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel6.Controls.Add(this.label15);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1189, 36);
		this.panel6.TabIndex = 46;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(8, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(304, 24);
		this.label15.TabIndex = 0;
		this.label15.Text = "Inward Gate Pass For Ghalla Mandi";
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1189, 29);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1138, -1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = null;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar3.TabIndex = 59;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1090, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 25);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnupdate, this.btnsave, this.btnAttachment, this.btnRefresh, this.btndefvehicle, this.btnPrint, this.toolStripButton4, this.btnDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1189, 27);
		this.toolStrip1.TabIndex = 58;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.ToolTipText = "Ctrl_N";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.ToolTipText = "Ctrl_U";
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.ToolTipText = "Ctrl_S";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.ToolTipText = "Alt_A";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.ToolTipText = "Alt_R";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btndefvehicle.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btndefvehicle.Name = "btndefvehicle";
		this.btndefvehicle.Size = new System.Drawing.Size(85, 24);
		this.btndefvehicle.Text = "Define Vehicle";
		this.btndefvehicle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.btndefvehicle.Click += new System.EventHandler(btndefvehicle_Click);
		this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = (System.Drawing.Image)resources.GetObject("btnPrint.Image");
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(79, 24);
		this.btnPrint.Text = "256-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(108, 24);
		this.toolStripButton4.Text = "252-LabReport";
		this.toolStripButton4.Visible = false;
		this.toolStripButton4.Click += new System.EventHandler(toolStripButton4_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(64, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1193, 709);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.AutoScroll = true;
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Controls.Add(this.panel8);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 2);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1189, 705);
		this.panel5.TabIndex = 0;
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1189, 28);
		this.panel7.TabIndex = 0;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1140, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdhistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar2.TabIndex = 73;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.toolStrip2.BackColor = System.Drawing.Color.White;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.toolStripLabel1, this.toolStripSeparator1, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1189, 27);
		this.toolStrip2.TabIndex = 73;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 28);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1189, 407);
		this.panel8.TabIndex = 3;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1189, 407);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 45;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistorydetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 435);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1189, 270);
		this.panel11.TabIndex = 4;
		this.grdhistorydetail.AllowEdit = (InheritableBoolean)2;
		grdhistorydetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistorydetail.DesignTimeLayout = grdhistorydetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistorydetail.DynamicFiltering = true;
		this.grdhistorydetail.FilterMode = (FilterMode)2;
		this.grdhistorydetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistorydetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Name = "grdhistorydetail";
		((ScrollableBase)this.grdhistorydetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).Size = new System.Drawing.Size(1189, 270);
		((System.Windows.Forms.Control)(object)this.grdhistorydetail).TabIndex = 45;
		this.grdhistorydetail.TotalRow = (InheritableBoolean)1;
		this.grdhistorydetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistorydetail).VisualStyle = (VisualStyle)4;
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 12.25f, System.Drawing.FontStyle.Bold);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(69, 24);
		this.toolStripLabel1.Text = "History";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1201, 735);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmGatePassInwardForGhallaMandi";
		this.Text = "Inward GatePass ";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(InwardGatePass_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InwardGatePass_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage5.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGpDetail).EndInit();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCarriageContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWeighBridge).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLoadingContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbvehicletype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGhallaMandi).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdhistorydetail).EndInit();
		base.ResumeLayout(false);
	}
}

}
