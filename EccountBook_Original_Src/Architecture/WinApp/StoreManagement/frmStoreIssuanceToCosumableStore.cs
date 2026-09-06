using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.Inventory;
using Architecture.BLL.PurchaseTrading;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Inventory;
using Architecture.Model.PurchaseTrading;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmStoreIssuanceToCosumableStore : Form
{
	private UserAccount UserAccount = new UserAccount();

	private bool UpdateMode;

	private bool Approve;

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formright = new Rightsobjects();

	private int RECID;

	private int RefDocNoId;

	private int RefDocTypeId;

	private int HeadId;

	private DataTable dtgrddetail = new DataTable();

	private LoadDepRequestToConsumableStore loadDepReq;

	private List<InvGsStoreIssuanceDetail> lstRemoveRecord = new List<InvGsStoreIssuanceDetail>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControlGsIssuance;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel2;

	private Panel panel6;

	private Panel panel5;

	private Panel panel4;

	private Panel panel3;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnsave;

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Label label15;

	private Label label4;

	internal UltraCombo cmbProject;

	internal UltraCombo cmbBranch;

	private Label label3;

	private DateTimePicker txtDocdate;

	private Label label1;

	private Label label5;

	private TextBox txtDocNo;

	internal GridEX grdDetail;

	private Panel panel7;

	private Panel panel10;

	internal GridEX DataGridHistory;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnLoadRequest;

	private Panel panel8;

	private GroupBox groupBox1;

	private CheckBox checkBox1;

	private RichTextBox txtRemarks;

	private Panel panel11;

	private Label label2;

	internal GridEX grddetailHistory;

	private Panel panel9;

	private ToolStrip toolStrip2;

	private ToolStripButton btnLoadAll;

	private Label label6;

	private Label label7;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnattachment;

	private ToolStripButton btnDelete;

	public frmStoreIssuanceToCosumableStore(UserAccount user)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGSIssuance_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1200, 750);
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			Print.Enabled = formright.DoHavePrintRights;
			checkBox1.Checked = formright.DoHavePrintRights;
			checkBox1.Enabled = formright.DoHavePrintRights;
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
			dtViewAttachments.Columns.Add("AttchmentPath");
			BranchFill();
			ProjectFill();
			GenerateDocNo();
			dtgrddetail.Columns.Add("Id", typeof(int));
			dtgrddetail.Columns.Add("DepartmentRequestId", typeof(int));
			dtgrddetail.Columns.Add("DepartmentRequestDetailId", typeof(int));
			dtgrddetail.Columns.Add("DepartmentRequestNo", typeof(int));
			dtgrddetail.Columns.Add("LocationId", typeof(int));
			dtgrddetail.Columns.Add("DepartmentFromId", typeof(int));
			dtgrddetail.Columns.Add("DepartmentFrom", typeof(string));
			dtgrddetail.Columns.Add("DepartmentToId", typeof(int));
			dtgrddetail.Columns.Add("DepartmentTo", typeof(string));
			dtgrddetail.Columns.Add("WorkOrderId", typeof(int));
			dtgrddetail.Columns.Add("WorkOrder", typeof(string));
			dtgrddetail.Columns.Add("ItemId", typeof(int));
			dtgrddetail.Columns.Add("Item", typeof(string));
			dtgrddetail.Columns.Add("ItemCode", typeof(string));
			dtgrddetail.Columns.Add("UnitId", typeof(int));
			dtgrddetail.Columns.Add("Unit", typeof(string));
			dtgrddetail.Columns.Add("AssetId", typeof(int));
			dtgrddetail.Columns.Add("Asset", typeof(string));
			dtgrddetail.Columns.Add("IssueQty", typeof(double));
			dtgrddetail.Columns.Add("Remarks", typeof(string));
			grdDetail.DataSource = dtgrddetail;
			grdDetail.RetrieveStructure();
			DetailGridSettings();
			txtDocNo.Focus();
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BranchFill()
	{
		try
		{
			DataTable dtBranch = CommonServices.BrancheServiceBind();
			if (dtBranch.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtBranch, cmbBranch, "Id", "BranchName", "BranchName", true);
				((UltraGridBase)cmbBranch).Rows[1].Activate();
				((UltraGridBase)cmbBranch).DisplayLayout.Bands[0].Columns[1].Width = 150;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProjectFill()
	{
		try
		{
			DataTable dtProject = CommonServices.ProjectServiceBind();
			if (dtProject.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProject, cmbProject, "Id", "ProjectName", "Project", true);
				((UltraGridBase)cmbProject).Rows[1].Activate();
				((UltraGridBase)cmbProject).DisplayLayout.Bands[0].Columns[1].Width = 150;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNo()
	{
		try
		{
			int code = CommonServices.StoreIssuanceGenerateCode(1616);
			if (code > 0)
			{
				txtDocNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Expected O, but got Unknown
		//IL_0563: Unknown result type (might be due to invalid IL or missing references)
		//IL_0568: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a8: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid record not found");
			}
			HeadId = CommonServices.VoucherHeadIdGet(RECID, 1616);
			if (HeadId > 0)
			{
				throw new Exception("Record Not Update because Record has exist in Voucher");
			}
			if (!FormValidation())
			{
				return;
			}
			InvGsStoreIssuanceHeader invGsStoreIssuanceHeader = new InvGsStoreIssuanceHeader();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				invGsStoreIssuanceHeader.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			invGsStoreIssuanceHeader.CompanyId = UserAccount.CompanyId;
			invGsStoreIssuanceHeader.BranchesId = Conversion.ToInt(cmbBranch.Value);
			invGsStoreIssuanceHeader.ProjectsId = Conversion.ToInt(cmbProject.Value);
			invGsStoreIssuanceHeader.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			invGsStoreIssuanceHeader.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			invGsStoreIssuanceHeader.DocumentTypeId = 1616;
			invGsStoreIssuanceHeader.RefDocNoId = RefDocNoId;
			invGsStoreIssuanceHeader.RefDocumentTypeId = RefDocTypeId;
			invGsStoreIssuanceHeader.Remarks = txtRemarks.Text;
			invGsStoreIssuanceHeader.OrganizationId = UserAccount.OrganizationId;
			invGsStoreIssuanceHeader.EntryDate = DateTime.Now;
			invGsStoreIssuanceHeader.EntryUser = UserAccount.ID;
			invGsStoreIssuanceHeader.ModifyDate = DateTime.Now;
			invGsStoreIssuanceHeader.ModifyUser = UserAccount.ID;
			invGsStoreIssuanceHeader.IsApproved = false;
			invGsStoreIssuanceHeader.ActionForVoucherId = 0;
			invGsStoreIssuanceHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invGsStoreIssuanceHeader.gsStoreIssuanceDetails = new List<InvGsStoreIssuanceDetail>();
			if (RECID > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					invGsStoreIssuanceHeader.gsStoreIssuanceDetails.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToInt(r.Cells["IssueQty"].Value) > 0)
				{
					InvGsStoreIssuanceDetail d1 = new InvGsStoreIssuanceDetail();
					d1.LineId = r.RowIndex + 1;
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					if (d1.Id > 0)
					{
						d1.ActionTypeId = 2;
					}
					else
					{
						d1.ActionTypeId = 1;
					}
					d1.DepartRequestId = Conversion.ToInt(r.Cells["DepartmentRequestId"].Value);
					d1.DepartRequestDetailId = Conversion.ToInt(r.Cells["DepartmentRequestDetailId"].Value);
					d1.Warehouseid = Conversion.ToInt(r.Cells["LocationId"].Value);
					if (Conversion.ToInt(r.Cells["DepartmentFromId"].Value) == 0)
					{
						throw new Exception("DepartmentFrom Required In grid Row No " + r.RowIndex + 1);
					}
					d1.DepartmentId = Conversion.ToInt(r.Cells["DepartmentFromId"].Value);
					if (Conversion.ToInt(r.Cells["DepartmentToId"].Value) == 0)
					{
						throw new Exception("DepartmentTo Required In grid Row No " + r.RowIndex + 1);
					}
					d1.DepartmentToId = Conversion.ToInt(r.Cells["DepartmentToId"].Value);
					d1.WorkStationFromId = d1.DepartmentId;
					d1.WorkStationToId = d1.DepartmentToId;
					d1.WorkOrderIdId = Conversion.ToInt(r.Cells["WorkOrderId"].Value);
					if (Conversion.ToInt(r.Cells["ItemId"].Value) == 0)
					{
						throw new Exception("Item Required In grid Row No " + r.RowIndex + 1);
					}
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					if (Conversion.ToInt(r.Cells["UnitId"].Value) == 0)
					{
						throw new Exception("Unit Required In grid Row No " + r.RowIndex + 1);
					}
					d1.ItemUomSchId = Conversion.ToInt(r.Cells["UnitId"].Value);
					d1.AssetsId = Conversion.ToInt(r.Cells["AssetId"].Value);
					d1.ReamarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					Conversion.ToDouble((object)r.Cells["IssueQty"].Value.ToString());
					d1.IssueQty = Conversion.ToDouble(r.Cells["IssueQty"].Value);
					if (d1.ItemId > 0)
					{
						double AvgRate = GetAvgRatesAndStockInHand.AvgRateOnlyForCGS(new ReportsParameters
						{
							OrganizationId = UserAccount.OrganizationId,
							CompanyId = UserAccount.CompanyId,
							ItemId = d1.ItemId,
							DocDate = invGsStoreIssuanceHeader.DocDate
						});
						if (!(AvgRate > 0.0))
						{
							throw new Exception("AvgRate Not Found");
						}
						d1.ItemRate = Conversion.ToDouble((object)AvgRate);
						if (d1.ItemRate == 0.0)
						{
							throw new Exception("Item Rate Not Found");
						}
						d1.ItemAmount = d1.IssueQty * d1.ItemRate;
					}
					d1.SupplierCustomerId = 0;
					invGsStoreIssuanceHeader.gsStoreIssuanceDetails.Add(d1);
					continue;
				}
				throw new Exception("Qty Required In grid Row No " + r.RowIndex + 1);
			}
			invGsStoreIssuanceHeader.AttachmentsList = new List<DMSAttachments>();
			invGsStoreIssuanceHeader.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGsStoreIssuanceHeader>(invGsStoreIssuanceHeader, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			InvGsStoreIssuanceHeader.Save(invGsStoreIssuanceHeader);
			if (RECID == 0)
			{
				MessageBox.Show("Record Save SuccessFully" + invGsStoreIssuanceHeader.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update SuccessFully" + invGsStoreIssuanceHeader.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, 1616, base.Name);
			}
			if (checkBox1.Checked)
			{
				PrintMethod(RECID);
			}
			formReset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RECID = ID;
			InvGsStoreIssuanceHeader head = InvGsStoreIssuanceHeader.GetByID(RECID);
			if (head != null)
			{
				tabControlGsIssuance.SelectedIndex = 0;
				btnsave.Visible = false;
				btnUpdate.Visible = true;
				txtDocdate.Value = head.DocDate;
				txtDocNo.Text = head.DocNo.ToString();
				cmbBranch.Value = head.BranchesId;
				cmbProject.Value = head.ProjectsId;
				txtRemarks.Text = head.Remarks;
				RefDocNoId = head.RefDocNoId;
				RefDocTypeId = head.RefDocumentTypeId;
				Approve = head.IsApproved;
				dtgrddetail.Clear();
				for (int i = 0; i < head.gsStoreIssuanceDetails.Count; i++)
				{
					dtgrddetail.Rows.Add(Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].Id), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].DepartRequestId), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].DepartRequestDetailId), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].DepartmentRequestNo), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].Warehouseid), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].DepartmentId), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].DepartmentName), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].DepartmentToId), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].ToDepartmentName), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].WorkOrderIdId), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].WorkOrderNo), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].ItemId), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].ItemName), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].ItemCode), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].ItemUomSchId), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].Equivalent), Conversion.ToInt((object)head.gsStoreIssuanceDetails[i].AssetsId), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].AssetName), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].IssueQty), Conversion.ToString((object)head.gsStoreIssuanceDetails[i].ReamarksDetail));
				}
				grdDetail.DataSource = dtgrddetail;
				grdDetail.RetrieveStructure();
				DetailGridSettings();
				FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
				UpdateMode = true;
			}
			else
			{
				dtgrddetail.Clear();
				grdDetail.ClearStructure();
				formReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNo_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvGsStoreIssuanceHeader.DeleteByID(UserAccount.ID, RECID);
					MessageBox.Show("Delete Record Seccessfully");
					formReset();
				}
			}
			else
			{
				MessageBox.Show("No Record Found For Deletion");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			BranchFill();
			ProjectFill();
			GenerateDocNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		formReset();
	}

	private void formReset()
	{
		try
		{
			AT = new Attachment();
			dtViewAttachments.Clear();
			RECID = 0;
			HeadId = 0;
			RefDocNoId = 0;
			RefDocTypeId = 0;
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			UpdateMode = false;
			Approve = false;
			dtgrddetail.Clear();
			grdDetail.ClearStructure();
			txtDocdate.Value = DateTime.Now;
			txtRemarks.Text = string.Empty;
			txtDocNo.Focus();
			GenerateDocNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmGSIssuance_FormClosing(object sender, FormClosingEventArgs e)
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

	private bool FormValidation()
	{
		if (((Control)(object)cmbBranch).Text.Trim() == string.Empty || ((Control)(object)cmbBranch).Text.Trim() == "0")
		{
			MessageBox.Show("Branch Field Required");
			cmbBranch.Focus();
			return false;
		}
		if (((Control)(object)cmbProject).Text.Trim() == string.Empty || ((Control)(object)cmbProject).Text.Trim() == "0")
		{
			MessageBox.Show("Project Field Required");
			cmbProject.Focus();
			return false;
		}
		if (txtDocNo.Text.Trim() == string.Empty || txtDocNo.Text.Trim() == "0" || txtDocNo.Text.Trim() == null)
		{
			MessageBox.Show("document Number Field Required");
			txtDocNo.Focus();
			return false;
		}
		return true;
	}

	private void Print_Click(object sender, EventArgs e)
	{
		PrintMethod(RECID);
	}

	private void PrintMethod(int ReportId)
	{
		try
		{
			CommonServices.StoreIssuanceSlip1616(ReportId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (e.Column.Key == "IssueQty" && item != null)
			{
				_ = item.RowType;
			}
			_ = e.Column.Key == "LocationId";
			grdDetail.UpdateData();
			CommonServices.GridAutoAdjustmentNew(grdDetail);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DetailGridSettings()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvWareHouse invWare = new InvWareHouse();
			invWare.OrganizationId = UserAccount.OrganizationId;
			invWare.CompanyId = UserAccount.CompanyId;
			grdDetail.RootTable.Columns["LocationId"].ColumnType = (ColumnType)0;
			grdDetail.RootTable.Columns["LocationId"].HasValueList = true;
			grdDetail.RootTable.Columns["LocationId"].LimitToList = true;
			grdDetail.RootTable.Columns["LocationId"].EditType = (EditType)4;
			grdDetail.RootTable.Columns["LocationId"].Caption = "Location";
			grdDetail.RootTable.Columns["LocationId"].ValueList.PopulateValueList((IEnumerable)InvWareHouse.GetList(invWare), "Id", "WareHouseName");
			grdDetail.RootTable.Columns["LocationId"].Width = 200;
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["WorkOrderId"].Visible = false;
			grdDetail.RootTable.Columns["DepartmentRequestId"].Visible = false;
			grdDetail.RootTable.Columns["DepartmentRequestDetailId"].Visible = false;
			grdDetail.RootTable.Columns["LocationId"].Visible = false;
			grdDetail.RootTable.Columns["DepartmentFromId"].Visible = false;
			grdDetail.RootTable.Columns["DepartmentToId"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["UnitId"].Visible = false;
			grdDetail.RootTable.Columns["AssetId"].Visible = false;
			grdDetail.RootTable.Columns["DepartmentRequestNo"].Caption = "Request No";
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["WorkOrderId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["WorkOrder"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DepartmentRequestId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DepartmentRequestDetailId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DepartmentRequestNo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["LocationId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DepartmentFromId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DepartmentFrom"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DepartmentToId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["DepartmentTo"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemCode"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["AssetId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Asset"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Unit"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["UnitId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["IssueQty"].AggregateFunction = (AggregateFunction)2;
			grdDetail.RootTable.Columns["IssueQty"].TextAlignment = (TextAlignment)3;
			grdDetail.RootTable.Columns["IssueQty"].FormatString = "#,##0.##";
			grdDetail.RootTable.Columns["IssueQty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].Width = 30;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.FrozenColumns = 1;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private double BalanceStock(int ItemId, int WareHouseId)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Expected O, but got Unknown
		double BalQty = 0.0;
		try
		{
			BalQty = GetAvgRatesAndStockInHand.GetQtyInHandForPackingMaterialAndStoreItems(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				WarehouseId = WareHouseId,
				ItemId = ItemId,
				DocDate = Conversion.ToDateTime((object)txtDocdate.Value)
			});
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
		return BalQty;
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (!(e.Column.Key == "Delete"))
			{
				return;
			}
			GridEXRow r = grdDetail.CurrentRow;
			if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvGsStoreIssuanceDetail d1 = new InvGsStoreIssuanceDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					d1.DepartRequestId = Conversion.ToInt(r.Cells["DepartmentRequestId"].Value);
					d1.DepartRequestDetailId = Conversion.ToInt(r.Cells["DepartmentRequestDetailId"].Value);
					d1.Warehouseid = Conversion.ToInt(r.Cells["LocationId"].Value);
					d1.DepartmentId = Conversion.ToInt(r.Cells["DepartmentFromId"].Value);
					d1.DepartmentToId = Conversion.ToInt(r.Cells["DepartmentToId"].Value);
					d1.WorkStationFromId = d1.DepartmentId;
					d1.WorkStationToId = d1.DepartmentToId;
					d1.WorkOrderIdId = Conversion.ToInt(r.Cells["WorkOrderId"].Value);
					d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					d1.ItemUomSchId = Conversion.ToInt(r.Cells["UnitId"].Value);
					d1.AssetsId = Conversion.ToInt(r.Cells["AssetId"].Value);
					d1.ReamarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					Conversion.ToDouble((object)BalanceStock(d1.ItemId, 0));
					Conversion.ToDouble((object)r.Cells["IssueQty"].Value.ToString());
					d1.IssueQty = Conversion.ToInt(r.Cells["IssueQty"].Value);
					double AvgRate = 0.0;
					d1.ItemRate = Conversion.ToDouble((object)AvgRate);
					d1.ItemAmount = d1.IssueQty * d1.ItemRate;
					d1.SupplierCustomerId = 0;
					d1.ActionTypeId = 3;
					lstRemoveRecord.Add(d1);
					dtgrddetail.Rows[r.RowIndex].Delete();
					if (r != null && (int)r.RowType == 0)
					{
						r.Delete();
					}
					grdDetail.UpdateData();
				}
			}
			else if (r != null && (int)r.RowType == 0)
			{
				r.Delete();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036a: Expected O, but got Unknown
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (!(grdDetail.CurrentColumn.Key == "Delete"))
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						InvGsStoreIssuanceDetail d1 = new InvGsStoreIssuanceDetail();
						d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d1.DepartRequestId = Conversion.ToInt(r.Cells["DepartmentRequestId"].Value);
						d1.DepartRequestDetailId = Conversion.ToInt(r.Cells["DepartmentRequestDetailId"].Value);
						d1.Warehouseid = Conversion.ToInt(r.Cells["LocationId"].Value);
						d1.DepartmentId = Conversion.ToInt(r.Cells["DepartmentFromId"].Value);
						d1.DepartmentToId = Conversion.ToInt(r.Cells["DepartmentToId"].Value);
						d1.WorkStationFromId = d1.DepartmentId;
						d1.WorkStationToId = d1.DepartmentToId;
						d1.WorkOrderIdId = Conversion.ToInt(r.Cells["WorkOrderId"].Value);
						d1.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d1.ItemUomSchId = Conversion.ToInt(r.Cells["UnitId"].Value);
						d1.AssetsId = Conversion.ToInt(r.Cells["AssetId"].Value);
						d1.ReamarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
						Conversion.ToDouble((object)BalanceStock(d1.ItemId, 0));
						Conversion.ToDouble((object)r.Cells["IssueQty"].Value.ToString());
						d1.IssueQty = Conversion.ToInt(r.Cells["IssueQty"].Value);
						double AvgRate = 0.0;
						d1.ItemRate = Conversion.ToDouble((object)AvgRate);
						d1.ItemAmount = d1.IssueQty * d1.ItemRate;
						d1.SupplierCustomerId = 0;
						d1.ActionTypeId = 3;
						lstRemoveRecord.Add(d1);
						dtgrddetail.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdDetail.UpdateData();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
			else
			{
				if (!e.Control || e.KeyCode != Keys.Delete)
				{
					return;
				}
				if (Conversion.ToInt(r.Cells["ItemId"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						InvGsStoreIssuanceDetail d2 = new InvGsStoreIssuanceDetail();
						d2.Id = Conversion.ToInt(r.Cells["Id"].Value);
						d2.DepartRequestId = Conversion.ToInt(r.Cells["DepartmentRequestId"].Value);
						d2.DepartRequestDetailId = Conversion.ToInt(r.Cells["DepartmentRequestDetailId"].Value);
						d2.Warehouseid = Conversion.ToInt(r.Cells["LocationId"].Value);
						d2.DepartmentId = Conversion.ToInt(r.Cells["DepartmentFromId"].Value);
						d2.DepartmentToId = Conversion.ToInt(r.Cells["DepartmentToId"].Value);
						d2.WorkStationFromId = d2.DepartmentId;
						d2.WorkStationToId = d2.DepartmentToId;
						d2.WorkOrderIdId = Conversion.ToInt(r.Cells["WorkOrderId"].Value);
						d2.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						d2.ItemUomSchId = Conversion.ToInt(r.Cells["UnitId"].Value);
						d2.AssetsId = Conversion.ToInt(r.Cells["AssetId"].Value);
						d2.ReamarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
						Conversion.ToDouble((object)BalanceStock(d2.ItemId, 0));
						Conversion.ToDouble((object)r.Cells["IssueQty"].Value.ToString());
						d2.IssueQty = Conversion.ToInt(r.Cells["IssueQty"].Value);
						double AvgRate2 = 0.0;
						d2.ItemRate = Conversion.ToDouble((object)AvgRate2);
						d2.ItemAmount = d2.IssueQty * d2.ItemRate;
						d2.SupplierCustomerId = 0;
						d2.ActionTypeId = 3;
						lstRemoveRecord.Add(d2);
						dtgrddetail.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdDetail.UpdateData();
					}
				}
				else if (r != null && (int)r.RowType == 0)
				{
					r.Delete();
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadRequest_Click(object sender, EventArgs e)
	{
		try
		{
			loadDepReq = new LoadDepRequestToConsumableStore(UserAccount);
			loadDepReq.DocumentTypeId = 1615;
			loadDepReq.ShowDialog();
			loadDepReq.BringToFront();
			LoadInGridDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetail()
	{
		try
		{
			DataTable dtLoader = loadDepReq.dtLoader;
			if (dtLoader != null && dtLoader.Rows.Count > 0)
			{
				HashSet<int> existingDetailIds = new HashSet<int>(from r in grdDetail.GetRows()
					select Conversion.ToInt(r.Cells["DepartmentRequestDetailId"].Value));
				foreach (DataRow row in loadDepReq.dtLoader.Rows)
				{
					int detailId = Conversion.ToInt(row["DetailId"]);
					if (existingDetailIds.Add(detailId))
					{
						dtgrddetail.Rows.Add(0, Conversion.ToInt(row["Id"]), detailId, Conversion.ToInt(row["DocNo"]), 0, Conversion.ToInt(row["FromDepartmentId"]), Conversion.ToString(row["FromDepartment"]), Conversion.ToInt(row["ToDepartmentId"]), Conversion.ToString(row["ToDepartment"]), Conversion.ToInt(row["WorkOrderId"]), Conversion.ToString(row["WorkOrderNo"]), Conversion.ToString(row["ItemId"]), Conversion.ToString(row["ItemName"]), Conversion.ToString(row["ItemCode"]), Conversion.ToInt(row["ItemUOMId"]), Conversion.ToString(row["UOM"]), Conversion.ToInt(row["AssetId"]), Conversion.ToString(row["AssetName"]), Conversion.ToDouble(row["BalQty"]), "");
					}
				}
				grdDetail.DataSource = dtgrddetail;
				grdDetail.RetrieveStructure();
				DetailGridSettings();
				loadDepReq.dtLoader.Clear();
			}
			else if (!grdDetail.GetRows().Any())
			{
				formReset();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControlGsIssuance_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControlGsIssuance.SelectedIndex == 1)
			{
				HistoryFill(50);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnLoadAll_Click(object sender, EventArgs e)
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

	private void HistoryFill(int NoOfRecords = 0)
	{
		try
		{
			DataTable dt = CommonServices.StoreIssuanceHistory(1616, formright.DoHaveCanViewAllRecordRights, NoOfRecords);
			if (dt.Rows.Count <= 0 || dt == null)
			{
				return;
			}
			DataTable dtGrid = new DataTable();
			dtGrid.Columns.Add("RecordNo", typeof(int));
			dtGrid.Columns.Add("Id", typeof(int));
			dtGrid.Columns.Add("DocNo", typeof(int));
			dtGrid.Columns.Add("DocDate", typeof(DateTime));
			dtGrid.Columns.Add("Remarks", typeof(string));
			dtGrid.Columns.Add("NoOfAttachments", typeof(int));
			dtGrid.Clear();
			int i;
			for (i = 0; i < dt.Rows.Count; i++)
			{
				if (!dtGrid.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dt.Rows[i]["Id"])))
				{
					dtGrid.Rows.Add(Conversion.ToInt(dt.Rows[i]["RecordNo"]), Conversion.ToInt(dt.Rows[i]["Id"]), Conversion.ToInt(dt.Rows[i]["DocNo"]), Conversion.ToDateTime(dt.Rows[i]["DocDate"]).ToShortDateString(), Conversion.ToString(dt.Rows[i]["Remarks"]), Conversion.ToInt(dt.Rows[i]["NoOfAttachments"]));
				}
			}
			DataGridHistory.DataSource = dtGrid;
			DataGridHistory.RetrieveStructure();
			if (formright.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Print");
				DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
				DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
				DataGridHistory.RootTable.Columns["Print"].Width = 50;
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 50;
				DataGridHistory.RootTable.Columns["Edit"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			if (formright.DoHavePrintRights && formright.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.RootTable.Columns["Edit"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			CommonServices.GridAutoAdjustmentNew(DataGridHistory);
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			if (formright.DoHaveUpdateRights)
			{
				RECID = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
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
		try
		{
			if (e.Column.Key == "Print")
			{
				PrintMethod(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
			if (e.Column.Key == "Edit")
			{
				ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 1616);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item == null || item.Cells.Count <= 0 || (int)item.RowType != 0)
			{
				return;
			}
			InvGsStoreIssuanceHeader po = new InvGsStoreIssuanceHeader();
			po = InvGsStoreIssuanceHeader.GetByID(Conversion.ToInt(item.Cells["Id"].Value));
			if (po != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DepartmentRequestNo", typeof(int));
				dt.Columns.Add("DepartmentFrom", typeof(string));
				dt.Columns.Add("DepartmentTo", typeof(string));
				dt.Columns.Add("WorkOrder", typeof(string));
				dt.Columns.Add("Item", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("Unit", typeof(string));
				dt.Columns.Add("Asset", typeof(string));
				dt.Columns.Add("IssueQty", typeof(double));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Clear();
				for (int i = 0; i < po.gsStoreIssuanceDetails.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt((object)po.gsStoreIssuanceDetails[i].Id), Conversion.ToInt((object)po.gsStoreIssuanceDetails[i].DepartmentRequestNo), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].DepartmentName), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].ToDepartmentName), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].WorkOrderNo), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].ItemName), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].ItemCode), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].Equivalent), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].AssetName), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].IssueQty), Conversion.ToString((object)po.gsStoreIssuanceDetails[i].ReamarksDetail));
				}
				grddetailHistory.DataSource = dt;
				grddetailHistory.RetrieveStructure();
				Grddetailhistorysettings();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grddetailhistorysettings()
	{
		try
		{
			grddetailHistory.RootTable.Columns["Id"].Visible = false;
			grddetailHistory.RootTable.Columns["IssueQty"].AggregateFunction = (AggregateFunction)2;
			grddetailHistory.RootTable.Columns["IssueQty"].TextAlignment = (TextAlignment)3;
			grddetailHistory.RootTable.Columns["IssueQty"].FormatString = "#,##0.##";
			grddetailHistory.RootTable.Columns["IssueQty"].TotalFormatString = "#,##0.##";
			CommonServices.GridAutoAdjustmentNew(grddetailHistory);
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
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdDetail).Name);
			if (fs != null)
			{
				grdDetail.LoadLayoutFile((Stream)fs);
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

	private void frmGSIssuance_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyData == Keys.Return)
			{
				SendKeys.Send("{TAB}");
			}
			if (e.Control && e.KeyCode == Keys.S && tabControlGsIssuance.SelectedIndex == 0 && !UpdateMode)
			{
				btnsave_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else
				{
					btnnew_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControlGsIssuance.SelectedIndex == 1)
				{
					tabControlGsIssuance.SelectedIndex = 0;
				}
				else
				{
					tabControlGsIssuance.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.KeyCode == Keys.U && UpdateMode)
			{
				btnUpdate_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.P && formright.DoHavePrintRights)
			{
				Print_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				btnLoadRequest_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				cmbBranch.Focus();
			}
			if (e.Control && e.KeyCode == Keys.F10)
			{
				btnattachment_Click(null, null);
			}
			if (e.Control && e.KeyCode == Keys.Return && tabControlGsIssuance.SelectedIndex == 1)
			{
				RECID = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RECID);
			}
			if (e.Control && e.KeyCode == Keys.Up)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					txtDocdate.Focus();
				}
				else if (tabControlGsIssuance.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControlGsIssuance.SelectedIndex == 1)
				{
					((Control)(object)grddetailHistory).Focus();
				}
				else if (tabControlGsIssuance.SelectedIndex == 0)
				{
					((Control)(object)grdDetail).Focus();
				}
			}
			if (e.Control && e.Alt)
			{
				MakeShortCutKeys();
			}
			if (e.Control && e.KeyCode == Keys.L)
			{
				loadDepReq = new LoadDepRequestToConsumableStore(UserAccount);
				loadDepReq.DocumentTypeId = 1615;
				loadDepReq.ShowDialog();
				loadDepReq.BringToFront();
				LoadInGridDetail();
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+L", "To Load Request");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On DocDate in Main Box");
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
			if (r != null && (int)r.RowType == 0)
			{
				Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (DataGridHistory.CurrentColumn.Key == "Print" && formright.DoHavePrintRights)
				{
					PrintMethod(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formright.DoHaveUpdateRights)
				{
					ReadById(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), 1616);
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
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Expected O, but got Unknown
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Expected O, but got Unknown
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Expected O, but got Unknown
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Expected O, but got Unknown
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Expected O, but got Unknown
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0796: Expected O, but got Unknown
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ad: Expected O, but got Unknown
		//IL_23bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_23c7: Expected O, but got Unknown
		//IL_23eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_23f5: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmStoreIssuanceToCosumableStore));
		GridEXLayout grddetailHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControlGsIssuance = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel11 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel6 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label2 = new System.Windows.Forms.Label();
		this.panel5 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.label3 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.RichTextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label5 = new System.Windows.Forms.Label();
		this.cmbBranch = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.cmbProject = new UltraCombo();
		this.panel4 = new System.Windows.Forms.Panel();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.btnLoadRequest = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grddetailHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label6 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControlGsIssuance.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranch).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProject).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlGsIssuance);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 511);
		this.panel1.TabIndex = 0;
		this.tabControlGsIssuance.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlGsIssuance.Controls.Add(this.tabPage1);
		this.tabControlGsIssuance.Controls.Add(this.tabPage2);
		this.tabControlGsIssuance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlGsIssuance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlGsIssuance.Location = new System.Drawing.Point(0, 0);
		this.tabControlGsIssuance.Name = "tabControlGsIssuance";
		this.tabControlGsIssuance.SelectedIndex = 0;
		this.tabControlGsIssuance.Size = new System.Drawing.Size(964, 511);
		this.tabControlGsIssuance.TabIndex = 0;
		this.tabControlGsIssuance.SelectedIndexChanged += new System.EventHandler(tabControlGsIssuance_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 482);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel11);
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(950, 476);
		this.panel2.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 200);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 276);
		this.panel11.TabIndex = 15;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(950, 276);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel6.BackColor = System.Drawing.Color.Teal;
		this.panel6.Controls.Add(this.ctrlGrdBar1);
		this.panel6.Controls.Add(this.label2);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel6.Location = new System.Drawing.Point(0, 175);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 25);
		this.panel6.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(906, -1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(42, 25);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label2.Location = new System.Drawing.Point(4, 4);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(58, 19);
		this.label2.TabIndex = 2;
		this.label2.Text = "Detail";
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add(this.groupBox1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 53);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(950, 122);
		this.panel5.TabIndex = 2;
		this.groupBox1.Controls.Add(this.checkBox1);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbBranch);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbProject);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(936, 111);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.checkBox1.AutoSize = true;
		this.checkBox1.Checked = true;
		this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkBox1.Location = new System.Drawing.Point(862, 80);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(67, 19);
		this.checkBox1.TabIndex = 5;
		this.checkBox1.Text = "Preview";
		this.checkBox1.UseVisualStyleBackColor = true;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(6, 23);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(79, 15);
		this.label3.TabIndex = 82;
		this.label3.Text = "Branch Name";
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(313, 49);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(546, 54);
		this.txtRemarks.TabIndex = 4;
		this.txtRemarks.Text = "";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(6, 84);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 79;
		this.label1.Text = "Doc No && Date";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(94, 80);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.ShortcutsEnabled = false;
		this.txtDocNo.Size = new System.Drawing.Size(71, 23);
		this.txtDocNo.TabIndex = 2;
		this.txtDocNo.Leave += new System.EventHandler(txtDocNo_TextChanged);
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(167, 80);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(140, 23);
		this.txtDocdate.TabIndex = 3;
		this.label5.BackColor = System.Drawing.Color.LightGray;
		this.label5.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(313, 18);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(546, 26);
		this.label5.TabIndex = 89;
		this.label5.Text = "Remarks";
		this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbBranch.AllowNull = (DefaultableBoolean)1;
		this.cmbBranch.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbBranch.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbBranch.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance7).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Appearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.cmbBranch).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbBranch).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranch).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranch).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance9;
		((SpecialBoxBase)((UltraGridBase)this.cmbBranch).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance11).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance11).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance11).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbBranch).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.cmbBranch).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbBranch).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance13).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance15).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance16).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance16;
		((AppearanceBase)appearance17).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbBranch).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance19;
		((UltraGridBase)this.cmbBranch).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbBranch).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbBranch).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbBranch).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbBranch.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbBranch).Location = new System.Drawing.Point(94, 18);
		((System.Windows.Forms.Control)(object)this.cmbBranch).Name = "cmbBranch";
		((System.Windows.Forms.Control)(object)this.cmbBranch).Size = new System.Drawing.Size(213, 26);
		((System.Windows.Forms.Control)(object)this.cmbBranch).TabIndex = 0;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(6, 53);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(79, 15);
		this.label4.TabIndex = 83;
		this.label4.Text = "Project Name";
		this.cmbProject.AllowNull = (DefaultableBoolean)1;
		this.cmbProject.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbProject.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbProject.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbProject).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.cmbProject).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbProject).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance5).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance5).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbProject).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance5;
		((AppearanceBase)appearance6).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProject).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance6;
		((SpecialBoxBase)((UltraGridBase)this.cmbProject).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance10).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance10).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbProject).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbProject).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbProject).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance20).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance20;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance23).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance24).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance24).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance24).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance24).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance24;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbProject).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.cmbProject).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbProject).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbProject).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbProject).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbProject.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbProject).Location = new System.Drawing.Point(94, 49);
		((System.Windows.Forms.Control)(object)this.cmbProject).Name = "cmbProject";
		((System.Windows.Forms.Control)(object)this.cmbProject).Size = new System.Drawing.Size(213, 26);
		((System.Windows.Forms.Control)(object)this.cmbProject).TabIndex = 1;
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 24);
		this.panel4.TabIndex = 1;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(3, 2);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(129, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Store Issuance";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(950, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnnew, this.btnRefresh, this.btnsave, this.btnUpdate, this.btnDelete, this.Print, this.btnattachment, this.btnLoadRequest, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(950, 27);
		this.toolStrip1.TabIndex = 2;
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
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.icons8_delete_bin_16;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(76, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.Print.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.Print.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.Print.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.Print.Name = "Print";
		this.Print.Size = new System.Drawing.Size(100, 24);
		this.Print.Text = "1616-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.btnLoadRequest.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadRequest.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadRequest.Name = "btnLoadRequest";
		this.btnLoadRequest.Size = new System.Drawing.Size(100, 24);
		this.btnLoadRequest.Text = "&Load Request";
		this.btnLoadRequest.Click += new System.EventHandler(btnLoadRequest_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(106, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 482);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(950, 476);
		this.panel7.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailHistory);
		this.panel10.Controls.Add(this.panel9);
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel10.Controls.Add(this.panel8);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(950, 476);
		this.panel10.TabIndex = 2;
		this.grddetailHistory.AllowEdit = (InheritableBoolean)2;
		grddetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailHistory.DesignTimeLayout = grddetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grddetailHistory.DynamicFiltering = true;
		this.grddetailHistory.FilterMode = (FilterMode)2;
		this.grddetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Location = new System.Drawing.Point(0, 318);
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Name = "grddetailHistory";
		((ScrollableBase)this.grddetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Size = new System.Drawing.Size(950, 158);
		((System.Windows.Forms.Control)(object)this.grddetailHistory).TabIndex = 68;
		this.grddetailHistory.TotalRow = (InheritableBoolean)1;
		this.grddetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailHistory).VisualStyle = (VisualStyle)4;
		this.panel9.BackColor = System.Drawing.Color.Teal;
		this.panel9.Controls.Add(this.label7);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(0, 293);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 25);
		this.panel9.TabIndex = 67;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(236, 17);
		this.label7.TabIndex = 24;
		this.label7.Text = "Detail of the Above Selected Row";
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Top;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 53);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(950, 240);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 64;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar2);
		this.panel8.Controls.Add(this.label6);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 0);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 53);
		this.panel8.TabIndex = 65;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(906, 26);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 25);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label6.Location = new System.Drawing.Point(5, 29);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(192, 19);
		this.label6.TabIndex = 2;
		this.label6.Text = "Store Issuance History";
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 25);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(78, 22);
		this.btnLoadAll.Text = "Load All";
		this.btnLoadAll.Click += new System.EventHandler(btnLoadAll_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 511);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmStoreIssuanceToCosumableStore";
		this.Text = "frmGSIssuance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmGSIssuance_FormClosing);
		base.Load += new System.EventHandler(frmGSIssuance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGSIssuance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControlGsIssuance.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel11.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel6.ResumeLayout(false);
		this.panel6.PerformLayout();
		this.panel5.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbBranch).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbProject).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grddetailHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
