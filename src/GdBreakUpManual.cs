using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Export;
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
namespace Architecture.WinApp.Export
{

public class GdBreakUpManual : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private ToolStrip toolStrip4;

	private ToolStripButton btnNew;

	private ToolStripButton btnsave;

	private ToolStripButton btnupdate;

	private Panel panel1;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label23;

	private Panel panel3;

	private Panel panel4;

	internal GridEX grdGdBreakUp;

	private Panel panel30;

	private TextBox txtRemarks;

	private Label label121;

	private TextBox txtFttAmount;

	private Label label120;

	private TextBox txtExchangeRate;

	private Label label119;

	private TextBox txtNetAmount;

	private Label label118;

	private TextBox txtFTTPrcnt;

	private Label label117;

	private TextBox txtFobValue;

	private Label label116;

	private TextBox txtFreight;

	private Label label110;

	private TextBox txtDueDays;

	private Label label106;

	internal UltraCombo CmbBankName;

	private Label label105;

	private TextBox txtGDValue;

	private Label label107;

	private TextBox txtGDNoBreakUp;

	private Label label102;

	private TextBox txtInvoiceNoGDBreakUp;

	private Label label100;

	private DateTimePicker datGDDateGDBreakUp;

	private Label label101;

	private DateTimePicker datInvoiceDateGdBreakUp;

	private Label label99;

	public GdBreakUpManual(UserAccount PUser)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = PUser;
	}

	private void GdBreakUpManual_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1100, 700);
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			ctrlGrdBar1.mGridChooseFielder.Enabled = formrights.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formrights.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formrights.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formrights.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formrights.DoHaveGridGroupExpandRights;
			BankBind();
			HistoryBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BankBind()
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
			if (lst.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("BranchName");
				for (int i = 0; i < lst.Count; i++)
				{
					if (lst[i].IsHomeland == "Home Country")
					{
						dt.Rows.Add(lst[i].Id, lst[i].BranchName);
					}
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbBankName, "Id", "BranchName", "Bank Name", ZeroIndex: false);
				}
			}
			else
			{
				((Control)(object)CmbBankName).Text = string.Empty;
				((UltraGridBase)CmbBankName).DataSource = null;
			}
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
			btnsave.Visible = true;
			btnupdate.Visible = false;
			RecId = 0;
			((Control)(object)CmbBankName).Text = string.Empty;
			txtGDNoBreakUp.Text = string.Empty;
			txtInvoiceNoGDBreakUp.Text = string.Empty;
			txtFreight.Clear();
			txtGDValue.Text = string.Empty;
			txtExchangeRate.Text = string.Empty;
			txtDueDays.Clear();
			txtFobValue.Clear();
			txtFTTPrcnt.Text = "10";
			txtFttAmount.Clear();
			txtNetAmount.Clear();
			txtRemarks.Clear();
			txtGDNoBreakUp.Focus();
			HistoryBind();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdGdBreakUp).Name);
			if (fs != null)
			{
				grdGdBreakUp.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool GdBreakUpFormValidation()
	{
		if (txtGDNoBreakUp.Text == null || txtGDNoBreakUp.Text == string.Empty)
		{
			MessageBox.Show("GdNo field is required");
			txtGDNoBreakUp.Focus();
			return false;
		}
		if (txtInvoiceNoGDBreakUp.Text == null || txtInvoiceNoGDBreakUp.Text == string.Empty)
		{
			MessageBox.Show("BankInvoiceNo field is required");
			txtInvoiceNoGDBreakUp.Focus();
			return false;
		}
		if (Conversion.ToInt((object)txtDueDays.Text.Trim()) <= 0)
		{
			MessageBox.Show("Due Days field is Required");
			txtDueDays.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtGDValue.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("GD Value field is Required");
			txtGDValue.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtFobValue.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("FOb Value field is Required");
			txtFobValue.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtNetAmount.Text.Trim()) <= 0.0)
		{
			MessageBox.Show("Net Amount field is Required");
			txtNetAmount.Focus();
			return false;
		}
		return true;
	}

	private void grdGdBreakUp_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdGdBreakUp.CurrentRow;
			if (item != null)
			{
				btnsave.Visible = false;
				btnupdate.Visible = true;
				RecId = Conversion.ToInt(item.Cells["Id"].Value);
				GDBreakUpManual obj = GDBreakUpManual.GetByID(RecId);
				if (obj != null)
				{
					txtGDNoBreakUp.Text = obj.GDNO;
					datGDDateGDBreakUp.Value = Conversion.ToDateTime((object)obj.GDDate);
					txtInvoiceNoGDBreakUp.Text = obj.BankInvoiceNo;
					datInvoiceDateGdBreakUp.Value = Conversion.ToDateTime((object)obj.BankInvoiceDate);
					CmbBankName.Value = obj.BankId;
					txtDueDays.Text = Conversion.ToString((object)obj.DueDays);
					txtExchangeRate.Text = Conversion.ToString((object)obj.ExchangeRate);
					txtGDValue.Text = Conversion.ToDouble((object)obj.GDValue).ToString("#,##0.###");
					txtFreight.Text = Conversion.ToDouble((object)obj.Freight).ToString("#,##0.###");
					txtFobValue.Text = Conversion.ToDouble((object)obj.FobValue).ToString("#,##0.###");
					txtFTTPrcnt.Text = ((Conversion.ToDouble((object)obj.CommPercent) > 0.0) ? Conversion.ToDouble((object)obj.CommPercent).ToString("#,##0.###") : "10");
					txtFttAmount.Text = Conversion.ToDouble((object)obj.CommAmount).ToString("#,##0.###");
					txtNetAmount.Text = Conversion.ToDouble((object)obj.NetToBeRealized).ToString("#,##0.###");
					txtRemarks.Text = Conversion.ToString((object)obj.Remarks);
				}
				txtGDNoBreakUp.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			GDBreakUpManual obj = new GDBreakUpManual();
			if (RecId == 0)
			{
				if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			if (GdBreakUpFormValidation())
			{
				obj.DocumentTypeId = 3;
				obj.GDNO = txtGDNoBreakUp.Text.Trim();
				obj.GDDate = Conversion.ToDateTime((object)datGDDateGDBreakUp.Value);
				obj.BankInvoiceNo = txtInvoiceNoGDBreakUp.Text.Trim();
				obj.BankInvoiceDate = Conversion.ToDateTime((object)datInvoiceDateGdBreakUp.Value);
				obj.BankId = Conversion.ToInt(CmbBankName.Value);
				obj.DueDays = Conversion.ToInt((object)txtDueDays.Text);
				obj.ExchangeRate = Conversion.ToDecimal((object)txtExchangeRate.Text);
				obj.GDValue = Conversion.ToDecimal((object)txtGDValue.Text);
				obj.Freight = Conversion.ToDecimal((object)txtFreight.Text);
				obj.FobValue = Conversion.ToDecimal((object)txtFobValue.Text);
				obj.CommPercent = Conversion.ToDecimal((object)txtFTTPrcnt.Text);
				obj.CommAmount = Conversion.ToDecimal((object)txtFttAmount.Text);
				obj.NetToBeRealized = Conversion.ToDecimal((object)txtNetAmount.Text);
				obj.Remarks = Conversion.ToString((object)txtRemarks.Text);
				obj.EntryUserId = UserAccount.ID;
				obj.ModifyUserId = UserAccount.ID;
				obj.OrganizationId = UserAccount.OrganizationId;
				obj.CompanyId = UserAccount.CompanyId;
				obj.ActionTypeId = ((obj.Id <= 0) ? 1 : 2);
				GDBreakUpManual.Save(obj);
				if (RecId == 0)
				{
					MessageBox.Show("Save SuccessFully");
				}
				if (RecId > 0)
				{
					MessageBox.Show("Update SuccessFully");
				}
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId not found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HistoryBind()
	{
		try
		{
			DataTable dt = GDBreakUpManual.FormHistory(UserAccount.OrganizationId, UserAccount.CompanyId);
			if (dt != null && dt.Rows.Count > 0)
			{
				DataTable dtHis = new DataTable();
				dtHis.Columns.Add("Id", typeof(int));
				dtHis.Columns.Add("DocumentTypeId", typeof(int));
				dtHis.Columns.Add("BankInvoiceNo");
				dtHis.Columns.Add("BankInvoiceDate", typeof(DateTime));
				dtHis.Columns.Add("GDNO", typeof(string));
				dtHis.Columns.Add("GDDate", typeof(DateTime));
				dtHis.Columns.Add("GDValue", typeof(double));
				dtHis.Columns.Add("GDWeight", typeof(double));
				dtHis.Columns.Add("FCL", typeof(int));
				dtHis.Columns.Add("BankName", typeof(string));
				dtHis.Columns.Add("DueDays", typeof(int));
				dtHis.Columns.Add("ExchangeRate", typeof(double));
				dtHis.Columns.Add("Freight", typeof(double));
				dtHis.Columns.Add("FobValue", typeof(double));
				dtHis.Columns.Add("FTT%", typeof(double));
				dtHis.Columns.Add("FttAmount", typeof(double));
				dtHis.Columns.Add("NetAmount", typeof(double));
				dtHis.Columns.Add("Remarks", typeof(string));
				dtHis.Columns.Add("EntryDate", typeof(DateTime));
				dtHis.Columns.Add("EntryUser", typeof(string));
				dtHis.Columns.Add("ModifyDate", typeof(DateTime));
				dtHis.Columns.Add("ModifyUser", typeof(string));
				foreach (DataRow dr in dt.Rows)
				{
					dtHis.Rows.Add(dr["Id"], dr["DocumentTypeId"], dr["BankInvoiceNo"], dr["BankInvoiceDate"], dr["GDNO"], dr["GDDate"], dr["GDValue"], dr["GDWeight"], dr["GDNoofContrainers"], dr["BankName"], dr["DueDays"], dr["ExchangeRate"], dr["Freight"], dr["FobValue"], dr["CommPercent"], dr["CommAmount"], dr["NetToBeRealized"], dr["Remarks"], dr["EntryDate"], dr["EntryUser"], dr["ModifyDate"], dr["ModifyUser"]);
				}
				grdGdBreakUp.DataSource = dtHis;
				grdGdBreakUp.RetrieveStructure();
				grdSettingsGdBreakUp();
			}
			else
			{
				grdGdBreakUp.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettingsGdBreakUp()
	{
		try
		{
			grdGdBreakUp.AllowEdit = (InheritableBoolean)2;
			grdGdBreakUp.AllowDelete = (InheritableBoolean)2;
			grdGdBreakUp.RootTable.Columns["Id"].Visible = false;
			grdGdBreakUp.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdGdBreakUp.RootTable.Columns["GDWeight"].Visible = false;
			grdGdBreakUp.RootTable.Columns["FCL"].Visible = false;
			grdGdBreakUp.RootTable.Columns["ExchangeRate"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["ExchangeRate"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["GDValue"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["GDValue"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["GDValue"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["GDValue"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["Freight"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["Freight"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["Freight"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["Freight"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FOBValue"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["FOBValue"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["FOBValue"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FOBValue"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FTT%"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["FTT%"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FttAmount"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["FttAmount"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["FttAmount"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["FttAmount"].TotalFormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["NetAmount"].AggregateFunction = (AggregateFunction)2;
			grdGdBreakUp.RootTable.Columns["NetAmount"].TextAlignment = (TextAlignment)3;
			grdGdBreakUp.RootTable.Columns["NetAmount"].FormatString = "#,##0.###";
			grdGdBreakUp.RootTable.Columns["NetAmount"].TotalFormatString = "#,##0.###";
			CommonServices.GridAutoAdjustmentNew(grdGdBreakUp);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDueDays_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtGDValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFreight_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFobValue_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFTTPrcnt_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFttAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtNetAmount_KeyPress(object sender, KeyPressEventArgs e)
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

	private void CalculateFobValue()
	{
		try
		{
			if (Conversion.ToDouble((object)txtFreight.Text) > Conversion.ToDouble((object)txtGDValue.Text))
			{
				throw new Exception("Freight Cant be Greater Than GD Value");
			}
			double Fob = Conversion.ToDouble((object)txtGDValue.Text) - Conversion.ToDouble((object)txtFreight.Text);
			txtFobValue.Text = Fob.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateFTTAmount()
	{
		try
		{
			if (Conversion.ToDouble((object)txtFTTPrcnt.Text) > 100.0)
			{
				txtFTTPrcnt.Text = "10";
				MessageBox.Show("Percent Cant be Greater Than 100", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			double FTTAmount = Conversion.ToDouble((object)txtFobValue.Text) * Conversion.ToDouble((object)txtFTTPrcnt.Text) / 100.0;
			txtFttAmount.Text = FTTAmount.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CalculateNetAmount()
	{
		try
		{
			if (Conversion.ToDouble((object)txtGDValue.Text) > 0.0 && Conversion.ToDouble((object)txtFttAmount.Text) > 0.0)
			{
				double NetAmount = Conversion.ToDouble((object)txtGDValue.Text) - Conversion.ToDouble((object)txtFttAmount.Text);
				txtNetAmount.Text = NetAmount.ToString("#,##0.###");
			}
			else
			{
				txtNetAmount.Text = 0.ToString("#,##0.###");
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtGDValue_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValue();
			CalculateFTTAmount();
			CalculateNetAmount();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtFreight_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFobValue();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtFobValue_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFTTPrcnt_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateFTTAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtFttAmount_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateNetAmount();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GdBreakUpManual_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControl1.SelectedIndex == 0)
			{
				Insert();
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				Reset();
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U)
			{
				btnupdate_Click(null, null);
			}
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
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Expected O, but got Unknown
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Expected O, but got Unknown
		GridEXLayout grdGdBreakUp_DesignTimeLayout = new GridEXLayout();
		Appearance appearance541 = new Appearance();
		Appearance appearance542 = new Appearance();
		Appearance appearance543 = new Appearance();
		Appearance appearance544 = new Appearance();
		Appearance appearance545 = new Appearance();
		Appearance appearance546 = new Appearance();
		Appearance appearance547 = new Appearance();
		Appearance appearance548 = new Appearance();
		Appearance appearance549 = new Appearance();
		Appearance appearance550 = new Appearance();
		Appearance appearance551 = new Appearance();
		Appearance appearance552 = new Appearance();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel4 = new System.Windows.Forms.Panel();
		this.grdGdBreakUp = new GridEX();
		this.panel30 = new System.Windows.Forms.Panel();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label121 = new System.Windows.Forms.Label();
		this.txtFttAmount = new System.Windows.Forms.TextBox();
		this.label120 = new System.Windows.Forms.Label();
		this.txtExchangeRate = new System.Windows.Forms.TextBox();
		this.label119 = new System.Windows.Forms.Label();
		this.txtNetAmount = new System.Windows.Forms.TextBox();
		this.label118 = new System.Windows.Forms.Label();
		this.txtFTTPrcnt = new System.Windows.Forms.TextBox();
		this.label117 = new System.Windows.Forms.Label();
		this.txtFobValue = new System.Windows.Forms.TextBox();
		this.label116 = new System.Windows.Forms.Label();
		this.txtFreight = new System.Windows.Forms.TextBox();
		this.label110 = new System.Windows.Forms.Label();
		this.txtDueDays = new System.Windows.Forms.TextBox();
		this.label106 = new System.Windows.Forms.Label();
		this.CmbBankName = new UltraCombo();
		this.label105 = new System.Windows.Forms.Label();
		this.txtGDValue = new System.Windows.Forms.TextBox();
		this.label107 = new System.Windows.Forms.Label();
		this.txtGDNoBreakUp = new System.Windows.Forms.TextBox();
		this.label102 = new System.Windows.Forms.Label();
		this.txtInvoiceNoGDBreakUp = new System.Windows.Forms.TextBox();
		this.label100 = new System.Windows.Forms.Label();
		this.datGDDateGDBreakUp = new System.Windows.Forms.DateTimePicker();
		this.label101 = new System.Windows.Forms.Label();
		this.datInvoiceDateGdBreakUp = new System.Windows.Forms.DateTimePicker();
		this.label99 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label23 = new System.Windows.Forms.Label();
		this.toolStrip4 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).BeginInit();
		this.panel30.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankName).BeginInit();
		this.panel1.SuspendLayout();
		this.toolStrip4.SuspendLayout();
		base.SuspendLayout();
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1125, 676);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel1);
		this.tabPage1.Controls.Add(this.toolStrip4);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1117, 650);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel30);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(3, 58);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1111, 589);
		this.panel3.TabIndex = 7;
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.grdGdBreakUp);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 107);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1111, 482);
		this.panel4.TabIndex = 607;
		this.grdGdBreakUp.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdGdBreakUp).BackColor = System.Drawing.Color.White;
		grdGdBreakUp_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdGdBreakUp.DesignTimeLayout = grdGdBreakUp_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdGdBreakUp.FilterMode = (FilterMode)2;
		this.grdGdBreakUp.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdGdBreakUp.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Name = "grdGdBreakUp";
		((ScrollableBase)this.grdGdBreakUp).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdGdBreakUp).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 0);
		((ScrollableBase)this.grdGdBreakUp).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).Size = new System.Drawing.Size(1111, 482);
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).TabIndex = 605;
		this.grdGdBreakUp.TotalRow = (InheritableBoolean)1;
		this.grdGdBreakUp.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdGdBreakUp).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdGdBreakUp).DoubleClick += new System.EventHandler(grdGdBreakUp_DoubleClick);
		this.panel30.Controls.Add(this.txtRemarks);
		this.panel30.Controls.Add(this.label121);
		this.panel30.Controls.Add(this.txtFttAmount);
		this.panel30.Controls.Add(this.label120);
		this.panel30.Controls.Add(this.txtExchangeRate);
		this.panel30.Controls.Add(this.label119);
		this.panel30.Controls.Add(this.txtNetAmount);
		this.panel30.Controls.Add(this.label118);
		this.panel30.Controls.Add(this.txtFTTPrcnt);
		this.panel30.Controls.Add(this.label117);
		this.panel30.Controls.Add(this.txtFobValue);
		this.panel30.Controls.Add(this.label116);
		this.panel30.Controls.Add(this.txtFreight);
		this.panel30.Controls.Add(this.label110);
		this.panel30.Controls.Add(this.txtDueDays);
		this.panel30.Controls.Add(this.label106);
		this.panel30.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBankName);
		this.panel30.Controls.Add(this.label105);
		this.panel30.Controls.Add(this.txtGDValue);
		this.panel30.Controls.Add(this.label107);
		this.panel30.Controls.Add(this.txtGDNoBreakUp);
		this.panel30.Controls.Add(this.label102);
		this.panel30.Controls.Add(this.txtInvoiceNoGDBreakUp);
		this.panel30.Controls.Add(this.label100);
		this.panel30.Controls.Add(this.datGDDateGDBreakUp);
		this.panel30.Controls.Add(this.label101);
		this.panel30.Controls.Add(this.datInvoiceDateGdBreakUp);
		this.panel30.Controls.Add(this.label99);
		this.panel30.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel30.Location = new System.Drawing.Point(0, 0);
		this.panel30.Name = "panel30";
		this.panel30.Size = new System.Drawing.Size(1111, 107);
		this.panel30.TabIndex = 606;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(685, 75);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(295, 23);
		this.txtRemarks.TabIndex = 14;
		this.label121.AutoSize = true;
		this.label121.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label121.Location = new System.Drawing.Point(687, 55);
		this.label121.Name = "label121";
		this.label121.Size = new System.Drawing.Size(52, 15);
		this.label121.TabIndex = 614;
		this.label121.Text = "Remarks";
		this.txtFttAmount.Enabled = false;
		this.txtFttAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFttAmount.Location = new System.Drawing.Point(424, 75);
		this.txtFttAmount.Name = "txtFttAmount";
		this.txtFttAmount.Size = new System.Drawing.Size(127, 23);
		this.txtFttAmount.TabIndex = 12;
		this.txtFttAmount.Text = "0";
		this.txtFttAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFttAmount.TextChanged += new System.EventHandler(txtFttAmount_TextChanged);
		this.txtFttAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFttAmount_KeyPress);
		this.label120.AutoSize = true;
		this.label120.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label120.Location = new System.Drawing.Point(425, 55);
		this.label120.Name = "label120";
		this.label120.Size = new System.Drawing.Size(74, 15);
		this.label120.TabIndex = 612;
		this.label120.Text = "FTT Amount";
		this.txtExchangeRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtExchangeRate.Location = new System.Drawing.Point(895, 27);
		this.txtExchangeRate.Name = "txtExchangeRate";
		this.txtExchangeRate.Size = new System.Drawing.Size(85, 23);
		this.txtExchangeRate.TabIndex = 7;
		this.txtExchangeRate.Text = "0";
		this.txtExchangeRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtExchangeRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtExchangeRate_KeyPress);
		this.label119.AutoSize = true;
		this.label119.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label119.Location = new System.Drawing.Point(896, 7);
		this.label119.Name = "label119";
		this.label119.Size = new System.Drawing.Size(84, 15);
		this.label119.TabIndex = 610;
		this.label119.Text = "Exchange Rate";
		this.txtNetAmount.Enabled = false;
		this.txtNetAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetAmount.Location = new System.Drawing.Point(552, 75);
		this.txtNetAmount.Name = "txtNetAmount";
		this.txtNetAmount.Size = new System.Drawing.Size(132, 23);
		this.txtNetAmount.TabIndex = 13;
		this.txtNetAmount.Text = "0";
		this.txtNetAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtNetAmount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtNetAmount_KeyPress);
		this.label118.AutoSize = true;
		this.label118.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label118.Location = new System.Drawing.Point(553, 55);
		this.label118.Name = "label118";
		this.label118.Size = new System.Drawing.Size(73, 15);
		this.label118.TabIndex = 608;
		this.label118.Text = "Net Amount";
		this.txtFTTPrcnt.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFTTPrcnt.Location = new System.Drawing.Point(353, 75);
		this.txtFTTPrcnt.Name = "txtFTTPrcnt";
		this.txtFTTPrcnt.Size = new System.Drawing.Size(70, 23);
		this.txtFTTPrcnt.TabIndex = 11;
		this.txtFTTPrcnt.Text = "10";
		this.txtFTTPrcnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFTTPrcnt.TextChanged += new System.EventHandler(txtFTTPrcnt_TextChanged);
		this.txtFTTPrcnt.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFTTPrcnt_KeyPress);
		this.label117.AutoSize = true;
		this.label117.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label117.Location = new System.Drawing.Point(354, 55);
		this.label117.Name = "label117";
		this.label117.Size = new System.Drawing.Size(40, 15);
		this.label117.TabIndex = 606;
		this.label117.Text = "% FTT";
		this.txtFobValue.Enabled = false;
		this.txtFobValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFobValue.Location = new System.Drawing.Point(237, 75);
		this.txtFobValue.Name = "txtFobValue";
		this.txtFobValue.Size = new System.Drawing.Size(115, 23);
		this.txtFobValue.TabIndex = 10;
		this.txtFobValue.Text = "0";
		this.txtFobValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFobValue.TextChanged += new System.EventHandler(txtFobValue_TextChanged);
		this.txtFobValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFobValue_KeyPress);
		this.label116.AutoSize = true;
		this.label116.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label116.Location = new System.Drawing.Point(238, 55);
		this.label116.Name = "label116";
		this.label116.Size = new System.Drawing.Size(61, 15);
		this.label116.TabIndex = 604;
		this.label116.Text = "FOB Value";
		this.txtFreight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFreight.Location = new System.Drawing.Point(121, 75);
		this.txtFreight.Name = "txtFreight";
		this.txtFreight.Size = new System.Drawing.Size(115, 23);
		this.txtFreight.TabIndex = 9;
		this.txtFreight.Text = "0";
		this.txtFreight.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtFreight.TextChanged += new System.EventHandler(txtFreight_TextChanged);
		this.txtFreight.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFreight_KeyPress);
		this.label110.AutoSize = true;
		this.label110.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label110.Location = new System.Drawing.Point(122, 55);
		this.label110.Name = "label110";
		this.label110.Size = new System.Drawing.Size(44, 15);
		this.label110.TabIndex = 602;
		this.label110.Text = "Freight";
		this.txtDueDays.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDueDays.Location = new System.Drawing.Point(834, 27);
		this.txtDueDays.Name = "txtDueDays";
		this.txtDueDays.Size = new System.Drawing.Size(60, 23);
		this.txtDueDays.TabIndex = 6;
		this.txtDueDays.Text = "0";
		this.txtDueDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtDueDays.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDueDays_KeyPress);
		this.label106.AutoSize = true;
		this.label106.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label106.Location = new System.Drawing.Point(833, 7);
		this.label106.Name = "label106";
		this.label106.Size = new System.Drawing.Size(58, 15);
		this.label106.TabIndex = 600;
		this.label106.Text = "Due Days";
		this.CmbBankName.AllowNull = (DefaultableBoolean)1;
		this.CmbBankName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBankName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBankName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance541).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance541).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance541;
		((UltraGridBase)this.CmbBankName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBankName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance542).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance542).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance542).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance542).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance542;
		((AppearanceBase)appearance543).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance543;
		((SpecialBoxBase)((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance544).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance544).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance544).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance544).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance544;
		((UltraGridBase)this.CmbBankName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBankName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance545).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance545).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance545;
		((AppearanceBase)appearance546).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance546).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance546;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance547).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance547;
		((AppearanceBase)appearance548).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance548).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance548;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance549).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance549).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance549).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance549).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance549).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance549;
		((AppearanceBase)appearance550).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance550;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance551).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance551).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance551;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance552).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBankName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance552;
		((UltraGridBase)this.CmbBankName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBankName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBankName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBankName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBankName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBankName).Location = new System.Drawing.Point(633, 25);
		((System.Windows.Forms.Control)(object)this.CmbBankName).Name = "CmbBankName";
		((System.Windows.Forms.Control)(object)this.CmbBankName).Size = new System.Drawing.Size(200, 26);
		((System.Windows.Forms.Control)(object)this.CmbBankName).TabIndex = 5;
		this.label105.AutoSize = true;
		this.label105.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label105.Location = new System.Drawing.Point(633, 5);
		this.label105.Name = "label105";
		this.label105.Size = new System.Drawing.Size(68, 15);
		this.label105.TabIndex = 598;
		this.label105.Text = "Bank Name";
		this.txtGDValue.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGDValue.Location = new System.Drawing.Point(5, 75);
		this.txtGDValue.Name = "txtGDValue";
		this.txtGDValue.Size = new System.Drawing.Size(115, 23);
		this.txtGDValue.TabIndex = 8;
		this.txtGDValue.Text = "0";
		this.txtGDValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtGDValue.TextChanged += new System.EventHandler(txtGDValue_TextChanged);
		this.txtGDValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtGDValue_KeyPress);
		this.label107.AutoSize = true;
		this.label107.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label107.Location = new System.Drawing.Point(6, 55);
		this.label107.Name = "label107";
		this.label107.Size = new System.Drawing.Size(56, 15);
		this.label107.TabIndex = 513;
		this.label107.Text = "GD Value";
		this.txtGDNoBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtGDNoBreakUp.Location = new System.Drawing.Point(6, 27);
		this.txtGDNoBreakUp.Name = "txtGDNoBreakUp";
		this.txtGDNoBreakUp.Size = new System.Drawing.Size(230, 23);
		this.txtGDNoBreakUp.TabIndex = 1;
		this.label102.AutoSize = true;
		this.label102.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label102.Location = new System.Drawing.Point(3, 7);
		this.label102.Name = "label102";
		this.label102.Size = new System.Drawing.Size(43, 15);
		this.label102.TabIndex = 509;
		this.label102.Text = "GD No";
		this.txtInvoiceNoGDBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtInvoiceNoGDBreakUp.Location = new System.Drawing.Point(364, 27);
		this.txtInvoiceNoGDBreakUp.Name = "txtInvoiceNoGDBreakUp";
		this.txtInvoiceNoGDBreakUp.Size = new System.Drawing.Size(139, 23);
		this.txtInvoiceNoGDBreakUp.TabIndex = 3;
		this.label100.AutoSize = true;
		this.label100.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label100.Location = new System.Drawing.Point(366, 7);
		this.label100.Name = "label100";
		this.label100.Size = new System.Drawing.Size(94, 15);
		this.label100.TabIndex = 509;
		this.label100.Text = "Bank Invoice No";
		this.datGDDateGDBreakUp.CustomFormat = "dd-MMM-yy";
		this.datGDDateGDBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datGDDateGDBreakUp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datGDDateGDBreakUp.Location = new System.Drawing.Point(237, 27);
		this.datGDDateGDBreakUp.Name = "datGDDateGDBreakUp";
		this.datGDDateGDBreakUp.Size = new System.Drawing.Size(125, 23);
		this.datGDDateGDBreakUp.TabIndex = 2;
		this.label101.AutoSize = true;
		this.label101.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label101.Location = new System.Drawing.Point(502, 7);
		this.label101.Name = "label101";
		this.label101.Size = new System.Drawing.Size(103, 15);
		this.label101.TabIndex = 593;
		this.label101.Text = "Bank Invoice Date";
		this.datInvoiceDateGdBreakUp.CustomFormat = "dd-MMM-yy";
		this.datInvoiceDateGdBreakUp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.datInvoiceDateGdBreakUp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.datInvoiceDateGdBreakUp.Location = new System.Drawing.Point(506, 27);
		this.datInvoiceDateGdBreakUp.Name = "datInvoiceDateGdBreakUp";
		this.datInvoiceDateGdBreakUp.Size = new System.Drawing.Size(125, 23);
		this.datInvoiceDateGdBreakUp.TabIndex = 4;
		this.label99.AutoSize = true;
		this.label99.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label99.Location = new System.Drawing.Point(233, 7);
		this.label99.Name = "label99";
		this.label99.Size = new System.Drawing.Size(52, 15);
		this.label99.TabIndex = 593;
		this.label99.Text = "GD Date";
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(3, 30);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1111, 28);
		this.panel1.TabIndex = 6;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1069, 2);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdGdBreakUp;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar1.TabIndex = 66;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(5, 4);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(170, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "GD Breakup Manual";
		this.toolStrip4.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip4.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.btnsave, this.btnupdate });
		this.toolStrip4.Location = new System.Drawing.Point(3, 3);
		this.toolStrip4.Name = "toolStrip4";
		this.toolStrip4.Size = new System.Drawing.Size(1111, 27);
		this.toolStrip4.TabIndex = 5;
		this.toolStrip4.Text = "toolStrip4";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnupdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1125, 676);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "GdBreakUpManual";
		this.Text = "GD Breakup Manual";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(GdBreakUpManual_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GdBreakUpManual_KeyDown);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.tabPage1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdGdBreakUp).EndInit();
		this.panel30.ResumeLayout(false);
		this.panel30.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbBankName).EndInit();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip4.ResumeLayout(false);
		this.toolStrip4.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
