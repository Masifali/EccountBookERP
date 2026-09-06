using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.Common;
using Architecture.Model;
using Architecture.WinApp.Common;
using Architecture.WinApp.Configurations;
using Architecture.WinApp.Properties;
using Infragistics.Shared;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp;

public class frmAppMenu_ScreensDefine : Form
{
	private UserAccount UserAccount = new UserAccount();

	private int RecId;

	private int RecIdDocType;

	private int RecIdMethodDefine;

	private DropDownBind DDL = new DropDownBind();

	private bool UpdateMode;

	private DataTable dtScreenDef = new DataTable();

	private DataTable dtCompany = new DataTable();

	private IContainer components;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private Panel panel2;

	internal UltraCombo cmbModuleName;

	private Label label3;

	private Panel panel3;

	private Panel panel5;

	private Panel panel4;

	private TextBox txtScreenName;

	private TextBox txtScreenAlias;

	private Label label1;

	private Label label2;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private Panel panel6;

	private Label label4;

	private Label label5;

	private TextBox txtTargetUrl;

	private CheckBox ChkIsActive;

	private Label label6;

	private TextBox txtSortNo;

	private ToolStripButton toolStripButton1;

	internal GridEX grdScreenDefination;

	private Panel panel7;

	internal GridEX grdScreenRights;

	private Panel panel8;

	private TabControl tabControl1;

	private TabPage tabSD;

	private Panel panel1;

	private TabPage tabDT;

	private Label label8;

	private Label label7;

	private Panel panel10;

	private Label label10;

	private Label label11;

	private TextBox txtDocTypeDescription;

	private TextBox txtDocTypeTargetUrl;

	private Label label13;

	private TextBox txtDocTypeCode;

	private TextBox txtDocTypeId;

	private Label label14;

	private Panel panel9;

	private Label label9;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewDocType;

	private ToolStripButton BtnSaveDocType;

	private ToolStripButton BtnUpdateDocType;

	internal GridEX grdDocType;

	private Panel panel11;

	private Label label17;

	private Label label16;

	private Label label15;

	private TextBox txtDocTypeScreenName;

	private TextBox txtDocTypeOtherLangDescription;

	private TextBox txtDocTypeRemarks;

	private Label label12;

	private TabPage tabMethods;

	internal GridEX grdMethodsDefine;

	private Panel panel12;

	private Label label18;

	private Panel panel13;

	private Label label19;

	private TextBox txtScreenNameMethodDefine;

	private TextBox txtRemarksMethodDefine;

	private TextBox txtReportMethod;

	private Label label24;

	private Label label25;

	private Panel panel14;

	private Label label26;

	private ToolStrip toolStrip3;

	private ToolStripButton BtnNewMethods;

	private ToolStripButton btnSaveMethod;

	private ToolStripButton BtnUpdateMethods;

	private ToolStripButton btnShortCutKeysSD;

	private ToolStripButton BtnShortCutKeyDT;

	private ToolStripButton btnShortcutkeysMethod;

	internal UltraCombo CmbDocumentTypeMethodDefine;

	private Label label20;

	private ToolStripButton BtnRefereshSD;

	private ToolStripButton BtnRefereshMethod;

	private Label label21;

	private TextBox txtAppId;

	private ToolStripButton BtnfrmUserModuleAdmin;

	private Label label22;

	internal UltraCombo CmbCompany;

	private ToolStripButton btnDefineModule;

	private CheckBox chkIsCompany;

	private CheckBox chkIsMobile;

	private CheckBox chkIsCloud;

	private TextBox textBox1;

	private Label label23;

	public frmAppMenu_ScreensDefine(UserAccount userAccount)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
	}

	private void frmScreenDefinetion_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(5, 5);
			base.AutoScrollMinSize = new Size(1060, 700);
			dtCompany.Columns.Add("Id", typeof(int));
			dtCompany.Columns.Add("CompanyName", typeof(string));
			ScreenDefinationGrd();
			ScreenRightsGrd();
			AppModuleFill();
			GrdDocTypeFill();
			DocumentTypeFill();
			CompanyNameComboFill();
			tabControl1.TabPages.Remove(tabDT);
			tabControl1.TabPages.Remove(tabMethods);
			cmbModuleName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmScreenDefinetion_FormClosed(object sender, FormClosedEventArgs e)
	{
		HistoryStack.Remove(this);
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedTab == tabSD)
			{
				cmbModuleName.Focus();
			}
			else if (tabControl1.SelectedTab == tabDT)
			{
				txtDocTypeCode.Focus();
			}
			else if (tabControl1.SelectedTab == tabMethods)
			{
				CmbDocumentTypeMethodDefine.Focus();
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
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			if (tabControl1.SelectedIndex == 0)
			{
				dt.Rows.Add("Ctrl+F5", "For Focus on Module Comb");
			}
			else if (tabControl1.SelectedIndex == 0)
			{
				dt.Rows.Add("Ctrl+F5", "For Focus on Code");
			}
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Grid");
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

	private void btnShortcutKeys_Click(object sender, EventArgs e)
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

	private void frmScreenDefinetion_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Shift && e.KeyCode == Keys.F10)
			{
				tabControl1.TabPages.Remove(tabDT);
				tabControl1.TabPages.Add(tabDT);
			}
			if (e.Control && e.Shift && e.KeyCode == Keys.F11)
			{
				tabControl1.TabPages.Remove(tabMethods);
				tabControl1.TabPages.Add(tabMethods);
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (tabControl1.SelectedTab == tabSD)
			{
				if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					cmbModuleName.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdScreenDefination).Focus();
				}
			}
			if (tabControl1.SelectedTab == tabDT)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					BtnSaveDocType_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewDocType_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U)
				{
					BtnUpdateDocType_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDocType).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtDocTypeCode.Focus();
				}
			}
			if (tabControl1.SelectedTab == tabMethods)
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					BtnSaveMethodDefine_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewMethodDefine_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U)
				{
					BtnUpdateMethodDefine_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdMethodsDefine).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					CmbDocumentTypeMethodDefine.Focus();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CompanyNameComboFill()
	{
		try
		{
			dtCompany.Clear();
			((UltraGridBase)CmbCompany).DataSource = null;
			DataTable dt = Company.GetAllCompanies();
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtCompany.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["CompName"]);
				}
			}
			if (dtCompany.Rows.Count > 0)
			{
				DDL.BindDDL(dtCompany, CmbCompany, "Id", "CompanyName", "Company Name", ZeroIndex: true);
				UltraGridColumn obj = ((UltraGridBase)CmbCompany).DisplayLayout.Bands[0].Columns.Add();
				((KeyedSubObjectBase)obj).Key = "Selected";
				((HeaderBase)obj.Header).Caption = string.Empty;
				obj.Header.CheckBoxVisibility = (HeaderCheckBoxVisibility)2;
				obj.DataType = typeof(bool);
				((HeaderBase)obj.Header).VisiblePosition = 0;
				CmbCompany.CheckedListSettings.CheckStateMember = "Selected";
				((EditorCheckedListSettings)CmbCompany.CheckedListSettings).EditorValueSource = (EditorWithComboValueSource)1;
				((EditorCheckedListSettings)CmbCompany.CheckedListSettings).ListSeparator = ",";
				((EditorCheckedListSettings)CmbCompany.CheckedListSettings).ItemCheckArea = (ItemCheckArea)1;
				((UltraGridBase)CmbCompany).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((Control)(object)CmbCompany).Text = UserAccount.CompName.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AppModuleFill()
	{
		try
		{
			int Id = Conversion.ToInt(cmbModuleName.Value);
			DataTable dt = AppMenu.GetModulesForAppMenu();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, cmbModuleName, "Id", "ModuleDescription", "Module Name", ZeroIndex: true);
				((UltraGridBase)cmbModuleName).DisplayLayout.Bands[0].Columns[1].Width = 168;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						cmbModuleName.Value = Id;
					}
					else
					{
						((Control)(object)cmbModuleName).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbModuleName).Text = "";
				((UltraGridBase)cmbModuleName).DataSource = null;
			}
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
			RecId = 0;
			txtScreenName.Text = string.Empty;
			txtScreenAlias.Text = string.Empty;
			txtTargetUrl.Text = string.Empty;
			txtSortNo.Text = string.Empty;
			cmbModuleName.Focus();
			ScreenDefinationGrd();
			ScreenRightsGrd();
			UpdateMode = false;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
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

	private void BtnRefereshSD_Click(object sender, EventArgs e)
	{
		try
		{
			AppModuleFill();
			CompanyNameComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScreenDefinationGrd()
	{
		try
		{
			dtScreenDef.Rows.Clear();
			dtScreenDef = AppMenu.FormHistory();
			if (dtScreenDef.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ScreenName");
				dt.Columns.Add("ScreenAlias");
				dt.Columns.Add("ModuleId");
				dt.Columns.Add("RouteUrl");
				dt.Columns.Add("TargetUrl");
				dt.Columns.Add("SortNo");
				dt.Columns.Add("AppId");
				dt.Columns.Add("IsCompany");
				dt.Columns.Add("IsMobile");
				dt.Columns.Add("IsCloud");
				foreach (DataRow dr in dtScreenDef.Rows)
				{
					dt.Rows.Add(dr["Id"], dr["ScreenName"], dr["ScreenAlias"], dr["ModuleId"], dr["RouteUrl"], dr["TargetUrl"], dr["SortNo"], dr["AppId"], dr["IsCompany"], dr["IsMobile"], dr["IsCloud"]);
				}
				grdScreenDefination.DataSource = dt;
				grdScreenDefination.RetrieveStructure();
				grdSettings();
			}
			else
			{
				grdScreenDefination.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSettings()
	{
		try
		{
			grdScreenDefination.RootTable.Columns["Id"].Visible = false;
			grdScreenDefination.RootTable.Columns["AppId"].Visible = false;
			grdScreenDefination.RootTable.Columns["ModuleId"].Visible = false;
			grdScreenDefination.RootTable.Columns["TargetUrl"].Visible = false;
			grdScreenDefination.RootTable.Columns["RouteUrl"].Visible = false;
			grdScreenDefination.RootTable.Columns["ScreenName"].Width = 200;
			grdScreenDefination.RootTable.Columns["ScreenAlias"].Width = 250;
			grdScreenDefination.RootTable.Columns["ScreenName"].ColumnType = (ColumnType)5;
			grdScreenDefination.RootTable.Columns["ScreenName"].EditType = (EditType)0;
			grdScreenDefination.RootTable.Columns["TargetUrl"].EditType = (EditType)0;
			grdScreenDefination.RootTable.Columns["RouteUrl"].EditType = (EditType)0;
			grdScreenDefination.RootTable.Columns["AppId"].EditType = (EditType)0;
			grdScreenDefination.RootTable.Columns["IsCompany"].EditType = (EditType)0;
			grdScreenDefination.RootTable.Columns["IsMobile"].EditType = (EditType)0;
			grdScreenDefination.RootTable.Columns["IsCloud"].EditType = (EditType)0;
			grdScreenDefination.RootTable.Columns["ScreenAlias"].EditType = (EditType)1;
			grdScreenDefination.RootTable.Columns["SortNo"].EditType = (EditType)1;
			grdScreenDefination.RootTable.Columns["ScreenName"].FilterEditType = (FilterEditType)1;
			grdScreenDefination.RootTable.Columns["ScreenAlias"].FilterEditType = (FilterEditType)1;
			grdScreenDefination.RootTable.Columns["SortNo"].FilterEditType = (FilterEditType)1;
			grdScreenDefination.RootTable.Columns["RouteUrl"].FilterEditType = (FilterEditType)1;
			grdScreenDefination.RootTable.Columns["AppId"].FilterEditType = (FilterEditType)1;
			grdScreenDefination.RootTable.Columns["IsCompany"].FilterEditType = (FilterEditType)7;
			grdScreenDefination.RootTable.Columns["IsMobile"].FilterEditType = (FilterEditType)7;
			grdScreenDefination.RootTable.Columns["IsCloud"].FilterEditType = (FilterEditType)7;
			grdScreenDefination.ColumnAutoResize = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScreenDefination_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdScreenDefination.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				RetrivedData(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbModuleName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbModuleName.Value) > 0)
			{
				if (dtScreenDef.Rows.Count <= 0)
				{
					return;
				}
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ScreenName");
				dt.Columns.Add("ScreenAlias");
				dt.Columns.Add("ModuleId");
				dt.Columns.Add("RouteUrl");
				dt.Columns.Add("TargetUrl");
				dt.Columns.Add("SortNo");
				dt.Columns.Add("AppId");
				dt.Columns.Add("IsCompany");
				dt.Columns.Add("IsMobile");
				dt.Columns.Add("IsCloud");
				foreach (DataRow dr in dtScreenDef.Rows)
				{
					if (Conversion.ToInt(dr["ModuleId"]) == Conversion.ToInt(cmbModuleName.Value))
					{
						dt.Rows.Add(dr["Id"], dr["ScreenName"], dr["ScreenAlias"], dr["ModuleId"], dr["RouteUrl"], dr["TargetUrl"], dr["SortNo"], dr["AppId"], dr["IsCompany"], dr["IsMobile"], dr["IsCloud"]);
					}
				}
				if (dt.Rows.Count > 0)
				{
					grdScreenDefination.DataSource = dt;
					grdScreenDefination.RetrieveStructure();
					grdSettings();
				}
				else
				{
					grdScreenDefination.DataSource = null;
				}
			}
			else
			{
				if (dtScreenDef.Rows.Count <= 0)
				{
					return;
				}
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id");
				dt2.Columns.Add("ScreenName");
				dt2.Columns.Add("ScreenAlias");
				dt2.Columns.Add("ModuleId");
				dt2.Columns.Add("RouteUrl");
				dt2.Columns.Add("TargetUrl");
				dt2.Columns.Add("SortNo");
				dt2.Columns.Add("AppId");
				dt2.Columns.Add("IsCompany");
				dt2.Columns.Add("IsMobile");
				dt2.Columns.Add("IsCloud");
				foreach (DataRow dr2 in dtScreenDef.Rows)
				{
					dt2.Rows.Add(dr2["Id"], dr2["ScreenName"], dr2["ScreenAlias"], dr2["ModuleId"], dr2["RouteUrl"], dr2["TargetUrl"], dr2["SortNo"], dr2["AppId"], dr2["IsCompany"], dr2["IsMobile"], dr2["IsCloud"]);
				}
				grdScreenDefination.DataSource = dt2;
				grdScreenDefination.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScreenDefination_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdScreenDefination.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "ScreenName")
			{
				string TargetUrl = Conversion.ToString(r.Cells["TargetUrl"].Value);
				string ScreenAlias = Conversion.ToString(r.Cells["ScreenAlias"].Value);
				string ScreenName = Conversion.ToString(r.Cells["ScreenName"].Value);
				if (TargetUrl == null)
				{
					throw new Exception("Target URL not Configured properly... " + ScreenAlias);
				}
				if (!(Type.GetType(TargetUrl) != null))
				{
					throw new Exception("Target URL not Configured properly... " + ScreenAlias);
				}
				if (Activator.CreateInstance(Type.GetType(TargetUrl), UserAccount) is Form form)
				{
					form.Tag = ScreenName;
					form.ShowDialog();
					form.BringToFront();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ScreenRightsGrd()
	{
		try
		{
			DataTable dtScreenRig = new DataTable();
			dtScreenRig = ScreenRights.GetAll();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("RightsName");
			dt.Columns.Add("Value", typeof(bool));
			if (dtScreenRig.Rows.Count > 0)
			{
				for (int i = 0; i < dtScreenRig.Rows.Count; i++)
				{
					dt.Rows.Add(dtScreenRig.Rows[i]["Id"], dtScreenRig.Rows[i]["RightsName"], false);
				}
			}
			grdScreenRights.DataSource = dt;
			grdScreenRights.RetrieveStructure();
			grdScreenRights.AllowEdit = (InheritableBoolean)1;
			grdScreenRights.RootTable.Columns["Id"].Visible = false;
			grdScreenRights.RootTable.Columns["RightsName"].Width = 190;
			grdScreenRights.RootTable.Columns["Id"].EditType = (EditType)0;
			grdScreenRights.RootTable.Columns["RightsName"].EditType = (EditType)0;
			grdScreenRights.RootTable.Columns["Value"].EditType = (EditType)8;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdScreenRights_Click(object sender, EventArgs e)
	{
		try
		{
			grdScreenRights.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public bool Validation()
	{
		if (((Control)(object)cmbModuleName).Text == string.Empty || Conversion.ToInt(cmbModuleName.Value) == 0)
		{
			MessageBox.Show("Module Required!");
			cmbModuleName.Focus();
			return false;
		}
		if (txtScreenName.Text == string.Empty)
		{
			MessageBox.Show("Screen Name Required!");
			txtScreenName.Focus();
			return false;
		}
		if (txtScreenAlias.Text == string.Empty)
		{
			MessageBox.Show("Screen Alias Required!");
			txtScreenAlias.Focus();
			return false;
		}
		return true;
	}

	public void RetrivedData(int Id)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			AppMenu obj = new AppMenu();
			RecId = Id;
			obj = AppMenu.GetById(RecId);
			if (obj == null)
			{
				return;
			}
			cmbModuleName.Value = Conversion.ToString((object)obj.ModuleId);
			txtScreenName.Text = Conversion.ToString((object)obj.ScreenName);
			txtScreenAlias.Text = Conversion.ToString((object)obj.ScreenAlias);
			textBox1.Text = Conversion.ToString((object)obj.RouteUrl);
			txtTargetUrl.Text = Conversion.ToString((object)obj.TargetUrl);
			txtSortNo.Text = Conversion.ToString((object)obj.SortNo);
			txtAppId.Text = Conversion.ToString((object)obj.AppId);
			ChkIsActive.Checked = Conversion.ToBool((object)obj.IsActive);
			chkIsCompany.Checked = obj.IsCompany;
			chkIsMobile.Checked = obj.IsMobile;
			chkIsCloud.Checked = obj.IsCloud;
			GridEXRow[] rows = grdScreenRights.GetRows();
			foreach (GridEXRow obj2 in rows)
			{
				obj2.BeginEdit();
				obj2.Cells["Value"].Value = false;
				obj2.EndEdit();
			}
			List<ScreenRights> lst = ScreenRights.ReadByScreenId(RecId);
			if (lst.Count > 0)
			{
				for (int i = 0; i < lst.Count; i++)
				{
					rows = grdScreenRights.GetRows();
					foreach (GridEXRow item in rows)
					{
						if (Conversion.ToString((object)lst[i].RightName) == Conversion.ToString(item.Cells["RightsName"].Value))
						{
							item.BeginEdit();
							item.Cells["Value"].Value = true;
							item.EndEdit();
							break;
						}
					}
				}
			}
			UpdateMode = true;
			btnSave.Visible = false;
			btnUpdate.Visible = true;
			grdScreenRights.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Expected O, but got Unknown
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Expected O, but got Unknown
		try
		{
			if (!Validation())
			{
				return;
			}
			if (Type.GetType(txtTargetUrl.Text) == null && Conversion.ToInt((object)txtAppId.Text) != 3)
			{
				MessageBox.Show("Target URL not Configured properly... ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			AppMenu obj = new AppMenu();
			if (RecId > 0)
			{
				obj.Id = RecId;
			}
			else
			{
				obj.Id = 0;
			}
			bool haveView = false;
			GridEXRow[] rows = grdScreenRights.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				if (Conversion.ToString(rows[i].Cells["RightsName"].Value) == "View")
				{
					haveView = true;
					break;
				}
			}
			if (!haveView)
			{
				throw new Exception("View Right For Screen Required....");
			}
			obj.ModuleId = Conversion.ToInt(cmbModuleName.Value);
			obj.ScreenName = txtScreenName.Text.Trim();
			obj.ScreenAlias = txtScreenAlias.Text.Trim();
			obj.TargetUrl = txtTargetUrl.Text.Trim();
			obj.RouteUrl = textBox1.Text.Trim();
			obj.AppId = Conversion.ToInt((object)txtAppId.Text);
			obj.SortNo = Conversion.ToInt((object)txtSortNo.Text);
			obj.IsActive = Conversion.ToBool((object)ChkIsActive.Checked);
			obj.IsCompany = Conversion.ToBool((object)chkIsCompany.Checked);
			obj.IsMobile = Conversion.ToBool((object)chkIsMobile.Checked);
			obj.IsCloud = Conversion.ToBool((object)chkIsCloud.Checked);
			if (!obj.IsCompany && !obj.IsMobile && !obj.IsCloud)
			{
				throw new Exception("Please Select atleast one check box of (IsCompany,IsMobile Or IsCloud)");
			}
			int count = 0;
			obj.DetailScreenRights = new List<ScreenRights>();
			rows = grdScreenRights.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToBool(r.Cells["Value"].Value))
				{
					ScreenRights detail = new ScreenRights();
					detail.RightName = r.Cells["RightsName"].Value.ToString();
					obj.DetailScreenRights.Add(detail);
					count = r.RowIndex + 1;
				}
			}
			if (count == 0)
			{
				MessageBox.Show("Please Check the Screen Rights");
				return;
			}
			AppMenu.save(obj);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
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

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			AppMenu obj = new AppMenu();
			obj.AppMenuList = new List<AppMenu>();
			GridEXRow[] rows = grdScreenDefination.GetRows();
			foreach (GridEXRow item in rows)
			{
				AppMenu sd = new AppMenu();
				sd.ScreenAlias = Conversion.ToString(item.Cells["ScreenAlias"].Value);
				sd.SortNo = Conversion.ToInt(item.Cells["SortNo"].Value);
				sd.Id = Conversion.ToInt(item.Cells["Id"].Value);
				obj.AppMenuList.Add(sd);
			}
			AppMenu.ScreenSortingAndAliasUpdate(obj);
			MessageBox.Show("Record Update Successfully");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtSortNo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ResetDocType()
	{
		try
		{
			txtDocTypeId.Enabled = true;
			RecIdDocType = 0;
			txtDocTypeId.Text = string.Empty;
			txtDocTypeCode.Text = string.Empty;
			txtDocTypeDescription.Text = string.Empty;
			txtDocTypeScreenName.Text = string.Empty;
			BtnSaveDocType.Visible = true;
			BtnUpdateDocType.Visible = false;
			GrdDocTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewDocType_Click(object sender, EventArgs e)
	{
		try
		{
			ResetDocType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public bool ValidationDocType()
	{
		if (txtDocTypeId.Text == string.Empty || Conversion.ToInt((object)txtDocTypeId.Text) == 0)
		{
			MessageBox.Show("DocTypeId Required!");
			txtDocTypeId.Focus();
			return false;
		}
		if (txtDocTypeCode.Text == string.Empty)
		{
			MessageBox.Show("DocType Code Required!");
			txtDocTypeCode.Focus();
			return false;
		}
		if (txtDocTypeCode.Text == string.Empty)
		{
			MessageBox.Show("DocType Code Required!");
			txtDocTypeCode.Focus();
			return false;
		}
		if (txtDocTypeScreenName.Text == string.Empty)
		{
			MessageBox.Show("Screen Name Required!");
			txtDocTypeScreenName.Focus();
			return false;
		}
		if (txtDocTypeTargetUrl.Text == string.Empty)
		{
			MessageBox.Show("Target Url Required!");
			txtDocTypeTargetUrl.Focus();
			return false;
		}
		return true;
	}

	private void InsertDocType()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!ValidationDocType())
			{
				return;
			}
			DocumentType obj = new DocumentType();
			if (RecIdDocType > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecIdDocType;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.Id = Conversion.ToInt((object)txtDocTypeId.Text);
			obj.DocumentTypeCode = Conversion.ToString((object)txtDocTypeCode.Text);
			obj.DocumentTypeDescription = Conversion.ToString((object)txtDocTypeDescription.Text);
			obj.DocumentTypeDescriptionOtherLing = Conversion.ToString((object)txtDocTypeOtherLangDescription.Text);
			obj.ScreenName = Conversion.ToString((object)txtDocTypeScreenName.Text);
			obj.TargetUrl = Conversion.ToString((object)txtDocTypeTargetUrl.Text);
			obj.Remarks = Conversion.ToString((object)txtDocTypeRemarks.Text);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.ModifyDate = DateTime.Now;
			obj.EntryDate = DateTime.Now;
			obj.PostDate = DateTime.Now;
			DocumentType.Save(obj);
			if (RecIdDocType > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			ResetDocType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveDocType_Click(object sender, EventArgs e)
	{
		try
		{
			InsertDocType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateDocType_Click(object sender, EventArgs e)
	{
		try
		{
			InsertDocType();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByIdDocType(int Id)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			RecIdDocType = Id;
			GridEXRow item = grdDocType.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				txtDocTypeId.Text = Conversion.ToInt(item.Cells["Id"].Value).ToString();
				txtDocTypeCode.Text = Conversion.ToString(item.Cells["Code"].Value);
				txtDocTypeDescription.Text = Conversion.ToString(item.Cells["Description"].Value);
				txtDocTypeOtherLangDescription.Text = Conversion.ToString(item.Cells["DescriptionOtherLing"].Value);
				txtDocTypeScreenName.Text = Conversion.ToString(item.Cells["ScreenName"].Value);
				txtDocTypeTargetUrl.Text = Conversion.ToString(item.Cells["TargetUrl"].Value);
				txtDocTypeRemarks.Text = Conversion.ToString(item.Cells["Remarks"].Value);
				BtnSaveDocType.Visible = false;
				BtnUpdateDocType.Visible = true;
				txtDocTypeId.Enabled = false;
				txtDocTypeCode.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDocTypeFill()
	{
		try
		{
			DataTable dtDocType = DocumentType.FormHistory();
			if (dtDocType.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("Code", typeof(string));
				dt.Columns.Add("Description", typeof(string));
				dt.Columns.Add("DescriptionOtherLing", typeof(string));
				dt.Columns.Add("ScreenName", typeof(string));
				dt.Columns.Add("TargetUrl", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUserName", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUserName", typeof(string));
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUserName", typeof(string));
				foreach (DataRow row in dtDocType.Rows)
				{
					dt.Rows.Add(row["Id"], row["DocumentTypeCode"], row["DocumentTypeDescription"], row["DocumentTypeDescriptionOtherLing"], row["ScreenName"], row["TargetUrl"], row["Remarks"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["ApprovedDate"], row["ApprovedUserName"]);
				}
				grdDocType.DataSource = dt;
				grdDocType.RetrieveStructure();
				grdDoctypeSetting();
			}
			else
			{
				grdDocType.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDoctypeSetting()
	{
		try
		{
			CommonServices.GridAutoAdjustmentNew(grdDocType);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDocType_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdDocType.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				ReadByIdDocType(Conversion.ToInt(item.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumentTypeFill()
	{
		try
		{
			int Id = Conversion.ToInt(CmbDocumentTypeMethodDefine.Value);
			DataTable dt = DocumentType.FormHistory();
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDLNew(dt, CmbDocumentTypeMethodDefine, "Id", "DocumentTypeDescription", "Document Type", true);
				((UltraGridBase)CmbDocumentTypeMethodDefine).DisplayLayout.Bands[0].Columns[1].Width = 168;
				((UltraGridBase)CmbDocumentTypeMethodDefine).DisplayLayout.Bands[0].Columns[0].Hidden = false;
				if (Id > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbDocumentTypeMethodDefine.Value = Id;
					}
					else
					{
						((Control)(object)CmbDocumentTypeMethodDefine).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbDocumentTypeMethodDefine).Text = "";
				((UltraGridBase)CmbDocumentTypeMethodDefine).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetMethodDefine()
	{
		try
		{
			RecIdMethodDefine = 0;
			((Control)(object)CmbDocumentTypeMethodDefine).Text = string.Empty;
			txtReportMethod.Text = string.Empty;
			txtScreenNameMethodDefine.Text = string.Empty;
			txtRemarksMethodDefine.Text = string.Empty;
			btnSaveMethod.Visible = true;
			BtnUpdateMethods.Visible = false;
			GrdMethodDefineFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewMethodDefine_Click(object sender, EventArgs e)
	{
		try
		{
			ResetMethodDefine();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnRefereshMethod_Click(object sender, EventArgs e)
	{
		try
		{
			DocumentTypeFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public bool ValidationMethodDefine()
	{
		if (((Control)(object)CmbDocumentTypeMethodDefine).Text == string.Empty || Conversion.ToInt(CmbDocumentTypeMethodDefine.Value) == 0)
		{
			MessageBox.Show("Document Type Required!");
			CmbDocumentTypeMethodDefine.Focus();
			return false;
		}
		if (txtScreenNameMethodDefine.Text == string.Empty)
		{
			MessageBox.Show("Screen Name Required!");
			txtScreenNameMethodDefine.Focus();
			return false;
		}
		if (txtReportMethod.Text == string.Empty)
		{
			MessageBox.Show("Report Method Required!");
			txtReportMethod.Focus();
			return false;
		}
		return true;
	}

	private void InsertMethodDefine()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!ValidationMethodDefine())
			{
				return;
			}
			ReportsMethod obj = new ReportsMethod();
			if (RecIdMethodDefine > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				obj.Id = RecIdMethodDefine;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			obj.RefDocumentTypeId = Conversion.ToInt(CmbDocumentTypeMethodDefine.Value);
			obj.ScreenName = Conversion.ToString((object)txtScreenNameMethodDefine.Text);
			obj.ReportMethod = Conversion.ToString((object)txtReportMethod.Text);
			obj.Remarks = Conversion.ToString((object)txtRemarksMethodDefine.Text);
			obj.ReportTypeId = 1;
			ReportsMethod.Save(obj);
			if (RecIdMethodDefine > 0)
			{
				MessageBox.Show("Record Update Successfully");
			}
			else
			{
				MessageBox.Show("Record Save Successfully");
			}
			ResetMethodDefine();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnSaveMethodDefine_Click(object sender, EventArgs e)
	{
		try
		{
			InsertMethodDefine();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnUpdateMethodDefine_Click(object sender, EventArgs e)
	{
		try
		{
			InsertMethodDefine();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadByIdMethodDefine(int Id)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			RecIdMethodDefine = Id;
			GridEXRow item = grdMethodsDefine.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				CmbDocumentTypeMethodDefine.Value = Conversion.ToInt(item.Cells["RefDocumentTypeId"].Value);
				txtScreenNameMethodDefine.Text = Conversion.ToString(item.Cells["ScreenName"].Value);
				txtReportMethod.Text = Conversion.ToString(item.Cells["ReportMethod"].Value);
				txtRemarksMethodDefine.Text = Conversion.ToString(item.Cells["Remarks"].Value);
				btnSaveMethod.Visible = false;
				BtnUpdateMethods.Visible = true;
				CmbDocumentTypeMethodDefine.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdMethodDefineFill()
	{
		try
		{
			DataTable dtMethodDefine = ReportsMethod.FormHistory();
			if (dtMethodDefine.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("RefDocumentTypeId", typeof(int));
				dt.Columns.Add("DocumentTypeCode", typeof(string));
				dt.Columns.Add("DocumentTypeDescription", typeof(string));
				dt.Columns.Add("ScreenName", typeof(string));
				dt.Columns.Add("ReportTypeId", typeof(int));
				dt.Columns.Add("ReportMethod", typeof(string));
				dt.Columns.Add("Remarks", typeof(string));
				foreach (DataRow row in dtMethodDefine.Rows)
				{
					dt.Rows.Add(row["Id"], row["RefDocumentTypeId"], row["DocumentTypeCode"], row["DocumentTypeDescription"], row["ScreenName"], row["ReportTypeId"], row["ReportMethod"], row["Remarks"]);
				}
				grdMethodsDefine.DataSource = dt;
				grdMethodsDefine.RetrieveStructure();
				grdMethodDefineSetting();
			}
			else
			{
				grdMethodsDefine.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMethodDefineSetting()
	{
		try
		{
			grdMethodsDefine.RootTable.Columns["Id"].Visible = false;
			grdMethodsDefine.RootTable.Columns["ReportTypeId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdMethodsDefine);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdMethodDefine_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdMethodsDefine.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				ReadByIdMethodDefine(Conversion.ToInt(item.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnfrmUserModuleAdmin_Click(object sender, EventArgs e)
	{
		try
		{
			new frmUserModuleAdmin(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDefineModule_Click(object sender, EventArgs e)
	{
		try
		{
			new frmDefineAppModules(UserAccount).Show();
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
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Expected O, but got Unknown
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Expected O, but got Unknown
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Expected O, but got Unknown
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Expected O, but got Unknown
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Expected O, but got Unknown
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Expected O, but got Unknown
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Expected O, but got Unknown
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Expected O, but got Unknown
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Expected O, but got Unknown
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Expected O, but got Unknown
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Expected O, but got Unknown
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Expected O, but got Unknown
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Expected O, but got Unknown
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Expected O, but got Unknown
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Expected O, but got Unknown
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Expected O, but got Unknown
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Expected O, but got Unknown
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_02b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Expected O, but got Unknown
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Expected O, but got Unknown
		//IL_0422: Unknown result type (might be due to invalid IL or missing references)
		//IL_042c: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Expected O, but got Unknown
		//IL_20b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_20c2: Expected O, but got Unknown
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		GridEXLayout grdScreenDefination_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdScreenRights_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDocType_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdMethodsDefine_DesignTimeLayout = new GridEXLayout();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.BtnRefereshSD = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnShortCutKeysSD = new System.Windows.Forms.ToolStripButton();
		this.BtnfrmUserModuleAdmin = new System.Windows.Forms.ToolStripButton();
		this.btnDefineModule = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.textBox1 = new System.Windows.Forms.TextBox();
		this.label23 = new System.Windows.Forms.Label();
		this.chkIsCompany = new System.Windows.Forms.CheckBox();
		this.chkIsMobile = new System.Windows.Forms.CheckBox();
		this.chkIsCloud = new System.Windows.Forms.CheckBox();
		this.label22 = new System.Windows.Forms.Label();
		this.CmbCompany = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.txtAppId = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.cmbModuleName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.txtSortNo = new System.Windows.Forms.TextBox();
		this.txtTargetUrl = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.ChkIsActive = new System.Windows.Forms.CheckBox();
		this.txtScreenAlias = new System.Windows.Forms.TextBox();
		this.txtScreenName = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdScreenDefination = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdScreenRights = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabSD = new System.Windows.Forms.TabPage();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabDT = new System.Windows.Forms.TabPage();
		this.grdDocType = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.label17 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.txtDocTypeScreenName = new System.Windows.Forms.TextBox();
		this.txtDocTypeOtherLangDescription = new System.Windows.Forms.TextBox();
		this.txtDocTypeDescription = new System.Windows.Forms.TextBox();
		this.txtDocTypeRemarks = new System.Windows.Forms.TextBox();
		this.txtDocTypeTargetUrl = new System.Windows.Forms.TextBox();
		this.label13 = new System.Windows.Forms.Label();
		this.txtDocTypeCode = new System.Windows.Forms.TextBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtDocTypeId = new System.Windows.Forms.TextBox();
		this.label14 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewDocType = new System.Windows.Forms.ToolStripButton();
		this.BtnSaveDocType = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdateDocType = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutKeyDT = new System.Windows.Forms.ToolStripButton();
		this.tabMethods = new System.Windows.Forms.TabPage();
		this.grdMethodsDefine = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label18 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.CmbDocumentTypeMethodDefine = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.txtScreenNameMethodDefine = new System.Windows.Forms.TextBox();
		this.txtRemarksMethodDefine = new System.Windows.Forms.TextBox();
		this.txtReportMethod = new System.Windows.Forms.TextBox();
		this.label24 = new System.Windows.Forms.Label();
		this.label20 = new System.Windows.Forms.Label();
		this.label25 = new System.Windows.Forms.Label();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label26 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.BtnNewMethods = new System.Windows.Forms.ToolStripButton();
		this.BtnRefereshMethod = new System.Windows.Forms.ToolStripButton();
		this.btnSaveMethod = new System.Windows.Forms.ToolStripButton();
		this.BtnUpdateMethods = new System.Windows.Forms.ToolStripButton();
		this.btnShortcutkeysMethod = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbModuleName).BeginInit();
		this.panel3.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScreenDefination).BeginInit();
		this.panel7.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdScreenRights).BeginInit();
		this.panel8.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabSD.SuspendLayout();
		this.panel1.SuspendLayout();
		this.tabDT.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDocType).BeginInit();
		this.panel11.SuspendLayout();
		this.panel10.SuspendLayout();
		this.panel9.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		this.tabMethods.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdMethodsDefine).BeginInit();
		this.panel12.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypeMethodDefine).BeginInit();
		this.panel14.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.BtnRefereshSD, this.btnSave, this.btnUpdate, this.toolStripButton1, this.btnShortCutKeysSD, this.BtnfrmUserModuleAdmin, this.btnDefineModule });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1025, 27);
		this.toolStrip1.TabIndex = 8;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.BtnRefereshSD.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnRefereshSD.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefereshSD.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefereshSD.Name = "BtnRefereshSD";
		this.BtnRefereshSD.Size = new System.Drawing.Size(80, 24);
		this.BtnRefereshSD.Text = "&Refresh";
		this.BtnRefereshSD.Click += new System.EventHandler(BtnRefereshSD_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(204, 24);
		this.toolStripButton1.Text = "Update_Sorting & Screen Alias";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnShortCutKeysSD.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortCutKeysSD.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortCutKeysSD.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortCutKeysSD.Name = "btnShortCutKeysSD";
		this.btnShortCutKeysSD.Size = new System.Drawing.Size(126, 24);
		this.btnShortCutKeysSD.Text = "ShortCut Keys";
		this.btnShortCutKeysSD.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.BtnfrmUserModuleAdmin.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnfrmUserModuleAdmin.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.BtnfrmUserModuleAdmin.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnfrmUserModuleAdmin.Name = "BtnfrmUserModuleAdmin";
		this.BtnfrmUserModuleAdmin.Size = new System.Drawing.Size(155, 24);
		this.BtnfrmUserModuleAdmin.Text = "Module Admin User";
		this.BtnfrmUserModuleAdmin.Click += new System.EventHandler(BtnfrmUserModuleAdmin_Click);
		this.btnDefineModule.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDefineModule.Image = Architecture.WinApp.Properties.Resources.Export_Reports;
		this.btnDefineModule.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDefineModule.Name = "btnDefineModule";
		this.btnDefineModule.Size = new System.Drawing.Size(123, 24);
		this.btnDefineModule.Text = "Define Module";
		this.btnDefineModule.Click += new System.EventHandler(btnDefineModule_Click);
		this.panel2.BackColor = System.Drawing.Color.White;
		this.panel2.Controls.Add(this.textBox1);
		this.panel2.Controls.Add(this.label23);
		this.panel2.Controls.Add(this.chkIsCompany);
		this.panel2.Controls.Add(this.chkIsMobile);
		this.panel2.Controls.Add(this.chkIsCloud);
		this.panel2.Controls.Add(this.label22);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCompany);
		this.panel2.Controls.Add(this.label21);
		this.panel2.Controls.Add(this.txtAppId);
		this.panel2.Controls.Add(this.label6);
		this.panel2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbModuleName);
		this.panel2.Controls.Add(this.label1);
		this.panel2.Controls.Add(this.txtSortNo);
		this.panel2.Controls.Add(this.txtTargetUrl);
		this.panel2.Controls.Add(this.label3);
		this.panel2.Controls.Add(this.label2);
		this.panel2.Controls.Add(this.ChkIsActive);
		this.panel2.Controls.Add(this.txtScreenAlias);
		this.panel2.Controls.Add(this.txtScreenName);
		this.panel2.Controls.Add(this.label5);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 61);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1025, 89);
		this.panel2.TabIndex = 0;
		this.textBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.textBox1.Location = new System.Drawing.Point(432, 34);
		this.textBox1.Name = "textBox1";
		this.textBox1.Size = new System.Drawing.Size(287, 23);
		this.textBox1.TabIndex = 4;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.Location = new System.Drawing.Point(360, 38);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(56, 15);
		this.label23.TabIndex = 34;
		this.label23.Text = "Route Url";
		this.chkIsCompany.AutoSize = true;
		this.chkIsCompany.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsCompany.Location = new System.Drawing.Point(853, 7);
		this.chkIsCompany.Name = "chkIsCompany";
		this.chkIsCompany.Size = new System.Drawing.Size(89, 19);
		this.chkIsCompany.TabIndex = 8;
		this.chkIsCompany.Text = "Is Company";
		this.chkIsCompany.UseVisualStyleBackColor = true;
		this.chkIsMobile.AutoSize = true;
		this.chkIsMobile.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsMobile.Location = new System.Drawing.Point(853, 26);
		this.chkIsMobile.Name = "chkIsMobile";
		this.chkIsMobile.Size = new System.Drawing.Size(75, 19);
		this.chkIsMobile.TabIndex = 9;
		this.chkIsMobile.Text = "Is Mobile";
		this.chkIsMobile.UseVisualStyleBackColor = true;
		this.chkIsCloud.AutoSize = true;
		this.chkIsCloud.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkIsCloud.Location = new System.Drawing.Point(853, 44);
		this.chkIsCloud.Name = "chkIsCloud";
		this.chkIsCloud.Size = new System.Drawing.Size(69, 19);
		this.chkIsCloud.TabIndex = 10;
		this.chkIsCloud.Text = "Is Cloud";
		this.chkIsCloud.UseVisualStyleBackColor = true;
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label22.Location = new System.Drawing.Point(360, 11);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(66, 15);
		this.label22.TabIndex = 29;
		this.label22.Text = "Companies";
		this.CmbCompany.AllowNull = (DefaultableBoolean)1;
		this.CmbCompany.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCompany.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCompany.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCompany).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCompany).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCompany).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCompany).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		((System.Windows.Forms.Control)(object)this.CmbCompany).Location = new System.Drawing.Point(432, 5);
		((System.Windows.Forms.Control)(object)this.CmbCompany).Name = "CmbCompany";
		((System.Windows.Forms.Control)(object)this.CmbCompany).Size = new System.Drawing.Size(287, 26);
		((System.Windows.Forms.Control)(object)this.CmbCompany).TabIndex = 3;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label21.Location = new System.Drawing.Point(724, 38);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(40, 15);
		this.label21.TabIndex = 27;
		this.label21.Text = "AppId";
		this.txtAppId.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAppId.Location = new System.Drawing.Point(775, 34);
		this.txtAppId.Name = "txtAppId";
		this.txtAppId.Size = new System.Drawing.Size(73, 23);
		this.txtAppId.TabIndex = 7;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(724, 11);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(48, 15);
		this.label6.TabIndex = 25;
		this.label6.Text = "Sort No";
		this.cmbModuleName.AllowNull = (DefaultableBoolean)1;
		this.cmbModuleName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbModuleName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbModuleName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbModuleName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbModuleName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance2).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance2;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance3).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance4).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance4).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance4).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance4).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance18).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbModuleName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbModuleName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbModuleName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbModuleName).Location = new System.Drawing.Point(89, 5);
		((System.Windows.Forms.Control)(object)this.cmbModuleName).Name = "cmbModuleName";
		((System.Windows.Forms.Control)(object)this.cmbModuleName).Size = new System.Drawing.Size(268, 26);
		((System.Windows.Forms.Control)(object)this.cmbModuleName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbModuleName).Leave += new System.EventHandler(cmbModuleName_Leave);
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(5, 38);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(78, 15);
		this.label1.TabIndex = 22;
		this.label1.Text = "Screen Name";
		this.txtSortNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSortNo.Location = new System.Drawing.Point(775, 7);
		this.txtSortNo.Name = "txtSortNo";
		this.txtSortNo.Size = new System.Drawing.Size(73, 23);
		this.txtSortNo.TabIndex = 6;
		this.txtSortNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtSortNo_KeyPress);
		this.txtTargetUrl.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTargetUrl.Location = new System.Drawing.Point(432, 61);
		this.txtTargetUrl.Name = "txtTargetUrl";
		this.txtTargetUrl.Size = new System.Drawing.Size(416, 23);
		this.txtTargetUrl.TabIndex = 5;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(5, 11);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(83, 15);
		this.label3.TabIndex = 12;
		this.label3.Text = "Module Name";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(5, 65);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(71, 15);
		this.label2.TabIndex = 23;
		this.label2.Text = "Screen Alias";
		this.ChkIsActive.AutoSize = true;
		this.ChkIsActive.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkIsActive.Location = new System.Drawing.Point(853, 64);
		this.ChkIsActive.Name = "ChkIsActive";
		this.ChkIsActive.Size = new System.Drawing.Size(71, 19);
		this.ChkIsActive.TabIndex = 11;
		this.ChkIsActive.Text = "Is Active";
		this.ChkIsActive.UseVisualStyleBackColor = true;
		this.txtScreenAlias.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScreenAlias.Location = new System.Drawing.Point(89, 61);
		this.txtScreenAlias.Name = "txtScreenAlias";
		this.txtScreenAlias.Size = new System.Drawing.Size(268, 23);
		this.txtScreenAlias.TabIndex = 2;
		this.txtScreenName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScreenName.Location = new System.Drawing.Point(89, 34);
		this.txtScreenName.Name = "txtScreenName";
		this.txtScreenName.Size = new System.Drawing.Size(268, 23);
		this.txtScreenName.TabIndex = 1;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(360, 65);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(58, 15);
		this.label5.TabIndex = 23;
		this.label5.Text = "Target Url";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.label8);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1025, 58);
		this.panel3.TabIndex = 1;
		this.label8.AutoSize = true;
		this.label8.BackColor = System.Drawing.Color.Teal;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 31);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(157, 19);
		this.label8.TabIndex = 25;
		this.label8.Text = "Screens Definition";
		this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdScreenDefination);
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(683, 494);
		this.panel5.TabIndex = 0;
		this.grdScreenDefination.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdScreenDefination_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdScreenDefination.DesignTimeLayout = grdScreenDefination_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScreenDefination).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdScreenDefination.FilterMode = (FilterMode)2;
		this.grdScreenDefination.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdScreenDefination).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScreenDefination.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdScreenDefination).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdScreenDefination).Name = "grdScreenDefination";
		((ScrollableBase)this.grdScreenDefination).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScreenDefination).Size = new System.Drawing.Size(683, 468);
		((System.Windows.Forms.Control)(object)this.grdScreenDefination).TabIndex = 0;
		this.grdScreenDefination.TotalRow = (InheritableBoolean)1;
		this.grdScreenDefination.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScreenDefination).VisualStyle = (VisualStyle)4;
		this.grdScreenDefination.LinkClicked += new ColumnActionEventHandler(grdScreenDefination_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdScreenDefination).DoubleClick += new System.EventHandler(grdScreenDefination_DoubleClick);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label7);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(683, 26);
		this.panel7.TabIndex = 2;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(5, 1);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(84, 23);
		this.label7.TabIndex = 25;
		this.label7.Text = "Screens";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Controls.Add(this.panel5);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1025, 494);
		this.panel4.TabIndex = 2;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdScreenRights);
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel6.Location = new System.Drawing.Point(683, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(341, 494);
		this.panel6.TabIndex = 24;
		this.grdScreenRights.AllowEdit = (InheritableBoolean)2;
		this.grdScreenRights.DefaultFilterRowComparison = (FilterConditionOperator)7;
		grdScreenRights_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdScreenRights.DesignTimeLayout = grdScreenRights_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdScreenRights.FilterMode = (FilterMode)2;
		this.grdScreenRights.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdScreenRights.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Location = new System.Drawing.Point(0, 26);
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Name = "grdScreenRights";
		((ScrollableBase)this.grdScreenRights).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Size = new System.Drawing.Size(341, 468);
		((System.Windows.Forms.Control)(object)this.grdScreenRights).TabIndex = 0;
		this.grdScreenRights.TotalRow = (InheritableBoolean)1;
		this.grdScreenRights.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdScreenRights).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdScreenRights).Click += new System.EventHandler(grdScreenRights_Click);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label4);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(341, 26);
		this.panel8.TabIndex = 3;
		this.label4.AutoSize = true;
		this.label4.BackColor = System.Drawing.Color.Teal;
		this.label4.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.White;
		this.label4.Location = new System.Drawing.Point(85, 1);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(142, 23);
		this.label4.TabIndex = 25;
		this.label4.Text = "Screen Rights";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.tabControl1.Controls.Add(this.tabSD);
		this.tabControl1.Controls.Add(this.tabDT);
		this.tabControl1.Controls.Add(this.tabMethods);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1039, 676);
		this.tabControl1.TabIndex = 4;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabSD.Controls.Add(this.panel1);
		this.tabSD.Controls.Add(this.panel2);
		this.tabSD.Controls.Add(this.panel3);
		this.tabSD.Location = new System.Drawing.Point(4, 25);
		this.tabSD.Name = "tabSD";
		this.tabSD.Padding = new System.Windows.Forms.Padding(3);
		this.tabSD.Size = new System.Drawing.Size(1031, 647);
		this.tabSD.TabIndex = 0;
		this.tabSD.Text = "Screen Definition";
		this.tabSD.UseVisualStyleBackColor = true;
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(3, 150);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1025, 494);
		this.panel1.TabIndex = 4;
		this.tabDT.Controls.Add((System.Windows.Forms.Control)(object)this.grdDocType);
		this.tabDT.Controls.Add(this.panel11);
		this.tabDT.Controls.Add(this.panel10);
		this.tabDT.Controls.Add(this.panel9);
		this.tabDT.Location = new System.Drawing.Point(4, 25);
		this.tabDT.Name = "tabDT";
		this.tabDT.Padding = new System.Windows.Forms.Padding(3);
		this.tabDT.Size = new System.Drawing.Size(1031, 647);
		this.tabDT.TabIndex = 1;
		this.tabDT.Text = "Document Types";
		this.tabDT.UseVisualStyleBackColor = true;
		this.grdDocType.AllowEdit = (InheritableBoolean)2;
		grdDocType_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDocType.DesignTimeLayout = grdDocType_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDocType).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDocType.FilterMode = (FilterMode)2;
		this.grdDocType.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDocType).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDocType.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDocType).Location = new System.Drawing.Point(3, 199);
		((System.Windows.Forms.Control)(object)this.grdDocType).Name = "grdDocType";
		((ScrollableBase)this.grdDocType).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDocType).Size = new System.Drawing.Size(1025, 445);
		((System.Windows.Forms.Control)(object)this.grdDocType).TabIndex = 1;
		this.grdDocType.TotalRow = (InheritableBoolean)1;
		this.grdDocType.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDocType).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDocType).DoubleClick += new System.EventHandler(grdDocType_DoubleClick);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.label17);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 173);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1025, 26);
		this.panel11.TabIndex = 5;
		this.label17.AutoSize = true;
		this.label17.BackColor = System.Drawing.Color.Teal;
		this.label17.Font = new System.Drawing.Font("Tahoma", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.ForeColor = System.Drawing.Color.White;
		this.label17.Location = new System.Drawing.Point(5, 1);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(80, 23);
		this.label17.TabIndex = 25;
		this.label17.Text = "History";
		this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel10.BackColor = System.Drawing.Color.White;
		this.panel10.Controls.Add(this.label16);
		this.panel10.Controls.Add(this.label15);
		this.panel10.Controls.Add(this.label10);
		this.panel10.Controls.Add(this.label11);
		this.panel10.Controls.Add(this.txtDocTypeScreenName);
		this.panel10.Controls.Add(this.txtDocTypeOtherLangDescription);
		this.panel10.Controls.Add(this.txtDocTypeDescription);
		this.panel10.Controls.Add(this.txtDocTypeRemarks);
		this.panel10.Controls.Add(this.txtDocTypeTargetUrl);
		this.panel10.Controls.Add(this.label13);
		this.panel10.Controls.Add(this.txtDocTypeCode);
		this.panel10.Controls.Add(this.label12);
		this.panel10.Controls.Add(this.txtDocTypeId);
		this.panel10.Controls.Add(this.label14);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 57);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1025, 116);
		this.panel10.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(658, 14);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(78, 15);
		this.label16.TabIndex = 25;
		this.label16.Text = "Screen Name";
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(272, 40);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(95, 15);
		this.label15.TabIndex = 25;
		this.label15.Text = "Other Lang Desc";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(272, 14);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(68, 15);
		this.label10.TabIndex = 25;
		this.label10.Text = "Description";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(8, 14);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(20, 15);
		this.label11.TabIndex = 22;
		this.label11.Text = "ID";
		this.txtDocTypeScreenName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTypeScreenName.Location = new System.Drawing.Point(739, 10);
		this.txtDocTypeScreenName.Name = "txtDocTypeScreenName";
		this.txtDocTypeScreenName.Size = new System.Drawing.Size(199, 23);
		this.txtDocTypeScreenName.TabIndex = 6;
		this.txtDocTypeOtherLangDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTypeOtherLangDescription.Location = new System.Drawing.Point(367, 36);
		this.txtDocTypeOtherLangDescription.Name = "txtDocTypeOtherLangDescription";
		this.txtDocTypeOtherLangDescription.Size = new System.Drawing.Size(288, 23);
		this.txtDocTypeOtherLangDescription.TabIndex = 5;
		this.txtDocTypeDescription.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTypeDescription.Location = new System.Drawing.Point(367, 10);
		this.txtDocTypeDescription.Name = "txtDocTypeDescription";
		this.txtDocTypeDescription.Size = new System.Drawing.Size(288, 23);
		this.txtDocTypeDescription.TabIndex = 4;
		this.txtDocTypeRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTypeRemarks.Location = new System.Drawing.Point(67, 89);
		this.txtDocTypeRemarks.Name = "txtDocTypeRemarks";
		this.txtDocTypeRemarks.Size = new System.Drawing.Size(588, 23);
		this.txtDocTypeRemarks.TabIndex = 3;
		this.txtDocTypeTargetUrl.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTypeTargetUrl.Location = new System.Drawing.Point(67, 63);
		this.txtDocTypeTargetUrl.Name = "txtDocTypeTargetUrl";
		this.txtDocTypeTargetUrl.Size = new System.Drawing.Size(588, 23);
		this.txtDocTypeTargetUrl.TabIndex = 2;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(8, 40);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(34, 15);
		this.label13.TabIndex = 23;
		this.label13.Text = "Code";
		this.txtDocTypeCode.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTypeCode.Location = new System.Drawing.Point(67, 36);
		this.txtDocTypeCode.Name = "txtDocTypeCode";
		this.txtDocTypeCode.Size = new System.Drawing.Size(199, 23);
		this.txtDocTypeCode.TabIndex = 1;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(8, 93);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 23;
		this.label12.Text = "Remarks";
		this.txtDocTypeId.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocTypeId.Location = new System.Drawing.Point(67, 10);
		this.txtDocTypeId.Name = "txtDocTypeId";
		this.txtDocTypeId.Size = new System.Drawing.Size(199, 23);
		this.txtDocTypeId.TabIndex = 0;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(8, 67);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(58, 15);
		this.label14.TabIndex = 23;
		this.label14.Text = "Target Url";
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label9);
		this.panel9.Controls.Add(this.toolStrip2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1025, 54);
		this.panel9.TabIndex = 2;
		this.label9.AutoSize = true;
		this.label9.BackColor = System.Drawing.Color.Teal;
		this.label9.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(3, 31);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(131, 19);
		this.label9.TabIndex = 25;
		this.label9.Text = "DocumentType";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.toolStrip2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.BtnNewDocType, this.BtnSaveDocType, this.BtnUpdateDocType, this.BtnShortCutKeyDT });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1025, 27);
		this.toolStrip2.TabIndex = 8;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewDocType.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnNewDocType.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewDocType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewDocType.Name = "BtnNewDocType";
		this.BtnNewDocType.Size = new System.Drawing.Size(59, 24);
		this.BtnNewDocType.Text = "&New";
		this.BtnNewDocType.Click += new System.EventHandler(btnNewDocType_Click);
		this.BtnSaveDocType.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnSaveDocType.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.BtnSaveDocType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnSaveDocType.Name = "BtnSaveDocType";
		this.BtnSaveDocType.Size = new System.Drawing.Size(64, 24);
		this.BtnSaveDocType.Text = "&Save";
		this.BtnSaveDocType.Click += new System.EventHandler(BtnSaveDocType_Click);
		this.BtnUpdateDocType.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnUpdateDocType.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateDocType.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdateDocType.Name = "BtnUpdateDocType";
		this.BtnUpdateDocType.Size = new System.Drawing.Size(78, 24);
		this.BtnUpdateDocType.Text = "&Update";
		this.BtnUpdateDocType.Visible = false;
		this.BtnUpdateDocType.Click += new System.EventHandler(BtnUpdateDocType_Click);
		this.BtnShortCutKeyDT.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutKeyDT.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutKeyDT.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutKeyDT.Name = "BtnShortCutKeyDT";
		this.BtnShortCutKeyDT.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutKeyDT.Text = "ShortCut Keys";
		this.BtnShortCutKeyDT.Click += new System.EventHandler(btnShortcutKeys_Click);
		this.tabMethods.Controls.Add((System.Windows.Forms.Control)(object)this.grdMethodsDefine);
		this.tabMethods.Controls.Add(this.panel12);
		this.tabMethods.Controls.Add(this.panel13);
		this.tabMethods.Controls.Add(this.panel14);
		this.tabMethods.Location = new System.Drawing.Point(4, 25);
		this.tabMethods.Name = "tabMethods";
		this.tabMethods.Size = new System.Drawing.Size(1031, 647);
		this.tabMethods.TabIndex = 2;
		this.tabMethods.Text = "Define Methods";
		this.tabMethods.UseVisualStyleBackColor = true;
		this.grdMethodsDefine.AllowEdit = (InheritableBoolean)2;
		grdMethodsDefine_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdMethodsDefine.DesignTimeLayout = grdMethodsDefine_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdMethodsDefine).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdMethodsDefine.FilterMode = (FilterMode)2;
		this.grdMethodsDefine.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdMethodsDefine).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdMethodsDefine.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdMethodsDefine).Location = new System.Drawing.Point(0, 143);
		((System.Windows.Forms.Control)(object)this.grdMethodsDefine).Name = "grdMethodsDefine";
		((ScrollableBase)this.grdMethodsDefine).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdMethodsDefine).Size = new System.Drawing.Size(1031, 504);
		((System.Windows.Forms.Control)(object)this.grdMethodsDefine).TabIndex = 1;
		this.grdMethodsDefine.TotalRow = (InheritableBoolean)1;
		this.grdMethodsDefine.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdMethodsDefine).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdMethodsDefine).DoubleClick += new System.EventHandler(grdMethodDefine_DoubleClick);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label18);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 117);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1031, 26);
		this.panel12.TabIndex = 9;
		this.label18.AutoSize = true;
		this.label18.BackColor = System.Drawing.Color.Teal;
		this.label18.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label18.ForeColor = System.Drawing.Color.White;
		this.label18.Location = new System.Drawing.Point(5, 1);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(67, 19);
		this.label18.TabIndex = 25;
		this.label18.Text = "History";
		this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel13.BackColor = System.Drawing.Color.White;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDocumentTypeMethodDefine);
		this.panel13.Controls.Add(this.label19);
		this.panel13.Controls.Add(this.txtScreenNameMethodDefine);
		this.panel13.Controls.Add(this.txtRemarksMethodDefine);
		this.panel13.Controls.Add(this.txtReportMethod);
		this.panel13.Controls.Add(this.label24);
		this.panel13.Controls.Add(this.label20);
		this.panel13.Controls.Add(this.label25);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 54);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1031, 63);
		this.panel13.TabIndex = 0;
		this.CmbDocumentTypeMethodDefine.AllowNull = (DefaultableBoolean)1;
		this.CmbDocumentTypeMethodDefine.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDocumentTypeMethodDefine.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDocumentTypeMethodDefine.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Appearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance31).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance31;
		((SpecialBoxBase)((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDocumentTypeMethodDefine).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeMethodDefine).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDocumentTypeMethodDefine.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeMethodDefine).Location = new System.Drawing.Point(101, 6);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeMethodDefine).Name = "CmbDocumentTypeMethodDefine";
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeMethodDefine).Size = new System.Drawing.Size(199, 26);
		((System.Windows.Forms.Control)(object)this.CmbDocumentTypeMethodDefine).TabIndex = 0;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label19.Location = new System.Drawing.Point(8, 39);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(78, 15);
		this.label19.TabIndex = 25;
		this.label19.Text = "Screen Name";
		this.txtScreenNameMethodDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtScreenNameMethodDefine.Location = new System.Drawing.Point(101, 35);
		this.txtScreenNameMethodDefine.Name = "txtScreenNameMethodDefine";
		this.txtScreenNameMethodDefine.Size = new System.Drawing.Size(199, 23);
		this.txtScreenNameMethodDefine.TabIndex = 1;
		this.txtRemarksMethodDefine.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksMethodDefine.Location = new System.Drawing.Point(389, 35);
		this.txtRemarksMethodDefine.Name = "txtRemarksMethodDefine";
		this.txtRemarksMethodDefine.Size = new System.Drawing.Size(562, 23);
		this.txtRemarksMethodDefine.TabIndex = 3;
		this.txtReportMethod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReportMethod.Location = new System.Drawing.Point(389, 8);
		this.txtReportMethod.Name = "txtReportMethod";
		this.txtReportMethod.Size = new System.Drawing.Size(562, 23);
		this.txtReportMethod.TabIndex = 2;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label24.Location = new System.Drawing.Point(304, 39);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(52, 15);
		this.label24.TabIndex = 23;
		this.label24.Text = "Remarks";
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(8, 12);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(92, 15);
		this.label20.TabIndex = 23;
		this.label20.Text = "Document Type";
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(304, 12);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(87, 15);
		this.label25.TabIndex = 23;
		this.label25.Text = "Report Method";
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label26);
		this.panel14.Controls.Add(this.toolStrip3);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel14.Location = new System.Drawing.Point(0, 0);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1031, 54);
		this.panel14.TabIndex = 8;
		this.label26.AutoSize = true;
		this.label26.BackColor = System.Drawing.Color.Teal;
		this.label26.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label26.ForeColor = System.Drawing.Color.White;
		this.label26.Location = new System.Drawing.Point(3, 31);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(136, 19);
		this.label26.TabIndex = 25;
		this.label26.Text = "Define Methods";
		this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.toolStrip3.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.BtnNewMethods, this.BtnRefereshMethod, this.btnSaveMethod, this.BtnUpdateMethods, this.btnShortcutkeysMethod });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1031, 27);
		this.toolStrip3.TabIndex = 8;
		this.toolStrip3.Text = "toolStrip3";
		this.BtnNewMethods.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnNewMethods.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewMethods.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewMethods.Name = "BtnNewMethods";
		this.BtnNewMethods.Size = new System.Drawing.Size(59, 24);
		this.BtnNewMethods.Text = "&New";
		this.BtnNewMethods.Click += new System.EventHandler(btnNewMethodDefine_Click);
		this.BtnRefereshMethod.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnRefereshMethod.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.BtnRefereshMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnRefereshMethod.Name = "BtnRefereshMethod";
		this.BtnRefereshMethod.Size = new System.Drawing.Size(80, 24);
		this.BtnRefereshMethod.Text = "&Refresh";
		this.BtnRefereshMethod.Click += new System.EventHandler(BtnRefereshMethod_Click);
		this.btnSaveMethod.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.btnSaveMethod.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveMethod.Name = "btnSaveMethod";
		this.btnSaveMethod.Size = new System.Drawing.Size(64, 24);
		this.btnSaveMethod.Text = "&Save";
		this.btnSaveMethod.Click += new System.EventHandler(BtnSaveMethodDefine_Click);
		this.BtnUpdateMethods.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.BtnUpdateMethods.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.BtnUpdateMethods.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnUpdateMethods.Name = "BtnUpdateMethods";
		this.BtnUpdateMethods.Size = new System.Drawing.Size(78, 24);
		this.BtnUpdateMethods.Text = "&Update";
		this.BtnUpdateMethods.Visible = false;
		this.BtnUpdateMethods.Click += new System.EventHandler(BtnUpdateMethodDefine_Click);
		this.btnShortcutkeysMethod.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShortcutkeysMethod.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnShortcutkeysMethod.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnShortcutkeysMethod.Name = "btnShortcutkeysMethod";
		this.btnShortcutkeysMethod.Size = new System.Drawing.Size(126, 24);
		this.btnShortcutkeysMethod.Text = "ShortCut Keys";
		this.btnShortcutkeysMethod.Click += new System.EventHandler(btnShortcutKeys_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1039, 676);
		base.Controls.Add(this.tabControl1);
		base.KeyPreview = true;
		base.Name = "frmAppMenu_ScreensDefine";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosed += new System.Windows.Forms.FormClosedEventHandler(frmScreenDefinetion_FormClosed);
		base.Load += new System.EventHandler(frmScreenDefinetion_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmScreenDefinetion_KeyDown);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbCompany).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbModuleName).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdScreenDefination).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdScreenRights).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.tabControl1.ResumeLayout(false);
		this.tabSD.ResumeLayout(false);
		this.panel1.ResumeLayout(false);
		this.tabDT.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDocType).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		this.tabMethods.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdMethodsDefine).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbDocumentTypeMethodDefine).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
