using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Production;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Production;

public class LoadConsumptionPendingforRates : Form
{
	private bool DoHaveCanRateandAmount;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dthis = new DataTable();

	private UserAccount UserAccount = new UserAccount();

	public int OutPutDocId;

	private IContainer components;

	private Button btngrnlod;

	private DateTimePicker Todate;

	private DateTimePicker FromDate;

	private Label label13;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel3;

	internal GridEX grd;

	private Label label2;

	internal UltraCombo CmbJobOrder;

	private Button btnReset;

	private Panel panel7;

	internal GridEX grddetail;

	private Panel panel8;

	private Panel panel6;

	private Panel panel5;

	private Panel panel4;

	private Panel panel1;

	private Label label23;

	private Label label3;

	private TextBox txtfromdoc;

	private Label label64;

	private TextBox txtToDoc;

	private Label label4;

	private Label label5;

	public LoadConsumptionPendingforRates(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void LoadInvoices_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 600);
			FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
			FromDate.Focus();
			JobOrderFill();
			OutputGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void JobOrderFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		try
		{
			ReportsParameters val = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 403,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			};
			DataTable dtPJO = new DataTable();
			dtPJO = InvProductionJobOrder.GetJobOrderNoForInvFoodProduction(val);
			if (dtPJO.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("PlanCode");
				for (int i = 0; i < dtPJO.Rows.Count; i++)
				{
					dt.Rows.Add(dtPJO.Rows[i]["Id"], dtPJO.Rows[i]["PlanCode"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbJobOrder, "Id", "PlanCode", "JobOrderNo", ZeroIndex: false);
				}
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

	private void btnclose_Click(object sender, EventArgs e)
	{
		OutPutDocId = 0;
		Hide();
	}

	private void btngrnlod_Click(object sender, EventArgs e)
	{
		try
		{
			OutputGridHistory();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void OutputGridHistory()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FromDate = Conversion.ToDateTime((object)FromDate.Value);
			obj.ToDate = Conversion.ToDateTime((object)Todate.Value);
			obj.JobOrderId = Conversion.ToInt(CmbJobOrder.Value);
			dthis = InvFoodProduction.GetProductionConsumptionDataForPendingRates(obj);
			if (dthis.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtgrdoutput = new DataTable();
			dtgrdoutput.Columns.Add("Id", typeof(int));
			dtgrdoutput.Columns.Add("JobOrderNo", typeof(string));
			dtgrdoutput.Columns.Add("DocDate", typeof(DateTime));
			dtgrdoutput.Columns.Add("DocCode", typeof(int));
			dtgrdoutput.Columns.Add("MainRemarks", typeof(string));
			dtgrdoutput.Columns.Add("TotalQty", typeof(double));
			dtgrdoutput.Columns.Add("TotalWeight", typeof(double));
			dtgrdoutput.Columns.Add("EntryDate", typeof(DateTime));
			dtgrdoutput.Columns.Add("EntryUser", typeof(string));
			dtgrdoutput.Columns.Add("ModifyDate", typeof(string));
			dtgrdoutput.Columns.Add("ModifyUser", typeof(string));
			foreach (var row2 in (from row in dthis.AsEnumerable()
				group row by row["Id"]).Select(delegate(IGrouping<object, DataRow> group)
			{
				double totalItemQty = group.Sum((DataRow row) => Conversion.ToDouble(row["Qty"]));
				double totalWeight = group.Average((DataRow row) => Conversion.ToDouble(row["Weight"]));
				return new
				{
					Id = group.Key,
					JobOrderNo = group.First()["JobOrderNo"],
					DocDate = Conversion.ToDateTime(group.First()["DocDate"]).ToShortDateString(),
					DocCode = (int)group.First()["DocCode"],
					MainRemarks = group.First()["MainRemarks"],
					TotalItemQty = totalItemQty,
					TotalWeight = totalWeight,
					EntryDate = Conversion.ToDateTime(group.First()["EntryDate"]).ToShortDateString(),
					EntryUser = group.First()["EntryUser"],
					ModifyDate = Conversion.ToDateTime(group.First()["ModifyDate"]).ToShortDateString(),
					ModifyUser = group.First()["ModifyUser"]
				};
			}))
			{
				dtgrdoutput.Rows.Add(row2.Id, row2.JobOrderNo, row2.DocDate, row2.DocCode, row2.MainRemarks, row2.TotalItemQty, row2.TotalWeight, row2.EntryDate, row2.EntryUser, row2.ModifyDate, row2.ModifyUser);
			}
			grd.DataSource = dtgrdoutput;
			grd.RetrieveStructure();
			GridOutputSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridOutputSetting()
	{
		try
		{
			grd.RootTable.Columns.Add("Load");
			grd.RootTable.Columns["Load"].Key = "Load";
			grd.RootTable.Columns["Load"].Caption = "Load";
			grd.RootTable.Columns["Load"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Load"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Load"].ButtonText = "Load";
			grd.RootTable.Columns["Load"].Width = 40;
			grd.RootTable.Columns["Load"].Position = 0;
			grd.FrozenColumns = 1;
			grd.RootTable.Columns["Id"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grd);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
		OutputGridHistory();
		FromDate.Value = clsGlobalVariables.ActiveYr.Start_Period;
		FromDate.Focus();
		((Control)(object)CmbJobOrder).Text = string.Empty;
		grd.DataSource = null;
		grd.RetrieveStructure();
		grddetail.DataSource = null;
		grddetail.RetrieveStructure();
	}

	private void txtToDoc_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void txtfromdoc_KeyPress(object sender, KeyPressEventArgs e)
	{
		CommonServices.OnlytextNumberFunction(sender, e);
	}

	private void grd_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow Item = grd.CurrentRow;
			if (Item == null || (int)Item.RowType != 0 || Item.Cells.Count <= 0 || dthis.Rows.Count <= 0)
			{
				return;
			}
			DataTable dtgrdoutput = new DataTable();
			dtgrdoutput.Columns.Add("DetailId", typeof(int));
			dtgrdoutput.Columns.Add("EntryType", typeof(string));
			dtgrdoutput.Columns.Add("ItemName", typeof(string));
			dtgrdoutput.Columns.Add("Cropyear", typeof(string));
			dtgrdoutput.Columns.Add("WareHouseName", typeof(string));
			dtgrdoutput.Columns.Add("JobLotDescription", typeof(string));
			dtgrdoutput.Columns.Add("PackType", typeof(string));
			dtgrdoutput.Columns.Add("PackUom", typeof(string));
			dtgrdoutput.Columns.Add("Qty", typeof(double));
			dtgrdoutput.Columns.Add("Weight", typeof(double));
			dtgrdoutput.Clear();
			for (int i = 0; i < dthis.Rows.Count; i++)
			{
				if (Conversion.ToInt(Item.Cells["Id"].Value) == Conversion.ToInt(dthis.Rows[i]["Id"]))
				{
					dtgrdoutput.Rows.Add(dthis.Rows[i]["DetailId"], dthis.Rows[i]["EntryType"], dthis.Rows[i]["ItemName"], dthis.Rows[i]["CropBatch"], dthis.Rows[i]["WareHouseName"], dthis.Rows[i]["JobLotDescription"], dthis.Rows[i]["PackTypeDesc"], dthis.Rows[i]["PackUom"], dthis.Rows[i]["Qty"], dthis.Rows[i]["Weight"]);
				}
			}
			grddetail.DataSource = dtgrdoutput;
			grddetail.RetrieveStructure();
			GriddetailOutputSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GriddetailOutputSetting()
	{
		try
		{
			grddetail.RootTable.Columns["DetailId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grddetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Load")
			{
				OutPutDocId = Conversion.ToInt(grd.CurrentRow.Cells["Id"].Value);
				Hide();
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
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Expected O, but got Unknown
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Expected O, but got Unknown
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Expected O, but got Unknown
		//IL_08d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_08dd: Expected O, but got Unknown
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grddetail_DesignTimeLayout = new GridEXLayout();
		this.btnReset = new System.Windows.Forms.Button();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbJobOrder = new UltraCombo();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grd = new GridEX();
		this.btngrnlod = new System.Windows.Forms.Button();
		this.Todate = new System.Windows.Forms.DateTimePicker();
		this.FromDate = new System.Windows.Forms.DateTimePicker();
		this.label13 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.grddetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label3 = new System.Windows.Forms.Label();
		this.txtfromdoc = new System.Windows.Forms.TextBox();
		this.label64 = new System.Windows.Forms.Label();
		this.txtToDoc = new System.Windows.Forms.TextBox();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.panel3.SuspendLayout();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetail).BeginInit();
		this.panel8.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel1.SuspendLayout();
		base.SuspendLayout();
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(841, 6);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(61, 26);
		this.btnReset.TabIndex = 6;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(452, 12);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(68, 17);
		this.label2.TabIndex = 24;
		this.label2.Text = "Job Order";
		this.CmbJobOrder.AllowNull = (DefaultableBoolean)1;
		this.CmbJobOrder.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbJobOrder.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbJobOrder.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance7).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance7).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance10;
		((AppearanceBase)appearance11).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance11).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbJobOrder).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbJobOrder.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Location = new System.Drawing.Point(526, 7);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Name = "CmbJobOrder";
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).Size = new System.Drawing.Size(236, 26);
		((System.Windows.Forms.Control)(object)this.CmbJobOrder).TabIndex = 4;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(915, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grd;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 26);
		this.ctrlGrdBar1.TabIndex = 10;
		this.ctrlGrdBar1.Visible = false;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(964, 290);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 0;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		this.grd.SelectionChanged += new System.EventHandler(grd_SelectionChanged);
		this.btngrnlod.BackColor = System.Drawing.Color.Teal;
		this.btngrnlod.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btngrnlod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btngrnlod.ForeColor = System.Drawing.Color.White;
		this.btngrnlod.Location = new System.Drawing.Point(768, 6);
		this.btngrnlod.Name = "btngrnlod";
		this.btngrnlod.Size = new System.Drawing.Size(72, 26);
		this.btngrnlod.TabIndex = 5;
		this.btngrnlod.Text = "Search";
		this.btngrnlod.UseVisualStyleBackColor = false;
		this.btngrnlod.Click += new System.EventHandler(btngrnlod_Click);
		this.Todate.CustomFormat = "dd-MMM-yy";
		this.Todate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Todate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Todate.Location = new System.Drawing.Point(86, 37);
		this.Todate.Name = "Todate";
		this.Todate.Size = new System.Drawing.Size(187, 23);
		this.Todate.TabIndex = 1;
		this.FromDate.CustomFormat = "dd-MMM-yy";
		this.FromDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDate.Location = new System.Drawing.Point(86, 9);
		this.FromDate.Name = "FromDate";
		this.FromDate.Size = new System.Drawing.Size(187, 23);
		this.FromDate.TabIndex = 0;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(12, 40);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(54, 17);
		this.label13.TabIndex = 11;
		this.label13.Text = "To Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(12, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(72, 17);
		this.label1.TabIndex = 12;
		this.label1.Text = "From Date";
		this.panel3.Controls.Add(this.panel7);
		this.panel3.Controls.Add(this.panel8);
		this.panel3.Controls.Add(this.panel6);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Controls.Add(this.panel4);
		this.panel3.Controls.Add(this.panel1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(964, 561);
		this.panel3.TabIndex = 31;
		this.panel7.Controls.Add((System.Windows.Forms.Control)(object)this.grddetail);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 440);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(964, 121);
		this.panel7.TabIndex = 8;
		this.grddetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grddetail).BackColor = System.Drawing.Color.White;
		grddetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetail.DesignTimeLayout = grddetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetail.FilterMode = (FilterMode)2;
		this.grddetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grddetail).Name = "grddetail";
		((ScrollableBase)this.grddetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetail).Size = new System.Drawing.Size(964, 121);
		((System.Windows.Forms.Control)(object)this.grddetail).TabIndex = 0;
		this.grddetail.TotalRow = (InheritableBoolean)1;
		this.grddetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 416);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(964, 24);
		this.panel8.TabIndex = 7;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(3, 3);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(211, 17);
		this.label4.TabIndex = 25;
		this.label4.Text = "Detail Of Above Selected Row";
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 126);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(964, 290);
		this.panel6.TabIndex = 6;
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 98);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(964, 28);
		this.panel5.TabIndex = 5;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(6, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(40, 17);
		this.label5.TabIndex = 26;
		this.label5.Text = "Main";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtfromdoc);
		this.panel4.Controls.Add(this.label64);
		this.panel4.Controls.Add(this.txtToDoc);
		this.panel4.Controls.Add(this.btnReset);
		this.panel4.Controls.Add(this.FromDate);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.Todate);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.btngrnlod);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.CmbJobOrder);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(964, 69);
		this.panel4.TabIndex = 0;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(280, 13);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 15);
		this.label3.TabIndex = 210;
		this.label3.Text = "Doc No from";
		this.txtfromdoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtfromdoc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtfromdoc.Location = new System.Drawing.Point(357, 9);
		this.txtfromdoc.Name = "txtfromdoc";
		this.txtfromdoc.Size = new System.Drawing.Size(88, 23);
		this.txtfromdoc.TabIndex = 2;
		this.txtfromdoc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtfromdoc_KeyPress);
		this.label64.AutoSize = true;
		this.label64.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label64.Location = new System.Drawing.Point(280, 41);
		this.label64.Name = "label64";
		this.label64.Size = new System.Drawing.Size(64, 15);
		this.label64.TabIndex = 208;
		this.label64.Text = "Doc No To";
		this.txtToDoc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtToDoc.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDoc.Location = new System.Drawing.Point(357, 37);
		this.txtToDoc.Name = "txtToDoc";
		this.txtToDoc.Size = new System.Drawing.Size(88, 23);
		this.txtToDoc.TabIndex = 3;
		this.txtToDoc.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDoc_KeyPress);
		this.panel1.BackColor = System.Drawing.Color.Teal;
		this.panel1.Controls.Add(this.label23);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 29);
		this.panel1.TabIndex = 3;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(7, 5);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(269, 19);
		this.label23.TabIndex = 24;
		this.label23.Text = "Pending Consumption For Rates";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 561);
		base.Controls.Add(this.panel3);
		base.Name = "LoadConsumptionPendingforRates";
		this.Text = "ProductionOutPutLoader";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(LoadInvoices_Load);
		((System.ComponentModel.ISupportInitialize)this.CmbJobOrder).EndInit();
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
