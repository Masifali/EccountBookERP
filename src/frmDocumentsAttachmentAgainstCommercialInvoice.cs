using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Export;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.VPS;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.DynamicCards;
using Architecture.WinApp.Helper;
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
namespace Architecture.WinApp.DashBoard
{

public class frmDocumentsAttachmentAgainstCommercialInvoice : Form
{
	private bool FormInitialized;

	private int RecId;

	private bool IsVpsAttachmentsServiceOn;

	private ServerCredentials Sc = new ServerCredentials();

	private string ScreenName = "frmDocumentsAttachmentAgainstCommercialInvoice";

	private DataSet dtAllData = new DataSet();

	private DataTable dtSummary = new DataTable();

	private DataTable dtDetail = new DataTable();

	private DataTable dtAttachment = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private int SelectedDocumentId;

	private int SelectedDocumentDocumentTypeId;

	private string SelectedDocumentScreenName = "";

	private IContainer components;

	private Panel panel1;

	private Panel panel3;

	private Panel panel5;

	private Panel panel6;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar1;

	internal GridEX grdSummary;

	private Panel panel4;

	private GroupBox groupBox1;

	private Button btnShowData;

	private Label label34;

	internal UltraCombo CmbInvoiceNo;

	private Label label2;

	private Panel panel2;

	private Label label16;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private ToolStripButton toolStripButton1;

	private ToolStripButton BtnShortCutkeys;

	private ToolStrip miniToolStrip;

	private Panel panel9;

	private Panel panel10;

	private Label label1;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdDetail;

	private Panel panel12;

	private Panel panel13;

	private Panel panel14;

	private Button btnSave;

	private Button Choose;

	private Label label3;

	private Panel panel15;

	private Label label5;

	private Label lblTotalFiles;

	private Label lblTotalSize;

	private Label label4;

	private FlowLayoutPanel AttachmentsFlowLayoutPanel;

	public frmDocumentsAttachmentAgainstCommercialInvoice(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
		List<UserAccountAllocation> userAccountAllocationsList = user.UserAccountAllocationsList;
		if (userAccountAllocationsList != null && userAccountAllocationsList.Count > 0)
		{
			ScreenName = user.UserAccountAllocationsList[0].ScreenName;
		}
		InitializeComponentCustom();
		InitializeComponentMethod();
		grdSummary.SelectionChanged += grdSummary_SelectionChanged;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			dtSummary.Columns.Add("DocumentType", typeof(string));
			dtSummary.Columns.Add("ScreenName", typeof(string));
			dtSummary.Columns.Add("DocumentTypeId", typeof(int));
			dtSummary.Columns.Add("TotalAttachments", typeof(int));
			dtDetail.Columns.Add("Id", typeof(int));
			dtDetail.Columns.Add("DocumentTypeId", typeof(int));
			dtDetail.Columns.Add("DocumentType", typeof(string));
			dtDetail.Columns.Add("ScreenName", typeof(string));
			dtDetail.Columns.Add("DocNo", typeof(string));
			dtDetail.Columns.Add("DocDate", typeof(DateTime));
			dtDetail.Columns.Add("NoOfAttachments", typeof(int));
			dtAttachment.Columns.Add("Id");
			dtAttachment.Columns.Add("AttachmentName");
			dtAttachment.Columns.Add("CustomName");
			dtAttachment.Columns.Add("EntryDate", typeof(DateTime));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			return new
			{
				InvoiceComboData = InvoiceComboDbCall()
			};
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				InvoiceComboBind(result.InvoiceComboData);
				IsVpsAttachmentsServiceOn = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("IsVpsAttachmentsServiceOn"));
				BindGrids();
				FormInitialized = true;
				((Control)(object)CmbInvoiceNo).Enabled = true;
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void InvFrmGRN_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(980, 700);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable InvoiceComboDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected O, but got Unknown
		try
		{
			return ExImLcOrder.GetDataForDropDownFromExportInvoice(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Activity = "Invoice",
				DocumentTypeIds = "204"
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void InvoiceComboBind(DataTable dtcombo)
	{
		try
		{
			InfragisticsHelper.BindAndRetainSelection(CmbInvoiceNo, dtcombo, "Id", "name", "Invoice No", AllColumns: false, Conversion.ToInt(CmbInvoiceNo.Value), insertDefaultRow: true, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			((Control)(object)CmbInvoiceNo).Text = string.Empty;
			grdSummary.DataSource = null;
			grdDetail.DataSource = null;
			AttachmentsFlowLayoutPanel.Controls.Clear();
			panel14.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GrnPackingMaterial_FormClosing(object sender, FormClosingEventArgs e)
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

	private void BindGrids()
	{
	}

	private void InvFrmGRN_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.KeyCode == Keys.N)
			{
				btnNew_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.R)
			{
				toolStripButton1_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.S)
			{
				btnShowData_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				((Control)(object)grdSummary).Focus();
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				CmbInvoiceNo.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				CmbInvoiceNo.Focus();
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdSummary, dtSummary);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
		try
		{
			InvoiceComboBind(InvoiceComboDbCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowData_Click(object sender, EventArgs e)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		try
		{
			if (((UltraGridBase)CmbInvoiceNo).ActiveRow == null || Conversion.ToInt(CmbInvoiceNo.Value) == 0)
			{
				CmbInvoiceNo.Focus();
				throw new Exception("Please select Invoice No.");
			}
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.InvoiceId = Conversion.ToInt(CmbInvoiceNo.Value);
			dtAllData = Dashboard.GetAllDocumentsAttachmentsByInvoiceId(obj);
			var list = (from r in dtAllData.Tables[0].AsEnumerable()
				group r by (Conversion.ToInt(r["DocumentTypeId"]) <= 0) ? Conversion.ToString(r["ScreenName"]) : Conversion.ToString(r["DocumentType"]) into g
				select new
				{
					GroupKey = g.Key,
					DocumentType = Conversion.ToString(g.First()["DocumentType"]),
					ScreenName = Conversion.ToString(g.First()["ScreenName"]),
					DocumentTypeId = Conversion.ToInt(g.First()["DocumentTypeId"]),
					TotalAttachments = g.Sum((DataRow r) => Conversion.ToInt(r["NoOfAttachments"]))
				} into x
				orderby x.GroupKey
				select x).ToList();
			dtSummary.Rows.Clear();
			foreach (var item in list)
			{
				dtSummary.Rows.Add(item.DocumentType, item.ScreenName, item.DocumentTypeId, item.TotalAttachments);
			}
			grdSummary.DataSource = dtSummary;
			grdSummary.RetrieveStructure();
			grdSummarySettings();
			if (grdSummary.RowCount > 0)
			{
				grdSummary.MoveTo(0);
				((Control)(object)grdSummary).Focus();
				ShowDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSummarySettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "DocumentTypeId", "ScreenName" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdSummary, 2, 0, hiddenColumns);
			grdSummary.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType + 45;
			grdSummary.RootTable.Columns["TotalAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 5;
			ctrlGrdBar1_Load(null, null);
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
			dt.Rows.Add("Ctrl+S", "For show data");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Invoice");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Summary Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Invoice No");
			ShortCutKeyPopUp shortCutKeyPopUp = new ShortCutKeyPopUp(dt);
			shortCutKeyPopUp.StartPosition = FormStartPosition.CenterScreen;
			shortCutKeyPopUp.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSummary_SelectionChanged(object sender, EventArgs e)
	{
		ShowDetail();
	}

	private void grdSummary_GotFocus(object sender, EventArgs e)
	{
		ShowDetail();
	}

	private void grdSummary_Click(object sender, EventArgs e)
	{
		try
		{
			ShowDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdSummary_KeyUp(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
		{
			ShowDetail();
		}
	}

	private void ShowDetail()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdSummary.CurrentRow;
			if (r == null || (int)r.RowType != 0 || r.Cells.Count == 0)
			{
				return;
			}
			grdSummary.MoveTo(r.Position);
			grdSummary.HideSelection = (HideSelection)2;
			r.CheckState = (RowCheckState)0;
			int documentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			string screenName = Conversion.ToString(r.Cells["ScreenName"].Value);
			DataTable dtData = dtAllData.Tables[0].Copy();
			if (dtData.Rows.Count <= 0)
			{
				return;
			}
			IEnumerable<DataRow> detailRows = ((documentTypeId <= 0) ? (from dr in dtData.AsEnumerable()
				where Conversion.ToString(dr["ScreenName"]) == screenName
				select dr) : (from dr in dtData.AsEnumerable()
				where Conversion.ToInt(dr["DocumentTypeId"]) == documentTypeId
				select dr));
			dtDetail.Rows.Clear();
			foreach (DataRow dr2 in detailRows)
			{
				dtDetail.Rows.Add(dr2["Id"], dr2["DocumentTypeId"], dr2["DocumentType"], dr2["ScreenName"], dr2["DocNo"], dr2["DocDate"], dr2["NoOfAttachments"]);
			}
			if (dtDetail.Rows.Count == 0)
			{
				MessageBox.Show("No detail found for selected row.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
				return;
			}
			grdDetail.DataSource = dtDetail;
			grdDetail.RetrieveStructure();
			List<string> hiddenColumns = new List<string> { "Id", "DocumentTypeId", "DocumentType", "ScreenName" };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 0, hiddenColumns);
			grdDetail.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.InvoiceNoConstant + 50;
			grdDetail.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 20;
			grdDetail.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			grdDetail.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			ctrlGrdBar2_Load(null, null);
			GetSavedAttachmentsAgainstDocument(documentTypeId, screenName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetSavedAttachmentsAgainstDocument(int DocumentTypeId, string screenName)
	{
		try
		{
			dtAttachment.Rows.Clear();
			AttachmentsFlowLayoutPanel.Controls.Clear();
			List<global::Architecture.Model.DMSAttachments> dMSList = new List<DMSAttachments>();
			List<string> distinctIds = (from r in dtDetail.AsEnumerable()
				select Conversion.ToString(r["Id"])).Distinct().ToList();
			string.Join(",", distinctIds);
			DataTable dtAllAttachments = dtAllData.Tables[1];
			IEnumerable<DataRow> filteredAttachments = ((DocumentTypeId <= 0) ? (from dr in dtAllAttachments.AsEnumerable()
				where Conversion.ToString(dr["ScreenName"]) == screenName && distinctIds.Contains(Conversion.ToString(dr["DocumentId"]))
				select dr) : (from dr in dtAllAttachments.AsEnumerable()
				where Conversion.ToInt(dr["DocumentTypeId"]) == DocumentTypeId && distinctIds.Contains(Conversion.ToString(dr["DocumentId"]))
				select dr));
			if (!filteredAttachments.Any())
			{
				dtAllAttachments.Clone();
			}
			else
			{
				filteredAttachments.CopyToDataTable();
			}
			dMSList = (filteredAttachments.Any() ? filteredAttachments.Select((Func<DataRow, DMSAttachments>)((DataRow dr) => new DMSAttachments
			{
				Id = Conversion.ToInt(dr["Id"]),
				RefDocumentTypeId = Conversion.ToInt(dr["DocumentTypeId"]),
				Attachment = Conversion.ToString(dr["Attachment"]),
				UploadedFileCustomName = Conversion.ToString(dr["UploadedFileCustomName"]),
				EntryDate = Conversion.ToDateTime(dr["EntryDate"]),
				ScreenName = Conversion.ToString(dr["ScreenName"])
			})).ToList() : new List<DMSAttachments>());
			foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
			{
				dtAttachment.Rows.Add(itemList.Id, itemList.Attachment, itemList.UploadedFileCustomName, itemList.EntryDate);
			}
			if (distinctIds.Count == 1)
			{
				SelectedDocumentId = dMSList[0].Id;
				SelectedDocumentScreenName = dMSList[0].ScreenName;
				SelectedDocumentDocumentTypeId = DocumentTypeId;
			}
			AddAttachmentsToDataTable(distinctIds.Count == 1);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void AddAttachmentsToDataTable(bool ShowAddDeleteButtons = false)
	{
		try
		{
			if (dtAttachment.Rows.Count <= 0)
			{
				return;
			}
			AttachmentsFlowLayoutPanel.Controls.Clear();
			panel14.Visible = ShowAddDeleteButtons;
			lblTotalFiles.Text = Conversion.ToString((object)dtAttachment.Rows.Count);
			string saveDirectory = "";
			saveDirectory = clsGlobalVariables.AttachmentFolderPath;
			double TotalFileSizeInMbs = 0.0;
			foreach (Control control in AttachmentsFlowLayoutPanel.Controls)
			{
				control.Click -= UserControl_Click;
				control.Dispose();
			}
			AttachmentPreviewControl[] AttachmentCards = new AttachmentPreviewControl[dtAttachment.Rows.Count];
			for (int i = 0; i < AttachmentCards.Length; i++)
			{
				string ForPath = ((Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) != null && Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) != string.Empty) ? Conversion.ToString(dtAttachment.Rows[i]["CustomName"]) : Conversion.ToString(dtAttachment.Rows[i]["AttachmentName"]));
				AttachmentCards[i] = new AttachmentPreviewControl();
				AttachmentCards[i].btnDelete.Visible = ShowAddDeleteButtons;
				string AttachmentName = Path.GetFileName(ForPath);
				string path = "";
				if (IsVpsAttachmentsServiceOn)
				{
					Sc = CommonServices.GetServerCredentials();
					string uri = "ftp://" + Sc.ServerIp + "/" + AttachmentName;
					try
					{
						FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
						request.Method = "RETR";
						request.UsePassive = true;
						request.UseBinary = true;
						request.KeepAlive = false;
						request.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
						using FtpWebResponse ftpWebResponse = (FtpWebResponse)request.GetResponse();
						using Stream responseStream = ftpWebResponse.GetResponseStream();
						using MemoryStream memoryStream = new MemoryStream();
						responseStream.CopyTo(memoryStream);
						double fileSizeInMb = (double)memoryStream.Length / 1048576.0;
						TotalFileSizeInMbs += fileSizeInMb;
						AttachmentCards[i].FileSize = fileSizeInMb.ToString("0.000") + " Mb";
						Path.GetExtension(Path.GetFileName(request.RequestUri.LocalPath))?.ToUpper();
						memoryStream.Position = 0L;
						byte[] streamData = memoryStream.ToArray();
						AttachmentCards[i].memoryStream = streamData;
					}
					catch (WebException webEx)
					{
						if (webEx.Response is FtpWebResponse { StatusCode: FtpStatusCode.ActionNotTakenFileUnavailable })
						{
							Console.WriteLine("[INFO] Skipping missing FTP file: " + AttachmentName);
						}
						else
						{
							Console.WriteLine("[FTP ERROR] " + webEx.Message);
						}
						continue;
					}
					catch (Exception ex)
					{
						throw new Exception("Error: " + ex.Message);
					}
					AttachmentCards[i].path = "";
				}
				else
				{
					path = Path.Combine(saveDirectory, ForPath);
					AttachmentCards[i].path = path;
					FileInfo fileInfo = new FileInfo(path);
					if (!fileInfo.Exists)
					{
						continue;
					}
					TotalFileSizeInMbs += (double)fileInfo.Length / 1048576.0;
					AttachmentCards[i].FileSize = ((double)fileInfo.Length / 1048576.0).ToString("0.000") + " Mb";
				}
				AttachmentCards[i].AttachmentId = Conversion.ToInt(dtAttachment.Rows[i]["Id"]);
				AttachmentCards[i].FileName = Path.GetFileName(Conversion.ToString(dtAttachment.Rows[i]["AttachmentName"]));
				AttachmentCards[i].FileDate = Conversion.ToDateTime(dtAttachment.Rows[i]["EntryDate"]).ToString("dd-MMM-yyyy hh:mm tt");
				AttachmentsFlowLayoutPanel.Controls.Add(AttachmentCards[i]);
				AttachmentCards[i].UserControlClicked += UserControl_Click;
			}
			lblTotalSize.Text = TotalFileSizeInMbs.ToString("0.000");
		}
		catch (Exception ex2)
		{
			throw new Exception(ex2.Message);
		}
	}

	private void UserControl_Click(object sender, EventArgs e)
	{
		try
		{
			AttachmentPreviewControl obj = (AttachmentPreviewControl)sender;
			string path = obj.path;
			int Id = obj.AttachmentId;
			if (obj.ActiveControl != null && obj.ActiveControl.Text == "Delete")
			{
				if (Id > 0)
				{
					if (MessageBox.Show("Are you sure to delete this attachment? This is permanent delete", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					DMSAttachments.AttachmentDeleteByID(Id);
					if (!IsVpsAttachmentsServiceOn)
					{
						if (File.Exists(path))
						{
							obj.PictureBox.Image = null;
							obj.PictureBox.Invalidate();
							using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
							{
								fs.Flush();
								fs.Close();
								fs.Dispose();
							}
							bool fileDeleted = false;
							int attempts = 0;
							while (!fileDeleted && attempts < 5)
							{
								try
								{
									File.Delete(path);
									fileDeleted = true;
								}
								catch (IOException)
								{
									attempts++;
									Thread.Sleep(500);
								}
							}
						}
					}
					else
					{
						string uri = "ftp://" + Sc.ServerIp + "/" + path;
						try
						{
							FtpWebRequest obj2 = (FtpWebRequest)WebRequest.Create(uri);
							obj2.Method = "DELE";
							obj2.UsePassive = true;
							obj2.Credentials = new NetworkCredential(Sc.UserName, Sc.Password);
							using FtpWebResponse deleteResponse = (FtpWebResponse)obj2.GetResponse();
							if (deleteResponse.StatusCode != FtpStatusCode.FileActionOK)
							{
								throw new Exception("Failed to delete file from VPS.");
							}
						}
						catch (Exception)
						{
						}
					}
				}
				obj.PictureBox = null;
				foreach (DataRow item in (from row in dtAttachment.AsEnumerable()
					where Path.GetFileName(Conversion.ToString(row["AttachmentName"])) == obj.FileName
					select row).ToList())
				{
					item.Delete();
				}
				AttachmentsFlowLayoutPanel.Controls.Remove(obj);
				return;
			}
			if (!IsVpsAttachmentsServiceOn)
			{
				if (File.Exists(path))
				{
					Process.Start(path);
				}
				return;
			}
			using MemoryStream memoryStream = new MemoryStream(obj.memoryStream);
			memoryStream.Position = 0L;
			string fileExtension = Path.GetExtension(obj.FileName).ToLower();
			string tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + fileExtension);
			try
			{
				using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
				{
					memoryStream.CopyTo(fileStream);
				}
				ProcessStartInfo processStartInfo = new ProcessStartInfo(tempFilePath)
				{
					UseShellExecute = true,
					Verb = "open"
				};
				try
				{
					Process.Start(processStartInfo);
				}
				catch (Exception ex3)
				{
					MessageBox.Show("Could not open the file: " + ex3.Message);
					return;
				}
				Task.Run(async delegate
				{
					await Task.Delay(10000);
					for (int i = 0; i < 3; i++)
					{
						try
						{
							if (File.Exists(tempFilePath))
							{
								File.Delete(tempFilePath);
							}
							break;
						}
						catch (IOException)
						{
							await Task.Delay(1000);
						}
					}
				});
			}
			catch (Exception ex2)
			{
				MessageBox.Show("An error occurred while trying to open the file: " + ex2.Message);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("An unexpected error occurred: " + ex.Message);
		}
	}

	private void grdDetail_Click(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			try
			{
				GridEXRow r = grdDetail.CurrentRow;
				if (r != null && (int)r.RowType == 0 && r.Cells.Count != 0)
				{
					grdDetail.MoveTo(r.Position);
					grdDetail.HideSelection = (HideSelection)2;
					r.CheckState = (RowCheckState)0;
					SelectedDocumentId = Conversion.ToInt(r.Cells["Id"].Value);
					SelectedDocumentDocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
					SelectedDocumentScreenName = Conversion.ToString(r.Cells["ScreenName"].Value);
					GetSavedAttachmentsAgainstSelectedDocument(SelectedDocumentId, SelectedDocumentDocumentTypeId, SelectedDocumentScreenName);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		catch (Exception ex2)
		{
			MessageBox.Show(ex2.Message);
		}
	}

	private void GetSavedAttachmentsAgainstSelectedDocument(int Id, int DocumentTypeId, string screenName)
	{
		try
		{
			dtAttachment.Rows.Clear();
			AttachmentsFlowLayoutPanel.Controls.Clear();
			List<global::Architecture.Model.DMSAttachments> dMSList = new List<DMSAttachments>();
			DataTable dtAllAttachments = dtAllData.Tables[1];
			IEnumerable<DataRow> filteredAttachments = ((DocumentTypeId <= 0) ? (from dr in dtAllAttachments.AsEnumerable()
				where Conversion.ToString(dr["ScreenName"]) == screenName && Conversion.ToInt(dr["DocumentId"]) == Id
				select dr) : (from dr in dtAllAttachments.AsEnumerable()
				where Conversion.ToInt(dr["DocumentTypeId"]) == DocumentTypeId && Conversion.ToInt(dr["DocumentId"]) == Id
				select dr));
			if (!filteredAttachments.Any())
			{
				dtAllAttachments.Clone();
			}
			else
			{
				filteredAttachments.CopyToDataTable();
			}
			dMSList = (filteredAttachments.Any() ? filteredAttachments.Select((Func<DataRow, DMSAttachments>)((DataRow dr) => new DMSAttachments
			{
				Id = Conversion.ToInt(dr["Id"]),
				RefDocumentTypeId = Conversion.ToInt(dr["DocumentTypeId"]),
				Attachment = Conversion.ToString(dr["Attachment"]),
				UploadedFileCustomName = Conversion.ToString(dr["UploadedFileCustomName"]),
				EntryDate = Conversion.ToDateTime(dr["EntryDate"]),
				ScreenName = Conversion.ToString(dr["ScreenName"])
			})).ToList() : new List<DMSAttachments>());
			foreach (DMSAttachments itemList2 in dMSList)
			{
				dtAttachment.Rows.Add(itemList2.Id, itemList2.Attachment, itemList2.UploadedFileCustomName, itemList2.EntryDate);
			}
			if (dMSList.Count > 0)
			{
				foreach (global::Architecture.Model.DMSAttachments itemList in dMSList)
				{
					dtAttachment.Rows.Add(itemList.Id, itemList.Attachment, itemList.UploadedFileCustomName, itemList.EntryDate);
				}
			}
			AddAttachmentsToDataTable(ShowAddDeleteButtons: true);
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Expected O, but got Unknown
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		try
		{
			if (!dtAttachment.AsEnumerable().Any((DataRow r) => Conversion.ToInt(r["Id"]) == 0))
			{
				throw new Exception("No record found for save");
			}
			if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			DMSAttachments obj = new DMSAttachments();
			obj.AttachmentList = new List<DMSAttachments>();
			foreach (DataRow item in from r in dtAttachment.AsEnumerable()
				where Conversion.ToInt(r["Id"]) == 0
				select r)
			{
				string filePath = Conversion.ToString(item["AttachmentName"]);
				FileInfo info = new FileInfo(filePath);
				if (info.Exists && (double)info.Length / 1048576.0 > 5.0)
				{
					MessageBox.Show("File Size Exceeds 5MB Of File: " + Path.GetFileName(filePath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					return;
				}
				obj.AttachmentList.Add(new DMSAttachments
				{
					RefAccountId = 0,
					RefDocumentTypeId = SelectedDocumentDocumentTypeId,
					ScreenName = ((SelectedDocumentDocumentTypeId > 0) ? "" : SelectedDocumentScreenName),
					RefDocumentNo = SelectedDocumentId,
					EntryDate = DateTime.Now,
					ModifyDate = DateTime.Now,
					EntryUser = UserAccount.ID,
					ModifyUser = UserAccount.ID,
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					Attachment = filePath
				});
			}
			DMSAttachments.SaveList(obj);
			MessageBox.Show("Save Successfully");
			dtAttachment.Clear();
			AttachmentsFlowLayoutPanel.Controls.Clear();
			btnShowData_Click(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Choose_Click(object sender, EventArgs e)
	{
		try
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif)|*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.jfif|Excel Files (*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx)|*.xlsx;*.xlsm;*.xls;*.xlsb;*.xltx|Word Documents (*.docx;*.doc)|*.docx;*.doc|PDF Files (*.pdf)|*.pdf|Text Files (*.txt)|*.txt|Compressed Files (*.zip;*.rar)|*.zip;*.rar|All Files (*.*)|*.*";
			openFileDialog.Title = "Select a File";
			openFileDialog.Multiselect = true;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				string[] fileNames = openFileDialog.FileNames;
				foreach (string file in fileNames)
				{
					if ((from row in dtAttachment.AsEnumerable()
						where Conversion.ToString(row["AttachmentName"]).Contains(file)
						select row).ToList().Count == 0)
					{
						dtAttachment.Rows.Add(0, file, "", DateTime.Now);
					}
					else
					{
						MessageBox.Show(Path.GetFileName(file) + " File Already Present");
					}
				}
			}
			if (dtAttachment.Rows.Count > 0)
			{
				AddAttachmentsToDataTable(ShowAddDeleteButtons: true);
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
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Expected O, but got Unknown
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdSummary_DesignTimeLayout = new GridEXLayout();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.AttachmentsFlowLayoutPanel = new System.Windows.Forms.FlowLayoutPanel();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label5 = new System.Windows.Forms.Label();
		this.lblTotalFiles = new System.Windows.Forms.Label();
		this.lblTotalSize = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.panel14 = new System.Windows.Forms.Panel();
		this.btnSave = new System.Windows.Forms.Button();
		this.Choose = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel10 = new System.Windows.Forms.Panel();
		this.label1 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdSummary = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnShowData = new System.Windows.Forms.Button();
		this.label34 = new System.Windows.Forms.Label();
		this.CmbInvoiceNo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label16 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.miniToolStrip = new System.Windows.Forms.ToolStrip();
		this.panel1.SuspendLayout();
		this.panel3.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel15.SuspendLayout();
		this.panel13.SuspendLayout();
		this.panel14.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel10.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdSummary).BeginInit();
		this.panel6.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		base.SuspendLayout();
		this.panel1.BackColor = System.Drawing.Color.White;
		this.panel1.Controls.Add(this.panel3);
		this.panel1.Controls.Add(this.panel4);
		this.panel1.Controls.Add(this.panel2);
		this.panel1.Controls.Add(this.toolStrip1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1065, 703);
		this.panel1.TabIndex = 0;
		this.panel3.AutoScroll = true;
		this.panel3.Controls.Add(this.panel12);
		this.panel3.Controls.Add(this.panel9);
		this.panel3.Controls.Add(this.panel5);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 130);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1065, 573);
		this.panel3.TabIndex = 6;
		this.panel12.Controls.Add(this.AttachmentsFlowLayoutPanel);
		this.panel12.Controls.Add(this.panel15);
		this.panel12.Controls.Add(this.panel13);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel12.Location = new System.Drawing.Point(643, 0);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(417, 573);
		this.panel12.TabIndex = 11;
		this.AttachmentsFlowLayoutPanel.AutoScroll = true;
		this.AttachmentsFlowLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
		this.AttachmentsFlowLayoutPanel.Location = new System.Drawing.Point(0, 69);
		this.AttachmentsFlowLayoutPanel.Name = "AttachmentsFlowLayoutPanel";
		this.AttachmentsFlowLayoutPanel.Size = new System.Drawing.Size(417, 504);
		this.AttachmentsFlowLayoutPanel.TabIndex = 8;
		this.panel15.BackColor = System.Drawing.Color.White;
		this.panel15.Controls.Add(this.label5);
		this.panel15.Controls.Add(this.lblTotalFiles);
		this.panel15.Controls.Add(this.lblTotalSize);
		this.panel15.Controls.Add(this.label4);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 28);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(417, 41);
		this.panel15.TabIndex = 7;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.ForeColor = System.Drawing.Color.Black;
		this.label5.Location = new System.Drawing.Point(285, 3);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(111, 16);
		this.label5.TabIndex = 3;
		this.label5.Text = "Total Size (Mbs)";
		this.lblTotalFiles.AutoSize = true;
		this.lblTotalFiles.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalFiles.ForeColor = System.Drawing.Color.Black;
		this.lblTotalFiles.Location = new System.Drawing.Point(44, 21);
		this.lblTotalFiles.Name = "lblTotalFiles";
		this.lblTotalFiles.Size = new System.Drawing.Size(19, 18);
		this.lblTotalFiles.TabIndex = 2;
		this.lblTotalFiles.Text = "0";
		this.lblTotalSize.AutoSize = true;
		this.lblTotalSize.Font = new System.Drawing.Font("Verdana", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblTotalSize.ForeColor = System.Drawing.Color.Black;
		this.lblTotalSize.Location = new System.Drawing.Point(325, 21);
		this.lblTotalSize.Name = "lblTotalSize";
		this.lblTotalSize.Size = new System.Drawing.Size(19, 18);
		this.lblTotalSize.TabIndex = 1;
		this.lblTotalSize.Text = "0";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.ForeColor = System.Drawing.Color.Black;
		this.label4.Location = new System.Drawing.Point(20, 4);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(70, 16);
		this.label4.TabIndex = 0;
		this.label4.Text = "Total Files";
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.panel14);
		this.panel13.Controls.Add(this.label3);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 0);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(417, 28);
		this.panel13.TabIndex = 6;
		this.panel14.Controls.Add(this.btnSave);
		this.panel14.Controls.Add(this.Choose);
		this.panel14.Location = new System.Drawing.Point(277, 1);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(136, 27);
		this.panel14.TabIndex = 1;
		this.panel14.Visible = false;
		this.btnSave.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Location = new System.Drawing.Point(69, 0);
		this.btnSave.Margin = new System.Windows.Forms.Padding(2);
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(65, 26);
		this.btnSave.TabIndex = 7;
		this.btnSave.Text = "Save";
		this.btnSave.UseVisualStyleBackColor = true;
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.Choose.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Choose.Location = new System.Drawing.Point(3, 0);
		this.Choose.Margin = new System.Windows.Forms.Padding(2);
		this.Choose.Name = "Choose";
		this.Choose.Size = new System.Drawing.Size(65, 26);
		this.Choose.TabIndex = 6;
		this.Choose.Text = "Browse";
		this.Choose.UseVisualStyleBackColor = true;
		this.Choose.Click += new System.EventHandler(Choose_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.ForeColor = System.Drawing.Color.White;
		this.label3.Location = new System.Drawing.Point(5, 4);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(113, 19);
		this.label3.TabIndex = 0;
		this.label3.Text = "Attachments";
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel9.Controls.Add(this.panel10);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel9.Location = new System.Drawing.Point(302, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(341, 573);
		this.panel9.TabIndex = 10;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(341, 545);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 10;
		((System.Windows.Forms.Control)(object)this.grdDetail).TabStop = false;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdDetail).Click += new System.EventHandler(grdDetail_Click);
		this.panel10.BackColor = System.Drawing.Color.Teal;
		this.panel10.Controls.Add(this.label1);
		this.panel10.Controls.Add(this.ctrlGrdBar2);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(341, 28);
		this.panel10.TabIndex = 9;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.White;
		this.label1.Location = new System.Drawing.Point(6, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(216, 18);
		this.label1.TabIndex = 0;
		this.label1.Text = "Detail Info Of Selected Row";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(300, 3);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdDetail;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar2.TabIndex = 1;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdSummary);
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(302, 573);
		this.panel5.TabIndex = 9;
		((ScrollableBase)this.grdSummary).BackColor = System.Drawing.Color.White;
		grdSummary_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdSummary.DesignTimeLayout = grdSummary_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdSummary).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdSummary.FilterRowButtonStyle = (FilterRowButtonStyle)1;
		((System.Windows.Forms.Control)(object)this.grdSummary).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdSummary.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.White;
		this.grdSummary.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdSummary).Location = new System.Drawing.Point(0, 28);
		((System.Windows.Forms.Control)(object)this.grdSummary).Name = "grdSummary";
		((ScrollableBase)this.grdSummary).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdSummary).Size = new System.Drawing.Size(302, 545);
		((System.Windows.Forms.Control)(object)this.grdSummary).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.grdSummary).TabStop = false;
		this.grdSummary.TotalRow = (InheritableBoolean)1;
		this.grdSummary.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdSummary).VisualStyle = (VisualStyle)4;
		((System.Windows.Forms.Control)(object)this.grdSummary).Click += new System.EventHandler(grdSummary_Click);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.label15);
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(302, 28);
		this.panel6.TabIndex = 8;
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(6, 5);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(113, 18);
		this.label15.TabIndex = 0;
		this.label15.Text = "Summary Info";
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(261, 3);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdSummary;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 21);
		this.ctrlGrdBar1.TabIndex = 1;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 57);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1065, 73);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add(this.btnShowData);
		this.groupBox1.Controls.Add(this.label34);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbInvoiceNo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(363, 68);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Filters";
		this.btnShowData.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnShowData.FlatAppearance.BorderSize = 0;
		this.btnShowData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowData.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowData.ForeColor = System.Drawing.Color.White;
		this.btnShowData.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnShowData.Location = new System.Drawing.Point(302, 36);
		this.btnShowData.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowData.Name = "btnShowData";
		this.btnShowData.Size = new System.Drawing.Size(57, 26);
		this.btnShowData.TabIndex = 1;
		this.btnShowData.Text = "Show";
		this.btnShowData.UseVisualStyleBackColor = false;
		this.btnShowData.Click += new System.EventHandler(btnShowData_Click);
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.ForeColor = System.Drawing.Color.Red;
		this.label34.Location = new System.Drawing.Point(121, 17);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(14, 17);
		this.label34.TabIndex = 4;
		this.label34.Text = "*";
		this.CmbInvoiceNo.AllowNull = (DefaultableBoolean)1;
		this.CmbInvoiceNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbInvoiceNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbInvoiceNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance44).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance45).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance45).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance45).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance46).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbInvoiceNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbInvoiceNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Location = new System.Drawing.Point(6, 36);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Name = "CmbInvoiceNo";
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).Size = new System.Drawing.Size(291, 26);
		((System.Windows.Forms.Control)(object)this.CmbInvoiceNo).TabIndex = 0;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(6, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(113, 15);
		this.label2.TabIndex = 0;
		this.label2.Text = "Commercial Invoice";
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.label16);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(0, 27);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1065, 30);
		this.panel2.TabIndex = 0;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(5, 5);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(521, 19);
		this.label16.TabIndex = 1;
		this.label16.Text = "Documents Against Commercial Invoice And their Attachments";
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.btnNew, this.toolStripButton1, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1065, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.ToolTipText = "Ctrl_N";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(80, 24);
		this.toolStripButton1.Text = "&Refresh";
		this.toolStripButton1.ToolTipText = "Alt_R";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.miniToolStrip.AccessibleName = "New item selection";
		this.miniToolStrip.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDown;
		this.miniToolStrip.AutoSize = false;
		this.miniToolStrip.CanOverflow = false;
		this.miniToolStrip.Dock = System.Windows.Forms.DockStyle.None;
		this.miniToolStrip.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.miniToolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
		this.miniToolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.miniToolStrip.Location = new System.Drawing.Point(274, 4);
		this.miniToolStrip.Name = "miniToolStrip";
		this.miniToolStrip.Size = new System.Drawing.Size(1000, 27);
		this.miniToolStrip.TabIndex = 0;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1065, 703);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmDocumentsAttachmentAgainstCommercialInvoice";
		this.Text = "Documents Against Commercial Invoice And Their Attachments";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(GrnPackingMaterial_FormClosing);
		base.Load += new System.EventHandler(InvFrmGRN_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(InvFrmGRN_KeyDown);
		this.panel1.ResumeLayout(false);
		this.panel1.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.panel14.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel10.ResumeLayout(false);
		this.panel10.PerformLayout();
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdSummary).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbInvoiceNo).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
