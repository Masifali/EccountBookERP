// using Architecture.Reports;
// using Architecture.WinApp.Configurations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Architecture.BLL.Inventory;
using Architecture.BLL.Main;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Architecture.ZKTFingerPrint;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;
using libzkfpcsharp;
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.Sale
{

public class frmDriverBio : Form
{
	private Rightsobjects formright = new Rightsobjects();

	private FilterInfoCollection captureDevice;

	private VideoCaptureDevice videosource;

	public static Image Logo;

	private UserAccount UserAccount = new UserAccount();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	private bool UpdateMode;

	private string FingerPrintPicPath;

	private DataTable dtPendingFromDb = new DataTable();

	private DataTable dtPendingGrid = new DataTable();

	private DataTable dtHistoryGrid = new DataTable();

	private int RECID;

	private int DefaultDaysToLessFromHistoryFromDate;

	public int RefDocumentTypeId;

	private string ScreenName = "";

	private bool Confiquration;

	private string saveDirectory = "";

	private string DriverImagefileSavePath = "";

	private string DriverImageofd = string.Empty;

	private IntPtr mDevHandle = IntPtr.Zero;

	private IntPtr mDBHandle = IntPtr.Zero;

	private IntPtr FormHandle = IntPtr.Zero;

	private bool bIsTimeToDie;

	private bool IsRegister;

	private bool bIdentify = true;

	private byte[] FPBuffer;

	private byte[] DriverImageBytes;

	private byte[] FingerPrintImage;

	private int RegisterCount;

	private const int REGISTER_FINGER_COUNT = 3;

	private byte[][] RegTmps = new byte[3][];

	private byte[] RegTmp = new byte[2048];

	private byte[] CapTmp = new byte[2048];

	private int cbCapTmp = 2048;

	private int cbRegTmp;

	private int iFid = 1;

	private int mfpWidth;

	private int mfpHeight;

	private int mfpDpi;

	private const int MESSAGE_CAPTURED_OK = 1030;

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private DateTimePicker txtGatepassDate;

	internal UltraCombo cmbGatePass;

	private Label label4;

	private Label label17;

	private Label label8;

	private TextBox txtVehicleNo;

	private Label label5;

	private TextBox txtContainer;

	private TextBox txtNetWeight;

	private Label label1;

	private TextBox txtSealNo;

	private Label label12;

	private TextBox txtRemarks;

	private Label label11;

	private Label label10;

	private Label label9;

	private Label label7;

	private TextBox txtFatherName;

	private Label label6;

	private Label label3;

	private TextBox txtForwarderName;

	private GroupBox grp;

	private Button btnPreview;

	private Button btnDriverImage;

	private Panel panel6;

	private Label label13;

	private TextBox txtBiltyNo;

	private PictureBox picBoxDriver;

	private Panel panel7;

	private Panel panel9;

	internal GridEX DataGridHistory;

	private ToolStrip toolStrip2;

	private ToolStripButton BtnNewHistory;

	private PictureBox picFPImg;

	private Button btnFpCapture;

	private Button btnFpStart;

	private GroupBox groupBox1;

	private Button btnReset;

	private Button btnResetDriv;

	private MaskedTextBox txtCNIC;

	private ComboBox comboBox1;

	private ToolStripButton btnattachment;

	private ToolStripButton btnSMS;

	private Label label14;

	private Panel panel11;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar1;

	private Panel panel12;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label16;

	private Button btnBrowse;

	internal GridEX grdOutstandingGatePass;

	private Label label2;

	private GroupBox groupBox2;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnShowHistory;

	private Label label51;

	private Label label52;

	private Label label53;

	private Label label54;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private CheckBox ChkBox;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnRefresh;

	private MaskedTextBox txtAlternateCellNo;

	private MaskedTextBox txtDriverCellNo;

	private TextBox txtDriverName;

	private PictureBox btnResetDriverInfo;

	[DllImport("user32.dll", EntryPoint = "SendMessageA")]
	public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

	public frmDriverBio(UserAccount user)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmDriverBio_Load(object sender, EventArgs e)
	{
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Expected O, but got Unknown
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1150, 650);
			ScreenName = Conversion.ToString(base.Tag);
			if (RefDocumentTypeId == 0)
			{
				if (ScreenName == "frmDriverBioForInWard")
				{
					RefDocumentTypeId = 51;
				}
				else if (ScreenName == "frmDriverBioForOutWard")
				{
					RefDocumentTypeId = 91;
				}
			}
			else if (RefDocumentTypeId == 51)
			{
				ScreenName = "frmDriverBioForInWard";
			}
			else if (RefDocumentTypeId == 91)
			{
				ScreenName = "frmDriverBioForOutWard";
			}
			formright = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnsave.Enabled = formright.DoHaveSaveRight;
			Print.Enabled = formright.DoHavePrintRights;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			captureDevice = new FilterInfoCollection(FilterCategory.VideoInputDevice);
			foreach (FilterInfo item in (CollectionBase)(object)captureDevice)
			{
				FilterInfo device = item;
				comboBox1.Items.Add(device.Name);
			}
			if (comboBox1.Text == null || comboBox1.Text == string.Empty)
			{
				comboBox1.Text = string.Empty;
			}
			else
			{
				comboBox1.SelectedIndex = 0;
			}
			pendingGatepass();
			FormHandle = base.Handle;
			btnFpStart.Enabled = true;
			btnFpCapture.Enabled = false;
			cmbGatePass.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
			txtDriverCellNo.Text = "0092-3-";
			txtAlternateCellNo.Text = "0092-3-";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GpNoBind(DataTable dtPending)
	{
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("GpSrNo", typeof(int));
			dt.Columns.Add("VehicleNo", typeof(string));
			dt.Columns.Add("GpDate", typeof(string));
			dt.Columns.Add("BiltyNo", typeof(string));
			dt.Columns.Add("GatePassType", typeof(string));
			foreach (DataRow row in dtPending.Rows)
			{
				dt.Rows.Add(Conversion.ToInt(row["Id"]), Conversion.ToInt(row["GpSrNo"]), Conversion.ToString(row["VehicleNo"]), Conversion.ToDateTime(row["GpDate"]).ToShortDateString(), Conversion.ToString(row["BiltyNo"]), Conversion.ToString(row["GatePassType"]));
			}
			InfragisticsHelper.BindAndRetainSelection(cmbGatePass, dt, "Id", "GpSrNo", "Gp No", AllColumns: true, Conversion.ToInt(cmbGatePass.Value), insertDefaultRow: false, null, null, ActivateRow: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error binding GP data: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void cmbGatePass_TextChanged(object sender, EventArgs e)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		try
		{
			if (((Control)(object)cmbGatePass).Text != string.Empty && ((Control)(object)cmbGatePass).Text != "0")
			{
				DataTable dtgetgpsr = GatePassOutward.ReadByGpNoForExportDriverInformation(new GatePassOutward
				{
					OrganizationId = UserAccount.OrganizationId,
					CompanyId = UserAccount.CompanyId,
					BranchesId = UserAccount.BranchesId,
					FinancialYearId = clsGlobalVariables.ActiveYr.Id,
					DocumentTypeId = ((RefDocumentTypeId > 0) ? RefDocumentTypeId : 91),
					GpSrNo = Conversion.ToInt((object)((Control)(object)cmbGatePass).Text.Trim())
				});
				if (dtgetgpsr.Rows.Count > 0)
				{
					txtBiltyNo.Text = dtgetgpsr.Rows[0]["BiltyNo"].ToString();
					txtVehicleNo.Text = dtgetgpsr.Rows[0]["VehicleNo"].ToString();
					txtGatepassDate.Value = Conversion.ToDateTime(dtgetgpsr.Rows[0]["GpDate"]);
					txtContainer.Text = dtgetgpsr.Rows[0]["Container"].ToString() + "0";
					txtSealNo.Text = dtgetgpsr.Rows[0]["OtherRemarks"].ToString();
					txtNetWeight.Text = dtgetgpsr.Rows[0]["FactoryWeight"].ToString();
				}
				else
				{
					txtBiltyNo.Text = string.Empty;
					txtVehicleNo.Text = string.Empty;
					txtGatepassDate.Value = DateTime.Now;
					txtContainer.Text = string.Empty;
					txtSealNo.Text = string.Empty;
					txtNetWeight.Text = string.Empty;
				}
				txtCNIC.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtCNIC.Text.Trim() == "-       -" || txtCNIC.Text.Length < 15 || !txtCNIC.MaskFull)
		{
			MessageBox.Show("CNIC Field Required");
			txtCNIC.Focus();
			return false;
		}
		if (txtDriverCellNo.Text.Trim() == string.Empty || !txtDriverCellNo.MaskFull)
		{
			MessageBox.Show("Cell No. Field Required");
			txtDriverCellNo.Focus();
			return false;
		}
		if (txtDriverName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Driver Name  Field Required");
			txtDriverName.Focus();
			return false;
		}
		if (txtFatherName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Father Name  Field Required");
			txtFatherName.Focus();
			return false;
		}
		if (txtForwarderName.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Forwarder Name Field Required");
			txtForwarderName.Focus();
			return false;
		}
		return true;
	}

	public void pendingGatepass()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters gatePass = new ReportsParameters();
			gatePass.OrganizationId = UserAccount.OrganizationId;
			gatePass.CompanyId = UserAccount.CompanyId;
			gatePass.BranchesId = UserAccount.BranchesId;
			gatePass.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			gatePass.DocumentTypeId = ((RefDocumentTypeId == 0) ? 91 : RefDocumentTypeId);
			dtPendingFromDb = GatePassOutwardDriverInfo.GatePass_PendingForDriverInfo(gatePass);
			if (dtPendingFromDb.Rows.Count <= 0)
			{
				return;
			}
			GpNoBind(dtPendingFromDb);
			if (dtPendingGrid.Columns.Count == 0)
			{
				dtPendingGrid.Columns.Add("Id", typeof(int));
				dtPendingGrid.Columns.Add("DocumentTypeId", typeof(int));
				dtPendingGrid.Columns.Add("DocumentType", typeof(string));
				dtPendingGrid.Columns.Add("GatePassType", typeof(string));
				dtPendingGrid.Columns.Add("GpSrNo", typeof(int));
				dtPendingGrid.Columns.Add("GpDate", typeof(DateTime));
				dtPendingGrid.Columns.Add("VehicleNo", typeof(string));
				dtPendingGrid.Columns.Add("BiltyNo", typeof(string));
				dtPendingGrid.Columns.Add("Container", typeof(string));
				dtPendingGrid.Columns.Add("Container1", typeof(string));
				dtPendingGrid.Columns.Add("FirstWeight", typeof(decimal));
				dtPendingGrid.Columns.Add("SecondWeight", typeof(decimal));
				dtPendingGrid.Columns.Add("NetWeight", typeof(decimal));
			}
			dtPendingGrid.Rows.Clear();
			foreach (DataRow row in dtPendingFromDb.Rows)
			{
				dtPendingGrid.Rows.Add(row["Id"], row["DocumentTypeId"], row["DocumentType"], row["GatepassType"], row["GpSrNo"], row["GpDate"], row["VehicleNo"], row["BiltyNo"], row["Container"], row["Container1"], row["FirstWeight"], row["SecondWeight"], row["NetWeight"]);
			}
			grdOutstandingGatePass.DataSource = dtPendingGrid;
			grdOutstandingGatePass.RetrieveStructure();
			grdOutstandingGatePassSetting();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdOutstandingGatePassSetting()
	{
		try
		{
			grdOutstandingGatePass.RootTable.Columns["Id"].Visible = false;
			grdOutstandingGatePass.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(grdOutstandingGatePass, 2);
			grdOutstandingGatePass.RootTable.Columns["DocumentType"].Width = Constants.InventoryConstants.DocumentType;
			grdOutstandingGatePass.RootTable.Columns["GatePassType"].Width = Constants.InventoryConstants.GpType;
			grdOutstandingGatePass.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.GpNoConstant;
			grdOutstandingGatePass.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			grdOutstandingGatePass.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			grdOutstandingGatePass.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			grdOutstandingGatePass.RootTable.Columns["Container"].Width = 100;
			grdOutstandingGatePass.RootTable.Columns["Container1"].Width = 100;
			grdOutstandingGatePass.RootTable.Columns["FirstWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutstandingGatePass.RootTable.Columns["SecondWeight"].Width = Constants.InventoryConstants.WeightConstant;
			grdOutstandingGatePass.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			GridEX_Helper.AddButton(grdOutstandingGatePass, "Load", "Load", 40, 1);
			grdOutstandingGatePass.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdOutstandingGatePass_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdOutstandingGatePass.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0 && e.Column.Key == "Load")
			{
				cmbGatePass.Value = Conversion.ToInt(r.Cells["Id"].Value);
				cmbGatePass_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			GatePassOutwardDriverInfo driverInfo = new GatePassOutwardDriverInfo();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				driverInfo.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			driverInfo.OrganizationId = UserAccount.OrganizationId;
			driverInfo.CompanyId = UserAccount.CompanyId;
			driverInfo.BranchId = UserAccount.BranchesId;
			driverInfo.DocumentTypeId = 93;
			driverInfo.RefDocumentTypeId = ((RefDocumentTypeId == 0) ? 91 : RefDocumentTypeId);
			driverInfo.EntryUser = UserAccount.ID;
			driverInfo.ModifyUser = UserAccount.ID;
			driverInfo.EntryDate = DateTime.Now;
			driverInfo.ModifyDate = DateTime.Now;
			driverInfo.GatePassOutwardId = Conversion.ToInt(cmbGatePass.Value);
			driverInfo.GpDate = txtGatepassDate.Value;
			driverInfo.ForwarderName = txtForwarderName.Text;
			driverInfo.DriverName = txtDriverName.Text;
			driverInfo.FatherName = txtFatherName.Text;
			driverInfo.CnicNo = txtCNIC.Text;
			driverInfo.DriverCellNo = txtDriverCellNo.Text;
			driverInfo.AlternateCellNo = txtAlternateCellNo.Text;
			driverInfo.RemarksHeader = txtRemarks.Text;
			driverInfo.DriverPic = Path.GetFileName(Conversion.ToString((object)DriverImagefileSavePath));
			driverInfo.ThumbPic = Path.GetFileName(FingerPrintPicPath);
			driverInfo.DriverImage = null;
			driverInfo.FingerPrintImage = null;
			driverInfo.ScreenName = ScreenName;
			driverInfo.dmsAttachment = new List<DMSAttachments>();
			driverInfo.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<GatePassOutwardDriverInfo>(driverInfo, base.Name, AT, "dmsAttachment", "DeleteAttachmentsList");
			int success = GatePassOutwardDriverInfo.Save(driverInfo);
			if (RECID > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + driverInfo.Id);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, driverInfo.DocumentTypeId, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + driverInfo.Id);
			}
			if (!string.IsNullOrEmpty(DriverImageofd))
			{
				File.Copy(DriverImageofd, DriverImagefileSavePath, overwrite: true);
			}
			else if (picBoxDriver.Image != null && !File.Exists(DriverImagefileSavePath))
			{
				picBoxDriver.Image.Save(DriverImagefileSavePath, ImageFormat.Jpeg);
			}
			DeletePreviousDriverPicPhysically();
			DriverImageofd = string.Empty;
			DriverImagefileSavePath = string.Empty;
			FormReset();
			if (ChkBox.Checked)
			{
				GenerateReport(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeletePreviousDriverPicPhysically()
	{
		try
		{
			string picNameInReadById = Conversion.ToString(picBoxDriver.Tag);
			if (string.IsNullOrEmpty(picNameInReadById) || Path.GetFileName(DriverImagefileSavePath) == picNameInReadById)
			{
				return;
			}
			string path = Path.Combine(clsGlobalVariables.AttachmentFolderPath, picNameInReadById);
			if (File.Exists(path))
			{
				using (new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
				{
				}
				File.Delete(path);
			}
		}
		catch (UnauthorizedAccessException)
		{
			MessageBox.Show("Access denied. The file may be in use or read-only.");
		}
		catch (IOException ex2)
		{
			MessageBox.Show("File is currently in use and cannot be deleted.\nDetails: " + ex2.Message);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message);
		}
	}

	public void ReadById(int Id)
	{
		try
		{
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			RECID = Id;
			DataTable dtgetbyId = GatePassOutwardDriverInfo.GetByID(RECID);
			if (dtgetbyId.Rows.Count <= 0)
			{
				return;
			}
			UpdateMode = true;
			tabControl1.SelectedIndex = 0;
			cmbGatePass.Value = dtgetbyId.Rows[0]["GatePassOutwardId"];
			txtGatepassDate.Value = Conversion.ToDateTime(dtgetbyId.Rows[0]["GpDate"]);
			txtVehicleNo.Text = dtgetbyId.Rows[0]["VehicleNo"].ToString();
			txtBiltyNo.Text = dtgetbyId.Rows[0]["BiltyNo"].ToString();
			txtContainer.Text = dtgetbyId.Rows[0]["Container"].ToString();
			txtSealNo.Text = txtSealNo.Text;
			txtNetWeight.Text = dtgetbyId.Rows[0]["FactoryWeight"].ToString();
			txtForwarderName.Text = dtgetbyId.Rows[0]["ForwarderName"].ToString();
			txtCNIC.Text = dtgetbyId.Rows[0]["CnicNo"].ToString();
			txtDriverName.Text = dtgetbyId.Rows[0]["DriverName"].ToString();
			txtDriverCellNo.Text = dtgetbyId.Rows[0]["DriverCellNo"].ToString();
			txtAlternateCellNo.Text = dtgetbyId.Rows[0]["AlternateCellNo"].ToString();
			txtFatherName.Text = dtgetbyId.Rows[0]["FatherName"].ToString();
			if (Conversion.ToInt(dtgetbyId.Rows[0]["driverBiodataId"]) > 0)
			{
				DriverFieldsDisableOrEnable(flag: false);
			}
			txtRemarks.Text = dtgetbyId.Rows[0]["RemarksHeader"].ToString();
			((Control)(object)cmbGatePass).Enabled = false;
			if (dtgetbyId.Rows[0]["ThumbPic"].ToString() != "")
			{
				picFPImg.Image = Image.FromFile(dtgetbyId.Rows[0]["ThumbPic"].ToString());
				FingerPrintPicPath = dtgetbyId.Rows[0]["ThumbPic"].ToString();
			}
			string driverPic = Path.GetFileName(Conversion.ToString(dtgetbyId.Rows[0]["DriverPic"]));
			if (driverPic != "")
			{
				DriverImagefileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, driverPic);
				if (File.Exists(DriverImagefileSavePath))
				{
					picBoxDriver.Image = Image.FromFile(DriverImagefileSavePath);
					picBoxDriver.Tag = driverPic;
				}
				else
				{
					DriverImagefileSavePath = "";
					picBoxDriver.Image = null;
					picBoxDriver.Invalidate();
				}
			}
			else
			{
				DriverImagefileSavePath = "";
				picBoxDriver.Image = null;
				picBoxDriver.Invalidate();
			}
			FormHelper.LoadAttachmentsForObject(AT, RECID, ScreenName);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		try
		{
			RECID = 0;
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
			if (RECID == 0)
			{
				throw new Exception("Record Not Update because RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		try
		{
			FormReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void FormReset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			RECID = 0;
			txtBiltyNo.Text = string.Empty;
			txtForwarderName.Text = string.Empty;
			txtGatepassDate.Text = string.Empty;
			txtNetWeight.Text = string.Empty;
			txtRemarks.Text = string.Empty;
			txtSealNo.Text = string.Empty;
			txtVehicleNo.Text = string.Empty;
			((Control)(object)cmbGatePass).Text = string.Empty;
			txtContainer.Text = string.Empty;
			((Control)(object)cmbGatePass).Enabled = true;
			txtCNIC.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtAlternateCellNo.Text = string.Empty;
			txtFatherName.Text = string.Empty;
			txtDriverCellNo.Text = "0092-3-";
			txtAlternateCellNo.Text = "0092-3-";
			DriverFieldsDisableOrEnable(flag: true);
			pendingGatepass();
			cmbGatePass.Focus();
			picBoxDriver.Image?.Dispose();
			picBoxDriver.Image = null;
			picBoxDriver.Tag = "";
			picBoxDriver.Invalidate();
			btnResetDriv_Click(null, null);
			btnReset_Click(null, null);
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDriverBio_FormClosed(object sender, FormClosedEventArgs e)
	{
		try
		{
			btnReset_Click(null, null);
			btnResetDriv_Click(null, null);
			HistoryStack.Remove(this);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
			pendingGatepass();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void definition()
	{
		try
		{
			List<ConfigrationsAllocation> list = clsGlobalVariables.configrationsAllocation.Where((ConfigrationsAllocation x) => x.ConfigDescription == "Attachment Folder Path").ToList();
			if (list.Count > 0)
			{
				string value = list[0].ConfigKey.ToString();
				if (value != null)
				{
					saveDirectory = value;
					Confiquration = true;
				}
			}
			else
			{
				MessageBox.Show("Please Map the Path in Configration");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void videosource_NewFrame(object sender, NewFrameEventArgs eventArgs)
	{
		picBoxDriver.Image = (Bitmap)eventArgs.Frame.Clone();
	}

	private void btnFpStart_Click(object sender, EventArgs e)
	{
		try
		{
			MessageBox.Show("Not Configured...");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnFpCapture_Click(object sender, EventArgs e)
	{
	}

	private void btnReset_Click(object sender, EventArgs e)
	{
	}

	public void Intilize()
	{
		try
		{
			List<string> cmbIdx = new List<string>();
			int ret = zkfperrdef.ZKFP_ERR_OK;
			if ((ret = zkfp2.Init()) == zkfperrdef.ZKFP_ERR_OK)
			{
				int nCount = zkfp2.GetDeviceCount();
				if (nCount > 0)
				{
					for (int i = 0; i < nCount; i++)
					{
						cmbIdx.Add(i.ToString());
					}
					return;
				}
				zkfp2.Terminate();
				throw new Exception("No device connected!");
			}
			throw new Exception("Initialize fail, ret=" + ret + " !");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Connect(int Idx)
	{
		try
		{
			_ = zkfp.ZKFP_ERR_OK;
			if (IntPtr.Zero == (mDevHandle = zkfp2.OpenDevice(Idx)))
			{
				throw new Exception("OpenDevice fail");
			}
			if (IntPtr.Zero == (mDBHandle = zkfp2.DBInit()))
			{
				zkfp2.CloseDevice(mDevHandle);
				mDevHandle = IntPtr.Zero;
				throw new Exception("Init DB fail");
			}
			RegisterCount = 0;
			cbRegTmp = 0;
			iFid = 1;
			for (int i = 0; i < 3; i++)
			{
				RegTmps[i] = new byte[2048];
			}
			byte[] paramValue = new byte[4];
			int size = 4;
			zkfp2.GetParameters(mDevHandle, 1, paramValue, ref size);
			zkfp2.ByteArray2Int(paramValue, ref mfpWidth);
			size = 4;
			zkfp2.GetParameters(mDevHandle, 2, paramValue, ref size);
			zkfp2.ByteArray2Int(paramValue, ref mfpHeight);
			FPBuffer = new byte[mfpWidth * mfpHeight];
			size = 4;
			zkfp2.GetParameters(mDevHandle, 3, paramValue, ref size);
			zkfp2.ByteArray2Int(paramValue, ref mfpDpi);
			Thread thread = new Thread(DoCapture);
			thread.IsBackground = true;
			thread.Start();
			bIsTimeToDie = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void DoCapture()
	{
		try
		{
			while (!bIsTimeToDie)
			{
				cbCapTmp = 2048;
				if (zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, ref cbCapTmp) == zkfp.ZKFP_ERR_OK)
				{
					SendMessage(FormHandle, 1030, IntPtr.Zero, IntPtr.Zero);
				}
				Thread.Sleep(200);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	protected override void DefWndProc(ref Message m)
	{
		try
		{
			if (m.Msg == 1030)
			{
				MemoryStream ms = new MemoryStream();
				BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ref ms);
				Bitmap bmp = new Bitmap(ms);
				picFPImg.Image = bmp;
				zkfp2.BlobToBase64(CapTmp, cbCapTmp);
				if (IsRegister)
				{
					int ret = zkfp.ZKFP_ERR_OK;
					int fid = 0;
					int score = 0;
					ret = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid, ref score);
					if (zkfp.ZKFP_ERR_OK == ret)
					{
						IsRegister = true;
						return;
					}
					if (RegisterCount > 0 && zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps[RegisterCount - 1]) <= 0)
					{
						MessageBox.Show("Please press the same finger 3 times for the enrollment.\n");
					}
					Array.Copy(CapTmp, RegTmps[RegisterCount], cbCapTmp);
					zkfp2.Base64ToBlob(zkfp2.BlobToBase64(CapTmp, cbCapTmp));
					RegisterCount++;
					if (RegisterCount >= 3)
					{
						RegisterCount = 0;
						if (zkfp.ZKFP_ERR_OK == (ret = zkfp2.DBMerge(mDBHandle, RegTmps[0], RegTmps[1], RegTmps[2], RegTmp, ref cbRegTmp)))
						{
							iFid++;
							btnFpCapture.Enabled = true;
						}
						else
						{
							MessageBox.Show("enroll fail, error code=" + ret + "\n");
						}
						IsRegister = false;
					}
					return;
				}
				if (cbRegTmp <= 0)
				{
					MessageBox.Show("Please register your finger first!\n");
				}
				if (bIdentify)
				{
					int ret2 = zkfp.ZKFP_ERR_OK;
					int fid2 = 0;
					int score2 = 0;
					ret2 = zkfp2.DBIdentify(mDBHandle, CapTmp, ref fid2, ref score2);
					if (zkfp.ZKFP_ERR_OK != ret2)
					{
						MessageBox.Show("Identify fail, ret= " + ret2 + "\n");
					}
				}
				else
				{
					int ret3 = zkfp2.DBMatch(mDBHandle, CapTmp, RegTmp);
					if (0 >= ret3)
					{
						MessageBox.Show("Match finger fail, ret= " + ret3 + "\n");
					}
				}
			}
			else
			{
				base.DefWndProc(ref m);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void Disconnect()
	{
		try
		{
			zkfp2.Terminate();
			cbRegTmp = 0;
			bIsTimeToDie = true;
			RegisterCount = 0;
			Thread.Sleep(1000);
			zkfp2.CloseDevice(mDevHandle);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void CaptureBmp(string FileName)
	{
		try
		{
			if (!(FileName != "") || FileName == null || picFPImg.Image == null)
			{
				return;
			}
			Bitmap bmp = new Bitmap(picFPImg.Image.Width, picFPImg.Image.Height, PixelFormat.Format24bppRgb);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.DrawImage(picFPImg.Image, 0, 0, bmp.Width, bmp.Height);
			}
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, bmp.PixelFormat);
			IntPtr scan = bmpData.Scan0;
			int bytes = bmpData.Stride * bmpData.Height;
			byte[] rgbValues = new byte[bytes];
			Marshal.Copy(scan, rgbValues, 0, bytes);
			Rectangle rect2 = new Rectangle(0, 0, bmp.Width, bmp.Height);
			Bitmap bit = new Bitmap(bmp.Width, bmp.Height, PixelFormat.Format8bppIndexed);
			BitmapData bmpData2 = bit.LockBits(rect2, ImageLockMode.ReadWrite, bit.PixelFormat);
			IntPtr ptr2 = bmpData2.Scan0;
			int bytes2 = bmpData2.Stride * bmpData2.Height;
			byte[] rgbValues2 = new byte[bytes2];
			Marshal.Copy(ptr2, rgbValues2, 0, bytes2);
			double colorTemp = 0.0;
			for (int j = 0; j < bmpData.Height; j++)
			{
				for (int k = 0; k < bmpData.Width * 3; k += 3)
				{
					colorTemp = (double)(int)rgbValues[j * bmpData.Stride + k + 2] * 0.299 + (double)(int)rgbValues[j * bmpData.Stride + k + 1] * 0.578 + (double)(int)rgbValues[j * bmpData.Stride + k] * 0.114;
					rgbValues2[j * bmpData2.Stride + k / 3] = (byte)colorTemp;
				}
			}
			Marshal.Copy(rgbValues2, 0, ptr2, bytes2);
			bmp.UnlockBits(bmpData);
			ColorPalette tempPalette;
			using (Bitmap tempBmp = new Bitmap(1, 1, PixelFormat.Format8bppIndexed))
			{
				tempPalette = tempBmp.Palette;
			}
			for (int i = 0; i < 256; i++)
			{
				tempPalette.Entries[i] = Color.FromArgb(i, i, i);
			}
			bit.Palette = tempPalette;
			bit.UnlockBits(bmpData2);
			bit.Save(FileName, picFPImg.Image.RawFormat);
			bit.Dispose();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Enroll()
	{
		try
		{
			if (!IsRegister)
			{
				IsRegister = true;
				RegisterCount = 0;
				cbRegTmp = 0;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateReport(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateReport(int PrintId)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt = GatePassOutwardDriverInfo.GatePassOutwardDriverInfoRpt(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				Id = PrintId
			});
			if (dt == null || dt.Rows.Count == 0)
			{
				MessageBox.Show("Not Record Found For Display");
				return;
			}
			Reporting rpt = new Reporting();
			if (UserAccount.CompAddress != null)
			{
				rpt.RptPerameter("@CompanyAddress", UserAccount.CompAddress.ToString());
			}
			if (UserAccount.CompName != null)
			{
				rpt.RptPerameter("@CompanyName", UserAccount.CompName.ToString());
			}
			rpt.ShowReportWithDataTable(dt, "07_RptGatepassOutwardDriverInfoSlipA.rpt");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnattachment_Click(object sender, EventArgs e)
	{
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSMS_Click(object sender, EventArgs e)
	{
		try
		{
			CreateSMSTemplate createSMSTemplate = new CreateSMSTemplate(UserAccount);
			createSMSTemplate.ScreenName = ScreenName;
			createSMSTemplate.Show();
			createSMSTemplate.BringToFront();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 0)
			{
				FromDateHistory.Focus();
			}
			else
			{
				txtCNIC.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now.AddDays(-3.0);
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Clear();
			ToDocNoHistory.Clear();
			DataGridHistory.DataSource = null;
			drdocdate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryGridFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridFill()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		try
		{
			dtHistoryGrid.Rows.Clear();
			ReportsParameters driverInfo = new ReportsParameters();
			driverInfo.OrganizationId = UserAccount.OrganizationId;
			driverInfo.CompanyId = UserAccount.CompanyId;
			driverInfo.BranchesId = UserAccount.BranchesId;
			driverInfo.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			driverInfo.DocumentTypeId = ((RefDocumentTypeId == 0) ? 91 : RefDocumentTypeId);
			driverInfo.CanViewAllRecord = formright.DoHaveCanViewAllRecordRights;
			if (!driverInfo.CanViewAllRecord)
			{
				driverInfo.EntryUser = UserAccount.ID;
			}
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					driverInfo.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					driverInfo.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					driverInfo.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					driverInfo.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					driverInfo.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					driverInfo.ModifyDateTo = ToDateHistory.Value;
				}
			}
			driverInfo.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			driverInfo.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			DataTable dtGetAll = GatePassOutwardDriverInfo.FormHistory(driverInfo);
			if (dtGetAll.Rows.Count > 0)
			{
				new DataTable();
				if (dtHistoryGrid.Columns.Count == 0)
				{
					dtHistoryGrid.Columns.Add("Id", typeof(int));
					dtHistoryGrid.Columns.Add("DocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("GpDocumentType", typeof(string));
					dtHistoryGrid.Columns.Add("DriverCNIC", typeof(string));
					dtHistoryGrid.Columns.Add("DriverName", typeof(string));
					dtHistoryGrid.Columns.Add("FatherName", typeof(string));
					dtHistoryGrid.Columns.Add("ForwarderName", typeof(string));
					dtHistoryGrid.Columns.Add("DriverCellNo", typeof(string));
					dtHistoryGrid.Columns.Add("AlternateCellNo", typeof(string));
					dtHistoryGrid.Columns.Add("GpDocumentTypeId", typeof(int));
					dtHistoryGrid.Columns.Add("GatePassType", typeof(string));
					dtHistoryGrid.Columns.Add("GpSrNo", typeof(string));
					dtHistoryGrid.Columns.Add("GpDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("VehicleNo", typeof(string));
					dtHistoryGrid.Columns.Add("BiltyNo", typeof(string));
					dtHistoryGrid.Columns.Add("Container", typeof(string));
					dtHistoryGrid.Columns.Add("Container1", typeof(string));
					dtHistoryGrid.Columns.Add("FirstWeight", typeof(decimal));
					dtHistoryGrid.Columns.Add("SecondWeight", typeof(decimal));
					dtHistoryGrid.Columns.Add("NetWeight", typeof(decimal));
					dtHistoryGrid.Columns.Add("EntryDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("EntryUser", typeof(string));
					dtHistoryGrid.Columns.Add("ModifyDate", typeof(DateTime));
					dtHistoryGrid.Columns.Add("ModifyUser", typeof(string));
					dtHistoryGrid.Columns.Add("RemarksHeader", typeof(string));
					dtHistoryGrid.Columns.Add("NoOfAttachments", typeof(int));
				}
				foreach (DataRow row in dtGetAll.Rows)
				{
					dtHistoryGrid.Rows.Add(row["Id"], row["DocumentTypeId"], row["GpDocumentType"], row["DriverCNIC"], row["DriverName"], row["FatherName"], row["ForwarderName"], row["DriverCellNo"], row["AlternateCellNo"], row["GpDocumentTypeId"], row["GatepassType"], row["GpSrNo"], row["GpDate"], row["VehicleNo"], row["BiltyNo"], row["Container"], row["Container1"], row["FirstWeight"], row["SecondWeight"], row["NetWeight"], row["EntryDate"], row["EntryUser"], row["ModifyDate"], row["ModifyUser"], row["RemarksHeader"], row["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dtHistoryGrid;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
			}
			else
			{
				DataGridHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistorySetting()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["GpDocumentTypeId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			CommonServices.GridWrappingAndColumnSettings(DataGridHistory, 3);
			DataGridHistory.RootTable.Columns["GpDocumentType"].Width = Constants.InventoryConstants.DocumentType;
			DataGridHistory.RootTable.Columns["DriverCNIC"].Width = Constants.InventoryConstants.DriverCnicNoConstant;
			DataGridHistory.RootTable.Columns["DriverName"].Width = Constants.InventoryConstants.DriverName;
			DataGridHistory.RootTable.Columns["FatherName"].Width = Constants.InventoryConstants.DriverName;
			DataGridHistory.RootTable.Columns["ForwarderName"].Width = Constants.InventoryConstants.DriverName;
			DataGridHistory.RootTable.Columns["DriverCellNo"].Width = Constants.InventoryConstants.CellNo;
			DataGridHistory.RootTable.Columns["AlternateCellNo"].Width = Constants.InventoryConstants.CellNo;
			DataGridHistory.RootTable.Columns["GatePassType"].Width = Constants.InventoryConstants.GpType;
			DataGridHistory.RootTable.Columns["GpSrNo"].Width = Constants.InventoryConstants.GpNoConstant;
			DataGridHistory.RootTable.Columns["GpDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["VehicleNo"].Width = Constants.InventoryConstants.VehicleNoConstant;
			DataGridHistory.RootTable.Columns["BiltyNo"].Width = Constants.InventoryConstants.BiltyNoConstant;
			DataGridHistory.RootTable.Columns["Container"].Width = 100;
			DataGridHistory.RootTable.Columns["Container1"].Width = 100;
			DataGridHistory.RootTable.Columns["FirstWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["SecondWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["NetWeight"].Width = Constants.InventoryConstants.WeightConstant;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 30;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 35;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 40;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(id, ScreenName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				RECID = id;
				ReadById(RECID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				int id = Conversion.ToInt(r.Cells["Id"].Value);
				if (e.Column.Key == "Edit")
				{
					RECID = id;
					ReadById(RECID);
				}
				if (e.Column.Key == "Print")
				{
					GenerateReport(id);
				}
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
			GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, grdOutstandingGatePass, dtPendingGrid);
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
			GridLayoutService.EnsureGridLayoutMatchesDataTable(ScreenName, DataGridHistory, dtHistoryGrid);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDriverCellNo_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void txtAlternateCellNo_KeyPress(object sender, KeyPressEventArgs e)
	{
	}

	private void FromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void ToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void btnBrowse_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (Confiquration)
			{
				OpenFileDialog openFileDialog = new OpenFileDialog();
				openFileDialog.Filter = "(*.jpg; *.jpeg; *.PNG; *.png)| *.jpg; *.jpeg;  *.PNG;*.png; ";
				if (openFileDialog.ShowDialog() == DialogResult.OK)
				{
					DriverImageofd = openFileDialog.FileName;
					string UniqFileName = $"{Guid.NewGuid()}.Bmp";
					DriverImagefileSavePath = Path.Combine(saveDirectory, UniqFileName);
					Bitmap image = new Bitmap(openFileDialog.FileName);
					picBoxDriver.Image = image;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDriverImage_Click(object sender, EventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Expected O, but got Unknown
		try
		{
			if (comboBox1.Text != string.Empty)
			{
				videosource = new VideoCaptureDevice(captureDevice[comboBox1.SelectedIndex].MonikerString);
				videosource.NewFrame += new NewFrameEventHandler(videosource_NewFrame);
				videosource.Start();
				return;
			}
			comboBox1.Focus();
			throw new Exception("Please Select Web Cam ");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnPreview_Click(object sender, EventArgs e)
	{
		try
		{
			definition();
			if (!Confiquration)
			{
				return;
			}
			if (picBoxDriver.Image != null)
			{
				if (videosource != null)
				{
					videosource.Stop();
				}
				string UniqFileName = $"{Guid.NewGuid()}.Bmp";
				DriverImagefileSavePath = Path.Combine(saveDirectory, UniqFileName);
				Image image = picBoxDriver.Image;
				picBoxDriver.Image = image;
			}
			else
			{
				MessageBox.Show("Please Capture the Picture first");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetDriv_Click(object sender, EventArgs e)
	{
		try
		{
			if (videosource != null)
			{
				videosource.Stop();
			}
			picBoxDriver.Image = null;
			picBoxDriver.Invalidate();
			DriverImagefileSavePath = "";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDriverBio_KeyDown(object sender, KeyEventArgs e)
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
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtCNIC.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					FromDateHistory.Focus();
				}
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					cmbGatePass.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnattachment_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdOutstandingGatePass).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					txtCNIC.Focus();
				}
				if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
				{
					Print_Click(null, null);
				}
				if (e.Alt && (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1) && formright.DoHavePrintRights)
				{
					Print_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShowHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewHistory_Click(null, null);
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 82;
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Alt+1", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Gp No");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Pending GP Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on CNIC in Pending GP Grid when in Form tab and For focus on FromDate in history tab");
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

	private void grdOutstandingGatePass_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow r = grdOutstandingGatePass.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Control && e.KeyCode == Keys.Space && grdOutstandingGatePass.CurrentColumn != null && grdOutstandingGatePass.CurrentColumn.Key == "Load")
			{
				cmbGatePass.Value = Conversion.ToInt(r.Cells["Id"].Value);
				cmbGatePass_TextChanged(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || (int)item.RowType != 0)
			{
				return;
			}
			int CuurID = Conversion.ToInt(item.Cells["Id"].Value);
			if (e.Control && e.KeyCode == Keys.Return)
			{
				FormReset();
				ReadById(CuurID);
			}
			else if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					FormReset();
					ReadById(CuurID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					GenerateReport(CuurID);
				}
				else if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByScreenName(CuurID, ScreenName);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtCNIC_Leave(object sender, EventArgs e)
	{
		try
		{
			DriverBioGlobal obj = clsGlobalVariables.globalAllDriverBioInfo.Find((DriverBioGlobal row) => row.CnicNo == txtCNIC.Text.Trim());
			if (obj != null)
			{
				txtDriverCellNo.Text = obj.DriverCellNo;
				txtDriverName.Text = obj.DriverName;
				txtAlternateCellNo.Text = obj.AlternateCellNo;
				txtFatherName.Text = obj.FatherName;
				DriverFieldsDisableOrEnable(flag: false);
			}
			else
			{
				DriverFieldsDisableOrEnable(flag: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DriverFieldsDisableOrEnable(bool flag)
	{
		try
		{
			txtCNIC.Enabled = flag;
			txtDriverCellNo.Enabled = flag;
			txtDriverName.Enabled = flag;
			txtAlternateCellNo.Enabled = flag;
			txtFatherName.Enabled = flag;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ResetDriverFields()
	{
		try
		{
			txtCNIC.Text = string.Empty;
			txtDriverCellNo.Text = string.Empty;
			txtDriverName.Text = string.Empty;
			txtAlternateCellNo.Text = string.Empty;
			txtFatherName.Text = string.Empty;
			txtDriverCellNo.Text = "0092-3-";
			txtAlternateCellNo.Text = "0092-3-";
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtDriverCellNo_Leave(object sender, EventArgs e)
	{
		try
		{
			DriverBioGlobal obj = clsGlobalVariables.globalAllDriverBioInfo.Find((DriverBioGlobal row) => row.DriverCellNo == txtDriverCellNo.Text.Trim());
			if (obj != null)
			{
				txtCNIC.Text = obj.CnicNo;
				txtDriverName.Text = obj.DriverName;
				txtAlternateCellNo.Text = obj.AlternateCellNo;
				txtFatherName.Text = obj.FatherName;
				DriverFieldsDisableOrEnable(flag: false);
			}
			else
			{
				DriverFieldsDisableOrEnable(flag: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnResetDriverInfo_Click(object sender, EventArgs e)
	{
		try
		{
			clsGlobalVariables.globalAllDriverBioInfo = driverBiodata.ReadAll(UserAccount.OrganizationId, UserAccount.CompanyId);
			ResetDriverFields();
			DriverFieldsDisableOrEnable(flag: true);
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
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Expected O, but got Unknown
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_033a: Expected O, but got Unknown
		//IL_09e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_09ef: Expected O, but got Unknown
		//IL_35c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_35cc: Expected O, but got Unknown
		//IL_35d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35e3: Expected O, but got Unknown
		GridEXLayout grdOutstandingGatePass_DesignTimeLayout = new GridEXLayout();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance120 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.Sale.frmDriverBio));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel5 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdOutstandingGatePass = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.btnResetDriverInfo = new System.Windows.Forms.PictureBox();
		this.btnBrowse = new System.Windows.Forms.Button();
		this.txtAlternateCellNo = new System.Windows.Forms.MaskedTextBox();
		this.txtDriverCellNo = new System.Windows.Forms.MaskedTextBox();
		this.txtCNIC = new System.Windows.Forms.MaskedTextBox();
		this.btnResetDriv = new System.Windows.Forms.Button();
		this.btnReset = new System.Windows.Forms.Button();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.picFPImg = new System.Windows.Forms.PictureBox();
		this.btnFpCapture = new System.Windows.Forms.Button();
		this.btnFpStart = new System.Windows.Forms.Button();
		this.label13 = new System.Windows.Forms.Label();
		this.txtBiltyNo = new System.Windows.Forms.TextBox();
		this.btnPreview = new System.Windows.Forms.Button();
		this.btnDriverImage = new System.Windows.Forms.Button();
		this.grp = new System.Windows.Forms.GroupBox();
		this.picBoxDriver = new System.Windows.Forms.PictureBox();
		this.label12 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label11 = new System.Windows.Forms.Label();
		this.label10 = new System.Windows.Forms.Label();
		this.txtDriverName = new System.Windows.Forms.TextBox();
		this.label9 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.txtFatherName = new System.Windows.Forms.TextBox();
		this.label6 = new System.Windows.Forms.Label();
		this.label3 = new System.Windows.Forms.Label();
		this.txtForwarderName = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtSealNo = new System.Windows.Forms.TextBox();
		this.txtNetWeight = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtVehicleNo = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtContainer = new System.Windows.Forms.TextBox();
		this.txtGatepassDate = new System.Windows.Forms.DateTimePicker();
		this.cmbGatePass = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label14 = new System.Windows.Forms.Label();
		this.comboBox1 = new System.Windows.Forms.ComboBox();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnSMS = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label16 = new System.Windows.Forms.Label();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label51 = new System.Windows.Forms.Label();
		this.label52 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.label2 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel5.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdOutstandingGatePass).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.btnResetDriverInfo).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picFPImg).BeginInit();
		this.grp.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.picBoxDriver).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePass).BeginInit();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1134, 511);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1134, 511);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage1.Size = new System.Drawing.Size(1126, 483);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(4, 3);
		this.panel2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1118, 477);
		this.panel2.TabIndex = 0;
		this.panel5.Controls.Add(this.panel6);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(0, 238);
		this.panel5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1118, 239);
		this.panel5.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass);
		this.panel6.Controls.Add(this.panel11);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1118, 239);
		this.panel6.TabIndex = 0;
		this.grdOutstandingGatePass.AllowEdit = (InheritableBoolean)2;
		grdOutstandingGatePass_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdOutstandingGatePass.DesignTimeLayout = grdOutstandingGatePass_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdOutstandingGatePass.DynamicFiltering = true;
		this.grdOutstandingGatePass.FilterMode = (FilterMode)2;
		this.grdOutstandingGatePass.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdOutstandingGatePass.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).Location = new System.Drawing.Point(0, 27);
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).Name = "grdOutstandingGatePass";
		((ScrollableBase)this.grdOutstandingGatePass).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).Size = new System.Drawing.Size(1118, 212);
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).TabIndex = 0;
		this.grdOutstandingGatePass.TotalRow = (InheritableBoolean)1;
		this.grdOutstandingGatePass.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdOutstandingGatePass).VisualStyle = (VisualStyle)4;
		this.grdOutstandingGatePass.ColumnButtonClick += new ColumnActionEventHandler(grdOutstandingGatePass_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdOutstandingGatePass).KeyDown += new System.Windows.Forms.KeyEventHandler(grdOutstandingGatePass_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar1);
		this.panel11.Controls.Add(this.label15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1118, 27);
		this.panel11.TabIndex = 85;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1069, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar1.MyGrid = this.grdOutstandingGatePass;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(43, 24);
		this.ctrlGrdBar1.TabIndex = 11;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label15.AutoSize = true;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.Color.White;
		this.label15.Location = new System.Drawing.Point(5, 3);
		this.label15.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(161, 19);
		this.label15.TabIndex = 0;
		this.label15.Text = "Pending Gate Pass";
		this.panel4.Controls.Add(this.btnResetDriverInfo);
		this.panel4.Controls.Add(this.btnBrowse);
		this.panel4.Controls.Add(this.txtAlternateCellNo);
		this.panel4.Controls.Add(this.txtDriverCellNo);
		this.panel4.Controls.Add(this.txtCNIC);
		this.panel4.Controls.Add(this.btnResetDriv);
		this.panel4.Controls.Add(this.btnReset);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.btnFpCapture);
		this.panel4.Controls.Add(this.btnFpStart);
		this.panel4.Controls.Add(this.label13);
		this.panel4.Controls.Add(this.txtBiltyNo);
		this.panel4.Controls.Add(this.btnPreview);
		this.panel4.Controls.Add(this.btnDriverImage);
		this.panel4.Controls.Add(this.grp);
		this.panel4.Controls.Add(this.label12);
		this.panel4.Controls.Add(this.txtRemarks);
		this.panel4.Controls.Add(this.label11);
		this.panel4.Controls.Add(this.label10);
		this.panel4.Controls.Add(this.txtDriverName);
		this.panel4.Controls.Add(this.label9);
		this.panel4.Controls.Add(this.label7);
		this.panel4.Controls.Add(this.txtFatherName);
		this.panel4.Controls.Add(this.label6);
		this.panel4.Controls.Add(this.label3);
		this.panel4.Controls.Add(this.txtForwarderName);
		this.panel4.Controls.Add(this.label1);
		this.panel4.Controls.Add(this.txtSealNo);
		this.panel4.Controls.Add(this.txtNetWeight);
		this.panel4.Controls.Add(this.label17);
		this.panel4.Controls.Add(this.label8);
		this.panel4.Controls.Add(this.txtVehicleNo);
		this.panel4.Controls.Add(this.label5);
		this.panel4.Controls.Add(this.txtContainer);
		this.panel4.Controls.Add(this.txtGatepassDate);
		this.panel4.Controls.Add((System.Windows.Forms.Control)(object)this.cmbGatePass);
		this.panel4.Controls.Add(this.label4);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(0, 50);
		this.panel4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1118, 188);
		this.panel4.TabIndex = 0;
		this.btnResetDriverInfo.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnResetDriverInfo.Location = new System.Drawing.Point(375, 5);
		this.btnResetDriverInfo.Name = "btnResetDriverInfo";
		this.btnResetDriverInfo.Size = new System.Drawing.Size(25, 24);
		this.btnResetDriverInfo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
		this.btnResetDriverInfo.TabIndex = 84;
		this.btnResetDriverInfo.TabStop = false;
		this.btnResetDriverInfo.Click += new System.EventHandler(btnResetDriverInfo_Click);
		this.btnBrowse.BackColor = System.Drawing.Color.Teal;
		this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnBrowse.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnBrowse.ForeColor = System.Drawing.Color.White;
		this.btnBrowse.Location = new System.Drawing.Point(771, 159);
		this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnBrowse.Name = "btnBrowse";
		this.btnBrowse.Size = new System.Drawing.Size(73, 25);
		this.btnBrowse.TabIndex = 17;
		this.btnBrowse.Text = "Browse";
		this.btnBrowse.UseVisualStyleBackColor = false;
		this.btnBrowse.Click += new System.EventHandler(btnBrowse_Click);
		this.txtAlternateCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAlternateCellNo.Location = new System.Drawing.Point(405, 108);
		this.txtAlternateCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtAlternateCellNo.Mask = "0092-300-0000000";
		this.txtAlternateCellNo.Name = "txtAlternateCellNo";
		this.txtAlternateCellNo.Size = new System.Drawing.Size(168, 23);
		this.txtAlternateCellNo.TabIndex = 12;
		this.txtDriverCellNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverCellNo.Location = new System.Drawing.Point(405, 32);
		this.txtDriverCellNo.Margin = new System.Windows.Forms.Padding(2);
		this.txtDriverCellNo.Mask = "0092-300-0000000";
		this.txtDriverCellNo.Name = "txtDriverCellNo";
		this.txtDriverCellNo.Size = new System.Drawing.Size(168, 23);
		this.txtDriverCellNo.TabIndex = 9;
		this.txtDriverCellNo.Leave += new System.EventHandler(txtDriverCellNo_Leave);
		this.txtCNIC.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtCNIC.Location = new System.Drawing.Point(405, 6);
		this.txtCNIC.Margin = new System.Windows.Forms.Padding(2);
		this.txtCNIC.Mask = "00000-0000000-0";
		this.txtCNIC.Name = "txtCNIC";
		this.txtCNIC.Size = new System.Drawing.Size(168, 23);
		this.txtCNIC.TabIndex = 8;
		this.txtCNIC.Leave += new System.EventHandler(txtCNIC_Leave);
		this.btnResetDriv.BackColor = System.Drawing.Color.Teal;
		this.btnResetDriv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnResetDriv.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnResetDriv.ForeColor = System.Drawing.Color.White;
		this.btnResetDriv.Location = new System.Drawing.Point(711, 159);
		this.btnResetDriv.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnResetDriv.Name = "btnResetDriv";
		this.btnResetDriv.Size = new System.Drawing.Size(61, 25);
		this.btnResetDriv.TabIndex = 16;
		this.btnResetDriv.Text = "Reset";
		this.btnResetDriv.UseVisualStyleBackColor = false;
		this.btnResetDriv.Click += new System.EventHandler(btnResetDriv_Click);
		this.btnReset.BackColor = System.Drawing.Color.Teal;
		this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnReset.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnReset.ForeColor = System.Drawing.Color.White;
		this.btnReset.Location = new System.Drawing.Point(1031, 159);
		this.btnReset.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnReset.Name = "btnReset";
		this.btnReset.Size = new System.Drawing.Size(65, 25);
		this.btnReset.TabIndex = 20;
		this.btnReset.Text = "Reset";
		this.btnReset.UseVisualStyleBackColor = false;
		this.btnReset.Click += new System.EventHandler(btnReset_Click);
		this.groupBox1.Controls.Add(this.picFPImg);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(850, 5);
		this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.groupBox1.Size = new System.Drawing.Size(265, 151);
		this.groupBox1.TabIndex = 83;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "FingerPrint";
		this.picFPImg.Dock = System.Windows.Forms.DockStyle.Fill;
		this.picFPImg.Location = new System.Drawing.Point(4, 19);
		this.picFPImg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.picFPImg.Name = "picFPImg";
		this.picFPImg.Size = new System.Drawing.Size(257, 129);
		this.picFPImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picFPImg.TabIndex = 80;
		this.picFPImg.TabStop = false;
		this.btnFpCapture.BackColor = System.Drawing.Color.Teal;
		this.btnFpCapture.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFpCapture.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFpCapture.ForeColor = System.Drawing.Color.White;
		this.btnFpCapture.Location = new System.Drawing.Point(952, 159);
		this.btnFpCapture.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnFpCapture.Name = "btnFpCapture";
		this.btnFpCapture.Size = new System.Drawing.Size(78, 25);
		this.btnFpCapture.TabIndex = 19;
		this.btnFpCapture.Text = "Capture";
		this.btnFpCapture.UseVisualStyleBackColor = false;
		this.btnFpCapture.Click += new System.EventHandler(btnFpCapture_Click);
		this.btnFpStart.BackColor = System.Drawing.Color.Teal;
		this.btnFpStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnFpStart.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnFpStart.ForeColor = System.Drawing.Color.White;
		this.btnFpStart.Location = new System.Drawing.Point(887, 159);
		this.btnFpStart.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnFpStart.Name = "btnFpStart";
		this.btnFpStart.Size = new System.Drawing.Size(64, 25);
		this.btnFpStart.TabIndex = 18;
		this.btnFpStart.Text = "Start";
		this.btnFpStart.UseVisualStyleBackColor = false;
		this.btnFpStart.Click += new System.EventHandler(btnFpStart_Click);
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(6, 61);
		this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(37, 15);
		this.label13.TabIndex = 79;
		this.label13.Text = "Bilty#";
		this.txtBiltyNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBiltyNo.Enabled = false;
		this.txtBiltyNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBiltyNo.Location = new System.Drawing.Point(88, 57);
		this.txtBiltyNo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtBiltyNo.Name = "txtBiltyNo";
		this.txtBiltyNo.Size = new System.Drawing.Size(217, 23);
		this.txtBiltyNo.TabIndex = 3;
		this.txtBiltyNo.TabStop = false;
		this.btnPreview.BackColor = System.Drawing.Color.Teal;
		this.btnPreview.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPreview.ForeColor = System.Drawing.Color.White;
		this.btnPreview.Location = new System.Drawing.Point(635, 159);
		this.btnPreview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnPreview.Name = "btnPreview";
		this.btnPreview.Size = new System.Drawing.Size(76, 25);
		this.btnPreview.TabIndex = 15;
		this.btnPreview.Text = "Capture";
		this.btnPreview.UseVisualStyleBackColor = false;
		this.btnPreview.Click += new System.EventHandler(btnPreview_Click);
		this.btnDriverImage.BackColor = System.Drawing.Color.Teal;
		this.btnDriverImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnDriverImage.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDriverImage.ForeColor = System.Drawing.Color.White;
		this.btnDriverImage.Location = new System.Drawing.Point(579, 159);
		this.btnDriverImage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.btnDriverImage.Name = "btnDriverImage";
		this.btnDriverImage.Size = new System.Drawing.Size(55, 25);
		this.btnDriverImage.TabIndex = 14;
		this.btnDriverImage.Text = "Start";
		this.btnDriverImage.UseVisualStyleBackColor = false;
		this.btnDriverImage.Click += new System.EventHandler(btnDriverImage_Click);
		this.grp.Controls.Add(this.picBoxDriver);
		this.grp.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.grp.Location = new System.Drawing.Point(579, 5);
		this.grp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.grp.Name = "grp";
		this.grp.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.grp.Size = new System.Drawing.Size(265, 151);
		this.grp.TabIndex = 76;
		this.grp.TabStop = false;
		this.grp.Text = "Driver Picture";
		this.picBoxDriver.Dock = System.Windows.Forms.DockStyle.Fill;
		this.picBoxDriver.Location = new System.Drawing.Point(4, 19);
		this.picBoxDriver.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.picBoxDriver.Name = "picBoxDriver";
		this.picBoxDriver.Size = new System.Drawing.Size(257, 129);
		this.picBoxDriver.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
		this.picBoxDriver.TabIndex = 0;
		this.picBoxDriver.TabStop = false;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(6, 160);
		this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(52, 15);
		this.label12.TabIndex = 75;
		this.label12.Text = "Remarks";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(88, 160);
		this.txtRemarks.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(485, 23);
		this.txtRemarks.TabIndex = 7;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(309, 110);
		this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(84, 15);
		this.label11.TabIndex = 73;
		this.label11.Text = "Alternate Cell#";
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(309, 36);
		this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(71, 15);
		this.label10.TabIndex = 71;
		this.label10.Text = "Driver Cell #";
		this.txtDriverName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDriverName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDriverName.Location = new System.Drawing.Point(405, 57);
		this.txtDriverName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtDriverName.Name = "txtDriverName";
		this.txtDriverName.Size = new System.Drawing.Size(168, 23);
		this.txtDriverName.TabIndex = 10;
		this.txtDriverName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDriverCellNo_KeyPress);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(309, 10);
		this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(41, 15);
		this.label9.TabIndex = 69;
		this.label9.Text = "CNIC#";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(309, 86);
		this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(75, 15);
		this.label7.TabIndex = 67;
		this.label7.Text = "Father Name";
		this.txtFatherName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtFatherName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFatherName.Location = new System.Drawing.Point(405, 82);
		this.txtFatherName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtFatherName.Name = "txtFatherName";
		this.txtFatherName.Size = new System.Drawing.Size(168, 23);
		this.txtFatherName.TabIndex = 11;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(309, 61);
		this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(74, 15);
		this.label6.TabIndex = 65;
		this.label6.Text = "Driver Name";
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(309, 136);
		this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(95, 15);
		this.label3.TabIndex = 63;
		this.label3.Text = "Forwarder Name";
		this.txtForwarderName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtForwarderName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtForwarderName.Location = new System.Drawing.Point(405, 132);
		this.txtForwarderName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtForwarderName.Name = "txtForwarderName";
		this.txtForwarderName.Size = new System.Drawing.Size(168, 23);
		this.txtForwarderName.TabIndex = 13;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 113);
		this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 61;
		this.label1.Text = "Seal No";
		this.txtSealNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtSealNo.Enabled = false;
		this.txtSealNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtSealNo.Location = new System.Drawing.Point(88, 108);
		this.txtSealNo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtSealNo.Name = "txtSealNo";
		this.txtSealNo.Size = new System.Drawing.Size(217, 23);
		this.txtSealNo.TabIndex = 5;
		this.txtSealNo.TabStop = false;
		this.txtNetWeight.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtNetWeight.Enabled = false;
		this.txtNetWeight.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtNetWeight.Location = new System.Drawing.Point(88, 133);
		this.txtNetWeight.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtNetWeight.Name = "txtNetWeight";
		this.txtNetWeight.Size = new System.Drawing.Size(217, 23);
		this.txtNetWeight.TabIndex = 6;
		this.txtNetWeight.TabStop = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(6, 137);
		this.label17.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(68, 15);
		this.label17.TabIndex = 58;
		this.label17.Text = "Net Weight";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(6, 36);
		this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(64, 15);
		this.label8.TabIndex = 57;
		this.label8.Text = "Vehicle No";
		this.txtVehicleNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtVehicleNo.Enabled = false;
		this.txtVehicleNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtVehicleNo.Location = new System.Drawing.Point(88, 32);
		this.txtVehicleNo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtVehicleNo.Name = "txtVehicleNo";
		this.txtVehicleNo.Size = new System.Drawing.Size(217, 23);
		this.txtVehicleNo.TabIndex = 2;
		this.txtVehicleNo.TabStop = false;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(6, 86);
		this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(58, 15);
		this.label5.TabIndex = 56;
		this.label5.Text = "Container";
		this.txtContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtContainer.Enabled = false;
		this.txtContainer.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtContainer.Location = new System.Drawing.Point(88, 82);
		this.txtContainer.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtContainer.Name = "txtContainer";
		this.txtContainer.Size = new System.Drawing.Size(217, 23);
		this.txtContainer.TabIndex = 4;
		this.txtContainer.TabStop = false;
		this.txtGatepassDate.CustomFormat = "dd-MMM-yy";
		this.txtGatepassDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtGatepassDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtGatepassDate.Location = new System.Drawing.Point(187, 6);
		this.txtGatepassDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.txtGatepassDate.Name = "txtGatepassDate";
		this.txtGatepassDate.Size = new System.Drawing.Size(118, 22);
		this.txtGatepassDate.TabIndex = 1;
		this.cmbGatePass.AllowNull = (DefaultableBoolean)1;
		this.cmbGatePass.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbGatePass.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbGatePass.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance109).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance109).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Appearance = (AppearanceBase)(object)appearance109;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance110).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance110;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance111;
		((SpecialBoxBase)((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance112).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance112).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance113;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance114).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance114;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance115).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance115;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance116).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance116;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance117).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance117).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance117).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance117).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance117).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance117;
		((AppearanceBase)appearance118).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance119).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbGatePass).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbGatePass.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Location = new System.Drawing.Point(88, 4);
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Name = "cmbGatePass";
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Size = new System.Drawing.Size(98, 26);
		((System.Windows.Forms.Control)(object)this.cmbGatePass).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.cmbGatePass).Leave += new System.EventHandler(cmbGatePass_TextChanged);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 10);
		this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(72, 15);
		this.label4.TabIndex = 52;
		this.label4.Text = "GP # && Date";
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.ChkBox);
		this.panel3.Controls.Add(this.label14);
		this.panel3.Controls.Add(this.comboBox1);
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1118, 50);
		this.panel3.TabIndex = 0;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(1031, 29);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(84, 20);
		this.ChkBox.TabIndex = 85;
		this.ChkBox.Text = "Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(2, 30);
		this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(90, 19);
		this.label14.TabIndex = 0;
		this.label14.Text = "Driver Bio";
		this.comboBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.comboBox1.FormattingEnabled = true;
		this.comboBox1.Location = new System.Drawing.Point(898, 0);
		this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.comboBox1.Name = "comboBox1";
		this.comboBox1.Size = new System.Drawing.Size(210, 24);
		this.comboBox1.TabIndex = 84;
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.Print, this.btnattachment, this.btnSMS, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1118, 27);
		this.toolStrip1.TabIndex = 5;
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
		this.btnsave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnsave.Image = (System.Drawing.Image)resources.GetObject("btnsave.Image");
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(85, 24);
		this.Print.Text = "&Print_07";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnSMS.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSMS.Image = Architecture.WinApp.Properties.Resources.icons8_messaging_16;
		this.btnSMS.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSMS.Name = "btnSMS";
		this.btnSMS.Size = new System.Drawing.Size(60, 24);
		this.btnSMS.Text = "&SMS";
		this.btnSMS.Visible = false;
		this.btnSMS.Click += new System.EventHandler(btnSMS_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.tabPage2.Size = new System.Drawing.Size(1126, 483);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(4, 125);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1118, 355);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 46;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Controls.Add(this.label16);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(4, 98);
		this.panel12.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1118, 27);
		this.panel12.TabIndex = 86;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1069, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(43, 24);
		this.ctrlGrdBar2.TabIndex = 11;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.ForeColor = System.Drawing.Color.White;
		this.label16.Location = new System.Drawing.Point(5, 3);
		this.label16.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(142, 19);
		this.label16.TabIndex = 0;
		this.label16.Text = "Filtered Records";
		this.panel9.Controls.Add(this.groupBox2);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(4, 55);
		this.panel9.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1118, 43);
		this.panel9.TabIndex = 1;
		this.groupBox2.Controls.Add(this.rdmodifydate);
		this.groupBox2.Controls.Add(this.drdocdate);
		this.groupBox2.Controls.Add(this.rdentrydate);
		this.groupBox2.Controls.Add(this.btnShowHistory);
		this.groupBox2.Controls.Add(this.label51);
		this.groupBox2.Controls.Add(this.label52);
		this.groupBox2.Controls.Add(this.label53);
		this.groupBox2.Controls.Add(this.label54);
		this.groupBox2.Controls.Add(this.ToDocNoHistory);
		this.groupBox2.Controls.Add(this.FromDocNoHistory);
		this.groupBox2.Controls.Add(this.ToDateHistory);
		this.groupBox2.Controls.Add(this.FromDateHistory);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 1);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1109, 41);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Filters";
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(1012, 14);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 9;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(852, 14);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(68, 19);
		this.drdocdate.TabIndex = 7;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Gp Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(926, 14);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 8;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(736, 10);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 26);
		this.btnShowHistory.TabIndex = 6;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.Location = new System.Drawing.Point(573, 16);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(57, 15);
		this.label51.TabIndex = 86;
		this.label51.Text = "To Gp No";
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(393, 16);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(72, 15);
		this.label52.TabIndex = 85;
		this.label52.Text = "From Gp No";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(205, 16);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "To Date";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(3, 16);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(630, 12);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(102, 23);
		this.ToDocNoHistory.TabIndex = 4;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(466, 12);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(102, 23);
		this.FromDocNoHistory.TabIndex = 3;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(254, 12);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(136, 23);
		this.ToDateHistory.TabIndex = 2;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(66, 12);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(136, 23);
		this.FromDateHistory.TabIndex = 1;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Controls.Add(this.label2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(4, 3);
		this.panel7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1118, 52);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnNewHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1118, 27);
		this.toolStrip2.TabIndex = 6;
		this.toolStrip2.Text = "toolStrip2";
		this.BtnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.BtnNewHistory.Text = "&Reset";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.White;
		this.label2.Location = new System.Drawing.Point(4, 31);
		this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(153, 19);
		this.label2.TabIndex = 0;
		this.label2.Text = "Driver Bio History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(7f, 15f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1134, 511);
		base.Controls.Add(this.panel1);
		this.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
		base.Name = "frmDriverBio";
		this.Text = "EccountBookERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.Load += new System.EventHandler(frmDriverBio_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmDriverBio_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdOutstandingGatePass).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.btnResetDriverInfo).EndInit();
		this.groupBox1.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picFPImg).EndInit();
		this.grp.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.picBoxDriver).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbGatePass).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel9.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
