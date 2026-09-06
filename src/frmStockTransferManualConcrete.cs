// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.pcc;
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
namespace Architecture.WinApp.pcc.Production
{

public class frmStockTransferManualConcrete : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private int updateDetailIndex;

	private bool IsApproved;

	public int RecId;

	private bool WagesStatus;

	private bool StockTransferFinancialEffectIsActive;

	private DataTable table = new DataTable();

	private DataTable dtExp = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private bool FifoMethod;

	private List<InvStockTransferDetail> lstRemoveRecord = new List<InvStockTransferDetail>();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private Panel panel4;

	private Label label13;

	private Label label18;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	private Button btnplus;

	private TextBox txtQty;

	private Label label38;

	private Panel panel6;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Button btnUpdateDetail;

	private Button btnCancelUpdateDetial;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	internal UltraCombo combproject1;

	internal UltraCombo cmbbranch1;

	internal UltraCombo cmbItemName;

	private Label label2;

	internal UltraCombo CmbWareHouseFrom;

	private Label label3;

	internal GridEX grd;

	private Label label4;

	private TextBox txtRemarksDetail;

	private Label label12;

	internal UltraCombo cmbWareHouseTo;

	private Label label11;

	private TextBox txtDetailNetWeight;

	private Label label8;

	internal UltraCombo cmbJobLot;

	private Label label7;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label15;

	private CheckBox ChkBoxPrintPreview;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private ToolStripButton btnAttachment;

	internal UltraCombo CmbJobLotTo;

	private Label label21;

	private TextBox txtAvgRate;

	private Label lblAvgRate;

	private TextBox txtAmount;

	private Label lblAmount;

	private Label label24;

	private Panel panel5;

	private Panel panel11;

	private Label label23;

	private GroupBox groupBox2;

	private GroupBox groupBox1;

	private TextBox txtgatepsrno;

	private TextBox txtvehichleno;

	private Label label25;

	private CtrlGrdBar ctrlGrdBar6;

	internal UltraCombo CmbAttributeVarient;

	private Label label51;

	private Label label52;

	private TextBox txtVarientEquivalent;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistory;

	private Panel panel13;

	private Panel panel8;

	private Button btnshow;

	private DateTimePicker ToDate;

	private DateTimePicker fromdate;

	private Panel panel15;

	private Label label26;

	private DateTimePicker txtgpdate;

	private TextBox txtitemweight;

	private Label label28;

	private ToolStripButton btnSaveAs;

	private TextBox txttodocno;

	private TextBox txtfromdocno;

	private Label bg;

	private TextBox txtVehicleNoHistory;

	private Label label10;

	private GroupBox groupBox3;

	internal UltraCombo cmbperemeter;

	private Label label20;

	private Label label34;

	private Label label37;

	private TextBox txtRemarksHead;

	internal GridEX GridDetailHistory;

	private Panel panel14;

	private Label label27;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator1;

	private Label LabBalanceQty;

	internal UltraCombo CmbPackUom;

	private Label label1;

	public frmStockTransferManualConcrete(UserAccount user)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void PurchsaeOrder_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			FifoMethod = CommonServices.GetERPFeatureById(5);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formrights.DoHaveSaveRight;
			btnupdate.Enabled = formrights.DoHaveUpdateRights;
			btnDelete.Enabled = formrights.DoHaveUpdateRights;
			btnprint.Enabled = formrights.DoHavePrintRights;
			ChkBoxPrintPreview.Enabled = formrights.DoHavePrintRights;
			ChkBoxPrintPreview.Checked = formrights.DoHavePrintRights;
			StockTransferFinancialEffectIsActive = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("StockTransferFinancialEffectIsActive"));
			WagesStatus = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("WagesCompulsoryOnStockTransfer"));
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName", typeof(string));
			dtItem.Columns.Add("ItemCode", typeof(string));
			dtItem.Columns.Add("ItemWeight", typeof(double));
			dtItem.Columns.Add("WithVariant");
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("WareHouseFromId", typeof(int));
			table.Columns.Add("WareHouseFrom");
			table.Columns.Add("WareHouseToId", typeof(int));
			table.Columns.Add("WareHouseTo");
			table.Columns.Add("JobLotId", typeof(int));
			table.Columns.Add("JobLot");
			table.Columns.Add("JobLotIdTo", typeof(int));
			table.Columns.Add("JobLotTo");
			table.Columns.Add("ItemId", typeof(int));
			table.Columns.Add("ItemName");
			table.Columns.Add("PackUomId", typeof(int));
			table.Columns.Add("PackUom");
			table.Columns.Add("AttributeVarientId", typeof(int));
			table.Columns.Add("AttributeVarient");
			table.Columns.Add("VarientUnit", typeof(decimal));
			table.Columns.Add("QTY", typeof(decimal));
			table.Columns.Add("ItemWeight", typeof(decimal));
			table.Columns.Add("NetWeight", typeof(decimal));
			table.Columns.Add("ItemRate", typeof(decimal));
			table.Columns.Add("ItemAmount", typeof(decimal));
			table.Columns.Add("Remarks");
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			DocumentNoFill();
			WareHouseFill();
			GetJobLot();
			ItemDetailFill();
			ParameterFill();
			((UltraGridBase)cmbperemeter).Rows[1].Activate();
			DocDate.Focus();
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnsave.Visible = true;
			btnSaveAs.Visible = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				fromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				fromdate.Value = first;
				ToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				ToDate.Value = first2;
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			int ItId = Conversion.ToInt(CmbWareHouseFrom.Value);
			int ItId2 = Conversion.ToInt(cmbWareHouseTo.Value);
			DataTable dtWh = CommonServices.getActiveWareHouse();
			if (dtWh.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDL(dtWh, CmbWareHouseFrom, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
			((UltraGridBase)CmbWareHouseFrom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDL(dtWh, cmbWareHouseTo, "Id", "WareHouseName", "WareHouseName", ZeroIndex: true);
			((UltraGridBase)cmbWareHouseTo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dtWh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					CmbWareHouseFrom.Value = ItId;
				}
				else
				{
					((Control)(object)CmbWareHouseFrom).Text = string.Empty;
				}
			}
			if (ItId2 > 0)
			{
				if (dtWh.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId2))
				{
					cmbWareHouseTo.Value = ItId2;
				}
				else
				{
					((Control)(object)cmbWareHouseTo).Text = string.Empty;
				}
			}
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
			int code = InvStockTransferHeader.GenerateCode(new InvStockTransferHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1860,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (code > 0)
			{
				txtdocno.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDetailFill()
	{
		try
		{
			int ItId = Conversion.ToInt(cmbItemName.Value);
			dtItem.Clear();
			DataTable dt = CommonServices.ItemGetAllServiceBind();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"], dt.Rows[i]["WeightKgs"], dt.Rows[i]["ItemWithVarient"]);
				}
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItemName, "Id", "ItemName", "Item Name", ZeroIndex: true);
				((UltraGridBase)cmbItemName).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (ItId > 0)
				{
					if (dtItem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
					{
						cmbItemName.Value = ItId;
					}
					else
					{
						((Control)(object)cmbItemName).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)cmbItemName).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetJobLot()
	{
		try
		{
			int ItId = Conversion.ToInt(cmbJobLot.Value);
			int ItId2 = Conversion.ToInt(CmbJobLotTo.Value);
			DataTable dt = CommonServices.JobLotGetAllService();
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbJobLot, "Id", "JobLotDescription", "Job Lot From", true);
			((UltraGridBase)cmbJobLot).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			DDL.BindDDLNew(dt, CmbJobLotTo, "Id", "JobLotDescription", "Job Lot To", true);
			((UltraGridBase)CmbJobLotTo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					cmbJobLot.Value = ItId;
				}
				else
				{
					((Control)(object)cmbJobLot).Text = string.Empty;
				}
			}
			if (ItId2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId2))
				{
					CmbJobLotTo.Value = ItId2;
				}
				else
				{
					((Control)(object)CmbJobLotTo).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void bindvarientunit(int ID)
	{
		try
		{
			string ItId = Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text);
			DataTable dtunit = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ID);
			if (dtunit.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemAttributeVarientId", typeof(int));
				dt.Columns.Add("ItemAttribute", typeof(string));
				dt.Columns.Add("VarientEquivalent", typeof(double));
				dt.Clear();
				for (int i = 0; i < dtunit.Rows.Count; i++)
				{
					dt.Rows.Add(dtunit.Rows[i]["ItemAttributeVarientId"], dtunit.Rows[i]["ItemAttribute"], dtunit.Rows[i]["VarientEquivalent"]);
				}
				DDL.BindDDL(dt, CmbAttributeVarient, "ItemAttributeVarientId", "ItemAttribute", "Varient", ZeroIndex: true);
				if (ItId != null && ItId != string.Empty)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("ItemAttribute") == ItId))
					{
						((Control)(object)CmbAttributeVarient).Text = ItId;
					}
					else
					{
						((Control)(object)CmbAttributeVarient).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAttributeVarient).Text = string.Empty;
				txtVarientEquivalent.Text = string.Empty;
				((UltraGridBase)CmbAttributeVarient).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbAttributeVarient_Leave(object sender, EventArgs e)
	{
		try
		{
			double Equivalent = 0.0;
			if (Conversion.ToInt(CmbAttributeVarient.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbAttributeVarient).SelectedRow.Cells["VarientEquivalent"].Value);
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			else
			{
				txtVarientEquivalent.Text = Equivalent.ToString();
			}
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetItemStock()
	{
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		try
		{
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			int ItemId = Conversion.ToInt(cmbItemName.Value);
			int wareId = Conversion.ToInt(CmbWareHouseFrom.Value);
			int JobLotId = Conversion.ToInt(cmbJobLot.Value);
			int VarientId = Conversion.ToInt(CmbAttributeVarient.Value);
			double Qty = Conversion.ToDouble((object)txtQty.Text);
			if (ItemId > 0 || wareId > 0)
			{
				DataTable DtStock = GetAvgRatesAndStockInHand.GetStockInHandAndAvgRateFromEvaluationConcrete(OrganizationId, CompanyId, ItemId, Conversion.ToDateTime((object)DocDate.Value), wareId, 0, 0);
				if (DtStock.Rows.Count > 0)
				{
					decimal AvgRate = default(decimal);
					LabBalanceQty.Text = Conversion.ToDecimal(DtStock.Rows[0]["QtyInHand"]).ToString();
					if (!FifoMethod)
					{
						AvgRate = Conversion.ToDecimal(DtStock.Rows[0]["AvgRate"]);
					}
					else if (ItemId > 0 && wareId > 0 && Qty > 0.0)
					{
						ReportsParameters obj1 = new ReportsParameters();
						obj1.OrganizationId = OrganizationId;
						obj1.CompanyId = CompanyId;
						obj1.ItemId = ItemId;
						obj1.DocDate = Conversion.ToDateTime((object)DocDate.Value);
						obj1.WarehouseId = wareId;
						obj1.JobLotId = JobLotId;
						obj1.Id = RecId;
						if (obj1.Id > 0)
						{
							obj1.DocumentTypeId = 1860;
						}
						obj1.stockUOM = (Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value) ? VarientId : 0);
						obj1.ItemQty = Qty;
						obj1.NetWeight = Qty;
						AvgRate = Conversion.ToDecimal((object)CommonServices.GetAvgRateFromFIFOMethodConcrete(obj1));
					}
					else
					{
						AvgRate = default(decimal);
					}
					txtAvgRate.Text = AvgRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
				else
				{
					LabBalanceQty.Text = 0.ToString();
					txtAvgRate.Text = 0.ToString();
				}
			}
			else
			{
				LabBalanceQty.Text = 0.ToString();
				txtAvgRate.Text = 0.ToString();
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (txtgatepsrno.Text == "")
		{
			MessageBox.Show("GatePassNo Field is Required");
			txtgatepsrno.Focus();
			return false;
		}
		if (txtvehichleno.Text == "")
		{
			MessageBox.Show("Vehicle No Field is Required");
			txtvehichleno.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbWareHouseFrom).ActiveRow == null || Conversion.ToInt(CmbWareHouseFrom.Value) == 0)
		{
			MessageBox.Show("WarehouseFrom field is required");
			CmbWareHouseFrom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbWareHouseTo).ActiveRow == null || Conversion.ToInt(cmbWareHouseTo.Value) == 0)
		{
			MessageBox.Show("WarehouseTo field is required");
			cmbWareHouseTo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbJobLot).ActiveRow == null || Conversion.ToInt(cmbJobLot.Value) == 0)
		{
			MessageBox.Show("JobLot From field is required");
			cmbJobLot.Focus();
			return false;
		}
		if (((UltraGridBase)CmbJobLotTo).ActiveRow == null || Conversion.ToInt(CmbJobLotTo.Value) == 0)
		{
			MessageBox.Show("JobLotTo field is required");
			CmbJobLotTo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItemName).ActiveRow == null || Conversion.ToInt(cmbItemName.Value) == 0)
		{
			MessageBox.Show("Item Name field is required");
			cmbItemName.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Pack Uom field is required");
			CmbPackUom.Focus();
			return false;
		}
		if (Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value) && Conversion.ToInt(CmbAttributeVarient.Value) == 0)
		{
			MessageBox.Show("Attribute Variant field is required");
			CmbAttributeVarient.Focus();
			return false;
		}
		if (!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value) && Conversion.ToInt(CmbAttributeVarient.Value) > 0)
		{
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
		}
		if (txtQty.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtQty.Text) == 0.0)
		{
			MessageBox.Show("Qty field is required");
			txtQty.Focus();
			return false;
		}
		if (txtitemweight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtitemweight.Text) == 0.0)
		{
			MessageBox.Show("ItemWeight field is required");
			txtitemweight.Focus();
			return false;
		}
		if (txtDetailNetWeight.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtDetailNetWeight.Text) == 0.0)
		{
			MessageBox.Show("NetWeight field is required");
			txtDetailNetWeight.Focus();
			return false;
		}
		if (txtAvgRate.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAvgRate.Text) == 0.0)
		{
			MessageBox.Show("AvgRate field is required");
			txtAvgRate.Focus();
			return false;
		}
		if (txtAmount.Text.Trim() == string.Empty || Conversion.ToDouble((object)txtAmount.Text) == 0.0)
		{
			MessageBox.Show("Amount field is required");
			txtAmount.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbWareHouseFrom.Value) == Conversion.ToInt(cmbWareHouseTo.Value))
		{
			if (Conversion.ToInt(cmbJobLot.Value) == Conversion.ToInt(CmbJobLotTo.Value))
			{
				MessageBox.Show("JoLotFrom Can not Equal to JobLotTo when Warehouse From and WarehouseTo is same");
				cmbJobLot.Focus();
				return false;
			}
			return true;
		}
		if (Conversion.ToInt(cmbJobLot.Value) == Conversion.ToInt(CmbJobLotTo.Value))
		{
			if (Conversion.ToInt(CmbWareHouseFrom.Value) == Conversion.ToInt(cmbWareHouseTo.Value))
			{
				MessageBox.Show("Warehouse From Can not Equal to WarehouseTo when JoLotFrom and JoLotTo is same");
				cmbJobLot.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ItemDetailFill();
			DocumentNoFill();
			WareHouseFill();
			GetJobLot();
			bindvarientunit(Conversion.ToInt(cmbItemName.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		ResetDetail();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			lstRemoveRecord.Clear();
			AT = new Attachment();
			RecId = 0;
			IsApproved = false;
			txtgatepsrno.Text = string.Empty;
			txtvehichleno.Text = string.Empty;
			txtRemarksHead.Text = string.Empty;
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			((Control)(object)cmbWareHouseTo).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((Control)(object)CmbJobLotTo).Text = string.Empty;
			((Control)(object)cmbItemName).Text = string.Empty;
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
			txtQty.Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnplus.Visible = true;
			btnSaveAs.Visible = false;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			((Control)(object)CmbWareHouseFrom).Enabled = true;
			((Control)(object)cmbItemName).Enabled = true;
			((Control)(object)cmbJobLot).Enabled = true;
			DocDate.Focus();
			DocumentNoFill();
			table.Rows.Clear();
			grd.DataSource = table;
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
			CmbWareHouseFrom.Focus();
			((Control)(object)CmbWareHouseFrom).Text = string.Empty;
			((Control)(object)cmbWareHouseTo).Text = string.Empty;
			((Control)(object)cmbJobLot).Text = string.Empty;
			((Control)(object)CmbJobLotTo).Text = string.Empty;
			((Control)(object)cmbItemName).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
			txtVarientEquivalent.Text = string.Empty;
			txtDetailNetWeight.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			txtitemweight.Text = string.Empty;
			txtAmount.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockTransferManual_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "tabControl2")
			{
				CmbWareHouseFrom.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
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
				table.Rows[updateDetailIndex]["WareHouseFromId"] = CmbWareHouseFrom.Value;
				table.Rows[updateDetailIndex]["WareHouseFrom"] = ((Control)(object)CmbWareHouseFrom).Text;
				table.Rows[updateDetailIndex]["WareHouseToId"] = cmbWareHouseTo.Value;
				table.Rows[updateDetailIndex]["WareHouseTo"] = ((Control)(object)cmbWareHouseTo).Text;
				table.Rows[updateDetailIndex]["JobLotId"] = cmbJobLot.Value;
				table.Rows[updateDetailIndex]["JobLot"] = ((Control)(object)cmbJobLot).Text;
				table.Rows[updateDetailIndex]["JobLotIdTo"] = CmbJobLotTo.Value;
				table.Rows[updateDetailIndex]["JobLotTo"] = ((Control)(object)CmbJobLotTo).Text;
				table.Rows[updateDetailIndex]["ItemId"] = cmbItemName.Value;
				table.Rows[updateDetailIndex]["ItemName"] = ((Control)(object)cmbItemName).Text;
				table.Rows[updateDetailIndex]["PackUomId"] = CmbPackUom.Value;
				table.Rows[updateDetailIndex]["PackUom"] = ((Control)(object)CmbPackUom).Text;
				table.Rows[updateDetailIndex]["AttributeVarientId"] = ((!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value)) ? ((object)0) : CmbAttributeVarient.Value);
				table.Rows[updateDetailIndex]["AttributeVarient"] = ((!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value)) ? "" : ((Control)(object)CmbAttributeVarient).Text);
				table.Rows[updateDetailIndex]["VarientUnit"] = ((!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value)) ? 0m : Conversion.ToDecimal((object)txtVarientEquivalent.Text));
				table.Rows[updateDetailIndex]["QTY"] = Conversion.ToDecimal((object)txtQty.Text);
				table.Rows[updateDetailIndex]["ItemWeight"] = Conversion.ToDecimal((object)txtitemweight.Text);
				table.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtDetailNetWeight.Text);
				table.Rows[updateDetailIndex]["ItemRate"] = Conversion.ToDouble((object)txtAvgRate.Text);
				table.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDouble((object)txtAmount.Text);
				table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				grd.DataSource = table;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ItemAmountCalculation();
				ResetDetail();
			}
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
			GridEXRow item = grd.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				CmbWareHouseFrom.Value = item.Cells["WareHouseFromId"].Value.ToString();
				cmbWareHouseTo.Value = item.Cells["WareHouseToId"].Value.ToString();
				cmbJobLot.Value = item.Cells["JobLotId"].Value.ToString();
				CmbJobLotTo.Value = item.Cells["JobLotIdTo"].Value.ToString();
				cmbItemName.Value = item.Cells["ItemId"].Value.ToString();
				bindvarientunit(Conversion.ToInt(item.Cells["ItemId"].Value));
				bindPackUom(Conversion.ToInt(item.Cells["ItemId"].Value));
				if (Conversion.ToInt(item.Cells["PackUomId"].Value) > 0)
				{
					CmbPackUom.Value = Conversion.ToInt(item.Cells["PackUomId"].Value);
				}
				if (Conversion.ToInt(item.Cells["AttributeVarientId"].Value) > 0)
				{
					CmbAttributeVarient.Value = Conversion.ToInt(item.Cells["AttributeVarientId"].Value);
				}
				txtVarientEquivalent.Text = item.Cells["VarientUnit"].Value.ToString();
				txtQty.Text = item.Cells["QTY"].Value.ToString();
				txtitemweight.Text = item.Cells["ItemWeight"].Value.ToString();
				txtDetailNetWeight.Text = item.Cells["NetWeight"].Value.ToString();
				txtAvgRate.Text = item.Cells["ItemRate"].Value.ToString();
				txtAmount.Text = item.Cells["ItemAmount"].Value.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Value.ToString();
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbWareHouseFrom.Focus();
			}
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
			if (FormValidationDetail())
			{
				table.Rows.Add(0, CmbWareHouseFrom.Value, ((Control)(object)CmbWareHouseFrom).Text.Trim(), cmbWareHouseTo.Value, ((Control)(object)cmbWareHouseTo).Text.Trim(), cmbJobLot.Value, ((Control)(object)cmbJobLot).Text.Trim(), CmbJobLotTo.Value, ((Control)(object)CmbJobLotTo).Text.Trim(), cmbItemName.Value, ((Control)(object)cmbItemName).Text.Trim(), CmbPackUom.Value, ((Control)(object)CmbPackUom).Text.Trim(), (!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value)) ? ((object)0) : CmbAttributeVarient.Value, (!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value)) ? "" : ((Control)(object)CmbAttributeVarient).Text.Trim(), (!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value)) ? 0m : Conversion.ToDecimal((object)txtVarientEquivalent.Text.Trim()), Conversion.ToDouble((object)txtQty.Text.Trim()), Conversion.ToDouble((object)txtitemweight.Text.Trim()), Conversion.ToDouble((object)txtDetailNetWeight.Text.Trim()), Conversion.ToDouble((object)txtAvgRate.Text), Conversion.ToDouble((object)txtAmount.Text), txtRemarksDetail.Text.Trim());
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				ResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					InvStockTransferDetail vd = new InvStockTransferDetail();
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
					vd.WarehouseFromId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
					vd.WarehouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
					vd.JobLotFromId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					vd.JobLotToId = Conversion.ToInt(r.Cells["JobLotIdTo"].Value);
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
					vd.AttributeVareintId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					vd.VareintEquivalent = Conversion.ToInt(r.Cells["VarientUnit"].Value);
					vd.ItemQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
					vd.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
					vd.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					vd.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					vd.ItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
					vd.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grd.UpdateData();
				AvgAmountCalculation();
			}
			if (e.Column.Key == "Edit")
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdSettings()
	{
		try
		{
			grd.ColumnAutoResize = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["WareHouseFromId"].Visible = false;
			grd.RootTable.Columns["WareHouseToId"].Visible = false;
			grd.RootTable.Columns["AttributeVarientId"].Visible = false;
			grd.RootTable.Columns["JobLotIdTo"].Visible = false;
			grd.RootTable.Columns["PackUomId"].Visible = false;
			grd.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VarientUnit"].FormatString = "#,##0.###";
			grd.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["QTY"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["QTY"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["QTY"].FormatString = "#,##0.###";
			grd.RootTable.Columns["QTY"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemWeight"].FormatString = "#,##0.###";
			grd.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.###";
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 50;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.RootTable.Columns.Add("Edit");
			grd.RootTable.Columns["Edit"].Key = "Edit";
			grd.RootTable.Columns["Edit"].Caption = "Edit";
			grd.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Edit"].ButtonText = "Edit";
			grd.RootTable.Columns["Edit"].Width = 50;
			grd.RootTable.Columns["Edit"].Position = 1;
			grd.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_0846: Unknown result type (might be due to invalid IL or missing references)
		//IL_084b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0866: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvStockTransferHeader sh = new InvStockTransferHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				sh.Id = RecId;
				sh.ModifyUserId = UserAccount.ID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			sh.DocumentTypeId = 1860;
			sh.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			sh.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			sh.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			sh.GpDate = Conversion.ToDateTime((object)DocDate.Value);
			sh.EntryDate = DateTime.Now;
			sh.ModifyDate = DateTime.Now;
			sh.ApprovedDate = DateTime.Now;
			sh.DocNo = Conversion.ToInt((object)txtdocno.Text);
			sh.RemarksHeader = Conversion.ToString((object)txtRemarksHead.Text);
			sh.VehicleNo = Conversion.ToString((object)txtvehichleno.Text);
			sh.ApprovedUserId = Conversion.ToInt((object)UserAccount.ID);
			sh.GPSrNo = Conversion.ToInt((object)txtgatepsrno.Text);
			sh.OrganizationId = UserAccount.OrganizationId;
			sh.CompanyId = UserAccount.CompanyId;
			sh.EntryUserId = UserAccount.ID;
			sh.ModifyUserId = UserAccount.ID;
			sh.IsApproved = false;
			sh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			sh.InvStockTransferDetailList = new List<InvStockTransferDetail>();
			if (lstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					sh.InvStockTransferDetailList.Add(lstRemoveRecord[j]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvStockTransferDetail vd = new InvStockTransferDetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				if (Conversion.ToDouble(r.Cells["ItemId"].Value) == 0.0)
				{
					throw new Exception("Item Name Feild Required In Detail Grid...");
				}
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				if (Conversion.ToDouble(r.Cells["JobLotId"].Value) == 0.0)
				{
					throw new Exception("JobLot From Feild Required In Detail Grid...");
				}
				vd.JobLotFromId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				if (Conversion.ToDouble(r.Cells["JobLotIdTo"].Value) == 0.0)
				{
					throw new Exception("JobLot To Feild Required In Detail Grid...");
				}
				vd.JobLotToId = Conversion.ToInt(r.Cells["JobLotIdTo"].Value);
				if (Conversion.ToDouble(r.Cells["WareHouseFromId"].Value) == 0.0)
				{
					throw new Exception("WareHouseFrom  Feild Required In Detail Grid...");
				}
				vd.WarehouseFromId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				if (Conversion.ToDouble(r.Cells["WareHouseToId"].Value) == 0.0)
				{
					throw new Exception("WareHouseTo Feild Required In Detail Grid...");
				}
				vd.WarehouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
				vd.ItemUomId = Conversion.ToInt(r.Cells["PackUomId"].Value);
				vd.AttributeVareintId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				vd.VareintEquivalent = Conversion.ToDecimal((object)r.Cells["VarientUnit"].Text);
				vd.ItemQty = Conversion.ToDecimal(r.Cells["QTY"].Value);
				vd.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
				vd.NetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
				vd.ItemRate = Conversion.ToDecimal(r.Cells["ItemRate"].Value);
				vd.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
				vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
				if (StockTransferFinancialEffectIsActive)
				{
					if (vd.ItemRate <= 0m)
					{
						throw new Exception("AvgRate field required");
					}
					if (vd.ItemAmount <= 0m)
					{
						throw new Exception("Amount field required");
					}
				}
				sh.InvStockTransferDetailList.Add(vd);
			}
			if (sh.InvStockTransferDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			sh.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				sh.AttachmentsList.Add(attach);
				if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
				{
					throw new Exception("Attachment Folder Path Not Configure Properly");
				}
				string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
				if (!File.Exists(fileSavePath))
				{
					File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
				}
			}
			int success = InvStockTransferHeader.Save(sh);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + sh.DocNo);
				if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
				{
					for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
					{
						using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
						{
							fs.Flush();
							fs.Close();
							fs.Dispose();
						}
						bool fileDeleted = false;
						int attempts = 0;
						while (!fileDeleted && attempts < 5)
						{
							try
							{
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
								fileDeleted = true;
							}
							catch (IOException)
							{
								attempts++;
								Thread.Sleep(500);
							}
						}
					}
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + sh.DocNo);
			}
			Reset();
			if (ChkBoxPrintPreview.Checked)
			{
				DataTable dt = InvStockTransferHeader.StockTransferRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 1860,
					Id = success
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1860-StockTransferManual-Slip.rpt");
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (IsApproved)
			{
				MessageBox.Show("Approved Record Not Update");
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void getUpdate(int ID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RecId = ID;
			InvStockTransferHeader Stock = new InvStockTransferHeader();
			Stock = InvStockTransferHeader.ReadById(ID);
			if (Stock != null)
			{
				cmbbranch1.Value = Conversion.ToInt((object)Stock.BranchesId);
				combproject1.Value = Conversion.ToInt((object)Stock.ProjectsId);
				tabControl1.SelectedIndex = 0;
				DocDate.Value = Conversion.ToDateTime((object)Stock.DocDate);
				txtdocno.Text = Conversion.ToString((object)Stock.DocNo);
				txtgpdate.Value = Conversion.ToDateTime((object)Stock.GpDate);
				txtgatepsrno.Text = Conversion.ToString((object)Stock.GPSrNo);
				txtvehichleno.Text = Conversion.ToString((object)Stock.VehicleNo);
				txtRemarksHead.Text = Conversion.ToString((object)Stock.RemarksHeader);
				IsApproved = Stock.IsApproved;
				table.Clear();
				for (int i = 0; i < Stock.InvStockTransferDetailList.Count; i++)
				{
					table.Rows.Add(Stock.InvStockTransferDetailList[i].Id, Stock.InvStockTransferDetailList[i].WarehouseFromId, Stock.InvStockTransferDetailList[i].WarehouseFrom, Stock.InvStockTransferDetailList[i].WarehouseToId, Stock.InvStockTransferDetailList[i].WarehouseTo, Stock.InvStockTransferDetailList[i].JobLotFromId, Stock.InvStockTransferDetailList[i].JobLotFrom, Stock.InvStockTransferDetailList[i].JobLotToId, Stock.InvStockTransferDetailList[i].JobLotTo, Stock.InvStockTransferDetailList[i].ItemId, Stock.InvStockTransferDetailList[i].ItemName, Stock.InvStockTransferDetailList[i].ItemUomId, Stock.InvStockTransferDetailList[i].PackUom, Stock.InvStockTransferDetailList[i].AttributeVareintId, Stock.InvStockTransferDetailList[i].AttributeVareint, Stock.InvStockTransferDetailList[i].VareintEquivalent, Stock.InvStockTransferDetailList[i].ItemQty, Stock.InvStockTransferDetailList[i].ItemWeight, Stock.InvStockTransferDetailList[i].NetWeight, Stock.InvStockTransferDetailList[i].ItemRate, Stock.InvStockTransferDetailList[i].ItemAmount, Stock.InvStockTransferDetailList[i].RemarksDetail);
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(RecId, base.Name);
				AT = new Attachment();
				foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
				{
					TempAttachments TA = new TempAttachments();
					TA.Id = 0;
					TA.Path = Attachment.Attachment;
					AT.lst.Add(TA);
				}
				btnsave.Visible = false;
				btnupdate.Visible = true;
				btnDelete.Visible = true;
				return;
			}
			throw new Exception("Record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					new InvStockTransferHeader();
					InvStockTransferHeader.DeleteByID(UserAccount.EntryUserId, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("RecId not Found");
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

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					fromdate.Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && formrights.DoHaveCanDelete && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnprint.Visible && btnprint.Enabled)
				{
					btnprint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.F12 && btnsave.Visible && btnsave.Enabled)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbWareHouseFrom.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grd_DoubleClick(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdhistory_DoubleClick_1(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)GridDetailHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdhistory).Focus();
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
			dt.Rows.Add("Ctrl+F12", "For Save As");
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

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			cmbperemeter.Focus();
		}
	}

	private void grdhistory_DoubleClick_1(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
			getUpdate(RecId);
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
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
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
			obj.DocumentTypeId = 1860;
			obj.FromDate = Conversion.ToDateTime((object)fromdate.Value);
			obj.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			obj.ToDocNo = Conversion.ToInt((object)txttodocno.Text);
			obj.FromDocNo = Conversion.ToInt((object)txtfromdocno.Text);
			if (Conversion.ToString((object)txtVehicleNoHistory.Text) != string.Empty)
			{
				obj.VehicleNo = Conversion.ToString((object)txtVehicleNoHistory.Text);
			}
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dthistoryfill = InvStockTransferHeader.FormHistory(obj);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dtGridHistory = new DataTable();
				dtGridHistory.Columns.Add("Id", typeof(int));
				dtGridHistory.Columns.Add("DocumentTypeId", typeof(int));
				dtGridHistory.Columns.Add("DocNo");
				dtGridHistory.Columns.Add("DocDate", typeof(DateTime));
				dtGridHistory.Columns.Add("GPSrNo");
				dtGridHistory.Columns.Add("GpDate", typeof(DateTime));
				dtGridHistory.Columns.Add("VehicleNo");
				dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtGridHistory.Columns.Add("EntryUser", typeof(string));
				dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtGridHistory.Columns.Add("ModifyUser");
				dtGridHistory.Columns.Add("RemarksHeader");
				dtGridHistory.Columns.Add("NoOfAttachments");
				dtGridHistory.Clear();
				for (int i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					dtGridHistory.Rows.Add(dthistoryfill.Rows[i]["Id"], dthistoryfill.Rows[i]["DocumentTypeId"], dthistoryfill.Rows[i]["DocNo"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["GPSrNo"], dthistoryfill.Rows[i]["GpDate"], dthistoryfill.Rows[i]["VehicleNo"], Conversion.ToDateTime(dthistoryfill.Rows[i]["EntryDate"]).ToShortDateString(), dthistoryfill.Rows[i]["EntryUserName"], Conversion.ToDateTime(dthistoryfill.Rows[i]["ModifyDate"]).ToShortDateString(), dthistoryfill.Rows[i]["ModifyUserName"], dthistoryfill.Rows[i]["RemarksHeader"], dthistoryfill.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtGridHistory;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
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

	private void HistoryGridSettings()
	{
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridAutoAdjustmentNew(grdhistory);
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1860);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString());
				getUpdate(RecId);
			}
			if (e.Column.Key == "Print")
			{
				DataTable dt = InvStockTransferHeader.StockTransferRegister(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 1860,
					Id = Conversion.ToInt((object)grdhistory.CurrentRow.Cells["Id"].Value.ToString())
				});
				if (dt.Rows.Count == 0 || dt == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "1860-StockTransferManual-Slip.rpt");
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
			txtfromdocno.Text = "";
			txttodocno.Text = "";
			txtvehichleno.Text = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count <= 0)
			{
				return;
			}
			int num = Conversion.ToInt(row.Cells["Id"].Value);
			InvStockTransferHeader obj = new InvStockTransferHeader();
			obj = InvStockTransferHeader.ReadById(num);
			if (obj.InvStockTransferDetailList != null && obj.InvStockTransferDetailList.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("FromWarehouse");
				dt.Columns.Add("ToWarehouse");
				dt.Columns.Add("JobLotFrom");
				dt.Columns.Add("JobLotTo");
				dt.Columns.Add("ItemName");
				dt.Columns.Add("AttributeVarient");
				dt.Columns.Add("VarientUnit");
				dt.Columns.Add("Qty", typeof(double));
				dt.Columns.Add("ItemWeight", typeof(double));
				dt.Columns.Add("NetWeight", typeof(double));
				dt.Columns.Add("ItemRate", typeof(double));
				dt.Columns.Add("ItemAmount", typeof(double));
				dt.Columns.Add("RemarksDetail");
				for (int i = 0; i < obj.InvStockTransferDetailList.Count; i++)
				{
					dt.Rows.Add(obj.InvStockTransferDetailList[i].WarehouseFrom, obj.InvStockTransferDetailList[i].WarehouseTo, obj.InvStockTransferDetailList[i].JobLotFrom, obj.InvStockTransferDetailList[i].JobLotTo, obj.InvStockTransferDetailList[i].ItemName, obj.InvStockTransferDetailList[i].AttributeVareint, obj.InvStockTransferDetailList[i].VareintEquivalent, obj.InvStockTransferDetailList[i].ItemQty, obj.InvStockTransferDetailList[i].ItemWeight, obj.InvStockTransferDetailList[i].NetWeight, obj.InvStockTransferDetailList[i].ItemRate, obj.InvStockTransferDetailList[i].ItemAmount, obj.InvStockTransferDetailList[i].RemarksDetail);
				}
				GridDetailHistory.DataSource = dt;
				GridDetailHistory.RetrieveStructure();
				GridDetailHistory.RootTable.Columns["Qty"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["Qty"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["Qty"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
				GridDetailHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				GridDetailHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
				GridDetailHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
				GridDetailHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
				GridDetailHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				CommonServices.GridAutoAdjustmentNew(GridDetailHistory);
				ctrlGrdBar6_Load(null, null);
			}
			else
			{
				GridDetailHistory.ClearStructure();
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

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
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

	private void ctrlGrdBar6_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridDetailHistory).Name);
			if (fs != null)
			{
				GridDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = InvStockTransferHeader.StockTransferRegister(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 1860,
				Id = RecId
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1860-StockTransferManual-Slip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		GetItemStock();
		AvgAmountCalculation();
	}

	private void cmbItemName_Leave(object sender, EventArgs e)
	{
		try
		{
			bindPackUom(Conversion.ToInt(cmbItemName.Value));
			if (Conversion.ToInt(cmbItemName.Value) > 0)
			{
				txtitemweight.Text = Conversion.ToString(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["ItemWeight"].Value);
				bindvarientunit(Conversion.ToInt(cmbItemName.Value));
				GetItemStock();
				AvgAmountCalculation();
			}
			else
			{
				txtitemweight.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AvgAmountCalculation()
	{
		try
		{
			decimal rate = Conversion.ToDecimal((object)txtAvgRate.Text);
			decimal NetWeight = Conversion.ToDecimal((object)txtDetailNetWeight.Text);
			decimal Qty = Conversion.ToDecimal((object)txtQty.Text);
			decimal ItemWeight = Conversion.ToDecimal((object)txtitemweight.Text);
			decimal VarientUnit = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
			if (Conversion.ToDecimal((object)txtQty.Text) > 0m && Conversion.ToDecimal((object)txtitemweight.Text) > 0m && Conversion.ToInt(cmbItemName.Value) > 0)
			{
				if (!Conversion.ToBool(((UltraDropDownBase)cmbItemName).SelectedRow.Cells["WithVariant"].Value))
				{
					VarientUnit = ((Conversion.ToInt(CmbPackUom.Value) <= 0) ? default(decimal) : Conversion.ToDecimal(((UltraDropDownBase)CmbPackUom).SelectedRow.Cells[2].Value));
					NetWeight = VarientUnit * Qty;
				}
				else
				{
					NetWeight = VarientUnit * Qty * ItemWeight;
				}
				txtDetailNetWeight.Text = NetWeight.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtDetailNetWeight.Text = 0.ToString();
			}
			if (Conversion.ToDecimal((object)txtQty.Text) > 0m && Conversion.ToDecimal((object)txtAvgRate.Text) > 0m)
			{
				decimal Amount = Qty * rate;
				txtAmount.Text = Amount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemAmountCalculation()
	{
		grd.UpdateData();
	}

	private void cmbItemName_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetItemStock();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void bindPackUom(int ItemId)
	{
		try
		{
			string Packuom = ((Control)(object)CmbPackUom).Text;
			((Control)(object)CmbPackUom).Text = string.Empty;
			DataTable dtUom = CommonServices.GetUomScheduleByItemId(ItemId);
			if (dtUom.Rows.Count > 0)
			{
				DDL.BindDDL(dtUom, CmbPackUom, "Id", "UOMCode", "PackUOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 75;
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[2].Width = 85;
				if (dtUom.AsEnumerable().Any((DataRow row) => row.Field<string>("UOMCode") == Packuom))
				{
					((Control)(object)CmbPackUom).Text = Packuom;
				}
				else
				{
					((Control)(object)CmbPackUom).Text = string.Empty;
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbWareHouseFrom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			GetItemStock();
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvgRate_TextChanged(object sender, EventArgs e)
	{
		AvgAmountCalculation();
	}

	private void txtDetailNetWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvgAmountCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtitemweight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			AvgAmountCalculation();
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

	private void txtHeadNetWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbTicketNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void cmbGatePass_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGrossWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAvailableStock_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtAvgRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtitemweight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfromdocno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txttodocno_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CmbPackUom_Leave(object sender, EventArgs e)
	{
		try
		{
			AvgAmountCalculation();
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
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0435: Expected O, but got Unknown
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Expected O, but got Unknown
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e5: Expected O, but got Unknown
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Expected O, but got Unknown
		//IL_05cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Expected O, but got Unknown
		//IL_05d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e2: Expected O, but got Unknown
		//IL_06eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f5: Expected O, but got Unknown
		//IL_0717: Unknown result type (might be due to invalid IL or missing references)
		//IL_0721: Expected O, but got Unknown
		//IL_074e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0758: Expected O, but got Unknown
		//IL_0f2c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f36: Expected O, but got Unknown
		//IL_5eeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ef5: Expected O, but got Unknown
		//IL_5f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f23: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance42 = new Appearance();
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
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
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
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
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
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		GridEXLayout GridDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grd = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbPackUom = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.btnplus = new System.Windows.Forms.Button();
		this.txtitemweight = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbAttributeVarient = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.LabBalanceQty = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lblAmount = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.CmbWareHouseFrom = new UltraCombo();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.lblAvgRate = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.cmbJobLot = new UltraCombo();
		this.CmbJobLotTo = new UltraCombo();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label8 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbItemName = new UltraCombo();
		this.txtDetailNetWeight = new System.Windows.Forms.TextBox();
		this.cmbWareHouseTo = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label38 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtgpdate = new System.Windows.Forms.DateTimePicker();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtgatepsrno = new System.Windows.Forms.TextBox();
		this.txtRemarksHead = new System.Windows.Forms.TextBox();
		this.txtvehichleno = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.combproject1 = new UltraCombo();
		this.cmbbranch1 = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.GridDetailHistory = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar6 = new grid.User_Conrtols.CtrlGrdBar();
		this.label27 = new System.Windows.Forms.Label();
		this.grdhistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.cmbperemeter = new UltraCombo();
		this.txtVehicleNoHistory = new System.Windows.Forms.TextBox();
		this.btnshow = new System.Windows.Forms.Button();
		this.label10 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txttodocno = new System.Windows.Forms.TextBox();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.txtfromdocno = new System.Windows.Forms.TextBox();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.bg = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combproject1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(967, 661);
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
		this.tabControl1.Size = new System.Drawing.Size(967, 661);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(959, 632);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 279);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(955, 351);
		this.panel6.TabIndex = 4;
		this.panel3.Controls.Add(this.tabControl2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(955, 351);
		this.panel3.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(955, 351);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add(this.panel5);
		this.tabPage3.Controls.Add(this.panel11);
		this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(947, 322);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 29);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(941, 290);
		this.panel5.TabIndex = 54;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(941, 290);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar5);
		this.panel11.Controls.Add(this.label23);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(941, 26);
		this.panel11.TabIndex = 53;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(897, 1);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar5.MyGrid = this.grd;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(40, 24);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.combproject1);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranch1);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 52);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(955, 227);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.groupBox2.Controls.Add(this.label1);
		this.groupBox2.Controls.Add(this.btnplus);
		this.groupBox2.Controls.Add(this.txtitemweight);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeVarient);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.txtVarientEquivalent);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.txtAmount);
		this.groupBox2.Controls.Add(this.LabBalanceQty);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add(this.lblAmount);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom);
		this.groupBox2.Controls.Add(this.txtAvgRate);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.lblAvgRate);
		this.groupBox2.Controls.Add(this.label21);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbJobLot);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobLotTo);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.groupBox2.Controls.Add(this.txtDetailNetWeight);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWareHouseTo);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtQty);
		this.groupBox2.Controls.Add(this.label38);
		this.groupBox2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(4, 96);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(944, 125);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance105).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(860, 36);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 5;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(CmbPackUom_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(860, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(61, 15);
		this.label1.TabIndex = 55;
		this.label1.Text = "Pack Uom";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(879, 92);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(26, 26);
		this.btnplus.TabIndex = 14;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.txtitemweight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtitemweight.Enabled = false;
		this.txtitemweight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtitemweight.Location = new System.Drawing.Point(278, 94);
		this.txtitemweight.Name = "txtitemweight";
		this.txtitemweight.Size = new System.Drawing.Size(80, 23);
		this.txtitemweight.TabIndex = 9;
		this.txtitemweight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtitemweight.TextChanged += new System.EventHandler(txtitemweight_TextChanged);
		this.txtitemweight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtitemweight_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(278, 71);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(74, 15);
		this.label28.TabIndex = 59;
		this.label28.Text = "Item Weight";
		this.CmbAttributeVarient.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeVarient.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeVarient.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeVarient.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttributeVarient.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Location = new System.Drawing.Point(4, 92);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Name = "CmbAttributeVarient";
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Size = new System.Drawing.Size(109, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label51.Location = new System.Drawing.Point(6, 71);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(94, 15);
		this.label51.TabIndex = 53;
		this.label51.Text = "Attribute Variant";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label52.Location = new System.Drawing.Point(115, 71);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(69, 15);
		this.label52.TabIndex = 55;
		this.label52.Text = "Variant Unit";
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(115, 94);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(68, 23);
		this.txtVarientEquivalent.TabIndex = 7;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(4, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(100, 15);
		this.label3.TabIndex = 0;
		this.label3.Text = "WareHouse From";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(546, 94);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(106, 23);
		this.txtAmount.TabIndex = 12;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.LabBalanceQty.AutoSize = true;
		this.LabBalanceQty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.LabBalanceQty.Location = new System.Drawing.Point(786, 17);
		this.LabBalanceQty.Name = "LabBalanceQty";
		this.LabBalanceQty.Size = new System.Drawing.Size(14, 15);
		this.LabBalanceQty.TabIndex = 0;
		this.LabBalanceQty.Text = "0";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(642, 17);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(67, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Item Name";
		this.lblAmount.AutoSize = true;
		this.lblAmount.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAmount.Location = new System.Drawing.Point(546, 71);
		this.lblAmount.Name = "lblAmount";
		this.lblAmount.Size = new System.Drawing.Size(51, 15);
		this.lblAmount.TabIndex = 51;
		this.lblAmount.Text = "Amount";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(879, 64);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(57, 26);
		this.btnCancelUpdateDetial.TabIndex = 16;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnplus_Leave);
		this.CmbWareHouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance104;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWareHouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Location = new System.Drawing.Point(4, 36);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Name = "CmbWareHouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Size = new System.Drawing.Size(166, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFrom).Leave += new System.EventHandler(CmbWareHouseFrom_TextChanged);
		this.txtAvgRate.BackColor = System.Drawing.Color.White;
		this.txtAvgRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgRate.Enabled = false;
		this.txtAvgRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAvgRate.Location = new System.Drawing.Point(461, 94);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.Size = new System.Drawing.Size(84, 23);
		this.txtAvgRate.TabIndex = 11;
		this.txtAvgRate.Text = "0";
		this.txtAvgRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtAvgRate.TextChanged += new System.EventHandler(txtAvgRate_TextChanged);
		this.txtAvgRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtAvgRate_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(879, 92);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(57, 26);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(349, 17);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(77, 15);
		this.label7.TabIndex = 35;
		this.label7.Text = "Job Lot From";
		this.lblAvgRate.AutoSize = true;
		this.lblAvgRate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblAvgRate.Location = new System.Drawing.Point(461, 71);
		this.lblAvgRate.Name = "lblAvgRate";
		this.lblAvgRate.Size = new System.Drawing.Size(54, 15);
		this.lblAvgRate.TabIndex = 49;
		this.lblAvgRate.Text = "Avg Rate";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(495, 17);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(62, 15);
		this.label21.TabIndex = 35;
		this.label21.Text = "Job Lot To";
		this.cmbJobLot.AllowNull = (DefaultableBoolean)1;
		this.cmbJobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbJobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbJobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance50).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance51).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance51).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance51).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbJobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbJobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Location = new System.Drawing.Point(349, 36);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Name = "cmbJobLot";
		((System.Windows.Forms.Control)(object)this.cmbJobLot).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.cmbJobLot).TabIndex = 2;
		this.CmbJobLotTo.AllowNull = (DefaultableBoolean)1;
		this.CmbJobLotTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobLotTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobLotTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobLotTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbJobLotTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Location = new System.Drawing.Point(495, 36);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Name = "CmbJobLotTo";
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobLotTo).TabIndex = 3;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(654, 94);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(220, 23);
		this.txtRemarksDetail.TabIndex = 13;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(359, 71);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(68, 15);
		this.label8.TabIndex = 37;
		this.label8.Text = "Net Weight";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(654, 71);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 16;
		this.label12.Text = "Remarks";
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(642, 36);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(216, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItemName).TextChanged += new System.EventHandler(cmbItemName_TextChanged);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Leave += new System.EventHandler(cmbItemName_Leave);
		this.txtDetailNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDetailNetWeight.Enabled = false;
		this.txtDetailNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDetailNetWeight.Location = new System.Drawing.Point(359, 94);
		this.txtDetailNetWeight.Name = "txtDetailNetWeight";
		this.txtDetailNetWeight.ReadOnly = true;
		this.txtDetailNetWeight.ShortcutsEnabled = false;
		this.txtDetailNetWeight.Size = new System.Drawing.Size(100, 23);
		this.txtDetailNetWeight.TabIndex = 10;
		this.txtDetailNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDetailNetWeight.TextChanged += new System.EventHandler(txtDetailNetWeight_TextChanged);
		this.txtDetailNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGrossWeight_KeyPress);
		this.cmbWareHouseTo.AllowNull = (DefaultableBoolean)1;
		this.cmbWareHouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWareHouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWareHouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance58;
		((SpecialBoxBase)((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWareHouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbWareHouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Location = new System.Drawing.Point(171, 36);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Name = "cmbWareHouseTo";
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).Size = new System.Drawing.Size(176, 26);
		((System.Windows.Forms.Control)(object)this.cmbWareHouseTo).TabIndex = 1;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(171, 17);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(85, 15);
		this.label11.TabIndex = 43;
		this.label11.Text = "WareHouse To";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(184, 94);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(93, 23);
		this.txtQty.TabIndex = 8;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtqty_KeyPress);
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label38.Location = new System.Drawing.Point(184, 71);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 13;
		this.label38.Text = "Qty";
		this.groupBox1.Controls.Add(this.label26);
		this.groupBox1.Controls.Add(this.txtgpdate);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtgatepsrno);
		this.groupBox1.Controls.Add(this.txtRemarksHead);
		this.groupBox1.Controls.Add(this.txtvehichleno);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(944, 95);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(6, 43);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(81, 15);
		this.label26.TabIndex = 20;
		this.label26.Text = "Gp No && Date";
		this.txtgpdate.CustomFormat = "dd-MMM-yy";
		this.txtgpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgpdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtgpdate.Location = new System.Drawing.Point(172, 39);
		this.txtgpdate.Name = "txtgpdate";
		this.txtgpdate.Size = new System.Drawing.Size(117, 23);
		this.txtgpdate.TabIndex = 3;
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(172, 13);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(117, 23);
		this.DocDate.TabIndex = 1;
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(94, 13);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(75, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(6, 17);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(88, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No && Date";
		this.label15.BackColor = System.Drawing.Color.LightGray;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 11f, System.Drawing.FontStyle.Bold);
		this.label15.Location = new System.Drawing.Point(291, 13);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(646, 23);
		this.label15.TabIndex = 17;
		this.label15.Text = "Remarks";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtgatepsrno.BackColor = System.Drawing.Color.White;
		this.txtgatepsrno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtgatepsrno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtgatepsrno.Location = new System.Drawing.Point(94, 39);
		this.txtgatepsrno.Name = "txtgatepsrno";
		this.txtgatepsrno.Size = new System.Drawing.Size(75, 23);
		this.txtgatepsrno.TabIndex = 2;
		this.txtgatepsrno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(cmbGatePass_KeyPress);
		this.txtRemarksHead.BackColor = System.Drawing.Color.White;
		this.txtRemarksHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHead.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksHead.Location = new System.Drawing.Point(291, 39);
		this.txtRemarksHead.Multiline = true;
		this.txtRemarksHead.Name = "txtRemarksHead";
		this.txtRemarksHead.Size = new System.Drawing.Size(646, 48);
		this.txtRemarksHead.TabIndex = 5;
		this.txtvehichleno.BackColor = System.Drawing.Color.White;
		this.txtvehichleno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtvehichleno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtvehichleno.Location = new System.Drawing.Point(94, 64);
		this.txtvehichleno.Name = "txtvehichleno";
		this.txtvehichleno.Size = new System.Drawing.Size(195, 23);
		this.txtvehichleno.TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(6, 68);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(64, 15);
		this.label4.TabIndex = 13;
		this.label4.Text = "Vehicle No";
		this.combproject1.AllowNull = (DefaultableBoolean)1;
		this.combproject1.AutoCompleteMode = (AutoCompleteMode)4;
		this.combproject1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combproject1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combproject1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combproject1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combproject1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combproject1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combproject1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance96).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance97).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance97).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance97).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).TextHAlignAsString = "Left";
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combproject1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combproject1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combproject1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combproject1).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combproject1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combproject1).Location = new System.Drawing.Point(1094, 43);
		((System.Windows.Forms.Control)(object)this.combproject1).Name = "combproject1";
		((System.Windows.Forms.Control)(object)this.combproject1).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.combproject1).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combproject1).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combproject1).Visible = false;
		this.cmbbranch1.AllowNull = (DefaultableBoolean)1;
		this.cmbbranch1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranch1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranch1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance17;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranch1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbbranch1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Location = new System.Drawing.Point(1094, 70);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Name = "cmbbranch1";
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranch1).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).TabStop = false;
		((System.Windows.Forms.Control)(object)this.cmbbranch1).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(1046, 76);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(44, 15);
		this.label13.TabIndex = 0;
		this.label13.Text = "Branch";
		this.label13.Visible = false;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(1046, 49);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(44, 15);
		this.label18.TabIndex = 0;
		this.label18.Text = "Project";
		this.label18.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(1046, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc No";
		this.label5.Visible = false;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ChkBoxPrintPreview);
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(955, 50);
		this.panel2.TabIndex = 0;
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(866, 29);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkBoxPrintPreview.TabIndex = 6;
		this.ChkBoxPrintPreview.Text = "Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(6, 29);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(128, 19);
		this.label24.TabIndex = 35;
		this.label24.Text = "Stock Transfer";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[14]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnupdate, this.btnDelete, this.btnsave, this.btnSaveAs, this.toolStripSeparator5, this.btnAttachment, this.toolStripSeparator3,
			this.btnprint, this.toolStripSeparator4, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(955, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(100, 24);
		this.btnprint.Text = "1860-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(959, 632);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(955, 628);
		this.panel7.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailHistory);
		this.panel9.Controls.Add(this.panel14);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel9.Controls.Add(this.panel15);
		this.panel9.Controls.Add(this.panel8);
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.toolStrip3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(955, 628);
		this.panel9.TabIndex = 47;
		this.GridDetailHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailHistory).BackColor = System.Drawing.Color.White;
		GridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailHistory.DesignTimeLayout = GridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridDetailHistory.DynamicFiltering = true;
		this.GridDetailHistory.FilterMode = (FilterMode)2;
		this.GridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailHistory.GroupByBoxVisible = false;
		this.GridDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Location = new System.Drawing.Point(0, 552);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Name = "GridDetailHistory";
		((ScrollableBase)this.GridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Size = new System.Drawing.Size(955, 76);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).TabIndex = 2;
		this.GridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.ctrlGrdBar6);
		this.panel14.Controls.Add(this.label27);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 530);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(955, 22);
		this.panel14.TabIndex = 54;
		this.ctrlGrdBar6.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar6.FormName = this;
		this.ctrlGrdBar6.Location = new System.Drawing.Point(911, 0);
		this.ctrlGrdBar6.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar6.MyGrid = this.GridDetailHistory;
		this.ctrlGrdBar6.Name = "ctrlGrdBar6";
		this.ctrlGrdBar6.Size = new System.Drawing.Size(40, 20);
		this.ctrlGrdBar6.TabIndex = 24;
		this.ctrlGrdBar6.Load += new System.EventHandler(ctrlGrdBar6_Load);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(4, 3);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(299, 17);
		this.label27.TabIndex = 37;
		this.label27.Text = "Detail Information Of Above Selected Row";
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 151);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(955, 379);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.ctrlGrdBar3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 129);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(955, 22);
		this.panel15.TabIndex = 53;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(912, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdhistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel8.Controls.Add(this.groupBox3);
		this.panel8.Controls.Add(this.bg);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 53);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(955, 76);
		this.panel8.TabIndex = 0;
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox3.Controls.Add(this.txtVehicleNoHistory);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label10);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.txttodocno);
		this.groupBox3.Controls.Add(this.fromdate);
		this.groupBox3.Controls.Add(this.txtfromdocno);
		this.groupBox3.Controls.Add(this.ToDate);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(2, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(682, 71);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance99).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance99).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(97, 14);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.txtVehicleNoHistory.BackColor = System.Drawing.Color.White;
		this.txtVehicleNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtVehicleNoHistory.Location = new System.Drawing.Point(440, 43);
		this.txtVehicleNoHistory.Name = "txtVehicleNoHistory";
		this.txtVehicleNoHistory.Size = new System.Drawing.Size(156, 23);
		this.txtVehicleNoHistory.TabIndex = 5;
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(601, 41);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(68, 26);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(348, 47);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(64, 15);
		this.label10.TabIndex = 167;
		this.label10.Text = "Vehicle No";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(5, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(60, 15);
		this.label20.TabIndex = 592;
		this.label20.Text = "Date Type";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(5, 47);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(91, 15);
		this.label34.TabIndex = 12;
		this.label34.Text = "From && To Date";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(348, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(82, 15);
		this.label37.TabIndex = 28;
		this.label37.Text = "No From && To";
		this.txttodocno.BackColor = System.Drawing.Color.White;
		this.txttodocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txttodocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txttodocno.Location = new System.Drawing.Point(518, 16);
		this.txttodocno.Name = "txttodocno";
		this.txttodocno.ReadOnly = true;
		this.txttodocno.Size = new System.Drawing.Size(75, 23);
		this.txttodocno.TabIndex = 4;
		this.txttodocno.TabStop = false;
		this.txttodocno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txttodocno_KeyPress);
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(97, 43);
		this.fromdate.Name = "fromdate";
		this.fromdate.Size = new System.Drawing.Size(123, 23);
		this.fromdate.TabIndex = 1;
		this.txtfromdocno.BackColor = System.Drawing.Color.White;
		this.txtfromdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfromdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfromdocno.Location = new System.Drawing.Point(440, 16);
		this.txtfromdocno.Name = "txtfromdocno";
		this.txtfromdocno.ReadOnly = true;
		this.txtfromdocno.Size = new System.Drawing.Size(75, 23);
		this.txtfromdocno.TabIndex = 3;
		this.txtfromdocno.TabStop = false;
		this.txtfromdocno.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfromdocno_KeyPress);
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(223, 43);
		this.ToDate.Name = "ToDate";
		this.ToDate.Size = new System.Drawing.Size(123, 23);
		this.ToDate.TabIndex = 2;
		this.bg.AutoSize = true;
		this.bg.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.bg.Location = new System.Drawing.Point(6, 11);
		this.bg.Name = "bg";
		this.bg.Size = new System.Drawing.Size(0, 15);
		this.bg.TabIndex = 164;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label25);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 27);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(955, 26);
		this.panel13.TabIndex = 49;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(3, 3);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(191, 19);
		this.label25.TabIndex = 36;
		this.label25.Text = "Stock Transfer History";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(955, 27);
		this.toolStrip3.TabIndex = 48;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(967, 661);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmStockTransferManualConcrete";
		this.Text = "Stock Transfer";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockTransferManual_FormClosing);
		base.Load += new System.EventHandler(PurchsaeOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbJobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbJobLotTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWareHouseTo).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combproject1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranch1).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
