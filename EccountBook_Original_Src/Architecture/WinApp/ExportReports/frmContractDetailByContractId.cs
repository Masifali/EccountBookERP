using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

namespace Architecture.WinApp.ExportReports;

public class frmContractDetailByContractId : Form
{
	public int ContractId;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtComboData = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private ToolStrip tsItemType;

	private Panel panel5;

	private Label label3;

	private Panel panel1;

	internal Label label2;

	internal Label label5;

	internal Label label1;

	internal Label label8;

	internal Label label9;

	internal Label label10;

	internal Label label4;

	internal Label label6;

	internal Label label7;

	internal Label ValueLastShipmentDate;

	internal Label ValueCustomerName;

	internal Label ValueContractDate;

	internal Label ValueContractNo;

	internal Label ValueFcyAmount;

	internal Label ValueFcyBalance;

	internal Label ValueFcyShipped;

	internal Label ValueContractMtons;

	internal Label ValueBalanceMtons;

	internal Label ValueShippedMtons;

	internal GridEX GridInvoiceInfo;

	private Panel panel2;

	private Panel panel3;

	internal GridEX GridItemInfo;

	private Label label17;

	private Panel panel4;

	private Label label18;

	private Panel panel6;

	private Label label19;

	internal GridEX GridScheduleInformation;

	internal Label ValueFcyCode;

	private Label lblContractNo;

	internal UltraCombo cmbContractNo;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label11;

	internal UltraCombo CmbPartyName;

	public frmContractDetailByContractId(UserAccount userAccount)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmContractDetailByContractId_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1330, 630);
			ComboDbCall();
			PartyNameBind();
			ContractNoBind();
			GetData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ComboDbCall()
	{
		try
		{
			dtComboData.Clear();
			dtComboData = CommonServices.GetLcOrderNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ContractNoBind()
	{
		try
		{
			int SupCustId = Conversion.ToInt(CmbPartyName.Value);
			DataTable dt = (from r in dtComboData.AsEnumerable()
				where SupCustId == 0 || r.Field<int>("SupCustId") == SupCustId
				select r).CopyToDataTable();
			InfragisticsHelper.BindAndRetainSelection(cmbContractNo, dt, "Id", "LcOrderNo", "Contract No", AllColumns: true, Conversion.ToInt(cmbContractNo.Value), insertDefaultRow: false, null, null, ActivateRow: false);
			if (dt != null && dt.Rows.Count > 0)
			{
				((UltraGridBase)cmbContractNo).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				if (ContractId > 0)
				{
					cmbContractNo.Value = ContractId;
					cmbContractNo.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PartyNameBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			HashSet<int> ints = new HashSet<int>();
			foreach (DataRow dr in dtComboData.Rows)
			{
				int Id = Conversion.ToInt(dr["SupCustId"]);
				if (ints.Add(Id))
				{
					dt.Rows.Add(Id, dr["PartyName"]);
				}
			}
			InfragisticsHelper.BindAndRetainSelection(CmbPartyName, dt, "Id", "Name", "PartyName", AllColumns: true, Conversion.ToInt(CmbPartyName.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetData()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a57: Unknown result type (might be due to invalid IL or missing references)
		//IL_134f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataSet ds = ExImLcOrder.ExportContractDetailById(new ReportsParameters
			{
				ContractId = ContractId,
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (ds == null)
			{
				return;
			}
			DataTable dtHeaderInfo = ds.Tables[0];
			ValueContractNo.Tag = Conversion.ToInt(dtHeaderInfo.Rows[0]["Id"]);
			ValueContractNo.Text = Conversion.ToString(dtHeaderInfo.Rows[0]["LcOrderNo"]);
			ValueContractDate.Text = Conversion.ToDateTime(dtHeaderInfo.Rows[0]["LcOrderDate"]).ToShortDateString();
			ValueCustomerName.Text = Conversion.ToString(dtHeaderInfo.Rows[0]["CustomerName"]);
			ValueLastShipmentDate.Text = Conversion.ToDateTime(dtHeaderInfo.Rows[0]["LastShipmentDate"]).ToShortDateString();
			ValueContractMtons.Text = Conversion.ToDouble(dtHeaderInfo.Rows[0]["lcMtons"]).ToString("#,##0.00");
			ValueShippedMtons.Text = Conversion.ToDouble(dtHeaderInfo.Rows[0]["InvoiceMtons"]).ToString("#,##0.00");
			ValueBalanceMtons.Text = (Conversion.ToDouble(dtHeaderInfo.Rows[0]["lcMtons"]) - Conversion.ToDouble(dtHeaderInfo.Rows[0]["InvoiceMtons"])).ToString("#,##0.00");
			ValueFcyCode.Text = Conversion.ToString(dtHeaderInfo.Rows[0]["CurrencyCode"]);
			ValueFcyAmount.Text = Conversion.ToDouble(dtHeaderInfo.Rows[0]["LcFcyAmount"]).ToString("#,##0.00");
			ValueFcyShipped.Text = Conversion.ToDouble(dtHeaderInfo.Rows[0]["InvoiceValue"]).ToString("#,##0.00");
			ValueFcyBalance.Text = (Conversion.ToDouble(dtHeaderInfo.Rows[0]["LcFcyAmount"]) - Conversion.ToDouble(dtHeaderInfo.Rows[0]["InvoiceValue"])).ToString("#,##0.00");
			DataTable dtItemInfo = ds.Tables[1];
			if (dtItemInfo.Rows.Count > 0)
			{
				DataTable dt3 = new DataTable();
				dt3.Columns.Add("ItemId", typeof(int));
				dt3.Columns.Add("ItemName", typeof(string));
				dt3.Columns.Add("TotalMTons", typeof(double));
				dt3.Columns.Add("ShippedMTons", typeof(double));
				dt3.Columns.Add("BalanceMTons", typeof(double));
				for (int k = 0; k < dtItemInfo.Rows.Count; k++)
				{
					dt3.Rows.Add(dtItemInfo.Rows[k]["ExImItemId"], dtItemInfo.Rows[k]["ItemName"], dtItemInfo.Rows[k]["TotalMTon"], dtItemInfo.Rows[k]["ShippedMton"], dtItemInfo.Rows[k]["BalMTon"]);
				}
				GridItemInfo.DataSource = dt3;
				GridItemInfo.RetrieveStructure();
				GridItemInfo.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)GridItemInfo.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				GridItemInfo.RootTable.Columns["ItemId"].Visible = false;
				GridItemInfo.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
				GridItemInfo.RootTable.Columns["TotalMTons"].Width = Constants.ExportConstants.FclConstant;
				GridItemInfo.RootTable.Columns["ShippedMTons"].Width = Constants.ExportConstants.FclConstant;
				GridItemInfo.RootTable.Columns["BalanceMTons"].Width = Constants.ExportConstants.FclConstant;
				GridItemInfo.RootTable.Columns["TotalMTons"].AggregateFunction = (AggregateFunction)2;
				GridItemInfo.RootTable.Columns["TotalMTons"].TextAlignment = (TextAlignment)3;
				GridItemInfo.RootTable.Columns["TotalMTons"].FormatString = "#,##0.00";
				GridItemInfo.RootTable.Columns["TotalMTons"].TotalFormatString = "#,##0.00";
				GridItemInfo.RootTable.Columns["ShippedMTons"].AggregateFunction = (AggregateFunction)2;
				GridItemInfo.RootTable.Columns["ShippedMTons"].TextAlignment = (TextAlignment)3;
				GridItemInfo.RootTable.Columns["ShippedMTons"].FormatString = "#,##0.00";
				GridItemInfo.RootTable.Columns["ShippedMTons"].TotalFormatString = "#,##0.00";
				GridItemInfo.RootTable.Columns["BalanceMTons"].AggregateFunction = (AggregateFunction)2;
				GridItemInfo.RootTable.Columns["BalanceMTons"].TextAlignment = (TextAlignment)3;
				GridItemInfo.RootTable.Columns["BalanceMTons"].FormatString = "#,##0.00";
				GridItemInfo.RootTable.Columns["BalanceMTons"].TotalFormatString = "#,##0.00";
			}
			else
			{
				GridItemInfo.DataSource = null;
			}
			DataTable dtInvoiceInfo = ds.Tables[2];
			if (dtInvoiceInfo.Rows.Count > 0)
			{
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("DocType", typeof(string));
				dt2.Columns.Add("InvoiceId", typeof(int));
				dt2.Columns.Add("InvoiceNo", typeof(string));
				dt2.Columns.Add("InvoiceDate", typeof(DateTime));
				dt2.Columns.Add("BolDate", typeof(DateTime));
				dt2.Columns.Add("CustomerContractNo", typeof(string));
				dt2.Columns.Add("MTons", typeof(double));
				dt2.Columns.Add("PaymentTerm", typeof(string));
				dt2.Columns.Add("DueDate", typeof(DateTime));
				dt2.Columns.Add("ReceivedDate", typeof(DateTime));
				dt2.Columns.Add("Amount", typeof(double));
				dt2.Columns.Add("Received", typeof(double));
				dt2.Columns.Add("Balance", typeof(double));
				for (int j = 0; j < dtInvoiceInfo.Rows.Count; j++)
				{
					dt2.Rows.Add(dtInvoiceInfo.Rows[j]["DocumentTypeCode"], dtInvoiceInfo.Rows[j]["InvoiceId"], dtInvoiceInfo.Rows[j]["InvoiceNo"], dtInvoiceInfo.Rows[j]["InvoiceDate"], dtInvoiceInfo.Rows[j]["BolDate"], dtInvoiceInfo.Rows[j]["CustomerContractNos"], dtInvoiceInfo.Rows[j]["MTons"], dtInvoiceInfo.Rows[j]["PaymentTerm"], dtInvoiceInfo.Rows[j]["DueDate"], dtInvoiceInfo.Rows[j]["ReceivedDate"], dtInvoiceInfo.Rows[j]["Amount"], dtInvoiceInfo.Rows[j]["Received"], dtInvoiceInfo.Rows[j]["BalAmount"]);
				}
				GridInvoiceInfo.DataSource = dt2;
				GridInvoiceInfo.RetrieveStructure();
				GridInvoiceInfo.ColumnAutoResize = false;
				GridInvoiceInfo.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item2 in (JanusCollectionBase)GridInvoiceInfo.RootTable.Columns)
				{
					item2.HeaderAlignment = (TextAlignment)2;
				}
				GridInvoiceInfo.RootTable.Columns["InvoiceId"].Visible = false;
				GridInvoiceInfo.RootTable.Columns["InvoiceDate"].FormatString = "dd-MMM-yy";
				GridInvoiceInfo.RootTable.Columns["BolDate"].FormatString = "dd-MMM-yy";
				GridInvoiceInfo.RootTable.Columns["DueDate"].FormatString = "dd-MMM-yy";
				GridInvoiceInfo.RootTable.Columns["ReceivedDate"].FormatString = "dd-MMM-yy";
				GridInvoiceInfo.RootTable.Columns["CustomerContractNo"].Caption = "Customer ContractNo";
				GridInvoiceInfo.RootTable.Columns["InvoiceNo"].ColumnType = (ColumnType)5;
				GridInvoiceInfo.RootTable.Columns["DocType"].Width = 60;
				GridInvoiceInfo.RootTable.Columns["InvoiceNo"].Width = Constants.ExportConstants.InvoiceNoConstant + 30;
				GridInvoiceInfo.RootTable.Columns["InvoiceDate"].Width = Constants.ExportConstants.DateConstant;
				GridInvoiceInfo.RootTable.Columns["BolDate"].Width = Constants.ExportConstants.DateConstant;
				GridInvoiceInfo.RootTable.Columns["PaymentTerm"].Width = Constants.ExportConstants.TermConstant + 20;
				GridInvoiceInfo.RootTable.Columns["CustomerContractNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
				GridInvoiceInfo.RootTable.Columns["DueDate"].Width = Constants.ExportConstants.DateConstant;
				GridInvoiceInfo.RootTable.Columns["ReceivedDate"].Width = Constants.ExportConstants.DateConstant;
				GridInvoiceInfo.RootTable.Columns["Amount"].Width = Constants.ExportConstants.AmountConstant;
				GridInvoiceInfo.RootTable.Columns["Received"].Width = Constants.ExportConstants.AmountConstant;
				GridInvoiceInfo.RootTable.Columns["Balance"].Width = Constants.ExportConstants.AmountConstant;
				GridInvoiceInfo.RootTable.Columns["MTons"].Width = Constants.ExportConstants.FclConstant;
				GridInvoiceInfo.RootTable.Columns["Amount"].AggregateFunction = (AggregateFunction)2;
				GridInvoiceInfo.RootTable.Columns["Amount"].TextAlignment = (TextAlignment)3;
				GridInvoiceInfo.RootTable.Columns["Amount"].FormatString = "#,##0.00";
				GridInvoiceInfo.RootTable.Columns["Amount"].TotalFormatString = "#,##0.00";
				GridInvoiceInfo.RootTable.Columns["Received"].AggregateFunction = (AggregateFunction)2;
				GridInvoiceInfo.RootTable.Columns["Received"].TextAlignment = (TextAlignment)3;
				GridInvoiceInfo.RootTable.Columns["Received"].FormatString = "#,##0.00";
				GridInvoiceInfo.RootTable.Columns["Received"].TotalFormatString = "#,##0.00";
				GridInvoiceInfo.RootTable.Columns["Balance"].AggregateFunction = (AggregateFunction)2;
				GridInvoiceInfo.RootTable.Columns["Balance"].TextAlignment = (TextAlignment)3;
				GridInvoiceInfo.RootTable.Columns["Balance"].FormatString = "#,##0.00";
				GridInvoiceInfo.RootTable.Columns["Balance"].TotalFormatString = "#,##0.00";
				GridInvoiceInfo.RootTable.Columns["MTons"].AggregateFunction = (AggregateFunction)2;
				GridInvoiceInfo.RootTable.Columns["MTons"].TextAlignment = (TextAlignment)3;
				GridInvoiceInfo.RootTable.Columns["MTons"].FormatString = "#,##0.00";
				GridInvoiceInfo.RootTable.Columns["MTons"].TotalFormatString = "#,##0.00";
				ctrlGrdBar2_Load(null, null);
			}
			else
			{
				GridInvoiceInfo.DataSource = null;
			}
			DataTable dtScheduleInfo = ds.Tables[3];
			if (dtScheduleInfo.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("ContractId", typeof(int));
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocumentTypeId", typeof(int));
				dt.Columns.Add("ScheduleCode", typeof(string));
				dt.Columns.Add("CustomerName", typeof(string));
				dt.Columns.Add("CustomerContractNo", typeof(string));
				dt.Columns.Add("LoadingDate", typeof(DateTime));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("PackSize", typeof(string));
				dt.Columns.Add("NoOfBags", typeof(double));
				dt.Columns.Add("MTon", typeof(double));
				dt.Columns.Add("ShippedMTon", typeof(double));
				dt.Columns.Add("BalanceMTon", typeof(double));
				dt.Columns.Add("NoOfAttachments", typeof(int));
				dt.Columns.Add("DestinationPort", typeof(string));
				for (int i = 0; i < dtScheduleInfo.Rows.Count; i++)
				{
					dt.Rows.Add(dtScheduleInfo.Rows[i]["ContractId"], dtScheduleInfo.Rows[i]["Id"], dtScheduleInfo.Rows[i]["DocumentTypeId"], dtScheduleInfo.Rows[i]["ScheduleCode"], dtScheduleInfo.Rows[i]["CustomerName"], dtScheduleInfo.Rows[i]["CustomerContractNo"], dtScheduleInfo.Rows[i]["LoadingDate"], dtScheduleInfo.Rows[i]["ItemName"], dtScheduleInfo.Rows[i]["PackSize"], dtScheduleInfo.Rows[i]["NoOfBags"], dtScheduleInfo.Rows[i]["MTon"], dtScheduleInfo.Rows[i]["ShippedMTon"], dtScheduleInfo.Rows[i]["BalMton"], dtScheduleInfo.Rows[i]["NoOfAttachments"], dtScheduleInfo.Rows[i]["DestinationPort"]);
				}
				GridScheduleInformation.DataSource = dt;
				GridScheduleInformation.RetrieveStructure();
				GridScheduleInformation.ColumnAutoResize = false;
				GridScheduleInformation.RootTable.HeaderLines = 3;
				foreach (GridEXColumn item3 in (JanusCollectionBase)GridScheduleInformation.RootTable.Columns)
				{
					item3.HeaderAlignment = (TextAlignment)2;
				}
				GridScheduleInformation.RootTable.Groups.Add("CustomerContractNo");
				GridScheduleInformation.RootTable.Columns["Id"].Visible = false;
				GridScheduleInformation.RootTable.Columns["ContractId"].Visible = false;
				GridScheduleInformation.RootTable.Columns["DocumentTypeId"].Visible = false;
				GridScheduleInformation.RootTable.Columns["LoadingDate"].Caption = "Attentive Loading Date";
				GridScheduleInformation.RootTable.Columns["CustomerName"].Caption = "Buyer Reference";
				GridScheduleInformation.RootTable.Columns["CustomerContractNo"].Caption = "Customer ContractNo";
				GridScheduleInformation.RootTable.Columns["NoOfBags"].Caption = "Number Of Bags";
				GridScheduleInformation.RootTable.Columns["DestinationPort"].Caption = "Destination Port";
				GridScheduleInformation.RootTable.Columns["PackSize"].Caption = "Packing Size";
				GridScheduleInformation.RootTable.Columns["LoadingDate"].FormatString = "dd-MMM-yy";
				GridScheduleInformation.RootTable.Columns["CustomerContractNo"].ColumnType = (ColumnType)5;
				GridScheduleInformation.RootTable.Columns["NoOfAttachments"].TextAlignment = (TextAlignment)3;
				GridScheduleInformation.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
				GridScheduleInformation.RootTable.Columns["ScheduleCode"].Width = Constants.ExportConstants.ScheduleCode;
				GridScheduleInformation.RootTable.Columns["CustomerContractNo"].Width = Constants.ExportConstants.InvoiceNoConstant;
				GridScheduleInformation.RootTable.Columns["LoadingDate"].Width = Constants.ExportConstants.DateConstant;
				GridScheduleInformation.RootTable.Columns["CustomerName"].Width = Constants.ExportConstants.StringConstant;
				GridScheduleInformation.RootTable.Columns["ItemName"].Width = Constants.ExportConstants.ItemNameConstants;
				GridScheduleInformation.RootTable.Columns["MTon"].Width = Constants.ExportConstants.FclConstant;
				GridScheduleInformation.RootTable.Columns["NoOfBags"].Width = Constants.ExportConstants.FclConstant;
				GridScheduleInformation.RootTable.Columns["PackSize"].Width = Constants.ExportConstants.PackingTypeConstant;
				GridScheduleInformation.RootTable.Columns["ShippedMTon"].Width = Constants.ExportConstants.FclConstant;
				GridScheduleInformation.RootTable.Columns["BalanceMTon"].Width = Constants.ExportConstants.FclConstant;
				GridScheduleInformation.RootTable.Columns["NoOfAttachments"].Width = Constants.ExportConstants.NoOfAttachmentsConstants;
				GridScheduleInformation.RootTable.Columns["DestinationPort"].Width = Constants.ExportConstants.PortConstant;
				GridScheduleInformation.RootTable.Columns["MTon"].AggregateFunction = (AggregateFunction)2;
				GridScheduleInformation.RootTable.Columns["MTon"].TextAlignment = (TextAlignment)3;
				GridScheduleInformation.RootTable.Columns["MTon"].FormatString = "#,##0.00";
				GridScheduleInformation.RootTable.Columns["MTon"].TotalFormatString = "#,##0.00";
				GridScheduleInformation.RootTable.Columns["NoOfBags"].AggregateFunction = (AggregateFunction)2;
				GridScheduleInformation.RootTable.Columns["NoOfBags"].TextAlignment = (TextAlignment)3;
				GridScheduleInformation.RootTable.Columns["NoOfBags"].FormatString = "#,##0.00";
				GridScheduleInformation.RootTable.Columns["NoOfBags"].TotalFormatString = "#,##0.00";
				GridScheduleInformation.RootTable.Columns["ShippedMTon"].AggregateFunction = (AggregateFunction)2;
				GridScheduleInformation.RootTable.Columns["ShippedMTon"].TextAlignment = (TextAlignment)3;
				GridScheduleInformation.RootTable.Columns["ShippedMTon"].FormatString = "#,##0.00";
				GridScheduleInformation.RootTable.Columns["ShippedMTon"].TotalFormatString = "#,##0.00";
				GridScheduleInformation.RootTable.Columns["BalanceMTon"].AggregateFunction = (AggregateFunction)2;
				GridScheduleInformation.RootTable.Columns["BalanceMTon"].TextAlignment = (TextAlignment)3;
				GridScheduleInformation.RootTable.Columns["BalanceMTon"].FormatString = "#,##0.00";
				GridScheduleInformation.RootTable.Columns["BalanceMTon"].TotalFormatString = "#,##0.00";
				ctrlGrdBar1_Load(null, null);
			}
			else
			{
				GridScheduleInformation.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GridInvoiceInfo_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "InvoiceNo")
			{
				GridEXRow item = GridInvoiceInfo.CurrentRow;
				if (item != null)
				{
					CommonServices.CommercialInvoiceSlip521(Conversion.ToInt(item.Cells["InvoiceId"].Value));
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ValueContractNo_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.SalesContractExportSlip523(Conversion.ToInt(ValueContractNo.Tag));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridScheduleInformation_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GridScheduleInformation.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "CustomerContractNo")
				{
					CommonServices.LcOrderShipmentScheduleLcOrderWise_Slip(Conversion.ToInt(item.Cells["ContractId"].Value));
				}
				if (e.Column.Key == "NoOfAttachments")
				{
					int id = Conversion.ToInt(item.Cells["Id"].Value);
					int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(id, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbContractNo_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbContractNo.Value) > 0 && ((UltraGridBase)cmbContractNo).ActiveRow != null)
			{
				ContractId = Conversion.ToInt(cmbContractNo.Value);
				GetData();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridScheduleInformation).Name);
			if (fs != null)
			{
				GridScheduleInformation.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GridInvoiceInfo).Name);
			if (fs != null)
			{
				GridInvoiceInfo.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPartyName_Leave(object sender, EventArgs e)
	{
		ContractNoBind();
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
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Expected O, but got Unknown
		//IL_18fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1905: Expected O, but got Unknown
		//IL_1eee: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef8: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout GridInvoiceInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridItemInfo_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridScheduleInformation_DesignTimeLayout = new GridEXLayout();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		this.tsItemType = new System.Windows.Forms.ToolStrip();
		this.panel5 = new System.Windows.Forms.Panel();
		this.lblContractNo = new System.Windows.Forms.Label();
		this.cmbContractNo = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ValueFcyCode = new System.Windows.Forms.Label();
		this.ValueFcyAmount = new System.Windows.Forms.Label();
		this.ValueFcyBalance = new System.Windows.Forms.Label();
		this.ValueFcyShipped = new System.Windows.Forms.Label();
		this.ValueContractMtons = new System.Windows.Forms.Label();
		this.ValueBalanceMtons = new System.Windows.Forms.Label();
		this.ValueShippedMtons = new System.Windows.Forms.Label();
		this.ValueLastShipmentDate = new System.Windows.Forms.Label();
		this.ValueCustomerName = new System.Windows.Forms.Label();
		this.ValueContractDate = new System.Windows.Forms.Label();
		this.ValueContractNo = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.GridInvoiceInfo = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.GridItemInfo = new GridEX();
		this.label17 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.GridScheduleInformation = new GridEX();
		this.label19 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.CmbPartyName = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContractNo).BeginInit();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridInvoiceInfo).BeginInit();
		this.panel2.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridItemInfo).BeginInit();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridScheduleInformation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).BeginInit();
		base.SuspendLayout();
		this.tsItemType.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tsItemType.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.tsItemType.Location = new System.Drawing.Point(0, 0);
		this.tsItemType.Name = "tsItemType";
		this.tsItemType.Size = new System.Drawing.Size(1324, 25);
		this.tsItemType.TabIndex = 4;
		this.tsItemType.Text = "toolStrip4";
		this.panel5.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.lblContractNo);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPartyName);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.cmbContractNo);
		this.panel5.Controls.Add(this.label3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 25);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1324, 28);
		this.panel5.TabIndex = 592;
		this.lblContractNo.AutoSize = true;
		this.lblContractNo.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblContractNo.ForeColor = System.Drawing.Color.White;
		this.lblContractNo.Location = new System.Drawing.Point(508, 6);
		this.lblContractNo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.lblContractNo.Name = "lblContractNo";
		this.lblContractNo.Size = new System.Drawing.Size(74, 15);
		this.lblContractNo.TabIndex = 35;
		this.lblContractNo.Text = "Contract No";
		this.cmbContractNo.AllowNull = (DefaultableBoolean)1;
		this.cmbContractNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbContractNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbContractNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbContractNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbContractNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Location = new System.Drawing.Point(588, 0);
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Name = "cmbContractNo";
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.cmbContractNo).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbContractNo).Leave += new System.EventHandler(cmbContractNo_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(3, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(173, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Sale Contract Detail";
		this.panel1.Controls.Add(this.ValueFcyCode);
		this.panel1.Controls.Add(this.ValueFcyAmount);
		this.panel1.Controls.Add(this.ValueFcyBalance);
		this.panel1.Controls.Add(this.ValueFcyShipped);
		this.panel1.Controls.Add(this.ValueContractMtons);
		this.panel1.Controls.Add(this.ValueBalanceMtons);
		this.panel1.Controls.Add(this.ValueShippedMtons);
		this.panel1.Controls.Add(this.ValueLastShipmentDate);
		this.panel1.Controls.Add(this.ValueCustomerName);
		this.panel1.Controls.Add(this.ValueContractDate);
		this.panel1.Controls.Add(this.ValueContractNo);
		this.panel1.Controls.Add(this.label8);
		this.panel1.Controls.Add(this.label9);
		this.panel1.Controls.Add(this.label10);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.label6);
		this.panel1.Controls.Add(this.label7);
		this.panel1.Controls.Add(this.label2);
		this.panel1.Controls.Add(this.label5);
		this.panel1.Controls.Add(this.label1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 53);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1324, 67);
		this.panel1.TabIndex = 593;
		this.ValueFcyCode.AutoSize = true;
		this.ValueFcyCode.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueFcyCode.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueFcyCode.Location = new System.Drawing.Point(751, 6);
		this.ValueFcyCode.Name = "ValueFcyCode";
		this.ValueFcyCode.Size = new System.Drawing.Size(59, 17);
		this.ValueFcyCode.TabIndex = 600;
		this.ValueFcyCode.Text = "FcyCode";
		this.ValueFcyAmount.AutoEllipsis = true;
		this.ValueFcyAmount.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueFcyAmount.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueFcyAmount.Location = new System.Drawing.Point(806, 6);
		this.ValueFcyAmount.Name = "ValueFcyAmount";
		this.ValueFcyAmount.Size = new System.Drawing.Size(109, 17);
		this.ValueFcyAmount.TabIndex = 599;
		this.ValueFcyAmount.Text = "0";
		this.ValueFcyAmount.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.ValueFcyBalance.AutoEllipsis = true;
		this.ValueFcyBalance.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueFcyBalance.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueFcyBalance.Location = new System.Drawing.Point(806, 44);
		this.ValueFcyBalance.Name = "ValueFcyBalance";
		this.ValueFcyBalance.Size = new System.Drawing.Size(109, 17);
		this.ValueFcyBalance.TabIndex = 598;
		this.ValueFcyBalance.Text = "0";
		this.ValueFcyBalance.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.ValueFcyShipped.AutoEllipsis = true;
		this.ValueFcyShipped.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueFcyShipped.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueFcyShipped.Location = new System.Drawing.Point(806, 25);
		this.ValueFcyShipped.Name = "ValueFcyShipped";
		this.ValueFcyShipped.Size = new System.Drawing.Size(109, 17);
		this.ValueFcyShipped.TabIndex = 597;
		this.ValueFcyShipped.Text = "0";
		this.ValueFcyShipped.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.ValueContractMtons.AutoEllipsis = true;
		this.ValueContractMtons.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueContractMtons.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueContractMtons.Location = new System.Drawing.Point(503, 6);
		this.ValueContractMtons.Name = "ValueContractMtons";
		this.ValueContractMtons.Size = new System.Drawing.Size(83, 17);
		this.ValueContractMtons.TabIndex = 596;
		this.ValueContractMtons.Text = "0";
		this.ValueContractMtons.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.ValueBalanceMtons.AutoEllipsis = true;
		this.ValueBalanceMtons.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueBalanceMtons.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueBalanceMtons.Location = new System.Drawing.Point(503, 44);
		this.ValueBalanceMtons.Name = "ValueBalanceMtons";
		this.ValueBalanceMtons.Size = new System.Drawing.Size(83, 17);
		this.ValueBalanceMtons.TabIndex = 595;
		this.ValueBalanceMtons.Text = "0";
		this.ValueBalanceMtons.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.ValueShippedMtons.AutoEllipsis = true;
		this.ValueShippedMtons.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueShippedMtons.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueShippedMtons.Location = new System.Drawing.Point(503, 25);
		this.ValueShippedMtons.Name = "ValueShippedMtons";
		this.ValueShippedMtons.Size = new System.Drawing.Size(83, 17);
		this.ValueShippedMtons.TabIndex = 594;
		this.ValueShippedMtons.Text = "0";
		this.ValueShippedMtons.TextAlign = System.Drawing.ContentAlignment.TopRight;
		this.ValueLastShipmentDate.AutoSize = true;
		this.ValueLastShipmentDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueLastShipmentDate.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueLastShipmentDate.Location = new System.Drawing.Point(139, 44);
		this.ValueLastShipmentDate.Name = "ValueLastShipmentDate";
		this.ValueLastShipmentDate.Size = new System.Drawing.Size(37, 17);
		this.ValueLastShipmentDate.TabIndex = 18;
		this.ValueLastShipmentDate.Text = "Date";
		this.ValueCustomerName.AutoEllipsis = true;
		this.ValueCustomerName.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueCustomerName.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueCustomerName.Location = new System.Drawing.Point(139, 25);
		this.ValueCustomerName.Name = "ValueCustomerName";
		this.ValueCustomerName.Size = new System.Drawing.Size(246, 17);
		this.ValueCustomerName.TabIndex = 17;
		this.ValueCustomerName.Text = "Customer";
		this.ValueContractDate.AutoSize = true;
		this.ValueContractDate.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueContractDate.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.ValueContractDate.Location = new System.Drawing.Point(258, 6);
		this.ValueContractDate.Name = "ValueContractDate";
		this.ValueContractDate.Size = new System.Drawing.Size(37, 17);
		this.ValueContractDate.TabIndex = 16;
		this.ValueContractDate.Text = "Date";
		this.ValueContractNo.AutoSize = true;
		this.ValueContractNo.Cursor = System.Windows.Forms.Cursors.Hand;
		this.ValueContractNo.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ValueContractNo.ForeColor = System.Drawing.Color.Blue;
		this.ValueContractNo.Location = new System.Drawing.Point(139, 6);
		this.ValueContractNo.Name = "ValueContractNo";
		this.ValueContractNo.Size = new System.Drawing.Size(30, 17);
		this.ValueContractNo.TabIndex = 15;
		this.ValueContractNo.Tag = "";
		this.ValueContractNo.Text = "No.";
		this.ValueContractNo.Click += new System.EventHandler(ValueContractNo_Click);
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label8.Location = new System.Drawing.Point(619, 44);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(113, 17);
		this.label8.TabIndex = 14;
		this.label8.Text = "Balance Amount:";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label9.Location = new System.Drawing.Point(619, 25);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(116, 17);
		this.label9.TabIndex = 13;
		this.label9.Text = "Shipped Amount:";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label10.Location = new System.Drawing.Point(619, 6);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(136, 17);
		this.label10.TabIndex = 12;
		this.label10.Text = "Fcy Code && Amount:";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label4.Location = new System.Drawing.Point(392, 44);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(108, 17);
		this.label4.TabIndex = 11;
		this.label4.Text = "Balance M.Tons:";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label6.Location = new System.Drawing.Point(392, 25);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(111, 17);
		this.label6.TabIndex = 10;
		this.label6.Text = "Shipped M.Tons:";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label7.Location = new System.Drawing.Point(392, 6);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(113, 17);
		this.label7.TabIndex = 9;
		this.label7.Text = "Contract M.Tons:";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label2.Location = new System.Drawing.Point(8, 44);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(133, 17);
		this.label2.TabIndex = 8;
		this.label2.Text = "Shipment Last Date:";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label5.Location = new System.Drawing.Point(8, 25);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(111, 17);
		this.label5.TabIndex = 6;
		this.label5.Text = "Customer Name:";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.FromArgb(0, 0, 64);
		this.label1.Location = new System.Drawing.Point(8, 6);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(134, 17);
		this.label1.TabIndex = 4;
		this.label1.Text = "Contract No && Date:";
		this.GridInvoiceInfo.AllowEdit = (InheritableBoolean)2;
		this.GridInvoiceInfo.AlternatingColors = true;
		((ScrollableBase)this.GridInvoiceInfo).BackColor = System.Drawing.Color.White;
		this.GridInvoiceInfo.ColumnAutoResize = true;
		GridInvoiceInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridInvoiceInfo.DesignTimeLayout = GridInvoiceInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridInvoiceInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridInvoiceInfo.DynamicFiltering = true;
		this.GridInvoiceInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridInvoiceInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridInvoiceInfo.GroupByBoxVisible = false;
		this.GridInvoiceInfo.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GridInvoiceInfo).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.GridInvoiceInfo).Name = "GridInvoiceInfo";
		((ScrollableBase)this.GridInvoiceInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridInvoiceInfo).Size = new System.Drawing.Size(806, 242);
		((System.Windows.Forms.Control)(object)this.GridInvoiceInfo).TabIndex = 6;
		this.GridInvoiceInfo.TotalRow = (InheritableBoolean)1;
		this.GridInvoiceInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridInvoiceInfo).VisualStyle = (VisualStyle)4;
		this.GridInvoiceInfo.LinkClicked += new ColumnActionEventHandler(GridInvoiceInfo_LinkClicked);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 120);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1324, 261);
		this.panel2.TabIndex = 594;
		this.panel4.Controls.Add(this.ctrlGrdBar2);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.GridInvoiceInfo);
		this.panel4.Controls.Add(this.label18);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel4.Location = new System.Drawing.Point(518, 0);
		this.panel4.Margin = new System.Windows.Forms.Padding(0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(806, 261);
		this.panel4.TabIndex = 1;
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Dock = System.Windows.Forms.DockStyle.Top;
		this.label18.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(0, 0);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(806, 19);
		this.label18.TabIndex = 1;
		this.label18.Text = "Invoice Information";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.Controls.Add((System.Windows.Forms.Control)(object)this.GridItemInfo);
		this.panel3.Controls.Add(this.label17);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(518, 261);
		this.panel3.TabIndex = 0;
		this.GridItemInfo.AllowEdit = (InheritableBoolean)2;
		this.GridItemInfo.AlternatingColors = true;
		((ScrollableBase)this.GridItemInfo).BackColor = System.Drawing.Color.White;
		this.GridItemInfo.ColumnAutoResize = true;
		GridItemInfo_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridItemInfo.DesignTimeLayout = GridItemInfo_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridItemInfo).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridItemInfo.DynamicFiltering = true;
		this.GridItemInfo.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridItemInfo).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridItemInfo.GroupByBoxVisible = false;
		this.GridItemInfo.GroupTotals = (GroupTotals)3;
		((System.Windows.Forms.Control)(object)this.GridItemInfo).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.GridItemInfo).Name = "GridItemInfo";
		((ScrollableBase)this.GridItemInfo).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridItemInfo).Size = new System.Drawing.Size(518, 242);
		((System.Windows.Forms.Control)(object)this.GridItemInfo).TabIndex = 7;
		this.GridItemInfo.TotalRow = (InheritableBoolean)1;
		this.GridItemInfo.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridItemInfo).VisualStyle = (VisualStyle)4;
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Dock = System.Windows.Forms.DockStyle.Top;
		this.label17.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(0, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(518, 19);
		this.label17.TabIndex = 0;
		this.label17.Text = "Item Information";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.GridScheduleInformation);
		this.panel6.Controls.Add(this.label19);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 381);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1324, 210);
		this.panel6.TabIndex = 595;
		this.GridScheduleInformation.AllowEdit = (InheritableBoolean)2;
		this.GridScheduleInformation.AlternatingColors = true;
		((ScrollableBase)this.GridScheduleInformation).BackColor = System.Drawing.Color.White;
		this.GridScheduleInformation.ColumnAutoResize = true;
		GridScheduleInformation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridScheduleInformation.DesignTimeLayout = GridScheduleInformation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridScheduleInformation).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GridScheduleInformation.DynamicFiltering = true;
		this.GridScheduleInformation.FilterMode = (FilterMode)2;
		this.GridScheduleInformation.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridScheduleInformation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridScheduleInformation.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridScheduleInformation).Location = new System.Drawing.Point(0, 19);
		((System.Windows.Forms.Control)(object)this.GridScheduleInformation).Name = "GridScheduleInformation";
		((ScrollableBase)this.GridScheduleInformation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridScheduleInformation).Size = new System.Drawing.Size(1324, 191);
		((System.Windows.Forms.Control)(object)this.GridScheduleInformation).TabIndex = 7;
		this.GridScheduleInformation.TotalRow = (InheritableBoolean)1;
		this.GridScheduleInformation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridScheduleInformation).VisualStyle = (VisualStyle)4;
		this.GridScheduleInformation.LinkClicked += new ColumnActionEventHandler(GridScheduleInformation_LinkClicked);
		this.label19.BackColor = System.Drawing.Color.Teal;
		this.label19.Dock = System.Windows.Forms.DockStyle.Top;
		this.label19.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.White;
		this.label19.Location = new System.Drawing.Point(0, 0);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(1324, 19);
		this.label19.TabIndex = 2;
		this.label19.Text = "Schedule Information";
		this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1284, -2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.GridScheduleInformation;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 8;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(764, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GridInvoiceInfo;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 9;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.CmbPartyName.AllowNull = (DefaultableBoolean)1;
		this.CmbPartyName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPartyName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPartyName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance13).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance13).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance18).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPartyName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPartyName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Location = new System.Drawing.Point(256, 0);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Name = "CmbPartyName";
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Size = new System.Drawing.Size(247, 26);
		((System.Windows.Forms.Control)(object)this.CmbPartyName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbPartyName).Leave += new System.EventHandler(CmbPartyName_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(176, 6);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(72, 15);
		this.label11.TabIndex = 35;
		this.label11.Text = "Party Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1324, 591);
		base.Controls.Add(this.panel6);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel1);
		base.Controls.Add(this.panel5);
		base.Controls.Add(this.tsItemType);
		base.Name = "frmContractDetailByContractId";
		this.Text = "Contract Detail";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmContractDetailByContractId_Load);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbContractNo).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridInvoiceInfo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridItemInfo).EndInit();
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridScheduleInformation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPartyName).EndInit();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
