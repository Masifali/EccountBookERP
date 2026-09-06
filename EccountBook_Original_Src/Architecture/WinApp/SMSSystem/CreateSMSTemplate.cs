using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Architecture.BLL.SMS;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.SMS;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;

namespace Architecture.WinApp.SMSSystem;

public class CreateSMSTemplate : Form
{
	private DropDownBind DDL = new DropDownBind();

	public string ScreenName = string.Empty;

	public UserAccount UserAccount = new UserAccount();

	private List<MessageBody> lst;

	private DataTable dtlstmessage = new DataTable();

	private DataTable dtGetScreenName = new DataTable();

	private DataTable dtTemplate = new DataTable();

	private string scrnName = string.Empty;

	private int RecId;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage2;

	private Panel panel5;

	private Panel panel7;

	private Panel panel6;

	internal ToolStrip toolStrip2;

	internal ToolStripButton btnTemRefresh;

	internal ToolStripButton btnSaveTemplate;

	internal ToolStripButton btnRefresh;

	private Button btnAddParameter;

	private Button btnGroup;

	private Label label4;

	private TextBox txtParameter;

	internal ListBox lstParameterDynamic;

	internal UltraCombo cmbGroup;

	internal UltraCombo cmbScreenName;

	private Label label5;

	private Label label3;

	private Label label1;

	internal UltraCombo cmbTemplateName;

	internal TextBox txtTemplateView;

	private ToolStripButton btnupdate;

	private GroupBox groupBox1;

	internal ToolStripButton toolStripButton1;

	private ToolStripSeparator toolStripSeparator1;

	public CreateSMSTemplate(UserAccount User_Account)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = User_Account;
	}

	private void txtSMSTemplate_DragDrop(object sender, DragEventArgs e)
	{
		txtTemplateView.SelectedText = e.Data.GetData(DataFormats.Text).ToString() + " ";
	}

	private void txtSMSTemplate_DragOver(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.StringFormat))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void CreateSMSTemplate_Load(object sender, EventArgs e)
	{
		try
		{
			cmbScreenName.Focus();
			BindGroup();
			BindScreenName();
			txtTemplateView.AllowDrop = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void RefereshCombo()
	{
		try
		{
			((Control)(object)cmbGroup).Text = string.Empty;
			((Control)(object)cmbTemplateName).Text = string.Empty;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindScreenName()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters body = new ReportsParameters();
			body.IsCreateSMSTemplate = true;
			dtGetScreenName = MessageBody.GetScreenName(body);
			if (dtGetScreenName.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("ScreenName");
				for (int i = 0; i < dtGetScreenName.Rows.Count; i++)
				{
					dt.Rows.Add(dtGetScreenName.Rows[i]["Id"], dtGetScreenName.Rows[i]["ScreenName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbScreenName, "Id", "ScreenName", "Screen Name", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindGroup()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		try
		{
			tblReceiverGroup val = new tblReceiverGroup
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			DataTable dtgroup = new DataTable();
			dtgroup = tblReceiverGroup.GetAllocateGroup(val);
			if (dtgroup.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id");
				dt.Columns.Add("Group");
				for (int i = 0; i < dtgroup.Rows.Count; i++)
				{
					dt.Rows.Add(dtgroup.Rows[i]["Id"], dtgroup.Rows[i]["GroupName"]);
				}
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDL(dt, cmbGroup, "Id", "Group", "Group", ZeroIndex: false);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindTemplate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			MessageBody body = new MessageBody();
			body.OrganizationId = UserAccount.OrganizationId;
			body.CompanyId = UserAccount.CompanyId;
			dtlstmessage = MessageBody.GetMessageBodytable(body);
			if (dtlstmessage.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtlstmessage, cmbTemplateName, "GroupId", "SMSBody", "Template", false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindAllocateTemplate()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			tblAllocateTemplateToGroup toGroup = new tblAllocateTemplateToGroup();
			toGroup.OrganizationId = UserAccount.OrganizationId;
			toGroup.CompanyId = UserAccount.CompanyId;
			toGroup.GroupId = Conversion.ToInt(cmbGroup.Value);
			dtTemplate = tblAllocateTemplateToGroup.GetTemplate(toGroup);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAddParameter_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (formvalidation())
			{
				SmsScreenParameterList.Save(new SmsScreenParameterList
				{
					ScreenId = Conversion.ToString((object)((Control)(object)cmbScreenName).Text),
					smsDocumentTypeId = 0,
					smsParameterFeild = "@" + txtParameter.Text.Trim()
				});
				lstParameterDynamic.Items.Add("@" + txtParameter.Text);
				txtParameter.Text = "";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool formvalidation()
	{
		if (((Control)(object)cmbScreenName).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Screen Name");
			cmbScreenName.Focus();
			return false;
		}
		if (txtParameter.Text.Trim() == "")
		{
			MessageBox.Show("Parameter Filed Required");
			txtParameter.Focus();
			return false;
		}
		return true;
	}

	private void lstParameterDynamic_MouseDown(object sender, MouseEventArgs e)
	{
		try
		{
			lstParameterDynamic.DoDragDrop(lstParameterDynamic.SelectedItem, DragDropEffects.Copy);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void lstParameterDynamic_DragDrop(object sender, DragEventArgs e)
	{
		lstParameterDynamic.DoDragDrop(lstParameterDynamic.SelectedItem, DragDropEffects.Copy);
	}

	private void txtTemplateView_DragDrop(object sender, DragEventArgs e)
	{
		txtTemplateView.SelectedText = e.Data.GetData(DataFormats.Text).ToString() + " ";
	}

	private void txtTemplateView_DragOver(object sender, DragEventArgs e)
	{
		if (e.Data.GetDataPresent(DataFormats.StringFormat))
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}
	}

	private void cmbScreenName_Leave(object sender, EventArgs e)
	{
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		try
		{
			if (!(((Control)(object)cmbScreenName).Text != string.Empty))
			{
				return;
			}
			if (dtGetScreenName.Rows.Count > 0)
			{
				for (int j = 0; j < dtGetScreenName.Rows.Count; j++)
				{
					if (Conversion.ToInt(dtGetScreenName.Rows[j]["Id"]) == Conversion.ToInt(cmbScreenName.Value))
					{
						scrnName = dtGetScreenName.Rows[j]["screenDefinitionName"].ToString();
					}
				}
			}
			MessageBody body = new MessageBody();
			body.OrganizationId = UserAccount.OrganizationId;
			body.CompanyId = UserAccount.CompanyId;
			body.ScreenName = scrnName;
			dtlstmessage = MessageBody.GetMessageBodytable(body);
			if (dtlstmessage.Rows.Count > 0 && dtlstmessage.Rows.Count > 0)
			{
				DDL.BindDDL(dtlstmessage, cmbTemplateName, "GroupId", "SMSBody", "Template", ZeroIndex: false);
			}
			SmsScreenParameterList val = new SmsScreenParameterList
			{
				ScreenId = Conversion.ToString((object)((Control)(object)cmbScreenName).Text)
			};
			DataTable dtPara = new DataTable();
			dtPara = SmsScreenParameterList.GetParameter(val);
			if (dtPara.Rows.Count > 0)
			{
				lstParameterDynamic.Items.Clear();
				for (int i = 0; i < dtPara.Rows.Count; i++)
				{
					lstParameterDynamic.Items.Add(dtPara.Rows[i]["smsParameterFeild"]);
				}
			}
			else
			{
				lstParameterDynamic.Items.Clear();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbTemplateName_TextChanged(object sender, EventArgs e)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (((Control)(object)cmbTemplateName).Text != string.Empty)
			{
				new MessageBody();
				if (dtlstmessage.Rows.Count <= 0)
				{
					return;
				}
				for (int i = 0; i < dtlstmessage.Rows.Count; i++)
				{
					if (Conversion.ToString(dtlstmessage.Rows[i]["SMSBody"]) == Conversion.ToString((object)((Control)(object)cmbTemplateName).Text))
					{
						txtTemplateView.Text = dtlstmessage.Rows[i]["SMSBody"].ToString();
						cmbGroup.Value = Conversion.ToInt(dtlstmessage.Rows[i]["GroupId"]);
						txtTemplateView.Visible = true;
						btnupdate.Visible = true;
						btnSaveTemplate.Visible = false;
						RecId = Conversion.ToInt(dtlstmessage.Rows[i]["Id"]);
					}
					else
					{
						txtTemplateView.Text = string.Empty;
						btnupdate.Visible = false;
						btnSaveTemplate.Visible = true;
					}
				}
			}
			else
			{
				txtTemplateView.Text = string.Empty;
				btnupdate.Visible = false;
				btnSaveTemplate.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveTemplate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		try
		{
			if (TemplateSavevalidation())
			{
				int num = MessageBody.Save(new MessageBody
				{
					ScreenName = scrnName,
					GroupId = Conversion.ToInt(cmbGroup.Value),
					SMSBody = txtTemplateView.Text,
					SMSTemplateName = ((Control)(object)cmbTemplateName).Text,
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyUser = UserAccount.ID,
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId
				});
				if (((Control)(object)cmbGroup).Text != string.Empty)
				{
					tblAllocateTemplateToGroup.Save(new tblAllocateTemplateToGroup
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						EntryUser = UserAccount.ID,
						EntryDate = DateTime.Now,
						GroupId = Conversion.ToInt(cmbGroup.Value),
						TemplateId = Conversion.ToInt(cmbTemplateName.Value)
					});
				}
				if (num > 0)
				{
					MessageBox.Show("Save Successfully");
					Reset();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		try
		{
			if (TemplateSavevalidation())
			{
				int num = MessageBody.Save(new MessageBody
				{
					Id = RecId,
					ScreenName = scrnName,
					GroupId = Conversion.ToInt(cmbGroup.Value),
					SMSBody = txtTemplateView.Text,
					SMSTemplateName = ((Control)(object)cmbTemplateName).Text,
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyUser = UserAccount.ID,
					CompanyId = UserAccount.CompanyId,
					OrganizationId = UserAccount.OrganizationId
				});
				if (((Control)(object)cmbGroup).Text != string.Empty)
				{
					tblAllocateTemplateToGroup.Save(new tblAllocateTemplateToGroup
					{
						OrganizationId = UserAccount.OrganizationId,
						CompanyId = UserAccount.CompanyId,
						EntryUser = UserAccount.ID,
						EntryDate = DateTime.Now,
						GroupId = Conversion.ToInt(cmbGroup.Value),
						TemplateId = Conversion.ToInt(cmbTemplateName.Value)
					});
				}
				if (num > 0)
				{
					MessageBox.Show("Update successfully");
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool TemplateSavevalidation()
	{
		if (((Control)(object)cmbScreenName).Text.Trim() == "")
		{
			MessageBox.Show("Please Select Screen Name");
			cmbScreenName.Focus();
			return false;
		}
		if (((Control)(object)cmbTemplateName).Text.Trim() == "")
		{
			MessageBox.Show("Template Name Filed Required");
			cmbTemplateName.Focus();
			return false;
		}
		if (txtTemplateView.Text == string.Empty.Trim())
		{
			MessageBox.Show("SMS Body Filed Required");
			txtTemplateView.Focus();
			return false;
		}
		return true;
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			BindGroup();
			BindScreenName();
			Reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Reset()
	{
		try
		{
			((Control)(object)cmbGroup).Text = string.Empty;
			((Control)(object)cmbScreenName).Text = string.Empty;
			((Control)(object)cmbTemplateName).Text = string.Empty;
			txtParameter.Text = string.Empty;
			txtTemplateView.AllowDrop = true;
			lstParameterDynamic.Items.Clear();
			txtTemplateView.Text = string.Empty;
			cmbScreenName.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnGroup_Click(object sender, EventArgs e)
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
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Expected O, but got Unknown
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Expected O, but got Unknown
		//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Expected O, but got Unknown
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
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
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.SMSSystem.CreateSMSTemplate));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel7 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtTemplateView = new System.Windows.Forms.TextBox();
		this.btnAddParameter = new System.Windows.Forms.Button();
		this.cmbTemplateName = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.btnGroup = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.txtParameter = new System.Windows.Forms.TextBox();
		this.cmbScreenName = new UltraCombo();
		this.lstParameterDynamic = new System.Windows.Forms.ListBox();
		this.cmbGroup = new UltraCombo();
		this.panel6 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnTemRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSaveTemplate = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel7.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTemplateName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbScreenName).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroup).BeginInit();
		this.panel6.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(810, 448);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(810, 448);
		this.tabControl1.TabIndex = 0;
		this.tabPage2.Controls.Add(this.panel5);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(802, 422);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "Template Allocate To Group";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add(this.panel7);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(796, 416);
		this.panel5.TabIndex = 0;
		this.panel7.Controls.Add(this.groupBox1);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(0, 29);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(796, 387);
		this.panel7.TabIndex = 1;
		this.groupBox1.Controls.Add(this.txtTemplateView);
		this.groupBox1.Controls.Add(this.btnAddParameter);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbTemplateName);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.btnGroup);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.txtParameter);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbScreenName);
		this.groupBox1.Controls.Add(this.lstParameterDynamic);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGroup);
		this.groupBox1.Location = new System.Drawing.Point(5, 6);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(575, 313);
		this.groupBox1.TabIndex = 55;
		this.groupBox1.TabStop = false;
		this.txtTemplateView.Location = new System.Drawing.Point(16, 74);
		this.txtTemplateView.Multiline = true;
		this.txtTemplateView.Name = "txtTemplateView";
		this.txtTemplateView.Size = new System.Drawing.Size(341, 198);
		this.txtTemplateView.TabIndex = 54;
		this.txtTemplateView.DragDrop += new System.Windows.Forms.DragEventHandler(txtTemplateView_DragDrop);
		this.txtTemplateView.DragOver += new System.Windows.Forms.DragEventHandler(txtTemplateView_DragOver);
		this.btnAddParameter.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAddParameter.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAddParameter.Location = new System.Drawing.Point(542, 32);
		this.btnAddParameter.Name = "btnAddParameter";
		this.btnAddParameter.Size = new System.Drawing.Size(24, 22);
		this.btnAddParameter.TabIndex = 4;
		this.btnAddParameter.UseVisualStyleBackColor = true;
		this.btnAddParameter.Click += new System.EventHandler(btnAddParameter_Click);
		this.cmbTemplateName.AllowNull = (DefaultableBoolean)1;
		this.cmbTemplateName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbTemplateName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbTemplateName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbTemplateName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance21;
		((SpecialBoxBase)((UltraGridBase)this.cmbTemplateName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance23).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance23).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance25).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance27).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance28).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance28;
		((AppearanceBase)appearance29).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbTemplateName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbTemplateName).Location = new System.Drawing.Point(102, 46);
		((System.Windows.Forms.Control)(object)this.cmbTemplateName).Name = "cmbTemplateName";
		((System.Windows.Forms.Control)(object)this.cmbTemplateName).Size = new System.Drawing.Size(255, 22);
		((System.Windows.Forms.Control)(object)this.cmbTemplateName).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbTemplateName).TextChanged += new System.EventHandler(cmbTemplateName_TextChanged);
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(13, 51);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(82, 13);
		this.label1.TabIndex = 46;
		this.label1.Text = "Template Name";
		this.btnGroup.Font = new System.Drawing.Font("Calibri", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGroup.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnGroup.Location = new System.Drawing.Point(333, 278);
		this.btnGroup.Name = "btnGroup";
		this.btnGroup.Size = new System.Drawing.Size(24, 22);
		this.btnGroup.TabIndex = 52;
		this.btnGroup.UseVisualStyleBackColor = true;
		this.btnGroup.Click += new System.EventHandler(btnGroup_Click);
		this.label3.AutoSize = true;
		this.label3.Location = new System.Drawing.Point(13, 25);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(72, 13);
		this.label3.TabIndex = 48;
		this.label3.Text = "Screen Name";
		this.label4.AutoSize = true;
		this.label4.Location = new System.Drawing.Point(381, 15);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(92, 13);
		this.label4.TabIndex = 51;
		this.label4.Text = "Pareameter Name";
		this.label4.Visible = false;
		this.label5.AutoSize = true;
		this.label5.Location = new System.Drawing.Point(13, 283);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(36, 13);
		this.label5.TabIndex = 44;
		this.label5.Text = "Group";
		this.txtParameter.Location = new System.Drawing.Point(384, 34);
		this.txtParameter.Name = "txtParameter";
		this.txtParameter.Size = new System.Drawing.Size(152, 20);
		this.txtParameter.TabIndex = 3;
		this.cmbScreenName.AllowNull = (DefaultableBoolean)1;
		this.cmbScreenName.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbScreenName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbScreenName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance18;
		((SpecialBoxBase)((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance32).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance33;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance35).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance36).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance36).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance36).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance36;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbScreenName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbScreenName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Location = new System.Drawing.Point(102, 20);
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Name = "cmbScreenName";
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Size = new System.Drawing.Size(255, 22);
		((System.Windows.Forms.Control)(object)this.cmbScreenName).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbScreenName).Leave += new System.EventHandler(cmbScreenName_Leave);
		this.lstParameterDynamic.FormattingEnabled = true;
		this.lstParameterDynamic.Items.AddRange(new object[1] { "" });
		this.lstParameterDynamic.Location = new System.Drawing.Point(384, 62);
		this.lstParameterDynamic.Name = "lstParameterDynamic";
		this.lstParameterDynamic.Size = new System.Drawing.Size(182, 238);
		this.lstParameterDynamic.TabIndex = 42;
		this.lstParameterDynamic.DragDrop += new System.Windows.Forms.DragEventHandler(lstParameterDynamic_DragDrop);
		this.lstParameterDynamic.MouseDown += new System.Windows.Forms.MouseEventHandler(lstParameterDynamic_MouseDown);
		this.cmbGroup.AllowNull = (DefaultableBoolean)1;
		this.cmbGroup.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGroup.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGroup.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbGroup).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGroup).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbGroup).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGroup).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGroup).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbGroup).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGroup).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGroup).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.cmbGroup.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGroup).Location = new System.Drawing.Point(102, 278);
		((System.Windows.Forms.Control)(object)this.cmbGroup).Name = "cmbGroup";
		((System.Windows.Forms.Control)(object)this.cmbGroup).Size = new System.Drawing.Size(225, 22);
		((System.Windows.Forms.Control)(object)this.cmbGroup).TabIndex = 2;
		this.panel6.Controls.Add(this.toolStrip2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(796, 29);
		this.panel6.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[6] { this.toolStripButton1, this.toolStripSeparator1, this.btnRefresh, this.btnTemRefresh, this.btnSaveTemplate, this.btnupdate });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(796, 28);
		this.toolStrip2.TabIndex = 5;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripButton1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(207, 25);
		this.toolStripButton1.Text = "Create SMS Template        ";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
		this.btnRefresh.Image = (System.Drawing.Image)resources.GetObject("btnRefresh.Image");
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(101, 25);
		this.btnRefresh.Text = "Form Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnTemRefresh.Image = (System.Drawing.Image)resources.GetObject("btnTemRefresh.Image");
		this.btnTemRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnTemRefresh.Name = "btnTemRefresh";
		this.btnTemRefresh.Size = new System.Drawing.Size(55, 25);
		this.btnTemRefresh.Text = "&New";
		this.btnSaveTemplate.Image = (System.Drawing.Image)resources.GetObject("btnSaveTemplate.Image");
		this.btnSaveTemplate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveTemplate.Name = "btnSaveTemplate";
		this.btnSaveTemplate.Size = new System.Drawing.Size(55, 25);
		this.btnSaveTemplate.Text = "&Save";
		this.btnSaveTemplate.Click += new System.EventHandler(btnSaveTemplate_Click);
		this.btnupdate.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnupdate.Image = (System.Drawing.Image)resources.GetObject("btnupdate.Image");
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(69, 25);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(810, 448);
		base.Controls.Add(this.panel1);
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "CreateSMSTemplate";
		this.Text = "CreateSMSTemplate";
		base.Load += new System.EventHandler(CreateSMSTemplate_Load);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbTemplateName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbScreenName).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGroup).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
