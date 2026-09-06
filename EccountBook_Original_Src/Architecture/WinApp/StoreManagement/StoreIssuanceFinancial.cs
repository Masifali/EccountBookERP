using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Architecture.BLL.Accounts;
using Architecture.BLL.PurchaseTrading;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Accounts;
using Architecture.Model.Inventory;
using Architecture.Model.PurchaseTrading;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Windows.GridEX;

namespace Architecture.WinApp.StoreManagement;

public class StoreIssuanceFinancial : Form
{
	private UserAccount UserAccount = new UserAccount();

	private bool UpdateMode;

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private int RECID;

	private int HeadId;

	private int RefDocNoId;

	private int RefDocTypeId;

	private string RequestType = "";

	public static string PoDemondIdexist = "";

	private DataTable dtgrddetail = new DataTable();

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

	private ToolStripButton btnUpdate;

	private ToolStripButton Print;

	private Label label15;

	private DateTimePicker txtDocdate;

	private Label label2;

	private Label label1;

	private Label label5;

	private TextBox txtRemarks;

	private TextBox txtDocNo;

	internal GridEX grdDetail;

	private Panel panel7;

	private Panel panel10;

	internal GridEX DataGridHistory;

	private ToolStripButton btnRefresh;

	internal UltraCombo CmbAccountId;

	private Label label11;

	private ToolStripButton btnStoreIssuance;

	private CheckBox ChkPrintPreview;

	public StoreIssuanceFinancial(UserAccount user)
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
			formrights = CommonServices.SetRightsValueInRightsObject(base.Name);
			btnUpdate.Enabled = formrights.DoHaveUpdateRights;
			GenerateDocNo();
			DetailAccountFill();
			tabControlGsIssuance.SelectedIndex = 1;
			dtgrddetail.Columns.Add("Id", typeof(int));
			dtgrddetail.Columns.Add("ItemId");
			dtgrddetail.Columns.Add("Item");
			dtgrddetail.Columns.Add("WarehouseId", typeof(int));
			dtgrddetail.Columns.Add("Warehouse", typeof(string));
			dtgrddetail.Columns.Add("RackId", typeof(int));
			dtgrddetail.Columns.Add("RackName", typeof(string));
			dtgrddetail.Columns.Add("ItemConditionId", typeof(int));
			dtgrddetail.Columns.Add("ItemCondition", typeof(string));
			dtgrddetail.Columns.Add("UnitId");
			dtgrddetail.Columns.Add("Unit");
			dtgrddetail.Columns.Add("IssueQty", typeof(double));
			dtgrddetail.Columns.Add("ItemRate", typeof(double));
			dtgrddetail.Columns.Add("ItemAmount", typeof(double));
			dtgrddetail.Columns.Add("DepartmentId");
			dtgrddetail.Columns.Add("Department");
			dtgrddetail.Columns.Add("AssetId");
			dtgrddetail.Columns.Add("Asset");
			dtgrddetail.Columns.Add("Remarks");
			dtgrddetail.Columns.Add("SupplierCustomerId");
			dtgrddetail.Columns.Add("SupplierName");
			txtDocNo.Focus();
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
			int code = CommonServices.StoreIssuanceGenerateCode(451);
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

	private void DetailAccountFill()
	{
		try
		{
			DataTable dtcoalst = CommonServices.Accounts_GetAccountTitleByAccountTypeIds("11,12,13,20,21");
			if (dtcoalst.Rows.Count > 0 && dtcoalst != null)
			{
				DDL.BindDDL(dtcoalst, CmbAccountId, "Id", "AccountTitle", "Account Title", ZeroIndex: false);
				((UltraGridBase)CmbAccountId).DisplayLayout.Bands[0].Columns[1].Width = 350;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void VoucherHeadIdGet()
	{
		try
		{
			HeadId = CommonServices.VoucherHeadIdGet(RECID, 451);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetRefAccountId()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		try
		{
			DataTable dt = VoucherHead.GetAll(new VoucherHead
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 451
			});
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				if (RECID == Conversion.ToInt(dt.Rows[i]["DocumentTypeSrNo"]))
				{
					CmbAccountId.Value = Conversion.ToInt(dt.Rows[i]["RefAccountId"]);
				}
			}
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

	private void Insert()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		try
		{
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
			invGsStoreIssuanceHeader.CompanyId = UserAccount.CompanyId;
			invGsStoreIssuanceHeader.BranchesId = UserAccount.BranchesId;
			invGsStoreIssuanceHeader.DocNo = Conversion.ToInt((object)txtDocNo.Text);
			invGsStoreIssuanceHeader.DocDate = Conversion.ToDateTime((object)txtDocdate.Value);
			invGsStoreIssuanceHeader.DocumentTypeId = 451;
			invGsStoreIssuanceHeader.RefDocNoId = RefDocNoId;
			invGsStoreIssuanceHeader.RefDocumentTypeId = RefDocTypeId;
			invGsStoreIssuanceHeader.Remarks = txtRemarks.Text;
			invGsStoreIssuanceHeader.OrganizationId = UserAccount.OrganizationId;
			invGsStoreIssuanceHeader.EntryDate = DateTime.Now;
			invGsStoreIssuanceHeader.EntryUser = UserAccount.ID;
			invGsStoreIssuanceHeader.ModifyDate = DateTime.Now;
			invGsStoreIssuanceHeader.ModifyUser = UserAccount.ID;
			invGsStoreIssuanceHeader.ApprovedDate = DateTime.Now;
			invGsStoreIssuanceHeader.ApprovedUserId = UserAccount.ID;
			invGsStoreIssuanceHeader.IsApproved = true;
			invGsStoreIssuanceHeader.ActionForVoucherId = 1;
			invGsStoreIssuanceHeader.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			invGsStoreIssuanceHeader.gsStoreIssuanceDetails = new List<InvGsStoreIssuanceDetail>();
			if (grdDetail.GetRows().Count() == 0)
			{
				throw new Exception("Grid record not found");
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				if (Conversion.ToDouble(r.Cells["IssueQty"].Value) > 0.0)
				{
					InvGsStoreIssuanceDetail invGsStoreIssuanceDetail = new InvGsStoreIssuanceDetail();
					invGsStoreIssuanceDetail.Id = Conversion.ToInt(r.Cells["Id"].Value);
					invGsStoreIssuanceDetail.LineId = r.RowIndex + 1;
					invGsStoreIssuanceDetail.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
					invGsStoreIssuanceDetail.Warehouseid = Conversion.ToInt(r.Cells["WarehouseId"].Value);
					invGsStoreIssuanceDetail.RackId = Conversion.ToInt(r.Cells["RackId"].Value);
					invGsStoreIssuanceDetail.ItemUomSchId = Conversion.ToInt(r.Cells["UnitId"].Value);
					invGsStoreIssuanceDetail.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
					invGsStoreIssuanceDetail.IssueQty = Conversion.ToDouble(r.Cells["IssueQty"].Value);
					invGsStoreIssuanceDetail.ItemRate = Conversion.ToDouble(r.Cells["ItemRate"].Value);
					invGsStoreIssuanceDetail.ItemAmount = invGsStoreIssuanceDetail.IssueQty * invGsStoreIssuanceDetail.ItemRate;
					invGsStoreIssuanceDetail.DepartmentId = Conversion.ToInt(r.Cells["DepartmentId"].Value);
					invGsStoreIssuanceDetail.AssetsId = Conversion.ToInt(r.Cells["AssetId"].Value);
					invGsStoreIssuanceDetail.ReamarksDetail = Conversion.ToString(r.Cells["Remarks"].Value);
					invGsStoreIssuanceDetail.SupplierCustomerId = Conversion.ToInt(r.Cells["SupplierCustomerId"].Value);
					invGsStoreIssuanceDetail.DrAccountId = Conversion.ToInt(CmbAccountId.Value);
					invGsStoreIssuanceHeader.gsStoreIssuanceDetails.Add(invGsStoreIssuanceDetail);
					FormHelper.ValidateField(invGsStoreIssuanceDetail.Warehouseid, "Warehouse", r.RowIndex);
					FormHelper.ValidateField(invGsStoreIssuanceDetail.RackId, "Rack Name", r.RowIndex);
					FormHelper.ValidateField(invGsStoreIssuanceDetail.ItemConditionId, "Item Condition", r.RowIndex);
				}
			}
			InvGsStoreIssuanceHeader.Save(invGsStoreIssuanceHeader);
			if (RECID == 0)
			{
				MessageBox.Show("Record Save SuccessFully" + invGsStoreIssuanceHeader.DocNo);
			}
			else
			{
				MessageBox.Show("Record Update SuccessFully" + invGsStoreIssuanceHeader.DocNo);
			}
			int VoucherId = CommonServices.VoucherHeadIdGet(RECID, 451);
			formReset();
			if (ChkPrintPreview.Checked)
			{
				CommonServices.VoucherReport_118(VoucherId, 451);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Expected O, but got Unknown
		try
		{
			if (RECID == 0)
			{
				throw new Exception("Record Not Found For Update");
			}
			if (InvGsStoreIssuanceHeader.ReadByDocNo(new InvGsStoreIssuanceHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 451,
				DocNo = Conversion.ToInt((object)txtDocNo.Text.Trim())
			}) == 0)
			{
				throw new Exception("Record Not Exist against this number");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void formReset()
	{
		try
		{
			tabControlGsIssuance.SelectedIndex = 1;
			RECID = 0;
			HeadId = 0;
			RefDocNoId = 0;
			RefDocTypeId = 0;
			PoDemondIdexist = "";
			RequestType = "";
			btnUpdate.Visible = false;
			UpdateMode = false;
			dtgrddetail.Clear();
			grdDetail.ClearStructure();
			txtRemarks.Text = string.Empty;
			txtDocNo.Focus();
			GenerateDocNo();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtDocNo.Text.Trim() == string.Empty)
		{
			MessageBox.Show("document Number Field Required");
			txtDocNo.Focus();
			return false;
		}
		return true;
	}

	private void Print_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.VoucherReport_118(HeadId, 451);
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
			if (e.Control && e.KeyCode == Keys.P)
			{
				Print_Click(null, null);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			InvGsStoreIssuanceHeader head = new InvGsStoreIssuanceHeader();
			head = InvGsStoreIssuanceHeader.GetByID(ID);
			if (head != null)
			{
				tabControlGsIssuance.SelectedIndex = 0;
				btnUpdate.Visible = true;
				txtDocdate.Value = head.DocDate;
				txtDocNo.Text = head.DocNo.ToString();
				txtRemarks.Text = head.Remarks;
				RefDocNoId = head.RefDocNoId;
				RefDocTypeId = head.RefDocumentTypeId;
				if (RefDocTypeId == 85)
				{
					RequestType = "DeliveryOrder";
				}
				else
				{
					RequestType = "DepartmentRequest";
				}
				dtgrddetail.Clear();
				for (int i = 0; i < head.gsStoreIssuanceDetails.Count; i++)
				{
					dtgrddetail.Rows.Add(head.gsStoreIssuanceDetails[i].Id, head.gsStoreIssuanceDetails[i].ItemId, head.gsStoreIssuanceDetails[i].ItemName, head.gsStoreIssuanceDetails[i].Warehouseid, head.gsStoreIssuanceDetails[i].WareHouseName, head.gsStoreIssuanceDetails[i].RackId, head.gsStoreIssuanceDetails[i].rackName, head.gsStoreIssuanceDetails[i].ItemConditionId, head.gsStoreIssuanceDetails[i].ItemCondition, head.gsStoreIssuanceDetails[i].ItemUomSchId, head.gsStoreIssuanceDetails[i].Equivalent, head.gsStoreIssuanceDetails[i].IssueQty, head.gsStoreIssuanceDetails[i].ItemRate, head.gsStoreIssuanceDetails[i].ItemAmount, head.gsStoreIssuanceDetails[i].DepartmentId, head.gsStoreIssuanceDetails[i].DepartmentName, head.gsStoreIssuanceDetails[i].AssetsId, head.gsStoreIssuanceDetails[i].AssetName, head.gsStoreIssuanceDetails[i].ReamarksDetail, head.gsStoreIssuanceDetails[i].SupplierCustomerId, head.gsStoreIssuanceDetails[i].CompanyName);
				}
				grdDetail.DataSource = dtgrddetail;
				grdDetail.RetrieveStructure();
				DetailGridSettings();
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

	private void DetailGridSettings()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			new InvWareHouse
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			};
			Dictionary<string, EditType> editableColumns = new Dictionary<string, EditType> { 
			{
				"Remarks",
				(EditType)1
			} };
			GridEX_Helper.GridWrappingAndColumnSettings(grdDetail, 2, 3, null, editableColumns);
			grdDetail.RootTable.Columns["Item"].Width = 250;
			if (RequestType == "DeliveryOrder")
			{
				grdDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
				grdDetail.RootTable.Columns["DepartmentId"].Visible = false;
				grdDetail.RootTable.Columns["Department"].Visible = false;
				grdDetail.RootTable.Columns["AssetId"].Visible = false;
				grdDetail.RootTable.Columns["Asset"].Visible = false;
			}
			else
			{
				grdDetail.RootTable.Columns["SupplierCustomerId"].Visible = false;
				grdDetail.RootTable.Columns["SupplierName"].Visible = false;
				grdDetail.RootTable.Columns["DepartmentId"].Visible = false;
				grdDetail.RootTable.Columns["AssetId"].Visible = false;
				grdDetail.RootTable.Columns["Department"].Width = 150;
				grdDetail.RootTable.Columns["ItemAmount"].AggregateFunction = (AggregateFunction)2;
				grdDetail.RootTable.Columns["IssueQty"].AggregateFunction = (AggregateFunction)2;
			}
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["UnitId"].Visible = false;
			grdDetail.RootTable.Columns["ItemConditionId"].Visible = false;
			grdDetail.RootTable.Columns["WarehouseId"].Visible = false;
			grdDetail.RootTable.Columns["RackId"].Visible = false;
			grdDetail.RootTable.Columns["Unit"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["Id"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Item"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Unit"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemConditionId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemCondition"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["WarehouseId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Warehouse"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["RackId"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["RackName"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Asset"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Department"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["IssueQty"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemRate"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["ItemAmount"].EditType = (EditType)0;
			grdDetail.RootTable.Columns["Item"].Width = Constants.InventoryConstants.ItemName;
			grdDetail.RootTable.Columns["Warehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["RackName"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["ItemCondition"].Width = Constants.InventoryConstants.StringConstant;
			grdDetail.RootTable.Columns["Unit"].Width = Constants.InventoryConstants.UomConstant;
			grdDetail.RootTable.Columns["IssueQty"].Width = Constants.InventoryConstants.QtyConstant;
			grdDetail.RootTable.Columns["ItemRate"].Width = Constants.InventoryConstants.ExRateConstant;
			grdDetail.RootTable.Columns["ItemAmount"].Width = Constants.InventoryConstants.AmountConstant;
			grdDetail.RootTable.Columns["Department"].Width = Constants.InventoryConstants.WareHouseName;
			grdDetail.RootTable.Columns["Asset"].Width = Constants.InventoryConstants.AccountTitle;
			grdDetail.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grdDetail.RootTable.Columns["SupplierName"].Width = Constants.InventoryConstants.SupplierName;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNo_TextChanged(object sender, EventArgs e)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		try
		{
			int Code = InvGsStoreIssuanceHeader.ReadByDocNo(new InvGsStoreIssuanceHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 451,
				DocNo = Conversion.ToInt((object)txtDocNo.Text.Trim())
			});
			if (Code > 0)
			{
				RECID = Code;
				ReadById(RECID);
				VoucherHeadIdGet();
				GetRefAccountId();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		PoDemondIdexist = "";
		Close();
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
			MessageBox.Show(ex.Message);
		}
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			GenerateDocNo();
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
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		try
		{
			DataTable dtHistory = InvGsStoreIssuanceHeader.FormHistoryForFinancialEffects(new ReportsParameters
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				DocumentTypeId = 451,
				NoOfRecords = 0,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id,
				UserId = UserAccount.ID,
				CanViewAllRecord = true
			});
			if (dtHistory.Rows.Count > 0 && dtHistory != null)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("DocDate", typeof(DateTime));
				dt.Columns.Add("DocNo", typeof(int));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("Equivalent", typeof(double));
				dt.Columns.Add("IssueQty", typeof(double));
				dt.Columns.Add("WareHouseName", typeof(string));
				dt.Columns.Add("DepartmentName", typeof(string));
				dt.Columns.Add("AssetName", typeof(string));
				dt.Columns.Add("EntryDate", typeof(DateTime));
				dt.Columns.Add("EntryUser", typeof(string));
				dt.Columns.Add("ModifyDate", typeof(DateTime));
				dt.Columns.Add("ModifyUser", typeof(string));
				foreach (DataRow row in dtHistory.Rows)
				{
					int id = Conversion.ToInt(row["Id"]);
					DateTime docDate = Conversion.ToDateTime(row["DocDate"]);
					int docNo = Conversion.ToInt(row["DocNo"]);
					string remarks = row["Remarks"].ToString();
					string itemName = row["ItemName"].ToString();
					double equivalent = Conversion.ToDouble(row["Equivalent"]);
					double issueQty = Conversion.ToDouble(row["IssueQty"]);
					string wareHouseName = row["WareHouseName"].ToString();
					string departmentName = row["DepartmentName"].ToString();
					string assetName = row["AssetName"].ToString();
					DateTime entryDate = Conversion.ToDateTime(row["EntryDate"]);
					string entryUser = row["EntryUser"].ToString();
					DateTime modifyDate = Conversion.ToDateTime(row["ModifyDate"]);
					string modifyUser = row["ModifyUser"].ToString();
					dt.Rows.Add(id, docDate, docNo, remarks, itemName, equivalent, issueQty, wareHouseName, departmentName, assetName, entryDate, entryUser, modifyDate, modifyUser);
				}
				DataGridHistory.DataSource = dt;
				DataGridHistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				DataGridHistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		try
		{
			DataGridHistory.RootTable.Columns["Id"].Visible = false;
			DataGridHistory.RootTable.Columns["DocDate"].FormatString = "dd-MMM-yyyy";
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yyyy hh:mm tt";
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3);
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant + 10;
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			DataGridHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			DataGridHistory.RootTable.Columns["Equivalent"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistory.RootTable.Columns["IssueQty"].Width = Constants.InventoryConstants.QtyConstant;
			DataGridHistory.RootTable.Columns["WareHouseName"].Width = Constants.InventoryConstants.WareHouseName;
			DataGridHistory.RootTable.Columns["DepartmentName"].Width = Constants.InventoryConstants.WareHouseName;
			DataGridHistory.RootTable.Columns["AssetName"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			GridEX_Helper.AddButton(DataGridHistory, "Edit", "Edit", 40, 0);
			ctrlGrdBar1_Load(null, null);
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
			HistoryFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdDetail.UpdateData();
			GridEXRow item = grdDetail.CurrentRow;
			if (e.Column.Key == "IssueQty" && item != null && (int)item.RowType == 0 && item.Cells["IssueQty"].Value.ToString() != string.Empty && item.Cells["BalanceStock"].Value.ToString() != string.Empty)
			{
				double BalanceQty = Conversion.ToDouble((object)item.Cells["BalanceStock"].Value.ToString());
				if (Conversion.ToDouble((object)item.Cells["IssueQty"].Value.ToString()) > BalanceQty)
				{
					MessageBox.Show("IssueQty grater than BalanceQty Please Check");
					return;
				}
			}
			grdDetail.UpdateData();
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
			if (e.Column.Key == "Edit")
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

	private void StoreIssuanceFinancial_FormClosing(object sender, FormClosingEventArgs e)
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

	private void btnStoreIssuance_Click(object sender, EventArgs e)
	{
		try
		{
			new frmGSIssuance(UserAccount).Show();
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
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Expected O, but got Unknown
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Expected O, but got Unknown
		//IL_05e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ec: Expected O, but got Unknown
		//IL_16e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_16eb: Expected O, but got Unknown
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControlGsIssuance = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel2 = new System.Windows.Forms.Panel();
		this.panel6 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.CmbAccountId = new UltraCombo();
		this.label11 = new System.Windows.Forms.Label();
		this.txtDocNo = new System.Windows.Forms.TextBox();
		this.label5 = new System.Windows.Forms.Label();
		this.txtRemarks = new System.Windows.Forms.TextBox();
		this.txtDocdate = new System.Windows.Forms.DateTimePicker();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.ChkPrintPreview = new System.Windows.Forms.CheckBox();
		this.label15 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.Print = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.btnStoreIssuance = new System.Windows.Forms.ToolStripButton();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.DataGridHistory = new GridEX();
		this.panel1.SuspendLayout();
		this.tabControlGsIssuance.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel6.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountId).BeginInit();
		this.panel4.SuspendLayout();
		this.panel3.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControlGsIssuance);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(929, 463);
		this.panel1.TabIndex = 0;
		this.tabControlGsIssuance.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControlGsIssuance.Controls.Add(this.tabPage1);
		this.tabControlGsIssuance.Controls.Add(this.tabPage2);
		this.tabControlGsIssuance.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControlGsIssuance.Location = new System.Drawing.Point(0, 0);
		this.tabControlGsIssuance.Name = "tabControlGsIssuance";
		this.tabControlGsIssuance.SelectedIndex = 0;
		this.tabControlGsIssuance.Size = new System.Drawing.Size(929, 463);
		this.tabControlGsIssuance.TabIndex = 0;
		this.tabControlGsIssuance.SelectedIndexChanged += new System.EventHandler(tabControlGsIssuance_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage1.Size = new System.Drawing.Size(921, 437);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel2.Controls.Add(this.panel6);
		this.panel2.Controls.Add(this.panel5);
		this.panel2.Controls.Add(this.panel4);
		this.panel2.Controls.Add(this.panel3);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel2.Location = new System.Drawing.Point(3, 3);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(915, 431);
		this.panel2.TabIndex = 0;
		this.panel6.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(0, 110);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(915, 321);
		this.panel6.TabIndex = 3;
		((ScrollableBase)this.grdDetail).BackColor = System.Drawing.Color.White;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(915, 321);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 14;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.CellUpdated += new ColumnActionEventHandler(grdDetail_CellUpdated);
		this.panel5.BackColor = System.Drawing.Color.White;
		this.panel5.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAccountId);
		this.panel5.Controls.Add(this.label11);
		this.panel5.Controls.Add(this.txtDocNo);
		this.panel5.Controls.Add(this.label5);
		this.panel5.Controls.Add(this.txtRemarks);
		this.panel5.Controls.Add(this.txtDocdate);
		this.panel5.Controls.Add(this.label2);
		this.panel5.Controls.Add(this.label1);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 56);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(915, 54);
		this.panel5.TabIndex = 2;
		this.CmbAccountId.AllowNull = (DefaultableBoolean)1;
		this.CmbAccountId.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAccountId.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAccountId.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance85).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Appearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance86).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance86).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance86).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance87;
		((SpecialBoxBase)((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance88;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance89;
		((AppearanceBase)appearance90).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance90).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance91;
		((AppearanceBase)appearance92).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance92).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance93).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance93).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance93).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance93;
		((AppearanceBase)appearance94).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance94;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance95).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance95).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance95;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAccountId).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAccountId.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Location = new System.Drawing.Point(221, 24);
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Name = "CmbAccountId";
		((System.Windows.Forms.Control)(object)this.CmbAccountId).Size = new System.Drawing.Size(172, 26);
		((System.Windows.Forms.Control)(object)this.CmbAccountId).TabIndex = 90;
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label11.Location = new System.Drawing.Point(221, 5);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(68, 15);
		this.label11.TabIndex = 91;
		this.label11.Text = "Account Dr";
		this.txtDocNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNo.Enabled = false;
		this.txtDocNo.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocNo.Location = new System.Drawing.Point(9, 25);
		this.txtDocNo.Name = "txtDocNo";
		this.txtDocNo.Size = new System.Drawing.Size(79, 23);
		this.txtDocNo.TabIndex = 3;
		this.txtDocNo.Leave += new System.EventHandler(txtDocNo_TextChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(394, 5);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(52, 15);
		this.label5.TabIndex = 89;
		this.label5.Text = "Remarks";
		this.txtRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarks.Location = new System.Drawing.Point(394, 25);
		this.txtRemarks.Name = "txtRemarks";
		this.txtRemarks.Size = new System.Drawing.Size(347, 23);
		this.txtRemarks.TabIndex = 7;
		this.txtDocdate.CustomFormat = "dd-MMM-yyyy";
		this.txtDocdate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtDocdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.txtDocdate.Location = new System.Drawing.Point(89, 25);
		this.txtDocdate.Name = "txtDocdate";
		this.txtDocdate.Size = new System.Drawing.Size(131, 23);
		this.txtDocdate.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(89, 5);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(57, 15);
		this.label2.TabIndex = 80;
		this.label2.Text = "Doc Date";
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(9, 5);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(48, 15);
		this.label1.TabIndex = 79;
		this.label1.Text = "Doc No";
		this.panel4.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel4.Controls.Add(this.ChkPrintPreview);
		this.panel4.Controls.Add(this.label15);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(0, 29);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(915, 27);
		this.panel4.TabIndex = 1;
		this.ChkPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkPrintPreview.AutoSize = true;
		this.ChkPrintPreview.Checked = true;
		this.ChkPrintPreview.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkPrintPreview.Location = new System.Drawing.Point(795, 4);
		this.ChkPrintPreview.Margin = new System.Windows.Forms.Padding(2);
		this.ChkPrintPreview.Name = "ChkPrintPreview";
		this.ChkPrintPreview.Size = new System.Drawing.Size(122, 20);
		this.ChkPrintPreview.TabIndex = 31;
		this.ChkPrintPreview.TabStop = false;
		this.ChkPrintPreview.Text = "Print Preview";
		this.ChkPrintPreview.UseVisualStyleBackColor = true;
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.label15.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.ForeColor = System.Drawing.SystemColors.ControlLightLight;
		this.label15.Location = new System.Drawing.Point(2, 3);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(266, 19);
		this.label15.TabIndex = 1;
		this.label15.Text = "Store Issuance Financial Effects";
		this.panel3.Controls.Add(this.toolStrip1);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(915, 29);
		this.panel3.TabIndex = 0;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[5] { this.btnnew, this.btnRefresh, this.btnUpdate, this.Print, this.btnStoreIssuance });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(915, 27);
		this.toolStrip1.TabIndex = 2;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
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
		this.Print.Size = new System.Drawing.Size(115, 24);
		this.Print.Text = "118-Voucher";
		this.Print.Click += new System.EventHandler(Print_Click);
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.btnStoreIssuance.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnStoreIssuance.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnStoreIssuance.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnStoreIssuance.Name = "btnStoreIssuance";
		this.btnStoreIssuance.Size = new System.Drawing.Size(131, 24);
		this.btnStoreIssuance.Text = "Store Issuance";
		this.btnStoreIssuance.Click += new System.EventHandler(btnStoreIssuance_Click);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage2.Size = new System.Drawing.Size(921, 437);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel10);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(3, 3);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(915, 431);
		this.panel7.TabIndex = 0;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 0);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(915, 431);
		this.panel10.TabIndex = 2;
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(915, 431);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 64;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(929, 463);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "StoreIssuanceFinancial";
		this.Text = "frmGSIssuance";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(StoreIssuanceFinancial_FormClosing);
		base.Load += new System.EventHandler(frmGSIssuance_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(frmGSIssuance_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControlGsIssuance.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel2.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbAccountId).EndInit();
		this.panel4.ResumeLayout(false);
		this.panel4.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		base.ResumeLayout(false);
	}
}
