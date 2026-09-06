using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.Common;
using Architecture.Model;
using Janus.Data;
using Janus.Windows.GridEX;
using InheritableBoolean = Janus.Windows.GridEX.InheritableBoolean;
using GroupTotals = Janus.Windows.GridEX.GroupTotals;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.DynamicCards
{

public class LabPurchaseAnalyticCard : UserControl
{
	public UserAccount UserAccount = new UserAccount();

	private DataTable _dt;

	private int _Id;

	private int _ItemId;

	private string _Caption;

	private string _RequestFor;

	public bool LableClicked;

	private IContainer components;

	private Label lblTitle;

	internal GridEX grdfrm;

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

	public event EventHandler UserControlClicked;

	public LabPurchaseAnalyticCard(UserAccount pUserAccount)
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
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
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
				grdfrm.RootTable.Columns["NoofVehicles"].Width = 65;
				grdfrm.RootTable.Columns["TotalQty"].Width = 63;
				grdfrm.RootTable.Columns["Moisture"].Width = 60;
				grdfrm.RootTable.Columns["GreenGrain"].Width = 60;
				grdfrm.RootTable.Columns["EmptyShell"].Width = 60;
				grdfrm.RootTable.Columns["Damage"].Width = 60;
				grdfrm.RootTable.Columns["Sherivalled"].Width = 75;
				grdfrm.RootTable.Columns["Fungus"].Width = 55;
				grdfrm.RootTable.Columns["AGL"].Width = 55;
				grdfrm.RootTable.Columns["Broken"].Width = 55;
				grdfrm.RootTable.Columns["NoofVehicles"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["NoofVehicles"].FormatString = "#,##0.###";
				grdfrm.RootTable.Columns["TotalQty"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["TotalQty"].FormatString = "#,##0.###";
				grdfrm.RootTable.Columns["Moisture"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Moisture"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["GreenGrain"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["GreenGrain"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["EmptyShell"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["EmptyShell"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["Damage"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Damage"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["Sherivalled"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Sherivalled"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["Fungus"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Fungus"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["AGL"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["AGL"].FormatString = "#,##0.####";
				grdfrm.RootTable.Columns["Broken"].TextAlignment = (TextAlignment)3;
				grdfrm.RootTable.Columns["Broken"].FormatString = "#,##0.####";
				if (RequestFor == "Product_Wise")
				{
					grdfrm.RootTable.Columns["SortNo"].Visible = false;
					grdfrm.RootTable.Columns["AvgRate"].Width = 85;
					grdfrm.RootTable.Columns["Description"].ColumnType = (ColumnType)5;
					grdfrm.RootTable.Columns["AvgRate"].TextAlignment = (TextAlignment)3;
					grdfrm.RootTable.Columns["AvgRate"].FormatString = clsGlobalVariables.DecimalRateFormate;
				}
				else
				{
					if (!(RequestFor == "ProductAndPartyWise"))
					{
						return;
					}
					grdfrm.RootTable.HeaderLines = 2;
					foreach (GridEXColumn item2 in (JanusCollectionBase)grdfrm.RootTable.Columns)
					{
						item2.HeaderAlignment = (TextAlignment)2;
					}
					grdfrm.RootTable.Columns["NoofVehicles"].Width = 70;
					grdfrm.RootTable.Columns["TotalQty"].Width = 65;
					grdfrm.RootTable.Columns["Moisture"].Width = 65;
					grdfrm.RootTable.Columns["GreenGrain"].Width = 65;
					grdfrm.RootTable.Columns["EmptyShell"].Width = 65;
					grdfrm.RootTable.Columns["Damage"].Width = 65;
					grdfrm.RootTable.Columns["Sherivalled"].Width = 75;
					grdfrm.RootTable.Columns["Fungus"].Width = 60;
					grdfrm.RootTable.Columns["AGL"].Width = 55;
					grdfrm.RootTable.Columns["Broken"].Width = 60;
					grdfrm.TotalRow = (InheritableBoolean)1;
					grdfrm.RootTable.Columns["TotalQty"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["TotalQty"].TotalFormatString = "#,##0.###";
					grdfrm.RootTable.Columns["NoofVehicles"].AggregateFunction = (AggregateFunction)2;
					grdfrm.RootTable.Columns["NoofVehicles"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["Moisture"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["Moisture"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["GreenGrain"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["GreenGrain"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["EmptyShell"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["EmptyShell"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["Damage"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["Damage"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["Sherivalled"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["Sherivalled"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["Fungus"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["Fungus"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["AGL"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["AGL"].TotalFormatString = "#,##0.####";
					grdfrm.RootTable.Columns["Broken"].AggregateFunction = (AggregateFunction)3;
					grdfrm.RootTable.Columns["Broken"].TotalFormatString = "#,##0.####";
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
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		this.lblTitle = new System.Windows.Forms.Label();
		this.grdfrm = new GridEX();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		base.SuspendLayout();
		this.lblTitle.BackColor = System.Drawing.Color.Teal;
		this.lblTitle.Cursor = System.Windows.Forms.Cursors.Hand;
		this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
		this.lblTitle.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTitle.ForeColor = System.Drawing.Color.White;
		this.lblTitle.Location = new System.Drawing.Point(0, 0);
		this.lblTitle.Name = "lblTitle";
		this.lblTitle.Size = new System.Drawing.Size(829, 28);
		this.lblTitle.TabIndex = 0;
		this.lblTitle.Tag = "LableClick";
		this.lblTitle.Text = "Paddy";
		this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.lblTitle.Click += new System.EventHandler(lblTitle_Click);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(829, 136);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 84;
		((System.Windows.Forms.Control)(object)this.grdfrm).Tag = "GrdClick";
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		base.Controls.Add(this.lblTitle);
		base.Name = "LabPurchaseAnalyticCard";
		base.Size = new System.Drawing.Size(829, 164);
		base.Load += new System.EventHandler(PurchaseAnalyticCard_Load);
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		base.ResumeLayout(false);
	}
}

}
