using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.FixedAssets;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.FixedAssets;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.PurchaseTrading;

public class GoodsReceivingNote_Engr : Form
{
	private Rightsobjects formRights = new Rightsobjects();

	private Attachment AT = new Attachment();

	private int GPID;

	public int RecId;

	private bool UpdateMode;

	private int BillCalculateTypeId;

	private DataTable dtdetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtUOM = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtCity = new DataTable();

	private DataTable dtSupplier = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtCondition = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	public string GRNS = string.Empty;

	public bool SupplySelected;

	private LoadPurchaseOrderForGrn LoadPo;

	private DataTable dtsupcustomer = new DataTable();

	private DataTable dtPendingGatePass = new DataTable();

	private DataTable dtOutStandingOrdersAndParties = new DataTable();

	private DataTable dtpo = new DataTable();

	private DataTable dtAsset = new DataTable();

	private frmLoadPendingReturnableGdnForReceiving Loader;

	private bool SubsidiaryAccountAllownOnVouchers;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

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

	private Label label24;

	private Label label25;

	private Label label21;

	private Label label19;

	private TextBox txtbltyno;

	private Label label7;

	private TextBox txtvehno;

	internal UltraCombo CmbTransport;

	private Label label48;

	private Label label26;

	private TextBox txtcarramount;

	private Label label20;

	private TextBox txtsuppwt;

	private Label label6;

	private TextBox txtfctwt;

	private Label label11;

	private TextBox txtwtdiff;

	internal UltraCombo combrefprty;

	internal UltraCombo combstockprty;

	internal UltraCombo combsupplier;

	private Label label2;

	private Label label12;

	private Label label10;

	private Label label8;

	private TextBox txtsuppref;

	private Label label4;

	private TextBox txtrefno;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label14;

	private ToolStripButton toolStripButton1;

	private Label label15;

	private TextBox txtDeliverTerm;

	private CheckBox ChkBox;

	internal UltraCombo combbrnch;

	internal UltraCombo combproject;

	private ToolStripButton btnLoadPo;

	internal GridEX grd;

	internal GridEX grdDetailHistory;

	internal GridEX GrdHistory;

	private ToolStripButton btnIGP;

	internal UltraCombo combvehtyp;

	private Label label1;

	internal UltraCombo CmbDeliveryTerm;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label5;

	internal GridEX GrdPendingGatePass;

	private GroupBox groupBox1;

	private Label label22;

	internal UltraCombo cmbOrderNo;

	private TextBox combgatepass;

	private Label label27;

	internal UltraCombo CmbType;

	private Label label28;

	private DateTimePicker datReturnableDate;

	private TextBox txtremarks;

	private Label label29;

	internal UltraCombo CmbRequestedBy;

	private Panel DetailPanel;

	private GroupBox groupBox2;

	internal UltraCombo CmbJobLot;

	private Label label30;

	private TextBox txtRemarksDetail;

	internal UltraCombo CmbWarehouse;

	private Label label31;

	private Label label32;

	private Label label33;

	private Button btnCancelDetail;

	private TextBox txtItemQty;

	private Button Add;

	internal UltraCombo cmbAssetsRef;

	private Label label34;

	internal UltraCombo cmbUOM;

	private Label label35;

	internal UltraCombo cmbItem;

	private Label label44;

	private Button btnUpdateDetail;

	internal UltraCombo CmbCondition;

	private Label label3;

	private ToolStripButton btnPendingGdn;

	internal UltraCombo CmbCity;

	private Label label36;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnResetHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel7;

	private GroupBox groupBox3;

	private Button btnShow;

	private Label label9;

	private Label label16;

	private Label label37;

	private Label label38;

	private Label label39;

	private Label label40;

	internal UltraCombo CmbRequestedByHistory;

	internal UltraCombo CmbCustomerHistory;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel15;

	private Label label41;

	private Panel panel9;

	private Panel panel11;

	private ToolStripButton btnUpdateDetailRowsData;

	private Panel panel10;

	private Panel panel12;

	private Panel panel14;

	private Panel panel13;

	private RadioButton rdapproved;

	private RadioButton rdmodify;

	private RadioButton rddoc;

	private RadioButton rdentry;

	public GoodsReceivingNote_Engr(UserAccount user)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 625);
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnSave.Enabled = formRights.DoHaveSaveRight;
			btnUpdate.Enabled = formRights.DoHaveUpdateRights;
			printToolStripButton.Enabled = formRights.DoHavePrintRights;
			dtpo.Columns.Add("Id");
			dtpo.Columns.Add("No");
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("GdnId", typeof(int));
			dtdetail.Columns.Add("GdnDetailId", typeof(int));
			dtdetail.Columns.Add("OrderId", typeof(int));
			dtdetail.Columns.Add("OrderDetailId", typeof(int));
			dtdetail.Columns.Add("OrderNo", typeof(int));
			dtdetail.Columns.Add("WareHouse");
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackUomId", typeof(int));
			dtdetail.Columns.Add("PackUom", typeof(string));
			dtdetail.Columns.Add("Remarks");
			dtdetail.Columns.Add("JobLotId", typeof(int));
			dtdetail.Columns.Add("ItemQty", typeof(double));
			dtdetail.Columns.Add("AssetId", typeof(int));
			dtdetail.Columns.Add("ConditionId", typeof(int));
			dtdetail.Columns.Add("City", typeof(int));
			dtdetail.Columns.Add("WareHouseFromId", typeof(int));
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			dtwh.Columns.Add("Id");
			dtwh.Columns.Add("WareHouseName");
			GenerateCode();
			BranchFill();
			ProjectFill();
			DeliveryTerm();
			AssetFill();
			TransportFill();
			TypeFill();
			RequestedByFill();
			SupplierNameFilll();
			VehicleTypesFill();
			GetOutStandingOrderAndParties();
			ConditionFill();
			WarehouseFill();
			JobLotFill();
			CityFill();
			ItemFillWithOutOrder();
			GetPendingGatePass();
			((UltraGridBase)CmbType).Rows[0].Activate();
			CmbType_Leave(null, null);
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			ChkBox.Checked = true;
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int DocNo = InvGrn.GenerateInvGrnCode(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 1618
			});
			txtdocno.Text = DocNo.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryComboFill()
	{
		try
		{
			DataTable dtSupplier = new DataTable();
			dtSupplier.Columns.Add("Id", typeof(int));
			dtSupplier.Columns.Add("Supplier", typeof(string));
			DataTable dtRequestedBy = new DataTable();
			dtRequestedBy.Columns.Add("Id", typeof(int));
			dtRequestedBy.Columns.Add("RequestedBy", typeof(string));
			DataTable dt = InvGrn.GetDataForDropDownFromGrn(UserAccount.OrganizationId, UserAccount.CompanyId, "1618", (string)null, (string)null, 0);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtSupplier.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "RequestedBy")
					{
						dtRequestedBy.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtSupplier, CmbCustomerHistory, "Id", "Supplier", "Supplier Name", ZeroIndex: false);
			DDL.BindDDL(dtRequestedBy, CmbRequestedByHistory, "Id", "RequestedBy", "Requested By", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AssetFill()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			dtAsset.Rows.Clear();
			int faId = Conversion.ToInt(cmbAssetsRef.Value);
			FixedAssetsRegister fixedAssets = new FixedAssetsRegister();
			fixedAssets.OrganizationId = UserAccount.OrganizationId;
			fixedAssets.CompanyId = UserAccount.CompanyId;
			dtAsset = FixedAssetsRegister.GetAll(fixedAssets);
			if (dtAsset.Rows.Count > 0)
			{
				DDL.BindDDL(dtAsset, cmbAssetsRef, "Id", "AssetName", "Machine/Product", ZeroIndex: true);
				((UltraGridBase)cmbAssetsRef).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (faId > 0)
			{
				if (dtAsset.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == faId))
				{
					cmbAssetsRef.Value = faId;
				}
				else
				{
					((Control)(object)cmbAssetsRef).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RequestedByFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbRequestedBy.Value);
			DataTable dt = CommonServices.GetLookupsByTypeIdDt(13);
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbRequestedBy, "Id", "LookupName", "Requested By", true);
			}
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbRequestedBy.Value = Id;
				}
				else
				{
					((Control)(object)CmbRequestedBy).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("LookUpName");
			DataTable dtType = InvLookUp.GetLookUpsERP(1);
			if (dtType.Rows.Count > 0)
			{
				for (int i = 0; i < dtType.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtType.Rows[i]["Id"]) == 2 || Conversion.ToInt(dtType.Rows[i]["Id"]) == 6)
					{
						dt.Rows.Add(dtType.Rows[i]["Id"], dtType.Rows[i]["LookUpName"]);
					}
				}
			}
			DDL.BindDDL(dt, CmbType, "Id", "LookUpName", "Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ConditionFill()
	{
		try
		{
			dtCondition.Rows.Clear();
			dtCondition = InvLookUp.GetLookUpsERP(2);
			if (dtCondition.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCondition, CmbCondition, "Id", "LookUpName", "Condition", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetOutStandingOrderAndParties(int RecId = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.Id = RecId;
			dtOutStandingOrdersAndParties = PurchaseOrder.GetOutstandOrdersAndPartiesForReceivingGrn(obj);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemFillWithOutOrder()
	{
		try
		{
			dtItem.Rows.Clear();
			dtItem = CommonServices.ItemGetForComboServiceBindForEngr();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SupplierNameFilll()
	{
		try
		{
			dtSupplier.Rows.Clear();
			if (SubsidiaryAccountAllownOnVouchers)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers();
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TransportFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTransport.Value);
			if (SubsidiaryAccountAllownOnVouchers)
			{
				DataTable dtSupplier = CommonServices.GetVendorsAndCustomers(1);
				if (dtSupplier.Rows.Count <= 0)
				{
					return;
				}
				DDL.BindDDL(dtSupplier, CmbTransport, "Id", "CompanyName", "Transporter Name", ZeroIndex: true);
				((UltraGridBase)CmbTransport).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				if (Id > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbTransport.Value = Id;
					}
					else
					{
						((Control)(object)CmbTransport).Text = string.Empty;
					}
				}
				return;
			}
			DataTable dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtAccountlst.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < dtAccountlst.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 2 && Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 15 && Conversion.ToInt(dtAccountlst.Rows[i]["AccountTypeId"]) != 11)
				{
					dt.Rows.Add(dtAccountlst.Rows[i]["Id"], dtAccountlst.Rows[i]["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTransport, "Id", "AccountTitle", "Transporter Name", ZeroIndex: false);
			}
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbTransport.Value = Id;
				}
				else
				{
					((Control)(object)CmbTransport).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				DDL.BindDDL(dt, combvehtyp, "Id", "VehicleDescription", "Vehicle Type", ZeroIndex: false);
				((UltraGridBase)combvehtyp).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void JobLotFill()
	{
		try
		{
			dtjob.Rows.Clear();
			dtjob = CommonServices.JobLotGetAllService();
			DDL.BindDDLNew(dtjob, CmbJobLot, "Id", "JobLotDescription", "Job Lot", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void WarehouseFill()
	{
		try
		{
			dtwh.Rows.Clear();
			dtwh = CommonServices.getActiveWareHouse();
			DDL.BindDDLNew(dtwh, CmbWarehouse, "Id", "WareHouseName", "Warehouse", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CityFill()
	{
		try
		{
			dtCity.Rows.Clear();
			dtCity = CommonServices.CityGetAllService();
			DDL.BindDDLNew(dtCity, CmbCity, "Id", "CityName", "City Name", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, CmbDeliveryTerm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)CmbDeliveryTerm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbDeliveryTerm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			GPID = 0;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtcarramount.Text = string.Empty;
			((Control)(object)CmbType).Enabled = true;
			((Control)(object)combsupplier).Enabled = true;
			((Control)(object)cmbOrderNo).Enabled = true;
			((Control)(object)CmbRequestedBy).Enabled = true;
			((Control)(object)CmbType).Text = string.Empty;
			((Control)(object)combsupplier).Text = string.Empty;
			((Control)(object)cmbOrderNo).Text = string.Empty;
			datReturnableDate.Value = DateTime.Now;
			((Control)(object)CmbRequestedBy).Text = string.Empty;
			txtsuppref.Text = string.Empty;
			txtrefno.Text = string.Empty;
			((Control)(object)combvehtyp).Text = string.Empty;
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			combgatepass.Text = string.Empty;
			((Control)(object)combstockprty).Text = string.Empty;
			((Control)(object)combrefprty).Text = string.Empty;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			txtwtdiff.Text = string.Empty;
			txtremarks.Text = string.Empty;
			dtdetail.Clear();
			grd.DataSource = dtdetail;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			GenerateCode();
			DocDate.Focus();
			((UltraGridBase)cmbOrderNo).DataSource = null;
			((Control)(object)cmbOrderNo).Text = string.Empty;
			GetPendingGatePass();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CompanyFill()
	{
	}

	private void BranchFill()
	{
	}

	private void ProjectFill()
	{
	}

	private void cmbOrderNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbOrderNo).ActiveRow != null)
			{
				FillGridDetailByOrderNo(Conversion.ToInt(cmbOrderNo.Value), GPID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbType_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbType.Value) == 1)
			{
				GetOutStandingParties();
				btnPendingGdn.Visible = false;
				DetailPanel.Visible = true;
			}
			else
			{
				((Control)(object)cmbOrderNo).Text = string.Empty;
				((UltraGridBase)cmbOrderNo).DataSource = null;
				int Id = Conversion.ToInt(combsupplier.Value);
				((UltraGridBase)combsupplier).DataSource = null;
				DDL.BindDDL(dtSupplier, combsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
				if (Id > 0)
				{
					if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						combsupplier.Value = Id;
					}
					else
					{
						((Control)(object)combsupplier).Text = string.Empty;
					}
				}
				int ItemId = Conversion.ToInt(cmbItem.Value);
				((Control)(object)cmbItem).Text = string.Empty;
				((UltraGridBase)cmbItem).DataSource = null;
				DDL.BindDDL(dtItem, cmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				if (ItemId > 0)
				{
					if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItemId))
					{
						cmbItem.Value = ItemId;
					}
					else
					{
						((Control)(object)cmbItem).Text = string.Empty;
					}
				}
				if (Conversion.ToInt(CmbType.Value) == 2 || Conversion.ToInt(CmbType.Value) == 6)
				{
					btnPendingGdn.Visible = true;
					DetailPanel.Visible = false;
				}
				else
				{
					btnPendingGdn.Visible = false;
					DetailPanel.Visible = true;
				}
			}
			WareHouseToShowHide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetOutStandingParties()
	{
		try
		{
			int Id = Conversion.ToInt(combsupplier.Value);
			DataTable dt = new DataTable();
			if (Conversion.ToInt(CmbType.Value) == 1)
			{
				if (dtOutStandingOrdersAndParties.Rows.Count > 0)
				{
					dt = (from row in dtOutStandingOrdersAndParties.AsEnumerable()
						group row by row.Field<int>("Id") into @group
						select @group.First()).CopyToDataTable();
				}
				else
				{
					((Control)(object)combsupplier).Text = string.Empty;
					((UltraGridBase)combsupplier).DataSource = null;
					((Control)(object)cmbOrderNo).Text = null;
					((UltraGridBase)cmbOrderNo).DataSource = null;
				}
				DDL.BindDDLNew(dt, combsupplier, "Id", "CompanyName", "Customer Name", true);
				return;
			}
			((Control)(object)cmbOrderNo).Text = string.Empty;
			((UltraGridBase)cmbOrderNo).DataSource = null;
			((UltraGridBase)combsupplier).DataSource = null;
			DDL.BindDDL(dtSupplier, combsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					combsupplier.Value = Id;
				}
				else
				{
					((Control)(object)combsupplier).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetailRowsData_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() <= 0 || MessageBox.Show(" Are you sure to Update? This will take first row data and Update remaining rows and if there is another warehouse,joblot,ItemCondition and City then that data will be lost.", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			GridEXRow r = grd.GetRow(0);
			GridEXRow[] rows;
			if (Conversion.ToInt(r.Cells["WareHouse"].Value) > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow item4 in rows)
				{
					if (r.RowIndex != item4.RowIndex)
					{
						item4.BeginEdit();
						item4.Cells["WareHouse"].Value = Conversion.ToInt(r.Cells["WareHouse"].Value);
						item4.EndEdit();
					}
				}
			}
			if (Conversion.ToInt(r.Cells["JobLotId"].Value) > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow item3 in rows)
				{
					if (r.RowIndex != item3.RowIndex)
					{
						item3.BeginEdit();
						item3.Cells["JobLotId"].Value = Conversion.ToInt(r.Cells["JobLotId"].Value);
						item3.EndEdit();
					}
				}
			}
			if (Conversion.ToInt(r.Cells["ConditionId"].Value) > 0)
			{
				rows = grd.GetRows();
				foreach (GridEXRow item2 in rows)
				{
					if (r.RowIndex != item2.RowIndex)
					{
						item2.BeginEdit();
						item2.Cells["ConditionId"].Value = Conversion.ToInt(r.Cells["ConditionId"].Value);
						item2.EndEdit();
					}
				}
			}
			if (Conversion.ToInt(r.Cells["City"].Value) <= 0)
			{
				return;
			}
			rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (r.RowIndex != item.RowIndex)
				{
					item.BeginEdit();
					item.Cells["City"].Value = Conversion.ToInt(r.Cells["City"].Value);
					item.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbTransport).ActiveRow == null && Conversion.ToDouble((object)txtcarramount.Text) != 0.0)
		{
			MessageBox.Show("Transporter Field is Required");
			CmbTransport.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTransport).ActiveRow != null && Conversion.ToDouble((object)txtcarramount.Text) == 0.0)
		{
			MessageBox.Show("Freight Amount Field is Required");
			txtcarramount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbDeliveryTerm).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Field is Required");
			CmbDeliveryTerm.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbType.Value) <= 0)
		{
			MessageBox.Show("Type Field Required");
			CmbType.Focus();
			return false;
		}
		if (((Control)(object)combsupplier).Text.Trim() == "")
		{
			MessageBox.Show("Supplier Field is Required");
			combsupplier.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbType.Value) == 1 && ((UltraGridBase)cmbOrderNo).ActiveRow == null)
		{
			MessageBox.Show("Order No Field is Required");
			cmbOrderNo.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbRequestedBy.Value) <= 0)
		{
			MessageBox.Show("RequestedBy Field Required");
			CmbRequestedBy.Focus();
			return false;
		}
		if (combgatepass.Text.Trim() == "")
		{
			MessageBox.Show("Gate Pass Field is Required");
			combgatepass.Focus();
			return false;
		}
		if (((UltraGridBase)combvehtyp).ActiveRow == null)
		{
			MessageBox.Show("Vehicle Type Field is Required");
			combvehtyp.Focus();
			return false;
		}
		if (txtvehno.Text == "")
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtvehno.Focus();
			return false;
		}
		return true;
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			SupplierNameFilll();
			TransportFill();
			VehicleTypesFill();
			GetPendingGatePass();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void InvFrmGRN_Qty_FormClosing(object sender, FormClosingEventArgs e)
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

	private void FillGridDetailByOrderNo(int OrderId, int GPID)
	{
		try
		{
			if (OrderId != 0 && GPID != 0)
			{
				if (dtPendingGatePass.Rows.Count > 0)
				{
					dtdetail.Clear();
					DataTable resultRows = (from row in dtPendingGatePass.AsEnumerable()
						where row.Field<int>("PurchaseOrderId") == Conversion.ToInt((object)OrderId) && row.Field<int>("GpId") == Conversion.ToInt((object)GPID)
						select row).CopyToDataTable();
					for (int i = 0; i < resultRows.Rows.Count; i++)
					{
						dtdetail.Rows.Add(0, resultRows.Rows[i]["GpDetailId"], resultRows.Rows[i]["PurchaseOrderId"], resultRows.Rows[i]["PurchaseOrderDetailId"], resultRows.Rows[i]["PurchaseOrderNo"], resultRows.Rows[i]["ItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["ItemUOMId"], resultRows.Rows[i]["PackUomEquivalent"], resultRows.Rows[i]["ItemQty"], resultRows.Rows[i]["Weight"], 0, resultRows.Rows[i]["Weight"], resultRows.Rows[i]["WareHouseId"], 0, resultRows.Rows[i]["CityId"]);
					}
					grd.DataSource = dtdetail;
					grd.RetrieveStructure();
					grdSettings();
				}
				else
				{
					grd.ClearStructure();
				}
			}
			else
			{
				grd.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void grdSettings()
	{
		try
		{
			if (Conversion.ToInt(grd.GetRow(0).Cells["GdnId"].Value) > 0)
			{
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)1;
				grd.RootTable.Columns["OrderNo"].Visible = false;
			}
			else
			{
				grd.RootTable.Columns["ItemQty"].EditType = (EditType)0;
				grd.RootTable.Columns["ItemQty"].Visible = true;
			}
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["OrderDetailId"].Visible = false;
			grd.RootTable.Columns["GdnId"].Visible = false;
			grd.RootTable.Columns["GdnDetailId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["WareHouseFromId"].Visible = false;
			grd.RootTable.Columns["Id"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnId"].EditType = (EditType)0;
			grd.RootTable.Columns["GdnDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderNo"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["PackUom"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseFromId"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouse"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouse"].HasValueList = true;
			grd.RootTable.Columns["WareHouse"].LimitToList = true;
			grd.RootTable.Columns["WareHouse"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouse"].Caption = "WareHouse";
			grd.RootTable.Columns["WareHouse"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["WareHouseFromId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouseFromId"].HasValueList = true;
			grd.RootTable.Columns["WareHouseFromId"].LimitToList = true;
			grd.RootTable.Columns["WareHouseFromId"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouseFromId"].Caption = "WareHouse From";
			grd.RootTable.Columns["WareHouseFromId"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobLotId"].HasValueList = true;
			grd.RootTable.Columns["JobLotId"].LimitToList = true;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobLotId"].Caption = "Job Lot";
			grd.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtjob.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["AssetId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["AssetId"].HasValueList = true;
			grd.RootTable.Columns["AssetId"].LimitToList = true;
			grd.RootTable.Columns["AssetId"].EditType = (EditType)4;
			grd.RootTable.Columns["AssetId"].Caption = "Machine/Asset";
			grd.RootTable.Columns["AssetId"].ValueList.PopulateValueList((IEnumerable)dtAsset.DefaultView, "Id", "AssetName");
			grd.RootTable.Columns["AssetId"].Width = 150;
			grd.RootTable.Columns["ConditionId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["ConditionId"].HasValueList = true;
			grd.RootTable.Columns["ConditionId"].LimitToList = true;
			grd.RootTable.Columns["ConditionId"].EditType = (EditType)4;
			grd.RootTable.Columns["ConditionId"].Caption = "Condition";
			grd.RootTable.Columns["ConditionId"].ValueList.PopulateValueList((IEnumerable)dtCondition.DefaultView, "Id", "LookUpName");
			grd.RootTable.Columns["ConditionId"].Width = 150;
			grd.RootTable.Columns["City"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["City"].HasValueList = true;
			grd.RootTable.Columns["City"].LimitToList = true;
			grd.RootTable.Columns["City"].EditType = (EditType)4;
			grd.RootTable.Columns["City"].Caption = "CityName";
			grd.RootTable.Columns["City"].ValueList.PopulateValueList((IEnumerable)dtCity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Add");
			grd.RootTable.Columns["Add"].Key = "Add";
			grd.RootTable.Columns["Add"].Caption = "+";
			grd.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Add"].ButtonText = "+";
			grd.RootTable.Columns["Add"].Width = 20;
			grd.RootTable.Columns["Add"].Position = 0;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 1;
			grd.FrozenColumns = 2;
			WareHouseToShowHide();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void WareHouseToShowHide()
	{
		try
		{
			if (((UltraGridBase)CmbType).ActiveRow != null && Conversion.ToInt(CmbType.Value) > 0)
			{
				if (Conversion.ToInt(CmbType.Value) == 6)
				{
					if (grd.RowCount > 0)
					{
						grd.RootTable.Columns["WareHouse"].Caption = "WareHouse To";
						grd.RootTable.Columns["WareHouseFromId"].Visible = true;
					}
				}
				else if (grd.RowCount > 0)
				{
					grd.RootTable.Columns["WareHouse"].Caption = "WareHouse";
					grd.RootTable.Columns["WareHouseFromId"].Visible = false;
				}
			}
			else if (grd.RowCount > 0)
			{
				grd.RootTable.Columns["WareHouse"].Caption = "WareHouse";
				grd.RootTable.Columns["WareHouseFromId"].Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Add")
			{
				GridEXRow r = grd.CurrentRow;
				DataRow dr = dtdetail.Rows[r.RowIndex];
				dtdetail.Rows.Add(dr.ItemArray);
				grd.DataSource = dtdetail;
				if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					dtdetail.Rows[dtdetail.Rows.Count - 1]["Id"] = 0;
				}
			}
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grd.CurrentRow;
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			grd.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grd);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
	}

	private void GetPendingGatePass()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1601;
			dtPendingGatePass = GatePassInward.PendingInwardReturnedForGrn_Engr(obj);
			if (dtPendingGatePass.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("GpId", typeof(int));
				dtcol.Columns.Add("GpDetailId", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("RefDocumentTypeId", typeof(int));
				dtcol.Columns.Add("OrderType", typeof(string));
				dtcol.Columns.Add("GpSrNo", typeof(int));
				dtcol.Columns.Add("GpDate", typeof(DateTime));
				dtcol.Columns.Add("SupplierCustomerId", typeof(int));
				dtcol.Columns.Add("SupplierName", typeof(string));
				dtcol.Columns.Add("VehicleType", typeof(string));
				dtcol.Columns.Add("VehicleNo", typeof(string));
				dtcol.Columns.Add("BiltyNo", typeof(string));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("Freight", typeof(double));
				dtcol.Columns.Add("ItemId", typeof(int));
				dtcol.Columns.Add("ItemName", typeof(string));
				dtcol.Columns.Add("ItemUOMId", typeof(int));
				dtcol.Columns.Add("PackUom", typeof(string));
				dtcol.Columns.Add("PackUomEquivalent", typeof(double));
				dtcol.Columns.Add("ItemQty", typeof(double));
				dtcol.Columns.Add("Weight", typeof(double));
				dtcol.Columns.Add("WareHouseId", typeof(int));
				dtcol.Columns.Add("WareHouseName", typeof(string));
				dtcol.Columns.Add("CityId", typeof(int));
				dtcol.Columns.Add("CityName", typeof(string));
				dtcol.Columns.Add("SupplierWeight", typeof(double));
				dtcol.Columns.Add("FactoryWeight", typeof(double));
				dtcol.Columns.Add("DifferenceWeight", typeof(double));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Rows.Clear();
				for (int i = 0; i < dtPendingGatePass.Rows.Count; i++)
				{
					dtcol.Rows.Add(dtPendingGatePass.Rows[i]["GpId"], dtPendingGatePass.Rows[i]["GpDetailId"], dtPendingGatePass.Rows[i]["DocumentTypeId"], dtPendingGatePass.Rows[i]["RefDocumentTypeId"], dtPendingGatePass.Rows[i]["OrderType"], dtPendingGatePass.Rows[i]["GpSrNo"], dtPendingGatePass.Rows[i]["GpDate"], dtPendingGatePass.Rows[i]["SupplierCustomerId"], dtPendingGatePass.Rows[i]["SupplierName"], dtPendingGatePass.Rows[i]["VehicleType"], dtPendingGatePass.Rows[i]["VehicleNo"], dtPendingGatePass.Rows[i]["BiltyNo"], dtPendingGatePass.Rows[i]["Status"], dtPendingGatePass.Rows[i]["NetPaid"], dtPendingGatePass.Rows[i]["ItemId"], dtPendingGatePass.Rows[i]["ItemName"], dtPendingGatePass.Rows[i]["ItemUOMId"], dtPendingGatePass.Rows[i]["PackUom"], dtPendingGatePass.Rows[i]["PackUomEquivalent"], dtPendingGatePass.Rows[i]["ItemQty"], dtPendingGatePass.Rows[i]["Weight"], dtPendingGatePass.Rows[i]["WareHouseId"], dtPendingGatePass.Rows[i]["WareHouseName"], dtPendingGatePass.Rows[i]["CityId"], dtPendingGatePass.Rows[i]["CityName"], dtPendingGatePass.Rows[i]["SupplierWeight"], dtPendingGatePass.Rows[i]["FactoryWeight"], dtPendingGatePass.Rows[i]["DifferenceWeight"], dtPendingGatePass.Rows[i]["EntryDate"], dtPendingGatePass.Rows[i]["EntryUser"], dtPendingGatePass.Rows[i]["ModifyDate"], dtPendingGatePass.Rows[i]["ModifyUser"]);
				}
				GrdPendingGatePass.DataSource = dtcol;
				GrdPendingGatePass.RetrieveStructure();
				GrdPendingGatePassSetting();
			}
			else
			{
				GPID = 0;
				combgatepass.Text = string.Empty;
				((Control)(object)combvehtyp).Text = string.Empty;
				txtvehno.Text = string.Empty;
				txtbltyno.Text = string.Empty;
				txtcarramount.Text = string.Empty;
				GrdPendingGatePass.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GrdPendingGatePassSetting()
	{
		try
		{
			GrdPendingGatePass.RootTable.Columns["GpId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["GpDetailId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["SupplierCustomerId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["DocumentTypeId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["ItemId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["ItemUOMId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["WareHouseId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["CityId"].Visible = false;
			GrdPendingGatePass.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			GrdPendingGatePass.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			GrdPendingGatePass.RootTable.Columns["Freight"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdPendingGatePass.RootTable.Columns["Freight"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(GrdPendingGatePass);
			GrdPendingGatePass.RootTable.Columns.Add("Load");
			GrdPendingGatePass.RootTable.Columns["Load"].Caption = "Load";
			GrdPendingGatePass.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdPendingGatePass.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			GrdPendingGatePass.RootTable.Columns["Load"].ButtonText = "Load";
			GrdPendingGatePass.RootTable.Columns["Load"].Position = 0;
			GrdPendingGatePass.RootTable.Columns["Load"].Width = 50;
			GrdPendingGatePass.RootTable.Columns["Load"].Position = 0;
			GrdPendingGatePass.FrozenColumns = 1;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdPendingGatePass_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdPendingGatePass.CurrentRow;
			if (!(e.Column.Key == "Load"))
			{
				return;
			}
			if (btnSave.Enabled && btnSave.Visible)
			{
				if (item == null || (int)item.RowType != 0)
				{
					return;
				}
				if (Conversion.ToString(item.Cells["Status"].Value) != "Accepted")
				{
					MessageBox.Show("Selected Gate Pass Status Should be Accepted...", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				GPID = Conversion.ToInt(item.Cells["GpId"].Value);
				int RefDocId = Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value);
				combgatepass.Text = Conversion.ToString(item.Cells["GpSrNo"].Value);
				((Control)(object)combvehtyp).Text = Conversion.ToString(item.Cells["VehicleType"].Value);
				txtvehno.Text = Conversion.ToString(item.Cells["VehicleNo"].Value);
				txtbltyno.Text = Conversion.ToString(item.Cells["BiltyNo"].Value);
				txtsuppwt.Text = Conversion.ToString(item.Cells["SupplierWeight"].Value);
				if (Conversion.ToDouble(item.Cells["FactoryWeight"].Value) > 0.0)
				{
					txtfctwt.Text = Conversion.ToString(item.Cells["FactoryWeight"].Value);
					txtfctwt.Enabled = false;
				}
				txtwtdiff.Text = Conversion.ToString(item.Cells["DifferenceWeight"].Value);
				txtcarramount.Text = Conversion.ToDouble(item.Cells["Freight"].Value).ToString(clsGlobalVariables.stringFormatsingle);
				switch (RefDocId)
				{
				case 1612:
					((Control)(object)CmbType).Enabled = false;
					CmbType.Value = 2;
					CmbType_Leave(null, null);
					break;
				case 5:
					((Control)(object)CmbType).Enabled = false;
					CmbType.Value = 6;
					CmbType_Leave(null, null);
					break;
				}
			}
			else
			{
				MessageBox.Show("Can't Load In Update Mode... Reset The Form first", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ba: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvGrn GRN = new InvGrn();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				GRN.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			if (grd.RowCount <= 0)
			{
				return;
			}
			GRN.DocumentTypeId = 1618;
			GRN.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			GRN.DeliveryTerm = Conversion.ToString((object)((Control)(object)CmbDeliveryTerm).Text);
			if (!SubsidiaryAccountAllownOnVouchers)
			{
				GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
				GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
				GRN.TransporterSupCustId = 0;
			}
			else
			{
				GRN.TransporterId = Conversion.ToInt(((UltraDropDownBase)CmbTransport).SelectedRow.Cells[3].Value);
				GRN.TransporterSupCustId = Conversion.ToInt(CmbTransport.Value);
				GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
			}
			GRN.GrnTypeId = Conversion.ToInt(CmbType.Value);
			GRN.SupplierCustomerId = Conversion.ToInt(combsupplier.Value);
			if (GRN.GrnTypeId == 3)
			{
				GRN.ReturnableDate = datReturnableDate.Value;
			}
			GRN.ReqestedById = Conversion.ToInt(CmbRequestedBy.Value);
			GRN.SupplierReference = txtsuppref.Text.Trim();
			GRN.ReferenceDocNo = txtrefno.Text.Trim();
			GRN.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.BiltyNo = txtbltyno.Text.Trim();
			GRN.GpNo = Conversion.ToInt((object)combgatepass.Text);
			GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text);
			GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text);
			GRN.InwardGatePassId = Conversion.ToInt((object)GPID);
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.IsApproved = false;
			GRN.EntryDate = DateTime.Today;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyDate = DateTime.Today;
			GRN.ModifyUser = UserAccount.ID;
			GRN.PostDate = DateTime.Today;
			GRN.PostUser = UserAccount.ID;
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			GRN.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			GRN.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			GRN.ScreenName = Conversion.ToString((object)base.Name);
			GRN.BillCalculateTypeId = Conversion.ToInt((object)BillCalculateTypeId);
			GRN.invGrnDetail = new List<InvGrnDetail>();
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGrnDetail Detail = new InvGrnDetail();
				Detail.Id = ((RecId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				if (GRN.GrnTypeId == 2 || GRN.GrnTypeId == 6)
				{
					Detail.GdnId = Conversion.ToInt(r.Cells["GdnId"].Value);
					if (Detail.GdnId == 0)
					{
						throw new Exception("Gdn Id Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Detail.GdnDetailId = Conversion.ToInt(r.Cells["GdnDetailId"].Value);
					if (Detail.GdnDetailId == 0)
					{
						throw new Exception("Gdn Detail Id Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
				}
				else if (GRN.GrnTypeId == 1)
				{
					Detail.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					if (Detail.PurchaseOrderId == 0)
					{
						throw new Exception("Order Id Field Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Detail.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["OrderDetailId"].Value);
					if (Detail.PurchaseOrderDetailId == 0)
					{
						throw new Exception("Order Detail Id Field Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
					}
					Detail.PurchaserOrderNo = Conversion.ToInt(r.Cells["OrderNo"].Value);
				}
				Detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (Detail.ItemId == 0)
				{
					throw new Exception("Item Field Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
				}
				Detail.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
				if (Detail.ItemUomId == 0)
				{
					throw new Exception("Uom Id not found.Please Check!");
				}
				Detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				if (Detail.JobLotId == 0)
				{
					throw new Exception("JobLot Field Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
				}
				Detail.ItemQty = Conversion.ToDouble(r.Cells["ItemQty"].Value);
				if (Detail.ItemQty == 0.0)
				{
					throw new Exception("Item Qty Field Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
				}
				Detail.GrossWeight = Detail.ItemQty;
				Detail.NetBillWeight = Detail.ItemQty;
				Detail.StockWeight = Detail.ItemQty;
				Detail.WarehouseId = Conversion.ToInt(r.Cells["WareHouse"].Value);
				if (Detail.WarehouseId == 0)
				{
					throw new Exception("Warehouse Field Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
				}
				if (Conversion.ToInt(CmbType.Value) == 6)
				{
					Detail.WareHouseFromId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				}
				if (Conversion.ToInt(CmbType.Value) == 6 && Detail.WareHouseFromId == 0)
				{
					throw new Exception("WareHouse From Field required");
				}
				Detail.AssetId = Conversion.ToInt(r.Cells["AssetId"].Value);
				Detail.ConditionId = Conversion.ToInt(r.Cells["ConditionId"].Value);
				if (GRN.GrnTypeId != 1 && Detail.ConditionId == 0)
				{
					throw new Exception("Condition Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
				}
				Detail.CityId = Conversion.ToInt(r.Cells["City"].Value);
				Detail.AreaCity = Conversion.ToString((object)r.Cells["City"].Text);
				if (Detail.AreaCity == string.Empty)
				{
					throw new Exception("City Field Required in Row#" + (r.RowIndex + 1) + " In Detail Grid");
				}
				Detail.CommentsDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
				GRN.invGrnDetail.Add(Detail);
			}
			_ = BillCalculateTypeId;
			_ = 1;
			GRN.AttachmentsList = new List<DMSAttachments>();
			GRN.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGrn>(GRN, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			GRN.AttachmentsValues = AttachmentsValues;
			GRN.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvGrn.Save(GRN);
			if (RecId > 0)
			{
				MessageBox.Show("Record Updated Successfully!");
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1618, base.Name);
			}
			else
			{
				MessageBox.Show("Record Saved Successfully!");
			}
			reset();
			if (ChkBox.Checked)
			{
				GeneratePrint(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			InvGrn invGrn = InvGrn.GetByID(RecId);
			combbrnch.Value = invGrn.BranchesId;
			combproject.Value = invGrn.ProjectsId;
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			tabControl1.SelectedIndex = 0;
			((Control)(object)CmbDeliveryTerm).Text = invGrn.DeliveryTerm;
			if (invGrn.TransporterId > 0)
			{
				CmbTransport.Value = invGrn.TransporterId;
			}
			txtcarramount.Text = invGrn.CarriageAmount.ToString(clsGlobalVariables.stringFormatsingle);
			((Control)(object)CmbType).Enabled = false;
			CmbType.Value = invGrn.GrnTypeId;
			CmbType_Leave(null, null);
			combsupplier.Value = invGrn.SupplierCustomerId;
			((Control)(object)combsupplier).Enabled = false;
			((Control)(object)cmbOrderNo).Enabled = false;
			if (invGrn.invGrnDetail != null && invGrn.invGrnDetail.Count > 0 && invGrn.invGrnDetail[0].PurchaseOrderId > 0)
			{
				cmbOrderNo.Value = invGrn.invGrnDetail[0].PurchaseOrderId;
			}
			if (invGrn.GrnTypeId == 3)
			{
				datReturnableDate.Value = Conversion.ToDateTime((object)invGrn.ReturnableDate);
			}
			((Control)(object)CmbRequestedBy).Enabled = false;
			CmbRequestedBy.Value = invGrn.ReqestedById;
			txtsuppref.Text = Conversion.ToString((object)invGrn.SupplierReference);
			txtrefno.Text = Conversion.ToString((object)invGrn.ReferenceDocNo);
			((Control)(object)combvehtyp).Text = invGrn.VehicleType;
			txtvehno.Text = invGrn.VehicleNo;
			txtbltyno.Text = invGrn.BiltyNo;
			combgatepass.Text = invGrn.GpNo.ToString();
			txtsuppwt.Text = invGrn.PartyWeight.ToString("#,##0.###");
			txtfctwt.Text = invGrn.FactoryWeight.ToString("#,##0.###");
			txtwtdiff.Text = (Conversion.ToDouble((object)invGrn.PartyWeight) - Conversion.ToDouble((object)invGrn.FactoryWeight)).ToString("#,##0.###");
			txtremarks.Text = invGrn.RemarksHeader;
			AttachmentsValues = invGrn.AttachmentsValues;
			CustomAttachmentsValues = invGrn.CustomAttachmentsValues;
			GPID = Conversion.ToInt((object)invGrn.InwardGatePassId);
			BillCalculateTypeId = Conversion.ToInt((object)invGrn.BillCalculateTypeId);
			dtdetail.Clear();
			for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
			{
				dtdetail.Rows.Add(invGrn.invGrnDetail[i].Id, invGrn.invGrnDetail[i].GdnId, invGrn.invGrnDetail[i].GdnDetailId, invGrn.invGrnDetail[i].PurchaseOrderId, invGrn.invGrnDetail[i].PurchaseOrderDetailId, invGrn.invGrnDetail[i].PurchaserOrderNo, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].ItemId, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].ItemUomId, invGrn.invGrnDetail[i].UOMCode, invGrn.invGrnDetail[i].CommentsDetail, invGrn.invGrnDetail[i].JobLotId, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].AssetId, invGrn.invGrnDetail[i].ConditionId, invGrn.invGrnDetail[i].CityId, invGrn.invGrnDetail[i].WareHouseFromId);
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
			WareHouseToShowHide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UpdateRecievedWtAndBalanceWtOnEdit()
	{
	}

	private void HistoryGridFill(int NoOfRecods = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters grn = new ReportsParameters();
			grn.OrganizationId = UserAccount.OrganizationId;
			grn.CompanyId = UserAccount.CompanyId;
			grn.BranchesId = UserAccount.BranchesId;
			grn.DocumentTypeId = 1618;
			grn.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			grn.CanViewAllRecord = formRights.DoHaveCanViewAllRecordRights;
			if (!grn.CanViewAllRecord)
			{
				grn.EntryUser = UserAccount.ID;
			}
			if (rddoc.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentry.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodify.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproved.Checked)
			{
				if (FromDateHistory.Checked)
				{
					grn.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					grn.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			grn.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			grn.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			grn.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			grn.RequestedById = Conversion.ToInt(CmbRequestedByHistory.Value);
			DataTable dthistory = InvGrn.GetHisoty(grn);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("Transporter", typeof(string));
				dt.Columns.Add("FreightAmount", typeof(double));
				dt.Columns.Add("OrderType", typeof(string));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("GpNo", typeof(int));
				dt.Columns.Add("VehicleNo", typeof(string));
				dt.Columns.Add("BiltyNo", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				for (int i = 0; i < dthistory.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dthistory.Rows[i]["Id"]), Conversion.ToDateTime(dthistory.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToInt(dthistory.Rows[i]["DocNo"]), Conversion.ToString(dthistory.Rows[i]["DeliveryTerm"]), Conversion.ToString(dthistory.Rows[i]["Transporter"]), Conversion.ToDouble(dthistory.Rows[i]["CarriageAmount"]), Conversion.ToString(dthistory.Rows[i]["OrderType"]), Conversion.ToString(dthistory.Rows[i]["SupplierName"]), Conversion.ToInt(dthistory.Rows[i]["GpNo"]), Conversion.ToString(dthistory.Rows[i]["VehicleNo"]), Conversion.ToString(dthistory.Rows[i]["BiltyNo"]), dthistory.Rows[i]["EntryUser"], dthistory.Rows[i]["EntryDate"], dthistory.Rows[i]["ModifyUser"], dthistory.Rows[i]["ModifyDate"], dthistory.Rows[i]["ApprovedUser"], dthistory.Rows[i]["PostDate"], Conversion.ToString(dthistory.Rows[i]["RemarksHeader"]), Conversion.ToInt(dthistory.Rows[i]["NoOfAttachments"]));
				}
				GrdHistory.DataSource = dt;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			GrdHistory.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			GrdHistory.RootTable.Columns["FreightAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["FreightAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			GrdHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(GrdHistory);
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			GrdHistory.RootTable.Columns["Edit"].Position = 0;
			GrdHistory.RootTable.Columns["Edit"].Width = 50;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].ButtonText = "Print";
			GrdHistory.RootTable.Columns["Print"].Position = 1;
			GrdHistory.RootTable.Columns["Print"].Width = 50;
			GrdHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			ReadById(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (e.Column.Key == "Edit")
			{
				if (item != null && (int)item.RowType == 0)
				{
					ReadById(Conversion.ToInt(item.Cells["Id"].Value));
				}
			}
			else if (e.Column.Key == "Print" && item != null && (int)item.RowType == 0)
			{
				GeneratePrint(Conversion.ToInt(item.Cells["Id"].Value));
			}
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
			FromDateHistory.Focus();
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1618);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GrdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || item.Cells.Count <= 0)
			{
				return;
			}
			InvGrn invGrn = new InvGrn();
			invGrn = InvGrn.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
			if (invGrn != null && invGrn.invGrnDetail != null)
			{
				DataTable detail = new DataTable();
				detail.Columns.Add("GdnId", typeof(int));
				detail.Columns.Add("OrderNo", typeof(int));
				detail.Columns.Add("Warehouse");
				detail.Columns.Add("ItemName");
				detail.Columns.Add("PackUom");
				detail.Columns.Add("Remarks");
				detail.Columns.Add("JobLot");
				detail.Columns.Add("ItemQty", typeof(double));
				detail.Columns.Add("Machine/Product");
				detail.Columns.Add("ItemCondition");
				detail.Columns.Add("City");
				detail.Columns.Add("WarehouseFrom", typeof(string));
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					detail.Rows.Add(invGrn.invGrnDetail[i].GdnId, invGrn.invGrnDetail[i].PurchaserOrderNo, invGrn.invGrnDetail[i].WareHouseCode, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].UOMCode, invGrn.invGrnDetail[i].CommentsDetail, invGrn.invGrnDetail[i].JobLot, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].AssetName, invGrn.invGrnDetail[i].ItemCondition, invGrn.invGrnDetail[i].AreaCity, invGrn.invGrnDetail[i].WareHouseFromCode);
				}
				grdDetailHistory.DataSource = detail;
				grdDetailHistory.RetrieveStructure();
				grdDetailhistorySetting();
				if (Conversion.ToString((object)item.Cells["OrderType"].Text) == "Transfer")
				{
					grdDetailHistory.RootTable.Columns["Warehouse"].Caption = "WareHouse To";
					grdDetailHistory.RootTable.Columns["WarehouseFrom"].Visible = true;
				}
				else
				{
					grdDetailHistory.RootTable.Columns["Warehouse"].Caption = "WareHouse";
					grdDetailHistory.RootTable.Columns["WarehouseFrom"].Visible = false;
				}
			}
			else
			{
				grdDetailHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdDetailhistorySetting()
	{
		try
		{
			if (Conversion.ToInt(grdDetailHistory.GetRow(0).Cells["GdnId"].Value) > 0)
			{
				grdDetailHistory.RootTable.Columns["OrderNo"].Visible = false;
			}
			grdDetailHistory.RootTable.Columns["GdnId"].Visible = false;
			grdDetailHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.###";
			grdDetailHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdDetailHistory);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnResetHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			((Control)(object)CmbRequestedByHistory).Text = string.Empty;
			GrdHistory.DataSource = null;
			grdDetailHistory.DataSource = null;
			rddoc.Checked = true;
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
			HistoryComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
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
			GridEXRow[] checkedRows = grd.GetCheckedRows();
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
		if (e.Control && e.KeyCode == Keys.L)
		{
			btnLoadSupplySchedule_Click(null, null);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdPendingGatePass).Name);
			if (fs != null)
			{
				GrdPendingGatePass.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			GeneratePrint(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnIGP_Click(object sender, EventArgs e)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Conversion.ToInt((object)combgatepass.Text) == 0)
			{
				throw new Exception("Record Not Found For Display");
			}
			DataTable dt = GatePassInwardReports.GatePassInwardSlipandRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				DocumentTypeId = 1601,
				FromDocNo = Conversion.ToInt((object)combgatepass.Text)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dt, "1601-InwardGatePassSlipEngr.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GeneratePrint(int PrintId)
	{
		try
		{
			CommonServices.GoodsReceiptsNotesSlip1618(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void GridStockWeightCalculation()
	{
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void combsupplier_ValueChanged(object sender, EventArgs e)
	{
	}

	private void btnLoadSupplySchedule_Click(object sender, EventArgs e)
	{
	}

	private void combgatepass_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtcarramount_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextNumberFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void btnPendingGdn_Click(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				if (GPID == 0)
				{
					throw new Exception("Please Select GatePass First...");
				}
				Loader = new frmLoadPendingReturnableGdnForReceiving(UserAccount);
				if (Conversion.ToInt(CmbType.Value) == 2)
				{
					Loader.DeliveryTypeIds = "4";
				}
				else if (Conversion.ToInt(CmbType.Value) == 6)
				{
					Loader.DeliveryTypeIds = "5";
				}
				Loader.ShowDialog();
				Loader.BringToFront();
				LoadInGridDetail();
				return;
			}
			if (Conversion.ToInt(grd.GetRow(0).Cells["GdnId"].Value) > 0)
			{
				if (GPID == 0)
				{
					throw new Exception("Please Select GatePass First...");
				}
				Loader = new frmLoadPendingReturnableGdnForReceiving(UserAccount);
				if (Conversion.ToInt(CmbType.Value) == 2)
				{
					Loader.DeliveryTypeIds = "4";
				}
				else if (Conversion.ToInt(CmbType.Value) == 6)
				{
					Loader.DeliveryTypeIds = "5";
				}
				Loader.ShowDialog();
				Loader.BringToFront();
				LoadInGridDetail();
				return;
			}
			throw new Exception("You Can't Load Pending Gdn Because Other Entry Already Exist");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void LoadInGridDetail()
	{
		try
		{
			DataTable loader = Loader.dtloader;
			if (loader.Rows.Count <= 0)
			{
				return;
			}
			if (((UltraGridBase)combsupplier).ActiveRow != null && grd.GetRows().Count() > 0)
			{
				if (Conversion.ToInt(combsupplier.Value) != Conversion.ToInt(loader.Rows[0]["SupplierCustomerId"]))
				{
					throw new Exception("You Can't Load Different Supplier");
				}
				if (Conversion.ToInt(CmbRequestedBy.Value) != Conversion.ToInt(loader.Rows[0]["RequestedByLookUpId"]))
				{
					throw new Exception("You Can't Load Different Requested By at Same Type");
				}
			}
			if (Conversion.ToInt(loader.Rows[0]["SupplierCustomerId"]) > 0)
			{
				((Control)(object)combsupplier).Enabled = false;
			}
			else
			{
				((Control)(object)combsupplier).Enabled = true;
			}
			((Control)(object)CmbRequestedBy).Enabled = false;
			combsupplier.Value = Conversion.ToInt(loader.Rows[0]["SupplierCustomerId"]);
			CmbRequestedBy.Value = Conversion.ToInt(loader.Rows[0]["RequestedByLookUpId"]);
			for (int i = 0; i < loader.Rows.Count; i++)
			{
				bool flag = false;
				if (grd.GetRows().Length != 0)
				{
					GridEXRow[] rows = grd.GetRows();
					for (int j = 0; j < rows.Length; j++)
					{
						if (Conversion.ToInt(rows[j].Cells["GdnDetailId"].Value) == Conversion.ToInt(loader.Rows[i]["DetailId"]))
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					dtdetail.Rows.Add(0, loader.Rows[i]["Id"], loader.Rows[i]["DetailId"], 0, 0, 0, 0, loader.Rows[i]["ItemId"], loader.Rows[i]["ItemName"], loader.Rows[i]["ItemUomId"], loader.Rows[i]["PackUom"], "", 0, loader.Rows[i]["BalQty"], 0, 0, 0);
				}
				flag = false;
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "GrdHistory")
			{
				FromDateHistory.Focus();
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
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_0603: Unknown result type (might be due to invalid IL or missing references)
		//IL_060a: Expected O, but got Unknown
		//IL_060a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Expected O, but got Unknown
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_0618: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_061f: Expected O, but got Unknown
		//IL_061f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0626: Expected O, but got Unknown
		//IL_0626: Unknown result type (might be due to invalid IL or missing references)
		//IL_062d: Expected O, but got Unknown
		//IL_062d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Expected O, but got Unknown
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_063b: Expected O, but got Unknown
		//IL_063b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0642: Expected O, but got Unknown
		//IL_0642: Unknown result type (might be due to invalid IL or missing references)
		//IL_0649: Expected O, but got Unknown
		//IL_0649: Unknown result type (might be due to invalid IL or missing references)
		//IL_0650: Expected O, but got Unknown
		//IL_0650: Unknown result type (might be due to invalid IL or missing references)
		//IL_0657: Expected O, but got Unknown
		//IL_0657: Unknown result type (might be due to invalid IL or missing references)
		//IL_065e: Expected O, but got Unknown
		//IL_065e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0665: Expected O, but got Unknown
		//IL_0665: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0673: Expected O, but got Unknown
		//IL_0673: Unknown result type (might be due to invalid IL or missing references)
		//IL_067a: Expected O, but got Unknown
		//IL_067a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0681: Expected O, but got Unknown
		//IL_0681: Unknown result type (might be due to invalid IL or missing references)
		//IL_0688: Expected O, but got Unknown
		//IL_0688: Unknown result type (might be due to invalid IL or missing references)
		//IL_068f: Expected O, but got Unknown
		//IL_068f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0696: Expected O, but got Unknown
		//IL_0696: Unknown result type (might be due to invalid IL or missing references)
		//IL_069d: Expected O, but got Unknown
		//IL_069d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a4: Expected O, but got Unknown
		//IL_06a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ab: Expected O, but got Unknown
		//IL_06ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b2: Expected O, but got Unknown
		//IL_06b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b9: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Expected O, but got Unknown
		//IL_0712: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Expected O, but got Unknown
		//IL_0754: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Expected O, but got Unknown
		//IL_076a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0774: Expected O, but got Unknown
		//IL_0780: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Expected O, but got Unknown
		//IL_07a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ab: Expected O, but got Unknown
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f8: Expected O, but got Unknown
		//IL_0804: Unknown result type (might be due to invalid IL or missing references)
		//IL_080e: Expected O, but got Unknown
		//IL_081a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0824: Expected O, but got Unknown
		//IL_0867: Unknown result type (might be due to invalid IL or missing references)
		//IL_0871: Expected O, but got Unknown
		//IL_0893: Unknown result type (might be due to invalid IL or missing references)
		//IL_089d: Expected O, but got Unknown
		//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08df: Expected O, but got Unknown
		//IL_0901: Unknown result type (might be due to invalid IL or missing references)
		//IL_090b: Expected O, but got Unknown
		//IL_090c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0916: Expected O, but got Unknown
		//IL_092d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Expected O, but got Unknown
		//IL_0990: Unknown result type (might be due to invalid IL or missing references)
		//IL_099a: Expected O, but got Unknown
		//IL_09fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Expected O, but got Unknown
		//IL_0a09: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a13: Expected O, but got Unknown
		//IL_0a2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a34: Expected O, but got Unknown
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4a: Expected O, but got Unknown
		//IL_0b3d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b47: Expected O, but got Unknown
		//IL_0b69: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Expected O, but got Unknown
		//IL_0c03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c0d: Expected O, but got Unknown
		//IL_0c0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c18: Expected O, but got Unknown
		//IL_12b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c3: Expected O, but got Unknown
		//IL_12d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_12da: Expected O, but got Unknown
		//IL_14b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14bd: Expected O, but got Unknown
		//IL_9143: Unknown result type (might be due to invalid IL or missing references)
		//IL_914d: Expected O, but got Unknown
		//IL_9171: Unknown result type (might be due to invalid IL or missing references)
		//IL_917b: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdPendingGatePass_DesignTimeLayout = new GridEXLayout();
		Appearance appearance215 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
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
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
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
		Appearance appearance151 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.PurchaseTrading.GoodsReceivingNote_Engr));
		GridEXLayout grdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.GrdPendingGatePass = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.DetailPanel = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbCity = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.CmbCondition = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbJobLot = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.CmbWarehouse = new UltraCombo();
		this.label31 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.Add = new System.Windows.Forms.Button();
		this.cmbAssetsRef = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.cmbUOM = new UltraCombo();
		this.label35 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label44 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.CmbRequestedBy = new UltraCombo();
		this.label28 = new System.Windows.Forms.Label();
		this.datReturnableDate = new System.Windows.Forms.DateTimePicker();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbType = new UltraCombo();
		this.combgatepass = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbOrderNo = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtrefno = new System.Windows.Forms.TextBox();
		this.txtsuppref = new System.Windows.Forms.TextBox();
		this.combsupplier = new UltraCombo();
		this.combvehtyp = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbDeliveryTerm = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtwtdiff = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbTransport = new UltraCombo();
		this.txtcarramount = new System.Windows.Forms.TextBox();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.label48 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.combbrnch = new UltraCombo();
		this.combproject = new UltraCombo();
		this.label15 = new System.Windows.Forms.Label();
		this.txtDeliverTerm = new System.Windows.Forms.TextBox();
		this.combrefprty = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.combstockprty = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnIGP = new System.Windows.Forms.ToolStripButton();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnLoadPo = new System.Windows.Forms.ToolStripButton();
		this.btnPendingGdn = new System.Windows.Forms.ToolStripButton();
		this.btnUpdateDetailRowsData = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdDetailHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label41 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproved = new System.Windows.Forms.RadioButton();
		this.rdmodify = new System.Windows.Forms.RadioButton();
		this.rddoc = new System.Windows.Forms.RadioButton();
		this.rdentry = new System.Windows.Forms.RadioButton();
		this.btnShow = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.label39 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.CmbRequestedByHistory = new UltraCombo();
		this.CmbCustomerHistory = new UltraCombo();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnResetHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel10.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdPendingGatePass).BeginInit();
		this.panel12.SuspendLayout();
		this.DetailPanel.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsRef).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedBy).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedByHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1129, 744);
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
		this.tabControl1.Size = new System.Drawing.Size(1129, 744);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1121, 718);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel5.Controls.Add(this.panel13);
		this.panel5.Controls.Add(this.panel10);
		this.panel5.Controls.Add(this.DetailPanel);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(2, 219);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1117, 497);
		this.panel5.TabIndex = 2;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 116);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1117, 157);
		this.panel13.TabIndex = 5;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1117, 157);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.panel10.Controls.Add(this.panel14);
		this.panel10.Controls.Add(this.panel12);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel10.Location = new System.Drawing.Point(0, 273);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1117, 224);
		this.panel10.TabIndex = 4;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.GrdPendingGatePass);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 25);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1117, 199);
		this.panel14.TabIndex = 1;
		this.GrdPendingGatePass.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdPendingGatePass).BackColor = System.Drawing.Color.White;
		GrdPendingGatePass_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdPendingGatePass.DesignTimeLayout = GrdPendingGatePass_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdPendingGatePass).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GrdPendingGatePass).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdPendingGatePass.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdPendingGatePass).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdPendingGatePass).Name = "GrdPendingGatePass";
		((ScrollableBase)this.GrdPendingGatePass).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdPendingGatePass).Size = new System.Drawing.Size(1117, 199);
		((System.Windows.Forms.Control)(object)this.GrdPendingGatePass).TabIndex = 0;
		this.GrdPendingGatePass.TotalRow = (InheritableBoolean)1;
		this.GrdPendingGatePass.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdPendingGatePass).VisualStyle = (VisualStyle)4;
		this.GrdPendingGatePass.ColumnButtonClick += new ColumnActionEventHandler(GrdPendingGatePass_ColumnButtonClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label5);
		this.panel12.Controls.Add(this.ctrlGrdBar3);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1117, 25);
		this.panel12.TabIndex = 0;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(4, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(292, 17);
		this.label5.TabIndex = 1;
		this.label5.Text = "Pending GatePasses Of Selected Supplier ";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1077, 2);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.GrdPendingGatePass;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 22;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.DetailPanel.Controls.Add(this.groupBox2);
		this.DetailPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.DetailPanel.Location = new System.Drawing.Point(0, 0);
		this.DetailPanel.Name = "DetailPanel";
		this.DetailPanel.Size = new System.Drawing.Size(1117, 116);
		this.DetailPanel.TabIndex = 3;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCondition);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLot);
		this.groupBox2.Controls.Add(this.label30);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouse);
		this.groupBox2.Controls.Add(this.label31);
		this.groupBox2.Controls.Add(this.label32);
		this.groupBox2.Controls.Add(this.label33);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Controls.Add(this.txtItemQty);
		this.groupBox2.Controls.Add(this.Add);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAssetsRef);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(5, 3);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1047, 109);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance215).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance220).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance220).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance220;
		((AppearanceBase)appearance221).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance221;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance222).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance235;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance236).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance237).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance237).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance237).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance237).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance239;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(611, 79);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 8;
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(610, 62);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(27, 15);
		this.label36.TabIndex = 119;
		this.label36.Text = "City";
		this.CmbCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.CmbCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCondition).Location = new System.Drawing.Point(460, 79);
		((System.Windows.Forms.Control)(object)this.CmbCondition).Name = "CmbCondition";
		((System.Windows.Forms.Control)(object)this.CmbCondition).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.CmbCondition).TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(459, 62);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(59, 15);
		this.label3.TabIndex = 117;
		this.label3.Text = "Condition";
		this.CmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance124).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Location = new System.Drawing.Point(8, 79);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Name = "CmbJobLot";
		((System.Windows.Forms.Control)(object)this.CmbJobLot).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLot).TabIndex = 4;
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(9, 59);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(46, 15);
		this.label30.TabIndex = 115;
		this.label30.Text = "Job Lot";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(543, 31);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(499, 23);
		this.txtRemarksDetail.TabIndex = 3;
		this.CmbWarehouse.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouse.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouse.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouse.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Appearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance161;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance162).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance162).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance167).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance167;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance168).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance168;
		((AppearanceBase)appearance169).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouse).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouse.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Location = new System.Drawing.Point(8, 31);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Name = "CmbWarehouse";
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouse).TabIndex = 0;
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(7, 14);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(67, 15);
		this.label31.TabIndex = 106;
		this.label31.Text = "Warehouse";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(190, 59);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(26, 15);
		this.label32.TabIndex = 110;
		this.label32.Text = "Qty";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(542, 14);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(132, 15);
		this.label33.TabIndex = 113;
		this.label33.Text = "Specification / Remarks";
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(833, 79);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(67, 25);
		this.btnCancelDetail.TabIndex = 11;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(189, 81);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.ShortcutsEnabled = false;
		this.txtItemQty.Size = new System.Drawing.Size(87, 23);
		this.txtItemQty.TabIndex = 5;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(761, 79);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(26, 25);
		this.Add.TabIndex = 9;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.cmbAssetsRef.AllowNull = (DefaultableBoolean)1;
		this.cmbAssetsRef.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAssetsRef.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAssetsRef.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Appearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance211;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance225).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance225).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance225;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance226;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance227).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance227;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance228;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance229).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance229;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance231).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance231).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance231).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance231).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance231;
		((AppearanceBase)appearance232).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance234;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAssetsRef.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Location = new System.Drawing.Point(278, 79);
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Name = "cmbAssetsRef";
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Size = new System.Drawing.Size(180, 26);
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).TabIndex = 6;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(279, 59);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(106, 15);
		this.label34.TabIndex = 111;
		this.label34.Text = "Machine /\u00a0Product";
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance94;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(460, 30);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 2;
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(190, 14);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(67, 15);
		this.label35.TabIndex = 108;
		this.label35.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance218;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(189, 31);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(269, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(459, 13);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 107;
		this.label44.Text = "UOM";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(761, 79);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(73, 25);
		this.btnUpdateDetail.TabIndex = 10;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combbrnch);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.txtDeliverTerm);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combrefprty);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combstockprty);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(2, 59);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1117, 160);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add(this.label29);
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRequestedBy);
		this.groupBox1.Controls.Add(this.label28);
		this.groupBox1.Controls.Add(this.datReturnableDate);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbType);
		this.groupBox1.Controls.Add(this.combgatepass);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbOrderNo);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtrefno);
		this.groupBox1.Controls.Add(this.txtsuppref);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combsupplier);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.combvehtyp);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtwtdiff);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.txtfctwt);
		this.groupBox1.Controls.Add(this.txtsuppwt);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.groupBox1.Controls.Add(this.txtcarramount);
		this.groupBox1.Controls.Add(this.txtvehno);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label19);
		this.groupBox1.Controls.Add(this.txtbltyno);
		this.groupBox1.Controls.Add(this.label48);
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1048, 154);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(504, 20);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(78, 15);
		this.label29.TabIndex = 123;
		this.label29.Text = "Requested By";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Enabled = false;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(91, 125);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(952, 23);
		this.txtremarks.TabIndex = 139;
		this.CmbRequestedBy.AllowNull = (DefaultableBoolean)1;
		this.CmbRequestedBy.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRequestedBy.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRequestedBy.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRequestedBy).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRequestedBy.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Location = new System.Drawing.Point(585, 14);
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Name = "CmbRequestedBy";
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.CmbRequestedBy).TabIndex = 9;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(276, 102);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(64, 15);
		this.label28.TabIndex = 138;
		this.label28.Text = "Returnable";
		this.datReturnableDate.CustomFormat = "dd-MMM-yy";
		this.datReturnableDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datReturnableDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datReturnableDate.Location = new System.Drawing.Point(342, 98);
		this.datReturnableDate.Name = "datReturnableDate";
		this.datReturnableDate.Size = new System.Drawing.Size(159, 23);
		this.datReturnableDate.TabIndex = 8;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(276, 20);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(32, 15);
		this.label27.TabIndex = 125;
		this.label27.Text = "Type";
		this.CmbType.AllowNull = (DefaultableBoolean)1;
		this.CmbType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.CmbType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbType).Location = new System.Drawing.Point(342, 14);
		((System.Windows.Forms.Control)(object)this.CmbType).Name = "CmbType";
		((System.Windows.Forms.Control)(object)this.CmbType).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.CmbType).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbType).Leave += new System.EventHandler(CmbType_Leave);
		this.combgatepass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.combgatepass.Enabled = false;
		this.combgatepass.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combgatepass.Location = new System.Drawing.Point(775, 43);
		this.combgatepass.Name = "combgatepass";
		this.combgatepass.Size = new System.Drawing.Size(88, 23);
		this.combgatepass.TabIndex = 14;
		this.combgatepass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(combgatepass_KeyPress);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(3, 20);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(276, 75);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(53, 15);
		this.label22.TabIndex = 27;
		this.label22.Text = "OrderNo";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(3, 129);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(91, 16);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.Size = new System.Drawing.Size(75, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.cmbOrderNo.AllowNull = (DefaultableBoolean)1;
		this.cmbOrderNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbOrderNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbOrderNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance114).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance115).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbOrderNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbOrderNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Location = new System.Drawing.Point(342, 69);
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Name = "cmbOrderNo";
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.cmbOrderNo).Leave += new System.EventHandler(cmbOrderNo_Leave);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(168, 16);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(106, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.TabStop = false;
		this.txtrefno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtrefno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtrefno.Location = new System.Drawing.Point(585, 71);
		this.txtrefno.Name = "txtrefno";
		this.txtrefno.Size = new System.Drawing.Size(114, 23);
		this.txtrefno.TabIndex = 11;
		this.txtsuppref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppref.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuppref.Location = new System.Drawing.Point(585, 43);
		this.txtsuppref.Name = "txtsuppref";
		this.txtsuppref.Size = new System.Drawing.Size(114, 23);
		this.txtsuppref.TabIndex = 10;
		this.combsupplier.AllowNull = (DefaultableBoolean)1;
		this.combsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance212).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.combsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance213).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance213).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance214;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance56).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsupplier).Location = new System.Drawing.Point(342, 41);
		((System.Windows.Forms.Control)(object)this.combsupplier).Name = "combsupplier";
		((System.Windows.Forms.Control)(object)this.combsupplier).Size = new System.Drawing.Size(159, 26);
		((System.Windows.Forms.Control)(object)this.combsupplier).TabIndex = 6;
		this.combsupplier.ValueChanged += new System.EventHandler(combsupplier_ValueChanged);
		this.combvehtyp.AllowNull = (DefaultableBoolean)1;
		this.combvehtyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvehtyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvehtyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combvehtyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Enabled = false;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combvehtyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Location = new System.Drawing.Point(585, 96);
		((System.Windows.Forms.Control)(object)this.combvehtyp).Name = "combvehtyp";
		((System.Windows.Forms.Control)(object)this.combvehtyp).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabIndex = 12;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 47);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(80, 15);
		this.label1.TabIndex = 21;
		this.label1.Text = "Delivery Term";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(504, 75);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 8;
		this.label10.Text = "Reference No";
		this.CmbDeliveryTerm.AllowNull = (DefaultableBoolean)1;
		this.CmbDeliveryTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDeliveryTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDeliveryTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance63).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance63).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance64;
		((SpecialBoxBase)((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance82;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDeliveryTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDeliveryTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Location = new System.Drawing.Point(91, 41);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Name = "CmbDeliveryTerm";
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbDeliveryTerm).TabIndex = 2;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(276, 47);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(51, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier";
		this.txtwtdiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtdiff.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtdiff.Location = new System.Drawing.Point(941, 43);
		this.txtwtdiff.Name = "txtwtdiff";
		this.txtwtdiff.ReadOnly = true;
		this.txtwtdiff.Size = new System.Drawing.Size(102, 23);
		this.txtwtdiff.TabIndex = 18;
		this.txtwtdiff.TabStop = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(504, 47);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(71, 15);
		this.label4.TabIndex = 0;
		this.label4.Text = "Supplier Ref";
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfctwt.Location = new System.Drawing.Point(941, 16);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.ReadOnly = true;
		this.txtfctwt.Size = new System.Drawing.Size(102, 23);
		this.txtfctwt.TabIndex = 17;
		this.txtfctwt.TabStop = false;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuppwt.Location = new System.Drawing.Point(775, 98);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.ReadOnly = true;
		this.txtsuppwt.Size = new System.Drawing.Size(88, 23);
		this.txtsuppwt.TabIndex = 16;
		this.txtsuppwt.TabStop = false;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(702, 75);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(49, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(864, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(65, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Factory Wt";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(702, 102);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(70, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Supplier Wt";
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(91, 69);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 3;
		this.txtcarramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcarramount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcarramount.Location = new System.Drawing.Point(91, 98);
		this.txtcarramount.Name = "txtcarramount";
		this.txtcarramount.Size = new System.Drawing.Size(183, 23);
		this.txtcarramount.TabIndex = 4;
		this.txtcarramount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcarramount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcarramount_KeyPress);
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Enabled = false;
		this.txtvehno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehno.Location = new System.Drawing.Point(775, 16);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.Size = new System.Drawing.Size(88, 23);
		this.txtvehno.TabIndex = 13;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(702, 47);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(57, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Gate Pass";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(865, 47);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(75, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Differnce Wt";
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(702, 20);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(64, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Enabled = false;
		this.txtbltyno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbltyno.Location = new System.Drawing.Point(775, 71);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.Size = new System.Drawing.Size(88, 23);
		this.txtbltyno.TabIndex = 15;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(3, 75);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(67, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 102);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(70, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freight Amt";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(504, 102);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(73, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.combbrnch.AllowNull = (DefaultableBoolean)1;
		this.combbrnch.AutoCompleteMode = (AutoCompleteMode)4;
		this.combbrnch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combbrnch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance172).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combbrnch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.combbrnch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance173).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance173).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance173;
		((AppearanceBase)appearance174).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance174;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance176).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance176).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance176).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance178).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance180).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance181).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance181).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance181).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance181;
		((AppearanceBase)appearance182).TextHAlignAsString = "Left";
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance183;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combbrnch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combbrnch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combbrnch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combbrnch).Location = new System.Drawing.Point(1176, 32);
		((System.Windows.Forms.Control)(object)this.combbrnch).Name = "combbrnch";
		((System.Windows.Forms.Control)(object)this.combbrnch).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.combbrnch).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combbrnch).Visible = false;
		this.combproject.AllowNull = (DefaultableBoolean)1;
		this.combproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.combproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject).Location = new System.Drawing.Point(1176, 59);
		((System.Windows.Forms.Control)(object)this.combproject).Name = "combproject";
		((System.Windows.Forms.Control)(object)this.combproject).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.combproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combproject).Visible = false;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1091, 11);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(80, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Delivery Term";
		this.label15.Visible = false;
		this.txtDeliverTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliverTerm.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliverTerm.Location = new System.Drawing.Point(1176, 7);
		this.txtDeliverTerm.Name = "txtDeliverTerm";
		this.txtDeliverTerm.ReadOnly = true;
		this.txtDeliverTerm.Size = new System.Drawing.Size(104, 23);
		this.txtDeliverTerm.TabIndex = 20;
		this.txtDeliverTerm.TabStop = false;
		this.txtDeliverTerm.Visible = false;
		this.combrefprty.AllowNull = (DefaultableBoolean)1;
		this.combrefprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrefprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrefprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrefprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combrefprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrefprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrefprty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrefprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrefprty).Location = new System.Drawing.Point(1176, 91);
		((System.Windows.Forms.Control)(object)this.combrefprty).Name = "combrefprty";
		((System.Windows.Forms.Control)(object)this.combrefprty).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.combrefprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combrefprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combrefprty).Visible = false;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(1091, 130);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.combstockprty.AllowNull = (DefaultableBoolean)1;
		this.combstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.combstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance200).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance200).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance201;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance156).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combstockprty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combstockprty).Location = new System.Drawing.Point(1176, 121);
		((System.Windows.Forms.Control)(object)this.combstockprty).Name = "combstockprty";
		((System.Windows.Forms.Control)(object)this.combstockprty).Size = new System.Drawing.Size(104, 26);
		((System.Windows.Forms.Control)(object)this.combstockprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combstockprty).Visible = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1091, 100);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(54, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Ref Party";
		this.label12.Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1091, 37);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1091, 63);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label23);
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 33);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1117, 26);
		this.panel3.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(195, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Goods Receiving Notes";
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1045, 4);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(72, 19);
		this.ChkBox.TabIndex = 19;
		this.ChkBox.TabStop = false;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1117, 31);
		this.panel2.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1079, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnNew, this.toolStripButton1, this.btnUpdate, this.btnSave, this.btnAttachment, this.btnIGP, this.printToolStripButton, this.btnLoadPo, this.btnPendingGdn, this.btnUpdateDetailRowsData });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1117, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(60, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Image = (System.Drawing.Image)resources.GetObject("toolStripButton1.Image");
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(81, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(79, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(110, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnIGP.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnIGP.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnIGP.Name = "btnIGP";
		this.btnIGP.Size = new System.Drawing.Size(93, 24);
		this.btnIGP.Text = "1601-&IGP";
		this.btnIGP.Click += new System.EventHandler(btnIGP_Click);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(101, 24);
		this.printToolStripButton.Text = "1618-&Print";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnLoadPo.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadPo.Name = "btnLoadPo";
		this.btnLoadPo.Size = new System.Drawing.Size(64, 24);
		this.btnLoadPo.Text = "Load Po";
		this.btnLoadPo.Visible = false;
		this.btnLoadPo.Click += new System.EventHandler(btnLoadSupplySchedule_Click);
		this.btnPendingGdn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnPendingGdn.Image = (System.Drawing.Image)resources.GetObject("btnPendingGdn.Image");
		this.btnPendingGdn.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPendingGdn.Name = "btnPendingGdn";
		this.btnPendingGdn.Size = new System.Drawing.Size(93, 24);
		this.btnPendingGdn.Text = "Pending Gdn";
		this.btnPendingGdn.Click += new System.EventHandler(btnPendingGdn_Click);
		this.btnUpdateDetailRowsData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnUpdateDetailRowsData.Image = (System.Drawing.Image)resources.GetObject("btnUpdateDetailRowsData.Image");
		this.btnUpdateDetailRowsData.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdateDetailRowsData.Name = "btnUpdateDetailRowsData";
		this.btnUpdateDetailRowsData.Size = new System.Drawing.Size(179, 24);
		this.btnUpdateDetailRowsData.Text = "Detail Row's Data Update";
		this.btnUpdateDetailRowsData.ToolTipText = "Warehouse,JobLot,Condition,City";
		this.btnUpdateDetailRowsData.Click += new System.EventHandler(btnUpdateDetailRowsData_Click);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1121, 715);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel11);
		this.panel8.Controls.Add(this.panel15);
		this.panel8.Controls.Add(this.panel9);
		this.panel8.Controls.Add(this.panel7);
		this.panel8.Controls.Add(this.panel6);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1117, 711);
		this.panel8.TabIndex = 1;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistory);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 425);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1117, 286);
		this.panel11.TabIndex = 73;
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
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Size = new System.Drawing.Size(1117, 286);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).TabIndex = 46;
		this.grdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label41);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 401);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1117, 24);
		this.panel15.TabIndex = 72;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.White;
		this.label41.Location = new System.Drawing.Point(1, 2);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(242, 19);
		this.label41.TabIndex = 23;
		this.label41.Text = "Detail of above selected row";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 127);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1117, 274);
		this.panel9.TabIndex = 71;
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
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1117, 274);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 45;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.SelectionChanged += new System.EventHandler(GrdHistory_SelectionChanged);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel7.Controls.Add(this.groupBox3);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 53);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1117, 74);
		this.panel7.TabIndex = 70;
		this.groupBox3.Controls.Add(this.rdapproved);
		this.groupBox3.Controls.Add(this.rdmodify);
		this.groupBox3.Controls.Add(this.rddoc);
		this.groupBox3.Controls.Add(this.rdentry);
		this.groupBox3.Controls.Add(this.btnShow);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.label16);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.label38);
		this.groupBox3.Controls.Add(this.label39);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(5, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1106, 67);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproved.AutoSize = true;
		this.rdapproved.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproved.Location = new System.Drawing.Point(1002, 41);
		this.rdapproved.Name = "rdapproved";
		this.rdapproved.Size = new System.Drawing.Size(105, 19);
		this.rdapproved.TabIndex = 9;
		this.rdapproved.Text = "Approved Date";
		this.rdapproved.UseVisualStyleBackColor = true;
		this.rdmodify.AutoSize = true;
		this.rdmodify.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodify.Location = new System.Drawing.Point(1002, 22);
		this.rdmodify.Name = "rdmodify";
		this.rdmodify.Size = new System.Drawing.Size(91, 19);
		this.rdmodify.TabIndex = 8;
		this.rdmodify.Text = "Modify Date";
		this.rdmodify.UseVisualStyleBackColor = true;
		this.rddoc.AutoSize = true;
		this.rddoc.Checked = true;
		this.rddoc.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rddoc.Location = new System.Drawing.Point(916, 22);
		this.rddoc.Name = "rddoc";
		this.rddoc.Size = new System.Drawing.Size(75, 19);
		this.rddoc.TabIndex = 6;
		this.rddoc.TabStop = true;
		this.rddoc.Text = "Doc Date";
		this.rddoc.UseVisualStyleBackColor = true;
		this.rdentry.AutoSize = true;
		this.rdentry.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentry.Location = new System.Drawing.Point(916, 42);
		this.rdentry.Name = "rdentry";
		this.rdentry.Size = new System.Drawing.Size(80, 19);
		this.rdentry.TabIndex = 7;
		this.rdentry.Text = "Entry Date";
		this.rdentry.UseVisualStyleBackColor = true;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(847, 36);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 26);
		this.btnShow.TabIndex = 10;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(662, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(78, 15);
		this.label9.TabIndex = 88;
		this.label9.Text = "Requested By";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(436, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(34, 15);
		this.label16.TabIndex = 87;
		this.label16.Text = "Party";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(358, 18);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(64, 15);
		this.label37.TabIndex = 86;
		this.label37.Text = "To Doc No";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(277, 18);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(79, 15);
		this.label38.TabIndex = 85;
		this.label38.Text = "From Doc No";
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label39.Location = new System.Drawing.Point(142, 18);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(48, 15);
		this.label39.TabIndex = 84;
		this.label39.Text = "To Date";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(6, 18);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(63, 15);
		this.label40.TabIndex = 83;
		this.label40.Text = "From Date";
		this.CmbRequestedByHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbRequestedByHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRequestedByHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRequestedByHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRequestedByHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRequestedByHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Location = new System.Drawing.Point(665, 36);
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Name = "CmbRequestedByHistory";
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).Size = new System.Drawing.Size(179, 26);
		((System.Windows.Forms.Control)(object)this.CmbRequestedByHistory).TabIndex = 5;
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance192;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance193).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance193).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance193).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance193;
		((AppearanceBase)appearance194).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance194;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance196).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance196).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance196).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance198;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance203).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance204).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance204).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance204).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance204;
		((AppearanceBase)appearance205).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(439, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(223, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 4;
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(358, 38);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(279, 38);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(78, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(142, 38);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(134, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(6, 38);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(130, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.label14);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 27);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1117, 26);
		this.panel6.TabIndex = 69;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1078, 2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 3);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(258, 19);
		this.label14.TabIndex = 0;
		this.label14.Text = "Goods Receiving Notes History";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnResetHistory, this.btnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1117, 27);
		this.toolStrip2.TabIndex = 68;
		this.toolStrip2.Text = "toolStrip2";
		this.btnResetHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnResetHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnResetHistory.Name = "btnResetHistory";
		this.btnResetHistory.Size = new System.Drawing.Size(60, 24);
		this.btnResetHistory.Text = "&New";
		this.btnResetHistory.Click += new System.EventHandler(btnResetHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(81, 24);
		this.btnRefreshHistory.Text = "Refresh";
		this.btnRefreshHistory.Click += new System.EventHandler(btnRefreshHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1129, 744);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "GoodsReceivingNote_Engr";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvFrmGRN_Qty_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdPendingGatePass).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.DetailPanel.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouse).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsRef).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedBy).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbOrderNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDeliveryTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRequestedByHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
