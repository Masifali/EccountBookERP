using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.ApprovalDashboard;

public class DepartmentRequestApproval : Form
{
	private DataTable dtGrid = new DataTable();

	public DateTime RequestedFromDate;

	public DateTime RequestedToDate;

	private DropDownBind DDL = new DropDownBind();

	private UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private Label lblCaption;

	private ToolStrip toolStrip1;

	private Panel panel3;

	private Panel panel2;

	internal GridEX grdfrm;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnApprove;

	private ToolStripButton BtnShortCutkeys;

	public DepartmentRequestApproval(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void Reset()
	{
		try
		{
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGatePassReport_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("DocumentTypeId", typeof(int));
			dtGrid.Columns.Add("DocDate", typeof(DateTime));
			dtGrid.Columns.Add("DocNo", typeof(int));
			dtGrid.Columns.Add("DepartmentNameFrom");
			dtGrid.Columns.Add("DepartmentNameTo");
			dtGrid.Columns.Add("ItemName");
			dtGrid.Columns.Add("RequestedQty", typeof(double));
			dtGrid.Columns.Add("AssetName");
			dtGrid.Columns.Add("EntryUser");
			dtGrid.Columns.Add("EntryDate", typeof(DateTime));
			dtGrid.Columns.Add("ModifyUser");
			dtGrid.Columns.Add("ModifyDate", typeof(DateTime));
			dtGrid.Columns.Add("NoOfAttachments", typeof(int));
			GridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			DataTable dt = DepartmentRequest.DepartmentRequestHistory(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				IsApproved = false,
				FromDate = Conversion.ToDateTime((object)RequestedFromDate),
				ToDate = Conversion.ToDateTime((object)RequestedToDate)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				dtGrid.Rows.Clear();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtGrid.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["DocTypeId"], Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), dt.Rows[i]["DocNo"], dt.Rows[i]["DepartmentNameFrom"], dt.Rows[i]["ToDepartmentName"], dt.Rows[i]["ItemName"], dt.Rows[i]["RequestedQty"], dt.Rows[i]["AssetName"], dt.Rows[i]["EntryUser"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUser"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["NoOfAttachments"]);
				}
				grdfrm.DataSource = dtGrid;
				grdfrm.RetrieveStructure();
				gridSetting();
			}
			else
			{
				grdfrm.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridSetting()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdfrm.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdfrm.RootTable.Columns)
			{
				item.HeaderStyle.TextAlignment = (TextAlignment)2;
			}
			grdfrm.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdfrm.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdfrm.RootTable.Columns["DepartmentNameFrom"].Width = Constants.InventoryConstants.WareHouseName;
			grdfrm.RootTable.Columns["DepartmentNameTo"].Width = Constants.InventoryConstants.WareHouseName;
			grdfrm.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grdfrm.RootTable.Columns["RequestedQty"].Width = Constants.InventoryConstants.QtyConstant + 5;
			grdfrm.RootTable.Columns["AssetName"].Width = Constants.InventoryConstants.StringConstant;
			grdfrm.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			grdfrm.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			grdfrm.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			grdfrm.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdfrm.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdfrm.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdfrm.RootTable.Columns["RequestedQty"].AggregateFunction = (AggregateFunction)2;
			grdfrm.RootTable.Columns["RequestedQty"].TextAlignment = (TextAlignment)3;
			grdfrm.RootTable.Columns["RequestedQty"].FormatString = "#,##0.000";
			grdfrm.RootTable.Columns["RequestedQty"].TotalFormatString = "#,##0.000";
			grdfrm.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdfrm.RootTable.Columns.Add("Select");
			grdfrm.RootTable.Columns["Select"].ActAsSelector = true;
			grdfrm.RootTable.Columns["Select"].UseHeaderSelector = true;
			grdfrm.RootTable.Columns["Select"].Width = 40;
			grdfrm.RootTable.Columns["Select"].Position = 0;
			grdfrm.RootTable.Columns.Add("Print");
			grdfrm.RootTable.Columns["Print"].Caption = "Print";
			grdfrm.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["Print"].ButtonText = "Print";
			grdfrm.RootTable.Columns["Print"].Width = 40;
			grdfrm.RootTable.Columns["Print"].Position = 1;
			grdfrm.RootTable.Columns.Add("Approve");
			grdfrm.RootTable.Columns["Approve"].Caption = "Approve";
			grdfrm.RootTable.Columns["Approve"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdfrm.RootTable.Columns["Approve"].ButtonStyle = (ButtonStyle)5;
			grdfrm.RootTable.Columns["Approve"].ButtonText = "Approve";
			grdfrm.RootTable.Columns["Approve"].Width = 60;
			grdfrm.RootTable.Columns["Approve"].Position = 2;
			grdfrm.FrozenColumns = 3;
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdfrm).Name);
			if (fs != null)
			{
				grdfrm.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void grdfrm_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		try
		{
			if (e.Column.Key == "Approve")
			{
				if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				ApprovalList val = new ApprovalList
				{
					AllApprovalLists = new List<ApprovalList>()
				};
				ApprovalList item = new ApprovalList();
				item.OrganizationId = UserAccount.OrganizationId;
				item.CompanyId = UserAccount.CompanyId;
				item.EntryUser = UserAccount.ID;
				item.ReqType = "Approve";
				item.Id = Conversion.ToInt(grdfrm.CurrentRow.Cells["Id"].Value);
				val.AllApprovalLists.Add(item);
				DepartmentRequest.DepartmentRequestApprovedForDepId(val);
				MessageBox.Show("Record Approved Successfully");
				Reset();
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.DepartmentSlip451(Conversion.ToInt(grdfrm.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnApprove_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (grdfrm.GetCheckedRows().Length == 0)
			{
				throw new Exception("Please check row first");
			}
			if (MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
			{
				ApprovalList obj = new ApprovalList();
				obj.AllApprovalLists = new List<ApprovalList>();
				GridEXRow[] checkedRows = grdfrm.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					ApprovalList item = new ApprovalList();
					item.OrganizationId = UserAccount.OrganizationId;
					item.CompanyId = UserAccount.CompanyId;
					item.EntryUser = UserAccount.ID;
					item.ReqType = "Approve";
					item.Id = Conversion.ToInt(r.Cells["Id"].Value);
					obj.AllApprovalLists.Add(item);
				}
				DepartmentRequest.DepartmentRequestApprovedForDepId(obj);
				MessageBox.Show("Record Approved Successfully");
				Reset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdfrm.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				int Id = Conversion.ToInt(item.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(item.Cells["DocumentTypeId"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdfrm_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Expected O, but got Unknown
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow item = grdfrm.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int Id = int.Parse(item.Cells["Id"].Value.ToString());
			if (grdfrm.CurrentColumn != null)
			{
				if (grdfrm.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, 450);
				}
				else if (grdfrm.CurrentColumn.Key == "Print")
				{
					CommonServices.DepartmentSlip451(Id);
				}
				else if (grdfrm.CurrentColumn.Key == "Approve" && MessageBox.Show("Are you sure to Approve?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.No)
				{
					ApprovalList val = new ApprovalList
					{
						AllApprovalLists = new List<ApprovalList>()
					};
					ApprovalList RecordTobeApprove = new ApprovalList();
					RecordTobeApprove.OrganizationId = UserAccount.OrganizationId;
					RecordTobeApprove.CompanyId = UserAccount.CompanyId;
					RecordTobeApprove.EntryUser = UserAccount.ID;
					RecordTobeApprove.ReqType = "Approve";
					RecordTobeApprove.Id = Id;
					val.AllApprovalLists.Add(RecordTobeApprove);
					DepartmentRequest.DepartmentRequestApprovedForDepId(val);
					MessageBox.Show("Record Approved Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnShortCutkeys_Click(object sender, EventArgs e)
	{
		try
		{
			MakeShortCutKeys();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void MakeShortCutKeys()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("KeyCombination");
			dt.Columns.Add("Description");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+A", "For Approve button click");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentRequestApproval_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				GridFill();
			}
			if (e.Control && e.KeyCode == Keys.A)
			{
				btnApprove_Click(null, null);
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdfrm).Focus();
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
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
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Expected O, but got Unknown
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.ApprovalDashboard.DepartmentRequestApproval));
		this.panel1 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnApprove = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.grdfrm = new GridEX();
		this.lblCaption = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel1.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(907, 30);
		this.panel1.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnRefresh, this.btnApprove, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(907, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnApprove.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnApprove.Name = "btnApprove";
		this.btnApprove.Size = new System.Drawing.Size(85, 24);
		this.btnApprove.Text = "Approve";
		this.btnApprove.Click += new System.EventHandler(btnApprove_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(106, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(865, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdfrm;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 21;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdfrm).BackColor = System.Drawing.Color.White;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdfrm.FilterMode = (FilterMode)2;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupTotals = (GroupTotals)2;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(907, 482);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 4;
		this.grdfrm.TotalRow = (InheritableBoolean)1;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.ColumnButtonClick += new ColumnActionEventHandler(grdfrm_ColumnButtonClick);
		this.grdfrm.LinkClicked += new ColumnActionEventHandler(grdfrm_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdfrm).KeyDown += new System.Windows.Forms.KeyEventHandler(grdfrm_KeyDown);
		this.lblCaption.AutoSize = true;
		this.lblCaption.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.lblCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.lblCaption.Location = new System.Drawing.Point(5, 4);
		this.lblCaption.Name = "lblCaption";
		this.lblCaption.Size = new System.Drawing.Size(179, 19);
		this.lblCaption.TabIndex = 0;
		this.lblCaption.Text = "Department Request";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ctrlGrdBar1);
		this.panel3.Controls.Add(this.lblCaption);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 30);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(907, 30);
		this.panel3.TabIndex = 3;
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 60);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(907, 482);
		this.panel2.TabIndex = 5;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(907, 542);
		base.Controls.Add(this.panel2);
		base.Controls.Add(this.panel3);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "DepartmentRequestApproval";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmGatePassReport_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(DepartmentRequestApproval_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
