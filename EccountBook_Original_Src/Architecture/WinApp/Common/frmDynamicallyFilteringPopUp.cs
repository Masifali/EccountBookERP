using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.WinApp.Helper;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.Common;

public class frmDynamicallyFilteringPopUp : Form
{
	public DataTable dtReturn = new DataTable();

	public DataTable _sourceTable = new DataTable();

	private Dictionary<string, Control> _filterControls = new Dictionary<string, Control>();

	private Dictionary<string, object> _lastFilterValues = new Dictionary<string, object>();

	private IContainer components;

	private Panel pnlTop;

	private Button btnClear;

	private Button btnShow;

	private FlowLayoutPanel flpFilters;

	internal GridEX grd;

	public frmDynamicallyFilteringPopUp(DataTable dt, List<FilterColumnConfig> filters, List<string> hiddenColumns = null)
	{
		try
		{
			InitializeComponent();
			_sourceTable = dt;
			BindGrid(hiddenColumns);
			BuildFilterUI(filters);
			base.Shown += delegate
			{
				((Control)(object)grd).Focus();
			};
			flpFilters.Visible = true;
			pnlTop.Visible = true;
			base.WindowState = FormWindowState.Normal;
			base.StartPosition = FormStartPosition.CenterScreen;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGrid(List<string> hiddenColumns)
	{
		if (_sourceTable != null)
		{
			grd.DataSource = _sourceTable;
			grd.RetrieveStructure();
			GridEX_Helper.GridWrappingAndColumnSettings(grd, 2, 2, hiddenColumns);
			grd.GroupByBoxVisible = true;
			((ScrollableBase)grd).RecordNavigator = true;
			grd.ColumnAutoResize = false;
			((Control)(object)grd).Focus();
		}
	}

	private void BuildFilterUI(List<FilterColumnConfig> _filters)
	{
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Expected O, but got Unknown
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Expected O, but got Unknown
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Expected O, but got Unknown
		//IL_00ea: Expected O, but got Unknown
		flpFilters.Controls.Clear();
		_filterControls.Clear();
		foreach (FilterColumnConfig f in _filters)
		{
			flpFilters.Controls.Add(new Label
			{
				Text = f.DisplayText,
				AutoSize = true,
				Margin = new Padding(6, 12, 2, 0)
			});
			Control ctrl;
			if (f.FilterType == FilterType.DateRange)
			{
				ctrl = new DateTimePicker
				{
					Width = f.Width
				};
			}
			else
			{
				UltraCombo val = new UltraCombo();
				((Control)val).Width = f.Width;
				((Control)val).Dock = DockStyle.Fill;
				((Control)val).Font = new Font("Verdana", 10f);
				DataTable dtCombo = GetComboDt(_sourceTable, f);
				InfragisticsHelper.BindAndRetainSelection(val, dtCombo, f.ColumnName, f.DisplayText, f.DisplayText, AllColumns: true, null, insertDefaultRow: false, null, null, ActivateRow: false);
				ctrl = (Control)val;
			}
			ctrl.Tag = f;
			flpFilters.Controls.Add(ctrl);
			_filterControls.Add(f.ColumnName, ctrl);
		}
		btnShow.Click += BtnShow_Click;
		btnClear.Click += BtnClear_Click;
	}

	private DataTable GetComboDt(DataTable sourceDt, FilterColumnConfig filter)
	{
		DataTable dt = new DataTable();
		dt.Columns.Add(filter.ColumnName, typeof(string));
		dt.Columns.Add(filter.DisplayText, typeof(string));
		HashSet<string> uniqueValues = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		foreach (DataRow dr in sourceDt.Rows)
		{
			if (dr[filter.ColumnName] != DBNull.Value && dr[filter.DisplayText] != DBNull.Value)
			{
				string value = dr[filter.DisplayText].ToString();
				if (!string.IsNullOrWhiteSpace(value) && uniqueValues.Add(value))
				{
					dt.Rows.Add(dr[filter.ColumnName]?.ToString(), dr[filter.DisplayText]?.ToString());
				}
			}
		}
		return dt;
	}

	private void BtnShow_Click(object sender, EventArgs e)
	{
		List<string> conditions = new List<string>();
		foreach (KeyValuePair<string, Control> kv in _filterControls)
		{
			FilterColumnConfig config = (FilterColumnConfig)kv.Value.Tag;
			Control value = kv.Value;
			UltraCombo combo = (UltraCombo)(object)((value is UltraCombo) ? value : null);
			if (combo != null && combo.Value != null)
			{
				string val = combo.Value.ToString().Replace("'", "''");
				if (config.FilterType == FilterType.Contains)
				{
					conditions.Add("[" + config.ColumnName + "] LIKE '%" + val.ToString() + "%'");
				}
				else
				{
					conditions.Add("[" + config.ColumnName + "] = '" + val + "'");
				}
				_lastFilterValues[config.ColumnName] = combo.Value.ToString();
			}
			if (kv.Value is DateTimePicker dt)
			{
				conditions.Add($"[{config.ColumnName}] >= #{dt.Value:MM/dd/yyyy}#");
				_lastFilterValues[config.ColumnName] = dt.Value;
			}
		}
		_sourceTable.DefaultView.RowFilter = string.Join(" AND ", conditions);
	}

	private void BtnClear_Click(object sender, EventArgs e)
	{
		foreach (Control value in _filterControls.Values)
		{
			UltraCombo c = (UltraCombo)(object)((value is UltraCombo) ? value : null);
			if (c != null)
			{
				c.Value = null;
			}
			if (value is DateTimePicker d)
			{
				d.Value = DateTime.Today;
			}
		}
		_sourceTable.DefaultView.RowFilter = string.Empty;
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grd.CurrentRow;
			if (row != null && (int)row.RowType == 0)
			{
				dtReturn = (from r in grd.GetDataRows()
					where (int)r.RowType == 0
					select ((DataRowView)r.DataRow).Row into dr
					where dr == ((DataRowView)row.DataRow).Row
					select dr).CopyToDataTable();
				base.DialogResult = DialogResult.OK;
				Close();
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

	private void frmDynamicallyFilteringPopUp_KeyDown(object sender, KeyEventArgs e)
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

	private void frmDynamicallyFilteringPopUp_Load(object sender, EventArgs e)
	{
		AutoScroll = true;
		base.AutoScrollMargin = new Size(20, 20);
		base.AutoScrollMinSize = new Size(750, 400);
		base.Width = 800;
		base.Height = 550;
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
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		this.pnlTop = new System.Windows.Forms.Panel();
		this.btnClear = new System.Windows.Forms.Button();
		this.btnShow = new System.Windows.Forms.Button();
		this.flpFilters = new System.Windows.Forms.FlowLayoutPanel();
		this.grd = new GridEX();
		this.pnlTop.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		base.SuspendLayout();
		this.pnlTop.Controls.Add(this.btnClear);
		this.pnlTop.Controls.Add(this.btnShow);
		this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTop.Location = new System.Drawing.Point(0, 55);
		this.pnlTop.Name = "pnlTop";
		this.pnlTop.Size = new System.Drawing.Size(511, 26);
		this.pnlTop.TabIndex = 4;
		this.pnlTop.Visible = false;
		this.btnClear.BackColor = System.Drawing.Color.Teal;
		this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnClear.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnClear.ForeColor = System.Drawing.Color.White;
		this.btnClear.Location = new System.Drawing.Point(59, 1);
		this.btnClear.Name = "btnClear";
		this.btnClear.Size = new System.Drawing.Size(56, 23);
		this.btnClear.TabIndex = 0;
		this.btnClear.Text = "Reset";
		this.btnClear.UseVisualStyleBackColor = false;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(3, 1);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(56, 23);
		this.btnShow.TabIndex = 0;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.flpFilters.Dock = System.Windows.Forms.DockStyle.Top;
		this.flpFilters.Location = new System.Drawing.Point(0, 0);
		this.flpFilters.Name = "flpFilters";
		this.flpFilters.Size = new System.Drawing.Size(511, 55);
		this.flpFilters.TabIndex = 3;
		this.flpFilters.Visible = false;
		this.grd.AllowEdit = (InheritableBoolean)2;
		this.grd.AlternatingColors = true;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		this.grd.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grd.FilterMode = (FilterMode)2;
		this.grd.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		this.grd.FilterRowUpdateMode = (FilterRowUpdateMode)2;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(0, 81);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		this.grd.RowHeaders = (InheritableBoolean)1;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(511, 334);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 5;
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
		base.Controls.Add(this.pnlTop);
		base.Controls.Add(this.flpFilters);
		base.KeyPreview = true;
		base.Name = "frmDynamicallyFilteringPopUp";
		this.Text = "EccountBook ERP";
		base.Load += new System.EventHandler(frmDynamicallyFilteringPopUp_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDynamicallyFilteringPopUp_KeyDown);
		this.pnlTop.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		base.ResumeLayout(false);
	}
}
