using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.Common;
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
namespace Architecture.WinApp.Common
{

public class GrdPopUpMultiSelection : Form
{
	private DataTable data = new DataTable();

	public DataTable dtReturn = new DataTable();

	private string valueMember = "";

	private string DisplayMember = "";

	private string DisplayCode = "";

	private IContainer components;

	internal GridEX grd;

	public GrdPopUpMultiSelection(DataTable dt, string ValueMember, string displayMember, string displaydode = "")
	{
		try
		{
			InitializeComponent();
			dtReturn.Columns.Add("ReturnId", typeof(int));
			dtReturn.Columns.Add("ReturnName", typeof(string));
			dtReturn.Columns.Add("ReturnCode", typeof(string));
			dtReturn.Clear();
			valueMember = ValueMember;
			DisplayMember = displayMember;
			DisplayCode = displaydode;
			data.Rows.Clear();
			if (string.IsNullOrEmpty(displaydode))
			{
				data.Columns.Add(ValueMember);
				data.Columns.Add(displayMember);
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					data.Rows.Add(dt.Rows[i][ValueMember], dt.Rows[i][displayMember]);
				}
			}
			else
			{
				data.Columns.Add(ValueMember);
				data.Columns.Add(displayMember);
				data.Columns.Add(displaydode);
				for (int j = 0; j < dt.Rows.Count; j++)
				{
					data.Rows.Add(dt.Rows[j][ValueMember], dt.Rows[j][displayMember], dt.Rows[j][displaydode]);
				}
			}
			grd.DataSource = data;
			grd.RetrieveStructure();
			grd.RootTable.Columns[ValueMember].Visible = false;
			grd.RootTable.Columns.Add("Select");
			grd.RootTable.Columns["Select"].ActAsSelector = true;
			grd.RootTable.Columns["Select"].UseHeaderSelector = true;
			grd.RootTable.Columns["Select"].Width = 40;
			grd.RootTable.Columns["Select"].Position = 0;
			((Control)(object)grd).Focus();
			SendKeys.Send("{Up}");
			SendKeys.Send("{Right}");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (int.Parse(grd.GetCheckedRows().Count().ToString()) > 0)
			{
				GridEXRow[] checkedRows = grd.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					dtReturn.Rows.Add(Conversion.ToInt(r.Cells[valueMember].Value), Conversion.ToString((object)r.Cells[DisplayMember].Text), Conversion.ToString((object)r.Cells[DisplayMember].Text));
				}
				Hide();
			}
			else
			{
				GridEXRow row = grd.CurrentRow;
				if (row != null && (int)row.RowType == 0)
				{
					dtReturn.Rows.Add(Conversion.ToInt(row.Cells[valueMember].Value), Conversion.ToString((object)row.Cells[DisplayMember].Text), Conversion.ToString((object)row.Cells[DisplayMember].Text));
					Close();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_Enter(object sender, EventArgs e)
	{
	}

	private void GrdPopUp_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.KeyCode == Keys.Return)
			{
				grd_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	private void GrdPopUp_Load(object sender, EventArgs e)
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
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.grd = new GridEX();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(511, 415);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).Enter += new System.EventHandler(grd_Enter);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(511, 415);
		base.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		base.KeyPreview = true;
		base.Name = "GrdPopUp";
		this.Text = "GrdPopUp";
		base.Load += new System.EventHandler(GrdPopUp_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(GrdPopUp_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}

}
