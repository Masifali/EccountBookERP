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
using Architecture.BLL.Inventory;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.pcc;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.pcc.purchase;

public class PurchaseOrderConcrete : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDetail = new DataTable();

	private int updateDetailIndex;

	private int RecId;

	private long RowVersionId;

	private bool Approved;

	private bool HasMultiCurrencyFeature;

	public bool TaxPercntEditableOrNot;

	private decimal BaseRate;

	private int BaseCurrency;

	private List<PurchaseOrderDetail> lstRemoveDetailRecord = new List<PurchaseOrderDetail>();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnSave;

	private ToolStripButton btnAttachment;

	private TabPage tabPage2;

	private GroupBox groupBox1;

	private TextBox txtAmount;

	private Label label14;

	private Label label42;

	private TextBox txtRate;

	private Label label40;

	private TextBox txtQty;

	private Label label38;

	private Label label32;

	private Label label33;

	internal UltraCombo comjobLot;

	internal UltraCombo comItem;

	private Button btnAdd;

	internal GridEX grd;

	private TextBox txtCommAgentId;

	private TextBox txtSupplierGLId;

	internal GridEX grdHistory;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private ToolStripButton btnSlip;

	private Label label4;

	private TextBox txtduedays;

	private Label label10;

	private DateTimePicker duedate;

	private Label label29;

	internal UltraCombo comsupplier;

	internal UltraCombo combcommAgent;

	private Label label7;

	private Label label20;

	private Label label26;

	private Label label11;

	private TextBox txtCommissionRemarks;

	private Label label6;

	private Label label25;

	private Label label21;

	private TextBox txtReferenceNo;

	private TextBox txtcommrate;

	private Label label19;

	private TextBox txtcommamount;

	internal UltraCombo comproject;

	internal UltraCombo combranches;

	private Label label13;

	private Label label5;

	private Label label18;

	private DateTimePicker DocDate;

	private TextBox txtdocno;

	private ToolStripButton btnDelete;

	internal UltraCombo combpttrm;

	internal UltraCombo combCommType;

	internal UltraCombo combdeliverytrm;

	internal UltraCombo combcommUOM;

	private Panel panel9;

	private Panel panel2;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton btnRefresh;

	private Panel panel10;

	internal GridEX grdDetailHistory;

	private CheckBox ChkPrintslip;

	private CheckBox ChkResetOnSave;

	internal UltraCombo txtCity;

	private Label label8;

	private Panel panel11;

	private GroupBox groupBox2;

	private Label label17;

	private Panel panel16;

	private Label label23;

	private Panel panel17;

	private Label label54;

	private Panel panel18;

	private CtrlGrdBar ctrlGrdBar3;

	private Label label55;

	private Panel panel19;

	private GroupBox groupBox3;

	private TextBox txtOrderWeight;

	private TextBox txtOrderQty;

	private Label label62;

	private Label label63;

	internal UltraCombo cmbCurrency;

	private TextBox txtFcyAmount;

	private TextBox txtExchangeRate;

	private Label label59;

	private Label label60;

	private Label label61;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripButton btnshortcutkeys;

	private TabControl tabControl2;

	private TabPage tabPage3;

	internal UltraCombo CmbAttributeVarient;

	private Label label1;

	private Label label2;

	private Label label9;

	private TextBox txtItemWeight;

	private TextBox txtVarientEquivalent;

	private Label label71;

	private TextBox txtTotalAmount;

	private Panel panel5;

	private Label label78;

	private GroupBox groupBox4;

	private Button btnshow;

	private Label label73;

	internal UltraCombo CmbCustomerHistory;

	private Label label75;

	internal UltraCombo cmbDateTypeHistory;

	private DateTimePicker txtToDateHistory;

	private Label label76;

	private TextBox txtToDocNoHistory;

	private DateTimePicker txtFromdateHistory;

	private Label label77;

	private TextBox txtFromNoHistory;

	private Panel panel8;

	private TextBox txtDeliverydays;

	private Label label28;

	private DateTimePicker txtDeliveryStartDate;

	private Label label41;

	private Label label43;

	private TextBox txtNetWeight;

	private Label label3;

	private ToolStripButton BtnRefreshHistory;

	private Label label49;

	internal UltraCombo cmbtaxtype;

	private Label label50;

	private Label label64;

	private TextBox txtTaxAmount;

	private TextBox txtTaxPercent;

	private TextBox txtremarks;

	private Label label16;

	private GroupBox CurrencyInfoBox;

	private Label label22;

	private TextBox txtOrderAmount;

	private Label label12;

	private Label label24;

	private TextBox txtRemarksDetail;

	private ToolStripButton btnSaveAs;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	public PurchaseOrderConcrete(UserAccount user)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void InvfrmPurchasedirectInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1310, 700);
			TaxPercntEditableOrNot = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("TaxPercentEditable"));
			if (TaxPercntEditableOrNot)
			{
				txtTaxPercent.Enabled = true;
			}
			else
			{
				txtTaxPercent.Enabled = false;
			}
			string ScreenName = Conversion.ToString(base.Tag);
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnSave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			btnDelete.Visible = false;
			btnSlip.Enabled = formright.DoHavePrintRights;
			ChkPrintslip.Enabled = formright.DoHavePrintRights;
			ChkPrintslip.Checked = formright.DoHavePrintRights;
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
			ctrlGrdBar3.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar3.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar3.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar3.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar3.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar3.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			CurrencyFill();
			MultiCurrencyFeature();
			GetallTaxType();
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("ItemId", typeof(int));
			dtDetail.Columns.Add("ItemName", typeof(string));
			dtDetail.Columns.Add("ItemCode", typeof(string));
			dtDetail.Columns.Add("AttributeVarientId", typeof(int));
			dtDetail.Columns.Add("AttributeVarient", typeof(string));
			dtDetail.Columns.Add("VarientUnit", typeof(decimal));
			dtDetail.Columns.Add("ItemQty", typeof(decimal));
			dtDetail.Columns.Add("ItemWeight", typeof(decimal));
			dtDetail.Columns.Add("NetWeight", typeof(decimal));
			dtDetail.Columns.Add("Rate", typeof(decimal));
			dtDetail.Columns.Add("ItemAmount", typeof(decimal));
			dtDetail.Columns.Add("TaxId", typeof(int));
			dtDetail.Columns.Add("TaxName", typeof(string));
			dtDetail.Columns.Add("Tax%", typeof(decimal));
			dtDetail.Columns.Add("TaxAmount", typeof(decimal));
			dtDetail.Columns.Add("TotalAmount", typeof(decimal));
			dtDetail.Columns.Add("FcyAmount", typeof(decimal));
			dtDetail.Columns.Add("JobLotId", typeof(int));
			dtDetail.Columns.Add("JobLot", typeof(string));
			dtDetail.Columns.Add("CityId", typeof(int));
			dtDetail.Columns.Add("CityName");
			dtDetail.Columns.Add("Remarks");
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSettings();
			Project();
			braches();
			DocumentNo();
			suppliercustomer();
			PaymentTerms();
			DeliveryTerm();
			CommissionTypeFill();
			CommissionUOMFill();
			item();
			JobLotBind();
			BinCity();
			ConfigurationDefault();
			HistoryComboBind();
			ParameterFill();
			if (Conversion.ToInt(cmbDateTypeHistory.Value) == 0)
			{
				((UltraGridBase)cmbDateTypeHistory).Rows[0].Activate();
			}
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			DocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Project()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, comproject, "Id", "ProjectName", "Project Name", false);
				((UltraGridBase)comproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void braches()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, combranches, "Id", "BranchName", "Branch Name", false);
				((UltraGridBase)combranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentNo()
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
			int Code = PurchaseOrder.GenerateCode(new PurchaseOrder
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1863,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			if (Code > 0)
			{
				txtdocno.Text = Code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetallTaxType()
	{
		try
		{
			DataTable dtTaxType = ItemTaxSchedule.GetAllTaxSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, DocDate.Value, 2);
			if (dtTaxType.Rows.Count > 0)
			{
				DDL.BindDDL(dtTaxType, cmbtaxtype, "TaxNameId", "TaxName", "Tax Type", ZeroIndex: false);
				((UltraGridBase)cmbtaxtype).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				((UltraGridBase)cmbtaxtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void suppliercustomer()
	{
		try
		{
			int Id = Conversion.ToInt(comsupplier.Value);
			int Idr = Conversion.ToInt(combcommAgent.Value);
			bool eRPFeatureById = CommonServices.GetERPFeatureById(4);
			DataTable dtSupplier = new DataTable();
			if (eRPFeatureById)
			{
				dtSupplier = CommonServices.GetVendorsAndCustomers(1);
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, combcommAgent, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
					((UltraGridBase)combcommAgent).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				}
			}
			else
			{
				dtSupplier = CommonServices.SupplierCustomerGetforComboServiceBind();
				if (dtSupplier.Rows.Count > 0)
				{
					DDL.BindDDL(dtSupplier, comsupplier, "Id", "CompanyName", "Customer Name", ZeroIndex: true);
					DDL.BindDDL(dtSupplier, combcommAgent, "Id", "CompanyName", "SalesMan Name", ZeroIndex: true);
				}
			}
			if (Id > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comsupplier.Value = Id;
				}
				else
				{
					((Control)(object)comsupplier).Text = string.Empty;
				}
			}
			if (Idr > 0)
			{
				if (dtSupplier.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idr))
				{
					combcommAgent.Value = Idr;
				}
				else
				{
					((Control)(object)combcommAgent).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTerms()
	{
		try
		{
			int Id = Conversion.ToInt(combpttrm.Value);
			DataTable dtTerm = CommonServices.GetDueTermServiceBind();
			if (dtTerm.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtTerm, combpttrm, "Id", "TermsDescription", "Payment Term", true);
			((UltraGridBase)combpttrm).Rows[2].Activate();
			if (Id > 0)
			{
				if (dtTerm.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					combpttrm.Value = Id;
				}
				else
				{
					((Control)(object)combpttrm).Text = string.Empty;
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
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("DeliveryTerm");
			dt.Rows.Add(1, "Load");
			dt.Rows.Add(2, "Ponch");
			DDL.BindDDL(dt, combdeliverytrm, "Id", "DeliveryTerm", "Delivery Term", ZeroIndex: false);
			((UltraGridBase)combdeliverytrm).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CommissionType");
			dt.Rows.Add(1, "Flat");
			dt.Rows.Add(2, "Percent");
			DDL.BindDDL(dt, combCommType, "Id", "CommissionType", "CommissionType", ZeroIndex: false);
			((UltraGridBase)combCommType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CommissionUOMFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("UOM");
			dt.Rows.Add(1, "40");
			dt.Rows.Add(2, "50");
			dt.Rows.Add(3, "60");
			dt.Rows.Add(4, "100");
			DDL.BindDDL(dt, combcommUOM, "Id", "UOM", "UOM", ZeroIndex: false);
			((UltraGridBase)combcommUOM).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void item()
	{
		try
		{
			int Id = Conversion.ToInt(comItem.Value);
			((Control)(object)comItem).Text = string.Empty;
			DataTable dtitem = new DataTable();
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtitem.Columns.Add("ItemWeight", typeof(double));
			DataTable dt = CommonServices.ReadAllItems();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtitem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCodeNew"], dt.Rows[i]["WeightKgs"]);
				}
			}
			if (dtitem.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtitem, comItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)comItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				if (Id > 0)
				{
					if (dtitem.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						comItem.Value = Id;
					}
					else
					{
						((Control)(object)comItem).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)comItem).Text = string.Empty;
				((UltraGridBase)comItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comItem_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(comItem.Value) > 0)
			{
				txtItemWeight.Text = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemWeight"].Value);
				bindvarientunit(Conversion.ToInt(comItem.Value));
			}
			else
			{
				txtItemWeight.Text = 0.ToString();
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
				dt.Columns.Add("ItemAttribute");
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

	private void JobLotBind()
	{
		try
		{
			int Id = Conversion.ToInt(comjobLot.Value);
			DataTable dtjoblot = CommonServices.JobLotGetAllService();
			if (dtjoblot.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtjoblot, comjobLot, "Id", "JobLotDescription", "Job Lot ", true);
			if (Id > 0)
			{
				if (dtjoblot.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					comjobLot.Value = Id;
				}
				else
				{
					((Control)(object)comjobLot).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BinCity()
	{
		try
		{
			int Id = Conversion.ToInt(txtCity.Value);
			DataTable dtCity = CommonServices.CityGetAllService();
			if (dtCity.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dtCity, txtCity, "Id", "CityName", "City Name ", true);
			if (Id > 0)
			{
				if (dtCity.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					txtCity.Value = Id;
				}
				else
				{
					((Control)(object)txtCity).Text = string.Empty;
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("City Not Found");
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				int CityId = Conversion.ToInt((object)list[0].ConfigKey.ToString());
				if (Conversion.ToInt(txtCity.Value) == 0)
				{
					txtCity.Value = CityId;
				}
			}
			List<ConfigrationsAllocation> lstJob = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Job/Lot").ToList();
			if (lstJob.Count > 0)
			{
				int JobId = Conversion.ToInt((object)lstJob[0].ConfigKey.ToString());
				if (Conversion.ToInt(comjobLot.Value) == 0)
				{
					comjobLot.Value = JobId;
				}
			}
			List<ConfigrationsAllocation> BaseCurrencylist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Base Currency").ToList();
			if (BaseCurrencylist.Count > 0)
			{
				int Currency = Conversion.ToInt((object)BaseCurrencylist[0].ConfigKey.ToString());
				BaseCurrency = Currency;
				if (Conversion.ToInt(cmbCurrency.Value) == 0)
				{
					cmbCurrency.Value = BaseCurrency;
				}
			}
			List<ConfigrationsAllocation> BaseRatelist = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "BaseCurrencyRate").ToList();
			if (BaseRatelist.Count > 0)
			{
				decimal BaseCurrencyRate = (BaseRate = Conversion.ToDecimal((object)BaseRatelist[0].ConfigKey.ToString()));
				if (Conversion.ToDecimal((object)txtExchangeRate.Text) == 0m)
				{
					txtExchangeRate.Text = BaseCurrencyRate.ToString(clsGlobalVariables.DecimalRateFormate);
				}
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
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
			int id = Conversion.ToInt(cmbCurrency.Value);
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, cmbCurrency, "Id", "CurrencyCode", "Currency", true);
			((UltraGridBase)cmbCurrency).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
				{
					cmbCurrency.Value = id;
				}
				else
				{
					((Control)(object)cmbCurrency).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void MultiCurrencyFeature()
	{
		try
		{
			HasMultiCurrencyFeature = CommonServices.GetERPFeatureById(6);
			if (HasMultiCurrencyFeature)
			{
				CurrencyInfoBox.Visible = true;
			}
			else
			{
				CurrencyInfoBox.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbCurrency_Leave(object sender, EventArgs e)
	{
	}

	private void txtExchangeRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (grd.GetRows().Count() > 0 && Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m)
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					decimal amount = Conversion.ToDecimal(obj.Cells["ItemAmount"].Value) / Conversion.ToDecimal((object)txtExchangeRate.Text);
					obj.Cells["FcyAmount"].Value = amount;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grd.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["FcyAmount"].Value = 0;
					obj2.EndEdit();
				}
			}
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void txtExchangeRate_Leave(object sender, EventArgs e)
	{
		try
		{
			txtExchangeRate.Text = Conversion.ToDecimal((object)txtExchangeRate.Text).ToString(clsGlobalVariables.DecimalRateFormate);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnAdd_Click(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow obj in rows)
			{
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int AttributeVarientId = Conversion.ToInt(obj.Cells["AttributeVarientId"].Value);
				int JobLotId = Conversion.ToInt(obj.Cells["JobLotId"].Value);
				if (Conversion.ToInt(comItem.Value) == ItemId && Conversion.ToInt(CmbAttributeVarient.Value) == AttributeVarientId && Conversion.ToInt(comjobLot.Value) == JobLotId)
				{
					MessageBox.Show("Duplicate items are not added to the grid. An item with the same warehouse, job lot, and AttributeVarient already exists");
					return;
				}
			}
			if (FormValidationDetail())
			{
				dtDetail.Rows.Add(0, Conversion.ToInt(comItem.Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value), Conversion.ToInt(CmbAttributeVarient.Value), Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text), Conversion.ToDecimal((object)txtVarientEquivalent.Text), Conversion.ToDecimal((object)txtQty.Text), Conversion.ToDecimal((object)txtItemWeight.Text), Conversion.ToDecimal((object)txtNetWeight.Text), Conversion.ToDecimal((object)txtRate.Text), Conversion.ToDecimal((object)txtAmount.Text), Conversion.ToInt(cmbtaxtype.Value), Conversion.ToString((object)((Control)(object)cmbtaxtype).Text), Conversion.ToDecimal((object)txtTaxPercent.Text), Conversion.ToDecimal((object)txtTaxAmount.Text), Conversion.ToDecimal((object)txtTotalAmount.Text), 0, Conversion.ToInt(comjobLot.Value), Conversion.ToString((object)((Control)(object)comjobLot).Text), Conversion.ToInt(txtCity.Value), Conversion.ToString((object)((Control)(object)txtCity).Text), Conversion.ToString((object)txtRemarksDetail.Text));
				grd.DataSource = dtDetail;
				grd.RetrieveStructure();
				grdSettings();
				txtExchangeRate_TextChanged(null, null);
				TotalCommissionAmount();
				ResetDetail();
				BillAmount();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				updateDetailIndex = r.RowIndex;
				comItem.Value = r.Cells["ItemId"].Value.ToString();
				bindvarientunit(Conversion.ToInt(comItem.Value));
				CmbAttributeVarient.Value = r.Cells["AttributeVarientId"].Value.ToString();
				txtVarientEquivalent.Text = r.Cells["VarientUnit"].Value.ToString();
				txtQty.Text = Conversion.ToDecimal(r.Cells["ItemQty"].Value).ToString("#,##0.###");
				txtItemWeight.Text = Conversion.ToDecimal(r.Cells["ItemWeight"].Value).ToString("#,##0.####");
				txtNetWeight.Text = Conversion.ToDecimal(r.Cells["NetWeight"].Value).ToString("#,##0.####");
				txtRate.Text = Conversion.ToDecimal(r.Cells["Rate"].Value).ToString(clsGlobalVariables.DecimalRateFormate);
				txtAmount.Text = Conversion.ToDecimal(r.Cells["ItemAmount"].Value).ToString("#,##0.####");
				if (Conversion.ToInt(r.Cells["TaxId"].Value) > 0)
				{
					cmbtaxtype.Value = r.Cells["TaxId"].Value.ToString();
				}
				txtTaxPercent.Text = Conversion.ToDecimal(r.Cells["Tax%"].Value).ToString("#,##0.###");
				txtTaxAmount.Text = Conversion.ToDecimal(r.Cells["TaxAmount"].Value).ToString("#,##0.####");
				txtTotalAmount.Text = Conversion.ToDecimal(r.Cells["TotalAmount"].Value).ToString("#,##0.####");
				comjobLot.Value = r.Cells["JobLotId"].Value.ToString();
				txtCity.Value = r.Cells["CityId"].Value.ToString();
				txtRemarksDetail.Text = r.Cells["Remarks"].Value.ToString();
				btnAdd.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				comItem.Focus();
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
			if (!FormValidationDetail())
			{
				return;
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow item in rows)
			{
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int AttributeVarientId = Conversion.ToInt(item.Cells["AttributeVarientId"].Value);
				int JobLotId = Conversion.ToInt(item.Cells["JobLotId"].Value);
				if (Conversion.ToInt(comItem.Value) == ItemId && Conversion.ToInt(CmbAttributeVarient.Value) == AttributeVarientId && Conversion.ToInt(comjobLot.Value) == JobLotId && item.RowIndex != updateDetailIndex)
				{
					MessageBox.Show("Duplicate items are not added to the grid. An item with the same warehouse, job lot, and AttributeVarient already exists");
					return;
				}
			}
			dtDetail.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(comItem.Value);
			dtDetail.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemName"].Value);
			dtDetail.Rows[updateDetailIndex]["ItemCode"] = Conversion.ToString(((UltraDropDownBase)comItem).SelectedRow.Cells["ItemCode"].Value);
			dtDetail.Rows[updateDetailIndex]["AttributeVarientId"] = Conversion.ToInt(CmbAttributeVarient.Value);
			dtDetail.Rows[updateDetailIndex]["AttributeVarient"] = Conversion.ToString((object)((Control)(object)CmbAttributeVarient).Text);
			dtDetail.Rows[updateDetailIndex]["VarientUnit"] = Conversion.ToString((object)txtVarientEquivalent.Text);
			dtDetail.Rows[updateDetailIndex]["ItemQty"] = Conversion.ToDecimal((object)txtQty.Text);
			dtDetail.Rows[updateDetailIndex]["ItemWeight"] = Conversion.ToDecimal((object)txtItemWeight.Text);
			dtDetail.Rows[updateDetailIndex]["NetWeight"] = Conversion.ToDecimal((object)txtNetWeight.Text);
			dtDetail.Rows[updateDetailIndex]["Rate"] = Conversion.ToDecimal((object)txtRate.Text);
			dtDetail.Rows[updateDetailIndex]["ItemAmount"] = Conversion.ToDecimal((object)txtAmount.Text);
			dtDetail.Rows[updateDetailIndex]["TaxId"] = Conversion.ToInt(cmbtaxtype.Value);
			dtDetail.Rows[updateDetailIndex]["TaxName"] = Conversion.ToInt((object)((Control)(object)cmbtaxtype).Text);
			dtDetail.Rows[updateDetailIndex]["Tax%"] = Conversion.ToDecimal((object)txtTaxPercent.Text);
			dtDetail.Rows[updateDetailIndex]["TaxAmount"] = Conversion.ToDecimal((object)txtTaxAmount.Text);
			dtDetail.Rows[updateDetailIndex]["TotalAmount"] = Conversion.ToDecimal((object)txtTotalAmount.Text);
			dtDetail.Rows[updateDetailIndex]["JobLotId"] = Conversion.ToInt(comjobLot.Value);
			dtDetail.Rows[updateDetailIndex]["JobLot"] = Conversion.ToString((object)((Control)(object)comjobLot).Text);
			dtDetail.Rows[updateDetailIndex]["CityId"] = Conversion.ToInt(txtCity.Value);
			dtDetail.Rows[updateDetailIndex]["CityName"] = Conversion.ToString((object)((Control)(object)txtCity).Text);
			dtDetail.Rows[updateDetailIndex]["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
			BillAmount();
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
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			updateDetailIndex = -1;
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
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["AttributeVarientId"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["TaxId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["CityId"].Visible = false;
			grd.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grd.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["VarientUnit"].FormatString = "#,##0.##";
			grd.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grd.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grd.RootTable.Columns["Rate"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Rate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grd.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["Tax%"].FormatString = "#,##0.###";
			grd.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TaxAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TaxAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grd.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grd.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grd.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grd.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grd);
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
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
					PurchaseOrderDetail detail = new PurchaseOrderDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
					detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
					detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					detail.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					detail.TaxTypeId = Conversion.ToInt(r.Cells["TaxId"].Value);
					detail.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
					detail.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
					detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					detail.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					detail.ItemNetAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
					detail.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtDetail.Rows[r.RowIndex].Delete();
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
			}
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
			BillAmount();
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
		if (((UltraGridBase)comsupplier).ActiveRow == null || Conversion.ToInt(comsupplier.Value) == 0)
		{
			MessageBox.Show("CustomerName Field is Required");
			comsupplier.Focus();
			return false;
		}
		if (((UltraGridBase)combpttrm).ActiveRow == null || Conversion.ToInt(combpttrm.Value) == 0)
		{
			MessageBox.Show("Payment Term Field is Required");
			combpttrm.Focus();
			return false;
		}
		if (((UltraGridBase)combdeliverytrm).ActiveRow == null || Conversion.ToInt(combdeliverytrm.Value) == 0)
		{
			MessageBox.Show("Delivery Term Field is Required");
			combdeliverytrm.Focus();
			return false;
		}
		if (HasMultiCurrencyFeature)
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Fcy Code Field is Required");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Exchange Rate Field is Required");
				txtExchangeRate.Focus();
				return false;
			}
			if (txtFcyAmount.Text.Trim() == string.Empty || txtFcyAmount.Text.Trim() == "0")
			{
				MessageBox.Show("Fcy Amount Rate Field is Required");
				txtFcyAmount.Focus();
				return false;
			}
		}
		else
		{
			if (((UltraGridBase)cmbCurrency).ActiveRow == null || Conversion.ToInt(cmbCurrency.Value) == 0)
			{
				MessageBox.Show("Please Configure Your Base Currency In configurations");
				cmbCurrency.Focus();
				return false;
			}
			if (txtExchangeRate.Text.Trim() == string.Empty || txtExchangeRate.Text.Trim() == "0")
			{
				MessageBox.Show("Please Configure Your Base Currency Rate In configurations");
				txtExchangeRate.Focus();
				return false;
			}
		}
		if (Conversion.ToString((object)((Control)(object)combpttrm).Text) == "Credit" && Conversion.ToInt((object)txtduedays.Text.Trim()) <= 0)
		{
			MessageBox.Show("Due Days Field is Required");
			txtduedays.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtOrderAmount.Text) > 0.0))
		{
			MessageBox.Show("Order Amount Field is Required");
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)comItem).ActiveRow == null || Conversion.ToInt(comItem.Value) == 0)
		{
			MessageBox.Show("Item Name Field is Required");
			comItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbAttributeVarient).ActiveRow == null || Conversion.ToInt(CmbAttributeVarient.Value) == 0)
		{
			MessageBox.Show("Attribute Varient Field is Required");
			CmbAttributeVarient.Focus();
			return false;
		}
		if (txtQty.Text.Trim() == "" || txtQty.Text.Trim() == "0")
		{
			MessageBox.Show("Qty Field is Required");
			txtQty.Focus();
			return false;
		}
		if (txtItemWeight.Text.Trim() == "" || txtItemWeight.Text.Trim() == "0")
		{
			MessageBox.Show("Item Weight Field is Required");
			txtItemWeight.Focus();
			return false;
		}
		if (txtRate.Text.Trim() == "" || txtRate.Text.Trim() == "0")
		{
			MessageBox.Show("Rate Field is Required");
			txtRate.Focus();
			return false;
		}
		if (txtAmount.Text == string.Empty || txtAmount.Text == "0")
		{
			throw new Exception("Please Check Item Amount");
		}
		if (txtTotalAmount.Text == string.Empty || txtTotalAmount.Text == "0")
		{
			throw new Exception("Please Check Total Amount");
		}
		if (((UltraGridBase)comjobLot).ActiveRow == null || Conversion.ToInt(comjobLot.Value) == 0)
		{
			MessageBox.Show("Job/Lot Field is Required");
			comjobLot.Focus();
			return false;
		}
		if (((UltraGridBase)txtCity).ActiveRow == null || Conversion.ToInt(txtCity.Value) == 0)
		{
			MessageBox.Show("City Field is Required");
			txtCity.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0)
		{
			if (txtTaxPercent.Text.Trim() == string.Empty || txtTaxPercent.Text.Trim() == "0" || txtTaxPercent.Text.Trim() == null)
			{
				MessageBox.Show("Tax Percent is Required");
				txtTaxPercent.Focus();
				return false;
			}
			if (Conversion.ToDouble((object)txtTaxAmount.Text) > 0.0 && ((UltraGridBase)cmbtaxtype).ActiveRow == null)
			{
				MessageBox.Show("Tax Name is Required");
				cmbtaxtype.Focus();
				return false;
			}
			return true;
		}
		return true;
	}

	private void DisableFields()
	{
		try
		{
			((Control)(object)comItem).Enabled = false;
			((Control)(object)comjobLot).Enabled = false;
			txtQty.Enabled = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void EnablesFields()
	{
		try
		{
			((Control)(object)comItem).Enabled = true;
			((Control)(object)comjobLot).Enabled = true;
			txtQty.Enabled = true;
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
			lstRemoveDetailRecord.Clear();
			btnDelete.Visible = false;
			RecId = 0;
			RowVersionId = 0L;
			Approved = false;
			((Control)(object)comsupplier).Text = string.Empty;
			txtReferenceNo.Text = string.Empty;
			((Control)(object)combcommAgent).Text = string.Empty;
			((Control)(object)combCommType).Text = string.Empty;
			txtOrderQty.Text = string.Empty;
			txtOrderWeight.Text = string.Empty;
			txtOrderAmount.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtCommissionRemarks.Text = string.Empty;
			txtcommrate.Text = string.Empty;
			txtcommamount.Text = string.Empty;
			((Control)(object)combcommUOM).Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtduedays.Text = "0";
			txtDeliverydays.Text = "0";
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
			txtVarientEquivalent.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)cmbtaxtype).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			DocumentNo();
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			dtDetail.Rows.Clear();
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSettings();
			btnAdd.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ConfigurationDefault();
			DocDate.Focus();
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
			((Control)(object)comItem).Text = string.Empty;
			((Control)(object)CmbAttributeVarient).Text = string.Empty;
			txtVarientEquivalent.Text = string.Empty;
			txtQty.Text = string.Empty;
			txtItemWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtRate.Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)cmbtaxtype).Text = string.Empty;
			txtTaxPercent.Text = string.Empty;
			txtTaxAmount.Text = string.Empty;
			txtTotalAmount.Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			comItem.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OptionResetFields()
	{
		((Control)(object)comItem).Text = string.Empty;
		((Control)(object)comjobLot).Text = string.Empty;
		((Control)(object)txtCity).Text = string.Empty;
		comItem.Focus();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			Project();
			braches();
			suppliercustomer();
			PaymentTerms();
			CurrencyFill();
			item();
			JobLotBind();
			BinCity();
			GetallTaxType();
			ConfigurationDefault();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void InvfrmInvSaleInvoiceDirect_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Insert()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_043a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Expected O, but got Unknown
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Expected O, but got Unknown
		try
		{
			if (grd.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record Not Found");
			}
			if (!FormValidation())
			{
				return;
			}
			if (Conversion.ToDecimal((object)txtcommamount.Text.Trim()) > 0m && Conversion.ToInt(combcommAgent.Value) == 0)
			{
				MessageBox.Show("Please Select Commission Agent Account First");
				combcommAgent.Focus();
				return;
			}
			PurchaseOrder obj = new PurchaseOrder();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
				obj.RowVersion = RowVersionId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			txtExchangeRate_TextChanged(null, null);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1863;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.IsApproved = false;
			obj.DocDate = DocDate.Value;
			obj.DocNo = Conversion.ToInt((object)txtdocno.Text);
			obj.ReferenceNo = txtReferenceNo.Text.Trim();
			obj.SupplierCustomerId = Conversion.ToInt(comsupplier.Value);
			obj.RemarksHeader = txtremarks.Text.Trim();
			obj.PaymentTermsId = Conversion.ToInt(combpttrm.Value);
			obj.DueDate = duedate.Value;
			obj.DueDays = Conversion.ToInt((object)txtduedays.Text);
			obj.DeliveryTerm = Conversion.ToString((object)((Control)(object)combdeliverytrm).Text);
			obj.DeliveryStartDate = Conversion.ToDateTime((object)txtDeliveryStartDate.Value);
			obj.DeliveryDays = Conversion.ToInt((object)txtDeliverydays.Text);
			obj.ExpiryDate = Conversion.ToDateTime((object)txtDeliveryStartDate.Value).AddDays(Conversion.ToInt((object)txtDeliverydays.Text));
			obj.CommissionAgentId = Conversion.ToInt(combcommAgent.Value);
			obj.CommissionType = ((Control)(object)combCommType).Text.Trim();
			obj.CommissionRate = Conversion.ToDecimal((object)txtcommrate.Text);
			obj.CommissionUom = Conversion.ToDecimal((object)((Control)(object)combcommUOM).Text);
			obj.CommissionAmount = Conversion.ToDecimal((object)txtcommamount.Text);
			obj.CommissionRemarks = txtCommissionRemarks.Text.Trim();
			obj.OrderQty = Conversion.ToDecimal((object)txtOrderQty.Text);
			obj.OrderWeight = Conversion.ToDecimal((object)txtOrderWeight.Text);
			obj.OrderAmount = Conversion.ToDecimal((object)txtOrderAmount.Text);
			obj.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
			obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
			obj.FcyAmount = Conversion.ToDecimal((object)txtFcyAmount.Text);
			obj.PurchaseOrderDetaillist = new List<PurchaseOrderDetail>();
			obj.AttachmentsList = new List<DMSAttachments>();
			decimal GrossWt = default(decimal);
			if (lstRemoveDetailRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveDetailRecord.Count; j++)
				{
					obj.PurchaseOrderDetaillist.Add(lstRemoveDetailRecord[j]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				PurchaseOrderDetail detail = new PurchaseOrderDetail();
				detail.IsApproved = false;
				detail.Id = ((RecId > 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				detail.ActionTypeId = ((detail.Id <= 0) ? 1 : 2);
				detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
				detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
				detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
				detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
				detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
				GrossWt += detail.ItemNetWeight;
				detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
				detail.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
				detail.TaxTypeId = Conversion.ToInt(r.Cells["TaxId"].Value);
				detail.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
				detail.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
				detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
				detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
				detail.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
				detail.ItemNetAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
				detail.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
				obj.PurchaseOrderDetaillist.Add(detail);
			}
			if (obj.PurchaseOrderDetaillist.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
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
			int sucess = PurchaseOrder.Save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Saved Successfully [" + obj.DocNo + "] ");
			}
			else
			{
				MessageBox.Show("Record Update Successfully [" + obj.DocNo + "] ");
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
			Reset();
			if (ChkPrintslip.Checked)
			{
				GenerateOrderSlip(sucess);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnSave_Click(object sender, EventArgs e)
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

	public void ReadById_Update(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			PurchaseOrder invPI = PurchaseOrder.GetByID(RecId);
			if (invPI == null)
			{
				return;
			}
			tabControl1.SelectedIndex = 0;
			RowVersionId = invPI.RowVersion;
			btnDelete.Visible = true;
			DocDate.Value = invPI.DocDate;
			txtdocno.Text = invPI.DocNo.ToString();
			comsupplier.Value = invPI.SupplierCustomerId;
			txtReferenceNo.Text = invPI.ReferenceNo;
			combpttrm.Value = invPI.PaymentTermsId;
			txtduedays.Text = invPI.DueDays.ToString();
			if (Conversion.CheckDateTimeNull(invPI.DueDate))
			{
				duedate.Value = invPI.DocDate;
			}
			else
			{
				duedate.Value = invPI.DueDate;
			}
			((Control)(object)combdeliverytrm).Text = invPI.DeliveryTerm;
			txtDeliveryStartDate.Value = invPI.DeliveryStartDate;
			txtDeliverydays.Text = invPI.DeliveryDays.ToString();
			combcommAgent.Value = invPI.CommissionAgentId;
			((Control)(object)combCommType).Text = invPI.CommissionType.ToString();
			txtcommrate.Text = invPI.CommissionRate.ToString("#,##0.####");
			((Control)(object)combcommUOM).Text = invPI.CommissionUom.ToString();
			txtcommamount.Text = invPI.CommissionAmount.ToString("#,##0.###");
			txtCommissionRemarks.Text = invPI.CommissionRemarks;
			txtOrderQty.Text = invPI.OrderQty.ToString("#,##0.####");
			txtOrderWeight.Text = invPI.OrderWeight.ToString("#,##0.####");
			txtOrderAmount.Text = invPI.OrderAmount.ToString("#,##0.####");
			txtExchangeRate.Text = invPI.ExchangeRate.ToString(clsGlobalVariables.DecimalFCYRateFormate);
			cmbCurrency.Value = invPI.CurrencyId;
			txtFcyAmount.Text = invPI.FcyAmount.ToString("#,##0.####");
			txtremarks.Text = invPI.RemarksHeader;
			combranches.Value = invPI.BranchesId;
			comproject.Value = invPI.ProjectsId;
			Approved = invPI.IsApproved;
			if (invPI.CurrencyId > 0)
			{
				txtExchangeRate.Text = Conversion.ToString((object)invPI.ExchangeRate);
			}
			else if (invPI.CurrencyId == 0)
			{
				ConfigurationDefault();
			}
			dtDetail.Rows.Clear();
			for (int i = 0; i < invPI.PurchaseOrderDetaillist.Count; i++)
			{
				dtDetail.Rows.Add(invPI.PurchaseOrderDetaillist[i].Id, invPI.PurchaseOrderDetaillist[i].ItemId, invPI.PurchaseOrderDetaillist[i].ItemName, invPI.PurchaseOrderDetaillist[i].ItemCodeNew, invPI.PurchaseOrderDetaillist[i].ItemAttributeVarientId, invPI.PurchaseOrderDetaillist[i].ItemAttributeVarient, invPI.PurchaseOrderDetaillist[i].VarientEquivalent, invPI.PurchaseOrderDetaillist[i].ItemQty, invPI.PurchaseOrderDetaillist[i].ItemWeight, invPI.PurchaseOrderDetaillist[i].ItemNetWeight, invPI.PurchaseOrderDetaillist[i].ItemRate, invPI.PurchaseOrderDetaillist[i].ItemAmount, invPI.PurchaseOrderDetaillist[i].TaxTypeId, invPI.PurchaseOrderDetaillist[i].TaxName, invPI.PurchaseOrderDetaillist[i].TaxPercent, invPI.PurchaseOrderDetaillist[i].TaxAmount, invPI.PurchaseOrderDetaillist[i].ItemNetAmount, invPI.PurchaseOrderDetaillist[i].FcyAmount, invPI.PurchaseOrderDetaillist[i].JobLotId, invPI.PurchaseOrderDetaillist[i].JobLotDescription, invPI.PurchaseOrderDetaillist[i].CityId, invPI.PurchaseOrderDetaillist[i].CityName, invPI.PurchaseOrderDetaillist[i].RemarksDetail);
			}
			grd.DataSource = dtDetail;
			grd.RetrieveStructure();
			grdSettings();
			List<DMSAttachments> byID = DMSAttachments.GetByID(RecId, base.Name);
			AT = new Attachment();
			foreach (DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = true;
			BillAmount();
			txtExchangeRate_TextChanged(null, null);
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
			if (RecId == 0)
			{
				throw new Exception("Record Id Not Found....");
			}
			if (Approved)
			{
				throw new Exception("Record Not Delete beacause Record has approved");
			}
			if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				PurchaseOrder.DeleteById(UserAccount.ID, RecId);
				MessageBox.Show("Delete Record Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (Approved)
			{
				throw new Exception("Record Not Update beacause Record has approved");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryComboBind()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbCustomerHistory.Value);
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtCustomer = new DataTable
			{
				Columns = 
				{
					{
						"Id",
						typeof(int)
					},
					"CustomerName"
				}
			};
			DataTable dt = PurchaseOrder.GetDataForDropDownFromPurchaseOrder(val);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Supplier")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			DDL.BindDDL(dtCustomer, CmbCustomerHistory, "Id", "CustomerName", "Supplier Name", ZeroIndex: true);
			if (Id > 0)
			{
				if (dtCustomer.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbCustomerHistory.Value = Id;
				}
				else
				{
					((Control)(object)CmbCustomerHistory).Text = string.Empty;
				}
			}
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
			DDL.BindDDL(dt, cmbDateTypeHistory, "Id", "Parameters", "Parameters", ZeroIndex: false);
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
				DateTime now = DateTime.UtcNow;
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

	private void btnshow_Click(object sender, EventArgs e)
	{
		try
		{
			FillHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters PI = new ReportsParameters();
			PI.OrganizationId = UserAccount.OrganizationId;
			PI.CompanyId = UserAccount.CompanyId;
			PI.BranchesId = UserAccount.BranchesId;
			PI.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PI.DocumentTypeId = 1863;
			PI.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			PI.FromDate = Conversion.ToDateTime((object)txtFromdateHistory.Value);
			PI.ToDate = Conversion.ToDateTime((object)txtToDateHistory.Value);
			PI.FromDocNo = Conversion.ToInt((object)txtFromNoHistory.Text);
			PI.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			PI.SupplierCustomerId = Conversion.ToInt(CmbCustomerHistory.Value);
			if (!PI.CanViewAllRecord)
			{
				PI.EntryUser = UserAccount.ID;
			}
			DataTable dthistory = PurchaseOrder.FormHistory(PI);
			if (dthistory.Rows.Count > 0)
			{
				DataTable dtSales = new DataTable();
				dtSales.Columns.Add("Id", typeof(int));
				dtSales.Columns.Add("DocNo", typeof(string));
				dtSales.Columns.Add("DocDate", typeof(DateTime));
				dtSales.Columns.Add("SupplierName", typeof(string));
				dtSales.Columns.Add("ReferenceNo", typeof(string));
				dtSales.Columns.Add("PaymentTerm", typeof(string));
				dtSales.Columns.Add("DueDays", typeof(int));
				dtSales.Columns.Add("DueDate", typeof(DateTime));
				dtSales.Columns.Add("DeliveryTerm", typeof(string));
				dtSales.Columns.Add("DeliveryStartDate", typeof(DateTime));
				dtSales.Columns.Add("OrderQty", typeof(int));
				dtSales.Columns.Add("OrderWeight", typeof(decimal));
				dtSales.Columns.Add("OrderAmount", typeof(decimal));
				dtSales.Columns.Add("CommissionAgent", typeof(string));
				dtSales.Columns.Add("CommRate", typeof(decimal));
				dtSales.Columns.Add("CommAmount", typeof(decimal));
				dtSales.Columns.Add("FcyCode", typeof(string));
				dtSales.Columns.Add("ExchangeRate", typeof(decimal));
				dtSales.Columns.Add("FcyAmount", typeof(decimal));
				dtSales.Columns.Add("IsApproved", typeof(bool));
				dtSales.Columns.Add("EntryDate", typeof(DateTime));
				dtSales.Columns.Add("EntryUser", typeof(string));
				dtSales.Columns.Add("NoOfAttachments", typeof(int));
				dtSales.Rows.Clear();
				foreach (DataRow row in dthistory.Rows)
				{
					dtSales.Rows.Add(row["Id"], row["DocNo"], row["DocDate"], row["SupplierName"], row["ReferenceNo"], row["TermsDescription"], row["DueDays"], Conversion.ToDateTime(row["DueDate"]).ToShortDateString(), row["DeliveryTerm"], Conversion.ToDateTime(row["DeliveryStartDate"]).ToShortDateString(), row["OrderQty"], row["OrderWeight"], row["OrderAmount"], row["CommissionAgent"], row["CommissionRate"], row["CommissionAmount"], row["CurrencyName"], row["ExchangeRate"], row["FcyAmount"], row["IsApproved"], row["EntryDate"], row["EntryUser"], row["NoOfAttachments"]);
				}
				grdHistory.DataSource = dtSales;
				grdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdHistory.ClearStructure();
				grdDetailHistory.ClearStructure();
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
			grdHistory.AllowEdit = (InheritableBoolean)2;
			grdHistory.AllowDelete = (InheritableBoolean)2;
			grdHistory.RootTable.Columns["Id"].Visible = false;
			grdHistory.RootTable.Columns["IsApproved"].Visible = false;
			grdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			if (HasMultiCurrencyFeature)
			{
				grdHistory.RootTable.Columns["FcyCode"].Visible = true;
				grdHistory.RootTable.Columns["ExchangeRate"].Visible = true;
				grdHistory.RootTable.Columns["FcyAmount"].Visible = true;
			}
			else
			{
				grdHistory.RootTable.Columns["FcyCode"].Visible = false;
				grdHistory.RootTable.Columns["ExchangeRate"].Visible = false;
				grdHistory.RootTable.Columns["FcyAmount"].Visible = false;
			}
			grdHistory.RootTable.Columns["OrderAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["OrderAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["OrderAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["OrderAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommRate"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommRate"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["CommAmount"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["CommAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["CommAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdHistory.RootTable.Columns["OrderQty"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["OrderQty"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["OrderQty"].FormatString = "##,##.##";
			grdHistory.RootTable.Columns["OrderQty"].TotalFormatString = "##,##.##";
			grdHistory.RootTable.Columns["OrderWeight"].AggregateFunction = (AggregateFunction)2;
			grdHistory.RootTable.Columns["OrderWeight"].TextAlignment = (TextAlignment)3;
			grdHistory.RootTable.Columns["OrderWeight"].FormatString = "##,##.##";
			grdHistory.RootTable.Columns["OrderWeight"].TotalFormatString = "##,##.##";
			CommonServices.GridAutoAdjustmentNew(grdHistory);
			grdHistory.RootTable.Columns.Add("Edit");
			grdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdHistory.RootTable.Columns["Edit"].Width = 40;
			grdHistory.RootTable.Columns.Add("SaveAs");
			grdHistory.RootTable.Columns["SaveAs"].Caption = "SaveAs";
			grdHistory.RootTable.Columns["SaveAs"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["SaveAs"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["SaveAs"].ButtonText = "SaveAs";
			grdHistory.RootTable.Columns["SaveAs"].Width = 60;
			grdHistory.RootTable.Columns.Add("Print");
			grdHistory.RootTable.Columns["Print"].Caption = "Print-1863";
			grdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdHistory.RootTable.Columns["Print"].ButtonText = "Print-1863";
			grdHistory.RootTable.Columns["Print"].Width = 80;
			grdHistory.RootTable.Columns["Edit"].Position = 0;
			grdHistory.RootTable.Columns["SaveAs"].Position = 1;
			grdHistory.RootTable.Columns["Print"].Position = 2;
			grdHistory.FrozenColumns = 3;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			cmbDateTypeHistory.Focus();
		}
	}

	private void grdHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value);
			ReadById_Update(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1863);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int RId = Conversion.ToInt(item.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					Reset();
					ReadById_Update(RId);
				}
				else if (e.Column.Key == "Print")
				{
					GenerateOrderSlip(RId);
				}
				else if (e.Column.Key == "SaveAs")
				{
					Reset();
					ReadById_Update(RId);
					btnUpdate.Visible = false;
					btnSaveAs.Visible = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdHistory_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			GetDetailGrdByHeadId();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		try
		{
			if (grdHistory.CurrentRow == null || (int)grdHistory.CurrentRow.RowType != 0 || grdHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			PurchaseOrder invPI = new PurchaseOrder();
			invPI = PurchaseOrder.GetByID(Conversion.ToInt(grdHistory.CurrentRow.Cells["Id"].Value));
			if (invPI != null && invPI.PurchaseOrderDetaillist != null && invPI.PurchaseOrderDetaillist.Count > 0)
			{
				DataTable dtdetail = new DataTable();
				dtdetail.Columns.Add("ItemName", typeof(string));
				dtdetail.Columns.Add("ItemCode", typeof(string));
				dtdetail.Columns.Add("AttributeVarient", typeof(string));
				dtdetail.Columns.Add("VarientUnit", typeof(decimal));
				dtdetail.Columns.Add("ItemQty", typeof(decimal));
				dtdetail.Columns.Add("ItemWeight", typeof(decimal));
				dtdetail.Columns.Add("NetWeight", typeof(decimal));
				dtdetail.Columns.Add("ItemRate", typeof(decimal));
				dtdetail.Columns.Add("ItemAmount", typeof(decimal));
				dtdetail.Columns.Add("TaxName", typeof(string));
				dtdetail.Columns.Add("Tax%", typeof(decimal));
				dtdetail.Columns.Add("TaxAmount", typeof(decimal));
				dtdetail.Columns.Add("TotalAmount", typeof(decimal));
				dtdetail.Columns.Add("FcyAmount", typeof(decimal));
				dtdetail.Columns.Add("JobLot", typeof(string));
				dtdetail.Columns.Add("CityName");
				dtdetail.Columns.Add("Remarks");
				dtdetail.Rows.Clear();
				for (int i = 0; i < invPI.PurchaseOrderDetaillist.Count; i++)
				{
					dtdetail.Rows.Add(invPI.PurchaseOrderDetaillist[i].ItemName, invPI.PurchaseOrderDetaillist[i].ItemCodeNew, invPI.PurchaseOrderDetaillist[i].ItemAttributeVarient, invPI.PurchaseOrderDetaillist[i].VarientEquivalent, invPI.PurchaseOrderDetaillist[i].ItemQty, invPI.PurchaseOrderDetaillist[i].ItemWeight, invPI.PurchaseOrderDetaillist[i].ItemNetWeight, invPI.PurchaseOrderDetaillist[i].ItemRate, invPI.PurchaseOrderDetaillist[i].ItemAmount, invPI.PurchaseOrderDetaillist[i].TaxName, invPI.PurchaseOrderDetaillist[i].TaxPercent, invPI.PurchaseOrderDetaillist[i].TaxAmount, invPI.PurchaseOrderDetaillist[i].ItemNetAmount, invPI.PurchaseOrderDetaillist[i].FcyAmount, invPI.PurchaseOrderDetaillist[i].JobLotDescription, invPI.PurchaseOrderDetaillist[i].CityName, invPI.PurchaseOrderDetaillist[i].RemarksDetail);
				}
				grdDetailHistory.DataSource = dtdetail;
				grdDetailHistory.RetrieveStructure();
				DetailGridSetting();
			}
			else
			{
				grdDetailHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSetting()
	{
		try
		{
			grdDetailHistory.RootTable.Columns["FcyAmount"].Visible = HasMultiCurrencyFeature;
			grdDetailHistory.RootTable.Columns["VarientUnit"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["VarientUnit"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["VarientUnit"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["VarientUnit"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemQty"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemQty"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemQty"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemQty"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemWeight"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemWeight"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemWeight"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["NetWeight"].FormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.##";
			grdDetailHistory.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
			grdDetailHistory.RootTable.Columns["Tax%"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["Tax%"].FormatString = "#,##0.###";
			grdDetailHistory.RootTable.Columns["TaxAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["TaxAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["TaxAmount"].FormatString = "#,##0.####";
			grdDetailHistory.RootTable.Columns["TaxAmount"].TotalFormatString = "#,##0.####";
			grdDetailHistory.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["ItemAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["ItemAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["ItemAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetailHistory.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdDetailHistory.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetailHistory.RootTable.Columns["TotalAmount"].TextAlignment = (TextAlignment)3;
			grdDetailHistory.RootTable.Columns["TotalAmount"].FormatString = clsGlobalVariables.stringFormatsingle;
			grdDetailHistory.RootTable.Columns["TotalAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
			CommonServices.GridAutoAdjustmentNew(grdDetailHistory);
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Resethistory()
	{
		try
		{
			cmbDateTypeHistory.Value = 3;
			txtFromNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			((Control)(object)CmbCustomerHistory).Text = string.Empty;
			cmbDateTypeHistory.Focus();
			grdHistory.ClearStructure();
			grdDetailHistory.ClearStructure();
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
			Resethistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryComboBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSlip_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				GenerateOrderSlip(RecId);
				return;
			}
			throw new Exception("No Record Found For Display");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateOrderSlip(int PrintId)
	{
		try
		{
			CommonServices.PurchaseOrderSlipConcrete(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
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

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
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
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetailHistory).Name);
			if (fs != null)
			{
				grdDetailHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateDetailAmount()
	{
		try
		{
			decimal qty = default(decimal);
			decimal amount = default(decimal);
			decimal unit = default(decimal);
			decimal ItemWeight = default(decimal);
			decimal NetWeight = default(decimal);
			qty = Conversion.ToDecimal((object)txtQty.Text);
			unit = Conversion.ToDecimal((object)txtVarientEquivalent.Text);
			ItemWeight = Conversion.ToDecimal((object)txtItemWeight.Text);
			decimal Rate = ((Conversion.ToDecimal((object)txtRate.Text) > 0m) ? Conversion.ToDecimal((object)txtRate.Text) : 0m);
			if (unit > 0m && qty > 0m)
			{
				NetWeight = Conversion.ToDecimal((object)(unit * qty * ItemWeight));
				txtNetWeight.Text = NetWeight.ToString("##,#.##");
				amount = Conversion.ToDecimal((object)(unit * qty * Rate));
				txtAmount.Text = amount.ToString("#,##0.####");
				txtTotalAmount.Text = amount.ToString("#,##0.####");
			}
			else
			{
				txtAmount.Text = 0.ToString();
				txtTotalAmount.Text = 0.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void BillAmount()
	{
		try
		{
			if (grd.GetRows().Count() > 0)
			{
				decimal TotalQty = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["ItemQty"], (AggregateFunction)2));
				decimal TotalWeight = Conversion.ToDecimal(grd.GetTotal(grd.RootTable.Columns["NetWeight"], (AggregateFunction)2));
				decimal TotalAmount = Conversion.ToDecimal((object)grd.GetTotal(grd.RootTable.Columns["TotalAmount"], (AggregateFunction)2).ToString());
				txtOrderQty.Text = Math.Round(TotalQty, 2).ToString("#,##0.##");
				txtOrderWeight.Text = Math.Round(TotalWeight, 3).ToString("#,##0.###");
				txtOrderAmount.Text = Math.Round(TotalAmount, 4).ToString("#,##0.####");
				if (Conversion.ToDecimal((object)txtExchangeRate.Text) > 0m && TotalAmount > 0m)
				{
					txtFcyAmount.Text = (TotalAmount / Conversion.ToDecimal((object)txtExchangeRate.Text)).ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
				else
				{
					txtFcyAmount.Text = 0.ToString(clsGlobalVariables.stringFormatsingleForFcy);
				}
			}
			else
			{
				txtOrderQty.Text = "0";
				txtOrderWeight.Text = "0";
				txtOrderAmount.Text = "0";
				txtFcyAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void comsupplier_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDetailAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateDetailAmount();
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
			CalculateDetailAmount();
			CalculateTaxAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TotalCommissionAmount()
	{
		try
		{
			if (txtcommrate.Text != string.Empty)
			{
				if (((Control)(object)combCommType).Text == "Flat")
				{
					decimal CommRate2 = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
					txtcommamount.Text = Math.Round(CommRate2, clsGlobalVariables.DefaultNoofDecimalPointsForAmount, MidpointRounding.AwayFromZero).ToString(clsGlobalVariables.stringFormatsingle);
				}
				if (((Control)(object)combCommType).Text == "Percent" || ((Control)(object)combCommType).Text == "Percentage")
				{
					decimal CommRate = Conversion.ToDecimal((object)txtcommrate.Text.Trim());
					object total = grd.GetTotal(grd.RootTable.Columns["ItemAmount"], (AggregateFunction)2);
					if (total != null)
					{
						decimal CommAmt = Conversion.ToDecimal((object)total.ToString()) * CommRate / 100m;
						txtcommamount.Text = CommAmt.ToString("#,##0.###");
					}
				}
				if (!(((Control)(object)combCommType).Text == "Comm Weight"))
				{
				}
			}
			else
			{
				txtcommamount.Text = 0.ToString("#,##0.###");
			}
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
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommUOM_Leave_1(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combCommType_Leave_1(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combcommAgent_Leave(object sender, EventArgs e)
	{
		try
		{
			TotalCommissionAmount();
			txtExchangeRate_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbtaxtype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtTaxPercent_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				CalculateTaxAmount();
				return;
			}
			txtTaxPercent.Text = "";
			txtTaxAmount.Text = "";
			txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CalculateTaxAmount()
	{
		try
		{
			if (((UltraGridBase)cmbtaxtype).ActiveRow != null)
			{
				double TaxPercent = 0.0;
				if (!(Conversion.ToDouble((object)txtTaxPercent.Text) > 0.0))
				{
					txtTaxPercent.Text = Conversion.ToDouble(((UltraDropDownBase)cmbtaxtype).SelectedRow.Cells[2].Value).ToString("#,##0.###");
					TaxPercent = Conversion.ToDouble(((UltraDropDownBase)cmbtaxtype).SelectedRow.Cells[2].Value);
				}
				else
				{
					TaxPercent = Conversion.ToDouble((object)txtTaxPercent.Text);
				}
				if (TaxPercent > 100.0)
				{
					MessageBox.Show("Tax % Can't be greater than 100...");
					TaxPercent = 100.0;
					txtTaxPercent.Text = TaxPercent.ToString();
				}
				double num = Conversion.ToDouble((object)txtAmount.Text);
				double TaxAmt = num * TaxPercent / 100.0;
				txtTaxAmount.Text = TaxAmt.ToString(clsGlobalVariables.stringFormatsingle);
				double TotalAmount = num + TaxAmt;
				txtTotalAmount.Text = TotalAmount.ToString(clsGlobalVariables.stringFormatsingle);
			}
			else
			{
				txtTotalAmount.Text = Conversion.ToDouble((object)txtAmount.Text).ToString(clsGlobalVariables.stringFormatsingle);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_Leave(object sender, EventArgs e)
	{
		try
		{
			GetallTaxType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtduedays_TextChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DueDateGenerate()
	{
		try
		{
			if (txtduedays.Text.Trim() != string.Empty && txtduedays.Text != null)
			{
				duedate.Value = DocDate.Value.AddDays(Conversion.ToDouble((object)txtduedays.Text.Trim()));
			}
			else
			{
				duedate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combdeliverytrm_TextChanged(object sender, EventArgs e)
	{
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			DueDateGenerate();
			if (txtDeliveryStartDate.Value.Date < DocDate.Value.Date)
			{
				txtDeliveryStartDate.Value = DocDate.Value;
			}
			GetallTaxType();
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
			CommonServices.OnlytextdecimelFunction(sender, e);
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

	private void txtRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtduedays_KeyPress(object sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
	}

	private void txtExchangeRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void InvfrmPurchasedirectInvoice_KeyDown_1(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					BtnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down && tabControl2.SelectedIndex == 0)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					comItem.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.F12)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Alt && e.KeyCode == Keys.P)
				{
					btnSlip_Click(null, null);
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnLoadAll_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					BtnRefreshHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbDateTypeHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdHistory).Focus();
				}
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
					cmbDateTypeHistory.Focus();
				}
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
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Customer Name");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("F12", "For Save As");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+Delete", "For Deleting a row of Delete Grid when focused");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Name in Detail Box");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_0686: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					GridEXRow r2 = grd.CurrentRow;
					if (Conversion.ToInt(r2.Cells["Id"].Value) > 0)
					{
						if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
						{
							return;
						}
						PurchaseOrderDetail detail2 = new PurchaseOrderDetail();
						detail2.Id = Conversion.ToInt(r2.Cells["Id"].Value);
						detail2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
						detail2.ItemAttributeVarientId = Conversion.ToInt(r2.Cells["AttributeVarientId"].Value);
						detail2.VarientEquivalent = Conversion.ToDecimal(r2.Cells["VarientUnit"].Value);
						detail2.ItemQty = Conversion.ToDecimal(r2.Cells["ItemQty"].Value);
						detail2.ItemWeight = Conversion.ToDecimal(r2.Cells["ItemWeight"].Value);
						detail2.ItemNetWeight = Conversion.ToDecimal(r2.Cells["NetWeight"].Value);
						detail2.ItemRate = Conversion.ToDecimal(r2.Cells["Rate"].Value);
						detail2.ItemAmount = Conversion.ToDecimal(r2.Cells["ItemAmount"].Value);
						detail2.TaxTypeId = Conversion.ToInt(r2.Cells["TaxId"].Value);
						detail2.TaxPercent = Conversion.ToDecimal(r2.Cells["Tax%"].Value);
						detail2.TaxAmount = Conversion.ToDecimal(r2.Cells["TaxAmount"].Value);
						detail2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
						detail2.CityId = Conversion.ToInt(r2.Cells["CityId"].Value);
						detail2.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
						detail2.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
						detail2.FcyAmount = Conversion.ToDecimal(r2.Cells["FcyAmount"].Value);
						detail2.ItemNetAmount = Conversion.ToDecimal(r2.Cells["TotalAmount"].Value);
						detail2.RemarksDetail = Conversion.ToString(r2.Cells["Remarks"].Value);
						detail2.ActionTypeId = 3;
						lstRemoveDetailRecord.Add(detail2);
						dtDetail.Rows[r2.RowIndex].Delete();
						if (r2 != null && (int)r2.RowType == 0)
						{
							r2.Delete();
						}
					}
					else if (r2 != null && (int)r2.RowType == 0)
					{
						r2.Delete();
					}
					grd.UpdateData();
					TotalCommissionAmount();
					txtExchangeRate_TextChanged(null, null);
					BillAmount();
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					grd_DoubleClick(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				GridEXRow r = grd.CurrentRow;
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					PurchaseOrderDetail detail = new PurchaseOrderDetail();
					detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					detail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					detail.ItemAttributeVarientId = Conversion.ToInt(r.Cells["AttributeVarientId"].Value);
					detail.VarientEquivalent = Conversion.ToDecimal(r.Cells["VarientUnit"].Value);
					detail.ItemQty = Conversion.ToDecimal(r.Cells["ItemQty"].Value);
					detail.ItemWeight = Conversion.ToDecimal(r.Cells["ItemWeight"].Value);
					detail.ItemNetWeight = Conversion.ToDecimal(r.Cells["NetWeight"].Value);
					detail.ItemRate = Conversion.ToDecimal(r.Cells["Rate"].Value);
					detail.ItemAmount = Conversion.ToDecimal(r.Cells["ItemAmount"].Value);
					detail.TaxTypeId = Conversion.ToInt(r.Cells["TaxId"].Value);
					detail.TaxPercent = Conversion.ToDecimal(r.Cells["Tax%"].Value);
					detail.TaxAmount = Conversion.ToDecimal(r.Cells["TaxAmount"].Value);
					detail.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
					detail.CityId = Conversion.ToInt(r.Cells["CityId"].Value);
					detail.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
					detail.CurrencyId = Conversion.ToInt(cmbCurrency.Value);
					detail.FcyAmount = Conversion.ToDecimal(r.Cells["FcyAmount"].Value);
					detail.ItemNetAmount = Conversion.ToDecimal(r.Cells["TotalAmount"].Value);
					detail.RemarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtDetail.Rows[r.RowIndex].Delete();
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
				TotalCommissionAmount();
				txtExchangeRate_TextChanged(null, null);
				BillAmount();
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdHistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdHistory.CurrentColumn.Key == "Edit")
				{
					if (formright.DoHaveUpdateRights)
					{
						ReadById_Update(CurrId);
						comsupplier.Focus();
					}
				}
				else if (grdHistory.CurrentColumn.Key == "Print")
				{
					if (formright.DoHavePrintRights)
					{
						GenerateOrderSlip(CurrId);
					}
				}
				else if (grdHistory.CurrentColumn.Key == "SaveAs")
				{
					if (!formright.DoHaveSaveRight)
					{
						throw new Exception("You don't have Right");
					}
					Reset();
					ReadById_Update(CurrId);
					btnUpdate.Visible = false;
					btnSaveAs.Visible = true;
				}
				else if (grdHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1863);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formright.DoHaveUpdateRights)
			{
				ReadById_Update(CurrId);
				comsupplier.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combpttrm_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)combpttrm).Text == "Cash")
			{
				txtduedays.Enabled = false;
				duedate.Enabled = false;
				txtduedays.Text = string.Empty;
				duedate.Value = DocDate.Value;
			}
			else
			{
				txtduedays.Enabled = true;
				duedate.Enabled = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDeliveryStartDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (txtDeliveryStartDate.Value.Date < DocDate.Value.Date)
			{
				txtDeliveryStartDate.Value = DocDate.Value;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void duedate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (duedate.Value.Date < DocDate.Value.Date)
			{
				duedate.Value = DocDate.Value;
				throw new Exception("Due Date Can't less Than DocDate");
			}
			int days = (duedate.Value.Date - DocDate.Value.Date).Days;
			txtduedays.Text = days.ToString();
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
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0588: Expected O, but got Unknown
		//IL_05c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ca: Expected O, but got Unknown
		//IL_0618: Unknown result type (might be due to invalid IL or missing references)
		//IL_0622: Expected O, but got Unknown
		//IL_064f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0659: Expected O, but got Unknown
		//IL_06a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b1: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_0740: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e2: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ed: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0924: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097c: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0987: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099d: Expected O, but got Unknown
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6e: Expected O, but got Unknown
		//IL_0aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab0: Expected O, but got Unknown
		//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af2: Expected O, but got Unknown
		//IL_0afe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b08: Expected O, but got Unknown
		//IL_1122: Unknown result type (might be due to invalid IL or missing references)
		//IL_112c: Expected O, but got Unknown
		//IL_8dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e06: Expected O, but got Unknown
		//IL_8e2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_8e34: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance Appearance1 = new Appearance();
		Appearance Appearance5 = new Appearance();
		Appearance Appearance6 = new Appearance();
		Appearance Appearance7 = new Appearance();
		Appearance Appearance8 = new Appearance();
		Appearance Appearance9 = new Appearance();
		Appearance Appearance10 = new Appearance();
		Appearance Appearance11 = new Appearance();
		Appearance Appearance12 = new Appearance();
		Appearance Appearance2 = new Appearance();
		Appearance Appearance3 = new Appearance();
		Appearance Appearance4 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
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
		Appearance appearance42 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
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
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
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
		GridEXLayout grdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel16 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CurrencyInfoBox = new System.Windows.Forms.GroupBox();
		this.cmbCurrency = new UltraCombo();
		this.label61 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.txtFcyAmount = new System.Windows.Forms.TextBox();
		this.label59 = new System.Windows.Forms.Label();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.label20 = new System.Windows.Forms.Label();
		this.combcommAgent = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtCommissionRemarks = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.combCommType = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.txtcommrate = new System.Windows.Forms.TextBox();
		this.txtcommamount = new System.Windows.Forms.TextBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label24 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.label49 = new System.Windows.Forms.Label();
		this.cmbtaxtype = new UltraCombo();
		this.label50 = new System.Windows.Forms.Label();
		this.label64 = new System.Windows.Forms.Label();
		this.txtTaxAmount = new System.Windows.Forms.TextBox();
		this.txtTaxPercent = new System.Windows.Forms.TextBox();
		this.btnAdd = new System.Windows.Forms.Button();
		this.label43 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label71 = new System.Windows.Forms.Label();
		this.txtTotalAmount = new System.Windows.Forms.TextBox();
		this.CmbAttributeVarient = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.comjobLot = new UltraCombo();
		this.label33 = new System.Windows.Forms.Label();
		this.txtQty = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label38 = new System.Windows.Forms.Label();
		this.txtItemWeight = new System.Windows.Forms.TextBox();
		this.txtVarientEquivalent = new System.Windows.Forms.TextBox();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.txtCity = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.comItem = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.txtRate = new System.Windows.Forms.TextBox();
		this.label40 = new System.Windows.Forms.Label();
		this.label32 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtOrderAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtDeliverydays = new System.Windows.Forms.TextBox();
		this.label41 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtDeliveryStartDate = new System.Windows.Forms.DateTimePicker();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtOrderWeight = new System.Windows.Forms.TextBox();
		this.txtOrderQty = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtReferenceNo = new System.Windows.Forms.TextBox();
		this.combpttrm = new UltraCombo();
		this.combdeliverytrm = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.txtduedays = new System.Windows.Forms.TextBox();
		this.comsupplier = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.duedate = new System.Windows.Forms.DateTimePicker();
		this.label29 = new System.Windows.Forms.Label();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.ChkPrintslip = new System.Windows.Forms.CheckBox();
		this.ChkResetOnSave = new System.Windows.Forms.CheckBox();
		this.comproject = new UltraCombo();
		this.combranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.combcommUOM = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.label19 = new System.Windows.Forms.Label();
		this.txtSupplierGLId = new System.Windows.Forms.TextBox();
		this.txtCommAgentId = new System.Windows.Forms.TextBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnSlip = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel17 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.grdDetailHistory = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.label55 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox4 = new System.Windows.Forms.GroupBox();
		this.label15 = new System.Windows.Forms.Label();
		this.btnshow = new System.Windows.Forms.Button();
		this.label73 = new System.Windows.Forms.Label();
		this.CmbCustomerHistory = new UltraCombo();
		this.label75 = new System.Windows.Forms.Label();
		this.cmbDateTypeHistory = new UltraCombo();
		this.txtToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label76 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromdateHistory = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label77 = new System.Windows.Forms.Label();
		this.txtFromNoHistory = new System.Windows.Forms.TextBox();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label54 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.label23 = new System.Windows.Forms.Label();
		this.label78 = new System.Windows.Forms.Label();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel3.SuspendLayout();
		this.CurrencyInfoBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).BeginInit();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel17.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).BeginInit();
		this.panel18.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel5.SuspendLayout();
		this.groupBox4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).BeginInit();
		this.panel9.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1275, 720);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1267, 691);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.tabControl2);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1261, 685);
		this.panel1.TabIndex = 7;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1222, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 111;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1247, 328);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 322);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1261, 363);
		this.tabControl2.TabIndex = 33;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.panel16);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1253, 334);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(200, 100);
		this.panel16.TabIndex = 1;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.CurrencyInfoBox);
		this.panel3.Controls.Add(this.groupBox3);
		this.panel3.Controls.Add(this.groupBox1);
		this.panel3.Controls.Add(this.groupBox2);
		this.panel3.Controls.Add(this.panel11);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.comproject);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combranches);
		this.panel3.Controls.Add(this.label13);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommUOM);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Controls.Add(this.label19);
		this.panel3.Controls.Add(this.txtSupplierGLId);
		this.panel3.Controls.Add(this.txtCommAgentId);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel3.Location = new System.Drawing.Point(0, 25);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1261, 297);
		this.panel3.TabIndex = 29;
		this.CurrencyInfoBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbCurrency);
		this.CurrencyInfoBox.Controls.Add(this.label61);
		this.CurrencyInfoBox.Controls.Add(this.txtExchangeRate);
		this.CurrencyInfoBox.Controls.Add(this.label60);
		this.CurrencyInfoBox.Controls.Add(this.txtFcyAmount);
		this.CurrencyInfoBox.Controls.Add(this.label59);
		this.CurrencyInfoBox.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.CurrencyInfoBox.Location = new System.Drawing.Point(1004, 26);
		this.CurrencyInfoBox.Name = "CurrencyInfoBox";
		this.CurrencyInfoBox.Size = new System.Drawing.Size(202, 148);
		this.CurrencyInfoBox.TabIndex = 2;
		this.CurrencyInfoBox.TabStop = false;
		this.CurrencyInfoBox.Text = "Currency Information";
		this.cmbCurrency.AllowNull = (DefaultableBoolean)1;
		this.cmbCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)Appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)Appearance1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)Appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)Appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)Appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)Appearance5;
		((AppearanceBase)Appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)Appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)Appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)Appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)Appearance7;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)Appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)Appearance8;
		((AppearanceBase)Appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)Appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)Appearance9;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)Appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)Appearance10;
		((AppearanceBase)Appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)Appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)Appearance11;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)Appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)Appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)Appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)Appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)Appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)Appearance12;
		((AppearanceBase)Appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)Appearance2;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)Appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)Appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)Appearance3;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)Appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)Appearance4;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Location = new System.Drawing.Point(76, 18);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Name = "cmbCurrency";
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Size = new System.Drawing.Size(118, 24);
		((System.Windows.Forms.Control)(object)this.cmbCurrency).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbCurrency).Leave += new System.EventHandler(cmbCurrency_Leave);
		this.label61.AutoSize = true;
		this.label61.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label61.Location = new System.Drawing.Point(4, 23);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(55, 15);
		this.label61.TabIndex = 128;
		this.label61.Text = "Fcy Code";
		this.txtExchangeRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(76, 45);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.ShortcutsEnabled = false;
		this.txtExchangeRate.Size = new System.Drawing.Size(118, 23);
		this.txtExchangeRate.TabIndex = 1;
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.TextChanged += new System.EventHandler(txtExchangeRate_TextChanged);
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.txtExchangeRate.Leave += new System.EventHandler(txtExchangeRate_Leave);
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(4, 49);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(58, 15);
		this.label60.TabIndex = 127;
		this.label60.Text = "Exch Rate";
		this.txtFcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFcyAmount.Enabled = false;
		this.txtFcyAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFcyAmount.Location = new System.Drawing.Point(76, 72);
		this.txtFcyAmount.Name = "txtFcyAmount";
		this.txtFcyAmount.ShortcutsEnabled = false;
		this.txtFcyAmount.Size = new System.Drawing.Size(118, 23);
		this.txtFcyAmount.TabIndex = 2;
		this.txtFcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(4, 76);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(72, 15);
		this.label59.TabIndex = 126;
		this.label59.Text = "Fcy Amount";
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combcommAgent);
		this.groupBox3.Controls.Add(this.label26);
		this.groupBox3.Controls.Add(this.label11);
		this.groupBox3.Controls.Add(this.txtCommissionRemarks);
		this.groupBox3.Controls.Add(this.label25);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.combCommType);
		this.groupBox3.Controls.Add(this.label21);
		this.groupBox3.Controls.Add(this.txtcommrate);
		this.groupBox3.Controls.Add(this.txtcommamount);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(716, 27);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(286, 147);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Commission";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label20.Location = new System.Drawing.Point(3, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(61, 15);
		this.label20.TabIndex = 94;
		this.label20.Text = "Agent (Cr)";
		this.combcommAgent.AllowNull = (DefaultableBoolean)1;
		this.combcommAgent.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommAgent.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommAgent.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Appearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.combcommAgent).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance30).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance30).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance70).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommAgent).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommAgent).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommAgent.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Location = new System.Drawing.Point(67, 14);
		((System.Windows.Forms.Control)(object)this.combcommAgent).Name = "combcommAgent";
		((System.Windows.Forms.Control)(object)this.combcommAgent).Size = new System.Drawing.Size(212, 26);
		((System.Windows.Forms.Control)(object)this.combcommAgent).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combcommAgent).Leave += new System.EventHandler(combcommAgent_Leave);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.Location = new System.Drawing.Point(3, 74);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(51, 15);
		this.label26.TabIndex = 96;
		this.label26.Text = "Amount";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(3, 48);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(32, 15);
		this.label11.TabIndex = 92;
		this.label11.Text = "Type";
		this.txtCommissionRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommissionRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommissionRemarks.Location = new System.Drawing.Point(67, 96);
		this.txtCommissionRemarks.Name = "txtCommissionRemarks";
		this.txtCommissionRemarks.Size = new System.Drawing.Size(212, 23);
		this.txtCommissionRemarks.TabIndex = 4;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label25.Location = new System.Drawing.Point(3, 100);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(52, 15);
		this.label25.TabIndex = 95;
		this.label25.Text = "Remarks";
		this.combCommType.AllowNull = (DefaultableBoolean)1;
		this.combCommType.AutoCompleteMode = (AutoCompleteMode)4;
		this.combCommType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combCommType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combCommType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.combCommType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combCommType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance34;
		((SpecialBoxBase)((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combCommType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combCommType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combCommType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combCommType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combCommType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combCommType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combCommType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combCommType).Location = new System.Drawing.Point(67, 42);
		((System.Windows.Forms.Control)(object)this.combCommType).Name = "combCommType";
		((System.Windows.Forms.Control)(object)this.combCommType).Size = new System.Drawing.Size(84, 26);
		((System.Windows.Forms.Control)(object)this.combCommType).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.combCommType).Leave += new System.EventHandler(combCommType_Leave_1);
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(155, 48);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(30, 15);
		this.label21.TabIndex = 97;
		this.label21.Text = "Rate";
		this.txtcommrate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommrate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtcommrate.ForeColor = System.Drawing.SystemColors.WindowText;
		this.txtcommrate.Location = new System.Drawing.Point(189, 44);
		this.txtcommrate.Name = "txtcommrate";
		this.txtcommrate.Size = new System.Drawing.Size(90, 23);
		this.txtcommrate.TabIndex = 2;
		this.txtcommrate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtcommrate.TextChanged += new System.EventHandler(txtcommrate_TextChanged);
		this.txtcommrate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtcommrate_KeyPress);
		this.txtcommamount.BackColor = System.Drawing.Color.White;
		this.txtcommamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcommamount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtcommamount.Location = new System.Drawing.Point(67, 70);
		this.txtcommamount.Name = "txtcommamount";
		this.txtcommamount.ReadOnly = true;
		this.txtcommamount.Size = new System.Drawing.Size(212, 23);
		this.txtcommamount.TabIndex = 3;
		this.txtcommamount.TabStop = false;
		this.txtcommamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.groupBox1.BackColor = System.Drawing.Color.White;
		this.groupBox1.Controls.Add(this.label24);
		this.groupBox1.Controls.Add(this.txtRemarksDetail);
		this.groupBox1.Controls.Add(this.label49);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbtaxtype);
		this.groupBox1.Controls.Add(this.label50);
		this.groupBox1.Controls.Add(this.label64);
		this.groupBox1.Controls.Add(this.txtTaxAmount);
		this.groupBox1.Controls.Add(this.txtTaxPercent);
		this.groupBox1.Controls.Add(this.btnAdd);
		this.groupBox1.Controls.Add(this.label43);
		this.groupBox1.Controls.Add(this.txtNetWeight);
		this.groupBox1.Controls.Add(this.label71);
		this.groupBox1.Controls.Add(this.txtTotalAmount);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAttributeVarient);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comjobLot);
		this.groupBox1.Controls.Add(this.label33);
		this.groupBox1.Controls.Add(this.txtQty);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.label38);
		this.groupBox1.Controls.Add(this.txtItemWeight);
		this.groupBox1.Controls.Add(this.txtVarientEquivalent);
		this.groupBox1.Controls.Add(this.rdbtnItemCode);
		this.groupBox1.Controls.Add(this.rdbtnItemName);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.txtCity);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.comItem);
		this.groupBox1.Controls.Add(this.label42);
		this.groupBox1.Controls.Add(this.txtRate);
		this.groupBox1.Controls.Add(this.label40);
		this.groupBox1.Controls.Add(this.label32);
		this.groupBox1.Controls.Add(this.txtAmount);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 177);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1201, 114);
		this.groupBox1.TabIndex = 3;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(535, 66);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 150;
		this.label24.Text = "Remarks";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(536, 84);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(525, 23);
		this.txtRemarksDetail.TabIndex = 14;
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label49.Location = new System.Drawing.Point(913, 16);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(53, 15);
		this.label49.TabIndex = 116;
		this.label49.Text = "Tax Type";
		this.cmbtaxtype.AllowNull = (DefaultableBoolean)1;
		this.cmbtaxtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbtaxtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbtaxtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance107;
		((SpecialBoxBase)((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance118).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance118).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance127).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance132;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance133).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance135).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance136).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbtaxtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbtaxtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Location = new System.Drawing.Point(913, 34);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Name = "cmbtaxtype";
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Size = new System.Drawing.Size(100, 26);
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.cmbtaxtype).Leave += new System.EventHandler(cmbtaxtype_Leave);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label50.Location = new System.Drawing.Point(1015, 16);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(38, 15);
		this.label50.TabIndex = 114;
		this.label50.Text = "Tax %";
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(1083, 16);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(72, 15);
		this.label64.TabIndex = 115;
		this.label64.Text = "Tax Amount";
		this.txtTaxAmount.Enabled = false;
		this.txtTaxAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxAmount.Location = new System.Drawing.Point(1083, 36);
		this.txtTaxAmount.Name = "txtTaxAmount";
		this.txtTaxAmount.Size = new System.Drawing.Size(114, 23);
		this.txtTaxAmount.TabIndex = 10;
		this.txtTaxAmount.TabStop = false;
		this.txtTaxAmount.Text = "0";
		this.txtTaxAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtTaxPercent.Enabled = false;
		this.txtTaxPercent.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTaxPercent.Location = new System.Drawing.Point(1015, 36);
		this.txtTaxPercent.Name = "txtTaxPercent";
		this.txtTaxPercent.Size = new System.Drawing.Size(66, 23);
		this.txtTaxPercent.TabIndex = 9;
		this.txtTaxPercent.TextChanged += new System.EventHandler(txtTaxPercent_TextChanged);
		this.btnAdd.BackColor = System.Drawing.Color.Teal;
		this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnAdd.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAdd.ForeColor = System.Drawing.Color.White;
		this.btnAdd.Location = new System.Drawing.Point(1063, 82);
		this.btnAdd.Margin = new System.Windows.Forms.Padding(2);
		this.btnAdd.Name = "btnAdd";
		this.btnAdd.Size = new System.Drawing.Size(40, 27);
		this.btnAdd.TabIndex = 15;
		this.btnAdd.Text = "+";
		this.btnAdd.UseVisualStyleBackColor = false;
		this.btnAdd.Click += new System.EventHandler(btnAdd_Click);
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(625, 16);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(68, 15);
		this.label43.TabIndex = 149;
		this.label43.Text = "Net Weight";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(626, 36);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(93, 23);
		this.txtNetWeight.TabIndex = 5;
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label71.AutoSize = true;
		this.label71.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label71.Location = new System.Drawing.Point(4, 64);
		this.label71.Name = "label71";
		this.label71.Size = new System.Drawing.Size(80, 15);
		this.label71.TabIndex = 145;
		this.label71.Text = "Total Amount";
		this.txtTotalAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtTotalAmount.Location = new System.Drawing.Point(4, 84);
		this.txtTotalAmount.Name = "txtTotalAmount";
		this.txtTotalAmount.ReadOnly = true;
		this.txtTotalAmount.Size = new System.Drawing.Size(117, 23);
		this.txtTotalAmount.TabIndex = 11;
		this.txtTotalAmount.TabStop = false;
		this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.CmbAttributeVarient.AllowNull = (DefaultableBoolean)1;
		this.CmbAttributeVarient.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAttributeVarient.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAttributeVarient.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance43;
		((SpecialBoxBase)((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance75).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAttributeVarient).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAttributeVarient.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Location = new System.Drawing.Point(267, 34);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Name = "CmbAttributeVarient";
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Size = new System.Drawing.Size(101, 26);
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbAttributeVarient).Leave += new System.EventHandler(CmbAttributeVarient_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(534, 16);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(74, 15);
		this.label1.TabIndex = 131;
		this.label1.Text = "Item Weight";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(377, 16);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 132;
		this.label2.Text = "Varient Unit";
		this.comjobLot.AllowNull = (DefaultableBoolean)1;
		this.comjobLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.comjobLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comjobLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comjobLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.comjobLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comjobLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comjobLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comjobLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comjobLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comjobLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comjobLot).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comjobLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comjobLot).Location = new System.Drawing.Point(124, 82);
		((System.Windows.Forms.Control)(object)this.comjobLot).Name = "comjobLot";
		((System.Windows.Forms.Control)(object)this.comjobLot).Size = new System.Drawing.Size(183, 26);
		((System.Windows.Forms.Control)(object)this.comjobLot).TabIndex = 12;
		this.label33.AutoSize = true;
		this.label33.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label33.Location = new System.Drawing.Point(124, 64);
		this.label33.Name = "label33";
		this.label33.Size = new System.Drawing.Size(46, 15);
		this.label33.TabIndex = 37;
		this.label33.Text = "Job Lot";
		this.txtQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtQty.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtQty.Location = new System.Drawing.Point(452, 36);
		this.txtQty.Name = "txtQty";
		this.txtQty.Size = new System.Drawing.Size(82, 23);
		this.txtQty.TabIndex = 3;
		this.txtQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtQty.TextChanged += new System.EventHandler(txtQty_TextChanged);
		this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtQty_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(267, 16);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(94, 15);
		this.label9.TabIndex = 133;
		this.label9.Text = "Attribute Varient";
		this.label38.AutoSize = true;
		this.label38.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label38.Location = new System.Drawing.Point(460, 16);
		this.label38.Name = "label38";
		this.label38.Size = new System.Drawing.Size(26, 15);
		this.label38.TabIndex = 47;
		this.label38.Text = "Qty";
		this.txtItemWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtItemWeight.Enabled = false;
		this.txtItemWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtItemWeight.Location = new System.Drawing.Point(536, 36);
		this.txtItemWeight.Name = "txtItemWeight";
		this.txtItemWeight.Size = new System.Drawing.Size(88, 23);
		this.txtItemWeight.TabIndex = 4;
		this.txtVarientEquivalent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVarientEquivalent.Enabled = false;
		this.txtVarientEquivalent.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVarientEquivalent.Location = new System.Drawing.Point(369, 36);
		this.txtVarientEquivalent.Name = "txtVarientEquivalent";
		this.txtVarientEquivalent.Size = new System.Drawing.Size(82, 23);
		this.txtVarientEquivalent.TabIndex = 2;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(94, 14);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 127;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(41, 14);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 126;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.txtCity.AllowNull = (DefaultableBoolean)1;
		this.txtCity.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtCity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtCity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtCity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.txtCity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtCity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance139).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance139).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance139).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance140;
		((SpecialBoxBase)((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance141).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance141).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance141).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtCity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtCity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance143).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance144;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance145).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance146).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance146).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance146).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance146;
		((AppearanceBase)appearance102).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtCity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtCity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtCity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtCity).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtCity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtCity).Location = new System.Drawing.Point(309, 82);
		((System.Windows.Forms.Control)(object)this.txtCity).Name = "txtCity";
		((System.Windows.Forms.Control)(object)this.txtCity).Size = new System.Drawing.Size(225, 26);
		((System.Windows.Forms.Control)(object)this.txtCity).TabIndex = 13;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(309, 64);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(62, 15);
		this.label8.TabIndex = 84;
		this.label8.Text = "City Name";
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(1132, 82);
		this.btnCancelUpdateDetial.Margin = new System.Windows.Forms.Padding(2);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(66, 27);
		this.btnCancelUpdateDetial.TabIndex = 17;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(1062, 82);
		this.btnUpdateDetail.Margin = new System.Windows.Forms.Padding(2);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 27);
		this.btnUpdateDetail.TabIndex = 16;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.comItem.AllowNull = (DefaultableBoolean)1;
		this.comItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.comItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.comItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.comItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.comItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.comItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comItem).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.comItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comItem).Location = new System.Drawing.Point(4, 34);
		((System.Windows.Forms.Control)(object)this.comItem).Name = "comItem";
		((System.Windows.Forms.Control)(object)this.comItem).Size = new System.Drawing.Size(261, 26);
		((System.Windows.Forms.Control)(object)this.comItem).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.comItem).Leave += new System.EventHandler(comItem_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label42.Location = new System.Drawing.Point(721, 16);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(58, 15);
		this.label42.TabIndex = 55;
		this.label42.Text = "Item Rate";
		this.txtRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRate.Location = new System.Drawing.Point(720, 36);
		this.txtRate.Name = "txtRate";
		this.txtRate.Size = new System.Drawing.Size(87, 23);
		this.txtRate.TabIndex = 6;
		this.txtRate.Tag = "ItemRate";
		this.txtRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtRate.TextChanged += new System.EventHandler(txtRate_TextChanged);
		this.txtRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRate_KeyPress);
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label40.Location = new System.Drawing.Point(682, 359);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(39, 18);
		this.label40.TabIndex = 51;
		this.label40.Text = "Rate";
		this.label32.AutoSize = true;
		this.label32.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label32.Location = new System.Drawing.Point(6, 16);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(32, 15);
		this.label32.TabIndex = 39;
		this.label32.Text = "Item";
		this.txtAmount.BackColor = System.Drawing.Color.White;
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtAmount.Location = new System.Drawing.Point(808, 36);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.ReadOnly = true;
		this.txtAmount.Size = new System.Drawing.Size(103, 23);
		this.txtAmount.TabIndex = 7;
		this.txtAmount.TabStop = false;
		this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(812, 16);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(51, 15);
		this.label14.TabIndex = 21;
		this.label14.Text = "Amount";
		this.groupBox2.Controls.Add(this.txtOrderAmount);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.label22);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add(this.txtDeliverydays);
		this.groupBox2.Controls.Add(this.label41);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.txtDeliveryStartDate);
		this.groupBox2.Controls.Add(this.txtremarks);
		this.groupBox2.Controls.Add(this.txtOrderWeight);
		this.groupBox2.Controls.Add(this.txtOrderQty);
		this.groupBox2.Controls.Add(this.label62);
		this.groupBox2.Controls.Add(this.txtdocno);
		this.groupBox2.Controls.Add(this.DocDate);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label63);
		this.groupBox2.Controls.Add(this.txtReferenceNo);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combpttrm);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.combdeliverytrm);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.txtduedays);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.comsupplier);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.duedate);
		this.groupBox2.Controls.Add(this.label29);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(2, 27);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(711, 147);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtOrderAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderAmount.Enabled = false;
		this.txtOrderAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderAmount.Location = new System.Drawing.Point(595, 93);
		this.txtOrderAmount.Name = "txtOrderAmount";
		this.txtOrderAmount.ShortcutsEnabled = false;
		this.txtOrderAmount.Size = new System.Drawing.Size(108, 23);
		this.txtOrderAmount.TabIndex = 12;
		this.txtOrderAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(506, 97);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(84, 15);
		this.label12.TabIndex = 142;
		this.label12.Text = "Order Amount";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(3, 121);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(52, 15);
		this.label22.TabIndex = 140;
		this.label22.Text = "Remarks";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(300, 43);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(57, 15);
		this.label16.TabIndex = 137;
		this.label16.Text = "Due Date";
		this.txtDeliverydays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDeliverydays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliverydays.Location = new System.Drawing.Point(595, 12);
		this.txtDeliverydays.Name = "txtDeliverydays";
		this.txtDeliverydays.Size = new System.Drawing.Size(108, 23);
		this.txtDeliverydays.TabIndex = 9;
		this.txtDeliverydays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(506, 16);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(79, 15);
		this.label41.TabIndex = 134;
		this.label41.Text = "Delivery Days";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(300, 97);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(88, 15);
		this.label28.TabIndex = 134;
		this.label28.Text = "Delivery S.Date";
		this.txtDeliveryStartDate.CustomFormat = "dd-MMM-yy";
		this.txtDeliveryStartDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDeliveryStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDeliveryStartDate.Location = new System.Drawing.Point(389, 93);
		this.txtDeliveryStartDate.Name = "txtDeliveryStartDate";
		this.txtDeliveryStartDate.Size = new System.Drawing.Size(114, 23);
		this.txtDeliveryStartDate.TabIndex = 8;
		this.txtDeliveryStartDate.ValueChanged += new System.EventHandler(txtDeliveryStartDate_ValueChanged);
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(88, 119);
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(615, 23);
		this.txtremarks.TabIndex = 13;
		this.txtOrderWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderWeight.Enabled = false;
		this.txtOrderWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderWeight.Location = new System.Drawing.Point(595, 67);
		this.txtOrderWeight.Name = "txtOrderWeight";
		this.txtOrderWeight.ShortcutsEnabled = false;
		this.txtOrderWeight.Size = new System.Drawing.Size(108, 23);
		this.txtOrderWeight.TabIndex = 11;
		this.txtOrderWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOrderQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOrderQty.Enabled = false;
		this.txtOrderQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOrderQty.Location = new System.Drawing.Point(595, 40);
		this.txtOrderQty.Name = "txtOrderQty";
		this.txtOrderQty.ShortcutsEnabled = false;
		this.txtOrderQty.Size = new System.Drawing.Size(108, 23);
		this.txtOrderQty.TabIndex = 10;
		this.txtOrderQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(506, 71);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(79, 15);
		this.label62.TabIndex = 129;
		this.label62.Text = "Order Weight";
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(88, 13);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(92, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_TextChanged);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(182, 13);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(115, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.DocDate.Leave += new System.EventHandler(DocDate_Leave);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(3, 17);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(88, 15);
		this.label5.TabIndex = 88;
		this.label5.Text = "Doc No && Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(505, 44);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(59, 15);
		this.label63.TabIndex = 130;
		this.label63.Text = "Order Qty";
		this.txtReferenceNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtReferenceNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReferenceNo.Location = new System.Drawing.Point(88, 66);
		this.txtReferenceNo.Name = "txtReferenceNo";
		this.txtReferenceNo.Size = new System.Drawing.Size(209, 23);
		this.txtReferenceNo.TabIndex = 3;
		this.combpttrm.AllowNull = (DefaultableBoolean)1;
		this.combpttrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combpttrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combpttrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance79).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combpttrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.combpttrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance80;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance81;
		((SpecialBoxBase)((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance82).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance82).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combpttrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combpttrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance86).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance87).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance87).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance87).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance89).TextHAlignAsString = "Left";
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combpttrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combpttrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combpttrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combpttrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combpttrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combpttrm).Location = new System.Drawing.Point(88, 91);
		((System.Windows.Forms.Control)(object)this.combpttrm).Name = "combpttrm";
		((System.Windows.Forms.Control)(object)this.combpttrm).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.combpttrm).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combpttrm).TextChanged += new System.EventHandler(combpttrm_TextChanged);
		this.combdeliverytrm.AllowNull = (DefaultableBoolean)1;
		this.combdeliverytrm.AutoCompleteMode = (AutoCompleteMode)4;
		this.combdeliverytrm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combdeliverytrm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance106).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance106;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance108;
		((SpecialBoxBase)((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance95;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combdeliverytrm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.combdeliverytrm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Location = new System.Drawing.Point(389, 64);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Name = "combdeliverytrm";
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.combdeliverytrm).TextChanged += new System.EventHandler(combdeliverytrm_TextChanged);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(3, 70);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(78, 15);
		this.label6.TabIndex = 64;
		this.label6.Text = "Reference No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(3, 44);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(73, 15);
		this.label7.TabIndex = 65;
		this.label7.Text = "Supplier (Cr)";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label4.Location = new System.Drawing.Point(3, 97);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(84, 15);
		this.label4.TabIndex = 100;
		this.label4.Text = "Payment Term";
		this.txtduedays.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtduedays.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtduedays.Location = new System.Drawing.Point(389, 13);
		this.txtduedays.Name = "txtduedays";
		this.txtduedays.Size = new System.Drawing.Size(114, 23);
		this.txtduedays.TabIndex = 5;
		this.txtduedays.Text = "0";
		this.txtduedays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtduedays.TextChanged += new System.EventHandler(txtduedays_TextChanged);
		this.txtduedays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtduedays_KeyPress);
		this.comsupplier.AllowNull = (DefaultableBoolean)1;
		this.comsupplier.AutoCompleteMode = (AutoCompleteMode)4;
		this.comsupplier.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comsupplier.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comsupplier).DisplayLayout.Appearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.comsupplier).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance109).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance109).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance110;
		((SpecialBoxBase)((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comsupplier).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance160;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comsupplier).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance161).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance161).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance161;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance164).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance165).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance165).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance165).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance165).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance165;
		((AppearanceBase)appearance111).TextHAlignAsString = "Left";
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance112).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comsupplier).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comsupplier).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comsupplier).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comsupplier).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comsupplier.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comsupplier).Location = new System.Drawing.Point(88, 38);
		((System.Windows.Forms.Control)(object)this.comsupplier).Name = "comsupplier";
		((System.Windows.Forms.Control)(object)this.comsupplier).Size = new System.Drawing.Size(209, 26);
		((System.Windows.Forms.Control)(object)this.comsupplier).TabIndex = 2;
		this.comsupplier.ValueChanged += new System.EventHandler(comsupplier_ValueChanged);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(300, 17);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(58, 15);
		this.label10.TabIndex = 98;
		this.label10.Text = "Due Days";
		this.duedate.CustomFormat = "dd-MM-yy";
		this.duedate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.duedate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.duedate.Location = new System.Drawing.Point(389, 39);
		this.duedate.Name = "duedate";
		this.duedate.Size = new System.Drawing.Size(114, 23);
		this.duedate.TabIndex = 6;
		this.duedate.ValueChanged += new System.EventHandler(duedate_ValueChanged);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(300, 70);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(74, 15);
		this.label29.TabIndex = 101;
		this.label29.Text = "Deliver Term";
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Controls.Add(this.ChkPrintslip);
		this.panel11.Controls.Add(this.ChkResetOnSave);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1261, 26);
		this.panel11.TabIndex = 110;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(4, 3);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(134, 19);
		this.label17.TabIndex = 34;
		this.label17.Text = "Purchase Order";
		this.ChkPrintslip.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintslip.AutoSize = true;
		this.ChkPrintslip.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintslip.ForeColor = System.Drawing.Color.White;
		this.ChkPrintslip.Location = new System.Drawing.Point(1185, 4);
		this.ChkPrintslip.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintslip.Name = "ChkPrintslip";
		this.ChkPrintslip.Size = new System.Drawing.Size(76, 19);
		this.ChkPrintslip.TabIndex = 7;
		this.ChkPrintslip.Text = "Print Slip";
		this.ChkPrintslip.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkResetOnSave.AutoSize = true;
		this.ChkResetOnSave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkResetOnSave.ForeColor = System.Drawing.Color.White;
		this.ChkResetOnSave.Location = new System.Drawing.Point(1018, 4);
		this.ChkResetOnSave.Margin = new System.Windows.Forms.Padding(2);
		this.ChkResetOnSave.Name = "ChkResetOnSave";
		this.ChkResetOnSave.Size = new System.Drawing.Size(158, 19);
		this.ChkResetOnSave.TabIndex = 6;
		this.ChkResetOnSave.Text = "Reset On Add and  Save";
		this.ChkResetOnSave.UseVisualStyleBackColor = true;
		this.ChkResetOnSave.Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance173).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance174).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance174).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance174).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance174;
		((AppearanceBase)appearance58).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1392, 41);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.combranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance5;
		((SpecialBoxBase)((UltraGridBase)this.combranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.combranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance115).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance116).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance119).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance120).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance120).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance120).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance175).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance176).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1392, 73);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(78, 26);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(1302, 79);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(79, 15);
		this.label13.TabIndex = 86;
		this.label13.Text = "Branch Name";
		this.label13.Visible = false;
		this.combcommUOM.AllowNull = (DefaultableBoolean)1;
		this.combcommUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.combcommUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combcommUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.combcommUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combcommUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combcommUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.combcommUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Location = new System.Drawing.Point(1333, 106);
		((System.Windows.Forms.Control)(object)this.combcommUOM).Name = "combcommUOM";
		((System.Windows.Forms.Control)(object)this.combcommUOM).Size = new System.Drawing.Size(87, 26);
		((System.Windows.Forms.Control)(object)this.combcommUOM).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Visible = false;
		((System.Windows.Forms.Control)(object)this.combcommUOM).Leave += new System.EventHandler(combcommUOM_Leave_1);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(1302, 47);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(79, 15);
		this.label18.TabIndex = 87;
		this.label18.Text = "Project Name";
		this.label18.Visible = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(1372, 135);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(35, 15);
		this.label19.TabIndex = 93;
		this.label19.Text = "UOM";
		this.label19.Visible = false;
		this.txtSupplierGLId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSupplierGLId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSupplierGLId.Location = new System.Drawing.Point(450, 247);
		this.txtSupplierGLId.Multiline = true;
		this.txtSupplierGLId.Name = "txtSupplierGLId";
		this.txtSupplierGLId.Size = new System.Drawing.Size(16, 26);
		this.txtSupplierGLId.TabIndex = 61;
		this.txtSupplierGLId.Visible = false;
		this.txtCommAgentId.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtCommAgentId.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCommAgentId.Location = new System.Drawing.Point(469, 247);
		this.txtCommAgentId.Multiline = true;
		this.txtCommAgentId.Name = "txtCommAgentId";
		this.txtCommAgentId.Size = new System.Drawing.Size(16, 26);
		this.txtCommAgentId.TabIndex = 62;
		this.txtCommAgentId.Visible = false;
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnSaveAs, this.btnDelete, this.btnAttachment, this.toolStripSeparator4, this.btnSlip, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1261, 25);
		this.toolStrip1.TabIndex = 26;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(76, 22);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(74, 22);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(60, 22);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(BtnSave_Click);
		this.btnSaveAs.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(81, 22);
		this.btnSaveAs.Text = "&Save As";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(69, 22);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(105, 22);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
		this.btnSlip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSlip.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnSlip.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSlip.Name = "btnSlip";
		this.btnSlip.Size = new System.Drawing.Size(96, 22);
		this.btnSlip.Text = "1863-Print";
		this.btnSlip.Click += new System.EventHandler(btnSlip_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.tabPage2.Controls.Add(this.panel17);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel2);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1267, 691);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel17.Controls.Add(this.panel19);
		this.panel17.Controls.Add(this.panel18);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel17.Location = new System.Drawing.Point(3, 482);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1261, 206);
		this.panel17.TabIndex = 49;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailHistory);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 27);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(1261, 179);
		this.panel19.TabIndex = 49;
		this.grdDetailHistory.AllowEdit = (InheritableBoolean)2;
		grdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailHistory.DesignTimeLayout = grdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailHistory.DynamicFiltering = true;
		this.grdDetailHistory.FilterMode = (FilterMode)2;
		this.grdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Name = "grdDetailHistory";
		((ScrollableBase)this.grdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).Size = new System.Drawing.Size(1261, 179);
		((System.Windows.Forms.Control)(object)this.grdDetailHistory).TabIndex = 0;
		this.grdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.grdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel18.BackColor = System.Drawing.Color.Teal;
		this.panel18.Controls.Add(this.ctrlGrdBar3);
		this.panel18.Controls.Add(this.label55);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 0);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(1261, 27);
		this.panel18.TabIndex = 48;
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1220, 3);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdDetailHistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar3.TabIndex = 34;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.ForeColor = System.Drawing.Color.White;
		this.label55.Location = new System.Drawing.Point(5, 3);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(353, 19);
		this.label55.TabIndex = 35;
		this.label55.Text = "Detail Information of Above Selected Row";
		this.panel10.Controls.Add(this.panel8);
		this.panel10.Controls.Add(this.panel5);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1261, 425);
		this.panel10.TabIndex = 48;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 81);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1261, 344);
		this.panel8.TabIndex = 49;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		grdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistory.DesignTimeLayout = grdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(1261, 344);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 0;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.grdHistory.ColumnButtonClick += new ColumnActionEventHandler(grdHistory_ColumnButtonClick);
		this.grdHistory.SelectionChanged += new System.EventHandler(grdHistory_SelectionChanged);
		this.grdHistory.LinkClicked += new ColumnActionEventHandler(grdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdHistory).DoubleClick += new System.EventHandler(grdHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel5.Controls.Add(this.groupBox4);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1261, 81);
		this.panel5.TabIndex = 0;
		this.groupBox4.Controls.Add(this.label15);
		this.groupBox4.Controls.Add(this.btnshow);
		this.groupBox4.Controls.Add(this.label73);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCustomerHistory);
		this.groupBox4.Controls.Add(this.label75);
		this.groupBox4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory);
		this.groupBox4.Controls.Add(this.txtToDateHistory);
		this.groupBox4.Controls.Add(this.label76);
		this.groupBox4.Controls.Add(this.txtToDocNoHistory);
		this.groupBox4.Controls.Add(this.txtFromdateHistory);
		this.groupBox4.Controls.Add(this.label3);
		this.groupBox4.Controls.Add(this.label77);
		this.groupBox4.Controls.Add(this.txtFromNoHistory);
		this.groupBox4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox4.Location = new System.Drawing.Point(9, 1);
		this.groupBox4.Name = "groupBox4";
		this.groupBox4.Size = new System.Drawing.Size(813, 74);
		this.groupBox4.TabIndex = 0;
		this.groupBox4.TabStop = false;
		this.groupBox4.Text = "Filters";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(234, 16);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(48, 15);
		this.label15.TabIndex = 179;
		this.label15.Text = "To Date";
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 10.2f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(741, 37);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 25);
		this.btnshow.TabIndex = 6;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label73.AutoSize = true;
		this.label73.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label73.Location = new System.Drawing.Point(539, 16);
		this.label73.Name = "label73";
		this.label73.Size = new System.Drawing.Size(86, 15);
		this.label73.TabIndex = 178;
		this.label73.Text = "Supplier Name";
		this.CmbCustomerHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbCustomerHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCustomerHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCustomerHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance147).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance148).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance148).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance148;
		((AppearanceBase)appearance149).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance149;
		((SpecialBoxBase)((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance150).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance152;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance153;
		((AppearanceBase)appearance154).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance154).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance156).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance156;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance158;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance159).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCustomerHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCustomerHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Location = new System.Drawing.Point(539, 36);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Name = "CmbCustomerHistory";
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbCustomerHistory).TabIndex = 5;
		this.label75.AutoSize = true;
		this.label75.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label75.Location = new System.Drawing.Point(5, 16);
		this.label75.Name = "label75";
		this.label75.Size = new System.Drawing.Size(60, 15);
		this.label75.TabIndex = 174;
		this.label75.Text = "Date Type";
		this.cmbDateTypeHistory.AllowNull = (DefaultableBoolean)1;
		this.cmbDateTypeHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateTypeHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateTypeHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance178).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance179).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance179).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance179;
		((AppearanceBase)appearance180).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance180;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance122).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance122).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance123).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance126).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance128).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance128).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance128).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance128;
		((AppearanceBase)appearance129).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateTypeHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateTypeHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Location = new System.Drawing.Point(5, 36);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Name = "cmbDateTypeHistory";
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).Size = new System.Drawing.Size(115, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateTypeHistory).TabIndex = 0;
		this.cmbDateTypeHistory.ValueChanged += new System.EventHandler(cmbDateTypeHistory_ValueChanged);
		this.txtToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.CustomFormat = "dd-MMM-yy";
		this.txtToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateHistory.Location = new System.Drawing.Point(234, 38);
		this.txtToDateHistory.Name = "txtToDateHistory";
		this.txtToDateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtToDateHistory.TabIndex = 2;
		this.label76.AutoSize = true;
		this.label76.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label76.Location = new System.Drawing.Point(122, 16);
		this.label76.Name = "label76";
		this.label76.Size = new System.Drawing.Size(63, 15);
		this.label76.TabIndex = 171;
		this.label76.Text = "From Date";
		this.txtToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(440, 38);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.Size = new System.Drawing.Size(97, 23);
		this.txtToDocNoHistory.TabIndex = 4;
		this.txtFromdateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.CustomFormat = "dd-MMM-yy";
		this.txtFromdateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromdateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromdateHistory.Location = new System.Drawing.Point(122, 38);
		this.txtFromdateHistory.Name = "txtFromdateHistory";
		this.txtFromdateHistory.Size = new System.Drawing.Size(111, 23);
		this.txtFromdateHistory.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(440, 16);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(45, 15);
		this.label3.TabIndex = 173;
		this.label3.Text = "Doc To";
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label77.Location = new System.Drawing.Point(346, 16);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(60, 15);
		this.label77.TabIndex = 173;
		this.label77.Text = "Doc From";
		this.txtFromNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFromNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromNoHistory.Location = new System.Drawing.Point(346, 38);
		this.txtFromNoHistory.Name = "txtFromNoHistory";
		this.txtFromNoHistory.Size = new System.Drawing.Size(92, 23);
		this.txtFromNoHistory.TabIndex = 3;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label54);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 30);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1261, 27);
		this.panel9.TabIndex = 47;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1220, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
		this.ctrlGrdBar2.MyGrid = this.grdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 112;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.ForeColor = System.Drawing.Color.White;
		this.label54.Location = new System.Drawing.Point(5, 3);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(197, 19);
		this.label54.TabIndex = 35;
		this.label54.Text = "Purchase Order History";
		this.panel2.Controls.Add(this.toolStrip2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1261, 27);
		this.panel2.TabIndex = 46;
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.BtnNewHistory, this.BtnRefreshHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1261, 25);
		this.toolStrip2.TabIndex = 27;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(btnLoadAll_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(76, 22);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 2);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(46, 17);
		this.label23.TabIndex = 35;
		this.label23.Text = "Detail";
		this.label78.AutoSize = true;
		this.label78.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label78.ForeColor = System.Drawing.Color.White;
		this.label78.Location = new System.Drawing.Point(5, 3);
		this.label78.Name = "label78";
		this.label78.Size = new System.Drawing.Size(72, 19);
		this.label78.TabIndex = 35;
		this.label78.Text = "History ";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1275, 720);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "PurchaseOrderConcrete";
		this.Text = "Purchase Invocie";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InvfrmInvSaleInvoiceDirect_FormClosing);
		base.Load += new System.EventHandler(InvfrmPurchasedirectInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvfrmPurchasedirectInvoice_KeyDown_1);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.CurrencyInfoBox.ResumeLayout(false);
		this.CurrencyInfoBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbCurrency).EndInit();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combcommAgent).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combCommType).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbtaxtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAttributeVarient).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comjobLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtCity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comItem).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.combpttrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combdeliverytrm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comsupplier).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combcommUOM).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel17.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailHistory).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel18.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel5.ResumeLayout(false);
		this.groupBox4.ResumeLayout(false);
		this.groupBox4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCustomerHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDateTypeHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
