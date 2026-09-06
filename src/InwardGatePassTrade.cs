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
using Architecture.BLL.Inventory;
using Architecture.BLL.WeightBridge;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.WeightBridge;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.PurchaseTrading
{

public class InwardGatePassTrade : Form
{
	private int updateDetailIndex;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	public DataTable dtSupplier = new DataTable();

	private int Id;

	public string dtOrderIds = "";

	public string dtOrderMainIds = "";

	public int POId;

	private DataTable dtdetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private bool SubsidiaryAccountAllownOnVouchers;

	private DataTable dtItem = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	public string GRNS = string.Empty;

	public bool SupplySelected;

	private Rightsobjects formright = new Rightsobjects();

	private frmLoadPurchaseOrderForGatePass LoadOrder;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton printToolStripButton;

	private ToolStripButton btnAttachment;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Label label25;

	private Label label21;

	private Label label19;

	private TextBox txtbilityNo;

	private TextBox txtvehno;

	internal UltraCombo cmbsupplier;

	private Panel panel7;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label14;

	private ToolStripButton toolStripButton1;

	private Panel panel10;

	private CheckBox ChkBox;

	private Timer timer1;

	internal GridEX GrdDetail;

	internal GridEX GrdHistory;

	internal UltraCombo cmbVehicleType;

	private DateTimePicker txtgpdate;

	private TextBox txtgptypeno;

	private TextBox txtgpno;

	private Label label8;

	private Label label9;

	private Label label11;

	internal UltraCombo cmbgptype;

	private Label label20;

	internal UltraCombo cmbcityDetail;

	private DateTimePicker txtintime;

	private Label label3;

	private Label label22;

	private TextBox txtNetPaid;

	private Label label1;

	internal UltraCombo CmbOrderType;

	private TextBox CmbOrderno;

	private Label label57;

	private Label label58;

	private Label label60;

	internal UltraCombo CmbVariety;

	private TextBox txtqty;

	private TextBox txtWeightCompared;

	private TextBox txtPackUnit;

	private TextBox txtfreight;

	private Label label42;

	private Label label13;

	private DateTimePicker txtouttime;

	private Label lblDiffRemarks;

	private TextBox txtWeightDiffRemarks;

	private GroupBox groupBox1;

	private Label label59;

	private Label label72;

	private TextBox txtWeighBridgeSlipNo;

	private TextBox txtSupplierWeight;

	private TextBox txtFactoryWeight;

	private TextBox txtDifferenceWeight;

	internal UltraCombo CmbStatus;

	internal UltraCombo cmbWeighBridge;

	private Label label73;

	private Label label74;

	private Label label71;

	private Label label70;

	private Label label75;

	private Label label67;

	private Label label66;

	private Label label78;

	private Label label81;

	private Label label79;

	private RichTextBox txtRemarks;

	private GroupBox groupBox2;

	internal UltraCombo cmbPackUom;

	internal UltraCombo cmbWarehouse;

	private Label label2;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	internal GridEX grdDetailHistory;

	private ToolStrip toolStrip2;

	private Label label5;

	private Label label6;

	internal UltraCombo cmbOrderNoDetail;

	private TextBox txtRemrksDetail;

	private Label label10;

	private Label label7;

	private TabPage tabPage3;

	internal GridEX GridDetailOpenGp;

	private Panel panel9;

	private Label label15;

	internal GridEX GridMainOpenGP;

	private Panel panel5;

	private ToolStrip toolStrip3;

	private ToolStripButton btnLoadAllOpenGP;

	private Label label12;

	private CtrlGrdBar ctrlGrdBar3;

	private ToolStripSeparator toolStripSeparator1;

	private ToolStripButton btnLoaderView2;

	private ToolStripButton btnLoadOrderMainDetail;

	private ToolStripSeparator toolStripSeparator2;

	private GroupBox groupBox3;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShowHistory;

	private Label label18;

	private Label label16;

	private Label label17;

	private TextBox ToDocNoHistory;

	private Label label24;

	private Label label26;

	private TextBox FromDocNoHistory;

	internal UltraCombo CmbCustomerHistory;

	private DateTimePicker txtToDateHistory;

	private DateTimePicker txtFromDateHistory;

	private Panel panel12;

	private Panel panel11;

	private Panel panel6;

	private Panel panel13;

	private Label label4;

	private ToolStripButton btnloadall;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	public InwardGatePassTrade(UserAccount user)
	{
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 625);
			dtViewAttachments.Columns.Add("AttchmentPath");
			SubsidiaryAccountAllownOnVouchers = CommonServices.GetERPFeatureById(4);
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("RefDocumentTypeId", typeof(int));
			dtdetail.Columns.Add("SuppliercustomerId", typeof(int));
			dtdetail.Columns.Add("OrderId", typeof(int));
			dtdetail.Columns.Add("OrderDetailId", typeof(int));
			dtdetail.Columns.Add("OrderNo", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName", typeof(string));
			dtdetail.Columns.Add("PackUOM", typeof(string));
			dtdetail.Columns.Add("PackUOMEquivalent", typeof(double));
			dtdetail.Columns.Add("UOMId", typeof(int));
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("ItemWeight", typeof(double));
			dtdetail.Columns.Add("WareHouseId", typeof(int));
			dtdetail.Columns.Add("CityId", typeof(int));
			dtdetail.Columns.Add("RemarksDetail", typeof(string));
			GrdDetail.DataSource = dtdetail;
			GrdDetail.RetrieveStructure();
			grdSettings();
			SupplierCustomerFill();
			GenerateCode();
			VehicleTypesFill();
			CityNameBind();
			WareHouseFill();
			OrderTypeFill();
			ItemNameFill();
			Status();
			gatepasstype();
			btnSave.Visible = true;
			btnplus.Visible = true;
			btnUpdate.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			txtgpdate.Focus();
			ChkBox.Checked = true;
			defaultConfiquration();
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
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0377: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			GatePassInward gatePass = new GatePassInward();
			if (Id > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				gatePass.Id = Id;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (GrdDetail.RowCount > 0)
			{
				gatePass.BiltyNo = txtbilityNo.Text.Trim();
				gatePass.CityId = Conversion.ToInt(cmbcityDetail.Value);
				gatePass.OrganizationId = UserAccount.OrganizationId;
				gatePass.CompanyId = UserAccount.CompanyId;
				gatePass.BranchesId = UserAccount.BranchesId;
				gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				gatePass.DocumentTypeId = 1601;
				gatePass.DocAttachment = "";
				gatePass.FactoryWeight = Conversion.ToDouble((object)txtFactoryWeight.Text);
				gatePass.SupplierWeight = Conversion.ToDouble((object)txtSupplierWeight.Text);
				gatePass.DifferenceWeight = Conversion.ToDouble((object)txtDifferenceWeight.Text);
				gatePass.Freight = Conversion.ToDouble((object)txtfreight.Text);
				gatePass.GatepassType = ((Control)(object)cmbgptype).Text.ToString();
				gatePass.GpDate = txtgpdate.Value;
				gatePass.GpSrNo = int.Parse(txtgpno.Text);
				gatePass.GpTypeSrNo = Conversion.ToInt((object)txtgptypeno.Text.Trim());
				gatePass.InDateTimeStamp = txtintime.Value;
				gatePass.NoOfPackages = 0;
				gatePass.OtherRemarks = txtRemarks.Text.Trim();
				gatePass.OtherSupCust = ((Control)(object)CmbOrderType).Text;
				gatePass.IsApproved = true;
				gatePass.OutDateTimeStamp = DateTime.Now;
				gatePass.RefDocumentEntryNo = 0;
				gatePass.RefDocumentTypeId = Conversion.ToInt(CmbOrderType.Value);
				gatePass.SupplierContractCode = "";
				gatePass.VehicleNo = txtvehno.Text.Trim();
				gatePass.VehicleType = ((Control)(object)cmbVehicleType).Text.Trim();
				gatePass.SupplierCustomerId = Conversion.ToInt(cmbsupplier.Value);
				gatePass.Status = ((Control)(object)CmbStatus).Text.Trim();
				if (gatePass.Status == "Rejected" && MessageBox.Show("Are you sure to Reject GatePass?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
				if (txtNetPaid.Text.Trim() != string.Empty && txtNetPaid.Text.Trim() != null)
				{
					gatePass.NetPaid = float.Parse(txtNetPaid.Text.Trim());
				}
				else
				{
					gatePass.NetPaid = 0.0;
				}
				gatePass.WarehouseId = 0;
				gatePass.WeighBridgeId = 0;
				gatePass.VarietyName = "";
				gatePass.PurchaseOrderId = 0;
				gatePass.EntryUser = UserAccount.ID;
				gatePass.ModifyUser = UserAccount.ID;
				GenerateCode();
				gatePass.gatePassInwardDetails = new List<GatePassInwardDetail>();
				GridEXRow[] rows = GrdDetail.GetRows();
				foreach (GridEXRow a in rows)
				{
					GatePassInwardDetail inwardDetail = new GatePassInwardDetail();
					inwardDetail.Id = Conversion.ToInt(a.Cells["Id"].Value);
					inwardDetail.RefDocumentTypeId = Conversion.ToInt(a.Cells["RefDocumentTypeId"].Value);
					inwardDetail.SupplierCustomerId = Conversion.ToInt(a.Cells["SupplierCustomerId"].Value);
					if (inwardDetail.SupplierCustomerId > 0)
					{
						if (Conversion.ToInt(a.Cells["OrderId"].Value) == 0)
						{
							MessageBox.Show("Order No required In Detail...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
							return;
						}
						if (Conversion.ToInt(a.Cells["OrderId"].Value) > 0 && Conversion.ToInt(CmbOrderType.Value) != 1600)
						{
							MessageBox.Show("Order Type Should be Purchase Order as Order No In Detail is Greater then Zero...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
							CmbOrderType.Focus();
							return;
						}
						inwardDetail.PurchaseOrderId = Conversion.ToInt(a.Cells["OrderId"].Value);
						inwardDetail.PurchaseOrderDetailId = Conversion.ToInt(a.Cells["OrderDetailId"].Value);
					}
					if (Conversion.ToInt(a.Cells["ItemId"].Value) == 0)
					{
						MessageBox.Show("Item required In Detail...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
						return;
					}
					inwardDetail.ItemId = Conversion.ToInt(a.Cells["ItemId"].Value);
					if (Conversion.ToInt(a.Cells["UOMId"].Value) == 0)
					{
						MessageBox.Show("Pack UOM required In Detail...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
						return;
					}
					inwardDetail.ItemUOMId = Conversion.ToInt(a.Cells["UOMId"].Value);
					if (Conversion.ToDouble(a.Cells["ItemQty"].Value) == 0.0)
					{
						MessageBox.Show("ItemQty required In Detail...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
						return;
					}
					inwardDetail.ItemQty = Conversion.ToDouble(a.Cells["ItemQty"].Value);
					if (Conversion.ToDouble(a.Cells["ItemWeight"].Value) == 0.0)
					{
						MessageBox.Show("ItemWeight required In Detail...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
						return;
					}
					inwardDetail.Weight = Conversion.ToDouble(a.Cells["ItemWeight"].Value);
					if (Conversion.ToInt(a.Cells["WareHouseId"].Value) == 0)
					{
						MessageBox.Show("WareHouse required In Detail...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
						return;
					}
					inwardDetail.WareHouseId = Conversion.ToInt(a.Cells["WareHouseId"].Value);
					if (Conversion.ToInt(a.Cells["CityId"].Value) == 0)
					{
						MessageBox.Show("City required In Detail...\n In row No. :" + (Conversion.ToInt((object)a.RowIndex) + 1));
						return;
					}
					inwardDetail.CityId = Conversion.ToInt(a.Cells["CityId"].Value);
					inwardDetail.RemarksDetail = Conversion.ToString(a.Cells["RemarksDetail"].Value);
					gatePass.gatePassInwardDetails.Add(inwardDetail);
				}
				gatePass.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				gatePass.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<GatePassInward>(gatePass, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
				GatePassInward.Save(gatePass);
				if (Id > 0)
				{
					MessageBox.Show("Record Update Successfully[" + gatePass.GpSrNo + "]");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0 && AT.lst.Count == 0)
					{
						global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(Id, base.Name, 1601, 0);
					}
				}
				else
				{
					MessageBox.Show("Record Save Successfully[" + gatePass.GpSrNo + "]");
				}
				reset();
			}
			else
			{
				MessageBox.Show("Grid Have No Record ....");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
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

	public void ReadById(int ID)
	{
		try
		{
			Id = Conversion.ToInt((object)ID);
			GatePassInward inward = GatePassInward.GetByDetailID(Id);
			if (inward != null)
			{
				tabControl1.SelectedIndex = 0;
				SupplierCustomerFill();
				cmbsupplier.Value = inward.SupplierCustomerId;
				CmbStatus.Value = inward.Status;
				txtgpno.Text = inward.GpSrNo.ToString();
				CmbOrderType.Value = inward.RefDocumentTypeId;
				txtgpdate.Value = inward.GpDate;
				txtbilityNo.Text = inward.BiltyNo;
				cmbgptype.Value = inward.GatepassType;
				txtgptypeno.Text = inward.GpTypeSrNo.ToString();
				txtDifferenceWeight.Text = inward.DifferenceWeight.ToString();
				txtFactoryWeight.Text = inward.FactoryWeight.ToString();
				txtSupplierWeight.Text = inward.SupplierWeight.ToString();
				if (!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)inward.InDateTimeStamp)))
				{
					txtintime.Value = Conversion.ToDateTime((object)inward.InDateTimeStamp);
				}
				txtNetPaid.Text = inward.NetPaid.ToString();
				txtvehno.Text = inward.VehicleNo;
				((Control)(object)cmbVehicleType).Text = inward.VehicleType;
				cmbcityDetail.Value = inward.CityId;
				txtRemarks.Text = inward.OtherRemarks;
				dtdetail.Clear();
				for (int i = 0; i < inward.gatePassInwardDetails.Count; i++)
				{
					dtdetail.Rows.Add(inward.gatePassInwardDetails[i].Id, inward.gatePassInwardDetails[i].RefDocumentTypeId, inward.gatePassInwardDetails[i].SupplierCustomerId, inward.gatePassInwardDetails[i].PurchaseOrderId, inward.gatePassInwardDetails[i].PurchaseOrderDetailId, inward.gatePassInwardDetails[i].OrderNo, inward.gatePassInwardDetails[i].ItemId, inward.gatePassInwardDetails[i].ItemName, inward.gatePassInwardDetails[i].UOMCode, inward.gatePassInwardDetails[i].Equivalent, inward.gatePassInwardDetails[i].ItemUOMId, inward.gatePassInwardDetails[i].ItemQty, inward.gatePassInwardDetails[i].Weight, inward.gatePassInwardDetails[i].WareHouseId, inward.gatePassInwardDetails[i].CityId, inward.gatePassInwardDetails[i].RemarksDetail);
				}
				GrdDetail.DataSource = dtdetail;
				GrdDetail.RetrieveStructure();
				grdSettings();
				GetWeighBridgeWeightAndTicketNos(Id);
				Total();
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				((UltraGridBase)CmbStatus).Rows[1].Activate();
				FormHelper.LoadAttachmentsForObject(AT, Id, base.Name);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetWeighBridgeWeightAndTicketNos(int recordId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		try
		{
			DataTable lstWb = WbTransactions.GetNetWeightFromWbTransactions(new WbTransactions
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				ReferenceDocTypeId = 1601,
				ReferenceDocNoId = recordId
			});
			if (lstWb.Rows.Count > 0)
			{
				string TicketNo = "";
				double NetWeight = 0.0;
				for (int i = 0; i < lstWb.Rows.Count; i++)
				{
					TicketNo = TicketNo + "," + lstWb.Rows[i]["TicketNo"].ToString();
					NetWeight += Convert.ToDouble(lstWb.Rows[i]["NetWbWeight"]);
				}
				txtWeighBridgeSlipNo.Text = TicketNo;
				txtFactoryWeight.Text = NetWeight.ToString();
				if (txtFactoryWeight.Text != string.Empty && txtFactoryWeight.Text != "0")
				{
					((Control)(object)cmbWeighBridge).Enabled = false;
				}
			}
			else
			{
				txtWeighBridgeSlipNo.Text = "0";
				txtFactoryWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtgpno.Text.Trim() == string.Empty || txtgpno.Text.Trim() == "0" || txtgpno.Text.Trim() == null)
		{
			MessageBox.Show("GatePassNo Field Required");
			txtgpno.Focus();
			return false;
		}
		if (((Control)(object)cmbgptype).Text.Trim() == string.Empty)
		{
			MessageBox.Show("GatePass Type  Field is Required");
			cmbgptype.Focus();
			return false;
		}
		if (((Control)(object)cmbsupplier).Text.Trim() == string.Empty || Conversion.ToInt(cmbsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			cmbsupplier.Focus();
			return false;
		}
		if (((UltraGridBase)cmbVehicleType).ActiveRow == null || Conversion.ToInt(cmbVehicleType.Value) == 0)
		{
			MessageBox.Show("VehicleType Field Required");
			cmbVehicleType.Focus();
			return false;
		}
		if (txtvehno.Text.Trim() == string.Empty)
		{
			MessageBox.Show("VehicleNo Field Required");
			txtvehno.Focus();
			return false;
		}
		if (txtSupplierWeight.Text.Trim() == string.Empty && Conversion.ToInt(CmbOrderType.Value) != 141)
		{
			MessageBox.Show("SupplierWeight Field Required");
			txtSupplierWeight.Focus();
			return false;
		}
		if (((UltraGridBase)CmbStatus).ActiveRow == null || Conversion.ToInt(CmbStatus.Value) == 0)
		{
			MessageBox.Show("Status Field Required");
			CmbStatus.Focus();
			return false;
		}
		return true;
	}

	private bool FormDetailValidation()
	{
		if (((UltraGridBase)CmbVariety).ActiveRow == null || Conversion.ToInt(CmbVariety.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			CmbVariety.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPackUom).ActiveRow == null || Conversion.ToInt(cmbPackUom.Value) == 0)
		{
			MessageBox.Show("Item Pack Unit Field is Required");
			cmbPackUom.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Item Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWarehouse).ActiveRow == null || Conversion.ToInt(cmbWarehouse.Value) == 0)
		{
			MessageBox.Show("Warehouse Field is Required");
			cmbWarehouse.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcityDetail).ActiveRow == null || Conversion.ToInt(cmbcityDetail.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			cmbcityDetail.Focus();
			return false;
		}
		return true;
	}

	private void defaultConfiquration()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				cmbcityDetail.Value = CityId;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration Error");
		}
	}

	private void GenerateCode()
	{
		try
		{
			int Code = CommonServices.GatePassInwardGenerateCode(1601);
			if (Code > 0)
			{
				txtgpno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierCustomerFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbsupplier.Value);
			int Id2 = Conversion.ToInt(CmbCustomerHistory.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers(1);
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			}
			if (dtSupplier.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CompanyName");
				for (int i = 0; i < dtSupplier.Rows.Count; i++)
				{
					dt.Rows.Add(dtSupplier.Rows[i]["Id"], dtSupplier.Rows[i]["CompanyName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbsupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: true);
					DDL.BindDDL(dt, CmbCustomerHistory, "Id", "CompanyName", "Supplier", ZeroIndex: true);
					if (Id > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							cmbsupplier.Value = Id;
						}
						else
						{
							((Control)(object)cmbsupplier).Text = string.Empty;
						}
					}
					if (Id2 > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
						{
							CmbCustomerHistory.Value = Id2;
						}
						else
						{
							((Control)(object)CmbCustomerHistory).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)cmbsupplier).DataSource = null;
					((Control)(object)cmbsupplier).Text = string.Empty;
					((UltraGridBase)CmbCustomerHistory).DataSource = null;
					((Control)(object)CmbCustomerHistory).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)cmbsupplier).DataSource = null;
				((Control)(object)cmbsupplier).Text = string.Empty;
				((UltraGridBase)CmbCustomerHistory).DataSource = null;
				((Control)(object)CmbCustomerHistory).Text = string.Empty;
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
			dtItem = CommonServices.ItemGetForComboServiceBindForEngr();
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItem, CmbVariety, "Id", "ItemName", "Item Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		try
		{
			DataTable dt = CommonServices.WareHouseGetAllService();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbWarehouse, "Id", "WareHouseName", "Item Name", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VehicleTypesFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new VehicleType();
			DataTable dtvt = new DataTable();
			dtvt = VehicleType.GetAll();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("VehicleDescription");
			for (int i = 0; i < dtvt.Rows.Count; i++)
			{
				dt.Rows.Add(dtvt.Rows[i]["Id"], dtvt.Rows[i]["VehicleDescription"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbVehicleType, "Id", "VehicleDescription", "Vehicle Description", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gatepasstype()
	{
		try
		{
			DataTable dt = CommonServices.GatePassTypeFill("Inward_Engr");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbgptype, "Id", "GpTypeDescription", "GpType", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityNameBind()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			City val = new City
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtcity = new DataTable();
			dtcity = City.GetAll(val);
			DataTable Dtc = new DataTable();
			Dtc.Columns.Add("Id");
			Dtc.Columns.Add("City");
			for (int i = 0; i < dtcity.Rows.Count; i++)
			{
				Dtc.Rows.Add(dtcity.Rows[i]["Id"], dtcity.Rows[i]["CityName"]);
			}
			if (Dtc.Rows.Count > 0)
			{
				DDL.BindDDL(Dtc, cmbcityDetail, "Id", "City", "City Name", ZeroIndex: false);
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Status");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "Accepted");
			dt.Rows.Add(3, "Rejected");
			DDL.BindDDL(dt, CmbStatus, "Id", "Status", "Status", ZeroIndex: false);
			((UltraGridBase)CmbStatus).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("OrderType");
			dt.Rows.Add(1600, "Purchase Order");
			dt.Rows.Add(4, "Returnable");
			dt.Rows.Add(1612, "Returned");
			dt.Rows.Add(141, "Market/Demand");
			dt.Rows.Add(5, "Transfer");
			DDL.BindDDL(dt, CmbOrderType, "Id", "OrderType", "OrderType", ZeroIndex: false);
			((UltraGridBase)CmbOrderType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbgptype_Leave(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbgptype).Text != string.Empty)
			{
				int Code = GatePassInward.GenerateGPTypeCode(new GatePassInward
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					GatepassType = ((Control)(object)cmbgptype).Text.ToString(),
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = 1601
				});
				if (Code > 0)
				{
					txtgptypeno.Text = Code.ToString();
					return;
				}
				MessageBox.Show("Please GatePass Type Select");
				cmbgptype.Focus();
			}
			else
			{
				txtgptypeno.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbVariety_Leave(object sender, EventArgs e)
	{
		try
		{
			bindRateUomAndItemPackUom();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindRateUomAndItemPackUom()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable listUOM = UOMSchedule.Getall(new UOMSchedule
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (listUOM.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtUOM = new DataTable();
			dtUOM.Columns.Add("Id");
			dtUOM.Columns.Add("UOM");
			dtUOM.Columns.Add("UOMCode");
			dtUOM.Rows.Clear();
			for (int i = 0; i < listUOM.Rows.Count; i++)
			{
				if (Conversion.ToInt(listUOM.Rows[i]["ItemId"]) == Conversion.ToInt(CmbVariety.Value))
				{
					dtUOM.Rows.Add(listUOM.Rows[i]["Id"], listUOM.Rows[i]["UOMCode"], listUOM.Rows[i]["Equivalent"]);
				}
				if (dtUOM.Rows.Count > 0)
				{
					DDL.BindDDL(dtUOM, cmbPackUom, "Id", "UOM", "PackUOM", ZeroIndex: false);
					((UltraGridBase)cmbPackUom).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
		ResetDetail();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			((Control)(object)cmbsupplier).Text = string.Empty;
			((Control)(object)cmbcityDetail).Text = string.Empty;
			((Control)(object)cmbgptype).Text = string.Empty;
			((Control)(object)cmbVehicleType).Text = string.Empty;
			txtbilityNo.Text = string.Empty;
			txtgpno.Text = string.Empty;
			txtgptypeno.Text = string.Empty;
			txtNetPaid.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtvehno.Text = string.Empty;
			btnSave.Visible = true;
			dtdetail.Clear();
			GrdDetail.DataSource = dtdetail;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			GenerateCode();
			defaultConfiquration();
			txtgpdate.Focus();
			POId = 0;
			dtOrderIds = string.Empty;
			dtOrderMainIds = string.Empty;
			((Control)(object)CmbOrderType).Enabled = true;
			((Control)(object)cmbsupplier).Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierCustomerFill();
			GenerateCode();
			VehicleTypesFill();
			CityNameBind();
			Status();
			gatepasstype();
			ItemNameFill();
			WareHouseFill();
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
			((Control)(object)CmbVariety).Text = string.Empty;
			((Control)(object)cmbOrderNoDetail).Text = string.Empty;
			((Control)(object)cmbPackUom).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtWeightCompared.Text = string.Empty;
			((Control)(object)cmbWarehouse).Text = string.Empty;
			((UltraGridBase)cmbOrderNoDetail).DataSource = null;
			((Control)(object)CmbVariety).Enabled = true;
			((Control)(object)cmbOrderNoDetail).Enabled = true;
			CmbVariety.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InwardGatePassTrade_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			if (GrdDetail.GetRows().Count() > 0)
			{
				GridEXRow item = GrdDetail.GetRow(0);
				if (item != null && Conversion.ToInt(item.Cells["OrderId"].Value) > 0)
				{
					MessageBox.Show("You cannot Add because record exist with Order");
					return;
				}
			}
			if (!FormDetailValidation())
			{
				return;
			}
			GridEXRow[] rows = GrdDetail.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int ItemId = Conversion.ToInt(rows[i].Cells["ItemId"].Value);
				if (Conversion.ToInt(CmbVariety.Value) == ItemId)
				{
					MessageBox.Show("Duplicate Item Not Add in Grid");
					return;
				}
			}
			dtdetail.Rows.Add(0, 0, 0, 0, 0, 0, CmbVariety.Value, ((Control)(object)CmbVariety).Text.Trim(), ((Control)(object)cmbPackUom).Text.Trim(), ((UltraDropDownBase)cmbPackUom).SelectedRow.Cells[2].Value, cmbPackUom.Value, txtqty.Text.Trim(), txtWeightCompared.Text.Trim(), cmbWarehouse.Value, cmbcityDetail.Value, txtRemrksDetail.Text.Trim());
			GrdDetail.DataSource = dtdetail;
			GrdDetail.RetrieveStructure();
			grdSettings();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = GrdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				if (Conversion.ToInt(item.Cells["OrderId"].Value) > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id");
					dt.Columns.Add("No");
					dt.Clear();
					dt.Rows.Add(item.Cells["OrderId"].Value.ToString(), item.Cells["OrderNo"].Value.ToString());
					DDL.BindDDL(dt, cmbOrderNoDetail, "Id", "No", "Order No", ZeroIndex: false);
					cmbOrderNoDetail.Value = item.Cells["OrderId"].Value.ToString();
					((Control)(object)cmbOrderNoDetail).Enabled = false;
					((Control)(object)CmbVariety).Enabled = false;
				}
				CmbVariety.Value = item.Cells["ItemId"].Value.ToString();
				CmbVariety_Leave(null, null);
				cmbPackUom.Value = item.Cells["UOMId"].Value.ToString();
				txtqty.Text = item.Cells["ItemQty"].Value.ToString();
				txtWeightCompared.Text = item.Cells["ItemWeight"].Value.ToString();
				cmbWarehouse.Value = item.Cells["WareHouseId"].Value.ToString();
				cmbcityDetail.Value = item.Cells["CityId"].Value.ToString();
				txtRemrksDetail.Text = item.Cells["RemarksDetail"].Value.ToString();
				btnplus.Visible = false;
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
			if (FormDetailValidation())
			{
				dtdetail.Rows[updateDetailIndex]["ItemId"] = CmbVariety.Value;
				dtdetail.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)CmbVariety).Text;
				dtdetail.Rows[updateDetailIndex]["PackUOM"] = ((Control)(object)cmbPackUom).Text;
				dtdetail.Rows[updateDetailIndex]["UOMId"] = cmbPackUom.Value;
				dtdetail.Rows[updateDetailIndex]["ItemQty"] = txtqty.Text;
				dtdetail.Rows[updateDetailIndex]["ItemWeight"] = txtWeightCompared.Text;
				dtdetail.Rows[updateDetailIndex]["WareHouseId"] = cmbWarehouse.Value;
				dtdetail.Rows[updateDetailIndex]["CityId"] = cmbcityDetail.Value;
				dtdetail.Rows[updateDetailIndex]["RemarksDetail"] = txtRemrksDetail.Text;
				GrdDetail.DataSource = dtdetail;
				ResetDetail();
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		ResetDetail();
	}

	public void grdSettings()
	{
		try
		{
			GrdDetail.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			GrdDetail.RootTable.Columns["Id"].Visible = false;
			GrdDetail.RootTable.Columns["OrderId"].Visible = false;
			GrdDetail.RootTable.Columns["OrderdetailId"].Visible = false;
			GrdDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GrdDetail.RootTable.Columns["ItemId"].Visible = false;
			GrdDetail.RootTable.Columns["UOMId"].Visible = false;
			GrdDetail.RootTable.Columns["PackUOMEquivalent"].Visible = false;
			GrdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["OrderId"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["OrderdetailId"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["SupplierCustomerId"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["PackUOM"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["UOMId"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["ItemWeight"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["PackUOMEquivalent"].EditType = (EditType)0;
			GrdDetail.RootTable.Columns["ItemQty"].ColumnType = (ColumnType)0;
			GrdDetail.RootTable.Columns["RemarksDetail"].ColumnType = (ColumnType)0;
			DataTable dtCity = CommonServices.CityGetAllService();
			GrdDetail.RootTable.Columns["CityId"].ColumnType = (ColumnType)0;
			GrdDetail.RootTable.Columns["CityId"].HasValueList = true;
			GrdDetail.RootTable.Columns["CityId"].LimitToList = true;
			GrdDetail.RootTable.Columns["CityId"].EditType = (EditType)4;
			GrdDetail.RootTable.Columns["CityId"].Caption = "City Name";
			GrdDetail.RootTable.Columns["CityId"].ValueList.PopulateValueList((IEnumerable)dtCity.DefaultView, "Id", "CityName");
			DataTable dtWare = CommonServices.WareHouseGetAllService();
			GrdDetail.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
			GrdDetail.RootTable.Columns["WareHouseId"].HasValueList = true;
			GrdDetail.RootTable.Columns["WareHouseId"].LimitToList = true;
			GrdDetail.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
			GrdDetail.RootTable.Columns["WareHouseId"].Caption = "WareHouse Name";
			GrdDetail.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtWare.DefaultView, "Id", "WareHouseName");
			GrdDetail.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			GrdDetail.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			GrdDetail.RootTable.Columns["ItemQty"].FormatString = "0,0";
			GrdDetail.RootTable.Columns["ItemQty"].TotalFormatString = "0,0";
			GrdDetail.RootTable.Columns.Add("Delete");
			GrdDetail.RootTable.Columns["Delete"].Key = "Delete";
			GrdDetail.RootTable.Columns["Delete"].Caption = "X";
			GrdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			GrdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			GrdDetail.RootTable.Columns["Delete"].Width = 20;
			GrdDetail.RootTable.Columns["Delete"].Position = 0;
			GrdDetail.FrozenColumns = 1;
			CommonServices.GridAutoAdjustmentNew(GrdDetail);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (!btnSave.Visible || !btnSave.Enabled)
				{
					MessageBox.Show("Record cannot Delete in Update Mode");
					return;
				}
				if (MessageBox.Show("Are you sure to Delete this Row?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.No)
				{
					return;
				}
				GridEXRow item = GrdDetail.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
			GetPurchaseOrderIds();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GrdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GrdDetail.UpdateData();
			GridEXRow item = GrdDetail.CurrentRow;
			if (e.Column.Key == "ItemQty" || e.Column.Key == "ItemWeight")
			{
				if (item != null && (int)item.RowType == 0 && item.Cells["PackUOMEquivalent"].Value.ToString() != string.Empty && item.Cells["ItemQty"].Value.ToString() != string.Empty)
				{
					double num = Conversion.ToDouble((object)item.Cells["ItemQty"].Value.ToString());
					double UOM = Conversion.ToDouble((object)item.Cells["PackUOMEquivalent"].Value.ToString());
					double Weight = num * UOM;
					item.Cells["ItemWeight"].Value = Weight;
				}
				GrdDetail.UpdateData();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridMainOpenGPfill()
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
			DataTable dt = GatePassInward.ReadByGPDate(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1601,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("GatepassType", typeof(string));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("NetPaid", typeof(string));
				dtcol.Columns.Add("SupplierWeight", typeof(double));
				dtcol.Columns.Add("FactoryWeight", typeof(double));
				dtcol.Columns.Add("DifferenceWeight", typeof(double));
				dtcol.Columns.Add("InTime", typeof(string));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("Remarks", typeof(string));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtcol.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["RefDocumentTypeId"], dt.Rows[i]["GatepassType"], dt.Rows[i]["GpDate"], dt.Rows[i]["GpSrNo"], dt.Rows[i]["SupplierName"], dt.Rows[i]["VehicleType"], dt.Rows[i]["VehicleNo"], dt.Rows[i]["BiltyNo"], dt.Rows[i]["NetPaid"], dt.Rows[i]["SupplierWeight"], dt.Rows[i]["FactoryWeight"], dt.Rows[i]["DifferenceWeight"], Conversion.ToDateTime(dt.Rows[i]["InTime"]).ToString("hh:mm tt"), dt.Rows[i]["Status"], dt.Rows[i]["OtherRemarks"], dt.Rows[i]["NoOfAttachments"]);
				}
				GridMainOpenGP.DataSource = dtcol;
				GridMainOpenGP.RetrieveStructure();
				GridMainOpenGPsetting();
			}
			else
			{
				GridMainOpenGP.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridMainOpenGPsetting()
	{
		try
		{
			GridMainOpenGP.RootTable.Columns["Id"].Visible = false;
			GridMainOpenGP.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			GridMainOpenGP.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GridMainOpenGP.RootTable.Columns.Add("Print");
			GridMainOpenGP.RootTable.Columns["Print"].Caption = "Print";
			GridMainOpenGP.RootTable.Columns["Print"].ButtonText = "Print";
			GridMainOpenGP.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridMainOpenGP.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GridMainOpenGP.RootTable.Columns["Print"].Width = 40;
			GridMainOpenGP.RootTable.Columns["Print"].Position = 0;
			GridMainOpenGP.RootTable.Columns.Add("Edit");
			GridMainOpenGP.RootTable.Columns["Edit"].Caption = "Edit";
			GridMainOpenGP.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GridMainOpenGP.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GridMainOpenGP.RootTable.Columns["Edit"].ButtonText = "Edit";
			GridMainOpenGP.RootTable.Columns["Edit"].Width = 40;
			GridMainOpenGP.RootTable.Columns["Edit"].Position = 1;
			GridMainOpenGP.FrozenColumns = 2;
			CommonServices.GridAutoAdjustmentNew(GridMainOpenGP);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GridMainOpenGP_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)GridMainOpenGP.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridMainOpenGP_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt((object)GridMainOpenGP.CurrentRow.Cells["Id"].Value.ToString()));
			}
			if (e.Column.Key == "Print")
			{
				Generatereport(Conversion.ToInt(GridMainOpenGP.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridMainOpenGP_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GridMainOpenGP.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("OrderNo");
			dt.Columns.Add("Item");
			dt.Columns.Add("PackUOM");
			dt.Columns.Add("ItemQty", typeof(double));
			dt.Columns.Add("ItemWeight", typeof(double));
			dt.Columns.Add("WareHouseName");
			dt.Columns.Add("CityName");
			dt.Columns.Add("RemarksDetail");
			GatePassInward inward = GatePassInward.GetByDetailID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			if (inward != null)
			{
				for (int i = 0; i < inward.gatePassInwardDetails.Count; i++)
				{
					dt.Rows.Add(inward.gatePassInwardDetails[i].OrderNo, inward.gatePassInwardDetails[i].ItemName, inward.gatePassInwardDetails[i].UOMCode, inward.gatePassInwardDetails[i].ItemQty, inward.gatePassInwardDetails[i].Weight, inward.gatePassInwardDetails[i].WareHouseName, inward.gatePassInwardDetails[i].CityName, inward.gatePassInwardDetails[i].RemarksDetail);
				}
				GridDetailOpenGp.DataSource = dt;
				GridDetailOpenGp.RetrieveStructure();
				CommonServices.GridAutoAdjustmentNew(GridDetailOpenGp);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridMainOpenGP_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GridMainOpenGP.CurrentRow;
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

	private void btnShowHistory_Click(object sender, EventArgs e)
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

	private void btnShowHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "GrdHistory")
			{
				txtFromDateHistory.Focus();
			}
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
			txtFromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			txtToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			txtFromDateHistory.Focus();
			GrdHistory.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierCustomerFill();
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
			GridMainOpenGPfill();
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.BranchesId = clsGlobalVariables.UserAccount.BranchesId;
			obj.DocumentTypeId = 1601;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = clsGlobalVariables.UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (txtFromDateHistory.Checked)
				{
					obj.FromDate = txtFromDateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.ToDate = txtToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (txtFromDateHistory.Checked)
				{
					obj.EntryDateFrom = txtFromDateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.EntryDateTo = txtToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (txtFromDateHistory.Checked)
				{
					obj.ModifyDateFrom = txtFromDateHistory.Value;
				}
				if (txtToDateHistory.Checked)
				{
					obj.ModifyDateTo = txtToDateHistory.Value;
				}
			}
			DataTable dthistory = GatePassInward.GatepassHistory(obj);
			GrdHistory.DataSource = null;
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("GatepassType", typeof(string));
			dt.Columns.Add("GpDate", typeof(DateTime));
			dt.Columns.Add("GpSrNo", typeof(int));
			dt.Columns.Add("SupplierName", typeof(string));
			dt.Columns.Add("VehicleType", typeof(string));
			dt.Columns.Add("VehicleNo", typeof(string));
			dt.Columns.Add("BiltyNo", typeof(string));
			dt.Columns.Add("NetPaid", typeof(double));
			dt.Columns.Add("SupplierWeight", typeof(double));
			dt.Columns.Add("FactoryWeight", typeof(double));
			dt.Columns.Add("DifferenceWeight", typeof(double));
			dt.Columns.Add("InTime", typeof(string));
			dt.Columns.Add("OutTime", typeof(string));
			dt.Columns.Add("Status", typeof(string));
			dt.Columns.Add("EntryUser", typeof(string));
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("ModifyUser", typeof(string));
			dt.Columns.Add("ModifyDate", typeof(DateTime));
			dt.Columns.Add("Remarks", typeof(string));
			dt.Columns.Add("NoOfAttachments", typeof(int));
			for (int i = 0; i < dthistory.Rows.Count; i++)
			{
				dt.Rows.Add(dthistory.Rows[i]["Id"], dthistory.Rows[i]["GatepassType"], Conversion.ToDateTime(dthistory.Rows[i]["GpDate"]).ToString("dd-MMM-yy"), dthistory.Rows[i]["GpSrNo"], dthistory.Rows[i]["CompanyName"], dthistory.Rows[i]["VehicleType"], dthistory.Rows[i]["VehicleNo"], dthistory.Rows[i]["BiltyNo"], dthistory.Rows[i]["NetPaid"], dthistory.Rows[i]["SupplierWeight"], dthistory.Rows[i]["FactoryWeight"], dthistory.Rows[i]["DifferenceWeight"], Conversion.ToDateTime(dthistory.Rows[i]["InTime"]).ToString("hh:mm tt"), Conversion.ToDateTime(dthistory.Rows[i]["OutTime"]).ToString("hh:mm tt"), dthistory.Rows[i]["Status"], dthistory.Rows[i]["UserName"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUserName"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["OtherRemarks"], dthistory.Rows[i]["NoOfAttachments"]);
			}
			GrdHistory.DataSource = dt;
			GrdHistory.RetrieveStructure();
			HistoryGridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["NetPaid"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["NetPaid"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns.Add("View");
			GrdHistory.RootTable.Columns["View"].Caption = "Print";
			GrdHistory.RootTable.Columns["View"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["View"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["View"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["View"].Width = 40;
			GrdHistory.RootTable.Columns["View"].Position = 0;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].Width = 40;
			GrdHistory.RootTable.Columns["Edit"].Position = 1;
			GrdHistory.FrozenColumns = 2;
			CommonServices.GridAutoAdjustmentNew(GrdHistory);
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString()));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "View")
			{
				Generatereport(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
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

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("OrderNo", typeof(int));
			dt.Columns.Add("Item");
			dt.Columns.Add("PackUOM");
			dt.Columns.Add("ItemQty", typeof(double));
			dt.Columns.Add("ItemWeight", typeof(double));
			dt.Columns.Add("WareHouseName");
			dt.Columns.Add("CityName");
			dt.Columns.Add("RemarksDetail");
			GatePassInward inward = GatePassInward.GetByDetailID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()));
			if (inward != null)
			{
				for (int i = 0; i < inward.gatePassInwardDetails.Count; i++)
				{
					dt.Rows.Add(inward.gatePassInwardDetails[i].OrderNo, inward.gatePassInwardDetails[i].ItemName, inward.gatePassInwardDetails[i].UOMCode, inward.gatePassInwardDetails[i].ItemQty, inward.gatePassInwardDetails[i].Weight, inward.gatePassInwardDetails[i].WareHouseName, inward.gatePassInwardDetails[i].CityName, inward.gatePassInwardDetails[i].RemarksDetail);
				}
				grdDetailHistory.DataSource = dt;
				grdDetailHistory.RetrieveStructure();
				CommonServices.GridAutoAdjustmentNew(grdDetailHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnloadall_Click(object sender, EventArgs e)
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

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyData == Keys.Return)
		{
			SendKeys.Send("{TAB}");
		}
		if (e.KeyCode == Keys.Delete && tabControl1.SelectedIndex == 1)
		{
			GridEXRow[] checkedRows = GrdDetail.GetCheckedRows();
			for (int i = 0; i < checkedRows.Length; i++)
			{
				int.Parse(checkedRows[i].Cells["Id"].Value.ToString());
			}
		}
		if (btnSave.Visible && btnSave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
		{
			btnSave_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.N && tabControl1.SelectedIndex == 0)
		{
			btnNew_Click(null, null);
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
		if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
		{
			Close();
		}
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U)
		{
			btnUpdate_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.P)
		{
			printToolStripButton_Click(null, null);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdDetail).Name);
			if (fs != null)
			{
				GrdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetailOpenGp).Name);
			if (fs != null)
			{
				GridDetailOpenGp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			Generatereport(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Generatereport(int PrintId)
	{
		try
		{
			CommonServices.InwardGatePassSlipEngr1601(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Total()
	{
		try
		{
			if (txtSupplierWeight.Text != string.Empty && txtFactoryWeight.Text != string.Empty)
			{
				double num = double.Parse(txtSupplierWeight.Text.Trim());
				double FactoryWeight = double.Parse(txtFactoryWeight.Text.Trim());
				txtDifferenceWeight.Text = (num - FactoryWeight).ToString();
			}
			else
			{
				txtDifferenceWeight.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbPackUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateWeight();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateWeight()
	{
		try
		{
			if (((UltraGridBase)cmbPackUom).ActiveRow != null)
			{
				if (((Control)(object)cmbPackUom).Text != string.Empty && txtqty.Text != string.Empty)
				{
					double ItemPack = Conversion.ToDouble(((UltraDropDownBase)cmbPackUom).SelectedRow.Cells[2].Value);
					double Qty = double.Parse(txtqty.Text);
					txtWeightCompared.Text = (ItemPack * Qty).ToString("0,0");
				}
				else
				{
					txtWeightCompared.Text = 0.ToString("0,0");
				}
			}
			else
			{
				txtWeightCompared.Text = 0.ToString("0,0");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupplierWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFactoryWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			Total();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbOrderno_Leave(object sender, EventArgs e)
	{
	}

	private void txtNetPaid_KeyPress(object sender, KeyPressEventArgs e)
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

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnLoadOrderMainDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbOrderType).ActiveRow == null || Conversion.ToInt(CmbOrderType.Value) == 0)
			{
				MessageBox.Show("Please Select Order Type first....");
				CmbOrderType.Focus();
				return;
			}
			if (Conversion.ToInt(CmbOrderType.Value) != 1600)
			{
				MessageBox.Show("You Can Only Load Data Against Purchase Order.\nPlease Select Order Type (Purchase Order)....");
				CmbOrderType.Focus();
				return;
			}
			if (GrdDetail.GetRows().Count() > 0)
			{
				GridEXRow item = GrdDetail.GetRow(0);
				if (item != null && Conversion.ToInt(item.Cells["OrderId"].Value) == 0)
				{
					MessageBox.Show("You cannot Load SaleOrder because manual entry exists without Order");
					return;
				}
			}
			LoadOrder = new frmLoadPurchaseOrderForGatePass(UserAccount);
			LoadOrder.MainDetail = true;
			if (dtOrderIds != string.Empty)
			{
				LoadOrder.orderIds = dtOrderIds;
				LoadOrder.orderMainIds = dtOrderMainIds;
			}
			LoadOrder.ShowDialog();
			LoadOrder.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoaderView2_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbOrderType).ActiveRow == null)
			{
				MessageBox.Show("Please Select Purchase Type first....");
				CmbOrderType.Focus();
				return;
			}
			LoadOrder = new frmLoadPurchaseOrderForGatePass(UserAccount);
			LoadOrder.MainDetail = false;
			if (dtOrderIds != string.Empty)
			{
				LoadOrder.orderIds = dtOrderIds;
				LoadOrder.orderMainIds = dtOrderMainIds;
			}
			LoadOrder.ShowDialog();
			LoadOrder.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		try
		{
			string PODIds = "";
			if (Conversion.ToInt((object)LoadOrder.dtGDNS.Rows[0][0].ToString()) == 0)
			{
				return;
			}
			for (int j = 0; j < LoadOrder.dtGDNS.Rows.Count; j++)
			{
				PODIds = PODIds + "," + int.Parse(LoadOrder.dtGDNS.Rows[j][0].ToString());
			}
			DataTable dtOrder = PurchaseOrder.PurchaseOrderLoadForGP_Engr(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				OrderDetailIds = PODIds
			});
			if (dtOrder.Rows.Count > 0)
			{
				((Control)(object)CmbOrderType).Enabled = false;
				cmbsupplier.Value = Conversion.ToInt(dtOrder.Rows[0]["OrderSupplierId"]);
				((Control)(object)cmbsupplier).Enabled = false;
				bool flag = false;
				for (int i = 0; i < dtOrder.Rows.Count; i++)
				{
					if (GrdDetail.GetRows().Length != 0)
					{
						GridEXRow[] rows = GrdDetail.GetRows();
						for (int k = 0; k < rows.Length; k++)
						{
							if (Conversion.ToInt(rows[k].Cells["OrderDetailId"].Value) == Conversion.ToInt(dtOrder.Rows[i]["PoDId"]))
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtdetail.Rows.Add(0, dtOrder.Rows[i]["DocumentTypeId"], dtOrder.Rows[i]["OrderSupplierId"], dtOrder.Rows[i]["Id"], dtOrder.Rows[i]["PoDId"], dtOrder.Rows[i]["DocNo"], dtOrder.Rows[i]["OrderItemId"], dtOrder.Rows[i]["ItemName"], dtOrder.Rows[i]["UOMCodeItm"], dtOrder.Rows[i]["PackUOMEquivalent"], dtOrder.Rows[i]["OrderItemUOMId"], dtOrder.Rows[i]["BalQty"], dtOrder.Rows[i]["BalWeight"], 0, dtOrder.Rows[i]["CityId"], dtOrder.Rows[i]["RemarksDetail"]);
					}
					flag = false;
				}
				if (dtItem.Rows.Count > 0)
				{
					DDL.BindDDL(dtItem, CmbVariety, "Id", "ItemName", "Item Name", ZeroIndex: false);
					((UltraGridBase)CmbVariety).DisplayLayout.Bands[0].Columns[1].Width = 450;
				}
				GrdDetail.DataSource = dtdetail;
				GrdDetail.RetrieveStructure();
				grdSettings();
				GetPurchaseOrderIds();
			}
			else
			{
				reset();
			}
			PODIds = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetPurchaseOrderIds()
	{
		try
		{
			dtOrderIds = "";
			dtOrderMainIds = "";
			GridEXRow[] rows = GrdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				dtOrderIds = dtOrderIds + "," + Conversion.ToInt(item.Cells["OrderDetailId"].Value);
				dtOrderMainIds = dtOrderMainIds + "," + Conversion.ToInt(item.Cells["OrderId"].Value);
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bc: Expected O, but got Unknown
		//IL_02bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Expected O, but got Unknown
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Expected O, but got Unknown
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Expected O, but got Unknown
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Expected O, but got Unknown
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Expected O, but got Unknown
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Expected O, but got Unknown
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0310: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Expected O, but got Unknown
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Expected O, but got Unknown
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Expected O, but got Unknown
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Expected O, but got Unknown
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_033a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0341: Expected O, but got Unknown
		//IL_0341: Unknown result type (might be due to invalid IL or missing references)
		//IL_0348: Expected O, but got Unknown
		//IL_0348: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Expected O, but got Unknown
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Expected O, but got Unknown
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0372: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_0379: Expected O, but got Unknown
		//IL_0379: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Expected O, but got Unknown
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Expected O, but got Unknown
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0395: Expected O, but got Unknown
		//IL_0395: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Expected O, but got Unknown
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03aa: Expected O, but got Unknown
		//IL_03aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b1: Expected O, but got Unknown
		//IL_03b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_03b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c6: Expected O, but got Unknown
		//IL_03c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Expected O, but got Unknown
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Expected O, but got Unknown
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Expected O, but got Unknown
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Expected O, but got Unknown
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Expected O, but got Unknown
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Expected O, but got Unknown
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Expected O, but got Unknown
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Expected O, but got Unknown
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e0: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Expected O, but got Unknown
		//IL_04ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_0539: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0586: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c7: Expected O, but got Unknown
		//IL_05ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Expected O, but got Unknown
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a3: Expected O, but got Unknown
		//IL_0707: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Expected O, but got Unknown
		//IL_080f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Expected O, but got Unknown
		//IL_0830: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Expected O, but got Unknown
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Expected O, but got Unknown
		//IL_08bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Expected O, but got Unknown
		//IL_0959: Unknown result type (might be due to invalid IL or missing references)
		//IL_0963: Expected O, but got Unknown
		//IL_0ec0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eca: Expected O, but got Unknown
		//IL_0ed7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ee1: Expected O, but got Unknown
		//IL_6ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ffe: Expected O, but got Unknown
		//IL_7022: Unknown result type (might be due to invalid IL or missing references)
		//IL_702c: Expected O, but got Unknown
		//IL_7863: Unknown result type (might be due to invalid IL or missing references)
		//IL_786d: Expected O, but got Unknown
		//IL_7891: Unknown result type (might be due to invalid IL or missing references)
		//IL_789b: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		GridEXLayout GrdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		GridEXLayout GridDetailOpenGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridMainOpenGP_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.GrdDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbOrderNoDetail = new UltraCombo();
		this.txtRemrksDetail = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbWarehouse = new UltraCombo();
		this.cmbPackUom = new UltraCombo();
		this.CmbVariety = new UltraCombo();
		this.label60 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtWeightCompared = new System.Windows.Forms.TextBox();
		this.label58 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.cmbcityDetail = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label6 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.label72 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbsupplier = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbVehicleType = new UltraCombo();
		this.txtSupplierWeight = new System.Windows.Forms.TextBox();
		this.txtFactoryWeight = new System.Windows.Forms.TextBox();
		this.cmbgptype = new UltraCombo();
		this.txtDifferenceWeight = new System.Windows.Forms.TextBox();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.txtbilityNo = new System.Windows.Forms.TextBox();
		this.label21 = new System.Windows.Forms.Label();
		this.label71 = new System.Windows.Forms.Label();
		this.CmbStatus = new UltraCombo();
		this.label70 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.label73 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label81 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label79 = new System.Windows.Forms.Label();
		this.txtgpno = new System.Windows.Forms.TextBox();
		this.txtgptypeno = new System.Windows.Forms.TextBox();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.CmbOrderType = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.txtintime = new System.Windows.Forms.DateTimePicker();
		this.label22 = new System.Windows.Forms.Label();
		this.txtouttime = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.txtNetPaid = new System.Windows.Forms.TextBox();
		this.txtWeighBridgeSlipNo = new System.Windows.Forms.TextBox();
		this.lblDiffRemarks = new System.Windows.Forms.Label();
		this.txtWeightDiffRemarks = new System.Windows.Forms.TextBox();
		this.cmbWeighBridge = new UltraCombo();
		this.label74 = new System.Windows.Forms.Label();
		this.label75 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label66 = new System.Windows.Forms.Label();
		this.CmbOrderno = new System.Windows.Forms.TextBox();
		this.txtPackUnit = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.txtfreight = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoadOrderMainDetail = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnLoaderView2 = new System.Windows.Forms.ToolStripButton();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.GridDetailOpenGp = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.GridMainOpenGP = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAllOpenGP = new System.Windows.Forms.ToolStripButton();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetailHistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.CmbCustomerHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.txtFromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnloadall = new System.Windows.Forms.ToolStripButton();
		this.label14 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.timer1 = new System.Windows.Forms.Timer(this.components);
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderNoDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcityDetail).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWeighBridge).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailOpenGp).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridMainOpenGP).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).BeginInit();
		this.panel13.SuspendLayout();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel11.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1102, 632);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1102, 632);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetail);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1094, 606);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		((ScrollableBase)this.GrdDetail).BackColor = System.Drawing.Color.White;
		GrdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetail.DesignTimeLayout = GrdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Location = new System.Drawing.Point(2, 402);
		((System.Windows.Forms.Control)(object)this.GrdDetail).Name = "GrdDetail";
		((ScrollableBase)this.GrdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetail).Size = new System.Drawing.Size(1090, 202);
		((System.Windows.Forms.Control)(object)this.GrdDetail).TabIndex = 0;
		this.GrdDetail.TotalRow = (InheritableBoolean)1;
		this.GrdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetail).VisualStyle = (VisualStyle)4;
		this.GrdDetail.CellUpdated += new ColumnActionEventHandler(GrdDetail_CellUpdated);
		this.GrdDetail.ColumnButtonClick += new ColumnActionEventHandler(GrdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.GrdDetail).DoubleClick += new System.EventHandler(GrdDetail_DoubleClick);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label5);
		this.panel10.Controls.Add(this.ctrlGrdBar1);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 377);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1090, 25);
		this.panel10.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 2);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(46, 17);
		this.label5.TabIndex = 1;
		this.label5.Text = "Detail";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1041, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.GrdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtWeighBridgeSlipNo);
		this.panel4.Controls.Add(this.lblDiffRemarks);
		this.panel4.Controls.Add(this.txtWeightDiffRemarks);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWeighBridge);
		this.panel4.Controls.Add(this.label74);
		this.panel4.Controls.Add(this.label75);
		this.panel4.Controls.Add(this.label67);
		this.panel4.Controls.Add(this.label66);
		this.panel4.Controls.Add(this.CmbOrderno);
		this.panel4.Controls.Add(this.txtPackUnit);
		this.panel4.Controls.Add(this.label59);
		this.panel4.Controls.Add(this.txtfreight);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(2, 62);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1090, 315);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOrderNoDetail);
		this.groupBox2.Controls.Add(this.txtRemrksDetail);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.btnplus);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWarehouse);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPackUom);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVariety);
		this.groupBox2.Controls.Add(this.label60);
		this.groupBox2.Controls.Add(this.label57);
		this.groupBox2.Controls.Add(this.txtqty);
		this.groupBox2.Controls.Add(this.txtWeightCompared);
		this.groupBox2.Controls.Add(this.label58);
		this.groupBox2.Controls.Add(this.label42);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcityDetail);
		this.groupBox2.Controls.Add(this.label20);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(8, 190);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(855, 122);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(4, 71);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(86, 15);
		this.label10.TabIndex = 246;
		this.label10.Text = "Remarks Detail";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(4, 21);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(53, 15);
		this.label7.TabIndex = 248;
		this.label7.Text = "OrderNo";
		this.cmbOrderNoDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbOrderNoDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOrderNoDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOrderNoDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOrderNoDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOrderNoDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbOrderNoDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOrderNoDetail).Location = new System.Drawing.Point(4, 40);
		((System.Windows.Forms.Control)(object)this.cmbOrderNoDetail).Name = "cmbOrderNoDetail";
		((System.Windows.Forms.Control)(object)this.cmbOrderNoDetail).Size = new System.Drawing.Size(93, 26);
		((System.Windows.Forms.Control)(object)this.cmbOrderNoDetail).TabIndex = 0;
		this.txtRemrksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemrksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemrksDetail.Location = new System.Drawing.Point(4, 89);
		this.txtRemrksDetail.Name = "txtRemrksDetail";
		this.txtRemrksDetail.Size = new System.Drawing.Size(672, 23);
		this.txtRemrksDetail.TabIndex = 7;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(786, 87);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(66, 26);
		this.btnCancelUpdateDetial.TabIndex = 10;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(715, 87);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(68, 26);
		this.btnUpdateDetail.TabIndex = 9;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(682, 87);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(30, 26);
		this.btnplus.TabIndex = 8;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(540, 21);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 141;
		this.label2.Text = "WareHouse";
		this.cmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.cmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Location = new System.Drawing.Point(540, 41);
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Name = "cmbWarehouse";
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).Size = new System.Drawing.Size(179, 25);
		((System.Windows.Forms.Control)(object)this.cmbWarehouse).TabIndex = 5;
		this.cmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.cmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Location = new System.Drawing.Point(303, 41);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Name = "cmbPackUom";
		((System.Windows.Forms.Control)(object)this.cmbPackUom).Size = new System.Drawing.Size(62, 25);
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbPackUom).TextChanged += new System.EventHandler(cmbPackUom_TextChanged);
		this.CmbVariety.AllowNull = (DefaultableBoolean)1;
		this.CmbVariety.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVariety.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVariety.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbVariety).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVariety).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVariety).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVariety.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Location = new System.Drawing.Point(101, 41);
		((System.Windows.Forms.Control)(object)this.CmbVariety).Name = "CmbVariety";
		((System.Windows.Forms.Control)(object)this.CmbVariety).Size = new System.Drawing.Size(200, 25);
		((System.Windows.Forms.Control)(object)this.CmbVariety).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbVariety).Leave += new System.EventHandler(CmbVariety_Leave);
		this.label60.AutoSize = true;
		this.label60.BackColor = System.Drawing.Color.White;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.Location = new System.Drawing.Point(100, 21);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(67, 15);
		this.label60.TabIndex = 134;
		this.label60.Text = "Item Name";
		this.label57.AutoSize = true;
		this.label57.BackColor = System.Drawing.Color.White;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(368, 21);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(54, 15);
		this.label57.TabIndex = 137;
		this.label57.Text = "Item Qty";
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(368, 42);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(77, 22);
		this.txtqty.TabIndex = 3;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.txtWeightCompared.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeightCompared.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtWeightCompared.Location = new System.Drawing.Point(448, 42);
		this.txtWeightCompared.Name = "txtWeightCompared";
		this.txtWeightCompared.ReadOnly = true;
		this.txtWeightCompared.Size = new System.Drawing.Size(89, 22);
		this.txtWeightCompared.TabIndex = 4;
		this.label58.AutoSize = true;
		this.label58.BackColor = System.Drawing.Color.White;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.Location = new System.Drawing.Point(448, 21);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(46, 15);
		this.label58.TabIndex = 136;
		this.label58.Text = "Weight";
		this.label42.AutoSize = true;
		this.label42.BackColor = System.Drawing.Color.White;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(303, 21);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(58, 15);
		this.label42.TabIndex = 132;
		this.label42.Text = "PackUom";
		this.cmbcityDetail.AllowNull = (DefaultableBoolean)1;
		this.cmbcityDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcityDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcityDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcityDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance79;
		((SpecialBoxBase)((UltraGridBase)this.cmbcityDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcityDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcityDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcityDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcityDetail).Location = new System.Drawing.Point(722, 40);
		((System.Windows.Forms.Control)(object)this.cmbcityDetail).Name = "cmbcityDetail";
		((System.Windows.Forms.Control)(object)this.cmbcityDetail).Size = new System.Drawing.Size(130, 26);
		((System.Windows.Forms.Control)(object)this.cmbcityDetail).TabIndex = 6;
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.White;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(722, 21);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(62, 15);
		this.label20.TabIndex = 84;
		this.label20.Text = "City Name";
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label72);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbVehicleType);
		this.groupBox1.Controls.Add(this.txtSupplierWeight);
		this.groupBox1.Controls.Add(this.txtFactoryWeight);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbgptype);
		this.groupBox1.Controls.Add(this.txtDifferenceWeight);
		this.groupBox1.Controls.Add(this.txtvehno);
		this.groupBox1.Controls.Add(this.txtbilityNo);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label71);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.groupBox1.Controls.Add(this.label70);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.label73);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label78);
		this.groupBox1.Controls.Add(this.ChkBox);
		this.groupBox1.Controls.Add(this.label81);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.label79);
		this.groupBox1.Controls.Add(this.txtgpno);
		this.groupBox1.Controls.Add(this.txtgptypeno);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbOrderType);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtintime);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.txtouttime);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtNetPaid);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(8, 5);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(855, 184);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(5, 75);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(83, 15);
		this.label6.TabIndex = 245;
		this.label6.Text = "Purchase Type";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(105, 127);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(425, 52);
		this.txtRemarks.TabIndex = 6;
		this.txtRemarks.Text = "";
		this.label72.AutoSize = true;
		this.label72.BackColor = System.Drawing.Color.White;
		this.label72.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label72.Location = new System.Drawing.Point(537, 19);
		this.label72.Name = "label72";
		this.label72.Size = new System.Drawing.Size(93, 15);
		this.label72.TabIndex = 232;
		this.label72.Text = "Supplier Weight";
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(5, 19);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(85, 15);
		this.label9.TabIndex = 78;
		this.label9.Text = "Gate Pass Date";
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(105, 98);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 5;
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(5, 104);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(86, 15);
		this.label11.TabIndex = 74;
		this.label11.Text = "Supplier Name";
		this.cmbVehicleType.AllowNull = (DefaultableBoolean)1;
		this.cmbVehicleType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbVehicleType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbVehicleType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance49).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance50).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance50).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance50).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbVehicleType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbVehicleType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Location = new System.Drawing.Point(417, 13);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Name = "cmbVehicleType";
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).Size = new System.Drawing.Size(113, 26);
		((System.Windows.Forms.Control)(object)this.cmbVehicleType).TabIndex = 7;
		this.txtSupplierWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtSupplierWeight.Location = new System.Drawing.Point(650, 15);
		this.txtSupplierWeight.Name = "txtSupplierWeight";
		this.txtSupplierWeight.Size = new System.Drawing.Size(128, 23);
		this.txtSupplierWeight.TabIndex = 11;
		this.txtSupplierWeight.TextChanged += new System.EventHandler(txtSupplierWeight_TextChanged);
		this.txtFactoryWeight.BackColor = System.Drawing.SystemColors.Control;
		this.txtFactoryWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFactoryWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtFactoryWeight.Location = new System.Drawing.Point(650, 43);
		this.txtFactoryWeight.Name = "txtFactoryWeight";
		this.txtFactoryWeight.Size = new System.Drawing.Size(128, 23);
		this.txtFactoryWeight.TabIndex = 12;
		this.txtFactoryWeight.TextChanged += new System.EventHandler(txtFactoryWeight_TextChanged);
		this.cmbgptype.AllowNull = (DefaultableBoolean)1;
		this.cmbgptype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbgptype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbgptype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbgptype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbgptype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbgptype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbgptype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbgptype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbgptype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbgptype).Location = new System.Drawing.Point(105, 41);
		((System.Windows.Forms.Control)(object)this.cmbgptype).Name = "cmbgptype";
		((System.Windows.Forms.Control)(object)this.cmbgptype).Size = new System.Drawing.Size(141, 26);
		((System.Windows.Forms.Control)(object)this.cmbgptype).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbgptype).Leave += new System.EventHandler(cmbgptype_Leave);
		this.txtDifferenceWeight.BackColor = System.Drawing.SystemColors.Control;
		this.txtDifferenceWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDifferenceWeight.Enabled = false;
		this.txtDifferenceWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDifferenceWeight.Location = new System.Drawing.Point(650, 71);
		this.txtDifferenceWeight.Name = "txtDifferenceWeight";
		this.txtDifferenceWeight.Size = new System.Drawing.Size(128, 23);
		this.txtDifferenceWeight.TabIndex = 13;
		this.txtDifferenceWeight.TabStop = false;
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehno.Location = new System.Drawing.Point(417, 43);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.Size = new System.Drawing.Size(113, 23);
		this.txtvehno.TabIndex = 8;
		this.txtbilityNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbilityNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbilityNo.Location = new System.Drawing.Point(417, 71);
		this.txtbilityNo.Name = "txtbilityNo";
		this.txtbilityNo.Size = new System.Drawing.Size(113, 23);
		this.txtbilityNo.TabIndex = 9;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(342, 19);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.label71.AutoSize = true;
		this.label71.BackColor = System.Drawing.Color.White;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label71.Location = new System.Drawing.Point(537, 47);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(88, 15);
		this.label71.TabIndex = 231;
		this.label71.Text = "Factory Weight";
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(650, 153);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(128, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 16;
		this.label70.AutoSize = true;
		this.label70.BackColor = System.Drawing.Color.White;
		this.label70.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label70.Location = new System.Drawing.Point(537, 75);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(104, 15);
		this.label70.TabIndex = 230;
		this.label70.Text = "Difference Weight";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(342, 47);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label73.AutoSize = true;
		this.label73.BackColor = System.Drawing.Color.White;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label73.Location = new System.Drawing.Point(537, 159);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(58, 15);
		this.label73.TabIndex = 229;
		this.label73.Text = "GP Status";
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(537, 131);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(54, 15);
		this.label13.TabIndex = 242;
		this.label13.Text = "OutTime";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(342, 75);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label78.AutoSize = true;
		this.label78.BackColor = System.Drawing.Color.White;
		this.label78.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.Red;
		this.label78.Location = new System.Drawing.Point(637, 18);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(13, 16);
		this.label78.TabIndex = 237;
		this.label78.Text = "*";
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.ChkBox.Location = new System.Drawing.Point(783, 157);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(67, 19);
		this.ChkBox.TabIndex = 17;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label81.AutoSize = true;
		this.label81.BackColor = System.Drawing.Color.White;
		this.label81.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label81.ForeColor = System.Drawing.Color.Red;
		this.label81.Location = new System.Drawing.Point(637, 46);
		this.label81.Name = "label81";
		this.label81.Size = new System.Drawing.Size(13, 16);
		this.label81.TabIndex = 236;
		this.label81.Text = "*";
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(5, 47);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(88, 15);
		this.label8.TabIndex = 76;
		this.label8.Text = "Gate Pass Type ";
		this.label79.AutoSize = true;
		this.label79.BackColor = System.Drawing.Color.White;
		this.label79.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label79.ForeColor = System.Drawing.Color.Red;
		this.label79.Location = new System.Drawing.Point(637, 74);
		this.label79.Name = "label79";
		this.label79.Size = new System.Drawing.Size(13, 16);
		this.label79.TabIndex = 235;
		this.label79.Text = "*";
		this.txtgpno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgpno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgpno.Location = new System.Drawing.Point(250, 15);
		this.txtgpno.Name = "txtgpno";
		this.txtgpno.ReadOnly = true;
		this.txtgpno.Size = new System.Drawing.Size(85, 23);
		this.txtgpno.TabIndex = 1;
		this.txtgpno.TabStop = false;
		this.txtgptypeno.BackColor = System.Drawing.Color.White;
		this.txtgptypeno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgptypeno.Enabled = false;
		this.txtgptypeno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgptypeno.Location = new System.Drawing.Point(250, 43);
		this.txtgptypeno.Name = "txtgptypeno";
		this.txtgptypeno.ReadOnly = true;
		this.txtgptypeno.Size = new System.Drawing.Size(85, 23);
		this.txtgptypeno.TabIndex = 3;
		this.txtgptypeno.TabStop = false;
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(105, 15);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(141, 23);
		this.txtgpdate.TabIndex = 0;
		this.CmbOrderType.AllowNull = (DefaultableBoolean)1;
		this.CmbOrderType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbOrderType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbOrderType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbOrderType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Location = new System.Drawing.Point(105, 70);
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Name = "CmbOrderType";
		((System.Windows.Forms.Control)(object)this.CmbOrderType).Size = new System.Drawing.Size(230, 25);
		((System.Windows.Forms.Control)(object)this.CmbOrderType).TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(537, 104);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 85;
		this.label3.Text = "InTime";
		this.txtintime.CustomFormat = "hh:mm tt";
		this.txtintime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtintime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtintime.Location = new System.Drawing.Point(650, 100);
		this.txtintime.Name = "txtintime";
		this.txtintime.Size = new System.Drawing.Size(128, 23);
		this.txtintime.TabIndex = 14;
		this.txtintime.TabStop = false;
		this.txtintime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.White;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(5, 134);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(93, 15);
		this.label22.TabIndex = 90;
		this.label22.Text = "Remarks Header";
		this.txtouttime.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
		this.txtouttime.CalendarFont = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtouttime.CalendarMonthBackground = System.Drawing.SystemColors.Control;
		this.txtouttime.CustomFormat = "hh:mm tt";
		this.txtouttime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtouttime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtouttime.Location = new System.Drawing.Point(650, 127);
		this.txtouttime.Name = "txtouttime";
		this.txtouttime.Size = new System.Drawing.Size(128, 23);
		this.txtouttime.TabIndex = 15;
		this.txtouttime.TabStop = false;
		this.txtouttime.Value = new System.DateTime(2020, 8, 27, 9, 36, 0, 0);
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(342, 104);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(70, 15);
		this.label1.TabIndex = 92;
		this.label1.Text = "Bilty Freight";
		this.txtNetPaid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetPaid.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetPaid.Location = new System.Drawing.Point(417, 100);
		this.txtNetPaid.Name = "txtNetPaid";
		this.txtNetPaid.Size = new System.Drawing.Size(113, 23);
		this.txtNetPaid.TabIndex = 10;
		this.txtNetPaid.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetPaid_KeyPress);
		this.txtWeighBridgeSlipNo.BackColor = System.Drawing.SystemColors.Control;
		this.txtWeighBridgeSlipNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtWeighBridgeSlipNo.Enabled = false;
		this.txtWeighBridgeSlipNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeighBridgeSlipNo.Location = new System.Drawing.Point(995, 61);
		this.txtWeighBridgeSlipNo.Name = "txtWeighBridgeSlipNo";
		this.txtWeighBridgeSlipNo.Size = new System.Drawing.Size(128, 23);
		this.txtWeighBridgeSlipNo.TabIndex = 245;
		this.txtWeighBridgeSlipNo.TabStop = false;
		this.txtWeighBridgeSlipNo.Visible = false;
		this.lblDiffRemarks.AutoSize = true;
		this.lblDiffRemarks.BackColor = System.Drawing.Color.White;
		this.lblDiffRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblDiffRemarks.Location = new System.Drawing.Point(881, 112);
		this.lblDiffRemarks.Name = "lblDiffRemarks";
		this.lblDiffRemarks.Size = new System.Drawing.Size(95, 15);
		this.lblDiffRemarks.TabIndex = 247;
		this.lblDiffRemarks.Text = "Wt Diff Remarks";
		this.lblDiffRemarks.Visible = false;
		this.txtWeightDiffRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtWeightDiffRemarks.Location = new System.Drawing.Point(995, 121);
		this.txtWeightDiffRemarks.Multiline = true;
		this.txtWeightDiffRemarks.Name = "txtWeightDiffRemarks";
		this.txtWeightDiffRemarks.Size = new System.Drawing.Size(128, 22);
		this.txtWeightDiffRemarks.TabIndex = 246;
		this.txtWeightDiffRemarks.Visible = false;
		this.cmbWeighBridge.AllowNull = (DefaultableBoolean)1;
		this.cmbWeighBridge.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWeighBridge.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWeighBridge.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWeighBridge).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWeighBridge.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Location = new System.Drawing.Point(995, 88);
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Name = "cmbWeighBridge";
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Size = new System.Drawing.Size(128, 26);
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).TabIndex = 224;
		((System.Windows.Forms.Control)(object)this.cmbWeighBridge).Visible = false;
		this.label74.AutoSize = true;
		this.label74.BackColor = System.Drawing.Color.White;
		this.label74.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label74.ForeColor = System.Drawing.Color.Red;
		this.label74.Location = new System.Drawing.Point(979, 93);
		this.label74.Name = "label74";
		this.label74.Size = new System.Drawing.Size(13, 16);
		this.label74.TabIndex = 241;
		this.label74.Text = "*";
		this.label74.Visible = false;
		this.label75.AutoSize = true;
		this.label75.BackColor = System.Drawing.Color.White;
		this.label75.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.ForeColor = System.Drawing.Color.Red;
		this.label75.Location = new System.Drawing.Point(979, 64);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(13, 16);
		this.label75.TabIndex = 240;
		this.label75.Text = "*";
		this.label75.Visible = false;
		this.label67.AutoSize = true;
		this.label67.BackColor = System.Drawing.Color.White;
		this.label67.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label67.Location = new System.Drawing.Point(881, 65);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(88, 15);
		this.label67.TabIndex = 233;
		this.label67.Text = "Weight Slip No";
		this.label67.Visible = false;
		this.label66.AutoSize = true;
		this.label66.BackColor = System.Drawing.Color.White;
		this.label66.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label66.Location = new System.Drawing.Point(881, 94);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(115, 15);
		this.label66.TabIndex = 234;
		this.label66.Text = "Weigh Bridge Status";
		this.label66.Visible = false;
		this.CmbOrderno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.CmbOrderno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbOrderno.Location = new System.Drawing.Point(995, 150);
		this.CmbOrderno.Name = "CmbOrderno";
		this.CmbOrderno.Size = new System.Drawing.Size(128, 22);
		this.CmbOrderno.TabIndex = 119;
		this.CmbOrderno.Visible = false;
		this.CmbOrderno.Leave += new System.EventHandler(CmbOrderno_Leave);
		this.txtPackUnit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtPackUnit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackUnit.Location = new System.Drawing.Point(995, 33);
		this.txtPackUnit.Name = "txtPackUnit";
		this.txtPackUnit.Size = new System.Drawing.Size(128, 22);
		this.txtPackUnit.TabIndex = 130;
		this.txtPackUnit.Visible = false;
		this.label59.AutoSize = true;
		this.label59.BackColor = System.Drawing.Color.White;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.Location = new System.Drawing.Point(881, 9);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(70, 15);
		this.label59.TabIndex = 135;
		this.label59.Text = "Bilty Freight";
		this.label59.Visible = false;
		this.txtfreight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfreight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfreight.Location = new System.Drawing.Point(995, 5);
		this.txtfreight.Name = "txtfreight";
		this.txtfreight.Size = new System.Drawing.Size(128, 22);
		this.txtfreight.TabIndex = 126;
		this.txtfreight.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 33);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1090, 29);
		this.panel3.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(206, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Inward GatePass Trade ";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1090, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnNew, this.btnUpdate, this.btnSave, this.printToolStripButton, this.btnAttachment, this.toolStripButton1, this.toolStripSeparator1, this.btnLoadOrderMainDetail, this.toolStripSeparator2, this.btnLoaderView2 });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1090, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(100, 24);
		this.printToolStripButton.Text = "1601-&Print";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnLoadOrderMainDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOrderMainDetail.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadOrderMainDetail.Name = "btnLoadOrderMainDetail";
		this.btnLoadOrderMainDetail.Size = new System.Drawing.Size(225, 24);
		this.btnLoadOrderMainDetail.Text = "Load Purchase Order Main Detail";
		this.btnLoadOrderMainDetail.Click += new System.EventHandler(btnLoadOrderMainDetail_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnLoaderView2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoaderView2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoaderView2.Name = "btnLoaderView2";
		this.btnLoaderView2.Size = new System.Drawing.Size(183, 24);
		this.btnLoaderView2.Text = "Load Purchase Order Main";
		this.btnLoaderView2.Click += new System.EventHandler(btnLoaderView2_Click);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailOpenGp);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.GridMainOpenGP);
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Location = new System.Drawing.Point(4, 4);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1094, 606);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Open Gate Passes";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.GridDetailOpenGp.AllowEdit = (InheritableBoolean)2;
		GridDetailOpenGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailOpenGp.DesignTimeLayout = GridDetailOpenGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailOpenGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailOpenGp.DynamicFiltering = true;
		this.GridDetailOpenGp.FilterMode = (FilterMode)2;
		this.GridDetailOpenGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailOpenGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailOpenGp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridDetailOpenGp).Location = new System.Drawing.Point(0, 401);
		((System.Windows.Forms.Control)(object)this.GridDetailOpenGp).Name = "GridDetailOpenGp";
		((ScrollableBase)this.GridDetailOpenGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailOpenGp).Size = new System.Drawing.Size(1094, 205);
		((System.Windows.Forms.Control)(object)this.GridDetailOpenGp).TabIndex = 49;
		this.GridDetailOpenGp.TotalRow = (InheritableBoolean)1;
		this.GridDetailOpenGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailOpenGp).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label15);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 375);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1094, 26);
		this.panel9.TabIndex = 48;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(3, 4);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(238, 17);
		this.label15.TabIndex = 23;
		this.label15.Text = "Detail Of the Above Selected Row";
		this.GridMainOpenGP.AllowEdit = (InheritableBoolean)2;
		GridMainOpenGP_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridMainOpenGP.DesignTimeLayout = GridMainOpenGP_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridMainOpenGP).Dock = System.Windows.Forms.DockStyle.Top;
		this.GridMainOpenGP.DynamicFiltering = true;
		this.GridMainOpenGP.FilterMode = (FilterMode)2;
		this.GridMainOpenGP.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridMainOpenGP).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridMainOpenGP.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridMainOpenGP).Location = new System.Drawing.Point(0, 53);
		((System.Windows.Forms.Control)(object)this.GridMainOpenGP).Name = "GridMainOpenGP";
		((ScrollableBase)this.GridMainOpenGP).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridMainOpenGP).Size = new System.Drawing.Size(1094, 322);
		((System.Windows.Forms.Control)(object)this.GridMainOpenGP).TabIndex = 46;
		this.GridMainOpenGP.TotalRow = (InheritableBoolean)1;
		this.GridMainOpenGP.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridMainOpenGP).VisualStyle = (VisualStyle)4;
		this.GridMainOpenGP.ColumnButtonClick += new ColumnActionEventHandler(GridMainOpenGP_ColumnButtonClick);
		this.GridMainOpenGP.SelectionChanged += new System.EventHandler(GridMainOpenGP_SelectionChanged);
		this.GridMainOpenGP.LinkClicked += new ColumnActionEventHandler(GridMainOpenGP_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GridMainOpenGP).DoubleClick += new System.EventHandler(GridMainOpenGP_DoubleClick);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.toolStrip3);
		this.panel5.Controls.Add(this.label12);
		this.panel5.Controls.Add(this.ctrlGrdBar3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1094, 53);
		this.panel5.TabIndex = 1;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAllOpenGP });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1094, 27);
		this.toolStrip3.TabIndex = 23;
		this.toolStrip3.Text = "toolStrip3";
		this.btnLoadAllOpenGP.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAllOpenGP.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAllOpenGP.Name = "btnLoadAllOpenGP";
		this.btnLoadAllOpenGP.Size = new System.Drawing.Size(77, 24);
		this.btnLoadAllOpenGP.Text = "&LoadAll";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(6, 31);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(216, 19);
		this.label12.TabIndex = 0;
		this.label12.Text = "Gate Pass Inward History";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1048, 27);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GridMainOpenGP;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(42, 24);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1094, 606);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel6);
		this.panel8.Controls.Add(this.panel13);
		this.panel8.Controls.Add(this.panel12);
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 55);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1090, 549);
		this.panel8.TabIndex = 1;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistory);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 386);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1090, 163);
		this.panel6.TabIndex = 53;
		this.grdDetailHistory.AllowEdit = (InheritableBoolean)2;
		grdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailHistory.DesignTimeLayout = grdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailHistory.DynamicFiltering = true;
		this.grdDetailHistory.FilterMode = (FilterMode)2;
		this.grdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Name = "grdDetailHistory";
		((ScrollableBase)this.grdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Size = new System.Drawing.Size(1090, 163);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).TabIndex = 48;
		this.grdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label4);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 360);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1090, 26);
		this.panel13.TabIndex = 52;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(238, 17);
		this.label4.TabIndex = 24;
		this.label4.Text = "Detail Of the Above Selected Row";
		this.panel12.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 73);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1090, 287);
		this.panel12.TabIndex = 51;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1090, 287);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 45;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1090, 73);
		this.panel11.TabIndex = 50;
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label18);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.label24);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox3.Controls.Add(this.txtToDateHistory);
		this.groupBox3.Controls.Add(this.txtFromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1089, 69);
		this.groupBox3.TabIndex = 49;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(981, 36);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(814, 36);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(895, 36);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(748, 32);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 8;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.btnShowHistory.Leave += new System.EventHandler(btnShowHistory_Leave);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(486, 15);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(51, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Supplier";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(380, 15);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(64, 15);
		this.label16.TabIndex = 86;
		this.label16.Text = "To Doc No";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(140, 15);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 84;
		this.label17.Text = "To Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(380, 34);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(104, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(9, 15);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(63, 15);
		this.label24.TabIndex = 83;
		this.label24.Text = "From Date";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(275, 15);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(79, 15);
		this.label26.TabIndex = 85;
		this.label26.Text = "From Doc No";
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(275, 34);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(104, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance98;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance102;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(486, 32);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(143, 34);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.ShowCheckBox = true;
		this.txtToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.txtToDateHistory.TabIndex = 1;
		this.txtFromDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateHistory.Location = new System.Drawing.Point(9, 34);
		this.txtFromDateHistory.Name = "txtFromDateHistory";
		this.txtFromDateHistory.ShowCheckBox = true;
		this.txtFromDateHistory.Size = new System.Drawing.Size(132, 23);
		this.txtFromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Controls.Add(this.label14);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1090, 53);
		this.panel7.TabIndex = 0;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNewHistory, this.btnRefreshHistory, this.btnloadall });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1090, 27);
		this.toolStrip2.TabIndex = 23;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 10f);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(61, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		this.btnloadall.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnloadall.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnloadall.Name = "btnloadall";
		this.btnloadall.Size = new System.Drawing.Size(77, 24);
		this.btnloadall.Text = "&LoadAll";
		this.btnloadall.Visible = false;
		this.btnloadall.Click += new System.EventHandler(btnloadall_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(6, 31);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(216, 19);
		this.label14.TabIndex = 0;
		this.label14.Text = "Gate Pass Inward History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1044, 27);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 24);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1102, 632);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "InwardGatePassTrade";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InwardGatePassTrade_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderNoDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariety).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcityDetail).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbVehicleType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbgptype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbOrderType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWeighBridge).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridDetailOpenGp).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridMainOpenGP).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel12.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel11.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
