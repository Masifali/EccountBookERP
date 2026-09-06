using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.DMS;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.DMS;
using Architecture.WinApp.Common;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.DMS_FolderHierarchy;

public class FolderHierarchyForSubUser : Form
{
	private DropDownBind DDL = new DropDownBind();

	private List<FolderHierarchy> list = new List<FolderHierarchy>();

	private UserAccount UserAccount = new UserAccount();

	private List<int> DocFilePathIds = new List<int>();

	private List<int> UnAllocatedDocFilePathIds = new List<int>();

	private int RecId;

	private IContainer components;

	private Button btnCreateFolder;

	private TextBox FolderName;

	private Label label1;

	private TreeView treeView;

	private ContextMenuStrip MenuStrip;

	private ToolStripMenuItem AddNewFolder;

	private ImageList imageList;

	private Panel ParentPanel;

	private Label label4;

	private Label lblPathOfFolder;

	private TabControl TabControl;

	private TabPage FolderTab;

	private TabPage DocumentsTab;

	internal GridEX GrdDocuments;

	private ToolStripMenuItem btnUploadFileInSelectedFolder;

	private TabControl tabControl1;

	private TabPage CardViewOfDocuments;

	private TabPage GridViewOfDocuments;

	private Panel panel3;

	private FlowLayoutPanel DocumentsFlowLayout;

	private Label label5;

	private Label lblTotalFiles;

	private Label lblTotalSize;

	private Label label2;

	private Panel TreeViewPanel;

	private Panel HeaderFolderPanel;

	private Panel FolderInfoPanel;

	private Label label6;

	private Panel panel2;

	private Panel panel1;

	private Panel TreeViewContainPanel;

	private Panel AllocationPanel;

	private Label label3;

	private Panel ButtonPanelForAllocation;

	private PictureBox btnAllocateUsers;

	private Label label7;

	internal GridEX GridUsersForAllocation;

	internal UltraCombo CmbUserName;

	private Label label8;

	private Panel UnAllocatedDocumentsPanel;

	private Panel panel4;

	private Label label9;

	private TabControl UnAllocatedDocumentsTabControl;

	private TabPage CardsTab;

	private FlowLayoutPanel UnAllocatedDocumentsFlowLayout;

	private Panel panel6;

	private Label label10;

	private Label label11;

	private Label lblTotalUnAllocatedFilesCount;

	private Label lblTotalUnAllocatedFilesSize;

	private TabPage GridTab;

	internal GridEX GridUnAllocatedDocumets;

	private PictureBox btnAllocateUnAllocatedDocuments;

	private PictureBox btnUnAllocateAllocatedDocuments;

	public FolderHierarchyForSubUser(UserAccount pUser)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = pUser;
	}

	private bool FormValidation()
	{
		if (FolderName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Folder Name Field Required");
			FolderName.Focus();
			return false;
		}
		return true;
	}

	private void Insert()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			string path = CommonServices.GetConfigurationByOrgCompandConfigDescription("RootPathForShareFolderStructure") + "\\" + FolderName.Text;
			if (Directory.Exists(path))
			{
				MessageBox.Show("Folder Name " + FolderName.Text + "Already Exist");
				return;
			}
			Directory.CreateDirectory(path);
			FolderHierarchy data = new FolderHierarchy();
			if (RecId > 0)
			{
				data.FolderHierarchyId = RecId;
			}
			else
			{
				data.FolderHierarchyId = 0L;
			}
			data.DriveName = "";
			data.ParentFolderHierarchyId = 0L;
			data.FolderLevel = 1;
			data.FolderName = FolderName.Text;
			data.FolderPath = path;
			data.FolderOwnerId = UserAccount.ID;
			data.CreatedById = UserAccount.ID;
			data.CreatedDate = DateTime.Now;
			data.ModifyDate = DateTime.Now;
			data.ModifyById = UserAccount.ID;
			data.OrganizationId = UserAccount.OrganizationId;
			data.CompanyId = UserAccount.CompanyId;
			FolderHierarchy.Save(data);
			if (RecId > 0)
			{
				MessageBox.Show("Folder Update Successfully...");
			}
			else
			{
				MessageBox.Show("Folder Created Successfully...");
			}
			FolderName.Clear();
			treeView.Nodes.Clear();
			TreeViewFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCreateFolder_Click(object sender, EventArgs e)
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

	private void CreateFolderStructure_Load(object sender, EventArgs e)
	{
		try
		{
			ParentPanel.HorizontalScroll.Enabled = true;
			ParentPanel.HorizontalScroll.Visible = true;
			HeaderFolderPanel.Hide();
			TreeViewContainPanel.Dock = DockStyle.Fill;
			AllocationPanel.Hide();
			btnUnAllocateAllocatedDocuments.Visible = false;
			TreeViewFill();
			FolderInfoPanel.Hide();
			UnAllocatedDocumentsPanel.Hide();
			UsersForAllocationGridBind();
			TabControl.TabPages.Remove(FolderTab);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
			throw;
		}
	}

	private void TreeViewFill()
	{
		try
		{
			int OrganizationId = UserAccount.OrganizationId;
			int CompanyId = UserAccount.CompanyId;
			_ = UserAccount.ID;
			List<FolderHierarchy> list = new List<FolderHierarchy>();
			list = ((UserAccount.SupplierCustomerId <= 0) ? FolderHierarchy.GetAll(OrganizationId, CompanyId) : FolderHierarchy.ReadFoldersBySubUserId(OrganizationId, CompanyId, UserAccount.SupplierCustomerId));
			if (list.Count > 0)
			{
				PopulateTreeViewFromDb(list, 0L, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PopulateTreeViewFromDb(List<FolderHierarchy> lst, long parentId, TreeNode node)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			foreach (FolderHierarchy obj in lst)
			{
				TreeNode child = new TreeNode(obj.FolderName);
				child.Tag = obj;
				if (obj.ParentFolderHierarchyId == 0L)
				{
					treeView.Nodes.Add(child);
				}
				else
				{
					node?.Nodes.Add(child);
				}
				List<FolderHierarchy> list = FolderHierarchy.GetChildDataByParentId(((FolderHierarchy)child.Tag).FolderHierarchyId);
				PopulateTreeViewFromDb(list, ((FolderHierarchy)child.Tag).FolderHierarchyId, child);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UsersForAllocationGridBind()
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ID");
			dt.Columns.Add("UserName");
			dt.Columns.Add("PartyName");
			DataTable dtUser = UserAccount.ReadUserDataByUserTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, 2);
			if (dtUser.Rows.Count > 0)
			{
				for (int i = 0; i < dtUser.Rows.Count; i++)
				{
					dt.Rows.Add(dtUser.Rows[i]["ID"], dtUser.Rows[i]["UserName"], dtUser.Rows[i]["PartyName"]);
				}
				DDL.BindDDL(dt, CmbUserName, "ID", "UserName", "User Name", ZeroIndex: false);
				((UltraGridBase)CmbUserName).DisplayLayout.Bands[0].Columns[2].Hidden = true;
				GridUsersForAllocation.DataSource = dt;
				GridUsersForAllocation.RetrieveStructure();
				GridUsersForAllocation.RootTable.Columns["ID"].Visible = false;
				GridUsersForAllocation.RootTable.Columns["UserName"].Width = 100;
				GridUsersForAllocation.RootTable.Columns["PartyName"].Width = 200;
				GridUsersForAllocation.RootTable.Columns.Add("Select");
				GridUsersForAllocation.RootTable.Columns["Select"].ActAsSelector = true;
				GridUsersForAllocation.RootTable.Columns["Select"].UseHeaderSelector = true;
				GridUsersForAllocation.RootTable.Columns["Select"].Width = 30;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InsertSubFolder(FolderHierarchy obj, string SubFolderName)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			FolderHierarchy data = new FolderHierarchy();
			string path = obj.FolderPath + "\\" + SubFolderName;
			if (RecId > 0)
			{
				data.FolderHierarchyId = RecId;
			}
			else
			{
				data.FolderHierarchyId = 0L;
			}
			data.DriveName = obj.DriveName;
			data.ParentFolderHierarchyId = obj.FolderHierarchyId;
			int ParentFolderLevel = obj.FolderLevel;
			ParentFolderLevel = (data.FolderLevel = ParentFolderLevel + 1);
			data.FolderName = SubFolderName;
			data.FolderPath = path;
			data.FolderOwnerId = UserAccount.ID;
			data.CreatedById = UserAccount.ID;
			data.CreatedDate = DateTime.Now;
			data.ModifyDate = DateTime.Now;
			data.ModifyById = UserAccount.ID;
			data.OrganizationId = UserAccount.OrganizationId;
			data.CompanyId = UserAccount.CompanyId;
			FolderHierarchy.Save(data);
			if (RecId > 0)
			{
				MessageBox.Show("Folder Updated Successfully...");
			}
			else
			{
				MessageBox.Show("Folder Created Successfully...");
			}
			treeView.Nodes.Clear();
			TreeViewFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void treeView_MouseUp(object sender, MouseEventArgs e)
	{
		try
		{
			if (Conversion.ToInt((object)UserAccount.SupplierCustomerId) == 0 && e.Button == MouseButtons.Right)
			{
				Point ClickPoint = new Point(e.X, e.Y);
				if (treeView.GetNodeAt(ClickPoint) != null)
				{
					Point ScreenPoint = treeView.PointToScreen(ClickPoint);
					Point FormPoint = PointToClient(ScreenPoint);
					MenuStrip.Show(this, FormPoint);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddNewFolder_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Expected O, but got Unknown
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		try
		{
			SubFolderPopUp form = new SubFolderPopUp((FolderHierarchy)treeView.SelectedNode.Tag);
			form.ShowDialog();
			string SubFolderName = form.txtNodeName.Text;
			if (!(SubFolderName == ""))
			{
				form.Dispose();
				if (treeView.SelectedNode == null)
				{
					MessageBox.Show("No tree node selected. Please Select!");
				}
				else
				{
					InsertSubFolder((FolderHierarchy)treeView.SelectedNode.Tag, SubFolderName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUploadFileInSelectedFolder_Click(object sender, EventArgs e)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			DMSAttachmentsUpload dMSAttachmentsUpload = new DMSAttachmentsUpload(UserAccount);
			dMSAttachmentsUpload.FolderHierarchyId = ((FolderHierarchy)treeView.SelectedNode.Tag).FolderHierarchyId;
			dMSAttachmentsUpload.FolderPath = ((FolderHierarchy)treeView.SelectedNode.Tag).FolderPath;
			dMSAttachmentsUpload.ShowDialog();
			dMSAttachmentsUpload.BringToFront();
			treeView_AfterSelect(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			lblPathOfFolder.Text = ((FolderHierarchy)treeView.SelectedNode.Tag).FolderPath;
			lblTotalFiles.Text = "0";
			lblTotalSize.Text = "0";
			lblTotalUnAllocatedFilesCount.Text = "0";
			lblTotalUnAllocatedFilesSize.Text = "0";
			DocumentsFlowLayout.Controls.Clear();
			UnAllocatedDocumentsFlowLayout.Controls.Clear();
			DocFilePathIds.Clear();
			UnAllocatedDocFilePathIds.Clear();
			GrdDocuments.ClearStructure();
			GridUnAllocatedDocumets.ClearStructure();
			if (Conversion.ToInt(CmbUserName.Value) > 0)
			{
				DataTable dtAllocatedDocuments = FolderHierarchy.GetDocFilePathbyFolderId(UserAccount.OrganizationId, UserAccount.CompanyId, ((FolderHierarchy)treeView.SelectedNode.Tag).FolderHierarchyId, Conversion.ToInt(CmbUserName.Value));
				if (dtAllocatedDocuments.Rows.Count > 0)
				{
					FolderInfoPanel.Show();
					DocumnetsPanelInfo(dtAllocatedDocuments, "Allocated");
					lblTotalFiles.Text = Conversion.ToString((object)dtAllocatedDocuments.Rows.Count);
					TabControl.SelectedTab = DocumentsTab;
					GridFillForAllocatedAndUnAllocatedDocuments(GrdDocuments, dtAllocatedDocuments);
				}
				DataTable dtUnAllocatedDocuments = FolderHierarchy.GetUnAllocatedFilesbyFolderId(UserAccount.OrganizationId, UserAccount.CompanyId, ((FolderHierarchy)treeView.SelectedNode.Tag).FolderHierarchyId, Conversion.ToInt(CmbUserName.Value));
				if (dtUnAllocatedDocuments.Rows.Count > 0)
				{
					UnAllocatedDocumentsPanel.Show();
					DocumnetsPanelInfo(dtUnAllocatedDocuments, "UnAllocated");
					lblTotalUnAllocatedFilesCount.Text = Conversion.ToString((object)dtUnAllocatedDocuments.Rows.Count);
					UnAllocatedDocumentsTabControl.SelectedTab = CardsTab;
					GridFillForAllocatedAndUnAllocatedDocuments(GridUnAllocatedDocumets, dtUnAllocatedDocuments);
				}
			}
			else
			{
				DataTable dtDocuments = FolderHierarchy.GetDocFilePathbyFolderId(UserAccount.OrganizationId, UserAccount.CompanyId, ((FolderHierarchy)treeView.SelectedNode.Tag).FolderHierarchyId, UserAccount.ID);
				FolderInfoPanel.Hide();
				UnAllocatedDocumentsPanel.Hide();
				if (dtDocuments.Rows.Count > 0)
				{
					FolderInfoPanel.Show();
					DocumnetsPanelInfo(dtDocuments, "Allocated");
					lblTotalFiles.Text = Conversion.ToString((object)dtDocuments.Rows.Count);
					TabControl.SelectedTab = DocumentsTab;
					GridFillForAllocatedAndUnAllocatedDocuments(GrdDocuments, dtDocuments);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocumnetsPanelInfo(DataTable dtDocuments, string type)
	{
		try
		{
			double TotalFileSizeInMbs = 0.0;
			FolderHierarchyAttachments[] AttachmentCards = new FolderHierarchyAttachments[dtDocuments.Rows.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				AttachmentCards[i] = new FolderHierarchyAttachments();
				string path = Conversion.ToString(dtDocuments.Rows[i]["FilePath"]);
				AttachmentCards[i].DocFileId = Conversion.ToInt(dtDocuments.Rows[i]["DocFilePathId"]);
				if (type == "Allocated")
				{
					DocFilePathIds.Add(Conversion.ToInt(dtDocuments.Rows[i]["DocFilePathId"]));
				}
				else
				{
					UnAllocatedDocFilePathIds.Add(Conversion.ToInt(dtDocuments.Rows[i]["DocFilePathId"]));
				}
				AttachmentCards[i].FromUSer = "SubUser";
				AttachmentCards[i].path = path;
				FileInfo fileInfo = new FileInfo(path);
				AttachmentCards[i].FileName = fileInfo.Name;
				TotalFileSizeInMbs += (double)fileInfo.Length / 1048576.0;
				AttachmentCards[i].FileDate = fileInfo.CreationTime.ToShortDateString();
				AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.00") + " Mb";
				if (type == "Allocated")
				{
					DocumentsFlowLayout.Controls.Add(AttachmentCards[i]);
				}
				else
				{
					UnAllocatedDocumentsFlowLayout.Controls.Add(AttachmentCards[i]);
				}
				if (type == "Allocated")
				{
					AttachmentCards[i].UserControlClicked += UserControl_Click;
				}
				else
				{
					AttachmentCards[i].UserControlClicked += UserControlForUnAllocatedDocuments_Click;
				}
			}
			if (type == "Allocated")
			{
				lblTotalSize.Text = TotalFileSizeInMbs.ToString("0.000");
			}
			else
			{
				lblTotalUnAllocatedFilesSize.Text = TotalFileSizeInMbs.ToString("0.000");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridFillForAllocatedAndUnAllocatedDocuments(GridEX Grid, DataTable dt)
	{
		try
		{
			Grid.DataSource = dt;
			Grid.RetrieveStructure();
			Grid.RootTable.Columns["DocFilePathId"].Visible = false;
			Grid.RootTable.Columns["FilePath"].Visible = false;
			Grid.RootTable.Columns["CreatedDate"].Caption = "Upload Date";
			Grid.RootTable.Columns["CreatedDate"].Width = 60;
			Grid.RootTable.Columns["UserName"].Caption = "Upload User";
			Grid.RootTable.Columns["UserName"].Width = 80;
			Grid.RootTable.Columns["FileName"].Width = 250;
			Grid.RootTable.Columns["FileExtension"].Width = 50;
			Grid.RootTable.Columns["FileName"].ColumnType = (ColumnType)5;
			Grid.RootTable.Columns.Add("Select");
			Grid.RootTable.Columns["Select"].ActAsSelector = true;
			Grid.RootTable.Columns["Select"].UseHeaderSelector = true;
			Grid.RootTable.Columns["Select"].Width = 30;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			FolderHierarchyAttachments obj = (FolderHierarchyAttachments)sender;
			if (Conversion.ToString(obj.Tag) == "FormCheckBox")
			{
				if (!obj.IsSelected)
				{
					int id = 0;
					if (DocFilePathIds.Count > 0)
					{
						id = DocFilePathIds.Find((int Id) => Id == obj.DocFileId);
					}
					if (id > 0)
					{
						DocFilePathIds.Remove(id);
					}
					return;
				}
				int id2 = 0;
				if (DocFilePathIds.Count > 0)
				{
					id2 = DocFilePathIds.Find((int Id) => Id == obj.DocFileId);
				}
				if (id2 > 0)
				{
					DocFilePathIds.Remove(id2);
				}
				else
				{
					DocFilePathIds.Add(obj.DocFileId);
				}
			}
			else
			{
				string path = obj.path;
				if (File.Exists(path))
				{
					Process.Start(path);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UserControlForUnAllocatedDocuments_Click(object sender, EventArgs e)
	{
		try
		{
			FolderHierarchyAttachments obj = (FolderHierarchyAttachments)sender;
			if (Conversion.ToString(obj.Tag) == "FormCheckBox")
			{
				if (!obj.IsSelected)
				{
					int id = 0;
					if (UnAllocatedDocFilePathIds.Count > 0)
					{
						id = UnAllocatedDocFilePathIds.Find((int Id) => Id == obj.DocFileId);
					}
					if (id > 0)
					{
						UnAllocatedDocFilePathIds.Remove(id);
					}
					return;
				}
				int id2 = 0;
				if (UnAllocatedDocFilePathIds.Count > 0)
				{
					id2 = UnAllocatedDocFilePathIds.Find((int Id) => Id == obj.DocFileId);
				}
				if (id2 > 0)
				{
					UnAllocatedDocFilePathIds.Remove(id2);
				}
				else
				{
					UnAllocatedDocFilePathIds.Add(obj.DocFileId);
				}
			}
			else
			{
				string path = obj.path;
				if (File.Exists(path))
				{
					Process.Start(path);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAllocateUsers_Click(object sender, EventArgs e)
	{
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Expected O, but got Unknown
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Expected O, but got Unknown
		try
		{
			if (GridUsersForAllocation.GetCheckedRows().Count() == 0)
			{
				throw new Exception("Please select User first");
			}
			GridEXRow[] checkedRows;
			if (tabControl1.SelectedIndex == 0)
			{
				if (DocFilePathIds.Count <= 0)
				{
					throw new Exception("No File selected");
				}
				if (MessageBox.Show("Are you sure to Allocate These Files To Selected User's?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				DocFilePathAllocation obj = new DocFilePathAllocation();
				obj.DocFilePathAllocationslist = new List<DocFilePathAllocation>();
				checkedRows = GridUsersForAllocation.GetCheckedRows();
				foreach (GridEXRow r in checkedRows)
				{
					for (int i = 0; i < DocFilePathIds.Count; i++)
					{
						DocFilePathAllocation item = new DocFilePathAllocation();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.Id = 0L;
						item.SubUserId = Conversion.ToInt(r.Cells["ID"].Value);
						item.DocFilePathId = DocFilePathIds[i];
						item.EnteryUserId = UserAccount.ID;
						item.EnteryDate = DateTime.Now;
						obj.DocFilePathAllocationslist.Add(item);
					}
				}
				DocFilePathAllocation.Save(obj);
				MessageBox.Show("Files Allocated To User's Successfully");
				treeView_AfterSelect(null, null);
				return;
			}
			if (GrdDocuments.GetCheckedRows().Count() == 0)
			{
				throw new Exception("No File selected in Grid");
			}
			if (MessageBox.Show("Are you sure to Allocate These Files To Selected User's?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			DocFilePathAllocation obj2 = new DocFilePathAllocation();
			obj2.DocFilePathAllocationslist = new List<DocFilePathAllocation>();
			checkedRows = GridUsersForAllocation.GetCheckedRows();
			foreach (GridEXRow r2 in checkedRows)
			{
				GridEXRow[] rows = GrdDocuments.GetRows();
				foreach (GridEXRow doc in rows)
				{
					DocFilePathAllocation item2 = new DocFilePathAllocation();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.Id = 0L;
					item2.SubUserId = Conversion.ToInt(r2.Cells["ID"].Value);
					item2.DocFilePathId = Conversion.ToInt(doc.Cells["DocFilePathId"].Value);
					item2.EnteryUserId = UserAccount.ID;
					item2.EnteryDate = DateTime.Now;
					obj2.DocFilePathAllocationslist.Add(item2);
				}
			}
			DocFilePathAllocation.Save(obj2);
			MessageBox.Show("Files Allocated To User's Successfully");
			treeView_AfterSelect(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrdDocuments_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = GrdDocuments.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				string path = Conversion.ToString(item.Cells["FilePath"].Value);
				if (File.Exists(path))
				{
					Process.Start(path);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbUserName_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(CmbUserName.Value) > 0)
			{
				btnUnAllocateAllocatedDocuments.Visible = true;
			}
			else
			{
				btnUnAllocateAllocatedDocuments.Visible = false;
			}
			if (treeView.SelectedNode != null)
			{
				treeView_AfterSelect(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAllocateUnAllocatedDocuments_Click(object sender, EventArgs e)
	{
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Expected O, but got Unknown
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		try
		{
			if (UnAllocatedDocumentsTabControl.SelectedIndex == 0)
			{
				if (UnAllocatedDocFilePathIds.Count <= 0)
				{
					throw new Exception("No File selected");
				}
				if (MessageBox.Show("Are you sure to Allocate These Files To Selected User?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					DocFilePathAllocation obj = new DocFilePathAllocation();
					obj.DocFilePathAllocationslist = new List<DocFilePathAllocation>();
					for (int i = 0; i < UnAllocatedDocFilePathIds.Count; i++)
					{
						DocFilePathAllocation item = new DocFilePathAllocation();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.Id = 0L;
						item.SubUserId = Conversion.ToInt(CmbUserName.Value);
						item.DocFilePathId = UnAllocatedDocFilePathIds[i];
						item.EnteryUserId = UserAccount.ID;
						item.EnteryDate = DateTime.Now;
						obj.DocFilePathAllocationslist.Add(item);
					}
					DocFilePathAllocation.Save(obj);
					MessageBox.Show("Files Allocated To User Successfully");
					treeView_AfterSelect(null, null);
				}
				return;
			}
			if (GridUnAllocatedDocumets.GetCheckedRows().Count() == 0)
			{
				throw new Exception("No File selected in UnAllocated Grid");
			}
			if (MessageBox.Show("Are you sure to Allocate These Files To Selected User?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				DocFilePathAllocation obj2 = new DocFilePathAllocation();
				obj2.DocFilePathAllocationslist = new List<DocFilePathAllocation>();
				GridEXRow[] rows = GridUnAllocatedDocumets.GetRows();
				foreach (GridEXRow doc in rows)
				{
					DocFilePathAllocation item2 = new DocFilePathAllocation();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.Id = 0L;
					item2.SubUserId = Conversion.ToInt(CmbUserName.Value);
					item2.DocFilePathId = Conversion.ToInt(doc.Cells["DocFilePathId"].Value);
					item2.EnteryUserId = UserAccount.ID;
					item2.EnteryDate = DateTime.Now;
					obj2.DocFilePathAllocationslist.Add(item2);
				}
				DocFilePathAllocation.Save(obj2);
				MessageBox.Show("Files Allocated To User Successfully");
				treeView_AfterSelect(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUnAllocateAllocatedDocuments_Click(object sender, EventArgs e)
	{
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Expected O, but got Unknown
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Expected O, but got Unknown
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		try
		{
			if (UnAllocatedDocumentsTabControl.SelectedIndex == 0)
			{
				if (DocFilePathIds.Count <= 0)
				{
					throw new Exception("No File selected");
				}
				if (MessageBox.Show("Are you sure to De_Allocate These Files To Selected User?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					DocFilePathAllocation obj = new DocFilePathAllocation();
					obj.DocFilePathAllocationslist = new List<DocFilePathAllocation>();
					for (int i = 0; i < DocFilePathIds.Count; i++)
					{
						DocFilePathAllocation item = new DocFilePathAllocation();
						item.OrganizationId = UserAccount.OrganizationId;
						item.CompanyId = UserAccount.CompanyId;
						item.SubUserId = Conversion.ToInt(CmbUserName.Value);
						item.DocFilePathId = DocFilePathIds[i];
						obj.DocFilePathAllocationslist.Add(item);
					}
					DocFilePathAllocation.DeleteFileFromAllocationByUserId(obj);
					MessageBox.Show("Files De_Allocated To User Successfully");
					treeView_AfterSelect(null, null);
				}
				return;
			}
			if (GrdDocuments.GetCheckedRows().Count() == 0)
			{
				throw new Exception("No File selected in Allocated Grid");
			}
			if (MessageBox.Show("Are you sure to De_Allocate These Files To Selected User?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
			{
				DocFilePathAllocation obj2 = new DocFilePathAllocation();
				obj2.DocFilePathAllocationslist = new List<DocFilePathAllocation>();
				GridEXRow[] rows = GrdDocuments.GetRows();
				foreach (GridEXRow doc in rows)
				{
					DocFilePathAllocation item2 = new DocFilePathAllocation();
					item2.OrganizationId = UserAccount.OrganizationId;
					item2.CompanyId = UserAccount.CompanyId;
					item2.SubUserId = Conversion.ToInt(CmbUserName.Value);
					item2.DocFilePathId = Conversion.ToInt(doc.Cells["DocFilePathId"].Value);
					obj2.DocFilePathAllocationslist.Add(item2);
				}
				DocFilePathAllocation.DeleteFileFromAllocationByUserId(obj2);
				MessageBox.Show("Files De_Allocated To User Successfully");
				treeView_AfterSelect(null, null);
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
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
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
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Expected O, but got Unknown
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Expected O, but got Unknown
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_1a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a45: Expected O, but got Unknown
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.DMS_FolderHierarchy.FolderHierarchyForSubUser));
		GridEXLayout GridUnAllocatedDocumets_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GrdDocuments_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridUsersForAllocation_DesignTimeLayout = new GridEXLayout();
		Appearance appearance20 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		this.btnCreateFolder = new System.Windows.Forms.Button();
		this.FolderName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.treeView = new System.Windows.Forms.TreeView();
		this.imageList = new System.Windows.Forms.ImageList(this.components);
		this.MenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
		this.AddNewFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.btnUploadFileInSelectedFolder = new System.Windows.Forms.ToolStripMenuItem();
		this.ParentPanel = new System.Windows.Forms.Panel();
		this.UnAllocatedDocumentsPanel = new System.Windows.Forms.Panel();
		this.UnAllocatedDocumentsTabControl = new System.Windows.Forms.TabControl();
		this.CardsTab = new System.Windows.Forms.TabPage();
		this.UnAllocatedDocumentsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.btnAllocateUnAllocatedDocuments = new System.Windows.Forms.PictureBox();
		this.label10 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.lblTotalUnAllocatedFilesCount = new System.Windows.Forms.Label();
		this.lblTotalUnAllocatedFilesSize = new System.Windows.Forms.Label();
		this.GridTab = new System.Windows.Forms.TabPage();
		this.GridUnAllocatedDocumets = new GridEX();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.FolderInfoPanel = new System.Windows.Forms.Panel();
		this.panel2 = new System.Windows.Forms.Panel();
		this.TabControl = new System.Windows.Forms.TabControl();
		this.FolderTab = new System.Windows.Forms.TabPage();
		this.DocumentsTab = new System.Windows.Forms.TabPage();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.CardViewOfDocuments = new System.Windows.Forms.TabPage();
		this.DocumentsFlowLayout = new System.Windows.Forms.FlowLayoutPanel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.btnUnAllocateAllocatedDocuments = new System.Windows.Forms.PictureBox();
		this.label5 = new System.Windows.Forms.Label();
		this.label2 = new System.Windows.Forms.Label();
		this.lblTotalFiles = new System.Windows.Forms.Label();
		this.lblTotalSize = new System.Windows.Forms.Label();
		this.GridViewOfDocuments = new System.Windows.Forms.TabPage();
		this.GrdDocuments = new GridEX();
		this.panel1 = new System.Windows.Forms.Panel();
		this.label4 = new System.Windows.Forms.Label();
		this.lblPathOfFolder = new System.Windows.Forms.Label();
		this.TreeViewPanel = new System.Windows.Forms.Panel();
		this.AllocationPanel = new System.Windows.Forms.Panel();
		this.GridUsersForAllocation = new GridEX();
		this.ButtonPanelForAllocation = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.btnAllocateUsers = new System.Windows.Forms.PictureBox();
		this.label3 = new System.Windows.Forms.Label();
		this.TreeViewContainPanel = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.HeaderFolderPanel = new System.Windows.Forms.Panel();
		this.CmbUserName = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.MenuStrip.SuspendLayout();
		this.ParentPanel.SuspendLayout();
		this.UnAllocatedDocumentsPanel.SuspendLayout();
		this.UnAllocatedDocumentsTabControl.SuspendLayout();
		this.CardsTab.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnAllocateUnAllocatedDocuments).BeginInit();
		this.GridTab.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedDocumets).BeginInit();
		this.panel4.SuspendLayout();
		this.FolderInfoPanel.SuspendLayout();
		this.panel2.SuspendLayout();
		this.TabControl.SuspendLayout();
		this.DocumentsTab.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.CardViewOfDocuments.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnUnAllocateAllocatedDocuments).BeginInit();
		this.GridViewOfDocuments.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GrdDocuments).BeginInit();
		this.panel1.SuspendLayout();
		this.TreeViewPanel.SuspendLayout();
		this.AllocationPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridUsersForAllocation).BeginInit();
		this.ButtonPanelForAllocation.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnAllocateUsers).BeginInit();
		this.TreeViewContainPanel.SuspendLayout();
		this.HeaderFolderPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).BeginInit();
		base.SuspendLayout();
		this.btnCreateFolder.Location = new System.Drawing.Point(296, 8);
		this.btnCreateFolder.Name = "btnCreateFolder";
		this.btnCreateFolder.Size = new System.Drawing.Size(104, 23);
		this.btnCreateFolder.TabIndex = 1;
		this.btnCreateFolder.Text = "Create Root Folder";
		this.btnCreateFolder.UseVisualStyleBackColor = true;
		this.btnCreateFolder.Click += new System.EventHandler(btnCreateFolder_Click);
		this.FolderName.Location = new System.Drawing.Point(107, 9);
		this.FolderName.Name = "FolderName";
		this.FolderName.Size = new System.Drawing.Size(183, 20);
		this.FolderName.TabIndex = 0;
		this.label1.AutoSize = true;
		this.label1.Location = new System.Drawing.Point(34, 12);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(67, 13);
		this.label1.TabIndex = 3;
		this.label1.Text = "Folder Name";
		this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
		this.treeView.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.treeView.ImageIndex = 0;
		this.treeView.ImageList = this.imageList;
		this.treeView.Location = new System.Drawing.Point(0, 0);
		this.treeView.Name = "treeView";
		this.treeView.SelectedImageIndex = 0;
		this.treeView.Size = new System.Drawing.Size(323, 388);
		this.treeView.TabIndex = 0;
		this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(treeView_AfterSelect);
		this.treeView.MouseUp += new System.Windows.Forms.MouseEventHandler(treeView_MouseUp);
		this.imageList.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList.ImageStream");
		this.imageList.TransparentColor = System.Drawing.Color.Transparent;
		this.imageList.Images.SetKeyName(0, "FolderImage.jpg");
		this.MenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.AddNewFolder, this.btnUploadFileInSelectedFolder });
		this.MenuStrip.Name = "MenuStrip";
		this.MenuStrip.Size = new System.Drawing.Size(191, 48);
		this.AddNewFolder.Name = "AddNewFolder";
		this.AddNewFolder.Size = new System.Drawing.Size(190, 22);
		this.AddNewFolder.Text = "Add Child New Folder";
		this.AddNewFolder.Click += new System.EventHandler(AddNewFolder_Click);
		this.btnUploadFileInSelectedFolder.Name = "btnUploadFileInSelectedFolder";
		this.btnUploadFileInSelectedFolder.Size = new System.Drawing.Size(190, 22);
		this.btnUploadFileInSelectedFolder.Text = "Upload File";
		this.btnUploadFileInSelectedFolder.Click += new System.EventHandler(btnUploadFileInSelectedFolder_Click);
		this.ParentPanel.AutoScroll = true;
		this.ParentPanel.BackColor = System.Drawing.Color.White;
		this.ParentPanel.Controls.Add(this.UnAllocatedDocumentsPanel);
		this.ParentPanel.Controls.Add(this.FolderInfoPanel);
		this.ParentPanel.Controls.Add(this.TreeViewPanel);
		this.ParentPanel.Controls.Add(this.HeaderFolderPanel);
		this.ParentPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.ParentPanel.Location = new System.Drawing.Point(0, 0);
		this.ParentPanel.Name = "ParentPanel";
		this.ParentPanel.Size = new System.Drawing.Size(1574, 1006);
		this.ParentPanel.TabIndex = 10;
		this.UnAllocatedDocumentsPanel.Controls.Add(this.UnAllocatedDocumentsTabControl);
		this.UnAllocatedDocumentsPanel.Controls.Add(this.panel4);
		this.UnAllocatedDocumentsPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.UnAllocatedDocumentsPanel.Location = new System.Drawing.Point(939, 39);
		this.UnAllocatedDocumentsPanel.Name = "UnAllocatedDocumentsPanel";
		this.UnAllocatedDocumentsPanel.Size = new System.Drawing.Size(608, 967);
		this.UnAllocatedDocumentsPanel.TabIndex = 16;
		this.UnAllocatedDocumentsTabControl.Controls.Add(this.CardsTab);
		this.UnAllocatedDocumentsTabControl.Controls.Add(this.GridTab);
		this.UnAllocatedDocumentsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.UnAllocatedDocumentsTabControl.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.UnAllocatedDocumentsTabControl.Location = new System.Drawing.Point(0, 61);
		this.UnAllocatedDocumentsTabControl.Name = "UnAllocatedDocumentsTabControl";
		this.UnAllocatedDocumentsTabControl.SelectedIndex = 0;
		this.UnAllocatedDocumentsTabControl.Size = new System.Drawing.Size(608, 906);
		this.UnAllocatedDocumentsTabControl.TabIndex = 1;
		this.CardsTab.Controls.Add(this.UnAllocatedDocumentsFlowLayout);
		this.CardsTab.Controls.Add(this.panel6);
		this.CardsTab.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CardsTab.Location = new System.Drawing.Point(4, 26);
		this.CardsTab.Name = "CardsTab";
		this.CardsTab.Padding = new System.Windows.Forms.Padding(3);
		this.CardsTab.Size = new System.Drawing.Size(600, 876);
		this.CardsTab.TabIndex = 0;
		this.CardsTab.Text = "Card View";
		this.CardsTab.UseVisualStyleBackColor = true;
		this.UnAllocatedDocumentsFlowLayout.AutoScroll = true;
		this.UnAllocatedDocumentsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.UnAllocatedDocumentsFlowLayout.Location = new System.Drawing.Point(3, 44);
		this.UnAllocatedDocumentsFlowLayout.Name = "UnAllocatedDocumentsFlowLayout";
		this.UnAllocatedDocumentsFlowLayout.Size = new System.Drawing.Size(594, 829);
		this.UnAllocatedDocumentsFlowLayout.TabIndex = 2;
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.btnAllocateUnAllocatedDocuments);
		this.panel6.Controls.Add(this.label10);
		this.panel6.Controls.Add(this.label11);
		this.panel6.Controls.Add(this.lblTotalUnAllocatedFilesCount);
		this.panel6.Controls.Add(this.lblTotalUnAllocatedFilesSize);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(594, 41);
		this.panel6.TabIndex = 1;
		this.btnAllocateUnAllocatedDocuments.BackColor = System.Drawing.Color.White;
		this.btnAllocateUnAllocatedDocuments.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAllocateUnAllocatedDocuments.Image = Architecture.WinApp.Properties.Resources.arrow_left_line;
		this.btnAllocateUnAllocatedDocuments.Location = new System.Drawing.Point(14, 8);
		this.btnAllocateUnAllocatedDocuments.Name = "btnAllocateUnAllocatedDocuments";
		this.btnAllocateUnAllocatedDocuments.Size = new System.Drawing.Size(33, 26);
		this.btnAllocateUnAllocatedDocuments.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btnAllocateUnAllocatedDocuments.TabIndex = 4;
		this.btnAllocateUnAllocatedDocuments.TabStop = false;
		this.btnAllocateUnAllocatedDocuments.Click += new System.EventHandler(btnAllocateUnAllocatedDocuments_Click);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.ForeColor = System.Drawing.Color.White;
		this.label10.Location = new System.Drawing.Point(285, 8);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(157, 25);
		this.label10.TabIndex = 3;
		this.label10.Text = "Total Size (Mbs):";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(83, 8);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(103, 25);
		this.label11.TabIndex = 0;
		this.label11.Text = "Total Files:";
		this.lblTotalUnAllocatedFilesCount.AutoSize = true;
		this.lblTotalUnAllocatedFilesCount.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalUnAllocatedFilesCount.ForeColor = System.Drawing.Color.White;
		this.lblTotalUnAllocatedFilesCount.Location = new System.Drawing.Point(189, 8);
		this.lblTotalUnAllocatedFilesCount.Name = "lblTotalUnAllocatedFilesCount";
		this.lblTotalUnAllocatedFilesCount.Size = new System.Drawing.Size(23, 25);
		this.lblTotalUnAllocatedFilesCount.TabIndex = 2;
		this.lblTotalUnAllocatedFilesCount.Text = "0";
		this.lblTotalUnAllocatedFilesSize.AutoSize = true;
		this.lblTotalUnAllocatedFilesSize.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalUnAllocatedFilesSize.ForeColor = System.Drawing.Color.White;
		this.lblTotalUnAllocatedFilesSize.Location = new System.Drawing.Point(454, 8);
		this.lblTotalUnAllocatedFilesSize.Name = "lblTotalUnAllocatedFilesSize";
		this.lblTotalUnAllocatedFilesSize.Size = new System.Drawing.Size(23, 25);
		this.lblTotalUnAllocatedFilesSize.TabIndex = 1;
		this.lblTotalUnAllocatedFilesSize.Text = "0";
		this.GridTab.Controls.Add((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets);
		this.GridTab.Location = new System.Drawing.Point(4, 26);
		this.GridTab.Name = "GridTab";
		this.GridTab.Padding = new System.Windows.Forms.Padding(3);
		this.GridTab.Size = new System.Drawing.Size(600, 876);
		this.GridTab.TabIndex = 1;
		this.GridTab.Text = "Grid View";
		this.GridTab.UseVisualStyleBackColor = true;
		this.GridUnAllocatedDocumets.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridUnAllocatedDocumets).BackColor = System.Drawing.Color.White;
		this.GridUnAllocatedDocumets.ColumnAutoResize = true;
		GridUnAllocatedDocumets_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridUnAllocatedDocumets.DesignTimeLayout = GridUnAllocatedDocumets_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUnAllocatedDocumets.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets).Name = "GridUnAllocatedDocumets";
		((ScrollableBase)this.GridUnAllocatedDocumets).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets).Size = new System.Drawing.Size(594, 870);
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.GridUnAllocatedDocumets).TabStop = false;
		this.GridUnAllocatedDocumets.TotalRow = (InheritableBoolean)1;
		this.GridUnAllocatedDocumets.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUnAllocatedDocumets).VisualStyle = (VisualStyle)4;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label9);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 0);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(608, 61);
		this.panel4.TabIndex = 0;
		this.label9.Dock = System.Windows.Forms.DockStyle.Top;
		this.label9.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(608, 61);
		this.label9.TabIndex = 1;
		this.label9.Text = "Un Allocated Documents";
		this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.FolderInfoPanel.Controls.Add(this.panel2);
		this.FolderInfoPanel.Controls.Add(this.panel1);
		this.FolderInfoPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.FolderInfoPanel.Location = new System.Drawing.Point(323, 39);
		this.FolderInfoPanel.Name = "FolderInfoPanel";
		this.FolderInfoPanel.Size = new System.Drawing.Size(616, 967);
		this.FolderInfoPanel.TabIndex = 15;
		this.panel2.Controls.Add(this.TabControl);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(0, 33);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(616, 934);
		this.panel2.TabIndex = 5;
		this.TabControl.Controls.Add(this.FolderTab);
		this.TabControl.Controls.Add(this.DocumentsTab);
		this.TabControl.Dock = System.Windows.Forms.DockStyle.Fill;
		this.TabControl.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.TabControl.Location = new System.Drawing.Point(0, 0);
		this.TabControl.Name = "TabControl";
		this.TabControl.SelectedIndex = 0;
		this.TabControl.Size = new System.Drawing.Size(616, 934);
		this.TabControl.TabIndex = 3;
		this.FolderTab.Location = new System.Drawing.Point(4, 26);
		this.FolderTab.Name = "FolderTab";
		this.FolderTab.Padding = new System.Windows.Forms.Padding(3);
		this.FolderTab.Size = new System.Drawing.Size(608, 904);
		this.FolderTab.TabIndex = 0;
		this.FolderTab.Text = "Folders";
		this.FolderTab.UseVisualStyleBackColor = true;
		this.DocumentsTab.Controls.Add(this.tabControl1);
		this.DocumentsTab.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocumentsTab.Location = new System.Drawing.Point(4, 26);
		this.DocumentsTab.Name = "DocumentsTab";
		this.DocumentsTab.Padding = new System.Windows.Forms.Padding(3);
		this.DocumentsTab.Size = new System.Drawing.Size(608, 904);
		this.DocumentsTab.TabIndex = 1;
		this.DocumentsTab.Text = "Allocatd Documents";
		this.DocumentsTab.UseVisualStyleBackColor = true;
		this.tabControl1.Controls.Add(this.CardViewOfDocuments);
		this.tabControl1.Controls.Add(this.GridViewOfDocuments);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(3, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(602, 898);
		this.tabControl1.TabIndex = 0;
		this.CardViewOfDocuments.Controls.Add(this.DocumentsFlowLayout);
		this.CardViewOfDocuments.Controls.Add(this.panel3);
		this.CardViewOfDocuments.Location = new System.Drawing.Point(4, 26);
		this.CardViewOfDocuments.Name = "CardViewOfDocuments";
		this.CardViewOfDocuments.Padding = new System.Windows.Forms.Padding(3);
		this.CardViewOfDocuments.Size = new System.Drawing.Size(594, 868);
		this.CardViewOfDocuments.TabIndex = 0;
		this.CardViewOfDocuments.Text = "Card View";
		this.CardViewOfDocuments.UseVisualStyleBackColor = true;
		this.DocumentsFlowLayout.AutoScroll = true;
		this.DocumentsFlowLayout.Dock = System.Windows.Forms.DockStyle.Fill;
		this.DocumentsFlowLayout.Location = new System.Drawing.Point(3, 44);
		this.DocumentsFlowLayout.Name = "DocumentsFlowLayout";
		this.DocumentsFlowLayout.Size = new System.Drawing.Size(588, 821);
		this.DocumentsFlowLayout.TabIndex = 2;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.btnUnAllocateAllocatedDocuments);
		this.panel3.Controls.Add(this.label5);
		this.panel3.Controls.Add(this.label2);
		this.panel3.Controls.Add(this.lblTotalFiles);
		this.panel3.Controls.Add(this.lblTotalSize);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(588, 41);
		this.panel3.TabIndex = 1;
		this.btnUnAllocateAllocatedDocuments.BackColor = System.Drawing.Color.White;
		this.btnUnAllocateAllocatedDocuments.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnUnAllocateAllocatedDocuments.Image = Architecture.WinApp.Properties.Resources.arrow_right_line;
		this.btnUnAllocateAllocatedDocuments.Location = new System.Drawing.Point(541, 8);
		this.btnUnAllocateAllocatedDocuments.Name = "btnUnAllocateAllocatedDocuments";
		this.btnUnAllocateAllocatedDocuments.Size = new System.Drawing.Size(33, 26);
		this.btnUnAllocateAllocatedDocuments.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.btnUnAllocateAllocatedDocuments.TabIndex = 5;
		this.btnUnAllocateAllocatedDocuments.TabStop = false;
		this.btnUnAllocateAllocatedDocuments.Click += new System.EventHandler(btnUnAllocateAllocatedDocuments_Click);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.White;
		this.label5.Location = new System.Drawing.Point(262, 8);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(157, 25);
		this.label5.TabIndex = 3;
		this.label5.Text = "Total Size (Mbs):";
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(60, 8);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(103, 25);
		this.label2.TabIndex = 0;
		this.label2.Text = "Total Files:";
		this.lblTotalFiles.AutoSize = true;
		this.lblTotalFiles.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalFiles.ForeColor = System.Drawing.Color.White;
		this.lblTotalFiles.Location = new System.Drawing.Point(166, 8);
		this.lblTotalFiles.Name = "lblTotalFiles";
		this.lblTotalFiles.Size = new System.Drawing.Size(23, 25);
		this.lblTotalFiles.TabIndex = 2;
		this.lblTotalFiles.Text = "0";
		this.lblTotalSize.AutoSize = true;
		this.lblTotalSize.Font = new System.Drawing.Font("Segoe UI", 14.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalSize.ForeColor = System.Drawing.Color.White;
		this.lblTotalSize.Location = new System.Drawing.Point(431, 8);
		this.lblTotalSize.Name = "lblTotalSize";
		this.lblTotalSize.Size = new System.Drawing.Size(23, 25);
		this.lblTotalSize.TabIndex = 1;
		this.lblTotalSize.Text = "0";
		this.GridViewOfDocuments.Controls.Add((System.Windows.Forms.Control)(object)this.GrdDocuments);
		this.GridViewOfDocuments.Location = new System.Drawing.Point(4, 26);
		this.GridViewOfDocuments.Name = "GridViewOfDocuments";
		this.GridViewOfDocuments.Padding = new System.Windows.Forms.Padding(3);
		this.GridViewOfDocuments.Size = new System.Drawing.Size(594, 868);
		this.GridViewOfDocuments.TabIndex = 1;
		this.GridViewOfDocuments.Text = "Grid View";
		this.GridViewOfDocuments.UseVisualStyleBackColor = true;
		this.GrdDocuments.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GrdDocuments).BackColor = System.Drawing.Color.White;
		this.GrdDocuments.ColumnAutoResize = true;
		this.GrdDocuments.DefaultFilterRowComparison = (FilterConditionOperator)7;
		GrdDocuments_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GrdDocuments.DesignTimeLayout = GrdDocuments_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GrdDocuments).Dock = System.Windows.Forms.DockStyle.Fill;
		this.GrdDocuments.FilterMode = (FilterMode)2;
		this.GrdDocuments.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GrdDocuments).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GrdDocuments.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GrdDocuments).Location = new System.Drawing.Point(3, 3);
		((System.Windows.Forms.Control)(object)this.GrdDocuments).Name = "GrdDocuments";
		((ScrollableBase)this.GrdDocuments).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GrdDocuments).Size = new System.Drawing.Size(588, 862);
		((System.Windows.Forms.Control)(object)this.GrdDocuments).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.GrdDocuments).TabStop = false;
		this.GrdDocuments.TotalRow = (InheritableBoolean)1;
		this.GrdDocuments.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GrdDocuments).VisualStyle = (VisualStyle)4;
		this.GrdDocuments.LinkClicked += new ColumnActionEventHandler(GrdDocuments_LinkClicked);
		this.panel1.Controls.Add(this.label4);
		this.panel1.Controls.Add(this.lblPathOfFolder);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(616, 33);
		this.panel1.TabIndex = 4;
		this.label4.BackColor = System.Drawing.Color.Silver;
		this.label4.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.CadetBlue;
		this.label4.Location = new System.Drawing.Point(2, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(52, 32);
		this.label4.TabIndex = 1;
		this.label4.Text = "Path: ";
		this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.label4.Visible = false;
		this.lblPathOfFolder.AutoEllipsis = true;
		this.lblPathOfFolder.BackColor = System.Drawing.Color.White;
		this.lblPathOfFolder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.lblPathOfFolder.Font = new System.Drawing.Font("Segoe UI", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.lblPathOfFolder.ForeColor = System.Drawing.Color.CadetBlue;
		this.lblPathOfFolder.Location = new System.Drawing.Point(55, 0);
		this.lblPathOfFolder.Name = "lblPathOfFolder";
		this.lblPathOfFolder.Size = new System.Drawing.Size(559, 32);
		this.lblPathOfFolder.TabIndex = 2;
		this.lblPathOfFolder.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.lblPathOfFolder.Visible = false;
		this.TreeViewPanel.Controls.Add(this.AllocationPanel);
		this.TreeViewPanel.Controls.Add(this.TreeViewContainPanel);
		this.TreeViewPanel.Controls.Add(this.label6);
		this.TreeViewPanel.Dock = System.Windows.Forms.DockStyle.Left;
		this.TreeViewPanel.Location = new System.Drawing.Point(0, 39);
		this.TreeViewPanel.Name = "TreeViewPanel";
		this.TreeViewPanel.Size = new System.Drawing.Size(323, 967);
		this.TreeViewPanel.TabIndex = 14;
		this.AllocationPanel.Controls.Add((System.Windows.Forms.Control)(object)this.GridUsersForAllocation);
		this.AllocationPanel.Controls.Add(this.ButtonPanelForAllocation);
		this.AllocationPanel.Controls.Add(this.label3);
		this.AllocationPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.AllocationPanel.Location = new System.Drawing.Point(0, 421);
		this.AllocationPanel.Name = "AllocationPanel";
		this.AllocationPanel.Size = new System.Drawing.Size(323, 546);
		this.AllocationPanel.TabIndex = 16;
		this.GridUsersForAllocation.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridUsersForAllocation).BackColor = System.Drawing.Color.White;
		this.GridUsersForAllocation.ColumnAutoResize = true;
		GridUsersForAllocation_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridUsersForAllocation.DesignTimeLayout = GridUsersForAllocation_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridUsersForAllocation).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.GridUsersForAllocation).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridUsersForAllocation.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.GridUsersForAllocation).Location = new System.Drawing.Point(0, 61);
		((System.Windows.Forms.Control)(object)this.GridUsersForAllocation).Name = "GridUsersForAllocation";
		((ScrollableBase)this.GridUsersForAllocation).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridUsersForAllocation).Size = new System.Drawing.Size(323, 485);
		((System.Windows.Forms.Control)(object)this.GridUsersForAllocation).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.GridUsersForAllocation).TabStop = false;
		this.GridUsersForAllocation.TotalRow = (InheritableBoolean)1;
		this.GridUsersForAllocation.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridUsersForAllocation).VisualStyle = (VisualStyle)4;
		this.ButtonPanelForAllocation.Controls.Add(this.label7);
		this.ButtonPanelForAllocation.Controls.Add(this.btnAllocateUsers);
		this.ButtonPanelForAllocation.Dock = System.Windows.Forms.DockStyle.Top;
		this.ButtonPanelForAllocation.Location = new System.Drawing.Point(0, 33);
		this.ButtonPanelForAllocation.Name = "ButtonPanelForAllocation";
		this.ButtonPanelForAllocation.Size = new System.Drawing.Size(323, 28);
		this.ButtonPanelForAllocation.TabIndex = 1;
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(34, 5);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(60, 18);
		this.label7.TabIndex = 1;
		this.label7.Text = "Allocate";
		this.btnAllocateUsers.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnAllocateUsers.Image = Architecture.WinApp.Properties.Resources.check_line;
		this.btnAllocateUsers.Location = new System.Drawing.Point(5, 3);
		this.btnAllocateUsers.Name = "btnAllocateUsers";
		this.btnAllocateUsers.Size = new System.Drawing.Size(23, 22);
		this.btnAllocateUsers.TabIndex = 0;
		this.btnAllocateUsers.TabStop = false;
		this.btnAllocateUsers.Click += new System.EventHandler(btnAllocateUsers_Click);
		this.label3.BackColor = System.Drawing.Color.Teal;
		this.label3.Dock = System.Windows.Forms.DockStyle.Top;
		this.label3.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(0, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(323, 33);
		this.label3.TabIndex = 0;
		this.label3.Text = "Customer Allocation";
		this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.TreeViewContainPanel.AutoScroll = true;
		this.TreeViewContainPanel.Controls.Add(this.treeView);
		this.TreeViewContainPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.TreeViewContainPanel.Location = new System.Drawing.Point(0, 33);
		this.TreeViewContainPanel.Name = "TreeViewContainPanel";
		this.TreeViewContainPanel.Size = new System.Drawing.Size(323, 388);
		this.TreeViewContainPanel.TabIndex = 2;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Dock = System.Windows.Forms.DockStyle.Top;
		this.label6.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(0, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(323, 33);
		this.label6.TabIndex = 1;
		this.label6.Text = "Folders";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.HeaderFolderPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbUserName);
		this.HeaderFolderPanel.Controls.Add(this.label8);
		this.HeaderFolderPanel.Controls.Add(this.FolderName);
		this.HeaderFolderPanel.Controls.Add(this.label1);
		this.HeaderFolderPanel.Controls.Add(this.btnCreateFolder);
		this.HeaderFolderPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.HeaderFolderPanel.Location = new System.Drawing.Point(0, 0);
		this.HeaderFolderPanel.Name = "HeaderFolderPanel";
		this.HeaderFolderPanel.Size = new System.Drawing.Size(1574, 39);
		this.HeaderFolderPanel.TabIndex = 13;
		this.CmbUserName.AllowNull = (DefaultableBoolean)1;
		this.CmbUserName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbUserName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbUserName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbUserName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance21).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbUserName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbUserName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		this.CmbUserName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Location = new System.Drawing.Point(543, 7);
		((System.Windows.Forms.Control)(object)this.CmbUserName).Name = "CmbUserName";
		((System.Windows.Forms.Control)(object)this.CmbUserName).Size = new System.Drawing.Size(212, 22);
		((System.Windows.Forms.Control)(object)this.CmbUserName).TabIndex = 13;
		((System.Windows.Forms.Control)(object)this.CmbUserName).Leave += new System.EventHandler(CmbUserName_Leave);
		this.label8.AutoSize = true;
		this.label8.Location = new System.Drawing.Point(475, 11);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(60, 13);
		this.label8.TabIndex = 14;
		this.label8.Text = "User Name";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		base.ClientSize = new System.Drawing.Size(1574, 1006);
		base.Controls.Add(this.ParentPanel);
		base.Name = "FolderHierarchyForSubUser";
		this.Text = "CreateFolderStructure";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(CreateFolderStructure_Load);
		this.MenuStrip.ResumeLayout(false);
		this.ParentPanel.ResumeLayout(false);
		this.UnAllocatedDocumentsPanel.ResumeLayout(false);
		this.UnAllocatedDocumentsTabControl.ResumeLayout(false);
		this.CardsTab.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnAllocateUnAllocatedDocuments).EndInit();
		this.GridTab.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUnAllocatedDocumets).EndInit();
		this.panel4.ResumeLayout(false);
		this.FolderInfoPanel.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.TabControl.ResumeLayout(false);
		this.DocumentsTab.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.CardViewOfDocuments.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnUnAllocateAllocatedDocuments).EndInit();
		this.GridViewOfDocuments.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GrdDocuments).EndInit();
		this.panel1.ResumeLayout(false);
		this.TreeViewPanel.ResumeLayout(false);
		this.AllocationPanel.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.GridUsersForAllocation).EndInit();
		this.ButtonPanelForAllocation.ResumeLayout(false);
		this.ButtonPanelForAllocation.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnAllocateUsers).EndInit();
		this.TreeViewContainPanel.ResumeLayout(false);
		this.HeaderFolderPanel.ResumeLayout(false);
		this.HeaderFolderPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbUserName).EndInit();
		base.ResumeLayout(false);
	}
}
