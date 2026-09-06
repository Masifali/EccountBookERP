// using Architecture.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Accounts;
using Architecture.BLL.Reports.Accounts;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
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
namespace Architecture.WinApp.Account_Definition
{

public class InLandFreightAgreement : Form
{
	private UserAccount userAccount = new UserAccount();

	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private int RecId;

	private int updateRowIndex;

	private List<InvInLandFreightAgreementDetail> lstRemoveRecord = new List<InvInLandFreightAgreementDetail>();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel1;

	private TabPage tabPage2;

	private Panel panel2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnAttachment;

	private Panel panel3;

	private Label label18;

	private GroupBox groupBox1;

	private DateTimePicker txtDocDate;

	private Label label3;

	private Label label9;

	private TextBox TxtDocNo;

	private GroupBox groupBox2;

	private Label label2;

	internal UltraCombo cmbDistrict;

	private Label label1;

	internal UltraCombo CmbTransporter;

	private Label label8;

	internal UltraCombo CmbParty;

	private Label label14;

	private TextBox txtMinAmount;

	private Label label13;

	private TextBox txtFreightAmount;

	private Label label12;

	private TextBox txtPackSize;

	private Label lblUom;

	private TextBox txtFreightUom;

	private Label label10;

	private Label label7;

	private TextBox txtFreightRate;

	internal UltraCombo CmbFreightType;

	private DateTimePicker txtEffectiveDate;

	private Label label6;

	private Label label5;

	internal UltraCombo cmbUnloadingLocation;

	private Label label4;

	internal UltraCombo cmbLoadingLocation;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Button btnAddDetail;

	private Panel panel4;

	internal GridEX grdDetail;

	private TextBox txtDetailRemarks;

	private Label label28;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label11;

	private TextBox txtAgreementCode;

	private Panel panel5;

	private Panel panel6;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private ToolStripButton btnGrnFormHistory;

	private Panel panel7;

	internal GridEX GrdHistory;

	internal GridEX GrdDetailHistory;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton toolStripButton1;

	public InLandFreightAgreement(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		userAccount = pUser;
	}

	private void InLandFreightAgreement_Load(object sender, EventArgs e)
	{
		try
		{
			AccountsFill();
			DistrictFill();
			CityFill();
			FreightTypeFill();
			GenerateCode();
			dtViewAttachments.Columns.Add("AttchmentPath");
			table.Columns.Add("Id");
			table.Columns.Add("DistrictId");
			table.Columns.Add("DistrictName");
			table.Columns.Add("LoadingLocationId");
			table.Columns.Add("LoadingLocationName");
			table.Columns.Add("UnLoadingLocationId");
			table.Columns.Add("UnLoadingLocationName");
			table.Columns.Add("EffectiveDate");
			table.Columns.Add("FreightTypeId");
			table.Columns.Add("FreightType");
			table.Columns.Add("FreightRate", typeof(double));
			table.Columns.Add("FreightUom", typeof(int));
			table.Columns.Add("PackSizeTo", typeof(int));
			table.Columns.Add("FreightAmount", typeof(double));
			table.Columns.Add("MinAmount", typeof(double));
			table.Columns.Add("RemarksDetail");
			TxtDocNo.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			grdSettings();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
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
			DataTable SuppCust = CommonServices.SupplierCustomerGetAllServiceBind();
			DataTable accounts = new DataTable();
			if (SuppCust.Rows.Count > 0)
			{
				if (accounts.Columns.Count == 0)
				{
					accounts.Columns.Add("Id");
					accounts.Columns.Add("CompanyName");
				}
				accounts.Rows.Clear();
				for (int i = 0; i < SuppCust.Rows.Count; i++)
				{
					accounts.Rows.Add(SuppCust.Rows[i]["Id"], SuppCust.Rows[i]["CompanyName"]);
				}
				if (accounts.Rows.Count > 0)
				{
					DDL.BindDDL(accounts, CmbParty, "Id", "CompanyName", "Customer", ZeroIndex: false);
					DDL.BindDDL(accounts, CmbTransporter, "Id", "CompanyName", "Transporter A/c", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DistrictFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			DataTable dtDistrict = District.GetAll(new District
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId
			});
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("District");
			if (dtDistrict.Rows.Count > 0)
			{
				for (int i = 0; i < dtDistrict.Rows.Count; i++)
				{
					dt.Rows.Add(dtDistrict.Rows[i]["Id"], dtDistrict.Rows[i]["District"]);
				}
			}
			DDL.BindDDL(dt, cmbDistrict, "Id", "District", "District", ZeroIndex: false);
			((UltraGridBase)cmbDistrict).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateCode()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		try
		{
			int Code = InvInLandFreightAgreement.GenerateCode(new InvInLandFreightAgreement
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 161
			});
			TxtDocNo.Text = Conversion.ToString((object)Code);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CityFill()
	{
		try
		{
			DataTable dtCity = CommonServices.CityGetAllService();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("CityName");
			if (dtCity.Rows.Count > 0)
			{
				for (int i = 0; i < dtCity.Rows.Count; i++)
				{
					dt.Rows.Add(dtCity.Rows[i]["Id"], dtCity.Rows[i]["CityName"]);
				}
			}
			DDL.BindDDL(dt, cmbLoadingLocation, "Id", "CityName", "Loading Location", ZeroIndex: false);
			DDL.BindDDL(dt, cmbUnloadingLocation, "Id", "CityName", "UnLoading Location", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FreightTypeFill()
	{
		try
		{
			DataTable dt = CommonServices.StaticColumnsService("FreightType");
			DDL.BindDDLNew(dt, CmbFreightType, "Id", "Type", "Freight Type", false);
			((UltraGridBase)CmbFreightType).Rows[2].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbFreightType_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (((Control)(object)CmbFreightType).Text == "Lump Sum")
			{
				txtFreightUom.ReadOnly = true;
				txtFreightUom.Text = "1";
				lblUom.Text = "Freight Uom";
			}
			else if (((Control)(object)CmbFreightType).Text == "By Weight")
			{
				lblUom.Text = "Freight Uom Kg";
				txtFreightUom.ReadOnly = false;
				txtFreightUom.Text = "";
			}
			else if (((Control)(object)CmbFreightType).Text == "By Qty")
			{
				lblUom.Text = "No. Of Bags";
				txtFreightUom.ReadOnly = false;
				txtFreightUom.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightRate_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreightRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDecimal((object)txtFreightRate.Text) > 0m)
			{
				CalculateAmount();
			}
			else
			{
				txtFreightAmount.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightUom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreightUom_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDecimal((object)txtFreightRate.Text) > 0m)
			{
				CalculateAmount();
			}
			else
			{
				CalculateRate();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateAmount()
	{
		try
		{
			double rate = Conversion.ToDouble((object)txtFreightRate.Text);
			int uom = Conversion.ToInt((object)txtFreightUom.Text);
			if (rate > 0.0 && uom > 0)
			{
				txtFreightAmount.Text = Conversion.ToString((object)(rate * (double)uom));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CalculateRate()
	{
		try
		{
			double Amount = Conversion.ToDouble((object)txtFreightAmount.Text);
			int uom = Conversion.ToInt((object)txtFreightUom.Text);
			if (Amount > 0.0 && uom > 0)
			{
				txtFreightRate.Text = Conversion.ToString((object)(Amount / (double)uom));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (TxtDocNo.Text.Trim() == "" || TxtDocNo.Text.Trim() == "0" || TxtDocNo.Text.Trim() == null)
		{
			MessageBox.Show("DocNo Field is Required");
			TxtDocNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbParty).ActiveRow == null)
		{
			MessageBox.Show("Customer Field is Required");
			CmbParty.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTransporter).ActiveRow == null)
		{
			MessageBox.Show("Transporter Field is Required");
			CmbTransporter.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)cmbDistrict).ActiveRow == null)
		{
			MessageBox.Show("District Field is Required");
			cmbDistrict.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLoadingLocation).ActiveRow == null)
		{
			MessageBox.Show("Loading Location Field is Required");
			cmbLoadingLocation.Focus();
			return false;
		}
		if (((UltraGridBase)cmbUnloadingLocation).ActiveRow == null)
		{
			MessageBox.Show("Unloading Location Field is Required");
			cmbUnloadingLocation.Focus();
			return false;
		}
		if (((UltraGridBase)CmbFreightType).ActiveRow == null)
		{
			MessageBox.Show("Freight Type Field is Required");
			CmbFreightType.Focus();
			return false;
		}
		if (Conversion.ToDecimal((object)txtFreightRate.Text.Trim()) == 0m)
		{
			MessageBox.Show("Freight Rate Field is Required");
			txtFreightRate.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightUom.Text.Trim()) == 0.0)
		{
			MessageBox.Show("freight Uom Field is Required");
			txtFreightUom.Focus();
			return false;
		}
		if (((Control)(object)CmbFreightType).Text == "By Qty" && Conversion.ToDouble((object)txtPackSize.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Pack Size Field is Required");
			txtPackSize.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFreightAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Freight Amount Field is Required");
			txtFreightAmount.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtMinAmount.Text.Trim()) == 0.0)
		{
			MessageBox.Show("Min Amount Field is Required");
			txtMinAmount.Focus();
			return false;
		}
		return true;
	}

	private void btnAddDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (!FormValidationDetail())
			{
				return;
			}
			double freightAmount = Conversion.ToDouble((object)txtFreightAmount.Text);
			if (Conversion.ToDouble((object)txtMinAmount.Text) > freightAmount)
			{
				MessageBox.Show("Min Amount Can't be Greater Than Freight Amount");
				txtMinAmount.Focus();
				return;
			}
			if (table.Rows.Count > 0)
			{
				DateTime EffectiveDate = Conversion.ToDateTime(table.Rows[table.Rows.Count - 1]["EffectiveDate"]);
				if (txtEffectiveDate.Value <= EffectiveDate)
				{
					MessageBox.Show("Can't Add Contract Against this Selected Effective Date!");
					return;
				}
			}
			table.Rows.Add(0, cmbDistrict.Value, ((Control)(object)cmbDistrict).Text, cmbLoadingLocation.Value, ((Control)(object)cmbLoadingLocation).Text.Trim(), cmbUnloadingLocation.Value, ((Control)(object)cmbUnloadingLocation).Text.Trim(), txtEffectiveDate.Value, CmbFreightType.Value, ((Control)(object)CmbFreightType).Text.Trim(), Conversion.ToDouble((object)txtFreightRate.Text.Trim()), Conversion.ToInt((object)txtFreightUom.Text.Trim()), Conversion.ToInt((object)txtPackSize.Text.Trim()), Conversion.ToDouble((object)txtFreightAmount.Text.Trim()), Conversion.ToDouble((object)txtMinAmount.Text.Trim()), txtDetailRemarks.Text.Trim());
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			grdSettings();
			DetailFormReset();
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
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Key = "Delete";
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["DistrictId"].Visible = false;
			grdDetail.RootTable.Columns["LoadingLocationId"].Visible = false;
			grdDetail.RootTable.Columns["UnLoadingLocationId"].Visible = false;
			grdDetail.RootTable.Columns["FreightTypeId"].Visible = false;
			grdDetail.RootTable.Columns["DistrictName"].Width = 150;
			grdDetail.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["MinAmount"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["FreightAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["MinAmount"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["FreightAmount"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["MinAmount"].FormatString = "#,##.##";
			grdDetail.RootTable.Columns["FreightAmount"].TotalFormatString = "#,##.##";
			grdDetail.RootTable.Columns["MinAmount"].TotalFormatString = "#,##.##";
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

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateRowIndex = item.RowIndex;
				cmbDistrict.Value = item.Cells["DistrictId"].Value.ToString();
				cmbLoadingLocation.Value = item.Cells["LoadingLocationId"].Text.ToString();
				cmbUnloadingLocation.Value = item.Cells["UnLoadingLocationId"].Value.ToString();
				txtEffectiveDate.Value = Conversion.ToDateTime(item.Cells["EffectiveDate"].Value);
				CmbFreightType.Value = item.Cells["FreightTypeId"].Value.ToString();
				txtFreightRate.Text = item.Cells["FreightRate"].Value.ToString();
				txtFreightUom.Text = item.Cells["FreightUom"].Value.ToString();
				txtPackSize.Text = item.Cells["PackSizeTo"].Value.ToString();
				txtFreightAmount.Text = item.Cells["FreightAmount"].Value.ToString();
				txtMinAmount.Text = item.Cells["MinAmount"].Value.ToString();
				txtDetailRemarks.Text = item.Cells["RemarksDetail"].Text.ToString();
				btnAddDetail.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbDistrict.Focus();
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
			double freightAmount = Conversion.ToDouble((object)txtFreightAmount.Text);
			if (Conversion.ToDouble((object)txtMinAmount.Text) > freightAmount)
			{
				MessageBox.Show("Min Amount Can't be Greater Than Freight Amount");
				txtMinAmount.Focus();
				return;
			}
			if (table.Rows.Count > 0)
			{
				DateTime EffectiveDate = Conversion.ToDateTime(table.Rows[table.Rows.Count - 1]["EffectiveDate"]);
				if (txtEffectiveDate.Value.Date < EffectiveDate.Date)
				{
					MessageBox.Show("Can't update Contract Against this Selected Effective Date!");
					return;
				}
			}
			table.Rows[updateRowIndex]["DistrictId"] = cmbDistrict.Value;
			table.Rows[updateRowIndex]["DistrictName"] = ((Control)(object)cmbDistrict).Text;
			table.Rows[updateRowIndex]["LoadingLocationId"] = cmbLoadingLocation.Value;
			table.Rows[updateRowIndex]["LoadingLocationName"] = ((Control)(object)cmbLoadingLocation).Text;
			table.Rows[updateRowIndex]["UnLoadingLocationId"] = cmbUnloadingLocation.Value;
			table.Rows[updateRowIndex]["UnLoadingLocationName"] = ((Control)(object)cmbUnloadingLocation).Text;
			table.Rows[updateRowIndex]["EffectiveDate"] = txtEffectiveDate.Value;
			table.Rows[updateRowIndex]["FreightTypeId"] = CmbFreightType.Value;
			table.Rows[updateRowIndex]["FreightType"] = ((Control)(object)CmbFreightType).Text;
			table.Rows[updateRowIndex]["FreightRate"] = Conversion.ToDouble((object)txtFreightRate.Text);
			table.Rows[updateRowIndex]["FreightUom"] = Conversion.ToInt((object)txtFreightUom.Text);
			table.Rows[updateRowIndex]["PackSizeTo"] = Conversion.ToInt((object)txtPackSize.Text.Trim());
			table.Rows[updateRowIndex]["FreightAmount"] = Conversion.ToDouble((object)txtFreightAmount.Text);
			table.Rows[updateRowIndex]["MinAmount"] = Conversion.ToDouble((object)txtMinAmount.Text.Trim());
			table.Rows[updateRowIndex]["RemarksDetail"] = txtDetailRemarks.Text.Trim();
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			DetailFormReset();
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Expected O, but got Unknown
		//IL_049d: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Expected O, but got Unknown
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
			InvInLandFreightAgreement obj = new InvInLandFreightAgreement();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else
			{
				DialogResult num = MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
				obj.Id = 0;
				if (num == DialogResult.No)
				{
					return;
				}
			}
			obj.BranchId = 0;
			obj.ProjectId = 0;
			obj.AgreementDate = Conversion.ToDateTime((object)txtDocDate.Value);
			obj.AgreementCode = Conversion.ToString((object)txtAgreementCode.Text.Trim());
			obj.DocNo = Conversion.ToInt((object)TxtDocNo.Text.Trim());
			obj.SupplierCustomerId = Conversion.ToInt(CmbParty.Value);
			obj.TransporterId = Conversion.ToInt(CmbTransporter.Value);
			obj.OrganizationId = userAccount.OrganizationId;
			obj.CompanyId = userAccount.CompanyId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.EntryUser = userAccount.ID;
			obj.ModifyUser = userAccount.ID;
			obj.ApprovedUser = userAccount.ID;
			obj.IsApproved = false;
			obj.DocumentTypeId = 161;
			obj.InvInLandFreightAgreementDetailList = new List<InvInLandFreightAgreementDetail>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvInLandFreightAgreementDetail Detail = new InvInLandFreightAgreementDetail();
				Detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
				Detail.EnteryDate = DateTime.Now;
				Detail.EnteryUserId = userAccount.ID;
				Detail.ModifyDate = DateTime.Now;
				Detail.ModifyUserId = userAccount.ID;
				Detail.ApprovedDate = DateTime.Now;
				Detail.ApprovedUserId = userAccount.ID;
				Detail.IsApproved = false;
				Detail.SubAgreementCode = obj.AgreementCode;
				Detail.LoadingDistrictId = Conversion.ToInt(r.Cells["DistrictId"].Value);
				Detail.DistrictName = Conversion.ToString((object)r.Cells["DistrictName"].Text);
				Detail.LoadingLocationId = Conversion.ToInt(r.Cells["LoadingLocationId"].Value);
				Detail.LoadingLocationName = Conversion.ToString((object)r.Cells["LoadingLocationName"].Text);
				Detail.DestinationLocationId = Conversion.ToInt(r.Cells["UnLoadingLocationId"].Value);
				Detail.UnLoadingLocationName = Conversion.ToString((object)r.Cells["UnLoadingLocationName"].Text);
				Detail.EffectiveDate = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
				Detail.FreightTypeId = Conversion.ToInt((object)r.Cells["FreightTypeId"].Text);
				Detail.FreightType = Conversion.ToString(r.Cells["FreightType"].Value);
				Detail.FreightRate = Conversion.ToDecimal(r.Cells["FreightRate"].Value);
				Detail.FreightUom = Conversion.ToInt(r.Cells["FreightUom"].Value);
				Detail.PackSizeTo = Conversion.ToInt(r.Cells["PackSizeTo"].Value);
				Detail.FregihtAmount = Conversion.ToDecimal(r.Cells["FreightAmount"].Value);
				Detail.MinAmount = Conversion.ToDecimal(r.Cells["MinAmount"].Value);
				Detail.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
				if (Detail.Id > 0)
				{
					Detail.ActionId = 2;
				}
				else
				{
					Detail.ActionId = 1;
				}
				obj.InvInLandFreightAgreementDetailList.Add(Detail);
			}
			obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			for (int j = 0; j < Attachment.dtAttachments.Rows.Count; j++)
			{
				DMSAttachments attach = new DMSAttachments();
				attach.Attachment = Attachment.dtAttachments.Rows[j][1].ToString();
				attach.ScreenName = base.Name;
				obj.AttachmentsList.Add(attach);
			}
			if (RecId > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					obj.InvInLandFreightAgreementDetailList.Add(lstRemoveRecord[i]);
				}
			}
			InvInLandFreightAgreement.Save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully [ " + obj.DocNo + " ] ");
			}
			else
			{
				MessageBox.Show("Record Save Successfully  [ " + obj.DocNo + " ] ");
			}
			Reset();
			grdDetail.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void If(bool v)
	{
		throw new NotImplementedException();
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
			throw;
		}
	}

	private void MainFormReset()
	{
		try
		{
			txtAgreementCode.Clear();
			((Control)(object)CmbParty).Text = string.Empty;
			((Control)(object)CmbTransporter).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailFormReset()
	{
		try
		{
			((Control)(object)cmbDistrict).Text = string.Empty;
			((Control)(object)cmbLoadingLocation).Text = string.Empty;
			((Control)(object)cmbUnloadingLocation).Text = string.Empty;
			((UltraGridBase)CmbFreightType).Rows[2].Activate();
			txtFreightRate.Clear();
			txtFreightUom.Clear();
			txtPackSize.Clear();
			txtFreightAmount.Clear();
			txtMinAmount.Clear();
			txtDetailRemarks.Clear();
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
			RecId = 0;
			MainFormReset();
			DetailFormReset();
			GenerateCode();
			table.Rows.Clear();
			grdDetail.DataSource = table;
			dtViewAttachments.Clear();
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			btnAddDetail.Visible = true;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.Column.Key == "Delete")
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				GridEXRow r = grdDetail.CurrentRow;
				if (btnsave.Visible && btnsave.Enabled)
				{
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else
				{
					InvInLandFreightAgreementDetail Detail = new InvInLandFreightAgreementDetail();
					Detail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					Detail.LoadingDistrictId = Conversion.ToInt(r.Cells["DistrictId"].Value);
					Detail.DistrictName = Conversion.ToString((object)r.Cells["DistrictName"].Text);
					Detail.LoadingLocationId = Conversion.ToInt(r.Cells["LoadingLocationId"].Value);
					Detail.LoadingLocationName = Conversion.ToString((object)r.Cells["LoadingLocationName"].Text);
					Detail.DestinationLocationId = Conversion.ToInt(r.Cells["UnLoadingLocationId"].Value);
					Detail.UnLoadingLocationName = Conversion.ToString((object)r.Cells["UnLoadingLocationName"].Text);
					Detail.EffectiveDate = Conversion.ToDateTime(r.Cells["EffectiveDate"].Value);
					Detail.FreightTypeId = Conversion.ToInt((object)r.Cells["FreightTypeId"].Text);
					Detail.FreightType = Conversion.ToString(r.Cells["FreightType"].Value);
					Detail.FreightRate = Conversion.ToDecimal(r.Cells["FreightRate"].Value);
					Detail.FreightUom = Conversion.ToInt(r.Cells["FreightUom"].Value);
					Detail.PackSizeTo = Conversion.ToInt(r.Cells["PackSizeTo"].Value);
					Detail.FregihtAmount = Conversion.ToDecimal(r.Cells["FreightAmount"].Value);
					Detail.MinAmount = Conversion.ToDecimal(r.Cells["MinAmount"].Value);
					Detail.RemarksDetail = Conversion.ToString((object)r.Cells["RemarksDetail"].Text.Trim());
					Detail.ActionId = 3;
					lstRemoveRecord.Add(Detail);
					table.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
			}
			grdDetail.UpdateData();
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

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		try
		{
			btnAddDetail.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			DetailFormReset();
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
			MessageBox.Show(ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			RefreshFormFields();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RefreshFormFields()
	{
		try
		{
			AccountsFill();
			DistrictFill();
			CityFill();
			FreightTypeFill();
			GenerateCode();
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
				HistoryGridFill();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 161
			};
			DataTable dtHistory = new DataTable();
			dtHistory = InvInLandFreightAgreement.FormHistory(val);
			if (dtHistory.Rows.Count > 0)
			{
				GrdHistory.DataSource = dtHistory;
				GrdHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				GrdHistory.ClearStructure();
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
			GrdHistory.RootTable.Columns["Id"].Visible = false;
			GrdHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			GrdHistory.RootTable.Columns.Add("Detail");
			GrdHistory.RootTable.Columns["Detail"].Caption = "Detail";
			GrdHistory.RootTable.Columns["Detail"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Detail"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Detail"].ButtonText = "Detail";
			GrdHistory.RootTable.Columns["Detail"].Width = 30;
			GrdHistory.RootTable.Columns.Add("Edit");
			GrdHistory.RootTable.Columns["Edit"].Caption = "Edit";
			GrdHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Edit"].Image = Resources.icons8_pencil_16;
			GrdHistory.RootTable.Columns["Edit"].Width = 25;
			GrdHistory.RootTable.Columns.Add("Print");
			GrdHistory.RootTable.Columns["Print"].Caption = "Print";
			GrdHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			GrdHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			GrdHistory.RootTable.Columns["Print"].Image = Resources.icons8_print_16;
			GrdHistory.RootTable.Columns["Print"].Width = 25;
			ctrlGrdBar2_Load(null, null);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)GrdHistory).Name);
			if (fs != null)
			{
				GrdHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void GrdHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				Reset();
				RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById_Update(RecId);
			}
			_ = e.Column.Key == "Print";
			if (e.Column.Key == "Detail")
			{
				InvInLandFreightAgreement obj = InvInLandFreightAgreement.GetByID(Conversion.ToInt(GrdHistory.CurrentRow.Cells["Id"].Value));
				DataTable dthistorydetail = new DataTable();
				dthistorydetail.Columns.Add("DistrictName");
				dthistorydetail.Columns.Add("LoadingLocationName");
				dthistorydetail.Columns.Add("UnLoadingLocationName");
				dthistorydetail.Columns.Add("EffectiveDate");
				dthistorydetail.Columns.Add("FreightType");
				dthistorydetail.Columns.Add("FreightRate", typeof(double));
				dthistorydetail.Columns.Add("FreightUom", typeof(int));
				dthistorydetail.Columns.Add("PackSizeTo", typeof(int));
				dthistorydetail.Columns.Add("FreightAmount", typeof(double));
				dthistorydetail.Columns.Add("MinAmount", typeof(double));
				dthistorydetail.Columns.Add("RemarksDetail");
				dthistorydetail.Clear();
				for (int i = 0; i < obj.InvInLandFreightAgreementDetailList.Count; i++)
				{
					dthistorydetail.Rows.Add(obj.InvInLandFreightAgreementDetailList[i].DistrictName, obj.InvInLandFreightAgreementDetailList[i].LoadingLocationName, obj.InvInLandFreightAgreementDetailList[i].UnLoadingLocationName, obj.InvInLandFreightAgreementDetailList[i].EffectiveDate, obj.InvInLandFreightAgreementDetailList[i].FreightType, obj.InvInLandFreightAgreementDetailList[i].FreightRate, obj.InvInLandFreightAgreementDetailList[i].FreightUom, obj.InvInLandFreightAgreementDetailList[i].PackSizeTo, obj.InvInLandFreightAgreementDetailList[i].FregihtAmount, obj.InvInLandFreightAgreementDetailList[i].MinAmount, obj.InvInLandFreightAgreementDetailList[i].RemarksDetail);
				}
				GrdDetailHistory.DataSource = dthistorydetail;
				GrdDetailHistory.RetrieveStructure();
				gridhistorydetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistorydetail()
	{
		try
		{
			GrdDetailHistory.RootTable.Columns["DistrictName"].Width = 150;
			GrdDetailHistory.RootTable.Columns["LoadingLocationName"].Width = 150;
			GrdDetailHistory.RootTable.Columns["UnLoadingLocationName"].Width = 150;
			GrdDetailHistory.RootTable.Columns["FreightAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["MinAmount"].AggregateFunction = (AggregateFunction)2;
			GrdDetailHistory.RootTable.Columns["FreightAmount"].FormatString = "0,0";
			GrdDetailHistory.RootTable.Columns["MinAmount"].FormatString = "0,0";
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
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			RecId = Conversion.ToInt((object)ID);
			InvInLandFreightAgreement obj = InvInLandFreightAgreement.GetByID(RecId);
			tabControl1.SelectedIndex = 0;
			TxtDocNo.Text = obj.DocNo.ToString();
			txtDocDate.Value = obj.AgreementDate;
			txtAgreementCode.Text = obj.AgreementCode;
			CmbParty.Value = obj.SupplierCustomerId;
			CmbTransporter.Value = obj.TransporterId;
			table.Rows.Clear();
			for (int i = 0; i < obj.InvInLandFreightAgreementDetailList.Count; i++)
			{
				table.Rows.Add(obj.InvInLandFreightAgreementDetailList[i].Id, obj.InvInLandFreightAgreementDetailList[i].LoadingDistrictId, obj.InvInLandFreightAgreementDetailList[i].DistrictName, obj.InvInLandFreightAgreementDetailList[i].LoadingLocationId, obj.InvInLandFreightAgreementDetailList[i].LoadingLocationName, obj.InvInLandFreightAgreementDetailList[i].DestinationLocationId, obj.InvInLandFreightAgreementDetailList[i].UnLoadingLocationName, obj.InvInLandFreightAgreementDetailList[i].EffectiveDate, obj.InvInLandFreightAgreementDetailList[i].FreightTypeId, obj.InvInLandFreightAgreementDetailList[i].FreightType, obj.InvInLandFreightAgreementDetailList[i].FreightRate, obj.InvInLandFreightAgreementDetailList[i].FreightUom, obj.InvInLandFreightAgreementDetailList[i].PackSizeTo, obj.InvInLandFreightAgreementDetailList[i].FregihtAmount, obj.InvInLandFreightAgreementDetailList[i].MinAmount, obj.InvInLandFreightAgreementDetailList[i].RemarksDetail);
			}
			grdDetail.DataSource = table;
			grdDetail.RetrieveStructure();
			grdSettings();
			List<DMSAttachments> byID = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(RecId.ToString()), base.Name);
			AT = new Attachment();
			foreach (global::Architecture.Model.DMSAttachments Attachment in byID)
			{
				TempAttachments TA = new TempAttachments();
				TA.Id = 0;
				TA.Path = Attachment.Attachment;
				AT.lst.Add(TA);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "NoOfAttachments"))
			{
				return;
			}
			GridEXRow item = GrdHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			List<global::Architecture.Model.DMSAttachments> dMSList = global::Architecture.BLL.DMSAttachments.GetByID(int.Parse(item.Cells["Id"].Value.ToString().ToString()), base.Name);
			dtViewAttachments.Clear();
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtViewAttachments.Rows.Add(itemList.Attachment);
				}
			}
			else
			{
				dtViewAttachments.Clear();
			}
			new AttachmentView(dtViewAttachments).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.InlandFreightAggreement(new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 161,
				Id = RecId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", userAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", userAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "242-InLandFreightAgreementSlip.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGrnFormHistory_Click(object sender, EventArgs e)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DataTable dt = new DataTable();
			dt = VoucherReports.InlandFreightAggreement(new ReportsParameters
			{
				OrganizationId = userAccount.OrganizationId,
				CompanyId = userAccount.CompanyId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 161
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting val = new Reporting();
			val.RptPerameter("@CompanyAddress", userAccount.CompAddress.ToString());
			val.RptPerameter("@CompanyName", userAccount.CompName.ToString());
			val.ShowReportWithDataTable(dt, "243-InLandFreightAgreementRegister.rpt");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFreightAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToDecimal((object)txtFreightAmount.Text) > 0m)
			{
				CalculateRate();
			}
			else
			{
				txtFreightRate.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdHistory_DoubleClick(object sender, EventArgs e)
	{
		Reset();
		RecId = Conversion.ToInt((object)GrdHistory.CurrentRow.Cells["Id"].Value.ToString());
		ReadById_Update(RecId);
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
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Expected O, but got Unknown
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Expected O, but got Unknown
		//IL_0339: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Expected O, but got Unknown
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bc: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Expected O, but got Unknown
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_093a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0944: Expected O, but got Unknown
		//IL_3d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d1a: Expected O, but got Unknown
		//IL_3d27: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d31: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
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
		Appearance appearance3 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance16 = new Appearance();
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
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance17 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Account_Definition.InLandFreightAgreement));
		GridEXLayout GrdDetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdHistory_DesignTimeLayout = new GridEXLayout();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtDetailRemarks = new System.Windows.Forms.TextBox();
		this.label28 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.btnAddDetail = new System.Windows.Forms.Button();
		this.label14 = new System.Windows.Forms.Label();
		this.txtMinAmount = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtFreightAmount = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtPackSize = new System.Windows.Forms.TextBox();
		this.lblUom = new System.Windows.Forms.Label();
		this.txtFreightUom = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtFreightRate = new System.Windows.Forms.TextBox();
		this.CmbFreightType = new UltraCombo();
		this.txtEffectiveDate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbUnloadingLocation = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbLoadingLocation = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbDistrict = new UltraCombo();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtAgreementCode = new System.Windows.Forms.TextBox();
		this.CmbTransporter = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbParty = new UltraCombo();
		this.txtDocDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.TxtDocNo = new System.Windows.Forms.TextBox();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.GrdDetailHistory = new GridEX();
		this.GrdHistory = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.btnGrnFormHistory = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUnloadingLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingLocation).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrict).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParty).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDetailHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1157, 583);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1149, 557);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.groupBox2);
		this.panel1.Controls.Add(this.groupBox1);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1143, 551);
		this.panel1.TabIndex = 0;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 231);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1143, 320);
		this.panel4.TabIndex = 4;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1143, 320);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		this.groupBox2.Controls.Add(this.txtDetailRemarks);
		this.groupBox2.Controls.Add(this.label28);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.btnAddDetail);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add(this.txtMinAmount);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add(this.txtFreightAmount);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Controls.Add(this.txtPackSize);
		this.groupBox2.Controls.Add(this.lblUom);
		this.groupBox2.Controls.Add(this.txtFreightUom);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtFreightRate);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbFreightType);
		this.groupBox2.Controls.Add(this.txtEffectiveDate);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUnloadingLocation);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLoadingLocation);
		this.groupBox2.Controls.Add(this.label2);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDistrict);
		this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox2.Location = new System.Drawing.Point(0, 126);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1143, 105);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail";
		this.txtDetailRemarks.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDetailRemarks.Location = new System.Drawing.Point(98, 70);
		this.txtDetailRemarks.Name = "txtDetailRemarks";
		this.txtDetailRemarks.Size = new System.Drawing.Size(838, 23);
		this.txtDetailRemarks.TabIndex = 10;
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label28.Location = new System.Drawing.Point(8, 70);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(54, 15);
		this.label28.TabIndex = 518;
		this.label28.Text = "Remarks";
		this.btnCancelDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelDetail.Location = new System.Drawing.Point(1044, 71);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(57, 23);
		this.btnCancelDetail.TabIndex = 13;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = true;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnUpdateDetail.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.Location = new System.Drawing.Point(984, 71);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(58, 23);
		this.btnUpdateDetail.TabIndex = 12;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = true;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.btnAddDetail.Location = new System.Drawing.Point(943, 71);
		this.btnAddDetail.Name = "btnAddDetail";
		this.btnAddDetail.Size = new System.Drawing.Size(39, 23);
		this.btnAddDetail.TabIndex = 11;
		this.btnAddDetail.Text = "Add";
		this.btnAddDetail.UseVisualStyleBackColor = true;
		this.btnAddDetail.Click += new System.EventHandler(btnAddDetail_Click);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(952, 46);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(74, 15);
		this.label14.TabIndex = 496;
		this.label14.Text = "Min Amount";
		this.txtMinAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtMinAmount.Location = new System.Drawing.Point(1041, 42);
		this.txtMinAmount.Name = "txtMinAmount";
		this.txtMinAmount.Size = new System.Drawing.Size(111, 23);
		this.txtMinAmount.TabIndex = 9;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(733, 46);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(90, 15);
		this.label13.TabIndex = 494;
		this.label13.Text = "Freight Amount";
		this.txtFreightAmount.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightAmount.Location = new System.Drawing.Point(825, 42);
		this.txtFreightAmount.Name = "txtFreightAmount";
		this.txtFreightAmount.Size = new System.Drawing.Size(111, 23);
		this.txtFreightAmount.TabIndex = 8;
		this.txtFreightAmount.TabStop = false;
		this.txtFreightAmount.TextChanged += new System.EventHandler(txtFreightAmount_TextChanged);
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(8, 46);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(72, 15);
		this.label12.TabIndex = 492;
		this.label12.Text = "Pack Size To";
		this.txtPackSize.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtPackSize.Location = new System.Drawing.Point(98, 43);
		this.txtPackSize.Name = "txtPackSize";
		this.txtPackSize.Size = new System.Drawing.Size(148, 23);
		this.txtPackSize.TabIndex = 5;
		this.lblUom.AutoSize = true;
		this.lblUom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblUom.Location = new System.Drawing.Point(252, 46);
		this.lblUom.Name = "lblUom";
		this.lblUom.Size = new System.Drawing.Size(73, 15);
		this.lblUom.TabIndex = 490;
		this.lblUom.Text = "Freight Uom";
		this.txtFreightUom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightUom.Location = new System.Drawing.Point(356, 42);
		this.txtFreightUom.Name = "txtFreightUom";
		this.txtFreightUom.Size = new System.Drawing.Size(123, 23);
		this.txtFreightUom.TabIndex = 6;
		this.txtFreightUom.TextChanged += new System.EventHandler(txtFreightUom_TextChanged);
		this.txtFreightUom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightUom_KeyPress);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(527, 46);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(72, 15);
		this.label10.TabIndex = 482;
		this.label10.Text = "Freight Rate";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(952, 20);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(72, 15);
		this.label7.TabIndex = 488;
		this.label7.Text = "Freight Type";
		this.txtFreightRate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreightRate.Location = new System.Drawing.Point(605, 42);
		this.txtFreightRate.Name = "txtFreightRate";
		this.txtFreightRate.Size = new System.Drawing.Size(122, 23);
		this.txtFreightRate.TabIndex = 7;
		this.txtFreightRate.TextChanged += new System.EventHandler(txtFreightRate_TextChanged);
		this.txtFreightRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreightRate_KeyPress);
		this.CmbFreightType.AllowNull = (DefaultableBoolean)1;
		this.CmbFreightType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbFreightType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbFreightType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.CmbFreightType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance47).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance47).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbFreightType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbFreightType).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbFreightType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbFreightType).Location = new System.Drawing.Point(1041, 14);
		((System.Windows.Forms.Control)(object)this.CmbFreightType).Name = "CmbFreightType";
		((System.Windows.Forms.Control)(object)this.CmbFreightType).Size = new System.Drawing.Size(111, 26);
		((System.Windows.Forms.Control)(object)this.CmbFreightType).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.CmbFreightType).TextChanged += new System.EventHandler(CmbFreightType_TextChanged);
		this.txtEffectiveDate.CustomFormat = "dd-MMM-yyyy";
		this.txtEffectiveDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtEffectiveDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectiveDate.Location = new System.Drawing.Point(825, 16);
		this.txtEffectiveDate.Name = "txtEffectiveDate";
		this.txtEffectiveDate.Size = new System.Drawing.Size(111, 23);
		this.txtEffectiveDate.TabIndex = 3;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(733, 20);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(80, 15);
		this.label6.TabIndex = 482;
		this.label6.Text = "Effective Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(485, 20);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(114, 15);
		this.label5.TabIndex = 486;
		this.label5.Text = "UnLoading Location";
		this.cmbUnloadingLocation.AllowNull = (DefaultableBoolean)1;
		this.cmbUnloadingLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUnloadingLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUnloadingLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUnloadingLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUnloadingLocation).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUnloadingLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUnloadingLocation).Location = new System.Drawing.Point(605, 14);
		((System.Windows.Forms.Control)(object)this.cmbUnloadingLocation).Name = "cmbUnloadingLocation";
		((System.Windows.Forms.Control)(object)this.cmbUnloadingLocation).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbUnloadingLocation).TabIndex = 2;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(252, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(99, 15);
		this.label4.TabIndex = 484;
		this.label4.Text = "Loading Location";
		this.cmbLoadingLocation.AllowNull = (DefaultableBoolean)1;
		this.cmbLoadingLocation.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLoadingLocation.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLoadingLocation.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance63;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance64).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance65).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance65).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance65).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance65).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLoadingLocation).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLoadingLocation).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLoadingLocation.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLoadingLocation).Location = new System.Drawing.Point(357, 14);
		((System.Windows.Forms.Control)(object)this.cmbLoadingLocation).Name = "cmbLoadingLocation";
		((System.Windows.Forms.Control)(object)this.cmbLoadingLocation).Size = new System.Drawing.Size(122, 26);
		((System.Windows.Forms.Control)(object)this.cmbLoadingLocation).TabIndex = 1;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(8, 20);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(88, 15);
		this.label2.TabIndex = 482;
		this.label2.Text = "Source District";
		this.cmbDistrict.AllowNull = (DefaultableBoolean)1;
		this.cmbDistrict.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDistrict.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDistrict.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance31).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance31).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDistrict).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance32;
		((SpecialBoxBase)((UltraGridBase)this.cmbDistrict).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDistrict).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDistrict).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDistrict.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDistrict).Location = new System.Drawing.Point(98, 14);
		((System.Windows.Forms.Control)(object)this.cmbDistrict).Name = "cmbDistrict";
		((System.Windows.Forms.Control)(object)this.cmbDistrict).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbDistrict).TabIndex = 0;
		this.groupBox1.Controls.Add(this.label11);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtAgreementCode);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransporter);
		this.groupBox1.Controls.Add(this.label8);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParty);
		this.groupBox1.Controls.Add(this.txtDocDate);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label9);
		this.groupBox1.Controls.Add(this.TxtDocNo);
		this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox1.Location = new System.Drawing.Point(0, 78);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1143, 48);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(311, 17);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(94, 15);
		this.label11.TabIndex = 520;
		this.label11.Text = "Agreement Code";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(819, 17);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(117, 15);
		this.label1.TabIndex = 480;
		this.label1.Text = "Transport/Suppliers";
		this.txtAgreementCode.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAgreementCode.Location = new System.Drawing.Point(414, 13);
		this.txtAgreementCode.Name = "txtAgreementCode";
		this.txtAgreementCode.Size = new System.Drawing.Size(111, 23);
		this.txtAgreementCode.TabIndex = 1;
		this.CmbTransporter.AllowNull = (DefaultableBoolean)1;
		this.CmbTransporter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransporter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransporter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransporter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbTransporter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Location = new System.Drawing.Point(938, 11);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Name = "CmbTransporter";
		((System.Windows.Forms.Control)(object)this.CmbTransporter).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransporter).TabIndex = 3;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(533, 17);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(93, 15);
		this.label8.TabIndex = 478;
		this.label8.Text = "Customer/Party";
		this.CmbParty.AllowNull = (DefaultableBoolean)1;
		this.CmbParty.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParty.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParty.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParty).DisplayLayout.Appearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbParty).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParty).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance18).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance18).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance19;
		((SpecialBoxBase)((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParty).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbParty).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParty).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParty).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbParty).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParty).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParty).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParty).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParty.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParty).Location = new System.Drawing.Point(632, 11);
		((System.Windows.Forms.Control)(object)this.CmbParty).Name = "CmbParty";
		((System.Windows.Forms.Control)(object)this.CmbParty).Size = new System.Drawing.Size(178, 26);
		((System.Windows.Forms.Control)(object)this.CmbParty).TabIndex = 2;
		this.txtDocDate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocDate.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocDate.Location = new System.Drawing.Point(62, 13);
		this.txtDocDate.Name = "txtDocDate";
		this.txtDocDate.Size = new System.Drawing.Size(111, 23);
		this.txtDocDate.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 17);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 475;
		this.label3.Text = "Doc Date";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(181, 17);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(46, 15);
		this.label9.TabIndex = 476;
		this.label9.Text = "Doc No";
		this.TxtDocNo.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TxtDocNo.Location = new System.Drawing.Point(233, 13);
		this.TxtDocNo.Name = "TxtDocNo";
		this.TxtDocNo.ReadOnly = true;
		this.TxtDocNo.Size = new System.Drawing.Size(70, 23);
		this.TxtDocNo.TabIndex = 1;
		this.TxtDocNo.TabStop = false;
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label18);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 36);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1143, 42);
		this.panel3.TabIndex = 1;
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 18f);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(6, 6);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(297, 29);
		this.label18.TabIndex = 26;
		this.label18.Text = "In Land Freight Agreement";
		this.panel2.Controls.Add(this.ctrlGrdBar1);
		this.panel2.Controls.Add(this.toolstrip);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1143, 36);
		this.panel2.TabIndex = 0;
		this.toolstrip.Dock = System.Windows.Forms.DockStyle.Fill;
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.btnnew, this.btnsave, this.btnUpdate, this.btnRefresh, this.btnAttachment, this.toolStripButton1 });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(1143, 36);
		this.toolstrip.TabIndex = 7;
		this.toolstrip.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = (System.Drawing.Image)resources.GetObject("btnnew.Image");
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 33);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 33);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 33);
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(50, 33);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnAttachment.Image = (System.Drawing.Image)resources.GetObject("btnAttachment.Image");
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(94, 33);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(79, 33);
		this.toolStripButton1.Text = "242-Print";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1149, 557);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1143, 551);
		this.panel5.TabIndex = 0;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDetailHistory);
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.GrdHistory);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 30);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1143, 521);
		this.panel7.TabIndex = 1;
		this.GrdDetailHistory.AllowEdit = (InheritableBoolean)2;
		this.GrdDetailHistory.ColumnAutoResize = true;
		GrdDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDetailHistory.DesignTimeLayout = GrdDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDetailHistory.DynamicFiltering = true;
		this.GrdDetailHistory.FilterMode = (FilterMode)2;
		this.GrdDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Location = new System.Drawing.Point(0, 219);
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Name = "GrdDetailHistory";
		((ScrollableBase)this.GrdDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).Size = new System.Drawing.Size(1143, 302);
		((System.Windows.Forms.Control)(object)this.GrdDetailHistory).TabIndex = 48;
		this.GrdDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GrdDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDetailHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.AllowEdit = (InheritableBoolean)2;
		this.GrdHistory.ColumnAutoResize = true;
		GrdHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdHistory.DesignTimeLayout = GrdHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.GrdHistory.DynamicFiltering = true;
		this.GrdHistory.FilterMode = (FilterMode)2;
		this.GrdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.GrdHistory).Name = "GrdHistory";
		((ScrollableBase)this.GrdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdHistory).Size = new System.Drawing.Size(1143, 219);
		((System.Windows.Forms.Control)(object)this.GrdHistory).TabIndex = 47;
		this.GrdHistory.TotalRow = (InheritableBoolean)1;
		this.GrdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdHistory).VisualStyle = (VisualStyle)4;
		this.GrdHistory.ColumnButtonClick += new ColumnActionEventHandler(GrdHistory_ColumnButtonClick);
		this.GrdHistory.LinkClicked += new ColumnActionEventHandler(GrdHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.GrdHistory).DoubleClick += new System.EventHandler(GrdHistory_DoubleClick);
		this.panel6.Controls.Add(this.ctrlGrdBar2);
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1143, 30);
		this.panel6.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnLoadAll, this.btnGrnFormHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1143, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(71, 24);
		this.btnLoadAll.Text = "&LoadAll";
		this.btnLoadAll.ToolTipText = "Ctrl_L";
		this.btnGrnFormHistory.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnGrnFormHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGrnFormHistory.Name = "btnGrnFormHistory";
		this.btnGrnFormHistory.Size = new System.Drawing.Size(73, 24);
		this.btnGrnFormHistory.Text = "Register";
		this.btnGrnFormHistory.ToolTipText = "Crtl_H";
		this.btnGrnFormHistory.Click += new System.EventHandler(btnGrnFormHistory_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1094, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 29);
		this.ctrlGrdBar1.TabIndex = 541;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1094, -1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.GrdHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar2.TabIndex = 25;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1157, 583);
		base.Controls.Add(this.tabControl1);
		base.Name = "InLandFreightAgreement";
		this.Text = "In Land Freight Agreement";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(InLandFreightAgreement_Load);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbFreightType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUnloadingLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLoadingLocation).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDistrict).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTransporter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParty).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDetailHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.GrdHistory).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
