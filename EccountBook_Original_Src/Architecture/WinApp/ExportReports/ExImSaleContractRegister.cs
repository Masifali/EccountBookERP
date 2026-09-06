using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Architecture.BLL.Export;
using Architecture.BLL.Reports.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Common;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ExportReports;

public class ExImSaleContractRegister : Form
{
	private bool ShiftTabPressed;

	private DropDownBind DDL = new DropDownBind();

	public UserAccount UserAccount = new UserAccount();

	private DataTable Data = new DataTable();

	private DataTable DataReport = new DataTable();

	private DataTable DataReportProductWise = new DataTable();

	private DataTable dtdetail = new DataTable();

	private bool FormInitialized;

	private DataTable dtHistory = new DataTable();

	private DataTable dtProductWise = new DataTable();

	private DataTable dtcard = new DataTable();

	private IContainer components;

	private Panel panel1;

	private ToolStrip tsItemType;

	private ToolStripButton btnnew;

	private ToolStripButton btnregisterprint526;

	internal UltraCombo cmbcontractNo;

	private Label label2;

	internal UltraCombo cmbSupplierName;

	private Label label50;

	private Button btnShow;

	private Label label4;

	private Label label5;

	private Label label1;

	private Label label51;

	private ToolStripButton toolStripButton2;

	private ToolStripButton btnregister523;

	internal UltraCombo cmbperemeter;

	private Label label11;

	private ToolStripButton btnShortCutKey;

	private ToolStripButton btnRefresh;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private Panel panel7;

	private Panel panel6;

	private CtrlGrdBar ctrlGrdBar1;

	private TabControl tabControl2;

	private TabPage tabPage2;

	private TabPage tabPage3;

	internal UltraCombo cmbItemName;

	private Label label6;

	private ToolStripButton btnregister561;

	private RadioButton ChkProductWise;

	private RadioButton ChkSummary;

	private Panel panel8;

	private CtrlGrdBar ctrlGrdBar2;

	private Panel panel9;

	private Panel panel10;

	private Panel panel11;

	private Panel panel5;

	private Label label3;

	internal UltraCombo Cmbtradtype;

	internal UltraCombo CmbFarmingType;

	private Label label8;

	private Label label7;

	internal GridEX grdContractRegister;

	internal GridEX grdProductWise;

	private RadioButton rdSelectedCriteria;

	private RadioButton rdExportYear;

	private RadioButton rdFinancialYear;

	private Panel panel2;

	private Label label9;

	internal UltraCombo CmbStatus;

	private RadioButton rdCancel;

	private CheckBox chkSkipZero;

	private RadioButton rdComplete;

	private RadioButton rdOpen;

	private GroupBox groupBox1;

	public DateTimePicker datDateTo;

	public DateTimePicker datDateFrom;

	private Panel panel3;

	internal GridEX grdCard;

	private Label label28;

	internal UltraCombo CmbSalePerson;

	private Label label10;

	private Label label12;

	private RadioButton rdAll;

	public ExImSaleContractRegister(UserAccount pUserAccount)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
	}

	private void frmExportShipingLineBookingRpt_Load(object sender, EventArgs e)
	{
		try
		{
			ctrlGrdBar1.mGridChooseFielder.Visible = false;
			ctrlGrdBar1.mGridSaveLayouts.Visible = false;
			ctrlGrdBar1.removeLayoutToolStripMenuItem.Visible = false;
			ctrlGrdBar2.mGridChooseFielder.Visible = false;
			ctrlGrdBar2.mGridSaveLayouts.Visible = false;
			ctrlGrdBar2.removeLayoutToolStripMenuItem.Visible = false;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			List<tblUserRights> screenViewReights = clsGlobalVariables.ScreenViewReights;
			FormInitialized = true;
			if (screenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmContractCurrentStatusReport").ToList().Count > 0)
			{
				tabControl1.TabPages.Add("Contracts Current Status");
				frmContractCurrentStatusReport frm3 = new frmContractCurrentStatusReport(UserAccount)
				{
					TopLevel = false,
					Visible = true,
					FormBorderStyle = FormBorderStyle.None,
					Dock = DockStyle.Fill
				};
				tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(frm3);
			}
			if (screenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmExportContractSchedulePeriodicB").ToList().Count > 0)
			{
				tabControl1.TabPages.Add("Contracts Schedule");
				frmExportContractSchedulePeriodicB frm2 = new frmExportContractSchedulePeriodicB(UserAccount)
				{
					TopLevel = false,
					Visible = true,
					FormBorderStyle = FormBorderStyle.None,
					Dock = DockStyle.Fill
				};
				tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(frm2);
			}
			if (screenViewReights.Where((tblUserRights x) => x.ScreenName.Trim() == "frmDepartmentStatusReport").ToList().Count > 0)
			{
				tabControl1.TabPages.Add("Pre Shipment Planning");
				frmDepartmentStatusReport frm = new frmDepartmentStatusReport(UserAccount);
				frm.TopLevel = false;
				frm.Visible = true;
				frm.FormBorderStyle = FormBorderStyle.None;
				frm.Dock = DockStyle.Fill;
				tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(frm);
			}
			ParameterFill();
			ComboFill();
			OrderStatusBind();
			GetDataForExportInfo();
			GridBind();
			GridBindProductWise();
			ReportsShowHide();
			datDateTo.Checked = false;
			tabControl1.SelectedIndex = 0;
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

	private void OrderStatusBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("OrderStatus");
			dt.Rows.Add(1, "Open");
			dt.Rows.Add(2, "In-Process");
			dt.Rows.Add(3, "Complete");
			DDL.BindDDL(dt, CmbStatus, "Id", "OrderStatus", "OrderStatus", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		try
		{
			DataTable dtContractNo = new DataTable();
			dtContractNo.Columns.Add("Id");
			dtContractNo.Columns.Add("name");
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id");
			dtCustomer.Columns.Add("name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id");
			dtItem.Columns.Add("name");
			DataTable dtFarm = new DataTable();
			dtFarm.Columns.Add("Id");
			dtFarm.Columns.Add("name");
			DataTable dtTrade = new DataTable();
			dtTrade.Columns.Add("Id");
			dtTrade.Columns.Add("name");
			DataTable dtSalePerson = new DataTable();
			dtSalePerson.Columns.Add("Id");
			dtSalePerson.Columns.Add("name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			Data = ExImLcOrder.GetDataForDropDownFromExportContract(obj);
			if (Data.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < Data.Rows.Count; i++)
			{
				if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "ContractNo")
				{
					dtContractNo.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
				else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Customer")
				{
					dtCustomer.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
				else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "Items")
				{
					dtItem.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
				else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "TradeType")
				{
					dtTrade.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
				else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "FarmingType")
				{
					dtFarm.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
				else if (Conversion.ToString(Data.Rows[i]["ActivityType"]) == "SalePerson")
				{
					dtSalePerson.Rows.Add(Data.Rows[i]["Id"], Data.Rows[i]["name"]);
				}
			}
			if (dtContractNo.Rows.Count > 0)
			{
				DDL.BindDDL(dtContractNo, cmbcontractNo, "Id", "name", "Contract No", ZeroIndex: false);
				((UltraGridBase)cmbcontractNo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, cmbSupplierName, "Id", "name", "Customer Name", ZeroIndex: false);
				((UltraGridBase)cmbSupplierName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItemName, "Id", "name", "Item Name", ZeroIndex: false);
				((UltraGridBase)cmbItemName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtTrade.Rows.Count > 0)
			{
				DDL.BindDDL(dtTrade, Cmbtradtype, "Id", "name", "Trade Type", ZeroIndex: false);
				((UltraGridBase)Cmbtradtype).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtFarm.Rows.Count > 0)
			{
				DDL.BindDDL(dtFarm, CmbFarmingType, "Id", "name", "Trade Type", ZeroIndex: false);
				((UltraGridBase)CmbFarmingType).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtSalePerson.Rows.Count > 0)
			{
				DDL.BindDDL(dtSalePerson, CmbSalePerson, "Id", "name", "Sale Person", ZeroIndex: false);
			}
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
			GetDataForExportInfo();
			if (ChkSummary.Checked)
			{
				tabControl2.SelectedIndex = 0;
				grdProductWise.DataSource = null;
				DataReportProductWise.Clear();
				GridBind();
			}
			else
			{
				tabControl2.SelectedIndex = 1;
				grdContractRegister.DataSource = null;
				DataReport.Clear();
				GridBindProductWise();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		try
		{
			DataReport.Rows.Clear();
			dtHistory.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ExImLcOrderId = Conversion.ToInt(cmbcontractNo.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value);
			obj.ItemId = Conversion.ToInt(cmbItemName.Value);
			obj.TradeTypeId = Conversion.ToInt(Cmbtradtype.Value);
			obj.FarmingTypeId = Conversion.ToInt(CmbFarmingType.Value);
			if (datDateFrom.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datDateFrom.Value);
			}
			if (datDateTo.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datDateTo.Value);
			}
			obj.SkipZero = (chkSkipZero.Checked ? 1 : 0);
			if (rdOpen.Checked)
			{
				obj.Status = "Open";
			}
			else if (rdComplete.Checked)
			{
				obj.Status = "Complete";
			}
			else if (rdCancel.Checked)
			{
				obj.Status = "Cancel";
			}
			obj.RefSaleManId = Conversion.ToInt(CmbSalePerson.Value);
			DataReport = ExImLcOrder.EximLcOrder_Register(obj);
			if (dtHistory.Columns.Count == 0)
			{
				dtHistory.Columns.Add("Id", typeof(int));
				dtHistory.Columns.Add("DocNo", typeof(int));
				dtHistory.Columns.Add("DocDate", typeof(DateTime));
				dtHistory.Columns.Add("ContractNo");
				dtHistory.Columns.Add("FarmingTrade");
				dtHistory.Columns.Add("CustomerName");
				dtHistory.Columns.Add("DeliveryTerms");
				dtHistory.Columns.Add("PaymentTerms");
				dtHistory.Columns.Add("Fcy");
				dtHistory.Columns.Add("FcyAmount", typeof(double));
				dtHistory.Columns.Add("FcyShipped", typeof(double));
				dtHistory.Columns.Add("FcyBalance", typeof(double));
				dtHistory.Columns.Add("NetWeight", typeof(double));
				dtHistory.Columns.Add("ShippedWeight", typeof(double));
				dtHistory.Columns.Add("BalanceWeight", typeof(double));
				dtHistory.Columns.Add("Fcl", typeof(int));
				dtHistory.Columns.Add("ShippedFcl", typeof(int));
				dtHistory.Columns.Add("BalanceFcl", typeof(int));
				dtHistory.Columns.Add("DestinationPort");
				dtHistory.Columns.Add("Status");
				dtHistory.Columns.Add("ActionRemarks", typeof(string));
				dtHistory.Columns.Add("NoOfAttachments", typeof(int));
			}
			if (DataReport.Rows.Count > 0)
			{
				for (int i = 0; i < DataReport.Rows.Count; i++)
				{
					dtHistory.Rows.Add(DataReport.Rows[i]["Id"], DataReport.Rows[i]["LcOrderDocNo"], DataReport.Rows[i]["ContractDate"], DataReport.Rows[i]["ContractNo"], DataReport.Rows[i]["FarmingTrade"], DataReport.Rows[i]["CustomerName"], DataReport.Rows[i]["DeliveryTerms"], DataReport.Rows[i]["PaymentTerms"], DataReport.Rows[i]["Fcy"], DataReport.Rows[i]["FcyAmount"], DataReport.Rows[i]["FcyShipped"], DataReport.Rows[i]["FcyBalance"], DataReport.Rows[i]["NetWeight"], DataReport.Rows[i]["ShippedWeight"], DataReport.Rows[i]["BalanceWeight"], DataReport.Rows[i]["NoOfContainersHeader"], DataReport.Rows[i]["ShippedNoOfContainersHeader"], DataReport.Rows[i]["BalNoofContainerHeader"], DataReport.Rows[i]["DestinationPort"], DataReport.Rows[i]["LcStatus"], "", DataReport.Rows[i]["NoOfAttachments"]);
				}
				grdContractRegister.DataSource = dtHistory;
				GridSetting();
			}
			else
			{
				grdContractRegister.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBindProductWise()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		try
		{
			DataReportProductWise.Clear();
			dtProductWise.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.ExImLcOrderId = Conversion.ToInt(cmbcontractNo.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value);
			obj.ItemId = Conversion.ToInt(cmbItemName.Value);
			obj.TradeTypeId = Conversion.ToInt(Cmbtradtype.Value);
			obj.FarmingTypeId = Conversion.ToInt(CmbFarmingType.Value);
			if (datDateFrom.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datDateFrom.Value);
			}
			if (datDateTo.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datDateTo.Value);
			}
			obj.SkipZero = (chkSkipZero.Checked ? 1 : 0);
			if (rdOpen.Checked)
			{
				obj.Status = "Open";
			}
			else if (rdComplete.Checked)
			{
				obj.Status = "Complete";
			}
			else if (rdCancel.Checked)
			{
				obj.Status = "Cancel";
			}
			obj.RefSaleManId = Conversion.ToInt(CmbSalePerson.Value);
			DataReportProductWise = ExImLcOrder.ExImLcOrder_ExportRegisterItemWise(obj);
			if (dtProductWise.Columns.Count == 0)
			{
				dtProductWise.Columns.Add("Id", typeof(int));
				dtProductWise.Columns.Add("DocNo", typeof(int));
				dtProductWise.Columns.Add("DocDate", typeof(DateTime));
				dtProductWise.Columns.Add("ContractNo");
				dtProductWise.Columns.Add("CustomerName");
				dtProductWise.Columns.Add("DeliveryTerms");
				dtProductWise.Columns.Add("PaymentTerms");
				dtProductWise.Columns.Add("ItemName");
				dtProductWise.Columns.Add("PackUom");
				dtProductWise.Columns.Add("Fcy");
				dtProductWise.Columns.Add("RatePrice", typeof(double));
				dtProductWise.Columns.Add("FcyAmount", typeof(double));
				dtProductWise.Columns.Add("FcyShipped", typeof(double));
				dtProductWise.Columns.Add("FcyBalance", typeof(double));
				dtProductWise.Columns.Add("NetWeight", typeof(double));
				dtProductWise.Columns.Add("ShippedWeight", typeof(double));
				dtProductWise.Columns.Add("BalanceWeight", typeof(double));
				dtProductWise.Columns.Add("Fcl", typeof(int));
				dtProductWise.Columns.Add("ShippedFcl", typeof(int));
				dtProductWise.Columns.Add("BalanceFcl", typeof(int));
				dtProductWise.Columns.Add("CommodityDetail");
				dtProductWise.Columns.Add("Status");
			}
			if (DataReportProductWise.Rows.Count > 0)
			{
				for (int i = 0; i < DataReportProductWise.Rows.Count; i++)
				{
					dtProductWise.Rows.Add(DataReportProductWise.Rows[i]["Id"], DataReportProductWise.Rows[i]["LcOrderDocNo"], DataReportProductWise.Rows[i]["ContractDate"], DataReportProductWise.Rows[i]["ContractNo"], DataReportProductWise.Rows[i]["CustomerName"], DataReportProductWise.Rows[i]["DeliveryTerms"], DataReportProductWise.Rows[i]["PaymentTerms"], DataReportProductWise.Rows[i]["ItemName"], DataReportProductWise.Rows[i]["PackUom"], DataReportProductWise.Rows[i]["Fcy"], DataReportProductWise.Rows[i]["RatePrice"], DataReportProductWise.Rows[i]["FcyAmount"], DataReportProductWise.Rows[i]["FcyShipped"], DataReportProductWise.Rows[i]["FcyBalance"], DataReportProductWise.Rows[i]["NetWeight"], DataReportProductWise.Rows[i]["ShippedWeight"], DataReportProductWise.Rows[i]["BalanceWeight"], DataReportProductWise.Rows[i]["NoofContainer"], DataReportProductWise.Rows[i]["ShippedContrainer"], DataReportProductWise.Rows[i]["BalNoofContainer"], DataReportProductWise.Rows[i]["CommodityDetial"], DataReportProductWise.Rows[i]["LcStatus"]);
				}
				grdProductWise.DataSource = dtProductWise;
				GridSettingProductWise();
			}
			else
			{
				grdProductWise.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDataForExportInfo()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtcard.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (datDateFrom.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)datDateFrom.Value);
			}
			if (datDateTo.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)datDateTo.Value);
			}
			if (rdFinancialYear.Checked)
			{
				obj.ReqType = "1";
			}
			else if (rdExportYear.Checked)
			{
				obj.ReqType = "2";
			}
			else if (rdSelectedCriteria.Checked)
			{
				obj.ReqType = "3";
			}
			DataTable DataCard = ExImLcOrder.GetDataForExportInfo(obj);
			if (dtcard.Columns.Count == 0)
			{
				dtcard.Columns.Add("Descriptions", typeof(string));
				dtcard.Columns.Add("Fcl", typeof(double));
				dtcard.Columns.Add("MTons", typeof(double));
				dtcard.Columns.Add("FcyAmount", typeof(double));
				dtcard.Columns.Add("PrctOfTot", typeof(double));
			}
			if (DataCard.Rows.Count > 0)
			{
				for (int i = 0; i < DataCard.Rows.Count; i++)
				{
					dtcard.Rows.Add(Conversion.ToString(DataCard.Rows[i]["Descriptions"]), Conversion.ToDouble(DataCard.Rows[i]["Fcl"]), Conversion.ToDouble(DataCard.Rows[i]["MTons"]), Conversion.ToDouble(DataCard.Rows[i]["FcyAmount"]), DataCard.Rows[i]["PrctOfTot"]);
				}
				grdCard.DataSource = dtcard;
				grdCard.RetrieveStructure();
				GridSettingCard();
			}
			else
			{
				grdCard.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSetting()
	{
		try
		{
			grdContractRegister.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdContractRegister.AllowEdit = (InheritableBoolean)1;
			grdContractRegister.RootTable.Columns["DocNo"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["DocDate"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["ContractNo"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["CustomerName"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["DeliveryTerms"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["PaymentTerms"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["Fcy"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["FcyAmount"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["FcyShipped"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["FcyBalance"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["NetWeight"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["ShippedWeight"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["BalanceWeight"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["Fcl"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["ShippedFcl"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["BalanceFcl"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["DestinationPort"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["Status"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["FarmingTrade"].EditType = (EditType)0;
			grdContractRegister.RootTable.Columns["DocNo"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["DocDate"].FilterEditType = (FilterEditType)6;
			grdContractRegister.RootTable.Columns["ContractNo"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["CustomerName"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["DeliveryTerms"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["PaymentTerms"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["Fcy"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["FcyAmount"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["FcyShipped"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["FcyBalance"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["NetWeight"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["ShippedWeight"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["BalanceWeight"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["Fcl"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["ShippedFcl"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["BalanceFcl"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["DestinationPort"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["Status"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["FarmingTrade"].FilterEditType = (FilterEditType)1;
			grdContractRegister.RootTable.Columns["ActionRemarks"].EditType = (EditType)1;
			grdContractRegister.RootTable.Columns["ActionRemarks"].DefaultValue = " ";
			grdContractRegister.RootTable.Columns["DocNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdContractRegister.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			grdContractRegister.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdContractRegister.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			grdContractRegister.RootTable.Columns["DeliveryTerms"].Width = Constants.ExportConstants.TermConstant;
			grdContractRegister.RootTable.Columns["PaymentTerms"].Width = Constants.ExportConstants.TermConstant;
			grdContractRegister.RootTable.Columns["Fcy"].Width = Constants.ExportConstants.FcyConstant;
			grdContractRegister.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdContractRegister.RootTable.Columns["FcyShipped"].Width = Constants.ExportConstants.AmountConstant;
			grdContractRegister.RootTable.Columns["FcyBalance"].Width = Constants.ExportConstants.AmountConstant;
			grdContractRegister.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdContractRegister.RootTable.Columns["ShippedWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdContractRegister.RootTable.Columns["BalanceWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdContractRegister.RootTable.Columns["Fcl"].Width = Constants.ExportConstants.FclConstant;
			grdContractRegister.RootTable.Columns["ShippedFcl"].Width = Constants.ExportConstants.FclConstant;
			grdContractRegister.RootTable.Columns["BalanceFcl"].Width = Constants.ExportConstants.FclConstant;
			grdContractRegister.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			grdContractRegister.RootTable.Columns["Status"].Width = Constants.ExportConstants.StatusConstant;
			grdContractRegister.RootTable.Columns["FarmingTrade"].Width = 100;
			grdContractRegister.RootTable.Columns["NoOfAttachments"].Width = 100;
			grdContractRegister.RootTable.Columns["Id"].Visible = false;
			grdContractRegister.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["ContractNo"].ColumnType = (ColumnType)5;
			grdContractRegister.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["BalanceWeight"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["ShippedWeight"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["ShippedWeight"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["ShippedWeight"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["ShippedWeight"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyShipped"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyShipped"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyShipped"].FormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyShipped"].TotalFormatString = "#,##0.###";
			grdContractRegister.RootTable.Columns["FcyBalance"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyBalance"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyBalance"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdContractRegister.RootTable.Columns["FcyBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdContractRegister.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdContractRegister.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdContractRegister.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["Fcl"].FormatString = "#,##0.##";
			grdContractRegister.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdContractRegister.RootTable.Columns["ShippedFcl"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["ShippedFcl"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["ShippedFcl"].FormatString = "#,##0.##";
			grdContractRegister.RootTable.Columns["ShippedFcl"].TotalFormatString = "#,##0.##";
			grdContractRegister.RootTable.Columns["BalanceFcl"].AggregateFunction = (AggregateFunction)2;
			grdContractRegister.RootTable.Columns["BalanceFcl"].TextAlignment = (TextAlignment)3;
			grdContractRegister.RootTable.Columns["BalanceFcl"].FormatString = "#,##0.##";
			grdContractRegister.RootTable.Columns["BalanceFcl"].TotalFormatString = "#,##0.##";
			grdContractRegister.RootTable.Columns.Add("Print");
			grdContractRegister.RootTable.Columns["Print"].Caption = "Print";
			grdContractRegister.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractRegister.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdContractRegister.RootTable.Columns["Print"].ButtonText = "Print";
			grdContractRegister.RootTable.Columns["Print"].Width = 50;
			grdContractRegister.RootTable.Columns.Add("Open");
			grdContractRegister.RootTable.Columns["Open"].Caption = "Open";
			grdContractRegister.RootTable.Columns["Open"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractRegister.RootTable.Columns["Open"].ButtonStyle = (ButtonStyle)5;
			grdContractRegister.RootTable.Columns["Open"].ButtonText = "Open";
			grdContractRegister.RootTable.Columns["Open"].Width = 50;
			grdContractRegister.RootTable.Columns.Add("Cancel");
			grdContractRegister.RootTable.Columns["Cancel"].Caption = "Cancel";
			grdContractRegister.RootTable.Columns["Cancel"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractRegister.RootTable.Columns["Cancel"].ButtonStyle = (ButtonStyle)5;
			grdContractRegister.RootTable.Columns["Cancel"].ButtonText = "Cancel";
			grdContractRegister.RootTable.Columns["Cancel"].Width = 50;
			grdContractRegister.RootTable.Columns.Add("Complete");
			grdContractRegister.RootTable.Columns["Complete"].Caption = "Complete";
			grdContractRegister.RootTable.Columns["Complete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdContractRegister.RootTable.Columns["Complete"].ButtonStyle = (ButtonStyle)5;
			grdContractRegister.RootTable.Columns["Complete"].ButtonText = "Complete";
			grdContractRegister.RootTable.Columns["Complete"].Width = 70;
			grdContractRegister.RootTable.ColumnSets["Open"].Visible = false;
			grdContractRegister.RootTable.ColumnSets["Cancel"].Visible = false;
			grdContractRegister.RootTable.ColumnSets["Complete"].Visible = false;
			if (rdOpen.Checked)
			{
				grdContractRegister.RootTable.ColumnSets["Cancel"].Visible = true;
				grdContractRegister.RootTable.ColumnSets["Complete"].Visible = true;
			}
			if (rdCancel.Checked || rdComplete.Checked)
			{
				grdContractRegister.RootTable.ColumnSets["Open"].Visible = true;
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingProductWise()
	{
		try
		{
			grdProductWise.RootTable.Columns["Id"].Visible = false;
			grdProductWise.RootTable.ColumnSets["Fcl"].Visible = false;
			grdProductWise.RootTable.ColumnSets["ShippedFcl"].Visible = false;
			grdProductWise.RootTable.ColumnSets["BalanceFcl"].Visible = false;
			grdProductWise.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yy";
			grdProductWise.RootTable.Columns["DocNo"].Width = Constants.ExportConstants.DocNoConstant;
			grdProductWise.RootTable.Columns["DocDate"].Width = Constants.ExportConstants.DateConstant;
			grdProductWise.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grdProductWise.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
			grdProductWise.RootTable.Columns["DeliveryTerms"].Width = Constants.ExportConstants.TermConstant;
			grdProductWise.RootTable.Columns["PaymentTerms"].Width = Constants.ExportConstants.TermConstant;
			grdProductWise.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.StringConstant;
			grdProductWise.RootTable.Columns["PackUom"].Width = Constants.ExportConstants.UomConstant;
			grdProductWise.RootTable.Columns["Fcy"].Width = Constants.ExportConstants.FcyConstant;
			grdProductWise.RootTable.Columns["RatePrice"].Width = Constants.ExportConstants.ExRateConstant;
			grdProductWise.RootTable.Columns["FcyAmount"].Width = Constants.ExportConstants.AmountConstant;
			grdProductWise.RootTable.Columns["FcyShipped"].Width = Constants.ExportConstants.AmountConstant;
			grdProductWise.RootTable.Columns["FcyBalance"].Width = Constants.ExportConstants.AmountConstant;
			grdProductWise.RootTable.Columns["NetWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdProductWise.RootTable.Columns["ShippedWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdProductWise.RootTable.Columns["BalanceWeight"].Width = Constants.ExportConstants.WeightConstant;
			grdProductWise.RootTable.Columns["Fcl"].Width = Constants.ExportConstants.FclConstant;
			grdProductWise.RootTable.Columns["ShippedFcl"].Width = Constants.ExportConstants.FclConstant;
			grdProductWise.RootTable.Columns["BalanceFcl"].Width = Constants.ExportConstants.FclConstant;
			grdProductWise.RootTable.Columns["CommodityDetail"].Width = Constants.ExportConstants.CommodityDetail;
			grdProductWise.RootTable.Columns["Status"].Width = Constants.ExportConstants.StatusConstant;
			grdProductWise.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["NetWeight"].FormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["BalanceWeight"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["BalanceWeight"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["BalanceWeight"].FormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["BalanceWeight"].TotalFormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["ShippedWeight"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["ShippedWeight"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["ShippedWeight"].FormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["ShippedWeight"].TotalFormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["FcyShipped"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["FcyShipped"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["FcyShipped"].FormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["FcyShipped"].TotalFormatString = "#,##0.###";
			grdProductWise.RootTable.Columns["FcyBalance"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["FcyBalance"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["FcyBalance"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdProductWise.RootTable.Columns["FcyBalance"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdProductWise.RootTable.Columns["FcyAmount"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdProductWise.RootTable.Columns["FcyAmount"].TotalFormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdProductWise.RootTable.Columns["Fcl"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["Fcl"].FormatString = "#,##0.##";
			grdProductWise.RootTable.Columns["Fcl"].TotalFormatString = "#,##0.##";
			grdProductWise.RootTable.Columns["ShippedFcl"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["ShippedFcl"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["ShippedFcl"].FormatString = "#,##0.##";
			grdProductWise.RootTable.Columns["ShippedFcl"].TotalFormatString = "#,##0.##";
			grdProductWise.RootTable.Columns["BalanceFcl"].AggregateFunction = (AggregateFunction)2;
			grdProductWise.RootTable.Columns["BalanceFcl"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["BalanceFcl"].FormatString = "#,##0.##";
			grdProductWise.RootTable.Columns["BalanceFcl"].TotalFormatString = "#,##0.##";
			grdProductWise.RootTable.Columns["RatePrice"].TextAlignment = (TextAlignment)3;
			grdProductWise.RootTable.Columns["RatePrice"].FormatString = "#,##0.##";
			grdProductWise.RootTable.Columns["RatePrice"].TotalFormatString = "#,##0.##";
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingCard()
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdCard.AutomaticSort = false;
			grdCard.FilterMode = (FilterMode)0;
			grdCard.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdCard.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdCard.RootTable.Columns["Descriptions"].Width = 160;
			grdCard.RootTable.Columns["Fcl"].Width = 60;
			grdCard.RootTable.Columns["MTons"].Width = 90;
			grdCard.RootTable.Columns["FcyAmount"].Width = 80;
			grdCard.RootTable.Columns["PrctOfTot"].Width = 90;
			grdCard.RootTable.Columns["PrctOfTot"].Caption = "% Of Total";
			grdCard.RootTable.Columns["FcyAmount"].TextAlignment = (TextAlignment)3;
			grdCard.RootTable.Columns["FcyAmount"].FormatString = clsGlobalVariables.stringFormatsingleForFcy;
			grdCard.RootTable.Columns["Fcl"].TextAlignment = (TextAlignment)3;
			grdCard.RootTable.Columns["Fcl"].FormatString = "#,##0.##";
			grdCard.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
			grdCard.RootTable.Columns["MTons"].FormatString = "#,##0.##";
			grdCard.RootTable.Columns["PrctOfTot"].TextAlignment = (TextAlignment)3;
			grdCard.RootTable.Columns["PrctOfTot"].FormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Expected O, but got Unknown
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Expected O, but got Unknown
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Print")
			{
				DataTable dt = new DataTable();
				ReportsParameters obj = new ReportsParameters();
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.Id = Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value);
				obj.ApprovedFilter = "All";
				dt = SaleContractReports.SaleContractReports501(obj);
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
				}
				else
				{
					List<SubReportObject> lstSubReports = new List<SubReportObject>();
					SubReportObject subReportObject1 = new SubReportObject(SaleContractReports.LcorderOtherItem_SubRpt501(obj.Id), "LcorderOtherItem_SubRpt.rpt");
					lstSubReports.Add(subReportObject1);
					SubReportObject subReportObject2 = new SubReportObject(SaleContractReports.ExImLcOrderPaymentTermsDetail_SubRpt501(obj.Id), "ExImLcOrderPaymentTermsDetail_SubRpt.rpt");
					lstSubReports.Add(subReportObject2);
					SubReportObject subReportObject3 = new SubReportObject(SaleContractReports.ExImLcContractPackingMaterialDetail_SubRpt501(obj.Id), "ExImLcContractPackingMaterialDetail_SubRpt.rpt");
					lstSubReports.Add(subReportObject3);
					Reporting val = new Reporting();
					val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTableSubReprt(dt, "501-ExportSalesContractExportNew.rpt", lstSubReports);
				}
			}
			if (e.Column.Key == "Complete")
			{
				if (string.IsNullOrEmpty(Conversion.ToString(grdContractRegister.CurrentRow.Cells["ActionRemarks"].Value)))
				{
					MessageBox.Show("Action Remarks Field is Compulsory.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				ContractStatusChange(Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value), "Complete", Conversion.ToString(grdContractRegister.CurrentRow.Cells["ActionRemarks"].Value));
			}
			if (e.Column.Key == "Open")
			{
				if (string.IsNullOrEmpty(Conversion.ToString(grdContractRegister.CurrentRow.Cells["ActionRemarks"].Value)))
				{
					MessageBox.Show("Action Remarks Field is Compulsory.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				ContractStatusChange(Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value), "Open", Conversion.ToString(grdContractRegister.CurrentRow.Cells["ActionRemarks"].Value));
			}
			if (e.Column.Key == "Cancel")
			{
				if (Conversion.ToDouble(grdContractRegister.CurrentRow.Cells["FcyShipped"].Value) > 0.0)
				{
					MessageBox.Show("You cannot cancel this record because the shipped weight is greater than zero. You should mark it as complete.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else if (string.IsNullOrEmpty(Conversion.ToString(grdContractRegister.CurrentRow.Cells["ActionRemarks"].Value)))
				{
					MessageBox.Show("Action Remarks Field is Compulsory.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					ContractStatusChange(Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value), "Cancel", Conversion.ToString(grdContractRegister.CurrentRow.Cells["ActionRemarks"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContractStatusChange(int Id, string ReqType, string ActionRemarks)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Expected O, but got Unknown
		try
		{
			if (MessageBox.Show("Are you sure to " + ReqType + " ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ExImLcOrder.ExportContractStatusUpdate(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					ModifyUserId = UserAccount.ID,
					DocumentTypeId = 202,
					Id = Id,
					ReqType = ReqType,
					Remarks = ActionRemarks
				});
				MessageBox.Show(ReqType + " Successfully");
				if (ChkSummary.Checked)
				{
					tabControl2.SelectedIndex = 0;
					grdProductWise.DataSource = null;
					DataReportProductWise.Clear();
					GridBind();
				}
				else
				{
					tabControl2.SelectedIndex = 1;
					grdContractRegister.DataSource = null;
					DataReport.Clear();
					GridBindProductWise();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			datDateFrom.Focus();
			((Control)(object)cmbcontractNo).Text = string.Empty;
			((Control)(object)cmbSupplierName).Text = string.Empty;
			((Control)(object)CmbSalePerson).Text = string.Empty;
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

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortCutKey_Click(object sender, EventArgs e)
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

	private void frmExportShipingLineBookingRpt_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbperemeter.Focus();
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdContractRegister).Focus();
			}
			if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1))
			{
				toolStripButton2_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2))
			{
				btnregister523_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3))
			{
				btnPrintRegister_Click(null, null);
			}
			if (e.Alt && (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4))
			{
				btnregister561_Click(null, null);
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
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+F5", "For Focus on Date Type");
			dt.Rows.Add("Ctrl+S", "For Showing Data");
			dt.Rows.Add("Alt+1", "For Print 518");
			dt.Rows.Add("Alt+2", "For Print 523");
			dt.Rows.Add("Alt+3", "For Print 526");
			dt.Rows.Add("Alt+4", "For Print 561");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdContractRegister).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grdContractRegister.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grdContractRegister.RootTable.Columns).Count;
			int TableColumnsCount = dtHistory.Columns.Count + 1;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(dtHistory, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(dtHistory.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdContractRegister).Name);
				grdContractRegister.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
			}
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
			if (!FormInitialized)
			{
				return;
			}
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdProductWise).Name);
			if (fs == null)
			{
				return;
			}
			string fileName = fs.Name;
			grdProductWise.LoadLayoutFile((Stream)fs);
			fs.Close();
			fs.Dispose();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.Load(fileName);
			int count = ((JanusCollectionBase)grdProductWise.RootTable.Columns).Count;
			int TableColumnsCount = dtProductWise.Columns.Count;
			if (count == TableColumnsCount)
			{
				return;
			}
			List<string> missingColumns = CommonServices.GetMissingColumns(dtProductWise, xmlDoc);
			foreach (string column in missingColumns)
			{
				string d = CommonServices.GetDataTypeString(dtProductWise.Columns[column].DataType);
				CommonServices.AddNewColumn(xmlDoc, fileName, column, d);
			}
			if (missingColumns.Count > 0)
			{
				FileStream fsNew = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdProductWise).Name);
				grdProductWise.LoadLayoutFile((Stream)fsNew);
				fsNew.Close();
				fsNew.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
	{
		try
		{
			ShowRegister518();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPrintRegister_Click(object sender, EventArgs e)
	{
		try
		{
			ShowRegister526();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowRegister518()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Expected O, but got Unknown
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = ExportPdfReport.ExpRptSalesContractExportRegister_518(new ReportsParameters
			{
				OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId),
				CompanyId = Conversion.ToInt((object)UserAccount.CompanyId),
				FromDate = Conversion.ToDateTime((object)datDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)datDateTo.Value),
				LcOrderId = Conversion.ToInt(cmbcontractNo.Value),
				SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value),
				ItemId = Conversion.ToInt(cmbItemName.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "518-ExpRptSalesContractExportRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowRegister523()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			bool flag = true;
			if ((Conversion.ToInt(cmbcontractNo.Value) == 0 && Conversion.ToInt(cmbSupplierName.Value) == 0) || 1 == 0)
			{
				throw new Exception("Please Select Contract No or Customer To Proceed!");
			}
			DataTable dt = ExportPdfReport.ExpRptSalesContractExportRegister_523(new ReportsParameters
			{
				OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId),
				CompanyId = Conversion.ToInt((object)UserAccount.CompanyId),
				ExImLcOrderId = Conversion.ToInt(cmbcontractNo.Value),
				SupplierCustomerId = Conversion.ToInt(cmbSupplierName.Value),
				FromDate = Conversion.ToDateTime((object)datDateFrom.Value),
				ToDate = Conversion.ToDateTime((object)datDateTo.Value)
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "523-ExpRptSalesContractWiseInvoiceRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowRegister526()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataReport == null || DataReport.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(DataReport, "526-SaleContractRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ShowRegister561()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (DataReportProductWise == null || DataReportProductWise.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(DataReportProductWise, "561-SaleContractRegisterItemWise.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnregister523_Click(object sender, EventArgs e)
	{
		try
		{
			ShowRegister523();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnregister561_Click(object sender, EventArgs e)
	{
		try
		{
			ShowRegister561();
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
				datDateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				datDateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				datDateFrom.Value = first;
				datDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				datDateFrom.Value = first2;
				datDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				datDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
	{
		try
		{
			if (e.Shift && e.KeyCode == Keys.Tab)
			{
				ShiftTabPressed = true;
			}
			else
			{
				ShiftTabPressed = false;
			}
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
			if (!ShiftTabPressed)
			{
				cmbperemeter.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractRegister_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			int ID = 0;
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (grdContractRegister.GetRows().Length != 0)
			{
				ID = Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value);
			}
			if (e.Control && e.KeyCode == Keys.Space && grdContractRegister.CurrentColumn.Key == "Print")
			{
				DataTable dt = new DataTable();
				dt = ExportPdfReport.ExpRptSalesContractExport_501(new ReportsParameters
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Id = ID,
					ApprovedFilter = Conversion.ToString((object)"All")
				});
				if (dt == null || dt.Rows.Count == 0)
				{
					MessageBox.Show("No Record Found For Display");
					return;
				}
				Reporting val = new Reporting();
				val.RptPerameter("CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dt, "501-ExpRptSalesContractExport.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkSummary_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (ChkSummary.Checked)
			{
				btnregisterprint526.Visible = true;
				btnregister561.Visible = false;
			}
			else if (ChkProductWise.Checked)
			{
				btnregister561.Visible = true;
				btnregisterprint526.Visible = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdContractRegister_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdContractRegister.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int ContractId = Conversion.ToInt(grdContractRegister.CurrentRow.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(ContractId, 202);
				}
				else if (e.Column.Key == "ContractNo")
				{
					frmContractDetailByContractId obj = new frmContractDetailByContractId(UserAccount);
					obj.ContractId = ContractId;
					obj.Show();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChkProductWise_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ReportsShowHide();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportsShowHide()
	{
		try
		{
			btnregister523.Visible = false;
			if (ChkSummary.Checked)
			{
				btnregisterprint526.Visible = true;
				btnregister561.Visible = false;
			}
			else
			{
				btnregisterprint526.Visible = false;
				toolStripButton2.Visible = false;
				btnregister561.Visible = true;
			}
			if (Conversion.ToInt(cmbcontractNo.Value) > 0)
			{
				btnregister523.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ExImSaleContractRegister_FormClosed(object sender, FormClosedEventArgs e)
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
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Expected O, but got Unknown
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Expected O, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Expected O, but got Unknown
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Expected O, but got Unknown
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Expected O, but got Unknown
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Expected O, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Expected O, but got Unknown
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Expected O, but got Unknown
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Expected O, but got Unknown
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Expected O, but got Unknown
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Expected O, but got Unknown
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Expected O, but got Unknown
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Expected O, but got Unknown
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Expected O, but got Unknown
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
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
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Expected O, but got Unknown
		//IL_0310: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_0394: Unknown result type (might be due to invalid IL or missing references)
		//IL_039e: Expected O, but got Unknown
		//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bf: Expected O, but got Unknown
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_0422: Expected O, but got Unknown
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0438: Expected O, but got Unknown
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_047a: Expected O, but got Unknown
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bc: Expected O, but got Unknown
		//IL_0bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bcc: Expected O, but got Unknown
		//IL_0bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0be3: Expected O, but got Unknown
		GridEXLayout grdContractRegister_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ExportReports.ExImSaleContractRegister));
		GridEXLayout grdProductWise_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdCard_DesignTimeLayout = new GridEXLayout();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance14 = new Appearance();
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
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdContractRegister = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label10 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdProductWise = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.grdCard = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.CmbSalePerson = new UltraCombo();
		this.datDateTo = new System.Windows.Forms.DateTimePicker();
		this.datDateFrom = new System.Windows.Forms.DateTimePicker();
		this.chkSkipZero = new System.Windows.Forms.CheckBox();
		this.label1 = new System.Windows.Forms.Label();
		this.ChkProductWise = new System.Windows.Forms.RadioButton();
		this.CmbStatus = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.ChkSummary = new System.Windows.Forms.RadioButton();
		this.cmbperemeter = new UltraCombo();
		this.label51 = new System.Windows.Forms.Label();
		this.Cmbtradtype = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.CmbFarmingType = new UltraCombo();
		this.btnShow = new System.Windows.Forms.Button();
		this.label8 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbItemName = new UltraCombo();
		this.cmbcontractNo = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label50 = new System.Windows.Forms.Label();
		this.cmbSupplierName = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdComplete = new System.Windows.Forms.RadioButton();
		this.rdOpen = new System.Windows.Forms.RadioButton();
		this.rdCancel = new System.Windows.Forms.RadioButton();
		this.panel5 = new System.Windows.Forms.Panel();
		this.rdSelectedCriteria = new System.Windows.Forms.RadioButton();
		this.label3 = new System.Windows.Forms.Label();
		this.rdExportYear = new System.Windows.Forms.RadioButton();
		this.rdFinancialYear = new System.Windows.Forms.RadioButton();
		this.panel4 = new System.Windows.Forms.Panel();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnregister523 = new System.Windows.Forms.ToolStripButton();
		this.btnregisterprint526 = new System.Windows.Forms.ToolStripButton();
		this.btnregister561 = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKey = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdContractRegister).BeginInit();
		this.panel8.SuspendLayout();
		this.tabPage3.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdProductWise).BeginInit();
		this.panel9.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdCard).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbtradtype).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcontractNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).BeginInit();
		this.groupBox1.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.tsItemType.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1370, 674);
		this.panel1.TabIndex = 0;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1370, 674);
		this.tabControl1.TabIndex = 3;
		this.tabPage1.Controls.Add(this.panel7);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Location = new System.Drawing.Point(4, 25);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1362, 645);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Contract Register";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.tabControl2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 239);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1356, 403);
		this.panel7.TabIndex = 4;
		this.tabControl2.Controls.Add(this.tabPage2);
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1356, 403);
		this.tabControl2.TabIndex = 0;
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 25);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1348, 374);
		this.tabPage2.TabIndex = 0;
		this.tabPage2.Text = "Contract Summary";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdContractRegister);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(3, 27);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1342, 344);
		this.panel10.TabIndex = 21;
		this.grdContractRegister.AllowEdit = (InheritableBoolean)2;
		this.grdContractRegister.AlternatingColors = true;
		this.grdContractRegister.AutomaticSort = false;
		this.grdContractRegister.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdContractRegister_DesignTimeLayout.LayoutString = resources.GetString("grdContractRegister_DesignTimeLayout.LayoutString");
		this.grdContractRegister.DesignTimeLayout = grdContractRegister_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdContractRegister.DynamicFiltering = true;
		this.grdContractRegister.FilterMode = (FilterMode)2;
		this.grdContractRegister.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdContractRegister.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Name = "grdContractRegister";
		((ScrollableBase)this.grdContractRegister).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdContractRegister).Size = new System.Drawing.Size(1342, 344);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).TabIndex = 2;
		this.grdContractRegister.TotalRow = (InheritableBoolean)1;
		this.grdContractRegister.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdContractRegister).VisualStyle = (VisualStyle)4;
		this.grdContractRegister.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdContractRegister.LinkClicked += new ColumnActionEventHandler(grdContractRegister_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdContractRegister).KeyDown += new System.Windows.Forms.KeyEventHandler(grdContractRegister_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel8.Controls.Add(this.label10);
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1342, 24);
		this.panel8.TabIndex = 20;
		this.label10.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(1168, 4);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(132, 15);
		this.label10.TabIndex = 601;
		this.label10.Text = "Grd bar used for export ";
		this.label10.Visible = false;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1304, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdContractRegister;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar1.TabIndex = 587;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.tabPage3.Controls.Add(this.panel11);
		this.tabPage3.Controls.Add(this.panel9);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1348, 374);
		this.tabPage3.TabIndex = 1;
		this.tabPage3.Text = "Contract Product Wise";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdProductWise);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(3, 27);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1342, 344);
		this.panel11.TabIndex = 21;
		this.grdProductWise.AllowEdit = (InheritableBoolean)2;
		this.grdProductWise.AlternatingColors = true;
		this.grdProductWise.AutomaticSort = false;
		this.grdProductWise.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdProductWise_DesignTimeLayout.LayoutString = resources.GetString("grdProductWise_DesignTimeLayout.LayoutString");
		this.grdProductWise.DesignTimeLayout = grdProductWise_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdProductWise).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdProductWise.DynamicFiltering = true;
		this.grdProductWise.FilterMode = (FilterMode)2;
		this.grdProductWise.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdProductWise).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdProductWise.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdProductWise).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdProductWise).Name = "grdProductWise";
		((ScrollableBase)this.grdProductWise).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdProductWise).Size = new System.Drawing.Size(1342, 344);
		((System.Windows.Forms.Control)(object)this.grdProductWise).TabIndex = 3;
		this.grdProductWise.TotalRow = (InheritableBoolean)1;
		this.grdProductWise.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdProductWise).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel9.Controls.Add(this.label12);
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1342, 24);
		this.panel9.TabIndex = 20;
		this.label12.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(1165, 4);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(132, 15);
		this.label12.TabIndex = 602;
		this.label12.Text = "Grd bar used for export ";
		this.label12.Visible = false;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1302, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdProductWise;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 587;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Controls.Add(this.panel2);
		this.panel6.Controls.Add(this.panel5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 34);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1356, 205);
		this.panel6.TabIndex = 0;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.grdCard);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(630, 28);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(506, 177);
		this.panel3.TabIndex = 593;
		this.grdCard.AllowEdit = (InheritableBoolean)2;
		this.grdCard.AutomaticSort = false;
		this.grdCard.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdCard_DesignTimeLayout.LayoutString = resources.GetString("grdCard_DesignTimeLayout.LayoutString");
		this.grdCard.DesignTimeLayout = grdCard_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdCard).Dock = System.Windows.Forms.DockStyle.Left;
		this.grdCard.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdCard).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdCard.GroupByBoxVisible = false;
		this.grdCard.GroupTotals = (GroupTotals)2;
		this.grdCard.HeaderFormatStyle.LineAlignment = (TextAlignment)2;
		this.grdCard.HeaderFormatStyle.TextAlignment = (TextAlignment)2;
		((System.Windows.Forms.Control)(object)this.grdCard).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdCard).Name = "grdCard";
		((ScrollableBase)this.grdCard).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdCard).Size = new System.Drawing.Size(505, 177);
		((System.Windows.Forms.Control)(object)this.grdCard).TabIndex = 48;
		this.grdCard.TotalRow = (InheritableBoolean)1;
		this.grdCard.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdCard).VisualStyle = (VisualStyle)4;
		this.panel2.Controls.Add(this.label28);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSalePerson);
		this.panel2.Controls.Add(this.datDateTo);
		this.panel2.Controls.Add(this.datDateFrom);
		this.panel2.Controls.Add(this.chkSkipZero);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.ChkProductWise);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStatus);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Controls.Add(this.ChkSummary);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.panel2.Controls.Add(this.label51);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbtradtype);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFarmingType);
		this.panel2.Controls.Add(this.btnShow);
		this.panel2.Controls.Add(this.label8);
		this.panel2.Controls.Add(this.label11);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbcontractNo);
		this.panel2.Controls.Add(this.label9);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.label50);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierName);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel2.Location = new System.Drawing.Point(0, 28);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(630, 177);
		this.panel2.TabIndex = 592;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(6, 124);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(73, 15);
		this.label28.TabIndex = 600;
		this.label28.Text = "Sales Person";
		this.CmbSalePerson.AllowNull = (DefaultableBoolean)1;
		this.CmbSalePerson.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSalePerson.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSalePerson.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance12).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance13).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSalePerson).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSalePerson.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Location = new System.Drawing.Point(83, 118);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Name = "CmbSalePerson";
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.CmbSalePerson).TabIndex = 4;
		this.datDateTo.Checked = false;
		this.datDateTo.CustomFormat = "dd-MMM-yy";
		this.datDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDateTo.Location = new System.Drawing.Point(83, 65);
		this.datDateTo.Name = "datDateTo";
		this.datDateTo.ShowCheckBox = true;
		this.datDateTo.Size = new System.Drawing.Size(162, 23);
		this.datDateTo.TabIndex = 2;
		this.datDateFrom.Checked = false;
		this.datDateFrom.CustomFormat = "dd-MMM-yy";
		this.datDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datDateFrom.Location = new System.Drawing.Point(83, 38);
		this.datDateFrom.Name = "datDateFrom";
		this.datDateFrom.ShowCheckBox = true;
		this.datDateFrom.Size = new System.Drawing.Size(162, 23);
		this.datDateFrom.TabIndex = 1;
		this.chkSkipZero.AutoSize = true;
		this.chkSkipZero.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkSkipZero.Location = new System.Drawing.Point(530, 73);
		this.chkSkipZero.Name = "chkSkipZero";
		this.chkSkipZero.Size = new System.Drawing.Size(76, 19);
		this.chkSkipZero.TabIndex = 11;
		this.chkSkipZero.Text = "Skip Zero";
		this.chkSkipZero.UseVisualStyleBackColor = true;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.Red;
		this.label1.Location = new System.Drawing.Point(69, 41);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(14, 17);
		this.label1.TabIndex = 573;
		this.label1.Text = "*";
		this.ChkProductWise.AutoSize = true;
		this.ChkProductWise.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkProductWise.Location = new System.Drawing.Point(530, 55);
		this.ChkProductWise.Name = "ChkProductWise";
		this.ChkProductWise.Size = new System.Drawing.Size(93, 19);
		this.ChkProductWise.TabIndex = 10;
		this.ChkProductWise.Text = "ProductWise";
		this.ChkProductWise.UseVisualStyleBackColor = true;
		this.ChkProductWise.CheckedChanged += new System.EventHandler(ChkProductWise_CheckedChanged);
		this.CmbStatus.AllowNull = (DefaultableBoolean)1;
		this.CmbStatus.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStatus.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStatus.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance90).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance90).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance96).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStatus).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStatus).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStatus.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Location = new System.Drawing.Point(531, 9);
		((System.Windows.Forms.Control)(object)this.CmbStatus).Name = "CmbStatus";
		((System.Windows.Forms.Control)(object)this.CmbStatus).Size = new System.Drawing.Size(91, 26);
		((System.Windows.Forms.Control)(object)this.CmbStatus).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbStatus).Visible = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 69);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(48, 15);
		this.label5.TabIndex = 575;
		this.label5.Text = "Date To";
		this.ChkSummary.AutoSize = true;
		this.ChkSummary.Checked = true;
		this.ChkSummary.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkSummary.Location = new System.Drawing.Point(530, 37);
		this.ChkSummary.Name = "ChkSummary";
		this.ChkSummary.Size = new System.Drawing.Size(77, 19);
		this.ChkSummary.TabIndex = 9;
		this.ChkSummary.TabStop = true;
		this.ChkSummary.Text = "Summary";
		this.ChkSummary.UseVisualStyleBackColor = true;
		this.ChkSummary.CheckedChanged += new System.EventHandler(ChkSummary_CheckedChanged);
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(83, 9);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(6, 42);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(63, 15);
		this.label51.TabIndex = 572;
		this.label51.Text = "Date From";
		this.Cmbtradtype.AllowNull = (DefaultableBoolean)1;
		this.Cmbtradtype.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbtradtype.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbtradtype.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbtradtype).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.Cmbtradtype).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbtradtype).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbtradtype).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Cmbtradtype.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbtradtype).Location = new System.Drawing.Point(331, 90);
		((System.Windows.Forms.Control)(object)this.Cmbtradtype).Name = "Cmbtradtype";
		((System.Windows.Forms.Control)(object)this.Cmbtradtype).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.Cmbtradtype).TabIndex = 8;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Red;
		this.label4.Location = new System.Drawing.Point(69, 68);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(14, 17);
		this.label4.TabIndex = 576;
		this.label4.Text = "*";
		this.CmbFarmingType.AllowNull = (DefaultableBoolean)1;
		this.CmbFarmingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFarmingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFarmingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbFarmingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFarmingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFarmingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFarmingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFarmingType).Location = new System.Drawing.Point(331, 63);
		((System.Windows.Forms.Control)(object)this.CmbFarmingType).Name = "CmbFarmingType";
		((System.Windows.Forms.Control)(object)this.CmbFarmingType).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.CmbFarmingType).TabIndex = 7;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(529, 90);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(65, 26);
		this.btnShow.TabIndex = 12;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.btnShow.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(btnShow_PreviewKeyDown);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(249, 96);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(64, 15);
		this.label8.TabIndex = 588;
		this.label8.Text = "Trade Type";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(6, 15);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 586;
		this.label11.Text = "Date Type";
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(331, 36);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 6;
		this.cmbcontractNo.AllowNull = (DefaultableBoolean)1;
		this.cmbcontractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbcontractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbcontractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance71).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance71).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance72;
		((SpecialBoxBase)((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance73).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance77).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance78).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance78).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance78).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance78).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance78;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbcontractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbcontractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Location = new System.Drawing.Point(83, 90);
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Name = "cmbcontractNo";
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).Size = new System.Drawing.Size(162, 26);
		((System.Windows.Forms.Control)(object)this.cmbcontractNo).TabIndex = 3;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(249, 124);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(40, 15);
		this.label9.TabIndex = 18;
		this.label9.Text = "Status";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(249, 69);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(79, 15);
		this.label7.TabIndex = 588;
		this.label7.Text = "Farming Type";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 96);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 18;
		this.label2.Text = "Contract No";
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(249, 15);
		this.label50.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(58, 15);
		this.label50.TabIndex = 16;
		this.label50.Text = "Customer";
		this.cmbSupplierName.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance85;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance86).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance87;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance91).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance92).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance92).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance92).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance92;
		((AppearanceBase)appearance93).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Location = new System.Drawing.Point(331, 9);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Name = "cmbSupplierName";
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).Size = new System.Drawing.Size(194, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierName).TabIndex = 5;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(249, 42);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(67, 15);
		this.label6.TabIndex = 588;
		this.label6.Text = "Item Name";
		this.groupBox1.Controls.Add(this.rdAll);
		this.groupBox1.Controls.Add(this.rdComplete);
		this.groupBox1.Controls.Add(this.rdOpen);
		this.groupBox1.Controls.Add(this.rdCancel);
		this.groupBox1.Location = new System.Drawing.Point(331, 111);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(292, 41);
		this.groupBox1.TabIndex = 13;
		this.groupBox1.TabStop = false;
		this.rdAll.AutoSize = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.rdAll.Location = new System.Drawing.Point(212, 14);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(39, 19);
		this.rdAll.TabIndex = 598;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdComplete.AutoSize = true;
		this.rdComplete.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdComplete.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.rdComplete.Location = new System.Drawing.Point(64, 14);
		this.rdComplete.Name = "rdComplete";
		this.rdComplete.Size = new System.Drawing.Size(76, 19);
		this.rdComplete.TabIndex = 597;
		this.rdComplete.Text = "Complete";
		this.rdComplete.UseVisualStyleBackColor = true;
		this.rdOpen.AutoSize = true;
		this.rdOpen.Checked = true;
		this.rdOpen.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdOpen.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.rdOpen.Location = new System.Drawing.Point(6, 14);
		this.rdOpen.Name = "rdOpen";
		this.rdOpen.Size = new System.Drawing.Size(54, 19);
		this.rdOpen.TabIndex = 595;
		this.rdOpen.TabStop = true;
		this.rdOpen.Text = "Open";
		this.rdOpen.UseVisualStyleBackColor = true;
		this.rdCancel.AutoSize = true;
		this.rdCancel.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdCancel.Location = new System.Drawing.Point(143, 14);
		this.rdCancel.Name = "rdCancel";
		this.rdCancel.Size = new System.Drawing.Size(60, 19);
		this.rdCancel.TabIndex = 592;
		this.rdCancel.Text = "Cancel";
		this.rdCancel.UseVisualStyleBackColor = true;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.rdSelectedCriteria);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Controls.Add(this.rdExportYear);
		this.panel5.Controls.Add(this.rdFinancialYear);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1356, 28);
		this.panel5.TabIndex = 591;
		this.rdSelectedCriteria.AutoSize = true;
		this.rdSelectedCriteria.Checked = true;
		this.rdSelectedCriteria.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSelectedCriteria.ForeColor = System.Drawing.Color.White;
		this.rdSelectedCriteria.Location = new System.Drawing.Point(779, 4);
		this.rdSelectedCriteria.Name = "rdSelectedCriteria";
		this.rdSelectedCriteria.Size = new System.Drawing.Size(110, 19);
		this.rdSelectedCriteria.TabIndex = 594;
		this.rdSelectedCriteria.TabStop = true;
		this.rdSelectedCriteria.Text = "Selected Criteria";
		this.rdSelectedCriteria.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(6, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(193, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Sale Contract Register";
		this.rdExportYear.AutoSize = true;
		this.rdExportYear.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdExportYear.ForeColor = System.Drawing.Color.White;
		this.rdExportYear.Location = new System.Drawing.Point(689, 4);
		this.rdExportYear.Name = "rdExportYear";
		this.rdExportYear.Size = new System.Drawing.Size(84, 19);
		this.rdExportYear.TabIndex = 593;
		this.rdExportYear.Text = "Export Year";
		this.rdExportYear.UseVisualStyleBackColor = true;
		this.rdFinancialYear.AutoSize = true;
		this.rdFinancialYear.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdFinancialYear.ForeColor = System.Drawing.Color.White;
		this.rdFinancialYear.Location = new System.Drawing.Point(586, 4);
		this.rdFinancialYear.Name = "rdFinancialYear";
		this.rdFinancialYear.Size = new System.Drawing.Size(97, 19);
		this.rdFinancialYear.TabIndex = 592;
		this.rdFinancialYear.Text = "Financial Year";
		this.rdFinancialYear.UseVisualStyleBackColor = true;
		this.panel4.Controls.Add(this.tsItemType);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(3, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1356, 31);
		this.panel4.TabIndex = 2;
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Items.AddRange(new System.Windows.Forms.ToolStripItem[7] { this.btnnew, this.btnRefresh, this.toolStripButton2, this.btnregister523, this.btnregisterprint526, this.btnregister561, this.btnShortCutKey });
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1356, 27);
		this.tsItemType.TabIndex = 3;
		this.tsItemType.Text = "toolStrip4";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(58, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripButton2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(150, 24);
		this.toolStripButton2.Text = "518-Print Register";
		this.toolStripButton2.Visible = false;
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.btnregister523.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnregister523.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnregister523.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnregister523.Name = "btnregister523";
		this.btnregister523.Size = new System.Drawing.Size(150, 24);
		this.btnregister523.Text = "523-Print Register";
		this.btnregister523.Click += new System.EventHandler(btnregister523_Click);
		this.btnregisterprint526.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnregisterprint526.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnregisterprint526.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnregisterprint526.Name = "btnregisterprint526";
		this.btnregisterprint526.Size = new System.Drawing.Size(150, 24);
		this.btnregisterprint526.Text = "526-Print Register";
		this.btnregisterprint526.Click += new System.EventHandler(btnPrintRegister_Click);
		this.btnregister561.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnregister561.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnregister561.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnregister561.Name = "btnregister561";
		this.btnregister561.Size = new System.Drawing.Size(150, 24);
		this.btnregister561.Text = "561-Print Register";
		this.btnregister561.Click += new System.EventHandler(btnregister561_Click);
		this.btnShortCutKey.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKey.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKey.Name = "btnShortCutKey";
		this.btnShortCutKey.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKey.Text = "ShortCut Keys";
		this.btnShortCutKey.Click += new System.EventHandler(btnShortCutKey_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1370, 674);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "ExImSaleContractRegister";
		this.Text = "Sale Contract Register";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ExImSaleContractRegister_FormClosed);
		base.Load += new System.EventHandler(frmExportShipingLineBookingRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmExportShipingLineBookingRpt_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdContractRegister).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabPage3.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdProductWise).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdCard).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbSalePerson).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStatus).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbtradtype).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbFarmingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbcontractNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierName).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.tsItemType.ResumeLayout(false);
		this.tsItemType.PerformLayout();
		base.ResumeLayout(false);
	}
}
