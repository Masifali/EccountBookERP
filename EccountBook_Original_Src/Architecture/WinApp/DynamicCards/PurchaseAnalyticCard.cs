using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Janus.Data;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.DynamicCards;

public class PurchaseAnalyticCard : UserControl
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable _dt;

	private int _Id;

	private int _ItemId;

	private string _Caption;

	private string _RequestFor;

	private string _ParentIds;

	public bool LableClicked;

	private IContainer components;

	internal GridEX grdfrm;

	public LinkLabel lblTitle;

	public DataTable dataTable
	{
		get
		{
			return _dt;
		}
		set
		{
			_dt = value;
		}
	}

	public int Id
	{
		get
		{
			return _Id;
		}
		set
		{
			_Id = value;
		}
	}

	public int ItemId
	{
		get
		{
			return _ItemId;
		}
		set
		{
			_ItemId = value;
		}
	}

	public string Caption
	{
		get
		{
			return _Caption;
		}
		set
		{
			_Caption = value;
			lblTitle.Text = value;
		}
	}

	public string RequestFor
	{
		get
		{
			return _RequestFor;
		}
		set
		{
			_RequestFor = value;
		}
	}

	public string ParentIds
	{
		get
		{
			return _ParentIds;
		}
		set
		{
			_ParentIds = value;
		}
	}

	public event EventHandler UserControlClicked;

	public PurchaseAnalyticCard(UserAccount pUserAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUserAccount;
		lblTitle.Click += ControlClicked;
		grdfrm.LinkClicked += new ColumnActionEventHandler(ControlClicked);
	}

	private void ControlClicked(object sender, EventArgs e)
	{
		this.UserControlClicked?.Invoke(this, e);
	}

	private void PurchaseAnalyticCard_Load(object sender, EventArgs e)
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (dataTable.Rows.Count > 0)
			{
				grdfrm.DataSource = dataTable;
				grdfrm.RetrieveStructure();
				grdfrm.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
				}
				grdfrm.RootTable.Columns["SortNo"].Width = 45;
				grdfrm.RootTable.Columns["Supplier"].Width = 190;
				grdfrm.RootTable.Columns["Descriptions"].Width = 100;
				grdfrm.RootTable.Columns["Qty"].Width = 60;
				grdfrm.RootTable.Columns["Weight"].Width = 70;
				grdfrm.RootTable.Columns["ItemRate"].Width = 70;
				grdfrm.RootTable.Columns["Exp40Kg"].Width = 70;
				grdfrm.RootTable.Columns["AvgRate"].Width = 70;
				grdfrm.RootTable.Columns["Exp+Amount"].Width = 80;
				grdfrm.RootTable.Columns["AnalysisQty"].Width = 60;
				grdfrm.RootTable.Columns["Moisture"].Width = 50;
				grdfrm.RootTable.Columns["Empty Shell / Trash"].Width = 45;
				grdfrm.RootTable.Columns["Dust / Stone"].Width = 45;
				grdfrm.RootTable.Columns["Broken"].Width = 45;
				grdfrm.RootTable.Columns["AGL"].Width = 45;
				grdfrm.RootTable.Columns["Damage"].Width = 45;
				grdfrm.RootTable.Columns["Empty Shell / Trash"].Visible = false;
				grdfrm.RootTable.Columns["Dust / Stone"].Visible = false;
				grdfrm.RootTable.Columns["AGL"].Visible = false;
				grdfrm.RootTable.Columns["Damage"].Visible = false;
				grdfrm.RootTable.Columns["Qty"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Qty"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["Qty"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["Weight"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Weight"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["Weight"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["ItemRate"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["ItemRate"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["Exp40Kg"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Exp40Kg"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["AnalysisQty"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["AnalysisQty"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["AnalysisQty"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Moisture"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["Moisture"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["Empty Shell / Trash"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Empty Shell / Trash"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["Empty Shell / Trash"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["Dust / Stone"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Dust / Stone"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["Dust / Stone"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["AGL"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["AGL"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Broken"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["Broken"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Damage"].FormatString = "#,##0";
				grdfrm.RootTable.Columns["Damage"].TotalFormatString = "#,##0";
				grdfrm.RootTable.Columns["AvgRate"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["Exp+Amount"].AggregateFunction = (AggregateFunction)2;
				grdfrm.RootTable.Columns["Exp+Amount"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Exp+Amount"].FormatString = clsGlobalVariables.stringFormatsingle;
				grdfrm.RootTable.Columns["Exp+Amount"].TotalFormatString = clsGlobalVariables.stringFormatsingle;
				if (!string.IsNullOrEmpty(ParentIds) && ParentIds.Contains("2"))
				{
					grdfrm.RootTable.Columns["Moisture"].Visible = false;
				}
				else
				{
					grdfrm.RootTable.Columns["Broken"].Visible = false;
				}
				if (RequestFor == "Product_Wise_Summ")
				{
					base.Size = new Size(500, 200);
					grdfrm.RootTable.Columns["Descriptions"].Width = 100;
					grdfrm.RootTable.Columns["Qty"].Width = 60;
					grdfrm.RootTable.Columns["Weight"].Width = 70;
					grdfrm.RootTable.Columns["ItemRate"].Width = 70;
					grdfrm.RootTable.Columns["Exp40Kg"].Width = 70;
					grdfrm.RootTable.Columns["AvgRate"].Width = 80;
					grdfrm.RootTable.Columns["Supplier"].Visible = false;
					grdfrm.RootTable.Columns["Exp+Amount"].Visible = false;
					grdfrm.RootTable.Columns["Descriptions"].ColumnType = (ColumnType)5;
					grdfrm.RootTable.Columns["Moisture"].Visible = false;
					grdfrm.RootTable.Columns["Broken"].Visible = false;
					grdfrm.RootTable.Columns["AnalysisQty"].Visible = false;
				}
				else if (RequestFor == "Product_Wise")
				{
					base.Size = new Size(600, 200);
					grdfrm.RootTable.Columns["Supplier"].Visible = false;
					grdfrm.RootTable.Columns["Exp+Amount"].Visible = false;
					grdfrm.RootTable.Columns["Descriptions"].ColumnType = (ColumnType)5;
				}
				else if (RequestFor == "ProductAndPartyWise")
				{
					base.Size = new Size(770, 200);
					grdfrm.RootTable.Columns["Descriptions"].Visible = false;
					grdfrm.TotalRow = (InheritableBoolean)1;
				}
			}
			else
			{
				grdfrm.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblTitle_Click(object sender, EventArgs e)
	{
		try
		{
			LableClicked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			LableClicked = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lblTitle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
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
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Expected O, but got Unknown
		GridEXLayout grdfrm_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DynamicCards.PurchaseAnalyticCard));
		this.grdfrm = new GridEX();
		this.lblTitle = new System.Windows.Forms.LinkLabel();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		base.SuspendLayout();
		this.grdfrm.AllowDelete = (InheritableBoolean)1;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		grdfrm_DesignTimeLayout.LayoutString = resources.GetString("grdfrm_DesignTimeLayout.LayoutString");
		this.grdfrm.DesignTimeLayout = grdfrm_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdfrm.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(835, 172);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 85;
		((System.Windows.Forms.Control)(object)this.grdfrm).TabStop = false;
		((System.Windows.Forms.Control)(object)this.grdfrm).Tag = "GrdClick";
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		this.lblTitle.BackColor = System.Drawing.Color.Teal;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.lblTitle.ForeColor = System.Drawing.Color.White;
		this.lblTitle.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
		this.lblTitle.LinkColor = System.Drawing.Color.White;
		this.lblTitle.Location = new System.Drawing.Point(0, 0);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(835, 28);
		this.lblTitle.TabIndex = 86;
		this.lblTitle.TabStop = true;
		this.lblTitle.Text = "Paddy";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTitle.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(lblTitle_LinkClicked);
		this.lblTitle.Click += new System.EventHandler(lblTitle_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.lblTitle);
		base.Name = "PurchaseAnalyticCard";
		base.Size = new System.Drawing.Size(835, 200);
		base.Load += new System.EventHandler(PurchaseAnalyticCard_Load);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		base.ResumeLayout(false);
	}
}
