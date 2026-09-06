// using Architecture.Reports;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.Templates;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Authentication;
using Architecture.Model.Templates;
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
namespace Architecture.WinApp.Templates
{

public class ChartOfAccountTemplate : Form
{
	private int ACCOUNTID;

	private Rightsobjects formRights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private DataTable table = new DataTable();

	private DataTable secondlevelac = new DataTable();

	private DataTable thirdlevelac = new DataTable();

	private DataTable fourthlevelac = new DataTable();

	private DataTable fivelevelac = new DataTable();

	private DataTable dtHistory = new DataTable();

	public UserAccount UserAccount = new UserAccount();

	private ChartofAccount coa = new ChartofAccount();

	private IContainer components;

	private PrintDialog printDialog1;

	private ContextMenuStrip contextMenuStrip1;

	private Panel panel5;

	private Label label1;

	private TextBox txtacclass;

	private Label label8;

	private Label label9;

	private Label label6;

	private Label label2;

	private TextBox txtactitle;

	private Label label4;

	private Label label11;

	private TextBox txtaccode;

	private TextBox txtaclevel;

	private Label label5;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnUpdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnPrintCOAform;

	private Panel panel1;

	private Panel panel2;

	private Label label12;

	private Label label10;

	internal UltraCombo CmbParentAccount;

	internal UltraCombo CmbAccountTypes;

	internal UltraCombo CmbBSNote;

	internal UltraCombo CmbPLNote;

	private CtrlGrdBar ctrlGrdBar1;

	private Button button1;

	private Panel panel6;

	internal GridEX grdlevel4;

	internal GridEX grdlevel3;

	internal UltraCombo CmbGroupDetail;

	internal GridEX grdParentCode;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbAccountTitlefilter;

	private Label label15;

	private Label label16;

	internal UltraCombo CmbAccountTypeFilter;

	private Label label17;

	private Label label21;

	private Label label20;

	private Label label19;

	private Label label18;

	private TextBox txtOtherCode;

	private Label label22;

	private TabControl TabCOA;

	private TabPage TabForm;

	private Panel ParentPanel;

	private TabPage TabHistory;

	private GroupBox DetailFilterGroup;

	private Panel panel15;

	internal GridEX grdFormHistory;

	private Panel panel17;

	private Panel panel16;

	private Label label24;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton btnUploadExcelSheet;

	private Label label7;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator3;

	private GroupBox groupBox5;

	private Button BtnFilterInChildGrid;

	private Panel panel7;

	private Label label26;

	private Panel panel3;

	private Label label29;

	private Panel panel12;

	private Label label32;

	private Panel panel11;

	private Label label31;

	internal GridEX grdlevel2;

	private Panel panel4;

	private Label label30;

	internal GridEX grdfrm;

	private Panel panel10;

	private Label label28;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnPrintCOAHistory;

	private ToolStripSeparator toolStripSeparator7;

	private ToolStripButton btnShortcutkeys;

	internal UltraCombo CmbTemplate;

	private Label label3;

	private Label label13;

	private GroupBox groupBox1;

	private Label label14;

	internal UltraCombo CmbParentAccountHistory;

	internal UltraCombo CmbAccountTitleHistory;

	private Label label25;

	private Button BtnShowHistory;

	private Label label23;

	internal UltraCombo CmbAccountTypHistory;

	internal UltraCombo CmbTemplateHistory;

	private Label label27;

	private ToolStripButton BtnNewHistory;

	private ToolStripButton BtnRefreshHistory;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator1;

	public ChartOfAccountTemplate(UserAccount puserAccount)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = puserAccount;
	}

	private void ChartOfAccountTemplate_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1120, 800);
			formRights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formRights.DoHaveSaveRight;
			btnUpdate.Enabled = formRights.DoHaveUpdateRights;
			btnPrintCOAform.Enabled = formRights.DoHavePrintRights;
			ConfigurationDefault();
			table.Columns.Add("Id", typeof(int));
			table.Columns.Add("AccountCode");
			table.Columns.Add("ParentAccountCode");
			table.Columns.Add("AccountTitle");
			secondlevelac.Columns.Add("Id", typeof(int));
			secondlevelac.Columns.Add("AccountCode");
			secondlevelac.Columns.Add("ParentAccountCode");
			secondlevelac.Columns.Add("AccountTitle");
			thirdlevelac.Columns.Add("Id", typeof(int));
			thirdlevelac.Columns.Add("AccountCode");
			thirdlevelac.Columns.Add("ParentAccountCode");
			thirdlevelac.Columns.Add("AccountTitle");
			fourthlevelac.Columns.Add("Id", typeof(int));
			fourthlevelac.Columns.Add("AccountCode");
			fourthlevelac.Columns.Add("ParentAccountCode");
			fourthlevelac.Columns.Add("AccountTitle");
			fivelevelac.Columns.Add("Id", typeof(int));
			fivelevelac.Columns.Add("AccountCode");
			fivelevelac.Columns.Add("ParentAccountCode");
			fivelevelac.Columns.Add("AccountTitle");
			btnsave.Visible = true;
			cmbbsnotefill();
			cmbplnotefill();
			CmbAccountType();
			AccountGroupFill();
			TemplateNameFill();
			((Control)(object)CmbGroupDetail).Enabled = false;
			((Control)(object)CmbPLNote).Enabled = false;
			((Control)(object)CmbBSNote).Enabled = false;
			((Control)(object)CmbTemplate).Enabled = true;
			CmbParentAccount.Focus();
			grdfrmfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdfrmfill()
	{
		try
		{
			if (dtHistory.Rows.Count > 0)
			{
				table.Rows.Clear();
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 1)
					{
						table.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["AccountTitle"]);
					}
				}
			}
			else
			{
				table.Clear();
			}
			grdfrm.DataSource = table;
			grdfrm.RetrieveStructure();
			GridSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings()
	{
		try
		{
			grdfrm.RootTable.Columns["Id"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdfrm.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdfrm.RootTable.Columns["AccountCode"].Width = 100;
			grdfrm.RootTable.Columns["AccountTitle"].Width = 250;
			grdfrm.RootTable.Columns["Id"].Visible = false;
			grdfrm.RootTable.Columns["ParentAccountCode"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdfrm);
			ctrlGrdBar1_Load_1(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdfrm_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			grdlevel2Fill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel2Fill()
	{
		try
		{
			if (grdfrm.SelectedItems.Count > 0)
			{
				if (dtHistory.Rows.Count > 0)
				{
					secondlevelac.Rows.Clear();
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 2 && Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == Conversion.ToString(grdfrm.CurrentRow.Cells[2].Value))
						{
							secondlevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
				}
				else
				{
					dtHistory.Rows.Clear();
				}
			}
			else
			{
				secondlevelac.Clear();
			}
			grdlevel2.DataSource = secondlevelac;
			grdlevel2.RetrieveStructure();
			GridSettings2ndlevel();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings2ndlevel()
	{
		try
		{
			grdlevel2.RootTable.Columns["Id"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdlevel2.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdlevel2.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdlevel2.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdlevel2.RootTable.Columns["AccountTitle"].Width = 250;
			grdlevel2.RootTable.Columns["Id"].Visible = false;
			grdlevel2.RootTable.Columns["ParentAccountCode"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdlevel2);
			grdlevel2.RootTable.Columns["AccountCode"].Width = 100;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdlevel2_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			grdlevel3Fill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel2_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				CmbParentAccount.Value = Conversion.ToString(grdlevel2.CurrentRow.Cells["ParentAccountCode"].Value);
				cmbparentac_Leave(null, null);
				return;
			}
			CmbTemplate.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel3Fill()
	{
		try
		{
			if (grdlevel2.SelectedItems.Count > 0)
			{
				if (dtHistory.Rows.Count > 0)
				{
					thirdlevelac.Rows.Clear();
					string ParentAccountCode = "";
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						ParentAccountCode = Conversion.ToString(grdlevel2.CurrentRow.Cells[1].Value);
						if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 3 && Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == ParentAccountCode)
						{
							thirdlevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
				}
				else
				{
					dtHistory.Rows.Clear();
				}
				grdlevel3.DataSource = thirdlevelac;
				grdlevel3.RetrieveStructure();
				GridSettings3rdlevel();
			}
			else
			{
				thirdlevelac.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings3rdlevel()
	{
		try
		{
			grdlevel3.RootTable.Columns["Id"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdlevel3.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdlevel3.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdlevel3.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdlevel3.RootTable.Columns["AccountTitle"].Width = 250;
			grdlevel3.RootTable.Columns["Id"].Visible = false;
			grdlevel3.RootTable.Columns["ParentAccountCode"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdlevel3);
			grdlevel3.RootTable.Columns["AccountCode"].Width = 100;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdlevel3_SelectionChanged(object sender, EventArgs e)
	{
		try
		{
			grdlevel4Fill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel3_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				CmbParentAccount.Value = Conversion.ToString(grdlevel3.CurrentRow.Cells["ParentAccountCode"].Value);
				cmbparentac_Leave(null, null);
				return;
			}
			CmbTemplate.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel4Fill()
	{
		try
		{
			if (grdlevel3.SelectedItems.Count > 0)
			{
				if (dtHistory.Rows.Count > 0)
				{
					fourthlevelac.Rows.Clear();
					string ParentAccountCode = "";
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						ParentAccountCode = Conversion.ToString(grdlevel3.CurrentRow.Cells[1].Value);
						if (Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 4 && Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == ParentAccountCode)
						{
							fourthlevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
					grdParentCode.ClearStructure();
				}
				else
				{
					dtHistory.Rows.Clear();
				}
				grdlevel4.DataSource = fourthlevelac;
				grdlevel4.RetrieveStructure();
				GridSettings4thlevel();
			}
			else
			{
				fourthlevelac.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridSettings4thlevel()
	{
		try
		{
			grdlevel4.RootTable.Columns["Id"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["AccountCode"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["ParentAccountCode"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["AccountTitle"].EditType = (EditType)0;
			grdlevel4.RootTable.Columns["Id"].FilterEditType = (FilterEditType)1;
			grdlevel4.RootTable.Columns["AccountCode"].FilterEditType = (FilterEditType)1;
			grdlevel4.RootTable.Columns["AccountTitle"].FilterEditType = (FilterEditType)1;
			grdlevel4.RootTable.Columns["AccountTitle"].Width = 300;
			grdlevel4.RootTable.Columns["Id"].Visible = false;
			grdlevel4.RootTable.Columns["ParentAccountCode"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdlevel4);
			grdlevel4.RootTable.Columns["AccountCode"].Width = 100;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel4_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				CmbParentAccount.Value = Conversion.ToString(grdlevel4.CurrentRow.Cells["ParentAccountCode"].Value);
				cmbparentac_Leave(null, null);
				return;
			}
			CmbTemplate.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdlevel4_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void FillChildOfSelectedParentGridByParentAccountCode(string ParentAccountCode)
	{
		try
		{
			fivelevelac.Rows.Clear();
			if (dtHistory.Rows.Count > 0)
			{
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					if (Conversion.ToString(dtHistory.Rows[i]["ParentAccountCode"]) == ParentAccountCode)
					{
						fivelevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
					}
				}
			}
			grdParentCode.DataSource = fivelevelac;
			grdParentCode.RetrieveStructure();
			GridParentcodeSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void GridParentcodeSetting()
	{
		try
		{
			grdParentCode.RootTable.Columns["Id"].Visible = false;
			grdParentCode.RootTable.Columns["AccountCode"].Visible = false;
			grdParentCode.RootTable.Columns["ParentAccountCode"].Visible = false;
			grdParentCode.RootTable.Columns["AccountTitle"].Width = 450;
			CommonServices.GridAutoAdjustmentNew(grdParentCode);
			grdParentCode.RootTable.Columns["AccountCode"].Width = 100;
			grdParentCode.RootTable.Columns.Add("Edit");
			grdParentCode.RootTable.Columns["Edit"].Caption = "Edit";
			grdParentCode.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdParentCode.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdParentCode.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdParentCode.RootTable.Columns["Edit"].Width = 40;
			grdParentCode.RootTable.Columns["Edit"].Position = 0;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdParentCode_SelectionChanged(object sender, EventArgs e)
	{
	}

	private void grdParentCode_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (((UltraGridBase)CmbTemplate).ActiveRow == null || Conversion.ToInt(CmbTemplate.Value) <= 0)
				{
					CmbTemplate.Focus();
					throw new Exception("Please Select Template first...");
				}
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				ACCOUNTID = Conversion.ToInt((object)grdParentCode.CurrentRow.Cells["Id"].Value.ToString());
				Organization_ChartOfAccountTemplate chart = Organization_ChartOfAccountTemplate.ReadById(ACCOUNTID, Conversion.ToInt(CmbTemplate.Value));
				if (chart != null)
				{
					txtactitle.Text = Conversion.ToString((object)chart.AccountTitle);
					txtOtherCode.Text = Conversion.ToString((object)chart.OtherErpCode);
				}
				if (btnUpdate.Visible)
				{
					button1.Text = "Update";
				}
				else
				{
					button1.Text = "Save";
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void TemplateNameFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbTemplate.Value);
			int Id2 = Conversion.ToInt(CmbTemplateHistory.Value);
			DataTable dt = OrganizationTemplates.FormHistory();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			DDL.BindDDLNew(dt, CmbTemplate, "Id", "TemplateName", "Template Name", true);
			DDL.BindDDLNew(dt, CmbTemplateHistory, "Id", "TemplateName", "Template Name", true);
			((UltraGridBase)CmbTemplate).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			((UltraGridBase)CmbTemplateHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbTemplate.Value = Id;
				}
				else
				{
					((Control)(object)CmbTemplate).Text = string.Empty;
				}
			}
			if (Id2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
				{
					CmbTemplateHistory.Value = Id2;
				}
				else
				{
					((Control)(object)CmbTemplateHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbTemplate_Leave(object sender, EventArgs e)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				AccountTitleFilterFill(Conversion.ToInt(CmbTemplate.Value));
				ParentAccountFill(Conversion.ToInt(CmbTemplate.Value));
				CmbAccountTypeForFilter(Conversion.ToInt(CmbTemplate.Value));
				Organization_ChartOfAccountTemplate obj = new Organization_ChartOfAccountTemplate();
				obj.OrganizationTemplateId = Conversion.ToInt(CmbTemplate.Value);
				dtHistory = Organization_ChartOfAccountTemplate.FormHistory(obj);
				grdfrmfill();
			}
			else
			{
				((Control)(object)CmbAccountTitlefilter).Text = string.Empty;
				((UltraGridBase)CmbAccountTitlefilter).DataSource = null;
				((Control)(object)CmbParentAccount).Text = string.Empty;
				((UltraGridBase)CmbParentAccount).DataSource = null;
				((Control)(object)CmbAccountTypeFilter).Text = string.Empty;
				((UltraGridBase)CmbAccountTypeFilter).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AccountTitleFilterFill(int TemplateId)
	{
		try
		{
			int Id = Conversion.ToInt(CmbAccountTitlefilter.Value);
			DataTable dt = Organization_ChartOfAccountTemplate.DataForDropDownFromOrganization_ChartOfAccountTemplate(TemplateId, "AccountTitle");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAccountTitlefilter, "Id", "ReferenceName", "Account Title", ZeroIndex: true);
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbAccountTitlefilter.Value = Id;
					}
					else
					{
						((Control)(object)CmbAccountTitlefilter).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAccountTitlefilter).Text = string.Empty;
				((UltraGridBase)CmbAccountTitlefilter).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbAccountTypeForFilter(int TemplateId)
	{
		try
		{
			int Idh = Conversion.ToInt(CmbAccountTypeFilter.Value);
			DataTable dt = Organization_ChartOfAccountTemplate.DataForDropDownFromOrganization_ChartOfAccountTemplate(TemplateId, "AccountType");
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbAccountTypeFilter, "Id", "ReferenceName", "Account Types", ZeroIndex: true);
				if (Idh > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Idh))
					{
						CmbAccountTypeFilter.Value = Idh;
					}
					else
					{
						((Control)(object)CmbAccountTypeFilter).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbAccountTypeFilter).Text = string.Empty;
				((UltraGridBase)CmbAccountTypeFilter).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ParentAccountFill(int TemplateId)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			string Id = Conversion.ToString(CmbParentAccount.Value);
			DataTable dtcoa = Organization_ChartOfAccountTemplate.ReadAllGroupAccounts(new ReportsParameters
			{
				OrganizationTemplateId = TemplateId
			});
			if (dtcoa.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("AccountCode");
				dt.Columns.Add("AccountTitle");
				dt.Columns.Add("ParentCodeId");
				for (int i = 0; i < dtcoa.Rows.Count; i++)
				{
					dt.Rows.Add(dtcoa.Rows[i]["AccountCode"], dtcoa.Rows[i]["AccountTitle"], dtcoa.Rows[i]["Id"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbParentAccount, "AccountCode", "AccountTitle", "Account Title", ZeroIndex: true);
					((UltraGridBase)CmbParentAccount).DisplayLayout.Bands[0].Columns[2].Hidden = true;
					if (Id != "" && Id != null)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<string>("AccountCode") == Id))
						{
							CmbParentAccount.Value = Id;
						}
						else
						{
							((Control)(object)CmbParentAccount).Text = string.Empty;
						}
					}
				}
				else
				{
					((Control)(object)CmbParentAccount).Text = string.Empty;
					((UltraGridBase)CmbParentAccount).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbParentAccount).Text = string.Empty;
				((UltraGridBase)CmbParentAccount).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbbsnotefill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbBSNote.Value);
			DataTable dtaccnote = new DataTable();
			dtaccnote = AccountNotes.ReadByBSNote(0);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("NoteTitle");
			for (int i = 0; i < dtaccnote.Rows.Count; i++)
			{
				dt.Rows.Add(dtaccnote.Rows[i]["Id"], dtaccnote.Rows[i]["NoteTitle"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbBSNote, "Id", "NoteTitle", "Note Title", ZeroIndex: true);
			}
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbBSNote.Value = Id;
				}
				else
				{
					((Control)(object)CmbBSNote).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbplnotefill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbPLNote.Value);
			DataTable dtplcnote = new DataTable();
			dtplcnote = AccountNotes.ReadByPLNote();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("NoteTitle");
			for (int i = 0; i < dtplcnote.Rows.Count; i++)
			{
				dt.Rows.Add(dtplcnote.Rows[i]["Id"], dtplcnote.Rows[i]["NoteTitle"]);
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbPLNote, "Id", "NoteTitle", "Note Title", ZeroIndex: false);
			}
			if (Id > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
				{
					CmbPLNote.Value = Id;
				}
				else
				{
					((Control)(object)CmbPLNote).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AccountGroupFill()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id");
			dt.Columns.Add("Name");
			dt.Rows.Add(1, "Detail");
			dt.Rows.Add(1, "Group");
			DDL.BindDDL(dt, CmbGroupDetail, "Id", "Name", "Account Type", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbAccountType()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbAccountTypes.Value);
			DataTable dttype = AccountTypes.GetAll(new AccountTypes
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dttype.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("AccountType");
				for (int i = 0; i < dttype.Rows.Count; i++)
				{
					dt.Rows.Add(dttype.Rows[i]["Id"], dttype.Rows[i]["AccountType"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, CmbAccountTypes, "Id", "AccountType", "Account Types", ZeroIndex: true);
					((Control)(object)CmbAccountTypes).Enabled = true;
					if (Id > 0)
					{
						if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
						{
							CmbAccountTypes.Value = Id;
						}
						else
						{
							((Control)(object)CmbAccountTypes).Text = string.Empty;
						}
					}
				}
				else
				{
					((UltraGridBase)CmbAccountTypes).DataSource = null;
					((Control)(object)CmbAccountTypes).Text = string.Empty;
				}
			}
			else
			{
				((UltraGridBase)CmbAccountTypes).DataSource = null;
				((Control)(object)CmbAccountTypes).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ConfigurationDefault()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "City Area").ToList();
			if (list.Count > 0)
			{
				Conversion.ToInt((object)list[0].ConfigKey.ToString());
			}
		}
		catch (Exception)
		{
			MessageBox.Show("Confiquration list Error");
		}
	}

	private void cmbgroupdetail_Leave(object sender, EventArgs e)
	{
	}

	private void cmbparentac_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow == null || Conversion.ToInt(CmbTemplate.Value) <= 0)
			{
				return;
			}
			if (btnUpdate.Visible)
			{
				button1.Text = "Update";
			}
			if (btnsave.Visible)
			{
				button1.Text = "Save";
			}
			DataTable dtparentLevelAcc = Organization_ChartOfAccountTemplate.ReadNewCodebyParentCode(Convert.ToString(CmbParentAccount.Value), Conversion.ToInt(CmbTemplate.Value));
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "1")
			{
				txtacclass.Text = "Capital";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "2")
			{
				txtacclass.Text = "Assets";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "3")
			{
				txtacclass.Text = "Liabilities";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "4")
			{
				txtacclass.Text = "Expenses";
			}
			if (dtparentLevelAcc.Rows[0]["AccountClass"].ToString() == "5")
			{
				txtacclass.Text = "Revenue";
			}
			txtaclevel.Text = dtparentLevelAcc.Rows[0]["Account_Level"].ToString();
			txtaccode.Text = dtparentLevelAcc.Rows[0]["AccountCode"].ToString();
			CmbAccountType();
			if (Conversion.ToDouble((object)txtaclevel.Text.Trim()) >= 4.0)
			{
				((Control)(object)CmbAccountTypes).Text = dtparentLevelAcc.Rows[0]["AccountType"].ToString();
				((Control)(object)CmbAccountTypes).Enabled = false;
			}
			if (Conversion.ToInt((object)txtaclevel.Text) < 4)
			{
				((Control)(object)CmbGroupDetail).Text = "Group";
				((Control)(object)CmbGroupDetail).Enabled = false;
			}
			else
			{
				((Control)(object)CmbGroupDetail).Enabled = true;
				((UltraGridBase)CmbGroupDetail).Rows[0].Activate();
			}
			if (Conversion.ToInt((object)txtaclevel.Text) == 3)
			{
				((Control)(object)CmbAccountTypes).Enabled = true;
			}
			int classcode = Conversion.ToInt(dtparentLevelAcc.Rows[0]["AccountClass"]);
			if (Conversion.ToInt((object)txtaclevel.Text) == 3)
			{
				if (classcode == 1 || classcode == 2 || classcode == 3)
				{
					((Control)(object)CmbPLNote).Enabled = false;
					((Control)(object)CmbBSNote).Enabled = true;
				}
				if (classcode == 4 || classcode == 5)
				{
					((Control)(object)CmbPLNote).Enabled = true;
					((Control)(object)CmbBSNote).Enabled = false;
				}
			}
			if (Conversion.ToInt((object)txtaclevel.Text) != 3)
			{
				((Control)(object)CmbPLNote).Enabled = false;
				((Control)(object)CmbBSNote).Enabled = false;
			}
			if (Conversion.ToInt((object)txtaclevel.Text) != 3)
			{
				((Control)(object)CmbAccountTypes).Enabled = false;
			}
			if (Conversion.ToInt((object)txtaclevel.Text) < 3)
			{
				((Control)(object)CmbAccountTypes).Enabled = false;
				((Control)(object)CmbAccountTypes).Text = string.Empty;
				((Control)(object)CmbBSNote).Text = string.Empty;
				((Control)(object)CmbPLNote).Text = string.Empty;
			}
			if (!string.IsNullOrEmpty(Convert.ToString(CmbParentAccount.Value)))
			{
				FillChildOfSelectedParentGridByParentAccountCode(Convert.ToString(CmbParentAccount.Value));
			}
			else
			{
				grdParentCode.ClearStructure();
			}
			cmbgroupdetail_Leave(null, null);
			cmbactype_Leave(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void cmbactype_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbAccountTypes.Value) != 3)
			{
				Conversion.ToInt(CmbAccountTypes.Value);
				_ = 22;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnFilterInChildGrid_Click(object sender, EventArgs e)
	{
	}

	private void CmbAccountTypeFilter_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				fivelevelac.Rows.Clear();
				if (dtHistory.Rows.Count > 0)
				{
					for (int i = 0; i < dtHistory.Rows.Count; i++)
					{
						if (Conversion.ToInt(dtHistory.Rows[i]["AccountTypeId"]) == Conversion.ToInt(CmbAccountTypeFilter.Value) && Conversion.ToInt(dtHistory.Rows[i]["Account_Level"]) == 3)
						{
							fivelevelac.Rows.Add(dtHistory.Rows[i]["Id"], dtHistory.Rows[i]["AccountCode"], dtHistory.Rows[i]["ParentAccountCode"], dtHistory.Rows[i]["AccountTitle"]);
						}
					}
				}
				grdParentCode.DataSource = fivelevelac;
				grdParentCode.RetrieveStructure();
				GridParentcodeSetting();
				return;
			}
			CmbTemplate.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbAccountTitle_Leave(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				int code = Organization_ChartOfAccountTemplate.ParentCodeByAccountId(Conversion.ToInt(CmbTemplate.Value), Conversion.ToInt(CmbAccountTitlefilter.Value));
				if (code > 0)
				{
					CmbParentAccount.Value = Conversion.ToString((object)code);
					cmbparentac_Leave(null, null);
					txtactitle.Focus();
				}
				return;
			}
			CmbTemplate.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)CmbTemplate).Text.Trim() == "" || Conversion.ToInt(CmbTemplate.Value) == 0)
		{
			MessageBox.Show("Please Select Template", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			CmbTemplate.Focus();
			return false;
		}
		if (txtactitle.Text.Trim() == "")
		{
			MessageBox.Show("Please Enter Account Title", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			txtactitle.Focus();
			return false;
		}
		if (((Control)(object)CmbParentAccount).Text.Trim() == "" || Conversion.ToInt(CmbParentAccount.Value) == 0)
		{
			MessageBox.Show("Please Enter Parent Account", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			CmbParentAccount.Focus();
			return false;
		}
		if (txtaclevel.Text == "3" && (((Control)(object)CmbAccountTypes).Text.Trim() == "" || Conversion.ToInt(CmbAccountTypes.Value) == 0))
		{
			MessageBox.Show("Please Enter Account Type", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			CmbAccountTypes.Focus();
			return false;
		}
		if (((Control)(object)CmbPLNote).Enabled && txtaclevel.Text == "3" && (((Control)(object)CmbPLNote).Text.Trim() == "" || Conversion.ToInt(CmbPLNote.Value) == 0))
		{
			MessageBox.Show("Please Enter PL Notes", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			CmbPLNote.Focus();
			return false;
		}
		if (((Control)(object)CmbBSNote).Enabled && txtaclevel.Text == "3" && (((Control)(object)CmbBSNote).Text.Trim() == "" || Conversion.ToInt(CmbBSNote.Value) == 0))
		{
			MessageBox.Show("Please Enter BS Notes", "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Question);
			CmbBSNote.Focus();
			return false;
		}
		return true;
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
		btnsave.Visible = true;
		btnUpdate.Visible = false;
		((Control)(object)CmbParentAccount).Text = string.Empty;
		grdParentCode.DataSource = null;
	}

	private void Reset()
	{
		try
		{
			button1.Text = "Save";
			ACCOUNTID = 0;
			((Control)(object)CmbBSNote).Enabled = false;
			((Control)(object)CmbPLNote).Enabled = false;
			((Control)(object)CmbAccountTypes).Enabled = false;
			txtactitle.Text = string.Empty;
			((Control)(object)CmbPLNote).Text = string.Empty;
			((Control)(object)CmbBSNote).Text = string.Empty;
			txtacclass.Text = string.Empty;
			txtaclevel.Text = string.Empty;
			txtaccode.Text = string.Empty;
			((Control)(object)CmbGroupDetail).Text = string.Empty;
			((Control)(object)CmbAccountTypes).Text = string.Empty;
			((Control)(object)CmbAccountTypeFilter).Text = string.Empty;
			((Control)(object)CmbAccountTitlefilter).Text = string.Empty;
			txtOtherCode.Text = string.Empty;
			ConfigurationDefault();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			AccountTitleFilterFill(Conversion.ToInt(CmbTemplate.Value));
			ParentAccountFill(Conversion.ToInt(CmbTemplate.Value));
			CmbAccountTypeForFilter(Conversion.ToInt(CmbTemplate.Value));
			ConfigurationDefault();
			TemplateNameFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ChartOfAccountTemplate_FormClosed(object sender, FormClosedEventArgs e)
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

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (formvalidation())
			{
				Organization_ChartOfAccountTemplate coa = new Organization_ChartOfAccountTemplate();
				coa.ParentAccountCode = Conversion.ToString(CmbParentAccount.Value);
				EnumAccountClass accountClassEnum;
				if (txtacclass.Text == "Capital")
				{
					coa.AccountClassEnum = (EnumAccountClass)1;
					coa.AccountClass = 1;
					TextBox textBox = txtacclass;
					accountClassEnum = coa.AccountClassEnum;
					textBox.Text = accountClassEnum.ToString();
				}
				if (txtacclass.Text == "Assets")
				{
					coa.AccountClassEnum = (EnumAccountClass)2;
					coa.AccountClass = 2;
					TextBox textBox2 = txtacclass;
					accountClassEnum = coa.AccountClassEnum;
					textBox2.Text = accountClassEnum.ToString();
				}
				if (txtacclass.Text == "Liabilities")
				{
					coa.AccountClassEnum = (EnumAccountClass)3;
					coa.AccountClass = 3;
					TextBox textBox3 = txtacclass;
					accountClassEnum = coa.AccountClassEnum;
					textBox3.Text = accountClassEnum.ToString();
				}
				if (txtacclass.Text == "Expenses")
				{
					coa.AccountClassEnum = (EnumAccountClass)4;
					coa.AccountClass = 4;
					TextBox textBox4 = txtacclass;
					accountClassEnum = coa.AccountClassEnum;
					textBox4.Text = accountClassEnum.ToString();
				}
				if (txtacclass.Text == "Revenue")
				{
					coa.AccountClassEnum = (EnumAccountClass)5;
					coa.AccountClass = 5;
					TextBox textBox5 = txtacclass;
					accountClassEnum = coa.AccountClassEnum;
					textBox5.Text = accountClassEnum.ToString();
				}
				coa.ParentCodeId = Conversion.ToInt(((UltraDropDownBase)CmbParentAccount).SelectedRow.Cells[2].Value);
				coa.Account_Level = Conversion.ToInt((object)txtaclevel.Text.Trim());
				coa.AccountCode = Conversion.ToString((object)txtaccode.Text.Trim());
				coa.AccountTitle = Conversion.ToString((object)txtactitle.Text.Trim());
				coa.AccountGroup = Conversion.ToString((object)((Control)(object)CmbGroupDetail).Text);
				coa.AccountTypeId = Conversion.ToInt(CmbAccountTypes.Value);
				coa.PLNoteId = Conversion.ToInt(CmbPLNote.Value);
				coa.BSNoteId = Conversion.ToInt(CmbBSNote.Value);
				coa.OtherErpCode = Conversion.ToString((object)txtOtherCode.Text.Trim());
				coa.OrganizationTemplateId = Conversion.ToInt(CmbTemplate.Value);
				coa.EntryUser = UserAccount.ID;
				coa.IsActive = true;
				coa.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
				Organization_ChartOfAccountTemplate.Save(coa);
				MessageBox.Show(txtactitle.Text + " Save Successfully");
				Reset();
				CmbTemplate_Leave(null, null);
				grdfrmfill();
				txtactitle.Focus();
				cmbparentac_Leave(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				if (txtactitle.Text == string.Empty || txtactitle.Text == "0")
				{
					throw new Exception("AccountTitle Field Required");
				}
				Organization_ChartOfAccountTemplate.UpdateAccountTitleById(ACCOUNTID, Conversion.ToInt(CmbTemplate.Value), Conversion.ToString((object)txtactitle.Text), Conversion.ToString((object)txtOtherCode.Text.Trim()), true);
				MessageBox.Show("Recored Update Successfully...;");
				Reset();
				btnsave.Visible = true;
				btnUpdate.Visible = false;
				CmbTemplate_Leave(null, null);
				grdfrmfill();
				return;
			}
			CmbTemplate.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button1_Click(object sender, EventArgs e)
	{
		try
		{
			if (ACCOUNTID > 0)
			{
				btnUpdate_Click(null, null);
			}
			else
			{
				btnsave_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar1_Load_1(object sender, EventArgs e)
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
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdFormHistory).Name);
			if (fs != null)
			{
				grdFormHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnPrintCOAform_Click(object sender, EventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((UltraGridBase)CmbTemplate).ActiveRow != null && Conversion.ToInt(CmbTemplate.Value) > 0)
			{
				DataTable dt = Organization_ChartOfAccountTemplate.FormHistory(new Organization_ChartOfAccountTemplate
				{
					OrganizationTemplateId = Conversion.ToInt(CmbTemplate.Value)
				});
				if (dt.Rows.Count > 0 && dt != null)
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "114_01-AcRptChartOfAccounts.rpt");
					return;
				}
				throw new Exception("Record Not Found For Display");
			}
			CmbTemplate.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnPrintCOAHistory_Click(object sender, EventArgs e)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((UltraGridBase)CmbTemplateHistory).ActiveRow != null && Conversion.ToInt(CmbTemplateHistory.Value) > 0)
			{
				DataTable dt = Organization_ChartOfAccountTemplate.FormHistory(new Organization_ChartOfAccountTemplate
				{
					OrganizationTemplateId = Conversion.ToInt(CmbTemplateHistory.Value),
					AccountTypeId = Conversion.ToInt(CmbAccountTypHistory.Value),
					ParentCodeId = Conversion.ToInt(CmbParentAccountHistory.Value),
					Id = Conversion.ToInt(CmbAccountTitleHistory.Value)
				});
				if (dt.Rows.Count > 0 && dt != null)
				{
					Reporting val = new Reporting();
					val.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
					val.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
					val.ShowReportWithDataTable(dt, "114_01-AcRptChartOfAccounts.rpt");
					return;
				}
				throw new Exception("Record Not Found For Display");
			}
			CmbTemplateHistory.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void AllHistoryCombobind(int TemplateId)
	{
		try
		{
			DataTable dtcombo = Organization_ChartOfAccountTemplate.DataForDropDownFromOrganization_ChartOfAccountTemplate(TemplateId, "");
			if (dtcombo.Rows.Count > 0)
			{
				DataTable dtAcountType = new DataTable();
				dtAcountType.Columns.Add("Id");
				dtAcountType.Columns.Add("Name");
				DataTable dtParentAcount = new DataTable();
				dtParentAcount.Columns.Add("Id");
				dtParentAcount.Columns.Add("Name");
				DataTable dtAcount = new DataTable();
				dtAcount.Columns.Add("Id");
				dtAcount.Columns.Add("Name");
				for (int i = 0; i < dtcombo.Rows.Count; i++)
				{
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "AccountType")
					{
						dtAcountType.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "ParentAccountTitle")
					{
						dtParentAcount.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
					if (Conversion.ToString(dtcombo.Rows[i]["Activity"]) == "AccountTitle")
					{
						dtAcount.Rows.Add(dtcombo.Rows[i]["Id"], dtcombo.Rows[i]["ReferenceName"]);
					}
				}
				if (dtAcountType.Rows.Count > 0)
				{
					DDL.BindDDL(dtAcountType, CmbAccountTypHistory, "Id", "Name", "Account Type", ZeroIndex: true);
					((UltraGridBase)CmbAccountTypHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((Control)(object)CmbAccountTypHistory).Text = string.Empty;
					((UltraGridBase)CmbAccountTypHistory).DataSource = null;
				}
				if (dtParentAcount.Rows.Count > 0)
				{
					DDL.BindDDL(dtParentAcount, CmbParentAccountHistory, "Id", "Name", "Parent Account Title", ZeroIndex: true);
					((UltraGridBase)CmbParentAccountHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((Control)(object)CmbParentAccountHistory).Text = string.Empty;
					((UltraGridBase)CmbParentAccountHistory).DataSource = null;
				}
				if (dtAcount.Rows.Count > 0)
				{
					DDL.BindDDL(dtAcount, CmbAccountTitleHistory, "Id", "Name", "Account Title", ZeroIndex: true);
					((UltraGridBase)CmbAccountTitleHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				}
				else
				{
					((Control)(object)CmbAccountTitleHistory).Text = string.Empty;
					((UltraGridBase)CmbAccountTitleHistory).DataSource = null;
				}
			}
			else
			{
				((Control)(object)CmbAccountTypHistory).Text = string.Empty;
				((UltraGridBase)CmbAccountTypHistory).DataSource = null;
				((Control)(object)CmbParentAccountHistory).Text = string.Empty;
				((UltraGridBase)CmbParentAccountHistory).DataSource = null;
				((Control)(object)CmbAccountTitleHistory).Text = string.Empty;
				((UltraGridBase)CmbAccountTitleHistory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void TabCOA_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (TabCOA.SelectedIndex == 1)
			{
				((Control)(object)grdFormHistory).Focus();
			}
			else
			{
				txtactitle.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FormhistoryBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormhistoryBind()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbTemplateHistory).ActiveRow != null && Conversion.ToInt(CmbTemplateHistory.Value) > 0)
			{
				Organization_ChartOfAccountTemplate obj = new Organization_ChartOfAccountTemplate();
				obj.OrganizationTemplateId = Conversion.ToInt(CmbTemplateHistory.Value);
				obj.AccountTypeId = Conversion.ToInt(CmbAccountTypHistory.Value);
				obj.ParentCodeId = Conversion.ToInt(CmbParentAccountHistory.Value);
				obj.Id = Conversion.ToInt(CmbAccountTitleHistory.Value);
				dtHistory = Organization_ChartOfAccountTemplate.FormHistory(obj);
				if (dtHistory.Rows.Count > 0)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("ParentCodeId", typeof(int));
					dt.Columns.Add("ParentAccountCode", typeof(string));
					dt.Columns.Add("ParentAccountTitle", typeof(string));
					dt.Columns.Add("AccountCode", typeof(string));
					dt.Columns.Add("AccountTitle", typeof(string));
					dt.Columns.Add("AccountTitleOtherLingo", typeof(string));
					dt.Columns.Add("OtherErpCode", typeof(string));
					dt.Columns.Add("AccountGroup", typeof(string));
					dt.Columns.Add("Account_Level", typeof(int));
					dt.Columns.Add("AccountClassId", typeof(int));
					dt.Columns.Add("AccountClass", typeof(string));
					dt.Columns.Add("AccountTypeId", typeof(int));
					dt.Columns.Add("AccountType", typeof(string));
					dt.Columns.Add("PLNoteId", typeof(int));
					dt.Columns.Add("PLNote", typeof(string));
					dt.Columns.Add("BSNoteId", typeof(int));
					dt.Columns.Add("BsNote", typeof(string));
					dt.Columns.Add("IsActive", typeof(bool));
					dt.Columns.Add("FinancialYearId", typeof(int));
					dt.Columns.Add("EntryDate", typeof(DateTime));
					dt.Columns.Add("EntryUserName", typeof(string));
					dt.Columns.Add("ModifyDate", typeof(DateTime));
					dt.Columns.Add("ModifyUserName", typeof(string));
					dt.Columns.Add("ApprovedDate", typeof(DateTime));
					dt.Columns.Add("ApprovedUserName", typeof(string));
					dt.Columns.Add("PostState", typeof(string));
					dt.Columns.Add("OrganizationTemplateId", typeof(int));
					dt.Columns.Add("TemplateDescription", typeof(string));
					dt.Columns.Add("QrCode", typeof(string));
					foreach (DataRow row in dtHistory.Rows)
					{
						if (Conversion.ToString(row["AccountGroup"]) == "Detail")
						{
							dt.Rows.Add(row["Id"], row["ParentCodeId"], row["ParentAccountCode"], row["ParentAccountTitle"], row["AccountCode"], row["AccountTitle"], row["AccountTitleOtherLingo"], row["OtherErpCode"], row["AccountGroup"], row["Account_Level"], row["AccountClass"], row["AccountClassName"], row["AccountTypeId"], row["AccountType"], row["PLNoteId"], row["PLNote"], row["BSNoteId"], row["BsNote"], row["IsActive"], row["FinancialYearId"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["PostDate"], row["ApprovedUserName"], row["PostUser"], row["OrganizationTemplateId"], row["TemplateDescription"], row["QrCode"]);
						}
					}
					grdFormHistory.DataSource = dt;
					grdFormHistory.RetrieveStructure();
					FormHistoryGridSettings();
				}
				else
				{
					grdFormHistory.ClearStructure();
				}
				return;
			}
			CmbTemplateHistory.Focus();
			throw new Exception("Please Select Template first...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FormHistoryGridSettings()
	{
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Expected O, but got Unknown
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		try
		{
			DataTable dttrue = new DataTable();
			dttrue.Columns.Add("Id");
			dttrue.Columns.Add("IsActive");
			dttrue.Rows.Add(0, "False");
			dttrue.Rows.Add(1, "True");
			grdFormHistory.RootTable.Columns["Id"].Visible = false;
			grdFormHistory.RootTable.Columns["AccountClassId"].Visible = false;
			grdFormHistory.RootTable.Columns["AccountTypeId"].Visible = false;
			grdFormHistory.RootTable.Columns["PLNoteId"].Visible = false;
			grdFormHistory.RootTable.Columns["BSNoteId"].Visible = false;
			grdFormHistory.RootTable.Columns["ParentCodeId"].Visible = false;
			grdFormHistory.RootTable.Columns["FinancialYearId"].Visible = false;
			grdFormHistory.RootTable.Columns["OrganizationTemplateId"].Visible = false;
			grdFormHistory.RootTable.Columns["QrCode"].Visible = false;
			grdFormHistory.RootTable.Columns["Id"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["AccountClassId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["AccountTypeId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["PLNoteId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["BSNoteId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["ParentCodeId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["FinancialYearId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["OrganizationTemplateId"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["QrCode"].EditType = (EditType)0;
			grdFormHistory.RootTable.Columns["AccountTitle"].EditType = (EditType)1;
			grdFormHistory.RootTable.Columns["OtherErpCode"].EditType = (EditType)1;
			grdFormHistory.RootTable.Columns["IsActive"].ColumnType = (ColumnType)0;
			grdFormHistory.RootTable.Columns["IsActive"].HasValueList = true;
			grdFormHistory.RootTable.Columns["IsActive"].LimitToList = true;
			grdFormHistory.RootTable.Columns["IsActive"].EditType = (EditType)4;
			grdFormHistory.RootTable.Columns["IsActive"].Caption = "IsActive";
			grdFormHistory.RootTable.Columns["IsActive"].ValueList.PopulateValueList((IEnumerable)dttrue.DefaultView, "Id", "IsActive");
			grdFormHistory.RootTable.Columns["IsActive"].Width = 60;
			grdFormHistory.RootTable.Columns["AccountTitle"].DefaultFilterRowComparison = (FilterConditionOperator)7;
			GridEXFormatCondition fc = new GridEXFormatCondition(grdFormHistory.RootTable.Columns["IsActive"], (ConditionOperator)0, (object)true);
			fc.TargetColumn = grdFormHistory.RootTable.Columns["IsActive"];
			fc.FormatStyle.ForeColor = Color.DarkGreen;
			grdFormHistory.RootTable.FormatConditions.Add(fc);
			GridEXFormatCondition fc2 = new GridEXFormatCondition(grdFormHistory.RootTable.Columns["IsActive"], (ConditionOperator)0, (object)false);
			fc2.TargetColumn = grdFormHistory.RootTable.Columns["IsActive"];
			fc2.FormatStyle.ForeColor = Color.Red;
			grdFormHistory.RootTable.FormatConditions.Add(fc2);
			grdFormHistory.RootTable.Columns["IsActive"].EditType = (EditType)4;
			grdFormHistory.RootTable.Columns["IsActive"].FilterEditType = (FilterEditType)2;
			CommonServices.GridAutoAdjustmentNew(grdFormHistory);
			grdFormHistory.RootTable.Columns.Add("Edit");
			grdFormHistory.RootTable.Columns["Edit"].Caption = "Update";
			grdFormHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFormHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdFormHistory.RootTable.Columns["Edit"].ButtonText = "Update";
			grdFormHistory.RootTable.Columns["Edit"].Width = 60;
			grdFormHistory.RootTable.Columns["Edit"].Position = 0;
			grdFormHistory.RootTable.Columns.Add("AddSibling");
			grdFormHistory.RootTable.Columns["AddSibling"].Caption = "AddSibling";
			grdFormHistory.RootTable.Columns["AddSibling"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdFormHistory.RootTable.Columns["AddSibling"].ButtonStyle = (ButtonStyle)5;
			grdFormHistory.RootTable.Columns["AddSibling"].ButtonText = "AddSibling";
			grdFormHistory.RootTable.Columns["AddSibling"].Width = 70;
			grdFormHistory.RootTable.Columns["AddSibling"].Position = 2;
			grdFormHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void grdFormHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				int TemplateId = Conversion.ToInt(CmbTemplate.Value);
				int num = Conversion.ToInt(grdFormHistory.CurrentRow.Cells["Id"].Value);
				string AccountTitle = Conversion.ToString(grdFormHistory.CurrentRow.Cells["AccountTitle"].Value);
				string OtherCode = Conversion.ToString(grdFormHistory.CurrentRow.Cells["OtherErpCode"].Value);
				bool IsApproved = false;
				IsApproved = Conversion.ToString((object)grdFormHistory.CurrentRow.Cells["IsActive"].Text) == "True";
				Organization_ChartOfAccountTemplate.UpdateAccountTitleById(num, TemplateId, AccountTitle, OtherCode, IsApproved);
				MessageBox.Show("Recored Update Successfully...;");
				FormhistoryBind();
				if (btnUpdate.Visible)
				{
					button1.Text = "Update";
				}
				else
				{
					button1.Text = "Save";
				}
			}
			if (e.Column.Key == "AddSibling")
			{
				btnsave.Visible = true;
				CmbParentAccount.Value = Conversion.ToInt((object)grdFormHistory.CurrentRow.Cells["ParentAccountCode"].Value.ToString());
				TabCOA.SelectedIndex = 0;
				cmbparentac_Leave(null, null);
				cmbactype_Leave(null, null);
				txtactitle.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbAccountTypHistory).Text = string.Empty;
			((Control)(object)CmbAccountTitleHistory).Text = string.Empty;
			((Control)(object)CmbParentAccountHistory).Text = string.Empty;
			FormhistoryBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void BtnRefreshHistory_Click(object sender, EventArgs e)
	{
		try
		{
			AllHistoryCombobind(Conversion.ToInt(CmbTemplateHistory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void CmbTemplateHistory_Leave(object sender, EventArgs e)
	{
		try
		{
			AllHistoryCombobind(Conversion.ToInt(CmbTemplateHistory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUploadExcelSheet_Click(object sender, EventArgs e)
	{
		try
		{
			new frmExcelSheetForCoaUpload(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ChartOfAccountTemplate_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (TabCOA.SelectedIndex == 1)
				{
					TabCOA.SelectedIndex = 0;
					txtactitle.Focus();
				}
				else
				{
					TabCOA.SelectedIndex = 1;
					((Control)(object)grdFormHistory).Focus();
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (TabCOA.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					if (btnsave.Visible && btnsave.Enabled)
					{
						btnsave_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.U)
				{
					if (btnUpdate.Visible && btnUpdate.Enabled)
					{
						btnUpdate_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.P)
				{
					if (btnPrintCOAform.Visible && btnPrintCOAform.Enabled)
					{
						BtnPrintCOAform_Click(null, null);
					}
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.F5)
				{
					txtactitle.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.F2)
				{
					btnUploadExcelSheet_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdfrm).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					txtactitle.Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Right)
				{
					if (((Control)(object)grdParentCode).Focused)
					{
						((Control)(object)grdfrm).Focus();
					}
					else if (((Control)(object)grdfrm).Focused)
					{
						((Control)(object)grdlevel2).Focus();
					}
					else if (((Control)(object)grdlevel2).Focused)
					{
						((Control)(object)grdlevel3).Focus();
					}
					else if (((Control)(object)grdlevel3).Focused)
					{
						((Control)(object)grdlevel4).Focus();
					}
					else if (((Control)(object)grdlevel4).Focused)
					{
						((Control)(object)grdParentCode).Focus();
					}
				}
				else if (e.Control && e.KeyCode == Keys.Left)
				{
					if (((Control)(object)grdlevel4).Focused)
					{
						((Control)(object)grdlevel3).Focus();
					}
					else if (((Control)(object)grdlevel3).Focused)
					{
						((Control)(object)grdlevel2).Focus();
					}
					else if (((Control)(object)grdlevel2).Focused)
					{
						((Control)(object)grdfrm).Focus();
					}
					else if (((Control)(object)grdfrm).Focused)
					{
						((Control)(object)grdParentCode).Focus();
					}
				}
			}
			else if (TabCOA.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdFormHistory).Focus();
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdFormHistory).Focus();
				}
				else if ((!e.Control || e.KeyCode != Keys.F2) && e.Control && e.KeyCode == Keys.P && btnPrintCOAform.Visible && btnPrintCOAform.Enabled)
				{
					BtnPrintCOAHistory_Click(null, null);
				}
			}
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Alt+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Company Name Text box");
			dt.Rows.Add("Ctrl+F1", "For Open Define City Form");
			dt.Rows.Add("Ctrl+F2", "For Open Upload Excel Sheet Form");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On AccountTitle Text box");
			dt.Rows.Add("Ctrl+ArrowRight", "To Change Focus Forward From one Grid to another");
			dt.Rows.Add("Ctrl+ArrowLeft", "To Change Focus Bakcward From one Grid to another");
			dt.Rows.Add("Ctrl+Enter", "When Focus On Any Grid For Update Record");
			dt.Rows.Add("Ctrl+Space", "When Focus On Any Grid To Call Function's On Button Or Link");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShortcutkeys_Click(object sender, EventArgs e)
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

	private void grdFormHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdFormHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdFormHistory.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (grdFormHistory.CurrentColumn.Key == "Edit")
				{
					if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					int TemplateId = Conversion.ToInt(CmbTemplate.Value);
					string AccountTitle = Conversion.ToString(r.Cells["AccountTitle"].Value);
					string OtherCode = Conversion.ToString(r.Cells["OtherErpCode"].Value);
					bool IsApproved = false;
					IsApproved = Conversion.ToString((object)r.Cells["IsActive"].Text) == "True";
					Organization_ChartOfAccountTemplate.UpdateAccountTitleById(Id, TemplateId, AccountTitle, OtherCode, IsApproved);
					MessageBox.Show("Recored Update Successfully...;");
					FormhistoryBind();
					if (btnUpdate.Visible)
					{
						button1.Text = "Update";
					}
					else
					{
						button1.Text = "Save";
					}
				}
				if (grdFormHistory.CurrentColumn.Key == "AddSibling")
				{
					btnsave.Visible = true;
					CmbParentAccount.Value = Conversion.ToInt((object)grdFormHistory.CurrentRow.Cells["ParentAccountCode"].Value.ToString());
					TabCOA.SelectedIndex = 0;
					cmbparentac_Leave(null, null);
					cmbactype_Leave(null, null);
					txtactitle.Focus();
				}
			}
			else if (e.Control && e.KeyCode == Keys.Return && formRights.DoHaveUpdateRights && MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				int TemplateId2 = Conversion.ToInt(CmbTemplate.Value);
				int num = Conversion.ToInt(r.Cells["Id"].Value);
				string AccountTitle2 = Conversion.ToString(r.Cells["AccountTitle"].Value);
				string OtherCode2 = Conversion.ToString(r.Cells["OtherErpCode"].Value);
				bool IsApproved2 = false;
				IsApproved2 = Conversion.ToString((object)r.Cells["IsActive"].Text) == "True";
				Organization_ChartOfAccountTemplate.UpdateAccountTitleById(num, TemplateId2, AccountTitle2, OtherCode2, IsApproved2);
				MessageBox.Show("Recored Update Successfully...;");
				FormhistoryBind();
				if (btnUpdate.Visible)
				{
					button1.Text = "Update";
				}
				else
				{
					button1.Text = "Save";
				}
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
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Expected O, but got Unknown
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Expected O, but got Unknown
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Expected O, but got Unknown
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Expected O, but got Unknown
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Expected O, but got Unknown
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Expected O, but got Unknown
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Expected O, but got Unknown
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Expected O, but got Unknown
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Expected O, but got Unknown
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Expected O, but got Unknown
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Expected O, but got Unknown
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Expected O, but got Unknown
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Expected O, but got Unknown
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Expected O, but got Unknown
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Expected O, but got Unknown
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Expected O, but got Unknown
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Expected O, but got Unknown
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Expected O, but got Unknown
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Expected O, but got Unknown
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Expected O, but got Unknown
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Expected O, but got Unknown
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Expected O, but got Unknown
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Expected O, but got Unknown
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Expected O, but got Unknown
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Expected O, but got Unknown
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Expected O, but got Unknown
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Expected O, but got Unknown
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Expected O, but got Unknown
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Expected O, but got Unknown
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Expected O, but got Unknown
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Expected O, but got Unknown
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d5: Expected O, but got Unknown
		//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Expected O, but got Unknown
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Expected O, but got Unknown
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Expected O, but got Unknown
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0206: Expected O, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Expected O, but got Unknown
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Expected O, but got Unknown
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Expected O, but got Unknown
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Expected O, but got Unknown
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Expected O, but got Unknown
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Expected O, but got Unknown
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Expected O, but got Unknown
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Expected O, but got Unknown
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Expected O, but got Unknown
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Expected O, but got Unknown
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Expected O, but got Unknown
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Expected O, but got Unknown
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Expected O, but got Unknown
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Expected O, but got Unknown
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Expected O, but got Unknown
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Expected O, but got Unknown
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Expected O, but got Unknown
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Expected O, but got Unknown
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0299: Expected O, but got Unknown
		//IL_0299: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_043a: Expected O, but got Unknown
		//IL_0467: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b3: Expected O, but got Unknown
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Expected O, but got Unknown
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058f: Expected O, but got Unknown
		//IL_059b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d1: Expected O, but got Unknown
		//IL_05e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_06a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ad: Expected O, but got Unknown
		//IL_06c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Expected O, but got Unknown
		//IL_06e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ef: Expected O, but got Unknown
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0710: Expected O, but got Unknown
		//IL_0753: Unknown result type (might be due to invalid IL or missing references)
		//IL_075d: Expected O, but got Unknown
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0794: Expected O, but got Unknown
		//IL_07ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b5: Expected O, but got Unknown
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07c0: Expected O, but got Unknown
		//IL_07e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ec: Expected O, but got Unknown
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Expected O, but got Unknown
		//IL_58b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c1: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance12 = new Appearance();
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
		Appearance appearance25 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Templates.ChartOfAccountTemplate));
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		this.printDialog1 = new System.Windows.Forms.PrintDialog();
		this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdParentCode = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.groupBox5 = new System.Windows.Forms.GroupBox();
		this.CmbTemplate = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.button1 = new System.Windows.Forms.Button();
		this.CmbParentAccount = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbGroupDetail = new UltraCombo();
		this.label6 = new System.Windows.Forms.Label();
		this.txtactitle = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label12 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.label21 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.txtOtherCode = new System.Windows.Forms.TextBox();
		this.label22 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.txtacclass = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.txtaccode = new System.Windows.Forms.TextBox();
		this.label4 = new System.Windows.Forms.Label();
		this.txtaclevel = new System.Windows.Forms.TextBox();
		this.CmbAccountTypes = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.CmbPLNote = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.CmbBSNote = new UltraCombo();
		this.label9 = new System.Windows.Forms.Label();
		this.DetailFilterGroup = new System.Windows.Forms.GroupBox();
		this.label16 = new System.Windows.Forms.Label();
		this.CmbAccountTitlefilter = new UltraCombo();
		this.BtnFilterInChildGrid = new System.Windows.Forms.Button();
		this.label15 = new System.Windows.Forms.Label();
		this.CmbAccountTypeFilter = new UltraCombo();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrintCOAform = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnUploadExcelSheet = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel1 = new System.Windows.Forms.Panel();
		this.TabCOA = new System.Windows.Forms.TabControl();
		this.TabForm = new System.Windows.Forms.TabPage();
		this.ParentPanel = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdlevel4 = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label32 = new System.Windows.Forms.Label();
		this.grdlevel3 = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label31 = new System.Windows.Forms.Label();
		this.grdlevel2 = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label30 = new System.Windows.Forms.Label();
		this.grdfrm = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label28 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label29 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.TabHistory = new System.Windows.Forms.TabPage();
		this.grdFormHistory = new GridEX();
		this.panel17 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel15 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbTemplateHistory = new UltraCombo();
		this.label27 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.CmbParentAccountHistory = new UltraCombo();
		this.CmbAccountTitleHistory = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.BtnShowHistory = new System.Windows.Forms.Button();
		this.label23 = new System.Windows.Forms.Label();
		this.CmbAccountTypHistory = new UltraCombo();
		this.panel16 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.BtnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnPrintCOAHistory = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.label24 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdParentCode).BeginInit();
		this.panel7.SuspendLayout();
		this.panel3.SuspendLayout();
		this.groupBox5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplate).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypes).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPLNote).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBSNote).BeginInit();
		this.DetailFilterGroup.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitlefilter).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypeFilter).BeginInit();
		this.toolStrip1.SuspendLayout();
		this.panel1.SuspendLayout();
		this.TabCOA.SuspendLayout();
		this.TabForm.SuspendLayout();
		this.ParentPanel.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel4).BeginInit();
		this.panel12.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel3).BeginInit();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel2).BeginInit();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).BeginInit();
		this.panel10.SuspendLayout();
		this.panel2.SuspendLayout();
		this.TabHistory.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdFormHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.panel15.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplateHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccountHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleHistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypHistory).BeginInit();
		this.panel16.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.printDialog1.UseEXDialog = true;
		this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.contextMenuStrip1.Name = "contextMenuStrip1";
		this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdParentCode);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.panel5.Location = new System.Drawing.Point(0, 53);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(616, 673);
		this.panel5.TabIndex = 2;
		this.grdParentCode.AllowEdit = (InheritableBoolean)2;
		this.grdParentCode.ColumnAutoResize = true;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdParentCode.FilterMode = (FilterMode)2;
		this.grdParentCode.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdParentCode.GroupByBoxVisible = false;
		this.grdParentCode.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Location = new System.Drawing.Point(0, 280);
		((System.Windows.Forms.Control)(object)this.grdParentCode).Name = "grdParentCode";
		((ScrollableBase)this.grdParentCode).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdParentCode).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdParentCode).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdParentCode).Size = new System.Drawing.Size(616, 393);
		((System.Windows.Forms.Control)(object)this.grdParentCode).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdParentCode).TabStop = false;
		this.grdParentCode.TotalRow = (InheritableBoolean)1;
		this.grdParentCode.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdParentCode).VisualStyle = (VisualStyle)4;
		this.grdParentCode.ColumnButtonClick += new ColumnActionEventHandler(grdParentCode_ColumnButtonClick);
		this.grdParentCode.SelectionChanged += new System.EventHandler(grdParentCode_SelectionChanged);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label26);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 258);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(616, 22);
		this.panel7.TabIndex = 75;
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.Teal;
		this.label26.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(6, 4);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(174, 17);
		this.label26.TabIndex = 0;
		this.label26.Text = "Child Of Selected Parent";
		this.panel3.Controls.Add(this.groupBox5);
		this.panel3.Controls.Add(this.DetailFilterGroup);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(616, 258);
		this.panel3.TabIndex = 0;
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTemplate);
		this.groupBox5.Controls.Add(this.label3);
		this.groupBox5.Controls.Add(this.button1);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccount);
		this.groupBox5.Controls.Add(this.label19);
		this.groupBox5.Controls.Add(this.label5);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbGroupDetail);
		this.groupBox5.Controls.Add(this.label6);
		this.groupBox5.Controls.Add(this.txtactitle);
		this.groupBox5.Controls.Add(this.label1);
		this.groupBox5.Controls.Add(this.label10);
		this.groupBox5.Controls.Add(this.label12);
		this.groupBox5.Controls.Add(this.label13);
		this.groupBox5.Controls.Add(this.label21);
		this.groupBox5.Controls.Add(this.label2);
		this.groupBox5.Controls.Add(this.txtOtherCode);
		this.groupBox5.Controls.Add(this.label22);
		this.groupBox5.Controls.Add(this.label20);
		this.groupBox5.Controls.Add(this.txtacclass);
		this.groupBox5.Controls.Add(this.label17);
		this.groupBox5.Controls.Add(this.label18);
		this.groupBox5.Controls.Add(this.txtaccode);
		this.groupBox5.Controls.Add(this.label4);
		this.groupBox5.Controls.Add(this.txtaclevel);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTypes);
		this.groupBox5.Controls.Add(this.label8);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPLNote);
		this.groupBox5.Controls.Add(this.label11);
		this.groupBox5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbBSNote);
		this.groupBox5.Controls.Add(this.label9);
		this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
		this.groupBox5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox5.Location = new System.Drawing.Point(0, 0);
		this.groupBox5.Name = "groupBox5";
		this.groupBox5.Size = new System.Drawing.Size(616, 190);
		this.groupBox5.TabIndex = 0;
		this.groupBox5.TabStop = false;
		this.groupBox5.Text = "Add New Account";
		this.CmbTemplate.AllowNull = (DefaultableBoolean)1;
		this.CmbTemplate.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTemplate.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTemplate.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Appearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance40).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance41;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance49).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance50;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTemplate).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbTemplate.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Location = new System.Drawing.Point(106, 18);
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Name = "CmbTemplate";
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbTemplate).TabIndex = 207;
		((System.Windows.Forms.Control)(object)this.CmbTemplate).Leave += new System.EventHandler(CmbTemplate_Leave);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(6, 24);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(56, 15);
		this.label3.TabIndex = 208;
		this.label3.Text = "Template";
		this.button1.BackColor = System.Drawing.Color.Teal;
		this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.button1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.button1.ForeColor = System.Drawing.Color.White;
		this.button1.Location = new System.Drawing.Point(545, 159);
		this.button1.Name = "button1";
		this.button1.Size = new System.Drawing.Size(65, 26);
		this.button1.TabIndex = 16;
		this.button1.Text = "Update";
		this.button1.UseVisualStyleBackColor = false;
		this.button1.Click += new System.EventHandler(button1_Click);
		this.CmbParentAccount.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAccount.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAccount.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAccount.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Appearance = (AppearanceBase)(object)appearance103;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance104).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance104).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance104).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance104).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance104;
		((AppearanceBase)appearance105).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance105;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance106).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance106).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance106).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance107;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance108).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance108;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance109;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance110).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance111).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance111).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance111).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance111).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance113).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance114).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAccount).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentAccount.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Location = new System.Drawing.Point(106, 45);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Name = "CmbParentAccount";
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbParentAccount).Leave += new System.EventHandler(cmbparentac_Leave);
		this.label19.AutoSize = true;
		this.label19.BackColor = System.Drawing.Color.White;
		this.label19.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.ForeColor = System.Drawing.Color.Red;
		this.label19.Location = new System.Drawing.Point(394, 23);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(14, 17);
		this.label19.TabIndex = 34;
		this.label19.Text = "*";
		this.label5.AutoSize = true;
		this.label5.BackColor = System.Drawing.Color.White;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 79);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(78, 15);
		this.label5.TabIndex = 2;
		this.label5.Text = "Account Title";
		this.CmbGroupDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbGroupDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbGroupDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbGroupDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroupDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbGroupDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbGroupDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbGroupDetail).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbGroupDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbGroupDetail).Location = new System.Drawing.Point(409, 18);
		((System.Windows.Forms.Control)(object)this.CmbGroupDetail).Name = "CmbGroupDetail";
		((System.Windows.Forms.Control)(object)this.CmbGroupDetail).Size = new System.Drawing.Size(201, 26);
		((System.Windows.Forms.Control)(object)this.CmbGroupDetail).TabIndex = 8;
		((System.Windows.Forms.Control)(object)this.CmbGroupDetail).Leave += new System.EventHandler(cmbgroupdetail_Leave);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(322, 24);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 15);
		this.label6.TabIndex = 11;
		this.label6.Text = "Group/Detail";
		this.txtactitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtactitle.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtactitle.Location = new System.Drawing.Point(106, 75);
		this.txtactitle.Name = "txtactitle";
		this.txtactitle.Size = new System.Drawing.Size(210, 23);
		this.txtactitle.TabIndex = 1;
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.White;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(89, 15);
		this.label1.TabIndex = 0;
		this.label1.Text = "Parent Account";
		this.label10.AutoSize = true;
		this.label10.BackColor = System.Drawing.Color.Transparent;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.Red;
		this.label10.Location = new System.Drawing.Point(92, 50);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(14, 17);
		this.label10.TabIndex = 17;
		this.label10.Text = "*";
		this.label12.AutoSize = true;
		this.label12.BackColor = System.Drawing.Color.White;
		this.label12.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.Red;
		this.label12.Location = new System.Drawing.Point(92, 78);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(14, 17);
		this.label12.TabIndex = 18;
		this.label12.Text = "*";
		this.label13.AutoSize = true;
		this.label13.BackColor = System.Drawing.Color.White;
		this.label13.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.ForeColor = System.Drawing.Color.Red;
		this.label13.Location = new System.Drawing.Point(92, 23);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(14, 17);
		this.label13.TabIndex = 67;
		this.label13.Text = "*";
		this.label21.AutoSize = true;
		this.label21.BackColor = System.Drawing.Color.White;
		this.label21.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.ForeColor = System.Drawing.Color.Red;
		this.label21.Location = new System.Drawing.Point(92, 164);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(14, 17);
		this.label21.TabIndex = 67;
		this.label21.Text = "*";
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(322, 79);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(81, 15);
		this.label2.TabIndex = 2;
		this.label2.Text = "Account Class";
		this.txtOtherCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOtherCode.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtOtherCode.Location = new System.Drawing.Point(409, 132);
		this.txtOtherCode.Name = "txtOtherCode";
		this.txtOtherCode.Size = new System.Drawing.Size(201, 24);
		this.txtOtherCode.TabIndex = 12;
		this.label22.AutoSize = true;
		this.label22.BackColor = System.Drawing.Color.White;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(322, 137);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(67, 15);
		this.label22.TabIndex = 2;
		this.label22.Text = "Other Code";
		this.label20.AutoSize = true;
		this.label20.BackColor = System.Drawing.Color.White;
		this.label20.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.ForeColor = System.Drawing.Color.Red;
		this.label20.Location = new System.Drawing.Point(92, 136);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(14, 17);
		this.label20.TabIndex = 66;
		this.label20.Text = "*";
		this.txtacclass.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtacclass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtacclass.Enabled = false;
		this.txtacclass.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtacclass.Location = new System.Drawing.Point(409, 74);
		this.txtacclass.Name = "txtacclass";
		this.txtacclass.ReadOnly = true;
		this.txtacclass.Size = new System.Drawing.Size(201, 24);
		this.txtacclass.TabIndex = 10;
		this.txtacclass.TabStop = false;
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.White;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(322, 51);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(82, 15);
		this.label17.TabIndex = 32;
		this.label17.Text = "Account Code";
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.White;
		this.label18.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.ForeColor = System.Drawing.Color.Red;
		this.label18.Location = new System.Drawing.Point(92, 107);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(14, 17);
		this.label18.TabIndex = 33;
		this.label18.Text = "*";
		this.txtaccode.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtaccode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaccode.Enabled = false;
		this.txtaccode.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtaccode.Location = new System.Drawing.Point(409, 46);
		this.txtaccode.Name = "txtaccode";
		this.txtaccode.ReadOnly = true;
		this.txtaccode.Size = new System.Drawing.Size(201, 24);
		this.txtaccode.TabIndex = 9;
		this.txtaccode.TabStop = false;
		this.txtaccode.Text = " ";
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.White;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(322, 108);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(82, 15);
		this.label4.TabIndex = 9;
		this.label4.Text = "Account Level";
		this.txtaclevel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
		this.txtaclevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtaclevel.Enabled = false;
		this.txtaclevel.Font = new System.Drawing.Font("Verdana", 10.25f);
		this.txtaclevel.Location = new System.Drawing.Point(409, 103);
		this.txtaclevel.Name = "txtaclevel";
		this.txtaclevel.ReadOnly = true;
		this.txtaclevel.Size = new System.Drawing.Size(201, 24);
		this.txtaclevel.TabIndex = 11;
		this.txtaclevel.TabStop = false;
		this.CmbAccountTypes.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTypes.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTypes.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTypes.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Appearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypes).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance118;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypes).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance119).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance119).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance119).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance120;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance121).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance121;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance122).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance122;
		((AppearanceBase)appearance124).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance124).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance125).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance125).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance125).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance125).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance127).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance127;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance128).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTypes).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypes).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountTypes.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypes).Location = new System.Drawing.Point(106, 102);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypes).Name = "CmbAccountTypes";
		((System.Windows.Forms.Control)(object)this.CmbAccountTypes).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypes).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypes).Leave += new System.EventHandler(cmbactype_Leave);
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.White;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 108);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(80, 15);
		this.label8.TabIndex = 4;
		this.label8.Text = "Account Type";
		this.CmbPLNote.AllowNull = (DefaultableBoolean)1;
		this.CmbPLNote.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPLNote.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPLNote.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPLNote).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.CmbPLNote).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance75;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPLNote).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPLNote).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPLNote.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPLNote).Location = new System.Drawing.Point(106, 131);
		((System.Windows.Forms.Control)(object)this.CmbPLNote).Name = "CmbPLNote";
		((System.Windows.Forms.Control)(object)this.CmbPLNote).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbPLNote).TabIndex = 3;
		this.label11.AutoSize = true;
		this.label11.BackColor = System.Drawing.Color.White;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(6, 165);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 8;
		this.label11.Text = "B/S Notes";
		this.CmbBSNote.AllowNull = (DefaultableBoolean)1;
		this.CmbBSNote.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbBSNote.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbBSNote.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance57).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance57).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbBSNote).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance68;
		((SpecialBoxBase)((UltraGridBase)this.CmbBSNote).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance79).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance79).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance79).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance90;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance101;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance112;
		((AppearanceBase)appearance123).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance123).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance123;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance134).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance134).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance134).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance134).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance134;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbBSNote).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbBSNote).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbBSNote.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbBSNote).Location = new System.Drawing.Point(106, 159);
		((System.Windows.Forms.Control)(object)this.CmbBSNote).Name = "CmbBSNote";
		((System.Windows.Forms.Control)(object)this.CmbBSNote).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbBSNote).TabIndex = 4;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.White;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(6, 137);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(59, 15);
		this.label9.TabIndex = 12;
		this.label9.Text = "P/L Notes";
		this.DetailFilterGroup.Controls.Add(this.label16);
		this.DetailFilterGroup.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitlefilter);
		this.DetailFilterGroup.Controls.Add(this.BtnFilterInChildGrid);
		this.DetailFilterGroup.Controls.Add(this.label15);
		this.DetailFilterGroup.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter);
		this.DetailFilterGroup.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.DetailFilterGroup.Location = new System.Drawing.Point(0, 189);
		this.DetailFilterGroup.Name = "DetailFilterGroup";
		this.DetailFilterGroup.Size = new System.Drawing.Size(616, 68);
		this.DetailFilterGroup.TabIndex = 1;
		this.DetailFilterGroup.TabStop = false;
		this.DetailFilterGroup.Text = "Filter Accounts In Child Grid";
		this.label16.AutoSize = true;
		this.label16.BackColor = System.Drawing.Color.White;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(2, 18);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(80, 15);
		this.label16.TabIndex = 31;
		this.label16.Text = "Account Type";
		this.CmbAccountTitlefilter.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitlefilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitlefilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitlefilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance91).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance91).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance92;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance94).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance96;
		((AppearanceBase)appearance97).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance97).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance97;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance98).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance98).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance98).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance98).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance98;
		((AppearanceBase)appearance99).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance99;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance100;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance102).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitlefilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitlefilter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccountTitlefilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitlefilter).Location = new System.Drawing.Point(225, 38);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitlefilter).Name = "CmbAccountTitlefilter";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitlefilter).Size = new System.Drawing.Size(316, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitlefilter).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitlefilter).Leave += new System.EventHandler(CmbAccountTitle_Leave);
		this.BtnFilterInChildGrid.BackColor = System.Drawing.Color.Teal;
		this.BtnFilterInChildGrid.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnFilterInChildGrid.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnFilterInChildGrid.ForeColor = System.Drawing.Color.White;
		this.BtnFilterInChildGrid.Location = new System.Drawing.Point(545, 38);
		this.BtnFilterInChildGrid.Name = "BtnFilterInChildGrid";
		this.BtnFilterInChildGrid.Size = new System.Drawing.Size(65, 26);
		this.BtnFilterInChildGrid.TabIndex = 2;
		this.BtnFilterInChildGrid.Text = "Filter";
		this.BtnFilterInChildGrid.UseVisualStyleBackColor = false;
		this.BtnFilterInChildGrid.Visible = false;
		this.BtnFilterInChildGrid.Click += new System.EventHandler(BtnFilterInChildGrid_Click);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.White;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(225, 18);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(78, 15);
		this.label15.TabIndex = 29;
		this.label15.Text = "Account Title";
		this.CmbAccountTypeFilter.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTypeFilter.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTypeFilter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTypeFilter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance142;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance143).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance143).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance143).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance143).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance143;
		((AppearanceBase)appearance144).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance144;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTypeFilter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccountTypeFilter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Location = new System.Drawing.Point(3, 38);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Name = "CmbAccountTypeFilter";
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypeFilter).Leave += new System.EventHandler(CmbAccountTypeFilter_Leave);
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.toolStripSeparator4, this.btnsave, this.btnUpdate, this.toolStripSeparator2, this.btnPrintCOAform, this.toolStripSeparator3, this.btnUploadExcelSheet, this.btnShortcutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1090, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = (System.Drawing.Image)resources.GetObject("btnUpdate.Image");
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPrintCOAform.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrintCOAform.Image = (System.Drawing.Image)resources.GetObject("btnPrintCOAform.Image");
		this.btnPrintCOAform.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrintCOAform.Name = "btnPrintCOAform";
		this.btnPrintCOAform.Size = new System.Drawing.Size(116, 24);
		this.btnPrintCOAform.Text = "114_01-Print";
		this.btnPrintCOAform.Click += new System.EventHandler(BtnPrintCOAform_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnUploadExcelSheet.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUploadExcelSheet.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnUploadExcelSheet.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUploadExcelSheet.Name = "btnUploadExcelSheet";
		this.btnUploadExcelSheet.Size = new System.Drawing.Size(158, 24);
		this.btnUploadExcelSheet.Text = "Upload Excel Sheet";
		this.btnUploadExcelSheet.Click += new System.EventHandler(btnUploadExcelSheet_Click);
		this.btnShortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutkeys.Name = "btnShortcutkeys";
		this.btnShortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutkeys.Text = "ShortCut Keys";
		this.btnShortcutkeys.Click += new System.EventHandler(btnShortcutkeys_Click);
		this.panel1.AutoScroll = true;
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.TabCOA);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1104, 761);
		this.panel1.TabIndex = 1;
		this.TabCOA.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.TabCOA.Controls.Add(this.TabForm);
		this.TabCOA.Controls.Add(this.TabHistory);
		this.TabCOA.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TabCOA.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TabCOA.Location = new System.Drawing.Point(0, 0);
		this.TabCOA.Name = "TabCOA";
		this.TabCOA.SelectedIndex = 0;
		this.TabCOA.Size = new System.Drawing.Size(1104, 761);
		this.TabCOA.TabIndex = 67;
		this.TabCOA.SelectedIndexChanged += new System.EventHandler(TabCOA_SelectedIndexChanged);
		this.TabForm.AutoScroll = true;
		this.TabForm.Controls.Add(this.ParentPanel);
		this.TabForm.Location = new System.Drawing.Point(4, 4);
		this.TabForm.Name = "TabForm";
		this.TabForm.Padding = new System.Windows.Forms.Padding(3);
		this.TabForm.Size = new System.Drawing.Size(1096, 732);
		this.TabForm.TabIndex = 1;
		this.TabForm.Text = "Form";
		this.TabForm.UseVisualStyleBackColor = true;
		this.ParentPanel.AutoScroll = true;
		this.ParentPanel.Controls.Add(this.panel6);
		this.ParentPanel.Controls.Add(this.panel5);
		this.ParentPanel.Controls.Add(this.panel2);
		this.ParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ParentPanel.Location = new System.Drawing.Point(3, 3);
		this.ParentPanel.Name = "ParentPanel";
		this.ParentPanel.Size = new System.Drawing.Size(1090, 726);
		this.ParentPanel.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdlevel4);
		this.panel6.Controls.Add(this.panel12);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdlevel3);
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdlevel2);
		this.panel6.Controls.Add(this.panel4);
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdfrm);
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(616, 53);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(473, 673);
		this.panel6.TabIndex = 65;
		this.grdlevel4.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdlevel4.FilterMode = (FilterMode)2;
		this.grdlevel4.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlevel4.GroupByBoxVisible = false;
		this.grdlevel4.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Location = new System.Drawing.Point(0, 562);
		((System.Windows.Forms.Control)(object)this.grdlevel4).Name = "grdlevel4";
		((ScrollableBase)this.grdlevel4).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdlevel4).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdlevel4).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlevel4).Size = new System.Drawing.Size(473, 111);
		((System.Windows.Forms.Control)(object)this.grdlevel4).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.grdlevel4).TabStop = false;
		this.grdlevel4.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlevel4).VisualStyle = (VisualStyle)4;
		this.grdlevel4.SelectionChanged += new System.EventHandler(grdlevel4_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdlevel4).DoubleClick += new System.EventHandler(grdlevel4_DoubleClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label32);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 540);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(473, 22);
		this.panel12.TabIndex = 84;
		this.label32.AutoSize = true;
		this.label32.BackColor = System.Drawing.Color.Teal;
		this.label32.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label32.ForeColor = System.Drawing.Color.White;
		this.label32.Location = new System.Drawing.Point(6, 4);
		this.label32.Name = "label32";
		this.label32.Size = new System.Drawing.Size(281, 17);
		this.label32.TabIndex = 0;
		this.label32.Text = "4th Level Account Of Selected 3rd Level";
		this.grdlevel3.AllowColumnDrag = false;
		this.grdlevel3.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdlevel3.FilterMode = (FilterMode)2;
		this.grdlevel3.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlevel3.GroupByBoxVisible = false;
		this.grdlevel3.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Location = new System.Drawing.Point(0, 360);
		((System.Windows.Forms.Control)(object)this.grdlevel3).Name = "grdlevel3";
		((ScrollableBase)this.grdlevel3).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdlevel3).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdlevel3).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlevel3).Size = new System.Drawing.Size(473, 180);
		((System.Windows.Forms.Control)(object)this.grdlevel3).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.grdlevel3).TabStop = false;
		this.grdlevel3.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlevel3).VisualStyle = (VisualStyle)4;
		this.grdlevel3.SelectionChanged += new System.EventHandler(grdlevel3_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdlevel3).DoubleClick += new System.EventHandler(grdlevel3_DoubleClick);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label31);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 338);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(473, 22);
		this.panel11.TabIndex = 83;
		this.label31.AutoSize = true;
		this.label31.BackColor = System.Drawing.Color.Teal;
		this.label31.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label31.ForeColor = System.Drawing.Color.White;
		this.label31.Location = new System.Drawing.Point(6, 4);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(284, 17);
		this.label31.TabIndex = 0;
		this.label31.Text = "3rd Level Account Of Selected 2nd Level";
		this.grdlevel2.AllowCardSizing = false;
		this.grdlevel2.AllowColumnDrag = false;
		this.grdlevel2.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdlevel2.FilterMode = (FilterMode)2;
		this.grdlevel2.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdlevel2.GroupByBoxVisible = false;
		this.grdlevel2.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Location = new System.Drawing.Point(0, 181);
		((System.Windows.Forms.Control)(object)this.grdlevel2).Name = "grdlevel2";
		((ScrollableBase)this.grdlevel2).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdlevel2).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdlevel2).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdlevel2).Size = new System.Drawing.Size(473, 157);
		((System.Windows.Forms.Control)(object)this.grdlevel2).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.grdlevel2).TabStop = false;
		this.grdlevel2.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdlevel2).VisualStyle = (VisualStyle)4;
		this.grdlevel2.SelectionChanged += new System.EventHandler(grdlevel2_SelectionChanged);
		((System.Windows.Forms.Control)(object)this.grdlevel2).DoubleClick += new System.EventHandler(grdlevel2_DoubleClick);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label30);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 159);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(473, 22);
		this.panel4.TabIndex = 81;
		this.label30.AutoSize = true;
		this.label30.BackColor = System.Drawing.Color.Teal;
		this.label30.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label30.ForeColor = System.Drawing.Color.White;
		this.label30.Location = new System.Drawing.Point(6, 4);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(279, 17);
		this.label30.TabIndex = 0;
		this.label30.Text = "2rd Level Account Of Selected 1st Level";
		this.grdfrm.AllowCardSizing = false;
		this.grdfrm.AllowColumnDrag = false;
		this.grdfrm.AllowEdit = (InheritableBoolean)2;
		((System.Windows.Forms.Control)(object)this.grdfrm).Dock = System.Windows.Forms.DockStyle.Top;
		this.grdfrm.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdfrm).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdfrm.GroupByBoxVisible = false;
		this.grdfrm.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Location = new System.Drawing.Point(0, 22);
		((System.Windows.Forms.Control)(object)this.grdfrm).Name = "grdfrm";
		((ScrollableBase)this.grdfrm).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdfrm).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdfrm).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdfrm).Size = new System.Drawing.Size(473, 137);
		((System.Windows.Forms.Control)(object)this.grdfrm).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdfrm).TabStop = false;
		this.grdfrm.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdfrm).VisualStyle = (VisualStyle)4;
		this.grdfrm.SelectionChanged += new System.EventHandler(grdfrm_SelectionChanged);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label28);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(473, 22);
		this.panel10.TabIndex = 79;
		this.label28.AutoSize = true;
		this.label28.BackColor = System.Drawing.Color.Teal;
		this.label28.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label28.ForeColor = System.Drawing.Color.White;
		this.label28.Location = new System.Drawing.Point(6, 4);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(130, 17);
		this.label28.TabIndex = 0;
		this.label28.Text = "1st Level Account";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label29);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Controls.Add(this.label7);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 0);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1090, 53);
		this.panel2.TabIndex = 0;
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label29.ForeColor = System.Drawing.Color.White;
		this.label29.Location = new System.Drawing.Point(616, 39);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(1282, 15);
		this.label29.TabIndex = 584;
		this.label29.Text = resources.GetString("label29.Text");
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(7, 30);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(227, 19);
		this.label7.TabIndex = 0;
		this.label7.Text = "Chart Of Account                ";
		this.TabHistory.Controls.Add((System.Windows.Forms.Control)(object)this.grdFormHistory);
		this.TabHistory.Controls.Add(this.panel17);
		this.TabHistory.Controls.Add(this.panel15);
		this.TabHistory.Controls.Add(this.panel16);
		this.TabHistory.Location = new System.Drawing.Point(4, 4);
		this.TabHistory.Name = "TabHistory";
		this.TabHistory.Padding = new System.Windows.Forms.Padding(3);
		this.TabHistory.Size = new System.Drawing.Size(1096, 732);
		this.TabHistory.TabIndex = 0;
		this.TabHistory.Text = "History";
		this.TabHistory.UseVisualStyleBackColor = true;
		this.grdFormHistory.AllowCardSizing = false;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdFormHistory.FilterMode = (FilterMode)2;
		this.grdFormHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdFormHistory.GroupTotals = (GroupTotals)2;
		this.grdFormHistory.Hierarchical = true;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Location = new System.Drawing.Point(3, 150);
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Name = "grdFormHistory";
		((ScrollableBase)this.grdFormHistory).Office2007ColorScheme = (Office2007ColorScheme)1;
		((ScrollableBase)this.grdFormHistory).Office2007CustomColor = System.Drawing.Color.FromArgb(0, 192, 192);
		((ScrollableBase)this.grdFormHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).Size = new System.Drawing.Size(1090, 579);
		((System.Windows.Forms.Control)(object)this.grdFormHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdFormHistory).TabStop = false;
		this.grdFormHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdFormHistory).VisualStyle = (VisualStyle)4;
		this.grdFormHistory.ColumnButtonClick += new ColumnActionEventHandler(grdFormHistory_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdFormHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdFormHistory_KeyDown);
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.ctrlGrdBar2);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(3, 125);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(1090, 25);
		this.panel17.TabIndex = 67;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1047, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdFormHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(40, 23);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel15.AutoScroll = true;
		this.panel15.Controls.Add(this.groupBox1);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(3, 54);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1090, 71);
		this.panel15.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTemplateHistory);
		this.groupBox1.Controls.Add(this.label27);
		this.groupBox1.Controls.Add(this.label14);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentAccountHistory);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory);
		this.groupBox1.Controls.Add(this.label25);
		this.groupBox1.Controls.Add(this.BtnShowHistory);
		this.groupBox1.Controls.Add(this.label23);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountTypHistory);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1083, 68);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filter Accounts In Child Grid";
		this.CmbTemplateHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbTemplateHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTemplateHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTemplateHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance52).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance52;
		((SpecialBoxBase)((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance53).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance53).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance53).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance53).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance55).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance56;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance58).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTemplateHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbTemplateHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTemplateHistory).Location = new System.Drawing.Point(6, 36);
		((System.Windows.Forms.Control)(object)this.CmbTemplateHistory).Name = "CmbTemplateHistory";
		((System.Windows.Forms.Control)(object)this.CmbTemplateHistory).Size = new System.Drawing.Size(210, 26);
		((System.Windows.Forms.Control)(object)this.CmbTemplateHistory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbTemplateHistory).Leave += new System.EventHandler(CmbTemplateHistory_Leave);
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label27.Location = new System.Drawing.Point(6, 18);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(56, 15);
		this.label27.TabIndex = 210;
		this.label27.Text = "Template";
		this.label14.AutoSize = true;
		this.label14.BackColor = System.Drawing.Color.White;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(218, 18);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(80, 15);
		this.label14.TabIndex = 31;
		this.label14.Text = "Account Type";
		this.CmbParentAccountHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentAccountHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentAccountHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentAccountHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance129;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance130).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance130).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance130).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance130).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance130;
		((AppearanceBase)appearance131).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance131;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance132).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance132).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance132).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance133;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance135).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance135;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance136).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance136;
		((AppearanceBase)appearance137).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance137).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance138).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance138).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance138).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance138).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance140).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance140;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance141).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentAccountHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentAccountHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbParentAccountHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentAccountHistory).Location = new System.Drawing.Point(441, 36);
		((System.Windows.Forms.Control)(object)this.CmbParentAccountHistory).Name = "CmbParentAccountHistory";
		((System.Windows.Forms.Control)(object)this.CmbParentAccountHistory).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentAccountHistory).TabIndex = 2;
		this.CmbAccountTitleHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTitleHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTitleHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTitleHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance28;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance29).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance29).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance30;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance33).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTitleHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccountTitleHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Location = new System.Drawing.Point(674, 36);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Name = "CmbAccountTitleHistory";
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).Size = new System.Drawing.Size(230, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTitleHistory).TabIndex = 3;
		this.label25.AutoSize = true;
		this.label25.BackColor = System.Drawing.Color.White;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(674, 18);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(78, 15);
		this.label25.TabIndex = 29;
		this.label25.Text = "Account Title";
		this.BtnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.BtnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
		this.BtnShowHistory.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShowHistory.ForeColor = System.Drawing.Color.White;
		this.BtnShowHistory.Location = new System.Drawing.Point(907, 36);
		this.BtnShowHistory.Name = "BtnShowHistory";
		this.BtnShowHistory.Size = new System.Drawing.Size(57, 26);
		this.BtnShowHistory.TabIndex = 4;
		this.BtnShowHistory.Text = "Show";
		this.BtnShowHistory.UseVisualStyleBackColor = false;
		this.BtnShowHistory.Click += new System.EventHandler(BtnShowHistory_Click);
		this.label23.AutoSize = true;
		this.label23.BackColor = System.Drawing.Color.White;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(441, 18);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(78, 15);
		this.label23.TabIndex = 29;
		this.label23.Text = "Account Title";
		this.CmbAccountTypHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountTypHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountTypHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountTypHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance76).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance77).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance77).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance80).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance80).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance80).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance83;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance84).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance85).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance85).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance85).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance85;
		((AppearanceBase)appearance86).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance86;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance87).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountTypHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypHistory).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbAccountTypHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountTypHistory).Location = new System.Drawing.Point(218, 36);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypHistory).Name = "CmbAccountTypHistory";
		((System.Windows.Forms.Control)(object)this.CmbAccountTypHistory).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountTypHistory).TabIndex = 1;
		this.panel16.BackColor = System.Drawing.Color.Teal;
		this.panel16.Controls.Add(this.toolStrip2);
		this.panel16.Controls.Add(this.label24);
		this.panel16.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel16.Location = new System.Drawing.Point(3, 3);
		this.panel16.Name = "panel16";
		this.panel16.Size = new System.Drawing.Size(1090, 51);
		this.panel16.TabIndex = 66;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripSeparator7, this.BtnNewHistory, this.BtnRefreshHistory, this.toolStripSeparator5, this.BtnPrintCOAHistory, this.toolStripSeparator1 });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1090, 27);
		this.toolStrip2.TabIndex = 25;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator7.Name = "toolStripSeparator7";
		this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.BtnNewHistory.Text = "&New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.BtnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefreshHistory.Name = "BtnRefreshHistory";
		this.BtnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.BtnRefreshHistory.Text = "Refresh";
		this.BtnRefreshHistory.Click += new System.EventHandler(BtnRefreshHistory_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.BtnPrintCOAHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnPrintCOAHistory.Image = (System.Drawing.Image)resources.GetObject("BtnPrintCOAHistory.Image");
		this.BtnPrintCOAHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnPrintCOAHistory.Name = "BtnPrintCOAHistory";
		this.BtnPrintCOAHistory.Size = new System.Drawing.Size(116, 24);
		this.BtnPrintCOAHistory.Text = "114_01-Print";
		this.BtnPrintCOAHistory.Click += new System.EventHandler(BtnPrintCOAHistory_Click);
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(5, 30);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(213, 19);
		this.label24.TabIndex = 0;
		this.label24.Text = "Chart OF Account History";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1116, 4);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = null;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(46, 21);
		this.ctrlGrdBar1.TabIndex = 20;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load_1);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1104, 761);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Name = "ChartOfAccountTemplate";
		this.Text = "Eccount Book";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(ChartOfAccountTemplate_FormClosed);
		base.Load += new System.EventHandler(ChartOfAccountTemplate_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(ChartOfAccountTemplate_KeyDown);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdParentCode).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.groupBox5.ResumeLayout(false);
		this.groupBox5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplate).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccount).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbGroupDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypes).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPLNote).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbBSNote).EndInit();
		this.DetailFilterGroup.ResumeLayout(false);
		this.DetailFilterGroup.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitlefilter).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypeFilter).EndInit();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel1.ResumeLayout(false);
		this.TabCOA.ResumeLayout(false);
		this.TabForm.ResumeLayout(false);
		this.ParentPanel.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdlevel4).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel3).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdlevel2).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdfrm).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.TabHistory.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdFormHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbTemplateHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbParentAccountHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTitleHistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountTypHistory).EndInit();
		this.panel16.ResumeLayout(false);
		this.panel16.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
