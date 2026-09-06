using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.ContractorWages;
using Architecture.Common;
using Architecture.Model;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Contractor_Wages;

public class frmStockContractorWagesHistory : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtGrid = new DataTable();

	public bool RequestedByOtherDocument;

	public int documentType;

	private bool IsPartyProcessingFeatureOn;

	private bool BranchFeature;

	private DataTable dtBranch = new DataTable();

	private WaitForm WF = new WaitForm();

	private IContainer components;

	private Panel panel1;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton Print;

	private Button btnshow;

	private Label label9;

	internal UltraCombo cmbSupplierCustomer;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label1;

	internal UltraCombo cmbDocumentType;

	private Label label4;

	internal UltraCombo CmbWagesAccount;

	private ToolStripButton btnRefresh;

	private Label label5;

	internal UltraCombo cmbWagesType;

	private Label label6;

	public DateTimePicker txtToDate;

	public DateTimePicker txtFromDate;

	private Panel panel5;

	private Label label7;

	private Label label8;

	internal UltraCombo CmbReportType;

	private RadioButton rdAll;

	private RadioButton rdIsThirdParty;

	private RadioButton rdIsCompany;

	private Label label10;

	internal UltraCombo CmbStockParty;

	private ToolStripButton btnShortCut;

	private Label label2;

	private CheckBox chkBranchWise;

	internal UltraCombo CmbBranchName;

	private ToolStripButton BtnPrintNew;

	private CheckBox chkGroupOnDocType;

	private CheckBox ChkGroupOnContractor;

	private CheckBox chkGroupOnDocDate;

	private CheckBox chkGroupOnwagesActivity;

	private CheckBox chkGroupOnItem;

	public frmStockContractorWagesHistory(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmPendingGrnRpt_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(950, 600);
			WF.Show(this);
			IsPartyProcessingFeatureOn = CommonServices.GetERPFeatureById(8);
			rdIsCompany.Visible = IsPartyProcessingFeatureOn;
			rdIsThirdParty.Visible = IsPartyProcessingFeatureOn;
			rdAll.Visible = IsPartyProcessingFeatureOn;
			BranchFeature = CommonServices.GetERPFeatureById(11);
			chkBranchWise.Visible = BranchFeature;
			BranchesFill();
			WagesTypesFill();
			ReportTypeBind();
			if (RequestedByOtherDocument)
			{
				cmbDocumentType.Value = documentType;
				btnshow_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void BranchesFill()
	{
		try
		{
			((UltraGridBase)CmbBranchName).DataSource = null;
			dtBranch.Clear();
			dtBranch = BranchesAllocationToUser.GetBranchsAllocatedToUserFromWages(UserAccount.OrganizationId, UserAccount.CompanyId, UserAccount.ID, 0);
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDL(dtBranch, CmbBranchName, "BranchId", "BranchName", "Branch Name", ZeroIndex: false);
				UltraGridColumn obj = ((UltraGridBase)CmbBranchName).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbBranchName.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbBranchName.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbBranchName).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbBranchName).Text = UserAccount.BranchName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WagesTypesFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("WagesType");
			dt.Rows.Add(1, "Regular");
			dt.Rows.Add(2, "Free of Cost");
			DDL.BindDDL(dt, cmbWagesType, "Id", "WagesType", "WagesType", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ComboFill()
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Expected O, but got Unknown
		try
		{
			DataTable dtRefDocumentType = new DataTable();
			dtRefDocumentType.Columns.Add("Id");
			dtRefDocumentType.Columns.Add("name");
			DataTable dtWagesAccount = new DataTable();
			dtWagesAccount.Columns.Add("Id");
			dtWagesAccount.Columns.Add("name");
			DataTable dtContractor = new DataTable();
			dtContractor.Columns.Add("Id");
			dtContractor.Columns.Add("name");
			DataTable dtStockParty = new DataTable();
			dtStockParty.Columns.Add("Id");
			dtStockParty.Columns.Add("name");
			new DataTable
			{
				Columns = { "Id", "name" }
			};
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				DataTable dtBindCombo = InvContractorWagesBillHeader.ComboAgainstContractorWages(obj);
				if (dtBindCombo.Rows.Count <= 0)
				{
					return;
				}
				for (int i = 0; i < dtBindCombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "RefDocumentType")
					{
						dtRefDocumentType.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "WagesAccount")
					{
						dtWagesAccount.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "Contractor")
					{
						dtContractor.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtBindCombo.Rows[i]["Activity"]) == "StockParty")
					{
						dtStockParty.Rows.Add(dtBindCombo.Rows[i]["Id"], dtBindCombo.Rows[i]["ReferenceName"]);
					}
				}
				DDL.BindDDL(dtRefDocumentType, cmbDocumentType, "Id", "name", "Document Type", ZeroIndex: false);
				DDL.BindDDL(dtWagesAccount, CmbWagesAccount, "Id", "name", "WagesAccount", ZeroIndex: false);
				DDL.BindDDL(dtContractor, cmbSupplierCustomer, "Id", "name", "Contractor", ZeroIndex: false);
				DDL.BindDDL(dtStockParty, CmbStockParty, "Id", "name", "Stock Party", ZeroIndex: false);
				return;
			}
			CmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ReportTypeBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("name");
			dt.Rows.Add(1, "Detail");
			dt.Rows.Add(2, "Summary");
			DDL.BindDDL(dt, CmbReportType, "Id", "name", "Report Type", ZeroIndex: false);
			((UltraGridBase)CmbReportType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbBranchName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbBranchName).ActiveRow != null)
			{
				ComboFill();
				return;
			}
			((Control)(object)cmbDocumentType).Text = string.Empty;
			((UltraGridBase)cmbDocumentType).DataSource = null;
			((Control)(object)CmbWagesAccount).Text = string.Empty;
			((UltraGridBase)CmbWagesAccount).DataSource = null;
			((Control)(object)cmbSupplierCustomer).Text = string.Empty;
			((UltraGridBase)cmbSupplierCustomer).DataSource = null;
			((Control)(object)CmbStockParty).Text = string.Empty;
			((UltraGridBase)CmbStockParty).DataSource = null;
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
			WF.Show(this);
			GridBind();
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void GridBind()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		try
		{
			BtnPrintNew.Visible = false;
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.DocumentTypeId = 101;
			obj.RefDocumentTypeId = Conversion.ToInt(cmbDocumentType.Value);
			obj.FromDate = Conversion.ToDateTime((object)txtFromDate.Value);
			obj.ToDate = Conversion.ToDateTime((object)txtToDate.Value);
			obj.SupplierCustomerId = Conversion.ToInt(cmbSupplierCustomer.Value);
			obj.AccountId = Conversion.ToInt(CmbWagesAccount.Value);
			obj.StockPartyId = Conversion.ToInt(CmbStockParty.Value);
			obj.SkipZero = (chkBranchWise.Checked ? 1 : 0);
			if (Conversion.ToInt(cmbWagesType.Value) == 1)
			{
				obj.IsApproved = false;
			}
			else if (Conversion.ToInt(cmbWagesType.Value) == 2)
			{
				obj.IsApproved = true;
			}
			else
			{
				obj.ApprovedFilter = "All";
			}
			if (IsPartyProcessingFeatureOn)
			{
				if (rdIsCompany.Checked)
				{
					obj.ActionId = 1;
				}
				else if (rdIsThirdParty.Checked)
				{
					obj.ActionId = 2;
				}
			}
			string BranchIds = "";
			if (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) != string.Empty)
			{
				string[] array = (Conversion.ToString((object)((Control)(object)CmbBranchName).Text) + ",").Split(',');
				foreach (string author in array)
				{
					DataRow[] dr = dtBranch.Select("BranchName='" + Conversion.ToString((object)author) + "'");
					if (dr.Length != 0)
					{
						BranchIds = BranchIds + "," + Conversion.ToString(dr[0].ItemArray[0]);
					}
				}
				obj.BranchesIds = BranchIds;
				if (Conversion.ToInt(CmbReportType.Value) == 1)
				{
					dtGrid = InvContractorWagesBillHeader.ContractorWagesBill_SlipandRegister(obj);
					if (dtGrid.Rows.Count > 0)
					{
						((Control)(object)grdfrm).Name = "WagesDetailGrid";
						Print.Text = "001-Print";
						BtnPrintNew.Visible = true;
						DataTable dt = new DataTable();
						dt.Columns.Add("Id", typeof(int));
						dt.Columns.Add("BranchName", typeof(string));
						dt.Columns.Add("DocumentTypeId", typeof(int));
						dt.Columns.Add("RefDocumentTypeId", typeof(int));
						dt.Columns.Add("D.Type", typeof(string));
						dt.Columns.Add("RefNo", typeof(int));
						dt.Columns.Add("RefDate", typeof(DateTime));
						dt.Columns.Add("DocNo", typeof(int));
						dt.Columns.Add("DocDate", typeof(DateTime));
						dt.Columns.Add("Contractor", typeof(string));
						dt.Columns.Add("WagesActivity", typeof(string));
						dt.Columns.Add("ItemName", typeof(string));
						dt.Columns.Add("CropYear", typeof(string));
						dt.Columns.Add("JobLot", typeof(string));
						dt.Columns.Add("PackingType", typeof(string));
						dt.Columns.Add("Qty", typeof(double));
						dt.Columns.Add("PackSize", typeof(string));
						dt.Columns.Add("WeightCut", typeof(double));
						dt.Columns.Add("BillWeight", typeof(double));
						dt.Columns.Add("WageRate", typeof(double));
						dt.Columns.Add("WagesAmount", typeof(double));
						dt.Columns.Add("WagesTypeId", typeof(int));
						dt.Columns.Add("WagesType", typeof(string));
						dt.Columns.Add("WarehouseFrom", typeof(string));
						dt.Columns.Add("WarehouseTo", typeof(string));
						dt.Columns.Add("EntryUser", typeof(string));
						dt.Columns.Add("EntryDate", typeof(DateTime));
						dt.Columns.Add("ModifyUser", typeof(string));
						dt.Columns.Add("ModifyDate", typeof(DateTime));
						dt.Columns.Add("ApprovedUser", typeof(string));
						dt.Columns.Add("ApprovedDate", typeof(DateTime));
						dt.Columns.Add("OtherRemarks", typeof(string));
						dt.Columns.Add("RemarksDetail", typeof(string));
						for (int i = 0; i < dtGrid.Rows.Count; i++)
						{
							dt.Rows.Add(dtGrid.Rows[i]["Id"], dtGrid.Rows[i]["BranchName"], dtGrid.Rows[i]["DocumentTypeId"], dtGrid.Rows[i]["RefDocumentTypeId"], dtGrid.Rows[i]["DocumentTypeDescription"], dtGrid.Rows[i]["RefDocNo"], Conversion.ToDateTime(dtGrid.Rows[i]["RefDocDate"]).ToShortDateString(), dtGrid.Rows[i]["DocNo"], Conversion.ToDateTime(dtGrid.Rows[i]["DocDate"]).ToShortDateString(), dtGrid.Rows[i]["ContractorName"], dtGrid.Rows[i]["WagesAccountName"], dtGrid.Rows[i]["ItemName"], dtGrid.Rows[i]["Crop"], dtGrid.Rows[i]["JobLotDescription"], dtGrid.Rows[i]["PackTypeDesc"], dtGrid.Rows[i]["Qty"], dtGrid.Rows[i]["PackSize"], dtGrid.Rows[i]["WeightCut"], dtGrid.Rows[i]["BillWeight"], dtGrid.Rows[i]["WageRate"], dtGrid.Rows[i]["WagesAmount"], dtGrid.Rows[i]["WagesTypeId"], dtGrid.Rows[i]["WagesType"], dtGrid.Rows[i]["WareHouseFrom"], dtGrid.Rows[i]["WareHouseNameTo"], dtGrid.Rows[i]["EntryUserName"], dtGrid.Rows[i]["EntryDate"], dtGrid.Rows[i]["ModifyUserName"], dtGrid.Rows[i]["ModifyDate"], dtGrid.Rows[i]["ApprovedUserName"], dtGrid.Rows[i]["ApprovedDate"], dtGrid.Rows[i]["OtherRemarks"], dtGrid.Rows[i]["RemarksDetail"]);
						}
						grdfrm.DataSource = dt;
						grdfrm.RetrieveStructure();
						GridSetting();
					}
					else
					{
						grdfrm.ClearStructure();
					}
				}
				else
				{
					if (Conversion.ToInt(CmbReportType.Value) != 2)
					{
						return;
					}
					dtGrid = InvContractorWagesBillHeader.GetSummaryWagesByRefDocumentsAndActivities(obj);
					if (dtGrid.Rows.Count > 0)
					{
						((Control)(object)grdfrm).Name = "WagesSummaryGrid";
						Print.Text = "003-Print";
						DataTable dt2 = new DataTable();
						dt2.Columns.Add("BranchName");
						dt2.Columns.Add("Description");
						dt2.Columns.Add("WagesType");
						dt2.Columns.Add("WagesAccount");
						dt2.Columns.Add("Qty", typeof(double));
						dt2.Columns.Add("Weight", typeof(double));
						dt2.Columns.Add("AvgRate", typeof(double));
						dt2.Columns.Add("Amount", typeof(double));
						for (int j = 0; j < dtGrid.Rows.Count; j++)
						{
							dt2.Rows.Add(dtGrid.Rows[j]["BranchName"], dtGrid.Rows[j]["Description"], dtGrid.Rows[j]["WagesType"], dtGrid.Rows[j]["WagesAccountName"], dtGrid.Rows[j]["Qty"], dtGrid.Rows[j]["Weight"], dtGrid.Rows[j]["AvgRate"], dtGrid.Rows[j]["Amount"]);
						}
						grdfrm.DataSource = dt2;
						grdfrm.RetrieveStructure();
						GridSettingForSummary();
					}
					else
					{
						grdfrm.ClearStructure();
					}
				}
				return;
			}
			CmbBranchName.Focus();
			throw new Exception("Select Branch First");
		}
		catch (Exception ex)
		{
			WF.Close();
			MessageBox.Show(ex.Message);
		}
		finally
		{
			WF.Close();
		}
	}

	private void ConfigureNumericalColumn(GridEXColumn column)
	{
		if (column.Key.Contains("Amount"))
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.stringFormatsingle;
			column.TotalFormatString = clsGlobalVariables.stringFormatsingle;
		}
		else if (column.Key.Contains("Rate"))
		{
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = clsGlobalVariables.DecimalRateFormate;
		}
		else
		{
			column.AggregateFunction = (AggregateFunction)2;
			column.TextAlignment = (TextAlignment)3;
			column.FormatString = "#,##0.##";
			column.TotalFormatString = "#,##0.##";
		}
	}

	private void GridSetting()
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		try
		{
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdfrm.RootTable.Columns["RefDocumentTypeId"].Visible = false;
			grdfrm.RootTable.Columns["ModifyUser"].Visible = false;
			grdfrm.RootTable.Columns["ModifyDate"].Visible = false;
			grdfrm.RootTable.Columns["ApprovedDate"].Visible = false;
			grdfrm.RootTable.Columns["RefDate"].Visible = false;
			grdfrm.RootTable.Columns["WagesTypeId"].Visible = false;
			grdfrm.RootTable.Columns["BranchName"].Visible = BranchFeature;
			grdfrm.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			if (chkGroupOnDocType.Checked)
			{
				grdfrm.RootTable.Groups.Add("D.Type");
			}
			grdfrm.RootTable.Columns["D.Type"].Visible = false;
			if (chkGroupOnDocDate.Checked)
			{
				grdfrm.RootTable.Groups.Add("DocDate");
			}
			grdfrm.RootTable.Columns["DocDate"].Visible = false;
			if (ChkGroupOnContractor.Checked)
			{
				grdfrm.RootTable.Groups.Add("Contractor");
			}
			grdfrm.RootTable.Columns["Contractor"].Visible = false;
			if (chkGroupOnwagesActivity.Checked)
			{
				grdfrm.RootTable.Groups.Add("WagesActivity");
			}
			grdfrm.RootTable.Columns["WagesActivity"].Visible = false;
			if (chkGroupOnItem.Checked)
			{
				grdfrm.RootTable.Groups.Add("ItemName");
			}
			grdfrm.RootTable.Columns["ItemName"].Visible = false;
			grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			grdfrm.RootTable.Columns["D.Type"].Width = 120;
			grdfrm.RootTable.Columns["RefNo"].Width = 60;
			grdfrm.RootTable.Columns["DocNo"].Width = 60;
			grdfrm.RootTable.Columns["DocDate"].Width = 70;
			grdfrm.RootTable.Columns["Contractor"].Width = 120;
			grdfrm.RootTable.Columns["WagesActivity"].Width = 120;
			grdfrm.RootTable.Columns["ItemName"].Width = 120;
			grdfrm.RootTable.Columns["CropYear"].Width = 80;
			grdfrm.RootTable.Columns["JobLot"].Width = 120;
			grdfrm.RootTable.Columns["PackingType"].Width = 80;
			grdfrm.RootTable.Columns["PackSize"].Width = 70;
			grdfrm.RootTable.Columns["Qty"].Width = 60;
			grdfrm.RootTable.Columns["WeightCut"].Width = 80;
			grdfrm.RootTable.Columns["BillWeight"].Width = 80;
			grdfrm.RootTable.Columns["WageRate"].Width = 80;
			grdfrm.RootTable.Columns["WagesAmount"].Width = 100;
			grdfrm.RootTable.Columns["WagesType"].Width = 80;
			grdfrm.RootTable.Columns["WarehouseFrom"].Width = 110;
			grdfrm.RootTable.Columns["WarehouseTo"].Width = 100;
			grdfrm.RootTable.Columns["BranchName"].Width = 110;
			grdfrm.RootTable.Columns["EntryUser"].Width = 120;
			grdfrm.RootTable.Columns["EntryDate"].Width = 80;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridSettingForSummary()
	{
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		try
		{
			grdfrm.RootTable.Columns["BranchName"].Visible = BranchFeature;
			if (chkGroupOnDocType.Checked)
			{
				grdfrm.RootTable.Groups.Add("Description");
			}
			grdfrm.RootTable.Columns["Description"].Visible = !chkGroupOnDocType.Checked;
			if (chkGroupOnwagesActivity.Checked)
			{
				grdfrm.RootTable.Groups.Add("WagesAccount");
			}
			grdfrm.RootTable.Columns["WagesAccount"].Visible = !chkGroupOnwagesActivity.Checked;
			grdfrm.RootTable.GroupTotals = (GroupTotals)2;
			grdfrm.RootTable.HideColumnsWhenGrouped = (InheritableBoolean)1;
			grdfrm.RootTable.HeaderLines = 3;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				GridEXColumn column = item;
				column.HeaderAlignment = (TextAlignment)2;
				if (column.Type == typeof(double) || column.Type == typeof(decimal))
				{
					ConfigureNumericalColumn(column);
				}
			}
			CommonServices.GridAutoAdjustmentNew(grdfrm);
			ctrlGrdBar1_Load(null, null);
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
			txtFromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			((Control)(object)cmbSupplierCustomer).Text = string.Empty;
			((Control)(object)cmbDocumentType).Text = string.Empty;
			((Control)(object)CmbWagesAccount).Text = string.Empty;
			((Control)(object)CmbBranchName).Text = string.Empty;
			txtFromDate.Focus();
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
			BranchesFill();
			ComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockContractorWagesHistory_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Print_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Expected O, but got Unknown
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting rpt = new Reporting();
			rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			if (Print.Text == "001-Print")
			{
				rpt.ShowReportWithDataTable(dtGrid, "001-ContractorWagesRegister.rpt");
			}
			else
			{
				rpt.ShowReportWithDataTable(dtGrid, "003-tSummaryWagesByRefDocumentsAndActivities.rpt");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnPrintNew_Click(object sender, EventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dtGrid.Rows.Count == 0 || dtGrid == null)
			{
				throw new Exception("Record Not Found For Display");
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			val.ShowReportWithDataTable(dtGrid, "003_A_WagesReportByContractor.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockContractorWagesHistory_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnshow_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				btnRefresh_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnnew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				txtFromDate.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtFromDate.Focus();
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

	private void btnShortCut_Click(object sender, EventArgs e)
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

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+S", "For Show");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+P", "For print");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On From Date in Filter");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbReportType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			chkGroupOnDocType.Visible = true;
			chkGroupOnDocDate.Visible = true;
			ChkGroupOnContractor.Visible = true;
			chkGroupOnwagesActivity.Visible = true;
			chkGroupOnItem.Visible = true;
			if (Conversion.ToInt(CmbReportType.Value) == 2)
			{
				chkGroupOnDocType.Checked = true;
				chkGroupOnDocDate.Visible = false;
				ChkGroupOnContractor.Visible = false;
				chkGroupOnItem.Visible = false;
			}
		}
		catch (Exception ex)
		{
			WF.Close();
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
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0345: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_0371: Expected O, but got Unknown
		//IL_0372: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03be: Expected O, but got Unknown
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdfrm = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel3 = new System.Windows.Forms.Panel();
		this.CmbBranchName = new UltraCombo();
		this.chkGroupOnItem = new System.Windows.Forms.CheckBox();
		this.chkGroupOnwagesActivity = new System.Windows.Forms.CheckBox();
		this.chkGroupOnDocDate = new System.Windows.Forms.CheckBox();
		this.chkGroupOnDocType = new System.Windows.Forms.CheckBox();
		this.ChkGroupOnContractor = new System.Windows.Forms.CheckBox();
		this.chkBranchWise = new System.Windows.Forms.CheckBox();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbReportType = new UltraCombo();
		this.rdAll = new System.Windows.Forms.RadioButton();
		this.rdIsThirdParty = new System.Windows.Forms.RadioButton();
		this.rdIsCompany = new System.Windows.Forms.RadioButton();
		this.label8 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbWagesType = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.CmbWagesAccount = new UltraCombo();
		this.CmbStockParty = new UltraCombo();
		this.cmbDocumentType = new UltraCombo();
		this.txtToDate = new System.Windows.Forms.DateTimePicker();
		this.txtFromDate = new System.Windows.Forms.DateTimePicker();
		this.btnshow = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbSupplierCustomer = new UltraCombo();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.BtnPrintNew = new System.Windows.Forms.ToolStripButton();
		this.btnShortCut = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReportType).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWagesType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierCustomer).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1034, 561);
		this.panel1.TabIndex = 0;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 200);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(1034, 361);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 1;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 178);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1034, 22);
		this.panel4.TabIndex = 2;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(991, 0);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel3.BackColor = System.Drawing.Color.White;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBranchName);
		this.panel3.Controls.Add(this.chkGroupOnItem);
		this.panel3.Controls.Add(this.chkGroupOnwagesActivity);
		this.panel3.Controls.Add(this.chkGroupOnDocDate);
		this.panel3.Controls.Add(this.chkGroupOnDocType);
		this.panel3.Controls.Add(this.ChkGroupOnContractor);
		this.panel3.Controls.Add(this.chkBranchWise);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbReportType);
		this.panel3.Controls.Add(this.rdAll);
		this.panel3.Controls.Add(this.rdIsThirdParty);
		this.panel3.Controls.Add(this.rdIsCompany);
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.label6);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbWagesType);
		this.panel3.Controls.Add(this.label4);
		this.panel3.Controls.Add(this.label10);
		this.panel3.Controls.Add(this.label1);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWagesAccount);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbStockParty);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDocumentType);
		this.panel3.Controls.Add(this.txtToDate);
		this.panel3.Controls.Add(this.txtFromDate);
		this.panel3.Controls.Add(this.btnshow);
		this.panel3.Controls.Add(this.label9);
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 31);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1034, 147);
		this.panel3.TabIndex = 0;
		this.CmbBranchName.AllowNull = (DefaultableBoolean)1;
		this.CmbBranchName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBranchName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBranchName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance64).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance67).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBranchName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Location = new System.Drawing.Point(98, 30);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Name = "CmbBranchName";
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbBranchName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbBranchName).Leave += new System.EventHandler(CmbBranchName_Leave);
		this.chkGroupOnItem.AutoSize = true;
		this.chkGroupOnItem.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGroupOnItem.Location = new System.Drawing.Point(659, 98);
		this.chkGroupOnItem.Name = "chkGroupOnItem";
		this.chkGroupOnItem.Size = new System.Drawing.Size(106, 19);
		this.chkGroupOnItem.TabIndex = 14;
		this.chkGroupOnItem.Text = "Group On Item";
		this.chkGroupOnItem.UseVisualStyleBackColor = true;
		this.chkGroupOnwagesActivity.AutoSize = true;
		this.chkGroupOnwagesActivity.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGroupOnwagesActivity.Location = new System.Drawing.Point(659, 81);
		this.chkGroupOnwagesActivity.Name = "chkGroupOnwagesActivity";
		this.chkGroupOnwagesActivity.Size = new System.Drawing.Size(160, 19);
		this.chkGroupOnwagesActivity.TabIndex = 13;
		this.chkGroupOnwagesActivity.Text = "Group On Wages Activity";
		this.chkGroupOnwagesActivity.UseVisualStyleBackColor = true;
		this.chkGroupOnDocDate.AutoSize = true;
		this.chkGroupOnDocDate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGroupOnDocDate.Location = new System.Drawing.Point(659, 64);
		this.chkGroupOnDocDate.Name = "chkGroupOnDocDate";
		this.chkGroupOnDocDate.Size = new System.Drawing.Size(131, 19);
		this.chkGroupOnDocDate.TabIndex = 12;
		this.chkGroupOnDocDate.Text = "Group On Doc Date";
		this.chkGroupOnDocDate.UseVisualStyleBackColor = true;
		this.chkGroupOnDocType.AutoSize = true;
		this.chkGroupOnDocType.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkGroupOnDocType.Location = new System.Drawing.Point(659, 46);
		this.chkGroupOnDocType.Name = "chkGroupOnDocType";
		this.chkGroupOnDocType.Size = new System.Drawing.Size(131, 19);
		this.chkGroupOnDocType.TabIndex = 11;
		this.chkGroupOnDocType.Text = "Group On Doc Type";
		this.chkGroupOnDocType.UseVisualStyleBackColor = true;
		this.ChkGroupOnContractor.AutoSize = true;
		this.ChkGroupOnContractor.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkGroupOnContractor.Location = new System.Drawing.Point(659, 29);
		this.ChkGroupOnContractor.Name = "ChkGroupOnContractor";
		this.ChkGroupOnContractor.Size = new System.Drawing.Size(137, 19);
		this.ChkGroupOnContractor.TabIndex = 10;
		this.ChkGroupOnContractor.Text = "Group On Contractor";
		this.ChkGroupOnContractor.UseVisualStyleBackColor = true;
		this.chkBranchWise.AutoSize = true;
		this.chkBranchWise.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkBranchWise.Location = new System.Drawing.Point(833, 94);
		this.chkBranchWise.Name = "chkBranchWise";
		this.chkBranchWise.Size = new System.Drawing.Size(92, 19);
		this.chkBranchWise.TabIndex = 18;
		this.chkBranchWise.Text = "Branch Wise";
		this.chkBranchWise.UseVisualStyleBackColor = true;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 64);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(91, 15);
		this.label2.TabIndex = 90;
		this.label2.Text = "From && To Date";
		this.CmbReportType.AllowNull = (DefaultableBoolean)1;
		this.CmbReportType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbReportType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbReportType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance6;
		((UltraGridBase)this.CmbReportType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbReportType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance7;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance8;
		((SpecialBoxBase)((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance9).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance9).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbReportType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbReportType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance13;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance14).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbReportType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbReportType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbReportType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbReportType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbReportType).Location = new System.Drawing.Point(413, 114);
		((System.Windows.Forms.Control)(object)this.CmbReportType).Name = "CmbReportType";
		((System.Windows.Forms.Control)(object)this.CmbReportType).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbReportType).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbReportType).TextChanged += new System.EventHandler(CmbReportType_TextChanged);
		this.rdAll.AutoSize = true;
		this.rdAll.Checked = true;
		this.rdAll.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdAll.Location = new System.Drawing.Point(833, 29);
		this.rdAll.Name = "rdAll";
		this.rdAll.Size = new System.Drawing.Size(39, 19);
		this.rdAll.TabIndex = 15;
		this.rdAll.TabStop = true;
		this.rdAll.Text = "All";
		this.rdAll.UseVisualStyleBackColor = true;
		this.rdIsThirdParty.AutoSize = true;
		this.rdIsThirdParty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdIsThirdParty.Location = new System.Drawing.Point(833, 66);
		this.rdIsThirdParty.Name = "rdIsThirdParty";
		this.rdIsThirdParty.Size = new System.Drawing.Size(95, 19);
		this.rdIsThirdParty.TabIndex = 17;
		this.rdIsThirdParty.Text = "Is Third Party";
		this.rdIsThirdParty.UseVisualStyleBackColor = true;
		this.rdIsCompany.AutoSize = true;
		this.rdIsCompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdIsCompany.Location = new System.Drawing.Point(833, 47);
		this.rdIsCompany.Name = "rdIsCompany";
		this.rdIsCompany.Size = new System.Drawing.Size(88, 19);
		this.rdIsCompany.TabIndex = 16;
		this.rdIsCompany.Text = "Is Company";
		this.rdIsCompany.UseVisualStyleBackColor = true;
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(322, 120);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(70, 15);
		this.label8.TabIndex = 89;
		this.label8.Text = "Report Type";
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1034, 26);
		this.panel5.TabIndex = 87;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(5, 2);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(218, 19);
		this.label7.TabIndex = 11;
		this.label7.Text = "Contractor Wages History";
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(5, 36);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(79, 15);
		this.label6.TabIndex = 86;
		this.label6.Text = "Branch Name";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 120);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 86;
		this.label5.Text = "Wages Type";
		this.cmbWagesType.AllowNull = (DefaultableBoolean)1;
		this.cmbWagesType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbWagesType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbWagesType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbWagesType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbWagesType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance40).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbWagesType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbWagesType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbWagesType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbWagesType).Location = new System.Drawing.Point(98, 114);
		((System.Windows.Forms.Control)(object)this.cmbWagesType).Name = "cmbWagesType";
		((System.Windows.Forms.Control)(object)this.cmbWagesType).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbWagesType).TabIndex = 4;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(322, 34);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(91, 15);
		this.label4.TabIndex = 84;
		this.label4.Text = "Wages Account";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.White;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(322, 92);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(67, 15);
		this.label10.TabIndex = 84;
		this.label10.Text = "Stock Party";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 92);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(92, 15);
		this.label1.TabIndex = 84;
		this.label1.Text = "Document Type";
		this.CmbWagesAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbWagesAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWagesAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWagesAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance11;
		((SpecialBoxBase)((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance66).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWagesAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWagesAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Location = new System.Drawing.Point(413, 28);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Name = "CmbWagesAccount";
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbWagesAccount).TabIndex = 5;
		this.CmbStockParty.AllowNull = (DefaultableBoolean)1;
		this.CmbStockParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbStockParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbStockParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbStockParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbStockParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Location = new System.Drawing.Point(413, 86);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Name = "CmbStockParty";
		((System.Windows.Forms.Control)(object)this.CmbStockParty).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.CmbStockParty).TabIndex = 7;
		this.cmbDocumentType.AllowNull = (DefaultableBoolean)1;
		this.cmbDocumentType.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDocumentType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDocumentType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance48;
		((SpecialBoxBase)((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDocumentType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDocumentType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Location = new System.Drawing.Point(98, 86);
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Name = "cmbDocumentType";
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.cmbDocumentType).TabIndex = 3;
		this.txtToDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtToDate.Location = new System.Drawing.Point(211, 60);
		this.txtToDate.Name = "txtToDate";
		this.txtToDate.Size = new System.Drawing.Size(111, 23);
		this.txtToDate.TabIndex = 2;
		this.txtFromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
		this.txtFromDate.Location = new System.Drawing.Point(98, 60);
		this.txtFromDate.Name = "txtFromDate";
		this.txtFromDate.Size = new System.Drawing.Size(111, 23);
		this.txtFromDate.TabIndex = 1;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(659, 114);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(57, 26);
		this.btnshow.TabIndex = 9;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(322, 62);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(63, 15);
		this.label9.TabIndex = 79;
		this.label9.Text = "Contractor";
		this.cmbSupplierCustomer.AllowNull = (DefaultableBoolean)1;
		this.cmbSupplierCustomer.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSupplierCustomer.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSupplierCustomer.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Appearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance73).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance73).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance74;
		((SpecialBoxBase)((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance76;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSupplierCustomer).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSupplierCustomer.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Location = new System.Drawing.Point(413, 56);
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Name = "cmbSupplierCustomer";
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).Size = new System.Drawing.Size(239, 26);
		((System.Windows.Forms.Control)(object)this.cmbSupplierCustomer).TabIndex = 6;
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1034, 31);
		this.panel2.TabIndex = 0;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.Print, this.BtnPrintNew, this.btnShortCut });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1034, 27);
		this.toolStrip1.TabIndex = 9;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "001-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.BtnPrintNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintNew.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.BtnPrintNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintNew.Name = "BtnPrintNew";
		this.BtnPrintNew.Size = new System.Drawing.Size(108, 24);
		this.BtnPrintNew.Text = "003-A-Print";
		this.BtnPrintNew.Visible = false;
		this.BtnPrintNew.Click += new System.EventHandler(BtnPrintNew_Click);
		this.btnShortCut.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCut.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCut.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCut.Name = "btnShortCut";
		this.btnShortCut.Size = new System.Drawing.Size(126, 24);
		this.btnShortCut.Text = "ShortCut Keys";
		this.btnShortCut.Click += new System.EventHandler(btnShortCut_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1034, 561);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmStockContractorWagesHistory";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockContractorWagesHistory_FormClosing);
		base.Load += new System.EventHandler(frmPendingGrnRpt_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmStockContractorWagesHistory_KeyDown);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBranchName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbReportType).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbWagesType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWagesAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbStockParty).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDocumentType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSupplierCustomer).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
