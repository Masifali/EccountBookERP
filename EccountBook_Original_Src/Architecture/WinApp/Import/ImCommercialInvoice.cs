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
using Architecture.BLL.Export;
using Architecture.BLL.Import;
using Architecture.BLL.Inventory;
using Architecture.BLL.Reports.Import;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Export;
using Architecture.Model.Import;
using Architecture.Model.Inventory;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Import;

public class ImCommercialInvoice : Form
{
	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int contractidInupdateCase;

	private bool UpdateMode;

	private int updateDetailIndex;

	private DataTable dtdetail = new DataTable();

	private DataTable listUOM = new DataTable();

	private DataTable dtShipmentExpenses = new DataTable();

	private DataTable dtAccountlst = new DataTable();

	private DataTable dtWarehouse = new DataTable();

	private DataTable dtJobLot = new DataTable();

	public int DocumentTypeId;

	private IContainer components;

	private Panel panel4;

	private Panel panel5;

	private Panel panel1;

	private Panel panel7;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel8;

	private GroupBox HeaderGropBox;

	private Label label28;

	internal UltraCombo cmbSupCust;

	private Label label21;

	private Label label3;

	private Label label2;

	private DateTimePicker txtDocDate;

	private Label label17;

	private TextBox txtdocno;

	private Label label5;

	private Label label1;

	private Label label43;

	internal UltraCombo cmbproject;

	private Label label18;

	internal UltraCombo cmbbranches;

	private Label label13;

	internal GridEX grdDetail;

	private Panel panel10;

	private Panel panel6;

	private Button btnclose;

	private Label label23;

	private Panel panel12;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private TabPage tabPage2;

	private Panel panel13;

	private Panel panel14;

	internal GridEX DataGridHistory;

	private Panel panel15;

	private Label label69;

	private Panel panel16;

	private ToolStrip toolStrip3;

	private ToolStripButton toolStripButton6;

	private ToolStripSeparator toolStripSeparator1;

	internal UltraCombo cmbFCurrencr;

	internal UltraCombo cmbPaymentTerm;

	internal UltraCombo combranches;

	internal UltraCombo comproject;

	internal UltraCombo ultraCombo3;

	internal UltraCombo CmbLCContractor;

	internal UltraCombo ultraCombo2;

	internal UltraCombo ultraCombo1;

	internal UltraCombo cmbCarierType;

	internal UltraCombo cmbExporterBank;

	internal UltraCombo cmbImporterBank;

	internal UltraCombo cmbEFormNo;

	internal UltraCombo cmbdeliveryterm;

	private ToolStrip miniToolStrip;

	private ToolStripButton toolStripButton1;

	private ToolStripButton toolStripButton2;

	private ToolStripButton toolStripButton3;

	private ToolStripSeparator toolStripSeparator;

	private ToolStripButton toolStripButton4;

	private ToolStripButton toolStripButton5;

	private ToolStripButton helpToolStripButton;

	private ToolStrip toolStrip2;

	private Panel panel2;

	private CtrlGrdBar ctrlGrdBar1;

	internal UltraCombo cmbLcContractNo;

	private Label lblPoNo;

	internal UltraCombo cmbNotifyParty2;

	private Label label10;

	internal UltraCombo cmbNotifyParty1;

	private Label label8;

	private Label label7;

	private TextBox txtLotRef;

	private Label label48;

	private Label label9;

	private Label label11;

	private Label label12;

	internal UltraCombo cmbexporterBankNew;

	internal UltraCombo cmbimporterBankNew;

	internal UltraCombo cmbPaymentTermsNew;

	internal UltraCombo cmbdeliverytermnew;

	private Label label24;

	private Label label19;

	private Label label26;

	private Label label56;

	internal UltraCombo cmbDestinationPort;

	internal UltraCombo cmbLoadingPort;

	private Label label16;

	private Label label41;

	private Label label20;

	private Label label47;

	internal UltraCombo cmbcareiertype;

	private Label label22;

	private TextBox txtfcyAmount;

	private Label label25;

	private Label label27;

	internal UltraCombo cmbfcycode;

	private TextBox txtLocalAmt;

	private Label label42;

	private TextBox txtExRate;

	private Label label44;

	private Label label45;

	private DateTimePicker txtIformDate;

	private Label label51;

	private TextBox txtIformNo;

	private Label label52;

	private Label label49;

	private Label label46;

	private TextBox txtGrossWeight;

	private Label label54;

	private TextBox txtNetWeight;

	private Label label55;

	private TextBox txtNoOfContainer;

	private Label label57;

	private Label label59;

	private Label label58;

	private Label label53;

	private Label label50;

	private Label label31;

	private Label label60;

	private Label label62;

	private Label label61;

	private TextBox txtcertificateOfOrigion;

	private TextBox txtcertificate;

	private Label label65;

	private TextBox txtTotalNetAmount;

	private Label label66;

	private TextBox txtaddlessamount;

	private Label label64;

	private Label label40;

	private TextBox txtaddlesscommnets;

	private Label label67;

	private TextBox txtinvoiceno;

	private Label label68;

	private Label label70;

	private Label label63;

	private Label label15;

	private Label label14;

	internal UltraCombo CmbHomeCurrency;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private TabPage tabPage4;

	internal GridEX grdShipmentExpenses;

	private ToolStripButton btnPrint;

	public ImCommercialInvoice(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void projectGetAll()
	{
		try
		{
			DataTable dtproject = CommonServices.ProjectServiceBind();
			if (dtproject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtproject, cmbproject, "Id", "ProjectName", "Project Name", true);
				((UltraGridBase)cmbproject).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void branchesGetAll()
	{
		try
		{
			DataTable BranchList = CommonServices.BrancheServiceBind();
			if (BranchList.Rows.Count > 0)
			{
				DDL.BindDDLNew(BranchList, cmbbranches, "Id", "BranchName", "Branch Name", true);
				((UltraGridBase)cmbbranches).Rows[0].Activate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void generateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Code = ImInvoice.GenerateCode(new ImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
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

	private void CustomerGetAll()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable Supplierlst = SupplierCustomer.ReadByOrganizationCompanyIdForExport(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (Supplierlst.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("CompanyName");
				dt.Columns.Add("GlAccountId");
				for (int i = 0; i < Supplierlst.Rows.Count; i++)
				{
					dt.Rows.Add(Supplierlst.Rows[i]["Id"], Supplierlst.Rows[i]["CompanyName"], Supplierlst.Rows[i]["GlAccountId"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbSupCust, "Id", "CompanyName", "Supplier Name", ZeroIndex: false);
					DDL.BindDDL(dt, cmbNotifyParty1, "Id", "CompanyName", "Notify Party", ZeroIndex: false);
					DDL.BindDDL(dt, cmbNotifyParty2, "Id", "CompanyName", "Notify Party", ZeroIndex: false);
					((UltraGridBase)cmbSupCust).DisplayLayout.Bands[0].Columns[1].Width = 300;
					((UltraGridBase)cmbNotifyParty1).DisplayLayout.Bands[0].Columns[1].Width = 300;
					((UltraGridBase)cmbNotifyParty2).DisplayLayout.Bands[0].Columns[1].Width = 300;
					((UltraGridBase)cmbSupCust).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbNotifyParty1).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					((UltraGridBase)cmbNotifyParty2).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSupCust_Leave(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			((UltraGridBase)cmbLcContractNo).DataSource = null;
			((Control)(object)cmbLcContractNo).Text = string.Empty;
			DataTable dtlcorder = ImLcOrder.GetLcOrderNo(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("LcOrderNo");
			if (dtlcorder.Rows.Count > 0)
			{
				for (int i = 0; i < dtlcorder.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtlcorder.Rows[i]["SupCustId"]) == Conversion.ToInt(cmbSupCust.Value))
					{
						dt.Rows.Add(dtlcorder.Rows[i]["Id"], dtlcorder.Rows[i]["LcOrderNo"]);
					}
				}
				DDL.BindDDL(dt, cmbLcContractNo, "Id", "LcOrderNo", "Contract No", ZeroIndex: false);
			}
			else
			{
				((Control)(object)cmbLcContractNo).Text = string.Empty;
				((UltraGridBase)cmbLcContractNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbLcContractNo_Leave(object sender, EventArgs e)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			if (Conversion.ToInt(cmbLcContractNo.Value) > 0)
			{
				if (Conversion.ToInt(cmbLcContractNo.Value) != contractidInupdateCase)
				{
					ExImLcOrderPurchaseOrder obj = new ExImLcOrderPurchaseOrder();
					obj.CompanyId = UserAccount.CompanyId;
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.Id = Conversion.ToInt(cmbLcContractNo.Value);
					obj.DocumentTypeId = DocumentTypeId;
					DataTable PIH = ExImLcOrderPurchaseOrder.GetImportPurchaseOrderDetailById(obj);
					if (PIH != null)
					{
						cmbPaymentTermsNew.Value = Conversion.ToInt(PIH.Rows[0]["PaymentTermId"]);
						cmbdeliverytermnew.Value = Conversion.ToInt(PIH.Rows[0]["DeliveryTermId"]);
						cmbexporterBankNew.Value = Conversion.ToInt(PIH.Rows[0]["ExporterBankId"]);
						cmbimporterBankNew.Value = Conversion.ToInt(PIH.Rows[0]["ImporterBankId"]);
						cmbLoadingPort.Value = Conversion.ToInt(PIH.Rows[0]["LoadingPortId"]);
						cmbDestinationPort.Value = Conversion.ToInt(PIH.Rows[0]["DestinationPortId"]);
						cmbfcycode.Value = Conversion.ToInt(PIH.Rows[0]["FcyId"]);
						CmbHomeCurrency.Value = Conversion.ToInt(PIH.Rows[0]["HomeCurrencyId"]);
						txtfcyAmount.Text = Conversion.ToString(PIH.Rows[0]["FcyAmount"]);
						txtExRate.Text = Conversion.ToString(PIH.Rows[0]["ExchangeRate"]);
						txtLocalAmt.Text = Conversion.ToString(PIH.Rows[0]["Amount"]);
						txtGrossWeight.Text = Conversion.ToString(PIH.Rows[0]["GrossWeight"]);
						txtNetWeight.Text = Conversion.ToString(PIH.Rows[0]["NetWeight"]);
						txtNoOfContainer.Text = Conversion.ToString(PIH.Rows[0]["Containers"]);
					}
					DataTable dt = new DataTable();
					if (DocumentTypeId == 237)
					{
						dt = ExImLcOrderPurchaseOrder.GETIMPORTCONTRACTDETAILBYCONTRACTID(obj);
					}
					else if (DocumentTypeId == 233)
					{
						dt = ExImLcOrderPurchaseOrder.GETIMPORTGRNDETAILBYPURCHASEORDERID(obj);
					}
					dtdetail.Rows.Clear();
					if (dt.Rows.Count <= 0)
					{
						return;
					}
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						double Mton = 0.0;
						double NoofBags = 0.0;
						if (DocumentTypeId == 237)
						{
							Mton = Conversion.ToDouble(dt.Rows[i]["ItemQty"]);
							NoofBags = Conversion.ToDouble(dt.Rows[i]["ItemQty"]) * Conversion.ToDouble(dt.Rows[i]["PackUom"]);
						}
						else
						{
							Mton = Conversion.ToDouble(dt.Rows[i]["ItemQty"]) * Conversion.ToDouble(dt.Rows[i]["PackUom"]) / Conversion.ToDouble((object)1000);
							NoofBags = Conversion.ToDouble(dt.Rows[i]["PackUom"]);
						}
						dtdetail.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["PackingMaterialId"], dt.Rows[i]["PackingType"], Mton, dt.Rows[i]["UOMScheduleIdOuter"], dt.Rows[i]["PackUom"], NoofBags, dt.Rows[i]["ItemRate"], dt.Rows[i]["RateUomId"], dt.Rows[i]["RateUom"], dt.Rows[i]["ItemAmount"], dt.Rows[i]["ItemDescription"], dt.Rows[i]["WarehouseId"], dt.Rows[i]["WareHouseName"], dt.Rows[i]["JobLotId"], dt.Rows[i]["JobLotDescription"]);
					}
					grdDetail.DataSource = dtdetail;
					grdDetail.RetrieveStructure();
					grdSettings();
					getTotalnetWeightFromGrid();
					ParporationHomeCurrencyAmount();
				}
				else
				{
					grdDetail.ClearStructure();
				}
			}
			else
			{
				((Control)(object)cmbPaymentTermsNew).Text = string.Empty;
				((Control)(object)cmbdeliverytermnew).Text = string.Empty;
				((Control)(object)cmbexporterBankNew).Text = string.Empty;
				((Control)(object)cmbimporterBankNew).Text = string.Empty;
				((Control)(object)cmbLoadingPort).Text = string.Empty;
				((Control)(object)cmbDestinationPort).Text = string.Empty;
				((Control)(object)cmbfcycode).Text = string.Empty;
				grdDetail.DataSource = new DataTable();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void getTotalnetWeightFromGrid()
	{
		try
		{
			double NetWeightkg = 0.0;
			object total = grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2);
			double Amount = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2));
			NetWeightkg = Conversion.ToDouble(total) * 1000.0;
			txtNetWeight.Text = NetWeightkg.ToString("0,0");
			if (DocumentTypeId == 237)
			{
				txtGrossWeight.Text = NetWeightkg.ToString("0,0");
			}
			txtfcyAmount.Text = Amount.ToString("0,0");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeliveryTermFill()
	{
		try
		{
			DataTable dtDeliveryTerms = ExImDeliveryTerm.Getall();
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, cmbdeliverytermnew, "Id", "Code", "DeliveryTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PaymentTermsFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDeliveryTerms = ExImLcPaymentTerm.Getall(new ExImLcPaymentTerm
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDeliveryTerms.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDeliveryTerms, cmbPaymentTermsNew, "Id", "LcOrderTerm", "PaymentTerms", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadingPortFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SeaPorts.Getall(new SeaPorts
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbLoadingPort, "Id", "PortName", "Loading Port", false);
				DDL.BindDDLNew(dt, cmbDestinationPort, "Id", "PortName", "Destination Port", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MultiCurrencyfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = MultiCurrency.GetAll(new MultiCurrency
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbfcycode, "Id", "CurrencyCode", "CurrencyName", false);
				DDL.BindDDLNew(dt, CmbHomeCurrency, "Id", "CurrencyCode", "HomeCurrencyName", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImporterandExportBankFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			List<Bank> lst = Bank.GetAll(new Bank
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (lst.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("BranchName");
			DataTable dtimporter = new DataTable();
			dtimporter.Columns.Add("Id");
			dtimporter.Columns.Add("BranchName");
			for (int i = 0; i < lst.Count; i++)
			{
				if (lst[i].IsHomeland == "Foreign Country")
				{
					dt.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
				if (lst[i].IsHomeland == "Home Country")
				{
					dtimporter.Rows.Add(lst[i].Id, lst[i].BranchName);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbexporterBankNew, "Id", "BranchName", "ExporterBank", ZeroIndex: false);
				DDL.BindDDL(dtimporter, cmbimporterBankNew, "Id", "BranchName", "ImporterBank", ZeroIndex: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CarierType()
	{
		try
		{
			DataTable dtcariertype = new DataTable();
			dtcariertype.Columns.Add("Id");
			dtcariertype.Columns.Add("Type");
			dtcariertype.Rows.Add(1, "By Sea");
			dtcariertype.Rows.Add(2, "By Air");
			dtcariertype.Rows.Add(3, "By Road");
			DDL.BindDDL(dtcariertype, cmbcareiertype, "Id", "Type", "Carier type", ZeroIndex: false);
			((UltraGridBase)cmbcareiertype).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OrderStatus()
	{
		try
		{
			DataTable dtstatus = new DataTable();
			dtstatus.Columns.Add("Id");
			dtstatus.Columns.Add("Status");
			dtstatus.Rows.Add(1, "Open");
			dtstatus.Rows.Add(2, "Complete");
			dtstatus.Rows.Add(3, "Cancel");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AccountsFill()
	{
		try
		{
			dtAccountlst = CommonServices.CoaAllocationGetAllServiceBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WarehouseAndJobLotBind()
	{
		try
		{
			dtWarehouse = CommonServices.getActiveWareHouse();
			dtJobLot = CommonServices.JobLotGetAllService();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImProformaInvoice_Load(object sender, EventArgs e)
	{
		try
		{
			if (DocumentTypeId == 233)
			{
				lblPoNo.Text = "Purchase Order";
				CmbHomeCurrency.ReadOnly = true;
				((Control)(object)CmbHomeCurrency).TabStop = true;
			}
			else
			{
				lblPoNo.Text = "Contract No";
				WarehouseAndJobLotBind();
				CmbHomeCurrency.ReadOnly = false;
			}
			AccountsFill();
			txtinvoiceno.Focus();
			generateCode();
			branchesGetAll();
			projectGetAll();
			CustomerGetAll();
			DeliveryTermFill();
			PaymentTermsFill();
			LoadingPortFill();
			MultiCurrencyfill();
			ImporterandExportBankFill();
			CarierType();
			dtdetail.Columns.Add("ItemId");
			dtdetail.Columns.Add("ItemName");
			dtdetail.Columns.Add("PackTypeId");
			dtdetail.Columns.Add("PackType");
			dtdetail.Columns.Add("Qty/M.Ton", typeof(double));
			dtdetail.Columns.Add("PackSizeId");
			dtdetail.Columns.Add("PackSize");
			dtdetail.Columns.Add("NoOfBags", typeof(double));
			dtdetail.Columns.Add("Cost/M.Ton", typeof(double));
			dtdetail.Columns.Add("RateUOMId");
			dtdetail.Columns.Add("RateUOM");
			dtdetail.Columns.Add("Amount", typeof(double));
			dtdetail.Columns.Add("ItemDetail");
			dtdetail.Columns.Add("WareHouseId");
			dtdetail.Columns.Add("WareHouseName");
			dtdetail.Columns.Add("JobLotId");
			dtdetail.Columns.Add("JobLot");
			dtdetail.Columns.Add("AddLssAmount", typeof(double));
			dtdetail.Columns.Add("Expenses", typeof(double));
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			dtShipmentExpenses.Columns.Add("AccountId");
			dtShipmentExpenses.Columns.Add("Remarks");
			dtShipmentExpenses.Columns.Add("FcAmount", typeof(double));
			dtShipmentExpenses.Columns.Add("LocalAmount", typeof(double));
			AddRowInShipmentExpensesGrid();
			grdShipmentExpenses.RetrieveStructure();
			GrdSettingShipmentExpenses();
			tabControl1.SelectedIndex = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInShipmentExpensesGrid()
	{
		dtShipmentExpenses.Rows.Add(0, "", 0, 0);
		grdShipmentExpenses.DataSource = dtShipmentExpenses;
	}

	private void GrdSettingShipmentExpenses()
	{
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Expected O, but got Unknown
		try
		{
			List<COAAllocation> Coalst = new List<COAAllocation>();
			if (dtAccountlst.Rows.Count > 0)
			{
				for (int i = 0; i < dtAccountlst.Rows.Count; i++)
				{
					if (Convert.ToInt32(dtAccountlst.Rows[i]["AccountTypeId"]) != 2 && Convert.ToInt32(dtAccountlst.Rows[i]["AccountTypeId"]) != 11 && Convert.ToInt32(dtAccountlst.Rows[i]["AccountTypeId"]) != 15)
					{
						COAAllocation cOAAllocation = new COAAllocation();
						cOAAllocation.Id = Conversion.ToInt(dtAccountlst.Rows[i]["Id"]);
						cOAAllocation.AccountTitle = dtAccountlst.Rows[i]["AccountTitle"].ToString();
						Coalst.Add(cOAAllocation);
					}
				}
			}
			grdShipmentExpenses.RootTable.Columns["AccountId"].ColumnType = (ColumnType)0;
			grdShipmentExpenses.RootTable.Columns["AccountId"].HasValueList = true;
			grdShipmentExpenses.RootTable.Columns["AccountId"].LimitToList = true;
			grdShipmentExpenses.RootTable.Columns["AccountId"].EditType = (EditType)4;
			grdShipmentExpenses.RootTable.Columns["AccountId"].Caption = "Account";
			grdShipmentExpenses.RootTable.Columns["AccountId"].ValueList.PopulateValueList((IEnumerable)Coalst, "Id", "AccountTitle");
			grdShipmentExpenses.RootTable.Columns["AccountId"].Width = 250;
			grdShipmentExpenses.RootTable.Columns["Remarks"].Width = 300;
			grdShipmentExpenses.RootTable.Columns.Add("Delete");
			grdShipmentExpenses.RootTable.Columns["Delete"].Key = "Delete";
			grdShipmentExpenses.RootTable.Columns["Delete"].Caption = "X";
			grdShipmentExpenses.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdShipmentExpenses.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdShipmentExpenses.RootTable.Columns["Delete"].ButtonText = "X";
			grdShipmentExpenses.RootTable.Columns["Delete"].Width = 20;
			grdShipmentExpenses.RootTable.Columns.Add("Add");
			grdShipmentExpenses.RootTable.Columns["Add"].Key = "Add";
			grdShipmentExpenses.RootTable.Columns["Add"].Caption = "+";
			grdShipmentExpenses.RootTable.Columns["Add"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdShipmentExpenses.RootTable.Columns["Add"].ButtonStyle = (ButtonStyle)5;
			grdShipmentExpenses.RootTable.Columns["Add"].ButtonText = "+";
			grdShipmentExpenses.RootTable.Columns["Add"].Width = 20;
			grdShipmentExpenses.RootTable.Columns["FcAmount"].AggregateFunction = (AggregateFunction)2;
			grdShipmentExpenses.RootTable.Columns["FcAmount"].TextAlignment = (TextAlignment)3;
			grdShipmentExpenses.RootTable.Columns["FcAmount"].FormatString = "#,#";
			grdShipmentExpenses.RootTable.Columns["FcAmount"].TotalFormatString = "#,#";
			grdShipmentExpenses.RootTable.Columns["LocalAmount"].AggregateFunction = (AggregateFunction)2;
			grdShipmentExpenses.RootTable.Columns["LocalAmount"].TextAlignment = (TextAlignment)3;
			grdShipmentExpenses.RootTable.Columns["LocalAmount"].FormatString = "#,#";
			grdShipmentExpenses.RootTable.Columns["LocalAmount"].TotalFormatString = "#,#";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ImProformaInvoice_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (btnsave.Visible && btnsave.Enabled && e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0 && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControl1.SelectedIndex == 0)
				{
					btnNew_Click(null, null);
				}
				else
				{
					btnNew_Click(null, null);
				}
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
			if (btnupdate.Visible && btnupdate.Enabled && e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 80;
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void FormReset()
	{
		try
		{
			btnsave.Visible = true;
			btnupdate.Visible = false;
			UpdateMode = false;
			RecId = 0;
			contractidInupdateCase = 0;
			txtdocno.Text = string.Empty;
			generateCode();
			((UltraGridBase)cmbbranches).Rows[0].Activate();
			((UltraGridBase)cmbproject).Rows[0].Activate();
			((UltraGridBase)cmbcareiertype).Rows[0].Activate();
			txtDocDate.Value = DateTime.Now;
			txtinvoiceno.Text = string.Empty;
			((Control)(object)cmbLcContractNo).Text = string.Empty;
			((Control)(object)cmbNotifyParty1).Text = string.Empty;
			((Control)(object)cmbNotifyParty2).Text = string.Empty;
			((Control)(object)cmbSupCust).Text = string.Empty;
			((Control)(object)cmbPaymentTerm).Text = string.Empty;
			((Control)(object)cmbimporterBankNew).Text = string.Empty;
			((Control)(object)cmbdeliverytermnew).Text = string.Empty;
			((Control)(object)cmbLoadingPort).Text = string.Empty;
			((Control)(object)cmbDestinationPort).Text = string.Empty;
			txtcertificate.Text = string.Empty;
			txtcertificateOfOrigion.Text = string.Empty;
			((Control)(object)CmbHomeCurrency).Text = string.Empty;
			txtLotRef.Text = string.Empty;
			((Control)(object)cmbexporterBankNew).Text = string.Empty;
			((Control)(object)cmbfcycode).Text = string.Empty;
			txtfcyAmount.Text = string.Empty;
			txtExRate.Text = string.Empty;
			txtLocalAmt.Text = string.Empty;
			txtGrossWeight.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtNoOfContainer.Text = string.Empty;
			txtIformNo.Text = string.Empty;
			txtaddlesscommnets.Text = string.Empty;
			txtaddlessamount.Text = string.Empty;
			txtTotalNetAmount.Text = string.Empty;
			((Control)(object)cmbLcContractNo).Text = string.Empty;
			((UltraGridBase)cmbLcContractNo).DataSource = null;
			dtdetail.Rows.Clear();
			grdDetail.ClearStructure();
			txtinvoiceno.Focus();
			dtShipmentExpenses.Rows.Clear();
			AddRowInShipmentExpensesGrid();
			grdShipmentExpenses.RetrieveStructure();
			GrdSettingShipmentExpenses();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0522: Unknown result type (might be due to invalid IL or missing references)
		//IL_0529: Expected O, but got Unknown
		//IL_0840: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid Record not found");
			}
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			GridEXRow[] rows = grdShipmentExpenses.GetRows();
			foreach (GridEXRow r3 in rows)
			{
				if (Conversion.ToDouble(r3.Cells["FcAmount"].Value) > 0.0 && Conversion.ToInt(r3.Cells["LocalAmount"].Value) > 0 && Conversion.ToInt(r3.Cells["AccountId"].Value) == 0)
				{
					throw new Exception("Please Select an Account Against JL First");
				}
			}
			double NetWeightkg = 0.0;
			NetWeightkg = Conversion.ToDouble(grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2)) * 1000.0;
			txtNetWeight.Text = NetWeightkg.ToString();
			ImInvoice PIH = new ImInvoice();
			if (!formvalidation())
			{
				return;
			}
			if (RecId > 0)
			{
				PIH.Id = RecId;
			}
			PIH.OrganizationId = UserAccount.OrganizationId;
			PIH.CompanyId = UserAccount.CompanyId;
			PIH.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			PIH.IsApproved = false;
			PIH.EntryDate = DateTime.Now;
			PIH.EntryUser = UserAccount.ID;
			PIH.ModifyDate = DateTime.Now;
			PIH.ModifyUser = UserAccount.ID;
			PIH.ApprovedDate = DateTime.Now;
			PIH.DocumentTypeId = DocumentTypeId;
			PIH.BranchesId = Conversion.ToInt(cmbbranches.Value);
			PIH.ProjectsId = Conversion.ToInt(cmbproject.Value);
			PIH.DocCode = Conversion.ToInt((object)txtdocno.Text);
			PIH.DocDate = Conversion.ToDateTime((object)txtDocDate.Value);
			PIH.InvoiceNo = Conversion.ToString((object)txtinvoiceno.Text.Trim());
			PIH.SupplierCustomerId = Conversion.ToInt(cmbSupCust.Value);
			PIH.LcOrderNoId = Conversion.ToInt(cmbLcContractNo.Value);
			PIH.NotifyParty1 = Conversion.ToInt(cmbNotifyParty1.Value);
			PIH.NotifyParty2 = Conversion.ToInt(cmbNotifyParty2.Value);
			PIH.LotNoRef = Conversion.ToString((object)txtLotRef.Text);
			PIH.PaymentTermId = Conversion.ToInt(cmbPaymentTermsNew.Value);
			PIH.ImporterBankId = Conversion.ToInt(cmbimporterBankNew.Value);
			PIH.ExporteBankId = Conversion.ToInt(cmbexporterBankNew.Value);
			PIH.DeliveryTermId = Conversion.ToInt(cmbdeliverytermnew.Value);
			PIH.LoadingPortId = Conversion.ToInt(cmbLoadingPort.Value);
			PIH.DestinationPortId = Conversion.ToInt(cmbDestinationPort.Value);
			PIH.CarierType = Conversion.ToString((object)((Control)(object)cmbcareiertype).Text);
			PIH.HomeCurrencyId = Conversion.ToInt(CmbHomeCurrency.Value);
			PIH.FcurrencyId = Conversion.ToInt(cmbfcycode.Value);
			PIH.FCurrencyAmount = Conversion.ToDouble((object)txtfcyAmount.Text.Trim());
			PIH.ConversionRate = Conversion.ToDouble((object)txtExRate.Text.Trim());
			PIH.EquivalentAmount = Conversion.ToDouble((object)txtLocalAmt.Text.Trim());
			PIH.GrossWeight = Conversion.ToDouble((object)txtGrossWeight.Text.Trim());
			PIH.NetWeight = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
			PIH.NoOfContainers = Conversion.ToInt((object)txtNoOfContainer.Text.Trim());
			PIH.EFormNo = Conversion.ToString((object)txtIformNo.Text.Trim());
			PIH.EFormDate = Conversion.ToDateTime((object)txtIformDate.Value);
			PIH.Certificate1 = Conversion.ToString((object)txtcertificate.Text.Trim());
			PIH.Certificate2 = Conversion.ToString((object)txtcertificateOfOrigion.Text.Trim());
			PIH.AddLessComments = Conversion.ToString((object)txtaddlesscommnets.Text);
			PIH.AddLessAmount = Conversion.ToDouble((object)txtaddlessamount.Text.Trim());
			PIH.TotalAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
			PIH.ExImInvoiceDetail = new List<ImInvoicePackingDetail>();
			PIH.ImInvoiceExpenseslst = new List<ImInvoiceExpenses>();
			rows = grdDetail.GetRows();
			foreach (GridEXRow r2 in rows)
			{
				ImInvoicePackingDetail vd2 = new ImInvoicePackingDetail();
				vd2.ItemId = Conversion.ToInt(r2.Cells["ItemId"].Value);
				if (vd2.ItemId == 0)
				{
					throw new Exception("ItemName field required");
				}
				vd2.PackingMaterialTypeId = Conversion.ToInt(r2.Cells["PackTypeId"].Value);
				vd2.NetWeight = Conversion.ToDouble(r2.Cells["Qty/M.Ton"].Value) * 1000.0;
				if (vd2.NetWeight == 0.0)
				{
					throw new Exception("Qty/M.Ton field required");
				}
				vd2.OuterQtyUomId = Conversion.ToInt(r2.Cells["PackSizeId"].Value);
				if (vd2.OuterQtyUomId == 0)
				{
					throw new Exception("PackUom field required");
				}
				vd2.OuterQty = Conversion.ToDouble(r2.Cells["NoOfBags"].Value);
				if (vd2.OuterQty == 0.0)
				{
					throw new Exception("NoOfBags field required");
				}
				vd2.InnerQtyUomId = Conversion.ToInt(r2.Cells["PackSizeId"].Value);
				vd2.InnerQty = Conversion.ToInt(r2.Cells["NoOfBags"].Value);
				vd2.RatePrice = Conversion.ToDouble(r2.Cells["Cost/M.Ton"].Value);
				if (vd2.RatePrice == 0.0)
				{
					throw new Exception("RatePrice field required");
				}
				vd2.RateUomId = Conversion.ToInt(r2.Cells["RateUOMId"].Value);
				if (vd2.RateUomId == 0)
				{
					throw new Exception("RateUom field required");
				}
				vd2.FcAmount = Conversion.ToDouble(r2.Cells["Amount"].Value);
				if (vd2.FcAmount == 0.0)
				{
					throw new Exception("FcAmount field required");
				}
				vd2.WarehouseId = Conversion.ToInt(r2.Cells["WareHouseId"].Value);
				if (vd2.WarehouseId == 0)
				{
					throw new Exception("Warehouse field required");
				}
				vd2.JobLotId = Conversion.ToInt(r2.Cells["JobLotId"].Value);
				if (vd2.JobLotId == 0)
				{
					throw new Exception("JobLot field required");
				}
				vd2.ItemDescriptionManual = Conversion.ToString((object)r2.Cells["ItemDetail"].Text);
				vd2.AddLessAmount = Conversion.ToDouble(r2.Cells["AddLssAmount"].Value);
				vd2.Expenses = Conversion.ToDouble(r2.Cells["Expenses"].Value);
				PIH.ExImInvoiceDetail.Add(vd2);
			}
			rows = grdShipmentExpenses.GetRows();
			foreach (GridEXRow r in rows)
			{
				ImInvoiceExpenses vd = new ImInvoiceExpenses();
				vd.AccountId = Conversion.ToInt(r.Cells["AccountId"].Value);
				if (vd.AccountId == 0)
				{
					throw new Exception("AccountTitle field required");
				}
				vd.Remarks = Conversion.ToString((object)r.Cells["Remarks"].Text);
				vd.FcAmount = Conversion.ToDouble(r.Cells["FcAmount"].Value);
				if (vd.FcAmount == 0.0)
				{
					throw new Exception("FcAmount field required");
				}
				vd.LocalAmount = Conversion.ToDouble(r.Cells["LocalAmount"].Value);
				if (vd.LocalAmount == 0.0)
				{
					throw new Exception("LocalAmount field required");
				}
				PIH.ImInvoiceExpenseslst.Add(vd);
			}
			int success = ImInvoice.Save(PIH);
			if (!UpdateMode && success > 0)
			{
				MessageBox.Show("Save SuccessFully");
			}
			if (UpdateMode && success > 0)
			{
				MessageBox.Show("Update SuccessFully");
			}
			FormReset();
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
			btnsave_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		ReadById(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString()));
	}

	private void ReadById(int Id)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		try
		{
			btnsave.Visible = false;
			btnupdate.Visible = true;
			ImInvoice PIH = new ImInvoice();
			RecId = Id;
			PIH = ImInvoice.GetByID(RecId);
			if (PIH != null)
			{
				tabControl1.SelectedIndex = 0;
				cmbbranches.Value = PIH.BranchesId;
				cmbproject.Value = PIH.ProjectsId;
				txtdocno.Text = PIH.DocCode.ToString();
				txtDocDate.Value = Conversion.ToDateTime((object)PIH.DocDate);
				txtinvoiceno.Text = PIH.InvoiceNo.ToString();
				cmbSupCust.Value = PIH.SupplierCustomerId;
				contractidInupdateCase = PIH.LcOrderNoId;
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("OrderNo");
				dt.Rows.Add(PIH.LcOrderNoId, PIH.PurchaseOrderNo);
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbLcContractNo, "Id", "OrderNo", "Order No", ZeroIndex: false);
					((UltraGridBase)cmbLcContractNo).Rows[0].Activate();
				}
				cmbNotifyParty1.Value = PIH.NotifyParty1;
				cmbNotifyParty2.Value = PIH.NotifyParty2;
				txtLotRef.Text = PIH.LotNoRef;
				cmbPaymentTermsNew.Value = PIH.PaymentTermId;
				cmbimporterBankNew.Value = PIH.ImporterBankId;
				cmbexporterBankNew.Value = PIH.ExporteBankId;
				cmbdeliverytermnew.Value = PIH.DeliveryTermId;
				cmbLoadingPort.Value = PIH.LoadingPortId;
				cmbDestinationPort.Value = PIH.DestinationPortId;
				((Control)(object)cmbcareiertype).Text = PIH.CarierType;
				cmbfcycode.Value = PIH.FcurrencyId;
				CmbHomeCurrency.Value = PIH.HomeCurrencyId;
				txtfcyAmount.Text = PIH.FCurrencyAmount.ToString();
				txtExRate.Text = PIH.ConversionRate.ToString();
				txtLocalAmt.Text = PIH.EquivalentAmount.ToString();
				txtGrossWeight.Text = PIH.GrossWeight.ToString();
				txtNetWeight.Text = PIH.NetWeight.ToString();
				txtNoOfContainer.Text = PIH.NoOfContainers.ToString();
				txtIformNo.Text = PIH.EFormNo;
				txtIformDate.Value = PIH.EFormDate;
				txtcertificate.Text = PIH.Certificate1;
				txtcertificateOfOrigion.Text = PIH.Certificate2;
				txtaddlesscommnets.Text = PIH.AddLessComments;
				txtaddlessamount.Text = PIH.AddLessAmount.ToString();
				txtTotalNetAmount.Text = PIH.TotalAmount.ToString();
				dtdetail.Clear();
				grdDetail.ClearStructure();
				grdDetail.DataSource = null;
				dtdetail.Rows.Clear();
				for (int j = 0; j < PIH.ExImInvoiceDetail.Count; j++)
				{
					dtdetail.Rows.Add(PIH.ExImInvoiceDetail[j].ItemId, PIH.ExImInvoiceDetail[j].ItemName, PIH.ExImInvoiceDetail[j].PackingMaterialTypeId, PIH.ExImInvoiceDetail[j].PackMaterilaType, PIH.ExImInvoiceDetail[j].NetWeight / 1000.0, PIH.ExImInvoiceDetail[j].OuterQtyUomId, PIH.ExImInvoiceDetail[j].OuterUOM, PIH.ExImInvoiceDetail[j].OuterQty, PIH.ExImInvoiceDetail[j].RatePrice, PIH.ExImInvoiceDetail[j].RateUomId, PIH.ExImInvoiceDetail[j].RateUOM, PIH.ExImInvoiceDetail[j].FcAmount, PIH.ExImInvoiceDetail[j].ItemDescriptionManual, PIH.ExImInvoiceDetail[j].WarehouseId, PIH.ExImInvoiceDetail[j].WareHouseName, PIH.ExImInvoiceDetail[j].JobLotId, PIH.ExImInvoiceDetail[j].JobLotDescription, PIH.ExImInvoiceDetail[j].AddLessAmount, PIH.ExImInvoiceDetail[j].Expenses);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				dtShipmentExpenses.Rows.Clear();
				for (int i = 0; i < PIH.ImInvoiceExpenseslst.Count; i++)
				{
					dtShipmentExpenses.Rows.Add(PIH.ImInvoiceExpenseslst[i].AccountId, PIH.ImInvoiceExpenseslst[i].Remarks, PIH.ImInvoiceExpenseslst[i].FcAmount, PIH.ImInvoiceExpenseslst[i].LocalAmount);
				}
				grdShipmentExpenses.DataSource = dtShipmentExpenses;
				grdShipmentExpenses.RetrieveStructure();
				GrdSettingShipmentExpenses();
				if (dtShipmentExpenses.Rows.Count == 0)
				{
					AddRowInShipmentExpensesGrid();
				}
				UpdateMode = true;
				BillAmount();
				CalculateLocalAmount();
			}
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
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((UltraGridBase)cmbbranches).ActiveRow == null)
		{
			MessageBox.Show("Branch Is Required");
			cmbbranches.Focus();
			return false;
		}
		if (((UltraGridBase)cmbproject).ActiveRow == null)
		{
			MessageBox.Show("Project Is Required");
			cmbproject.Focus();
			return false;
		}
		if (txtdocno.Text == "" || Conversion.ToInt((object)txtdocno.Text) == 0)
		{
			MessageBox.Show("Doc No Is Required");
			txtdocno.Focus();
			return false;
		}
		if (txtinvoiceno.Text == "" || txtinvoiceno.Text.Trim() == "0")
		{
			MessageBox.Show("Invoice No Is Required");
			txtinvoiceno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSupCust).ActiveRow == null)
		{
			MessageBox.Show("Customer Is Required");
			cmbSupCust.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLcContractNo).ActiveRow == null)
		{
			MessageBox.Show("Contract No  Is Required");
			cmbLcContractNo.Focus();
			return false;
		}
		if (((UltraGridBase)cmbPaymentTermsNew).ActiveRow == null)
		{
			MessageBox.Show("Payment Term Is Required");
			cmbPaymentTermsNew.Focus();
			return false;
		}
		if (((UltraGridBase)cmbdeliverytermnew).ActiveRow == null)
		{
			MessageBox.Show("Delivery Term Is Required");
			cmbdeliverytermnew.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoadingPort).ActiveRow == null)
		{
			MessageBox.Show("Loading Port Is Required");
			cmbLoadingPort.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDestinationPort).ActiveRow == null)
		{
			MessageBox.Show("Destination Port  Is Required");
			cmbDestinationPort.Focus();
			return false;
		}
		if (((UltraGridBase)cmbcareiertype).ActiveRow == null)
		{
			MessageBox.Show("Carier Type  Is Required");
			cmbcareiertype.Focus();
			return false;
		}
		if (((UltraGridBase)cmbfcycode).ActiveRow == null)
		{
			MessageBox.Show("Fcy Code Is Required");
			cmbfcycode.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtfcyAmount.Text) == 0.0)
		{
			MessageBox.Show("Fcy Amount  Is Required");
			txtfcyAmount.Focus();
			return false;
		}
		if (((UltraGridBase)CmbHomeCurrency).ActiveRow == null)
		{
			MessageBox.Show("HomeCurrency field Required");
			CmbHomeCurrency.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtExRate.Text) == 0.0)
		{
			MessageBox.Show("Exchange Rate  Is Required");
			txtExRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtLocalAmt.Text) == 0.0)
		{
			MessageBox.Show("LocalAmount Is Required");
			txtLocalAmt.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtNetWeight.Text) > 0.0))
		{
			MessageBox.Show("Net Weight  Is Required");
			txtNetWeight.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtGrossWeight.Text) >= Conversion.ToDouble((object)txtNetWeight.Text)))
		{
			MessageBox.Show("Gross Weight Must Be Equal Or Greater Than Net Weight Thank You");
			txtGrossWeight.Focus();
			return false;
		}
		if (txtIformNo.Text == "" || txtIformNo.Text == "0")
		{
			MessageBox.Show("I-Form No Is Required");
			txtIformNo.Focus();
			return false;
		}
		if (!(Conversion.ToDouble((object)txtTotalNetAmount.Text) > 0.0))
		{
			MessageBox.Show("Total Amount  Is Required");
			txtTotalNetAmount.Focus();
			return false;
		}
		return true;
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			HistoryGridFill();
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			DataTable dtgrd = ImInvoice.GetData(new ImInvoice
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("InvoiceNo");
			dt.Columns.Add("DocCode");
			dt.Columns.Add("DocDate");
			dt.Columns.Add("ContractNo");
			dt.Columns.Add("ContractDocNo");
			dt.Columns.Add("Supplier");
			dt.Columns.Add("NoOfContainers");
			dt.Columns.Add("GrossWeight");
			dt.Columns.Add("NetWeight");
			dt.Columns.Add("FcyAmount");
			dt.Columns.Add("Add/Less");
			dt.Columns.Add("TotalAmount");
			dt.Columns.Add("LoadingPort");
			dt.Columns.Add("DestinationPort");
			for (int i = 0; i < dtgrd.Rows.Count; i++)
			{
				dt.Rows.Add(dtgrd.Rows[i]["Id"], dtgrd.Rows[i]["InvoiceNo"], dtgrd.Rows[i]["DocCode"], dtgrd.Rows[i]["DocDate"], dtgrd.Rows[i]["LcOrderNo"], dtgrd.Rows[i]["LcOrderDocNo"], dtgrd.Rows[i]["Customer"], dtgrd.Rows[i]["NoOfContainers"], dtgrd.Rows[i]["GrossWeight"], dtgrd.Rows[i]["NetWeight"], dtgrd.Rows[i]["FCurrencyAmount"], dtgrd.Rows[i]["AddLessAmount"], dtgrd.Rows[i]["TotalAmount"], dtgrd.Rows[i]["LoadingPort"], dtgrd.Rows[i]["DestinationPort"]);
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
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
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["InvoiceNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocCode"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DocDate"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ContractNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["ContractDocNo"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Supplier"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["NoOfContainers"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["GrossWeight"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["Add/Less"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["TotalAmount"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["LoadingPort"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["DestinationPort"].EditType = (EditType)0;
			DataGridHistory.RootTable.Columns["InvoiceNo"].Width = 100;
			DataGridHistory.RootTable.Columns["DocDate"].Width = 100;
			DataGridHistory.RootTable.Columns["InvoiceNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocCode"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			DataGridHistory.RootTable.Columns["ContractNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["ContractDocNo"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Supplier"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["NoOfContainers"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["GrossWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["NetWeight"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["FcyAmount"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["Add/Less"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["TotalAmount"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["LoadingPort"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["DestinationPort"].FilterEditType = (FilterEditType)1;
			DataGridHistory.RootTable.Columns["NoOfContainers"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["GrossWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["TotalAmount"].AggregateFunction = (AggregateFunction)2;
			DataGridHistory.RootTable.Columns["NoOfContainers"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["GrossWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["NetWeight"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["FcyAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns["TotalAmount"].FormatString = "0,0";
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
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
			if (File.Exists(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name))
			{
				FileStream fs = new FileStream(Application.ExecutablePath + "\\..\\Layouts\\" + base.Name + "_" + ((Control)(object)DataGridHistory).Name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			grdDetail.RootTable.Columns["ItemName"].Width = 220;
			grdDetail.RootTable.Columns["PackType"].Width = 150;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["PackTypeId"].Visible = false;
			grdDetail.RootTable.Columns["PackSizeId"].Visible = false;
			grdDetail.RootTable.Columns["RateUOMId"].Visible = false;
			grdDetail.RootTable.Columns["AddLssAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackTypeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackSizeId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["RateUOMId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackType"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["PackSize"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["NoOfBags"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Cost/M.Ton"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["RateUOM"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Amount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Expenses"].EditType = (EditType)0;
			if (DocumentTypeId == 237)
			{
				grdDetail.AllowDelete = (InheritableBoolean)1;
				grdDetail.RootTable.Columns["WareHouseName"].Visible = false;
				grdDetail.RootTable.Columns["JobLot"].Visible = false;
				grdDetail.RootTable.Columns["WareHouseId"].ColumnType = (ColumnType)0;
				grdDetail.RootTable.Columns["WareHouseId"].HasValueList = true;
				grdDetail.RootTable.Columns["WareHouseId"].LimitToList = true;
				grdDetail.RootTable.Columns["WareHouseId"].EditType = (EditType)4;
				grdDetail.RootTable.Columns["WareHouseId"].Caption = "WareHouseName";
				grdDetail.RootTable.Columns["WareHouseId"].ValueList.PopulateValueList((IEnumerable)dtWarehouse.DefaultView, "Id", "WareHouseName");
				grdDetail.RootTable.Columns["JobLotId"].ColumnType = (ColumnType)0;
				grdDetail.RootTable.Columns["JobLotId"].HasValueList = true;
				grdDetail.RootTable.Columns["JobLotId"].LimitToList = true;
				grdDetail.RootTable.Columns["JobLotId"].EditType = (EditType)4;
				grdDetail.RootTable.Columns["JobLotId"].Caption = "JobLot";
				grdDetail.RootTable.Columns["JobLotId"].ValueList.PopulateValueList((IEnumerable)dtJobLot.DefaultView, "Id", "JobLotDescription");
				grdDetail.RootTable.Columns["JobLotId"].Width = 180;
			}
			else
			{
				grdDetail.AllowDelete = (InheritableBoolean)2;
				grdDetail.RootTable.Columns["WareHouseId"].EditType = (EditType)0;
				grdDetail.RootTable.Columns["JobLotId"].EditType = (EditType)0;
				grdDetail.RootTable.Columns["WareHouseId"].Visible = false;
				grdDetail.RootTable.Columns["JobLotId"].Visible = false;
				grdDetail.RootTable.Columns["Qty/M.Ton"].EditType = (EditType)0;
				grdDetail.RootTable.Columns["WareHouseName"].EditType = (EditType)0;
				grdDetail.RootTable.Columns["JobLot"].EditType = (EditType)0;
			}
			grdDetail.RootTable.Columns["Qty/M.Ton"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Qty/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Qty/M.Ton"].FormatString = "#,#.###";
			grdDetail.RootTable.Columns["Qty/M.Ton"].TotalFormatString = "#,#.###";
			grdDetail.RootTable.Columns["AddLssAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["AddLssAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["AddLssAmount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["AddLssAmount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Expenses"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Expenses"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Expenses"].FormatString = "#,#.##";
			grdDetail.RootTable.Columns["Expenses"].TotalFormatString = "#,#.##";
			grdDetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["NoOfBags"].FormatString = "0,0";
			grdDetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Amount"].FormatString = "0,0";
			grdDetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0.##";
			grdDetail.RootTable.Columns["Cost/M.Ton"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["Cost/M.Ton"].FormatString = "0,0";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateLocalAmount()
	{
		try
		{
			double fcyAmount = Conversion.ToDouble((object)txtTotalNetAmount.Text);
			double ExchangeRate = Conversion.ToDouble((object)txtExRate.Text);
			if (fcyAmount > 0.0 && ExchangeRate > 0.0)
			{
				txtLocalAmt.Text = (fcyAmount * ExchangeRate).ToString("#,#");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtExRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateLocalAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BillAmount()
	{
		try
		{
			double BillAmount = 0.0;
			double ItemAmount = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["Amount"], (AggregateFunction)2).ToString());
			ItemAmount = Math.Round(ItemAmount);
			double TransporterCredit = 0.0;
			GridEXRow[] rows = grdShipmentExpenses.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 && Conversion.ToInt(((UltraDropDownBase)cmbSupCust).SelectedRow.Cells[2].Value) == Conversion.ToInt(r.Cells["AccountId"].Value))
				{
					TransporterCredit += Conversion.ToDouble((object)r.Cells["FcAmount"].Value.ToString());
				}
			}
			BillAmount = BillAmount + ItemAmount + TransporterCredit + Conversion.ToDouble((object)txtaddlessamount.Text.Trim());
			txtTotalNetAmount.Text = Math.Round(BillAmount).ToString("#,#");
			CalculateLocalAmount();
			ShipmentExpensesProportion();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbSupCust_Leave_1(object sender, EventArgs e)
	{
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			if (DocumentTypeId == 237)
			{
				if (Conversion.ToInt(cmbSupCust.Value) <= 0)
				{
					return;
				}
				((UltraGridBase)cmbLcContractNo).DataSource = null;
				((Control)(object)cmbLcContractNo).Text = string.Empty;
				DataTable dtlcorder = ImLcOrder.GetLcOrderNo(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 231
				});
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("LcOrderNo");
				if (dtlcorder.Rows.Count <= 0)
				{
					return;
				}
				for (int i = 0; i < dtlcorder.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtlcorder.Rows[i]["SupCustId"]) == Conversion.ToInt(cmbSupCust.Value))
					{
						dt.Rows.Add(dtlcorder.Rows[i]["Id"], dtlcorder.Rows[i]["LcOrderNo"]);
					}
				}
				DDL.BindDDL(dt, cmbLcContractNo, "Id", "LcOrderNo", "Contract No", ZeroIndex: false);
			}
			else
			{
				if (DocumentTypeId != 233)
				{
					return;
				}
				DataTable dtlcorder2 = ExImLcOrderPurchaseOrder.GetPurchaseOrderNo(new ExImLcOrderPurchaseOrder
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					DocumentTypeId = 232
				});
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("OrderNo");
				if (dtlcorder2.Rows.Count <= 0)
				{
					return;
				}
				for (int j = 0; j < dtlcorder2.Rows.Count; j++)
				{
					if (Conversion.ToInt(dtlcorder2.Rows[j]["SupplierCustomerId"]) == Conversion.ToInt(cmbSupCust.Value))
					{
						dt2.Rows.Add(dtlcorder2.Rows[j]["Id"], dtlcorder2.Rows[j]["OrderNo"]);
					}
				}
				DDL.BindDDL(dt2, cmbLcContractNo, "Id", "OrderNo", "OrderNo", ZeroIndex: false);
			}
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

	private void txtaddlessamount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			double ShipmentExpenses = 0.0;
			double AddlessAmount = Conversion.ToDouble((object)txtaddlessamount.Text);
			double FcyAmount = Conversion.ToDouble((object)txtfcyAmount.Text);
			if (FcyAmount > 0.0)
			{
				if (AddlessAmount.ToString() != string.Empty)
				{
					GridEXRow[] rows = grdShipmentExpenses.GetRows();
					foreach (GridEXRow r in rows)
					{
						if (Conversion.ToInt(r.Cells["AccountId"].Value) > 0 && Conversion.ToDouble(r.Cells["FcAmount"].Value) > 0.0 && Conversion.ToInt(((UltraDropDownBase)cmbSupCust).SelectedRow.Cells[2].Value) == Conversion.ToInt(r.Cells["AccountId"].Value))
						{
							ShipmentExpenses += Conversion.ToDouble((object)r.Cells["FcAmount"].Value.ToString());
						}
					}
					double netTotalAmount = FcyAmount + ShipmentExpenses + AddlessAmount;
					txtTotalNetAmount.Text = netTotalAmount.ToString();
				}
				else
				{
					double netTotalAmount2 = FcyAmount;
					txtTotalNetAmount.Text = netTotalAmount2.ToString("#,#");
				}
			}
			CalculateLocalAmount();
			ParporationHomeCurrencyAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtfcyAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			txtaddlessamount_TextChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParporationHomeCurrencyAmount()
	{
		try
		{
			grdDetail.UpdateData();
			double num = Conversion.ToDouble((object)txtTotalNetAmount.Text.Trim());
			double NetWeight = Conversion.ToDouble((object)txtNetWeight.Text.Trim());
			if (num > 0.0 && NetWeight > 0.0)
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double AddLessAmount = Conversion.ToDouble((object)txtaddlessamount.Text.Trim());
					double DetailWeight = Conversion.ToDouble(obj.Cells["Qty/M.Ton"].Value) * 1000.0;
					obj.Cells["AddLssAmount"].Value = (AddLessAmount / NetWeight * DetailWeight).ToString();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["AddLssAmount"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdShipmentExpenses_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				GridEXRow item = grdShipmentExpenses.CurrentRow;
				dtShipmentExpenses.Rows[item.RowIndex].Delete();
				if (item != null && (int)item.RowType == 0)
				{
					item.Delete();
				}
				if (dtShipmentExpenses.Rows.Count == 0)
				{
					AddRowInShipmentExpensesGrid();
				}
			}
			if (e.Column.Key == "Add")
			{
				AddRowInShipmentExpensesGrid();
			}
			grdShipmentExpenses.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdShipmentExpenses_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			double ExchangeRate = Conversion.ToDouble((object)txtExRate.Text.Trim());
			if (ExchangeRate > 0.0)
			{
				grdShipmentExpenses.UpdateData();
				GridEXRow item = grdShipmentExpenses.CurrentRow;
				if (e.Column.Key == "FcAmount" && item != null && (int)item.RowType == 0 && item.Cells["FcAmount"].Value.ToString() != string.Empty && ExchangeRate > 0.0)
				{
					double FcAmount = Conversion.ToDouble((object)item.Cells["FcAmount"].Value.ToString());
					item.Cells["LocalAmount"].Value = FcAmount * ExchangeRate;
				}
				if (e.Column.Key == "LocalAmount" && item != null && (int)item.RowType == 0 && item.Cells["LocalAmount"].Value.ToString() != string.Empty && ExchangeRate > 0.0)
				{
					double LocalAmount = Conversion.ToDouble((object)item.Cells["LocalAmount"].Value.ToString());
					item.Cells["FcAmount"].Value = LocalAmount / ExchangeRate;
				}
			}
			grdShipmentExpenses.UpdateData();
			BillAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ShipmentExpensesProportion()
	{
		try
		{
			double NetWeight = Conversion.ToDouble((object)grdDetail.GetTotal(grdDetail.RootTable.Columns["Qty/M.Ton"], (AggregateFunction)2).ToString());
			double CreditForFreight = Conversion.ToDouble((object)grdShipmentExpenses.GetTotal(grdShipmentExpenses.RootTable.Columns["FcAmount"], (AggregateFunction)2).ToString());
			if (CreditForFreight > 0.0)
			{
				double TotalDiffernce = CreditForFreight;
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj in rows)
				{
					obj.BeginEdit();
					double Weight = Conversion.ToDouble((object)obj.Cells["Qty/M.Ton"].Value.ToString());
					obj.Cells["Expenses"].Value = Math.Round(TotalDiffernce) / NetWeight * Weight;
					obj.EndEdit();
				}
			}
			else
			{
				GridEXRow[] rows = grdDetail.GetRows();
				foreach (GridEXRow obj2 in rows)
				{
					obj2.BeginEdit();
					obj2.Cells["Expenses"].Value = 0;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Expected O, but got Unknown
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = ImportReports.ImInvoiceSlip_806(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = RecId,
				ApprovedFilter = "All"
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "806-ImInvoiceSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ImportReports.ImInvoiceSlip_806(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ApprovedFilter = "All",
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value)
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("Not Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "806-ImInvoiceSlip.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			grdDetail.UpdateData();
			if (e.Column.Key == "Qty/M.Ton")
			{
				r.BeginEdit();
				double MTon = Conversion.ToDouble(r.Cells["Qty/M.Ton"].Value);
				double PackSize = Conversion.ToDouble(r.Cells["PackSize"].Value);
				double RateUOM = Conversion.ToDouble(r.Cells["RateUOM"].Value);
				double ItemRate = Conversion.ToDouble(r.Cells["Cost/M.Ton"].Value);
				double NoOfBags = MTon * PackSize;
				double ItemAmount = 0.0;
				r.Cells["NoOfBags"].Value = NoOfBags.ToString();
				if (MTon > 0.0 && RateUOM > 0.0 && ItemRate > 0.0)
				{
					ItemAmount = MTon * 1000.0 / RateUOM * ItemRate;
					r.Cells["Amount"].Value = ItemAmount.ToString();
				}
				else
				{
					r.Cells["Amount"].Value = ItemAmount.ToString();
				}
				getTotalnetWeightFromGrid();
				r.EndEdit();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_RecordsDeleted(object sender, EventArgs e)
	{
		try
		{
			getTotalnetWeightFromGrid();
			ShipmentExpensesProportion();
			ParporationHomeCurrencyAmount();
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
		//IL_0507: Unknown result type (might be due to invalid IL or missing references)
		//IL_050e: Expected O, but got Unknown
		//IL_050e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0515: Expected O, but got Unknown
		//IL_0515: Unknown result type (might be due to invalid IL or missing references)
		//IL_051c: Expected O, but got Unknown
		//IL_051c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0523: Expected O, but got Unknown
		//IL_0523: Unknown result type (might be due to invalid IL or missing references)
		//IL_052a: Expected O, but got Unknown
		//IL_052a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Expected O, but got Unknown
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0538: Expected O, but got Unknown
		//IL_0538: Unknown result type (might be due to invalid IL or missing references)
		//IL_053f: Expected O, but got Unknown
		//IL_053f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Expected O, but got Unknown
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_054d: Expected O, but got Unknown
		//IL_054d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0554: Expected O, but got Unknown
		//IL_0554: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Expected O, but got Unknown
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0562: Expected O, but got Unknown
		//IL_0562: Unknown result type (might be due to invalid IL or missing references)
		//IL_0569: Expected O, but got Unknown
		//IL_0569: Unknown result type (might be due to invalid IL or missing references)
		//IL_0570: Expected O, but got Unknown
		//IL_0570: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Expected O, but got Unknown
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Expected O, but got Unknown
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Expected O, but got Unknown
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0593: Expected O, but got Unknown
		//IL_0593: Unknown result type (might be due to invalid IL or missing references)
		//IL_059a: Expected O, but got Unknown
		//IL_059a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a1: Expected O, but got Unknown
		//IL_05a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		//IL_05a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05af: Expected O, but got Unknown
		//IL_05af: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Expected O, but got Unknown
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Expected O, but got Unknown
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Expected O, but got Unknown
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cb: Expected O, but got Unknown
		//IL_05cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d2: Expected O, but got Unknown
		//IL_05d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d9: Expected O, but got Unknown
		//IL_05d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e0: Expected O, but got Unknown
		//IL_05e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e7: Expected O, but got Unknown
		//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ee: Expected O, but got Unknown
		//IL_05ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f5: Expected O, but got Unknown
		//IL_05f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05fc: Expected O, but got Unknown
		//IL_05fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0603: Expected O, but got Unknown
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
		//IL_06b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c0: Expected O, but got Unknown
		//IL_06c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c7: Expected O, but got Unknown
		//IL_06c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Expected O, but got Unknown
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Expected O, but got Unknown
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Expected O, but got Unknown
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ea: Expected O, but got Unknown
		//IL_06ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f1: Expected O, but got Unknown
		//IL_06f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f8: Expected O, but got Unknown
		//IL_06f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ff: Expected O, but got Unknown
		//IL_06ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Expected O, but got Unknown
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0713: Expected O, but got Unknown
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071e: Expected O, but got Unknown
		//IL_0720: Unknown result type (might be due to invalid IL or missing references)
		//IL_0729: Expected O, but got Unknown
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0734: Expected O, but got Unknown
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073f: Expected O, but got Unknown
		//IL_0741: Unknown result type (might be due to invalid IL or missing references)
		//IL_074a: Expected O, but got Unknown
		//IL_074c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0755: Expected O, but got Unknown
		//IL_0757: Unknown result type (might be due to invalid IL or missing references)
		//IL_0760: Expected O, but got Unknown
		//IL_0762: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Expected O, but got Unknown
		//IL_076d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0776: Expected O, but got Unknown
		//IL_0778: Unknown result type (might be due to invalid IL or missing references)
		//IL_0781: Expected O, but got Unknown
		//IL_0783: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Expected O, but got Unknown
		//IL_078e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0797: Expected O, but got Unknown
		//IL_0799: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Expected O, but got Unknown
		//IL_07a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_07af: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Expected O, but got Unknown
		//IL_07ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c3: Expected O, but got Unknown
		//IL_07c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ce: Expected O, but got Unknown
		//IL_07d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d9: Expected O, but got Unknown
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07e4: Expected O, but got Unknown
		//IL_07e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ef: Expected O, but got Unknown
		//IL_07f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fa: Expected O, but got Unknown
		//IL_07fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0805: Expected O, but got Unknown
		//IL_0807: Unknown result type (might be due to invalid IL or missing references)
		//IL_0810: Expected O, but got Unknown
		//IL_0812: Unknown result type (might be due to invalid IL or missing references)
		//IL_081b: Expected O, but got Unknown
		//IL_081d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0826: Expected O, but got Unknown
		//IL_0828: Unknown result type (might be due to invalid IL or missing references)
		//IL_0831: Expected O, but got Unknown
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083c: Expected O, but got Unknown
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0847: Expected O, but got Unknown
		//IL_0849: Unknown result type (might be due to invalid IL or missing references)
		//IL_0852: Expected O, but got Unknown
		//IL_0854: Unknown result type (might be due to invalid IL or missing references)
		//IL_085d: Expected O, but got Unknown
		//IL_085f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0868: Expected O, but got Unknown
		//IL_086a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0873: Expected O, but got Unknown
		//IL_0875: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Expected O, but got Unknown
		//IL_0880: Unknown result type (might be due to invalid IL or missing references)
		//IL_0889: Expected O, but got Unknown
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0894: Expected O, but got Unknown
		//IL_0896: Unknown result type (might be due to invalid IL or missing references)
		//IL_089f: Expected O, but got Unknown
		//IL_08a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08aa: Expected O, but got Unknown
		//IL_08ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b5: Expected O, but got Unknown
		//IL_08b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c0: Expected O, but got Unknown
		//IL_08c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08cb: Expected O, but got Unknown
		//IL_08cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d6: Expected O, but got Unknown
		//IL_08d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e1: Expected O, but got Unknown
		//IL_08e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ec: Expected O, but got Unknown
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f7: Expected O, but got Unknown
		//IL_08f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0902: Expected O, but got Unknown
		//IL_0904: Unknown result type (might be due to invalid IL or missing references)
		//IL_090d: Expected O, but got Unknown
		//IL_090f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Expected O, but got Unknown
		//IL_091a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0923: Expected O, but got Unknown
		//IL_0925: Unknown result type (might be due to invalid IL or missing references)
		//IL_092e: Expected O, but got Unknown
		//IL_0930: Unknown result type (might be due to invalid IL or missing references)
		//IL_0939: Expected O, but got Unknown
		//IL_093b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_0946: Unknown result type (might be due to invalid IL or missing references)
		//IL_094f: Expected O, but got Unknown
		//IL_0951: Unknown result type (might be due to invalid IL or missing references)
		//IL_095a: Expected O, but got Unknown
		//IL_095c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0965: Expected O, but got Unknown
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_0970: Expected O, but got Unknown
		//IL_0972: Unknown result type (might be due to invalid IL or missing references)
		//IL_097b: Expected O, but got Unknown
		//IL_097d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0986: Expected O, but got Unknown
		//IL_0988: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Expected O, but got Unknown
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_099c: Expected O, but got Unknown
		//IL_099e: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a7: Expected O, but got Unknown
		//IL_09a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b2: Expected O, but got Unknown
		//IL_09b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09bd: Expected O, but got Unknown
		//IL_09bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09c8: Expected O, but got Unknown
		//IL_09ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d3: Expected O, but got Unknown
		//IL_09d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09de: Expected O, but got Unknown
		//IL_09e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_09e9: Expected O, but got Unknown
		//IL_09eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Expected O, but got Unknown
		//IL_09f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ff: Expected O, but got Unknown
		//IL_0a01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Expected O, but got Unknown
		//IL_0a0c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0a17: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a20: Expected O, but got Unknown
		//IL_0a22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a2b: Expected O, but got Unknown
		//IL_0a2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a36: Expected O, but got Unknown
		//IL_0a38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Expected O, but got Unknown
		//IL_0a43: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a4c: Expected O, but got Unknown
		//IL_0a4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Expected O, but got Unknown
		//IL_0a59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a62: Expected O, but got Unknown
		//IL_0a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a6d: Expected O, but got Unknown
		//IL_0a6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Expected O, but got Unknown
		//IL_0a7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a83: Expected O, but got Unknown
		//IL_0a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a8e: Expected O, but got Unknown
		//IL_0a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a99: Expected O, but got Unknown
		//IL_0a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa4: Expected O, but got Unknown
		//IL_0aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b09: Expected O, but got Unknown
		//IL_0b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b1f: Expected O, but got Unknown
		//IL_0cd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ce2: Expected O, but got Unknown
		//IL_0d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d24: Expected O, but got Unknown
		//IL_0d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d2f: Expected O, but got Unknown
		//IL_0d51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d5b: Expected O, but got Unknown
		//IL_0d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d66: Expected O, but got Unknown
		//IL_0d9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Expected O, but got Unknown
		//IL_0da9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0db3: Expected O, but got Unknown
		//IL_0db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dbe: Expected O, but got Unknown
		//IL_0dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc9: Expected O, but got Unknown
		//IL_0df6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e00: Expected O, but got Unknown
		//IL_0e22: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2c: Expected O, but got Unknown
		//IL_0e38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e42: Expected O, but got Unknown
		//IL_0e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e63: Expected O, but got Unknown
		//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0edc: Expected O, but got Unknown
		//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef2: Expected O, but got Unknown
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f97: Expected O, but got Unknown
		//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fef: Expected O, but got Unknown
		//IL_0ff0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ffa: Expected O, but got Unknown
		//IL_0ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Expected O, but got Unknown
		//IL_1006: Unknown result type (might be due to invalid IL or missing references)
		//IL_1010: Expected O, but got Unknown
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_101b: Expected O, but got Unknown
		//IL_101c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1026: Expected O, but got Unknown
		//IL_1027: Unknown result type (might be due to invalid IL or missing references)
		//IL_1031: Expected O, but got Unknown
		//IL_1032: Unknown result type (might be due to invalid IL or missing references)
		//IL_103c: Expected O, but got Unknown
		//IL_103d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1047: Expected O, but got Unknown
		//IL_1048: Unknown result type (might be due to invalid IL or missing references)
		//IL_1052: Expected O, but got Unknown
		//IL_1053: Unknown result type (might be due to invalid IL or missing references)
		//IL_105d: Expected O, but got Unknown
		//IL_105e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1068: Expected O, but got Unknown
		//IL_1069: Unknown result type (might be due to invalid IL or missing references)
		//IL_1073: Expected O, but got Unknown
		//IL_1880: Unknown result type (might be due to invalid IL or missing references)
		//IL_188a: Expected O, but got Unknown
		//IL_1a2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a38: Expected O, but got Unknown
		//IL_1a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a4f: Expected O, but got Unknown
		//IL_90bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_90c9: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdShipmentExpenses_DesignTimeLayout = new GridEXLayout();
		Appearance appearance221 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance198 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance292 = new Appearance();
		Appearance appearance395 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance261 = new Appearance();
		Appearance appearance269 = new Appearance();
		Appearance appearance278 = new Appearance();
		Appearance appearance373 = new Appearance();
		Appearance appearance384 = new Appearance();
		Appearance appearance396 = new Appearance();
		Appearance appearance421 = new Appearance();
		Appearance appearance422 = new Appearance();
		Appearance appearance423 = new Appearance();
		Appearance appearance424 = new Appearance();
		Appearance appearance425 = new Appearance();
		Appearance appearance426 = new Appearance();
		Appearance appearance427 = new Appearance();
		Appearance appearance428 = new Appearance();
		Appearance appearance429 = new Appearance();
		Appearance appearance430 = new Appearance();
		Appearance appearance431 = new Appearance();
		Appearance appearance432 = new Appearance();
		Appearance appearance433 = new Appearance();
		Appearance appearance434 = new Appearance();
		Appearance appearance435 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance415 = new Appearance();
		Appearance appearance416 = new Appearance();
		Appearance appearance417 = new Appearance();
		Appearance appearance418 = new Appearance();
		Appearance appearance419 = new Appearance();
		Appearance appearance420 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance397 = new Appearance();
		Appearance appearance401 = new Appearance();
		Appearance appearance402 = new Appearance();
		Appearance appearance403 = new Appearance();
		Appearance appearance404 = new Appearance();
		Appearance appearance405 = new Appearance();
		Appearance appearance406 = new Appearance();
		Appearance appearance407 = new Appearance();
		Appearance appearance408 = new Appearance();
		Appearance appearance409 = new Appearance();
		Appearance appearance413 = new Appearance();
		Appearance appearance414 = new Appearance();
		Appearance appearance284 = new Appearance();
		Appearance appearance285 = new Appearance();
		Appearance appearance286 = new Appearance();
		Appearance appearance287 = new Appearance();
		Appearance appearance288 = new Appearance();
		Appearance appearance289 = new Appearance();
		Appearance appearance290 = new Appearance();
		Appearance appearance291 = new Appearance();
		Appearance appearance293 = new Appearance();
		Appearance appearance306 = new Appearance();
		Appearance appearance307 = new Appearance();
		Appearance appearance308 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance241 = new Appearance();
		Appearance appearance267 = new Appearance();
		Appearance appearance268 = new Appearance();
		Appearance appearance270 = new Appearance();
		Appearance appearance294 = new Appearance();
		Appearance appearance295 = new Appearance();
		Appearance appearance296 = new Appearance();
		Appearance appearance297 = new Appearance();
		Appearance appearance298 = new Appearance();
		Appearance appearance299 = new Appearance();
		Appearance appearance300 = new Appearance();
		Appearance appearance301 = new Appearance();
		Appearance appearance302 = new Appearance();
		Appearance appearance303 = new Appearance();
		Appearance appearance304 = new Appearance();
		Appearance appearance305 = new Appearance();
		Appearance appearance271 = new Appearance();
		Appearance appearance272 = new Appearance();
		Appearance appearance273 = new Appearance();
		Appearance appearance274 = new Appearance();
		Appearance appearance275 = new Appearance();
		Appearance appearance276 = new Appearance();
		Appearance appearance277 = new Appearance();
		Appearance appearance279 = new Appearance();
		Appearance appearance280 = new Appearance();
		Appearance appearance281 = new Appearance();
		Appearance appearance282 = new Appearance();
		Appearance appearance283 = new Appearance();
		Appearance appearance242 = new Appearance();
		Appearance appearance243 = new Appearance();
		Appearance appearance244 = new Appearance();
		Appearance appearance245 = new Appearance();
		Appearance appearance246 = new Appearance();
		Appearance appearance247 = new Appearance();
		Appearance appearance248 = new Appearance();
		Appearance appearance249 = new Appearance();
		Appearance appearance250 = new Appearance();
		Appearance appearance251 = new Appearance();
		Appearance appearance252 = new Appearance();
		Appearance appearance253 = new Appearance();
		Appearance appearance254 = new Appearance();
		Appearance appearance255 = new Appearance();
		Appearance appearance256 = new Appearance();
		Appearance appearance257 = new Appearance();
		Appearance appearance258 = new Appearance();
		Appearance appearance259 = new Appearance();
		Appearance appearance260 = new Appearance();
		Appearance appearance262 = new Appearance();
		Appearance appearance263 = new Appearance();
		Appearance appearance264 = new Appearance();
		Appearance appearance265 = new Appearance();
		Appearance appearance266 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Import.ImCommercialInvoice));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance398 = new Appearance();
		Appearance appearance399 = new Appearance();
		Appearance appearance400 = new Appearance();
		Appearance appearance410 = new Appearance();
		Appearance appearance411 = new Appearance();
		Appearance appearance412 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance348 = new Appearance();
		Appearance appearance349 = new Appearance();
		Appearance appearance350 = new Appearance();
		Appearance appearance351 = new Appearance();
		Appearance appearance352 = new Appearance();
		Appearance appearance353 = new Appearance();
		Appearance appearance354 = new Appearance();
		Appearance appearance355 = new Appearance();
		Appearance appearance356 = new Appearance();
		Appearance appearance309 = new Appearance();
		Appearance appearance310 = new Appearance();
		Appearance appearance311 = new Appearance();
		Appearance appearance312 = new Appearance();
		Appearance appearance313 = new Appearance();
		Appearance appearance314 = new Appearance();
		Appearance appearance315 = new Appearance();
		Appearance appearance316 = new Appearance();
		Appearance appearance317 = new Appearance();
		Appearance appearance318 = new Appearance();
		Appearance appearance319 = new Appearance();
		Appearance appearance320 = new Appearance();
		Appearance appearance357 = new Appearance();
		Appearance appearance358 = new Appearance();
		Appearance appearance359 = new Appearance();
		Appearance appearance360 = new Appearance();
		Appearance appearance361 = new Appearance();
		Appearance appearance362 = new Appearance();
		Appearance appearance363 = new Appearance();
		Appearance appearance364 = new Appearance();
		Appearance appearance365 = new Appearance();
		Appearance appearance321 = new Appearance();
		Appearance appearance322 = new Appearance();
		Appearance appearance323 = new Appearance();
		Appearance appearance366 = new Appearance();
		Appearance appearance367 = new Appearance();
		Appearance appearance368 = new Appearance();
		Appearance appearance369 = new Appearance();
		Appearance appearance370 = new Appearance();
		Appearance appearance371 = new Appearance();
		Appearance appearance372 = new Appearance();
		Appearance appearance374 = new Appearance();
		Appearance appearance375 = new Appearance();
		Appearance appearance376 = new Appearance();
		Appearance appearance377 = new Appearance();
		Appearance appearance378 = new Appearance();
		Appearance appearance379 = new Appearance();
		Appearance appearance380 = new Appearance();
		Appearance appearance381 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance382 = new Appearance();
		Appearance appearance383 = new Appearance();
		Appearance appearance385 = new Appearance();
		Appearance appearance386 = new Appearance();
		Appearance appearance387 = new Appearance();
		Appearance appearance388 = new Appearance();
		Appearance appearance389 = new Appearance();
		Appearance appearance390 = new Appearance();
		Appearance appearance391 = new Appearance();
		Appearance appearance392 = new Appearance();
		Appearance appearance393 = new Appearance();
		Appearance appearance394 = new Appearance();
		Appearance appearance324 = new Appearance();
		Appearance appearance325 = new Appearance();
		Appearance appearance326 = new Appearance();
		Appearance appearance327 = new Appearance();
		Appearance appearance328 = new Appearance();
		Appearance appearance329 = new Appearance();
		Appearance appearance330 = new Appearance();
		Appearance appearance331 = new Appearance();
		Appearance appearance332 = new Appearance();
		Appearance appearance333 = new Appearance();
		Appearance appearance334 = new Appearance();
		Appearance appearance335 = new Appearance();
		Appearance appearance336 = new Appearance();
		Appearance appearance337 = new Appearance();
		Appearance appearance338 = new Appearance();
		Appearance appearance339 = new Appearance();
		Appearance appearance340 = new Appearance();
		Appearance appearance341 = new Appearance();
		Appearance appearance342 = new Appearance();
		Appearance appearance343 = new Appearance();
		Appearance appearance344 = new Appearance();
		Appearance appearance345 = new Appearance();
		Appearance appearance346 = new Appearance();
		Appearance appearance347 = new Appearance();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel8 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grdDetail = new GridEX();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.grdShipmentExpenses = new GridEX();
		this.HeaderGropBox = new System.Windows.Forms.GroupBox();
		this.label70 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label65 = new System.Windows.Forms.Label();
		this.txtTotalNetAmount = new System.Windows.Forms.TextBox();
		this.label66 = new System.Windows.Forms.Label();
		this.txtaddlessamount = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.label40 = new System.Windows.Forms.Label();
		this.txtaddlesscommnets = new System.Windows.Forms.TextBox();
		this.label62 = new System.Windows.Forms.Label();
		this.label61 = new System.Windows.Forms.Label();
		this.txtcertificateOfOrigion = new System.Windows.Forms.TextBox();
		this.txtcertificate = new System.Windows.Forms.TextBox();
		this.label60 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.txtIformDate = new System.Windows.Forms.DateTimePicker();
		this.label51 = new System.Windows.Forms.Label();
		this.txtIformNo = new System.Windows.Forms.TextBox();
		this.label52 = new System.Windows.Forms.Label();
		this.label49 = new System.Windows.Forms.Label();
		this.label46 = new System.Windows.Forms.Label();
		this.txtGrossWeight = new System.Windows.Forms.TextBox();
		this.label54 = new System.Windows.Forms.Label();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label55 = new System.Windows.Forms.Label();
		this.txtNoOfContainer = new System.Windows.Forms.TextBox();
		this.label57 = new System.Windows.Forms.Label();
		this.label45 = new System.Windows.Forms.Label();
		this.txtLocalAmt = new System.Windows.Forms.TextBox();
		this.label42 = new System.Windows.Forms.Label();
		this.txtExRate = new System.Windows.Forms.TextBox();
		this.label44 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label47 = new System.Windows.Forms.Label();
		this.cmbcareiertype = new UltraCombo();
		this.label22 = new System.Windows.Forms.Label();
		this.txtfcyAmount = new System.Windows.Forms.TextBox();
		this.label25 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label27 = new System.Windows.Forms.Label();
		this.CmbHomeCurrency = new UltraCombo();
		this.cmbfcycode = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label26 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.cmbDestinationPort = new UltraCombo();
		this.cmbLoadingPort = new UltraCombo();
		this.label16 = new System.Windows.Forms.Label();
		this.label48 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.cmbexporterBankNew = new UltraCombo();
		this.cmbimporterBankNew = new UltraCombo();
		this.cmbPaymentTermsNew = new UltraCombo();
		this.cmbdeliverytermnew = new UltraCombo();
		this.label24 = new System.Windows.Forms.Label();
		this.label31 = new System.Windows.Forms.Label();
		this.label67 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.cmbNotifyParty2 = new UltraCombo();
		this.txtinvoiceno = new System.Windows.Forms.TextBox();
		this.txtLotRef = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbNotifyParty1 = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.cmbLcContractNo = new UltraCombo();
		this.lblPoNo = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.cmbSupCust = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.label68 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.label17 = new System.Windows.Forms.Label();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.cmbproject = new UltraCombo();
		this.label18 = new System.Windows.Forms.Label();
		this.cmbbranches = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnclose = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.panel12 = new System.Windows.Forms.Panel();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label69 = new System.Windows.Forms.Label();
		this.panel16 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.panel1 = new System.Windows.Forms.Panel();
		this.cmbFCurrencr = new UltraCombo();
		this.cmbPaymentTerm = new UltraCombo();
		this.combranches = new UltraCombo();
		this.comproject = new UltraCombo();
		this.ultraCombo3 = new UltraCombo();
		this.CmbLCContractor = new UltraCombo();
		this.ultraCombo2 = new UltraCombo();
		this.ultraCombo1 = new UltraCombo();
		this.cmbCarierType = new UltraCombo();
		this.cmbExporterBank = new UltraCombo();
		this.cmbImporterBank = new UltraCombo();
		this.cmbEFormNo = new UltraCombo();
		this.cmbdeliveryterm = new UltraCombo();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
		this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel2.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.tabPage4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdShipmentExpenses).BeginInit();
		this.HeaderGropBox.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHomeCurrency).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTermsNew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLcContractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranches).BeginInit();
		this.panel10.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel12.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel16.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).BeginInit();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel4.Controls.Add(this.panel7);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1181, 739);
		this.panel4.TabIndex = 3;
		this.panel7.BackColor = System.Drawing.Color.White;
		this.panel7.Controls.Add(this.tabControl1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1181, 739);
		this.panel7.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1181, 739);
		this.tabControl1.TabIndex = 2;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel8);
		this.tabPage1.Controls.Add(this.panel10);
		this.tabPage1.Controls.Add(this.panel12);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1173, 713);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel8.Controls.Add(this.panel2);
		this.panel8.Controls.Add(this.HeaderGropBox);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(3, 71);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1167, 639);
		this.panel8.TabIndex = 3;
		this.panel2.Controls.Add(this.tabControl2);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 310);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1167, 329);
		this.panel2.TabIndex = 2;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Controls.Add(this.tabPage4);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1167, 329);
		this.tabControl2.TabIndex = 1;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1159, 303);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).Office2007ColorScheme = (Office2007ColorScheme)4;
		((ScrollableBase)this.grdDetail).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1153, 297);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.RecordsDeleted += new System.EventHandler(grdDetail_RecordsDeleted);
		this.tabPage4.Controls.Add((System.Windows.Forms.Control)(object)this.grdShipmentExpenses);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage4.Size = new System.Drawing.Size(1159, 303);
		this.tabPage4.TabIndex = 1;
		this.tabPage4.Text = "Shipment Expenses";
		this.tabPage4.UseVisualStyleBackColor = true;
		((ScrollableBase)this.grdShipmentExpenses).BackColor = System.Drawing.Color.White;
		grdShipmentExpenses_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdShipmentExpenses.DesignTimeLayout = grdShipmentExpenses_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdShipmentExpenses).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdShipmentExpenses).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdShipmentExpenses.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdShipmentExpenses).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.grdShipmentExpenses).Name = "grdShipmentExpenses";
		((ScrollableBase)this.grdShipmentExpenses).Office2007ColorScheme = (Office2007ColorScheme)4;
		((ScrollableBase)this.grdShipmentExpenses).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdShipmentExpenses).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdShipmentExpenses).Size = new System.Drawing.Size(1153, 297);
		((System.Windows.Forms.Control)(object)this.grdShipmentExpenses).TabIndex = 1;
		this.grdShipmentExpenses.TotalRow = (InheritableBoolean)1;
		this.grdShipmentExpenses.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdShipmentExpenses).VisualStyle = (VisualStyle)4;
		this.grdShipmentExpenses.CellUpdated += new ColumnActionEventHandler(grdShipmentExpenses_CellUpdated);
		this.grdShipmentExpenses.ColumnButtonClick += new ColumnActionEventHandler(grdShipmentExpenses_ColumnButtonClick);
		this.HeaderGropBox.Controls.Add(this.label70);
		this.HeaderGropBox.Controls.Add(this.label63);
		this.HeaderGropBox.Controls.Add(this.label15);
		this.HeaderGropBox.Controls.Add(this.label65);
		this.HeaderGropBox.Controls.Add(this.txtTotalNetAmount);
		this.HeaderGropBox.Controls.Add(this.label66);
		this.HeaderGropBox.Controls.Add(this.txtaddlessamount);
		this.HeaderGropBox.Controls.Add(this.label64);
		this.HeaderGropBox.Controls.Add(this.label40);
		this.HeaderGropBox.Controls.Add(this.txtaddlesscommnets);
		this.HeaderGropBox.Controls.Add(this.label62);
		this.HeaderGropBox.Controls.Add(this.label61);
		this.HeaderGropBox.Controls.Add(this.txtcertificateOfOrigion);
		this.HeaderGropBox.Controls.Add(this.txtcertificate);
		this.HeaderGropBox.Controls.Add(this.label60);
		this.HeaderGropBox.Controls.Add(this.label59);
		this.HeaderGropBox.Controls.Add(this.label58);
		this.HeaderGropBox.Controls.Add(this.label53);
		this.HeaderGropBox.Controls.Add(this.label50);
		this.HeaderGropBox.Controls.Add(this.txtIformDate);
		this.HeaderGropBox.Controls.Add(this.label51);
		this.HeaderGropBox.Controls.Add(this.txtIformNo);
		this.HeaderGropBox.Controls.Add(this.label52);
		this.HeaderGropBox.Controls.Add(this.label49);
		this.HeaderGropBox.Controls.Add(this.label46);
		this.HeaderGropBox.Controls.Add(this.txtGrossWeight);
		this.HeaderGropBox.Controls.Add(this.label54);
		this.HeaderGropBox.Controls.Add(this.txtNetWeight);
		this.HeaderGropBox.Controls.Add(this.label55);
		this.HeaderGropBox.Controls.Add(this.txtNoOfContainer);
		this.HeaderGropBox.Controls.Add(this.label57);
		this.HeaderGropBox.Controls.Add(this.label45);
		this.HeaderGropBox.Controls.Add(this.txtLocalAmt);
		this.HeaderGropBox.Controls.Add(this.label42);
		this.HeaderGropBox.Controls.Add(this.txtExRate);
		this.HeaderGropBox.Controls.Add(this.label44);
		this.HeaderGropBox.Controls.Add(this.label41);
		this.HeaderGropBox.Controls.Add(this.label20);
		this.HeaderGropBox.Controls.Add(this.label47);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcareiertype);
		this.HeaderGropBox.Controls.Add(this.label22);
		this.HeaderGropBox.Controls.Add(this.txtfcyAmount);
		this.HeaderGropBox.Controls.Add(this.label25);
		this.HeaderGropBox.Controls.Add(this.label14);
		this.HeaderGropBox.Controls.Add(this.label27);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.CmbHomeCurrency);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbfcycode);
		this.HeaderGropBox.Controls.Add(this.label19);
		this.HeaderGropBox.Controls.Add(this.label26);
		this.HeaderGropBox.Controls.Add(this.label56);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDestinationPort);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingPort);
		this.HeaderGropBox.Controls.Add(this.label16);
		this.HeaderGropBox.Controls.Add(this.label48);
		this.HeaderGropBox.Controls.Add(this.label9);
		this.HeaderGropBox.Controls.Add(this.label11);
		this.HeaderGropBox.Controls.Add(this.label12);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbexporterBankNew);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbimporterBankNew);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew);
		this.HeaderGropBox.Controls.Add(this.label24);
		this.HeaderGropBox.Controls.Add(this.label31);
		this.HeaderGropBox.Controls.Add(this.label67);
		this.HeaderGropBox.Controls.Add(this.label7);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty2);
		this.HeaderGropBox.Controls.Add(this.txtinvoiceno);
		this.HeaderGropBox.Controls.Add(this.txtLotRef);
		this.HeaderGropBox.Controls.Add(this.label10);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbNotifyParty1);
		this.HeaderGropBox.Controls.Add(this.label8);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLcContractNo);
		this.HeaderGropBox.Controls.Add(this.lblPoNo);
		this.HeaderGropBox.Controls.Add(this.label28);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupCust);
		this.HeaderGropBox.Controls.Add(this.label21);
		this.HeaderGropBox.Controls.Add(this.label3);
		this.HeaderGropBox.Controls.Add(this.label68);
		this.HeaderGropBox.Controls.Add(this.label2);
		this.HeaderGropBox.Controls.Add(this.txtDocDate);
		this.HeaderGropBox.Controls.Add(this.label17);
		this.HeaderGropBox.Controls.Add(this.txtdocno);
		this.HeaderGropBox.Controls.Add(this.label5);
		this.HeaderGropBox.Controls.Add(this.label1);
		this.HeaderGropBox.Controls.Add(this.label43);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbproject);
		this.HeaderGropBox.Controls.Add(this.label18);
		this.HeaderGropBox.Controls.Add((System.Windows.Forms.Control)(object)this.cmbbranches);
		this.HeaderGropBox.Controls.Add(this.label13);
		this.HeaderGropBox.Dock = System.Windows.Forms.DockStyle.Top;
		this.HeaderGropBox.Location = new System.Drawing.Point(0, 0);
		this.HeaderGropBox.Name = "HeaderGropBox";
		this.HeaderGropBox.Size = new System.Drawing.Size(1167, 310);
		this.HeaderGropBox.TabIndex = 1;
		this.HeaderGropBox.TabStop = false;
		this.HeaderGropBox.Text = "Main Header";
		this.label70.AutoSize = true;
		this.label70.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label70.ForeColor = System.Drawing.Color.Red;
		this.label70.Location = new System.Drawing.Point(680, 73);
		this.label70.Name = "label70";
		this.label70.Size = new System.Drawing.Size(14, 17);
		this.label70.TabIndex = 589;
		this.label70.Text = "*";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label63.ForeColor = System.Drawing.Color.Red;
		this.label63.Location = new System.Drawing.Point(384, 99);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(14, 17);
		this.label63.TabIndex = 588;
		this.label63.Text = "*";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.Red;
		this.label15.Location = new System.Drawing.Point(384, 71);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(14, 17);
		this.label15.TabIndex = 587;
		this.label15.Text = "*";
		this.label65.AutoSize = true;
		this.label65.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label65.ForeColor = System.Drawing.Color.Red;
		this.label65.Location = new System.Drawing.Point(973, 278);
		this.label65.Name = "label65";
		this.label65.Size = new System.Drawing.Size(14, 17);
		this.label65.TabIndex = 31;
		this.label65.Text = "*";
		this.txtTotalNetAmount.BackColor = System.Drawing.Color.White;
		this.txtTotalNetAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTotalNetAmount.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtTotalNetAmount.Location = new System.Drawing.Point(988, 278);
		this.txtTotalNetAmount.Name = "txtTotalNetAmount";
		this.txtTotalNetAmount.ReadOnly = true;
		this.txtTotalNetAmount.Size = new System.Drawing.Size(171, 23);
		this.txtTotalNetAmount.TabIndex = 584;
		this.txtTotalNetAmount.TabStop = false;
		this.txtTotalNetAmount.Text = "0";
		this.txtTotalNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label66.AutoSize = true;
		this.label66.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label66.Location = new System.Drawing.Point(888, 284);
		this.label66.Name = "label66";
		this.label66.Size = new System.Drawing.Size(79, 15);
		this.label66.TabIndex = 585;
		this.label66.Text = "Total Amount";
		this.txtaddlessamount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlessamount.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtaddlessamount.Location = new System.Drawing.Point(987, 249);
		this.txtaddlessamount.Name = "txtaddlessamount";
		this.txtaddlessamount.Size = new System.Drawing.Size(171, 23);
		this.txtaddlessamount.TabIndex = 30;
		this.txtaddlessamount.Text = "0";
		this.txtaddlessamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtaddlessamount.TextChanged += new System.EventHandler(txtaddlessamount_TextChanged);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label64.Location = new System.Drawing.Point(987, 231);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(99, 15);
		this.label64.TabIndex = 582;
		this.label64.Text = "Add Less Amount";
		this.label40.AutoSize = true;
		this.label40.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label40.Location = new System.Drawing.Point(603, 253);
		this.label40.Name = "label40";
		this.label40.Size = new System.Drawing.Size(114, 15);
		this.label40.TabIndex = 580;
		this.label40.Text = "Add Less Comments";
		this.txtaddlesscommnets.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaddlesscommnets.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtaddlesscommnets.Location = new System.Drawing.Point(721, 249);
		this.txtaddlesscommnets.Name = "txtaddlesscommnets";
		this.txtaddlesscommnets.Size = new System.Drawing.Size(260, 23);
		this.txtaddlesscommnets.TabIndex = 29;
		this.label62.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label62.Location = new System.Drawing.Point(840, 167);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(73, 37);
		this.label62.TabIndex = 577;
		this.label62.Text = "Certificate OF Origion";
		this.label61.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label61.Location = new System.Drawing.Point(840, 81);
		this.label61.Name = "label61";
		this.label61.Size = new System.Drawing.Size(83, 14);
		this.label61.TabIndex = 576;
		this.label61.Text = "Certificate -1";
		this.txtcertificateOfOrigion.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcertificateOfOrigion.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtcertificateOfOrigion.Location = new System.Drawing.Point(924, 153);
		this.txtcertificateOfOrigion.Multiline = true;
		this.txtcertificateOfOrigion.Name = "txtcertificateOfOrigion";
		this.txtcertificateOfOrigion.Size = new System.Drawing.Size(234, 76);
		this.txtcertificateOfOrigion.TabIndex = 28;
		this.txtcertificate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtcertificate.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtcertificate.Location = new System.Drawing.Point(924, 69);
		this.txtcertificate.Multiline = true;
		this.txtcertificate.Name = "txtcertificate";
		this.txtcertificate.Size = new System.Drawing.Size(234, 76);
		this.txtcertificate.TabIndex = 27;
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label60.ForeColor = System.Drawing.Color.Red;
		this.label60.Location = new System.Drawing.Point(682, 98);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(14, 17);
		this.label60.TabIndex = 573;
		this.label60.Text = "*";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label59.ForeColor = System.Drawing.Color.Red;
		this.label59.Location = new System.Drawing.Point(909, 36);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(14, 17);
		this.label59.TabIndex = 26;
		this.label59.Text = "*";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label58.ForeColor = System.Drawing.Color.Red;
		this.label58.Location = new System.Drawing.Point(106, 191);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(14, 17);
		this.label58.TabIndex = 571;
		this.label58.Text = "*";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.ForeColor = System.Drawing.Color.Red;
		this.label53.Location = new System.Drawing.Point(680, 213);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(14, 17);
		this.label53.TabIndex = 570;
		this.label53.Text = "*";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.ForeColor = System.Drawing.Color.Red;
		this.label50.Location = new System.Drawing.Point(680, 155);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(14, 17);
		this.label50.TabIndex = 569;
		this.label50.Text = "*";
		this.txtIformDate.CustomFormat = "dd-MMM-yy";
		this.txtIformDate.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtIformDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtIformDate.Location = new System.Drawing.Point(924, 39);
		this.txtIformDate.Name = "txtIformDate";
		this.txtIformDate.Size = new System.Drawing.Size(234, 23);
		this.txtIformDate.TabIndex = 26;
		this.label51.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label51.Location = new System.Drawing.Point(840, 43);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(73, 14);
		this.label51.TabIndex = 568;
		this.label51.Text = "I-Form Date";
		this.txtIformNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtIformNo.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtIformNo.Location = new System.Drawing.Point(924, 13);
		this.txtIformNo.Name = "txtIformNo";
		this.txtIformNo.Size = new System.Drawing.Size(234, 23);
		this.txtIformNo.TabIndex = 25;
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.ForeColor = System.Drawing.Color.Red;
		this.label52.Location = new System.Drawing.Point(904, 11);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(14, 17);
		this.label52.TabIndex = 564;
		this.label52.Text = "*";
		this.label49.AutoSize = true;
		this.label49.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label49.ForeColor = System.Drawing.Color.Red;
		this.label49.Location = new System.Drawing.Point(680, 185);
		this.label49.Name = "label49";
		this.label49.Size = new System.Drawing.Size(14, 17);
		this.label49.TabIndex = 560;
		this.label49.Text = "*";
		this.label46.AutoSize = true;
		this.label46.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label46.Location = new System.Drawing.Point(841, 13);
		this.label46.Name = "label46";
		this.label46.Size = new System.Drawing.Size(61, 15);
		this.label46.TabIndex = 561;
		this.label46.Text = "I-Form No";
		this.txtGrossWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtGrossWeight.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtGrossWeight.Location = new System.Drawing.Point(699, 152);
		this.txtGrossWeight.Name = "txtGrossWeight";
		this.txtGrossWeight.ReadOnly = true;
		this.txtGrossWeight.Size = new System.Drawing.Size(135, 23);
		this.txtGrossWeight.TabIndex = 22;
		this.txtGrossWeight.TabStop = false;
		this.txtGrossWeight.Text = "0";
		this.txtGrossWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label54.Location = new System.Drawing.Point(580, 156);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(98, 15);
		this.label54.TabIndex = 553;
		this.label54.Text = "Gross Weight KG";
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtNetWeight.Location = new System.Drawing.Point(699, 182);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.ReadOnly = true;
		this.txtNetWeight.Size = new System.Drawing.Size(135, 23);
		this.txtNetWeight.TabIndex = 23;
		this.txtNetWeight.TabStop = false;
		this.txtNetWeight.Text = "0";
		this.txtNetWeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label55.Location = new System.Drawing.Point(580, 186);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(84, 15);
		this.label55.TabIndex = 554;
		this.label55.Text = "Net Weight KG";
		this.txtNoOfContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNoOfContainer.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtNoOfContainer.Location = new System.Drawing.Point(699, 210);
		this.txtNoOfContainer.Name = "txtNoOfContainer";
		this.txtNoOfContainer.ReadOnly = true;
		this.txtNoOfContainer.Size = new System.Drawing.Size(135, 23);
		this.txtNoOfContainer.TabIndex = 24;
		this.txtNoOfContainer.TabStop = false;
		this.txtNoOfContainer.Text = "0";
		this.txtNoOfContainer.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label57.Location = new System.Drawing.Point(580, 214);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(95, 15);
		this.label57.TabIndex = 552;
		this.label57.Text = "No.Of Container";
		this.label45.AutoSize = true;
		this.label45.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label45.ForeColor = System.Drawing.Color.Red;
		this.label45.Location = new System.Drawing.Point(680, 127);
		this.label45.Name = "label45";
		this.label45.Size = new System.Drawing.Size(14, 17);
		this.label45.TabIndex = 550;
		this.label45.Text = "*";
		this.txtLocalAmt.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
		this.txtLocalAmt.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLocalAmt.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtLocalAmt.Location = new System.Drawing.Point(699, 124);
		this.txtLocalAmt.Name = "txtLocalAmt";
		this.txtLocalAmt.ReadOnly = true;
		this.txtLocalAmt.Size = new System.Drawing.Size(135, 23);
		this.txtLocalAmt.TabIndex = 21;
		this.txtLocalAmt.TabStop = false;
		this.txtLocalAmt.Text = "0";
		this.txtLocalAmt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label42.Location = new System.Drawing.Point(580, 128);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(81, 15);
		this.label42.TabIndex = 549;
		this.label42.Text = "Local Amount";
		this.txtExRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtExRate.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtExRate.Location = new System.Drawing.Point(699, 97);
		this.txtExRate.Name = "txtExRate";
		this.txtExRate.Size = new System.Drawing.Size(135, 23);
		this.txtExRate.TabIndex = 20;
		this.txtExRate.Text = "0";
		this.txtExRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExRate.TextChanged += new System.EventHandler(txtExRate_TextChanged);
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label44.Location = new System.Drawing.Point(580, 99);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(85, 15);
		this.label44.TabIndex = 546;
		this.label44.Text = "Exchange Rate";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label41.ForeColor = System.Drawing.Color.Red;
		this.label41.Location = new System.Drawing.Point(385, 216);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(14, 17);
		this.label41.TabIndex = 545;
		this.label41.Text = "*";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(680, 46);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 544;
		this.label20.Text = "*";
		this.label47.AutoSize = true;
		this.label47.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label47.ForeColor = System.Drawing.Color.Red;
		this.label47.Location = new System.Drawing.Point(680, 16);
		this.label47.Name = "label47";
		this.label47.Size = new System.Drawing.Size(14, 17);
		this.label47.TabIndex = 543;
		this.label47.Text = "*";
		this.cmbcareiertype.AllowNull = (DefaultableBoolean)1;
		this.cmbcareiertype.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcareiertype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcareiertype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance221;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance222).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance222).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance222;
		((AppearanceBase)appearance223).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance223;
		((SpecialBoxBase)((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance224).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance224).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance224).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance224;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance225).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance226).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance228).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance228).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance229).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance229).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance229).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance229).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance231).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance233;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcareiertype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbcareiertype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Location = new System.Drawing.Point(404, 211);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Name = "cmbcareiertype";
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbcareiertype).TabIndex = 16;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label22.Location = new System.Drawing.Point(301, 217);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(68, 15);
		this.label22.TabIndex = 542;
		this.label22.Text = "Carier Type";
		this.txtfcyAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfcyAmount.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtfcyAmount.Location = new System.Drawing.Point(699, 43);
		this.txtfcyAmount.Name = "txtfcyAmount";
		this.txtfcyAmount.ReadOnly = true;
		this.txtfcyAmount.Size = new System.Drawing.Size(135, 23);
		this.txtfcyAmount.TabIndex = 18;
		this.txtfcyAmount.TabStop = false;
		this.txtfcyAmount.Text = "0";
		this.txtfcyAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtfcyAmount.TextChanged += new System.EventHandler(txtfcyAmount_TextChanged);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label25.Location = new System.Drawing.Point(580, 47);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(71, 15);
		this.label25.TabIndex = 538;
		this.label25.Text = "Fcy-Amount";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label14.Location = new System.Drawing.Point(580, 75);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(90, 15);
		this.label14.TabIndex = 537;
		this.label14.Text = "Home Currency";
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label27.Location = new System.Drawing.Point(580, 17);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(55, 15);
		this.label27.TabIndex = 537;
		this.label27.Text = "Fcy Code";
		this.CmbHomeCurrency.AllowNull = (DefaultableBoolean)1;
		this.CmbHomeCurrency.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbHomeCurrency.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbHomeCurrency.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Appearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance197).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance197).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance197).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance197;
		((AppearanceBase)appearance198).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance198;
		((SpecialBoxBase)((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance199).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance199).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance199).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance199;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance200).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance200).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance200;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance201).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance201;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance202).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance202;
		((AppearanceBase)appearance204).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance204).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance205).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance205).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance205).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance205;
		((AppearanceBase)appearance206).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance206;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance207).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance207).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance207;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance208;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbHomeCurrency).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.CmbHomeCurrency.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Location = new System.Drawing.Point(699, 69);
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Name = "CmbHomeCurrency";
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.CmbHomeCurrency).TabIndex = 19;
		this.cmbfcycode.AllowNull = (DefaultableBoolean)1;
		this.cmbfcycode.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbfcycode.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbfcycode.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Appearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance292).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance292).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance292).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance292).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance292;
		((AppearanceBase)appearance395).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance395;
		((SpecialBoxBase)((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance140).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance140).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance140).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance151;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance162).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance261).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance261;
		((AppearanceBase)appearance269).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance269).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance269;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance278).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance278).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance278).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance278).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance278).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance278;
		((AppearanceBase)appearance373).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance373;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance384).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance384).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance384;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance396).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance396;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbfcycode).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbfcycode.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Location = new System.Drawing.Point(699, 11);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Name = "cmbfcycode";
		this.cmbfcycode.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).Size = new System.Drawing.Size(135, 26);
		((System.Windows.Forms.Control)(object)this.cmbfcycode).TabIndex = 17;
		((System.Windows.Forms.Control)(object)this.cmbfcycode).TabStop = false;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(385, 186);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 536;
		this.label19.Text = "*";
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.Red;
		this.label26.Location = new System.Drawing.Point(385, 158);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(14, 17);
		this.label26.TabIndex = 535;
		this.label26.Text = "*";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(301, 159);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(76, 15);
		this.label56.TabIndex = 534;
		this.label56.Text = "Loading Port";
		this.cmbDestinationPort.AllowNull = (DefaultableBoolean)1;
		this.cmbDestinationPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDestinationPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDestinationPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance421).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance421).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance421;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance422).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance422).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance422).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance422).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance422;
		((AppearanceBase)appearance423).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance423;
		((SpecialBoxBase)((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance424).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance424).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance424).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance424).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance424;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance425).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance425).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance425;
		((AppearanceBase)appearance426).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance426).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance426;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance427).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance427;
		((AppearanceBase)appearance428).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance428).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance428;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance429).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance429).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance429).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance429).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance429).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance429;
		((AppearanceBase)appearance430).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance430;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance431).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance431).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance431;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance432).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance432;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDestinationPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbDestinationPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Location = new System.Drawing.Point(404, 181);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Name = "cmbDestinationPort";
		this.cmbDestinationPort.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabIndex = 15;
		((System.Windows.Forms.Control)(object)this.cmbDestinationPort).TabStop = false;
		this.cmbLoadingPort.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingPort.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingPort.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingPort.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance433).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance433).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Appearance = (AppearanceBase)(object)appearance433;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance434).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance434).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance434).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance434).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance434;
		((AppearanceBase)appearance435).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance435;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance100).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance102).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance104).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance105).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance105).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance105).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance105;
		((AppearanceBase)appearance106).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingPort).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbLoadingPort.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Location = new System.Drawing.Point(404, 153);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Name = "cmbLoadingPort";
		this.cmbLoadingPort.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabIndex = 14;
		((System.Windows.Forms.Control)(object)this.cmbLoadingPort).TabStop = false;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(301, 185);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(76, 19);
		this.label16.TabIndex = 533;
		this.label16.Text = "Destination Port";
		this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label48.AutoSize = true;
		this.label48.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label48.ForeColor = System.Drawing.Color.Red;
		this.label48.Location = new System.Drawing.Point(385, 42);
		this.label48.Name = "label48";
		this.label48.Size = new System.Drawing.Size(14, 17);
		this.label48.TabIndex = 529;
		this.label48.Text = "*";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.Red;
		this.label9.Location = new System.Drawing.Point(385, 128);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(14, 17);
		this.label9.TabIndex = 528;
		this.label9.Text = "*";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label11.Location = new System.Drawing.Point(301, 43);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(83, 15);
		this.label11.TabIndex = 527;
		this.label11.Text = "Payment Term";
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label12.Location = new System.Drawing.Point(301, 129);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(81, 15);
		this.label12.TabIndex = 526;
		this.label12.Text = "Delivery Term";
		this.cmbexporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbexporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbexporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbexporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance415).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance415).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance415;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance416).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance416).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance416).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance416).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance416;
		((AppearanceBase)appearance417).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance417;
		((SpecialBoxBase)((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance418).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance418).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance418).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance418).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance418;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance419).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance419).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance419;
		((AppearanceBase)appearance420).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance420).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance420;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance113).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance113).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance114).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance114).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance114).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbexporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbexporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Location = new System.Drawing.Point(404, 95);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Name = "cmbexporterBankNew";
		this.cmbexporterBankNew.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).TabIndex = 12;
		((System.Windows.Forms.Control)(object)this.cmbexporterBankNew).TabStop = false;
		this.cmbimporterBankNew.AllowNull = (DefaultableBoolean)1;
		this.cmbimporterBankNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbimporterBankNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbimporterBankNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance122).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance122;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance123).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance123).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance124;
		((SpecialBoxBase)((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance136;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance147).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance147;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance148).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance149).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance149).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance149).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance149).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance149;
		((AppearanceBase)appearance173).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance174).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbimporterBankNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbimporterBankNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Location = new System.Drawing.Point(404, 66);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Name = "cmbimporterBankNew";
		this.cmbimporterBankNew.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).TabIndex = 11;
		((System.Windows.Forms.Control)(object)this.cmbimporterBankNew).TabStop = false;
		this.cmbPaymentTermsNew.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTermsNew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTermsNew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTermsNew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance209).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance210).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance210).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance210).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance211;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance212).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance212).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance212).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance212;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance213).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance213).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance213;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance214).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance214;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance215).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance215;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance216).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance217).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance217).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance217).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance217).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance217;
		((AppearanceBase)appearance218).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance218;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance219).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance220).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTermsNew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbPaymentTermsNew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Location = new System.Drawing.Point(404, 37);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Name = "cmbPaymentTermsNew";
		this.cmbPaymentTermsNew.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTermsNew).TabStop = false;
		this.cmbdeliverytermnew.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliverytermnew.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliverytermnew.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliverytermnew.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance397).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance397).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Appearance = (AppearanceBase)(object)appearance397;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance401).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance401).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance401).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance401).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance401;
		((AppearanceBase)appearance402).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance402;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance403).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance403).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance403).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance403).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance403;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance404).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance404).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance404;
		((AppearanceBase)appearance405).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance405).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance405;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance406).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance406;
		((AppearanceBase)appearance407).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance407).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance407;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance408).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance408).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance408).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance408).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance408).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance408;
		((AppearanceBase)appearance409).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance409;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance413).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance413).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance413;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance414).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance414;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliverytermnew).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbdeliverytermnew.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Location = new System.Drawing.Point(404, 123);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Name = "cmbdeliverytermnew";
		this.cmbdeliverytermnew.ReadOnly = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).Size = new System.Drawing.Size(170, 26);
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.cmbdeliverytermnew).TabStop = false;
		this.label24.BackColor = System.Drawing.Color.White;
		this.label24.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label24.Location = new System.Drawing.Point(301, 99);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(76, 19);
		this.label24.TabIndex = 525;
		this.label24.Text = "Exporter Bank";
		this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label31.BackColor = System.Drawing.Color.White;
		this.label31.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label31.Location = new System.Drawing.Point(301, 70);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(76, 19);
		this.label31.TabIndex = 524;
		this.label31.Text = "Importer Bank";
		this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label67.AutoSize = true;
		this.label67.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label67.Location = new System.Drawing.Point(6, 133);
		this.label67.Name = "label67";
		this.label67.Size = new System.Drawing.Size(65, 15);
		this.label67.TabIndex = 57;
		this.label67.Text = "Invoice No";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label7.Location = new System.Drawing.Point(301, 15);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 57;
		this.label7.Text = "Lot Reference";
		this.cmbNotifyParty2.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty2.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance284).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance284).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance284;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance285).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance285).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance285).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance285).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance285;
		((AppearanceBase)appearance286).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance286;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance287).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance287).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance287).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance287).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance287;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance288).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance288).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance288;
		((AppearanceBase)appearance289).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance289).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance289;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance290).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance290;
		((AppearanceBase)appearance291).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance291).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance291;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance293).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance293).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance293).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance293).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance293).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance293;
		((AppearanceBase)appearance306).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance306;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance307).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance307).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance307;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance308).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance308;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbNotifyParty2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Location = new System.Drawing.Point(126, 242);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Name = "cmbNotifyParty2";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty2).TabIndex = 8;
		this.txtinvoiceno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtinvoiceno.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtinvoiceno.Location = new System.Drawing.Point(126, 128);
		this.txtinvoiceno.Name = "txtinvoiceno";
		this.txtinvoiceno.Size = new System.Drawing.Size(171, 23);
		this.txtinvoiceno.TabIndex = 4;
		this.txtLotRef.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLotRef.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtLotRef.Location = new System.Drawing.Point(404, 11);
		this.txtLotRef.Name = "txtLotRef";
		this.txtLotRef.Size = new System.Drawing.Size(170, 23);
		this.txtLotRef.TabIndex = 9;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label10.Location = new System.Drawing.Point(6, 249);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(83, 19);
		this.label10.TabIndex = 120;
		this.label10.Text = "Notify Party 2";
		this.cmbNotifyParty1.AllowNull = (DefaultableBoolean)1;
		this.cmbNotifyParty1.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbNotifyParty1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbNotifyParty1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance230;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance234).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance234).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance234).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance234).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance234;
		((AppearanceBase)appearance235).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance235;
		((SpecialBoxBase)((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance236).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance236;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance237).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance237;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance240).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance241).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance241).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance241).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance241).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance241).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance241;
		((AppearanceBase)appearance267).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance267;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance268).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance268).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance268;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance270).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance270;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbNotifyParty1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbNotifyParty1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Location = new System.Drawing.Point(126, 214);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Name = "cmbNotifyParty1";
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbNotifyParty1).TabIndex = 7;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label8.Location = new System.Drawing.Point(6, 221);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(83, 16);
		this.label8.TabIndex = 117;
		this.label8.Text = "Notify Party 1";
		this.cmbLcContractNo.AllowNull = (DefaultableBoolean)1;
		this.cmbLcContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLcContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLcContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance294).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance294).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance294;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance295).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance295).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance295).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance295).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance295;
		((AppearanceBase)appearance296).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance296;
		((SpecialBoxBase)((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance297).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance297).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance297).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance297).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance297;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance298).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance298).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance298;
		((AppearanceBase)appearance299).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance299).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance299;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance300).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance300;
		((AppearanceBase)appearance301).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance301).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance301;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance302).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance302).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance302).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance302).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance302).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance302;
		((AppearanceBase)appearance303).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance303;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance304).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance304).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance304;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance305).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance305;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLcContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbLcContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Location = new System.Drawing.Point(126, 186);
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Name = "cmbLcContractNo";
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).TabIndex = 6;
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).TextChanged += new System.EventHandler(cmbLcContractNo_Leave);
		((System.Windows.Forms.Control)(object)this.cmbLcContractNo).Leave += new System.EventHandler(cmbLcContractNo_Leave);
		this.lblPoNo.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.lblPoNo.Location = new System.Drawing.Point(6, 191);
		this.lblPoNo.Name = "lblPoNo";
		this.lblPoNo.Size = new System.Drawing.Size(96, 21);
		this.lblPoNo.TabIndex = 114;
		this.lblPoNo.Text = "Purchase Order";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.ForeColor = System.Drawing.Color.Red;
		this.label28.Location = new System.Drawing.Point(106, 162);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(14, 17);
		this.label28.TabIndex = 111;
		this.label28.Text = "*";
		this.cmbSupCust.AllowNull = (DefaultableBoolean)1;
		this.cmbSupCust.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupCust.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupCust.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance271).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance271).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Appearance = (AppearanceBase)(object)appearance271;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance272).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance272).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance272).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance272).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance272;
		((AppearanceBase)appearance273).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance273;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance274).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance274).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance274).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance274).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance274;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance275).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance275).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance275;
		((AppearanceBase)appearance276).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance276).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance276;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance277).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance277;
		((AppearanceBase)appearance279).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance279).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance279;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance280).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance280).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance280).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance280).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance280).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance280;
		((AppearanceBase)appearance281).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance281;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance282).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance282).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance282;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance283).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance283;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupCust).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbSupCust.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Location = new System.Drawing.Point(126, 156);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Name = "cmbSupCust";
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).TabIndex = 5;
		this.cmbSupCust.ValueChanged += new System.EventHandler(cmbSupCust_Leave_1);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).TextChanged += new System.EventHandler(cmbSupCust_Leave_1);
		((System.Windows.Forms.Control)(object)this.cmbSupCust).Leave += new System.EventHandler(cmbSupCust_Leave_1);
		this.label21.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label21.Location = new System.Drawing.Point(6, 163);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(96, 19);
		this.label21.TabIndex = 78;
		this.label21.Text = "Supplier Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.Red;
		this.label3.Location = new System.Drawing.Point(106, 72);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(14, 17);
		this.label3.TabIndex = 51;
		this.label3.Text = "*";
		this.label68.AutoSize = true;
		this.label68.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label68.ForeColor = System.Drawing.Color.Red;
		this.label68.Location = new System.Drawing.Point(106, 125);
		this.label68.Name = "label68";
		this.label68.Size = new System.Drawing.Size(14, 17);
		this.label68.TabIndex = 50;
		this.label68.Text = "*";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.Red;
		this.label2.Location = new System.Drawing.Point(106, 102);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(14, 17);
		this.label2.TabIndex = 50;
		this.label2.Text = "*";
		this.txtDocDate.CustomFormat = "dd-MMM-yy";
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(126, 99);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(171, 23);
		this.txtDocDate.TabIndex = 3;
		this.label17.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label17.Location = new System.Drawing.Point(6, 103);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(63, 16);
		this.label17.TabIndex = 48;
		this.label17.Text = "Doc Date";
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(126, 69);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(171, 23);
		this.txtdocno.TabIndex = 2;
		this.txtdocno.TabStop = false;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label5.Location = new System.Drawing.Point(6, 73);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(54, 17);
		this.label5.TabIndex = 49;
		this.label5.Text = "Doc No";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(106, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 43;
		this.label1.Text = "*";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.Color.Red;
		this.label43.Location = new System.Drawing.Point(106, 16);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(14, 17);
		this.label43.TabIndex = 42;
		this.label43.Text = "*";
		this.cmbproject.AllowNull = (DefaultableBoolean)1;
		this.cmbproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance242).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance242).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance242;
		((UltraGridBase)this.cmbproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance243).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance243).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance243).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance243).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance243;
		((AppearanceBase)appearance244).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance244;
		((SpecialBoxBase)((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance245).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance245).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance245).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance245).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance245;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance246).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance246).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance246;
		((AppearanceBase)appearance247).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance247).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance247;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance248).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance248;
		((AppearanceBase)appearance249).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance249).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance249;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance250).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance250).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance250).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance250).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance250).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance250;
		((AppearanceBase)appearance251).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance251;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance252).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance252).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance252;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance253).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance253;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbproject).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbproject).Location = new System.Drawing.Point(126, 41);
		((System.Windows.Forms.Control)(object)this.cmbproject).Name = "cmbproject";
		((System.Windows.Forms.Control)(object)this.cmbproject).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbproject).TabIndex = 1;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label18.Location = new System.Drawing.Point(6, 40);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(46, 15);
		this.label18.TabIndex = 41;
		this.label18.Text = "Project";
		this.cmbbranches.AllowNull = (DefaultableBoolean)1;
		this.cmbbranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbbranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbbranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance254).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance254).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance254;
		((UltraGridBase)this.cmbbranches).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbbranches).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance255).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance255).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance255).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance255).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance255;
		((AppearanceBase)appearance256).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance256;
		((SpecialBoxBase)((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance257).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance257).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance257).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance257).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance257;
		((UltraGridBase)this.cmbbranches).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbbranches).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance258).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance258).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance258;
		((AppearanceBase)appearance259).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance259).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance259;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance260).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance260;
		((AppearanceBase)appearance262).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance262).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance262;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance263).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance263).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance263).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance263).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance263).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance263;
		((AppearanceBase)appearance264).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance264;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance265).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance265).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance265;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance266).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbbranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance266;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbbranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Font = new System.Drawing.Font("Calibri", 9.75f);
		this.cmbbranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbbranches).Location = new System.Drawing.Point(126, 11);
		((System.Windows.Forms.Control)(object)this.cmbbranches).Name = "cmbbranches";
		((System.Windows.Forms.Control)(object)this.cmbbranches).Size = new System.Drawing.Size(171, 26);
		((System.Windows.Forms.Control)(object)this.cmbbranches).TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label13.Location = new System.Drawing.Point(6, 17);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(46, 15);
		this.label13.TabIndex = 40;
		this.label13.Text = "Branch";
		this.panel10.Controls.Add(this.panel6);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 30);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1167, 41);
		this.panel10.TabIndex = 1;
		this.panel6.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.panel6.Controls.Add(this.btnclose);
		this.panel6.Controls.Add(this.label23);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1167, 41);
		this.panel6.TabIndex = 7;
		this.btnclose.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnclose.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.btnclose.FlatAppearance.BorderSize = 0;
		this.btnclose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnclose.Image = (System.Drawing.Image)resources.GetObject("btnclose.Image");
		this.btnclose.Location = new System.Drawing.Point(1112, 7);
		this.btnclose.Margin = new System.Windows.Forms.Padding(2);
		this.btnclose.Name = "btnclose";
		this.btnclose.Size = new System.Drawing.Size(44, 28);
		this.btnclose.TabIndex = 26;
		this.btnclose.UseVisualStyleBackColor = false;
		this.btnclose.Click += new System.EventHandler(btnclose_Click);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(19, 7);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(163, 24);
		this.label23.TabIndex = 25;
		this.label23.Text = "IMPORT INVOICE";
		this.panel12.Controls.Add(this.toolStrip4);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1167, 27);
		this.panel12.TabIndex = 0;
		this.toolStrip4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.btnsave, this.btnupdate, this.btnPrint });
		this.toolStrip4.Location = new System.Drawing.Point(0, 0);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1167, 27);
		this.toolStrip4.TabIndex = 4;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(84, 24);
		this.btnPrint.Text = "806-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1173, 713);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Controls.Add(this.panel15);
		this.panel13.Controls.Add(this.panel16);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1167, 707);
		this.panel13.TabIndex = 0;
		this.panel14.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel14.Location = new System.Drawing.Point(0, 68);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1167, 639);
		this.panel14.TabIndex = 2;
		this.DataGridHistory.AllowDelete = (InheritableBoolean)1;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).Office2007ColorScheme = (Office2007ColorScheme)4;
		((ScrollableBase)this.DataGridHistory).Office2007CustomColor = System.Drawing.Color.LimeGreen;
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1167, 639);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		this.panel15.BackColor = System.Drawing.Color.FromArgb(0, 192, 0);
		this.panel15.Controls.Add(this.label69);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 30);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1167, 38);
		this.panel15.TabIndex = 1;
		this.label69.AutoSize = true;
		this.label69.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label69.ForeColor = System.Drawing.Color.White;
		this.label69.Location = new System.Drawing.Point(17, 6);
		this.label69.Name = "label69";
		this.label69.Size = new System.Drawing.Size(189, 24);
		this.label69.TabIndex = 1;
		this.label69.Text = "Import Invoice History";
		this.panel16.Controls.Add(this.ctrlGrdBar1);
		this.panel16.Controls.Add(this.toolStrip3);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(0, 0);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1167, 30);
		this.panel16.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton6, this.toolStripSeparator1 });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1167, 27);
		this.toolStrip3.TabIndex = 52;
		this.toolStrip3.Text = "toolStrip2";
		this.toolStripButton6.Image = (System.Drawing.Image)resources.GetObject("toolStripButton6.Image");
		this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton6.Name = "toolStripButton6";
		this.toolStripButton6.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton6.Text = "&New";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1181, 739);
		this.panel1.TabIndex = 1;
		this.cmbFCurrencr.AllowNull = (DefaultableBoolean)1;
		this.cmbFCurrencr.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbFCurrencr.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbFCurrencr.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance398).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance398).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Appearance = (AppearanceBase)(object)appearance398;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance399).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance399).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance399).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance399).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance399;
		((AppearanceBase)appearance400).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance400;
		((SpecialBoxBase)((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance410).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance410).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance410).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance410).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance410;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance411).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance411).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance411;
		((AppearanceBase)appearance412).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance412).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance412;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance161).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance163).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance163).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance163).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance163).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance163;
		((AppearanceBase)appearance110).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance111;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbFCurrencr).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbFCurrencr.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Location = new System.Drawing.Point(385, 129);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Name = "cmbFCurrencr";
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbFCurrencr).TabIndex = 12;
		this.cmbPaymentTerm.AllowNull = (DefaultableBoolean)1;
		this.cmbPaymentTerm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbPaymentTerm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbPaymentTerm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance119;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance120;
		((SpecialBoxBase)((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance184).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance186;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbPaymentTerm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbPaymentTerm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Location = new System.Drawing.Point(111, 210);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Name = "cmbPaymentTerm";
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbPaymentTerm).TabIndex = 7;
		this.combranches.AllowNull = (DefaultableBoolean)1;
		this.combranches.AutoCompleteMode = (AutoCompleteMode)4;
		this.combranches.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.combranches.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.combranches).DisplayLayout.Appearance = (AppearanceBase)(object)appearance130;
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
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance137;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.combranches).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance138;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.combranches).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance139;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.combranches).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.combranches).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).TextHAlignAsString = "Left";
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.combranches).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.combranches).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.combranches).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance145;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.combranches).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.combranches).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.combranches).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.combranches.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.combranches).Location = new System.Drawing.Point(1307, 237);
		((System.Windows.Forms.Control)(object)this.combranches).Name = "combranches";
		((System.Windows.Forms.Control)(object)this.combranches).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.combranches).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.combranches).TabStop = false;
		((System.Windows.Forms.Control)(object)this.combranches).Visible = false;
		this.comproject.AllowNull = (DefaultableBoolean)1;
		this.comproject.AutoCompleteMode = (AutoCompleteMode)4;
		this.comproject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.comproject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance146).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.comproject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.comproject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.comproject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance150).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance150).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance150;
		((AppearanceBase)appearance152).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance152;
		((SpecialBoxBase)((UltraGridBase)this.comproject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance153).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance153).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance153).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.comproject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance153;
		((UltraGridBase)this.comproject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.comproject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance154;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance155).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.comproject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance155;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.comproject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance157).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.comproject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance158).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance158).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance158).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance158).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.comproject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).TextHAlignAsString = "Left";
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.comproject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance164).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance164).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance164;
		((UltraGridBase)this.comproject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance165).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.comproject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance165;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.comproject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.comproject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.comproject).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.comproject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.comproject).Location = new System.Drawing.Point(1404, 237);
		((System.Windows.Forms.Control)(object)this.comproject).Name = "comproject";
		((System.Windows.Forms.Control)(object)this.comproject).Size = new System.Drawing.Size(91, 27);
		((System.Windows.Forms.Control)(object)this.comproject).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.comproject).TabStop = false;
		((System.Windows.Forms.Control)(object)this.comproject).Visible = false;
		this.ultraCombo3.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo3.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo3.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo3.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance166).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Appearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance167).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance167).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance167).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance168;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance169).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance169).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance169).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance169;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance170).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance170).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance170;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance171).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance171;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance172).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance172;
		((AppearanceBase)appearance176).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance176).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance177).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance177).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance177).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance177;
		((AppearanceBase)appearance178).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance180;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo3).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo3.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Location = new System.Drawing.Point(111, 91);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Name = "ultraCombo3";
		((System.Windows.Forms.Control)(object)this.ultraCombo3).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo3).TabIndex = 3;
		this.CmbLCContractor.AllowNull = (DefaultableBoolean)1;
		this.CmbLCContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbLCContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbLCContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance181).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance181).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance181;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance182).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance182;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance183;
		((SpecialBoxBase)((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance348).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance348).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance348).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance348).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance348;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance349).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance349).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance349;
		((AppearanceBase)appearance350).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance350).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance350;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance351).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance351;
		((AppearanceBase)appearance352).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance352).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance352;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance353).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance353).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance353).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance353).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance353).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance353;
		((AppearanceBase)appearance354).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance354;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance355).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance355).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance355;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance356).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance356;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbLCContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.CmbLCContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Location = new System.Drawing.Point(111, 120);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Name = "CmbLCContractor";
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.CmbLCContractor).TabIndex = 4;
		this.ultraCombo2.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo2.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo2.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo2.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance309).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance309).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Appearance = (AppearanceBase)(object)appearance309;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance310).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance310).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance310).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance310).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance310;
		((AppearanceBase)appearance311).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance311;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance312).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance312).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance312).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance312).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance312;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance313).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance313).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance313;
		((AppearanceBase)appearance314).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance314).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance314;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance315).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance315;
		((AppearanceBase)appearance316).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance316).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance316;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance317).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance317).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance317).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance317).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance317).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance317;
		((AppearanceBase)appearance318).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance318;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance319).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance319).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance319;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance320).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance320;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo2).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo2.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Location = new System.Drawing.Point(385, 37);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Name = "ultraCombo2";
		((System.Windows.Forms.Control)(object)this.ultraCombo2).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo2).TabIndex = 9;
		this.ultraCombo1.AllowNull = (DefaultableBoolean)1;
		this.ultraCombo1.AutoCompleteMode = (AutoCompleteMode)4;
		this.ultraCombo1.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.ultraCombo1.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance357).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance357).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Appearance = (AppearanceBase)(object)appearance357;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance358).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance358).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance358).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance358).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance358;
		((AppearanceBase)appearance359).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance359;
		((SpecialBoxBase)((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance360).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance360).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance360).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance360).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance360;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance361).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance361).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance361;
		((AppearanceBase)appearance362).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance362).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance362;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance363).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance363;
		((AppearanceBase)appearance364).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance364).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance364;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance365).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance365).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance365).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance365).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance365).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance365;
		((AppearanceBase)appearance321).TextHAlignAsString = "Left";
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance321;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance322).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance322).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance322;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance323).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance323;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.ultraCombo1).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.ultraCombo1.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Location = new System.Drawing.Point(385, 67);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Name = "ultraCombo1";
		((System.Windows.Forms.Control)(object)this.ultraCombo1).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.ultraCombo1).TabIndex = 10;
		this.cmbCarierType.AllowNull = (DefaultableBoolean)1;
		this.cmbCarierType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbCarierType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbCarierType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance366).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance366).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance366;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance367).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance367).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance367).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance367).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance367;
		((AppearanceBase)appearance368).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance368;
		((SpecialBoxBase)((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance369).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance369).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance369).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance369).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance369;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance370).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance370).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance370;
		((AppearanceBase)appearance371).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance371).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance371;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance372).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance372;
		((AppearanceBase)appearance374).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance374).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance374;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance375).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance375).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance375).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance375).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance375).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance375;
		((AppearanceBase)appearance376).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance376;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance377).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance377).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance377;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance378).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance378;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbCarierType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbCarierType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Location = new System.Drawing.Point(385, 96);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Name = "cmbCarierType";
		((System.Windows.Forms.Control)(object)this.cmbCarierType).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbCarierType).TabIndex = 11;
		this.cmbExporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbExporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbExporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbExporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance379).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance379).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance379;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance380).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance380).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance380).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance380).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance380;
		((AppearanceBase)appearance381).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance381;
		((SpecialBoxBase)((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance187).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance187;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance188).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance188;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance191).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance192).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance192).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance192).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance192).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance194;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance195).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbExporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbExporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Location = new System.Drawing.Point(111, 180);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Name = "cmbExporterBank";
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbExporterBank).TabIndex = 6;
		this.cmbImporterBank.AllowNull = (DefaultableBoolean)1;
		this.cmbImporterBank.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbImporterBank.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbImporterBank.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance382).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance382).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Appearance = (AppearanceBase)(object)appearance382;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance383).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance383).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance383).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance383).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance383;
		((AppearanceBase)appearance385).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance385;
		((SpecialBoxBase)((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance386).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance386).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance386).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance386).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance386;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance387).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance387).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance387;
		((AppearanceBase)appearance388).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance388).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance388;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance389).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance389;
		((AppearanceBase)appearance390).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance390).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance390;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance391).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance391).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance391).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance391).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance391).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance391;
		((AppearanceBase)appearance392).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance392;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance393).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance393).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance393;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance394).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance394;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbImporterBank).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbImporterBank.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Location = new System.Drawing.Point(111, 151);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Name = "cmbImporterBank";
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).Size = new System.Drawing.Size(163, 27);
		((System.Windows.Forms.Control)(object)this.cmbImporterBank).TabIndex = 5;
		this.cmbEFormNo.AllowNull = (DefaultableBoolean)1;
		this.cmbEFormNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEFormNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEFormNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance324).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance324).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance324;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance325).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance325).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance325).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance325).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance325;
		((AppearanceBase)appearance326).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance326;
		((SpecialBoxBase)((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance327).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance327).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance327).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance327).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance327;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance328).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance328).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance328;
		((AppearanceBase)appearance329).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance329).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance329;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance330).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance330;
		((AppearanceBase)appearance331).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance331).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance331;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance332).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance332).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance332).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance332).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance332).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance332;
		((AppearanceBase)appearance333).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance333;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance334).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance334).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance334;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance335).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance335;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEFormNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbEFormNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Location = new System.Drawing.Point(662, 164);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Name = "cmbEFormNo";
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).Size = new System.Drawing.Size(164, 22);
		((System.Windows.Forms.Control)(object)this.cmbEFormNo).TabIndex = 0;
		this.cmbdeliveryterm.AllowNull = (DefaultableBoolean)1;
		this.cmbdeliveryterm.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbdeliveryterm.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbdeliveryterm.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance336).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance336).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Appearance = (AppearanceBase)(object)appearance336;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance337).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance337).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance337).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance337).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance337;
		((AppearanceBase)appearance338).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance338;
		((SpecialBoxBase)((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance339).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance339).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance339).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance339).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance339;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance340).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance340).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance340;
		((AppearanceBase)appearance341).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance341).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance341;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance342).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance342;
		((AppearanceBase)appearance343).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance343).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance343;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance344).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance344).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance344).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance344).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance344).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance344;
		((AppearanceBase)appearance345).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance345;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance346).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance346).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance346;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance347).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance347;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbdeliveryterm).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Font = new System.Drawing.Font("Segoe UI", 9.75f);
		this.cmbdeliveryterm.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Location = new System.Drawing.Point(385, 6);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Name = "cmbdeliveryterm";
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).Size = new System.Drawing.Size(159, 27);
		((System.Windows.Forms.Control)(object)this.cmbdeliveryterm).TabIndex = 8;
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(346, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(280, 27);
		this.miniToolStrip.TabIndex = 0;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(35, 24);
		this.toolStripButton1.Text = "&New";
		this.toolStripSeparator.Name = "toolStripSeparator";
		this.toolStripSeparator.Size = new System.Drawing.Size(6, 27);
		this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.toolStripButton1, this.toolStripButton2, this.toolStripButton3, this.toolStripSeparator, this.toolStripButton4, this.toolStripButton5, this.helpToolStripButton });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(280, 27);
		this.toolStrip2.TabIndex = 0;
		this.toolStrip2.Text = "toolStrip1";
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(55, 24);
		this.toolStripButton2.Text = "&Save";
		this.toolStripButton3.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton3.Name = "toolStripButton3";
		this.toolStripButton3.Size = new System.Drawing.Size(69, 24);
		this.toolStripButton3.Text = "&Update";
		this.toolStripButton3.Visible = false;
		this.toolStripButton4.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton4.Name = "toolStripButton4";
		this.toolStripButton4.Size = new System.Drawing.Size(94, 24);
		this.toolStripButton4.Text = "&Attachment";
		this.toolStripButton5.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton5.Name = "toolStripButton5";
		this.toolStripButton5.Size = new System.Drawing.Size(54, 24);
		this.toolStripButton5.Text = "&SMS";
		this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.helpToolStripButton.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.helpToolStripButton.Name = "helpToolStripButton";
		this.helpToolStripButton.Size = new System.Drawing.Size(24, 24);
		this.helpToolStripButton.Text = "He&lp";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1121, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 21);
		this.ctrlGrdBar1.TabIndex = 45;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1181, 739);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ImCommercialInvoice";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ImProformaInvoice_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ImProformaInvoice_KeyDown);
		this.panel4.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.tabPage4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdShipmentExpenses).EndInit();
		this.HeaderGropBox.ResumeLayout(false);
		this.HeaderGropBox.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbcareiertype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbHomeCurrency).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbfcycode).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDestinationPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingPort).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbexporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbimporterBankNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTermsNew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliverytermnew).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbNotifyParty1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLcContractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupCust).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbbranches).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		this.panel14.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.cmbFCurrencr).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbPaymentTerm).EndInit();
		((System.ComponentModel.ISupportInitialize)this.combranches).EndInit();
		((System.ComponentModel.ISupportInitialize)this.comproject).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo3).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbLCContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo2).EndInit();
		((System.ComponentModel.ISupportInitialize)this.ultraCombo1).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbCarierType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbExporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbImporterBank).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbEFormNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbdeliveryterm).EndInit();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
