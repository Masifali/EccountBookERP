using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Inventory;
using Architecture.BLL.Production;
using Architecture.BLL.pcc;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.Production;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Pcc.Reports;

public class WagesReportWithActivities : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtBindCombo = new DataTable();

	public DateTime FromDate;

	public DateTime ToDate;

	private DataTable dtWagesActivity = new DataTable();

	private DataTable dtWagesDetail = new DataTable();

	private DataTable dtWagesDetailWithoutItem = new DataTable();

	private DataTable dtSummaryByItem = new DataTable();

	private DataTable dtSummaryByContractor = new DataTable();

	private DataTable dtSummaryByItemandContractor = new DataTable();

	private DataTable dtSummaryByItemandServiceActivity = new DataTable();

	private DataTable dtSummaryByContractorandServiceActivity = new DataTable();

	private DataTable dttSummaryByItemContractorandServiceActivity = new DataTable();

	private DataTable dtSummaryByWagesGroup = new DataTable();

	private DataTable dtSummaryByWagesGroupandServiceActivity = new DataTable();

	private DataTable dtSummaryByWagesGroupandContractor = new DataTable();

	private DataTable dtSummaryByWagesGroupContractorandServiceActivity = new DataTable();

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton BtnRefresh;

	private ToolStripButton btnPrint;

	private Panel panel5;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	private Panel panel2;

	private Panel panel3;

	private GroupBox groupBox1;

	private Label label11;

	internal UltraCombo cmbReportType;

	private Label label9;

	internal UltraCombo cmbItem;

	private Label label1;

	private DateTimePicker txtDateFrom;

	private DateTimePicker txtDateTo;

	private Label label3;

	private TextBox txtDocTo;

	private TextBox txtFromDoc;

	private Label label6;

	internal UltraCombo cmbContractor;

	private Label label5;

	internal UltraCombo cmbServiceActivity;

	private Button btnshow;

	private ToolStripButton btnNew;

	private ToolStripButton btnshortcutkeys;

	private Label label2;

	internal UltraCombo cmbDateType;

	internal GridEX grdWagesReport;

	private Label label7;

	internal UltraCombo Cmbplant;

	private Label label4;

	private RadioButton RadProduction;

	private RadioButton RadSales;

	private RadioButton RadAll;

	public WagesReportWithActivities(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void WagesReportWithActivities_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			dtWagesDetail.Columns.Add("DocumentTypeId", typeof(int));
			dtWagesDetail.Columns.Add("DocumentType");
			dtWagesDetail.Columns.Add("DocDate", typeof(DateTime));
			dtWagesDetail.Columns.Add("DocNo");
			dtWagesDetail.Columns.Add("RefDocDate", typeof(DateTime));
			dtWagesDetail.Columns.Add("RefDocNo");
			dtWagesDetail.Columns.Add("PlantName");
			dtWagesDetail.Columns.Add("ReferenceNo");
			dtWagesDetail.Columns.Add("ReferenceParty");
			dtWagesDetail.Columns.Add("RefPartyAddress");
			dtWagesDetail.Columns.Add("RefPartyCellNo");
			dtWagesDetail.Columns.Add("ItemName");
			dtWagesDetail.Columns.Add("ContractorName");
			dtWagesDetail.Columns.Add("WagesActivity");
			dtWagesDetail.Columns.Add("Qty", typeof(double));
			dtWagesDetail.Columns.Add("AvgRate", typeof(double));
			dtWagesDetail.Columns.Add("Amount", typeof(double));
			dtWagesDetail.Columns.Add("PrcntOfTotal", typeof(double));
			dtWagesDetailWithoutItem.Columns.Add("DocumentTypeId", typeof(int));
			dtWagesDetailWithoutItem.Columns.Add("DocumentType");
			dtWagesDetailWithoutItem.Columns.Add("DocDate", typeof(DateTime));
			dtWagesDetailWithoutItem.Columns.Add("DocNo");
			dtWagesDetailWithoutItem.Columns.Add("RefDocDate", typeof(DateTime));
			dtWagesDetailWithoutItem.Columns.Add("RefDocNo");
			dtWagesDetailWithoutItem.Columns.Add("PlantName");
			dtWagesDetailWithoutItem.Columns.Add("ReferenceNo");
			dtWagesDetailWithoutItem.Columns.Add("ReferenceParty");
			dtWagesDetailWithoutItem.Columns.Add("RefPartyAddress");
			dtWagesDetailWithoutItem.Columns.Add("RefPartyCellNo");
			dtWagesDetailWithoutItem.Columns.Add("ContractorName");
			dtWagesDetailWithoutItem.Columns.Add("WagesActivity");
			dtWagesDetailWithoutItem.Columns.Add("Qty", typeof(double));
			dtWagesDetailWithoutItem.Columns.Add("AvgRate", typeof(double));
			dtWagesDetailWithoutItem.Columns.Add("Amount", typeof(double));
			dtWagesDetailWithoutItem.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByContractor.Columns.Add("ContractorId", typeof(int));
			dtSummaryByContractor.Columns.Add("ContractorName", typeof(string));
			dtSummaryByContractor.Columns.Add("Qty", typeof(decimal));
			dtSummaryByContractor.Columns.Add("AvgRate", typeof(decimal));
			dtSummaryByContractor.Columns.Add("Amount", typeof(decimal));
			dtSummaryByContractor.Columns.Add("PrcntOfTotal", typeof(decimal));
			dtSummaryByItem.Columns.Add("ItemName");
			dtSummaryByItem.Columns.Add("Qty", typeof(double));
			dtSummaryByItem.Columns.Add("AvgRate", typeof(double));
			dtSummaryByItem.Columns.Add("Amount", typeof(double));
			dtSummaryByItem.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByItemandContractor.Columns.Add("ItemName");
			dtSummaryByItemandContractor.Columns.Add("ContractorName");
			dtSummaryByItemandContractor.Columns.Add("Qty", typeof(double));
			dtSummaryByItemandContractor.Columns.Add("AvgRate", typeof(double));
			dtSummaryByItemandContractor.Columns.Add("Amount", typeof(double));
			dtSummaryByItemandContractor.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByItemandServiceActivity.Columns.Add("ItemName");
			dtSummaryByItemandServiceActivity.Columns.Add("WagesActivity");
			dtSummaryByItemandServiceActivity.Columns.Add("Qty", typeof(double));
			dtSummaryByItemandServiceActivity.Columns.Add("AvgRate", typeof(double));
			dtSummaryByItemandServiceActivity.Columns.Add("Amount", typeof(double));
			dtSummaryByItemandServiceActivity.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByContractorandServiceActivity.Columns.Add("ContractorName");
			dtSummaryByContractorandServiceActivity.Columns.Add("WagesActivity");
			dtSummaryByContractorandServiceActivity.Columns.Add("Qty", typeof(double));
			dtSummaryByContractorandServiceActivity.Columns.Add("AvgRate", typeof(double));
			dtSummaryByContractorandServiceActivity.Columns.Add("Amount", typeof(double));
			dtSummaryByContractorandServiceActivity.Columns.Add("PrcntOfTotal", typeof(double));
			dttSummaryByItemContractorandServiceActivity.Columns.Add("ItemName");
			dttSummaryByItemContractorandServiceActivity.Columns.Add("ContractorName");
			dttSummaryByItemContractorandServiceActivity.Columns.Add("WagesActivity");
			dttSummaryByItemContractorandServiceActivity.Columns.Add("Qty", typeof(double));
			dttSummaryByItemContractorandServiceActivity.Columns.Add("AvgRate", typeof(double));
			dttSummaryByItemContractorandServiceActivity.Columns.Add("Amount", typeof(double));
			dttSummaryByItemContractorandServiceActivity.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByWagesGroup.Columns.Add("WagesGroupName");
			dtSummaryByWagesGroup.Columns.Add("Qty", typeof(double));
			dtSummaryByWagesGroup.Columns.Add("AvgRate", typeof(double));
			dtSummaryByWagesGroup.Columns.Add("Amount", typeof(double));
			dtSummaryByWagesGroup.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByWagesGroupandServiceActivity.Columns.Add("WagesGroupName");
			dtSummaryByWagesGroupandServiceActivity.Columns.Add("WagesActivity");
			dtSummaryByWagesGroupandServiceActivity.Columns.Add("Qty", typeof(double));
			dtSummaryByWagesGroupandServiceActivity.Columns.Add("AvgRate", typeof(double));
			dtSummaryByWagesGroupandServiceActivity.Columns.Add("Amount", typeof(double));
			dtSummaryByWagesGroupandServiceActivity.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByWagesGroupandContractor.Columns.Add("WagesGroupName");
			dtSummaryByWagesGroupandContractor.Columns.Add("ContractorName");
			dtSummaryByWagesGroupandContractor.Columns.Add("Qty", typeof(double));
			dtSummaryByWagesGroupandContractor.Columns.Add("AvgRate", typeof(double));
			dtSummaryByWagesGroupandContractor.Columns.Add("Amount", typeof(double));
			dtSummaryByWagesGroupandContractor.Columns.Add("PrcntOfTotal", typeof(double));
			dtSummaryByWagesGroupContractorandServiceActivity.Columns.Add("WagesGroupName");
			dtSummaryByWagesGroupContractorandServiceActivity.Columns.Add("ContractorName");
			dtSummaryByWagesGroupContractorandServiceActivity.Columns.Add("WagesActivity");
			dtSummaryByWagesGroupContractorandServiceActivity.Columns.Add("Qty", typeof(double));
			dtSummaryByWagesGroupContractorandServiceActivity.Columns.Add("AvgRate", typeof(double));
			dtSummaryByWagesGroupContractorandServiceActivity.Columns.Add("Amount", typeof(double));
			dtSummaryByWagesGroupContractorandServiceActivity.Columns.Add("PrcntOfTotal", typeof(double));
			cmbDateType.Focus();
			PlantFeader();
			ReportTypeFill();
			ItemName_ServiceActivity();
			ContractorNameFill();
			DateTypeFill();
			((UltraGridBase)cmbReportType).Rows[0].Activate();
			if (((Control)(object)cmbReportType).Text == Conversion.ToString((object)"Wages Detail"))
			{
				GridBind();
			}
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemName_ServiceActivity()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		try
		{
			DataTable dtServiceActivity = new DataTable();
			dtServiceActivity.Columns.Add("Id");
			dtServiceActivity.Columns.Add("Name");
			DataTable dtItemName = new DataTable();
			dtItemName.Columns.Add("Id");
			dtItemName.Columns.Add("Name");
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtBindCombo = ContractorWagesActivity.GetDataForDropDownFromWagesSchedule(obj);
			if (dtBindCombo.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dtBindCombo.Rows.Count; i++)
			{
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "GroupName")
				{
					dtItemName.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
				}
				if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Service Activity")
				{
					dtServiceActivity.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
					DDL.BindDDLNew(dtServiceActivity, cmbServiceActivity, "Id", "Name", "Service Activity", false);
					((UltraGridBase)cmbServiceActivity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
			}
			if (dtItemName.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemName, cmbItem, "Id", "Name", "Item Name", false);
				((UltraGridBase)cmbItem).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (dtServiceActivity.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtServiceActivity, cmbServiceActivity, "Id", "Name", "Service Activity", false);
				((UltraGridBase)cmbServiceActivity).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReportTypeFill()
	{
		try
		{
			DataTable dtreporttype = new DataTable();
			dtreporttype.Columns.Add("Id");
			dtreporttype.Columns.Add("name");
			dtreporttype.Rows.Add("Wages Detail", "Wages Detail");
			dtreporttype.Rows.Add("Wages Detail Without Item", "Wages Detail Without Item");
			dtreporttype.Rows.Add("Summary By Contractor", "Summary By Contractor");
			dtreporttype.Rows.Add("Summary By Item", "Summary By Item");
			dtreporttype.Rows.Add("Summary By Item and Contractor", "Summary By Item and Contractor");
			dtreporttype.Rows.Add("Summary By Item and Service Activity", "Summary By Item and Service Activity");
			dtreporttype.Rows.Add("Summary By Contractor and Service Activity", "Summary By Contractor and Service Activity");
			dtreporttype.Rows.Add("Summary By Item Contractor and Service Activity", "Summary By Item Contractor and Service Activity");
			dtreporttype.Rows.Add("Summary By Wages Group", "Summary By Wages Group");
			dtreporttype.Rows.Add("Summary By Wages Group and Service Activity", "Summary By Wages Group and Service Activity");
			dtreporttype.Rows.Add("Summary By Wages Group and Contractor", "Summary By Wages Group and Contractor");
			dtreporttype.Rows.Add("Summary By Wages Group Contractor and Service Activity", "Summary By Wages Group Contractor and Service Activity");
			DDL.BindDDLNew(dtreporttype, cmbReportType, "Id", "name", "Report Type", false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PlantFeader()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Expected O, but got Unknown
		try
		{
			int BomId = Conversion.ToInt(Cmbplant.Value);
			DataTable dtProductionplant = new DataTable();
			dtProductionplant = InvProductionPlant.GetAll(new InvProductionPlant
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtProductionplant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionplant, Cmbplant, "Id", "Description", "Plant", true);
				if (BomId > 0)
				{
					if (dtProductionplant.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == BomId))
					{
						Cmbplant.Value = BomId;
					}
					else
					{
						((Control)(object)Cmbplant).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)Cmbplant).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContractorNameFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dt = SupplierCustomer.ReadByOrganizationCompanyIdForContractorWages(new SupplierCustomer
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, cmbContractor, "Id", "CompanyName", "Contractor", false);
				((UltraGridBase)cmbContractor).DisplayLayout.Bands[0].Columns[1].Width = 275;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DateTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbDateType, "Id", "Parameters", "Parameters", ZeroIndex: false);
			((UltraGridBase)cmbDateType).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDateType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbDateType.Value) == 1)
			{
				txtDateFrom.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 2)
			{
				txtDateFrom.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				txtDateFrom.Value = first;
				txtDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				txtDateFrom.Value = first2;
				txtDateTo.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbDateType.Value) == 5)
			{
				txtDateFrom.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
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
			GridBind();
			PrintButtonManage();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridBind()
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
			obj.FromDate = Conversion.ToDateTime((object)txtDateFrom.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtDateTo.Value);
			obj.ToDocNo = Conversion.ToInt((object)txtDocTo.Text);
			obj.FromDocNo = Conversion.ToInt((object)txtFromDoc.Text);
			obj.ItemId = Conversion.ToInt(cmbItem.Value);
			obj.PlantId = Conversion.ToInt(Cmbplant.Value);
			obj.ContractorId = Conversion.ToInt(cmbContractor.Value);
			obj.ContractorWagesActivityId = Conversion.ToInt(cmbServiceActivity.Value);
			obj.ReportType = Conversion.ToString(cmbReportType.Value);
			obj.Activity = Conversion.ToString(cmbReportType.Value);
			if (RadSales.Checked)
			{
				obj.ActionId = 2;
			}
			else if (RadProduction.Checked)
			{
				obj.ActionId = 1;
			}
			dtWagesActivity = ContractorWagesActivity.WagesActivity(obj);
			if (dtWagesActivity.Rows.Count > 0)
			{
				if (Conversion.ToString(cmbReportType.Value) == "Wages Detail")
				{
					dtWagesDetail.Clear();
					for (int i7 = 0; i7 < dtWagesActivity.Rows.Count; i7++)
					{
						dtWagesDetail.Rows.Add(Conversion.ToInt(dtWagesActivity.Rows[i7]["DocumentTypeId"]), Conversion.ToString(dtWagesActivity.Rows[i7]["DocumentType"]), Conversion.ToDateTime(dtWagesActivity.Rows[i7]["DocDate"]).ToShortDateString(), Conversion.ToString(dtWagesActivity.Rows[i7]["DocNo"]), Conversion.ToDateTime(dtWagesActivity.Rows[i7]["RefDocDate"]).ToShortDateString(), Conversion.ToString(dtWagesActivity.Rows[i7]["RefDocNo"]), Conversion.ToString(dtWagesActivity.Rows[i7]["PlantName"]), Conversion.ToString(dtWagesActivity.Rows[i7]["ReferenceNo"]), Conversion.ToString(dtWagesActivity.Rows[i7]["ReferencPartyName"]), Conversion.ToString(dtWagesActivity.Rows[i7]["ReferencPartyAddress"]), Conversion.ToString(dtWagesActivity.Rows[i7]["ReferencPartyCellNo"]), Conversion.ToString(dtWagesActivity.Rows[i7]["ItemName"]), Conversion.ToString(dtWagesActivity.Rows[i7]["ContractorName"]), Conversion.ToString(dtWagesActivity.Rows[i7]["WagesActivity"]), Conversion.ToDouble(dtWagesActivity.Rows[i7]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[i7]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[i7]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[i7]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtWagesDetail";
					grdWagesReport.DataSource = dtWagesDetail;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Wages Detail Without Item")
				{
					dtWagesDetailWithoutItem.Clear();
					for (int i6 = 0; i6 < dtWagesActivity.Rows.Count; i6++)
					{
						dtWagesDetailWithoutItem.Rows.Add(Conversion.ToInt(dtWagesActivity.Rows[i6]["DocumentTypeId"]), Conversion.ToString(dtWagesActivity.Rows[i6]["DocumentType"]), Conversion.ToDateTime(dtWagesActivity.Rows[i6]["DocDate"]).ToShortDateString(), Conversion.ToString(dtWagesActivity.Rows[i6]["DocNo"]), Conversion.ToDateTime(dtWagesActivity.Rows[i6]["RefDocDate"]).ToShortDateString(), Conversion.ToString(dtWagesActivity.Rows[i6]["RefDocNo"]), Conversion.ToString(dtWagesActivity.Rows[i6]["PlantName"]), Conversion.ToString(dtWagesActivity.Rows[i6]["ReferenceNo"]), Conversion.ToString(dtWagesActivity.Rows[i6]["ReferencPartyName"]), Conversion.ToString(dtWagesActivity.Rows[i6]["ReferencPartyAddress"]), Conversion.ToString(dtWagesActivity.Rows[i6]["ReferencPartyCellNo"]), Conversion.ToString(dtWagesActivity.Rows[i6]["ContractorName"]), Conversion.ToString(dtWagesActivity.Rows[i6]["WagesActivity"]), Conversion.ToDouble(dtWagesActivity.Rows[i6]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[i6]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[i6]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[i6]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtWagesDetailWithoutItem";
					grdWagesReport.DataSource = dtWagesDetailWithoutItem;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Contractor")
				{
					dtSummaryByContractor.Clear();
					for (int i5 = 0; i5 < dtWagesActivity.Rows.Count; i5++)
					{
						dtSummaryByContractor.Rows.Add(Conversion.ToInt(dtWagesActivity.Rows[i5]["ContractorId"]), Conversion.ToString(dtWagesActivity.Rows[i5]["ContractorName"]), Conversion.ToDecimal(dtWagesActivity.Rows[i5]["Qty"]), Conversion.ToDecimal(dtWagesActivity.Rows[i5]["AvgRate"]), Conversion.ToDecimal(dtWagesActivity.Rows[i5]["Amount"]), Conversion.ToDecimal(dtWagesActivity.Rows[i5]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByContractor";
					grdWagesReport.DataSource = dtSummaryByContractor;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item")
				{
					dtSummaryByItem.Clear();
					for (int i4 = 0; i4 < dtWagesActivity.Rows.Count; i4++)
					{
						dtSummaryByItem.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[i4]["ItemName"]), Conversion.ToDouble(dtWagesActivity.Rows[i4]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[i4]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[i4]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[i4]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByItem";
					grdWagesReport.DataSource = dtSummaryByItem;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item and Contractor")
				{
					dtSummaryByItemandContractor.Clear();
					for (int i3 = 0; i3 < dtWagesActivity.Rows.Count; i3++)
					{
						dtSummaryByItemandContractor.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[i3]["ItemName"]), Conversion.ToString(dtWagesActivity.Rows[i3]["ContractorName"]), Conversion.ToDouble(dtWagesActivity.Rows[i3]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[i3]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[i3]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[i3]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByItemandContractor";
					grdWagesReport.DataSource = dtSummaryByItemandContractor;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item and Service Activity")
				{
					dtSummaryByItemandServiceActivity.Clear();
					for (int i2 = 0; i2 < dtWagesActivity.Rows.Count; i2++)
					{
						dtSummaryByItemandServiceActivity.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[i2]["ItemName"]), Conversion.ToString(dtWagesActivity.Rows[i2]["WagesActivity"]), Conversion.ToDouble(dtWagesActivity.Rows[i2]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[i2]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[i2]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[i2]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByItemandServiceActivity";
					grdWagesReport.DataSource = dtSummaryByItemandServiceActivity;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Contractor and Service Activity")
				{
					dtSummaryByContractorandServiceActivity.Clear();
					for (int n = 0; n < dtWagesActivity.Rows.Count; n++)
					{
						dtSummaryByContractorandServiceActivity.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[n]["ContractorName"]), Conversion.ToString(dtWagesActivity.Rows[n]["WagesActivity"]), Conversion.ToDouble(dtWagesActivity.Rows[n]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[n]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[n]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[n]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByContractorandServiceActivity";
					grdWagesReport.DataSource = dtSummaryByContractorandServiceActivity;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item Contractor and Service Activity")
				{
					dttSummaryByItemContractorandServiceActivity.Clear();
					for (int m = 0; m < dtWagesActivity.Rows.Count; m++)
					{
						dttSummaryByItemContractorandServiceActivity.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[m]["ItemName"]), Conversion.ToString(dtWagesActivity.Rows[m]["ContractorName"]), Conversion.ToString(dtWagesActivity.Rows[m]["WagesActivity"]), Conversion.ToDouble(dtWagesActivity.Rows[m]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[m]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[m]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[m]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dttSummaryByItemContractorandServiceActivity";
					grdWagesReport.DataSource = dttSummaryByItemContractorandServiceActivity;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group")
				{
					dtSummaryByWagesGroup.Clear();
					for (int l = 0; l < dtWagesActivity.Rows.Count; l++)
					{
						dtSummaryByWagesGroup.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[l]["WagesGroupName"]), Conversion.ToDouble(dtWagesActivity.Rows[l]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[l]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[l]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[l]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByWagesGroup";
					grdWagesReport.DataSource = dtSummaryByWagesGroup;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group and Service Activity")
				{
					dtSummaryByWagesGroupandServiceActivity.Clear();
					for (int k = 0; k < dtWagesActivity.Rows.Count; k++)
					{
						dtSummaryByWagesGroupandServiceActivity.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[k]["WagesGroupName"]), Conversion.ToString(dtWagesActivity.Rows[k]["WagesActivity"]), Conversion.ToDouble(dtWagesActivity.Rows[k]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[k]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[k]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[k]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByWagesGroupandServiceActivity";
					grdWagesReport.DataSource = dtSummaryByWagesGroupandServiceActivity;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group and Contractor")
				{
					dtSummaryByWagesGroupandContractor.Clear();
					for (int j = 0; j < dtWagesActivity.Rows.Count; j++)
					{
						dtSummaryByWagesGroupandContractor.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[j]["WagesGroupName"]), Conversion.ToString(dtWagesActivity.Rows[j]["ContractorName"]), Conversion.ToDouble(dtWagesActivity.Rows[j]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[j]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[j]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[j]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByWagesGroupandContractor";
					grdWagesReport.DataSource = dtSummaryByWagesGroupandContractor;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group Contractor and Service Activity")
				{
					dtSummaryByWagesGroupContractorandServiceActivity.Clear();
					for (int i = 0; i < dtWagesActivity.Rows.Count; i++)
					{
						dtSummaryByWagesGroupContractorandServiceActivity.Rows.Add(Conversion.ToString(dtWagesActivity.Rows[i]["WagesGroupName"]), Conversion.ToString(dtWagesActivity.Rows[i]["ContractorName"]), Conversion.ToString(dtWagesActivity.Rows[i]["WagesActivity"]), Conversion.ToDouble(dtWagesActivity.Rows[i]["Qty"]), Conversion.ToDouble(dtWagesActivity.Rows[i]["AvgRate"]), Conversion.ToDouble(dtWagesActivity.Rows[i]["Amount"]), Conversion.ToDouble(dtWagesActivity.Rows[i]["PrcntOfTotal"]));
					}
					((Control)(object)grdWagesReport).Name = "dtSummaryByWagesGroupContractorandServiceActivity";
					grdWagesReport.DataSource = dtSummaryByWagesGroupContractorandServiceActivity;
					grdWagesReport.RetrieveStructure();
					GridSettings();
				}
			}
			else
			{
				grdWagesReport.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettings()
	{
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (Conversion.ToString(cmbReportType.Value) == "Wages Detail" || Conversion.ToString(cmbReportType.Value) == "Wages Detail Without Item")
			{
				grdWagesReport.RootTable.Columns["DocumentTypeId"].Visible = false;
			}
			if (Conversion.ToString(cmbReportType.Value) == "Summary By Contractor")
			{
				grdWagesReport.RootTable.Columns["ContractorId"].Visible = false;
				grdWagesReport.RootTable.Columns["PrcntOfTotal"].Visible = false;
			}
			if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group Contractor and Service Activity")
			{
				grdWagesReport.GroupByBoxVisible = false;
				grdWagesReport.RootTable.Groups.Add("ContractorName");
				grdWagesReport.RootTable.GroupTotals = (GroupTotals)2;
				grdWagesReport.GroupByBoxVisible = true;
			}
			grdWagesReport.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdWagesReport.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item);
				item.Width = item.Key.Length * 10 + 40;
			}
			((JanusCollectionBase)grdWagesReport.RootTable.Groups).Clear();
			ctrlGrdBar1_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdWagesReport).Name);
			if (fs != null)
			{
				grdWagesReport.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void Reset()
	{
		try
		{
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbServiceActivity).Text = string.Empty;
			((Control)(object)cmbContractor).Text = string.Empty;
			((Control)(object)cmbReportType).Text = string.Empty;
			cmbDateType.Focus();
			DateTypeFill();
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
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ReportTypeFill();
			PlantFeader();
			ItemName_ServiceActivity();
			ContractorNameFill();
			DateTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesReportWithActivities_FormClosing(object sender, FormClosingEventArgs e)
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

	private void PrintButtonManage()
	{
		try
		{
			if (((UltraGridBase)cmbReportType).ActiveRow != null)
			{
				btnPrint.Visible = true;
				if (Conversion.ToString(cmbReportType.Value) == "Wages Detail")
				{
					btnPrint.Text = "1870-WagesDetail";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item")
				{
					btnPrint.Text = "1871-SummaryByItem";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item and Contractor")
				{
					btnPrint.Text = "1872-SummaryByItemandContractor";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item and Service Activity")
				{
					btnPrint.Text = "1873-SummaryByItemandServiceActivity";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Contractor and Service Activity")
				{
					btnPrint.Text = "1874-SummaryByContractorandServiceActivity";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Item Contractor and Service Activity")
				{
					btnPrint.Text = "1875-SummaryByItemContractorandServiceActivity";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group")
				{
					btnPrint.Text = "1876-SummaryByWagesGroup";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group and Service Activity")
				{
					btnPrint.Text = "1877-SummaryByWagesGroupandServiceActivity";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group and Contractor")
				{
					btnPrint.Text = "1878-SummaryByWagesGroupandContractor";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Wages Group Contractor and Service Activity")
				{
					btnPrint.Text = "1879-SummaryByWagesGroupContractorandServiceActivity";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Summary By Contractor")
				{
					btnPrint.Text = "1880-dtSummaryByContractor";
				}
				if (Conversion.ToString(cmbReportType.Value) == "Wages Detail Without Item")
				{
					btnPrint.Text = "1881_WagesDetailWithoutItem";
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

	private void btnPrint_Click(object sender, EventArgs e)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0609: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (btnPrint.Text == "1870-WagesDetail")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val = new Reporting();
				val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val.ShowReportWithDataTable(dtWagesActivity, "1870-WagesDetail.rpt");
			}
			if (btnPrint.Text == "1871-SummaryByItem")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val2 = new Reporting();
				val2.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val2.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val2.ShowReportWithDataTable(dtWagesActivity, "1871-SummaryByItem.rpt");
			}
			if (btnPrint.Text == "1872-SummaryByItemandContractor")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val3 = new Reporting();
				val3.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val3.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val3.ShowReportWithDataTable(dtWagesActivity, "1872-SummaryByItemandContractor.rpt");
			}
			if (btnPrint.Text == "1873-SummaryByItemandServiceActivity")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val4 = new Reporting();
				val4.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val4.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val4.ShowReportWithDataTable(dtWagesActivity, "1873-SummaryByItemandServiceActivity.rpt");
			}
			if (btnPrint.Text == "1874-SummaryByContractorandServiceActivity")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val5 = new Reporting();
				val5.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val5.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val5.ShowReportWithDataTable(dtWagesActivity, "1874-SummaryByContractorandServiceActivity.rpt");
			}
			if (btnPrint.Text == "1875-SummaryByItemContractorandServiceActivity")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val6 = new Reporting();
				val6.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val6.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val6.ShowReportWithDataTable(dtWagesActivity, "1875-SummaryByItemContractorandServiceActivity.rpt");
			}
			if (btnPrint.Text == "1876-SummaryByWagesGroup")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val7 = new Reporting();
				val7.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val7.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val7.ShowReportWithDataTable(dtWagesActivity, "1876-SummaryByWagesGroup.rpt");
			}
			if (btnPrint.Text == "1877-SummaryByWagesGroupandServiceActivity")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val8 = new Reporting();
				val8.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val8.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val8.ShowReportWithDataTable(dtWagesActivity, "1877-SummaryByWagesGroupandServiceActivity.rpt");
			}
			if (btnPrint.Text == "1878-SummaryByWagesGroupandContractor")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val9 = new Reporting();
				val9.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val9.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val9.ShowReportWithDataTable(dtWagesActivity, "1878-SummaryByWagesGroupandContractor.rpt");
			}
			if (btnPrint.Text == "1879-SummaryByWagesGroupContractorandServiceActivity")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val10 = new Reporting();
				val10.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val10.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val10.ShowReportWithDataTable(dtWagesActivity, "1879-SummaryByWagesGroupContractorandServiceActivity.rpt");
			}
			if (btnPrint.Text == "1880-dtSummaryByContractor")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val11 = new Reporting();
				val11.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val11.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val11.ShowReportWithDataTable(dtWagesActivity, "1880-dtSummaryByContractor.rpt");
			}
			if (btnPrint.Text == "1881_WagesDetailWithoutItem")
			{
				if (dtWagesActivity.Rows.Count == 0 || dtWagesActivity == null)
				{
					throw new Exception("Record Not Found For Display");
				}
				Reporting val12 = new Reporting();
				val12.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
				val12.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
				val12.ShowReportWithDataTable(dtWagesActivity, "1881_WagesDetailWithoutItem.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFromDoc_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDocTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void WagesReportWithActivities_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N)
			{
				BtnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if ((e.Alt && e.KeyCode == Keys.D1) || (e.Alt && e.KeyCode == Keys.NumPad1))
			{
				btnPrint_Click(null, null);
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
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Expected O, but got Unknown
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_029a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Expected O, but got Unknown
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Expected O, but got Unknown
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Expected O, but got Unknown
		//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Expected O, but got Unknown
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
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
		Appearance appearance44 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance57 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdWagesReport = new GridEX();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.RadProduction = new System.Windows.Forms.RadioButton();
		this.RadAll = new System.Windows.Forms.RadioButton();
		this.RadSales = new System.Windows.Forms.RadioButton();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbDateType = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbContractor = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.Cmbplant = new UltraCombo();
		this.cmbServiceActivity = new UltraCombo();
		this.txtDocTo = new System.Windows.Forms.TextBox();
		this.txtFromDoc = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.cmbReportType = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDateFrom = new System.Windows.Forms.DateTimePicker();
		this.txtDateTo = new System.Windows.Forms.DateTimePicker();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesReport).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContractor).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbplant).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbServiceActivity).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel5.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdWagesReport);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel5);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 661);
		this.panel1.TabIndex = 0;
		this.grdWagesReport.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdWagesReport).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdWagesReport).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdWagesReport.FilterMode = (FilterMode)2;
		this.grdWagesReport.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdWagesReport).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdWagesReport.GroupTotals = (GroupTotals)2;
		this.grdWagesReport.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdWagesReport).Location = new System.Drawing.Point(0, 199);
		((System.Windows.Forms.Control)(object)this.grdWagesReport).Name = "grdWagesReport";
		((ScrollableBase)this.grdWagesReport).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdWagesReport).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdWagesReport).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdWagesReport).Size = new System.Drawing.Size(964, 462);
		((System.Windows.Forms.Control)(object)this.grdWagesReport).TabIndex = 0;
		this.grdWagesReport.TotalRow = (InheritableBoolean)1;
		this.grdWagesReport.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdWagesReport).VisualStyle = (VisualStyle)4;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 170);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 29);
		this.panel3.TabIndex = 0;
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 56);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(964, 114);
		this.panel2.TabIndex = 111;
		this.groupBox1.Controls.Add(this.RadProduction);
		this.groupBox1.Controls.Add(this.RadAll);
		this.groupBox1.Controls.Add(this.RadSales);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDateType);
		this.groupBox1.Controls.Add(this.btnshow);
		this.groupBox1.Controls.Add(this.label7);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContractor);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.Cmbplant);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbServiceActivity);
		this.groupBox1.Controls.Add(this.txtDocTo);
		this.groupBox1.Controls.Add(this.txtFromDoc);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbReportType);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtDateFrom);
		this.groupBox1.Controls.Add(this.txtDateTo);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(2, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(956, 107);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.RadProduction.AutoSize = true;
		this.RadProduction.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.RadProduction.Location = new System.Drawing.Point(856, 16);
		this.RadProduction.Name = "RadProduction";
		this.RadProduction.Size = new System.Drawing.Size(96, 18);
		this.RadProduction.TabIndex = 10;
		this.RadProduction.Text = "Production";
		this.RadProduction.UseVisualStyleBackColor = true;
		this.RadAll.AutoSize = true;
		this.RadAll.Checked = true;
		this.RadAll.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.RadAll.Location = new System.Drawing.Point(856, 55);
		this.RadAll.Name = "RadAll";
		this.RadAll.Size = new System.Drawing.Size(55, 18);
		this.RadAll.TabIndex = 12;
		this.RadAll.TabStop = true;
		this.RadAll.Text = "Both";
		this.RadAll.UseVisualStyleBackColor = true;
		this.RadSales.AutoSize = true;
		this.RadSales.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.RadSales.Location = new System.Drawing.Point(856, 36);
		this.RadSales.Name = "RadSales";
		this.RadSales.Size = new System.Drawing.Size(61, 18);
		this.RadSales.TabIndex = 11;
		this.RadSales.Text = "Sales";
		this.RadSales.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(8, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(60, 15);
		this.label2.TabIndex = 125;
		this.label2.Text = "Date Type";
		this.cmbDateType.AllowNull = (DefaultableBoolean)1;
		this.cmbDateType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDateType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDateType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbDateType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance35).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance35).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance39).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDateType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDateType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDateType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDateType).Location = new System.Drawing.Point(74, 16);
		((System.Windows.Forms.Control)(object)this.cmbDateType).Name = "cmbDateType";
		((System.Windows.Forms.Control)(object)this.cmbDateType).Size = new System.Drawing.Size(145, 26);
		((System.Windows.Forms.Control)(object)this.cmbDateType).TabIndex = 0;
		this.cmbDateType.ValueChanged += new System.EventHandler(cmbDateType_ValueChanged);
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(856, 76);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 26);
		this.btnshow.TabIndex = 13;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label7.Location = new System.Drawing.Point(222, 80);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(69, 15);
		this.label7.TabIndex = 123;
		this.label7.Text = "Plant Name";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(222, 52);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(88, 15);
		this.label6.TabIndex = 123;
		this.label6.Text = "Service Activity";
		this.cmbContractor.AllowNull = (DefaultableBoolean)1;
		this.cmbContractor.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContractor.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContractor.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbContractor).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContractor).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbContractor).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContractor).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContractor).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbContractor).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContractor).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContractor).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContractor).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbContractor.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContractor).Location = new System.Drawing.Point(551, 46);
		((System.Windows.Forms.Control)(object)this.cmbContractor).Name = "cmbContractor";
		((System.Windows.Forms.Control)(object)this.cmbContractor).Size = new System.Drawing.Size(298, 26);
		((System.Windows.Forms.Control)(object)this.cmbContractor).TabIndex = 8;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(480, 52);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(63, 15);
		this.label5.TabIndex = 121;
		this.label5.Text = "Contractor";
		this.Cmbplant.AllowNull = (DefaultableBoolean)1;
		this.Cmbplant.AutoCompleteMode = (AutoCompleteMode)4;
		this.Cmbplant.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.Cmbplant.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.Cmbplant).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.Cmbplant).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.Cmbplant).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbplant).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.Cmbplant).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.Cmbplant).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.Cmbplant).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.Cmbplant).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance56).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.Cmbplant).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.Cmbplant).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.Cmbplant).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.Cmbplant).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.Cmbplant).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Cmbplant.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.Cmbplant).Location = new System.Drawing.Point(313, 74);
		((System.Windows.Forms.Control)(object)this.Cmbplant).Name = "Cmbplant";
		((System.Windows.Forms.Control)(object)this.Cmbplant).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.Cmbplant).TabIndex = 6;
		this.cmbServiceActivity.AllowNull = (DefaultableBoolean)1;
		this.cmbServiceActivity.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbServiceActivity.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbServiceActivity.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbServiceActivity).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbServiceActivity).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance62;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance65).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance66).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance68).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbServiceActivity).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbServiceActivity).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbServiceActivity.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbServiceActivity).Location = new System.Drawing.Point(313, 46);
		((System.Windows.Forms.Control)(object)this.cmbServiceActivity).Name = "cmbServiceActivity";
		((System.Windows.Forms.Control)(object)this.cmbServiceActivity).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbServiceActivity).TabIndex = 5;
		this.txtDocTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTo.Location = new System.Drawing.Point(394, 18);
		this.txtDocTo.Name = "txtDocTo";
		this.txtDocTo.Size = new System.Drawing.Size(79, 23);
		this.txtDocTo.TabIndex = 4;
		this.txtDocTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocTo_KeyPress);
		this.txtFromDoc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDoc.Location = new System.Drawing.Point(313, 18);
		this.txtFromDoc.Name = "txtFromDoc";
		this.txtFromDoc.Size = new System.Drawing.Size(79, 23);
		this.txtFromDoc.TabIndex = 3;
		this.txtFromDoc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDoc_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(222, 22);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(88, 15);
		this.label3.TabIndex = 116;
		this.label3.Text = "From Doc && To";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(480, 83);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(70, 15);
		this.label11.TabIndex = 115;
		this.label11.Text = "Report Type";
		this.cmbReportType.AllowNull = (DefaultableBoolean)1;
		this.cmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbReportType).Location = new System.Drawing.Point(551, 77);
		((System.Windows.Forms.Control)(object)this.cmbReportType).Name = "cmbReportType";
		((System.Windows.Forms.Control)(object)this.cmbReportType).Size = new System.Drawing.Size(298, 26);
		((System.Windows.Forms.Control)(object)this.cmbReportType).TabIndex = 9;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label9.Location = new System.Drawing.Point(480, 22);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(67, 15);
		this.label9.TabIndex = 114;
		this.label9.Text = "Item Name";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance51;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance52).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance52;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance53).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance53;
		((AppearanceBase)appearance54).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(551, 16);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(298, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 7;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(8, 80);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(48, 15);
		this.label4.TabIndex = 112;
		this.label4.Text = "Date To";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(8, 52);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 15);
		this.label1.TabIndex = 112;
		this.label1.Text = "Date From";
		this.txtDateFrom.CustomFormat = "dd-MMM-yy";
		this.txtDateFrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateFrom.Location = new System.Drawing.Point(74, 48);
		this.txtDateFrom.Name = "txtDateFrom";
		this.txtDateFrom.Size = new System.Drawing.Size(145, 23);
		this.txtDateFrom.TabIndex = 1;
		this.txtDateTo.CustomFormat = "dd-MMM-yy";
		this.txtDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDateTo.Location = new System.Drawing.Point(74, 76);
		this.txtDateTo.Name = "txtDateTo";
		this.txtDateTo.Size = new System.Drawing.Size(145, 23);
		this.txtDateTo.TabIndex = 2;
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label23);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 25);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 31);
		this.panel5.TabIndex = 110;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 6);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(248, 19);
		this.label23.TabIndex = 1;
		this.label23.Text = "Wages Report With Activities";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.btnNew, this.BtnRefresh, this.btnPrint, this.btnshortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(964, 25);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(55, 22);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.BtnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefresh.Name = "BtnRefresh";
		this.BtnRefresh.Size = new System.Drawing.Size(76, 22);
		this.BtnRefresh.Text = "&Refresh";
		this.BtnRefresh.Click += new System.EventHandler(BtnRefresh_Click);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(57, 22);
		this.btnPrint.Text = "Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(122, 22);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(913, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdWagesReport;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 0;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 661);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "WagesReportWithActivities";
		this.Text = "WagesReportWithActivities";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(WagesReportWithActivities_FormClosing);
		base.Load += new System.EventHandler(WagesReportWithActivities_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(WagesReportWithActivities_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdWagesReport).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDateType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbContractor).EndInit();
		((System.ComponentModel.ISupportInitialize)this.Cmbplant).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbServiceActivity).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbReportType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
