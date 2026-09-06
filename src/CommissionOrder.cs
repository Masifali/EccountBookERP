// using Architecture.Reports;
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
using Architecture.BLL.CmTr.TransactionForm;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.CmTr.TransactionForm;
using Architecture.Model.Inventory;
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
namespace Architecture.WinApp.CmTr.Transaction_Forms
{

public class CommissionOrder : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtDetail = new DataTable();

	private Attachment At = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtSupplier = new DataTable();

	private DataTable lst = new DataTable();

	private DataTable dtgrid = new DataTable();

	private DataTable dtCommission = new DataTable();

	private DataTable dtsummaryBySupplier = new DataTable();

	private bool ItemSearchWithNameOrCode;

	private int updateDetailIndex;

	private bool HasMultiCurrencyFeature;

	private bool UpdateMode;

	private int RecId;

	private List<CommTradeOrderDetail> lstRemoveRecord = new List<CommTradeOrderDetail>();

	private IContainer components;

	private Panel panel1;

	private GroupBox groupBox1;

	private TextBox txtfcyamount;

	private RichTextBox txtRemarks;

	private Label label11;

	private Label label23;

	private TextBox txtexchangerate;

	private TextBox txtmanualref;

	private Label label22;

	internal UltraCombo cmbpayterm;

	private Label label2;

	private DateTimePicker txtdocdate;

	private TextBox txtduedays;

	private Label label4;

	private Label label21;

	private Label label17;

	private Label label3;

	private TextBox txtdocno;

	internal UltraCombo cmbcurrency;

	internal UltraCombo cmbCustomername;

	private Label label20;

	private Label label7;

	private Label label5;

	private Label label6;

	internal UltraCombo cmbdeliveryterm;

	private TextBox txtCusOrderAmt;

	private GroupBox groupBox2;

	private Label label27;

	private TextBox txtsubbrokeryamnt;

	private Label label26;

	private TextBox txtsubbrokeryrate;

	private Label label25;

	internal UltraCombo cmbsubbrokeryacc;

	private Label label19;

	private TextBox txtbrokeryamount;

	private Label label18;

	private TextBox txtbrokeryrate;

	private Label label15;

	internal UltraCombo cmbcommtype;

	private Label label14;

	private TextBox txtcommamount;

	private Label label13;

	private TextBox txtcommrate;

	private Label label12;

	internal UltraCombo cmbCommAgent;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel6;

	private Label label39;

	private TabPage tabPage2;

	private ToolStrip toolStrip2;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnprint;

	private ToolStripButton e;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnShortCutKeys;

	private Panel panel7;

	private Panel panel2;

	private Button btnAddDetail;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Label label34;

	private TextBox txtcustremarks;

	private Label label35;

	private TextBox txtcustnetwt;

	private GroupBox groupBox5;

	private RadioButton rdSearchByCode;

	private RadioButton rdSearchByName;

	internal UltraCombo cmbitem;

	private Label label28;

	private TextBox txtItemQty;

	internal UltraCombo cmbuom;

	internal UltraCombo cmbcropYear;

	internal UltraCombo CmbPackingType;

	private Label label29;

	private Label labItemName;

	private Label label30;

	private Label label31;

	private Label label32;

	private TextBox txtsupremarks;

	private Label label33;

	private TextBox txtSupNetWeight;

	internal UltraCombo cmbsupplier;

	private Label label42;

	private Panel panel12;

	private Label label24;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel5;

	internal GridEX grdDetail;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnLoadAllHistory;

	private Panel panel10;

	private Label label44;

	private Panel panel9;

	private Panel panel8;

	private Label label43;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdHistory;

	internal GridEX grdHistoryDetail;

	private Label label9;

	private Label label8;

	private TextBox txtsupamount;

	private TextBox txtsuprate;

	private Label label10;

	private Label label16;

	private TextBox txtcusamount;

	private TextBox txtcusrate;

	private Panel panel3;

	internal GridEX grdsummary;

	private Panel panel4;

	private Label label1;

	private Panel panel11;

	public CommissionOrder(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void CommissionOrder_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(1200, 750);
		formright = CommonServices.SetRightsValueInRightsObject(base.Name);
		btnsave.Enabled = formright.DoHaveSaveRight;
		btnUpdate.Enabled = formright.DoHaveUpdateRights;
		btnUpdate.Enabled = formright.DoHavePrintRights;
		ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
		ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
		ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
		ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
		ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
		ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
		ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
		ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
		ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
		ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
		ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
		ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
		ItemSearchWithNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
		if (ItemSearchWithNameOrCode)
		{
			rdSearchByCode.Checked = true;
		}
		else
		{
			rdSearchByName.Checked = true;
		}
		lst.Columns.Add("Id", typeof(int));
		lst.Columns.Add("ItemName", typeof(string));
		lst.Columns.Add("ItemCode", typeof(string));
		dtCommission.Columns.Add("Id", typeof(int));
		dtCommission.Columns.Add("CommissionAgentId", typeof(int));
		dtCommission.Columns.Add("ChargesIncentiveId", typeof(int));
		dtCommission.Columns.Add("RevenueExpenseId", typeof(int));
		dtCommission.Columns.Add("CalcParameterId", typeof(int));
		dtCommission.Columns.Add("Rate", typeof(decimal));
		dtCommission.Columns.Add("Amount", typeof(decimal));
		dtCommission.Columns.Add("SupplierCustomerId", typeof(int));
		dtsummaryBySupplier.Columns.Add("SupplierId", typeof(int));
		dtsummaryBySupplier.Columns.Add("ItemAmount", typeof(decimal));
		grdsummary.DataSource = dtsummaryBySupplier;
		grdsummary.RetrieveStructure();
		grdsummarysetting();
		dtDetail.Columns.Add("Id", typeof(int));
		dtDetail.Columns.Add("ItemId", typeof(int));
		dtDetail.Columns.Add("ItemCode", typeof(string));
		dtDetail.Columns.Add("ItemName", typeof(string));
		dtDetail.Columns.Add("CropYearId", typeof(int));
		dtDetail.Columns.Add("CropYear", typeof(string));
		dtDetail.Columns.Add("PackingTypeId", typeof(int));
		dtDetail.Columns.Add("PackingType", typeof(string));
		dtDetail.Columns.Add("UomId", typeof(int));
		dtDetail.Columns.Add("UOM", typeof(string));
		dtDetail.Columns.Add("Qty", typeof(int));
		dtDetail.Columns.Add("SupplierId", typeof(int));
		dtDetail.Columns.Add("SupplierName", typeof(string));
		dtDetail.Columns.Add("SupNetWeight", typeof(decimal));
		dtDetail.Columns.Add("SupRate", typeof(decimal));
		dtDetail.Columns.Add("SupAmount", typeof(decimal));
		dtDetail.Columns.Add("SupRemarks", typeof(string));
		dtDetail.Columns.Add("CusNetWeight", typeof(decimal));
		dtDetail.Columns.Add("CusRate", typeof(decimal));
		dtDetail.Columns.Add("CusAmount", typeof(decimal));
		dtDetail.Columns.Add("CusRemarks", typeof(string));
		dtDetail.Columns.Add("CustFcyAmount", typeof(decimal));
		dtDetail.Columns.Add("SupFcyAmount", typeof(decimal));
		DocumentNoFill();
		suppliercustomer();
		PaymentTerms();
		DeliveryTerm();
		CurrencyFill();
		CommissionTypeFill();
		Itemfill();
		PackingTypefill();
		Cropfill();
		PackUOMFill();
		multiCurrencyFeature();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		txtdocno.Focus();
		btnAddDetail.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelDetail.Visible = false;
		ConfigurationDefault();
	}

	private void suppliercustomer()
	{
		try
		{
			int suppId = Conversion.ToInt(cmbCustomername.Value);
			int CusId = Conversion.ToInt(cmbsupplier.Value);
			int agentId = Conversion.ToInt(cmbCommAgent.Value);
			int SubId = Conversion.ToInt(cmbsubbrokeryacc.Value);
			dtSupplier = CommonServices.SupplierCustomerGetAllServiceBind();
			if (dtSupplier.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtSupplier, cmbCustomername, "Id", "CompanyName", "Customer Name", true);
				DDL.BindDDLNew(dtSupplier, cmbsupplier, "Id", "CompanyName", "Supplier Name", true);
				DDL.BindDDLNew(dtSupplier, cmbCommAgent, "Id", "CompanyName", "Broker Ac", true);
				DDL.BindDDLNew(dtSupplier, cmbsubbrokeryacc, "Id", "CompanyName", "Broker Ac", true);
				((UltraGridBase)cmbCustomername).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbCommAgent).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)cmbsubbrokeryacc).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
			if (suppId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == suppId))
				{
					cmbCustomername.Value = suppId;
				}
				else
				{
					((Control)(object)cmbCustomername).Text = string.Empty;
				}
			}
			if (CusId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == CusId))
				{
					cmbsupplier.Value = CusId;
				}
				else
				{
					((Control)(object)cmbsupplier).Text = string.Empty;
				}
			}
			if (agentId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == agentId))
				{
					cmbCommAgent.Value = agentId;
				}
				else
				{
					((Control)(object)cmbCommAgent).Text = string.Empty;
				}
			}
			if (agentId > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SubId))
				{
					cmbsubbrokeryacc.Value = SubId;
				}
				else
				{
					((Control)(object)cmbsubbrokeryacc).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Supplier Name Not Found");
		}
	}

	private void PaymentTerms()
	{
		try
		{
			int PayId = Conversion.ToInt(cmbpayterm.Value);
			DataTable dtTerm = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(5, 0);
			if (dtTerm.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtTerm, cmbpayterm, "Id", "LookUpName", "Payment Term", true);
				((UltraGridBase)cmbpayterm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (PayId > 0)
			{
				if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == PayId))
				{
					cmbpayterm.Value = PayId;
				}
				else
				{
					((Control)(object)cmbpayterm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTerm()
	{
		try
		{
			int DeliveryId = Conversion.ToInt(cmbdeliveryterm.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DeliveryTerm", typeof(string));
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, cmbdeliveryterm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: true);
			((UltraGridBase)cmbdeliveryterm).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (DeliveryId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DeliveryId))
				{
					cmbdeliveryterm.Value = DeliveryId;
				}
				else
				{
					((Control)(object)cmbdeliveryterm).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CurrencyFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbcurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbcurrency, "Id", "CurrencyCode", "Currency", true);
				((UltraGridBase)cmbcurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbcurrency.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbcurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			int OrderId = Conversion.ToInt(cmbcommtype.Value);
			DataTable dt = new DataTable();
			DataTable dtlook = CommTradeRevenueExpenseSchedule.GetLookUpsBylookTypeId(4, 0);
			if (dtlook.Rows.Count > 0)
			{
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("CommissionType", typeof(string));
				dt.Clear();
				for (int i = 0; i < dtlook.Rows.Count; i++)
				{
					dt.Rows.Add(dtlook.Rows[i]["Id"], dtlook.Rows[i]["LookupName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbcommtype, "Id", "CommissionType", "CommissionType", ZeroIndex: true);
					((UltraGridBase)cmbcommtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((UltraGridBase)cmbcommtype).DataSource = null;
					((Control)(object)cmbcommtype).Text = string.Empty;
				}
			}
			if (OrderId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbcommtype.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbcommtype).Text = string.Empty;
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
			int code = CommTradeOrder.GenerateCode(new CommTradeOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1705,
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

	private void Itemfill()
	{
		try
		{
			int idits = Conversion.ToInt(cmbitem.Value);
			DataTable dt = CommonServices.ItemGetForComboServiceBind();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					lst.Rows.Add(Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToString(dt.Rows[i]["ItemName"]), Conversion.ToString(dt.Rows[i]["ItemCode"]));
				}
			}
			if (rdSearchByName.Checked)
			{
				DDL.BindDDL(lst, cmbitem, "Id", "ItemName", "Item", ZeroIndex: true);
			}
			else
			{
				DDL.BindDDL(lst, cmbitem, "Id", "ItemCode", "Item", ZeroIndex: true);
			}
			((UltraGridBase)cmbitem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					cmbitem.Value = idits;
				}
				else
				{
					((Control)(object)cmbitem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackingTypefill()
	{
		try
		{
			int idits = Conversion.ToInt(CmbPackingType.Value);
			DataTable lst = CommonServices.InvPackingTypeGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDLNew(lst, CmbPackingType, "Id", "PackTypeDesc", "Packing Type", true);
				((UltraGridBase)CmbPackingType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					CmbPackingType.Value = idits;
				}
				else
				{
					((Control)(object)CmbPackingType).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Cropfill()
	{
		try
		{
			int idits = Conversion.ToInt(cmbcropYear.Value);
			DataTable lst = CommonServices.CropYearGetAllService();
			if (lst.Rows.Count > 0)
			{
				DDL.BindDDL(lst, cmbcropYear, "Id", "CropYear", "Crop Year", ZeroIndex: true);
				((UltraGridBase)cmbcropYear).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (idits > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == idits))
				{
					cmbcropYear.Value = idits;
				}
				else
				{
					((Control)(object)cmbcropYear).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PackUOMFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbuom.Value);
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbitem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, cmbuom, "Id", "Equivalent", "UOM", true);
				((UltraGridBase)cmbuom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dtsearchby.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbuom.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbuom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			int OrderId = Conversion.ToInt(cmbitem.Value);
			if (lst.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(cmbitem.Value) > 0)
				{
					ItemId = Conversion.ToInt(cmbitem.Value);
				}
				if (rdSearchByName.Checked)
				{
					DDL.BindDDL(lst, cmbitem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(lst, cmbitem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				cmbitem.Value = ItemId;
				cmbitem.Focus();
			}
			if (OrderId > 0)
			{
				if (lst.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbitem.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbitem).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbitem_Leave(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		try
		{
			int OrderId = Conversion.ToInt(cmbuom.Value);
			UOMSchedule val = new UOMSchedule
			{
				ItemId = Conversion.ToInt(cmbitem.Value),
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtsearchby = new DataTable();
			dtsearchby = UOMSchedule.SearchByObject(val);
			if (dtsearchby.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtsearchby, cmbuom, "Id", "Equivalent", "UOM", true);
				((UltraGridBase)cmbuom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (OrderId > 0)
			{
				if (dtsearchby.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == OrderId))
				{
					cmbuom.Value = OrderId;
				}
				else
				{
					((Control)(object)cmbuom).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void multiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				((Control)(object)cmbcurrency).Visible = true;
				txtexchangerate.Visible = true;
				txtfcyamount.Visible = true;
				label21.Visible = true;
				label22.Visible = true;
				label23.Visible = true;
				txtRemarks.Size = new Size(210, 80);
			}
			else
			{
				((Control)(object)cmbcurrency).Visible = false;
				txtexchangerate.Visible = false;
				txtfcyamount.Visible = false;
				label21.Visible = false;
				label22.Visible = false;
				label23.Visible = false;
				txtRemarks.Size = new Size(454, 80);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			int CurrId = Conversion.ToInt(cmbcurrency.Value);
			decimal CurrRate = Conversion.ToDecimal((object)txtexchangerate.Text);
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int BaseCurrency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				cmbcurrency.Value = BaseCurrency;
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				decimal BaseCurrencyRate = Conversion.ToInt((object)BaseRatelist[0].ConfigKey.ToString());
				txtexchangerate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
			}
			if (HasMultiCurrencyFeature)
			{
				if (CurrId > 0)
				{
					cmbcurrency.Value = CurrId;
				}
				if (CurrRate > 0m)
				{
					txtexchangerate.Text = CurrRate.ToString();
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void txtexchangerate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateUpdateFcyAmountInDetail();
			CustomerTotalCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Reset()
	{
		try
		{
			At = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			RecId = 0;
			((Control)(object)cmbCustomername).Text = string.Empty;
			txtdocno.Text = string.Empty;
			((Control)(object)cmbpayterm).Text = string.Empty;
			((Control)(object)cmbdeliveryterm).Text = string.Empty;
			txtdocdate.Value = DateTime.Now;
			txtduedays.Text = string.Empty;
			txtmanualref.Text = string.Empty;
			txtCusOrderAmt.Text = string.Empty;
			txtexchangerate.Text = string.Empty;
			txtfcyamount.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			((Control)(object)cmbCommAgent).Text = string.Empty;
			((Control)(object)cmbcommtype).Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			txtbrokeryamount.Text = string.Empty;
			txtbrokeryrate.Text = string.Empty;
			((Control)(object)cmbsubbrokeryacc).Text = string.Empty;
			txtsubbrokeryamnt.Text = string.Empty;
			txtsubbrokeryrate.Text = string.Empty;
			dtDetail.Clear();
			grdDetail.ClearStructure();
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)cmbuom).Text = string.Empty;
			txtItemQty.Text = string.Empty;
			((Control)(object)cmbsupplier).Text = string.Empty;
			txtSupNetWeight.Text = string.Empty;
			txtsuprate.Text = string.Empty;
			txtsupamount.Text = string.Empty;
			txtsupremarks.Text = string.Empty;
			txtcusamount.Text = string.Empty;
			txtcusrate.Text = string.Empty;
			txtcusrate.Text = string.Empty;
			txtcusamount.Text = string.Empty;
			txtcustremarks.Text = string.Empty;
			DocumentNoFill();
			cmbCustomername.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
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
			((Control)(object)cmbitem).Text = string.Empty;
			((Control)(object)cmbuom).Text = string.Empty;
			txtItemQty.Text = string.Empty;
			((Control)(object)cmbsupplier).Text = string.Empty;
			txtSupNetWeight.Text = string.Empty;
			txtsuprate.Text = string.Empty;
			txtsupamount.Text = string.Empty;
			txtsupremarks.Text = string.Empty;
			txtcusamount.Text = string.Empty;
			txtcusrate.Text = string.Empty;
			txtcusrate.Text = string.Empty;
			txtcusamount.Text = string.Empty;
			txtcustremarks.Text = string.Empty;
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			cmbitem.Focus();
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
			ResetDetail();
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
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			CurrencyFill();
			CommissionTypeFill();
			Itemfill();
			PackingTypefill();
			Cropfill();
			PackUOMFill();
			ConfigurationDefault();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbCustomername).ActiveRow == null || Conversion.ToInt(cmbCustomername.Value) == 0)
		{
			MessageBox.Show("Customer Name Field is Required");
			cmbCustomername.Focus();
			return false;
		}
		if (((UltraGridBase)cmbpayterm).ActiveRow == null || Conversion.ToInt(cmbpayterm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			cmbpayterm.Focus();
			return false;
		}
		if (((Control)(object)cmbpayterm).Text == "Credit" && Conversion.ToInt((object)txtduedays.Text) == 0)
		{
			MessageBox.Show("Due Days Field is Required when Payment Term is Credit");
			txtduedays.Focus();
			return false;
		}
		if (((UltraGridBase)cmbdeliveryterm).ActiveRow == null || Conversion.ToInt(cmbdeliveryterm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			cmbdeliveryterm.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbcurrency).ActiveRow == null || Conversion.ToInt(cmbcurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbcurrency.Focus();
				return false;
			}
			if (txtexchangerate.Text.Trim() == string.Empty || txtexchangerate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtexchangerate.Focus();
				return false;
			}
			if (txtfcyamount.Text.Trim() == string.Empty || txtfcyamount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtfcyamount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbcurrency).ActiveRow == null || Conversion.ToInt(cmbcurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbcurrency.Focus();
				return false;
			}
			if (txtexchangerate.Text.Trim() == string.Empty || txtexchangerate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtexchangerate.Focus();
				return false;
			}
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbitem).ActiveRow == null || Conversion.ToInt(cmbitem.Value) == 0)
		{
			MessageBox.Show("Item Field is Required");
			cmbitem.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcropYear).ActiveRow == null || Conversion.ToInt(cmbcropYear.Value) == 0)
		{
			MessageBox.Show("Crop Year Field is Required");
			cmbcropYear.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackingType).ActiveRow == null || Conversion.ToInt(CmbPackingType.Value) == 0)
		{
			MessageBox.Show("Packing Type Field is Required");
			CmbPackingType.Focus();
			return false;
		}
		if (((UltraGridBase)cmbuom).ActiveRow == null || Conversion.ToInt(cmbuom.Value) == 0)
		{
			MessageBox.Show("UOM Field is Required");
			cmbuom.Focus();
			return false;
		}
		if (txtItemQty.Text.Trim() == "" || Conversion.ToDouble((object)txtItemQty.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Qty Field is Required");
			txtItemQty.Focus();
			return false;
		}
		if (((UltraGridBase)cmbsupplier).ActiveRow == null || Conversion.ToInt(cmbsupplier.Value) == 0)
		{
			MessageBox.Show("Supplier Field is Required");
			cmbsupplier.Focus();
			return false;
		}
		if (txtSupNetWeight.Text.Trim() == "" || Conversion.ToDouble((object)txtSupNetWeight.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Supplier Net Weught Field is Required");
			txtSupNetWeight.Focus();
			return false;
		}
		if (txtsuprate.Text.Trim() == "" || Conversion.ToDouble((object)txtsuprate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Net Weught Field is Required");
			txtsuprate.Focus();
			return false;
		}
		if (txtsupamount.Text.Trim() == "" || Conversion.ToDouble((object)txtsupamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Net Weught Field is Required");
			txtsupamount.Focus();
			return false;
		}
		if (txtcustnetwt.Text.Trim() == "" || Conversion.ToDouble((object)txtcustnetwt.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Net Weught Field is Required");
			txtcustnetwt.Focus();
			return false;
		}
		if (txtcusrate.Text.Trim() == "" || Conversion.ToDouble((object)txtcusrate.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Net Weught Field is Required");
			txtcusrate.Focus();
			return false;
		}
		if (txtcusamount.Text.Trim() == "" || Conversion.ToDouble((object)txtcusamount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Customer Net Weught Field is Required");
			txtcusamount.Focus();
			return false;
		}
		return true;
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtDetail.Rows.Add(0, cmbitem.Value, ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemCode"].Text, ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemName"].Text, cmbcropYear.Value, ((Control)(object)cmbcropYear).Text, CmbPackingType.Value, ((Control)(object)CmbPackingType).Text, cmbuom.Value, ((Control)(object)cmbuom).Text, Conversion.ToInt((object)txtItemQty.Text.Trim()), cmbsupplier.Value, ((Control)(object)cmbsupplier).Text, Conversion.ToDecimal((object)txtSupNetWeight.Text.Trim()), Conversion.ToDecimal((object)txtsuprate.Text.Trim()), Conversion.ToDecimal((object)txtsupamount.Text.Trim()), Conversion.ToString((object)txtsupremarks.Text.Trim()), Conversion.ToDecimal((object)txtcustnetwt.Text.Trim()), Conversion.ToDecimal((object)txtcusrate.Text.Trim()), Conversion.ToDecimal((object)txtcusamount.Text.Trim()), Conversion.ToString((object)txtcustremarks.Text.Trim()));
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				ResetDetail();
				CalculateUpdateFcyAmountInDetail();
				TotalCommissionAmount();
				CustomerTotalCalculation();
				FillSummaryGrid();
			}
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
			FillSummaryGrid();
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["CropYearId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackingTypeId"].Visible = false;
			grdDetail.RootTable.Columns["UomId"].Visible = false;
			grdDetail.RootTable.Columns["SupplierId"].Visible = false;
			grdDetail.RootTable.Columns["CusRate"].AggregateFunction = (AggregateFunction)3;
			grdDetail.RootTable.Columns["CusRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["CusRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["CusAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CusAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["CustFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["CustFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["CustFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["CustFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.###";
			grdDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.###";
			grdDetail.RootTable.Columns["SupRate"].AggregateFunction = (AggregateFunction)3;
			grdDetail.RootTable.Columns["SupRate"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["SupRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetail.RootTable.Columns["SupAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SupAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SupFcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["SupFcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["SupFcyAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetail.RootTable.Columns["SupFcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Edit");
			grdDetail.RootTable.Columns["Edit"].Key = "Edit";
			grdDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDetail.RootTable.Columns["Edit"].Width = 50;
			grdDetail.RootTable.Columns["Edit"].Position = 0;
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "Delete";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "Delete";
			grdDetail.RootTable.Columns["Delete"].Width = 80;
			grdDetail.RootTable.Columns["Delete"].Position = 1;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDetail();
			grdDetail.ClearStructure();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationDetail())
			{
				dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(cmbitem.Value);
				dtDetail.Rows[updateDetailIndex]["ItemName"] = ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemName"].Value;
				dtDetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)cmbitem).SelectedRow.Cells["ItemCode"].Value;
				dtDetail.Rows[updateDetailIndex]["CropYearId"] = Conversion.ToInt(cmbcropYear.Value);
				dtDetail.Rows[updateDetailIndex]["CropYear"] = Conversion.ToString((object)((Control)(object)cmbcropYear).Text);
				dtDetail.Rows[updateDetailIndex]["PackingTypeId"] = Conversion.ToInt(CmbPackingType.Value);
				dtDetail.Rows[updateDetailIndex]["PackingType"] = Conversion.ToString((object)((Control)(object)CmbPackingType).Text);
				dtDetail.Rows[updateDetailIndex]["UomId"] = Conversion.ToInt(cmbuom.Value);
				dtDetail.Rows[updateDetailIndex]["Uom"] = Conversion.ToString((object)((Control)(object)cmbuom).Text);
				dtDetail.Rows[updateDetailIndex]["Qty"] = Conversion.ToInt((object)txtItemQty.Text);
				dtDetail.Rows[updateDetailIndex]["SupplierId"] = Conversion.ToInt(cmbsupplier.Value);
				dtDetail.Rows[updateDetailIndex]["SupplierName"] = Conversion.ToString((object)((Control)(object)cmbsupplier).Text);
				dtDetail.Rows[updateDetailIndex]["SupNetWeight"] = Conversion.ToDecimal((object)txtSupNetWeight.Text);
				dtDetail.Rows[updateDetailIndex]["SupRate"] = Conversion.ToDecimal((object)txtsuprate.Text);
				dtDetail.Rows[updateDetailIndex]["SupAmount"] = Conversion.ToDecimal((object)txtsupamount.Text);
				dtDetail.Rows[updateDetailIndex]["SupRemarks"] = Conversion.ToString((object)txtsupremarks.Text);
				dtDetail.Rows[updateDetailIndex]["CusNetWeight"] = Conversion.ToDecimal((object)txtcustnetwt.Text);
				dtDetail.Rows[updateDetailIndex]["CusRate"] = Conversion.ToDecimal((object)txtcusrate.Text);
				dtDetail.Rows[updateDetailIndex]["CusAmount"] = Conversion.ToDecimal((object)txtcusamount.Text);
				dtDetail.Rows[updateDetailIndex]["CusRemarks"] = Conversion.ToString((object)txtcustremarks.Text);
				grdDetail.DataSource = dtDetail;
				btnAddDetail.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelDetail.Visible = false;
				ResetDetail();
				CalculateUpdateFcyAmountInDetail();
				TotalCommissionAmount();
				CustomerTotalCalculation();
				FillSummaryGrid();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Edit")
			{
				grdDetail_DoubleClick(null, null);
			}
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow r = grdDetail.CurrentRow;
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					CommTradeOrderDetail vd = new CommTradeOrderDetail();
					vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					vd.CropYearId = Conversion.ToString(r.Cells["CropYearId"].Value);
					vd.PackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
					vd.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
					vd.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
					vd.SupplierId = Conversion.ToInt(r.Cells["SupplierId"].Value);
					vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
					vd.SupRemarks = Conversion.ToString(r.Cells["SupRemarks"].Value);
					vd.CustomerId = Conversion.ToInt(r.Cells["CustomerId"].Value);
					vd.CusAmount = Conversion.ToInt(r.Cells["CusAmount"].Value);
					vd.CusDueDays = Conversion.ToInt(r.Cells["CusDueDays"].Value);
					vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
					vd.CusRate = Conversion.ToDecimal(r.Cells["CusRate"].Value);
					vd.CustFcyAmount = Conversion.ToDecimal(r.Cells["CustFcyAmount"].Value);
					vd.CusRemarks = Conversion.ToString(r.Cells["CusRemarks"].Value);
					vd.SupAmount = Conversion.ToDecimal(r.Cells["SupAmount"].Value);
					vd.SupDueDays = Conversion.ToDecimal(r.Cells["SupDueDays"].Value);
					vd.SupFcyAmount = Conversion.ToDecimal(r.Cells["SupFcyAmount"].Value);
					vd.SupRate = Conversion.ToDecimal(r.Cells["SupRate"].Value);
					vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
					vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
					vd.CusDeliveryTerm = Conversion.ToString(cmbdeliveryterm.Value);
					vd.ActionTypeId = 3;
					lstRemoveRecord.Add(vd);
					dtDetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdDetail.UpdateData();
					CalculateUpdateFcyAmountInDetail();
					TotalCommissionAmount();
					CustomerTotalCalculation();
				}
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbitem.Value = item.Cells["ItemId"].Value.ToString();
				cmbcropYear.Value = Conversion.ToInt((object)item.Cells["CropYearId"].Value.ToString());
				CmbPackingType.Value = Conversion.ToInt((object)item.Cells["PackingTypeId"].Value.ToString());
				cmbuom.Value = Conversion.ToInt((object)item.Cells["UomId"].Value.ToString());
				txtItemQty.Text = item.Cells["Qty"].Value.ToString();
				cmbsupplier.Value = Conversion.ToInt((object)item.Cells["SupplierId"].Value.ToString());
				txtSupNetWeight.Text = item.Cells["SupNetWeight"].Value.ToString();
				txtsuprate.Text = item.Cells["SupRate"].Value.ToString();
				txtsupamount.Text = item.Cells["SupAmount"].Value.ToString();
				txtsupremarks.Text = item.Cells["SupRemarks"].Value.ToString();
				txtcustnetwt.Text = item.Cells["CusNetWeight"].Value.ToString();
				txtcusrate.Text = item.Cells["CusRate"].Value.ToString();
				txtcusamount.Text = item.Cells["CusAmount"].Value.ToString();
				txtcustremarks.Text = item.Cells["CusRemarks"].Value.ToString();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "rdSearchByName")
			{
				cmbitem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillSummaryGrid()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			dtsummaryBySupplier.Clear();
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			string supplierIds = "";
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow row in rows)
			{
				supplierIds = supplierIds + "," + Conversion.ToString(row.Cells["SupplierId"].Value);
			}
			if (string.IsNullOrEmpty(supplierIds))
			{
				return;
			}
			foreach (int Id in from id in supplierIds.Split(',').Distinct()
				select Conversion.ToInt((object)id))
			{
				if (Id == 0)
				{
					continue;
				}
				decimal ItemAmount = default(decimal);
				decimal ItemFcyAmount = default(decimal);
				rows = grdDetail.GetRows();
				foreach (GridEXRow rdetail in rows)
				{
					if (Conversion.ToInt(rdetail.Cells["SupplierId"].Value) == Id)
					{
						ItemAmount += Conversion.ToDecimal(rdetail.Cells["SupAmount"].Value);
						ItemFcyAmount += Conversion.ToDecimal(rdetail.Cells["SupFcyAmount"].Value);
					}
				}
				dtsummaryBySupplier.Rows.Add(Id, ItemAmount);
			}
			if (dtsummaryBySupplier.Rows.Count > 0)
			{
				grdsummary.DataSource = dtsummaryBySupplier;
				grdsummary.RetrieveStructure();
				grdsummarysetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdsummarysetting()
	{
		try
		{
			grdsummary.RootTable.Columns["SupplierId"].ColumnType = (ColumnType)0;
			grdsummary.RootTable.Columns["SupplierId"].HasValueList = true;
			grdsummary.RootTable.Columns["SupplierId"].LimitToList = true;
			grdsummary.RootTable.Columns["SupplierId"].EditType = (EditType)4;
			grdsummary.RootTable.Columns["SupplierId"].Caption = "Supplier Name";
			grdsummary.RootTable.Columns["SupplierId"].ValueList.PopulateValueList((IEnumerable)dtSupplier.DefaultView, "Id", "CompanyName");
			grdsummary.RootTable.Columns["SupplierId"].Width = 250;
			grdsummary.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdsummary.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdsummary.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdsummary.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdsummary);
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
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0539: Expected O, but got Unknown
		//IL_09de: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e5: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grdDetail.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			CommTradeOrder co = new CommTradeOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				co.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			CalculateUpdateFcyAmountInDetail();
			TotalCommissionAmount();
			CustomerTotalCalculation();
			dtCommission.Clear();
			if (Conversion.ToInt(cmbCommAgent.Value) != 0 && Conversion.ToDecimal((object)txtcommrate.Text) != 0m)
			{
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbCommAgent.Value), 5, 1, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtcommrate.Text), Conversion.ToDecimal((object)txtcommamount.Text), Conversion.ToInt64(cmbCustomername.Value));
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbCommAgent.Value), 6, 1, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtbrokeryrate.Text), Conversion.ToDecimal((object)txtbrokeryamount.Text), Conversion.ToInt64(cmbCustomername.Value));
				dtCommission.Rows.Add(0, Conversion.ToInt(cmbsubbrokeryacc.Value), 6, 2, Conversion.ToInt(cmbcommtype.Value), Conversion.ToDecimal((object)txtsubbrokeryrate.Text), Conversion.ToDecimal((object)txtsubbrokeryamnt.Text), Conversion.ToInt64(cmbCommAgent.Value));
			}
			co.OrganizationId = UserAccount.OrganizationId;
			co.CompanyId = UserAccount.CompanyId;
			co.BranchesId = UserAccount.BranchesId;
			co.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			co.DocumentTypeId = 1705;
			co.ModifyUserId = UserAccount.ID;
			co.EntryUserId = UserAccount.ID;
			co.ApprovedUserId = UserAccount.ID;
			co.EntryDate = DateTime.Now;
			co.ModifyDate = DateTime.Now;
			co.ApprovedDate = DateTime.Now;
			co.DocDate = Conversion.ToDateTime((object)txtdocdate.Value);
			co.DocNo = Conversion.ToInt((object)txtdocno.Text);
			co.IsApproved = false;
			co.ManualRefNo = Conversion.ToString((object)txtmanualref.Text);
			co.RemarksHeader = Conversion.ToString((object)txtRemarks.Text);
			co.ScreenName = Conversion.ToString((object)base.Name);
			co.CommissionAgentId = Conversion.ToInt(cmbCommAgent.Value);
			co.CalculationParameterId = Conversion.ToInt(cmbcommtype.Value);
			co.CommRate = Conversion.ToDecimal((object)txtcommrate.Text);
			co.CommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			co.BrokeryRate = Conversion.ToDecimal((object)txtbrokeryrate.Text);
			co.BrokeryAmount = Conversion.ToDecimal((object)txtbrokeryamount.Text);
			co.SubBrokerId = Conversion.ToInt(cmbsubbrokeryacc.Value);
			co.SubBrokeryRate = Conversion.ToDecimal((object)txtsubbrokeryrate.Text);
			co.SubBrokeryAmount = Conversion.ToDecimal((object)txtsubbrokeryamnt.Text);
			co.OrderAmount = Conversion.ToDecimal((object)txtCusOrderAmt.Text);
			co.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
			co.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
			co.FcyAmount = Conversion.ToDecimal((object)txtfcyamount.Text);
			co.CommTradeOrderDetailList = new List<CommTradeOrderDetail>();
			co.CommTradeOrderCommBrokeryList = new List<CommTradeOrderCommBrokery>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				CommTradeOrderDetail vd = new CommTradeOrderDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				vd.dDocDate = DateTime.Now;
				vd.ItemId = Conversion.ToInt((object)r.Cells["ItemId"].Text);
				vd.CropYearId = Conversion.ToString((object)r.Cells["CropYearId"].Text);
				vd.PackingTypeId = Conversion.ToInt((object)r.Cells["PackingTypeId"].Text);
				vd.UomId = Conversion.ToInt((object)r.Cells["UomId"].Text);
				vd.Qty = Conversion.ToInt(r.Cells["Qty"].Value);
				vd.SupplierId = Conversion.ToInt((object)r.Cells["SupplierId"].Text);
				vd.SupNetWeight = Conversion.ToDecimal(r.Cells["SupNetWeight"].Value);
				vd.CustomerId = Conversion.ToInt(cmbCustomername.Value);
				vd.CusNetWeight = Conversion.ToDecimal(r.Cells["CusNetWeight"].Value);
				vd.SupRate = Conversion.ToDecimal(r.Cells["SupRate"].Value);
				vd.SupDueDays = Conversion.ToDecimal((object)txtduedays.Text);
				vd.SupAmount = Conversion.ToDecimal(r.Cells["SupAmount"].Value);
				vd.SupFcyAmount = Conversion.ToDecimal(r.Cells["SupFcyAmount"].Value);
				vd.SupRemarks = Conversion.ToString((object)r.Cells["SupRemarks"].Text);
				vd.CustomerId = Conversion.ToInt(cmbCustomername.Value);
				vd.CusRate = Conversion.ToDecimal(r.Cells["CusRate"].Value);
				vd.CusDueDays = Conversion.ToInt((object)txtduedays.Text);
				vd.CusAmount = Conversion.ToDecimal(r.Cells["CusAmount"].Value);
				vd.CustFcyAmount = Conversion.ToDecimal(r.Cells["CustFcyAmount"].Value);
				vd.CusRemarks = Conversion.ToString(r.Cells["CusRemarks"].Value);
				DataTable dt = CommonServices.GetUomScheduleByItemId(Conversion.ToInt(r.Cells["ItemId"].Value));
				if (dt.Rows.Count > 0)
				{
					DataRow[] dr = dt.Select("Equivalent='" + 40 + "'");
					if (dr.Length == 0)
					{
						MessageBox.Show("RateUom of the Item " + Conversion.ToString((object)r.Cells["ItemId"].Text) + " in row No " + (r.RowIndex + 1) + " is not define please check ");
						return;
					}
					vd.CusRateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
					vd.SupRateUomId = Conversion.ToInt(dr[0].ItemArray[0]);
				}
				vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
				vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
				vd.CusDeliveryTerm = Conversion.ToString(cmbdeliveryterm.Value);
				vd.ExchangeRate = Conversion.ToDecimal((object)txtexchangerate.Text);
				co.CommTradeOrderDetailList.Add(vd);
			}
			if (RecId > 0 && lstRemoveRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveRecord.Count; j++)
				{
					co.CommTradeOrderDetailList.Add(lstRemoveRecord[j]);
				}
			}
			for (int i = 0; i < dtCommission.Rows.Count; i++)
			{
				if (Conversion.ToInt(dtCommission.Rows[i]["CommissionAgentId"]) != 0)
				{
					CommTradeOrderCommBrokery PJ = new CommTradeOrderCommBrokery();
					PJ.Id = Conversion.ToInt(dtCommission.Rows[i]["Id"]);
					PJ.CommissionAgentId = Conversion.ToInt(dtCommission.Rows[i]["CommissionAgentId"]);
					if (PJ.CommissionAgentId == 0)
					{
						MessageBox.Show("CommissionAgent  Is Required");
						return;
					}
					PJ.ChargesIncentiveId = Conversion.ToInt(dtCommission.Rows[i]["ChargesIncentiveId"]);
					PJ.RevenueExpenseId = Conversion.ToInt(dtCommission.Rows[i]["RevenueExpenseId"]);
					PJ.CalcParameterId = Conversion.ToInt(dtCommission.Rows[i]["CalcParameterId"]);
					PJ.Rate = Conversion.ToDecimal(dtCommission.Rows[i]["Rate"]);
					PJ.Amount = Conversion.ToDecimal(dtCommission.Rows[i]["Amount"]);
					PJ.SupplierCustomerId = Conversion.ToInt(dtCommission.Rows[i]["SupplierCustomerId"]);
					co.CommTradeOrderCommBrokeryList.Add(PJ);
				}
			}
			co.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			co.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<CommTradeOrder>(co, base.Name, At, "AttachmentsList", "DeleteAttachmentsList");
			int success = CommTradeOrder.Save(co);
			co.Id = success;
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + co.DocNo);
				if (At.RemovedAttachmentListInUpdateCase.Count > 0 && At.lst.Count == 0)
				{
					global::Architecture.BLL.DMSAttachments.RemoveByIdAndNames(RecId, base.Name, co.DocumentTypeId, 0);
				}
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + co.DocNo);
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

	private void ReadById(int ID)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		try
		{
			RecId = Conversion.ToInt((object)ID);
			lstRemoveRecord.Clear();
			CommTradeOrder po = new CommTradeOrder();
			po = CommTradeOrder.ReadById(RecId);
			tabControl1.SelectedIndex = 0;
			txtdocno.Text = Conversion.ToString((object)po.DocNo);
			txtdocdate.Text = Conversion.ToString((object)po.DocDate);
			txtexchangerate.Text = Conversion.ToString((object)po.ExchangeRate);
			txtfcyamount.Text = Conversion.ToString((object)po.FcyAmount);
			txtfcyamount.Text = Conversion.ToString((object)po.FcyAmount);
			cmbcurrency.Value = Conversion.ToInt((object)po.CurrencyId);
			txtmanualref.Text = Conversion.ToString((object)po.ManualRefNo);
			txtRemarks.Text = Conversion.ToString((object)po.RemarksHeader);
			if (po.CommTradeOrderCommBrokeryList.Count > 0)
			{
				for (int j = 0; j < po.CommTradeOrderCommBrokeryList.Count; j++)
				{
					if (po.CommTradeOrderCommBrokeryList[j].RevenueExpenseId == 1 && po.CommTradeOrderCommBrokeryList[j].ChargesIncentiveId == 5)
					{
						cmbCommAgent.Value = Conversion.ToInt((object)po.CommTradeOrderCommBrokeryList[j].CommissionAgentId);
						cmbcommtype.Value = Conversion.ToInt((object)po.CommTradeOrderCommBrokeryList[j].CalcParameterId);
						txtcommrate.Text = Conversion.ToString((object)po.CommTradeOrderCommBrokeryList[j].Rate);
						txtcommamount.Text = Conversion.ToString((object)po.CommTradeOrderCommBrokeryList[j].Amount);
					}
					if (po.CommTradeOrderCommBrokeryList[j].RevenueExpenseId == 1 && po.CommTradeOrderCommBrokeryList[j].ChargesIncentiveId == 6)
					{
						txtbrokeryrate.Text = Conversion.ToString((object)po.CommTradeOrderCommBrokeryList[j].Rate);
						txtbrokeryamount.Text = Conversion.ToString((object)po.CommTradeOrderCommBrokeryList[j].Amount);
					}
					if (po.CommTradeOrderCommBrokeryList[j].RevenueExpenseId == 2 && po.CommTradeOrderCommBrokeryList[j].ChargesIncentiveId == 6)
					{
						cmbsubbrokeryacc.Value = Conversion.ToInt((object)po.CommTradeOrderCommBrokeryList[j].CommissionAgentId);
						txtsubbrokeryrate.Text = Conversion.ToString((object)po.CommTradeOrderCommBrokeryList[j].Rate);
						txtsubbrokeryamnt.Text = Conversion.ToString((object)po.CommTradeOrderCommBrokeryList[j].Amount);
					}
				}
			}
			if (po.CommTradeOrderDetailList.Count > 0)
			{
				cmbCustomername.Value = Conversion.ToInt((object)po.CommTradeOrderDetailList[0].CustomerId);
				cmbpayterm.Value = Conversion.ToInt((object)po.CommTradeOrderDetailList[0].CusPaymentTermsId);
				txtduedays.Text = Conversion.ToString((object)po.CommTradeOrderDetailList[0].CusDueDays);
				cmbdeliveryterm.Value = Conversion.ToString((object)po.CommTradeOrderDetailList[0].CusDeliveryTerm);
				dtDetail.Clear();
				for (int i = 0; i < po.CommTradeOrderDetailList.Count; i++)
				{
					dtDetail.Rows.Add(po.CommTradeOrderDetailList[i].Id, po.CommTradeOrderDetailList[i].ItemId, po.CommTradeOrderDetailList[i].ItemCode, po.CommTradeOrderDetailList[i].ItemName, po.CommTradeOrderDetailList[i].CropYearId, po.CommTradeOrderDetailList[i].CropYear, po.CommTradeOrderDetailList[i].PackingTypeId, po.CommTradeOrderDetailList[i].PackingType, po.CommTradeOrderDetailList[i].UomId, po.CommTradeOrderDetailList[i].PackUom, po.CommTradeOrderDetailList[i].Qty, po.CommTradeOrderDetailList[i].SupplierId, po.CommTradeOrderDetailList[i].SupplierName, po.CommTradeOrderDetailList[i].SupNetWeight, po.CommTradeOrderDetailList[i].SupRate, po.CommTradeOrderDetailList[i].SupAmount, po.CommTradeOrderDetailList[i].SupRemarks, po.CommTradeOrderDetailList[i].CusNetWeight, po.CommTradeOrderDetailList[i].CusRate, po.CommTradeOrderDetailList[i].CusAmount, po.CommTradeOrderDetailList[i].CusRemarks, po.CommTradeOrderDetailList[i].CustFcyAmount, po.CommTradeOrderDetailList[i].SupFcyAmount);
				}
				grdDetail.DataSource = dtDetail;
				grdDetail.RetrieveStructure();
				grdSettings();
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			FormHelper.LoadAttachmentsForObject(At, RecId, base.Name);
			CalculateUpdateFcyAmountInDetail();
			TotalCommissionAmount();
			CustomerTotalCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					CommTradeOrder.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					Reset();
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

	private void gridfill(int NoOfRecords = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 1705;
			obj.EntryUser = UserAccount.ID;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.NoOfRecords = NoOfRecords;
			obj.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			dtgrid = CommTradeOrder.FormHistory(obj);
			if (dtgrid.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("RecordNo", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(string));
				dt.Columns.Add("ReferenceNo", typeof(string));
				dt.Columns.Add("CommRate", typeof(decimal));
				dt.Columns.Add("CommAmount", typeof(decimal));
				dt.Columns.Add("BrokeryRate", typeof(decimal));
				dt.Columns.Add("BrokeryAmount", typeof(decimal));
				dt.Columns.Add("SubBrokeryRate", typeof(decimal));
				dt.Columns.Add("SubBrokeryAmount", typeof(decimal));
				dt.Columns.Add("TotalQty", typeof(int));
				dt.Columns.Add("SupTotalWeight", typeof(decimal));
				dt.Columns.Add("CusTotalWeight", typeof(decimal));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("IsApproved", typeof(bool));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Clear();
				for (int i = 0; i < dtgrid.Rows.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt(dtgrid.Rows[i]["RecordNo"]), Conversion.ToInt(dtgrid.Rows[i]["DocumentTypeId"]), Conversion.ToInt(dtgrid.Rows[i]["Id"]), Conversion.ToDateTime(dtgrid.Rows[i]["DocDate"]), Conversion.ToString(dtgrid.Rows[i]["DocNo"]), Conversion.ToString(dtgrid.Rows[i]["ManualRefNo"]), Conversion.ToDouble(dtgrid.Rows[i]["CommRate"]), Conversion.ToDouble(dtgrid.Rows[i]["CommAmount"]), Conversion.ToDouble(dtgrid.Rows[i]["BrokeryRate"]), Conversion.ToDouble(dtgrid.Rows[i]["BrokeryAmount"]), Conversion.ToDouble(dtgrid.Rows[i]["SubBrokeryRate"]), Conversion.ToDouble(dtgrid.Rows[i]["SubBrokeryAmount"]), Conversion.ToInt(dtgrid.Rows[i]["TotalQty"]), Conversion.ToDouble(dtgrid.Rows[i]["SupTotalWeight"]), Conversion.ToDouble(dtgrid.Rows[i]["CusTotalWeight"]), Conversion.ToDateTime(dtgrid.Rows[i]["EntryDate"]), Conversion.ToString(dtgrid.Rows[i]["EntryUserName"]), Conversion.ToDateTime(dtgrid.Rows[i]["ModifyDate"]), Conversion.ToString(dtgrid.Rows[i]["ModifyUserName"]), Conversion.ToDateTime(dtgrid.Rows[i]["ApprovedDate"]), Conversion.ToBool(dtgrid.Rows[i]["IsApproved"]), Conversion.ToString(dtgrid.Rows[i]["RemarksHeader"]), Conversion.ToInt(dtgrid.Rows[i]["NoOfAttachments"]));
				}
				grdHistory.DataSource = dt;
				grdHistory.RetrieveStructure();
				grdhistorysetting();
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
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["RecordNo"].Visible = false;
			grdHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdHistory.RootTable.Columns["CommRate"].AggregateFunction = (AggregateFunction)3;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["CommRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryRate"].AggregateFunction = (AggregateFunction)3;
			grdHistory.RootTable.Columns["BrokeryRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["BrokeryRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["BrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["BrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["BrokeryAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["BrokeryAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SubBrokeryRate"].AggregateFunction = (AggregateFunction)3;
			grdHistory.RootTable.Columns["SubBrokeryRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SubBrokeryRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["SubBrokeryRate"].TotalFormatString = clsGlobalVariables.DecimalRateFormate;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SubBrokeryAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["SupTotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["SupTotalWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["SupTotalWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["SupTotalWeight"].TotalFormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CusTotalWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CusTotalWeight"].FormatString = "#,##0.###";
			grdHistory.RootTable.Columns["CusTotalWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns.Add("Edit");
				grdHistory.RootTable.Columns["Edit"].Key = "Edit";
				grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
				grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdHistory.RootTable.Columns["Edit"].Width = 50;
			}
			if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns.Add("Print");
				grdHistory.RootTable.Columns["Print"].Key = "Print";
				grdHistory.RootTable.Columns["Print"].Caption = "Print";
				grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				grdHistory.RootTable.Columns["Print"].ButtonText = "Print";
				grdHistory.RootTable.Columns["Print"].Width = 50;
			}
			if (formright.DoHaveUpdateRights && formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.RootTable.Columns["Edit"].Position = 1;
				grdHistory.FrozenColumns = 2;
			}
			else if (formright.DoHaveUpdateRights)
			{
				grdHistory.RootTable.Columns["Edit"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			else if (formright.DoHavePrintRights)
			{
				grdHistory.RootTable.Columns["Print"].Position = 0;
				grdHistory.FrozenColumns = 1;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				gridfill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadAllHistory_Click(object sender, EventArgs e)
	{
		try
		{
			gridfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
				txtdocno.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit" && formright.DoHaveUpdateRights)
			{
				int Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(Id);
				txtdocno.Focus();
			}
			if (e.Column.Key == "Print" && formright.DoHavePrintRights)
			{
				GenerateSlip(Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString()));
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
			if (item != null && (int)item.RowType == 0 && grdHistory.CurrentColumn != null)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt((object)item.Cells["Id"].Value.ToString()), 1705);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				grddetailbind(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailbind(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new CommTradeOrder();
			DataTable dtdeta = CommTradeOrder.FormHistoryDetail(Id);
			if (dtdeta.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("CropYear", typeof(string));
				dt.Columns.Add("PackingType", typeof(string));
				dt.Columns.Add("UOM", typeof(string));
				dt.Columns.Add("Qty", typeof(int));
				dt.Columns.Add("SupplierName", typeof(string));
				dt.Columns.Add("SupNetWeight", typeof(decimal));
				dt.Columns.Add("SupRemarks", typeof(string));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("CusNetWeight", typeof(decimal));
				dt.Columns.Add("CusRemarks", typeof(string));
				dt.Clear();
				for (int i = 0; i < dtdeta.Rows.Count; i++)
				{
					dt.Rows.Add(dtdeta.Rows[i]["ItemCode"], dtdeta.Rows[i]["ItemName"], dtdeta.Rows[i]["CropYear"], dtdeta.Rows[i]["PackingType"], dtdeta.Rows[i]["PackUom"], dtdeta.Rows[i]["Qty"], dtdeta.Rows[i]["SupplierName"], dtdeta.Rows[i]["SupNetWeight"], dtdeta.Rows[i]["SupRemarks"], dtdeta.Rows[i]["CustomerName"], dtdeta.Rows[i]["CusNetWeight"], dtdeta.Rows[i]["CusRemarks"]);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				grddetailsetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailsetting()
	{
		try
		{
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["SupNetWeight"].TotalFormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TextAlignment = (TextAlignment)3;
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].FormatString = "#,##0.###";
			grdHistoryDetail.RootTable.Columns["CusNetWeight"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcusrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsupgrosswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupPckMtrlWeight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtSupAdLsWt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsuprate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustgrosswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustpckmtrlwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustadlswt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustnetwt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcustbillamnt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtexchangerate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtfcyamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcommrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtcommamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtbrokeryrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtbrokeryamount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsubbrokeryrate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtsubbrokeryamnt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void CalculateUpdateFcyAmountInDetail()
	{
		try
		{
			if (grdDetail.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtexchangerate.Text) > 0m)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal custFcyAmount = Conversion.ToDecimal(obj.Cells["CusAmount"].Value) / Conversion.ToDecimal((object)txtexchangerate.Text);
					obj.Cells["CustFcyAmount"].Value = Math.Round(custFcyAmount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					decimal supFcyAmount = Conversion.ToDecimal(obj.Cells["SupAmount"].Value) / Conversion.ToDecimal((object)txtexchangerate.Text);
					obj.Cells["SupFcyAmount"].Value = Math.Round(supFcyAmount, clsGlobalVariables.DefaultNoofDecimalPointsForFcyAmount);
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["CustFcyAmount"].Value = 0;
					obj2.Cells["SupFcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DetailWeightCalculation()
	{
		try
		{
			decimal PackUOM = default(decimal);
			decimal Qty = default(decimal);
			decimal NetWeightSup = default(decimal);
			decimal NetWeightCus = default(decimal);
			if (!string.IsNullOrEmpty(txtItemQty.Text))
			{
				Qty = Conversion.ToDecimal((object)txtItemQty.Text.ToString());
			}
			if (((UltraGridBase)cmbuom).ActiveRow != null && Conversion.ToInt(cmbuom.Value) != 0)
			{
				PackUOM = Conversion.ToDecimal((object)((Control)(object)cmbuom).Text.ToString());
			}
			NetWeightSup = Qty * PackUOM;
			NetWeightCus = Qty * PackUOM;
			txtcustnetwt.Text = Conversion.ToDecimal((object)NetWeightCus).ToString("#,##0.###");
			txtSupNetWeight.Text = Conversion.ToDecimal((object)NetWeightSup).ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalAmountCust()
	{
		try
		{
			double RateUOM = 40.0;
			double Rate = 0.0;
			double TotalAmount = 0.0;
			if (txtcustnetwt.Text != string.Empty && txtcusrate.Text != string.Empty)
			{
				double num = Conversion.ToDouble((object)txtcustnetwt.Text);
				Rate = Conversion.ToDouble((object)txtcusrate.Text);
				TotalAmount = num / RateUOM * Rate;
				txtcusamount.Text = Math.Round(TotalAmount).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtcusamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalAmountSup()
	{
		try
		{
			decimal RateUOM = 40m;
			decimal Rate = default(decimal);
			decimal TotalAmount = default(decimal);
			if (txtSupNetWeight.Text != string.Empty && txtsuprate.Text != string.Empty)
			{
				decimal num = Conversion.ToDecimal((object)txtSupNetWeight.Text.ToString());
				Rate = Conversion.ToDecimal((object)txtsuprate.Text.ToString());
				TotalAmount = num / RateUOM * Rate;
				txtsupamount.Text = Math.Round(TotalAmount).ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtsupamount.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbuom_Leave(object sender, EventArgs e)
	{
		try
		{
			DetailWeightCalculation();
			TotalAmountCust();
			TotalAmountSup();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtItemQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailWeightCalculation();
			TotalAmountCust();
			TotalAmountSup();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSupNetWeight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailWeightCalculation();
			TotalAmountSup();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtsuprate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmountSup();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcustnetwt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DetailWeightCalculation();
			TotalAmountCust();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcusrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalAmountCust();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CustomerTotalCalculation()
	{
		try
		{
			if (grdDetail.GetRows().Count() > 0)
			{
				decimal TotalCommAmount = default(decimal);
				decimal TotalBokeryAmount = default(decimal);
				decimal TotalAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2));
				decimal TotalFcyAmount = Conversion.ToDecimal(grdDetail.GetTotal(grdDetail.RootTable.Columns["CustFcyAmount"], (AggregateFunction)2));
				if (((UltraGridBase)cmbCommAgent).ActiveRow != null && Conversion.ToDecimal((object)txtcommamount.Text) > 0m && Conversion.ToInt(cmbCommAgent.Value) != Conversion.ToInt(cmbCustomername.Value))
				{
					TotalCommAmount = Conversion.ToDecimal((object)txtcommamount.Text);
					TotalBokeryAmount = Conversion.ToDecimal((object)txtbrokeryamount.Text);
					TotalAmount -= TotalCommAmount + TotalBokeryAmount;
					if (Conversion.ToDecimal((object)txtexchangerate.Text) > 0m)
					{
						TotalFcyAmount -= (TotalCommAmount + TotalBokeryAmount) / Conversion.ToDecimal((object)txtexchangerate.Text);
					}
					else
					{
						TotalFcyAmount -= TotalCommAmount + TotalBokeryAmount;
					}
				}
				if (TotalAmount > 0m)
				{
					txtCusOrderAmt.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
				}
				else
				{
					txtCusOrderAmt.Text = "0";
				}
				if (TotalFcyAmount > 0m)
				{
					txtfcyamount.Text = TotalFcyAmount.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
				else
				{
					txtfcyamount.Text = "0";
				}
			}
			else
			{
				txtCusOrderAmt.Text = "0";
				txtfcyamount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TotalCommissionAmount()
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Expected O, but got Unknown
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0371: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Expected O, but got Unknown
		//IL_06c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0711: Unknown result type (might be due to invalid IL or missing references)
		//IL_0718: Unknown result type (might be due to invalid IL or missing references)
		//IL_0724: Expected O, but got Unknown
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_049c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Expected O, but got Unknown
		//IL_07e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0809: Unknown result type (might be due to invalid IL or missing references)
		//IL_081f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0835: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0848: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbCustomername.Value) != 0 && Conversion.ToInt(cmbcommtype.Value) != 0)
			{
				if (Conversion.ToString((object)((Control)(object)cmbcommtype).Text) == "Flat")
				{
					if (CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1
					}).Rows.Count > 0)
					{
						double CommRate2 = Conversion.ToDouble((object)((Control)(object)cmbcommtype).Text.Trim());
						txtcommrate.Text = CommRate2.ToString();
						txtcommamount.Text = CommRate2.ToString();
					}
					DataTable dtBrokery3 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1
					});
					if (dtBrokery3.Rows.Count > 0)
					{
						decimal BrokeryRate3 = Conversion.ToDecimal(dtBrokery3.Rows[0]["Rate"]);
						txtbrokeryrate.Text = BrokeryRate3.ToString();
						txtbrokeryamount.Text = BrokeryRate3.ToString();
					}
					DataTable dtSubBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2
					});
					if (dtSubBrokery.Rows.Count > 0)
					{
						decimal SubBrokeryRate3 = Conversion.ToDecimal(dtSubBrokery.Rows[0]["Rate"]);
						txtsubbrokeryrate.Text = SubBrokeryRate3.ToString();
						txtsubbrokeryamnt.Text = SubBrokeryRate3.ToString();
					}
				}
				if (((Control)(object)cmbcommtype).Text == "Percent")
				{
					DataTable dtcomm2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1
					});
					if (dtcomm2.Rows.Count > 0)
					{
						object total3 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2);
						decimal commRate = Conversion.ToDecimal(dtcomm2.Rows[0]["Rate"]);
						txtcommrate.Text = commRate.ToString();
						if (total3 != null)
						{
							decimal CommAmt4 = Conversion.ToDecimal((object)total3.ToString()) * commRate / 100m;
							txtcommamount.Text = Math.Round(CommAmt4).ToString();
						}
					}
					DataTable dtBrokery2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1
					});
					if (dtBrokery2.Rows.Count > 0)
					{
						object total2 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2);
						decimal BrokeryRate2 = Conversion.ToDecimal(dtBrokery2.Rows[0]["Rate"]);
						txtbrokeryrate.Text = BrokeryRate2.ToString();
						if (total2 != null)
						{
							decimal CommAmt3 = Conversion.ToDecimal((object)total2.ToString()) * BrokeryRate2 / 100m;
							txtbrokeryamount.Text = Math.Round(CommAmt3).ToString();
						}
					}
					DataTable SubdtBrokery2 = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2
					});
					if (SubdtBrokery2.Rows.Count > 0)
					{
						object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusAmount"], (AggregateFunction)2);
						decimal SubBrokeryRate2 = Conversion.ToDecimal(SubdtBrokery2.Rows[0]["Rate"]);
						txtsubbrokeryrate.Text = SubBrokeryRate2.ToString();
						if (total != null)
						{
							decimal CommAmt2 = Conversion.ToDecimal((object)total.ToString()) * SubBrokeryRate2 / 100m;
							txtsubbrokeryamnt.Text = Math.Round(CommAmt2).ToString();
						}
					}
				}
				if (((Control)(object)cmbcommtype).Text == "Weight")
				{
					DataTable dtcomm = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 5,
						RevenueExpenseId = 1
					});
					if (dtcomm.Rows.Count > 0)
					{
						object weight3 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusNetWeight"], (AggregateFunction)2);
						decimal CommRate = Conversion.ToDecimal(dtcomm.Rows[0]["Rate"]);
						decimal UOM3 = Conversion.ToDecimal(dtcomm.Rows[0]["RateUom"]);
						txtcommrate.Text = CommRate.ToString();
						if (weight3 != null)
						{
							decimal CommAmt = Conversion.ToDecimal((object)weight3.ToString()) / UOM3 * CommRate;
							txtcommamount.Text = Math.Round(CommAmt).ToString();
						}
					}
					DataTable dtBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 1
					});
					if (dtBrokery.Rows.Count > 0)
					{
						object weight2 = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusNetWeight"], (AggregateFunction)2);
						decimal BrokeryRate = Conversion.ToDecimal(dtBrokery.Rows[0]["Rate"]);
						decimal UOM2 = Conversion.ToDecimal(dtBrokery.Rows[0]["RateUom"]);
						txtbrokeryrate.Text = BrokeryRate.ToString();
						if (weight2 != null)
						{
							decimal BrokeryAmt2 = Conversion.ToDecimal((object)weight2.ToString()) / UOM2 * BrokeryRate;
							txtbrokeryamount.Text = Math.Round(BrokeryAmt2).ToString();
						}
					}
					DataTable SubdtBrokery = CommTradeRevenueExpenseSchedule.GetCommScheduleRateByDateCalParamAndRevExpId(new ReportsParameters
					{
						DocDate = Conversion.ToDateTime((object)txtdocdate.Value),
						SupplierCustomerId = Conversion.ToInt(cmbCustomername.Value),
						CalculationParameterId = Conversion.ToInt(cmbcommtype.Value),
						ChargesIncentiveId = 6,
						RevenueExpenseId = 2
					});
					if (SubdtBrokery.Rows.Count > 0)
					{
						object weight = grdDetail.GetTotal(grdDetail.RootTable.Columns["CusNetWeight"], (AggregateFunction)2);
						decimal SubBrokeryRate = Conversion.ToDecimal(SubdtBrokery.Rows[0]["Rate"]);
						decimal UOM = Conversion.ToDecimal(SubdtBrokery.Rows[0]["RateUom"]);
						txtsubbrokeryrate.Text = SubBrokeryRate.ToString();
						if (weight != null)
						{
							decimal BrokeryAmt = Conversion.ToDecimal((object)weight.ToString()) / UOM * SubBrokeryRate;
							txtsubbrokeryamnt.Text = Math.Round(BrokeryAmt).ToString();
						}
					}
				}
			}
			_ = Conversion.ToDecimal((object)txtsubbrokeryamnt.Text) > 0m;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtcommrate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			CustomerTotalCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbcommtype_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			CustomerTotalCalculation();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdDetail.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (grdDetail.CurrentColumn.Key == "Edit")
			{
				grdDetail_DoubleClick(null, null);
			}
			if (grdDetail.CurrentColumn.Key == "Delete")
			{
				CommTradeOrderDetail vd = new CommTradeOrderDetail();
				vd.dDocDate = DateTime.Now;
				vd.ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				vd.CropYearId = Conversion.ToString(item.Cells["CropYearId"].Value);
				vd.PackingTypeId = Conversion.ToInt((object)item.Cells["PackingTypeId"].Text);
				vd.UomId = Conversion.ToInt((object)item.Cells["UomId"].Text);
				vd.Qty = Conversion.ToInt(item.Cells["Qty"].Value);
				vd.SupplierId = Conversion.ToInt(item.Cells["SupplierId"].Value);
				vd.SupNetWeight = Conversion.ToDecimal(item.Cells["SupNetWeight"].Value);
				vd.SupRemarks = Conversion.ToString(item.Cells["SupRemarks"].Value);
				vd.CustomerId = Conversion.ToInt(item.Cells["CustomerId"].Value);
				vd.CusAmount = Conversion.ToInt(item.Cells["CustomerAmount"].Value);
				vd.CusDueDays = Conversion.ToInt(item.Cells["CusDueDays"].Value);
				vd.CusNetWeight = Conversion.ToDecimal(item.Cells["CusNetWeight"].Value);
				vd.CusRate = Conversion.ToDecimal(item.Cells["CusRate"].Value);
				vd.CustFcyAmount = Conversion.ToDecimal(item.Cells["CustFcyAmount"].Value);
				vd.CusRemarks = Conversion.ToString(item.Cells["CusRemarks"].Value);
				vd.SupAmount = Conversion.ToDecimal(item.Cells["SupAmount"].Value);
				vd.SupDueDays = Conversion.ToDecimal(item.Cells["SupDueDays"].Value);
				vd.SupFcyAmount = Conversion.ToDecimal(item.Cells["SupFcyAmount"].Value);
				vd.SupRateUomId = Conversion.ToInt(cmbuom.Value);
				vd.SupRate = Conversion.ToDecimal(item.Cells["SupRate"].Value);
				vd.ActionTypeId = UserAccount.ID;
				vd.CommTradeOrderId = UserAccount.ID;
				vd.CurrencyId = Conversion.ToInt(cmbcurrency.Value);
				vd.CusPaymentTermsId = Conversion.ToInt(cmbpayterm.Value);
				vd.CusRateUomId = Conversion.ToInt(cmbuom.Value);
				vd.CusDeliveryTerm = Conversion.ToString(cmbdeliveryterm.Value);
				vd.ActionTypeId = 3;
				lstRemoveRecord.Add(vd);
				dtDetail.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				grdDetail.UpdateData();
				CalculateUpdateFcyAmountInDetail();
				TotalCommissionAmount();
				CustomerTotalCalculation();
			}
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
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdHistory.CurrentColumn != null)
			{
				int Id = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
				if (grdHistory.CurrentColumn.Key == "Print")
				{
					Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					GenerateSlip(Id);
				}
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					Id = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(Id);
					txtdocno.Focus();
				}
				if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 1705);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateSlip(int PrintId)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = CommTradeOrder.CommissionOrderSlipAndRegister(new ReportsParameters
			{
				OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
				CompanyId = clsGlobalVariables.UserAccount.CompanyId,
				Id = PrintId,
				DocumentTypeId = 1705
			});
			if (dt.Rows.Count == 0 || dt == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "1705-CommissionTradeOrder_Slip.rpt");
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

	private void CommissionOrder_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtdocno.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)grdHistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (btnsave.Visible && btnsave.Enabled)
				{
					if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					if (tabControl1.SelectedIndex == 0)
					{
						btnNew_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (btnUpdate.Visible && btnUpdate.Enabled)
				{
					if (e.Control && e.KeyCode == Keys.U && (UpdateMode = true))
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtdocno.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbitem.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F10)
				{
					At.Show();
				}
				else if ((!e.Control || e.KeyCode != Keys.P) && e.Control && e.KeyCode == Keys.Return)
				{
					grdDetail_DoubleClick(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1 && e.Control && e.KeyCode == Keys.P)
			{
				if (formright.DoHavePrintRights)
				{
					GenerateSlip(RecId);
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistoryDetail).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Return)
				{
					RecId = Conversion.ToInt((object)grdHistory.CurrentRow.Cells["Id"].Value.ToString());
					ReadById(RecId);
					txtdocno.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Bill No");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item in Detail Box");
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

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		try
		{
			At.Show();
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
		//IL_040b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Expected O, but got Unknown
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0419: Expected O, but got Unknown
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_04b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Expected O, but got Unknown
		//IL_0559: Unknown result type (might be due to invalid IL or missing references)
		//IL_0563: Expected O, but got Unknown
		//IL_05b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bb: Expected O, but got Unknown
		//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fd: Expected O, but got Unknown
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0676: Expected O, but got Unknown
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Expected O, but got Unknown
		//IL_07cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Expected O, but got Unknown
		//IL_07d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e1: Expected O, but got Unknown
		//IL_07ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f7: Expected O, but got Unknown
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_080d: Expected O, but got Unknown
		//IL_089d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a7: Expected O, but got Unknown
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d3: Expected O, but got Unknown
		//IL_47b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_47bc: Expected O, but got Unknown
		//IL_7ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_7cf3: Expected O, but got Unknown
		//IL_7d17: Unknown result type (might be due to invalid IL or missing references)
		//IL_7d21: Expected O, but got Unknown
		GridEXLayout grdsummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance12 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
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
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.CmTr.Transaction_Forms.CommissionOrder));
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdsummary = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtfcyamount = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label23 = new System.Windows.Forms.Label();
		this.txtexchangerate = new System.Windows.Forms.TextBox();
		this.txtmanualref = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.cmbpayterm = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.txtdocdate = new System.Windows.Forms.DateTimePicker();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.cmbcurrency = new UltraCombo();
		this.cmbCustomername = new UltraCombo();
		this.label20 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbdeliveryterm = new UltraCombo();
		this.txtCusOrderAmt = new System.Windows.Forms.TextBox();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.label27 = new System.Windows.Forms.Label();
		this.txtsubbrokeryamnt = new System.Windows.Forms.TextBox();
		this.label26 = new System.Windows.Forms.Label();
		this.txtsubbrokeryrate = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.cmbsubbrokeryacc = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtbrokeryamount = new System.Windows.Forms.TextBox();
		this.label18 = new System.Windows.Forms.Label();
		this.txtbrokeryrate = new System.Windows.Forms.TextBox();
		this.label15 = new System.Windows.Forms.Label();
		this.cmbcommtype = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbCommAgent = new UltraCombo();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label24 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.label10 = new System.Windows.Forms.Label();
		this.txtsupremarks = new System.Windows.Forms.TextBox();
		this.label32 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtcusamount = new System.Windows.Forms.TextBox();
		this.txtcusrate = new System.Windows.Forms.TextBox();
		this.label34 = new System.Windows.Forms.Label();
		this.label35 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.txtcustremarks = new System.Windows.Forms.TextBox();
		this.cmbitem = new UltraCombo();
		this.txtcustnetwt = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label31 = new System.Windows.Forms.Label();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label33 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtsupamount = new System.Windows.Forms.TextBox();
		this.txtsuprate = new System.Windows.Forms.TextBox();
		this.txtSupNetWeight = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label30 = new System.Windows.Forms.Label();
		this.labItemName = new System.Windows.Forms.Label();
		this.CmbPackingType = new UltraCombo();
		this.cmbcropYear = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.cmbuom = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.cmbsupplier = new UltraCombo();
		this.txtItemQty = new System.Windows.Forms.TextBox();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label39 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.e = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.grdHistoryDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label44 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label43 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.BtnLoadAllHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdsummary).BeginInit();
		this.panel4.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpayterm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomername).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsubbrokeryacc).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).BeginInit();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel11);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1170, 230);
		this.panel1.TabIndex = 10;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdsummary);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(885, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(285, 230);
		this.panel3.TabIndex = 20;
		this.grdsummary.AllowDelete = (InheritableBoolean)1;
		((ScrollableBase)this.grdsummary).BackColor = System.Drawing.Color.White;
		grdsummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdsummary.DesignTimeLayout = grdsummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdsummary).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdsummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdsummary.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdsummary).Location = new System.Drawing.Point(0, 25);
		((System.Windows.Forms.Control)(object)this.grdsummary).Name = "grdsummary";
		((ScrollableBase)this.grdsummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdsummary).Size = new System.Drawing.Size(285, 205);
		((System.Windows.Forms.Control)(object)this.grdsummary).TabIndex = 0;
		this.grdsummary.TotalRow = (InheritableBoolean)1;
		this.grdsummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdsummary).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(285, 25);
		this.panel4.TabIndex = 11;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(3, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(157, 19);
		this.label1.TabIndex = 40;
		this.label1.Text = "Supplier Summary";
		this.panel11.Controls.Add(this.groupBox1);
		this.panel11.Controls.Add(this.groupBox2);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(885, 230);
		this.panel11.TabIndex = 0;
		this.groupBox1.Controls.Add(this.txtfcyamount);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add(this.txtexchangerate);
		this.groupBox1.Controls.Add(this.txtmanualref);
		this.groupBox1.Controls.Add(this.label22);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbpayterm);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtdocdate);
		this.groupBox1.Controls.Add(this.txtduedays);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label21);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcurrency);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCustomername);
		this.groupBox1.Controls.Add(this.label20);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliveryterm);
		this.groupBox1.Controls.Add(this.txtCusOrderAmt);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(534, 222);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtfcyamount.BackColor = System.Drawing.Color.White;
		this.txtfcyamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtfcyamount.Location = new System.Drawing.Point(376, 195);
		this.txtfcyamount.Name = "txtfcyamount";
		this.txtfcyamount.ReadOnly = true;
		this.txtfcyamount.ShortcutsEnabled = false;
		this.txtfcyamount.Size = new System.Drawing.Size(153, 23);
		this.txtfcyamount.TabIndex = 11;
		this.txtfcyamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcyamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfcyamount_KeyPress);
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarks.Location = new System.Drawing.Point(75, 138);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(210, 80);
		this.txtRemarks.TabIndex = 4;
		this.txtRemarks.Text = "";
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(5, 135);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(52, 15);
		this.label11.TabIndex = 61;
		this.label11.Text = "Remarks";
		this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(287, 199);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(72, 15);
		this.label23.TabIndex = 65;
		this.label23.Text = "Fcy Amount";
		this.txtexchangerate.BackColor = System.Drawing.Color.White;
		this.txtexchangerate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtexchangerate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtexchangerate.Location = new System.Drawing.Point(376, 168);
		this.txtexchangerate.Name = "txtexchangerate";
		this.txtexchangerate.ReadOnly = true;
		this.txtexchangerate.ShortcutsEnabled = false;
		this.txtexchangerate.Size = new System.Drawing.Size(153, 23);
		this.txtexchangerate.TabIndex = 10;
		this.txtexchangerate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtexchangerate.TextChanged += new System.EventHandler(txtexchangerate_TextChanged);
		this.txtexchangerate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtexchangerate_KeyPress);
		this.txtmanualref.BackColor = System.Drawing.Color.White;
		this.txtmanualref.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtmanualref.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtmanualref.Location = new System.Drawing.Point(75, 105);
		this.txtmanualref.Name = "txtmanualref";
		this.txtmanualref.Size = new System.Drawing.Size(210, 23);
		this.txtmanualref.TabIndex = 3;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(287, 172);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(84, 15);
		this.label22.TabIndex = 64;
		this.label22.Text = "Exchange Rate";
		this.cmbpayterm.AllowNull = (DefaultableBoolean)1;
		this.cmbpayterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbpayterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbpayterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance17).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbpayterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbpayterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Location = new System.Drawing.Point(376, 16);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Name = "cmbpayterm";
		((System.Windows.Forms.Control)(object)this.cmbpayterm).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbpayterm).TabIndex = 5;
		this.cmbpayterm.ValueChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(287, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(84, 15);
		this.label2.TabIndex = 48;
		this.label2.Text = "Payment Term";
		this.txtdocdate.CustomFormat = "dd-MMM-yy";
		this.txtdocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtdocdate.Location = new System.Drawing.Point(75, 46);
		this.txtdocdate.Name = "txtdocdate";
		this.txtdocdate.Size = new System.Drawing.Size(210, 23);
		this.txtdocdate.TabIndex = 1;
		this.txtdocdate.TabStop = false;
		this.txtdocdate.ValueChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtduedays.BackColor = System.Drawing.Color.White;
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(376, 46);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.ShortcutsEnabled = false;
		this.txtduedays.Size = new System.Drawing.Size(153, 23);
		this.txtduedays.TabIndex = 6;
		this.txtduedays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtduedays.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(5, 50);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(54, 15);
		this.label4.TabIndex = 46;
		this.label4.Text = "DocDate";
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(287, 144);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(55, 15);
		this.label21.TabIndex = 63;
		this.label21.Text = "Fcy Code";
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(5, 22);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 44;
		this.label17.Text = "Doc No";
		this.label17.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.label3.AutoSize = true;
		this.label3.BackColor = System.Drawing.Color.White;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(287, 50);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(58, 15);
		this.label3.TabIndex = 51;
		this.label3.Text = "Due Days";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(75, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.ShortcutsEnabled = false;
		this.txtdocno.Size = new System.Drawing.Size(210, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.cmbcurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbcurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance77;
		((SpecialBoxBase)((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance25).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance25).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance25).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Location = new System.Drawing.Point(376, 138);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Name = "cmbcurrency";
		((System.Windows.Forms.Control)(object)this.cmbcurrency).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbcurrency).TabIndex = 9;
		this.cmbCustomername.AllowNull = (DefaultableBoolean)1;
		this.cmbCustomername.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCustomername.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCustomername.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance80;
		((SpecialBoxBase)((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance90).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance90).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCustomername).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCustomername.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Location = new System.Drawing.Point(75, 73);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Name = "cmbCustomername";
		((System.Windows.Forms.Control)(object)this.cmbCustomername).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.cmbCustomername).TabIndex = 2;
		this.cmbCustomername.ValueChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(287, 109);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(88, 21);
		this.label20.TabIndex = 62;
		this.label20.Text = "Order Amt";
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.White;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(5, 79);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(58, 15);
		this.label7.TabIndex = 41;
		this.label7.Text = "Customer";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 109);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(67, 15);
		this.label5.TabIndex = 56;
		this.label5.Text = "Manual Ref";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(287, 79);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 57;
		this.label6.Text = "Delivery Term";
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance121;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance124;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance125;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance126;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance127).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(376, 73);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(153, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 7;
		this.txtCusOrderAmt.BackColor = System.Drawing.Color.White;
		this.txtCusOrderAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCusOrderAmt.Font = new System.Drawing.Font("Verdana", 15f);
		this.txtCusOrderAmt.Location = new System.Drawing.Point(376, 103);
		this.txtCusOrderAmt.Name = "txtCusOrderAmt";
		this.txtCusOrderAmt.ReadOnly = true;
		this.txtCusOrderAmt.ShortcutsEnabled = false;
		this.txtCusOrderAmt.Size = new System.Drawing.Size(153, 32);
		this.txtCusOrderAmt.TabIndex = 8;
		this.txtCusOrderAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtCusOrderAmt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustbillamnt_KeyPress);
		this.groupBox2.Controls.Add(this.label27);
		this.groupBox2.Controls.Add(this.txtsubbrokeryamnt);
		this.groupBox2.Controls.Add(this.label26);
		this.groupBox2.Controls.Add(this.txtsubbrokeryrate);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc);
		this.groupBox2.Controls.Add(this.label19);
		this.groupBox2.Controls.Add(this.txtbrokeryamount);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.txtbrokeryrate);
		this.groupBox2.Controls.Add(this.label15);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcommtype);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtcommamount);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.txtcommrate);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCommAgent);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(538, 6);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(345, 222);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Commission & Brokery";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label27.Location = new System.Drawing.Point(168, 172);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(73, 15);
		this.label27.TabIndex = 33;
		this.label27.Text = "Brokery Amt";
		this.txtsubbrokeryamnt.BackColor = System.Drawing.Color.White;
		this.txtsubbrokeryamnt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsubbrokeryamnt.Enabled = false;
		this.txtsubbrokeryamnt.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsubbrokeryamnt.Location = new System.Drawing.Point(244, 168);
		this.txtsubbrokeryamnt.Name = "txtsubbrokeryamnt";
		this.txtsubbrokeryamnt.ReadOnly = true;
		this.txtsubbrokeryamnt.ShortcutsEnabled = false;
		this.txtsubbrokeryamnt.Size = new System.Drawing.Size(97, 23);
		this.txtsubbrokeryamnt.TabIndex = 8;
		this.txtsubbrokeryamnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtsubbrokeryamnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsubbrokeryamnt_KeyPress);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(4, 172);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(73, 15);
		this.label26.TabIndex = 31;
		this.label26.Text = "Brokery Rate";
		this.txtsubbrokeryrate.BackColor = System.Drawing.Color.White;
		this.txtsubbrokeryrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsubbrokeryrate.Enabled = false;
		this.txtsubbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtsubbrokeryrate.Location = new System.Drawing.Point(92, 168);
		this.txtsubbrokeryrate.Name = "txtsubbrokeryrate";
		this.txtsubbrokeryrate.ReadOnly = true;
		this.txtsubbrokeryrate.ShortcutsEnabled = false;
		this.txtsubbrokeryrate.Size = new System.Drawing.Size(75, 23);
		this.txtsubbrokeryrate.TabIndex = 7;
		this.txtsubbrokeryrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtsubbrokeryrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsubbrokeryrate_KeyPress);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(4, 144);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(87, 15);
		this.label25.TabIndex = 29;
		this.label25.Text = "Sub Broker A/c";
		this.cmbsubbrokeryacc.AllowNull = (DefaultableBoolean)1;
		this.cmbsubbrokeryacc.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsubbrokeryacc.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsubbrokeryacc.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Appearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance37).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsubbrokeryacc).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsubbrokeryacc.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Location = new System.Drawing.Point(92, 138);
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Name = "cmbsubbrokeryacc";
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbsubbrokeryacc).TabIndex = 6;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(4, 107);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(73, 15);
		this.label19.TabIndex = 27;
		this.label19.Text = "Brokery Amt";
		this.txtbrokeryamount.BackColor = System.Drawing.Color.White;
		this.txtbrokeryamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbrokeryamount.Enabled = false;
		this.txtbrokeryamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbrokeryamount.Location = new System.Drawing.Point(92, 103);
		this.txtbrokeryamount.Name = "txtbrokeryamount";
		this.txtbrokeryamount.ReadOnly = true;
		this.txtbrokeryamount.ShortcutsEnabled = false;
		this.txtbrokeryamount.Size = new System.Drawing.Size(105, 23);
		this.txtbrokeryamount.TabIndex = 5;
		this.txtbrokeryamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtbrokeryamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtbrokeryamount_KeyPress);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(197, 81);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(73, 15);
		this.label18.TabIndex = 25;
		this.label18.Text = "Brokery Rate";
		this.txtbrokeryrate.BackColor = System.Drawing.Color.White;
		this.txtbrokeryrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtbrokeryrate.Enabled = false;
		this.txtbrokeryrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtbrokeryrate.Location = new System.Drawing.Point(274, 77);
		this.txtbrokeryrate.Name = "txtbrokeryrate";
		this.txtbrokeryrate.ShortcutsEnabled = false;
		this.txtbrokeryrate.Size = new System.Drawing.Size(67, 23);
		this.txtbrokeryrate.TabIndex = 4;
		this.txtbrokeryrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtbrokeryrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtbrokeryrate_KeyPress);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(4, 54);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(74, 15);
		this.label15.TabIndex = 21;
		this.label15.Text = "Comm. Type";
		this.cmbcommtype.AllowNull = (DefaultableBoolean)1;
		this.cmbcommtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcommtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcommtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance132).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcommtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcommtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Location = new System.Drawing.Point(92, 48);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Name = "cmbcommtype";
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Size = new System.Drawing.Size(105, 26);
		((System.Windows.Forms.Control)(object)this.cmbcommtype).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbcommtype).Leave += new System.EventHandler(cmbcommtype_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(4, 81);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(69, 15);
		this.label14.TabIndex = 19;
		this.label14.Text = "Comm Amt";
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Enabled = false;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommamount.Location = new System.Drawing.Point(92, 77);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.ShortcutsEnabled = false;
		this.txtcommamount.Size = new System.Drawing.Size(105, 23);
		this.txtcommamount.TabIndex = 3;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommamount_KeyPress);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(197, 54);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 15);
		this.label13.TabIndex = 17;
		this.label13.Text = "Comm Rate";
		this.txtcommrate.BackColor = System.Drawing.Color.White;
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Enabled = false;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.Location = new System.Drawing.Point(274, 50);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.ShortcutsEnabled = false;
		this.txtcommrate.Size = new System.Drawing.Size(67, 23);
		this.txtcommrate.TabIndex = 2;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(4, 24);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(78, 15);
		this.label12.TabIndex = 11;
		this.label12.Text = "Comm Agent";
		this.cmbCommAgent.AllowNull = (DefaultableBoolean)1;
		this.cmbCommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbCommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Location = new System.Drawing.Point(92, 18);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Name = "cmbCommAgent";
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).Size = new System.Drawing.Size(249, 26);
		((System.Windows.Forms.Control)(object)this.cmbCommAgent).TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1184, 711);
		this.tabControl1.TabIndex = 33;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.toolStrip2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1176, 682);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel5);
		this.panel7.Controls.Add(this.panel12);
		this.panel7.Controls.Add(this.panel2);
		this.panel7.Controls.Add(this.panel1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 55);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1170, 624);
		this.panel7.TabIndex = 12;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 378);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1170, 246);
		this.panel5.TabIndex = 46;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1170, 246);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label24);
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 355);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1170, 23);
		this.panel12.TabIndex = 45;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(3, 3);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(42, 14);
		this.label24.TabIndex = 40;
		this.label24.Text = "Detail";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 43;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel2.Controls.Add(this.groupBox5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 230);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1170, 125);
		this.panel2.TabIndex = 11;
		this.groupBox5.Controls.Add(this.label10);
		this.groupBox5.Controls.Add(this.txtsupremarks);
		this.groupBox5.Controls.Add(this.label32);
		this.groupBox5.Controls.Add(this.label16);
		this.groupBox5.Controls.Add(this.txtcusamount);
		this.groupBox5.Controls.Add(this.txtcusrate);
		this.groupBox5.Controls.Add(this.label34);
		this.groupBox5.Controls.Add(this.label35);
		this.groupBox5.Controls.Add(this.rdSearchByCode);
		this.groupBox5.Controls.Add(this.txtcustremarks);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbitem);
		this.groupBox5.Controls.Add(this.txtcustnetwt);
		this.groupBox5.Controls.Add(this.label28);
		this.groupBox5.Controls.Add(this.rdSearchByName);
		this.groupBox5.Controls.Add(this.label31);
		this.groupBox5.Controls.Add(this.btnAddDetail);
		this.groupBox5.Controls.Add(this.label9);
		this.groupBox5.Controls.Add(this.label8);
		this.groupBox5.Controls.Add(this.label33);
		this.groupBox5.Controls.Add(this.btnCancelDetail);
		this.groupBox5.Controls.Add(this.txtsupamount);
		this.groupBox5.Controls.Add(this.txtsuprate);
		this.groupBox5.Controls.Add(this.txtSupNetWeight);
		this.groupBox5.Controls.Add(this.btnUpdateDetail);
		this.groupBox5.Controls.Add(this.label30);
		this.groupBox5.Controls.Add(this.labItemName);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackingType);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcropYear);
		this.groupBox5.Controls.Add(this.label29);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbuom);
		this.groupBox5.Controls.Add(this.label42);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsupplier);
		this.groupBox5.Controls.Add(this.txtItemQty);
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox5.Location = new System.Drawing.Point(3, 6);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(1162, 116);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Detail Information";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(527, 69);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(73, 15);
		this.label10.TabIndex = 124;
		this.label10.Text = "Cus Amount";
		this.txtsupremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupremarks.Location = new System.Drawing.Point(5, 88);
		this.txtsupremarks.Name = "txtsupremarks";
		this.txtsupremarks.Size = new System.Drawing.Size(324, 23);
		this.txtsupremarks.TabIndex = 9;
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label32.Location = new System.Drawing.Point(5, 69);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(99, 15);
		this.label32.TabIndex = 121;
		this.label32.Text = "Supplier Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(435, 69);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(52, 15);
		this.label16.TabIndex = 125;
		this.label16.Text = "Cus Rate";
		this.txtcusamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcusamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcusamount.Location = new System.Drawing.Point(529, 88);
		this.txtcusamount.Name = "txtcusamount";
		this.txtcusamount.ReadOnly = true;
		this.txtcusamount.ShortcutsEnabled = false;
		this.txtcusamount.Size = new System.Drawing.Size(113, 23);
		this.txtcusamount.TabIndex = 12;
		this.txtcusamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcusrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcusrate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcusrate.Location = new System.Drawing.Point(435, 88);
		this.txtcusrate.Name = "txtcusrate";
		this.txtcusrate.ShortcutsEnabled = false;
		this.txtcusrate.Size = new System.Drawing.Size(91, 23);
		this.txtcusrate.TabIndex = 11;
		this.txtcusrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcusrate.TextChanged += new System.EventHandler(txtcusrate_TextChanged);
		this.txtcusrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcusrate_KeyPress);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(645, 69);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(106, 15);
		this.label34.TabIndex = 121;
		this.label34.Text = "Customer Remarks";
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(332, 69);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(67, 15);
		this.label35.TabIndex = 119;
		this.label35.Text = "Cus Net Wt";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(127, 19);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 111;
		this.rdSearchByCode.TabStop = true;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.txtcustremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustremarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustremarks.Location = new System.Drawing.Point(645, 88);
		this.txtcustremarks.Name = "txtcustremarks";
		this.txtcustremarks.Size = new System.Drawing.Size(322, 23);
		this.txtcustremarks.TabIndex = 13;
		this.cmbitem.AllowNull = (DefaultableBoolean)1;
		this.cmbitem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbitem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbitem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbitem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbitem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance55;
		((SpecialBoxBase)((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbitem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbitem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance61).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance62).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbitem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbitem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbitem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbitem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbitem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbitem).Location = new System.Drawing.Point(5, 40);
		((System.Windows.Forms.Control)(object)this.cmbitem).Name = "cmbitem";
		((System.Windows.Forms.Control)(object)this.cmbitem).Size = new System.Drawing.Size(202, 26);
		((System.Windows.Forms.Control)(object)this.cmbitem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbitem).Leave += new System.EventHandler(cmbitem_Leave);
		this.txtcustnetwt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcustnetwt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcustnetwt.Location = new System.Drawing.Point(332, 88);
		this.txtcustnetwt.Name = "txtcustnetwt";
		this.txtcustnetwt.ReadOnly = true;
		this.txtcustnetwt.ShortcutsEnabled = false;
		this.txtcustnetwt.Size = new System.Drawing.Size(100, 23);
		this.txtcustnetwt.TabIndex = 10;
		this.txtcustnetwt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcustnetwt.TextChanged += new System.EventHandler(txtcustnetwt_TextChanged);
		this.txtcustnetwt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcustnetwt_KeyPress);
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(560, 21);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(51, 15);
		this.label28.TabIndex = 109;
		this.label28.Text = "ItemQty";
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(64, 19);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 110;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label31.Location = new System.Drawing.Point(332, 21);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(77, 15);
		this.label31.TabIndex = 107;
		this.label31.Text = "Packing Type";
		this.btnAddDetail.BackColor = System.Drawing.Color.Teal;
		this.btnAddDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAddDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddDetail.ForeColor = System.Drawing.Color.White;
		this.btnAddDetail.Location = new System.Drawing.Point(970, 86);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(33, 27);
		this.btnAddDetail.TabIndex = 14;
		this.btnAddDetail.Text = "+";
		this.btnAddDetail.UseVisualStyleBackColor = false;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.btnAddDetail.Leave += new System.EventHandler(btnAddDetail_Leave);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(1045, 21);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(75, 15);
		this.label9.TabIndex = 119;
		this.label9.Text = "Sup Amount";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(952, 21);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(54, 15);
		this.label8.TabIndex = 119;
		this.label8.Text = "Sup Rate";
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label33.Location = new System.Drawing.Point(850, 21);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(69, 15);
		this.label33.TabIndex = 119;
		this.label33.Text = "Sup Net Wt";
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(1074, 86);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(67, 27);
		this.btnCancelDetail.TabIndex = 16;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(btnAddDetail_Leave);
		this.txtsupamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsupamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsupamount.Location = new System.Drawing.Point(1043, 42);
		this.txtsupamount.Name = "txtsupamount";
		this.txtsupamount.ReadOnly = true;
		this.txtsupamount.ShortcutsEnabled = false;
		this.txtsupamount.Size = new System.Drawing.Size(115, 23);
		this.txtsupamount.TabIndex = 8;
		this.txtsupamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtsupamount.TextChanged += new System.EventHandler(txtSupNetWeight_TextChanged);
		this.txtsupamount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsuprate_KeyPress);
		this.txtsuprate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtsuprate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtsuprate.Location = new System.Drawing.Point(950, 42);
		this.txtsuprate.Name = "txtsuprate";
		this.txtsuprate.ShortcutsEnabled = false;
		this.txtsuprate.Size = new System.Drawing.Size(91, 23);
		this.txtsuprate.TabIndex = 7;
		this.txtsuprate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtsuprate.TextChanged += new System.EventHandler(txtsuprate_TextChanged);
		this.txtsuprate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsuprate_KeyPress);
		this.txtSupNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupNetWeight.Location = new System.Drawing.Point(848, 42);
		this.txtSupNetWeight.Name = "txtSupNetWeight";
		this.txtSupNetWeight.ReadOnly = true;
		this.txtSupNetWeight.ShortcutsEnabled = false;
		this.txtSupNetWeight.Size = new System.Drawing.Size(100, 23);
		this.txtSupNetWeight.TabIndex = 6;
		this.txtSupNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtSupNetWeight.TextChanged += new System.EventHandler(txtSupNetWeight_TextChanged);
		this.txtSupNetWeight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtsuprate_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1003, 86);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(71, 27);
		this.btnUpdateDetail.TabIndex = 15;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label30.Location = new System.Drawing.Point(209, 21);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(57, 15);
		this.label30.TabIndex = 107;
		this.label30.Text = "Crop Year";
		this.labItemName.AutoSize = true;
		this.labItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labItemName.Location = new System.Drawing.Point(5, 21);
		this.labItemName.Name = "labItemName";
		this.labItemName.Size = new System.Drawing.Size(32, 15);
		this.labItemName.TabIndex = 105;
		this.labItemName.Text = "Item";
		this.CmbPackingType.AllowNull = (DefaultableBoolean)1;
		this.CmbPackingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance42;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance48).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance49).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbPackingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Location = new System.Drawing.Point(332, 40);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Name = "CmbPackingType";
		((System.Windows.Forms.Control)(object)this.CmbPackingType).Size = new System.Drawing.Size(146, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackingType).TabIndex = 2;
		this.cmbcropYear.AllowNull = (DefaultableBoolean)1;
		this.cmbcropYear.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcropYear.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcropYear.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance56).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance56).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcropYear).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbcropYear.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Location = new System.Drawing.Point(209, 40);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Name = "cmbcropYear";
		((System.Windows.Forms.Control)(object)this.cmbcropYear).Size = new System.Drawing.Size(120, 26);
		((System.Windows.Forms.Control)(object)this.cmbcropYear).TabIndex = 1;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.Location = new System.Drawing.Point(481, 21);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(35, 15);
		this.label29.TabIndex = 107;
		this.label29.Text = "UOM";
		this.cmbuom.AllowNull = (DefaultableBoolean)1;
		this.cmbuom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbuom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbuom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbuom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbuom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbuom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbuom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance92).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbuom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbuom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbuom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbuom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbuom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbuom).Location = new System.Drawing.Point(481, 40);
		((System.Windows.Forms.Control)(object)this.cmbuom).Name = "cmbuom";
		((System.Windows.Forms.Control)(object)this.cmbuom).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.cmbuom).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.cmbuom).Leave += new System.EventHandler(cmbuom_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(645, 21);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(51, 15);
		this.label42.TabIndex = 111;
		this.label42.Text = "Supplier";
		this.cmbsupplier.AllowNull = (DefaultableBoolean)1;
		this.cmbsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance91;
		((SpecialBoxBase)((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance99).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance99).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance99).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance107).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Location = new System.Drawing.Point(645, 40);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Name = "cmbsupplier";
		((System.Windows.Forms.Control)(object)this.cmbsupplier).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.cmbsupplier).TabIndex = 5;
		this.txtItemQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemQty.Location = new System.Drawing.Point(562, 42);
		this.txtItemQty.Name = "txtItemQty";
		this.txtItemQty.ShortcutsEnabled = false;
		this.txtItemQty.Size = new System.Drawing.Size(80, 23);
		this.txtItemQty.TabIndex = 4;
		this.txtItemQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtItemQty.TextChanged += new System.EventHandler(txtItemQty_TextChanged);
		this.txtItemQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtItemQty_KeyPress);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label39);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 30);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1170, 25);
		this.panel6.TabIndex = 10;
		this.label39.AutoSize = true;
		this.label39.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label39.ForeColor = System.Drawing.Color.White;
		this.label39.Location = new System.Drawing.Point(3, 3);
		this.label39.Name = "label39";
		this.label39.Size = new System.Drawing.Size(157, 19);
		this.label39.TabIndex = 40;
		this.label39.Text = "Commission Order";
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnRefresh, this.btnsave, this.btnUpdate, this.e, this.btnAttachment, this.btnprint, this.btnShortCutKeys });
		this.toolStrip2.Location = new System.Drawing.Point(3, 3);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip2.TabIndex = 9;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(100, 24);
		this.btnprint.Text = "1705-&Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.e.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.e.Image = Architecture.WinApp.Properties.Resources.icons8_cut_16;
		this.e.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.e.Name = "e";
		this.e.Size = new System.Drawing.Size(73, 24);
		this.e.Text = "Delete";
		this.e.Visible = false;
		this.e.Click += new System.EventHandler(BtnDelete_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnShortCutKeys.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnShortCutKeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeys.Image = (System.Drawing.Image)resources.GetObject("btnShortCutKeys.Image");
		this.btnShortCutKeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeys.Name = "btnShortCutKeys";
		this.btnShortCutKeys.Size = new System.Drawing.Size(106, 24);
		this.btnShortCutKeys.Text = "ShortCut Keys";
		this.btnShortCutKeys.Click += new System.EventHandler(btnShortCutKeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add(this.toolStrip1);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1176, 682);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(3, 454);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(1170, 225);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 49;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label44);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 431);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1170, 23);
		this.panel10.TabIndex = 48;
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Tahoma", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.ForeColor = System.Drawing.Color.White;
		this.label44.Location = new System.Drawing.Point(3, 3);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(191, 14);
		this.label44.TabIndex = 40;
		this.label44.Text = "Detail Of Above Selected Row";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 53);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1170, 378);
		this.panel9.TabIndex = 47;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistory).BackColor = System.Drawing.Color.White;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1170, 378);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 14;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label43);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 30);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1170, 23);
		this.panel8.TabIndex = 46;
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.White;
		this.label43.Location = new System.Drawing.Point(3, 3);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(67, 19);
		this.label43.TabIndex = 40;
		this.label43.Text = "History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1129, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 43;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnLoadAllHistory });
		this.toolStrip1.Location = new System.Drawing.Point(3, 3);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1170, 27);
		this.toolStrip1.TabIndex = 45;
		this.toolStrip1.Text = "toolStrip1";
		this.BtnLoadAllHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadAllHistory.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadAllHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadAllHistory.Name = "BtnLoadAllHistory";
		this.BtnLoadAllHistory.Size = new System.Drawing.Size(82, 24);
		this.BtnLoadAllHistory.Text = "&Load All";
		this.BtnLoadAllHistory.Click += new System.EventHandler(BtnLoadAllHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1184, 711);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "CommissionOrder";
		this.Text = "CommissionOrder";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(CommissionOrder_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(CommissionOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdsummary).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbpayterm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCustomername).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbsubbrokeryacc).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcommtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCommAgent).EndInit();
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbitem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcropYear).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbuom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsupplier).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.tabPage2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
