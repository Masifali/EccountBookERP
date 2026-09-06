using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL;
using Architecture.BLL.FixedAssets;
using Architecture.BLL.Inventory;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.FixedAssets;
using Architecture.Model.Inventory;
using Architecture.Model.Main;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Lookups;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.StoreManagement;

public class frmPurchaseDemand : Form
{
	private int RECID;

	private int ItemId;

	private int updateDetailIndex = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool UpdateMode;

	private bool ItemSearchByNameOrCode;

	private string ScreenName = "frmPurchaseDemand";

	private int ParentCategoryId = 8;

	private DataTable dtItem = new DataTable();

	private DataTable dtgrddetail = new DataTable();

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private HashSet<int> DetailIdsToDelete = new HashSet<int>();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private TabPage tabPage2;

	private Panel panel4;

	private Panel panel3;

	private Panel panel2;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNew;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripButton btnPrint;

	private Label labelFormCaption;

	private DateTimePicker txtDocdate;

	private Label label1;

	private Label label6;

	private TextBox txtdocnumber;

	private TextBox txtRemarks;

	private Label label5;

	internal UltraCombo cmbDepartment;

	private Label label8;

	private TextBox txtRequisition;

	internal UltraCombo cmbAssetsRef;

	private Label label11;

	internal UltraCombo cmbUOM;

	internal UltraCombo cmbLot;

	internal UltraCombo cmbItem;

	private Label label44;

	private Label label9;

	private TextBox txtRequiredQty;

	private Label label18;

	private Label label10;

	private TextBox txtLastRate;

	private Label label14;

	private TextBox txtLastQty;

	private Label label13;

	private Label label12;

	private Button btnCancelDetail;

	private Button btnUpdateDetail;

	private Button Add;

	private Panel panel5;

	internal GridEX grdPurchaseDemand;

	private Panel panel6;

	private Panel panel8;

	private Panel panel7;

	private ToolStrip toolStrip2;

	private ToolStripSeparator toolStripSeparator1;

	private Label label43;

	internal GridEX DatagridHistory;

	private CtrlGrdBar ctrlGrdBar1;

	private Button button2;

	private ToolStripButton btnRefresh;

	private Button btnAssetRefDefine;

	private Label label3;

	private TextBox txtBarcodeReader;

	private ToolStripButton btnLoadAll;

	private Label label4;

	private TextBox txtRemarksDetail;

	private ToolStripButton btnAttachment;

	private CheckBox chkPrintPreview;

	private RadioButton rdSearchByName;

	private RadioButton rdSearchByCode;

	private GroupBox groupBox1;

	private GroupBox groupBox2;

	private BackgroundWorker backgroundWorker1;

	private Panel panel11;

	private Label label15;

	private CtrlGrdBar ctrlGrdBar2;

	private Label label7;

	internal GridEX grdDetail;

	private ToolStripButton btnshortcutkeys;

	private ToolStripButton btnCity;

	private ToolStripButton btnNewHistory;

	private ToolStripButton btnRefreshHistory;

	private Panel panel10;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label57;

	private Label label58;

	private Label label59;

	private Label label60;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private Panel panel13;

	private Panel panel12;

	private Label label2;

	internal UltraCombo CmbParentCategory;

	private Label label85;

	private TextBox txtOutstandingDemandAndPOQty;

	private Label label16;

	private TextBox txtAvaialableStockQty;

	private Label label17;

	private TextBox txtLastDate;

	internal UltraCombo CmbItemCondition;

	private Label label25;

	public frmPurchaseDemand(UserAccount user)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = user;
	}

	private void frmPurchaseDemand_Load(object sender, EventArgs e)
	{
		try
		{
			AutoScroll = true;
			base.AutoScrollMargin = new Size(20, 20);
			base.AutoScrollMinSize = new Size(1030, 700);
			ScreenName = Conversion.ToString(base.Tag);
			if (ScreenName == "frmPurchaseDemandPackingMaterial")
			{
				labelFormCaption.Text = "Purchase Demand Packing Material";
				ParentCategoryId = 7;
			}
			else
			{
				labelFormCaption.Text = "Purchase Demand Store";
				ParentCategoryId = 8;
			}
			btnUpdate.Visible = false;
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			btnSave.Enabled = formrights.DoHaveSaveRight;
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			btnPrint.Enabled = formrights.DoHavePrintRights;
			chkPrintPreview.Checked = formrights.DoHavePrintRights;
			chkPrintPreview.Enabled = formrights.DoHavePrintRights;
			dtItem.Columns.Add("Id", typeof(int));
			dtItem.Columns.Add("ItemName");
			dtItem.Columns.Add("ItemCode");
			ItemSearchByNameOrCode = Conversion.ToBool((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("ItemSearchByCode"));
			if (ItemSearchByNameOrCode)
			{
				rdSearchByCode.Checked = true;
			}
			else
			{
				rdSearchByName.Checked = true;
			}
			dtgrddetail.Columns.Add("Id", typeof(int));
			dtgrddetail.Columns.Add("Department", typeof(string));
			dtgrddetail.Columns.Add("DepartmentId", typeof(int));
			dtgrddetail.Columns.Add("RequistionBy", typeof(string));
			dtgrddetail.Columns.Add("ItemId", typeof(int));
			dtgrddetail.Columns.Add("ItemCode", typeof(string));
			dtgrddetail.Columns.Add("ItemName", typeof(string));
			dtgrddetail.Columns.Add("ItemUOM", typeof(string));
			dtgrddetail.Columns.Add("ItemUOMId", typeof(int));
			dtgrddetail.Columns.Add("ItemConditionId", typeof(int));
			dtgrddetail.Columns.Add("ItemCondition", typeof(string));
			dtgrddetail.Columns.Add("RequiredQty", typeof(double));
			dtgrddetail.Columns.Add("ApprovedQty", typeof(double));
			dtgrddetail.Columns.Add("ApprovalStatus", typeof(bool));
			dtgrddetail.Columns.Add("JobLot", typeof(string));
			dtgrddetail.Columns.Add("JobLotId", typeof(int));
			dtgrddetail.Columns.Add("AssetsRef", typeof(string));
			dtgrddetail.Columns.Add("AssetsRefId", typeof(int));
			dtgrddetail.Columns.Add("Remarks", typeof(string));
			dtgrddetail.Columns.Add("LastDate", typeof(string));
			dtgrddetail.Columns.Add("LastQty", typeof(double));
			dtgrddetail.Columns.Add("LastRate", typeof(double));
			dtgrddetail.Columns.Add("OutstandingDemandAndPOQty", typeof(double));
			dtgrddetail.Columns.Add("AvaialableStockQty", typeof(double));
			DepartmentFill();
			ParentCategoryBind();
			ItemDtsFillFromGlobal(Conversion.ToInt(CmbParentCategory.Value));
			ItemNameBind();
			ItemConditionBindFromGlobal();
			combojoblotfill();
			FixedAssest();
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			GenerateDocNumber();
			txtdocnumber.Focus();
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
			FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDateHistory.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GenerateDocNumber()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			int code = InvPurchaseDemandHeader.GenerateCode(new InvPurchaseDemandHeader
			{
				CompanyId = UserAccount.CompanyId,
				OrganizationId = UserAccount.OrganizationId,
				BranchesId = UserAccount.BranchesId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				DocumentTypeId = 141
			});
			if (code > 0)
			{
				txtdocnumber.Text = code.ToString();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FixedAssest()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int ASId = Conversion.ToInt(cmbAssetsRef.Value);
			DataTable dtfixedAsset = FixedAssetsRegister.GetAll(new FixedAssetsRegister
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtfixedAsset.Rows.Count > 0)
			{
				DDL.BindDDL(dtfixedAsset, cmbAssetsRef, "Id", "AssetName", "Fixed Asset Name", ZeroIndex: true);
				((UltraGridBase)cmbAssetsRef).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (ASId > 0)
				{
					if (dtfixedAsset.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ASId))
					{
						cmbAssetsRef.Value = ASId;
					}
					else
					{
						((Control)(object)cmbAssetsRef).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbAssetsRef).Text = string.Empty;
				((UltraGridBase)cmbAssetsRef).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DepartmentFill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		try
		{
			int DPId = Conversion.ToInt(cmbDepartment.Value);
			DataTable dtDepartment = Department.GetAll(new Department
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
			if (dtDepartment.Rows.Count > 0)
			{
				DDL.BindDDL(dtDepartment, cmbDepartment, "Id", "DepartmentName", "Department", ZeroIndex: true);
				((UltraGridBase)cmbDepartment).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (DPId > 0)
				{
					if (dtDepartment.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DPId))
					{
						cmbDepartment.Value = DPId;
					}
					else
					{
						((Control)(object)cmbDepartment).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbDepartment).Text = string.Empty;
				((UltraGridBase)cmbDepartment).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParentCategoryBind()
	{
		try
		{
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems != null && allItems.Count != 0)
			{
				getGlobalAllItems parent = allItems.FirstOrDefault((getGlobalAllItems x) => x.InventoryParentCategoriesId == ParentCategoryId);
				if (parent != null)
				{
					DataTable dt = new DataTable();
					dt.Columns.Add("Id", typeof(int));
					dt.Columns.Add("Description", typeof(string));
					dt.Rows.Add(parent.InventoryParentCategoriesId, parent.InvParentCateDescription);
					int selectedId = Conversion.ToInt(CmbParentCategory.Value);
					InfragisticsHelper.BindAndRetainSelection(CmbParentCategory, dt, "Id", "Description", "Parent Category", AllColumns: true, selectedId);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal(int parentCategoryId)
	{
		try
		{
			dtItem.Clear();
			if (parentCategoryId == 0)
			{
				parentCategoryId = 8;
			}
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || allItems.Count == 0)
			{
				return;
			}
			foreach (var item in (from x in allItems
				where x.InventoryParentCategoriesId == parentCategoryId
				select new { x.Id, x.ItemName, x.ItemCode }).ToList())
			{
				dtItem.Rows.Add(item.Id, item.ItemName, item.ItemCode);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			string ValueMember = (rdSearchByName.Checked ? "ItemName" : "ItemCode");
			string DisplayMember = (rdSearchByName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtItem, "Id", ValueMember, DisplayMember, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtItem;
			if (dataTable != null && dataTable.Rows.Count > 0 && ((UltraGridBase)cmbItem).DataSource != null)
			{
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemConditionBindFromGlobal()
	{
		try
		{
			List<GlobalItemCondition> list = clsGlobalVariables.globalItemConditions.Where((GlobalItemCondition r) => r.Id != 4).ToList();
			int Id = Conversion.ToInt(CmbItemCondition.Value);
			DataTable dt = DatatableHelper.PopulateDataTableAndReturn(list, (GlobalItemCondition x) => x.Id, (GlobalItemCondition x) => x.ConditionStatus);
			InfragisticsHelper.BindAndRetainSelection(CmbItemCondition, dt, "Id", "Description", "Item Condition", AllColumns: true, Id, insertDefaultRow: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemUomFill()
	{
		try
		{
			string Packuom = ((Control)(object)cmbUOM).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbUOM, dtUOM, "Id", "UOMCode", "UOM", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void combojoblotfill()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			int joId = Conversion.ToInt(cmbLot.Value);
			DataTable dtJob = JobLotsAllocationToBranch.GetJobLotsAllocatedToBranchByBranchId(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId
			});
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtJob, cmbLot, "Id", "JobLotDescription", "JobLot Description", true);
				((UltraGridBase)cmbLot).DisplayLayout.Bands[0].Columns[1].Width = 150;
				if (joId > 0)
				{
					if (dtJob.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == joId))
					{
						cmbLot.Value = joId;
					}
					else
					{
						((Control)(object)cmbLot).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)cmbLot).Text = string.Empty;
				((UltraGridBase)cmbLot).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbParentCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDtsFillFromGlobal(Conversion.ToInt(CmbParentCategory.Value));
			ItemNameBind();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemUomFill();
			UpdateLastPurchaseInfo();
			UpdateOutstandingDemandAndStockQty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItemCondition_Leave(object sender, EventArgs e)
	{
		try
		{
			UpdateOutstandingDemandAndStockQty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateLastPurchaseInfo()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		try
		{
			DataTable dt = InvPurchaseDemandHeader.GetlastPurchaseQtyRateandDate(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 64,
				ItemId = Conversion.ToInt(cmbItem.Value)
			});
			if (dt.Rows.Count > 0 && dt != null)
			{
				txtLastDate.Text = Conversion.ToDateTime(dt.Rows[0]["DocDate"]).ToString("dd-MMM-yyyy");
				txtLastQty.Text = Conversion.ToString(dt.Rows[0]["ItemQty"]);
				txtLastRate.Text = Conversion.ToString(dt.Rows[0]["ItemRate"]);
			}
			else
			{
				txtLastDate.Text = "None";
				txtLastQty.Text = "0";
				txtLastRate.Text = "0";
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void UpdateOutstandingDemandAndStockQty()
	{
		try
		{
			double OutstandingDemandAndPOQty = 0.0;
			double AvaialableStockQty = 0.0;
			int ItemId = Conversion.ToInt(cmbItem.Value);
			DataTable dt = InvPurchaseDemandHeader.GetOutstandingDemandAndStockQty(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId, txtDocdate.Value, Conversion.ToInt(CmbItemCondition.Value), RECID);
			if (dt != null)
			{
				OutstandingDemandAndPOQty = Conversion.ToDouble(dt.Rows[0]["OutstandingDemandAndPOQty"]);
				AvaialableStockQty = Conversion.ToDouble(dt.Rows[0]["AvaialableStockQty"]);
			}
			txtOutstandingDemandAndPOQty.Text = OutstandingDemandAndPOQty.ToString("#,##.##");
			txtAvaialableStockQty.Text = AvaialableStockQty.ToString("#,##.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void rdSearchByName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			ItemNameBind();
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
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f1: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			InvPurchaseDemandHeader obj = new InvPurchaseDemandHeader();
			if (RECID > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
				{
					return;
				}
				obj.Id = RECID;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				return;
			}
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 141;
			obj.EntryUser = UserAccount.ID;
			obj.ModifyUser = UserAccount.ID;
			obj.EntryDate = DateTime.Now;
			obj.ModifyDate = DateTime.Now;
			obj.ApprovedDate = DateTime.Now;
			obj.ApprovedUserId = 0;
			obj.IsApproved = false;
			obj.DocNo = Conversion.ToInt((object)txtdocnumber.Text);
			obj.DocDate = txtDocdate.Value;
			obj.ParentCategoryId = ParentCategoryId;
			obj.Status = "Open";
			obj.RemarksHeader = txtRemarks.Text.ToString();
			obj.DetailIdsToDelete = ((DetailIdsToDelete != null && DetailIdsToDelete.Any()) ? string.Join(",", DetailIdsToDelete) : string.Empty);
			if (string.IsNullOrEmpty(obj.DetailIdsToDelete) || MessageBox.Show("You have deleted some rows from the detail data.\nAre you sure you want to proceed?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.No)
			{
				obj.invPurchaseDemandDetails = new List<InvPurchaseDemandDetail>();
				if (grdPurchaseDemand.GetRows().Count() == 0)
				{
					throw new Exception("Grid Record not found");
				}
				GridEXRow[] rows = grdPurchaseDemand.GetRows();
				foreach (GridEXRow r in rows)
				{
					InvPurchaseDemandDetail vd = new InvPurchaseDemandDetail();
					vd.Id = ((RECID != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
					FillDetailListCommonForInsertAndDelete(vd, r);
					FormHelper.ValidateField(vd.DepartmentId, "Department", r.RowIndex);
					FormHelper.ValidateField(vd.RequestBy, "Requistion By", r.RowIndex);
					FormHelper.ValidateField(vd.ItemId, "Item", r.RowIndex);
					FormHelper.ValidateField(vd.ItemSchuomId, "Uom", r.RowIndex);
					FormHelper.ValidateField(vd.RequiredQty, "Required Qty", r.RowIndex);
					FormHelper.ValidateField(vd.JobLotId, "JobLot", r.RowIndex);
					FormHelper.ValidateField(vd.FixedAssetsRegisterId, "Asset", r.RowIndex);
					obj.invPurchaseDemandDetails.Add(vd);
				}
				obj.AttachmentsList = new List<DMSAttachments>();
				obj.DeleteAttachmentsList = new List<DMSAttachments>();
				FormHelper.UpdateAttachmentsForObject<InvPurchaseDemandHeader>(obj, ScreenName, AT, "AttachmentsList", "DeleteAttachmentsList");
				int success = InvPurchaseDemandHeader.Save(obj);
				if (RECID == 0)
				{
					MessageBox.Show("Record Save Successfully");
				}
				else
				{
					MessageBox.Show("Record Update Successfully");
					FormHelper.DeleteAllAttachmentsFromDb(AT, RECID, 141, ScreenName);
				}
				reset();
				resetDetail();
				if (chkPrintPreview.Checked)
				{
					PurchaseDemandSlip454(success);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailListCommonForInsertAndDelete(InvPurchaseDemandDetail vd, GridEXRow r)
	{
		vd.DepartmentId = Conversion.ToInt(r.Cells["DepartmentId"].Value);
		vd.DepartmentName = Conversion.ToString(r.Cells["Department"].Value);
		vd.RequestBy = Conversion.ToString((object)r.Cells["RequistionBy"].Text);
		vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
		vd.ItemCode = Conversion.ToString(r.Cells["ItemCode"].Value);
		vd.ItemName = Conversion.ToString(r.Cells["ItemName"].Value);
		vd.ItemSchuomId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
		vd.UOMDescription = Conversion.ToString(r.Cells["ItemUOM"].Value);
		vd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
		vd.ItemCondition = Conversion.ToString(r.Cells["ItemCondition"].Value);
		vd.RequiredQty = Conversion.ToDouble(r.Cells["RequiredQty"].Value);
		vd.ApprovedQty = Conversion.ToDouble(r.Cells["ApprovedQty"].Value);
		vd.IsApproved = Conversion.ToBool(r.Cells["ApprovalStatus"].Value);
		vd.ApprovedUserId = UserAccount.ID;
		vd.ApprovedDate = DateTime.Now;
		vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
		vd.JobLotDescription = Conversion.ToString(r.Cells["JobLot"].Value);
		vd.FixedAssetsRegisterId = Conversion.ToInt(r.Cells["AssetsRefId"].Value);
		vd.AssetName = Conversion.ToString(r.Cells["AssetsRef"].Value);
		vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
		vd.LastPurchaseDate = Conversion.ToDateTime((object)r.Cells["LastDate"].Text);
		vd.lastPurchaseQty = Conversion.ToDouble((object)r.Cells["LastQty"].Text);
		vd.LastPurchaseRate = Conversion.ToDouble((object)r.Cells["LastRate"].Text);
		vd.OutstandingDemandAndPOQty = Conversion.ToDouble((object)r.Cells["OutstandingDemandAndPOQty"].Text);
		vd.AvaialableStockQty = Conversion.ToDouble((object)r.Cells["AvaialableStockQty"].Text);
	}

	private void btnSave_Click(object sender, EventArgs e)
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
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		try
		{
			RECID = ID;
			InvPurchaseDemandHeader SC = new InvPurchaseDemandHeader();
			SC = InvPurchaseDemandHeader.GetByID(ID);
			if (SC != null)
			{
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				tabControl1.SelectedIndex = 0;
				txtdocnumber.Text = Conversion.ToString((object)SC.DocNo);
				txtDocdate.Value = Conversion.ToDateTime((object)SC.DocDate);
				txtRemarks.Text = Conversion.ToString((object)SC.RemarksHeader);
				dtgrddetail.Rows.Clear();
				FillDetailFromListCommonForReadById(SC, dtgrddetail);
				grdPurchaseDemand.DataSource = dtgrddetail;
				grdPurchaseDemand.RetrieveStructure();
				gridsettings(grdPurchaseDemand);
				FormHelper.LoadAttachmentsForObject(AT, RECID, ScreenName);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void FillDetailFromListCommonForReadById(InvPurchaseDemandHeader invGrn, DataTable dt, bool AddExtraColumns = false)
	{
		if (invGrn == null || invGrn.invPurchaseDemandDetails == null || invGrn.invPurchaseDemandDetails.Count == 0)
		{
			return;
		}
		foreach (InvPurchaseDemandDetail detail in invGrn.invPurchaseDemandDetails)
		{
			string LastDate = ((!Conversion.CheckDateTimeNull(detail.LastPurchaseDate)) ? detail.LastPurchaseDate.ToString("dd-MMM-yyyy") : "");
			List<object> rowValues = new List<object>
			{
				detail.Id, detail.DepartmentName, detail.DepartmentId, detail.RequestBy, detail.ItemId, detail.ItemCode, detail.ItemName, detail.UOMDescription, detail.ItemSchuomId, detail.ItemConditionId,
				detail.ItemCondition, detail.RequiredQty, detail.ApprovedQty, detail.IsApproved, detail.JobLotDescription, detail.JobLotId, detail.AssetName, detail.FixedAssetsRegisterId, detail.RemarksDetail, LastDate,
				detail.lastPurchaseQty, detail.LastPurchaseRate, detail.OutstandingDemandAndPOQty, detail.AvaialableStockQty
			};
			dt.Rows.Add(rowValues.ToArray());
		}
	}

	private void grdPurchaseDemand_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		try
		{
			GridEXRow item = grdPurchaseDemand.CurrentRow;
			if (item != null)
			{
				if (e.Column.Key == "Edit")
				{
					grdPurchaseDemand_DoubleClick(null, null);
				}
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(item);
				}
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
		try
		{
			if (r == null || (int)r.RowType != 0)
			{
				return;
			}
			if (Conversion.ToBool(r.Cells["ApprovalStatus"].Value))
			{
				throw new Exception("You cannot Update Approved Row");
			}
			if (updateDetailIndex == -1)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				if (Id > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
					{
						return;
					}
					DetailIdsToDelete.Add(Id);
					dtgrddetail.Rows[r.RowIndex].Delete();
					r.Delete();
				}
				else
				{
					r.Delete();
				}
				grdPurchaseDemand.UpdateData();
				return;
			}
			throw new Exception("Reset Detail First");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool IsDuplicateItem(int itemId, int skipRowIndex = -1)
	{
		for (int i = 0; i < dtgrddetail.Rows.Count; i++)
		{
			if (i != skipRowIndex && Convert.ToInt32(dtgrddetail.Rows[i]["ItemId"]) == itemId)
			{
				return true;
			}
		}
		return false;
	}

	private void FillDetailRow(DataRow row)
	{
		string LastDate = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)txtLastDate.Text))) ? Conversion.ToDateTime((object)txtLastDate.Text).ToString("dd-MMM-yyyy") : "");
		row["Department"] = Conversion.ToString((object)((Control)(object)cmbDepartment).Text);
		row["DepartmentId"] = Conversion.ToInt(cmbDepartment.Value);
		row["RequistionBy"] = Conversion.ToString((object)txtRequisition.Text);
		row["ItemId"] = Conversion.ToInt(cmbItem.Value);
		row["ItemCode"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value);
		row["ItemName"] = Conversion.ToString(((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Value);
		row["ItemUOM"] = Conversion.ToString((object)((Control)(object)cmbUOM).Text);
		row["ItemUOMId"] = Conversion.ToInt(cmbUOM.Value);
		row["ItemConditionId"] = Conversion.ToInt(CmbItemCondition.Value);
		row["ItemCondition"] = Conversion.ToString((object)((Control)(object)CmbItemCondition).Text);
		row["RequiredQty"] = Conversion.ToDouble((object)txtRequiredQty.Text);
		row["ApprovedQty"] = Conversion.ToDouble((object)txtRequiredQty.Text);
		row["JobLot"] = Conversion.ToString((object)((Control)(object)cmbLot).Text);
		row["JobLotId"] = Conversion.ToInt(cmbLot.Value);
		row["AssetsRef"] = Conversion.ToString((object)((Control)(object)cmbAssetsRef).Text);
		row["AssetsRefId"] = Conversion.ToInt(cmbAssetsRef.Value);
		row["Remarks"] = Conversion.ToString((object)txtRemarksDetail.Text);
		row["LastDate"] = LastDate;
		row["LastQty"] = Conversion.ToDouble((object)txtLastQty.Text);
		row["LastRate"] = Conversion.ToDouble((object)txtLastRate.Text);
		row["OutstandingDemandAndPOQty"] = Conversion.ToDouble((object)txtOutstandingDemandAndPOQty.Text);
		row["AvaialableStockQty"] = Conversion.ToDouble((object)txtAvaialableStockQty.Text);
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (FormValidationOfDetailPortion())
			{
				int selectedItemId = Convert.ToInt32(cmbItem.Value);
				if (IsDuplicateItem(selectedItemId))
				{
					MessageBox.Show("Duplicate Item Not Allowed in Grid");
					return;
				}
				DataRow newRow = dtgrddetail.NewRow();
				newRow["Id"] = 0;
				FillDetailRow(newRow);
				dtgrddetail.Rows.Add(newRow);
				grdPurchaseDemand.DataSource = dtgrddetail;
				grdPurchaseDemand.RetrieveStructure();
				gridsettings(grdPurchaseDemand);
				resetDetail();
			}
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
			if (FormValidationOfDetailPortion())
			{
				int selectedItemId = Convert.ToInt32(cmbItem.Value);
				if (IsDuplicateItem(selectedItemId, updateDetailIndex))
				{
					MessageBox.Show("Duplicate Item Not Allowed in Grid");
					return;
				}
				DataRow row = dtgrddetail.Rows[updateDetailIndex];
				FillDetailRow(row);
				ItemId = 0;
				resetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseDemand_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdPurchaseDemand.CurrentRow;
			if (item != null)
			{
				if (Conversion.ToBool(item.Cells["ApprovalStatus"].Value))
				{
					throw new Exception("You cannot Update Approved Row");
				}
				string LastDate = ((!Conversion.CheckDateTimeNull(Conversion.ToDateTime((object)item.Cells["LastDate"].Text))) ? Conversion.ToDateTime((object)item.Cells["LastDate"].Text).ToString("dd-MMM-yyyy") : "");
				updateDetailIndex = item.RowIndex;
				cmbDepartment.Value = item.Cells["DepartmentId"].Text.ToString();
				txtRequisition.Text = item.Cells["RequistionBy"].Text.ToString();
				cmbLot.Value = item.Cells["JobLotId"].Value.ToString();
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				ItemUomFill();
				cmbUOM.Value = item.Cells["ItemUOMId"].Text.ToString();
				txtRequiredQty.Text = item.Cells["RequiredQty"].Text.ToString();
				cmbAssetsRef.Value = item.Cells["AssetsRefId"].Text.ToString();
				txtRemarksDetail.Text = item.Cells["Remarks"].Text.ToString();
				txtLastDate.Text = LastDate;
				txtLastQty.Text = item.Cells["LastQty"].Text.ToString();
				txtLastRate.Text = item.Cells["LastRate"].Text.ToString();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelDetail.Visible = true;
				cmbDepartment.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridsettings(GridEX gridEX)
	{
		try
		{
			List<string> hiddenColumns = new List<string> { "Id", "DepartmentId", "ItemId", "ItemUOMId", "ItemConditionId", "JobLotId", "AssetsRefId" };
			Dictionary<string, EditType> editableColumns = null;
			if (((Control)(object)gridEX).Name == "grdPurchaseDemand")
			{
				editableColumns = new Dictionary<string, EditType> { 
				{
					"Remarks",
					(EditType)1
				} };
			}
			GridEX_Helper.GridWrappingAndColumnSettings(gridEX, 3, 2, hiddenColumns, editableColumns);
			gridEX.DefaultFilterRowComparison = (FilterConditionOperator)7;
			gridEX.RootTable.Columns["ItemUOM"].Caption = "UOM";
			gridEX.RootTable.Columns["LastDate"].Caption = "Last Purchase Date";
			gridEX.RootTable.Columns["LastQty"].Caption = "Last Purchase Qty";
			gridEX.RootTable.Columns["LastRate"].Caption = "Last Purchase Rate";
			if (((Control)(object)gridEX).Name == "grdPurchaseDemand")
			{
				if (formrights.DoHaveUpdateRights)
				{
					GridEX_Helper.AddButton(grdPurchaseDemand, "Delete", "X", 20, 0);
					GridEX_Helper.AddButton(grdPurchaseDemand, "Edit", "Edit", 40, 1);
				}
				grdPurchaseDemand.FrozenColumns = 3;
				ctrlGrdBar2_Load(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelDetail_Click(object sender, EventArgs e)
	{
		resetDetail();
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

	private void frmPurchaseDemand_KeyDown(object sender, KeyEventArgs e)
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
					txtdocnumber.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					((Control)(object)DatagridHistory).Focus();
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
				if (e.Control && e.KeyCode == Keys.S && !UpdateMode)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					txtDocdate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdPurchaseDemand).Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbDepartment.Focus();
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.N)
				{
					btnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DatagridHistory).Focus();
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
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc No");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item in Detail Box");
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

	private void DatagridHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DatagridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt(r.Cells["Id"].Value);
			int DocTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DatagridHistory.CurrentColumn.Key == "Edit")
				{
					RECID = Id;
					ReadById(RECID);
				}
				if (DatagridHistory.CurrentColumn.Key == "Slip")
				{
					PurchaseDemandSlip454(Id);
				}
				if (DatagridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocTypeId);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				RECID = Id;
				ReadById(RECID);
			}
			if (e.Control && e.KeyCode == Keys.P)
			{
				PurchaseDemandSlip454(Id);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdPurchaseDemand_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdPurchaseDemand.CurrentRow;
			if (item != null && (int)item.RowType == 0 && grdPurchaseDemand.CurrentColumn != null)
			{
				Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				if (e.Control && e.KeyCode == Keys.Space)
				{
					grdPurchaseDemand_DoubleClick(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdPurchaseDemand_DoubleClick(null, null);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (((UltraGridBase)CmbParentCategory).ActiveRow == null || Conversion.ToInt(CmbParentCategory.Value) == 0)
		{
			MessageBox.Show("Parent Category Field Required");
			CmbParentCategory.Focus();
			return false;
		}
		if (txtdocnumber.Text.Trim() == string.Empty || Conversion.ToInt((object)txtdocnumber.Text.Trim()) == 0)
		{
			MessageBox.Show("Doc No Field Required");
			txtdocnumber.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationOfDetailPortion()
	{
		if (((UltraGridBase)CmbParentCategory).ActiveRow == null || Conversion.ToInt(CmbParentCategory.Value) == 0)
		{
			MessageBox.Show("Parent Category Field Required");
			CmbParentCategory.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDepartment).ActiveRow == null || Conversion.ToInt(cmbDepartment.Value) == 0)
		{
			MessageBox.Show("Department Field Required");
			cmbDepartment.Focus();
			return false;
		}
		if (txtRequisition.Text.Trim() == string.Empty)
		{
			MessageBox.Show("Requisition By Field Required");
			txtRequisition.Focus();
			return false;
		}
		if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
		{
			MessageBox.Show(" Item Field Required");
			cmbItem.Focus();
			return false;
		}
		if (txtRequiredQty.Text == string.Empty)
		{
			MessageBox.Show("Required Qty Field Required");
			cmbUOM.Focus();
			return false;
		}
		if (((UltraGridBase)cmbLot).ActiveRow == null || Conversion.ToInt(cmbLot.Value) == 0)
		{
			MessageBox.Show(" JobLot Field Required");
			cmbLot.Focus();
			return false;
		}
		if (((UltraGridBase)cmbAssetsRef).ActiveRow == null || Conversion.ToInt(cmbAssetsRef.Value) == 0)
		{
			MessageBox.Show("Asset Field Required");
			cmbAssetsRef.Focus();
			return false;
		}
		return true;
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		try
		{
			resetDetail();
			reset();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			DetailIdsToDelete.Clear();
			RECID = 0;
			ItemId = 0;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			txtRemarks.Text = string.Empty;
			resetDetail();
			dtgrddetail.Clear();
			grdPurchaseDemand.DataSource = null;
			txtDocdate.Focus();
			GenerateDocNumber();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void resetDetail()
	{
		try
		{
			cmbUOM.Value = "";
			txtRequiredQty.Text = "";
			txtRemarksDetail.Text = string.Empty;
			((Control)(object)cmbLot).Text = string.Empty;
			((Control)(object)cmbAssetsRef).Text = string.Empty;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelDetail.Visible = false;
			updateDetailIndex = -1;
			cmbDepartment.Focus();
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
			List<string> allServices = new List<string> { "UomSchedule", "AllItems", "CropYear", "JobLot", "ItemConditions" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			DepartmentFill();
			ParentCategoryBind();
			ItemDtsFillFromGlobal(Conversion.ToInt(CmbParentCategory.Value));
			ItemNameBind();
			ItemConditionBindFromGlobal();
			combojoblotfill();
			FixedAssest();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Add_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl.Name == "rdSearchByName")
			{
				cmbDepartment.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmPurchaseDemand_FormClosing(object sender, FormClosingEventArgs e)
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

	private void txtDocdate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GridEXRow[] dataRows = grdPurchaseDemand.GetDataRows();
			foreach (GridEXRow obj in dataRows)
			{
				double OutstandingDemandAndPOQty = 0.0;
				double AvaialableStockQty = 0.0;
				int ItemId = Conversion.ToInt(obj.Cells["ItemId"].Value);
				int ItemConditionId = Conversion.ToInt(obj.Cells["ItemConditionId"].Value);
				DataTable dt = InvPurchaseDemandHeader.GetOutstandingDemandAndStockQty(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId, txtDocdate.Value, ItemConditionId, RECID);
				if (dt != null)
				{
					OutstandingDemandAndPOQty = Conversion.ToDouble(dt.Rows[0]["OutstandingDemandAndPOQty"]);
					AvaialableStockQty = Conversion.ToDouble(dt.Rows[0]["AvaialableStockQty"]);
				}
				obj.BeginEdit();
				obj.Cells["OutstandingDemandAndPOQty"].Value = OutstandingDemandAndPOQty;
				obj.Cells["AvaialableStockQty"].Value = AvaialableStockQty;
				obj.EndEdit();
			}
			grdDetail.UpdateData();
			UpdateOutstandingDemandAndStockQty();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnshow_Click(object sender, EventArgs e)
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

	private void btnshow_Leave(object sender, EventArgs e)
	{
		try
		{
			if (base.ActiveControl != null && base.ActiveControl.Name == "")
			{
				FromDateHistory.Focus();
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
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			DatagridHistory.ClearStructure();
			grdDetail.ClearStructure();
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
			obj.BranchesId = UserAccount.BranchesId;
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			if (drdocdate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.FromDate = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ToDate = ToDateHistory.Value;
				}
			}
			else if (rdentrydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.EntryDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.EntryDateTo = ToDateHistory.Value;
				}
			}
			else if (rdmodifydate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ModifyDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ModifyDateTo = ToDateHistory.Value;
				}
			}
			else if (rdapproveddate.Checked)
			{
				if (FromDateHistory.Checked)
				{
					obj.ApprovedDateFrom = FromDateHistory.Value;
				}
				if (ToDateHistory.Checked)
				{
					obj.ApprovedDateTo = ToDateHistory.Value;
				}
			}
			obj.ParentCategoryId = ParentCategoryId;
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			DataTable dt = InvPurchaseDemandHeader.FormHistory(obj);
			if (dt.Rows.Count > 0)
			{
				DataTable dtcol = new DataTable();
				dtcol.Columns.Add("Id", typeof(int));
				dtcol.Columns.Add("RecordNo", typeof(int));
				dtcol.Columns.Add("DocumentTypeId", typeof(int));
				dtcol.Columns.Add("DocDate", typeof(DateTime));
				dtcol.Columns.Add("DocNo", typeof(int));
				dtcol.Columns.Add("Status", typeof(string));
				dtcol.Columns.Add("RequiredQty", typeof(double));
				dtcol.Columns.Add("RequestBy", typeof(string));
				dtcol.Columns.Add("EntryUser", typeof(string));
				dtcol.Columns.Add("EntryDate", typeof(DateTime));
				dtcol.Columns.Add("ModifyUser", typeof(string));
				dtcol.Columns.Add("ModifyDate", typeof(DateTime));
				dtcol.Columns.Add("NoOfAttachments", typeof(int));
				dtcol.Columns.Add("RemarksHeader", typeof(string));
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					int id = Conversion.ToInt(dt.Rows[i]["Id"]);
					if (!dtcol.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == id))
					{
						double sumRequiredQty = (from row in dt.AsEnumerable()
							where row.Field<int>("Id") == id
							select row).Sum((DataRow row) => row.Field<double>("RequiredQty"));
						string concateRequestedBy = string.Join(", ", (from row in dt.AsEnumerable()
							where row.Field<int>("Id") == id
							select row.Field<string>("RequestBy")).Distinct());
						dtcol.Rows.Add(dt.Rows[i]["Id"], dt.Rows[i]["RecordNo"], dt.Rows[i]["DocumentTypeId"], dt.Rows[i]["DocDate"], dt.Rows[i]["DocNo"], dt.Rows[i]["Status"], sumRequiredQty, concateRequestedBy, dt.Rows[i]["EntryUserName"], dt.Rows[i]["EntryDate"], dt.Rows[i]["ModifyUserName"], dt.Rows[i]["ModifyDate"], dt.Rows[i]["NoOfAttachments"], dt.Rows[i]["RemarksHeader"]);
					}
				}
				DatagridHistory.DataSource = dtcol;
				DatagridHistory.RetrieveStructure();
				DatagridHistorySetting();
			}
			else
			{
				DatagridHistory.DataSource = null;
				grdDetail.DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistorySetting()
	{
		try
		{
			DatagridHistory.RootTable.Columns["Id"].Visible = false;
			DatagridHistory.RootTable.Columns["RecordNo"].Visible = false;
			DatagridHistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DatagridHistory, 2, 3);
			DatagridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DatagridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DatagridHistory.RootTable.Columns["Status"].Width = Constants.InventoryConstants.StatusConstant;
			DatagridHistory.RootTable.Columns["RequiredQty"].Width = Constants.InventoryConstants.QtyConstant;
			DatagridHistory.RootTable.Columns["RequestBy"].Width = 250;
			DatagridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DatagridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DatagridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DatagridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DatagridHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants + 20;
			DatagridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			DatagridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DatagridHistory.RootTable.Columns.Add("Edit");
			DatagridHistory.RootTable.Columns["Edit"].Caption = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			DatagridHistory.RootTable.Columns["Edit"].Width = 40;
			DatagridHistory.RootTable.Columns["Edit"].Position = 0;
			DatagridHistory.RootTable.Columns.Add("Slip");
			DatagridHistory.RootTable.Columns["Slip"].Caption = "Slip";
			DatagridHistory.RootTable.Columns["Slip"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			DatagridHistory.RootTable.Columns["Slip"].ButtonStyle = (ButtonStyle)5;
			DatagridHistory.RootTable.Columns["Slip"].ButtonText = "Slip";
			DatagridHistory.RootTable.Columns["Slip"].Width = 40;
			DatagridHistory.RootTable.Columns["Slip"].Position = 1;
			DatagridHistory.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DatagridHistory.CurrentColumn != null)
			{
				int Id = Conversion.ToInt(r.Cells["Id"].Value);
				int DocumentTypeId = Conversion.ToInt(r.Cells["DocumentTypeId"].Value);
				if (e.Column.Key == "NoOfAttachments" && Conversion.ToInt(r.Cells["NoOfAttachments"].Value) > 0)
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, DocumentTypeId);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DatagridHistory_DoubleClick(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DatagridHistory.CurrentColumn != null)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				RECID = Id;
				ReadById(RECID);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && DatagridHistory.CurrentColumn != null)
			{
				int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				if (e.Column.Key == "Slip")
				{
					PurchaseDemandSlip454(Id);
				}
				if (e.Column.Key == "Edit")
				{
					RECID = Id;
					ReadById(RECID);
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DatagridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DatagridHistory.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				GetDetailGrdByHeadId(Conversion.ToInt(r.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetDetailGrdByHeadId(int Id)
	{
		try
		{
			InvPurchaseDemandHeader Obj = InvPurchaseDemandHeader.GetByID(Id);
			if (Obj != null && Obj.invPurchaseDemandDetails.Count == 0)
			{
				grdDetail.ClearStructure();
				return;
			}
			DataTable dtDetailHistory = dtgrddetail.Clone();
			FillDetailFromListCommonForReadById(Obj, dtDetailHistory, AddExtraColumns: true);
			grdDetail.DataSource = dtDetailHistory;
			grdDetail.RetrieveStructure();
			grddetailhistorySettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grddetailhistorySettings()
	{
		try
		{
			gridsettings(grdDetail);
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
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)grdPurchaseDemand).Name);
			if (fs != null)
			{
				grdPurchaseDemand.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void ctrlGrdBar1_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(ScreenName, ((Control)(object)DatagridHistory).Name);
			if (fs != null)
			{
				DatagridHistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void btnPrint_Click(object sender, EventArgs e)
	{
		try
		{
			PurchaseDemandSlip454(RECID);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchaseDemandSlip454(int PrintId)
	{
		try
		{
			CommonServices.PurchaseDemandSlip454(PrintId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void button2_Click(object sender, EventArgs e)
	{
		try
		{
			new Define_Department(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnAssetRefDefine_Click(object sender, EventArgs e)
	{
		try
		{
			new frmLookUpDefineAsset(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCity_Click(object sender, EventArgs e)
	{
		try
		{
			new DefineCity(UserAccount).Show();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBarcodeReader_KeyDown(object sender, KeyEventArgs e)
	{
		try
		{
			if (e.KeyCode == Keys.Return)
			{
				cmbItem.Value = GetItemIdByBarcode(txtBarcodeReader.Text.Trim());
				cmbItem_Leave(null, null);
				txtBarcodeReader.Text = string.Empty;
				cmbUOM.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private int GetItemIdByBarcode(string BarcodeNo)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		try
		{
			return Item.GetItemIdByBarcodeNo(new Item
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BarcodeNo = BarcodeNo
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void btnAttachment_Click(object sender, EventArgs e)
	{
		AT.Show();
	}

	private void txtRequiredQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtFromDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtToDocNoHistory_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtdocnumber_TextChanged(object sender, EventArgs e)
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
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Expected O, but got Unknown
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Expected O, but got Unknown
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Expected O, but got Unknown
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Expected O, but got Unknown
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Expected O, but got Unknown
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Expected O, but got Unknown
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Expected O, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Expected O, but got Unknown
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Expected O, but got Unknown
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Expected O, but got Unknown
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Expected O, but got Unknown
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Expected O, but got Unknown
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c3: Expected O, but got Unknown
		//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Expected O, but got Unknown
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Expected O, but got Unknown
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Expected O, but got Unknown
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Expected O, but got Unknown
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Expected O, but got Unknown
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Expected O, but got Unknown
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Expected O, but got Unknown
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Expected O, but got Unknown
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Expected O, but got Unknown
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Expected O, but got Unknown
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021e: Expected O, but got Unknown
		//IL_021e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0225: Expected O, but got Unknown
		//IL_0225: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Expected O, but got Unknown
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Expected O, but got Unknown
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Expected O, but got Unknown
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Expected O, but got Unknown
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Expected O, but got Unknown
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Expected O, but got Unknown
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Expected O, but got Unknown
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Expected O, but got Unknown
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e7: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0355: Expected O, but got Unknown
		//IL_0411: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Expected O, but got Unknown
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_0431: Expected O, but got Unknown
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Expected O, but got Unknown
		//IL_045e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0489: Expected O, but got Unknown
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_0592: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Expected O, but got Unknown
		//IL_0b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b85: Expected O, but got Unknown
		//IL_4e56: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e60: Expected O, but got Unknown
		//IL_4e84: Unknown result type (might be due to invalid IL or missing references)
		//IL_4e8e: Expected O, but got Unknown
		GridEXLayout grdPurchaseDemand_DesignTimeLayout = new GridEXLayout();
		Appearance appearance1 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance74 = new Appearance();
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
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		GridEXLayout DatagridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Architecture.WinApp.StoreManagement.frmPurchaseDemand));
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel5 = new System.Windows.Forms.Panel();
		this.grdPurchaseDemand = new GridEX();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.label15 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbItemCondition = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.Add = new System.Windows.Forms.Button();
		this.label11 = new System.Windows.Forms.Label();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.rdSearchByName = new System.Windows.Forms.RadioButton();
		this.label4 = new System.Windows.Forms.Label();
		this.rdSearchByCode = new System.Windows.Forms.RadioButton();
		this.label5 = new System.Windows.Forms.Label();
		this.txtRequisition = new System.Windows.Forms.TextBox();
		this.cmbDepartment = new UltraCombo();
		this.button2 = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.txtBarcodeReader = new System.Windows.Forms.TextBox();
		this.label10 = new System.Windows.Forms.Label();
		this.btnAssetRefDefine = new System.Windows.Forms.Button();
		this.label18 = new System.Windows.Forms.Label();
		this.btnCancelDetail = new System.Windows.Forms.Button();
		this.txtRequiredQty = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.label9 = new System.Windows.Forms.Label();
		this.label44 = new System.Windows.Forms.Label();
		this.txtAvaialableStockQty = new System.Windows.Forms.TextBox();
		this.txtOutstandingDemandAndPOQty = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.txtLastRate = new System.Windows.Forms.TextBox();
		this.label16 = new System.Windows.Forms.Label();
		this.cmbItem = new UltraCombo();
		this.label14 = new System.Windows.Forms.Label();
		this.cmbLot = new UltraCombo();
		this.txtLastDate = new System.Windows.Forms.TextBox();
		this.txtLastQty = new System.Windows.Forms.TextBox();
		this.cmbUOM = new UltraCombo();
		this.label13 = new System.Windows.Forms.Label();
		this.cmbAssetsRef = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbParentCategory = new UltraCombo();
		this.label85 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.label1 = new System.Windows.Forms.Label();
		this.txtdocnumber = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label6 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.labelFormCaption = new System.Windows.Forms.Label();
		this.chkPrintPreview = new System.Windows.Forms.CheckBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnPrint = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.btnCity = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.DatagridHistory = new GridEX();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label7 = new System.Windows.Forms.Label();
		this.grdDetail = new GridEX();
		this.panel12 = new System.Windows.Forms.Panel();
		this.label2 = new System.Windows.Forms.Label();
		this.panel10 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label57 = new System.Windows.Forms.Label();
		this.label58 = new System.Windows.Forms.Label();
		this.label59 = new System.Windows.Forms.Label();
		this.label60 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel8 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label43 = new System.Windows.Forms.Label();
		this.panel7 = new System.Windows.Forms.Panel();
		this.toolStrip2 = new System.Windows.Forms.ToolStrip();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.btnRefreshHistory = new System.Windows.Forms.ToolStripButton();
		this.btnLoadAll = new System.Windows.Forms.ToolStripButton();
		this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseDemand).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsRef).BeginInit();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).BeginInit();
		this.panel3.SuspendLayout();
		this.panel2.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		this.tabPage2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).BeginInit();
		this.panel13.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel12.SuspendLayout();
		this.panel10.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel8.SuspendLayout();
		this.panel7.SuspendLayout();
		this.toolStrip2.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1087, 588);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1087, 588);
		this.tabControl1.TabIndex = 0;
		this.tabPage1.Controls.Add(this.panel5);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(1079, 559);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.grdPurchaseDemand);
		this.panel5.Controls.Add(this.panel11);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel5.Location = new System.Drawing.Point(3, 244);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(1073, 312);
		this.panel5.TabIndex = 3;
		this.grdPurchaseDemand.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdPurchaseDemand).BackColor = System.Drawing.Color.White;
		grdPurchaseDemand_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdPurchaseDemand.DesignTimeLayout = grdPurchaseDemand_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdPurchaseDemand.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).Location = new System.Drawing.Point(0, 24);
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).Name = "grdPurchaseDemand";
		((ScrollableBase)this.grdPurchaseDemand).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).Size = new System.Drawing.Size(1073, 288);
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).TabIndex = 0;
		this.grdPurchaseDemand.TotalRow = (InheritableBoolean)1;
		this.grdPurchaseDemand.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdPurchaseDemand).VisualStyle = (VisualStyle)4;
		this.grdPurchaseDemand.ColumnButtonClick += new ColumnActionEventHandler(grdPurchaseDemand_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).DoubleClick += new System.EventHandler(grdPurchaseDemand_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdPurchaseDemand).KeyDown += new System.Windows.Forms.KeyEventHandler(grdPurchaseDemand_KeyDown);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar2);
		this.panel11.Controls.Add(this.label15);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(0, 0);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1073, 24);
		this.panel11.TabIndex = 2;
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1026, 0);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.grdPurchaseDemand;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(45, 22);
		this.ctrlGrdBar2.TabIndex = 63;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Teal;
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(5, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(58, 19);
		this.label15.TabIndex = 4;
		this.label15.Text = "Detail";
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.panel4.Location = new System.Drawing.Point(3, 56);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1073, 188);
		this.panel4.TabIndex = 0;
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add(this.Add);
		this.groupBox2.Controls.Add(this.label11);
		this.groupBox2.Controls.Add(this.txtRemarksDetail);
		this.groupBox2.Controls.Add(this.rdSearchByName);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.rdSearchByCode);
		this.groupBox2.Controls.Add(this.label5);
		this.groupBox2.Controls.Add(this.txtRequisition);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartment);
		this.groupBox2.Controls.Add(this.button2);
		this.groupBox2.Controls.Add(this.label3);
		this.groupBox2.Controls.Add(this.label8);
		this.groupBox2.Controls.Add(this.txtBarcodeReader);
		this.groupBox2.Controls.Add(this.label10);
		this.groupBox2.Controls.Add(this.btnAssetRefDefine);
		this.groupBox2.Controls.Add(this.label18);
		this.groupBox2.Controls.Add(this.btnCancelDetail);
		this.groupBox2.Controls.Add(this.txtRequiredQty);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add(this.label9);
		this.groupBox2.Controls.Add(this.label44);
		this.groupBox2.Controls.Add(this.txtAvaialableStockQty);
		this.groupBox2.Controls.Add(this.txtOutstandingDemandAndPOQty);
		this.groupBox2.Controls.Add(this.label17);
		this.groupBox2.Controls.Add(this.txtLastRate);
		this.groupBox2.Controls.Add(this.label16);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Controls.Add(this.label14);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbLot);
		this.groupBox2.Controls.Add(this.txtLastDate);
		this.groupBox2.Controls.Add(this.txtLastQty);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbUOM);
		this.groupBox2.Controls.Add(this.label13);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbAssetsRef);
		this.groupBox2.Controls.Add(this.label12);
		this.groupBox2.Font = new System.Drawing.Font("Verdana", 8f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(3, 74);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Size = new System.Drawing.Size(1065, 112);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Information";
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance12).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance12).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance12;
		((AppearanceBase)appearance23).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance23;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance43).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance58;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance63).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance3).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(761, 29);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(761, 12);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(59, 15);
		this.label25.TabIndex = 121;
		this.label25.Text = "Condition";
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(993, 82);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(41, 24);
		this.Add.TabIndex = 16;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.Add.Leave += new System.EventHandler(Add_Leave);
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(6, 61);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(60, 15);
		this.label11.TabIndex = 91;
		this.label11.Text = "Assets Ref";
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(165, 82);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(167, 23);
		this.txtRemarksDetail.TabIndex = 10;
		this.rdSearchByName.AutoSize = true;
		this.rdSearchByName.Checked = true;
		this.rdSearchByName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByName.Location = new System.Drawing.Point(441, 10);
		this.rdSearchByName.Name = "rdSearchByName";
		this.rdSearchByName.Size = new System.Drawing.Size(57, 19);
		this.rdSearchByName.TabIndex = 119;
		this.rdSearchByName.TabStop = true;
		this.rdSearchByName.Text = "Name";
		this.rdSearchByName.UseVisualStyleBackColor = true;
		this.rdSearchByName.CheckedChanged += new System.EventHandler(rdSearchByName_CheckedChanged);
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(168, 62);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(86, 15);
		this.label4.TabIndex = 67;
		this.label4.Text = "Remarks Detail";
		this.rdSearchByCode.AutoSize = true;
		this.rdSearchByCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdSearchByCode.Location = new System.Drawing.Point(498, 10);
		this.rdSearchByCode.Name = "rdSearchByCode";
		this.rdSearchByCode.Size = new System.Drawing.Size(52, 19);
		this.rdSearchByCode.TabIndex = 118;
		this.rdSearchByCode.Text = "Code";
		this.rdSearchByCode.UseVisualStyleBackColor = true;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(4, 12);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(71, 15);
		this.label5.TabIndex = 76;
		this.label5.Text = "Department";
		this.txtRequisition.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRequisition.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRequisition.Location = new System.Drawing.Point(192, 31);
		this.txtRequisition.Name = "txtRequisition";
		this.txtRequisition.Size = new System.Drawing.Size(140, 23);
		this.txtRequisition.TabIndex = 2;
		this.cmbDepartment.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartment.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartment.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartment.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance5).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Appearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance6).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance6).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance6).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance7;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance8;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance9).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance9).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance9;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance10).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance11;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance13).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance15;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance16).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance17;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartment).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbDepartment.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Location = new System.Drawing.Point(4, 29);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Name = "cmbDepartment";
		((System.Windows.Forms.Control)(object)this.cmbDepartment).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartment).TabIndex = 0;
		this.button2.BackColor = System.Drawing.Color.White;
		this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.button2.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.button2.Location = new System.Drawing.Point(165, 29);
		this.button2.Margin = new System.Windows.Forms.Padding(2);
		this.button2.Name = "button2";
		this.button2.Size = new System.Drawing.Size(26, 26);
		this.button2.TabIndex = 1;
		this.button2.UseVisualStyleBackColor = false;
		this.button2.Click += new System.EventHandler(button2_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(333, 12);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(73, 15);
		this.label3.TabIndex = 116;
		this.label3.Text = "Bar Code No";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.Location = new System.Drawing.Point(192, 12);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(82, 15);
		this.label8.TabIndex = 78;
		this.label8.Text = "Requisition By";
		this.txtBarcodeReader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBarcodeReader.Location = new System.Drawing.Point(333, 31);
		this.txtBarcodeReader.Name = "txtBarcodeReader";
		this.txtBarcodeReader.Size = new System.Drawing.Size(73, 23);
		this.txtBarcodeReader.TabIndex = 3;
		this.txtBarcodeReader.KeyDown += new System.Windows.Forms.KeyEventHandler(txtBarcodeReader_KeyDown);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label10.Location = new System.Drawing.Point(911, 12);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(43, 15);
		this.label10.TabIndex = 88;
		this.label10.Text = "JobLot";
		this.btnAssetRefDefine.BackColor = System.Drawing.Color.White;
		this.btnAssetRefDefine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAssetRefDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.btnAssetRefDefine.Location = new System.Drawing.Point(142, 57);
		this.btnAssetRefDefine.Margin = new System.Windows.Forms.Padding(2);
		this.btnAssetRefDefine.Name = "btnAssetRefDefine";
		this.btnAssetRefDefine.Size = new System.Drawing.Size(23, 22);
		this.btnAssetRefDefine.TabIndex = 9;
		this.btnAssetRefDefine.TabStop = false;
		this.btnAssetRefDefine.UseVisualStyleBackColor = false;
		this.btnAssetRefDefine.Click += new System.EventHandler(btnAssetRefDefine_Click);
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label18.Location = new System.Drawing.Point(682, 12);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(76, 15);
		this.label18.TabIndex = 89;
		this.label18.Text = "Required Qty";
		this.btnCancelDetail.BackColor = System.Drawing.Color.Teal;
		this.btnCancelDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelDetail.ForeColor = System.Drawing.Color.White;
		this.btnCancelDetail.Location = new System.Drawing.Point(993, 59);
		this.btnCancelDetail.Name = "btnCancelDetail";
		this.btnCancelDetail.Size = new System.Drawing.Size(70, 24);
		this.btnCancelDetail.TabIndex = 18;
		this.btnCancelDetail.Text = "Cancel";
		this.btnCancelDetail.UseVisualStyleBackColor = false;
		this.btnCancelDetail.Visible = false;
		this.btnCancelDetail.Click += new System.EventHandler(btnCancelDetail_Click);
		this.btnCancelDetail.Leave += new System.EventHandler(Add_Leave);
		this.txtRequiredQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRequiredQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRequiredQty.Location = new System.Drawing.Point(682, 31);
		this.txtRequiredQty.Name = "txtRequiredQty";
		this.txtRequiredQty.Size = new System.Drawing.Size(77, 23);
		this.txtRequiredQty.TabIndex = 6;
		this.txtRequiredQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRequiredQty_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(993, 82);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(70, 24);
		this.btnUpdateDetail.TabIndex = 17;
		this.btnUpdateDetail.Text = "Update ";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label9.Location = new System.Drawing.Point(409, 12);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(32, 15);
		this.label9.TabIndex = 85;
		this.label9.Text = "Item";
		this.label44.AutoSize = true;
		this.label44.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label44.Location = new System.Drawing.Point(600, 12);
		this.label44.Name = "label44";
		this.label44.Size = new System.Drawing.Size(35, 15);
		this.label44.TabIndex = 84;
		this.label44.Text = "UOM";
		this.txtAvaialableStockQty.BackColor = System.Drawing.SystemColors.Control;
		this.txtAvaialableStockQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvaialableStockQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvaialableStockQty.Location = new System.Drawing.Point(868, 82);
		this.txtAvaialableStockQty.Name = "txtAvaialableStockQty";
		this.txtAvaialableStockQty.ReadOnly = true;
		this.txtAvaialableStockQty.Size = new System.Drawing.Size(124, 23);
		this.txtAvaialableStockQty.TabIndex = 15;
		this.txtAvaialableStockQty.TabStop = false;
		this.txtOutstandingDemandAndPOQty.BackColor = System.Drawing.SystemColors.Control;
		this.txtOutstandingDemandAndPOQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtOutstandingDemandAndPOQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtOutstandingDemandAndPOQty.Location = new System.Drawing.Point(678, 82);
		this.txtOutstandingDemandAndPOQty.Name = "txtOutstandingDemandAndPOQty";
		this.txtOutstandingDemandAndPOQty.ReadOnly = true;
		this.txtOutstandingDemandAndPOQty.Size = new System.Drawing.Size(188, 23);
		this.txtOutstandingDemandAndPOQty.TabIndex = 14;
		this.txtOutstandingDemandAndPOQty.TabStop = false;
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(868, 62);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(116, 15);
		this.label17.TabIndex = 97;
		this.label17.Text = "Avaialable Stock Qty";
		this.txtLastRate.BackColor = System.Drawing.SystemColors.Control;
		this.txtLastRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLastRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastRate.Location = new System.Drawing.Point(569, 82);
		this.txtLastRate.Name = "txtLastRate";
		this.txtLastRate.ReadOnly = true;
		this.txtLastRate.Size = new System.Drawing.Size(107, 23);
		this.txtLastRate.TabIndex = 13;
		this.txtLastRate.TabStop = false;
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label16.Location = new System.Drawing.Point(678, 62);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(188, 15);
		this.label16.TabIndex = 97;
		this.label16.Text = "Outstanding Demand And PO Qty";
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance45).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance46).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance47;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(408, 29);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(190, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 4;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label14.Location = new System.Drawing.Point(569, 62);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(105, 15);
		this.label14.TabIndex = 97;
		this.label14.Text = "Last Purchase Rate";
		this.cmbLot.AllowNull = (DefaultableBoolean)1;
		this.cmbLot.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbLot.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbLot.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance18).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbLot).DisplayLayout.Appearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.cmbLot).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance19).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance19).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance19).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance20;
		((SpecialBoxBase)((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbLot).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbLot).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance22).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance25;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance26).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance26;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance27).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance27).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance27).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance27).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance27).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance27;
		((AppearanceBase)appearance28).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance29;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbLot).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbLot).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbLot).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbLot).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbLot.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbLot).Location = new System.Drawing.Point(911, 29);
		((System.Windows.Forms.Control)(object)this.cmbLot).Name = "cmbLot";
		((System.Windows.Forms.Control)(object)this.cmbLot).Size = new System.Drawing.Size(150, 26);
		((System.Windows.Forms.Control)(object)this.cmbLot).TabIndex = 8;
		this.txtLastDate.BackColor = System.Drawing.SystemColors.Control;
		this.txtLastDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLastDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastDate.Location = new System.Drawing.Point(333, 82);
		this.txtLastDate.Name = "txtLastDate";
		this.txtLastDate.ReadOnly = true;
		this.txtLastDate.Size = new System.Drawing.Size(131, 23);
		this.txtLastDate.TabIndex = 11;
		this.txtLastDate.TabStop = false;
		this.txtLastQty.BackColor = System.Drawing.SystemColors.Control;
		this.txtLastQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtLastQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtLastQty.Location = new System.Drawing.Point(466, 82);
		this.txtLastQty.Name = "txtLastQty";
		this.txtLastQty.ReadOnly = true;
		this.txtLastQty.Size = new System.Drawing.Size(101, 23);
		this.txtLastQty.TabIndex = 12;
		this.txtLastQty.TabStop = false;
		this.cmbUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance62).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.cmbUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance64).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance64).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance64;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance65;
		((SpecialBoxBase)((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance66).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance66).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance67).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance68).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbUOM).Location = new System.Drawing.Point(600, 29);
		((System.Windows.Forms.Control)(object)this.cmbUOM).Name = "cmbUOM";
		((System.Windows.Forms.Control)(object)this.cmbUOM).Size = new System.Drawing.Size(80, 26);
		((System.Windows.Forms.Control)(object)this.cmbUOM).TabIndex = 5;
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label13.Location = new System.Drawing.Point(466, 62);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(101, 15);
		this.label13.TabIndex = 95;
		this.label13.Text = "Last Purchase Qty";
		this.cmbAssetsRef.AllowNull = (DefaultableBoolean)1;
		this.cmbAssetsRef.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbAssetsRef.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbAssetsRef.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Appearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance60).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance60).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance60;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance61;
		((SpecialBoxBase)((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance69;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance70;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance72;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance73).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbAssetsRef).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbAssetsRef.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Location = new System.Drawing.Point(4, 79);
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Name = "cmbAssetsRef";
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).Size = new System.Drawing.Size(160, 26);
		((System.Windows.Forms.Control)(object)this.cmbAssetsRef).TabIndex = 9;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label12.Location = new System.Drawing.Point(334, 62);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(107, 15);
		this.label12.TabIndex = 93;
		this.label12.Text = "Purchase Last Date";
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbParentCategory);
		this.groupBox1.Controls.Add(this.label85);
		this.groupBox1.Controls.Add(this.txtRemarks);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.txtdocnumber);
		this.groupBox1.Controls.Add(this.txtDocdate);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.groupBox1.Location = new System.Drawing.Point(3, 2);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(1065, 72);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.CmbParentCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbParentCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbParentCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbParentCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance31).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance31;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance32).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance32).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance32).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance32;
		((AppearanceBase)appearance33).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance33;
		((SpecialBoxBase)((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance34).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance34).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance34).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance35;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance36).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance37;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance38).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance39).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance39).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance39).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance39;
		((AppearanceBase)appearance40).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance41;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbParentCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbParentCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Location = new System.Drawing.Point(94, 42);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Name = "CmbParentCategory";
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Size = new System.Drawing.Size(218, 26);
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbParentCategory).Leave += new System.EventHandler(CmbParentCategory_Leave);
		this.label85.AutoSize = true;
		this.label85.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label85.Location = new System.Drawing.Point(3, 48);
		this.label85.Name = "label85";
		this.label85.Size = new System.Drawing.Size(91, 15);
		this.label85.TabIndex = 586;
		this.label85.Text = "Parent Category";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(382, 16);
		this.txtRemarks.Multiline = true;
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(677, 52);
		this.txtRemarks.TabIndex = 3;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(3, 20);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(76, 15);
		this.label1.TabIndex = 64;
		this.label1.Text = "Doc No,Date";
		this.txtdocnumber.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocnumber.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocnumber.Location = new System.Drawing.Point(94, 16);
		this.txtdocnumber.Name = "txtdocnumber";
		this.txtdocnumber.ReadOnly = true;
		this.txtdocnumber.Size = new System.Drawing.Size(91, 23);
		this.txtdocnumber.TabIndex = 0;
		this.txtdocnumber.Leave += new System.EventHandler(txtdocnumber_TextChanged);
		this.txtDocdate.CustomFormat = "dd-MMM-yy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(187, 16);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(125, 22);
		this.txtDocdate.TabIndex = 1;
		this.txtDocdate.ValueChanged += new System.EventHandler(txtDocdate_ValueChanged);
		this.txtDocdate.Leave += new System.EventHandler(txtDocdate_ValueChanged);
		this.label6.AutoSize = true;
		this.label6.BackColor = System.Drawing.Color.White;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75f, System.Drawing.FontStyle.Bold);
		this.label6.Location = new System.Drawing.Point(316, 19);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(60, 17);
		this.label6.TabIndex = 67;
		this.label6.Text = "Remarks";
		this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.panel3.BackColor = System.Drawing.Color.Teal;
		this.panel3.Controls.Add(this.labelFormCaption);
		this.panel3.Controls.Add(this.chkPrintPreview);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(3, 33);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1073, 23);
		this.panel3.TabIndex = 1;
		this.labelFormCaption.AutoSize = true;
		this.labelFormCaption.BackColor = System.Drawing.Color.Teal;
		this.labelFormCaption.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.labelFormCaption.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.labelFormCaption.Location = new System.Drawing.Point(5, 1);
		this.labelFormCaption.Name = "labelFormCaption";
		this.labelFormCaption.Size = new System.Drawing.Size(155, 19);
		this.labelFormCaption.TabIndex = 4;
		this.labelFormCaption.Text = "Purchase Demand";
		this.chkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.chkPrintPreview.AutoSize = true;
		this.chkPrintPreview.Checked = true;
		this.chkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.chkPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.chkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.chkPrintPreview.Location = new System.Drawing.Point(989, 1);
		this.chkPrintPreview.Name = "chkPrintPreview";
		this.chkPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.chkPrintPreview.TabIndex = 3;
		this.chkPrintPreview.Text = "Preview";
		this.chkPrintPreview.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.toolStrip3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1073, 30);
		this.panel2.TabIndex = 0;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[9] { this.btnNew, this.btnRefresh, this.btnSave, this.btnUpdate, this.toolStripSeparator2, this.btnPrint, this.btnAttachment, this.btnCity, this.btnshortcutkeys });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1073, 27);
		this.toolStrip3.TabIndex = 59;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(59, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = (System.Drawing.Image)resources.GetObject("btnSave.Image");
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(64, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(78, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnPrint.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint.Name = "btnPrint";
		this.btnPrint.Size = new System.Drawing.Size(92, 24);
		this.btnPrint.Text = "454-Print";
		this.btnPrint.Click += new System.EventHandler(btnPrint_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.btnCity.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCity.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnCity.Name = "btnCity";
		this.btnCity.Size = new System.Drawing.Size(83, 24);
		this.btnCity.Text = "Define City";
		this.btnCity.Click += new System.EventHandler(btnCity_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.btnshortcutkeys.Click += new System.EventHandler(btnshortcutkeys_Click);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DatagridHistory);
		this.tabPage2.Controls.Add(this.panel13);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.tabPage2.Controls.Add(this.panel12);
		this.tabPage2.Controls.Add(this.panel10);
		this.tabPage2.Controls.Add(this.panel6);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(1079, 559);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.DatagridHistory.AllowEdit = (InheritableBoolean)2;
		DatagridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DatagridHistory.DesignTimeLayout = DatagridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DatagridHistory.DynamicFiltering = true;
		this.DatagridHistory.FilterMode = (FilterMode)2;
		this.DatagridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DatagridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Location = new System.Drawing.Point(3, 135);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Name = "DatagridHistory";
		((ScrollableBase)this.DatagridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DatagridHistory).Size = new System.Drawing.Size(1073, 186);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).TabIndex = 0;
		this.DatagridHistory.TotalRow = (InheritableBoolean)1;
		this.DatagridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DatagridHistory).VisualStyle = (VisualStyle)4;
		this.DatagridHistory.ColumnButtonClick += new ColumnActionEventHandler(DatagridHistory_ColumnButtonClick);
		this.DatagridHistory.SelectionChanged += new System.EventHandler(DatagridHistory_SelectionChanged);
		this.DatagridHistory.LinkClicked += new ColumnActionEventHandler(DatagridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).DoubleClick += new System.EventHandler(DatagridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DatagridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DatagridHistory_KeyDown);
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label7);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel13.Location = new System.Drawing.Point(3, 321);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1073, 22);
		this.panel13.TabIndex = 53;
		this.label7.AutoSize = true;
		this.label7.BackColor = System.Drawing.Color.Teal;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label7.Location = new System.Drawing.Point(4, 3);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(209, 17);
		this.label7.TabIndex = 1;
		this.label7.Text = "Detail of Above Selected Row";
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(3, 343);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(1073, 213);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.panel12.BackColor = System.Drawing.Color.Teal;
		this.panel12.Controls.Add(this.label2);
		this.panel12.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel12.Location = new System.Drawing.Point(3, 110);
		this.panel12.Name = "panel12";
		this.panel12.Size = new System.Drawing.Size(1073, 25);
		this.panel12.TabIndex = 52;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Teal;
		this.label2.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label2.Location = new System.Drawing.Point(2, 3);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(119, 17);
		this.label2.TabIndex = 1;
		this.label2.Text = "Filtered Records";
		this.panel10.Controls.Add(this.groupBox3);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel10.Location = new System.Drawing.Point(3, 60);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(1073, 50);
		this.panel10.TabIndex = 51;
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label57);
		this.groupBox3.Controls.Add(this.label58);
		this.groupBox3.Controls.Add(this.label59);
		this.groupBox3.Controls.Add(this.label60);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, 1);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(1001, 49);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(66, 19);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(139, 23);
		this.FromDateHistory.TabIndex = 0;
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(881, 29);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 8;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(881, 12);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 7;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(800, 12);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 5;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(800, 29);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 6;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(730, 17);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(62, 26);
		this.btnshow.TabIndex = 4;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.btnshow.Leave += new System.EventHandler(btnshow_Leave);
		this.label57.AutoSize = true;
		this.label57.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label57.Location = new System.Drawing.Point(571, 23);
		this.label57.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label57.Name = "label57";
		this.label57.Size = new System.Drawing.Size(64, 15);
		this.label57.TabIndex = 141;
		this.label57.Text = "To Doc No";
		this.label58.AutoSize = true;
		this.label58.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label58.Location = new System.Drawing.Point(401, 23);
		this.label58.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label58.Name = "label58";
		this.label58.Size = new System.Drawing.Size(79, 15);
		this.label58.TabIndex = 140;
		this.label58.Text = "From Doc No";
		this.label59.AutoSize = true;
		this.label59.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label59.Location = new System.Drawing.Point(208, 23);
		this.label59.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label59.Name = "label59";
		this.label59.Size = new System.Drawing.Size(48, 15);
		this.label59.TabIndex = 139;
		this.label59.Text = "To Date";
		this.label60.AutoSize = true;
		this.label60.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label60.Location = new System.Drawing.Point(3, 23);
		this.label60.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label60.Name = "label60";
		this.label60.Size = new System.Drawing.Size(63, 15);
		this.label60.TabIndex = 13;
		this.label60.Text = "From Date";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(640, 19);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(84, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(485, 19);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(84, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(258, 19);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(139, 23);
		this.ToDateHistory.TabIndex = 1;
		this.panel6.Controls.Add(this.panel8);
		this.panel6.Controls.Add(this.panel7);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel6.Location = new System.Drawing.Point(3, 3);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1073, 57);
		this.panel6.TabIndex = 0;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.ctrlGrdBar1);
		this.panel8.Controls.Add(this.label43);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel8.Location = new System.Drawing.Point(0, 28);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1073, 29);
		this.panel8.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(1026, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.DatagridHistory;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(45, 27);
		this.ctrlGrdBar1.TabIndex = 62;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label43.AutoSize = true;
		this.label43.BackColor = System.Drawing.Color.Teal;
		this.label43.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label43.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label43.Location = new System.Drawing.Point(3, 4);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(223, 19);
		this.label43.TabIndex = 1;
		this.label43.Text = "Purchase Demand History ";
		this.panel7.Controls.Add(this.toolStrip2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(0, 0);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1073, 28);
		this.panel7.TabIndex = 0;
		this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[4] { this.toolStripSeparator1, this.btnNewHistory, this.btnRefreshHistory, this.btnLoadAll });
		this.toolStrip2.Location = new System.Drawing.Point(0, 0);
		this.toolStrip2.Name = "toolStrip2";
		this.toolStrip2.Size = new System.Drawing.Size(1073, 27);
		this.toolStrip2.TabIndex = 3;
		this.toolStrip2.Text = "toolStrip2";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(68, 24);
		this.btnNewHistory.Text = "&Reset";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		this.btnRefreshHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefreshHistory.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefreshHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefreshHistory.Name = "btnRefreshHistory";
		this.btnRefreshHistory.Size = new System.Drawing.Size(80, 24);
		this.btnRefreshHistory.Text = "&Refresh";
		this.btnLoadAll.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadAll.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.btnLoadAll.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnLoadAll.Name = "btnLoadAll";
		this.btnLoadAll.Size = new System.Drawing.Size(77, 24);
		this.btnLoadAll.Text = "LoadAll";
		this.btnLoadAll.Visible = false;
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(1087, 588);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
		base.KeyPreview = true;
		base.Name = "frmPurchaseDemand";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmPurchaseDemand_FormClosing);
		base.Load += new System.EventHandler(frmPurchaseDemand_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmPurchaseDemand_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel5.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdPurchaseDemand).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel11.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartment).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbLot).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbAssetsRef).EndInit();
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbParentCategory).EndInit();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DatagridHistory).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel12.ResumeLayout(false);
		this.panel12.PerformLayout();
		this.panel10.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel6.ResumeLayout(false);
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.toolStrip2.ResumeLayout(false);
		this.toolStrip2.PerformLayout();
		base.ResumeLayout(false);
	}
}
