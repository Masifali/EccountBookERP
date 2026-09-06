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
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
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
namespace Architecture.WinApp.Purchase
{

public class MarketGrn : Form
{
	private bool DoHaveSaveRight;

	private bool DoHavePrintRights;

	private bool DoHaveUpdateRights;

	private bool DoHaveFieldChooserRights;

	private bool DoHaveSaveLayoutRights;

	private bool DoHaveGridPrintRights;

	private bool DoHaveGridExportRights;

	private bool DoHaveGridGroupExpandRights;

	private bool DoHaveGridGroupCollapseRights;

	private bool DoHaveReportExportRights;

	private bool DoHaveReportPrintRights;

	private bool DoHaveCanViewAllRecordRights;

	private int GPID;

	private double TotalQty;

	private double TotalGrossWeight;

	private int updateDetailIndex;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int Id;

	private bool UpdateMode;

	private DataTable dtdetail = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtItem = new DataTable();

	private DataTable dtjob = new DataTable();

	private DataTable dtPack = new DataTable();

	private DataTable dtUOM = new DataTable();

	private DataTable dtwh = new DataTable();

	private DataTable dtcity = new DataTable();

	private DataTable dtpo = new DataTable();

	private DataTable dtCrop = new DataTable();

	private DataTable dtsupbyGrpId = new DataTable();

	private DataTable dtAccountList = new DataTable();

	private UserAccount UserAccount = new UserAccount();

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

	private Panel panel4;

	private Panel panel5;

	private Label label24;

	private Label label25;

	private TextBox txtremarks;

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

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	private GroupBox groupBox3;

	internal UltraCombo combpckuom;

	internal UltraCombo CmbCity;

	internal UltraCombo combwr;

	internal UltraCombo combpcktyp;

	internal UltraCombo combjoblot;

	internal UltraCombo combitem;

	private Label label30;

	private Label label27;

	private Label label22;

	private TextBox txtebu;

	private TextBox txtgwt;

	private TextBox txtqty;

	private TextBox txtadlswt;

	private TextBox txtstockwt;

	private Label label29;

	private TextBox txtnetwt;

	private Label label46;

	private Label label28;

	private Label label35;

	private Label label45;

	private Label label44;

	private Label label43;

	private Label label42;

	private Label label31;

	private Label label41;

	private Label label40;

	private Label label36;

	private Label label37;

	private Panel panel7;

	private Panel panel8;

	internal GridEX GrdHistory;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo comborderno;

	internal UltraCombo combcropyear;

	private Label label9;

	private Label label3;

	private TextBox txtwtcuttotal;

	private TextBox txtwtcut;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton toolStripButton1;

	private Label label15;

	private TextBox txtDeliverTerm;

	private TextBox combgatepass;

	private Label label16;

	private TextBox txtEbWtOther;

	private Panel panel10;

	internal GridEX grd;

	private CheckBox ChkBox;

	private TextBox txtEmptyBagsTotal;

	private Panel panel6;

	internal GridEX grdGp;

	private ToolStripButton CityDefine;

	internal UltraCombo combbrnch;

	internal UltraCombo combproject;

	private Label label32;

	private Button Add;

	private Panel panel11;

	internal GridEX GrdHistoryDetail;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	internal UltraCombo combvehtyp;

	private ToolStripButton btnDelete;

	private TextBox comblabno;

	private ToolStripLabel toolStripLabel1;

	private TextBox txtGrnWeight;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label1;

	private TextBox txtAnaylstName;

	private TextBox txtDeductionKg;

	public MarketGrn(UserAccount user)
	{
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "POCompulsoryforGrn").ToList();
			if (lst.Count > 0 && bool.Parse(lst[0].ConfigKey.ToString()) && (((Control)(object)comborderno).Text.Trim() == "" || int.Parse(comborderno.Value.ToString()) == 0 || ((Control)(object)comborderno).Text.Trim() == "0"))
			{
				MessageBox.Show("Please Select an Order No First");
				comborderno.Focus();
			}
			else if (FormValidationDetail())
			{
				double Qty = 0.0;
				if (txtqty.Text != string.Empty && txtqty.Text != null)
				{
					Qty = Convert.ToDouble(txtqty.Text.Trim());
				}
				double EbU = 0.0;
				if (txtebu.Text != string.Empty && txtebu.Text != null)
				{
					EbU = Convert.ToDouble(txtebu.Text.Trim());
				}
				double EBT = 0.0;
				if (txtEmptyBagsTotal.Text != string.Empty && txtEmptyBagsTotal.Text != null)
				{
					EBT = Convert.ToDouble(txtEmptyBagsTotal.Text.Trim());
				}
				string UOM = ((Control)(object)combpckuom).Text.Trim().ToString();
				double UOMId = 0.0;
				if (((Control)(object)combpckuom).Text != string.Empty && ((Control)(object)combpckuom).Text != null)
				{
					UOMId = double.Parse(combpckuom.Value.ToString());
				}
				if (txtwtcut.Text == string.Empty)
				{
					txtwtcut.Text = "0";
				}
				if (txtwtcuttotal.Text == string.Empty)
				{
					txtwtcuttotal.Text = "0";
				}
				if (txtadlswt.Text == string.Empty)
				{
					txtadlswt.Text = "0";
				}
				if (txtnetwt.Text == string.Empty)
				{
					txtnetwt.Text = "0";
				}
				double num = Conversion.ToDouble((object)txtstockwt.Text.Trim());
				double NetWeight = Conversion.ToDouble((object)txtnetwt.Text.Trim());
				double GrossWeight = Conversion.ToDouble((object)txtGrnWeight.Text.Trim());
				if (num > GrossWeight)
				{
					throw new Exception("StockWeight cannot greater than Gross Weight Please check");
				}
				if (NetWeight > GrossWeight)
				{
					throw new Exception("NetBillWeight cannot greater than Gross Weight Please check");
				}
				((Control)(object)comborderno).Enabled = false;
				dtdetail.Rows.Add(comborderno.Value, ((Control)(object)comborderno).Text, combitem.Value, ((Control)(object)combitem).Text, ((Control)(object)combcropyear).Text, combjoblot.Value, ((Control)(object)combjoblot).Text, combpcktyp.Value, ((Control)(object)combpcktyp).Text, UOM, UOMId, Qty, txtgwt.Text, EbU, EBT, txtwtcut.Text, txtwtcuttotal.Text, txtadlswt.Text.ToString(), txtnetwt.Text, txtstockwt.Text, combwr.Value, comblabno.Text, ((Control)(object)CmbCity).Text);
				grd.DataSource = dtdetail;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Expected O, but got Unknown
		try
		{
			if (!FormValidation() || MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvGrn GRN = new InvGrn();
			GRN.DocumentTypeId = 46;
			GRN.DocDate = DocDate.Value;
			GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
			GRN.SupplierCustomerId = Conversion.ToInt(combsupplier.Value);
			GRN.GpNo = Conversion.ToInt((object)combgatepass.Text);
			GRN.ReferencePartyId = Conversion.ToInt(combrefprty.Value);
			GRN.StockPartyId = Conversion.ToInt(combstockprty.Value);
			GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
			GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
			GRN.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
			GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
			GRN.BiltyNo = txtbltyno.Text.Trim();
			GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
			GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
			GRN.RemarksHeader = txtremarks.Text.Trim();
			GRN.BranchesId = Conversion.ToInt(combbrnch.Value);
			GRN.ProjectsId = Conversion.ToInt(combproject.Value);
			GRN.InwardGatePassId = Conversion.ToInt((object)GPID);
			GRN.OrganizationId = UserAccount.OrganizationId;
			GRN.CompanyId = UserAccount.CompanyId;
			GRN.EntryDate = DateTime.Now;
			GRN.ModifyDate = DateTime.Now;
			GRN.EntryUser = UserAccount.ID;
			GRN.ModifyUser = UserAccount.ID;
			GRN.invGrnDetail = new List<InvGrnDetail>();
			GRN.InvgrnDetailEmptyBagslist = new List<InvgrnDetailEmptyBags>();
			Conversion.ToDouble((object)txtGrnWeight.Text.Trim());
			double GrnQty = 0.0;
			double GrossWeight = 0.0;
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGrnDetail grnDetail = new InvGrnDetail();
				grnDetail.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["PoDetailId"].Value);
				if (grnDetail.PurchaseOrderDetailId == 0)
				{
					throw new Exception("Purchase Order DetailId not found");
				}
				grnDetail.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
				if (grnDetail.PurchaseOrderId == 0)
				{
					throw new Exception("Purchase OrderId not found");
				}
				grnDetail.PurchaserOrderNo = Conversion.ToInt(r.Cells["Order"].Value);
				grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (grnDetail.ItemId == 0)
				{
					throw new Exception("ItemName not found");
				}
				grnDetail.CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
				if (grnDetail.CropYear == string.Empty || grnDetail.CropYear == null)
				{
					throw new Exception("CropYear not found");
				}
				grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
				if (grnDetail.JobLotId == 0)
				{
					throw new Exception("JobLot not found");
				}
				grnDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
				if (grnDetail.PackingTypeId == 0)
				{
					throw new Exception("PackingType not found");
				}
				grnDetail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
				if (grnDetail.ItemQty == 0.0 || Conversion.ToString((object)grnDetail.ItemQty) == string.Empty)
				{
					throw new Exception("ItemQty field required");
				}
				GrnQty += grnDetail.ItemQty;
				grnDetail.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
				if (grnDetail.ItemUomId == 0)
				{
					throw new Exception("ItemUom not found");
				}
				grnDetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
				if (grnDetail.GrossWeight == 0.0 || Conversion.ToString((object)grnDetail.GrossWeight) == string.Empty)
				{
					throw new Exception("GrossWeight field required");
				}
				GrossWeight += grnDetail.GrossWeight;
				grnDetail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
				grnDetail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
				grnDetail.WtCut = Conversion.ToDouble(r.Cells["WtCut"].Value);
				grnDetail.WtCutTotal = Conversion.ToDouble(r.Cells["WtCutTotal"].Value);
				grnDetail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
				grnDetail.NetBillWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
				if (grnDetail.NetBillWeight == 0.0 || Conversion.ToString((object)grnDetail.NetBillWeight) == string.Empty)
				{
					throw new Exception("NetBillWeight field required");
				}
				grnDetail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
				if (grnDetail.StockWeight == 0.0 || Conversion.ToString((object)grnDetail.StockWeight) == string.Empty)
				{
					throw new Exception("StockWeight field required");
				}
				grnDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouse"].Value);
				if (grnDetail.WarehouseId == 0)
				{
					throw new Exception("Warehouse not found");
				}
				grnDetail.AreaCity = Conversion.ToString((object)r.Cells["City"].Text);
				if (grnDetail.AreaCity == string.Empty || grnDetail.AreaCity == null)
				{
					throw new Exception("CityName not found");
				}
				GRN.invGrnDetail.Add(grnDetail);
			}
			GRN.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
				attach.ScreenName = base.Name;
				GRN.AttachmentsList.Add(attach);
			}
			int success = InvGrn.Save(GRN);
			if (success >= 1)
			{
				MessageBox.Show("Receord Save Successfully [" + GRN.DocNo + "]");
			}
			reset();
			if (ChkBox.Checked)
			{
				try
				{
					CommonServices.GrnSlipReport211(46, success);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
				}
			}
			grd.DataSource = null;
			btnSave.Visible = true;
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
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
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Expected O, but got Unknown
		//IL_0734: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Expected O, but got Unknown
		try
		{
			if (Id == 0)
			{
				throw new Exception("Record Not found For Update");
			}
			if (!FormValidation() || MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (grd.RowCount > 0)
			{
				InvGrn GRN = new InvGrn();
				GRN.Id = Id;
				GRN.DocumentTypeId = 46;
				GRN.DocDate = DocDate.Value;
				GRN.DocNo = Convert.ToInt32(txtdocno.Text.Trim());
				GRN.SupplierCustomerId = Conversion.ToInt(combsupplier.Value);
				GRN.GpNo = Conversion.ToInt((object)combgatepass.Text);
				GRN.ReferencePartyId = Conversion.ToInt(combrefprty.Value);
				GRN.StockPartyId = Conversion.ToInt(combstockprty.Value);
				GRN.PartyWeight = Conversion.ToDouble((object)txtsuppwt.Text.Trim());
				GRN.FactoryWeight = Conversion.ToDouble((object)txtfctwt.Text.Trim());
				GRN.VehicleType = Conversion.ToString((object)((Control)(object)combvehtyp).Text);
				GRN.VehicleNo = Conversion.ToString((object)txtvehno.Text.Trim());
				GRN.BiltyNo = txtbltyno.Text.Trim();
				GRN.TransporterId = Conversion.ToInt(CmbTransport.Value);
				GRN.CarriageAmount = Conversion.ToDouble((object)txtcarramount.Text.Trim());
				GRN.RemarksHeader = txtremarks.Text.Trim();
				GRN.BranchesId = Conversion.ToInt(combbrnch.Value);
				GRN.ProjectsId = Conversion.ToInt(combproject.Value);
				GRN.InwardGatePassId = Conversion.ToInt((object)GPID);
				GRN.OrganizationId = UserAccount.OrganizationId;
				GRN.CompanyId = UserAccount.CompanyId;
				GRN.EntryDate = DateTime.Now;
				GRN.ModifyDate = DateTime.Now;
				GRN.EntryUser = UserAccount.ID;
				GRN.ModifyUser = UserAccount.ID;
				GRN.invGrnDetail = new List<InvGrnDetail>();
				GRN.InvgrnDetailEmptyBagslist = new List<InvgrnDetailEmptyBags>();
				GRN.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				Conversion.ToDouble((object)txtGrnWeight.Text.Trim());
				double GrnQty = 0.0;
				double GrossWeight = 0.0;
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvGrnDetail grnDetail = new InvGrnDetail();
					grnDetail.PurchaseOrderDetailId = Conversion.ToInt(r.Cells["PoDetailId"].Value);
					if (grnDetail.PurchaseOrderDetailId == 0)
					{
						throw new Exception("Purchase Order DetailId not found");
					}
					grnDetail.PurchaseOrderId = Conversion.ToInt(r.Cells["OrderId"].Value);
					if (grnDetail.PurchaseOrderId == 0)
					{
						throw new Exception("Purchase OrderId not found");
					}
					grnDetail.PurchaserOrderNo = Conversion.ToInt(r.Cells["Order"].Value);
					grnDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (grnDetail.ItemId == 0)
					{
						throw new Exception("ItemName not found");
					}
					grnDetail.CropYear = Conversion.ToString(r.Cells["CropYear"].Value);
					if (grnDetail.CropYear == string.Empty || grnDetail.CropYear == null)
					{
						throw new Exception("CropYear not found");
					}
					grnDetail.JobLotId = Conversion.ToInt(r.Cells["JobId"].Value);
					if (grnDetail.JobLotId == 0)
					{
						throw new Exception("JobLot not found");
					}
					grnDetail.PackingTypeId = Conversion.ToInt(r.Cells["PackingTypeId"].Value);
					if (grnDetail.PackingTypeId == 0)
					{
						throw new Exception("PackingType not found");
					}
					grnDetail.ItemQty = Conversion.ToDouble(r.Cells["Qty"].Value);
					if (grnDetail.ItemQty == 0.0 || Conversion.ToString((object)grnDetail.ItemQty) == string.Empty)
					{
						throw new Exception("ItemQty field required");
					}
					GrnQty += grnDetail.ItemQty;
					grnDetail.ItemUomId = Conversion.ToInt(r.Cells["UOMId"].Value);
					if (grnDetail.ItemUomId == 0)
					{
						throw new Exception("ItemUom not found");
					}
					grnDetail.GrossWeight = Conversion.ToDouble(r.Cells["GrossWight"].Value);
					if (grnDetail.GrossWeight == 0.0 || Conversion.ToString((object)grnDetail.GrossWeight) == string.Empty)
					{
						throw new Exception("GrossWeight field required");
					}
					GrossWeight += grnDetail.GrossWeight;
					grnDetail.EBWPerUnit = Conversion.ToDouble(r.Cells["EbUnit"].Value);
					grnDetail.EBWTotal = Conversion.ToDouble(r.Cells["EbTotal"].Value);
					grnDetail.WtCut = Conversion.ToDouble(r.Cells["WtCut"].Value);
					grnDetail.WtCutTotal = Conversion.ToDouble(r.Cells["WtCutTotal"].Value);
					grnDetail.AdLsWeight = Conversion.ToDouble(r.Cells["AddLesswt"].Value);
					grnDetail.NetBillWeight = Conversion.ToDouble(r.Cells["NetWeight"].Value);
					if (grnDetail.NetBillWeight == 0.0 || Conversion.ToString((object)grnDetail.NetBillWeight) == string.Empty)
					{
						throw new Exception("NetBillWeight field required");
					}
					grnDetail.StockWeight = Conversion.ToDouble(r.Cells["StockWeight"].Value);
					if (grnDetail.StockWeight == 0.0 || Conversion.ToString((object)grnDetail.StockWeight) == string.Empty)
					{
						throw new Exception("StockWeight field required");
					}
					grnDetail.WarehouseId = Conversion.ToInt(r.Cells["WareHouse"].Value);
					if (grnDetail.WarehouseId == 0)
					{
						throw new Exception("Warehouse not found");
					}
					grnDetail.AreaCity = Conversion.ToString((object)r.Cells["City"].Text);
					if (grnDetail.AreaCity == string.Empty || grnDetail.AreaCity == null)
					{
						throw new Exception("CityName not found");
					}
					GRN.invGrnDetail.Add(grnDetail);
				}
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Attachment.dtAttachments.Rows[i][1].ToString();
					attach.ScreenName = base.Name;
					GRN.AttachmentsList.Add(attach);
				}
				int success = InvGrn.Save(GRN);
				if (success >= 1)
				{
					MessageBox.Show("Receord Update Successfully [" + GRN.DocNo + "]");
				}
				reset();
				if (ChkBox.Checked)
				{
					CommonServices.GrnSlipReport211(46, success);
				}
				grd.DataSource = null;
				return;
			}
			throw new Exception("Grid Record Not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		try
		{
			Id = Conversion.ToInt((object)ID);
			InvGrn invGrn = InvGrn.GetByID(Id);
			combbrnch.Value = invGrn.BranchesId;
			combproject.Value = invGrn.ProjectsId;
			txtdocno.Text = invGrn.DocNo.ToString();
			DocDate.Value = invGrn.DocDate;
			tabControl1.SelectedIndex = 0;
			SupplierNameFilll();
			combsupplier.Value = invGrn.SupplierCustomerId;
			((Control)(object)combsupplier).Enabled = false;
			combstockprty.Value = invGrn.StockPartyId;
			combrefprty.Value = invGrn.ReferencePartyId;
			CmbTransport.Value = invGrn.TransporterId;
			txtcarramount.Text = invGrn.CarriageAmount.ToString();
			txtsuppwt.Text = invGrn.PartyWeight.ToString();
			txtfctwt.Text = invGrn.FactoryWeight.ToString();
			combgatepass.Text = invGrn.GpNo.ToString();
			((Control)(object)combvehtyp).Text = invGrn.VehicleType;
			txtvehno.Text = invGrn.VehicleNo;
			txtbltyno.Text = invGrn.BiltyNo;
			txtremarks.Text = invGrn.RemarksHeader;
			GPID = Conversion.ToInt((object)invGrn.InwardGatePassId);
			DataTable dt = GatePassInward.GetDataByGpId(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = GPID
			});
			if (dt.Rows.Count > 0)
			{
				txtAnaylstName.Text = Conversion.ToString(dt.Rows[0]["AnalystName"]);
				txtDeductionKg.Text = Conversion.ToString(dt.Rows[0]["DeductionWeight"]);
			}
			else
			{
				txtAnaylstName.Text = string.Empty;
				txtDeductionKg.Text = string.Empty;
			}
			dtdetail.Clear();
			double GrossWeight = 0.0;
			for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
			{
				dtdetail.Rows.Add(invGrn.invGrnDetail[i].PurchaseOrderDetailId, invGrn.invGrnDetail[i].PurchaseOrderId, invGrn.invGrnDetail[i].PurchaseOrder, invGrn.invGrnDetail[i].ItemId, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].CropYear, invGrn.invGrnDetail[i].JobLotId, invGrn.invGrnDetail[i].PackingTypeId, invGrn.invGrnDetail[i].UOM, invGrn.invGrnDetail[i].ItemUomId, invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].EBWPerUnit, invGrn.invGrnDetail[i].EBWTotal, invGrn.invGrnDetail[i].WtCut, invGrn.invGrnDetail[i].WtCutTotal, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].StockWeight, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].AreaCity);
				GrossWeight += invGrn.invGrnDetail[i].GrossWeight;
			}
			txtGrnWeight.Text = GrossWeight.ToString();
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			StockWeightCalculations();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(Id.ToString()), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			UpdateMode = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || txtdocno.Text.Trim() == "0" || txtdocno.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)combbrnch).ActiveRow == null)
		{
			MessageBox.Show("Branch Field is Required");
			combbrnch.Focus();
			return false;
		}
		if (((UltraGridBase)combproject).ActiveRow == null)
		{
			MessageBox.Show("Project Field is Required");
			combproject.Focus();
			return false;
		}
		if (((UltraGridBase)combsupplier).ActiveRow == null)
		{
			MessageBox.Show("Supplier Field is Required");
			combsupplier.Focus();
			return false;
		}
		if (Conversion.ToInt((object)combgatepass.Text.Trim()) == 0)
		{
			MessageBox.Show("Gatepass Field is Required");
			combgatepass.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtsuppwt.Text.Trim()) == 0.0 || txtsuppwt.Text.Trim() == null)
		{
			MessageBox.Show("Supplier Weight Field is Required");
			txtsuppwt.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfctwt.Text.Trim()) == 0.0 || txtfctwt.Text.Trim() == null)
		{
			MessageBox.Show("Factory Weight Field is Required");
			txtfctwt.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)combitem).ActiveRow == null)
		{
			MessageBox.Show("Item Name Field is Required");
			combitem.Focus();
			return false;
		}
		if (((UltraGridBase)combcropyear).ActiveRow == null)
		{
			MessageBox.Show("Crop Year Field is Required");
			combcropyear.Focus();
			return false;
		}
		if (((UltraGridBase)combjoblot).ActiveRow == null)
		{
			MessageBox.Show("Job/Lot Field is Required");
			combjoblot.Focus();
			return false;
		}
		if (((UltraGridBase)combpcktyp).ActiveRow == null)
		{
			MessageBox.Show("Packing Type Field is Required");
			combpcktyp.Focus();
			return false;
		}
		if (txtqty.Text.Trim() == "" || Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtqty.Focus();
			return false;
		}
		if (((UltraGridBase)combpckuom).ActiveRow == null)
		{
			MessageBox.Show("Pack Unit Field is Required");
			combpckuom.Focus();
			return false;
		}
		if (txtgwt.Text.Trim() == "" || Conversion.ToDouble((object)txtgwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Gross Weight Field is Required");
			txtgwt.Focus();
			return false;
		}
		if (txtnetwt.Text.Trim() == "" || Conversion.ToDouble((object)txtnetwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Net Bill Weight Field is Required");
			txtnetwt.Focus();
			return false;
		}
		if (txtstockwt.Text.Trim() == "" || Conversion.ToDouble((object)txtstockwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Stock Weight Field is Required");
			txtstockwt.Focus();
			return false;
		}
		if (((UltraGridBase)combwr).ActiveRow == null)
		{
			MessageBox.Show("Warehouse Field is Required");
			combwr.Focus();
			return false;
		}
		return true;
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			int DocNo = Conversion.ToInt((object)InvGrn.GenerateInvGrnCode(new InvGrn
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 46
			}));
			txtdocno.Text = DocNo.ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchFill()
	{
		try
		{
			DataTable dtbra = CommonServices.BrancheServiceBind();
			if (dtbra.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtbra, combbrnch, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combbrnch).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dt = CommonServices.ProjectServiceBind();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, combproject, "Id", "ProjectName", "Project Name", false);
			}
			((UltraGridBase)combproject).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SupplierNameFilll()
	{
		try
		{
			dtsupbyGrpId = CommonServices.SupplierCustomerGetforComboServiceBind();
			if (dtsupbyGrpId.Rows.Count > 0)
			{
				DDL.BindDDL(dtsupbyGrpId, combsupplier, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				DDL.BindDDL(dtsupbyGrpId, combstockprty, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
				DDL.BindDDL(dtsupbyGrpId, combrefprty, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TransportFill()
	{
		try
		{
			dtAccountList = CommonServices.CoaAllocationGetAllServiceBind();
			if (dtAccountList.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("AccountTitle");
			for (int i = 0; i < dtAccountList.Rows.Count; i++)
			{
				if (Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtAccountList.Rows[i]["AccountTypeId"]) != 15)
				{
					dt.Rows.Add(dtAccountList.Rows[i]["Id"], dtAccountList.Rows[i]["AccountTitle"]);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTransport, "Id", "AccountTitle", "Supplier Name", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsettingPendingGatepass()
	{
		try
		{
			grdGp.RootTable.Columns["PoDetailId"].Visible = false;
			grdGp.RootTable.Columns["OrderId"].Visible = false;
			grdGp.RootTable.Columns["ItemId"].Visible = false;
			grdGp.RootTable.Columns["JobId"].Visible = false;
			grdGp.RootTable.Columns["PackingTypeId"].Visible = false;
			grdGp.RootTable.Columns["UOMId"].Visible = false;
			grdGp.RootTable.Columns["WtCut"].Visible = false;
			grdGp.RootTable.Columns["WtCutTotal"].Visible = false;
			grdGp.RootTable.Columns["AddLesswt"].Visible = false;
			grdGp.RootTable.Columns["StockWeight"].Visible = false;
			grdGp.RootTable.Columns["WareHouse"].Visible = false;
			grdGp.RootTable.Columns["City"].Visible = false;
			grdGp.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grdGp.RootTable.Columns["GpId"].Visible = false;
			grdGp.RootTable.Columns.Add("Load");
			grdGp.RootTable.Columns["Load"].Caption = "Load";
			grdGp.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdGp.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grdGp.RootTable.Columns["Load"].ButtonText = "Load";
			grdGp.RootTable.Columns["Load"].Width = 40;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void VehicleTypesFill()
	{
		try
		{
			DataTable dtVehicle = VehicleType.GetAll();
			if (dtVehicle.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtVehicle, combvehtyp, "Id", "VehicleDescription", "VehicleDescription", false);
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
			int num = Conversion.ToInt(comborderno.Value);
			DataTable dtpoId = new DataTable();
			dtpoId = PurchaseOrder.ReadByPurchaseOrderIDNOrderItemId(num);
			if (dtpoId.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtpoId, combitem, "OrderItemId", "ItemName", "Item Name", false);
				((UltraGridBase)combitem).DisplayLayout.Bands[0].Columns[1].Width = 500;
				((UltraGridBase)combitem).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CropYear()
	{
		try
		{
			dtCrop = CommonServices.CropYearGetAllService();
			if (dtCrop.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtCrop, combcropyear, "Id", "CropYear", "Crop Year", false);
				((UltraGridBase)combcropyear).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobLotFillWithoutOrder()
	{
		try
		{
			dtjob = CommonServices.JobLotGetAllService();
			if (dtjob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtjob, combjoblot, "Id", "JobLotDescription", "JobLot Description", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypeFill()
	{
		try
		{
			dtPack = InvPackingType.Getall();
			if (dtPack.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtPack, combpcktyp, "Id", "PackTypeDesc", "PackingType", false);
				((UltraGridBase)combpcktyp).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFillWithoutOrder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(combitem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, combpckuom, "Id", "Equivalent", "Item Pack UOM", false);
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
			dtwh = CommonServices.getActiveWareHouse();
			if (dtwh.Rows.Count > 0)
			{
				DDL.BindDDL(dtwh, combwr, "Id", "WareHouseName", "Godown Name", ZeroIndex: false);
				((UltraGridBase)combwr).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			City obj = new City();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtcity = City.GetAll(obj);
			if (dtcity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtcity, CmbCity, "Id", "CityName", "City Name", false);
				((UltraGridBase)CmbCity).Rows[1].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			GatePassInward val = new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				GpSrNo = Conversion.ToInt((object)combgatepass.Text.Trim()),
				DocumentTypeId = 51
			};
			DataTable dtgpForGRn = new DataTable();
			dtgpForGRn = GatePassInward.GetByGpNoForGRN(val);
			if (dtgpForGRn.Rows.Count > 0)
			{
				((Control)(object)combvehtyp).Enabled = false;
				txtvehno.Enabled = false;
				txtbltyno.Enabled = false;
				GPID = Conversion.ToInt(dtgpForGRn.Rows[0]["Id"]);
				((Control)(object)combvehtyp).Text = dtgpForGRn.Rows[0]["VehicleType"].ToString();
				txtvehno.Text = dtgpForGRn.Rows[0]["VehicleNo"].ToString();
				txtbltyno.Text = dtgpForGRn.Rows[0]["BiltyNo"].ToString();
				txtremarks.Text = dtgpForGRn.Rows[0]["OtherRemarks"].ToString();
				txtcarramount.Text = dtgpForGRn.Rows[0]["NetPaid"].ToString();
				txtqty.Text = dtgpForGRn.Rows[0]["NoOfPackages"].ToString();
				txtsuppwt.Text = dtgpForGRn.Rows[0]["SupplierWeight"].ToString();
				txtfctwt.Text = dtgpForGRn.Rows[0]["FactoryWeight"].ToString();
				txtwtdiff.Text = dtgpForGRn.Rows[0]["DifferenceWeight"].ToString();
				((Control)(object)combwr).Text = dtgpForGRn.Rows[0]["WareHouseName"].ToString();
				DataTable dt = new DataTable();
				dt.Columns.Add("PurchaseOrderId");
				dt.Columns.Add("SupplierContractCode");
				dt.Clear();
				for (int i = 0; i < dtgpForGRn.Rows.Count; i++)
				{
					dt.Rows.Add(dtgpForGRn.Rows[i]["PurchaseOrderId"], dtgpForGRn.Rows[i]["SupplierContractCode"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, comborderno, "PurchaseOrderId", "SupplierContractCode", "Order No", ZeroIndex: false);
					((UltraGridBase)comborderno).Rows[0].Activate();
					comborderno_Leave(null, null);
				}
			}
			else
			{
				((Control)(object)combvehtyp).Enabled = true;
				txtvehno.Enabled = true;
				txtbltyno.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFillWithLabAnaylsis()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		try
		{
			GatePassInward val = new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				GpSrNo = Conversion.ToInt((object)combgatepass.Text.Trim()),
				DocumentTypeId = 51
			};
			DataTable dtgpForGRn = new DataTable();
			dtgpForGRn = GatePassInward.ReadByGpNoForGRNWithLabAttach(val);
			if (dtgpForGRn.Rows.Count > 0)
			{
				((Control)(object)combvehtyp).Enabled = false;
				txtvehno.Enabled = false;
				txtbltyno.Enabled = false;
				GPID = Conversion.ToInt(dtgpForGRn.Rows[0]["Id"]);
				((Control)(object)combvehtyp).Text = dtgpForGRn.Rows[0]["VehicleType"].ToString();
				txtvehno.Text = dtgpForGRn.Rows[0]["VehicleNo"].ToString();
				txtbltyno.Text = dtgpForGRn.Rows[0]["BiltyNo"].ToString();
				txtremarks.Text = dtgpForGRn.Rows[0]["OtherRemarks"].ToString();
				txtcarramount.Text = dtgpForGRn.Rows[0]["NetPaid"].ToString();
				txtqty.Text = dtgpForGRn.Rows[0]["NoOfPackages"].ToString();
				txtsuppwt.Text = dtgpForGRn.Rows[0]["SupplierWeight"].ToString();
				txtfctwt.Text = dtgpForGRn.Rows[0]["NetWbWeight"].ToString();
				txtAnaylstName.Text = dtgpForGRn.Rows[0]["AnalystName"].ToString();
				txtDeductionKg.Text = dtgpForGRn.Rows[0]["DeductionWeight"].ToString();
				txtwtcut.Text = dtgpForGRn.Rows[0]["DeductionWeight"].ToString();
				((Control)(object)combwr).Text = dtgpForGRn.Rows[0]["WareHouseName"].ToString();
				DataTable dt = new DataTable();
				dt.Columns.Add("PurchaseOrderId");
				dt.Columns.Add("SupplierContractCode");
				dt.Clear();
				for (int i = 0; i < dtgpForGRn.Rows.Count; i++)
				{
					dt.Rows.Add(dtgpForGRn.Rows[i]["PurchaseOrderId"], dtgpForGRn.Rows[i]["SupplierContractCode"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, comborderno, "PurchaseOrderId", "SupplierContractCode", "Order No", ZeroIndex: false);
					((UltraGridBase)comborderno).Rows[0].Activate();
					comborderno_Leave(null, null);
				}
			}
			else
			{
				((Control)(object)combvehtyp).Enabled = true;
				txtvehno.Enabled = true;
				txtbltyno.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GatePassRecordFillWinthoutPurchaseorder()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			GatePassInward val = new GatePassInward
			{
				GpSrNo = Conversion.ToInt((object)combgatepass.Text.Trim())
			};
			DataTable dtGpgrn = new DataTable();
			dtGpgrn = GatePassInward.GetByGpNoForGRN(val);
			if (dtGpgrn.Rows.Count > 0)
			{
				GPID = Conversion.ToInt(dtGpgrn.Rows[0]["Id"]);
				((Control)(object)combvehtyp).Text = dtGpgrn.Rows[0]["VehicleType"].ToString();
				txtvehno.Text = dtGpgrn.Rows[0]["VehicleNo"].ToString();
				txtbltyno.Text = dtGpgrn.Rows[0]["BiltyNo"].ToString();
				txtremarks.Text = dtGpgrn.Rows[0]["OtherRemarks"].ToString();
				txtcarramount.Text = dtGpgrn.Rows[0]["NetPaid"].ToString();
				txtqty.Text = dtGpgrn.Rows[0]["NoOfPackages"].ToString();
				txtsuppwt.Text = dtGpgrn.Rows[0]["SupplierWeight"].ToString();
				txtfctwt.Text = dtGpgrn.Rows[0]["FactoryWeight"].ToString();
				txtwtdiff.Text = dtGpgrn.Rows[0]["DifferenceWeight"].ToString();
				((Control)(object)combwr).Text = dtGpgrn.Rows[0]["WareHouseName"].ToString();
				DataTable dt = new DataTable();
				dt.Columns.Add("SupplierCustomerId");
				dt.Columns.Add("CompanyName");
				dt.Clear();
				for (int i = 0; i < dtGpgrn.Rows.Count; i++)
				{
					dt.Rows.Add(dtGpgrn.Rows[i]["SupplierCustomerId"], dtGpgrn.Rows[i]["CompanyName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, combsupplier, "SupplierCustomerId", "CompanyName", "Supplier Name", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combitem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (comborderno.Value != null)
			{
				PackUOMFillWithoutOrder();
				if (txtDeliverTerm.Text != "Ponch")
				{
					txtgwt.Text = txtsuppwt.Text.Trim();
					txtGrnWeight.Text = txtsuppwt.Text.Trim();
				}
				if (txtDeliverTerm.Text == "Ponch")
				{
					double SupplierWeight = double.Parse(txtsuppwt.Text.Trim());
					double FactoryWeight = double.Parse(txtfctwt.Text.Trim());
					if (SupplierWeight > FactoryWeight)
					{
						txtgwt.Text = FactoryWeight.ToString();
						txtGrnWeight.Text = FactoryWeight.ToString();
					}
					else
					{
						txtgwt.Text = SupplierWeight.ToString();
						txtGrnWeight.Text = SupplierWeight.ToString();
					}
				}
				DataTable dtPOI = new DataTable();
				dtPOI = PurchaseOrder.ReadByPurchaseOrderIDNOrderItemId(Conversion.ToInt(comborderno.Value));
				if (dtPOI.Rows.Count > 0)
				{
					combpckuom.Value = dtPOI.Rows[0]["OrderItemUOMId"].ToString();
					((Control)(object)combjoblot).Text = dtPOI.Rows[0]["JobLotDescription"].ToString();
					if (Conversion.ToDouble(dtPOI.Rows[0]["BagWeight"]) > 0.0)
					{
						txtebu.Text = Math.Round(Conversion.ToDouble(dtPOI.Rows[0]["BagWeight"]), 3).ToString();
					}
					((Control)(object)CmbCity).Text = dtPOI.Rows[0]["CityArea"].ToString();
					comblabno.Text = dtPOI.Rows[0]["LabSampleNo"].ToString();
				}
			}
			else
			{
				double SupplierWeight2 = 0.0;
				double FactoryWeight2 = 0.0;
				if (txtsuppwt.Text.Trim() != string.Empty)
				{
					SupplierWeight2 = double.Parse(txtsuppwt.Text.Trim());
				}
				if (txtfctwt.Text.Trim() != string.Empty)
				{
					FactoryWeight2 = double.Parse(txtfctwt.Text.Trim());
				}
				if (SupplierWeight2 > FactoryWeight2)
				{
					txtgwt.Text = FactoryWeight2.ToString();
					txtGrnWeight.Text = FactoryWeight2.ToString();
				}
				else
				{
					txtgwt.Text = SupplierWeight2.ToString();
					txtGrnWeight.Text = SupplierWeight2.ToString();
				}
			}
			TotalQty = Conversion.ToDouble((object)txtqty.Text);
			TotalGrossWeight = Conversion.ToDouble((object)txtgwt.Text);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comborderno_Leave(object sender, EventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		try
		{
			if (comborderno.Value != null && Conversion.ToInt(comborderno.Value) != 0)
			{
				DataTable dtsPONo = new DataTable();
				dtsPONo = SupplierCustomer.GetSupplierByPurchaseOrderNo(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId,
					OrderId = Conversion.ToInt((object)((Control)(object)comborderno).Text),
					DocumentTypeId = 41,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id
				});
				if (dtsPONo.Rows.Count > 0)
				{
					txtDeliverTerm.Text = Conversion.ToString(dtsPONo.Rows[0]["DeliveryTerm"]);
					DDL.BindDDLNew(dtsPONo, combsupplier, "Id", "CompanyName", "Supplier Name", false);
					((UltraGridBase)combsupplier).Rows[0].Activate();
					ItemNameFill();
				}
				else
				{
					MessageBox.Show("Supplier Name Not Found");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetAllDataForLoader()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtAllPendingData = GatePassInward.GpNoBySupplyOrderPendingForGrn(new GatePassInward
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtAllPendingData.Rows.Count > 0)
			{
				grdGp.DataSource = dtAllPendingData;
				grdGp.RetrieveStructure();
				grdsettingPendingGatepass();
			}
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
			double Qty = 0.0;
			double GrossWeight = 0.0;
			double EBUnit = 0.0;
			double EBTotal = 0.0;
			double WeightCut = 0.0;
			double WeightCutTotal = 0.0;
			string AddLess = "0";
			double FactoryWeight = 0.0;
			double SupplierWeight = 0.0;
			if (txtfctwt.Text != string.Empty && txtfctwt.Text != null)
			{
				FactoryWeight = double.Parse(txtfctwt.Text.Trim().ToString());
			}
			if (txtsuppwt.Text != string.Empty && txtsuppwt.Text != null)
			{
				SupplierWeight = double.Parse(txtsuppwt.Text.Trim().ToString());
			}
			if (((Control)(object)combpckuom).Text != string.Empty)
			{
				double.Parse(((Control)(object)combpckuom).Text.Trim().ToString());
			}
			if (txtqty.Text != string.Empty && txtqty.Text != null)
			{
				Qty = double.Parse(txtqty.Text.Trim().ToString());
			}
			if (txtgwt.Text != string.Empty && txtgwt.Text != null)
			{
				GrossWeight = double.Parse(txtgwt.Text.Trim().ToString());
			}
			if (txtebu.Text != string.Empty && txtebu.Text != null)
			{
				EBUnit = double.Parse(txtebu.Text.Trim().ToString());
			}
			if (EBUnit > 0.0 && Qty > 0.0)
			{
				EBTotal = EBUnit * Qty;
			}
			txtEmptyBagsTotal.Text = Math.Round(EBTotal, 2).ToString();
			if (txtwtcut.Text != string.Empty && txtwtcut.Text != null)
			{
				WeightCut = double.Parse(txtwtcut.Text.Trim().ToString());
				txtwtcuttotal.Text = (Qty * WeightCut).ToString();
			}
			else
			{
				txtwtcuttotal.Text = "0";
			}
			if (txtwtcuttotal.Text != string.Empty && txtwtcuttotal.Text != null)
			{
				WeightCutTotal = double.Parse(txtwtcuttotal.Text.Trim().ToString());
			}
			AddLess = ((!(txtadlswt.Text != string.Empty) || txtadlswt.Text == null) ? "0" : txtadlswt.Text.Trim().ToString());
			if (txtnetwt.Text != string.Empty && txtnetwt.Text != null)
			{
				double.Parse(txtnetwt.Text.Trim().ToString());
			}
			double Weight = Math.Round(GrossWeight, 2) - EBTotal - Math.Round(WeightCutTotal, 2) + double.Parse(AddLess);
			txtnetwt.Text = Math.Round(Weight, 2).ToString();
			if (SupplierWeight > 0.0 && FactoryWeight > 0.0 && GrossWeight > 0.0)
			{
				double StockWt = Conversion.ToDouble((object)(FactoryWeight * (GrossWeight / SupplierWeight * 100.0) / 100.0));
				StockWt = StockWt - EBTotal - WeightCutTotal + double.Parse(AddLess);
				txtstockwt.Text = Math.Round(StockWt, 2).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalWeight()
	{
		try
		{
			double PackUOM = 0.0;
			double Qty = 0.0;
			double GrossWeight = 0.0;
			if (((Control)(object)combpckuom).Text != string.Empty)
			{
				PackUOM = double.Parse(((Control)(object)combpckuom).Text.ToString());
			}
			if (txtqty.Text != string.Empty)
			{
				Qty = double.Parse(txtqty.Text.ToString());
			}
			if (txtgwt.Text != string.Empty)
			{
				GrossWeight = double.Parse(txtgwt.Text.ToString());
			}
			txtgwt.Text = (PackUOM * Qty).ToString();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalSupplierWeight()
	{
		try
		{
			if (txtsuppwt.Text != string.Empty && txtfctwt.Text != string.Empty)
			{
				double num = double.Parse(txtsuppwt.Text.Trim());
				double FactoryWeight = double.Parse(txtfctwt.Text.Trim());
				txtwtdiff.Text = (num - FactoryWeight).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MarkeetPurchaseFormula()
	{
		try
		{
			if ((Conversion.ToDouble((object)txtqty.Text.Trim()) == 0.0 || txtqty.Text.Trim() == string.Empty) && txtGrnWeight.Text.Trim() != string.Empty && txtGrnWeight.Text.Trim() != "0" && ((Control)(object)combpckuom).Text.Trim() != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtGrnWeight.Text.Trim());
				double PackUom = Conversion.ToDouble((object)((Control)(object)combpckuom).Text.Trim());
				double Qty = num / PackUom;
				txtqty.Text = Math.Round(Qty).ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void StockWeightCalculations()
	{
		try
		{
			double FactoryWeight = 0.0;
			double SupplierWeight = 0.0;
			if (txtfctwt.Text != string.Empty && txtfctwt.Text != null)
			{
				FactoryWeight = double.Parse(txtfctwt.Text.Trim().ToString());
			}
			if (txtsuppwt.Text != string.Empty && txtsuppwt.Text != null)
			{
				SupplierWeight = double.Parse(txtsuppwt.Text.Trim().ToString());
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				item.BeginEdit();
				double GrossWeight = Conversion.ToDouble(item.Cells["GrossWight"].Value);
				if (SupplierWeight > 0.0 && FactoryWeight > 0.0 && GrossWeight > 0.0)
				{
					double StockWt = Conversion.ToDouble((object)(FactoryWeight * (GrossWeight / SupplierWeight * 100.0) / 100.0));
					item.Cells["StockWeight"].Value = Math.Round(StockWt, 2).ToString();
				}
				item.BeginEdit();
			}
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
			GPID = 0;
			txtGrnWeight.Text = string.Empty;
			((Control)(object)combsupplier).Text = string.Empty;
			((UltraGridBase)combsupplier).DataSource = null;
			((Control)(object)combstockprty).Text = string.Empty;
			((Control)(object)combrefprty).Text = string.Empty;
			txtAnaylstName.Text = string.Empty;
			txtDeductionKg.Text = string.Empty;
			((Control)(object)CmbTransport).Text = string.Empty;
			txtcarramount.Text = string.Empty;
			txtsuppwt.Text = string.Empty;
			txtfctwt.Text = string.Empty;
			txtwtdiff.Text = string.Empty;
			combgatepass.Text = string.Empty;
			((UltraGridBase)combvehtyp).Rows[0].Activate();
			txtvehno.Text = string.Empty;
			txtbltyno.Text = string.Empty;
			txtremarks.Text = string.Empty;
			((UltraGridBase)comborderno).DataSource = null;
			((Control)(object)comborderno).Text = string.Empty;
			((UltraGridBase)combitem).DataSource = null;
			((Control)(object)combitem).Text = string.Empty;
			txtqty.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			comblabno.Text = string.Empty;
			((Control)(object)CmbCity).Text = string.Empty;
			txtwtcut.Text = string.Empty;
			((Control)(object)combpckuom).Text = string.Empty;
			btnSave.Visible = true;
			DocDate.Focus();
			dtdetail.Clear();
			grd.DataSource = dtdetail;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			((Control)(object)comborderno).Enabled = true;
			((Control)(object)combsupplier).Enabled = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			GenerateCode();
			GetAllDataForLoader();
			txtgwt.Enabled = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetial()
	{
		try
		{
			txtqty.Text = string.Empty;
			txtgwt.Text = string.Empty;
			txtebu.Text = string.Empty;
			txtEmptyBagsTotal.Text = string.Empty;
			txtadlswt.Text = string.Empty;
			txtnetwt.Text = string.Empty;
			txtstockwt.Text = string.Empty;
			((Control)(object)combpckuom).Text = string.Empty;
			comblabno.Text = string.Empty;
			combitem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["PoDetailId"].Visible = false;
			grd.RootTable.Columns["OrderId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["UOMId"].Visible = false;
			grd.RootTable.Columns["WtCut"].Visible = false;
			grd.RootTable.Columns["AddLesswt"].Visible = false;
			grd.RootTable.Columns["WtCutTotal"].Visible = false;
			grd.RootTable.Columns["Order"].EditType = (EditType)0;
			grd.RootTable.Columns["Item"].EditType = (EditType)0;
			grd.RootTable.Columns["UOM"].EditType = (EditType)0;
			grd.RootTable.Columns["Qty"].EditType = (EditType)0;
			grd.RootTable.Columns["GrossWight"].EditType = (EditType)0;
			grd.RootTable.Columns["EbUnit"].EditType = (EditType)0;
			grd.RootTable.Columns["EbTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["StockWeight"].EditType = (EditType)0;
			grd.RootTable.Columns["PoDetailId"].EditType = (EditType)0;
			grd.RootTable.Columns["OrderId"].EditType = (EditType)0;
			grd.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grd.RootTable.Columns["UOMId"].EditType = (EditType)0;
			grd.RootTable.Columns["AddLesswt"].EditType = (EditType)0;
			grd.RootTable.Columns["WtCut"].EditType = (EditType)0;
			grd.RootTable.Columns["WtCutTotal"].EditType = (EditType)0;
			grd.RootTable.Columns["CropYear"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["JobId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["PackingTypeId"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["WareHouse"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["City"].FilterEditType = (FilterEditType)1;
			grd.RootTable.Columns["WareHouse"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["WareHouse"].HasValueList = true;
			grd.RootTable.Columns["WareHouse"].LimitToList = true;
			grd.RootTable.Columns["WareHouse"].EditType = (EditType)4;
			grd.RootTable.Columns["WareHouse"].Caption = "WareHouse Name";
			grd.RootTable.Columns["WareHouse"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			grd.RootTable.Columns["WareHouse"].Width = 150;
			grd.RootTable.Columns["PackingTypeId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["PackingTypeId"].HasValueList = true;
			grd.RootTable.Columns["PackingTypeId"].LimitToList = true;
			grd.RootTable.Columns["PackingTypeId"].EditType = (EditType)4;
			grd.RootTable.Columns["PackingTypeId"].Caption = "PackingType";
			grd.RootTable.Columns["PackingTypeId"].ValueList.PopulateValueList((IEnumerable)dtPack.DefaultView, "Id", "PackTypeDesc");
			grd.RootTable.Columns["JobId"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["JobId"].HasValueList = true;
			grd.RootTable.Columns["JobId"].LimitToList = true;
			grd.RootTable.Columns["JobId"].EditType = (EditType)4;
			grd.RootTable.Columns["JobId"].Caption = "Job/Lot";
			grd.RootTable.Columns["JobId"].ValueList.PopulateValueList((IEnumerable)dtjob.DefaultView, "Id", "JobLotDescription");
			grd.RootTable.Columns["City"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["City"].HasValueList = true;
			grd.RootTable.Columns["City"].LimitToList = true;
			grd.RootTable.Columns["City"].EditType = (EditType)4;
			grd.RootTable.Columns["City"].Caption = "City Name";
			grd.RootTable.Columns["City"].ValueList.PopulateValueList((IEnumerable)dtcity.DefaultView, "Id", "CityName");
			grd.RootTable.Columns["CropYear"].ColumnType = (ColumnType)0;
			grd.RootTable.Columns["CropYear"].HasValueList = true;
			grd.RootTable.Columns["CropYear"].LimitToList = true;
			grd.RootTable.Columns["CropYear"].EditType = (EditType)4;
			grd.RootTable.Columns["CropYear"].Caption = "CropYear";
			grd.RootTable.Columns["CropYear"].ValueList.PopulateValueList((IEnumerable)dtCrop.DefaultView, "Id", "CropYear");
			grd.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["GrossWight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["EbTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WtCut"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["WtCutTotal"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["AddLesswt"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["StockWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Qty"].FormatString = "#,##.##";
			grd.RootTable.Columns["GrossWight"].FormatString = "#,##.##";
			grd.RootTable.Columns["EbUnit"].FormatString = "#,##.##";
			grd.RootTable.Columns["EbTotal"].FormatString = "#,##.##";
			grd.RootTable.Columns["WtCut"].FormatString = "#,##.##";
			grd.RootTable.Columns["WtCutTotal"].FormatString = "#,##.##";
			grd.RootTable.Columns["AddLesswt"].FormatString = "#,##.##";
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##.##";
			grd.RootTable.Columns["StockWeight"].FormatString = "#,##.##";
			grd.RootTable.Columns["Qty"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["GrossWight"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["EbUnit"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["EbTotal"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["WtCut"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["WtCutTotal"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["AddLesswt"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##.##";
			grd.RootTable.Columns["StockWeight"].TotalFormatString = "#,##.##";
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		try
		{
			List<tblUserRights> lstRights = tblUserRights.GetByUserId(new tblUserRights
			{
				UserId = UserAccount.ID,
				ScreenName = base.Name,
				RightName = UserAccount.RoleName
			});
			if (lstRights.Count > 0)
			{
				List<tblUserRights> newList = lstRights.Where((tblUserRights x) => x.RightName == "Save").ToList();
				DoHaveSaveRight = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Print").ToList();
				DoHavePrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Update").ToList();
				DoHaveUpdateRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Field Chooser").ToList();
				DoHaveFieldChooserRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Save Layout").ToList();
				DoHaveSaveLayoutRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Grid Print").ToList();
				DoHaveGridPrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Grid Export").ToList();
				DoHaveGridExportRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Group Collapse").ToList();
				DoHaveGridGroupCollapseRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Group Expand").ToList();
				DoHaveGridGroupExpandRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Report Export").ToList();
				DoHaveReportExportRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "Report Print").ToList();
				DoHaveReportPrintRights = Conversion.ToBool((object)newList[0].Value.ToString());
				newList = lstRights.Where((tblUserRights x) => x.RightName == "CanView AllRecord").ToList();
				DoHaveCanViewAllRecordRights = Conversion.ToBool((object)newList[0].Value.ToString());
			}
			btnSave.Enabled = DoHaveSaveRight;
			printToolStripButton.Enabled = DoHavePrintRights;
			btnUpdate.Enabled = DoHaveUpdateRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = DoHaveGridGroupExpandRights;
			dtViewAttachments.Columns.Add("AttchmentPath");
			dtdetail.Columns.Add("PoDetailId");
			dtdetail.Columns.Add("OrderId");
			dtdetail.Columns.Add("Order");
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("Item");
			dtdetail.Columns.Add("CropYear");
			dtdetail.Columns.Add("JobId");
			dtdetail.Columns.Add("PackingTypeId");
			dtdetail.Columns.Add("UOM");
			dtdetail.Columns.Add("UOMId");
			dtdetail.Columns.Add("Qty", typeof(double));
			dtdetail.Columns.Add("GrossWight", typeof(double));
			dtdetail.Columns.Add("EbUnit", typeof(double));
			dtdetail.Columns.Add("EbTotal", typeof(double));
			dtdetail.Columns.Add("WtCut", typeof(double));
			dtdetail.Columns.Add("WtCutTotal", typeof(double));
			dtdetail.Columns.Add("AddLesswt", typeof(double));
			dtdetail.Columns.Add("NetWeight", typeof(double));
			dtdetail.Columns.Add("StockWeight", typeof(double));
			dtdetail.Columns.Add("WareHouse");
			dtdetail.Columns.Add("City");
			grd.DataSource = dtdetail;
			dtpo.Columns.Add("Id");
			dtpo.Columns.Add("DocNo");
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("ItemName");
			dtjob.Columns.Add("Id");
			dtjob.Columns.Add("JobLotDescription");
			dtPack.Columns.Add("Id");
			dtPack.Columns.Add("PackTypeDesc");
			dtUOM.Columns.Add("Id");
			dtUOM.Columns.Add("Equivalent");
			dtwh.Columns.Add("Id");
			dtwh.Columns.Add("WareHouseName");
			dtcity.Columns.Add("Id");
			dtcity.Columns.Add("CityName");
			dtCrop.Columns.Add("Id");
			dtCrop.Columns.Add("CropYear");
			BranchFill();
			ProjectFill();
			GenerateCode();
			TransportFill();
			VehicleTypesFill();
			CropYear();
			GetAllDataForLoader();
			JobLotFillWithoutOrder();
			PackingTypeFill();
			WarehouseFill();
			CityFill();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			DocDate.Focus();
			ChkBox.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				CanViewAllRecord = DoHaveCanViewAllRecordRights,
				NoOfRecords = NoOfRecords,
				DocumentTypeId = 46,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dthis = new DataTable();
			dthis = InvGrn.HistoryForMarketGrn(val);
			if (dthis.Rows.Count > 0)
			{
				GrdHistory.DataSource = dthis;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
			}
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
			GrdHistory.RootTable.Columns["OrderNo"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns.Add("Detail");
			GrdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			GrdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].Image = Resources.icons8_pencil_16;
			GrdHistory.RootTable.Columns["Edit"].Width = 25;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].Image = Resources.icons8_print_16;
			GrdHistory.RootTable.Columns["Print"].Width = 25;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		if (btnUpdate.Visible && btnUpdate.Enabled && e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
		{
			btnUpdate_Click(null, null);
		}
		if (printToolStripButton.Visible && printToolStripButton.Enabled && e.Control && e.KeyCode == Keys.P)
		{
			printToolStripButton_Click(null, null);
		}
		if (e.Control && e.KeyCode == Keys.F5)
		{
			combitem.Focus();
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)GrdHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)GrdHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grd).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
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

	private void btnNew_Click(object sender, EventArgs e)
	{
		reset();
	}

	private void txtqty_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebu_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells[1].Value.ToString());
			ReadById(Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void combgatepass_Leave(object sender, EventArgs e)
	{
		try
		{
			List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "POCompulsoryforGrn").ToList();
			if (lst.Count <= 0)
			{
				return;
			}
			if (bool.Parse(lst[0].ConfigKey.ToString()))
			{
				List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "LabCompulsoryForWeighBridge").ToList();
				if (list.Count > 0)
				{
					if (bool.Parse(list[0].ConfigKey.ToString()))
					{
						GatePassRecordFillWithLabAnaylsis();
					}
					else
					{
						GatePassRecordFill();
					}
				}
			}
			else
			{
				((Control)(object)comborderno).Enabled = false;
				GatePassRecordFillWinthoutPurchaseorder();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				reset();
				Id = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells[1].Value.ToString());
				ReadById(Id);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.GrnSlipReport211(46, Conversion.ToInt(GrdHistory.CurrentRow.Cells[1].Value));
			}
			if (e.Column.Key == "Detail")
			{
				InvGrn invGrn = InvGrn.GetByID(Conversion.ToInt(GrdHistory.CurrentRow.Cells[1].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("Order");
				dthistorydetail.Columns.Add("Item");
				dthistorydetail.Columns.Add("CropYear");
				dthistorydetail.Columns.Add("Job");
				dthistorydetail.Columns.Add("PackingType");
				dthistorydetail.Columns.Add("UOM");
				dthistorydetail.Columns.Add("Qty", typeof(double));
				dthistorydetail.Columns.Add("GrossWight", typeof(double));
				dthistorydetail.Columns.Add("EbUnit", typeof(double));
				dthistorydetail.Columns.Add("EbTotal", typeof(double));
				dthistorydetail.Columns.Add("WtCut", typeof(double));
				dthistorydetail.Columns.Add("WtCutTotal", typeof(double));
				dthistorydetail.Columns.Add("AddLesswt", typeof(double));
				dthistorydetail.Columns.Add("NetWeight", typeof(double));
				dthistorydetail.Columns.Add("StockWeight", typeof(double));
				dthistorydetail.Columns.Add("WareHouse");
				dthistorydetail.Columns.Add("City");
				dthistorydetail.Clear();
				for (int i = 0; i < invGrn.invGrnDetail.Count; i++)
				{
					dthistorydetail.Rows.Add(invGrn.invGrnDetail[i].PurchaseOrder, invGrn.invGrnDetail[i].Item, invGrn.invGrnDetail[i].CropYear, invGrn.invGrnDetail[i].JobLot, invGrn.invGrnDetail[i].PackingType, "", invGrn.invGrnDetail[i].ItemQty, invGrn.invGrnDetail[i].GrossWeight, invGrn.invGrnDetail[i].EBWPerUnit, invGrn.invGrnDetail[i].EBWTotal, invGrn.invGrnDetail[i].WtCut, invGrn.invGrnDetail[i].WtCutTotal, invGrn.invGrnDetail[i].AdLsWeight, invGrn.invGrnDetail[i].NetBillWeight, invGrn.invGrnDetail[i].StockWeight, invGrn.invGrnDetail[i].WarehouseId, invGrn.invGrnDetail[i].AreaCity);
				}
				GrdHistoryDetail.DataSource = dthistorydetail;
				GrdHistoryDetail.RetrieveStructure();
				gridhistorydetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistorydetail()
	{
		try
		{
			GrdHistoryDetail.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbUnit"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["GrossWight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["EbTotal"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["WtCut"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["WtCutTotal"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["StockWeight"].AggregateFunction = (AggregateFunction)2;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].ColumnType = (ColumnType)0;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].HasValueList = true;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].LimitToList = true;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].EditType = (EditType)4;
			GrdHistoryDetail.RootTable.Columns["WareHouse"].Caption = "WareHouse Name";
			GrdHistoryDetail.RootTable.Columns["WareHouse"].ValueList.PopulateValueList((IEnumerable)dtwh.DefaultView, "Id", "WareHouseName");
			GrdHistoryDetail.RootTable.Columns["Qty"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["EbUnit"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["GrossWight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["EbTotal"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["WtCut"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["WtCutTotal"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["AddLesswt"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["NetWeight"].FormatString = "0,0";
			GrdHistoryDetail.RootTable.Columns["StockWeight"].FormatString = "0,0";
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
			HistoryGridFill(50);
		}
	}

	private void combpckuom_Leave(object sender, EventArgs e)
	{
		Total();
		MarkeetPurchaseFormula();
	}

	private void txtgwt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtadlswt_Leave(object sender, EventArgs e)
	{
		Total();
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				DataTable dt = new DataTable();
				dt.Columns.Add("OrderId");
				dt.Columns.Add("Order");
				dt.Clear();
				dt.Rows.Add(item.Cells["OrderId"].Value.ToString(), item.Cells["Order"].Text.ToString());
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, comborderno, "OrderId", "Order", "Order No", ZeroIndex: false);
					((UltraGridBase)comborderno).Rows[0].Activate();
					comborderno_Leave(null, null);
				}
				combitem.Value = item.Cells["ItemId"].Value.ToString();
				((Control)(object)combcropyear).Text = item.Cells["CropYear"].Text.ToString();
				combjoblot.Value = item.Cells["JobId"].Value.ToString();
				combpcktyp.Value = item.Cells["PackingTypeId"].Value.ToString();
				txtqty.Text = item.Cells["Qty"].Value.ToString();
				combpckuom.Value = item.Cells["UOMId"].Value.ToString();
				txtgwt.Text = item.Cells["GrossWight"].Value.ToString();
				txtebu.Text = item.Cells["EbUnit"].Value.ToString();
				txtEmptyBagsTotal.Text = item.Cells["EbTotal"].Value.ToString();
				txtwtcut.Text = item.Cells["WtCut"].Value.ToString();
				txtwtcuttotal.Text = item.Cells["WtCutTotal"].Value.ToString();
				txtadlswt.Text = item.Cells["AddLesswt"].Value.ToString();
				txtnetwt.Text = item.Cells["NetWeight"].Value.ToString();
				txtstockwt.Text = item.Cells["StockWeight"].Value.ToString();
				combwr.Value = item.Cells["WareHouse"].Value.ToString();
				((Control)(object)CmbCity).Text = item.Cells["City"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
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
			List<ConfigrationsAllocation> lst = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "POCompulsoryforGrn").ToList();
			if (lst.Count > 0 && bool.Parse(lst[0].ConfigKey.ToString()) && (((Control)(object)comborderno).Text.Trim() == "" || int.Parse(comborderno.Value.ToString()) == 0 || ((Control)(object)comborderno).Text.Trim() == "0"))
			{
				MessageBox.Show("Please Select an Order No First");
				comborderno.Focus();
			}
			else if (FormValidationDetail())
			{
				double num = Conversion.ToDouble((object)txtstockwt.Text.Trim());
				double NetWeight = Conversion.ToDouble((object)txtnetwt.Text.Trim());
				double GrossWeight = Conversion.ToDouble((object)txtGrnWeight.Text.Trim());
				if (num > GrossWeight)
				{
					throw new Exception("StockWeight cannot greater than Gross Weight Please check");
				}
				if (NetWeight > GrossWeight)
				{
					throw new Exception("NetBillWeight cannot greater than Gross Weight Please check");
				}
				dtdetail.Rows[updateDetailIndex]["OrderId"] = comborderno.Value;
				dtdetail.Rows[updateDetailIndex]["Order"] = ((Control)(object)comborderno).Text;
				dtdetail.Rows[updateDetailIndex]["ItemId"] = combitem.Value;
				dtdetail.Rows[updateDetailIndex]["Item"] = ((Control)(object)combitem).Text;
				dtdetail.Rows[updateDetailIndex]["CropYear"] = combcropyear.Value;
				dtdetail.Rows[updateDetailIndex]["JobId"] = combjoblot.Value;
				dtdetail.Rows[updateDetailIndex]["PackingTypeId"] = combpcktyp.Value;
				dtdetail.Rows[updateDetailIndex]["Qty"] = txtqty.Text;
				dtdetail.Rows[updateDetailIndex]["UOM"] = ((Control)(object)combpckuom).Text;
				dtdetail.Rows[updateDetailIndex]["UOMId"] = combpckuom.Value;
				dtdetail.Rows[updateDetailIndex]["GrossWight"] = txtgwt.Text;
				dtdetail.Rows[updateDetailIndex]["EbUnit"] = txtebu.Text;
				dtdetail.Rows[updateDetailIndex]["EbTotal"] = txtEmptyBagsTotal.Text;
				dtdetail.Rows[updateDetailIndex]["WtCut"] = txtwtcut.Text;
				dtdetail.Rows[updateDetailIndex]["WtCutTotal"] = txtwtcuttotal.Text;
				dtdetail.Rows[updateDetailIndex]["AddLesswt"] = txtadlswt.Text;
				dtdetail.Rows[updateDetailIndex]["NetWeight"] = txtnetwt.Text;
				dtdetail.Rows[updateDetailIndex]["StockWeight"] = txtstockwt.Text;
				dtdetail.Rows[updateDetailIndex]["WareHouse"] = combwr.Value;
				dtdetail.Rows[updateDetailIndex]["City"] = CmbCity.Value;
				Add.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetial();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtsuppwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void txtfctwt_TextChanged(object sender, EventArgs e)
	{
		TotalSupplierWeight();
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			GetAllDataForLoader();
			SupplierNameFilll();
			TransportFill();
			VehicleTypesFill();
			CropYear();
			JobLotFillWithoutOrder();
			PackingTypeFill();
			WarehouseFill();
			CityFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport()
	{
		try
		{
			CommonServices.GrnSlipReport211(46, Id);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void txtEbWtOther_TextChanged(object sender, EventArgs e)
	{
		Total();
	}

	private void txtebu_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void CityDefine_Click(object sender, EventArgs e)
	{
		new DefineCity(UserAccount).Show();
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		Add.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
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

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "OrderNo")
			{
				PurchsaeOrder obj = new PurchsaeOrder(UserAccount);
				if (clsGlobalVariables.ScreenViewReights.Count > 0)
				{
					if (!bool.Parse(clsGlobalVariables.ScreenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "PurchsaeOrder").ToList()[0].Value.ToString()))
					{
						MessageBox.Show("Please Check Screen Rights");
						return;
					}
					obj.Show();
					obj.ReadById(Conversion.ToInt(GrdHistory.CurrentRow.Cells[2].Value));
				}
			}
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
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

	private void grdGp_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (!btnSave.Visible || !btnSave.Enabled || !(e.Column.Key == "Load"))
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("SupplierName");
			GPID = Conversion.ToInt(grdGp.CurrentRow.Cells["GpId"].Value);
			int GpId = Conversion.ToInt(grdGp.CurrentRow.Cells["GpId"].Value);
			combgatepass.Text = Conversion.ToString(grdGp.CurrentRow.Cells["GpSrNo"].Value);
			txtvehno.Text = Conversion.ToString(grdGp.CurrentRow.Cells["VehicleNo"].Value);
			txtbltyno.Text = Conversion.ToString(grdGp.CurrentRow.Cells["BiltyNo"].Value);
			txtcarramount.Text = Conversion.ToString(grdGp.CurrentRow.Cells["NetPaid"].Value);
			txtDeliverTerm.Text = Conversion.ToString(grdGp.CurrentRow.Cells["DeliveryTerm"].Value);
			txtsuppwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["SupplierWeight"].Value);
			txtfctwt.Text = Conversion.ToString(grdGp.CurrentRow.Cells["FactoryWeight"].Value);
			TotalSupplierWeight();
			dt.Rows.Clear();
			int SupplierId = 0;
			grd.ClearStructure();
			GridEXRow[] rows = grdGp.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (SupplierId == 0)
				{
					if (GpId == Conversion.ToInt(item.Cells["GpId"].Value))
					{
						dt.Rows.Add(item.Cells["SupplierCustomerId"].Value, item.Cells["SupplierName"].Value);
						SupplierId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
					}
				}
				else if (GpId == Conversion.ToInt(item.Cells["GpId"].Value) && SupplierId != Conversion.ToInt(item.Cells["SupplierCustomerId"].Value))
				{
					dt.Rows.Add(item.Cells["SupplierCustomerId"].Value, item.Cells["SupplierName"].Value);
					SupplierId = Conversion.ToInt(item.Cells["SupplierCustomerId"].Value);
				}
			}
			DDL.BindDDL(dt, combsupplier, "Id", "SupplierName", "Supplier Name", ZeroIndex: false);
			((Control)(object)combsupplier).Text = string.Empty;
			combsupplier.Focus();
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdGp).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)grdGp).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				grdGp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtadlswt_TextChanged_1(object sender, EventArgs e)
	{
		Total();
	}

	private void txtqty_Leave(object sender, EventArgs e)
	{
		MarkeetPurchaseFormula();
	}

	private void txtgwt_Leave(object sender, EventArgs e)
	{
		MarkeetPurchaseFormula();
	}

	private void LoadRecordinGridBySupplierCustomerandGpId()
	{
		try
		{
			dtdetail.Rows.Clear();
			GridEXRow[] rows = grdGp.GetRows();
			foreach (GridEXRow item in rows)
			{
				if (GPID == Conversion.ToInt(item.Cells["GpId"].Value) && Conversion.ToInt(combsupplier.Value) == Conversion.ToInt(item.Cells["SupplierCustomerId"].Value))
				{
					dtdetail.Rows.Add(item.Cells["PoDetailId"].Value, item.Cells["OrderId"].Value, item.Cells["OrderNo"].Value, item.Cells["ItemId"].Value, item.Cells["ItemName"].Value, item.Cells["CropYear"].Value, item.Cells["JobId"].Value, item.Cells["PackingTypeId"].Value, item.Cells["UOM"].Value, item.Cells["UOMId"].Value, item.Cells["Qty"].Value, item.Cells["GrossWeight"].Value, item.Cells["EbUnit"].Value, item.Cells["EbTotal"].Value, item.Cells["WtCut"].Value, item.Cells["WtCutTotal"].Value, item.Cells["AddLesswt"].Value, item.Cells["NetWeight"].Value, item.Cells["StockWeight"].Value, item.Cells["WareHouse"].Value, item.Cells["City"].Value);
				}
			}
			grd.DataSource = dtdetail;
			grd.RetrieveStructure();
			grdSettings();
			StockWeightCalculations();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsupplier_Leave(object sender, EventArgs e)
	{
		try
		{
			LoadRecordinGridBySupplierCustomerandGpId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			LoadRecordinGridBySupplierCustomerandGpId();
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
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0552: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Expected O, but got Unknown
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bf: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d5: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Expected O, but got Unknown
		//IL_05f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Expected O, but got Unknown
		//IL_060d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076c: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0777: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0782: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081c: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0895: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c1: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b3: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c9: Expected O, but got Unknown
		//IL_0fcd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd7: Expected O, but got Unknown
		//IL_116d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1177: Expected O, but got Unknown
		//IL_8578: Unknown result type (might be due to invalid IL or missing references)
		//IL_8582: Expected O, but got Unknown
		//IL_858f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8599: Expected O, but got Unknown
		GridEXLayout grdGp_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
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
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
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
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
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
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
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
		GridEXLayout GrdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdGp = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.comblabno = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.combcropyear = new UltraCombo();
		this.comborderno = new UltraCombo();
		this.combpckuom = new UltraCombo();
		this.CmbCity = new UltraCombo();
		this.combwr = new UltraCombo();
		this.combpcktyp = new UltraCombo();
		this.combjoblot = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.combitem = new UltraCombo();
		this.label30 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.txtEmptyBagsTotal = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.txtwtcuttotal = new System.Windows.Forms.TextBox();
		this.txtwtcut = new System.Windows.Forms.TextBox();
		this.txtebu = new System.Windows.Forms.TextBox();
		this.txtgwt = new System.Windows.Forms.TextBox();
		this.txtqty = new System.Windows.Forms.TextBox();
		this.txtadlswt = new System.Windows.Forms.TextBox();
		this.txtstockwt = new System.Windows.Forms.TextBox();
		this.label29 = new System.Windows.Forms.Label();
		this.txtnetwt = new System.Windows.Forms.TextBox();
		this.label46 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label42 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtEbWtOther = new System.Windows.Forms.TextBox();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.txtGrnWeight = new System.Windows.Forms.TextBox();
		this.combvehtyp = new UltraCombo();
		this.combbrnch = new UltraCombo();
		this.combproject = new UltraCombo();
		this.combgatepass = new System.Windows.Forms.TextBox();
		this.label7 = new System.Windows.Forms.Label();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.label24 = new System.Windows.Forms.Label();
		this.CmbTransport = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.txtDeliverTerm = new System.Windows.Forms.TextBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.combrefprty = new UltraCombo();
		this.txtbltyno = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.txtvehno = new System.Windows.Forms.TextBox();
		this.txtcarramount = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtsuppwt = new System.Windows.Forms.TextBox();
		this.txtfctwt = new System.Windows.Forms.TextBox();
		this.txtwtdiff = new System.Windows.Forms.TextBox();
		this.combstockprty = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.combsupplier = new UltraCombo();
		this.txtAnaylstName = new System.Windows.Forms.TextBox();
		this.txtDeductionKg = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.CityDefine = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.GrdHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.GrdHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGp).BeginInit();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combcropyear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpckuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combwr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combjoblot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1269, 736);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1269, 736);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1261, 710);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 344);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1257, 364);
		this.panel6.TabIndex = 6;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdGp);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1257, 364);
		this.panel3.TabIndex = 10;
		this.grdGp.AllowDelete = (InheritableBoolean)1;
		this.grdGp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGp).BackColor = System.Drawing.Color.White;
		grdGp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGp.DesignTimeLayout = grdGp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGp.FilterMode = (FilterMode)2;
		this.grdGp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGp.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdGp.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdGp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGp).Name = "grdGp";
		((ScrollableBase)this.grdGp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGp).Size = new System.Drawing.Size(1257, 364);
		((System.Windows.Forms.Control)(object)this.grdGp).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.grdGp).TabStop = false;
		this.grdGp.TotalRow = (InheritableBoolean)1;
		this.grdGp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGp).VisualStyle = (VisualStyle)4;
		this.grdGp.ColumnButtonClick += new ColumnActionEventHandler(grdGp_ColumnButtonClick);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(2, 147);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1257, 197);
		this.panel10.TabIndex = 5;
		this.grd.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1257, 197);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.grd).TabStop = false;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Controls.Add(this.label16);
		this.panel5.Controls.Add(this.txtEbWtOther);
		this.panel5.Location = new System.Drawing.Point(2, 147);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1255, 137);
		this.panel5.TabIndex = 0;
		this.groupBox3.BackColor = System.Drawing.Color.White;
		this.groupBox3.Controls.Add(this.comblabno);
		this.groupBox3.Controls.Add(this.label32);
		this.groupBox3.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox3.Controls.Add(this.btnUpdateDetail);
		this.groupBox3.Controls.Add(this.label9);
		this.groupBox3.Controls.Add(this.label3);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcropyear);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.comborderno);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combpckuom);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCity);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combwr);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combpcktyp);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combjoblot);
		this.groupBox3.Controls.Add(this.Add);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combitem);
		this.groupBox3.Controls.Add(this.label30);
		this.groupBox3.Controls.Add(this.label27);
		this.groupBox3.Controls.Add(this.txtEmptyBagsTotal);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.txtwtcuttotal);
		this.groupBox3.Controls.Add(this.txtwtcut);
		this.groupBox3.Controls.Add(this.txtebu);
		this.groupBox3.Controls.Add(this.txtgwt);
		this.groupBox3.Controls.Add(this.txtqty);
		this.groupBox3.Controls.Add(this.txtadlswt);
		this.groupBox3.Controls.Add(this.txtstockwt);
		this.groupBox3.Controls.Add(this.label29);
		this.groupBox3.Controls.Add(this.txtnetwt);
		this.groupBox3.Controls.Add(this.label46);
		this.groupBox3.Controls.Add(this.label28);
		this.groupBox3.Controls.Add(this.label35);
		this.groupBox3.Controls.Add(this.label45);
		this.groupBox3.Controls.Add(this.label44);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label42);
		this.groupBox3.Controls.Add(this.label31);
		this.groupBox3.Controls.Add(this.label41);
		this.groupBox3.Controls.Add(this.label40);
		this.groupBox3.Controls.Add(this.label36);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(2, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1236, 131);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Detail";
		this.comblabno.BackColor = System.Drawing.Color.White;
		this.comblabno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.comblabno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comblabno.Location = new System.Drawing.Point(687, 95);
		this.comblabno.Name = "comblabno";
		this.comblabno.ReadOnly = true;
		this.comblabno.Size = new System.Drawing.Size(118, 21);
		this.comblabno.TabIndex = 16;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(2214, -37);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(25, 17);
		this.label32.TabIndex = 45;
		this.label32.Text = "Eb";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1155, 92);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 26);
		this.btnCancelUpdateDetial.TabIndex = 20;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.White;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(1087, 92);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 26);
		this.btnUpdateDetail.TabIndex = 19;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(77, 69);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(55, 15);
		this.label9.TabIndex = 44;
		this.label9.Text = "Cut Total";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 69);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(42, 15);
		this.label3.TabIndex = 43;
		this.label3.Text = "Wt/Cut";
		this.combcropyear.AllowNull = (DefaultableBoolean)1;
		this.combcropyear.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcropyear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcropyear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcropyear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.combcropyear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcropyear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance115).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance115).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcropyear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcropyear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance170).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance170).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance170).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcropyear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.combcropyear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcropyear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcropyear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcropyear).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcropyear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcropyear).Location = new System.Drawing.Point(350, 37);
		((System.Windows.Forms.Control)(object)this.combcropyear).Name = "combcropyear";
		((System.Windows.Forms.Control)(object)this.combcropyear).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.combcropyear).TabIndex = 2;
		this.comborderno.AllowNull = (DefaultableBoolean)1;
		this.comborderno.AutoCompleteMode = (AutoCompleteMode)4;
		this.comborderno.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comborderno.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comborderno).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.comborderno).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comborderno).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance57;
		((SpecialBoxBase)((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comborderno).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comborderno).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance94).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance95).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance95).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance95).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).TextHAlignAsString = "Left";
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comborderno).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comborderno).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comborderno).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comborderno).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comborderno.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comborderno).Location = new System.Drawing.Point(5, 37);
		((System.Windows.Forms.Control)(object)this.comborderno).Name = "comborderno";
		((System.Windows.Forms.Control)(object)this.comborderno).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.comborderno).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comborderno).Leave += new System.EventHandler(comborderno_Leave);
		this.combpckuom.AllowNull = (DefaultableBoolean)1;
		this.combpckuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpckuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpckuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpckuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combpckuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpckuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance101;
		((SpecialBoxBase)((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance102).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance102).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpckuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.combpckuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpckuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpckuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.combpckuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpckuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpckuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpckuom).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpckuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpckuom).Location = new System.Drawing.Point(727, 37);
		((System.Windows.Forms.Control)(object)this.combpckuom).Name = "combpckuom";
		((System.Windows.Forms.Control)(object)this.combpckuom).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.combpckuom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.combpckuom).Leave += new System.EventHandler(combpckuom_Leave);
		this.CmbCity.AllowNull = (DefaultableBoolean)1;
		this.CmbCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance113).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance113).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance114;
		((SpecialBoxBase)((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance117).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance120).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCity).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCity).Location = new System.Drawing.Point(808, 93);
		((System.Windows.Forms.Control)(object)this.CmbCity).Name = "CmbCity";
		((System.Windows.Forms.Control)(object)this.CmbCity).Size = new System.Drawing.Size(244, 24);
		((System.Windows.Forms.Control)(object)this.CmbCity).TabIndex = 17;
		this.combwr.AllowNull = (DefaultableBoolean)1;
		this.combwr.AutoCompleteMode = (AutoCompleteMode)4;
		this.combwr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combwr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combwr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.combwr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combwr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance127).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance127).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combwr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combwr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance128;
		((SpecialBoxBase)((UltraGridBase)this.combwr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combwr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.combwr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combwr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combwr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combwr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.combwr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combwr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combwr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combwr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.combwr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.combwr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combwr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combwr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.combwr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combwr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combwr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combwr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combwr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combwr).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combwr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combwr).Location = new System.Drawing.Point(454, 93);
		((System.Windows.Forms.Control)(object)this.combwr).Name = "combwr";
		((System.Windows.Forms.Control)(object)this.combwr).Size = new System.Drawing.Size(227, 24);
		((System.Windows.Forms.Control)(object)this.combwr).TabIndex = 15;
		this.combpcktyp.AllowNull = (DefaultableBoolean)1;
		this.combpcktyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpcktyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpcktyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.combpcktyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpcktyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance141;
		((SpecialBoxBase)((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.combpcktyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpcktyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance146).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance147).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance147).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance147).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance149).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpcktyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.combpcktyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpcktyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpcktyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpcktyp).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combpcktyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpcktyp).Location = new System.Drawing.Point(602, 37);
		((System.Windows.Forms.Control)(object)this.combpcktyp).Name = "combpcktyp";
		((System.Windows.Forms.Control)(object)this.combpcktyp).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.combpcktyp).TabIndex = 4;
		this.combjoblot.AllowNull = (DefaultableBoolean)1;
		this.combjoblot.AutoCompleteMode = (AutoCompleteMode)4;
		this.combjoblot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combjoblot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combjoblot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.combjoblot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combjoblot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance153).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance154;
		((SpecialBoxBase)((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combjoblot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.combjoblot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combjoblot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance160).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance161).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance161).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance161).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).TextHAlignAsString = "Left";
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combjoblot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.combjoblot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combjoblot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combjoblot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combjoblot).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combjoblot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combjoblot).Location = new System.Drawing.Point(476, 37);
		((System.Windows.Forms.Control)(object)this.combjoblot).Name = "combjoblot";
		((System.Windows.Forms.Control)(object)this.combjoblot).Size = new System.Drawing.Size(121, 24);
		((System.Windows.Forms.Control)(object)this.combjoblot).TabIndex = 3;
		this.Add.BackColor = System.Drawing.Color.White;
		this.Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.Location = new System.Drawing.Point(1057, 93);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(29, 25);
		this.Add.TabIndex = 18;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.combitem.AllowNull = (DefaultableBoolean)1;
		this.combitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.combitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.combitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance166;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance167;
		((SpecialBoxBase)((UltraGridBase)this.combitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance168).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance168).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.combitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.combitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combitem).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combitem).Location = new System.Drawing.Point(128, 37);
		((System.Windows.Forms.Control)(object)this.combitem).Name = "combitem";
		((System.Windows.Forms.Control)(object)this.combitem).Size = new System.Drawing.Size(220, 24);
		((System.Windows.Forms.Control)(object)this.combitem).TabIndex = 1;
		this.combitem.ValueChanged += new System.EventHandler(combitem_Leave);
		((System.Windows.Forms.Control)(object)this.combitem).Leave += new System.EventHandler(combitem_Leave);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(456, 69);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(70, 15);
		this.label30.TabIndex = 41;
		this.label30.Text = "Warehouse";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(478, 16);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(47, 15);
		this.label27.TabIndex = 41;
		this.label27.Text = "Job/Lot";
		this.txtEmptyBagsTotal.BackColor = System.Drawing.Color.White;
		this.txtEmptyBagsTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEmptyBagsTotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEmptyBagsTotal.Location = new System.Drawing.Point(1103, 39);
		this.txtEmptyBagsTotal.Name = "txtEmptyBagsTotal";
		this.txtEmptyBagsTotal.ReadOnly = true;
		this.txtEmptyBagsTotal.Size = new System.Drawing.Size(78, 21);
		this.txtEmptyBagsTotal.TabIndex = 9;
		this.txtEmptyBagsTotal.TabStop = false;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(354, 16);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(61, 15);
		this.label22.TabIndex = 41;
		this.label22.Text = "Crop Year";
		this.txtwtcuttotal.BackColor = System.Drawing.Color.White;
		this.txtwtcuttotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcuttotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtcuttotal.Location = new System.Drawing.Point(74, 95);
		this.txtwtcuttotal.Name = "txtwtcuttotal";
		this.txtwtcuttotal.ReadOnly = true;
		this.txtwtcuttotal.Size = new System.Drawing.Size(78, 21);
		this.txtwtcuttotal.TabIndex = 11;
		this.txtwtcuttotal.TabStop = false;
		this.txtwtcut.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtcut.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtcut.Location = new System.Drawing.Point(6, 95);
		this.txtwtcut.Name = "txtwtcut";
		this.txtwtcut.Size = new System.Drawing.Size(66, 21);
		this.txtwtcut.TabIndex = 10;
		this.txtwtcut.TextChanged += new System.EventHandler(txtebu_TextChanged);
		this.txtebu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtebu.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtebu.Location = new System.Drawing.Point(1036, 39);
		this.txtebu.Name = "txtebu";
		this.txtebu.Size = new System.Drawing.Size(64, 21);
		this.txtebu.TabIndex = 8;
		this.txtebu.TextChanged += new System.EventHandler(txtebu_TextChanged_1);
		this.txtgwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtgwt.Location = new System.Drawing.Point(938, 39);
		this.txtgwt.Name = "txtgwt";
		this.txtgwt.Size = new System.Drawing.Size(96, 21);
		this.txtgwt.TabIndex = 7;
		this.txtgwt.TextChanged += new System.EventHandler(txtgwt_TextChanged);
		this.txtgwt.Leave += new System.EventHandler(txtgwt_Leave);
		this.txtqty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtqty.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtqty.Location = new System.Drawing.Point(852, 39);
		this.txtqty.Name = "txtqty";
		this.txtqty.Size = new System.Drawing.Size(83, 21);
		this.txtqty.TabIndex = 6;
		this.txtqty.TextChanged += new System.EventHandler(txtqty_TextChanged);
		this.txtqty.Leave += new System.EventHandler(txtqty_Leave);
		this.txtadlswt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtadlswt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtadlswt.Location = new System.Drawing.Point(156, 95);
		this.txtadlswt.Name = "txtadlswt";
		this.txtadlswt.Size = new System.Drawing.Size(53, 21);
		this.txtadlswt.TabIndex = 12;
		this.txtadlswt.TextChanged += new System.EventHandler(txtadlswt_TextChanged_1);
		this.txtadlswt.Leave += new System.EventHandler(txtadlswt_Leave);
		this.txtstockwt.BackColor = System.Drawing.Color.White;
		this.txtstockwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtstockwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtstockwt.Location = new System.Drawing.Point(333, 95);
		this.txtstockwt.Name = "txtstockwt";
		this.txtstockwt.Size = new System.Drawing.Size(118, 21);
		this.txtstockwt.TabIndex = 14;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(333, 69);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(78, 15);
		this.label29.TabIndex = 25;
		this.label29.Text = "Stock Weight";
		this.txtnetwt.BackColor = System.Drawing.Color.White;
		this.txtnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtnetwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtnetwt.Location = new System.Drawing.Point(212, 95);
		this.txtnetwt.Name = "txtnetwt";
		this.txtnetwt.ReadOnly = true;
		this.txtnetwt.Size = new System.Drawing.Size(118, 21);
		this.txtnetwt.TabIndex = 13;
		this.txtnetwt.TabStop = false;
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label46.Location = new System.Drawing.Point(212, 69);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(67, 15);
		this.label46.TabIndex = 25;
		this.label46.Text = "Net Weight";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(809, 69);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(54, 15);
		this.label28.TabIndex = 25;
		this.label28.Text = "City/Area";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(153, 69);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(37, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Ad/Ls";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.Location = new System.Drawing.Point(1107, 16);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(55, 15);
		this.label45.TabIndex = 23;
		this.label45.Text = "E.b Total";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(1040, 16);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(56, 15);
		this.label44.TabIndex = 23;
		this.label44.Text = "E.b / Unit";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.Location = new System.Drawing.Point(938, 16);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(80, 15);
		this.label43.TabIndex = 23;
		this.label43.Text = "Gross Weight";
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(730, 15);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(66, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "Pack UOM";
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(684, 69);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(93, 15);
		this.label31.TabIndex = 17;
		this.label31.Text = "Lab Sample No";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.Location = new System.Drawing.Point(854, 15);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(24, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Qty";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(603, 16);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(76, 15);
		this.label40.TabIndex = 17;
		this.label40.Text = "Packing type";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(131, 16);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(68, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item Name";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(6, 16);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(57, 15);
		this.label37.TabIndex = 0;
		this.label37.Text = "Order No";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(1285, 44);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(22, 15);
		this.label16.TabIndex = 45;
		this.label16.Text = "Eb";
		this.label16.Visible = false;
		this.txtEbWtOther.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtEbWtOther.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEbWtOther.Location = new System.Drawing.Point(1282, 66);
		this.txtEbWtOther.Name = "txtEbWtOther";
		this.txtEbWtOther.Size = new System.Drawing.Size(57, 21);
		this.txtEbWtOther.TabIndex = 8;
		this.txtEbWtOther.TabStop = false;
		this.txtEbWtOther.Visible = false;
		this.txtEbWtOther.TextChanged += new System.EventHandler(txtEbWtOther_TextChanged);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtGrnWeight);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combvehtyp);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combbrnch);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject);
		this.panel4.Controls.Add(this.combgatepass);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.ChkBox);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Controls.Add(this.label25);
		this.panel4.Controls.Add(this.label24);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransport);
		this.panel4.Controls.Add(this.label19);
		this.panel4.Controls.Add(this.label48);
		this.panel4.Controls.Add(this.label21);
		this.panel4.Controls.Add(this.txtDeliverTerm);
		this.panel4.Controls.Add(this.txtremarks);
		this.panel4.Controls.Add(this.label26);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combrefprty);
		this.panel4.Controls.Add(this.txtbltyno);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.txtvehno);
		this.panel4.Controls.Add(this.txtcarramount);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.label20);
		this.panel4.Controls.Add(this.txtsuppwt);
		this.panel4.Controls.Add(this.txtfctwt);
		this.panel4.Controls.Add(this.txtwtdiff);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combstockprty);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combsupplier);
		this.panel4.Controls.Add(this.txtAnaylstName);
		this.panel4.Controls.Add(this.txtDeductionKg);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.DocDate);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtdocno);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 33);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1257, 114);
		this.panel4.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(283, 88);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(86, 15);
		this.label1.TabIndex = 20;
		this.label1.Text = "Deduction(Kg)";
		this.txtGrnWeight.BackColor = System.Drawing.Color.White;
		this.txtGrnWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrnWeight.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGrnWeight.Location = new System.Drawing.Point(996, 113);
		this.txtGrnWeight.Name = "txtGrnWeight";
		this.txtGrnWeight.ReadOnly = true;
		this.txtGrnWeight.Size = new System.Drawing.Size(222, 21);
		this.txtGrnWeight.TabIndex = 19;
		this.txtGrnWeight.TabStop = false;
		this.txtGrnWeight.Visible = false;
		this.txtGrnWeight.WordWrap = false;
		this.combvehtyp.AllowNull = (DefaultableBoolean)1;
		this.combvehtyp.AutoCompleteMode = (AutoCompleteMode)4;
		this.combvehtyp.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combvehtyp.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.combvehtyp).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combvehtyp).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combvehtyp).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combvehtyp.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combvehtyp).Location = new System.Drawing.Point(692, 56);
		((System.Windows.Forms.Control)(object)this.combvehtyp).Name = "combvehtyp";
		((System.Windows.Forms.Control)(object)this.combvehtyp).Size = new System.Drawing.Size(202, 24);
		((System.Windows.Forms.Control)(object)this.combvehtyp).TabIndex = 10;
		this.combbrnch.AllowNull = (DefaultableBoolean)1;
		this.combbrnch.AutoCompleteMode = (AutoCompleteMode)4;
		this.combbrnch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combbrnch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combbrnch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.combbrnch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combbrnch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combbrnch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combbrnch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combbrnch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combbrnch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combbrnch).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combbrnch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combbrnch).Location = new System.Drawing.Point(75, 5);
		((System.Windows.Forms.Control)(object)this.combbrnch).Name = "combbrnch";
		((System.Windows.Forms.Control)(object)this.combbrnch).Size = new System.Drawing.Size(202, 24);
		((System.Windows.Forms.Control)(object)this.combbrnch).TabIndex = 0;
		this.combproject.AllowNull = (DefaultableBoolean)1;
		this.combproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.combproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.combproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.combproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject).Location = new System.Drawing.Point(75, 32);
		((System.Windows.Forms.Control)(object)this.combproject).Name = "combproject";
		((System.Windows.Forms.Control)(object)this.combproject).Size = new System.Drawing.Size(202, 24);
		((System.Windows.Forms.Control)(object)this.combproject).TabIndex = 1;
		this.combgatepass.BackColor = System.Drawing.Color.White;
		this.combgatepass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.combgatepass.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combgatepass.Location = new System.Drawing.Point(383, 8);
		this.combgatepass.Name = "combgatepass";
		this.combgatepass.ReadOnly = true;
		this.combgatepass.Size = new System.Drawing.Size(202, 21);
		this.combgatepass.TabIndex = 4;
		this.combgatepass.TextChanged += new System.EventHandler(combgatepass_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(283, 9);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(63, 15);
		this.label7.TabIndex = 0;
		this.label7.Text = "Gate Pass";
		this.ChkBox.AutoSize = true;
		this.ChkBox.Location = new System.Drawing.Point(1223, 91);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(15, 14);
		this.ChkBox.TabIndex = 18;
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(1072, 64);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(82, 15);
		this.label15.TabIndex = 0;
		this.label15.Text = "Delivery Term";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(778, 90);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(48, 15);
		this.label25.TabIndex = 3;
		this.label25.Text = "Bilty No";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(899, 92);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(57, 15);
		this.label24.TabIndex = 0;
		this.label24.Text = "Remarks";
		this.CmbTransport.AllowNull = (DefaultableBoolean)1;
		this.CmbTransport.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransport.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransport.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbTransport).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransport).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransport).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransport.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransport).Location = new System.Drawing.Point(692, 5);
		((System.Windows.Forms.Control)(object)this.CmbTransport).Name = "CmbTransport";
		((System.Windows.Forms.Control)(object)this.CmbTransport).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.CmbTransport).TabIndex = 8;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(595, 87);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(66, 15);
		this.label19.TabIndex = 0;
		this.label19.Text = "Vehicle No";
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.Location = new System.Drawing.Point(596, 9);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(70, 15);
		this.label48.TabIndex = 8;
		this.label48.Text = "Transporter";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(595, 58);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(76, 15);
		this.label21.TabIndex = 8;
		this.label21.Text = "Vehicle Type";
		this.txtDeliverTerm.BackColor = System.Drawing.Color.White;
		this.txtDeliverTerm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliverTerm.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeliverTerm.Location = new System.Drawing.Point(1157, 61);
		this.txtDeliverTerm.Name = "txtDeliverTerm";
		this.txtDeliverTerm.ReadOnly = true;
		this.txtDeliverTerm.Size = new System.Drawing.Size(61, 21);
		this.txtDeliverTerm.TabIndex = 16;
		this.txtDeliverTerm.TabStop = false;
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(996, 86);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(222, 21);
		this.txtremarks.TabIndex = 17;
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(596, 34);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(87, 15);
		this.label26.TabIndex = 8;
		this.label26.Text = "Freigh Amount";
		this.combrefprty.AllowNull = (DefaultableBoolean)1;
		this.combrefprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combrefprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combrefprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combrefprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combrefprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combrefprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combrefprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance60).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance61).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combrefprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combrefprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combrefprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combrefprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combrefprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combrefprty).Location = new System.Drawing.Point(384, 190);
		((System.Windows.Forms.Control)(object)this.combrefprty).Name = "combrefprty";
		((System.Windows.Forms.Control)(object)this.combrefprty).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combrefprty).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combrefprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combrefprty).Visible = false;
		this.txtbltyno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbltyno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtbltyno.Location = new System.Drawing.Point(840, 88);
		this.txtbltyno.Name = "txtbltyno";
		this.txtbltyno.Size = new System.Drawing.Size(53, 21);
		this.txtbltyno.TabIndex = 12;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(899, 64);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(66, 15);
		this.label11.TabIndex = 0;
		this.label11.Text = "Weight Diff";
		this.txtvehno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtvehno.Location = new System.Drawing.Point(692, 85);
		this.txtvehno.Name = "txtvehno";
		this.txtvehno.Size = new System.Drawing.Size(82, 21);
		this.txtvehno.TabIndex = 11;
		this.txtcarramount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcarramount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcarramount.Location = new System.Drawing.Point(692, 32);
		this.txtcarramount.Name = "txtcarramount";
		this.txtcarramount.Size = new System.Drawing.Size(202, 21);
		this.txtcarramount.TabIndex = 9;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(899, 10);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(77, 15);
		this.label6.TabIndex = 0;
		this.label6.Text = "Supp Weight";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(283, 167);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(67, 15);
		this.label8.TabIndex = 3;
		this.label8.Text = "Stock Party";
		this.label8.Visible = false;
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(899, 37);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(71, 15);
		this.label20.TabIndex = 3;
		this.label20.Text = "Fact Weight";
		this.txtsuppwt.BackColor = System.Drawing.Color.White;
		this.txtsuppwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuppwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuppwt.Location = new System.Drawing.Point(996, 8);
		this.txtsuppwt.Name = "txtsuppwt";
		this.txtsuppwt.ReadOnly = true;
		this.txtsuppwt.Size = new System.Drawing.Size(222, 21);
		this.txtsuppwt.TabIndex = 13;
		this.txtsuppwt.TabStop = false;
		this.txtsuppwt.TextChanged += new System.EventHandler(txtsuppwt_TextChanged);
		this.txtfctwt.BackColor = System.Drawing.Color.White;
		this.txtfctwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfctwt.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfctwt.Location = new System.Drawing.Point(996, 34);
		this.txtfctwt.Name = "txtfctwt";
		this.txtfctwt.ReadOnly = true;
		this.txtfctwt.Size = new System.Drawing.Size(222, 21);
		this.txtfctwt.TabIndex = 14;
		this.txtfctwt.TabStop = false;
		this.txtfctwt.TextChanged += new System.EventHandler(txtfctwt_TextChanged);
		this.txtwtdiff.BackColor = System.Drawing.Color.White;
		this.txtwtdiff.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtwtdiff.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtwtdiff.Location = new System.Drawing.Point(996, 61);
		this.txtwtdiff.Name = "txtwtdiff";
		this.txtwtdiff.ReadOnly = true;
		this.txtwtdiff.Size = new System.Drawing.Size(73, 21);
		this.txtwtdiff.TabIndex = 15;
		this.txtwtdiff.TabStop = false;
		this.combstockprty.AllowNull = (DefaultableBoolean)1;
		this.combstockprty.AutoCompleteMode = (AutoCompleteMode)4;
		this.combstockprty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combstockprty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combstockprty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combstockprty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance67;
		((SpecialBoxBase)((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combstockprty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combstockprty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combstockprty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combstockprty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combstockprty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combstockprty).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combstockprty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combstockprty).Location = new System.Drawing.Point(384, 163);
		((System.Windows.Forms.Control)(object)this.combstockprty).Name = "combstockprty";
		((System.Windows.Forms.Control)(object)this.combstockprty).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combstockprty).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.combstockprty).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combstockprty).Visible = false;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(283, 36);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(90, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Supplier Name";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(283, 64);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(79, 15);
		this.label10.TabIndex = 8;
		this.label10.Text = "AnalystName";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(283, 194);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(56, 15);
		this.label12.TabIndex = 0;
		this.label12.Text = "Ref Party";
		this.label12.Visible = false;
		this.combsupplier.AllowNull = (DefaultableBoolean)1;
		this.combsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.combsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.combsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).TextHAlignAsString = "Left";
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combsupplier).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combsupplier).Location = new System.Drawing.Point(384, 32);
		((System.Windows.Forms.Control)(object)this.combsupplier).Name = "combsupplier";
		((System.Windows.Forms.Control)(object)this.combsupplier).Size = new System.Drawing.Size(201, 24);
		((System.Windows.Forms.Control)(object)this.combsupplier).TabIndex = 5;
		this.combsupplier.ValueChanged += new System.EventHandler(combsupplier_ValueChanged);
		((System.Windows.Forms.Control)(object)this.combsupplier).Leave += new System.EventHandler(combsupplier_Leave);
		this.txtAnaylstName.BackColor = System.Drawing.Color.White;
		this.txtAnaylstName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAnaylstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAnaylstName.Location = new System.Drawing.Point(384, 60);
		this.txtAnaylstName.Name = "txtAnaylstName";
		this.txtAnaylstName.ReadOnly = true;
		this.txtAnaylstName.Size = new System.Drawing.Size(202, 21);
		this.txtAnaylstName.TabIndex = 6;
		this.txtAnaylstName.TabStop = false;
		this.txtDeductionKg.BackColor = System.Drawing.Color.White;
		this.txtDeductionKg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeductionKg.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDeductionKg.Location = new System.Drawing.Point(384, 85);
		this.txtDeductionKg.Name = "txtDeductionKg";
		this.txtDeductionKg.ReadOnly = true;
		this.txtDeductionKg.Size = new System.Drawing.Size(202, 21);
		this.txtDeductionKg.TabIndex = 7;
		this.txtDeductionKg.TabStop = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(9, 7);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(75, 85);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(201, 21);
		this.DocDate.TabIndex = 3;
		this.DocDate.TabStop = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(9, 34);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(45, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(9, 61);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(75, 58);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(202, 21);
		this.txtdocno.TabIndex = 2;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(9, 88);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(58, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc Date";
		this.panel2.Controls.Add(this.ctrlGrdBar3);
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1257, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.toolStripLabel1, this.btnNew, this.btnUpdate, this.btnSave, this.printToolStripButton, this.btnAttachment, this.toolStripButton1, this.CityDefine, this.btnDelete });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1257, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.toolStripLabel1.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripLabel1.Name = "toolStripLabel1";
		this.toolStripLabel1.Size = new System.Drawing.Size(133, 24);
		this.toolStripLabel1.Text = "Market GRN           ";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(55, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.printToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.printToolStripButton.Name = "printToolStripButton";
		this.printToolStripButton.Size = new System.Drawing.Size(79, 24);
		this.printToolStripButton.Text = "211-Print";
		this.printToolStripButton.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 24);
		this.btnAttachment.Text = "&Attechment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(50, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.CityDefine.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.CityDefine.Name = "CityDefine";
		this.CityDefine.Size = new System.Drawing.Size(32, 24);
		this.CityDefine.Text = "City";
		this.CityDefine.Click += new System.EventHandler(CityDefine_Click);
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(23, 24);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1261, 710);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistoryDetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(2, 384);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1257, 324);
		this.panel11.TabIndex = 2;
		this.GrdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		GrdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistoryDetail.DesignTimeLayout = GrdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistoryDetail.DynamicFiltering = true;
		this.GrdHistoryDetail.FilterMode = (FilterMode)2;
		this.GrdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Name = "GrdHistoryDetail";
		((ScrollableBase)this.GrdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).Size = new System.Drawing.Size(1257, 324);
		((System.Windows.Forms.Control)(object)this.GrdHistoryDetail).TabIndex = 45;
		this.GrdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.GrdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 30);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1257, 354);
		this.panel8.TabIndex = 1;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1257, 354);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 44;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1257, 28);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1257, 27);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1208, 1);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdGp;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1147, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1208, -3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 23;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1269, 736);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "MarketGrn";
		this.Text = "MarketGrn";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGp).EndInit();
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combcropyear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comborderno).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpckuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combwr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combpcktyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combjoblot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combitem).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combvehtyp).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combbrnch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransport).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combrefprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combstockprty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combsupplier).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
