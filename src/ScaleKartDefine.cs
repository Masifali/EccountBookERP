using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Lookups
{

public class ScaleKartDefine : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DataTable dtHistory = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private Panel panel2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private Panel panel3;

	private Label label23;

	private Panel panel4;

	private Label label4;

	private DateTimePicker DateFrom;

	internal UltraCombo CmbKartType;

	private Label label1;

	private Panel panel5;

	private Label label5;

	private TextBox txtScaleKart;

	private Label label3;

	private TextBox txtRangeTo;

	private Label label2;

	private TextBox txtRangeFrom;

	internal GridEX grdHistory;

	public ScaleKartDefine(UserAccount pUser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private void KartTypeFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Type");
			dt.Rows.Add(1, "Quantity");
			dt.Rows.Add(2, "Weight");
			DDL.BindDDL(dt, CmbKartType, "Id", "Type", "Type", ZeroIndex: false);
			((UltraGridBase)CmbKartType).Rows[0].Activate();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScaleKartDefine_Load(object sender, EventArgs e)
	{
		try
		{
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			KartTypeFill();
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRangeFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtRangeTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtScaleKart_KeyPress(object sender, KeyPressEventArgs e)
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

	private void Reset()
	{
		try
		{
			DateFrom.Value = DateTime.Now;
			txtRangeFrom.Clear();
			txtRangeTo.Clear();
			txtScaleKart.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		try
		{
			_ = DateFrom.Value;
			if (txtRangeFrom.Text.Trim() == string.Empty || Conversion.ToInt((object)txtRangeFrom.Text) == 0)
			{
				MessageBox.Show("Range From Field is Required");
				txtRangeFrom.Focus();
				return false;
			}
			if (txtRangeTo.Text.Trim() == string.Empty)
			{
				MessageBox.Show("Range To Field is Required");
				txtRangeTo.Focus();
				return false;
			}
			if (txtScaleKart.Text.Trim() == string.Empty)
			{
				MessageBox.Show("Scale Kart Field is Required");
				txtScaleKart.Focus();
				return false;
			}
			if (((Control)(object)CmbKartType).Text.Trim() == string.Empty || Convert.ToInt32(CmbKartType.Value) == 0)
			{
				MessageBox.Show("Type Field is Required");
				CmbKartType.Focus();
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			ScaleKartDefine obj = new ScaleKartDefine();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			if (Conversion.ToInt((object)txtRangeFrom.Text) > Conversion.ToInt((object)txtRangeTo.Text))
			{
				MessageBox.Show("'RangeFrom' Can't Be Greater Than 'RangeTo'");
				return;
			}
			if (dtHistory.Rows.Count > 0)
			{
				DateTime MAxDate = Conversion.ToDateTime((object)grdHistory.GetRows().Max((GridEXRow o) => Conversion.ToDateTime(o.Cells["EffectiveDate"].Value)));
				if (DateFrom.Value.Date < MAxDate.Date)
				{
					MessageBox.Show("You Can't Add Record Against " + DateFrom.Value.ToString("dd/MM/yyyy"));
					return;
				}
				if (MAxDate.Date == DateFrom.Value.Date)
				{
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						DateTime effectiveDate = Conversion.ToDateTime(dtHistory.Rows[i]["EffectiveDate"]);
						if (!(DateFrom.Value.Date == effectiveDate.Date))
						{
							continue;
						}
						if (((Control)(object)CmbKartType).Text == Conversion.ToString(dtHistory.Rows[i]["Type"]) && DateFrom.Value.Date == effectiveDate.Date)
						{
							if (Conversion.ToDouble((object)txtRangeFrom.Text) >= Conversion.ToDouble(dtHistory.Rows[i]["RangeFrom"]) && Conversion.ToDouble((object)txtRangeTo.Text) <= Conversion.ToDouble(dtHistory.Rows[i]["RangeTo"]))
							{
								MessageBox.Show("You Can't Add Record In Same Or Between The Range against Same Date!");
								return;
							}
							if (Conversion.ToDouble((object)txtRangeFrom.Text) >= Conversion.ToDouble(dtHistory.Rows[i]["RangeFrom"]) && Conversion.ToDouble((object)txtRangeFrom.Text) <= Conversion.ToDouble(dtHistory.Rows[i]["RangeTo"]))
							{
								MessageBox.Show("Range From Shoud not between in Already Defined Range!");
								return;
							}
							if (Conversion.ToDouble((object)txtRangeTo.Text) >= Conversion.ToDouble(dtHistory.Rows[i]["RangeFrom"]) && Conversion.ToDouble((object)txtRangeTo.Text) <= Conversion.ToDouble(dtHistory.Rows[i]["RangeTo"]))
							{
								MessageBox.Show("Range To Shoud not between in Already Defined Range!");
								return;
							}
							if (txtScaleKart.Text == Conversion.ToString(dtHistory.Rows[i]["ScaleKart"]))
							{
								MessageBox.Show("Can't Add " + txtScaleKart.Text + "kg Scale Kart In " + DateFrom.Value.ToString("dd/MM/yyyy"));
								return;
							}
							if (Conversion.ToDouble((object)txtRangeTo.Text) < Conversion.ToDouble(dtHistory.Rows[i]["RangeFrom"]))
							{
								if (Conversion.ToDouble((object)txtScaleKart.Text) > Conversion.ToDouble(dtHistory.Rows[i]["ScaleKart"]))
								{
									MessageBox.Show("Can't Add Greater Cart in This Range!");
									return;
								}
							}
							else if (Conversion.ToDouble((object)txtRangeFrom.Text) > Conversion.ToDouble(dtHistory.Rows[i]["RangeTo"]) && Conversion.ToDouble((object)txtScaleKart.Text) < Conversion.ToDouble(dtHistory.Rows[i]["ScaleKart"]))
							{
								MessageBox.Show("Can't Add Smaller Cart in This Range!");
								return;
							}
							continue;
						}
						MessageBox.Show("Can't Add Diffrent Type In Same Date");
						return;
					}
				}
			}
			obj.EffectiveDate = DateFrom.Value;
			obj.RangeFrom = Conversion.ToDouble((object)txtRangeFrom.Text.Trim());
			obj.RangeTo = Conversion.ToDouble((object)txtRangeTo.Text.Trim());
			obj.Type = Conversion.ToString((object)((Control)(object)CmbKartType).Text);
			obj.ScaleKart = Conversion.ToDouble((object)txtScaleKart.Text.Trim());
			obj.EntryDate = DateTime.Now;
			obj.EntryUserId = UserAccount.ID;
			obj.ModifyUserId = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.OrganizationId = Conversion.ToInt((object)UserAccount.OrganizationId);
			obj.CompanyId = Conversion.ToInt((object)UserAccount.CompanyId);
			int success = ScaleKart.save(obj);
			if (RecId == 0)
			{
				MessageBox.Show("Record Save Successfully...[" + success + "]");
			}
			else
			{
				MessageBox.Show("Record Update Successfully...[" + success + "]");
			}
			Reset();
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ScaleKartDefine obj = new ScaleKartDefine();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtHistory = ScaleKart.FormHistory(obj);
			if (dtHistory.Rows.Count > 0 && dtHistory != null)
			{
				grdHistory.DataSource = dtHistory;
				grdHistory.RetrieveStructure();
				grdHistory.RootTable.Columns["Id"].Visible = false;
			}
			else
			{
				grdHistory.ClearStructure();
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
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Expected O, but got Unknown
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
		this.panel1 = new System.Windows.Forms.Panel();
		this.grdHistory = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.txtScaleKart = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtRangeTo = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.txtRangeFrom = new System.Windows.Forms.TextBox();
		this.CmbKartType = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.DateFrom = new System.Windows.Forms.DateTimePicker();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistory).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbKartType).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistory);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(772, 523);
		this.panel1.TabIndex = 0;
		this.grdHistory.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdHistory.DynamicFiltering = true;
		this.grdHistory.FilterMode = (FilterMode)2;
		this.grdHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistory).Location = new System.Drawing.Point(0, 129);
		((System.Windows.Forms.Control)(object)this.grdHistory).Name = "grdHistory";
		((ScrollableBase)this.grdHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistory).Size = new System.Drawing.Size(772, 394);
		((System.Windows.Forms.Control)(object)this.grdHistory).TabIndex = 49;
		((System.Windows.Forms.Control)(object)this.grdHistory).TabStop = false;
		this.grdHistory.TotalRow = (InheritableBoolean)1;
		this.grdHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistory).VisualStyle = (VisualStyle)4;
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtScaleKart);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtRangeTo);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.txtRangeFrom);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbKartType);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.DateFrom);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 71);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(772, 58);
		this.panel4.TabIndex = 2;
		this.panel5.Location = new System.Drawing.Point(0, 61);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(963, 342);
		this.panel5.TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(491, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(66, 17);
		this.label5.TabIndex = 211;
		this.label5.Text = "Scale Kart";
		this.txtScaleKart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtScaleKart.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScaleKart.Location = new System.Drawing.Point(489, 24);
		this.txtScaleKart.Name = "txtScaleKart";
		this.txtScaleKart.Size = new System.Drawing.Size(117, 25);
		this.txtScaleKart.TabIndex = 212;
		this.txtScaleKart.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtScaleKart_KeyPress);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(373, 3);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(63, 17);
		this.label3.TabIndex = 209;
		this.label3.Text = "Range To";
		this.txtRangeTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRangeTo.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRangeTo.Location = new System.Drawing.Point(370, 24);
		this.txtRangeTo.Name = "txtRangeTo";
		this.txtRangeTo.Size = new System.Drawing.Size(116, 25);
		this.txtRangeTo.TabIndex = 210;
		this.txtRangeTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRangeTo_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(254, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(79, 17);
		this.label2.TabIndex = 207;
		this.label2.Text = "Range From";
		this.txtRangeFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRangeFrom.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRangeFrom.Location = new System.Drawing.Point(250, 24);
		this.txtRangeFrom.Name = "txtRangeFrom";
		this.txtRangeFrom.Size = new System.Drawing.Size(116, 25);
		this.txtRangeFrom.TabIndex = 208;
		this.txtRangeFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRangeFrom_KeyPress);
		this.CmbKartType.AllowNull = (DefaultableBoolean)1;
		this.CmbKartType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbKartType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbKartType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbKartType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbKartType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbKartType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbKartType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbKartType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbKartType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbKartType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbKartType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbKartType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbKartType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbKartType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbKartType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbKartType).Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbKartType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbKartType).Location = new System.Drawing.Point(126, 24);
		((System.Windows.Forms.Control)(object)this.CmbKartType).Name = "CmbKartType";
		((System.Windows.Forms.Control)(object)this.CmbKartType).Size = new System.Drawing.Size(120, 27);
		((System.Windows.Forms.Control)(object)this.CmbKartType).TabIndex = 205;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(128, 3);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(63, 17);
		this.label1.TabIndex = 206;
		this.label1.Text = "Kart Type";
		this.DateFrom.CustomFormat = "dd-MMM-yy";
		this.DateFrom.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DateFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DateFrom.Location = new System.Drawing.Point(14, 26);
		this.DateFrom.Name = "DateFrom";
		this.DateFrom.Size = new System.Drawing.Size(108, 23);
		this.DateFrom.TabIndex = 204;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(13, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(80, 15);
		this.label4.TabIndex = 203;
		this.label4.Text = "Effective Date";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 29);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(772, 42);
		this.panel3.TabIndex = 1;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(6, 8);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(153, 24);
		this.label23.TabIndex = 26;
		this.label23.Text = "Scale Kart Define";
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(772, 29);
		this.panel2.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnnew, this.btnUpdate, this.btnsave });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(772, 27);
		this.toolStrip1.TabIndex = 4;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(55, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(69, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnsave.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(55, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(772, 523);
		base.Controls.Add(this.panel1);
		base.Name = "ScaleKartDefine";
		this.Text = "Scale Kart Define";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(ScaleKartDefine_Load);
		this.panel1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdHistory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbKartType).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
