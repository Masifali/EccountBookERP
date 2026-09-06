using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Properties;
using Janus.Data;
using Janus.Windows.GridEX;
using Janus.Windows.GridEX.Export;

namespace grid.User_Conrtols;

public class CtrlGrdBar : UserControl
{
	public enum EnmAccountType
	{
		Customers,
		Vendors,
		General
	}

	public bool _AutoAdjust;

	private int[] AryColWidth;

	private GridEX _MyGrid;

	private bool _ExpandCheckbox;

	public string str_Path = string.Empty;

	private Form _FormName;

	public static string strForm_Name = string.Empty;

	private IContainer components;

	internal GridEXPrintDocument GridEXPrintDocument1;

	internal GridEXExporter GridEXExporter1;

	internal PrintPreviewDialog PrintPreviewDialog1;

	internal PageSetupDialog PageSetupDialog1;

	internal ToolStrip ToolStrip1;

	internal ToolStripSplitButton btnGridControl;

	internal BackgroundWorker BackgroundWorker1;

	internal ToolStripTextBox txtGridTitle;

	internal ToolStripMenuItem mGridChooseFielder;

	internal ToolStripMenuItem mGridSaveLayouts;

	internal ToolStripMenuItem mGridPrint;

	internal ToolStripMenuItem mGridExport;

	internal ToolStripMenuItem GroupCollaps;

	internal ToolStripMenuItem GroupExpand;

	internal ToolStripMenuItem GridResizeColumnToolStripMenuItem;

	internal ToolStripMenuItem removeLayoutToolStripMenuItem;

	public GridEX MyGrid
	{
		get
		{
			return _MyGrid;
		}
		set
		{
			_MyGrid = value;
			GridEXPrintDocument1.GridEX = value;
		}
	}

	public Form FormName
	{
		get
		{
			return _FormName;
		}
		set
		{
			_FormName = value;
		}
	}

	public CtrlGrdBar()
	{
		InitializeComponent();
	}

	private void mGridChooseFielder_Click(object sender, EventArgs e)
	{
		try
		{
			MyGrid.AllowRemoveColumns = (InheritableBoolean)1;
			if (!MyGrid.IsFieldChooserVisible())
			{
				MyGrid.ShowFieldChooser(FormName);
			}
			else
			{
				MyGrid.HideFieldChooser();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void mGridSaveLayouts_Click(object sender, EventArgs e)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		try
		{
			if (!Directory.Exists("C:\\SCS\\Layout\\"))
			{
				Directory.CreateDirectory("C:\\SCS\\Layout\\");
			}
			using FileStream fs = new FileStream("C:\\SCS\\Layout\\" + FormName.Name + "_" + ((Control)(object)MyGrid).Name + "_" + clsGlobalVariables.UserAccount.UserName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
			MyGrid.SaveLayoutFile((Stream)fs);
			GridLayout obj = new GridLayout();
			obj.OrganizationId = clsGlobalVariables.UserAccount.OrganizationId;
			obj.CompanyId = clsGlobalVariables.UserAccount.CompanyId;
			obj.EntryUserId = clsGlobalVariables.UserAccount.ID;
			obj.ScreenName = FormName.Name;
			obj.Layout = FormName.Name + "_" + ((Control)(object)MyGrid).Name + "_" + clsGlobalVariables.UserAccount.UserName;
			GridLayout.Save(obj);
			MessageBox.Show("Layout Saved successfully");
			fs.Flush();
			fs.Close();
			fs.Dispose();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void printToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			PageSetupDialog1.PageSettings.Landscape = true;
			GridEXPrintDocument1.PageHeaderCenter = txtGridTitle.Text;
			PageSetupDialog1.ShowDialog();
			if (PrintPreviewDialog1.Document != null)
			{
				PrintPreviewDialog1.ShowDialog();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void exportToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			SaveFileDialog myDailog = new SaveFileDialog();
			myDailog.AddExtension = true;
			myDailog.DefaultExt = ".xls";
			myDailog.Filter = "Excel Files|*.xls";
			if (myDailog.ShowDialog() == DialogResult.OK)
			{
				string strFileName = string.Empty;
				strFileName = myDailog.FileName;
				if (strFileName.Length > 0)
				{
					FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
					GridEXExporter1.GridEX = MyGrid;
					GridEXExporter1.Export((Stream)fs);
					fs.Flush();
					fs.Close();
					fs.Dispose();
					MessageBox.Show("Exported successfully");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupCollaps_Click(object sender, EventArgs e)
	{
		try
		{
			_ExpandCheckbox = false;
			if (_MyGrid != null && _MyGrid.RootTable != null)
			{
				if (!_ExpandCheckbox)
				{
					_MyGrid.CollapseGroups();
					_MyGrid.CollapseRecords();
				}
				else
				{
					_MyGrid.ExpandGroups();
					_MyGrid.ExpandRecords();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GroupExpand_Click(object sender, EventArgs e)
	{
		try
		{
			_ExpandCheckbox = true;
			if (_MyGrid != null && _MyGrid.RootTable != null)
			{
				if (!_ExpandCheckbox)
				{
					_MyGrid.CollapseGroups();
					_MyGrid.CollapseRecords();
				}
				else
				{
					_MyGrid.ExpandGroups();
					_MyGrid.ExpandRecords();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridResizeColumnToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			if (_MyGrid.RootTable == null || ((JanusCollectionBase)_MyGrid.RootTable.Columns).Count == 0)
			{
				return;
			}
			if (_AutoAdjust)
			{
				for (int index = 0; index < ((JanusCollectionBase)_MyGrid.RootTable.Columns).Count - 1; index++)
				{
					_MyGrid.RootTable.Columns[index].Width = AryColWidth[index];
				}
				_AutoAdjust = false;
				return;
			}
			Array.Resize(ref AryColWidth, ((JanusCollectionBase)_MyGrid.RootTable.Columns).Count - 1);
			for (int index3 = 0; index3 < ((JanusCollectionBase)_MyGrid.RootTable.Columns).Count - 1; index3++)
			{
				AryColWidth[index3] = _MyGrid.RootTable.Columns[index3].Width;
			}
			for (int index2 = 0; index2 < ((JanusCollectionBase)_MyGrid.RootTable.Columns).Count - 1; index2++)
			{
				_MyGrid.RootTable.Columns[index2].AutoSize();
			}
			_AutoAdjust = true;
		}
		catch (Exception)
		{
			throw;
		}
	}

	private void removeLayoutToolStripMenuItem_Click(object sender, EventArgs e)
	{
		try
		{
			string LayoutPath = "";
			string FileName = FormName.Name + "_" + ((Control)(object)MyGrid).Name + "_" + clsGlobalVariables.UserAccount.UserName;
			LayoutPath = "C:\\SCS\\Layout\\";
			if (File.Exists(LayoutPath + FileName))
			{
				LayoutPath += FileName;
				using FileStream fs = new FileStream(LayoutPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
				fs.Flush();
				fs.Close();
				fs.Dispose();
				File.Delete(fs.Name);
				GridLayout.DeleteByLayout(clsGlobalVariables.UserAccount.OrganizationId, clsGlobalVariables.UserAccount.CompanyId, FileName);
				MessageBox.Show("Layout Removed successfully");
				return;
			}
			throw new Exception("File Not Exist");
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected O, but got Unknown
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(grid.User_Conrtols.CtrlGrdBar));
		this.GridEXPrintDocument1 = new GridEXPrintDocument();
		this.GridEXExporter1 = new GridEXExporter(this.components);
		this.PrintPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
		this.PageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
		this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnGridControl = new System.Windows.Forms.ToolStripSplitButton();
		this.txtGridTitle = new System.Windows.Forms.ToolStripTextBox();
		this.mGridChooseFielder = new System.Windows.Forms.ToolStripMenuItem();
		this.mGridSaveLayouts = new System.Windows.Forms.ToolStripMenuItem();
		this.mGridPrint = new System.Windows.Forms.ToolStripMenuItem();
		this.mGridExport = new System.Windows.Forms.ToolStripMenuItem();
		this.GroupCollaps = new System.Windows.Forms.ToolStripMenuItem();
		this.GroupExpand = new System.Windows.Forms.ToolStripMenuItem();
		this.GridResizeColumnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.removeLayoutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
		this.ToolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.GridEXPrintDocument1.FitColumns = (FitColumnsMode)1;
		this.PrintPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
		this.PrintPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
		this.PrintPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
		this.PrintPreviewDialog1.Document = (System.Drawing.Printing.PrintDocument)(object)this.GridEXPrintDocument1;
		this.PrintPreviewDialog1.Enabled = true;
		this.PrintPreviewDialog1.Icon = (System.Drawing.Icon)resources.GetObject("PrintPreviewDialog1.Icon");
		this.PrintPreviewDialog1.Name = "PrintPreviewDialog1";
		this.PrintPreviewDialog1.Visible = false;
		this.PageSetupDialog1.Document = (System.Drawing.Printing.PrintDocument)(object)this.GridEXPrintDocument1;
		this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.ToolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnGridControl });
		this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
		this.ToolStrip1.Name = "ToolStrip1";
		this.ToolStrip1.Size = new System.Drawing.Size(150, 27);
		this.ToolStrip1.TabIndex = 1;
		this.ToolStrip1.Text = "ToolStrip1";
		this.btnGridControl.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
		this.btnGridControl.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.txtGridTitle, this.mGridChooseFielder, this.mGridSaveLayouts, this.mGridPrint, this.mGridExport, this.GroupCollaps, this.GroupExpand, this.GridResizeColumnToolStripMenuItem, this.removeLayoutToolStripMenuItem });
		this.btnGridControl.Image = (System.Drawing.Image)resources.GetObject("btnGridControl.Image");
		this.btnGridControl.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnGridControl.Name = "btnGridControl";
		this.btnGridControl.Size = new System.Drawing.Size(36, 24);
		this.btnGridControl.Text = "Grid Control";
		this.txtGridTitle.BackColor = System.Drawing.SystemColors.Window;
		this.txtGridTitle.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.txtGridTitle.Name = "txtGridTitle";
		this.txtGridTitle.ReadOnly = true;
		this.txtGridTitle.Size = new System.Drawing.Size(100, 23);
		this.mGridChooseFielder.Image = (System.Drawing.Image)resources.GetObject("mGridChooseFielder.Image");
		this.mGridChooseFielder.Name = "mGridChooseFielder";
		this.mGridChooseFielder.Size = new System.Drawing.Size(187, 26);
		this.mGridChooseFielder.Text = "Field Chooser";
		this.mGridChooseFielder.Click += new System.EventHandler(mGridChooseFielder_Click);
		this.mGridSaveLayouts.Image = (System.Drawing.Image)resources.GetObject("mGridSaveLayouts.Image");
		this.mGridSaveLayouts.Name = "mGridSaveLayouts";
		this.mGridSaveLayouts.Size = new System.Drawing.Size(187, 26);
		this.mGridSaveLayouts.Text = "Save Layout";
		this.mGridSaveLayouts.Click += new System.EventHandler(mGridSaveLayouts_Click);
		this.mGridPrint.Image = (System.Drawing.Image)resources.GetObject("mGridPrint.Image");
		this.mGridPrint.Name = "mGridPrint";
		this.mGridPrint.Size = new System.Drawing.Size(187, 26);
		this.mGridPrint.Text = "Print";
		this.mGridPrint.Click += new System.EventHandler(printToolStripMenuItem_Click);
		this.mGridExport.Image = (System.Drawing.Image)resources.GetObject("mGridExport.Image");
		this.mGridExport.Name = "mGridExport";
		this.mGridExport.Size = new System.Drawing.Size(187, 26);
		this.mGridExport.Text = "Export";
		this.mGridExport.Click += new System.EventHandler(exportToolStripMenuItem_Click);
		this.GroupCollaps.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.GroupCollaps.Name = "GroupCollaps";
		this.GroupCollaps.Size = new System.Drawing.Size(187, 26);
		this.GroupCollaps.Text = "Group Collapse";
		this.GroupCollaps.Click += new System.EventHandler(GroupCollaps_Click);
		this.GroupExpand.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.GroupExpand.Name = "GroupExpand";
		this.GroupExpand.Size = new System.Drawing.Size(187, 26);
		this.GroupExpand.Text = "Group Expand";
		this.GroupExpand.Click += new System.EventHandler(GroupExpand_Click);
		this.GridResizeColumnToolStripMenuItem.Image = Architecture.WinApp.Properties.Resources.icons8_help_16;
		this.GridResizeColumnToolStripMenuItem.Name = "GridResizeColumnToolStripMenuItem";
		this.GridResizeColumnToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
		this.GridResizeColumnToolStripMenuItem.Text = "Auto Adjust Column";
		this.GridResizeColumnToolStripMenuItem.Click += new System.EventHandler(GridResizeColumnToolStripMenuItem_Click);
		this.removeLayoutToolStripMenuItem.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.removeLayoutToolStripMenuItem.Name = "removeLayoutToolStripMenuItem";
		this.removeLayoutToolStripMenuItem.Size = new System.Drawing.Size(187, 26);
		this.removeLayoutToolStripMenuItem.Text = "Remove Layout";
		this.removeLayoutToolStripMenuItem.Click += new System.EventHandler(removeLayoutToolStripMenuItem_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.ToolStrip1);
		base.Name = "CtrlGrdBar";
		this.ToolStrip1.ResumeLayout(false);
		this.ToolStrip1.PerformLayout();
		base.ResumeLayout(false);
		base.PerformLayout();
	}
}
