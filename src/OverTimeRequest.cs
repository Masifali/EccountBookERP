using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Architecture.BLL.HRM.EmployeeManagement;
using Architecture.BLL.HRM.OverTimeManagement;
using Architecture.BLL.HRM.ProfileManagement;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.HRM.OverTimeManagement;
using Architecture.Model.HRM.ProfileManagement;
using Architecture.WinApp.Account_Definition;
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
namespace Architecture.WinApp.HRM.OverTimeManagement
{

public class OverTimeRequest : Form
{
	private UserAccount UserAccount = new UserAccount();

	private Rightsobjects formright = new Rightsobjects();

	private int DefaultDaysToLessFromHistoryFromDate;

	private DropDownBind DDL = new DropDownBind();

	private DataTable dtdetail = new DataTable();

	private int RecId;

	private int updateDetailIndex;

	private Attachment AT = new Attachment();

	private DataTable dtEmployees = new DataTable();

	private EmpOverTimeLoadForRequest Loader;

	private List<OverTimeRequestDetail> lstRemoveDetailRecord = new List<OverTimeRequestDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private Panel panel4;

	private Panel panel3;

	private TabPage tabPage2;

	private ToolStrip toolstrip;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private Panel panel13;

	private Panel panel17;

	private ToolStrip toolStrip3;

	private ToolStripButton btnUpdate;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	private Button btnplus;

	private Label label16;

	private TextBox txtOTHours;

	private Label label10;

	internal UltraCombo cmbEmployee;

	private Label label9;

	private Label label3;

	private DateTimePicker txtRequestDate;

	internal UltraCombo cmbDepartmnet;

	private GroupBox groupBox1;

	private TextBox txtReason;

	private Label label34;

	private CtrlGrdBar ctrlGrdBar1;

	private Label label4;

	private DateTimePicker txtFromDateTime;

	private Panel panel18;

	private Panel panel5;

	private Label label51;

	private Label label7;

	private DateTimePicker txtOverTimeDate;

	private Label label5;

	internal UltraCombo cmbRequestedBy;

	private CheckBox IsOffDuty;

	private Label label8;

	internal UltraCombo cmbSection;

	private CheckBox IsMealAllow;

	private TextBox txtOTRate;

	private Label label2;

	private Label label1;

	private DateTimePicker txtToDateTime;

	private GroupBox groupBox2;

	private Panel panel6;

	private Label label6;

	private Panel panel10;

	private Panel panel9;

	private Label label11;

	private ToolStripButton btnRefresh;

	private CtrlGrdBar ctrlGrdBar2;

	private ToolStripButton toolStripButton1;

	private ToolStripButton btnhistoryrefresh;

	private Panel panel12;

	private GroupBox groupBox3;

	private Label label17;

	internal UltraCombo cmbrequestedbthistory;

	private Label label13;

	internal UltraCombo cmbsectionhistory;

	private Button btnShow;

	private Label label50;

	private Label label53;

	private Label label54;

	internal UltraCombo CmbDepartmentHistory;

	private DateTimePicker RequestedDateTo;

	private DateTimePicker Requesteddatefrom;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton rdovertimedate;

	private RadioButton rdrequesteddate;

	private RadioButton rdentrydate;

	internal GridEX grdHistoryDetail;

	internal GridEX DataGridHistory;

	internal GridEX grdDetails;

	private Panel panel7;

	private Label label12;

	private Label label14;

	private ToolStripButton toolStripButton2;

	private ToolStripButton btnprint;

	private CheckBox ChkBox;

	private ToolStripButton btnshortcutkeys;

	private ToolStripButton btnLoader;

	public OverTimeRequest(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void FrmExportSalesContract_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(970, 600);
			formright = CommonServices.SetRightsValueInRightsObject("OverTimeRequest");
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnprint.Enabled = formright.DoHavePrintRights;
			ChkBox.Checked = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar1.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar1.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar1.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			ctrlGrdBar2.mGridChooseFielder.Enabled = formright.DoHaveFieldChooserRights;
			ctrlGrdBar2.mGridSaveLayouts.Enabled = formright.DoHaveSaveLayoutRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.GroupCollaps.Enabled = formright.DoHaveGridGroupCollapseRights;
			ctrlGrdBar2.GroupExpand.Enabled = formright.DoHaveGridGroupExpandRights;
			DepartmentFill();
			SectionFill();
			RequestedByFill();
			dtdetail.Columns.Add("Id", typeof(long));
			dtdetail.Columns.Add("EmployeeId", typeof(long));
			dtdetail.Columns.Add("EmployeeName");
			dtdetail.Columns.Add("FromDateTime", typeof(DateTime));
			dtdetail.Columns.Add("ToDateTime", typeof(DateTime));
			dtdetail.Columns.Add("OTHours", typeof(double));
			dtdetail.Columns.Add("OTRate", typeof(double));
			dtdetail.Columns.Add("IsAllowMeal", typeof(bool));
			dtdetail.Columns.Add("EntryByLoader", typeof(bool));
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
			grdSettings();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtRequestDate.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			Requesteddatefrom.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			RequestedDateTo.Value = DateTime.Now;
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
			GridEXRow row = grdDetails.GetRow(0);
			grdDetails.RootTable.Columns["Id"].Visible = false;
			grdDetails.RootTable.Columns["EmployeeId"].Visible = false;
			grdDetails.RootTable.Columns["OTHours"].Visible = false;
			grdDetails.RootTable.Columns["OTRate"].Visible = false;
			grdDetails.RootTable.Columns["IsAllowMeal"].Visible = false;
			grdDetails.RootTable.Columns["EntryByLoader"].Visible = false;
			grdDetails.RootTable.Columns["FromDateTime"].Caption = "From Time";
			grdDetails.RootTable.Columns["ToDateTime"].Caption = "To Time";
			grdDetails.RootTable.Columns["FromDateTime"].FormatString = "hh:mm tt";
			grdDetails.RootTable.Columns["ToDateTime"].FormatString = "hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(grdDetails);
			grdDetails.RootTable.Columns.Add("Delete");
			grdDetails.RootTable.Columns["Delete"].Key = "Delete";
			grdDetails.RootTable.Columns["Delete"].Caption = "X";
			grdDetails.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetails.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetails.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetails.RootTable.Columns["Delete"].Width = 30;
			if (!Conversion.ToBool(row.Cells["EntryByLoader"].Value))
			{
				grdDetails.RootTable.Columns.Add("Edit");
				grdDetails.RootTable.Columns["Edit"].Key = "Edit";
				grdDetails.RootTable.Columns["Edit"].Caption = "Edit";
				grdDetails.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				grdDetails.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				grdDetails.RootTable.Columns["Edit"].ButtonText = "Edit";
				grdDetails.RootTable.Columns["Edit"].Width = 40;
				grdDetails.RootTable.Columns["Edit"].Position = 0;
				grdDetails.RootTable.Columns["Delete"].Position = 1;
				grdDetails.FrozenColumns = 2;
			}
			else
			{
				grdDetails.RootTable.Columns["Delete"].Position = 0;
				grdDetails.FrozenColumns = 1;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetails.CurrentRow;
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (grdDetails.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					OverTimeRequestDetail detail = new OverTimeRequestDetail();
					detail.OverTimeRequestDetailId = Conversion.ToInt64(r.Cells["Id"].Value);
					detail.EmployeeId = Conversion.ToInt64(r.Cells["EmployeeId"].Value);
					detail.FromDateTime = Conversion.ToDateTime(r.Cells["FromDateTime"].Value);
					detail.ToDateTime = Conversion.ToDateTime(r.Cells["ToDateTime"].Value);
					detail.OTHours = Conversion.ToInt64(r.Cells["OTHours"].Value);
					detail.OTRate = Conversion.ToDecimal(r.Cells["OTRate"].Value);
					detail.IsAllowMeal = Conversion.ToBool(r.Cells["IsAllowMeal"].Value);
					detail.RequestStatus = Conversion.ToBool(r.Cells["EntryByLoader"].Value);
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grdDetails.UpdateData();
				if (grdDetails.GetRows().Count() == 0)
				{
					((Control)(object)cmbDepartmnet).Enabled = true;
					txtOverTimeDate.Enabled = true;
				}
			}
			if (e.Column.Key == "Edit")
			{
				grdDetails_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)cmbDepartmnet).ActiveRow == null || Conversion.ToInt(cmbDepartmnet.Value) == 0)
		{
			MessageBox.Show("Department Required");
			cmbDepartmnet.Focus();
			return false;
		}
		if (((UltraGridBase)cmbSection).ActiveRow == null || Conversion.ToInt(cmbSection.Value) == 0)
		{
			MessageBox.Show("Section Required");
			cmbSection.Focus();
			return false;
		}
		if (txtReason.Text == string.Empty)
		{
			MessageBox.Show("Reason Required");
			txtReason.Focus();
			return false;
		}
		if (((UltraGridBase)cmbRequestedBy).ActiveRow == null || Conversion.ToInt(cmbRequestedBy.Value) == 0)
		{
			MessageBox.Show("Requested By Required");
			cmbRequestedBy.Focus();
			return false;
		}
		return true;
	}

	private bool DetailFormValidation()
	{
		if (((UltraGridBase)cmbEmployee).ActiveRow == null || Conversion.ToInt(cmbEmployee.Value) == 0)
		{
			MessageBox.Show("Employee Required");
			cmbEmployee.Focus();
			return false;
		}
		return true;
	}

	private void DepartmentFill()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			int ItId = Conversion.ToInt(cmbDepartmnet.Value);
			int ItId2 = Conversion.ToInt(CmbDepartmentHistory.Value);
			DataTable dtDepartment = genDepartment.Getall(new genDepartment
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDepartment.Rows.Count > 0)
			{
				for (int i = 0; i < dtDepartment.Rows.Count; i++)
				{
					dt.Rows.Add(dtDepartment.Rows[i]["DepartmentId"], dtDepartment.Rows[i]["DepartmentName"]);
				}
				DDL.BindDDL(dt, cmbDepartmnet, "Id", "Name", "Department", ZeroIndex: true);
				DDL.BindDDL(dt, CmbDepartmentHistory, "Id", "Name", "Department", ZeroIndex: true);
				((UltraGridBase)cmbDepartmnet).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbDepartmentHistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					cmbDepartmnet.Value = ItId;
				}
				else
				{
					((Control)(object)cmbDepartmnet).Text = string.Empty;
				}
			}
			if (ItId2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId2))
				{
					CmbDepartmentHistory.Value = ItId2;
				}
				else
				{
					((Control)(object)CmbDepartmentHistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void SectionFill()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		try
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			int ItId = Conversion.ToInt(cmbSection.Value);
			int Id1 = Conversion.ToInt(cmbsectionhistory.Value);
			DataTable dtSection = genSection.Getall(new genSection
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtSection.Rows.Count > 0)
			{
				for (int i = 0; i < dtSection.Rows.Count; i++)
				{
					dt.Rows.Add(dtSection.Rows[i]["SectionId"], dtSection.Rows[i]["SectionName"]);
				}
				DDL.BindDDL(dt, cmbSection, "Id", "Name", "Section", ZeroIndex: true);
				DDL.BindDDL(dt, cmbsectionhistory, "Id", "Name", "Section", ZeroIndex: true);
				((UltraGridBase)cmbSection).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbsectionhistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (ItId > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId))
				{
					cmbSection.Value = ItId;
				}
				else
				{
					((Control)(object)cmbSection).Text = string.Empty;
				}
			}
			if (Id1 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id1))
				{
					cmbsectionhistory.Value = Id1;
				}
				else
				{
					((Control)(object)cmbsectionhistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void RequestedByFill()
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		try
		{
			dtEmployees.Rows.Clear();
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("Name", typeof(string));
			int ItId1 = Conversion.ToInt(cmbRequestedBy.Value);
			int ItId2 = Conversion.ToInt(cmbrequestedbthistory.Value);
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtEmployees = genEmployee.Getall(obj);
			if (dtEmployees.Rows.Count > 0)
			{
				for (int i = 0; i < dtEmployees.Rows.Count; i++)
				{
					dt.Rows.Add(dtEmployees.Rows[i]["EmployeeId"], dtEmployees.Rows[i]["EmployeeName"]);
				}
				DDL.BindDDL(dt, cmbRequestedBy, "Id", "Name", "Requested By", ZeroIndex: true);
				DDL.BindDDL(dt, cmbrequestedbthistory, "Id", "Name", "Requested By", ZeroIndex: true);
				((UltraGridBase)cmbRequestedBy).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)cmbrequestedbthistory).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
			}
			if (ItId1 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId1))
				{
					cmbRequestedBy.Value = ItId1;
				}
				else
				{
					((Control)(object)cmbRequestedBy).Text = string.Empty;
				}
			}
			if (ItId2 > 0)
			{
				if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ItId2))
				{
					cmbrequestedbthistory.Value = ItId2;
				}
				else
				{
					((Control)(object)cmbrequestedbthistory).Text = string.Empty;
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnsave_Click(object sender, EventArgs e)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			OverTimeRequest obj = new OverTimeRequest();
			if (RecId > 0)
			{
				obj.OverTimeRequestId = RecId;
			}
			obj.RequestDate = Conversion.ToDateTime((object)txtRequestDate.Value);
			obj.OverTimeDate = Conversion.ToDateTime((object)txtOverTimeDate.Value);
			obj.DepartmentId = Conversion.ToInt(cmbDepartmnet.Value);
			obj.SectionId = Conversion.ToInt(cmbSection.Value);
			obj.IsOffDuty = Conversion.ToBool((object)IsOffDuty.Checked);
			obj.Reason = txtReason.Text;
			obj.RequestById = Conversion.ToInt(cmbRequestedBy.Value);
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.CreatedOn = DateTime.Now;
			obj.AlteredOn = DateTime.Now;
			obj.CreatedById = UserAccount.ID;
			obj.AlteredById = UserAccount.ID;
			obj.ApprovedOn = DateTime.Now;
			obj.ApprovedById = UserAccount.ID;
			obj.IsApproved = false;
			obj.OverTimeRequestDetailList = new List<OverTimeRequestDetail>();
			if (lstRemoveDetailRecord.Count > 0)
			{
				for (int j = 0; j < lstRemoveDetailRecord.Count; j++)
				{
					obj.OverTimeRequestDetailList.Add(lstRemoveDetailRecord[j]);
				}
			}
			if (grdDetails.RowCount > 0)
			{
				GridEXRow[] rows = grdDetails.GetRows();
				foreach (GridEXRow r in rows)
				{
					OverTimeRequestDetail vd = new OverTimeRequestDetail();
					vd.OverTimeRequestDetailId = Conversion.ToInt64(r.Cells["Id"].Value);
					vd.ActionTypeId = ((vd.OverTimeRequestDetailId <= 0) ? 1 : 2);
					vd.EmployeeId = Conversion.ToInt64(r.Cells["EmployeeId"].Value);
					string overTimeDate = obj.OverTimeDate.ToShortDateString();
					string FromTime = Conversion.ToDateTime(r.Cells["FromDateTime"].Value).ToString("hh:mm tt");
					vd.FromDateTime = Conversion.ToDateTime((object)(overTimeDate + " " + FromTime));
					string ToTime = Conversion.ToDateTime(r.Cells["ToDateTime"].Value).ToString("hh:mm tt");
					vd.ToDateTime = Conversion.ToDateTime((object)(overTimeDate + " " + ToTime));
					vd.OTHours = Conversion.ToInt64(r.Cells["OTHours"].Value);
					vd.OTRate = Conversion.ToDecimal(r.Cells["OTRate"].Value);
					vd.IsAllowMeal = Conversion.ToBool(r.Cells["IsAllowMeal"].Value);
					vd.CreatedOn = DateTime.Now;
					vd.AlteredOn = DateTime.Now;
					vd.CreatedById = UserAccount.ID;
					vd.AlteredById = UserAccount.ID;
					vd.IsApproved = false;
					vd.RequestStatus = Conversion.ToBool(r.Cells["EntryByLoader"].Value);
					obj.OverTimeRequestDetailList.Add(vd);
				}
				obj.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
				for (int i = 0; i < Attachment.dtAttachments.Rows.Count; i++)
				{
					DMSAttachments attach = new DMSAttachments();
					attach.Attachment = Path.GetFileName(Attachment.dtAttachments.Rows[i][1].ToString());
					attach.ScreenName = base.Name;
					obj.AttachmentsList.Add(attach);
					if (!Directory.Exists(clsGlobalVariables.AttachmentFolderPath))
					{
						throw new Exception("Attachment Folder Path Not Configure Properly");
					}
					string fileSavePath = Path.Combine(clsGlobalVariables.AttachmentFolderPath, attach.Attachment);
					if (!File.Exists(fileSavePath))
					{
						File.Copy(Attachment.dtAttachments.Rows[i][1].ToString(), fileSavePath, overwrite: true);
					}
				}
				long success = (obj.RequestById = OverTimeRequest.Save(obj));
				if (RecId > 0)
				{
					MessageBox.Show("Data Update Successfully");
					if (AT.RemovedAttachmentListInUpdateCase.Count > 0)
					{
						for (int k = 0; k < AT.RemovedAttachmentListInUpdateCase.Count; k++)
						{
							try
							{
								using (FileStream fs = new FileStream(AT.RemovedAttachmentListInUpdateCase[k].Path, FileMode.Open, FileAccess.ReadWrite))
								{
									fs.Flush();
								}
								File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
							}
							catch (IOException)
							{
								int attempts = 0;
								bool fileDeleted = false;
								while (!fileDeleted && attempts < 5)
								{
									try
									{
										Thread.Sleep(500);
										File.Delete(AT.RemovedAttachmentListInUpdateCase[k].Path);
										fileDeleted = true;
									}
									catch (IOException)
									{
										attempts++;
									}
								}
								if (!fileDeleted)
								{
									throw new Exception("Unable to delete file. It is in use by another process.");
								}
							}
						}
					}
				}
				else
				{
					MessageBox.Show("Data Save Successfully");
				}
				Reset();
				if (ChkBox.Checked)
				{
					CommonServices.OverTimeSlip((int)success);
				}
				return;
			}
			throw new Exception("Grid record not found");
		}
		catch (Exception ex3)
		{
			MessageBox.Show(ex3.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
			ReadById(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ReadById(int ID)
	{
		try
		{
			RecId = ID;
			OverTimeRequest Obj = OverTimeRequest.GetByID(ID);
			if (Obj != null)
			{
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				txtRequestDate.Value = Obj.RequestDate;
				txtOverTimeDate.Value = Obj.OverTimeDate;
				cmbDepartmnet.Value = Conversion.ToInt((object)Obj.DepartmentId);
				cmbDepartmnet_Leave(null, null);
				cmbSection.Value = Conversion.ToInt((object)Obj.SectionId);
				IsOffDuty.Checked = Conversion.ToBool((object)Obj.IsOffDuty);
				txtReason.Text = Conversion.ToString((object)Obj.Reason);
				cmbRequestedBy.Value = Conversion.ToInt((object)Obj.RequestById);
				dtdetail.Rows.Clear();
				for (int i = 0; i < Obj.OverTimeRequestDetailList.Count; i++)
				{
					dtdetail.Rows.Add(Obj.OverTimeRequestDetailList[i].OverTimeRequestDetailId, Obj.OverTimeRequestDetailList[i].EmployeeId, Obj.OverTimeRequestDetailList[i].EmployeeName, Obj.OverTimeRequestDetailList[i].FromDateTime, Obj.OverTimeRequestDetailList[i].ToDateTime, Obj.OverTimeRequestDetailList[i].OTHours, Obj.OverTimeRequestDetailList[i].OTRate, Obj.OverTimeRequestDetailList[i].IsAllowMeal, Obj.OverTimeRequestDetailList[i].RequestStatus);
				}
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			if (rdrequesteddate.Checked)
			{
				if (Requesteddatefrom.Checked)
				{
					obj.FromDate = Requesteddatefrom.Value;
				}
				if (RequestedDateTo.Checked)
				{
					obj.ToDate = RequestedDateTo.Value;
				}
			}
			else if (rdovertimedate.Checked)
			{
				if (Requesteddatefrom.Checked)
				{
					obj.GpDateF = Requesteddatefrom.Value;
				}
				if (RequestedDateTo.Checked)
				{
					obj.GpDateT = RequestedDateTo.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (Requesteddatefrom.Checked)
				{
					obj.EntryDateFrom = Requesteddatefrom.Value;
				}
				if (RequestedDateTo.Checked)
				{
					obj.EntryDateTo = RequestedDateTo.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (Requesteddatefrom.Checked)
				{
					obj.ModifyDateFrom = Requesteddatefrom.Value;
				}
				if (RequestedDateTo.Checked)
				{
					obj.ModifyDateTo = RequestedDateTo.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (Requesteddatefrom.Checked)
				{
					obj.ApprovedDateFrom = Requesteddatefrom.Value;
				}
				if (RequestedDateTo.Checked)
				{
					obj.ApprovedDateTo = RequestedDateTo.Value;
				}
			}
			obj.SectionId = Conversion.ToInt(cmbsectionhistory.Value);
			obj.DepartmentId = Conversion.ToInt(CmbDepartmentHistory.Value);
			obj.RequestedById = Conversion.ToInt(cmbrequestedbthistory.Value);
			DataTable dtHistory = OverTimeRequest.ReadAll(obj);
			if (dtHistory.Rows.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("OverTimeRequestId", typeof(int));
				dt.Columns.Add("RequestDate", typeof(DateTime));
				dt.Columns.Add("OverTimeDate", typeof(DateTime));
				dt.Columns.Add("DepartmentName", typeof(string));
				dt.Columns.Add("SectionName", typeof(string));
				dt.Columns.Add("IsOffDuty", typeof(string));
				dt.Columns.Add("Reason", typeof(string));
				dt.Columns.Add("RequestedByPerson", typeof(string));
				dt.Columns.Add("EntryUser");
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser");
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ApprovedUser");
				dt.Columns.Add("ApprovedDate", typeof(DateTime));
				dt.Columns.Add("NoOfAttachment", typeof(int));
				for (int i = 0; i < dtHistory.Rows.Count; i++)
				{
					dt.Rows.Add(dtHistory.Rows[i]["OverTimeRequestId"], Conversion.ToDateTime(dtHistory.Rows[i]["RequestDate"]).ToString("dd-MMM-yyyy"), Conversion.ToDateTime(dtHistory.Rows[i]["OverTimeDate"]).ToString("dd-MMM-yyyy"), dtHistory.Rows[i]["DepartmentName"], dtHistory.Rows[i]["SectionName"], dtHistory.Rows[i]["IsOffDuty"], dtHistory.Rows[i]["Reason"], dtHistory.Rows[i]["RequestedByPerson"], dtHistory.Rows[i]["EntryUserName"], dtHistory.Rows[i]["CreatedOn"], dtHistory.Rows[i]["ModifyUserName"], dtHistory.Rows[i]["AlteredOn"], dtHistory.Rows[i]["ApprovedUserName"], dtHistory.Rows[i]["ApprovedOn"], dtHistory.Rows[i]["NoOfAttachments"]);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				DataGridHistorySetting();
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
			DataGridHistory.RootTable.Columns["OverTimeRequestId"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachment"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ApprovedDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			DataGridHistory.RootTable.Columns.Add("Edit");
			DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DataGridHistory.RootTable.Columns["Edit"].Width = 50;
			DataGridHistory.RootTable.Columns["Edit"].Position = 0;
			DataGridHistory.FrozenColumns = 1;
			DataGridHistory.RootTable.Columns.Add("Print");
			DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
			DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
			DataGridHistory.RootTable.Columns["Print"].Width = 50;
			DataGridHistory.RootTable.Columns["Print"].Position = 1;
			DataGridHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
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
			if (RecId == 0)
			{
				throw new Exception("Record Can't Update because no Id found");
			}
			btnsave_Click(null, null);
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
			if (tabControl1.SelectedIndex == 1)
			{
				Requesteddatefrom.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FrmExportSalesContract_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControl1.SelectedIndex == 1)
				{
					tabControl1.SelectedIndex = 0;
					txtRequestDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					Requesteddatefrom.Focus();
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
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdDetails_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					btnprint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					toolStripButton2_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click_1(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtRequestDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbEmployee.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetails).Focus();
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnShow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					DataGridHistory_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					toolStripButton1_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					Requesteddatefrom.Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save in Form Tab and For Show History in History Tab");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+F5", "For Focus on Requested Date");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid when focus in form tab and for focus on history grid when in history tab");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus on Employee in Detail Grid when in Form tab and For focus on FromDate in history tab");
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

	private void btnshortcutkeys_Click(object sender, EventArgs e)
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

	private void grdDetails_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetails.CurrentRow;
			if (item != null && !Conversion.ToBool(item.Cells["EntryByLoader"].Value))
			{
				updateDetailIndex = item.RowIndex;
				cmbEmployee.Value = item.Cells["EmployeeId"].Value;
				txtFromDateTime.Value = Conversion.ToDateTime(item.Cells["FromDateTime"].Value);
				txtToDateTime.Value = Conversion.ToDateTime(item.Cells["ToDateTime"].Value);
				txtOTHours.Text = item.Cells["OTHours"].Value.ToString();
				txtOTRate.Text = item.Cells["OTRate"].Value.ToString();
				IsMealAllow.Checked = Conversion.ToBool(item.Cells["IsAllowMeal"].Value);
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddtoGrid()
	{
		try
		{
			if (grdDetails.GetRows().Count() > 0 && Conversion.ToBool(grdDetails.GetRow(0).Cells["EntryByLoader"].Value))
			{
				MessageBox.Show("U Can't add Direct Entry because Entry From Loader Exist", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			else if (DetailFormValidation())
			{
				dtdetail.Rows.Add(0, cmbEmployee.Value, ((Control)(object)cmbEmployee).Text, txtFromDateTime.Value, txtToDateTime.Value, 0, 0, IsMealAllow.Checked, false);
				grdDetails.DataSource = dtdetail;
				grdDetails.RetrieveStructure();
				grdSettings();
				ResetDetail();
				cmbEmployee.Focus();
				((Control)(object)cmbDepartmnet).Enabled = false;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Click(object sender, EventArgs e)
	{
		try
		{
			AddtoGrid();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdateDetail_Click(object sender, EventArgs e)
	{
		try
		{
			if (DetailFormValidation())
			{
				dtdetail.Rows[updateDetailIndex]["EmployeeId"] = cmbEmployee.Value;
				dtdetail.Rows[updateDetailIndex]["EmployeeName"] = ((Control)(object)cmbEmployee).Text;
				dtdetail.Rows[updateDetailIndex]["FromDateTime"] = txtFromDateTime.Value;
				dtdetail.Rows[updateDetailIndex]["ToDateTime"] = txtToDateTime.Value;
				dtdetail.Rows[updateDetailIndex]["OTHours"] = txtOTHours.Text;
				dtdetail.Rows[updateDetailIndex]["OTRate"] = txtOTRate.Text;
				dtdetail.Rows[updateDetailIndex]["IsAllowMeal"] = IsMealAllow.Checked;
				grdDetails.DataSource = dtdetail;
				btnplus.Visible = true;
				btnUpdateDetail.Visible = false;
				btnCancelUpdateDetial.Visible = false;
				ResetDetail();
				cmbEmployee.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Click(object sender, EventArgs e)
	{
		try
		{
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)DataGridHistory).Name);
			if (fs != null)
			{
				DataGridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar2_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetails).Name);
			if (fs != null)
			{
				grdDetails.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void DataGridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "Edit")
			{
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["OverTimeRequestId"].Value);
				ReadById(RecId);
			}
			if (e.Column.Key == "Print")
			{
				CommonServices.OverTimeSlip(Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["OverTimeRequestId"].Value.ToString()));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click_1(object sender, EventArgs e)
	{
		try
		{
			DepartmentFill();
			SectionFill();
			RequestedByFill();
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
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		try
		{
			Close();
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
			((Control)(object)cmbDepartmnet).Text = string.Empty;
			((Control)(object)cmbSection).Text = string.Empty;
			IsOffDuty.Checked = false;
			txtReason.Text = string.Empty;
			((Control)(object)cmbRequestedBy).Text = string.Empty;
			((Control)(object)cmbDepartmnet).Enabled = true;
			txtOverTimeDate.Enabled = true;
			dtdetail.Rows.Clear();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			txtRequestDate.Focus();
			lstRemoveDetailRecord.Clear();
			((Control)(object)cmbEmployee).Text = string.Empty;
			txtFromDateTime.Text = string.Empty;
			txtToDateTime.Text = string.Empty;
			txtOTHours.Text = string.Empty;
			txtOTRate.Text = string.Empty;
			IsMealAllow.Checked = false;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ResetDetail()
	{
		try
		{
			lstRemoveDetailRecord.Clear();
			((Control)(object)cmbEmployee).Text = string.Empty;
			txtFromDateTime.Text = string.Empty;
			txtToDateTime.Text = string.Empty;
			txtOTHours.Text = string.Empty;
			txtOTRate.Text = string.Empty;
			IsMealAllow.Checked = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Expected O, but got Unknown
		try
		{
			if (DataGridHistory.CurrentRow == null || (int)DataGridHistory.CurrentRow.RowType != 0 || DataGridHistory.CurrentRow.Cells.Count <= 0)
			{
				return;
			}
			RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["OverTimeRequestId"].Value);
			OverTimeRequest Obj = new OverTimeRequest();
			Obj = OverTimeRequest.GetByID(RecId);
			if (Obj != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("EmployeeName");
				dt.Columns.Add("FromDateTime");
				dt.Columns.Add("ToDateTime");
				dt.Columns.Add("OTHours", typeof(double));
				dt.Columns.Add("OTRate", typeof(double));
				dt.Columns.Add("IsAllowMeal", typeof(bool));
				dt.Clear();
				for (int i = 0; i < Obj.OverTimeRequestDetailList.Count; i++)
				{
					dt.Rows.Add(Obj.OverTimeRequestDetailList[i].EmployeeName, Obj.OverTimeRequestDetailList[i].FromDateTime, Obj.OverTimeRequestDetailList[i].ToDateTime, Obj.OverTimeRequestDetailList[i].OTHours, Obj.OverTimeRequestDetailList[i].OTRate, Obj.OverTimeRequestDetailList[i].IsAllowMeal);
				}
				grdHistoryDetail.DataSource = dt;
				grdHistoryDetail.RetrieveStructure();
				grdHistoryDetail.RootTable.Columns["OTHours"].Visible = false;
				grdHistoryDetail.RootTable.Columns["OTRate"].Visible = false;
				grdHistoryDetail.RootTable.Columns["IsAllowMeal"].Visible = false;
				grdHistoryDetail.RootTable.Columns["FromDateTime"].Caption = "From Time";
				grdHistoryDetail.RootTable.Columns["ToDateTime"].Caption = "To Time";
				grdHistoryDetail.RootTable.Columns["FromDateTime"].FormatString = "hh:mm tt";
				grdHistoryDetail.RootTable.Columns["ToDateTime"].FormatString = "hh:mm tt";
				CommonServices.GridAutoAdjustmentNew(grdHistoryDetail);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnhistoryrefresh_Click(object sender, EventArgs e)
	{
		try
		{
			DepartmentFill();
			SectionFill();
			RequestedByFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOTRate_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtOTHours_KeyPress(object sender, KeyPressEventArgs e)
	{
		try
		{
			CommonServices.OnlytextdecimelFunction(sender, e);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "rdrequesteddate")
			{
				Requesteddatefrom.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShow_Click(object sender, EventArgs e)
	{
		try
		{
			HistoryFill();
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
			Requesteddatefrom.Value = DateTime.Now;
			RequestedDateTo.Value = DateTime.Now;
			((Control)(object)cmbsectionhistory).Text = string.Empty;
			((Control)(object)CmbDepartmentHistory).Text = string.Empty;
			((Control)(object)cmbrequestedbthistory).Text = string.Empty;
			DataGridHistory.DataSource = null;
			grdHistoryDetail.DataSource = null;
			rdrequesteddate.Checked = true;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void DataGridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DataGridHistory.CurrentColumn != null)
			{
				int Id = int.Parse(r.Cells["OverTimeRequestId"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Edit")
				{
					Reset();
					Id = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["OverTimeRequestId"].Value);
					ReadById(Id);
				}
				else if (DataGridHistory.CurrentColumn.Key == "Print")
				{
					CommonServices.OverTimeSlip(RecId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetails_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!e.Control || e.KeyCode != Keys.Space)
			{
				return;
			}
			GridEXRow r = grdDetails.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetails.CurrentColumn == null)
			{
				return;
			}
			if (grdDetails.CurrentColumn.Key == "Delete")
			{
				if (Conversion.ToInt(r.Cells["EmployeeId"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					OverTimeRequestDetail detail = new OverTimeRequestDetail();
					detail.EmployeeId = Conversion.ToInt(r.Cells["EmployeeId"].Value);
					detail.FromDateTime = Conversion.ToDateTime(r.Cells["FromDateTime"].Value);
					detail.ToDateTime = Conversion.ToDateTime(r.Cells["ToDateTime"].Value);
					detail.OTHours = Conversion.ToInt64(r.Cells["OTHours"].Value);
					detail.OTRate = Conversion.ToDecimal(r.Cells["OTRate"].Value);
					detail.IsAllowMeal = Conversion.ToBool(r.Cells["IsAllowMeal"].Value);
					detail.ActionTypeId = 3;
					lstRemoveDetailRecord.Add(detail);
					dtdetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
				grdDetails.UpdateData();
			}
			if (grdDetails.CurrentColumn.Key == "Edit")
			{
				grdDetails_DoubleClick(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnplus_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name != null && base.ActiveControl.Name == "IsMealAllow")
			{
				cmbEmployee.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void toolStripButton2_Click(object sender, EventArgs e)
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

	private void btnprint_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateOTSlip(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateOTSlip(int PrintId)
	{
		try
		{
			CommonServices.OverTimeSlip(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
	}

	private void txtFromDateTime_ValueChanged(object sender, EventArgs e)
	{
	}

	private void txtToDateTime_ValueChanged(object sender, EventArgs e)
	{
	}

	private void CalculateOTHours()
	{
		try
		{
			DateTime FromTime = txtFromDateTime.Value;
			TimeSpan duration = txtToDateTime.Value - FromTime;
			txtOTHours.Text = duration.TotalHours.ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoader_Click(object sender, EventArgs e)
	{
		if (grdDetails.GetRows().Count() == 0)
		{
			Loader = new EmpOverTimeLoadForRequest(UserAccount);
			Loader.ShowDialog();
			Loader.BringToFront();
			LoadInGridDetail();
		}
		else if (Conversion.ToBool(grdDetails.GetRow(0).Cells["EntryByLoader"].Value))
		{
			Loader = new EmpOverTimeLoadForRequest(UserAccount);
			Loader.ShowDialog();
			Loader.BringToFront();
			LoadInGridDetail();
		}
		else
		{
			MessageBox.Show("You Can't Load Because Direct Entry Already Exist", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}
	}

	public void LoadInGridDetail()
	{
		try
		{
			DataTable dt = Loader.dtLoader;
			if (dt.Rows.Count > 0)
			{
				if (grdDetails.GetRows().Count() > 0)
				{
					if (txtOverTimeDate.Value.Date != Conversion.ToDateTime(dt.Rows[0]["DutyDate"]))
					{
						MessageBox.Show("Already Loaded Rows have different overtime Date", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
					if (Conversion.ToInt(cmbDepartmnet.Value) != Conversion.ToInt(dt.Rows[0]["DepartmentId"]))
					{
						MessageBox.Show("Already Loaded Rows have different Department", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
						return;
					}
				}
				txtOverTimeDate.Enabled = false;
				txtOverTimeDate.Value = Conversion.ToDateTime(dt.Rows[0]["DutyDate"]);
				((Control)(object)cmbDepartmnet).Enabled = false;
				cmbDepartmnet.Value = Conversion.ToInt(dt.Rows[0]["DepartmentId"]);
				bool flag = false;
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					if (grdDetails.GetRows().Length != 0)
					{
						GridEXRow[] rows = grdDetails.GetRows();
						for (int j = 0; j < rows.Length; j++)
						{
							if (Conversion.ToInt(rows[j].Cells["EmployeeId"].Value) == Conversion.ToInt(dt.Rows[i]["EmployeeId"]) && txtOverTimeDate.Value.Date == Conversion.ToDateTime(dt.Rows[i]["DutyDate"]).Date)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						dtdetail.Rows.Add(0, dt.Rows[i]["EmployeeId"], dt.Rows[i]["EmployeeName"], dt.Rows[i]["OTInTime"], dt.Rows[i]["OTOutTime"], dt.Rows[i]["OverTime"], 0, false, true);
					}
					flag = false;
				}
			}
			grdDetails.DataSource = dtdetail;
			grdDetails.RetrieveStructure();
			grdSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbDepartmnet_Leave(object sender, EventArgs e)
	{
		try
		{
			if (Conversion.ToInt(cmbDepartmnet.Value) > 0)
			{
				int depId = Conversion.ToInt(cmbDepartmnet.Value);
				EnumerableRowCollection<DataRow> rows = from row in dtEmployees.AsEnumerable()
					where row.Field<int>("DepartmentId") == depId
					select row;
				DataTable dt = (rows.Any() ? rows.CopyToDataTable() : dtEmployees.Clone());
				if (dt.Rows.Count > 0)
				{
					DDL.BindDDLNew(dt, cmbEmployee, "EmployeeId", "EmployeeName", "Employee Name", false);
				}
			}
			else
			{
				((Control)(object)cmbEmployee).Text = string.Empty;
				((UltraGridBase)cmbEmployee).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void OverTimeRequest_FormClosing(object sender, FormClosingEventArgs e)
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
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Expected O, but got Unknown
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Expected O, but got Unknown
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Expected O, but got Unknown
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Expected O, but got Unknown
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Expected O, but got Unknown
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Expected O, but got Unknown
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Expected O, but got Unknown
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Expected O, but got Unknown
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Expected O, but got Unknown
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Expected O, but got Unknown
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Expected O, but got Unknown
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Expected O, but got Unknown
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0180: Expected O, but got Unknown
		//IL_0180: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Expected O, but got Unknown
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Expected O, but got Unknown
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Expected O, but got Unknown
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Expected O, but got Unknown
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Expected O, but got Unknown
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Expected O, but got Unknown
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Expected O, but got Unknown
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Expected O, but got Unknown
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Expected O, but got Unknown
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01db: Expected O, but got Unknown
		//IL_01db: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Expected O, but got Unknown
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Expected O, but got Unknown
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Expected O, but got Unknown
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Expected O, but got Unknown
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Expected O, but got Unknown
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Expected O, but got Unknown
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Expected O, but got Unknown
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Expected O, but got Unknown
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Expected O, but got Unknown
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Expected O, but got Unknown
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Expected O, but got Unknown
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Expected O, but got Unknown
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Expected O, but got Unknown
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_024b: Expected O, but got Unknown
		//IL_024b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Expected O, but got Unknown
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Expected O, but got Unknown
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Expected O, but got Unknown
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Expected O, but got Unknown
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fd: Expected O, but got Unknown
		//IL_0340: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Expected O, but got Unknown
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Expected O, but got Unknown
		//IL_03ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b8: Expected O, but got Unknown
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Expected O, but got Unknown
		//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ec: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_0587: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Expected O, but got Unknown
		//IL_05be: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_0b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b6f: Expected O, but got Unknown
		//IL_390b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3915: Expected O, but got Unknown
		//IL_3939: Unknown result type (might be due to invalid IL or missing references)
		//IL_3943: Expected O, but got Unknown
		GridEXLayout grdDetails_DesignTimeLayout = new GridEXLayout();
		Appearance appearance30 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
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
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.HRM.OverTimeManagement.OverTimeRequest));
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
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
		Appearance appearance66 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetails = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label11 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.txtReason = new System.Windows.Forms.TextBox();
		this.cmbDepartmnet = new UltraCombo();
		this.label7 = new System.Windows.Forms.Label();
		this.txtRequestDate = new System.Windows.Forms.DateTimePicker();
		this.txtOverTimeDate = new System.Windows.Forms.DateTimePicker();
		this.label3 = new System.Windows.Forms.Label();
		this.label5 = new System.Windows.Forms.Label();
		this.label9 = new System.Windows.Forms.Label();
		this.cmbRequestedBy = new UltraCombo();
		this.label34 = new System.Windows.Forms.Label();
		this.IsOffDuty = new System.Windows.Forms.CheckBox();
		this.cmbSection = new UltraCombo();
		this.label8 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.btnplus = new System.Windows.Forms.Button();
		this.IsMealAllow = new System.Windows.Forms.CheckBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtToDateTime = new System.Windows.Forms.DateTimePicker();
		this.cmbEmployee = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.txtOTHours = new System.Windows.Forms.TextBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.label16 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.txtFromDateTime = new System.Windows.Forms.DateTimePicker();
		this.txtOTRate = new System.Windows.Forms.TextBox();
		this.label2 = new System.Windows.Forms.Label();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ChkBox = new System.Windows.Forms.CheckBox();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolstrip = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.btnLoader = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel13 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label12 = new System.Windows.Forms.Label();
		this.grdHistoryDetail = new GridEX();
		this.panel18 = new System.Windows.Forms.Panel();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label14 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.rdovertimedate = new System.Windows.Forms.RadioButton();
		this.rdrequesteddate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.label17 = new System.Windows.Forms.Label();
		this.cmbrequestedbthistory = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbsectionhistory = new UltraCombo();
		this.btnShow = new System.Windows.Forms.Button();
		this.label50 = new System.Windows.Forms.Label();
		this.label53 = new System.Windows.Forms.Label();
		this.label54 = new System.Windows.Forms.Label();
		this.CmbDepartmentHistory = new UltraCombo();
		this.RequestedDateTo = new System.Windows.Forms.DateTimePicker();
		this.Requesteddatefrom = new System.Windows.Forms.DateTimePicker();
		this.panel17 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
		this.btnhistoryrefresh = new System.Windows.Forms.ToolStripButton();
		this.label51 = new System.Windows.Forms.Label();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetails).BeginInit();
		this.panel9.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmnet).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRequestedBy).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).BeginInit();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolstrip.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel7.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).BeginInit();
		this.panel18.SuspendLayout();
		this.panel12.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbrequestedbthistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsectionhistory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentHistory).BeginInit();
		this.panel17.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(959, 648);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(959, 648);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(951, 619);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel10);
		this.panel2.Controls.Add(this.panel9);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(945, 613);
		this.panel2.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetails);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 226);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(945, 387);
		this.panel10.TabIndex = 3;
		this.grdDetails.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdDetails).BackColor = System.Drawing.Color.White;
		grdDetails_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetails.DesignTimeLayout = grdDetails_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetails).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetails.DynamicFiltering = true;
		this.grdDetails.FilterMode = (FilterMode)2;
		this.grdDetails.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetails).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetails.GroupByBoxVisible = false;
		this.grdDetails.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetails).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetails).Name = "grdDetails";
		((ScrollableBase)this.grdDetails).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetails).Size = new System.Drawing.Size(945, 387);
		((System.Windows.Forms.Control)(object)this.grdDetails).TabIndex = 0;
		this.grdDetails.TotalRow = (InheritableBoolean)1;
		this.grdDetails.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetails).VisualStyle = (VisualStyle)4;
		this.grdDetails.ColumnButtonClick += new ColumnActionEventHandler(grdDetails_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).DoubleClick += new System.EventHandler(grdDetails_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetails).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetails_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.ctrlGrdBar2);
		this.panel9.Controls.Add(this.label11);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 204);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(945, 22);
		this.panel9.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(905, -2);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(5);
		this.ctrlGrdBar2.MyGrid = this.grdDetails;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar2.TabIndex = 465;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.ForeColor = System.Drawing.Color.White;
		this.label11.Location = new System.Drawing.Point(5, -1);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(58, 19);
		this.label11.TabIndex = 464;
		this.label11.Text = "Detail";
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.txtOTRate);
		this.panel4.Controls.Add(this.label2);
		this.panel4.Controls.Add(this.panel6);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 30);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(945, 174);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add(this.txtReason);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartmnet);
		this.groupBox2.Controls.Add(this.label7);
		this.groupBox2.Controls.Add(this.txtRequestDate);
		this.groupBox2.Controls.Add(this.txtOverTimeDate);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbRequestedBy);
		this.groupBox2.Controls.Add(this.label34);
		this.groupBox2.Controls.Add(this.IsOffDuty);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbSection);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox2.Location = new System.Drawing.Point(3, 24);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(928, 73);
		this.groupBox2.TabIndex = 0;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Main";
		this.txtReason.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtReason.Location = new System.Drawing.Point(556, 43);
		this.txtReason.Name = "txtReason";
		this.txtReason.Size = new System.Drawing.Size(367, 23);
		this.txtReason.TabIndex = 5;
		this.cmbDepartmnet.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartmnet.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartmnet.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartmnet.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance30).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Appearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmnet).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance34;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance36;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmnet).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance37).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance37).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance37).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance38).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance40;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance41).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance42).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance42).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance42).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance42;
		((AppearanceBase)appearance43).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartmnet).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartmnet).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartmnet.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartmnet).Location = new System.Drawing.Point(308, 12);
		((System.Windows.Forms.Control)(object)this.cmbDepartmnet).Name = "cmbDepartmnet";
		((System.Windows.Forms.Control)(object)this.cmbDepartmnet).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartmnet).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.cmbDepartmnet).Leave += new System.EventHandler(cmbDepartmnet_Leave);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(3, 47);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(90, 15);
		this.label7.TabIndex = 515;
		this.label7.Text = "Over Time Date";
		this.txtRequestDate.CustomFormat = "dd-MMM-yyyy";
		this.txtRequestDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRequestDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtRequestDate.Location = new System.Drawing.Point(97, 14);
		this.txtRequestDate.Name = "txtRequestDate";
		this.txtRequestDate.Size = new System.Drawing.Size(132, 23);
		this.txtRequestDate.TabIndex = 0;
		this.txtOverTimeDate.CustomFormat = "dd-MMM-yyyy";
		this.txtOverTimeDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOverTimeDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtOverTimeDate.Location = new System.Drawing.Point(97, 43);
		this.txtOverTimeDate.Name = "txtOverTimeDate";
		this.txtOverTimeDate.Size = new System.Drawing.Size(132, 23);
		this.txtOverTimeDate.TabIndex = 1;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(3, 18);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(77, 15);
		this.label3.TabIndex = 463;
		this.label3.Text = "Request Date";
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(477, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(78, 15);
		this.label5.TabIndex = 513;
		this.label5.Text = "Requested By";
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(233, 18);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(71, 15);
		this.label9.TabIndex = 466;
		this.label9.Text = "Department";
		this.cmbRequestedBy.AllowNull = (DefaultableBoolean)1;
		this.cmbRequestedBy.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbRequestedBy.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbRequestedBy.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbRequestedBy).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.cmbRequestedBy).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbRequestedBy).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbRequestedBy).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbRequestedBy.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbRequestedBy).Location = new System.Drawing.Point(556, 12);
		((System.Windows.Forms.Control)(object)this.cmbRequestedBy).Name = "cmbRequestedBy";
		((System.Windows.Forms.Control)(object)this.cmbRequestedBy).Size = new System.Drawing.Size(272, 26);
		((System.Windows.Forms.Control)(object)this.cmbRequestedBy).TabIndex = 4;
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(477, 47);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(45, 15);
		this.label34.TabIndex = 489;
		this.label34.Text = "Reason";
		this.IsOffDuty.AutoSize = true;
		this.IsOffDuty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsOffDuty.Location = new System.Drawing.Point(834, 16);
		this.IsOffDuty.Name = "IsOffDuty";
		this.IsOffDuty.Size = new System.Drawing.Size(84, 19);
		this.IsOffDuty.TabIndex = 6;
		this.IsOffDuty.Text = "Is Off Duty";
		this.IsOffDuty.UseVisualStyleBackColor = true;
		this.cmbSection.AllowNull = (DefaultableBoolean)1;
		this.cmbSection.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbSection.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbSection.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbSection).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbSection).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance35;
		((SpecialBoxBase)((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbSection).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbSection).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance57;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance83).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance84).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance84).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance84).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance84).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbSection).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbSection).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbSection).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbSection).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbSection.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbSection).Location = new System.Drawing.Point(308, 41);
		((System.Windows.Forms.Control)(object)this.cmbSection).Name = "cmbSection";
		((System.Windows.Forms.Control)(object)this.cmbSection).Size = new System.Drawing.Size(165, 26);
		((System.Windows.Forms.Control)(object)this.cmbSection).TabIndex = 3;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(233, 47);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(47, 15);
		this.label8.TabIndex = 510;
		this.label8.Text = "Section";
		this.groupBox1.Controls.Add(this.btnplus);
		this.groupBox1.Controls.Add(this.IsMealAllow);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtToDateTime);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbEmployee);
		this.groupBox1.Controls.Add(this.label10);
		this.groupBox1.Controls.Add(this.txtOTHours);
		this.groupBox1.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox1.Controls.Add(this.label16);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.btnUpdateDetail);
		this.groupBox1.Controls.Add(this.txtFromDateTime);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(3, 97);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(928, 71);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Detail";
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(531, 41);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(32, 23);
		this.btnplus.TabIndex = 5;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.IsMealAllow.AutoSize = true;
		this.IsMealAllow.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.IsMealAllow.Location = new System.Drawing.Point(532, 18);
		this.IsMealAllow.Name = "IsMealAllow";
		this.IsMealAllow.Size = new System.Drawing.Size(97, 19);
		this.IsMealAllow.TabIndex = 5;
		this.IsMealAllow.Text = "Is Meal Allow";
		this.IsMealAllow.UseVisualStyleBackColor = true;
		this.IsMealAllow.Visible = false;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(416, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(50, 15);
		this.label1.TabIndex = 508;
		this.label1.Text = "To Time";
		this.txtToDateTime.CustomFormat = "hh:mm tt";
		this.txtToDateTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtToDateTime.Location = new System.Drawing.Point(416, 41);
		this.txtToDateTime.Name = "txtToDateTime";
		this.txtToDateTime.ShowUpDown = true;
		this.txtToDateTime.Size = new System.Drawing.Size(112, 23);
		this.txtToDateTime.TabIndex = 2;
		this.txtToDateTime.ValueChanged += new System.EventHandler(txtToDateTime_ValueChanged);
		this.cmbEmployee.AllowNull = (DefaultableBoolean)1;
		this.cmbEmployee.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbEmployee.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbEmployee.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance12;
		((SpecialBoxBase)((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance20).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance20).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance23;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance25).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance27;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance28).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbEmployee).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbEmployee.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Location = new System.Drawing.Point(3, 39);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Name = "cmbEmployee";
		((System.Windows.Forms.Control)(object)this.cmbEmployee).Size = new System.Drawing.Size(301, 26);
		((System.Windows.Forms.Control)(object)this.cmbEmployee).TabIndex = 0;
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(3, 20);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(59, 15);
		this.label10.TabIndex = 476;
		this.label10.Text = "Employee";
		this.txtOTHours.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOTHours.Location = new System.Drawing.Point(677, 40);
		this.txtOTHours.Name = "txtOTHours";
		this.txtOTHours.ReadOnly = true;
		this.txtOTHours.Size = new System.Drawing.Size(77, 23);
		this.txtOTHours.TabIndex = 3;
		this.txtOTHours.TabStop = false;
		this.txtOTHours.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOTHours.Visible = false;
		this.txtOTHours.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOTHours_KeyPress);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(607, 41);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(67, 23);
		this.btnCancelUpdateDetial.TabIndex = 7;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnplus_Leave);
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(677, 19);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(57, 15);
		this.label16.TabIndex = 483;
		this.label16.Text = "OT Hours";
		this.label16.Visible = false;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(307, 20);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(65, 15);
		this.label4.TabIndex = 506;
		this.label4.Text = "From Time";
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.btnUpdateDetail.FlatAppearance.BorderColor = System.Drawing.Color.Lime;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(531, 41);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(73, 23);
		this.btnUpdateDetail.TabIndex = 6;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.txtFromDateTime.CustomFormat = " hh:mm tt";
		this.txtFromDateTime.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtFromDateTime.Location = new System.Drawing.Point(307, 41);
		this.txtFromDateTime.Name = "txtFromDateTime";
		this.txtFromDateTime.ShowUpDown = true;
		this.txtFromDateTime.Size = new System.Drawing.Size(107, 23);
		this.txtFromDateTime.TabIndex = 1;
		this.txtFromDateTime.ValueChanged += new System.EventHandler(txtFromDateTime_ValueChanged);
		this.txtOTRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOTRate.Location = new System.Drawing.Point(423, 241);
		this.txtOTRate.Name = "txtOTRate";
		this.txtOTRate.Size = new System.Drawing.Size(77, 23);
		this.txtOTRate.TabIndex = 4;
		this.txtOTRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
		this.txtOTRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtOTRate_KeyPress);
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(427, 220);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(48, 15);
		this.label2.TabIndex = 510;
		this.label2.Text = "OT Rate";
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ChkBox);
		this.panel6.Controls.Add(this.label6);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.ForeColor = System.Drawing.SystemColors.ControlText;
		this.panel6.Location = new System.Drawing.Point(0, 0);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(945, 24);
		this.panel6.TabIndex = 516;
		this.ChkBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBox.AutoSize = true;
		this.ChkBox.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBox.ForeColor = System.Drawing.Color.White;
		this.ChkBox.Location = new System.Drawing.Point(843, 2);
		this.ChkBox.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBox.Name = "ChkBox";
		this.ChkBox.Size = new System.Drawing.Size(102, 19);
		this.ChkBox.TabIndex = 464;
		this.ChkBox.Text = "Print Preview";
		this.ChkBox.UseVisualStyleBackColor = true;
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.Color.White;
		this.label6.Location = new System.Drawing.Point(1, 2);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(165, 19);
		this.label6.TabIndex = 463;
		this.label6.Text = "Over Time Request";
		this.panel3.Controls.Add(this.toolstrip);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(945, 30);
		this.panel3.TabIndex = 0;
		this.toolstrip.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.toolStripButton2, this.btnprint, this.btnshortcutkeys, this.btnLoader });
		this.toolstrip.Location = new System.Drawing.Point(0, 0);
		this.toolstrip.Name = "toolstrip";
		this.toolstrip.Size = new System.Drawing.Size(945, 27);
		this.toolstrip.TabIndex = 5;
		this.toolstrip.Text = "toolStrip1";
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
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click_1);
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
		this.btnUpdate.Text = "Update";
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripButton2.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton2.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton2.Name = "toolStripButton2";
		this.toolStripButton2.Size = new System.Drawing.Size(109, 24);
		this.toolStripButton2.Text = "Attachment";
		this.toolStripButton2.Click += new System.EventHandler(toolStripButton2_Click);
		this.btnprint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(100, 24);
		this.btnprint.Text = "1010-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.btnLoader.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
		this.btnLoader.Image = (System.Drawing.Image)resources.GetObject("btnLoader.Image");
		this.btnLoader.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoader.Name = "btnLoader";
		this.btnLoader.Size = new System.Drawing.Size(144, 24);
		this.btnLoader.Text = "Load Employee Overtime";
		this.btnLoader.Click += new System.EventHandler(btnLoader_Click);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(951, 619);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel13.Controls.Add(this.panel7);
		this.panel13.Controls.Add((System.Windows.Forms.Control)(object)this.grdHistoryDetail);
		this.panel13.Controls.Add(this.panel18);
		this.panel13.Controls.Add(this.panel17);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel13.Location = new System.Drawing.Point(3, 3);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(945, 613);
		this.panel13.TabIndex = 0;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.DataGridHistory).BackColor = System.Drawing.Color.White;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 155);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(945, 250);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 0;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label12);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel7.Location = new System.Drawing.Point(0, 405);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(945, 20);
		this.panel7.TabIndex = 3;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.ForeColor = System.Drawing.Color.White;
		this.label12.Location = new System.Drawing.Point(1, 1);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(200, 16);
		this.label12.TabIndex = 26;
		this.label12.Text = "Detail Of Above Selected Row";
		this.grdHistoryDetail.AllowDelete = (InheritableBoolean)1;
		this.grdHistoryDetail.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdHistoryDetail).BackColor = System.Drawing.Color.White;
		grdHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdHistoryDetail.DesignTimeLayout = grdHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdHistoryDetail.DynamicFiltering = true;
		this.grdHistoryDetail.FilterMode = (FilterMode)2;
		this.grdHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdHistoryDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Location = new System.Drawing.Point(0, 425);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Name = "grdHistoryDetail";
		((ScrollableBase)this.grdHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).Size = new System.Drawing.Size(945, 188);
		((System.Windows.Forms.Control)(object)this.grdHistoryDetail).TabIndex = 1;
		this.grdHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.grdHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel18.Controls.Add(this.panel12);
		this.panel18.Controls.Add(this.panel5);
		this.panel18.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel18.Location = new System.Drawing.Point(0, 51);
		this.panel18.Name = "panel18";
		this.panel18.Size = new System.Drawing.Size(945, 104);
		this.panel18.TabIndex = 1;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar1);
		this.panel12.Controls.Add(this.label14);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(0, 78);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(945, 24);
		this.panel12.TabIndex = 506;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(905, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
		this.ctrlGrdBar1.TabIndex = 46;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 11f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(1, 3);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(132, 18);
		this.label14.TabIndex = 25;
		this.label14.Text = "Filtered Records";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox3);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(945, 78);
		this.panel5.TabIndex = 505;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.rdovertimedate);
		this.groupBox3.Controls.Add(this.rdrequesteddate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.label17);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbrequestedbthistory);
		this.groupBox3.Controls.Add(this.label13);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbsectionhistory);
		this.groupBox3.Controls.Add(this.btnShow);
		this.groupBox3.Controls.Add(this.label50);
		this.groupBox3.Controls.Add(this.label53);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory);
		this.groupBox3.Controls.Add(this.RequestedDateTo);
		this.groupBox3.Controls.Add(this.Requesteddatefrom);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 3);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(948, 73);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(743, 51);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(853, 34);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 10;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.rdovertimedate.AutoSize = true;
		this.rdovertimedate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdovertimedate.Location = new System.Drawing.Point(743, 34);
		this.rdovertimedate.Name = "rdovertimedate";
		this.rdovertimedate.Size = new System.Drawing.Size(108, 19);
		this.rdovertimedate.TabIndex = 7;
		this.rdovertimedate.Text = "Over Time Date";
		this.rdovertimedate.UseVisualStyleBackColor = true;
		this.rdrequesteddate.AutoSize = true;
		this.rdrequesteddate.Checked = true;
		this.rdrequesteddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdrequesteddate.Location = new System.Drawing.Point(743, 17);
		this.rdrequesteddate.Name = "rdrequesteddate";
		this.rdrequesteddate.Size = new System.Drawing.Size(108, 19);
		this.rdrequesteddate.TabIndex = 6;
		this.rdrequesteddate.TabStop = true;
		this.rdrequesteddate.Text = "Requested Date";
		this.rdrequesteddate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(853, 17);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 9;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(444, 19);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(78, 15);
		this.label17.TabIndex = 91;
		this.label17.Text = "Requested By";
		this.cmbrequestedbthistory.AllowNull = (DefaultableBoolean)1;
		this.cmbrequestedbthistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbrequestedbthistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbrequestedbthistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance67;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbrequestedbthistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbrequestedbthistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbrequestedbthistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbrequestedbthistory).Location = new System.Drawing.Point(523, 13);
		((System.Windows.Forms.Control)(object)this.cmbrequestedbthistory).Name = "cmbrequestedbthistory";
		((System.Windows.Forms.Control)(object)this.cmbrequestedbthistory).Size = new System.Drawing.Size(217, 26);
		((System.Windows.Forms.Control)(object)this.cmbrequestedbthistory).TabIndex = 4;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(213, 19);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(47, 15);
		this.label13.TabIndex = 89;
		this.label13.Text = "Section";
		this.cmbsectionhistory.AllowNull = (DefaultableBoolean)1;
		this.cmbsectionhistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbsectionhistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbsectionhistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbsectionhistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.cmbsectionhistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance44;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance45;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance54;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbsectionhistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbsectionhistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbsectionhistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbsectionhistory).Location = new System.Drawing.Point(285, 13);
		((System.Windows.Forms.Control)(object)this.cmbsectionhistory).Name = "cmbsectionhistory";
		((System.Windows.Forms.Control)(object)this.cmbsectionhistory).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.cmbsectionhistory).TabIndex = 2;
		this.btnShow.BackColor = System.Drawing.Color.Teal;
		this.btnShow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShow.ForeColor = System.Drawing.Color.White;
		this.btnShow.Location = new System.Drawing.Point(682, 42);
		this.btnShow.Margin = new System.Windows.Forms.Padding(2);
		this.btnShow.Name = "btnShow";
		this.btnShow.Size = new System.Drawing.Size(58, 25);
		this.btnShow.TabIndex = 5;
		this.btnShow.Text = "Show";
		this.btnShow.UseVisualStyleBackColor = false;
		this.btnShow.Click += new System.EventHandler(btnShow_Click);
		this.btnShow.Leave += new System.EventHandler(btnShow_Leave);
		this.label50.AutoSize = true;
		this.label50.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label50.Location = new System.Drawing.Point(213, 47);
		this.label50.Name = "label50";
		this.label50.Size = new System.Drawing.Size(71, 15);
		this.label50.TabIndex = 87;
		this.label50.Text = "Department";
		this.label53.AutoSize = true;
		this.label53.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label53.Location = new System.Drawing.Point(2, 47);
		this.label53.Name = "label53";
		this.label53.Size = new System.Drawing.Size(48, 15);
		this.label53.TabIndex = 84;
		this.label53.Text = "Date To";
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(2, 19);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(63, 15);
		this.label54.TabIndex = 83;
		this.label54.Text = "Date From";
		this.CmbDepartmentHistory.AllowNull = (DefaultableBoolean)1;
		this.CmbDepartmentHistory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbDepartmentHistory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbDepartmentHistory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance58).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance58).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance59).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance59;
		((SpecialBoxBase)((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance61;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance66;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance71).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance72).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance72).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance72).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance72).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance80).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance81;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance82).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbDepartmentHistory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbDepartmentHistory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Location = new System.Drawing.Point(285, 41);
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Name = "CmbDepartmentHistory";
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).Size = new System.Drawing.Size(155, 26);
		((System.Windows.Forms.Control)(object)this.CmbDepartmentHistory).TabIndex = 3;
		this.RequestedDateTo.CustomFormat = "dd-MMM-yy";
		this.RequestedDateTo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.RequestedDateTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.RequestedDateTo.Location = new System.Drawing.Point(66, 43);
		this.RequestedDateTo.Name = "RequestedDateTo";
		this.RequestedDateTo.ShowCheckBox = true;
		this.RequestedDateTo.Size = new System.Drawing.Size(142, 23);
		this.RequestedDateTo.TabIndex = 1;
		this.Requesteddatefrom.CustomFormat = "dd-MMM-yy";
		this.Requesteddatefrom.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Requesteddatefrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.Requesteddatefrom.Location = new System.Drawing.Point(66, 15);
		this.Requesteddatefrom.Name = "Requesteddatefrom";
		this.Requesteddatefrom.ShowCheckBox = true;
		this.Requesteddatefrom.Size = new System.Drawing.Size(142, 23);
		this.Requesteddatefrom.TabIndex = 0;
		this.panel17.BackColor = System.Drawing.Color.Teal;
		this.panel17.Controls.Add(this.toolStrip3);
		this.panel17.Controls.Add(this.label51);
		this.panel17.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel17.Location = new System.Drawing.Point(0, 0);
		this.panel17.Name = "panel17";
		this.panel17.Size = new System.Drawing.Size(945, 51);
		this.panel17.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[2] { this.toolStripButton1, this.btnhistoryrefresh });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(945, 27);
		this.toolStrip3.TabIndex = 7;
		this.toolStrip3.Text = "toolStrip3";
		this.toolStripButton1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStripButton1.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.toolStripButton1.Name = "toolStripButton1";
		this.toolStripButton1.Size = new System.Drawing.Size(59, 24);
		this.toolStripButton1.Text = "New";
		this.toolStripButton1.Click += new System.EventHandler(toolStripButton1_Click);
		this.btnhistoryrefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnhistoryrefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnhistoryrefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnhistoryrefresh.Name = "btnhistoryrefresh";
		this.btnhistoryrefresh.Size = new System.Drawing.Size(80, 24);
		this.btnhistoryrefresh.Text = "Refresh";
		this.btnhistoryrefresh.Click += new System.EventHandler(btnhistoryrefresh_Click);
		this.label51.AutoSize = true;
		this.label51.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label51.ForeColor = System.Drawing.Color.White;
		this.label51.Location = new System.Drawing.Point(0, 29);
		this.label51.Name = "label51";
		this.label51.Size = new System.Drawing.Size(156, 19);
		this.label51.TabIndex = 25;
		this.label51.Text = "Over Time History";
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.AutoScroll = true;
		this.AutoSize = true;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(959, 648);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "OverTimeRequest";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(OverTimeRequest_FormClosing);
		base.Load += new System.EventHandler(FrmExportSalesContract_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(FrmExportSalesContract_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetails).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmnet).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbRequestedBy).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbSection).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbEmployee).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolstrip.ResumeLayout(false);
		this.toolstrip.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel13.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdHistoryDetail).EndInit();
		this.panel18.ResumeLayout(false);
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbrequestedbthistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbsectionhistory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbDepartmentHistory).EndInit();
		this.panel17.ResumeLayout(false);
		this.panel17.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}

}
