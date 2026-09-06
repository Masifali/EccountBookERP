using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Pos;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Reports;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.WholeSale;

public class SalePriceListWithDiscount : Form
{
	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtDiscountGrid = new DataTable();

	private double RoundingFactor;

	private int[] AryColWidth;

	private IContainer components;

	private Panel panel1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnRefresh;

	private Panel panel4;

	private Label label15;

	private Panel panel2;

	private Label label10;

	private DateTimePicker txtEffectedDate;

	internal UltraCombo cmbItemCategory;

	private Label label4;

	private Button btnShow;

	internal GridEX datagrid;

	private ToolStripButton btnRegister;

	private CtrlGrdBar ctrlGrdBar1;

	public SalePriceListWithDiscount(UserAccount Puser)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = Puser;
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			GetItemPriceList();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GetItemPriceList()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Expected O, but got Unknown
		try
		{
			DataTable dt = ItemPricingSchedule.GetItemSalePriceListWithDiscounts(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				EffectedDate = txtEffectedDate.Value,
				ItemCategoryId = Conversion.ToInt(cmbItemCategory.Value)
			});
			if (dt.Rows.Count > 0)
			{
				dtDiscountGrid.Rows.Clear();
				if (dtDiscountGrid.Columns.Count == 0)
				{
					dtDiscountGrid.Columns.Add("CategoryDescription");
					dtDiscountGrid.Columns.Add("EffectedDate", typeof(DateTime));
					dtDiscountGrid.Columns.Add("ItemName");
					dtDiscountGrid.Columns.Add("RateUom");
					dtDiscountGrid.Columns.Add("ItemPrice", typeof(double));
					dtDiscountGrid.Columns.Add("1%Dicount", typeof(double));
					dtDiscountGrid.Columns.Add("2%Dicount", typeof(double));
					dtDiscountGrid.Columns.Add("3%Dicount", typeof(double));
					dtDiscountGrid.Columns.Add("4%Dicount", typeof(double));
					dtDiscountGrid.Columns.Add("5%Dicount", typeof(double));
					dtDiscountGrid.Columns.Add("6%Dicount", typeof(double));
				}
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					DataRowCollection rows = dtDiscountGrid.Rows;
					object[] obj = new object[11]
					{
						dt.Rows[i]["CategoryDescription"],
						Conversion.ToDateTime(dt.Rows[i]["EffectedDate"]).ToShortDateString(),
						dt.Rows[i]["ItemName"],
						dt.Rows[i]["RateUom"],
						dt.Rows[i]["ItemPrice"],
						null,
						null,
						null,
						null,
						null,
						null
					};
					object obj3 = (dt.Rows[i]["1%Dicount"] = GeneratePriceForPercent(Conversion.ToDouble(dt.Rows[i]["ItemPrice"]), RoundingFactor, 0.01));
					obj[5] = obj3;
					obj3 = (dt.Rows[i]["2%Dicount"] = GeneratePriceForPercent(Conversion.ToDouble(dt.Rows[i]["ItemPrice"]), RoundingFactor, 0.02));
					obj[6] = obj3;
					obj3 = (dt.Rows[i]["3%Dicount"] = GeneratePriceForPercent(Conversion.ToDouble(dt.Rows[i]["ItemPrice"]), RoundingFactor, 0.03));
					obj[7] = obj3;
					obj3 = (dt.Rows[i]["4%Dicount"] = GeneratePriceForPercent(Conversion.ToDouble(dt.Rows[i]["ItemPrice"]), RoundingFactor, 0.04));
					obj[8] = obj3;
					obj3 = (dt.Rows[i]["5%Dicount"] = GeneratePriceForPercent(Conversion.ToDouble(dt.Rows[i]["ItemPrice"]), RoundingFactor, 0.05));
					obj[9] = obj3;
					obj3 = (dt.Rows[i]["6%Dicount"] = GeneratePriceForPercent(Conversion.ToDouble(dt.Rows[i]["ItemPrice"]), RoundingFactor, 0.06));
					obj[10] = obj3;
					rows.Add(obj);
				}
				datagrid.DataSource = dtDiscountGrid;
				datagrid.RetrieveStructure();
				datagrid.ColumnAutoResize = false;
				datagrid.RootTable.Columns["ItemPrice"].TextAlignment = (TextAlignment)3;
				datagrid.RootTable.Columns["1%Dicount"].TextAlignment = (TextAlignment)3;
				datagrid.RootTable.Columns["2%Dicount"].TextAlignment = (TextAlignment)3;
				datagrid.RootTable.Columns["3%Dicount"].TextAlignment = (TextAlignment)3;
				datagrid.RootTable.Columns["4%Dicount"].TextAlignment = (TextAlignment)3;
				datagrid.RootTable.Columns["5%Dicount"].TextAlignment = (TextAlignment)3;
				datagrid.RootTable.Columns["6%Dicount"].TextAlignment = (TextAlignment)3;
				datagrid.RootTable.Columns["ItemPrice"].FormatString = "#,#";
				datagrid.RootTable.Columns["1%Dicount"].FormatString = "#,#";
				datagrid.RootTable.Columns["2%Dicount"].FormatString = "#,#";
				datagrid.RootTable.Columns["3%Dicount"].FormatString = "#,#";
				datagrid.RootTable.Columns["4%Dicount"].FormatString = "#,#";
				datagrid.RootTable.Columns["5%Dicount"].FormatString = "#,#";
				datagrid.RootTable.Columns["6%Dicount"].FormatString = "#,#";
				datagrid.RootTable.Columns["1%Dicount"].Caption = "BRONZE";
				datagrid.RootTable.Columns["2%Dicount"].Caption = "PEARL";
				datagrid.RootTable.Columns["3%Dicount"].Caption = "SILVER";
				datagrid.RootTable.Columns["4%Dicount"].Caption = "GOLD";
				datagrid.RootTable.Columns["5%Dicount"].Caption = "PLATINUM";
				datagrid.RootTable.Columns["6%Dicount"].Caption = "DIAMOND";
				CommonServices.GridAutoAdjustment(AryColWidth, datagrid);
				ctrlGrdBar1_Load(null, null);
			}
			else
			{
				dtDiscountGrid.Rows.Clear();
				datagrid.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void SalePriceListWithDiscount_Load(object sender, EventArgs e)
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "RoundingForItemPricing").ToList();
			if (list.Count > 0)
			{
				RoundingFactor = Conversion.ToDouble((object)list[0].ConfigKey.ToString());
			}
			ItemCategoryFill();
			GetItemPriceList();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private double GeneratePriceForPercent(double Price, double RoundingBy, double Percent)
	{
		double discountedPrice = Price * (1.0 - Percent);
		double roundingFactor = Math.Round(discountedPrice / RoundingBy, 0) * RoundingBy - discountedPrice;
		double num = Math.Round((discountedPrice + roundingFactor) / RoundingBy, 0) * RoundingBy;
		double roundedPriceUp = num + RoundingBy;
		double roundedPriceDown = num;
		if (Math.Abs(discountedPrice - roundedPriceUp) <= Math.Abs(discountedPrice - roundedPriceDown))
		{
			return roundedPriceUp;
		}
		return roundedPriceDown;
	}

	private void ItemCategoryFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			DataTable dtItemCategory = CommonServices.GetItemCategoryFromPricingShedule(6);
			if (dtItemCategory.Rows.Count > 0)
			{
				for (int i = 0; i < dtItemCategory.Rows.Count; i++)
				{
					dt.Rows.Add(dtItemCategory.Rows[i]["ItemCategoryId"], dtItemCategory.Rows[i]["CategoryDescription"]);
				}
				DDL.BindDDL(dt, cmbItemCategory, "Id", "Name", "Item Category", ZeroIndex: false);
				((UltraGridBase)cmbItemCategory).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			ItemCategoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnRegister_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			Reporting val = new Reporting();
			val.RptPerameter("CompanyName", clsGlobalVariables.UserAccount.CompName.ToString());
			val.RptPerameter("CompanyAddress", clsGlobalVariables.UserAccount.CompAddress.ToString());
			val.ShowReportWithDataTable(dtDiscountGrid, "616-GetItemSalePriceListRegister.rpt");
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)datagrid).Name);
			if (fs != null)
			{
				datagrid.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SalePriceListWithDiscount_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Expected O, but got Unknown
		GridEXLayout datagrid_DesignTimeLayout = new GridEXLayout();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.datagrid = new GridEX();
		this.panel2 = new System.Windows.Forms.Panel();
		this.btnShow = new System.Windows.Forms.Button();
		this.cmbItemCategory = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtEffectedDate = new System.Windows.Forms.DateTimePicker();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnRegister = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.datagrid).BeginInit();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).BeginInit();
		this.panel4.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.ctrlGrdBar1);
		this.panel1.Controls.Add((System.Windows.Forms.Control)(object)this.datagrid);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(980, 630);
		this.panel1.TabIndex = 0;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(931, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.datagrid;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.datagrid.AllowEdit = (InheritableBoolean)2;
		this.datagrid.ColumnAutoResize = true;
		this.datagrid.DefaultFilterRowComparison = (FilterConditionOperator)7;
		datagrid_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.datagrid.DesignTimeLayout = datagrid_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.datagrid).Dock = System.Windows.Forms.DockStyle.Fill;
		this.datagrid.DynamicFiltering = true;
		this.datagrid.FilterMode = (FilterMode)2;
		this.datagrid.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.datagrid).Font = new System.Drawing.Font("Verdana", 8.25f);
		((System.Windows.Forms.Control)(object)this.datagrid).Location = new System.Drawing.Point(0, 101);
		((System.Windows.Forms.Control)(object)this.datagrid).Name = "datagrid";
		((ScrollableBase)this.datagrid).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.datagrid).Size = new System.Drawing.Size(980, 529);
		((System.Windows.Forms.Control)(object)this.datagrid).TabIndex = 11;
		this.datagrid.TotalRow = (InheritableBoolean)1;
		this.datagrid.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.datagrid).VisualStyle = (VisualStyle)4;
		this.panel2.Controls.Add(this.btnShow);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemCategory);
		this.panel2.Controls.Add(this.label4);
		this.panel2.Controls.Add(this.label10);
		this.panel2.Controls.Add(this.txtEffectedDate);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 61);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(980, 40);
		this.panel2.TabIndex = 10;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(550, 8);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(55, 23);
		this.btnShow.TabIndex = 586;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.cmbItemCategory.AllowNull = (DefaultableBoolean)1;
		this.cmbItemCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance14;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance15).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance15).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance15).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance15;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance16;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance17).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance17).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance18;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance19).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Location = new System.Drawing.Point(302, 6);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Name = "cmbItemCategory";
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).Size = new System.Drawing.Size(243, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemCategory).TabIndex = 584;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(218, 12);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 585;
		this.label4.Text = "Item Category";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.label10.Location = new System.Drawing.Point(16, 12);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(78, 15);
		this.label10.TabIndex = 578;
		this.label10.Text = "Effected Date";
		this.txtEffectedDate.CustomFormat = "dd-MMM-yy";
		this.txtEffectedDate.Font = new System.Drawing.Font("Calibri", 9.75f);
		this.txtEffectedDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtEffectedDate.Location = new System.Drawing.Point(104, 8);
		this.txtEffectedDate.Name = "txtEffectedDate";
		this.txtEffectedDate.Size = new System.Drawing.Size(109, 23);
		this.txtEffectedDate.TabIndex = 577;
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 27);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(980, 34);
		this.panel4.TabIndex = 9;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Verdana", 14.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(9, 6);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(287, 23);
		this.label15.TabIndex = 26;
		this.label15.Text = "Sale Price List With Discount";
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.btnRefresh, this.btnRegister });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(980, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(70, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnRegister.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRegister.Name = "btnRegister";
		this.btnRegister.Size = new System.Drawing.Size(79, 24);
		this.btnRegister.Text = "616-Print";
		this.btnRegister.Click += new System.EventHandler(btnRegister_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(980, 630);
		base.Controls.Add(this.panel1);
		base.Name = "SalePriceListWithDiscount";
		this.Text = "Sale Price List With Discount";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(SalePriceListWithDiscount_FormClosing);
		base.Load += new System.EventHandler(SalePriceListWithDiscount_Load);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.datagrid).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemCategory).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}
