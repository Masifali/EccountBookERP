using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.PurchaseTrading;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Main;
using Architecture.Model.PurchaseTrading;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmGSIssuance : Form
{
	private Attachment AT = new Attachment();

	private Rightsobjects formright = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private DataTable dtgrddetail = new DataTable();

	private DataTable dtGridHistory = new DataTable();

	private int RECID;

	private int VoucherHeadId;

	private int DocumentTypeId = 451;

	private bool Approve;

	private bool UpdateMode;

	private bool FormInitialized;

	private string RequestType = "";

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private LoadDepRequestToConsumableStore loadDepReq;

	private PendingDoPmForIssuance loadDO;

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

	private DateTimePicker txtDocdate;

	private Label label1;

	private TextBox txtDocNo;

	internal GridEX grdDetail;

	internal GridEX DataGridHistory;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnLoadDO;

	private Panel panel8;

	private GroupBox groupBox1;

	private CheckBox checkBox1;

	private Panel panel11;

	private Label label2;

	internal GridEX grddetailHistory;

	private ToolStrip toolStrip2;

	private Label label6;

	private Label label7;

	private CtrlGrdBar ctrlGrdBar2;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStripButton BtnShortCutkeys;

	private ToolStripButton btnattachment;

	private Panel panel9;

	private GroupBox groupBox3;

	private Button btnShowHistory;

	private Label label54;

	private Label label55;

	private Label label56;

	private Label label57;

	private TextBox ToDocNoHistory;

	private TextBox FromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnNewHistory;

	private TextBox txtRemarks;

	private Label label3;

	private Panel panel12;

	private TextBox txtBranchSrNo;

	private ToolStripButton btnDelete;

	private ToolStripButton BtnLoadDepRequest;

	private CheckBox ChkStoreReturnLink;

	private Button BtnStoreReturnLink;

	public frmGSIssuance(UserAccount user)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmGSIssuance_Load(object sender, EventArgs e)
	{
		try
		{
			formright = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnsave.Enabled = formright.DoHaveSaveRight;
			btnUpdate.Enabled = formright.DoHaveUpdateRights;
			btnDelete.Enabled = formright.DoHaveCanDelete;
			Print.Enabled = formright.DoHavePrintRights;
			checkBox1.Checked = formright.DoHavePrintRights;
			checkBox1.Enabled = formright.DoHavePrintRights;
			ctrlGrdBar1.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar1.mGridExport.Enabled = formright.DoHaveGridExportRights;
			ctrlGrdBar2.mGridPrint.Enabled = formright.DoHaveGridPrintRights;
			ctrlGrdBar2.mGridExport.Enabled = formright.DoHaveGridExportRights;
			dtgrddetail.Columns.Add("Id", typeof(int));
			dtgrddetail.Columns.Add("DepartmentRequestId", typeof(int));
			dtgrddetail.Columns.Add("DepartmentRequestDetailId", typeof(int));
			dtgrddetail.Columns.Add("DepartmentRequestNo", typeof(int));
			dtgrddetail.Columns.Add("DoDocumentTypeId", typeof(int));
			dtgrddetail.Columns.Add("DoId", typeof(int));
			dtgrddetail.Columns.Add("DoDetailId", typeof(int));
			dtgrddetail.Columns.Add("DoNo", typeof(int));
			dtgrddetail.Columns.Add("SupplierCustomerId", typeof(int));
			dtgrddetail.Columns.Add("SupplierName", typeof(string));
			dtgrddetail.Columns.Add("ItemId", typeof(int));
			dtgrddetail.Columns.Add("ItemCode", typeof(string));
			dtgrddetail.Columns.Add("Item", typeof(string));
			dtgrddetail.Columns.Add("LocationId", typeof(int));
			dtgrddetail.Columns.Add("WarehouseName", typeof(string));
			dtgrddetail.Columns.Add("RackId", typeof(int));
			dtgrddetail.Columns.Add("RackName", typeof(string));
			dtgrddetail.Columns.Add("ItemConditionId", typeof(int));
			dtgrddetail.Columns.Add("BagTypeId", typeof(int));
			dtgrddetail.Columns.Add("BagType", typeof(string));
			dtgrddetail.Columns.Add("UnitId", typeof(int));
			dtgrddetail.Columns.Add("Unit", typeof(string));
			dtgrddetail.Columns.Add("Equivalent", typeof(double));
			dtgrddetail.Columns.Add("ItemRate", typeof(double));
			dtgrddetail.Columns.Add("IssueQty", typeof(double));
			dtgrddetail.Columns.Add("RowBalanceQty", typeof(double));
			dtgrddetail.Columns.Add("BalanceStock", typeof(double));
			dtgrddetail.Columns.Add("DepartmentId", typeof(int));
			dtgrddetail.Columns.Add("Department", typeof(string));
			dtgrddetail.Columns.Add("AssetId", typeof(int));
			dtgrddetail.Columns.Add("Asset", typeof(string));
			dtgrddetail.Columns.Add("Remarks", typeof(string));
			grdDetail.DataSource = dtgrddetail;
			grdDetail.RetrieveStructure();
			DetailGridSettings();
			GenerateDocNo();
			GenerateBranchSrNo();
			txtDocNo.Focus();
			FormInitialized = true;
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
			int code = CommonServices.StoreIssuanceGenerateCode(DocumentTypeId);
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

	private void GenerateBranchSrNo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int code = InvGsStoreIssuanceHeader.GenerateBranchSrNo(new InvGsStoreIssuanceHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = DocumentTypeId
			});
			if (code > 0)
			{
				txtBranchSrNo.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Expected O, but got Unknown
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Expected O, but got Unknown
		try
		{
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid record not found");
			}
			if (!FormValidation())
			{
				return;
			}
			VoucherHeadId = CommonServices.VoucherHeadIdGet(RECID, DocumentTypeId);
			if (VoucherHeadId > 0)
			{
				throw new Exception("Record Not Update because Record has exist In voucher");
			}
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			InvGsStoreIssuanceHeader invGsStoreIssuanceHeader = new InvGsStoreIssuanceHeader();
			invGsStoreIssuanceHeader.OrganizationId = UserAccount.OrganizationId;
			invGsStoreIssuanceHeader.CompanyId = UserAccount.CompanyId;
			invGsStoreIssuanceHeader.BranchesId = UserAccount.BranchesId;
			invGsStoreIssuanceHeader.ProjectsId = UserAccount.BranchesId;
			invGsStoreIssuanceHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invGsStoreIssuanceHeader.DocumentTypeId = DocumentTypeId;
			invGsStoreIssuanceHeader.Id = RECID;
			invGsStoreIssuanceHeader.EntryDate = DateTime.Now;
			invGsStoreIssuanceHeader.EntryUser = UserAccount.ID;
			invGsStoreIssuanceHeader.ModifyDate = DateTime.Now;
			invGsStoreIssuanceHeader.ModifyUser = UserAccount.ID;
			invGsStoreIssuanceHeader.IsApproved = false;
			invGsStoreIssuanceHeader.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			invGsStoreIssuanceHeader.BranchSrNo = Conversion.ToInt((object)txtBranchSrNo.Text);
			invGsStoreIssuanceHeader.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			invGsStoreIssuanceHeader.Remarks = txtRemarks.Text;
			invGsStoreIssuanceHeader.ActionForVoucherId = 0;
			invGsStoreIssuanceHeader.gsStoreIssuanceDetails = new List<InvGsStoreIssuanceDetail>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvGsStoreIssuanceDetail Pd = new InvGsStoreIssuanceDetail();
				Pd.Id = ((RECID != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				Pd.ActionTypeId = ((Pd.Id <= 0) ? 1 : 2);
				FillDetailListCommonForInsertAndDelete(Pd, r);
				if (Pd.IssueQty > Conversion.ToDouble(r.Cells["RowBalanceQty"].Value))
				{
					MessageBox.Show("Issue Qty grater than Row Balance Qty Please Check in Row no " + r.RowIndex + 1);
					return;
				}
				if (RequestType != "DeliveryOrder")
				{
					FormHelper.ValidateField(Pd.DepartRequestId, "DepartRequestNo", r.RowIndex);
					FormHelper.ValidateField(Pd.DepartRequestDetailId, "DepartRequestDetailId", r.RowIndex);
				}
				else
				{
					FormHelper.ValidateField(Pd.RefDocumentTypeId, "DoDocumentType", r.RowIndex);
					FormHelper.ValidateField(Pd.RefDocId, "DoNo", r.RowIndex);
					FormHelper.ValidateField(Pd.RefDocDetailId, "DoDetailId", r.RowIndex);
					FormHelper.ValidateField(Pd.SupplierCustomerId, "Party Name", r.RowIndex);
				}
				FormHelper.ValidateField(Pd.Warehouseid, "WareHouse", r.RowIndex);
				FormHelper.ValidateField(Pd.RackId, "Rack Name", r.RowIndex);
				FormHelper.ValidateField(Pd.ItemConditionId, "Item Condition", r.RowIndex);
				FormHelper.ValidateField(Pd.ItemId, "Item", r.RowIndex);
				FormHelper.ValidateField(Pd.ItemUomSchId, "Unit", r.RowIndex);
				FormHelper.ValidateField(Pd.IssueQty, "IssueQty", r.RowIndex);
				invGsStoreIssuanceHeader.gsStoreIssuanceDetails.Add(Pd);
				if (invGsStoreIssuanceHeader.RefDocumentTypeId == 0)
				{
					invGsStoreIssuanceHeader.RefDocumentTypeId = ((Pd.RefDocumentTypeId > 0) ? Pd.RefDocumentTypeId : 450);
					invGsStoreIssuanceHeader.RefDocNoId = ((Pd.RefDocId > 0) ? Pd.RefDocId : Pd.DepartRequestId);
				}
			}
			if (lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					invGsStoreIssuanceHeader.gsStoreIssuanceDetails.Add(lstRemoveRecord[i]);
				}
			}
			invGsStoreIssuanceHeader.AttachmentsList = new List<DMSAttachments>();
			invGsStoreIssuanceHeader.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvGsStoreIssuanceHeader>(invGsStoreIssuanceHeader, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			invGsStoreIssuanceHeader.AttachmentsValues = AttachmentsValues;
			invGsStoreIssuanceHeader.CustomAttachmentsValues = CustomAttachmentsValues;
			int code = InvGsStoreIssuanceHeader.Save(invGsStoreIssuanceHeader);
			int RecId = RECID;
			if (RECID == 0)
			{
				MessageBox.Show("Record Save SuccessFully" + invGsStoreIssuanceHeader.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update SuccessFully" + invGsStoreIssuanceHeader.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, DocumentTypeId, base.Name);
			}
			string ReqT = RequestType;
			formReset();
			bool shouldPrint = checkBox1.Checked;
			if (ChkStoreReturnLink.Checked && RecId == 0 && ReqT != "DeliveryOrder")
			{
				StoreReturn storeReturn = new StoreReturn(UserAccount);
				storeReturn.FormClosed += delegate
				{
					if (shouldPrint)
					{
						CommonServices.StoreIssuanceSlip452(code);
					}
				};
				storeReturn.Show();
				storeReturn.LoadIssuanceByRecId(code);
			}
			else if (shouldPrint)
			{
				PrintMethod(code);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvGsStoreIssuanceDetail Pd, GridEXRow r)
	{
		Pd.DepartRequestId = Conversion.ToInt(r.Cells["DepartmentRequestId"].Value);
		Pd.DepartRequestDetailId = Conversion.ToInt(r.Cells["DepartmentRequestDetailId"].Value);
		Pd.DepartmentRequestNo = Conversion.ToInt(r.Cells["DepartmentRequestNo"].Value);
		Pd.RefDocumentTypeId = Conversion.ToInt(r.Cells["DoDocumentTypeId"].Value);
		Pd.RefDocId = Conversion.ToInt(r.Cells["DoId"].Value);
		Pd.RefDocDetailId = Conversion.ToInt(r.Cells["DoDetailId"].Value);
		Pd.RefDocNo = Conversion.ToInt(r.Cells["DoNo"].Value);
		Pd.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
		Pd.CompanyName = Conversion.ToString(r.Cells["SupplierName"].Value);
		Pd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		Pd.ItemName = Conversion.ToString(r.Cells["Item"].Value);
		Pd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		Pd.Warehouseid = Conversion.ToInt(r.Cells["LocationId"].Value);
		Pd.WareHouseName = Conversion.ToString(r.Cells["WarehouseName"].Value);
		Pd.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
		Pd.rackName = Conversion.ToString(r.Cells["RackName"].Value);
		Pd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		Pd.ItemCondition = Conversion.ToString((object)r.Cells["ItemConditionId"].Text);
		Pd.BagTypeId = Conversion.ToInt(r.Cells["BagTypeId"].Value);
		Pd.ItemUomSchId = Conversion.ToInt(r.Cells["UnitId"].Value);
		Pd.UomCode = Conversion.ToString(r.Cells["Unit"].Value);
		Pd.Equivalent = Conversion.ToDouble(r.Cells["Equivalent"].Value);
		Pd.IssueQty = Conversion.ToDouble(r.Cells["IssueQty"].Value);
		Pd.QtyBal = Conversion.ToDouble(r.Cells["BalanceStock"].Value);
		Pd.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
		if (Pd.ItemId > 0 && Pd.ItemRate <= 0.0)
		{
			double AvgRate = 0.0;
			DataTable dtRate = CommonServices.GetAvgRateQtyAndStockInHand(Pd.ItemId, txtDocdate.Value, Pd.ItemConditionId, RECID, (RECID > 0) ? DocumentTypeId : 0);
			if (dtRate.Rows.Count > 0)
			{
				AvgRate = Math.Round(Conversion.ToDouble(dtRate.Rows[0]["AvgRate"]), 3);
			}
			if (AvgRate > 0.0)
			{
				Pd.ItemRate = AvgRate;
			}
		}
		Pd.ItemAmount = Pd.IssueQty * Pd.ItemRate;
		Pd.DepartmentId = Conversion.ToInt(r.Cells["DepartmentId"].Value);
		Pd.DepartmentName = Conversion.ToString((object)"Department");
		Pd.AssetsId = Conversion.ToInt(r.Cells["AssetId"].Value);
		Pd.AssetName = Conversion.ToString((object)"Asset");
		Pd.ReamarksDetail = Conversion.ToString((object)"Remarks");
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
			if (RECID == 0)
			{
				throw new Exception("Record id not found...");
			}
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
			formReset();
			RECID = ID;
			InvGsStoreIssuanceHeader head = InvGsStoreIssuanceHeader.GetByID(ID);
			if (head == null)
			{
				throw new Exception("Record Not Found");
			}
			btnsave.Visible = false;
			btnUpdate.Visible = true;
			btnDelete.Visible = formright.DoHaveCanDelete;
			tabControlGsIssuance.SelectedIndex = 0;
			UpdateMode = true;
			txtDocdate.Value = head.DocDate;
			txtDocNo.Text = head.DocNo.ToString();
			txtBranchSrNo.Text = head.BranchSrNo.ToString();
			txtRemarks.Text = head.Remarks;
			Approve = head.IsApproved;
			RequestType = ((head.RefDocumentTypeId == 85) ? "DeliveryOrder" : "DepartmentRequest");
			AttachmentsValues = head.AttachmentsValues;
			CustomAttachmentsValues = head.CustomAttachmentsValues;
			dtgrddetail.Clear();
			FilldtDetailFromListCommonForReadById(head, dtgrddetail);
			grdDetail.DataSource = dtgrddetail;
			grdDetail.RetrieveStructure();
			DetailGridSettings();
			FormHelper.LoadAttachmentsForObject(AT, RECID, base.Name);
			txtDocdate_ValueChanged(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FilldtDetailFromListCommonForReadById(InvGsStoreIssuanceHeader invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.gsStoreIssuanceDetails == null || invGrn.gsStoreIssuanceDetails.Count == 0)
		{
			return;
		}
		foreach (InvGsStoreIssuanceDetail detail in invGrn.gsStoreIssuanceDetails)
		{
			int bagTypeId = detail.BagTypeId;
			string bagType = ((bagTypeId == 1) ? "Normal" : "Retain");
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.DepartRequestId, detail.DepartRequestDetailId, detail.DepartmentRequestNo, detail.RefDocumentTypeId, detail.RefDocId, detail.RefDocDetailId, detail.RefDocNo, detail.SupplierCustomerId, detail.CompanyName,
				detail.ItemId, detail.ItemCode, detail.ItemName, detail.Warehouseid, detail.WareHouseName, detail.RackId, detail.rackName, detail.ItemConditionId, bagTypeId, bagType,
				detail.ItemUomSchId, detail.UomCode, detail.Equivalent, detail.ItemRate, detail.IssueQty, detail.RowBalanceQty, detail.QtyBal, detail.DepartmentId, detail.DepartmentName, detail.AssetsId,
				detail.AssetName, detail.ReamarksDetail
			};
			if (AddExtraColumns)
			{
				rowValues.Add(detail.ItemCondition);
			}
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void txtDocNo_TextChanged(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (!btnDelete.Visible || !btnDelete.Enabled)
			{
				return;
			}
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvGsStoreIssuanceHeader.DeleteByID(UserAccount.ID, RECID);
					MessageBox.Show("Delete Record Successfully");
					formReset();
				}
			}
			else
			{
				MessageBox.Show("Record Not Found");
			}
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
			List<string> allServices = new List<string> { "ItemConditions", "RacksWithWarehouseAndItem" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			GridCombosBind();
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
			Attachment.dtAttachments.Clear();
			RECID = 0;
			VoucherHeadId = 0;
			RequestType = "";
			btnsave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			UpdateMode = false;
			Approve = false;
			lstRemoveRecord.Clear();
			txtRemarks.Text = string.Empty;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			dtgrddetail.Clear();
			grdDetail.DataSource = null;
			GenerateDocNo();
			GenerateBranchSrNo();
			txtDocNo.Focus();
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
			CommonServices.StoreIssuanceSlip452(ReportId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (RequestType != "DeliveryOrder" && e.Column.Key == "IssueQty" && item != null && (int)item.RowType == 0 && item.Cells["IssueQty"].Value.ToString() != string.Empty && item.Cells["BalanceStock"].Value.ToString() != string.Empty)
			{
				double BalanceQty = Conversion.ToDouble((object)item.Cells["BalanceStock"].Value.ToString());
				if (Conversion.ToDouble((object)item.Cells["IssueQty"].Value.ToString()) > BalanceQty)
				{
					MessageBox.Show("IssueQty greater than BalanceQty Please Check");
					return;
				}
			}
			if (e.Column.Key == "LocationId" || e.Column.Key == "ItemConditionId")
			{
				item.Cells["BalanceStock"].Value = "0";
				item.Cells["ItemRate"].Value = "0";
				int itemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int LocationId = Conversion.ToInt(item.Cells["LocationId"].Value);
				int rackId = Conversion.ToInt(item.Cells["RackId"].Value);
				int ItemConditionId = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				DateTime DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
				DataTable dtStock = CommonServices.GetAvgRateQtyAndStockInHand(itemId, DocDate, ItemConditionId, RECID, (RECID > 0) ? DocumentTypeId : 0, LocationId, rackId);
				if (dtStock.Rows.Count > 0 && dtStock != null)
				{
					double ItemQty = Math.Round(Conversion.ToDouble(dtStock.Rows[0]["QtyInHand"]), 2);
					item.Cells["BalanceStock"].Value = ItemQty;
				}
				DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(itemId, DocDate, ItemConditionId, RECID, (RECID > 0) ? DocumentTypeId : 0);
				if (dt.Rows.Count > 0 && dt != null)
				{
					double ItemRate = Math.Round(Conversion.ToDouble(dt.Rows[0]["AvgRate"]), 3);
					item.Cells["ItemRate"].Value = ItemRate;
				}
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GridCombosBind()
	{
		DataTable dt = DatatableHelper.PopulateDataTableAndReturn(clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList(), (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
		GridEX_Helper.ComboBind(grdDetail.RootTable.Columns["ItemConditionId"], dt, "Id", "Description", "Item Condition", Constants.InventoryConstants.StringConstant);
	}

	private void DetailGridSettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ItemId", "UnitId", "BagTypeId", "ItemRate", "SupplierCustomerId", "DepartmentId", "Department", "AssetId", "DepartmentRequestId",
				"DepartmentRequestDetailId", "DoDocumentTypeId", "DoId", "DoDetailId", "Equivalent", "LocationId", "RackId"
			};
			if (RequestType == "DeliveryOrder")
			{
				hiddenColumns.AddRange(new string[2] { "Asset", "DepartmentRequestNo" });
			}
			else
			{
				hiddenColumns.AddRange(new string[3] { "BagType", "DoNo", "SupplierName" });
			}
			Dictionary<string, EditType> editCol = new Dictionary<string, EditType>
			{
				{
					"IssueQty",
					(EditType)1
				},
				{
					"Remarks",
					(EditType)1
				}
			};
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 3, 3, hiddenColumns, editCol);
			grdDetail.RootTable.Columns["RowBalanceQty"].Caption = "Request / Do Balance Qty";
			GridCombosBind();
			grdDetail.RootTable.Columns["Item"].Width = 250;
			grdDetail.RootTable.Columns["DepartmentRequestNo"].Width = 100;
			GridEX_Helper.AddButton(grdDetail, "Delete", "X", 25, 0);
			grdDetail.FrozenColumns = 1;
			((Control)(object)grdDetail).Refresh();
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r != null && (int)r.RowType == 0 && e.Column.Key == "Delete")
			{
				DeleteDetailRow(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeleteDetailRow(GridEXRow r)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvGsStoreIssuanceDetail d1 = new InvGsStoreIssuanceDetail();
					d1.Id = Conversion.ToInt(r.Cells["Id"].Value);
					FillDetailListCommonForInsertAndDelete(d1, r);
					d1.ActionTypeId = 3;
					lstRemoveRecord.Add(d1);
					dtgrddetail.Rows[r.RowIndex].Delete();
					r.Delete();
					grdDetail.UpdateData();
				}
			}
			else
			{
				r.Delete();
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
			if (RequestType == "" || RequestType == "DeliveryOrder")
			{
				RequestType = "DeliveryOrder";
				loadDO = new PendingDoPmForIssuance(UserAccount);
				loadDO.ShowDialog();
				loadDO.BringToFront();
				LoadInGridDetailDoPM();
				RequestType = ((grdDetail.RowCount == 0) ? "" : "DeliveryOrder");
				txtDocdate_ValueChanged(null, null);
				return;
			}
			throw new Exception("Data Against Department Request Loaded,Please Reset That First If You Want to Load Delivery Order");
		}
		catch (Exception ex)
		{
			RequestType = "";
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailDoPM()
	{
		try
		{
			DataTable dtLoader = loadDO.dtLoader;
			if (dtLoader == null || dtLoader.Rows.Count <= 0)
			{
				return;
			}
			HashSet<int> existingDetailIds = new HashSet<int>(from GridEXRow r in grdDetail.GetRows()
				select Conversion.ToInt(r.Cells["DoDetailId"].Value));
			foreach (DataRow row in loadDO.dtLoader.Rows)
			{
				int detailId = Conversion.ToInt(row["DetailId"]);
				if (!existingDetailIds.Add(detailId))
				{
					continue;
				}
				DataRow dr = dtgrddetail.NewRow();
				dr["Id"] = 0;
				dr["DepartmentRequestId"] = 0;
				dr["DepartmentRequestDetailId"] = 0;
				dr["DepartmentRequestNo"] = 0;
				dr["DoDocumentTypeId"] = Conversion.ToInt(row["DocumentTypeId"]);
				dr["DoId"] = Conversion.ToInt(row["Id"]);
				dr["DoDetailId"] = detailId;
				dr["DoNo"] = Conversion.ToInt(row["DocNo"]);
				dr["SupplierCustomerId"] = Conversion.ToInt(row["SupplierCustomerId"]);
				dr["SupplierName"] = Conversion.ToString(row["CompanyName"]);
				dr["ItemId"] = Conversion.ToInt(row["ItemId"]);
				dr["ItemCode"] = Conversion.ToString(row["ItemCode"]);
				dr["Item"] = Conversion.ToString(row["ItemName"]);
				dr["ItemConditionId"] = Conversion.ToInt(row["ItemConditionId"]);
				dr["BagTypeId"] = Conversion.ToInt(row["BagTypeId"]);
				dr["BagType"] = Conversion.ToString(row["BagType"]);
				dr["UnitId"] = Conversion.ToInt(row["PackUOMId"]);
				dr["Unit"] = Conversion.ToString(row["UOMCode"]);
				dr["Equivalent"] = Conversion.ToDouble(row["Equivalent"]);
				dr["ItemRate"] = 0;
				dr["IssueQty"] = Conversion.ToDouble(row["BalQty"]);
				dr["RowBalanceQty"] = Conversion.ToDouble(row["BalQty"]);
				dr["BalanceStock"] = Conversion.ToDouble(row["BalQty"]);
				dr["DepartmentId"] = 0;
				dr["Department"] = "";
				dr["AssetId"] = 0;
				dr["Asset"] = "";
				Dictionary<int, List<RacksWithWarehouseAndItem>> dictionary = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
				int WareHouseConfigId = 0;
				switch (Conversion.ToInt(row["ParentCategoryId"]))
				{
				case 7:
					WareHouseConfigId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
					break;
				case 8:
					WareHouseConfigId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow"));
					break;
				}
				int itemId = Conversion.ToInt(row["ItemId"]);
				if (dictionary.TryGetValue(itemId, out var itemData))
				{
					(int, string, int, string) result = ResolveWarehouseAndRack(itemData, WareHouseConfigId);
					if (result.Item1 > 0)
					{
						dr["LocationId"] = result.Item1;
						dr["WarehouseName"] = result.Item2;
					}
					if (result.Item3 > 0)
					{
						dr["RackId"] = result.Item3;
						dr["RackName"] = result.Item4;
					}
				}
				dr["Remarks"] = Conversion.ToString(row["LoadingRemarks"]);
				dtgrddetail.Rows.Add(dr);
			}
			grdDetail.DataSource = dtgrddetail;
			grdDetail.RetrieveStructure();
			DetailGridSettings();
			UpdateBalanceStock();
			loadDO.dtLoader.Rows.Clear();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BtnLoadDepRequest_Click(object sender, EventArgs e)
	{
		try
		{
			if (RequestType == "" || RequestType == "DepartmentRequest")
			{
				RequestType = "DepartmentRequest";
				loadDepReq = new LoadDepRequestToConsumableStore(UserAccount);
				loadDepReq.DocumentTypeId = 450;
				loadDepReq.ShowDialog();
				loadDepReq.BringToFront();
				LoadInGridDetailDepartmentRequest();
				RequestType = ((grdDetail.RowCount == 0) ? "" : "DepartmentRequest");
				txtDocdate_ValueChanged(null, null);
				return;
			}
			throw new Exception("Data Against Delivery Order Loaded,Please Reset That First If You Want to Load Department Request");
		}
		catch (Exception ex)
		{
			RequestType = "";
			MessageBox.Show(ex.Message);
		}
	}

	private void LoadInGridDetailDepartmentRequest()
	{
		try
		{
			DataTable dtLoader = loadDepReq.dtLoader;
			if (dtLoader != null && dtLoader.Rows.Count > 0)
			{
				HashSet<int> existingDetailIds = new HashSet<int>(from GridEXRow r in grdDetail.GetRows()
					select Conversion.ToInt(r.Cells["DepartmentRequestDetailId"].Value));
				foreach (DataRow row in loadDepReq.dtLoader.Rows)
				{
					int detailId = Conversion.ToInt(row["DetailId"]);
					if (!existingDetailIds.Add(detailId))
					{
						continue;
					}
					DataRow dr = dtgrddetail.NewRow();
					dr["Id"] = 0;
					dr["DepartmentRequestId"] = Conversion.ToInt(row["Id"]);
					dr["DepartmentRequestDetailId"] = detailId;
					dr["DepartmentRequestNo"] = Conversion.ToInt(row["DocNo"]);
					dr["DoDocumentTypeId"] = 0;
					dr["DoId"] = 0;
					dr["DoDetailId"] = 0;
					dr["DoNo"] = 0;
					dr["SupplierCustomerId"] = 0;
					dr["SupplierName"] = "";
					dr["ItemId"] = Conversion.ToInt(row["ItemId"]);
					dr["ItemCode"] = Conversion.ToString(row["ItemCode"]);
					dr["Item"] = Conversion.ToString(row["ItemName"]);
					dr["ItemConditionId"] = Conversion.ToInt(row["ItemConditionId"]);
					dr["BagTypeId"] = 0;
					dr["BagType"] = "";
					dr["UnitId"] = Conversion.ToInt(row["ItemUOMId"]);
					dr["Unit"] = Conversion.ToString(row["UOMCode"]);
					dr["Equivalent"] = Conversion.ToDouble(row["Equivalent"]);
					dr["ItemRate"] = 0;
					dr["IssueQty"] = Conversion.ToDouble(row["BalQty"]);
					dr["RowBalanceQty"] = Conversion.ToDouble(row["BalQty"]);
					dr["BalanceStock"] = Conversion.ToDouble(row["StockQtyBal"]);
					dr["DepartmentId"] = Conversion.ToInt(row["FromDepartmentId"]);
					dr["Department"] = Conversion.ToString(row["FromDepartment"]);
					dr["AssetId"] = Conversion.ToInt(row["AssetId"]);
					dr["Asset"] = Conversion.ToString(row["AssetName"]);
					dr["Remarks"] = "";
					Dictionary<int, List<RacksWithWarehouseAndItem>> dictionary = (from x in clsGlobalVariables.racksWithWarehouseAndItems
						group x by x.ItemId).ToDictionary((IGrouping<int, RacksWithWarehouseAndItem> g) => g.Key, (IGrouping<int, RacksWithWarehouseAndItem> g) => g.ToList());
					int WareHouseConfigId = 0;
					switch (Conversion.ToInt(row["ParentCategoryId"]))
					{
					case 7:
						WareHouseConfigId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialDefaultWarehouse"));
						break;
					case 8:
						WareHouseConfigId = Conversion.ToInt((object)GlobalVariables_Helper.GetConfigValueFromGlobal("DefaultWarehouseForStoreFlow"));
						break;
					}
					int itemId = Conversion.ToInt(row["ItemId"]);
					if (dictionary.TryGetValue(itemId, out var itemData))
					{
						(int, string, int, string) result = ResolveWarehouseAndRack(itemData, WareHouseConfigId);
						if (result.Item1 > 0)
						{
							dr["LocationId"] = result.Item1;
							dr["WarehouseName"] = result.Item2;
						}
						if (result.Item3 > 0)
						{
							dr["RackId"] = result.Item3;
							dr["RackName"] = result.Item4;
						}
					}
					dtgrddetail.Rows.Add(dr);
				}
				grdDetail.DataSource = dtgrddetail;
				grdDetail.RetrieveStructure();
				DetailGridSettings();
				UpdateBalanceStock();
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

	private void UpdateBalanceStock()
	{
		try
		{
			GridEXRow[] dataRows = grdDetail.GetDataRows();
			foreach (GridEXRow r in dataRows)
			{
				r.BeginEdit();
				r.Cells["BalanceStock"].Value = 0;
				int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				int LocationId = Conversion.ToInt(r.Cells["LocationId"].Value);
				int rackId = Conversion.ToInt(r.Cells["RackId"].Value);
				int ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
				DateTime DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
				DataTable dtStock = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, DocDate, ItemConditionId, RECID, (RECID > 0) ? DocumentTypeId : 0, LocationId, rackId);
				if (dtStock.Rows.Count > 0 && dtStock != null)
				{
					double ItemQty = Math.Round(Conversion.ToDouble(dtStock.Rows[0]["QtyInHand"]), 2);
					r.Cells["BalanceStock"].Value = ItemQty;
				}
				r.EndEdit();
			}
			((Control)(object)grdDetail).Update();
		}
		catch (Exception)
		{
			throw new Exception();
		}
	}

	private (int WarehouseId, string WarehouseName, int RackId, string RackName) ResolveWarehouseAndRack(List<RacksWithWarehouseAndItem> itemData, int configWarehouseId)
	{
		if (itemData == null || itemData.Count == 0)
		{
			return default((int, string, int, string));
		}
		RacksWithWarehouseAndItem baseRack = itemData.FirstOrDefault((RacksWithWarehouseAndItem x) => x.BaseRackId > 0);
		if (baseRack != null)
		{
			return (WarehouseId: baseRack.WarehouseId, WarehouseName: baseRack.WareHouseName, RackId: baseRack.Id, RackName: baseRack.RackName);
		}
		List<RacksWithWarehouseAndItem> warehouses = (from x in itemData
			group x by x.WarehouseId into g
			select g.First()).ToList();
		if (warehouses.Count == 1)
		{
			RacksWithWarehouseAndItem wh2 = warehouses[0];
			List<RacksWithWarehouseAndItem> racks = (from x in itemData
				where x.WarehouseId == wh2.WarehouseId
				group x by x.Id into g
				select g.First()).ToList();
			if (racks.Count == 1)
			{
				return (WarehouseId: wh2.WarehouseId, WarehouseName: wh2.WareHouseName, RackId: racks[0].Id, RackName: racks[0].RackName);
			}
			return (WarehouseId: wh2.WarehouseId, WarehouseName: wh2.WareHouseName, RackId: 0, RackName: null);
		}
		if (configWarehouseId > 0)
		{
			RacksWithWarehouseAndItem wh = warehouses.FirstOrDefault((RacksWithWarehouseAndItem x) => x.WarehouseId == configWarehouseId);
			if (wh != null)
			{
				List<RacksWithWarehouseAndItem> racks2 = (from x in itemData
					where x.WarehouseId == wh.WarehouseId
					group x by x.Id into g
					select g.First()).ToList();
				if (racks2.Count == 1)
				{
					return (WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: racks2[0].Id, RackName: racks2[0].RackName);
				}
				return (WarehouseId: wh.WarehouseId, WarehouseName: wh.WareHouseName, RackId: 0, RackName: null);
			}
		}
		return default((int, string, int, string));
	}

	private void tabControlGsIssuance_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControlGsIssuance.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				txtDocdate.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			FromDocNoHistory.Text = string.Empty;
			ToDocNoHistory.Text = string.Empty;
			DataGridHistory.DataSource = null;
			grddetailHistory.DataSource = null;
			FromDateHistory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnShowHistory_Click(object sender, EventArgs e)
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

	private void HistoryFill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				CanViewAllRecord = formright.DoHaveCanViewAllRecordRights
			};
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			if (FromDateHistory.Checked)
			{
				obj.FromDate = FromDateHistory.Value;
			}
			if (ToDateHistory.Checked)
			{
				obj.ToDate = ToDateHistory.Value;
			}
			obj.FromDocNo = Conversion.ToInt((object)FromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)ToDocNoHistory.Text);
			DataTable dt = InvGsStoreIssuanceHeader.FormHistory(obj);
			if (dt != null && dt.Rows.Count > 0)
			{
				if (dtGridHistory.Columns.Count == 0)
				{
					dtGridHistory.Columns.Add("RecordNo", typeof(int));
					dtGridHistory.Columns.Add("Id", typeof(int));
					dtGridHistory.Columns.Add("DocNo", typeof(int));
					dtGridHistory.Columns.Add("DocDate", typeof(DateTime));
					dtGridHistory.Columns.Add("IssuanceType", typeof(string));
					dtGridHistory.Columns.Add("Remarks", typeof(string));
					dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
					dtGridHistory.Columns.Add("EntryUser", typeof(string));
					dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
					dtGridHistory.Columns.Add("ModifyUser", typeof(string));
					dtGridHistory.Columns.Add("NoOfAttachments", typeof(int));
					dtGridHistory.Columns.Add("BranchSrNo", typeof(int));
				}
				dtGridHistory.Clear();
				HashSet<int> existingIds = new HashSet<int>();
				foreach (DataRow row in dt.Rows)
				{
					int id = Conversion.ToInt(row["Id"]);
					if (existingIds.Add(id))
					{
						string issuanceType = ((Conversion.ToInt(row["RefDocumentTypeId"]) == 85) ? "DeliveryOrder" : "DepartmentRequest");
						dtGridHistory.Rows.Add(row["RecordNo"], id, row["DocNo"], row["DocDate"], issuanceType, row["Remarks"], row["EntryDate"], row["EntryUserName"], row["ModifyDate"], row["ModifyUserName"], row["NoOfAttachments"], row["BranchSrNo"]);
					}
				}
				DataGridHistory.DataSource = dtGridHistory;
				DataGridHistory.RetrieveStructure();
				Grdhistorysettings();
			}
			else
			{
				DataGridHistory.DataSource = null;
				grddetailHistory.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Grdhistorysettings()
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "RecordNo" };
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3, hiddenColumns);
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DataGridHistory.RootTable.Columns["IssuanceType"].Width = 150;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			int Pos = DataGridHistory.RootTable.Columns["RecordNo"].Position;
			if (formright.DoHavePrintRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Print", "Print", 40, (Pos != 0) ? (Pos - 1) : 0);
			}
			if (formright.DoHaveUpdateRights)
			{
				GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 40, (Pos != 0) ? (Pos - 1) : 0);
			}
			DataGridHistory.FrozenColumns = 2;
			((Control)(object)DataGridHistory).Refresh();
			ctrlGrdBar2_Load(null, null);
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
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), DocumentTypeId);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = DataGridHistory.CurrentRow;
			if (item != null && item.Cells.Count > 0 && (int)item.RowType == 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(item.Cells["Id"].Value), Conversion.ToString(item.Cells["IssuanceType"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id, string Type)
	{
		try
		{
			InvGsStoreIssuanceHeader Obj = InvGsStoreIssuanceHeader.GetByID(Id);
			if (Obj == null || Obj.gsStoreIssuanceDetails.Count == 0)
			{
				grddetailHistory.DataSource = null;
				return;
			}
			DataTable dtDetailHistory = dtgrddetail.Clone();
			dtDetailHistory.Columns.Add("ItemCondition");
			FilldtDetailFromListCommonForReadById(Obj, dtDetailHistory, AddExtraColumns: true);
			grddetailHistory.DataSource = dtDetailHistory;
			grddetailHistory.RetrieveStructure();
			grddetailhistorySettings(Type);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings(string Type)
	{
		try
		{
			List<string> hiddenColumns = new List<string>
			{
				"Id", "ItemId", "UnitId", "BagTypeId", "ItemRate", "SupplierCustomerId", "DepartmentId", "Department", "AssetId", "BalanceStock",
				"RowBalanceQty", "DepartmentRequestId", "DepartmentRequestDetailId", "DoDocumentTypeId", "DoId", "DoDetailId", "LocationId"
			};
			if (Type == "DeliveryOrder")
			{
				hiddenColumns.AddRange(new string[2] { "Asset", "DepartmentRequestNo" });
			}
			else
			{
				hiddenColumns.AddRange(new string[3] { "BagType", "DoNo", "SupplierName" });
			}
			GridEX_Helper.GridWrappingAndColumnSettings(grddetailHistory, 2, 3, hiddenColumns);
			grddetailHistory.RootTable.Columns["Item"].Width = 250;
			grddetailHistory.RootTable.Columns["DepartmentRequestNo"].Width = 100;
			grddetailHistory.RootTable.Columns["ItemCondition"].Position = grddetailHistory.RootTable.Columns["ItemConditionId"].Position + 1;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
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

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			if (FormInitialized)
			{
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grdDetail, dtgrddetail);
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, DataGridHistory, dtGridHistory);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void txtDocdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] dataRows = grdDetail.GetDataRows();
			foreach (GridEXRow item in dataRows)
			{
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				int LocationId = Conversion.ToInt(item.Cells["LocationId"].Value);
				int ItemConditionId = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				DateTime DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
				DataTable dtStock = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, DocDate, ItemConditionId, RECID, (RECID > 0) ? DocumentTypeId : 0, LocationId);
				item.BeginEdit();
				if (dtStock.Rows.Count > 0 && dtStock != null)
				{
					double ItemQty = Math.Round(Conversion.ToDouble(dtStock.Rows[0]["QtyInHand"]), 2);
					item.Cells["BalanceStock"].Value = ItemQty;
				}
				else
				{
					item.Cells["BalanceStock"].Value = "0";
				}
				DataTable dt = CommonServices.GetAvgRateQtyAndStockInHand(ItemId, DocDate, ItemConditionId, RECID, (RECID > 0) ? DocumentTypeId : 0);
				if (dt.Rows.Count > 0 && dt != null)
				{
					double ItemRate = Math.Round(Conversion.ToDouble(dt.Rows[0]["AvgRate"]), 3);
					item.Cells["ItemRate"].Value = ItemRate;
				}
				else
				{
					item.Cells["ItemRate"].Value = "0";
				}
				item.EndEdit();
			}
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			if (e.Control && e.KeyCode == Keys.S)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					if (!UpdateMode)
					{
						btnsave_Click(null, null);
					}
				}
				else if (tabControlGsIssuance.SelectedIndex == 1)
				{
					btnShowHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.N)
			{
				if (tabControlGsIssuance.SelectedIndex == 0)
				{
					btnnew_Click(null, null);
				}
				else if (tabControlGsIssuance.SelectedIndex == 1)
				{
					btnNewHistory_Click(null, null);
				}
			}
			if (e.Control && e.KeyCode == Keys.T)
			{
				if (tabControlGsIssuance.SelectedIndex == 1)
				{
					tabControlGsIssuance.SelectedIndex = 0;
					txtDocdate.Focus();
				}
				else
				{
					tabControlGsIssuance.SelectedIndex = 1;
					FromDateHistory.Focus();
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
			if (e.Control)
			{
				_ = e.KeyCode;
				_ = 76;
			}
			if (e.Control && e.KeyCode == Keys.F5)
			{
				txtDocdate.Focus();
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
					FromDateHistory.Focus();
				}
			}
			if (e.Control && e.KeyCode == Keys.Down)
			{
				if (tabControlGsIssuance.SelectedIndex == 1)
				{
					((Control)(object)DataGridHistory).Focus();
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
			if (e.Control && e.KeyCode == Keys.L && tabControlGsIssuance.SelectedIndex == 0)
			{
				if (RequestType == "DeliveryOrder")
				{
					btnLoadRequest_Click(null, null);
				}
				else
				{
					BtnLoadDepRequest_Click(null, null);
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
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+L", "To Load Request");
			dt.Rows.Add("Ctrl+F5", "For Focus on DocDate");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("F1", "Select Warehouse and Rack on F1 when focus on Warehouse or Rack Column in detail grid");
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
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), DocumentTypeId);
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

	private void grdDetail_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdDetail.CurrentRow;
			if (r == null || (int)r.RowType != 0 || grdDetail.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdDetail.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			if (e.KeyCode != Keys.F1)
			{
				return;
			}
			int WarehouseId = Conversion.ToInt(r.Cells["LocationId"].Value);
			int ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			int RackId = Conversion.ToInt(r.Cells["RackId"].Value);
			if (grdDetail.CurrentColumn.Key == "LocationId" || grdDetail.CurrentColumn.Key == "WarehouseName")
			{
				List<RacksWithWarehouseAndItem> list = (from x in clsGlobalVariables.racksWithWarehouseAndItems
					where x.ItemId == ItemId
					group x by x.WarehouseId into g
					select g.First()).ToList();
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.BeginLoadData();
				foreach (RacksWithWarehouseAndItem w in list)
				{
					dt.Rows.Add(w.WarehouseId, w.WareHouseName);
				}
				dt.EndLoadData();
				using (GrdPopUp obj = new GrdPopUp(dt, "Id", "WareHouseName"))
				{
					obj.StartPosition = FormStartPosition.CenterScreen;
					obj.ShowDialog();
					int selectedId2 = Conversion.ToInt((object)obj.ReturnId);
					string selectedName = Conversion.ToString((object)obj.ReturnName);
					r.Cells["LocationId"].Value = selectedId2;
					r.Cells["WarehouseName"].Value = selectedName;
					List<RacksWithWarehouseAndItem> racks = clsGlobalVariables.racksWithWarehouseAndItems.Where((RacksWithWarehouseAndItem x) => x.ItemId == ItemId && x.WarehouseId == selectedId2).ToList();
					if (RackId > 0)
					{
						if (!racks.Any((RacksWithWarehouseAndItem x) => x.Id == RackId))
						{
							if (racks.Count > 1)
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
							else if (racks.Count == 1)
							{
								RacksWithWarehouseAndItem singleRack = racks.First();
								r.Cells["RackId"].Value = singleRack.Id;
								r.Cells["RackName"].Value = singleRack.RackName;
							}
							else
							{
								r.Cells["RackId"].Value = 0;
								r.Cells["RackName"].Value = "";
							}
						}
					}
					else if (racks.Count == 1)
					{
						RacksWithWarehouseAndItem singleRack2 = racks.First();
						r.Cells["RackId"].Value = singleRack2.Id;
						r.Cells["RackName"].Value = singleRack2.RackName;
					}
				}
				grdDetail.UpdateData();
				GetStock(r);
			}
			else
			{
				if (!(grdDetail.CurrentColumn.Key == "RackId") && !(grdDetail.CurrentColumn.Key == "RackName"))
				{
					return;
				}
				List<RacksWithWarehouseAndItem> filteredRacks = (from rr in clsGlobalVariables.racksWithWarehouseAndItems
					where rr.ItemId == ItemId && (WarehouseId == 0 || rr.WarehouseId == WarehouseId)
					select rr into row
					group row by row.Id into g
					select g.First()).ToList();
				if (filteredRacks.Count == 0)
				{
					return;
				}
				DataTable dt2 = new DataTable();
				dt2.Columns.Add("Id", typeof(int));
				dt2.Columns.Add("RackName", typeof(string));
				dt2.Columns.Add("WarehouseId", typeof(int));
				dt2.Columns.Add("WarehouseName", typeof(string));
				dt2.BeginLoadData();
				foreach (RacksWithWarehouseAndItem item in filteredRacks)
				{
					dt2.Rows.Add(item.Id, item.RackName, item.WarehouseId, item.WareHouseName);
				}
				dt2.EndLoadData();
				using (GrdPopUp grdPopUp = new GrdPopUp(dt2, "Id", "RackName", "WarehouseName"))
				{
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					int selectedId = Conversion.ToInt((object)grdPopUp.ReturnId);
					string selectedName2 = Conversion.ToString((object)grdPopUp.ReturnName);
					r.Cells["RackId"].Value = selectedId;
					r.Cells["RackName"].Value = selectedName2;
					if (WarehouseId == 0)
					{
						RacksWithWarehouseAndItem selected = filteredRacks.FirstOrDefault((RacksWithWarehouseAndItem x) => x.Id == selectedId);
						if (selected != null)
						{
							r.Cells["LocationId"].Value = selected.WarehouseId;
							r.Cells["WarehouseName"].Value = selected.WareHouseName;
						}
					}
				}
				grdDetail.UpdateData();
				GetStock(r);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetStock(GridEXRow r)
	{
		try
		{
			r.BeginEdit();
			r.Cells["BalanceStock"].Value = 0;
			int itemId = Conversion.ToInt(r.Cells["ItemId"].Value);
			int LocationId = Conversion.ToInt(r.Cells["LocationId"].Value);
			int rackId = Conversion.ToInt(r.Cells["RackId"].Value);
			int ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
			DateTime DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			DataTable dtStock = CommonServices.GetAvgRateQtyAndStockInHand(itemId, DocDate, ItemConditionId, RECID, (RECID > 0) ? DocumentTypeId : 0, LocationId, rackId);
			if (dtStock.Rows.Count > 0 && dtStock != null)
			{
				double ItemQty = Math.Round(Conversion.ToDouble(dtStock.Rows[0]["QtyInHand"]), 2);
				r.Cells["BalanceStock"].Value = ItemQty;
			}
			r.EndEdit();
			grdDetail.UpdateData();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BtnStoreReturnLink_Click(object sender, EventArgs e)
	{
		try
		{
			StoreReturn storeReturn = new StoreReturn(UserAccount);
			storeReturn.Show();
			storeReturn.BringToFront();
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Expected O, but got Unknown
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Expected O, but got Unknown
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Expected O, but got Unknown
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Expected O, but got Unknown
		//IL_1a6c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a76: Expected O, but got Unknown
		//IL_1a9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa4: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grddetailHistory_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmGSIssuance));
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
		this.txtBranchSrNo = new System.Windows.Forms.TextBox();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label3 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ChkStoreReturnLink = new System.Windows.Forms.CheckBox();
		this.BtnStoreReturnLink = new System.Windows.Forms.Button();
		this.checkBox1 = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnattachment = new System.Windows.Forms.ToolStripButton();
		this.BtnLoadDepRequest = new System.Windows.Forms.ToolStripButton();
		this.btnLoadDO = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DataGridHistory = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel9 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.btnShowHistory = new System.Windows.Forms.Button();
		this.label54 = new System.Windows.Forms.Label();
		this.label55 = new System.Windows.Forms.Label();
		this.label56 = new System.Windows.Forms.Label();
		this.label57 = new System.Windows.Forms.Label();
		this.ToDocNoHistory = new System.Windows.Forms.TextBox();
		this.FromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.label7 = new System.Windows.Forms.Label();
		this.panel8 = new System.Windows.Forms.Panel();
		this.label6 = new System.Windows.Forms.Label();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.grddetailHistory = new GridEX();
		this.panel1.SuspendLayout();
		this.tabControlGsIssuance.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel11.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel6.SuspendLayout();
		this.panel5.SuspendLayout();
		this.groupBox1.SuspendLayout();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel12.SuspendLayout();
		this.panel9.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel8.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlGsIssuance);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(964, 623);
		this.panel1.TabIndex = 0;
		this.tabControlGsIssuance.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlGsIssuance.Controls.Add(this.tabPage1);
		this.tabControlGsIssuance.Controls.Add(this.tabPage2);
		this.tabControlGsIssuance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlGsIssuance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControlGsIssuance.Location = new System.Drawing.Point(0, 0);
		this.tabControlGsIssuance.Name = "tabControlGsIssuance";
		this.tabControlGsIssuance.SelectedIndex = 0;
		this.tabControlGsIssuance.Size = new System.Drawing.Size(964, 623);
		this.tabControlGsIssuance.TabIndex = 0;
		this.tabControlGsIssuance.SelectedIndexChanged += new System.EventHandler(tabControlGsIssuance_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(956, 594);
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
		this.panel2.Size = new System.Drawing.Size(950, 588);
		this.panel2.TabIndex = 0;
		this.panel11.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel11.Location = new System.Drawing.Point(0, 130);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(950, 458);
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
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(950, 458);
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
		this.panel6.Location = new System.Drawing.Point(0, 105);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(950, 25);
		this.panel6.TabIndex = 3;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(910, 0);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 23);
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
		this.panel5.Size = new System.Drawing.Size(950, 52);
		this.panel5.TabIndex = 2;
		this.groupBox1.Controls.Add(this.txtBranchSrNo);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtDocNo);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(942, 47);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.txtBranchSrNo.BackColor = System.Drawing.Color.White;
		this.txtBranchSrNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtBranchSrNo.Enabled = false;
		this.txtBranchSrNo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtBranchSrNo.Location = new System.Drawing.Point(165, 15);
		this.txtBranchSrNo.Name = "txtBranchSrNo";
		this.txtBranchSrNo.Size = new System.Drawing.Size(64, 23);
		this.txtBranchSrNo.TabIndex = 1;
		this.txtBranchSrNo.TabStop = false;
		this.txtRemarks.Location = new System.Drawing.Point(405, 15);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(532, 23);
		this.txtRemarks.TabIndex = 3;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(348, 19);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(52, 15);
		this.label3.TabIndex = 79;
		this.label3.Text = "Remarks";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(4, 19);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(88, 15);
		this.label1.TabIndex = 79;
		this.label1.Text = "Doc No && Date";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(92, 15);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.ReadOnly = true;
		this.txtDocNo.ShortcutsEnabled = false;
		this.txtDocNo.Size = new System.Drawing.Size(71, 23);
		this.txtDocNo.TabIndex = 0;
		this.txtDocNo.Leave += new System.EventHandler(txtDocNo_TextChanged);
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(231, 15);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(116, 23);
		this.txtDocdate.TabIndex = 2;
		this.txtDocdate.ValueChanged += new System.EventHandler(txtDocdate_ValueChanged);
		this.panel4.BackColor = System.Drawing.Color.Teal;
		this.panel4.Controls.Add(this.ChkStoreReturnLink);
		this.panel4.Controls.Add(this.BtnStoreReturnLink);
		this.panel4.Controls.Add(this.checkBox1);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(950, 24);
		this.panel4.TabIndex = 1;
		this.ChkStoreReturnLink.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkStoreReturnLink.AutoSize = true;
		this.ChkStoreReturnLink.BackColor = System.Drawing.Color.White;
		this.ChkStoreReturnLink.Checked = true;
		this.ChkStoreReturnLink.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkStoreReturnLink.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkStoreReturnLink.ForeColor = System.Drawing.Color.Blue;
		this.ChkStoreReturnLink.Location = new System.Drawing.Point(717, 6);
		this.ChkStoreReturnLink.Margin = new System.Windows.Forms.Padding(2);
		this.ChkStoreReturnLink.Name = "ChkStoreReturnLink";
		this.ChkStoreReturnLink.Size = new System.Drawing.Size(15, 14);
		this.ChkStoreReturnLink.TabIndex = 32;
		this.ChkStoreReturnLink.TabStop = false;
		this.ChkStoreReturnLink.UseVisualStyleBackColor = false;
		this.BtnStoreReturnLink.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.BtnStoreReturnLink.BackColor = System.Drawing.Color.White;
		this.BtnStoreReturnLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.BtnStoreReturnLink.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.BtnStoreReturnLink.ForeColor = System.Drawing.Color.Blue;
		this.BtnStoreReturnLink.Location = new System.Drawing.Point(714, 0);
		this.BtnStoreReturnLink.Name = "BtnStoreReturnLink";
		this.BtnStoreReturnLink.Size = new System.Drawing.Size(144, 25);
		this.BtnStoreReturnLink.TabIndex = 31;
		this.BtnStoreReturnLink.Text = "Add Replacement";
		this.BtnStoreReturnLink.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
		this.BtnStoreReturnLink.UseVisualStyleBackColor = false;
		this.BtnStoreReturnLink.Click += new System.EventHandler(BtnStoreReturnLink_Click);
		this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.checkBox1.AutoSize = true;
		this.checkBox1.Checked = true;
		this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
		this.checkBox1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.checkBox1.ForeColor = System.Drawing.Color.White;
		this.checkBox1.Location = new System.Drawing.Point(864, 3);
		this.checkBox1.Name = "checkBox1";
		this.checkBox1.Size = new System.Drawing.Size(84, 20);
		this.checkBox1.TabIndex = 5;
		this.checkBox1.Text = "Preview";
		this.checkBox1.UseVisualStyleBackColor = true;
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
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[10] { this.btnnew, this.btnRefresh, this.btnsave, this.btnDelete, this.btnUpdate, this.Print, this.btnattachment, this.BtnLoadDepRequest, this.btnLoadDO, this.BtnShortCutkeys });
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
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
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
		this.Print.Size = new System.Drawing.Size(92, 24);
		this.Print.Text = "452-Print";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnattachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnattachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnattachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnattachment.Name = "btnattachment";
		this.btnattachment.Size = new System.Drawing.Size(109, 24);
		this.btnattachment.Text = "&Attachment";
		this.btnattachment.Click += new System.EventHandler(btnattachment_Click);
		this.BtnLoadDepRequest.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnLoadDepRequest.Image = (System.Drawing.Image)resources.GetObject("BtnLoadDepRequest.Image");
		this.BtnLoadDepRequest.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadDepRequest.Name = "BtnLoadDepRequest";
		this.BtnLoadDepRequest.Size = new System.Drawing.Size(120, 24);
		this.BtnLoadDepRequest.Text = "&Load Request";
		this.BtnLoadDepRequest.Click += new System.EventHandler(BtnLoadDepRequest_Click);
		this.btnLoadDO.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadDO.Image = (System.Drawing.Image)resources.GetObject("btnLoadDO.Image");
		this.btnLoadDO.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadDO.Name = "btnLoadDO";
		this.btnLoadDO.Size = new System.Drawing.Size(110, 24);
		this.btnLoadDO.Text = "&Load DO PM";
		this.btnLoadDO.Click += new System.EventHandler(btnLoadRequest_Click);
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
		this.tabPage2.Controls.Add(this.label7);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grddetailHistory);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(956, 594);
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
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(3, 128);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(950, 214);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 1;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.ctrlGrdBar2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 104);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(950, 24);
		this.panel12.TabIndex = 72;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(906, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.panel9.Controls.Add(this.groupBox3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel9.Location = new System.Drawing.Point(3, 50);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(950, 54);
		this.panel9.TabIndex = 0;
		this.groupBox3.Controls.Add(this.btnShowHistory);
		this.groupBox3.Controls.Add(this.label54);
		this.groupBox3.Controls.Add(this.label55);
		this.groupBox3.Controls.Add(this.label56);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.ToDocNoHistory);
		this.groupBox3.Controls.Add(this.FromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(1, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(755, 47);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.btnShowHistory.BackColor = System.Drawing.Color.Teal;
		this.btnShowHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnShowHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnShowHistory.ForeColor = System.Drawing.Color.White;
		this.btnShowHistory.Location = new System.Drawing.Point(688, 14);
		this.btnShowHistory.Margin = new System.Windows.Forms.Padding(2);
		this.btnShowHistory.Name = "btnShowHistory";
		this.btnShowHistory.Size = new System.Drawing.Size(62, 25);
		this.btnShowHistory.TabIndex = 4;
		this.btnShowHistory.Text = "Show";
		this.btnShowHistory.UseVisualStyleBackColor = false;
		this.btnShowHistory.Click += new System.EventHandler(btnShowHistory_Click);
		this.label54.AutoSize = true;
		this.label54.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label54.Location = new System.Drawing.Point(557, 19);
		this.label54.Name = "label54";
		this.label54.Size = new System.Drawing.Size(64, 15);
		this.label54.TabIndex = 86;
		this.label54.Text = "To Doc No";
		this.label55.AutoSize = true;
		this.label55.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label55.Location = new System.Drawing.Point(410, 19);
		this.label55.Name = "label55";
		this.label55.Size = new System.Drawing.Size(79, 15);
		this.label55.TabIndex = 85;
		this.label55.Text = "From Doc No";
		this.label56.AutoSize = true;
		this.label56.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label56.Location = new System.Drawing.Point(216, 19);
		this.label56.Name = "label56";
		this.label56.Size = new System.Drawing.Size(48, 15);
		this.label56.TabIndex = 84;
		this.label56.Text = "To Date";
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label57.Location = new System.Drawing.Point(5, 19);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(63, 15);
		this.label57.TabIndex = 83;
		this.label57.Text = "From Date";
		this.ToDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.ToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDocNoHistory.Location = new System.Drawing.Point(624, 15);
		this.ToDocNoHistory.Name = "ToDocNoHistory";
		this.ToDocNoHistory.ShortcutsEnabled = false;
		this.ToDocNoHistory.Size = new System.Drawing.Size(62, 23);
		this.ToDocNoHistory.TabIndex = 3;
		this.ToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(ToDocNoHistory_KeyPress);
		this.FromDocNoHistory.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.FromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDocNoHistory.Location = new System.Drawing.Point(491, 15);
		this.FromDocNoHistory.Name = "FromDocNoHistory";
		this.FromDocNoHistory.ShortcutsEnabled = false;
		this.FromDocNoHistory.Size = new System.Drawing.Size(62, 23);
		this.FromDocNoHistory.TabIndex = 2;
		this.FromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(FromDocNoHistory_KeyPress);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(264, 15);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(142, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(71, 15);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(142, 23);
		this.FromDateHistory.TabIndex = 0;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 342);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(950, 18);
		this.label7.TabIndex = 24;
		this.label7.Text = "Detail of the Above Selected Row";
		this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.label6);
		this.panel8.Controls.Add(this.toolStrip2);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(3, 3);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(950, 47);
		this.panel8.TabIndex = 65;
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.Teal;
		this.label6.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label6.Location = new System.Drawing.Point(5, 26);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(192, 19);
		this.label6.TabIndex = 2;
		this.label6.Text = "Store Issuance History";
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistory });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(950, 25);
		this.toolStrip2.TabIndex = 1;
		this.toolStrip2.Text = "toolStrip2";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(55, 22);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.grddetailHistory.AllowEdit = (InheritableBoolean)2;
		grddetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grddetailHistory.DesignTimeLayout = grddetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grddetailHistory.DynamicFiltering = true;
		this.grddetailHistory.FilterMode = (FilterMode)2;
		this.grddetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grddetailHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Location = new System.Drawing.Point(3, 360);
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Name = "grddetailHistory";
		((ScrollableBase)this.grddetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grddetailHistory).Size = new System.Drawing.Size(950, 231);
		((System.Windows.Forms.Control)(object)this.grddetailHistory).TabIndex = 2;
		this.grddetailHistory.TotalRow = (InheritableBoolean)1;
		this.grddetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grddetailHistory).VisualStyle = (VisualStyle)4;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(964, 623);
		base.Controls.Add(this.panel1);
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmGSIssuance";
		this.Text = "EccountBook ERP";
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
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grddetailHistory).EndInit();
		base.ResumeLayout(false);
	}
}
