using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.ContractorWages;
using Architecture.Common;
using Architecture.Model;
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
namespace Architecture.WinApp.Inventory_Reports
{

public class frmWagesDashboardReport : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtSummaryByDocType = new DataTable();

	private DataTable dtSummaryByContractor = new DataTable();

	private DataTable dtSummaryByWagesAc = new DataTable();

	private DataTable dtDetailSummaryByDocType = new DataTable();

	private DataTable dtDetailSummaryByContractor = new DataTable();

	private DataTable dtDetailActivityDocumentType = new DataTable();

	private DataTable dtDetailDocumentTypeActivity = new DataTable();

	private DataTable dtDetailByDocType = new DataTable();

	private DataTable dtDetailByContractor = new DataTable();

	private IContainer components;

	private Panel PanelDetail;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnPrint;

	private Panel panel3;

	private Button btnShow;

	internal UltraCombo cmbWagesAccount;

	private Label label12;

	internal UltraCombo cmbDocumentType;

	private Label label10;

	private Label label2;

	private DateTimePicker txtToDate;

	private Label label1;

	private DateTimePicker txtDateFrom;

	internal UltraCombo cmbContractor;

	private Label label3;

	internal GridEX grdSummaryByDocType;

	private Panel panel5;

	internal GridEX grdSummarybyWagesAc;

	internal GridEX grdSummarybyContractor;

	internal GridEX GrdiDetailByContractory;

	private CtrlGrdBar ctrlGrdDetailByContractor;

	private Label label9;

	internal GridEX grdDetailSummaryByContractor;

	private CtrlGrdBar ctrlGrdDetaSummByDontractor;

	private Label label11;

	private GroupBox groupBox1;

	private Panel panel22;

	private Panel panel24;

	private Panel panel26;

	private Label label13;

	private Panel panel6;

	private Panel panel13;

	private Label label4;

	private TabControl tabControl1;

	private TabPage tabPage2;

	private Panel panel7;

	private Panel panel8;

	private Label label5;

	private Panel panel4;

	private Panel panel17;

	private Panel panel18;

	private Panel panel10;

	private Panel panel19;

	private Panel panel11;

	private TabPage tabPage3;

	private TabPage tabPage4;

	private Panel panel20;

	private Panel panel21;

	private Panel panel23;

	internal GridEX grdActivityaAndDocumentType;

	private Panel panel25;

	private CtrlGrdBar ctrlGrdActivityDocumentType;

	private Label label6;

	private Panel panel27;

	private Panel panel28;

	private Panel panel29;

	internal GridEX grdDocumentTypeAndActivity;

	private Panel panel30;

	private CtrlGrdBar ctrlGrdDocumentTypeActivity;

	private Label label14;

	public frmWagesDashboardReport(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmEvaulationDetailSalesReports_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1150, 620);
			dtSummaryByDocType.Columns.Add("ActivityDiscription");
			dtSummaryByDocType.Columns.Add("RefDocumentTypeId", typeof(int));
			dtSummaryByDocType.Columns.Add("RefDocumentType");
			dtSummaryByDocType.Columns.Add("Weight", typeof(double));
			dtSummaryByDocType.Columns.Add("Qty", typeof(double));
			dtSummaryByDocType.Columns.Add("Amount", typeof(double));
			dtSummaryByDocType.Columns.Add("AvgRate/40Kg", typeof(double));
			dtSummaryByContractor.Columns.Add("ActivityDiscription");
			dtSummaryByContractor.Columns.Add("ContractorNameId", typeof(int));
			dtSummaryByContractor.Columns.Add("ContractorName");
			dtSummaryByContractor.Columns.Add("Weight", typeof(double));
			dtSummaryByContractor.Columns.Add("Qty", typeof(double));
			dtSummaryByContractor.Columns.Add("Amount", typeof(double));
			dtSummaryByWagesAc.Columns.Add("ActivityDiscription");
			dtSummaryByWagesAc.Columns.Add("WagesAccountId", typeof(int));
			dtSummaryByWagesAc.Columns.Add("WagesAccount");
			dtSummaryByWagesAc.Columns.Add("Weight", typeof(double));
			dtSummaryByWagesAc.Columns.Add("Qty", typeof(double));
			dtSummaryByWagesAc.Columns.Add("Amount", typeof(double));
			dtSummaryByWagesAc.Columns.Add("AvgRate/40Kg", typeof(double));
			dtDetailActivityDocumentType.Columns.Add("RefDocumentTypeId", typeof(int));
			dtDetailActivityDocumentType.Columns.Add("RefDocumentType");
			dtDetailActivityDocumentType.Columns.Add("WagesAccountId", typeof(int));
			dtDetailActivityDocumentType.Columns.Add("WagesAccount");
			dtDetailActivityDocumentType.Columns.Add("RateEffectedFrom", typeof(DateTime));
			dtDetailActivityDocumentType.Columns.Add("PackSize");
			dtDetailActivityDocumentType.Columns.Add("WageRate");
			dtDetailActivityDocumentType.Columns.Add("Weight", typeof(double));
			dtDetailActivityDocumentType.Columns.Add("Qty", typeof(double));
			dtDetailActivityDocumentType.Columns.Add("Amount", typeof(double));
			dtDetailActivityDocumentType.Columns.Add("AvgRate/40Kg", typeof(double));
			dtDetailDocumentTypeActivity.Columns.Add("RefDocumentTypeId", typeof(int));
			dtDetailDocumentTypeActivity.Columns.Add("RefDocumentType");
			dtDetailDocumentTypeActivity.Columns.Add("WagesAccountId", typeof(int));
			dtDetailDocumentTypeActivity.Columns.Add("WagesAccount");
			dtDetailDocumentTypeActivity.Columns.Add("RateEffectedFrom", typeof(DateTime));
			dtDetailDocumentTypeActivity.Columns.Add("PackSize");
			dtDetailDocumentTypeActivity.Columns.Add("WageRate");
			dtDetailDocumentTypeActivity.Columns.Add("Weight", typeof(double));
			dtDetailDocumentTypeActivity.Columns.Add("Qty", typeof(double));
			dtDetailDocumentTypeActivity.Columns.Add("Amount", typeof(double));
			dtDetailDocumentTypeActivity.Columns.Add("AvgRate/40Kg", typeof(double));
			dtDetailSummaryByContractor.Columns.Add("RefDocumentTypeId", typeof(int));
			dtDetailSummaryByContractor.Columns.Add("RefDocumentType");
			dtDetailSummaryByContractor.Columns.Add("WagesAccountId", typeof(int));
			dtDetailSummaryByContractor.Columns.Add("WagesAccount");
			dtDetailSummaryByContractor.Columns.Add("ContractorNameId", typeof(int));
			dtDetailSummaryByContractor.Columns.Add("ContractorName");
			dtDetailSummaryByContractor.Columns.Add("Weight", typeof(double));
			dtDetailSummaryByContractor.Columns.Add("Qty", typeof(double));
			dtDetailSummaryByContractor.Columns.Add("Amount", typeof(double));
			dtDetailByContractor.Columns.Add("RefDocumentTypeId", typeof(int));
			dtDetailByContractor.Columns.Add("RefDocumentType");
			dtDetailByContractor.Columns.Add("WagesAccountId", typeof(int));
			dtDetailByContractor.Columns.Add("WagesAccount");
			dtDetailByContractor.Columns.Add("ContractorNameId", typeof(int));
			dtDetailByContractor.Columns.Add("ContractorName");
			dtDetailByContractor.Columns.Add("PackSize");
			dtDetailByContractor.Columns.Add("WageRate", typeof(double));
			dtDetailByContractor.Columns.Add("Weight", typeof(double));
			dtDetailByContractor.Columns.Add("Qty", typeof(double));
			dtDetailByContractor.Columns.Add("Amount", typeof(double));
			txtDateFrom.Value = DateTime.Today.AddDays(-7.0);
			txtDateFrom.Focus();
			AllGridFill();
			ReferenceDocumentType();
			WagesAccountFill();
			WagesContractorFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReferenceDocumentType()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDocType = InvContractorWagesBillHeader.GetDocumentTypesFromContractorWages(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDocType.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtDocType, cmbDocumentType, "Id", "DocumentTypeDescription", "Reference Document Type", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesAccountFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtWagesAc = InvContractorWagesBillHeader.GetWagesAccounts(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtWagesAc.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWagesAc, cmbWagesAccount, "AccountId", "WagesAccountName", "Wages Account", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesContractorFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtWagesCont = InvContractorWagesBillHeader.GetWagesContractors(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtWagesCont.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWagesCont, cmbContractor, "ContractorId", "ContractorName", "Wages Contractor", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		try
		{
			DataSet DtSet = InvContractorWagesBillHeader.ContractorWagesDashboardsandReport(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				FromDate = Conversion.ToDateTime((object)txtDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)txtToDate.Value),
				ContractorId = Conversion.ToInt(cmbContractor.Value),
				WagesItemId = Conversion.ToInt(cmbWagesAccount.Value),
				RefDocumentTypeId = Conversion.ToInt(cmbDocumentType.Value)
			});
			if (DtSet.Tables[0].Rows.Count > 0)
			{
				DataTable dt1 = DtSet.Tables[0];
				dtSummaryByDocType.Rows.Clear();
				dtSummaryByContractor.Rows.Clear();
				dtSummaryByWagesAc.Rows.Clear();
				for (int m = 0; m < dt1.Rows.Count; m++)
				{
					if (Conversion.ToString(dt1.Rows[m]["ActivityDescription"]) == "Summary By Document Type")
					{
						dtSummaryByDocType.Rows.Add(dt1.Rows[m]["ActivityDescription"], dt1.Rows[m]["RefDocumentTypeId"], dt1.Rows[m]["RefDocumentType"], dt1.Rows[m]["WagesWeight"], dt1.Rows[m]["WagesQty"], dt1.Rows[m]["WagesAmount"], dt1.Rows[m]["AvgRatePer40Kg"]);
					}
					if (Conversion.ToString(dt1.Rows[m]["ActivityDescription"]) == "Summary By Contractor")
					{
						dtSummaryByContractor.Rows.Add(dt1.Rows[m]["ActivityDescription"], dt1.Rows[m]["RefDocumentTypeId"], dt1.Rows[m]["RefDocumentType"], dt1.Rows[m]["WagesWeight"], dt1.Rows[m]["WagesQty"], dt1.Rows[m]["WagesAmount"]);
					}
					if (Conversion.ToString(dt1.Rows[m]["ActivityDescription"]) == "Summary By Wages Account")
					{
						dtSummaryByWagesAc.Rows.Add(dt1.Rows[m]["ActivityDescription"], dt1.Rows[m]["RefDocumentTypeId"], dt1.Rows[m]["RefDocumentType"], dt1.Rows[m]["WagesWeight"], dt1.Rows[m]["WagesQty"], dt1.Rows[m]["WagesAmount"], dt1.Rows[m]["AvgRatePer40Kg"]);
					}
				}
				if (dtSummaryByDocType.Rows.Count > 0)
				{
					grdSummaryByDocType.DataSource = dtSummaryByDocType;
					grdSummaryByDocType.RetrieveStructure();
					GridSummaryByDocTypeSetting();
				}
				else
				{
					grdSummaryByDocType.DataSource = null;
				}
				if (dtSummaryByContractor.Rows.Count > 0)
				{
					grdSummarybyContractor.DataSource = dtSummaryByContractor;
					grdSummarybyContractor.RetrieveStructure();
					GridSummaryByContractorSetting();
				}
				else
				{
					grdSummarybyContractor.DataSource = null;
				}
				if (dtSummaryByWagesAc.Rows.Count > 0)
				{
					grdSummarybyWagesAc.DataSource = dtSummaryByWagesAc;
					grdSummarybyWagesAc.RetrieveStructure();
					GridSummaryByWagesAcSetting();
				}
				else
				{
					grdSummarybyWagesAc.DataSource = null;
				}
			}
			else
			{
				dtSummaryByDocType.Rows.Clear();
				dtSummaryByContractor.Rows.Clear();
				dtSummaryByWagesAc.Rows.Clear();
			}
			if (DtSet.Tables[1].Rows.Count > 0)
			{
				DataTable dt2 = DtSet.Tables[1];
				dtDetailActivityDocumentType.Rows.Clear();
				for (int l = 0; l < dt2.Rows.Count; l++)
				{
					dtDetailActivityDocumentType.Rows.Add(dt2.Rows[l]["RefDocumentTypeId"], dt2.Rows[l]["RefDocumentType"], dt2.Rows[l]["InvConractorWagesAccountsId"], dt2.Rows[l]["WagesAccountName"], dt2.Rows[l]["RateEffectedFrom"], dt2.Rows[l]["PackSize"], dt2.Rows[l]["WageRate"], dt2.Rows[l]["WagesWeight"], dt2.Rows[l]["WagesQty"], dt2.Rows[l]["WagesAmount"], dt2.Rows[l]["AvgRatePer40Kg"]);
				}
				if (dtDetailActivityDocumentType.Rows.Count > 0)
				{
					grdActivityaAndDocumentType.DataSource = dtDetailActivityDocumentType;
					grdActivityaAndDocumentType.RetrieveStructure();
					grdActivityAndDocumentTypeSetting();
				}
				else
				{
					grdActivityaAndDocumentType.DataSource = null;
				}
			}
			else
			{
				dtDetailActivityDocumentType.Rows.Clear();
			}
			if (DtSet.Tables[2].Rows.Count > 0)
			{
				DataTable dt3 = DtSet.Tables[2];
				dtDetailDocumentTypeActivity.Rows.Clear();
				for (int k = 0; k < dt3.Rows.Count; k++)
				{
					dtDetailDocumentTypeActivity.Rows.Add(dt3.Rows[k]["RefDocumentTypeId"], dt3.Rows[k]["RefDocumentType"], dt3.Rows[k]["InvConractorWagesAccountsId"], dt3.Rows[k]["WagesAccountName"], dt3.Rows[k]["RateEffectedFrom"], dt3.Rows[k]["PackSize"], dt3.Rows[k]["WageRate"], dt3.Rows[k]["WagesWeight"], dt3.Rows[k]["WagesQty"], dt3.Rows[k]["WagesAmount"], dt3.Rows[k]["AvgRatePer40Kg"]);
				}
				if (dtDetailDocumentTypeActivity.Rows.Count > 0)
				{
					grdDocumentTypeAndActivity.DataSource = dtDetailDocumentTypeActivity;
					grdDocumentTypeAndActivity.RetrieveStructure();
					grdDocumentTypeAndActivitySetting();
				}
				else
				{
					grdDocumentTypeAndActivity.DataSource = null;
				}
			}
			else
			{
				dtDetailDocumentTypeActivity.Rows.Clear();
			}
			if (DtSet.Tables[3].Rows.Count > 0)
			{
				DataTable dt4 = DtSet.Tables[3];
				dtDetailSummaryByContractor.Rows.Clear();
				for (int j = 0; j < dt4.Rows.Count; j++)
				{
					dtDetailSummaryByContractor.Rows.Add(dt4.Rows[j]["RefDocumentTypeId"], dt4.Rows[j]["RefDocumentType"], dt4.Rows[j]["InvConractorWagesAccountsId"], dt4.Rows[j]["WagesAccountName"], dt4.Rows[j]["ContractorId"], dt4.Rows[j]["ContractorName"], dt4.Rows[j]["WagesWeight"], dt4.Rows[j]["WagesQty"], dt4.Rows[j]["WagesAmount"]);
				}
				if (dtDetailSummaryByContractor.Rows.Count > 0)
				{
					grdDetailSummaryByContractor.DataSource = dtDetailSummaryByContractor;
					grdDetailSummaryByContractor.RetrieveStructure();
					grdDetailSummaryByContractorSetting();
				}
				else
				{
					grdDetailSummaryByContractor.DataSource = null;
				}
			}
			else
			{
				dtDetailSummaryByContractor.Rows.Clear();
			}
			if (DtSet.Tables[4].Rows.Count > 0)
			{
				DataTable dt5 = DtSet.Tables[4];
				dtDetailByContractor.Rows.Clear();
				for (int i = 0; i < dt5.Rows.Count; i++)
				{
					dtDetailByContractor.Rows.Add(dt5.Rows[i]["RefDocumentTypeId"], dt5.Rows[i]["RefDocumentType"], dt5.Rows[i]["InvConractorWagesAccountsId"], dt5.Rows[i]["WagesAccountName"], dt5.Rows[i]["ContractorId"], dt5.Rows[i]["ContractorName"], dt5.Rows[i]["PackSize"], dt5.Rows[i]["WageRate"], dt5.Rows[i]["WagesWeight"], dt5.Rows[i]["WagesQty"], dt5.Rows[i]["WagesAmount"]);
				}
				if (dtDetailByContractor.Rows.Count > 0)
				{
					GrdiDetailByContractory.DataSource = dtDetailByContractor;
					GrdiDetailByContractory.RetrieveStructure();
					grdDetailByContractorSetting();
				}
				else
				{
					GrdiDetailByContractory.DataSource = null;
				}
			}
			else
			{
				dtDetailByContractor.Rows.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryByDocTypeSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "ActivityDiscription", "RefDocumentTypeId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummaryByDocType, 2, 2, hiddenColumns);
			grdSummaryByDocType.RootTable.Columns["RefDocumentType"].Width = 170;
			grdSummaryByDocType.RootTable.Columns["RefDocumentType"].ColumnType = (ColumnType)5;
			grdSummaryByDocType.RootTable.Columns["AvgRate/40Kg"].Width = 90;
			grdSummaryByDocType.RootTable.Columns["AvgRate/40Kg"].Caption = "AvgRate / 40Kg";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryByContractorSetting()
	{
		try
		{
			grdSummarybyContractor.RootTable.Columns["ContractorName"].Width = 170;
			grdSummarybyContractor.RootTable.Columns["ContractorName"].ColumnType = (ColumnType)5;
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummarybyContractor, 2, 2);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSummaryByWagesAcSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "ActivityDiscription", "WagesAccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummarybyWagesAc, 2, 2, hiddenColumns);
			grdSummarybyWagesAc.RootTable.Columns["WagesAccount"].ColumnType = (ColumnType)5;
			grdSummarybyWagesAc.RootTable.Columns["WagesAccount"].Caption = "Labour / WAges Activity";
			grdSummarybyWagesAc.RootTable.Columns["WagesAccount"].Width = 170;
			grdSummarybyWagesAc.RootTable.Columns["Weight"].Width = 80;
			grdSummarybyWagesAc.RootTable.Columns["Qty"].Width = 70;
			grdSummarybyWagesAc.RootTable.Columns["Amount"].Width = 90;
			grdSummarybyWagesAc.RootTable.Columns["AvgRate/40Kg"].Width = 90;
			grdSummarybyWagesAc.RootTable.Columns["AvgRate/40Kg"].Caption = "AvgRate / 40Kg";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdActivityAndDocumentTypeSetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "RefDocumentTypeId", "WagesAccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdActivityaAndDocumentType, 2, 2, hiddenColumns);
			grdActivityaAndDocumentType.GroupByBoxVisible = true;
			grdActivityaAndDocumentType.RootTable.Groups.Add("WagesAccount");
			grdActivityaAndDocumentType.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdActivityaAndDocumentType.GroupTotals = (GroupTotals)2;
			grdActivityaAndDocumentType.RootTable.Columns["WagesAccount"].ColumnType = (ColumnType)5;
			grdActivityaAndDocumentType.RootTable.Columns["AvgRate/40Kg"].Caption = "AvgRate / 40Kg";
			grdActivityaAndDocumentType.RootTable.Columns["WagesAccount"].Caption = "Labour / Wages Activity";
			grdActivityaAndDocumentType.RootTable.Columns["RefDocumentType"].Width = 580;
			grdActivityaAndDocumentType.RootTable.Columns["RateEffectedFrom"].Width = 100;
			grdActivityaAndDocumentType.RootTable.Columns["PackSize"].Width = 60;
			grdActivityaAndDocumentType.RootTable.Columns["WageRate"].Width = 70;
			grdActivityaAndDocumentType.RootTable.Columns["Weight"].Width = 95;
			grdActivityaAndDocumentType.RootTable.Columns["Qty"].Width = 95;
			grdActivityaAndDocumentType.RootTable.Columns["Amount"].Width = 90;
			grdActivityaAndDocumentType.RootTable.Columns["AvgRate/40Kg"].Width = 90;
			ctrlGrdActivityDocumentType_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocumentTypeAndActivitySetting()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "RefDocumentTypeId", "WagesAccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDocumentTypeAndActivity, 2, 2, hiddenColumns);
			grdDocumentTypeAndActivity.GroupByBoxVisible = true;
			grdDocumentTypeAndActivity.RootTable.Groups.Add("RefDocumentType");
			grdDocumentTypeAndActivity.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdDocumentTypeAndActivity.GroupTotals = (GroupTotals)2;
			grdDocumentTypeAndActivity.RootTable.Columns["WagesAccount"].ColumnType = (ColumnType)5;
			grdDocumentTypeAndActivity.RootTable.Columns["AvgRate/40Kg"].Caption = "AvgRate / 40Kg";
			grdDocumentTypeAndActivity.RootTable.Columns["WagesAccount"].Caption = "Labour / Wages Activity";
			grdDocumentTypeAndActivity.RootTable.Columns["RefDocumentType"].Width = 150;
			grdDocumentTypeAndActivity.RootTable.Columns["WagesAccount"].Width = 580;
			grdDocumentTypeAndActivity.RootTable.Columns["RateEffectedFrom"].Width = 100;
			grdDocumentTypeAndActivity.RootTable.Columns["PackSize"].Width = 60;
			grdDocumentTypeAndActivity.RootTable.Columns["WageRate"].Width = 70;
			grdDocumentTypeAndActivity.RootTable.Columns["Weight"].Width = 95;
			grdDocumentTypeAndActivity.RootTable.Columns["Qty"].Width = 95;
			grdDocumentTypeAndActivity.RootTable.Columns["Amount"].Width = 90;
			grdDocumentTypeAndActivity.RootTable.Columns["AvgRate/40Kg"].Width = 90;
			ctrlGrdDocumentTypeActivity_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSummaryByContractorSetting()
	{
		try
		{
			grdDetailSummaryByContractor.GroupByBoxVisible = true;
			grdDetailSummaryByContractor.RootTable.Groups.Add("ContractorName");
			grdDetailSummaryByContractor.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdDetailSummaryByContractor.GroupTotals = (GroupTotals)2;
			List<string> hiddenColumns = new List<string> { "ContractorNameId", "RefDocumentTypeId", "WagesAccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetailSummaryByContractor, 2, 2, hiddenColumns);
			grdDetailSummaryByContractor.RootTable.Columns["WagesAccount"].Caption = "Labour / Wages Activity";
			grdDetailSummaryByContractor.RootTable.Columns["ContractorName"].Width = 170;
			grdDetailSummaryByContractor.RootTable.Columns["RefDocumentType"].Width = 170;
			grdDetailSummaryByContractor.RootTable.Columns["WagesAccount"].Width = 250;
			grdDetailSummaryByContractor.RootTable.Columns["RefDocumentType"].ColumnType = (ColumnType)5;
			grdDetailSummaryByContractor.RootTable.Columns["Weight"].Width = 80;
			grdDetailSummaryByContractor.RootTable.Columns["Qty"].Width = 70;
			grdDetailSummaryByContractor.RootTable.Columns["Amount"].Width = 90;
			ctrlGrdDetaSummByDontractor_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailByContractorSetting()
	{
		try
		{
			GrdiDetailByContractory.RootTable.Groups.Add("ContractorName");
			GrdiDetailByContractory.HideColumnsWhenGrouped = (InheritableBoolean)1;
			GrdiDetailByContractory.GroupTotals = (GroupTotals)2;
			List<string> hiddenColumns = new List<string> { "ContractorNameId", "RefDocumentTypeId", "WagesAccountId" };
			GridEX_Helper.GridWrappingAndColumnSettings(GrdiDetailByContractory, 2, 2, hiddenColumns);
			GrdiDetailByContractory.RootTable.Columns["WagesAccount"].Caption = "Labour / Wages Activity";
			GrdiDetailByContractory.RootTable.Columns["ContractorName"].Width = 200;
			GrdiDetailByContractory.RootTable.Columns["RefDocumentType"].ColumnType = (ColumnType)5;
			GrdiDetailByContractory.RootTable.Columns["RefDocumentType"].Width = 140;
			GrdiDetailByContractory.RootTable.Columns["WagesAccount"].Width = 125;
			GrdiDetailByContractory.RootTable.Columns["Weight"].Width = 80;
			GrdiDetailByContractory.RootTable.Columns["Qty"].Width = 70;
			GrdiDetailByContractory.RootTable.Columns["WageRate"].Width = 60;
			GrdiDetailByContractory.RootTable.Columns["PackSize"].Width = 60;
			GrdiDetailByContractory.RootTable.Columns["Amount"].Width = 90;
			ctrlGrdDetailByContractor_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSummarybyContractor_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSummarybyContractor.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HandleLinkClicked(grdSummarybyContractor, r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSummaryByDocType_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSummaryByDocType.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HandleLinkClicked(grdSummaryByDocType, r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSummarybyWagesAc_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSummarybyWagesAc.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HandleLinkClicked(grdSummarybyWagesAc, r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSummaryByDocType_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdActivityaAndDocumentType.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HandleLinkClicked(grdActivityaAndDocumentType, r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDetailByDocType_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDocumentTypeAndActivity.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HandleLinkClicked(grdDocumentTypeAndActivity, r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetailSummaryByContractor_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetailSummaryByContractor.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HandleLinkClicked(grdDetailSummaryByContractor, r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdiDetailByContractory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = GrdiDetailByContractory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				HandleLinkClicked(GrdiDetailByContractory, r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HandleLinkClicked(GridEX grd, GridEXRow row)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (row == null || (int)row.RowType != 0 || row.Cells.Count <= 0)
			{
				return;
			}
			int RefDocumentTypeId = 0;
			int ContractorNameId = 0;
			int WagesAccountId = 0;
			if (grd.RootTable.Columns.Contains("ContractorNameId") && row.Cells["ContractorNameId"].Value != null)
			{
				ContractorNameId = Conversion.ToInt(row.Cells["ContractorNameId"].Value);
			}
			if (grd.RootTable.Columns.Contains("WagesAccountId"))
			{
				GridEXCell obj = row.Cells["WagesAccountId"];
				if (((obj != null) ? obj.Value : null) != null)
				{
					WagesAccountId = Conversion.ToInt(row.Cells["WagesAccountId"].Value);
				}
			}
			if (grd.RootTable.Columns.Contains("RefDocumentTypeId"))
			{
				GridEXCell obj2 = row.Cells["RefDocumentTypeId"];
				if (((obj2 != null) ? obj2.Value : null) != null)
				{
					RefDocumentTypeId = Conversion.ToInt(row.Cells["RefDocumentTypeId"].Value);
				}
			}
			tblUserRights screenRights = clsGlobalVariables.ScreenViewReights.FirstOrDefault((tblUserRights x) => x.ScreenName.Trim() == "frmEvaulationDetailWagesReports");
			if ((screenRights != null && Conversion.ToBool((object)screenRights.Value)) || UserAccount.RoleName == "Admin")
			{
				OpenWagesReportScreen(RefDocumentTypeId, ContractorNameId, WagesAccountId);
			}
			else
			{
				MessageBox.Show("You don't have the rights to view this form.");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OpenWagesReportScreen(int RefDocumentTypeId, int ContractorNameId, int WagesAccountId)
	{
		frmEvaulationDetailWagesReports reportForm = new frmEvaulationDetailWagesReports(UserAccount)
		{
			txtDateFrom = 
			{
				Value = txtDateFrom.Value
			},
			txtToDate = 
			{
				Value = txtToDate.Value
			}
		};
		if (RefDocumentTypeId != 0)
		{
			reportForm.cmbDocumentType.Value = RefDocumentTypeId;
		}
		else if (Conversion.ToInt(cmbDocumentType.Value) != 0)
		{
			reportForm.cmbDocumentType.Value = Conversion.ToInt(cmbDocumentType.Value);
		}
		if (ContractorNameId != 0)
		{
			reportForm.CmbContractorName.Value = ContractorNameId;
		}
		else if (Conversion.ToInt(cmbContractor.Value) != 0)
		{
			reportForm.CmbContractorName.Value = Conversion.ToInt(cmbContractor.Value);
		}
		if (WagesAccountId != 0)
		{
			reportForm.CmbWagesAccount.Value = WagesAccountId;
		}
		else if (Conversion.ToInt(cmbWagesAccount.Value) != 0)
		{
			reportForm.CmbWagesAccount.Value = Conversion.ToInt(cmbWagesAccount.Value);
		}
		reportForm.Show();
		reportForm.BringToFront();
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			AllGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
	}

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbWagesAccount).ActiveRow != null)
			{
				if (((Control)(object)cmbWagesAccount).Text == "Contractor Wages Register")
				{
					btnPrint.Text = "160-WagesRegister";
				}
				if (((Control)(object)cmbWagesAccount).Text == "Summary By Contractor and Document Type")
				{
					btnPrint.Text = "161-WagesbyContractor&DocumentType";
				}
				if (((Control)(object)cmbWagesAccount).Text == "Summary By Contractor")
				{
					btnPrint.Text = "162-WagesByContractor";
				}
				if (((Control)(object)cmbWagesAccount).Text == "Summary By Document Type")
				{
					btnPrint.Text = "163-WagesByDocumentType";
				}
			}
			else
			{
				btnPrint.Visible = false;
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
			ReferenceDocumentType();
			WagesAccountFill();
			WagesContractorFill();
			txtDateFrom.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmWagesDashboardReport_FormClosing(object sender, FormClosingEventArgs e)
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

	private void ctrlGrdDetaSummByDontractor_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetailSummaryByContractor).Name);
			if (fs != null)
			{
				grdDetailSummaryByContractor.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdDetailByContractor_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdiDetailByContractory).Name);
			if (fs != null)
			{
				GrdiDetailByContractory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdActivityDocumentType_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdActivityaAndDocumentType).Name);
			if (fs != null)
			{
				grdActivityaAndDocumentType.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdDocumentTypeActivity_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDocumentTypeAndActivity).Name);
			if (fs != null)
			{
				grdDocumentTypeAndActivity.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
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
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_12ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b6: Expected O, but got Unknown
		//IL_1639: Unknown result type (might be due to invalid IL or missing references)
		//IL_1643: Expected O, but got Unknown
		//IL_1a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a98: Expected O, but got Unknown
		//IL_1d32: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3c: Expected O, but got Unknown
		//IL_1fc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd0: Expected O, but got Unknown
		GridEXLayout grdActivityaAndDocumentType_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDocumentTypeAndActivity_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdiDetailByContractory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetailSummaryByContractor_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummarybyContractor_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummarybyWagesAc_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummaryByDocType_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Inventory_Reports.frmWagesDashboardReport));
		this.PanelDetail = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel20 = new System.Windows.Forms.Panel();
		this.panel21 = new System.Windows.Forms.Panel();
		this.panel23 = new System.Windows.Forms.Panel();
		this.grdActivityaAndDocumentType = new GridEX();
		this.panel25 = new System.Windows.Forms.Panel();
		this.ctrlGrdActivityDocumentType = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.tabPage4 = new System.Windows.Forms.TabPage();
		this.panel27 = new System.Windows.Forms.Panel();
		this.panel28 = new System.Windows.Forms.Panel();
		this.panel29 = new System.Windows.Forms.Panel();
		this.grdDocumentTypeAndActivity = new GridEX();
		this.panel30 = new System.Windows.Forms.Panel();
		this.ctrlGrdDocumentTypeActivity = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.panel19 = new System.Windows.Forms.Panel();
		this.GrdiDetailByContractory = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdDetailByContractor = new grid.User_Conrtols.CtrlGrdBar();
		this.label9 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel18 = new System.Windows.Forms.Panel();
		this.grdDetailSummaryByContractor = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdDetaSummByDontractor = new grid.User_Conrtols.CtrlGrdBar();
		this.label11 = new System.Windows.Forms.Label();
		this.panel22 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.grdSummarybyContractor = new GridEX();
		this.label5 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel13 = new System.Windows.Forms.Panel();
		this.grdSummarybyWagesAc = new GridEX();
		this.label4 = new System.Windows.Forms.Label();
		this.panel24 = new System.Windows.Forms.Panel();
		this.panel26 = new System.Windows.Forms.Panel();
		this.grdSummaryByDocType = new GridEX();
		this.label13 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label1 = new System.Windows.Forms.Label();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbContractor = new UltraCombo();
		this.cmbWagesAccount = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.label10 = new System.Windows.Forms.Label();
		this.cmbDocumentType = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.PanelDetail.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel20.SuspendLayout();
		this.panel21.SuspendLayout();
		this.panel23.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdActivityaAndDocumentType).BeginInit();
		this.panel25.SuspendLayout();
		this.tabPage4.SuspendLayout();
		this.panel27.SuspendLayout();
		this.panel28.SuspendLayout();
		this.panel29.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDocumentTypeAndActivity).BeginInit();
		this.panel30.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel19.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdiDetailByContractory).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel18.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetailSummaryByContractor).BeginInit();
		this.panel17.SuspendLayout();
		this.panel22.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel8.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummarybyContractor).BeginInit();
		this.panel6.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummarybyWagesAc).BeginInit();
		this.panel24.SuspendLayout();
		this.panel26.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummaryByDocType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWagesAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentType).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.PanelDetail.AutoScroll = true;
		this.PanelDetail.Controls.Add(this.tabControl1);
		this.PanelDetail.Controls.Add(this.panel22);
		this.PanelDetail.Controls.Add(this.panel3);
		this.PanelDetail.Controls.Add(this.panel2);
		this.PanelDetail.Dock = System.Windows.Forms.DockStyle.Fill;
		this.PanelDetail.Location = new System.Drawing.Point(0, 0);
		this.PanelDetail.Name = "PanelDetail";
		this.PanelDetail.Size = new System.Drawing.Size(1524, 720);
		this.PanelDetail.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage3);
		this.tabControl1.Controls.Add(this.tabPage4);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Tahoma", 8.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 354);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1524, 366);
		this.tabControl1.TabIndex = 170;
		this.tabPage3.Controls.Add(this.panel20);
		this.tabPage3.Location = new System.Drawing.Point(4, 22);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Size = new System.Drawing.Size(1516, 340);
		this.tabPage3.TabIndex = 2;
		this.tabPage3.Text = "Activity && Document Type";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel20.Controls.Add(this.panel21);
		this.panel20.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel20.Location = new System.Drawing.Point(0, 0);
		this.panel20.Name = "panel20";
		this.panel20.Size = new System.Drawing.Size(1516, 340);
		this.panel20.TabIndex = 0;
		this.panel21.Controls.Add(this.panel23);
		this.panel21.Controls.Add(this.panel25);
		this.panel21.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel21.Location = new System.Drawing.Point(0, 0);
		this.panel21.Name = "panel21";
		this.panel21.Size = new System.Drawing.Size(1217, 340);
		this.panel21.TabIndex = 4;
		this.panel23.Controls.Add((System.Windows.Forms.Control)(object)this.grdActivityaAndDocumentType);
		this.panel23.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel23.Location = new System.Drawing.Point(0, 28);
		this.panel23.Name = "panel23";
		this.panel23.Size = new System.Drawing.Size(1217, 312);
		this.panel23.TabIndex = 1;
		this.grdActivityaAndDocumentType.AllowEdit = (InheritableBoolean)2;
		grdActivityaAndDocumentType_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdActivityaAndDocumentType.DesignTimeLayout = grdActivityaAndDocumentType_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdActivityaAndDocumentType).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdActivityaAndDocumentType.DynamicFiltering = true;
		this.grdActivityaAndDocumentType.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdActivityaAndDocumentType).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdActivityaAndDocumentType.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdActivityaAndDocumentType).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdActivityaAndDocumentType).Name = "grdActivityaAndDocumentType";
		((System.Windows.Forms.Control)(object)this.grdActivityaAndDocumentType).Size = new System.Drawing.Size(1217, 312);
		((System.Windows.Forms.Control)(object)this.grdActivityaAndDocumentType).TabIndex = 0;
		this.grdActivityaAndDocumentType.TotalRow = (InheritableBoolean)1;
		this.grdActivityaAndDocumentType.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdActivityaAndDocumentType).VisualStyle = (VisualStyle)4;
		this.panel25.BackColor = System.Drawing.Color.Teal;
		this.panel25.Controls.Add(this.ctrlGrdActivityDocumentType);
		this.panel25.Controls.Add(this.label6);
		this.panel25.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel25.Location = new System.Drawing.Point(0, 0);
		this.panel25.Name = "panel25";
		this.panel25.Size = new System.Drawing.Size(1217, 28);
		this.panel25.TabIndex = 0;
		this.ctrlGrdActivityDocumentType.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdActivityDocumentType.FormName = this;
		this.ctrlGrdActivityDocumentType.Location = new System.Drawing.Point(1173, 2);
		this.ctrlGrdActivityDocumentType.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdActivityDocumentType.MyGrid = this.grdActivityaAndDocumentType;
		this.ctrlGrdActivityDocumentType.Name = "ctrlGrdActivityDocumentType";
		this.ctrlGrdActivityDocumentType.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdActivityDocumentType.TabIndex = 1;
		this.ctrlGrdActivityDocumentType.Load += new System.EventHandler(ctrlGrdActivityDocumentType_Load);
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(3, 5);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(177, 16);
		this.label6.TabIndex = 0;
		this.label6.Text = "Activity && Document Type";
		this.tabPage4.Controls.Add(this.panel27);
		this.tabPage4.Location = new System.Drawing.Point(4, 22);
		this.tabPage4.Name = "tabPage4";
		this.tabPage4.Size = new System.Drawing.Size(1516, 340);
		this.tabPage4.TabIndex = 3;
		this.tabPage4.Text = "Document Type && Activity";
		this.tabPage4.UseVisualStyleBackColor = true;
		this.panel27.Controls.Add(this.panel28);
		this.panel27.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel27.Location = new System.Drawing.Point(0, 0);
		this.panel27.Name = "panel27";
		this.panel27.Size = new System.Drawing.Size(1516, 340);
		this.panel27.TabIndex = 0;
		this.panel28.Controls.Add(this.panel29);
		this.panel28.Controls.Add(this.panel30);
		this.panel28.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel28.Location = new System.Drawing.Point(0, 0);
		this.panel28.Name = "panel28";
		this.panel28.Size = new System.Drawing.Size(1218, 340);
		this.panel28.TabIndex = 5;
		this.panel29.Controls.Add((System.Windows.Forms.Control)(object)this.grdDocumentTypeAndActivity);
		this.panel29.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel29.Location = new System.Drawing.Point(0, 28);
		this.panel29.Name = "panel29";
		this.panel29.Size = new System.Drawing.Size(1218, 312);
		this.panel29.TabIndex = 1;
		this.grdDocumentTypeAndActivity.AllowEdit = (InheritableBoolean)2;
		grdDocumentTypeAndActivity_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDocumentTypeAndActivity.DesignTimeLayout = grdDocumentTypeAndActivity_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDocumentTypeAndActivity).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDocumentTypeAndActivity.DynamicFiltering = true;
		this.grdDocumentTypeAndActivity.FilterMode = (FilterMode)2;
		this.grdDocumentTypeAndActivity.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDocumentTypeAndActivity).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDocumentTypeAndActivity.GroupTotals = (GroupTotals)2;
		this.grdDocumentTypeAndActivity.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grdDocumentTypeAndActivity).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDocumentTypeAndActivity).Name = "grdDocumentTypeAndActivity";
		((ScrollableBase)this.grdDocumentTypeAndActivity).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDocumentTypeAndActivity).Size = new System.Drawing.Size(1218, 312);
		((System.Windows.Forms.Control)(object)this.grdDocumentTypeAndActivity).TabIndex = 5;
		this.grdDocumentTypeAndActivity.TotalRow = (InheritableBoolean)1;
		this.grdDocumentTypeAndActivity.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDocumentTypeAndActivity).VisualStyle = (VisualStyle)4;
		this.panel30.BackColor = System.Drawing.Color.Teal;
		this.panel30.Controls.Add(this.ctrlGrdDocumentTypeActivity);
		this.panel30.Controls.Add(this.label14);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 0);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1218, 28);
		this.panel30.TabIndex = 0;
		this.ctrlGrdDocumentTypeActivity.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdDocumentTypeActivity.FormName = this;
		this.ctrlGrdDocumentTypeActivity.Location = new System.Drawing.Point(1175, 2);
		this.ctrlGrdDocumentTypeActivity.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdDocumentTypeActivity.MyGrid = this.grdDocumentTypeAndActivity;
		this.ctrlGrdDocumentTypeActivity.Name = "ctrlGrdDocumentTypeActivity";
		this.ctrlGrdDocumentTypeActivity.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdDocumentTypeActivity.TabIndex = 0;
		this.ctrlGrdDocumentTypeActivity.Load += new System.EventHandler(ctrlGrdDocumentTypeActivity_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(3, 5);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(177, 16);
		this.label14.TabIndex = 1;
		this.label14.Text = "Documnet Type && Activity";
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel4);
		this.tabPage2.Location = new System.Drawing.Point(4, 22);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1516, 340);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Contractor";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add(this.panel19);
		this.panel10.Controls.Add(this.panel11);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel10.Location = new System.Drawing.Point(700, 3);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(690, 334);
		this.panel10.TabIndex = 10;
		this.panel19.Controls.Add((System.Windows.Forms.Control)(object)this.GrdiDetailByContractory);
		this.panel19.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel19.Location = new System.Drawing.Point(0, 28);
		this.panel19.Name = "panel19";
		this.panel19.Size = new System.Drawing.Size(690, 306);
		this.panel19.TabIndex = 1;
		this.GrdiDetailByContractory.AllowEdit = (InheritableBoolean)2;
		this.GrdiDetailByContractory.ColumnAutoResize = true;
		GrdiDetailByContractory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdiDetailByContractory.DesignTimeLayout = GrdiDetailByContractory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdiDetailByContractory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdiDetailByContractory.DynamicFiltering = true;
		this.GrdiDetailByContractory.FilterMode = (FilterMode)2;
		this.GrdiDetailByContractory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdiDetailByContractory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdiDetailByContractory.GroupTotals = (GroupTotals)2;
		this.GrdiDetailByContractory.HideColumnsWhenGrouped = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.GrdiDetailByContractory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdiDetailByContractory).Name = "GrdiDetailByContractory";
		((ScrollableBase)this.GrdiDetailByContractory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdiDetailByContractory).Size = new System.Drawing.Size(690, 306);
		((System.Windows.Forms.Control)(object)this.GrdiDetailByContractory).TabIndex = 4;
		this.GrdiDetailByContractory.TotalRow = (InheritableBoolean)1;
		this.GrdiDetailByContractory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdiDetailByContractory).VisualStyle = (VisualStyle)4;
		this.GrdiDetailByContractory.LinkClicked += new ColumnActionEventHandler(GrdiDetailByContractory_LinkClicked);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdDetailByContractor);
		this.panel11.Controls.Add(this.label9);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(690, 28);
		this.panel11.TabIndex = 0;
		this.ctrlGrdDetailByContractor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdDetailByContractor.FormName = this;
		this.ctrlGrdDetailByContractor.Location = new System.Drawing.Point(647, 2);
		this.ctrlGrdDetailByContractor.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdDetailByContractor.MyGrid = this.GrdiDetailByContractory;
		this.ctrlGrdDetailByContractor.Name = "ctrlGrdDetailByContractor";
		this.ctrlGrdDetailByContractor.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdDetailByContractor.TabIndex = 0;
		this.ctrlGrdDetailByContractor.Load += new System.EventHandler(ctrlGrdDetailByContractor_Load);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(5, 6);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(139, 16);
		this.label9.TabIndex = 1;
		this.label9.Text = "Detail By Contractor";
		this.panel4.Controls.Add(this.panel18);
		this.panel4.Controls.Add(this.panel17);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(697, 334);
		this.panel4.TabIndex = 9;
		this.panel18.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetailSummaryByContractor);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel18.Location = new System.Drawing.Point(0, 28);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(697, 306);
		this.panel18.TabIndex = 1;
		this.grdDetailSummaryByContractor.AllowEdit = (InheritableBoolean)2;
		grdDetailSummaryByContractor_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetailSummaryByContractor.DesignTimeLayout = grdDetailSummaryByContractor_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetailSummaryByContractor).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetailSummaryByContractor.DynamicFiltering = true;
		this.grdDetailSummaryByContractor.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetailSummaryByContractor).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetailSummaryByContractor.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetailSummaryByContractor).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetailSummaryByContractor).Name = "grdDetailSummaryByContractor";
		((System.Windows.Forms.Control)(object)this.grdDetailSummaryByContractor).Size = new System.Drawing.Size(697, 306);
		((System.Windows.Forms.Control)(object)this.grdDetailSummaryByContractor).TabIndex = 0;
		this.grdDetailSummaryByContractor.TotalRow = (InheritableBoolean)1;
		this.grdDetailSummaryByContractor.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetailSummaryByContractor).VisualStyle = (VisualStyle)4;
		this.grdDetailSummaryByContractor.LinkClicked += new ColumnActionEventHandler(grdDetailSummaryByContractor_LinkClicked);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdDetaSummByDontractor);
		this.panel17.Controls.Add(this.label11);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(697, 28);
		this.panel17.TabIndex = 0;
		this.ctrlGrdDetaSummByDontractor.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdDetaSummByDontractor.FormName = this;
		this.ctrlGrdDetaSummByDontractor.Location = new System.Drawing.Point(654, 2);
		this.ctrlGrdDetaSummByDontractor.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdDetaSummByDontractor.MyGrid = this.grdDetailSummaryByContractor;
		this.ctrlGrdDetaSummByDontractor.Name = "ctrlGrdDetaSummByDontractor";
		this.ctrlGrdDetaSummByDontractor.Size = new System.Drawing.Size(41, 24);
		this.ctrlGrdDetaSummByDontractor.TabIndex = 0;
		this.ctrlGrdDetaSummByDontractor.Load += new System.EventHandler(ctrlGrdDetaSummByDontractor_Load);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(3, 7);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(203, 16);
		this.label11.TabIndex = 1;
		this.label11.Text = "Detail Summary By Contractor";
		this.panel22.BackColor = System.Drawing.Color.White;
		this.panel22.Controls.Add(this.panel7);
		this.panel22.Controls.Add(this.panel6);
		this.panel22.Controls.Add(this.panel24);
		this.panel22.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel22.Location = new System.Drawing.Point(0, 96);
		this.panel22.Name = "panel22";
		this.panel22.Size = new System.Drawing.Size(1524, 258);
		this.panel22.TabIndex = 169;
		this.panel7.Controls.Add(this.panel8);
		this.panel7.Controls.Add(this.label5);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel7.Location = new System.Drawing.Point(1220, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(303, 258);
		this.panel7.TabIndex = 2;
		this.panel8.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummarybyContractor);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 23);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(303, 235);
		this.panel8.TabIndex = 3;
		this.grdSummarybyContractor.AllowEdit = (InheritableBoolean)2;
		this.grdSummarybyContractor.ColumnAutoResize = true;
		grdSummarybyContractor_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummarybyContractor.DesignTimeLayout = grdSummarybyContractor_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummarybyContractor).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummarybyContractor.DynamicFiltering = true;
		this.grdSummarybyContractor.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummarybyContractor).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummarybyContractor.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummarybyContractor).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummarybyContractor).Name = "grdSummarybyContractor";
		((System.Windows.Forms.Control)(object)this.grdSummarybyContractor).Size = new System.Drawing.Size(303, 235);
		((System.Windows.Forms.Control)(object)this.grdSummarybyContractor).TabIndex = 1;
		this.grdSummarybyContractor.TotalRow = (InheritableBoolean)1;
		this.grdSummarybyContractor.TotalRowPosition = (TotalRowPosition)1;
		((System.Windows.Forms.Control)(object)this.grdSummarybyContractor).Visible = false;
		((ScrollableBase)this.grdSummarybyContractor).VisualStyle = (VisualStyle)4;
		this.grdSummarybyContractor.LinkClicked += new ColumnActionEventHandler(grdSummarybyContractor_LinkClicked);
		this.label5.BackColor = System.Drawing.Color.Teal;
		this.label5.Dock = System.Windows.Forms.DockStyle.Top;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(0, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(303, 23);
		this.label5.TabIndex = 2;
		this.label5.Text = "Summary By Contractor";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.label5.Visible = false;
		this.panel6.Controls.Add(this.panel13);
		this.panel6.Controls.Add(this.label4);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(610, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(610, 258);
		this.panel6.TabIndex = 1;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummarybyWagesAc);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(0, 23);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(610, 235);
		this.panel13.TabIndex = 2;
		this.grdSummarybyWagesAc.AllowEdit = (InheritableBoolean)2;
		this.grdSummarybyWagesAc.ColumnAutoResize = true;
		grdSummarybyWagesAc_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummarybyWagesAc.DesignTimeLayout = grdSummarybyWagesAc_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummarybyWagesAc).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummarybyWagesAc.DynamicFiltering = true;
		this.grdSummarybyWagesAc.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummarybyWagesAc).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummarybyWagesAc.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummarybyWagesAc).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummarybyWagesAc).Name = "grdSummarybyWagesAc";
		((System.Windows.Forms.Control)(object)this.grdSummarybyWagesAc).Size = new System.Drawing.Size(610, 235);
		((System.Windows.Forms.Control)(object)this.grdSummarybyWagesAc).TabIndex = 1;
		this.grdSummarybyWagesAc.TotalRow = (InheritableBoolean)1;
		this.grdSummarybyWagesAc.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummarybyWagesAc).VisualStyle = (VisualStyle)4;
		this.grdSummarybyWagesAc.LinkClicked += new ColumnActionEventHandler(grdSummarybyWagesAc_LinkClicked);
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Dock = System.Windows.Forms.DockStyle.Top;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(0, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(610, 23);
		this.label4.TabIndex = 1;
		this.label4.Text = "Summary By Wages Account";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel24.Controls.Add(this.panel26);
		this.panel24.Controls.Add(this.label13);
		this.panel24.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel24.Location = new System.Drawing.Point(0, 0);
		this.panel24.Name = "panel24";
		this.panel24.Size = new System.Drawing.Size(610, 258);
		this.panel24.TabIndex = 0;
		this.panel26.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummaryByDocType);
		this.panel26.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel26.Location = new System.Drawing.Point(0, 23);
		this.panel26.Name = "panel26";
		this.panel26.Size = new System.Drawing.Size(610, 235);
		this.panel26.TabIndex = 1;
		this.grdSummaryByDocType.AllowEdit = (InheritableBoolean)2;
		this.grdSummaryByDocType.ColumnAutoResize = true;
		grdSummaryByDocType_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummaryByDocType.DesignTimeLayout = grdSummaryByDocType_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummaryByDocType).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummaryByDocType.DynamicFiltering = true;
		this.grdSummaryByDocType.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdSummaryByDocType).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummaryByDocType.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummaryByDocType).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdSummaryByDocType).Name = "grdSummaryByDocType";
		((System.Windows.Forms.Control)(object)this.grdSummaryByDocType).Size = new System.Drawing.Size(610, 235);
		((System.Windows.Forms.Control)(object)this.grdSummaryByDocType).TabIndex = 1;
		this.grdSummaryByDocType.TotalRow = (InheritableBoolean)1;
		this.grdSummaryByDocType.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummaryByDocType).VisualStyle = (VisualStyle)4;
		this.grdSummaryByDocType.LinkClicked += new ColumnActionEventHandler(grdSummaryByDocType_LinkClicked);
		this.label13.BackColor = System.Drawing.Color.Teal;
		this.label13.Dock = System.Windows.Forms.DockStyle.Top;
		this.label13.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.White;
		this.label13.Location = new System.Drawing.Point(0, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(610, 23);
		this.label13.TabIndex = 0;
		this.label13.Text = "Summary By Document Type";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel3.AutoScroll = true;
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1524, 67);
		this.panel3.TabIndex = 1;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1524, 67);
		this.panel5.TabIndex = 0;
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnShow);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContractor);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWagesAccount);
		this.groupBox1.Controls.Add(this.label12);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add(this.txtToDate);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocumentType);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(0, 0);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(989, 67);
		this.groupBox1.TabIndex = 166;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(6, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 1;
		this.label1.Text = "From Date";
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnShow.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(921, 34);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(57, 26);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbContractor.AllowNull = (DefaultableBoolean)1;
		this.cmbContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContractor).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContractor).Location = new System.Drawing.Point(466, 35);
		((System.Windows.Forms.Control)(object)this.cmbContractor).Name = "cmbContractor";
		((System.Windows.Forms.Control)(object)this.cmbContractor).Size = new System.Drawing.Size(244, 26);
		((System.Windows.Forms.Control)(object)this.cmbContractor).TabIndex = 3;
		this.cmbWagesAccount.AllowNull = (DefaultableBoolean)1;
		this.cmbWagesAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWagesAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWagesAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWagesAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbWagesAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWagesAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWagesAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWagesAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWagesAccount).Location = new System.Drawing.Point(712, 35);
		((System.Windows.Forms.Control)(object)this.cmbWagesAccount).Name = "cmbWagesAccount";
		((System.Windows.Forms.Control)(object)this.cmbWagesAccount).Size = new System.Drawing.Size(207, 26);
		((System.Windows.Forms.Control)(object)this.cmbWagesAccount).TabIndex = 4;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(712, 17);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(128, 15);
		this.label12.TabIndex = 163;
		this.label12.Text = "Labour/Wages Activity";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtDateFrom.Location = new System.Drawing.Point(7, 37);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(113, 23);
		this.txtDateFrom.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(466, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 15);
		this.label3.TabIndex = 165;
		this.label3.Text = "Contractor";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(124, 19);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 3;
		this.label2.Text = "To Date";
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(122, 37);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(113, 23);
		this.txtToDate.TabIndex = 1;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(236, 17);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(92, 15);
		this.label10.TabIndex = 159;
		this.label10.Text = "Document Type";
		this.cmbDocumentType.AllowNull = (DefaultableBoolean)1;
		this.cmbDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance22;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Location = new System.Drawing.Point(236, 35);
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Name = "cmbDocumentType";
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Size = new System.Drawing.Size(228, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).TabIndex = 2;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1524, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnNew, this.btnPrint });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1524, 27);
		this.toolStrip3.TabIndex = 0;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(80, 24);
		this.btnNew.Text = "&Refresh";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(61, 24);
		this.btnPrint.Text = "&Print";
		this.btnPrint.Visible = false;
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1524, 720);
		base.Controls.Add(this.PanelDetail);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmWagesDashboardReport";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmWagesDashboardReport_FormClosing);
		base.Load += new System.EventHandler(frmEvaulationDetailSalesReports_Load);
		this.PanelDetail.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		this.panel20.ResumeLayout(false);
		this.panel21.ResumeLayout(false);
		this.panel23.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdActivityaAndDocumentType).EndInit();
		this.panel25.ResumeLayout(false);
		this.panel25.PerformLayout();
		this.tabPage4.ResumeLayout(false);
		this.panel27.ResumeLayout(false);
		this.panel28.ResumeLayout(false);
		this.panel29.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDocumentTypeAndActivity).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		this.panel19.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdiDetailByContractory).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel18.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetailSummaryByContractor).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.panel22.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummarybyContractor).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummarybyWagesAc).EndInit();
		this.panel24.ResumeLayout(false);
		this.panel26.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummaryByDocType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbWagesAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
