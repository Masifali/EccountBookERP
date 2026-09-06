using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using UserAccount = global::Architecture.Model.UserAccount;
using FolderHierarchy = global::Architecture.Model.DMS.FolderHierarchy;
using InvPurchaseInvoiceDetail = global::Architecture.Model.Inventory.InvPurchaseInvoiceDetail;
using InvPurchaseInvoice = global::Architecture.Model.Inventory.InvPurchaseInvoice;
using InvSaleInvoiceDetail = global::Architecture.Model.Inventory.InvSaleInvoiceDetail;
using SaleOrderDetail = global::Architecture.Model.Inventory.SaleOrderDetail;
using Appearance = Infragistics.Win.Appearance;
using AutoCompleteMode = Infragistics.Win.AutoCompleteMode;
using Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme;
namespace Architecture.WinApp.StoreManagement
{

public class frmDepartmentRequest : Form
{
	private int RecId;

	private int ItemId;

	private bool UpdateMode;

	private bool IsApproved;

	private int DocumentTypeId = 450;

	private int updateDetailIndex = -1;

	private int DefaultDaysToLessFromHistoryFromDate;

	private bool FormInitialized;

	private bool ItemSearchByNameOrCode;

	private bool PackingMaterialItemsNotIncludeOnStore;

	private DataTable dtitem = new DataTable();

	private DataTable dtdetail = new DataTable();

	private DataTable dtDepartment = new DataTable();

	private string ScreenName = "frmDepartmentRequest";

	private Attachment AT = new Attachment();

	private DropDownBind DDL = new DropDownBind();

	private Rightsobjects formrights = new Rightsobjects();

	private UserAccount UserAccount = new UserAccount();

	private List<DepartmentRequestDetail> lstRemoveRecord = new List<DepartmentRequestDetail>();

	private IContainer components;

	private Label label14;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX DataGridHistory;

	private Panel panel9;

	private Panel panel8;

	private TabPage tabPage2;

	private ToolStripButton btnRefresh;

	private ToolStripButton btnAttachment;

	private ToolStripButton btnPrint451;

	private ToolStripButton btnSave;

	private ToolStripButton btnUpdate;

	private CtrlGrdBar ctrlGrdBar1;

	private ToolStrip toolStrip1;

	private ToolStripButton btnNew;

	private Panel panel2;

	private Label label23;

	private Panel panel3;

	private Button btnCancelUpdateDetial;

	private Button btnUpdateDetail;

	internal UltraCombo cmbItemUOM;

	private Button Add;

	internal UltraCombo cmbItem;

	private TextBox txtRequestedQty;

	private Label label35;

	private Label label36;

	private Label label37;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel4;

	private Panel panel6;

	internal GridEX grdDetail;

	internal UltraCombo CmbAssetName;

	private Label label4;

	private Label label6;

	private TextBox txtBarcodeReader;

	private Label lblStockQty;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private GroupBox groupBox2;

	private Panel panel5;

	private Panel panel10;

	private Label label7;

	private ToolStrip btnLoadall;

	private ToolStripButton BtnNewHistory;

	private Panel panel11;

	private Panel panel7;

	private Label label8;

	internal GridEX DataGridHistoryDetail;

	private GroupBox groupBox1;

	private CheckBox ChkBok;

	private Label label1;

	internal UltraCombo cmbDepartmentFrom;

	private Label label2;

	internal UltraCombo cmbDepartmentTo;

	private Label label17;

	private TextBox txtdocno;

	private DateTimePicker DocDate;

	private Button BtnDepartmentDefine;

	private Label label3;

	private ToolStripButton BtnShortCutkeys;

	private TextBox txtremarks;

	private Label label5;

	private GroupBox groupBox3;

	private RadioButton rdapproveddate;

	private RadioButton rdmodifydate;

	private RadioButton drdocdate;

	private RadioButton rdentrydate;

	private Button btnshow;

	private Label label22;

	private Label label43;

	private Label label62;

	private Label label63;

	private TextBox txtToDocNoHistory;

	private TextBox txtFromDocNoHistory;

	private DateTimePicker ToDateHistory;

	private DateTimePicker FromDateHistory;

	private ToolStripButton btnDelete;

	internal UltraCombo CmbItemCondition;

	private Label label25;

	public frmDepartmentRequest(UserAccount userAccount)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Expected O, but got Unknown
		InitializeComponent();
		UserAccount = userAccount;
		List<UserAccountAllocation> userAccountAllocationsList = userAccount.UserAccountAllocationsList;
		if (userAccountAllocationsList != null && userAccountAllocationsList.Count > 0)
		{
			ScreenName = UserAccount.UserAccountAllocationsList[0].ScreenName;
		}
		InitializeComponentCustom();
		InitializeComponentMethod();
		FormHelper.FormEventHandler(this);
	}

	private void EccountBook_Load(object sender, EventArgs e)
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

	private void InitializeComponentCustom()
	{
		try
		{
			dtitem.Columns.Add("Id", typeof(int));
			dtitem.Columns.Add("ItemName", typeof(string));
			dtitem.Columns.Add("ItemCode", typeof(string));
			dtdetail.Columns.Add("Id", typeof(int));
			dtdetail.Columns.Add("ItemId", typeof(int));
			dtdetail.Columns.Add("Item", typeof(string));
			dtdetail.Columns.Add("ItemCode", typeof(string));
			dtdetail.Columns.Add("ItemConditionId", typeof(int));
			dtdetail.Columns.Add("ItemCondition", typeof(string));
			dtdetail.Columns.Add("ItemUOMId", typeof(int));
			dtdetail.Columns.Add("UOM", typeof(string));
			dtdetail.Columns.Add("AssetId", typeof(int));
			dtdetail.Columns.Add("AssetName", typeof(string));
			dtdetail.Columns.Add("RequestedQty", typeof(double));
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
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			DepartmentDbCall();
			string documentNoData = DocumentNoDbCall();
			DataTable assets = FixedAssestDbCall();
			return new
			{
				DocumentNoData = documentNoData,
				assets = assets
			};
		}).ContinueWith(t =>
		{
			if (!base.IsDisposed)
			{
				if (t.IsFaulted)
				{
					MessageBox.Show("Error occurred during database call.");
				}
				else
				{
					var result = t.Result;
					btnSave.Enabled = formrights.DoHaveSaveRight;
					btnUpdate.Enabled = formrights.DoHaveUpdateRights;
					btnDelete.Enabled = formrights.DoHaveCanDelete;
					btnPrint451.Enabled = formrights.DoHavePrintRights;
					ChkBok.Enabled = formrights.DoHavePrintRights;
					ChkBok.Checked = formrights.DoHavePrintRights;
					FormHelper.SetCtrlGrdBarRight(ctrlGrdBar1, formrights);
					FormHelper.SetCtrlGrdBarRight(ctrlGrdBar2, formrights);
					if (RecId == 0)
					{
						txtdocno.Text = result.DocumentNoData;
					}
					PackingMaterialItemsNotIncludeOnStore = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialItemsNotIncludeOnStore"));
					ItemSearchByNameOrCode = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("ItemSearchByCode"));
					if (ItemSearchByNameOrCode)
					{
						rdbtnItemCode.Checked = true;
					}
					else
					{
						rdbtnItemName.Checked = true;
					}
					ItemDtsFillFromGlobal();
					ItemNameBind();
					ItemConditionBindFromGlobal();
					DeprtmentFill();
					FixedAssest(result.assets);
					if (dtdetail.Columns.Count > 0)
					{
						grdDetail.DataSource = dtdetail;
						grdDetail.RetrieveStructure();
						grdSettings();
					}
					DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)CommonServices.GetConfigurationByOrgCompandConfigDescription("DefaultDaysToLessFromHistoryFromDate"));
					FromDateHistory.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
					ToDateHistory.Value = DateTime.Now;
					FormInitialized = true;
					DocDate.Focus();
				}
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void DepartmentDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			Department department = new Department();
			department.OrganizationId = UserAccount.OrganizationId;
			department.CompanyId = UserAccount.CompanyId;
			dtDepartment = Department.GetAll(department);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DeprtmentFill()
	{
		try
		{
			int idf = Conversion.ToInt(cmbDepartmentFrom.Value);
			int idt = Conversion.ToInt(cmbDepartmentTo.Value);
			if (dtDepartment.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(cmbDepartmentFrom, dtDepartment, "Id", "DepartmentName", "From Department", AllColumns: true, idf, insertDefaultRow: true, null, null, ActivateRow: false);
				InfragisticsHelper.BindAndRetainSelection(cmbDepartmentTo, dtDepartment, "Id", "DepartmentName", "To Department", AllColumns: true, idt, insertDefaultRow: true, null, null, ActivateRow: false);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemDtsFillFromGlobal()
	{
		try
		{
			dtitem.Clear();
			List<getGlobalAllItems> allItems = clsGlobalVariables.getGlobalAllItems;
			if (allItems == null || allItems.Count == 0)
			{
				return;
			}
			HashSet<int> allowedTypes = new HashSet<int>();
			if (PackingMaterialItemsNotIncludeOnStore)
			{
				allowedTypes.Add(17);
			}
			else
			{
				allowedTypes.Add(14);
				allowedTypes.Add(17);
			}
			dtitem.BeginLoadData();
			foreach (getGlobalAllItems item in allItems)
			{
				if (allowedTypes.Contains(item.ItemTypeOfTypeId))
				{
					dtitem.Rows.Add(item.Id, item.ItemName, item.ItemCode);
				}
			}
			dtitem.EndLoadData();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error loading items: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void ItemNameBind()
	{
		try
		{
			int Id = Conversion.ToInt(cmbItem.Value);
			string DisplayMember = (rdbtnItemName.Checked ? "ItemName" : "ItemCode");
			string caption = (rdbtnItemName.Checked ? "Item Name" : "Item Code");
			InfragisticsHelper.BindAndRetainSelection(cmbItem, dtitem, "Id", DisplayMember, caption, AllColumns: true, Id, insertDefaultRow: true, null, null, ActivateRow: false);
			DataTable dataTable = dtitem;
			if (dataTable != null && dataTable.Rows.Count > 0 && ((UltraGridBase)cmbItem).DataSource != null)
			{
				((UltraGridBase)cmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemUOMFill()
	{
		try
		{
			string Packuom = ((Control)(object)cmbItemUOM).Text;
			DataTable dtUOM = CommonServices.dtUomFromGloablUomScheduleByItemId(Conversion.ToInt(cmbItem.Value));
			InfragisticsHelper.BindAndRetainSelection(cmbItemUOM, dtUOM, "Id", "UOMCode", "Pack Uom", AllColumns: true, Packuom, insertDefaultRow: true, null, null, ActivateRow: true, 0, "UOMCode");
			if (Conversion.ToInt(cmbItemUOM.Value) != 0)
			{
				return;
			}
			int rowCount = dtUOM?.Rows.Count ?? 0;
			if (rowCount == 1)
			{
				((UltraGridBase)cmbItemUOM).Rows[1].Activate();
			}
			else
			{
				if (rowCount <= 1)
				{
					return;
				}
				DataRow eqRow = dtUOM.AsEnumerable().FirstOrDefault((DataRow r) => Conversion.ToDecimal(r["Equivalent"]) == 1m);
				if (eqRow != null)
				{
					int eqId = Conversion.ToInt(eqRow["Id"]);
					UltraGridRow obj = ((IEnumerable)((UltraGridBase)cmbItemUOM).Rows).Cast<UltraGridRow>().FirstOrDefault((UltraGridRow r) => Conversion.ToInt(r.Cells["Id"].Value) == eqId);
					if (obj != null)
					{
						obj.Activate();
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private DataTable FixedAssestDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		try
		{
			return FixedAssetsRegister.GetAll(new FixedAssetsRegister
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId
			});
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void FixedAssest(DataTable dt)
	{
		try
		{
			int id = Conversion.ToInt(CmbAssetName.Value);
			if (dt.Rows.Count > 0)
			{
				InfragisticsHelper.BindAndRetainSelection(CmbAssetName, dt, "Id", "AssetName", "Fixed Asset Name", AllColumns: true, id, insertDefaultRow: true);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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

	private void rdbtnItemName_CheckedChanged(object sender, EventArgs e)
	{
		try
		{
			if (dtitem.Rows.Count > 0)
			{
				ItemNameBind();
				cmbItem.Focus();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDbCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		try
		{
			return DepartmentRequest.GenerateDocNo(new DepartmentRequest
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchId = UserAccount.BranchesId,
				DocTypeId = DocumentTypeId,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}).ToString();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private bool FormValidation()
	{
		if (txtdocno.Text.Trim() == "" || Conversion.ToInt((object)txtdocno.Text.Trim()) == 0)
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDepartmentFrom).ActiveRow == null || Conversion.ToInt(cmbDepartmentFrom.Value) == 0)
		{
			MessageBox.Show("Department From Field is Required");
			cmbDepartmentFrom.Focus();
			return false;
		}
		if (((UltraGridBase)cmbDepartmentTo).ActiveRow == null || Conversion.ToInt(cmbDepartmentTo.Value) == 0)
		{
			MessageBox.Show("Department To Field is Required");
			cmbDepartmentTo.Focus();
			return false;
		}
		return true;
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Clear();
			RecId = 0;
			ItemId = 0;
			IsApproved = false;
			((Control)(object)cmbDepartmentFrom).Text = string.Empty;
			((Control)(object)cmbDepartmentTo).Text = string.Empty;
			txtremarks.Text = string.Empty;
			txtdocno.Text = string.Empty;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemUOM).Text = string.Empty;
			txtRequestedQty.Text = string.Empty;
			((Control)(object)CmbAssetName).Text = string.Empty;
			DocDate.Value = DateTime.Now;
			dtdetail.Rows.Clear();
			grdDetail.DataSource = dtdetail;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			Add.Visible = true;
			btnSave.Visible = true;
			btnUpdate.Visible = false;
			btnDelete.Visible = false;
			txtdocno.Text = DocumentNoDbCall();
			DocDate.Focus();
			updateDetailIndex = -1;
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			lstRemoveRecord.Clear();
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
			updateDetailIndex = -1;
			((Control)(object)cmbItem).Text = string.Empty;
			((Control)(object)cmbItemUOM).Text = string.Empty;
			txtRequestedQty.Text = string.Empty;
			((Control)(object)CmbAssetName).Text = string.Empty;
			cmbItem.Focus();
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnNew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			List<string> allServices = new List<string> { "AllItems", "RacksWithWarehouseAndItem", "ItemConditions", "UomSchedule" };
			DatatableHelper.GlobalServicesDbCall(UserAccount, allServices);
			PackingMaterialItemsNotIncludeOnStore = Conversion.ToBool((object)GlobalVariables_Helper.GetConfigValueFromGlobal("PackingMaterialItemsNotIncludeOnStore"));
			DepartmentDbCall();
			DeprtmentFill();
			FixedAssest(FixedAssestDbCall());
			ItemDtsFillFromGlobal();
			ItemNameBind();
			ItemConditionBindFromGlobal();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmDepartmentRequest_FormClosing(object sender, FormClosingEventArgs e)
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

	private void Insert()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Expected O, but got Unknown
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
			DepartmentRequest deprt = new DepartmentRequest();
			if (RecId > 0)
			{
				if (IsApproved)
				{
					throw new Exception("Record not Update because record has approved");
				}
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				deprt.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			deprt.OrganizationId = UserAccount.OrganizationId;
			deprt.CompanyId = UserAccount.CompanyId;
			deprt.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			deprt.BranchId = Conversion.ToInt((object)UserAccount.BranchesId);
			deprt.ProjectId = Conversion.ToInt((object)UserAccount.BranchesId);
			deprt.DocTypeId = 450;
			deprt.EntryDate = DateTime.Today;
			deprt.EntryUser = UserAccount.ID;
			deprt.ModifyDate = DateTime.Today;
			deprt.ModifyUser = UserAccount.ID;
			deprt.PostDate = DateTime.Today;
			deprt.PostUser = UserAccount.ID;
			deprt.DocNo = Conversion.ToInt((object)txtdocno.Text.Trim());
			deprt.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			deprt.FromDepartmentId = Conversion.ToInt(cmbDepartmentFrom.Value);
			deprt.ToDepartmentId = Conversion.ToInt(cmbDepartmentTo.Value);
			deprt.RemarksHeader = Conversion.ToString((object)txtremarks.Text);
			deprt.ScreenName = Conversion.ToString((object)base.Name);
			deprt.DepartmentRequestDetail = new List<DepartmentRequestDetail>();
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow r in rows)
			{
				DepartmentRequestDetail vd = new DepartmentRequestDetail();
				if (RecId > 0)
				{
					vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				}
				else
				{
					vd.Id = 0;
				}
				if (vd.Id > 0)
				{
					vd.ActionTypeId = 2;
				}
				else
				{
					vd.ActionTypeId = 1;
				}
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.ItemUOMId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
				vd.AssetId = Conversion.ToInt(r.Cells["AssetId"].Value);
				vd.RequestedQty = Conversion.ToDouble(r.Cells["RequestedQty"].Value);
				vd.ItemConditionId = Conversion.ToInt(r.Cells["ItemConditionId"].Value);
				deprt.DepartmentRequestDetail.Add(vd);
			}
			if (RecId > 0 && lstRemoveRecord.Count > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					deprt.DepartmentRequestDetail.Add(lstRemoveRecord[i]);
				}
			}
			deprt.AttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			deprt.DeleteAttachmentsList = new List<global::Architecture.Model.DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<DepartmentRequest>(deprt, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			int success = DepartmentRequest.Save(deprt);
			if (RecId > 0)
			{
				MessageBox.Show("Record Update Successfully " + deprt.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 450, base.Name);
			}
			else
			{
				MessageBox.Show("Record Save Successfully " + deprt.DocNo);
			}
			Reset();
			if (ChkBok.Checked)
			{
				CommonServices.DepartmentSlip451(success);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnSave_Click(object sender, EventArgs e)
	{
		try
		{
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnUpdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId == 0)
			{
				throw new Exception("RecId Not Found");
			}
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			RecId = Conversion.ToInt((object)ID);
			DepartmentRequest deprt = DepartmentRequest.GetByID(RecId);
			if (deprt != null)
			{
				btnSave.Visible = false;
				btnUpdate.Visible = true;
				btnDelete.Visible = true;
				UpdateMode = true;
				tabControl1.SelectedIndex = 0;
				txtdocno.Text = deprt.DocNo.ToString();
				DocDate.Value = deprt.DocDate;
				cmbDepartmentFrom.Value = deprt.FromDepartmentId;
				cmbDepartmentTo.Value = deprt.ToDepartmentId;
				txtremarks.Text = deprt.RemarksHeader;
				IsApproved = deprt.IsApproved;
				dtdetail.Rows.Clear();
				for (int i = 0; i < deprt.DepartmentRequestDetail.Count; i++)
				{
					dtdetail.Rows.Add(deprt.DepartmentRequestDetail[i].Id, deprt.DepartmentRequestDetail[i].ItemId, deprt.DepartmentRequestDetail[i].ItemName, deprt.DepartmentRequestDetail[i].ItemCode, deprt.DepartmentRequestDetail[i].ItemConditionId, deprt.DepartmentRequestDetail[i].ItemCondition, deprt.DepartmentRequestDetail[i].ItemUOMId, deprt.DepartmentRequestDetail[i].UOMCode, deprt.DepartmentRequestDetail[i].AssetId, deprt.DepartmentRequestDetail[i].AssetName, deprt.DepartmentRequestDetail[i].RequestedQty);
				}
				grdDetail.DataSource = dtdetail;
				grdDetail.RetrieveStructure();
				grdSettings();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				return;
			}
			Reset();
			throw new Exception("Record not Found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					DepartmentRequest.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Seccessfully");
					Reset();
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

	private void HistoryGridFill()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.DocumentTypeId = 450;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.ApprovedFilter = "All";
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
			obj.FromDocNo = Conversion.ToInt((object)txtFromDocNoHistory.Text);
			obj.ToDocNo = Conversion.ToInt((object)txtToDocNoHistory.Text);
			DataTable dtHistry = DepartmentRequest.FormHistory(obj);
			if (dtHistry.Rows.Count <= 0)
			{
				return;
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("DocTypeId", typeof(int));
			dt.Columns.Add("DocDate", typeof(DateTime));
			dt.Columns.Add("DocNo", typeof(int));
			dt.Columns.Add("DepartmentNameFrom", typeof(string));
			dt.Columns.Add("ToDepartmentName", typeof(string));
			dt.Columns.Add("RemarksHeader", typeof(string));
			dt.Columns.Add("EntryUser", typeof(string));
			dt.Columns.Add("EntryDate", typeof(DateTime));
			dt.Columns.Add("ModifyUser", typeof(string));
			dt.Columns.Add("ModifyDate", typeof(DateTime));
			dt.Columns.Add("NoOfAttachments", typeof(int));
			dt.Clear();
			int i;
			for (i = 0; i < dtHistry.Rows.Count; i++)
			{
				if (!dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Conversion.ToInt(dtHistry.Rows[i]["Id"])))
				{
					dt.Rows.Add(Conversion.ToInt(dtHistry.Rows[i]["Id"]), Conversion.ToInt(dtHistry.Rows[i]["DocTypeId"]), Conversion.ToDateTime(dtHistry.Rows[i]["DocDate"]), Conversion.ToString(dtHistry.Rows[i]["DocNo"]), Conversion.ToString(dtHistry.Rows[i]["FromDepartmentName"]), Conversion.ToString(dtHistry.Rows[i]["ToDepartmentName"]), Conversion.ToString(dtHistry.Rows[i]["RemarksHeader"]), Conversion.ToString(dtHistry.Rows[i]["CreatedBy"]), dtHistry.Rows[i]["EntryDate"], Conversion.ToString(dtHistry.Rows[i]["ModifiedBy"]), dtHistry.Rows[i]["ModifyDate"], Conversion.ToInt(dtHistry.Rows[i]["NoOfAttachments"]));
				}
			}
			DataGridHistory.DataSource = dt;
			DataGridHistory.RetrieveStructure();
			HistoryGridSettings();
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
			DataGridHistory.RootTable.Columns["DocTypeId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistory, 2, 3, null, null, null, ColumnSetting: false);
			DataGridHistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			DataGridHistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			DataGridHistory.RootTable.Columns["DepartmentNameFrom"].Width = Constants.InventoryConstants.WareHouseName;
			DataGridHistory.RootTable.Columns["ToDepartmentName"].Width = Constants.InventoryConstants.WareHouseName;
			DataGridHistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants + 50;
			DataGridHistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants + 20;
			DataGridHistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants + 20;
			DataGridHistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM + 10;
			DataGridHistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants - 10;
			DataGridHistory.RootTable.Columns["EntryDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MM-yyyy hh:mm tt";
			DataGridHistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			if (formrights.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns.Add("Print");
				DataGridHistory.RootTable.Columns["Print"].Caption = "Print";
				DataGridHistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Print"].ButtonText = "Print";
				DataGridHistory.RootTable.Columns["Print"].Width = 40;
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.FrozenColumns = 1;
			}
			if (formrights.DoHaveUpdateRights)
			{
				DataGridHistory.RootTable.Columns.Add("Edit");
				DataGridHistory.RootTable.Columns["Edit"].Caption = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
				DataGridHistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
				DataGridHistory.RootTable.Columns["Edit"].ButtonText = "Edit";
				DataGridHistory.RootTable.Columns["Edit"].Width = 40;
				DataGridHistory.RootTable.Columns["Edit"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			if (formrights.DoHaveUpdateRights && formrights.DoHavePrintRights)
			{
				DataGridHistory.RootTable.Columns["Print"].Position = 0;
				DataGridHistory.RootTable.Columns["Edit"].Position = 1;
				DataGridHistory.FrozenColumns = 2;
			}
			ctrlGrdBar2_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		try
		{
			if (tabControl1.SelectedIndex == 1)
			{
				FromDateHistory.Focus();
			}
			else
			{
				DocDate.Focus();
			}
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
			if (formrights.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value);
				ReadById(RecId);
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
			if (e.Column.Key == "Edit" && formrights.DoHaveUpdateRights)
			{
				RecId = Conversion.ToInt((object)DataGridHistory.CurrentRow.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
			if (e.Column.Key == "Print" && formrights.DoHavePrintRights)
			{
				CommonServices.DepartmentSlip451(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void DataGridHistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		try
		{
			if (Conversion.ToInt(DataGridHistory.CurrentRow.Cells["NoOfAttachments"].Value) > 0)
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(DataGridHistory.CurrentRow.Cells["Id"].Value), Conversion.ToInt(DataGridHistory.CurrentRow.Cells["DocTypeId"].Value));
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void toolStripButton1_Click(object sender, EventArgs e)
	{
	}

	private void DataGridHistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		try
		{
			GridEXRow gr = DataGridHistory.CurrentRow;
			if (gr != null && (int)gr.RowType == 0 && gr.Cells.Count > 0)
			{
				DepartmentRequest obj = new DepartmentRequest();
				obj = DepartmentRequest.GetByID(Conversion.ToInt(gr.Cells["Id"].Value));
				DataTable dt = new DataTable();
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("ItemCode", typeof(string));
				dt.Columns.Add("ItemUOMId", typeof(int));
				dt.Columns.Add("UOM", typeof(string));
				dt.Columns.Add("ItemCondition", typeof(string));
				dt.Columns.Add("AssetId", typeof(int));
				dt.Columns.Add("AssetName", typeof(string));
				dt.Columns.Add("RequestedQty", typeof(double));
				dt.Clear();
				for (int i = 0; i < obj.DepartmentRequestDetail.Count; i++)
				{
					dt.Rows.Add(Conversion.ToInt((object)obj.DepartmentRequestDetail[i].ItemId), Conversion.ToString((object)obj.DepartmentRequestDetail[i].ItemName), Conversion.ToString((object)obj.DepartmentRequestDetail[i].ItemCode), Conversion.ToInt((object)obj.DepartmentRequestDetail[i].ItemUOMId), Conversion.ToString((object)obj.DepartmentRequestDetail[i].UOMCode), Conversion.ToString((object)obj.DepartmentRequestDetail[i].ItemCondition), Conversion.ToInt((object)obj.DepartmentRequestDetail[i].AssetId), Conversion.ToString((object)obj.DepartmentRequestDetail[i].AssetName), Conversion.ToDouble((object)obj.DepartmentRequestDetail[i].RequestedQty));
				}
				DataGridHistoryDetail.DataSource = dt;
				DataGridHistoryDetail.RetrieveStructure();
				DataGridHistoryDetailSetting();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void DataGridHistoryDetailSetting()
	{
		try
		{
			DataGridHistoryDetail.RootTable.Columns["ItemId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			DataGridHistoryDetail.RootTable.Columns["AssetId"].Visible = false;
			GridEX_Helper.GridWrappingAndColumnSettings(DataGridHistoryDetail, 2, 3, null, null, null, ColumnSetting: false);
			DataGridHistoryDetail.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName + 25;
			DataGridHistoryDetail.RootTable.Columns["ItemCode"].Width = Constants.InventoryConstants.ItemCode;
			DataGridHistoryDetail.RootTable.Columns["UOM"].Width = Constants.InventoryConstants.UomConstant;
			DataGridHistoryDetail.RootTable.Columns["AssetName"].Width = Constants.InventoryConstants.AccountTitle;
			DataGridHistoryDetail.RootTable.Columns["RequestedQty"].Width = Constants.InventoryConstants.QtyConstant + 10;
			DataGridHistoryDetail.RootTable.Columns["RequestedQty"].AggregateFunction = (AggregateFunction)2;
			DataGridHistoryDetail.RootTable.Columns["RequestedQty"].TextAlignment = (TextAlignment)3;
			DataGridHistoryDetail.RootTable.Columns["RequestedQty"].FormatString = "#,##0.##";
			DataGridHistoryDetail.RootTable.Columns["RequestedQty"].TotalFormatString = "#,##0.##";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void BtnNewHistory_Click(object sender, EventArgs e)
	{
		try
		{
			FromDateHistory.Value = DateTime.Now;
			ToDateHistory.Value = DateTime.Now;
			txtFromDocNoHistory.Text = string.Empty;
			txtToDocNoHistory.Text = string.Empty;
			DataGridHistory.ClearStructure();
			DataGridHistoryDetail.ClearStructure();
			drdocdate.Checked = true;
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
			HistoryGridFill();
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
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
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
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Expected O, but got Unknown
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			if (updateDetailIndex == -1)
			{
				if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
				{
					if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
					{
						DepartmentRequestDetail vd = new DepartmentRequestDetail();
						vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
						vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
						vd.ItemUOMId = Conversion.ToInt(r.Cells["ItemUOMId"].Value);
						vd.AssetId = Conversion.ToInt(r.Cells["AssetId"].Value);
						vd.RequestedQty = Conversion.ToDouble(r.Cells["RequestedQty"].Value);
						vd.ActionTypeId = 3;
						lstRemoveRecord.Add(vd);
						dtdetail.Rows[r.RowIndex].Delete();
						if (r != null && (int)r.RowType == 0)
						{
							r.Delete();
						}
						grdDetail.UpdateData();
					}
				}
				else
				{
					r.Delete();
				}
				return;
			}
			throw new Exception("Please Reset the Detail first...");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void Add_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)cmbItem).ActiveRow == null || Conversion.ToInt(cmbItem.Value) == 0)
			{
				MessageBox.Show("Item Field is Required");
				cmbItem.Focus();
				return;
			}
			if (((UltraGridBase)cmbItemUOM).ActiveRow == null || Conversion.ToInt(cmbItemUOM.Value) == 0)
			{
				MessageBox.Show("UOM Field is Required");
				cmbItemUOM.Focus();
				return;
			}
			if (txtRequestedQty.Text.Trim() == "")
			{
				MessageBox.Show("RequestedQty Field is Required");
				txtRequestedQty.Focus();
				return;
			}
			if (((UltraGridBase)CmbAssetName).ActiveRow == null || Conversion.ToInt(CmbAssetName.Value) == 0)
			{
				MessageBox.Show("AssetName Field is Required");
				CmbAssetName.Focus();
				return;
			}
			if (((UltraGridBase)CmbItemCondition).ActiveRow == null || Conversion.ToInt(CmbItemCondition.Value) == 0)
			{
				MessageBox.Show("Item Condition Field is Required");
				CmbItemCondition.Focus();
				return;
			}
			GridEXRow[] rows = grdDetail.GetRows();
			for (int i = 0; i < rows.Length; i++)
			{
				int ItemId = Conversion.ToInt(rows[i].Cells["ItemId"].Value);
				if (Conversion.ToInt(cmbItem.Value) == ItemId)
				{
					MessageBox.Show("Duplicate Item Not Add in Grid");
					return;
				}
			}
			dtdetail.Rows.Add(0, cmbItem.Value, ((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Text, ((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value, CmbItemCondition.Value, ((Control)(object)CmbItemCondition).Text, cmbItemUOM.Value, ((Control)(object)cmbItemUOM).Text, CmbAssetName.Value, ((Control)(object)CmbAssetName).Text, txtRequestedQty.Text);
			grdDetail.DataSource = dtdetail;
			grdDetail.RetrieveStructure();
			grdSettings();
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void grdSettings()
	{
		try
		{
			grdDetail.RootTable.Columns["Id"].Visible = false;
			grdDetail.RootTable.Columns["ItemId"].Visible = false;
			grdDetail.RootTable.Columns["ItemUOMId"].Visible = false;
			grdDetail.RootTable.Columns["AssetId"].Visible = false;
			grdDetail.RootTable.Columns["ItemConditionId"].Visible = false;
			CommonServices.GridAutoAdjustmentNew(grdDetail);
			grdDetail.RootTable.Columns.Add("Delete");
			grdDetail.RootTable.Columns["Delete"].Caption = "X";
			grdDetail.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Delete"].ButtonText = "X";
			grdDetail.RootTable.Columns["Delete"].Width = 20;
			grdDetail.RootTable.Columns["Delete"].Position = 0;
			grdDetail.RootTable.Columns.Add("Edit");
			grdDetail.RootTable.Columns["Edit"].Caption = "Edit";
			grdDetail.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdDetail.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdDetail.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdDetail.RootTable.Columns["Edit"].Width = 40;
			grdDetail.RootTable.Columns["Edit"].Position = 1;
			grdDetail.FrozenColumns = 2;
			ctrlGrdBar1_Load(null, null);
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
			if (((UltraGridBase)cmbItem).ActiveRow == null)
			{
				MessageBox.Show("Item Field is Required");
				cmbItem.Focus();
				return;
			}
			if (((UltraGridBase)cmbItemUOM).ActiveRow == null)
			{
				MessageBox.Show("UOM Field is Required");
				cmbItemUOM.Focus();
				return;
			}
			if (txtRequestedQty.Text.Trim() == "")
			{
				MessageBox.Show("RequestedQty Field is Required");
				txtRequestedQty.Focus();
				return;
			}
			if (((UltraGridBase)CmbAssetName).ActiveRow == null)
			{
				MessageBox.Show("AssetName Field is Required");
				CmbAssetName.Focus();
				return;
			}
			if (((UltraGridBase)CmbItemCondition).ActiveRow == null || Conversion.ToInt(CmbItemCondition.Value) == 0)
			{
				MessageBox.Show("Item Condition Field is Required");
				CmbItemCondition.Focus();
				return;
			}
			GridEXRow[] rows = grdDetail.GetRows();
			foreach (GridEXRow item in rows)
			{
				int ItemId = Conversion.ToInt(item.Cells["ItemId"].Value);
				if (Conversion.ToInt(cmbItem.Value) == ItemId && item.RowIndex != updateDetailIndex)
				{
					MessageBox.Show("Duplicate Item Not Add in Grid");
					return;
				}
			}
			dtdetail.Rows[updateDetailIndex]["ItemId"] = cmbItem.Value;
			dtdetail.Rows[updateDetailIndex]["Item"] = ((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemName"].Text;
			dtdetail.Rows[updateDetailIndex]["ItemCode"] = ((UltraDropDownBase)cmbItem).SelectedRow.Cells["ItemCode"].Value;
			dtdetail.Rows[updateDetailIndex]["ItemConditionId"] = CmbItemCondition.Value;
			dtdetail.Rows[updateDetailIndex]["ItemCondition"] = ((Control)(object)CmbItemCondition).Text;
			dtdetail.Rows[updateDetailIndex]["ItemUOMId"] = cmbItemUOM.Value;
			dtdetail.Rows[updateDetailIndex]["UOM"] = ((Control)(object)cmbItemUOM).Text;
			dtdetail.Rows[updateDetailIndex]["AssetId"] = CmbAssetName.Value;
			dtdetail.Rows[updateDetailIndex]["AssetName"] = ((Control)(object)CmbAssetName).Text;
			dtdetail.Rows[updateDetailIndex]["RequestedQty"] = txtRequestedQty.Text.Trim();
			Add.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdDetail_DoubleClick(object sender, EventArgs e)
	{
		try
		{
			GridEXRow item = grdDetail.CurrentRow;
			if (item != null)
			{
				updateDetailIndex = item.RowIndex;
				cmbItem.Value = item.Cells["ItemId"].Value.ToString();
				ItemId = Conversion.ToInt((object)item.Cells["ItemId"].Value.ToString());
				((Control)(object)cmbItem).Text = item.Cells["Item"].Text.ToString();
				ItemUOMFill();
				((Control)(object)cmbItemUOM).Text = item.Cells["UOM"].Text.ToString();
				CmbItemCondition.Value = Conversion.ToInt(item.Cells["ItemConditionId"].Value);
				((Control)(object)CmbAssetName).Text = item.Cells["AssetName"].Text.ToString();
				txtRequestedQty.Text = item.Cells["RequestedQty"].Value.ToString();
				cmbItem.Focus();
				Add.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
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
			ResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void cmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemUOMFill();
			BalanceStock();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BalanceStock()
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Expected O, but got Unknown
		try
		{
			lblStockQty.Text = "0";
			int ItemId = Conversion.ToInt(cmbItem.Value);
			if (ItemId != 0)
			{
				int ItemConditionId = Conversion.ToInt(CmbItemCondition.Value);
				DataTable dt = GetAvgRatesAndStockInHand.GetAvgRateQtyAndStockInHand(new ReportsParameters
				{
					OrganizationId = clsGlobalVariables.UserAccount.OrganizationId,
					CompanyId = clsGlobalVariables.UserAccount.CompanyId,
					ItemId = ItemId,
					DocDate = Conversion.ToDateTime((object)DocDate.Value),
					ItemConditionId = ItemConditionId
				});
				if (dt.Rows.Count > 0 && dt != null)
				{
					double ItemQty = Math.Round(Conversion.ToDouble(dt.Rows[0]["QtyInHand"]), 2);
					lblStockQty.Text = Conversion.ToString((object)ItemQty);
					lblStockQty.Visible = true;
				}
			}
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

	private void printToolStripButton_Click(object sender, EventArgs e)
	{
		try
		{
			CommonServices.DepartmentSlip451(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
				cmbItemUOM.Focus();
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
		try
		{
			AT.Show();
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtRequestedQty_KeyPress(object sender, KeyPressEventArgs e)
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

	private void EccountBook_KeyDown(object sender, KeyEventArgs e)
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
				}
				else
				{
					tabControl1.SelectedIndex = 1;
				}
			}
			if ((e.Control && e.KeyCode == Keys.E) || e.KeyCode == Keys.Escape)
			{
				Close();
			}
			if (e.Control && e.Alt)
			{
				BtnShortCutkeys_Click(null, null);
			}
			if (tabControl1.SelectedIndex == 0)
			{
				if (e.Control && e.KeyCode == Keys.S && btnSave.Visible && btnSave.Enabled)
				{
					btnSave_Click(null, null);
				}
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && btnDelete.Visible && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnNew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnUpdate.Visible && btnUpdate.Enabled && UpdateMode)
				{
					btnUpdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P)
				{
					printToolStripButton_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					cmbItem.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grdDetail).Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					btnAttachment_Click(null, null);
				}
			}
			else
			{
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					BtnNewHistory_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Up)
				{
					FromDateHistory.Focus();
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)DataGridHistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On Item Combo in Detail Box");
			dt.Rows.Add("Ctrl+Enter", "For Update Record When Focus On Any Grid ");
			dt.Rows.Add("Ctrl+Space", "To Call Function's On Button Or Link When Focus On Any Grid ");
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
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = DataGridHistory.CurrentRow;
			if (r == null || (int)r.RowType != 0 || DataGridHistory.CurrentColumn == null)
			{
				return;
			}
			int Id = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (DataGridHistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					ReadById(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "Print" && formrights.DoHavePrintRights)
				{
					CommonServices.DepartmentSlip451(Id);
				}
				if (DataGridHistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Id, Conversion.ToInt(DataGridHistory.CurrentRow.Cells["DocTypeId"].Value));
				}
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				ReadById(Id);
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
				if (grdDetail.CurrentColumn.Key == "Edit")
				{
					grdDetail_DoubleClick(null, null);
				}
			}
			else if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(r);
			}
			if (e.Control && e.KeyCode == Keys.Return)
			{
				grdDetail_DoubleClick(null, null);
			}
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
			BalanceStock();
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
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Expected O, but got Unknown
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Expected O, but got Unknown
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Expected O, but got Unknown
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_045b: Expected O, but got Unknown
		//IL_04bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Expected O, but got Unknown
		//IL_04d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Expected O, but got Unknown
		//IL_0517: Unknown result type (might be due to invalid IL or missing references)
		//IL_0521: Expected O, but got Unknown
		//IL_052d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0537: Expected O, but got Unknown
		//IL_0842: Unknown result type (might be due to invalid IL or missing references)
		//IL_084c: Expected O, but got Unknown
		//IL_0870: Unknown result type (might be due to invalid IL or missing references)
		//IL_087a: Expected O, but got Unknown
		//IL_31aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b4: Expected O, but got Unknown
		GridEXLayout DataGridHistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout DataGridHistoryDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance34 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		GridEXLayout grdDetail_DesignTimeLayout = new GridEXLayout();
		Appearance appearance43 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance24 = new Appearance();
		this.label14 = new System.Windows.Forms.Label();
		this.DataGridHistory = new GridEX();
		this.panel9 = new System.Windows.Forms.Panel();
		this.DataGridHistoryDetail = new GridEX();
		this.panel8 = new System.Windows.Forms.Panel();
		this.btnLoadall = new System.Windows.Forms.ToolStrip();
		this.BtnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.panel11 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.rdapproveddate = new System.Windows.Forms.RadioButton();
		this.rdmodifydate = new System.Windows.Forms.RadioButton();
		this.drdocdate = new System.Windows.Forms.RadioButton();
		this.rdentrydate = new System.Windows.Forms.RadioButton();
		this.btnshow = new System.Windows.Forms.Button();
		this.label22 = new System.Windows.Forms.Label();
		this.label43 = new System.Windows.Forms.Label();
		this.label62 = new System.Windows.Forms.Label();
		this.label63 = new System.Windows.Forms.Label();
		this.txtToDocNoHistory = new System.Windows.Forms.TextBox();
		this.txtFromDocNoHistory = new System.Windows.Forms.TextBox();
		this.ToDateHistory = new System.Windows.Forms.DateTimePicker();
		this.FromDateHistory = new System.Windows.Forms.DateTimePicker();
		this.panel7 = new System.Windows.Forms.Panel();
		this.label8 = new System.Windows.Forms.Label();
		this.ctrlGrdBar2 = new grid.User_Conrtols.CtrlGrdBar();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnNew = new System.Windows.Forms.ToolStripButton();
		this.btnUpdate = new System.Windows.Forms.ToolStripButton();
		this.btnSave = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.btnPrint451 = new System.Windows.Forms.ToolStripButton();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.BtnShortCutkeys = new System.Windows.Forms.ToolStripButton();
		this.panel2 = new System.Windows.Forms.Panel();
		this.label23 = new System.Windows.Forms.Label();
		this.panel3 = new System.Windows.Forms.Panel();
		this.ChkBok = new System.Windows.Forms.CheckBox();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.cmbItemUOM = new UltraCombo();
		this.Add = new System.Windows.Forms.Button();
		this.cmbItem = new UltraCombo();
		this.txtRequestedQty = new System.Windows.Forms.TextBox();
		this.label35 = new System.Windows.Forms.Label();
		this.label36 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel10 = new System.Windows.Forms.Panel();
		this.grdDetail = new GridEX();
		this.panel5 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar1 = new grid.User_Conrtols.CtrlGrdBar();
		this.label7 = new System.Windows.Forms.Label();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.txtremarks = new System.Windows.Forms.TextBox();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.label1 = new System.Windows.Forms.Label();
		this.cmbDepartmentFrom = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.cmbDepartmentTo = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.label17 = new System.Windows.Forms.Label();
		this.BtnDepartmentDefine = new System.Windows.Forms.Button();
		this.label3 = new System.Windows.Forms.Label();
		this.groupBox2 = new System.Windows.Forms.GroupBox();
		this.CmbItemCondition = new UltraCombo();
		this.label25 = new System.Windows.Forms.Label();
		this.CmbAssetName = new UltraCombo();
		this.label4 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.txtBarcodeReader = new System.Windows.Forms.TextBox();
		this.lblStockQty = new System.Windows.Forms.Label();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).BeginInit();
		this.panel8.SuspendLayout();
		this.btnLoadall.SuspendLayout();
		this.panel11.SuspendLayout();
		this.groupBox3.SuspendLayout();
		this.panel7.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.panel2.SuspendLayout();
		this.panel3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).BeginInit();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel10.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdDetail).BeginInit();
		this.panel5.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmentFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmentTo).BeginInit();
		this.groupBox2.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetName).BeginInit();
		base.SuspendLayout();
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label14.ForeColor = System.Drawing.Color.White;
		this.label14.Location = new System.Drawing.Point(4, 2);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(211, 17);
		this.label14.TabIndex = 0;
		this.label14.Text = "Detail Of Above Selected Row";
		this.DataGridHistory.AllowEdit = (InheritableBoolean)2;
		DataGridHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistory.DesignTimeLayout = DataGridHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.DataGridHistory.DynamicFiltering = true;
		this.DataGridHistory.FilterMode = (FilterMode)2;
		this.DataGridHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Location = new System.Drawing.Point(2, 134);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Name = "DataGridHistory";
		((ScrollableBase)this.DataGridHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistory).Size = new System.Drawing.Size(1173, 189);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).TabIndex = 44;
		this.DataGridHistory.TotalRow = (InheritableBoolean)1;
		this.DataGridHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistory).VisualStyle = (VisualStyle)4;
		this.DataGridHistory.ColumnButtonClick += new ColumnActionEventHandler(DataGridHistory_ColumnButtonClick);
		this.DataGridHistory.SelectionChanged += new System.EventHandler(DataGridHistory_SelectionChanged);
		this.DataGridHistory.LinkClicked += new ColumnActionEventHandler(DataGridHistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).DoubleClick += new System.EventHandler(DataGridHistory_DoubleClick);
		((System.Windows.Forms.Control)(object)this.DataGridHistory).KeyDown += new System.Windows.Forms.KeyEventHandler(DataGridHistory_KeyDown);
		this.panel9.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel9.Controls.Add(this.label14);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel9.Location = new System.Drawing.Point(2, 323);
		this.panel9.Margin = new System.Windows.Forms.Padding(2);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1173, 22);
		this.panel9.TabIndex = 23;
		this.DataGridHistoryDetail.AcceptsEscape = false;
		this.DataGridHistoryDetail.AllowEdit = (InheritableBoolean)2;
		DataGridHistoryDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.DataGridHistoryDetail.DesignTimeLayout = DataGridHistoryDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.DataGridHistoryDetail.DynamicFiltering = true;
		this.DataGridHistoryDetail.FilterMode = (FilterMode)2;
		this.DataGridHistoryDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.DataGridHistoryDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Location = new System.Drawing.Point(2, 345);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Name = "DataGridHistoryDetail";
		((ScrollableBase)this.DataGridHistoryDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).Size = new System.Drawing.Size(1173, 206);
		((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail).TabIndex = 45;
		this.DataGridHistoryDetail.TotalRow = (InheritableBoolean)1;
		this.DataGridHistoryDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.DataGridHistoryDetail).VisualStyle = (VisualStyle)4;
		this.panel8.BackColor = System.Drawing.Color.Teal;
		this.panel8.Controls.Add(this.btnLoadall);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(2, 2);
		this.panel8.Margin = new System.Windows.Forms.Padding(2);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1173, 55);
		this.panel8.TabIndex = 1;
		this.btnLoadall.BackColor = System.Drawing.Color.White;
		this.btnLoadall.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLoadall.ImageScalingSize = new System.Drawing.Size(24, 24);
		this.btnLoadall.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.BtnNewHistory });
		this.btnLoadall.Location = new System.Drawing.Point(0, 0);
		this.btnLoadall.Name = "btnLoadall";
		this.btnLoadall.Size = new System.Drawing.Size(1173, 31);
		this.btnLoadall.TabIndex = 45;
		this.btnLoadall.Text = "toolStrip2";
		this.BtnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.BtnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnNewHistory.Name = "BtnNewHistory";
		this.BtnNewHistory.Size = new System.Drawing.Size(63, 28);
		this.BtnNewHistory.Text = "New";
		this.BtnNewHistory.Click += new System.EventHandler(BtnNewHistory_Click);
		this.panel11.Controls.Add(this.groupBox3);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(2, 57);
		this.panel11.Margin = new System.Windows.Forms.Padding(2);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1173, 51);
		this.panel11.TabIndex = 47;
		this.groupBox3.Controls.Add(this.rdapproveddate);
		this.groupBox3.Controls.Add(this.rdmodifydate);
		this.groupBox3.Controls.Add(this.drdocdate);
		this.groupBox3.Controls.Add(this.rdentrydate);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label22);
		this.groupBox3.Controls.Add(this.label43);
		this.groupBox3.Controls.Add(this.label62);
		this.groupBox3.Controls.Add(this.label63);
		this.groupBox3.Controls.Add(this.txtToDocNoHistory);
		this.groupBox3.Controls.Add(this.txtFromDocNoHistory);
		this.groupBox3.Controls.Add(this.ToDateHistory);
		this.groupBox3.Controls.Add(this.FromDateHistory);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(3, -2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(948, 51);
		this.groupBox3.TabIndex = 1;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Filters";
		this.rdapproveddate.AutoSize = true;
		this.rdapproveddate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdapproveddate.Location = new System.Drawing.Point(844, 31);
		this.rdapproveddate.Name = "rdapproveddate";
		this.rdapproveddate.Size = new System.Drawing.Size(105, 19);
		this.rdapproveddate.TabIndex = 9;
		this.rdapproveddate.Text = "Approved Date";
		this.rdapproveddate.UseVisualStyleBackColor = true;
		this.rdmodifydate.AutoSize = true;
		this.rdmodifydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdmodifydate.Location = new System.Drawing.Point(844, 12);
		this.rdmodifydate.Name = "rdmodifydate";
		this.rdmodifydate.Size = new System.Drawing.Size(91, 19);
		this.rdmodifydate.TabIndex = 8;
		this.rdmodifydate.Text = "Modify Date";
		this.rdmodifydate.UseVisualStyleBackColor = true;
		this.drdocdate.AutoSize = true;
		this.drdocdate.Checked = true;
		this.drdocdate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.drdocdate.Location = new System.Drawing.Point(763, 12);
		this.drdocdate.Name = "drdocdate";
		this.drdocdate.Size = new System.Drawing.Size(75, 19);
		this.drdocdate.TabIndex = 6;
		this.drdocdate.TabStop = true;
		this.drdocdate.Text = "Doc Date";
		this.drdocdate.UseVisualStyleBackColor = true;
		this.rdentrydate.AutoSize = true;
		this.rdentrydate.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdentrydate.Location = new System.Drawing.Point(763, 31);
		this.rdentrydate.Name = "rdentrydate";
		this.rdentrydate.Size = new System.Drawing.Size(80, 19);
		this.rdentrydate.TabIndex = 7;
		this.rdentrydate.Text = "Entry Date";
		this.rdentrydate.UseVisualStyleBackColor = true;
		this.btnshow.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(689, 18);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(60, 23);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(545, 22);
		this.label22.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(64, 15);
		this.label22.TabIndex = 141;
		this.label22.Text = "To Doc No";
		this.label43.AutoSize = true;
		this.label43.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label43.Location = new System.Drawing.Point(390, 22);
		this.label43.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label43.Name = "label43";
		this.label43.Size = new System.Drawing.Size(79, 15);
		this.label43.TabIndex = 140;
		this.label43.Text = "From Doc No";
		this.label62.AutoSize = true;
		this.label62.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label62.Location = new System.Drawing.Point(207, 22);
		this.label62.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label62.Name = "label62";
		this.label62.Size = new System.Drawing.Size(48, 15);
		this.label62.TabIndex = 139;
		this.label62.Text = "To Date";
		this.label63.AutoSize = true;
		this.label63.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label63.Location = new System.Drawing.Point(4, 22);
		this.label63.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
		this.label63.Name = "label63";
		this.label63.Size = new System.Drawing.Size(63, 15);
		this.label63.TabIndex = 13;
		this.label63.Text = "From Date";
		this.txtToDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtToDocNoHistory.Location = new System.Drawing.Point(614, 18);
		this.txtToDocNoHistory.Name = "txtToDocNoHistory";
		this.txtToDocNoHistory.ShortcutsEnabled = false;
		this.txtToDocNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtToDocNoHistory.TabIndex = 3;
		this.txtToDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtToDocNoHistory_KeyPress);
		this.txtFromDocNoHistory.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.txtFromDocNoHistory.Location = new System.Drawing.Point(469, 18);
		this.txtFromDocNoHistory.Name = "txtFromDocNoHistory";
		this.txtFromDocNoHistory.ShortcutsEnabled = false;
		this.txtFromDocNoHistory.Size = new System.Drawing.Size(72, 23);
		this.txtFromDocNoHistory.TabIndex = 2;
		this.txtFromDocNoHistory.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtFromDocNoHistory_KeyPress);
		this.ToDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.CustomFormat = "dd-MMM-yy";
		this.ToDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDateHistory.Location = new System.Drawing.Point(256, 18);
		this.ToDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.ToDateHistory.Name = "ToDateHistory";
		this.ToDateHistory.ShowCheckBox = true;
		this.ToDateHistory.Size = new System.Drawing.Size(131, 23);
		this.ToDateHistory.TabIndex = 1;
		this.FromDateHistory.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.CustomFormat = "dd-MMM-yy";
		this.FromDateHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.FromDateHistory.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.FromDateHistory.Location = new System.Drawing.Point(67, 18);
		this.FromDateHistory.Margin = new System.Windows.Forms.Padding(4);
		this.FromDateHistory.Name = "FromDateHistory";
		this.FromDateHistory.ShowCheckBox = true;
		this.FromDateHistory.Size = new System.Drawing.Size(137, 23);
		this.FromDateHistory.TabIndex = 0;
		this.panel7.BackColor = System.Drawing.Color.Teal;
		this.panel7.Controls.Add(this.label8);
		this.panel7.Controls.Add(this.ctrlGrdBar2);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel7.Location = new System.Drawing.Point(2, 108);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1173, 26);
		this.panel7.TabIndex = 46;
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label8.ForeColor = System.Drawing.Color.White;
		this.label8.Location = new System.Drawing.Point(3, 3);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(242, 19);
		this.label8.TabIndex = 23;
		this.label8.Text = "Department Request History";
		this.ctrlGrdBar2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar2.FormName = this;
		this.ctrlGrdBar2.Location = new System.Drawing.Point(1129, 1);
		this.ctrlGrdBar2.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar2.MyGrid = this.DataGridHistory;
		this.ctrlGrdBar2.Name = "ctrlGrdBar2";
		this.ctrlGrdBar2.Size = new System.Drawing.Size(42, 25);
		this.ctrlGrdBar2.TabIndex = 22;
		this.ctrlGrdBar2.Load += new System.EventHandler(ctrlGrdBar2_Load);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistory);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Controls.Add(this.panel11);
		this.tabPage2.Controls.Add(this.panel9);
		this.tabPage2.Controls.Add((System.Windows.Forms.Control)(object)this.DataGridHistoryDetail);
		this.tabPage2.Controls.Add(this.panel8);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1177, 553);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.btnRefresh.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "&Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[8] { this.btnNew, this.btnRefresh, this.btnUpdate, this.btnSave, this.btnDelete, this.btnPrint451, this.btnAttachment, this.BtnShortCutkeys });
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
		this.toolStrip1.Size = new System.Drawing.Size(974, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnNew.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNew.Name = "btnNew";
		this.btnNew.Size = new System.Drawing.Size(61, 24);
		this.btnNew.Text = "&New";
		this.btnNew.Click += new System.EventHandler(btnNew_Click);
		this.btnUpdate.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnUpdate.Name = "btnUpdate";
		this.btnUpdate.Size = new System.Drawing.Size(82, 24);
		this.btnUpdate.Text = "&Update";
		this.btnUpdate.Visible = false;
		this.btnUpdate.Click += new System.EventHandler(btnUpdate_Click);
		this.btnSave.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSave.Name = "btnSave";
		this.btnSave.Size = new System.Drawing.Size(66, 24);
		this.btnSave.Text = "&Save";
		this.btnSave.Click += new System.EventHandler(btnSave_Click);
		this.btnDelete.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(76, 24);
		this.btnDelete.Text = "&Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.btnPrint451.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPrint451.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnPrint451.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnPrint451.Name = "btnPrint451";
		this.btnPrint451.Size = new System.Drawing.Size(98, 24);
		this.btnPrint451.Text = "451-Print";
		this.btnPrint451.Click += new System.EventHandler(printToolStripButton_Click);
		this.btnAttachment.Font = new System.Drawing.Font("Verdana", 10f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(114, 24);
		this.btnAttachment.Text = "&Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.BtnShortCutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.BtnShortCutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.BtnShortCutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnShortCutkeys.Name = "BtnShortCutkeys";
		this.BtnShortCutkeys.Size = new System.Drawing.Size(126, 24);
		this.BtnShortCutkeys.Text = "ShortCut Keys";
		this.BtnShortCutkeys.Click += new System.EventHandler(BtnShortCutkeys_Click);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(974, 25);
		this.panel2.TabIndex = 0;
		this.label23.AutoSize = true;
		this.label23.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label23.ForeColor = System.Drawing.Color.White;
		this.label23.Location = new System.Drawing.Point(4, 3);
		this.label23.Name = "label23";
		this.label23.Size = new System.Drawing.Size(179, 19);
		this.label23.TabIndex = 0;
		this.label23.Text = "Department Request";
		this.panel3.BackColor = System.Drawing.Color.FromArgb(10, 110, 110);
		this.panel3.Controls.Add(this.ChkBok);
		this.panel3.Controls.Add(this.label23);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel3.Location = new System.Drawing.Point(2, 27);
		this.panel3.Margin = new System.Windows.Forms.Padding(2);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(974, 26);
		this.panel3.TabIndex = 0;
		this.ChkBok.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBok.AutoSize = true;
		this.ChkBok.Checked = true;
		this.ChkBok.CheckState = System.Windows.Forms.CheckState.Checked;
		this.ChkBok.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBok.ForeColor = System.Drawing.Color.White;
		this.ChkBok.Location = new System.Drawing.Point(887, 3);
		this.ChkBok.Margin = new System.Windows.Forms.Padding(2);
		this.ChkBok.Name = "ChkBok";
		this.ChkBok.Size = new System.Drawing.Size(84, 20);
		this.ChkBok.TabIndex = 7;
		this.ChkBok.Text = "Preview";
		this.ChkBok.UseVisualStyleBackColor = true;
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(893, 37);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(63, 25);
		this.btnCancelUpdateDetial.TabIndex = 9;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Visible = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(823, 37);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(69, 25);
		this.btnUpdateDetail.TabIndex = 8;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Visible = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.cmbItemUOM.AllowNull = (DefaultableBoolean)1;
		this.cmbItemUOM.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItemUOM.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItemUOM.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance34).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance35).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance35).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItemUOM).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItemUOM.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Location = new System.Drawing.Point(481, 36);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Name = "cmbItemUOM";
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).Size = new System.Drawing.Size(81, 26);
		((System.Windows.Forms.Control)(object)this.cmbItemUOM).TabIndex = 4;
		this.Add.BackColor = System.Drawing.Color.Teal;
		this.Add.FlatAppearance.BorderSize = 0;
		this.Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.Add.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.Add.ForeColor = System.Drawing.Color.White;
		this.Add.Location = new System.Drawing.Point(823, 37);
		this.Add.Margin = new System.Windows.Forms.Padding(2);
		this.Add.Name = "Add";
		this.Add.Size = new System.Drawing.Size(35, 25);
		this.Add.TabIndex = 7;
		this.Add.Text = "+";
		this.Add.UseVisualStyleBackColor = false;
		this.Add.Click += new System.EventHandler(Add_Click);
		this.cmbItem.AllowNull = (DefaultableBoolean)1;
		this.cmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance73).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance73;
		((UltraGridBase)this.cmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance74).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance74).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance74).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance74;
		((AppearanceBase)appearance75).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance75;
		((SpecialBoxBase)((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance76).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance76).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance76).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance76).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance77;
		((AppearanceBase)appearance78).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance78;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance79;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance80).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance82;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance83).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance84;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbItem).Location = new System.Drawing.Point(125, 36);
		((System.Windows.Forms.Control)(object)this.cmbItem).Name = "cmbItem";
		((System.Windows.Forms.Control)(object)this.cmbItem).Size = new System.Drawing.Size(222, 26);
		((System.Windows.Forms.Control)(object)this.cmbItem).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.cmbItem).Leave += new System.EventHandler(cmbItem_Leave);
		this.txtRequestedQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRequestedQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRequestedQty.Location = new System.Drawing.Point(738, 38);
		this.txtRequestedQty.Name = "txtRequestedQty";
		this.txtRequestedQty.Size = new System.Drawing.Size(84, 23);
		this.txtRequestedQty.TabIndex = 6;
		this.txtRequestedQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtRequestedQty_KeyPress);
		this.label35.AutoSize = true;
		this.label35.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label35.Location = new System.Drawing.Point(738, 17);
		this.label35.Name = "label35";
		this.label35.Size = new System.Drawing.Size(84, 15);
		this.label35.TabIndex = 25;
		this.label35.Text = "Requested Qty";
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label36.Location = new System.Drawing.Point(128, 17);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(32, 15);
		this.label36.TabIndex = 0;
		this.label36.Text = "Item";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label37.Location = new System.Drawing.Point(481, 17);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(35, 15);
		this.label37.TabIndex = 0;
		this.label37.Text = "UOM";
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(986, 582);
		this.panel1.TabIndex = 1;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(986, 582);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.BackColor = System.Drawing.Color.White;
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel3);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(978, 553);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.panel6.Controls.Add(this.panel10);
		this.panel6.Controls.Add(this.panel5);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 204);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(974, 347);
		this.panel6.TabIndex = 2;
		this.panel10.Controls.Add((System.Windows.Forms.Control)(object)this.grdDetail);
		this.panel10.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel10.Location = new System.Drawing.Point(0, 21);
		this.panel10.Margin = new System.Windows.Forms.Padding(2);
		this.panel10.Name = "panel10";
		this.panel10.Size = new System.Drawing.Size(974, 326);
		this.panel10.TabIndex = 2;
		this.grdDetail.AllowEdit = (InheritableBoolean)2;
		grdDetail_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdDetail.DesignTimeLayout = grdDetail_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdDetail).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdDetail.DynamicFiltering = true;
		this.grdDetail.FilterMode = (FilterMode)2;
		this.grdDetail.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdDetail).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdDetail.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdDetail).Location = new System.Drawing.Point(0, 0);
		((System.Windows.Forms.Control)(object)this.grdDetail).Name = "grdDetail";
		((ScrollableBase)this.grdDetail).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdDetail).Size = new System.Drawing.Size(974, 326);
		((System.Windows.Forms.Control)(object)this.grdDetail).TabIndex = 0;
		this.grdDetail.TotalRow = (InheritableBoolean)1;
		this.grdDetail.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdDetail).VisualStyle = (VisualStyle)4;
		this.grdDetail.ColumnButtonClick += new ColumnActionEventHandler(grdDetail_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).DoubleClick += new System.EventHandler(grdDetail_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grdDetail).KeyDown += new System.Windows.Forms.KeyEventHandler(grdDetail_KeyDown);
		this.panel5.BackColor = System.Drawing.Color.Teal;
		this.panel5.Controls.Add(this.ctrlGrdBar1);
		this.panel5.Controls.Add(this.label7);
		this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel5.Location = new System.Drawing.Point(0, 0);
		this.panel5.Margin = new System.Windows.Forms.Padding(2);
		this.panel5.Name = "panel5";
		this.panel5.Size = new System.Drawing.Size(974, 21);
		this.panel5.TabIndex = 1;
		this.ctrlGrdBar1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar1.FormName = this;
		this.ctrlGrdBar1.Location = new System.Drawing.Point(933, 1);
		this.ctrlGrdBar1.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar1.MyGrid = this.grdDetail;
		this.ctrlGrdBar1.Name = "ctrlGrdBar1";
		this.ctrlGrdBar1.Size = new System.Drawing.Size(38, 19);
		this.ctrlGrdBar1.TabIndex = 22;
		this.ctrlGrdBar1.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label7.ForeColor = System.Drawing.Color.White;
		this.label7.Location = new System.Drawing.Point(3, 2);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(50, 17);
		this.label7.TabIndex = 1;
		this.label7.Text = "Detail ";
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Controls.Add(this.groupBox2);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 53);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(974, 151);
		this.panel4.TabIndex = 1;
		this.groupBox1.Controls.Add(this.txtremarks);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartmentFrom);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.cmbDepartmentTo);
		this.groupBox1.Controls.Add(this.label5);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.BtnDepartmentDefine);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(5, 3);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(963, 77);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main Information";
		this.txtremarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtremarks.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtremarks.Location = new System.Drawing.Point(564, 16);
		this.txtremarks.Multiline = true;
		this.txtremarks.Name = "txtremarks";
		this.txtremarks.Size = new System.Drawing.Size(392, 53);
		this.txtremarks.TabIndex = 4;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtdocno.Location = new System.Drawing.Point(60, 18);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(125, 23);
		this.txtdocno.TabIndex = 1;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(60, 46);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(125, 23);
		this.DocDate.TabIndex = 2;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label1.Location = new System.Drawing.Point(187, 22);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(87, 15);
		this.label1.TabIndex = 5;
		this.label1.Text = "Department To";
		this.cmbDepartmentFrom.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartmentFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartmentFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartmentFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance43;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance44).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance44).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance44).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance45;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance46).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance46).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance46).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance46;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance47).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance47).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance47;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance48).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance48;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance49;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance54).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance54).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance54).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance54).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance54).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance54;
		((AppearanceBase)appearance55).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance55;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance56).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance56).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartmentFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartmentFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDepartmentFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartmentFrom).Location = new System.Drawing.Point(291, 44);
		((System.Windows.Forms.Control)(object)this.cmbDepartmentFrom).Name = "cmbDepartmentFrom";
		((System.Windows.Forms.Control)(object)this.cmbDepartmentFrom).Size = new System.Drawing.Size(220, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartmentFrom).TabIndex = 5;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.White;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label2.Location = new System.Drawing.Point(514, 22);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(52, 15);
		this.label2.TabIndex = 6;
		this.label2.Text = "Remarks";
		this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.cmbDepartmentTo.AllowNull = (DefaultableBoolean)1;
		this.cmbDepartmentTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbDepartmentTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbDepartmentTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance58).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance59).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance59).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance59).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance59).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance59;
		((AppearanceBase)appearance60).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance60;
		((SpecialBoxBase)((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance61).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance61).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance61).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance65).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance65;
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance66).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance68).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance68;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance69).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance69).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance69).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance69).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance69).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance69;
		((AppearanceBase)appearance70).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance71;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbDepartmentTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbDepartmentTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.cmbDepartmentTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbDepartmentTo).Location = new System.Drawing.Point(291, 16);
		((System.Windows.Forms.Control)(object)this.cmbDepartmentTo).Name = "cmbDepartmentTo";
		((System.Windows.Forms.Control)(object)this.cmbDepartmentTo).Size = new System.Drawing.Size(192, 26);
		((System.Windows.Forms.Control)(object)this.cmbDepartmentTo).TabIndex = 3;
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label5.Location = new System.Drawing.Point(5, 50);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(57, 15);
		this.label5.TabIndex = 3;
		this.label5.Text = "Doc Date";
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label17.Location = new System.Drawing.Point(5, 22);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(48, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No";
		this.BtnDepartmentDefine.BackColor = System.Drawing.Color.White;
		this.BtnDepartmentDefine.Font = new System.Drawing.Font("Verdana", 11.75f);
		this.BtnDepartmentDefine.Image = Architecture.WinApp.Properties.Resources.icons8_add_17;
		this.BtnDepartmentDefine.Location = new System.Drawing.Point(484, 16);
		this.BtnDepartmentDefine.Margin = new System.Windows.Forms.Padding(2);
		this.BtnDepartmentDefine.Name = "BtnDepartmentDefine";
		this.BtnDepartmentDefine.Size = new System.Drawing.Size(27, 27);
		this.BtnDepartmentDefine.TabIndex = 4;
		this.BtnDepartmentDefine.UseVisualStyleBackColor = false;
		this.BtnDepartmentDefine.Click += new System.EventHandler(button2_Click);
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label3.Location = new System.Drawing.Point(187, 50);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(102, 15);
		this.label3.TabIndex = 7;
		this.label3.Text = "Department From";
		this.groupBox2.Controls.Add(this.txtRequestedQty);
		this.groupBox2.Controls.Add(this.label35);
		this.groupBox2.Controls.Add(this.Add);
		this.groupBox2.Controls.Add(this.btnUpdateDetail);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItemCondition);
		this.groupBox2.Controls.Add(this.btnCancelUpdateDetial);
		this.groupBox2.Controls.Add(this.label25);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.CmbAssetName);
		this.groupBox2.Controls.Add(this.label4);
		this.groupBox2.Controls.Add(this.label6);
		this.groupBox2.Controls.Add(this.txtBarcodeReader);
		this.groupBox2.Controls.Add(this.lblStockQty);
		this.groupBox2.Controls.Add(this.rdbtnItemCode);
		this.groupBox2.Controls.Add(this.label36);
		this.groupBox2.Controls.Add(this.rdbtnItemName);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItemUOM);
		this.groupBox2.Controls.Add((System.Windows.Forms.Control)(object)this.cmbItem);
		this.groupBox2.Controls.Add(this.label37);
		this.groupBox2.Font = new System.Drawing.Font("Tahoma", 8.75f, System.Drawing.FontStyle.Bold);
		this.groupBox2.Location = new System.Drawing.Point(5, 80);
		this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
		this.groupBox2.Name = "groupBox2";
		this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
		this.groupBox2.Size = new System.Drawing.Size(963, 68);
		this.groupBox2.TabIndex = 1;
		this.groupBox2.TabStop = false;
		this.groupBox2.Text = "Detail Information";
		this.CmbItemCondition.AllowNull = (DefaultableBoolean)1;
		this.CmbItemCondition.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItemCondition.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItemCondition.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance37).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Appearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance38).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance38).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance38).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance38).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance38;
		((AppearanceBase)appearance39).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance39;
		((SpecialBoxBase)((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance40).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance40).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance40).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance41).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance42).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance51).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance52).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance52).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance52).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance52).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance62).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance63).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance63).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItemCondition).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItemCondition.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Location = new System.Drawing.Point(348, 36);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Name = "CmbItemCondition";
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Size = new System.Drawing.Size(132, 26);
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).TabIndex = 3;
		((System.Windows.Forms.Control)(object)this.CmbItemCondition).Leave += new System.EventHandler(CmbItemCondition_Leave);
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label25.Location = new System.Drawing.Point(348, 17);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(87, 15);
		this.label25.TabIndex = 108;
		this.label25.Text = "Item Condition";
		this.CmbAssetName.AllowNull = (DefaultableBoolean)1;
		this.CmbAssetName.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbAssetName.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbAssetName.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetName).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.CmbAssetName).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance21).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance21).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance21).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance21;
		((AppearanceBase)appearance22).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance22;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance23).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance24).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance24;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbAssetName).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbAssetName).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbAssetName.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbAssetName).Location = new System.Drawing.Point(563, 36);
		((System.Windows.Forms.Control)(object)this.CmbAssetName).Name = "CmbAssetName";
		((System.Windows.Forms.Control)(object)this.CmbAssetName).Size = new System.Drawing.Size(174, 26);
		((System.Windows.Forms.Control)(object)this.CmbAssetName).TabIndex = 5;
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(565, 17);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(67, 15);
		this.label4.TabIndex = 27;
		this.label4.Text = "AssetName";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(7, 17);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(76, 15);
		this.label6.TabIndex = 120;
		this.label6.Text = "Bar Code No.";
		this.txtBarcodeReader.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBarcodeReader.Location = new System.Drawing.Point(6, 38);
		this.txtBarcodeReader.Name = "txtBarcodeReader";
		this.txtBarcodeReader.Size = new System.Drawing.Size(117, 23);
		this.txtBarcodeReader.TabIndex = 0;
		this.txtBarcodeReader.KeyDown += new System.Windows.Forms.KeyEventHandler(txtBarcodeReader_KeyDown);
		this.lblStockQty.AutoSize = true;
		this.lblStockQty.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblStockQty.Location = new System.Drawing.Point(279, 17);
		this.lblStockQty.Name = "lblStockQty";
		this.lblStockQty.Size = new System.Drawing.Size(38, 15);
		this.lblStockQty.TabIndex = 121;
		this.lblStockQty.Text = "label7";
		this.lblStockQty.Visible = false;
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(219, 15);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 123;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(163, 15);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 122;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.ClientSize = new System.Drawing.Size(986, 582);
		base.Controls.Add(this.panel1);
		base.KeyPreview = true;
		base.Name = "frmDepartmentRequest";
		this.Text = "EccountBook";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmDepartmentRequest_FormClosing);
		base.Load += new System.EventHandler(EccountBook_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(EccountBook_KeyDown);
		((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.DataGridHistoryDetail).EndInit();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.btnLoadall.ResumeLayout(false);
		this.btnLoadall.PerformLayout();
		this.panel11.ResumeLayout(false);
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		this.panel7.ResumeLayout(false);
		this.panel7.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.panel3.ResumeLayout(false);
		this.panel3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbItemUOM).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbItem).EndInit();
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel10.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grdDetail).EndInit();
		this.panel5.ResumeLayout(false);
		this.panel5.PerformLayout();
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmentFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.cmbDepartmentTo).EndInit();
		this.groupBox2.ResumeLayout(false);
		this.groupBox2.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbItemCondition).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbAssetName).EndInit();
		base.ResumeLayout(false);
	}
}

}
