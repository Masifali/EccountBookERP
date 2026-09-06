using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Export;

public class LoadContractScheduleForInvoice : Form
{
	public int DocumentTypeId;

	public string ScheduleIds = "";

	private DataTable dtloadExIO = new DataTable();

	public DataTable LoaderData = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label label23;

	private Panel panel2;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private Panel panel3;

	internal GridEX grd;

	private Button btnLoadOnInvoice;

	private Label label2;

	internal UltraCombo CmbPartyName;

	private Button btnReset;

	private Label label3;

	internal UltraCombo cmbItemName;

	private GroupBox groupBox1;

	private Panel panel4;

	internal GridEX grddetail;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	public LoadContractScheduleForInvoice(UserAccount user)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			FromDate.Focus();
			FromDate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			AllComboBind();
			PendingScheduleForLoad();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AllComboBind()
	{
		try
		{
			DataTable dt = ExportContractSchedule.GetDataForDropDownFromSchedule(UserAccount.OrganizationId, UserAccount.CompanyId, (string)null, (string)null);
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtCustomer = new DataTable();
			dtCustomer.Columns.Add("Id", typeof(int));
			dtCustomer.Columns.Add("Name");
			DataTable dtItem = new DataTable();
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("Name");
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (Conversion.ToString(dt.Rows[i]["Activity"]) == "CustomerName")
					{
						dtCustomer.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
					else if (Conversion.ToString(dt.Rows[i]["Activity"]) == "Item")
					{
						dtItem.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["ReferenceName"]);
					}
				}
			}
			if (dtCustomer.Rows.Count > 0)
			{
				DDL.BindDDL(dtCustomer, CmbPartyName, "Id", "Name", "Party Name", ZeroIndex: false);
			}
			else
			{
				((UltraGridBase)CmbPartyName).DataSource = null;
			}
			if (dtItem.Rows.Count > 0)
			{
				DDL.BindDDL(dtItem, cmbItemName, "Id", "Name", "Item Name", ZeroIndex: false);
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

	private void PendingScheduleForLoad()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		try
		{
			dtloadExIO.Rows.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeIds = DocumentTypeId.ToString();
			obj.FromDate = FromDate.Value;
			obj.ToDate = Todate.Value;
			obj.SupplierCustomerId = Conversion.ToInt(CmbPartyName.Value);
			obj.ItemId = Conversion.ToInt(cmbItemName.Value);
			obj.ZeroBalanceType = 1;
			dtloadExIO = ExImInvoice.GetDataFromScheduleForInvoices(obj);
			if (dtloadExIO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ContractDetailId", typeof(int));
				dt.Columns.Add("ScheduleId", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("ContractNo", typeof(string));
				dt.Columns.Add("ContractDate", typeof(DateTime));
				dt.Columns.Add("OrderDate", typeof(DateTime));
				dt.Columns.Add("LoadingDate", typeof(DateTime));
				dt.Columns.Add("ScheduleStatus");
				dt.Columns.Add("ScheduleCode");
				dt.Columns.Add("SupplierCustomerId", typeof(string));
				dt.Columns.Add("PartyName", typeof(string));
				dt.Columns.Add("NoOfContainers", typeof(int));
				dt.Columns.Add("SalesMan", typeof(string));
				dt.Columns.Add("ExporterBank", typeof(string));
				dt.Columns.Add("ShipmentStartDate", typeof(DateTime));
				dt.Columns.Add("DeliveryDays", typeof(int));
				dt.Columns.Add("LastShipmentDate", typeof(DateTime));
				dt.Columns.Add("CustomerApprovedDate", typeof(DateTime));
				dt.Columns.Add("CustomerApprovalRemarks", typeof(string));
				dt.Columns.Add("FCurrencyId", typeof(string));
				dt.Columns.Add("CurrencyCode", typeof(string));
				dt.Columns.Add("ExchangeRate", typeof(double));
				dt.Columns.Add("DeliveryTerm", typeof(string));
				dt.Columns.Add("PaymentTerm", typeof(string));
				dt.Columns.Add("LoadingPort", typeof(string));
				dt.Columns.Add("DestinationPort", typeof(string));
				dt.Columns.Add("InsuranceRemarks", typeof(string));
				dt.Columns.Add("RemarksHeader", typeof(string));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				int i;
				for (i = 0; i < dtloadExIO.Rows.Count; i++)
				{
					if (!dt.AsEnumerable().Any((DataRow dr) => (int)dr["ScheduleId"] == Convert.ToInt32(dtloadExIO.Rows[i]["ScheduleId"])))
					{
						dt.Rows.Add(dtloadExIO.Rows[i]["Id"], dtloadExIO.Rows[i]["ContractDetailId"], dtloadExIO.Rows[i]["ScheduleId"], dtloadExIO.Rows[i]["DocumentTypeId"], dtloadExIO.Rows[i]["DocNo"], dtloadExIO.Rows[i]["ProformaNo"], Conversion.ToDateTime(dtloadExIO.Rows[i]["ProformaDate"]).ToShortDateString(), Conversion.ToDateTime(dtloadExIO.Rows[i]["SalesContratDate"]).ToShortDateString(), Conversion.ToDateTime(dtloadExIO.Rows[i]["LoadingDate"]).ToShortDateString(), Conversion.ToBool(dtloadExIO.Rows[i]["ContractScheduleStatus"]) ? "Exist" : "Not Exist", dtloadExIO.Rows[i]["ScheduleCode"], dtloadExIO.Rows[i]["SupCustId"], dtloadExIO.Rows[i]["CustomerName"], dtloadExIO.Rows[i]["NoOfContainers"], dtloadExIO.Rows[i]["SalesMan"], dtloadExIO.Rows[i]["ExporterBank"], Conversion.ToDateTime(dtloadExIO.Rows[i]["ShipmentStartDate"]).ToShortDateString(), dtloadExIO.Rows[i]["DeliveryDays"], Conversion.ToDateTime(dtloadExIO.Rows[i]["LastShipmentDate"]).ToShortDateString(), Conversion.ToDateTime(dtloadExIO.Rows[i]["CustomerApprovalDate"]).ToShortDateString(), dtloadExIO.Rows[i]["CustomerApprovalRemarks"], dtloadExIO.Rows[i]["FCurrencyId"], dtloadExIO.Rows[i]["CurrencyCode"], dtloadExIO.Rows[i]["ExchangeRate"], dtloadExIO.Rows[i]["DeliveryTerm"], dtloadExIO.Rows[i]["PaymentTerm"], dtloadExIO.Rows[i]["LoadingPort"], dtloadExIO.Rows[i]["DestinationPort"], dtloadExIO.Rows[i]["InsuranceRemarks"], dtloadExIO.Rows[i]["RemarksHeader"], dtloadExIO.Rows[i]["EntryUser"], dtloadExIO.Rows[i]["EntryDate"], dtloadExIO.Rows[i]["ModifyUser"], dtloadExIO.Rows[i]["ModifyDate"], dtloadExIO.Rows[i]["NoOfAttachments"]);
					}
				}
				grd.DataSource = dt;
				grd.RetrieveStructure();
				grdSettings();
			}
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdSettings()
	{
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ContractDetailId"].Visible = false;
			grd.RootTable.Columns["ScheduleId"].Visible = false;
			grd.RootTable.Columns["FCurrencyId"].Visible = false;
			grd.RootTable.Columns["DocumentTypeId"].Visible = false;
			grd.RootTable.Columns["SupplierCustomerId"].Visible = false;
			grd.RootTable.Columns["ScheduleStatus"].Visible = false;
			grd.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grd.RootTable.Columns["DocNo"].Width = Constants.ExportConstants.DocNoConstant;
			grd.RootTable.Columns["ContractNo"].Width = Constants.ExportConstants.ContractNoConstant;
			grd.RootTable.Columns["ContractDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["OrderDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["LoadingDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["ScheduleStatus"].Width = Constants.ExportConstants.StatusConstant;
			grd.RootTable.Columns["ScheduleCode"].Width = Constants.ExportConstants.HsCode;
			grd.RootTable.Columns["PartyName"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["NoOfContainers"].Width = Constants.ExportConstants.NoOfContainers;
			grd.RootTable.Columns["SalesMan"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["ExporterBank"].Width = Constants.ExportConstants.StringConstant;
			grd.RootTable.Columns["ShipmentStartDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["DeliveryDays"].Width = Constants.ExportConstants.DueDays;
			grd.RootTable.Columns["LastShipmentDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["CustomerApprovedDate"].Width = Constants.ExportConstants.DateConstant;
			grd.RootTable.Columns["CustomerApprovalRemarks"].Width = Constants.ExportConstants.RemarksConstants;
			grd.RootTable.Columns["CurrencyCode"].Width = Constants.ExportConstants.FcyConstant;
			grd.RootTable.Columns["ExchangeRate"].Width = Constants.ExportConstants.ExRateConstant;
			grd.RootTable.Columns["DeliveryTerm"].Width = Constants.ExportConstants.TermConstant;
			grd.RootTable.Columns["PaymentTerm"].Width = Constants.ExportConstants.TermConstant;
			grd.RootTable.Columns["LoadingPort"].Width = Constants.ExportConstants.PortConstant;
			grd.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
			grd.RootTable.Columns["InsuranceRemarks"].Width = Constants.ExportConstants.RemarksConstants;
			grd.RootTable.Columns["RemarksHeader"].Width = Constants.ExportConstants.RemarksConstants;
			grd.RootTable.Columns["EntryUser"].Width = Constants.ExportConstants.EntryUserConstants;
			grd.RootTable.Columns["EntryDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["ModifyUser"].Width = Constants.ExportConstants.ModifyUserConstants;
			grd.RootTable.Columns["ModifyDate"].Width = Constants.ExportConstants.DateTimeConstantddMMM;
			grd.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			grd.FrozenColumns = 1;
			grd.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			PendingScheduleForLoad();
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

	private void btnReset_Click(object sender, EventArgs e)
	{
		FromDate.Focus();
		((Control)(object)CmbPartyName).Text = string.Empty;
		PendingScheduleForLoad();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Hide();
	}

	private void btnLoadOnInvoice_Click_1(object sender, EventArgs e)
	{
		try
		{
			string Ids = "";
			GridEXRow[] checkedRows = grd.GetCheckedRows();
			if (checkedRows.Length != 0)
			{
				int supplierCustomerId = 0;
				int ContractId = 0;
				GridEXRow[] array = checkedRows;
				foreach (GridEXRow row in array)
				{
					int contract_Id = Conversion.ToInt(row.Cells["Id"].Value);
					int schedule_Id = Conversion.ToInt(row.Cells["ScheduleId"].Value);
					int currentSupplierCustomerId = Conversion.ToInt(row.Cells["SupplierCustomerId"].Value);
					int currentCurrencyId = Conversion.ToInt(row.Cells["FCurrencyId"].Value);
					if (Conversion.ToString(row.Cells["ScheduleStatus"].Value) == "Not Exist")
					{
						Ids = "";
						MessageBox.Show("Sorry! The selected ' " + Conversion.ToString(row.Cells["ContractNo"].Value) + " ' Contract. Schedule Not Exist. Please Select Contracts Whose Schedule Exists.");
						return;
					}
					if (supplierCustomerId == 0)
					{
						supplierCustomerId = currentSupplierCustomerId;
						ContractId = contract_Id;
					}
					if (supplierCustomerId != currentSupplierCustomerId)
					{
						Ids = "";
						MessageBox.Show("Sorry! The selected Schedule's are not of the same Customer.");
						return;
					}
					if (ContractId != contract_Id)
					{
						Ids = "";
						MessageBox.Show("Sorry! The selected Schedule's are not of the same Contract.");
						return;
					}
					Ids = Ids + "," + row.Cells["ScheduleId"].Value.ToString();
					supplierCustomerId = currentSupplierCustomerId;
				}
				if (Ids != string.Empty)
				{
					string[] idsArray = Ids.Split(',');
					EnumerableRowCollection<DataRow> rows = from r in dtloadExIO.AsEnumerable()
						where idsArray.Contains(r.Field<int>("ScheduleId").ToString())
						select r;
					LoaderData = (rows.Any() ? rows.CopyToDataTable() : dtloadExIO.Clone());
				}
				Hide();
				return;
			}
			throw new Exception("Check the row first");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				DataTable dtDetail = new DataTable();
				dtDetail.Columns.Add("DetailId", typeof(int));
				dtDetail.Columns.Add("ItemId", typeof(int));
				dtDetail.Columns.Add("ItemName", typeof(string));
				dtDetail.Columns.Add("PackingType", typeof(string));
				dtDetail.Columns.Add("CropYear", typeof(string));
				dtDetail.Columns.Add("CommodityDetail", typeof(string));
				dtDetail.Columns.Add("PackUom", typeof(string));
				dtDetail.Columns.Add("MTon", typeof(double));
				dtDetail.Columns.Add("ShipMTon", typeof(double));
				dtDetail.Columns.Add("BalMTon", typeof(double));
				dtDetail.Columns.Add("NoOfBags", typeof(double));
				dtDetail.Columns.Add("ShipBags", typeof(double));
				dtDetail.Columns.Add("BalBags", typeof(double));
				dtDetail.Columns.Add("NetWeight", typeof(double));
				dtDetail.Columns.Add("ShipWeight", typeof(double));
				dtDetail.Columns.Add("BalWeight", typeof(double));
				dtDetail.Columns.Add("ItemRate", typeof(double));
				dtDetail.Columns.Add("Amount", typeof(double));
				dtDetail.Columns.Add("ShipAmount", typeof(double));
				dtDetail.Columns.Add("BalAmount", typeof(double));
				DataTable resultRows = dtloadExIO.Clone();
				resultRows = (from row in dtloadExIO.AsEnumerable()
					where row.Field<int>("ScheduleId") == Conversion.ToInt(item.Cells["ScheduleId"].Value)
					select row).CopyToDataTable();
				for (int i = 0; i < resultRows.Rows.Count; i++)
				{
					dtDetail.Rows.Add(resultRows.Rows[i]["ContractDetailId"], resultRows.Rows[i]["ExImItemId"], resultRows.Rows[i]["ItemName"], resultRows.Rows[i]["PackingType"], resultRows.Rows[i]["CropYear"], resultRows.Rows[i]["CommodityDetail"], resultRows.Rows[i]["PackUom"], resultRows.Rows[i]["M_Ton"], resultRows.Rows[i]["ShipMTon"], resultRows.Rows[i]["BalMTon"], resultRows.Rows[i]["NoOfBags"], resultRows.Rows[i]["ShipBags"], resultRows.Rows[i]["BalNoofBags"], resultRows.Rows[i]["NetWeight"], resultRows.Rows[i]["ShipWeight"], resultRows.Rows[i]["BalWeight"], resultRows.Rows[i]["RatePrice"], resultRows.Rows[i]["Amount"], resultRows.Rows[i]["ShipAmount"], resultRows.Rows[i]["BalShipAmount"]);
				}
				grddetail.DataSource = dtDetail;
				grddetail.RetrieveStructure();
				GridDetailSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridDetailSettings()
	{
		try
		{
			grddetail.RootTable.Columns["ItemId"].Visible = false;
			grddetail.RootTable.Columns["DetailId"].Visible = false;
			grddetail.RootTable.Columns["ShipMTon"].Visible = false;
			grddetail.RootTable.Columns["BalMTon"].Visible = false;
			grddetail.RootTable.Columns["ShipBags"].Visible = false;
			grddetail.RootTable.Columns["BalBags"].Visible = false;
			grddetail.RootTable.Columns["ShipWeight"].Visible = false;
			grddetail.RootTable.Columns["BalWeight"].Visible = false;
			grddetail.RootTable.Columns["ShipAmount"].Visible = false;
			grddetail.RootTable.Columns["BalAmount"].Visible = false;
			grddetail.RootTable.Columns["MTon"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["MTon"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["MTon"].FormatString = "#,##0";
			grddetail.RootTable.Columns["MTon"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["ShipMTon"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["ShipMTon"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["ShipMTon"].FormatString = "#,##0";
			grddetail.RootTable.Columns["ShipMTon"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["BalMTon"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["BalMTon"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["BalMTon"].FormatString = "#,##0";
			grddetail.RootTable.Columns["BalMTon"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["Amount"].FormatString = "#,##0";
			grddetail.RootTable.Columns["Amount"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["ShipAmount"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["ShipAmount"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["ShipAmount"].FormatString = "#,##0";
			grddetail.RootTable.Columns["ShipAmount"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["BalAmount"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["BalAmount"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["BalAmount"].FormatString = "#,##0";
			grddetail.RootTable.Columns["BalAmount"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["NoOfBags"].FormatString = "#,##0";
			grddetail.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["ShipBags"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["ShipBags"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["ShipBags"].FormatString = "#,##0";
			grddetail.RootTable.Columns["ShipBags"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["BalBags"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["BalBags"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["BalBags"].FormatString = "#,##0";
			grddetail.RootTable.Columns["BalBags"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["NetWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["NetWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["NetWeight"].FormatString = "#,##0";
			grddetail.RootTable.Columns["NetWeight"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["ShipWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["ShipWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["ShipWeight"].FormatString = "#,##0";
			grddetail.RootTable.Columns["ShipWeight"].TotalFormatString = "#,##0";
			grddetail.RootTable.Columns["BalWeight"].AggregateFunction = (AggregateFunction)2;
			grddetail.RootTable.Columns["BalWeight"].TextAlignment = (TextAlignment)3;
			grddetail.RootTable.Columns["BalWeight"].FormatString = "#,##0";
			grddetail.RootTable.Columns["BalWeight"].TotalFormatString = "#,##0";
			CommonServices.GridAutoAdjustmentNew(grddetail);
			ctrlGrdBar2_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grddetail).Name);
			if (fs != null)
			{
				grddetail.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grd_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value), DocumentTypeId);
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
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
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
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		GridEXLayout grddetail_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.grd = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbPartyName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.cmbItemName = new UltraCombo();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.btnLoadOnInvoice = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.grddetail = new GridEX();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel2.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetail).BeginInit();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(919, 26);
		this.panel1.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(177, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Schedule For Invoice";
		this.grd.AllowDelete = (InheritableBoolean)1;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Top;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 165);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(919, 270);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.grd.LinkClicked += new ColumnActionEventHandler(grd_LinkClicked);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.groupBox1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 26);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(919, 113);
		this.panel2.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label13);
		this.groupBox1.Controls.Add(this.FromDate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.Todate);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemName);
		this.groupBox1.Controls.Add(this.btngrnlod);
		this.groupBox1.Controls.Add(this.btnReset);
		this.groupBox1.Controls.Add(this.btnLoadOnInvoice);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(564, 107);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(311, 19);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(81, 30);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Loading\r\nDate";
		this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(7, 44);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(69, 30);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Loading \r\nDate";
		this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(90, 21);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(141, 23);
		this.FromDate.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(236, 52);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(67, 15);
		this.label3.TabIndex = 26;
		this.label3.Text = "Item Name";
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(91, 48);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(141, 23);
		this.Todate.TabIndex = 1;
		this.cmbItemName.AllowNull = (DefaultableBoolean)1;
		this.cmbItemName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemName).Location = new System.Drawing.Point(311, 46);
		((System.Windows.Forms.Control)(object)this.cmbItemName).Name = "cmbItemName";
		((System.Windows.Forms.Control)(object)this.cmbItemName).Size = new System.Drawing.Size(246, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemName).TabIndex = 4;
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btngrnlod.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(367, 75);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(65, 24);
		this.btngrnlod.TabIndex = 6;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(500, 75);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(56, 24);
		this.btnReset.TabIndex = 8;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.btnLoadOnInvoice.BackColor = System.Drawing.Color.Teal;
		this.btnLoadOnInvoice.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.btnLoadOnInvoice.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadOnInvoice.ForeColor = System.Drawing.Color.White;
		this.btnLoadOnInvoice.Location = new System.Drawing.Point(434, 75);
		this.btnLoadOnInvoice.Name = "btnLoadOnInvoice";
		this.btnLoadOnInvoice.Size = new System.Drawing.Size(64, 24);
		this.btnLoadOnInvoice.TabIndex = 7;
		this.btnLoadOnInvoice.Text = "Load";
		this.btnLoadOnInvoice.UseVisualStyleBackColor = false;
		this.btnLoadOnInvoice.Click += new System.EventHandler(btnLoadOnInvoice_Click_1);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(235, 25);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(69, 15);
		this.label2.TabIndex = 24;
		this.label2.Text = "Party Name";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 139);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(919, 26);
		this.panel3.TabIndex = 31;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(871, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 23);
		this.ctrlGrdBar1.TabIndex = 65;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 435);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(919, 26);
		this.panel4.TabIndex = 32;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(870, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grddetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 24);
		this.ctrlGrdBar2.TabIndex = 64;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.grddetail.AllowDelete = (InheritableBoolean)1;
		this.grddetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetail).BackColor = System.Drawing.Color.White;
		this.grddetail.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grddetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetail.DesignTimeLayout = grddetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetail.FilterMode = (FilterMode)2;
		this.grddetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grddetail).Location = new System.Drawing.Point(0, 461);
		((System.Windows.Forms.Control)(object)this.grddetail).Name = "grddetail";
		((ScrollableBase)this.grddetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetail).Size = new System.Drawing.Size(919, 200);
		((System.Windows.Forms.Control)(object)this.grddetail).TabIndex = 33;
		this.grddetail.TotalRow = (InheritableBoolean)1;
		this.grddetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetail).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(919, 661);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grddetail);
		base.Controls.Add(this.panel4);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Name = "LoadContractScheduleForInvoice";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel2.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItemName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetail).EndInit();
		base.ResumeLayout(false);
	}
}
