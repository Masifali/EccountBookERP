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
using Architecture.BLL.Inventory;
using Architecture.BLL.Mfg;
using Architecture.Common;
using Architecture.Model;
using Architecture.Model.Authentication;
using Architecture.Model.Mfg;
using Architecture.WinApp.Account_Definition;
using Architecture.WinApp.Common;
using Architecture.WinApp.Helper;
using Architecture.WinApp.Properties;
using Infragistics.Win;
using Infragistics.Win.UltraWinGrid;
using Janus.Data;
using Janus.Windows.GridEX;
using grid.User_Conrtols;

namespace Architecture.WinApp.Mfg;

public class frmStockTransferForProductionRejection : Form
{
	private Rightsobjects formrights = new Rightsobjects();

	private DropDownBind DDL = new DropDownBind();

	private Attachment AT = new Attachment();

	public DataTable dtViewAttachments = new DataTable();

	public DataTable dtAttachmentsUpload = new DataTable();

	private DataTable table = new DataTable();

	private DataTable dtWarehouse = new DataTable();

	private DataTable dtProductionStage = new DataTable();

	private DataTable dtWorkStation = new DataTable();

	private DataTable dtItem = new DataTable();

	private DataTable dtAvailableStockData = new DataTable();

	private DataTable dtSampleModal = new DataTable();

	private DataTable dtCastingType = new DataTable();

	private DataTable dtRejectionReasonType = new DataTable();

	private int RecId;

	private bool IsApproved;

	private bool FormInitialized;

	private string AttachmentsValues = "";

	private string CustomAttachmentsValues = "";

	private string ScreenName = "frmStockTransferForProductionRejection";

	public int DefaultDaysToLessFromHistoryFromDate;

	private int GridDataTransferTypeId;

	private int updateDetailIndex = -1;

	private frmLoadAvailableStockEngr LoadSo;

	private List<InvStockTransferDetail> lstRemoveRecord = new List<InvStockTransferDetail>();

	public UserAccount UserAccount = new UserAccount();

	private IContainer components;

	private Panel panel1;

	private TabControl tabControl1;

	private TabPage tabPage1;

	private Panel panel2;

	private TabPage tabPage2;

	private ToolStrip toolStrip1;

	private ToolStripButton btnnew;

	private ToolStripButton btnupdate;

	private ToolStripButton btnsave;

	private ToolStripButton btnprint;

	private Panel panel4;

	private DateTimePicker DocDate;

	private Label label17;

	private TextBox txtdocno;

	private Panel panel6;

	private Panel panel7;

	private CtrlGrdBar ctrlGrdBar1;

	private CtrlGrdBar ctrlGrdBar2;

	internal GridEX grdhistory;

	private ToolStripButton btnRefresh;

	internal GridEX grd;

	private Panel panel3;

	private CtrlGrdBar ctrlGrdBar3;

	private Panel panel9;

	private CtrlGrdBar ctrlGrdBar5;

	private Label label15;

	private CheckBox ChkBoxPrintPreview;

	private TabControl tabControl2;

	private TabPage tabPage3;

	private ToolStripButton btnAttachment;

	private Label label24;

	private Panel panel11;

	private GroupBox groupBox1;

	private Label label25;

	private ToolStripButton btnDelete;

	private ToolStripButton btnshortcutkeys;

	private ToolStrip toolStrip3;

	private ToolStripButton btnNewHistory;

	private Panel panel13;

	private Panel panel8;

	private Button btnshow;

	private DateTimePicker ToDate;

	private DateTimePicker fromdate;

	private Panel panel15;

	private ToolStripButton btnSaveAs;

	private TextBox txtDocNoTo;

	private TextBox txtDocNoFrom;

	private Label bg;

	private GroupBox groupBox3;

	internal UltraCombo cmbperemeter;

	private Label label20;

	private Label label34;

	private Label label37;

	private TextBox txtRemarksHead;

	internal GridEX GridDetailHistory;

	private Panel panel14;

	private Label label27;

	private ToolStripSeparator toolStripSeparator2;

	private ToolStripSeparator toolStripSeparator5;

	private ToolStripSeparator toolStripSeparator3;

	private ToolStripSeparator toolStripSeparator4;

	private ToolStripSeparator toolStripSeparator1;

	private Label label4;

	internal UltraCombo CmbWarehouseFrom;

	private Label label6;

	internal UltraCombo CmbTransferType;

	private Label label3;

	internal UltraCombo CmbProductionStageTo;

	private Label label2;

	internal UltraCombo CmbProductionStageFrom;

	private Label label1;

	internal UltraCombo CmbWarehouseTo;

	private Label label9;

	private Label label7;

	private Label label8;

	private Panel DetailManualEntryPanel;

	private RadioButton rdbtnItemCode;

	private RadioButton rdbtnItemName;

	private Button btnCancelUpdateDetial;

	private Button btnplus;

	private Label label22;

	private TextBox txtRemarksDetail;

	internal UltraCombo CmbItem;

	private Label label36;

	private Label label41;

	internal UltraCombo CmbPackUom;

	internal UltraCombo CmbWareHouseFromDetail;

	private Label label42;

	private Label label13;

	private TextBox txtTransferQty;

	private Button btnUpdateDetail;

	internal UltraCombo CmbCategory;

	private Label label52;

	internal UltraCombo CmbVariantDescription;

	private Label label5;

	private Label label18;

	internal UltraCombo CmbWorkStationFromDetail;

	private Label label10;

	internal UltraCombo CmbProductionStageToDetail;

	private Label label21;

	internal UltraCombo CmbProductionStageFromDetail;

	private Label label19;

	internal UltraCombo CmbWarehouseToDetail;

	private Label label11;

	private Label label16;

	private TextBox txtAvgRate;

	internal UltraCombo CmbRateUom;

	private Label label26;

	private Label label28;

	private TextBox txtAmount;

	internal UltraCombo CmbCastingType;

	private Label label77;

	internal UltraCombo txtBatchNo;

	internal UltraCombo CmbSampleModal;

	private Label label29;

	private ToolStripButton BtnLoadStock;

	private Label label30;

	private TextBox txtAvailableQty;

	internal UltraCombo CmbRejectionReasonType;

	private Label label12;

	private Label label31;

	private Label label14;

	private TextBox txtRejectionReasonRemarks;

	private TextBox txtTeamWorkerName;

	public frmStockTransferForProductionRejection(UserAccount user)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Expected O, but got Unknown
		InitializeComponent();
		InitializeComponentCustom();
		InitializeComponentMethod();
		UserAccount = user;
	}

	private void InitializeComponentCustom()
	{
		try
		{
			FormInitialized = true;
			AutoScroll = true;
			base.AutoScrollMargin = new Size(10, 10);
			base.AutoScrollMinSize = new Size(1100, 600);
			ScreenName = base.Name;
			InitializeDataTables();
			InitializeDefaultSettings();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void InitializeDataTables()
	{
		if (dtItem.Columns.Count == 0)
		{
			dtItem.Columns.AddRange(new DataColumn[7]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("ItemName"),
				new DataColumn("ItemCode"),
				new DataColumn("ItemCategoryId", typeof(int)),
				new DataColumn("itemModalId", typeof(int)),
				new DataColumn("SandWeight", typeof(double)),
				new DataColumn("DyeWeight", typeof(double))
			});
		}
		if (dtSampleModal.Columns.Count == 0)
		{
			dtSampleModal.Columns.AddRange(new DataColumn[5]
			{
				new DataColumn("Id", typeof(int)),
				new DataColumn("SampleNo"),
				new DataColumn("ItemModal"),
				new DataColumn("ItemCategoryId", typeof(int)),
				new DataColumn("ItemId", typeof(int))
			});
		}
		dtCastingType.Columns.AddRange(new DataColumn[2]
		{
			new DataColumn("Id", typeof(int)),
			new DataColumn("CastingType", typeof(string))
		});
		dtWarehouse.Columns.AddRange(new DataColumn[2]
		{
			new DataColumn("Id", typeof(int)),
			new DataColumn("WareHouseName", typeof(string))
		});
		dtViewAttachments.Columns.Add("AttchmentPath");
		table.Columns.AddRange(new DataColumn[27]
		{
			new DataColumn("Id", typeof(int)),
			new DataColumn("ItemId", typeof(int)),
			new DataColumn("ItemName", typeof(string)),
			new DataColumn("UomId", typeof(int)),
			new DataColumn("Uom", typeof(string)),
			new DataColumn("VariantId", typeof(int)),
			new DataColumn("VariantDescription", typeof(string)),
			new DataColumn("JobLotId", typeof(int)),
			new DataColumn("BatchNo", typeof(string)),
			new DataColumn("CastingTypeId", typeof(int)),
			new DataColumn("CastingType", typeof(string)),
			new DataColumn("WareHouseFromId", typeof(int)),
			new DataColumn("WareHouseToId", typeof(int)),
			new DataColumn("ProductionStageFromId", typeof(int)),
			new DataColumn("ProductionStageToId", typeof(int)),
			new DataColumn("WorkStationFromId", typeof(int)),
			new DataColumn("AvailableQty", typeof(double)),
			new DataColumn("TransferQty", typeof(double)),
			new DataColumn("AvgRate", typeof(double)),
			new DataColumn("RateUomId", typeof(int)),
			new DataColumn("RateUom", typeof(string)),
			new DataColumn("RateEquivalent", typeof(double)),
			new DataColumn("Amount", typeof(double)),
			new DataColumn("Remarks", typeof(string)),
			new DataColumn("RejectionReasonTypeId", typeof(int)),
			new DataColumn("TeamWorkerName", typeof(string)),
			new DataColumn("RejectionReasonRemarks", typeof(string))
		});
	}

	private void InitializeDefaultSettings()
	{
		((Control)(object)CmbTransferType).Enabled = true;
		btnupdate.Visible = false;
		btnDelete.Visible = false;
		btnsave.Visible = true;
		btnSaveAs.Visible = false;
		btnplus.Visible = true;
		btnUpdateDetail.Visible = false;
		btnCancelUpdateDetial.Visible = false;
		DocDate.Focus();
	}

	private void InitializeComponentMethod()
	{
		Task.Run(delegate
		{
			formrights = CommonServices.SetRightsValueInRightsObject(ScreenName);
			ConfigrationsAllocation obj = clsGlobalVariables.configrationsAllocation.FirstOrDefault((ConfigrationsAllocation r) => r.ConfigDescription == "DefaultDaysToLessFromHistoryFromDate");
			DefaultDaysToLessFromHistoryFromDate = Conversion.ToInt((object)((obj != null) ? obj.ConfigKey : null));
			string documentNoData = DocumentNoDBCall();
			RejectionReasonTypeDbCall();
			return new { documentNoData };
		}).ContinueWith(t =>
		{
			if (t.IsFaulted)
			{
				MessageBox.Show("Error occurred during database call.");
			}
			else
			{
				var result = t.Result;
				btnsave.Enabled = formrights.DoHaveSaveRight;
				btnupdate.Enabled = formrights.DoHaveUpdateRights;
				btnDelete.Enabled = formrights.DoHaveUpdateRights;
				btnprint.Enabled = formrights.DoHavePrintRights;
				ChkBoxPrintPreview.Enabled = formrights.DoHavePrintRights;
				ChkBoxPrintPreview.Checked = formrights.DoHavePrintRights;
				ctrlGrdBar3.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar3.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				ctrlGrdBar5.mGridPrint.Enabled = formrights.DoHaveGridPrintRights;
				ctrlGrdBar5.mGridExport.Enabled = formrights.DoHaveGridExportRights;
				if (RecId == 0)
				{
					UpdateDocumentNoUI(result.documentNoData);
				}
				BindTransferType(clsGlobalVariables.GetAllLookUps_Engr);
				WorkStationFill();
				ProductionStageComboFill();
				WareHouseFill();
				RejectionReasonTypeBind();
				if (table != null && table.Columns.Count > 0)
				{
					grd.DataSource = table;
					grd.RetrieveStructure();
					grdSettings();
				}
				InitializeHistoryDefaults();
			}
		}, TaskScheduler.FromCurrentSynchronizationContext());
	}

	private void InitializeHistoryDefaults()
	{
		try
		{
			ParameterFill();
			((UltraGridBase)cmbperemeter).Rows[1].Activate();
			fromdate.Value = ((DefaultDaysToLessFromHistoryFromDate > 0) ? DateTime.Now.AddDays(-DefaultDaysToLessFromHistoryFromDate) : DateTime.Now.AddDays(-3.0));
			ToDate.Value = DateTime.Now;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private string DocumentNoDBCall()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Expected O, but got Unknown
		try
		{
			return Conversion.ToString((object)InvStockTransferHeader.GenerateCode(new InvStockTransferHeader
			{
				OrganizationId = UserAccount.OrganizationId,
				CompanyId = UserAccount.CompanyId,
				BranchesId = UserAccount.BranchesId,
				DocumentTypeId = 1665,
				FinancialYearId = clsGlobalVariables.ActiveYr.Id
			}));
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void UpdateDocumentNoUI(string documentData)
	{
		txtdocno.Text = documentData;
	}

	private void RejectionReasonTypeDbCall()
	{
		try
		{
			dtRejectionReasonType = CommonServices.StaticColumnsService("RejectionReasonType");
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void RejectionReasonTypeBind()
	{
		try
		{
			int Id = Conversion.ToInt(CmbRejectionReasonType.Value);
			if (dtRejectionReasonType.Rows.Count > 0)
			{
				DDL.BindDDL(dtRejectionReasonType, CmbRejectionReasonType, "Id", "type", "Rejection Reason Type", ZeroIndex: true);
				((UltraGridBase)CmbRejectionReasonType).Rows[1].Activate();
				if (Id > 0)
				{
					if (dtRejectionReasonType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbRejectionReasonType.Value = Id;
					}
					else
					{
						((Control)(object)CmbRejectionReasonType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbRejectionReasonType).Text = string.Empty;
				((UltraGridBase)CmbRejectionReasonType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindTransferType(List<LookUps> lst)
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbTransferType.Value);
			DataTable dt = new DataTable();
			dt.Columns.Add("Id", typeof(int));
			dt.Columns.Add("LookupName", typeof(string));
			dt.Clear();
			if (lst != null && lst.Count > 0)
			{
				List<LookUps> filteredList = lst.Where((LookUps r) => r.Id == 9).ToList();
				for (int i = 0; i < filteredList.Count; i++)
				{
					dt.Rows.Add(filteredList[i].Id, filteredList[i].LookupName);
				}
			}
			if (dt.Rows.Count > 0)
			{
				DDL.BindDDL(dt, CmbTransferType, "Id", "LookupName", "Transfer Type", ZeroIndex: true);
				((UltraGridBase)CmbTransferType).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (DetailId > 0)
				{
					if (dt.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
					{
						CmbTransferType.Value = DetailId;
					}
					else
					{
						((Control)(object)CmbTransferType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbTransferType).Text = string.Empty;
				((UltraGridBase)CmbTransferType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindItemCategory(DataTable dt)
	{
		try
		{
			int SupId = Conversion.ToInt(CmbCategory.Value);
			DataTable dtItemCategory = new DataTable();
			dtItemCategory.Columns.Add("Id", typeof(int));
			dtItemCategory.Columns.Add("CategoryDescription", typeof(string));
			if (dt != null && dt.Rows.Count > 0)
			{
				dtItemCategory.Rows.Clear();
				if (dt.Rows.Count > 0)
				{
					HashSet<int> existsIds = new HashSet<int>();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						int Id = Conversion.ToInt(dt.Rows[i]["ItemCategoryId"]);
						if (!existsIds.Contains(Id))
						{
							dtItemCategory.Rows.Add(dt.Rows[i]["ItemCategoryId"], dt.Rows[i]["ItemCategory"]);
							existsIds.Add(Id);
						}
					}
				}
			}
			if (dtItemCategory.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtItemCategory, CmbCategory, "Id", "CategoryDescription", "ItemCategory", false);
				if (SupId > 0)
				{
					if (dtItemCategory.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == SupId))
					{
						CmbCategory.Value = SupId;
					}
					else
					{
						((Control)(object)CmbCategory).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCategory).Text = string.Empty;
				((UltraGridBase)CmbCategory).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindBatch(DataTable dt)
	{
		try
		{
			int SupId = Conversion.ToInt(txtBatchNo.Value);
			DataTable dtJob = new DataTable();
			dtJob.Columns.Add("Id", typeof(int));
			dtJob.Columns.Add("JobLotDescription", typeof(string));
			if (dt != null && dt.Rows.Count > 0)
			{
				dtJob.Rows.Clear();
				if (dt.Rows.Count > 0)
				{
					HashSet<int> existsIds = new HashSet<int>();
					for (int i = 0; i < dt.Rows.Count; i++)
					{
						int Id = Conversion.ToInt(dt.Rows[i]["JobLotId"]);
						if (!existsIds.Contains(Id))
						{
							dtJob.Rows.Add(dt.Rows[i]["JobLotId"], dt.Rows[i]["BatchNo"]);
							existsIds.Add(Id);
						}
					}
				}
			}
			if (dtJob.Rows.Count > 0)
			{
				DDL.BindDDL(dtJob, txtBatchNo, "Id", "JobLotDescription", "Batch", ZeroIndex: false);
				CommonServices.SetComboValue(txtBatchNo, "Id", SupId, dtJob);
			}
			else
			{
				((Control)(object)txtBatchNo).Text = string.Empty;
				((UltraGridBase)txtBatchNo).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ItemdtFill(DataTable dt)
	{
		try
		{
			dtItem.Rows.Clear();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			HashSet<int> existsIds = new HashSet<int>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				int itemId = Conversion.ToInt(dt.Rows[i]["ItemId"]);
				if (!existsIds.Contains(itemId))
				{
					dtItem.Rows.Add(dt.Rows[i]["ItemId"], dt.Rows[i]["ItemName"], dt.Rows[i]["ItemCode"], dt.Rows[i]["ItemCategoryId"], dt.Rows[i]["itemModalId"], dt.Rows[i]["SandWeight"], dt.Rows[i]["DyeWeight"]);
					existsIds.Add(itemId);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemDetailBind(int CategoryId = 0, int itemModalId = 0)
	{
		try
		{
			int Id = Conversion.ToInt(CmbItem.Value);
			DataTable FilterItems = new DataTable();
			if (itemModalId > 0)
			{
				DataRow[] filteredRows = dtItem.Select($"itemModalId = {itemModalId}");
				FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else if (CategoryId > 0)
			{
				DataRow[] filteredRows2 = dtItem.Select($"ItemCategoryId = {CategoryId}");
				FilterItems = (filteredRows2.Any() ? filteredRows2.CopyToDataTable() : new DataTable());
			}
			else
			{
				FilterItems = dtItem;
			}
			if (FilterItems.Rows.Count > 0)
			{
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(FilterItems, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(FilterItems, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbItem).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				if (Id > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbItem.Value = Id;
					}
					else
					{
						((UltraGridBase)CmbItem).Rows[1].Activate();
					}
				}
				else
				{
					((UltraGridBase)CmbItem).Rows[1].Activate();
				}
			}
			else
			{
				((Control)(object)CmbItem).Text = string.Empty;
				((UltraGridBase)CmbItem).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void SampleModaldBCall(string ItemIds)
	{
		try
		{
			dtSampleModal.Rows.Clear();
			DataTable dt = ItemModel.ItemModel_GetByItemIds(UserAccount.OrganizationId, UserAccount.CompanyId, ItemIds);
			if (dt.Rows.Count > 0)
			{
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					dtSampleModal.Rows.Add(dt.Rows[i]["itemModalId"], dt.Rows[i]["ItemSampleNo"], dt.Rows[i]["ItemModal"], dt.Rows[i]["ItemCategoryId"], dt.Rows[i]["ItemId"]);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemModalBind(int CategoryId = 0, int itemId = 0)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSampleModal.Value);
			DataTable FilterItems = new DataTable();
			if (itemId > 0)
			{
				DataRow[] filteredRows = dtSampleModal.Select($"ItemId = {itemId}");
				FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			}
			else if (CategoryId > 0)
			{
				DataRow[] filteredRows2 = dtSampleModal.Select($"ItemCategoryId = {CategoryId}");
				FilterItems = (filteredRows2.Any() ? filteredRows2.CopyToDataTable() : new DataTable());
			}
			else
			{
				FilterItems = dtSampleModal;
			}
			if (FilterItems.Rows.Count > 0)
			{
				DDL.BindDDL(FilterItems, CmbSampleModal, "Id", "SampleNo", "SampleNo", ZeroIndex: true);
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[1].Width = 300;
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[3].Hidden = true;
				((UltraGridBase)CmbSampleModal).DisplayLayout.Bands[0].Columns[4].Hidden = true;
				((UltraGridBase)CmbSampleModal).Rows[1].Activate();
				if (Id > 0)
				{
					if (FilterItems.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbSampleModal.Value = Id;
					}
					else
					{
						((Control)(object)CmbSampleModal).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbSampleModal).Text = string.Empty;
				((UltraGridBase)CmbSampleModal).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable GetPackUom(DataTable dt)
	{
		try
		{
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("UOMCode", typeof(string));
			dtUom.Columns.Add("Equivalent", typeof(double));
			if (dt.Rows.Count > 0)
			{
				HashSet<int> existsIds = new HashSet<int>();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					int Id = Conversion.ToInt(dt.Rows[i]["ItemUomId"]);
					if (!existsIds.Contains(Id))
					{
						dtUom.Rows.Add(dt.Rows[i]["ItemUomId"], dt.Rows[i]["Uom"], dt.Rows[i]["PackEquivalent"]);
						existsIds.Add(Id);
					}
				}
			}
			return dtUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private DataTable GetRateUom(DataTable dt)
	{
		try
		{
			DataTable dtUom = new DataTable();
			dtUom.Columns.Add("Id", typeof(int));
			dtUom.Columns.Add("UOMCode", typeof(string));
			dtUom.Columns.Add("Equivalent", typeof(double));
			if (dt.Rows.Count > 0)
			{
				HashSet<int> existsIds = new HashSet<int>();
				for (int i = 0; i < dt.Rows.Count; i++)
				{
					int Id = Conversion.ToInt(dt.Rows[i]["RateUomId"]);
					if (!existsIds.Contains(Id))
					{
						dtUom.Rows.Add(dt.Rows[i]["RateUomId"], dt.Rows[i]["RateUom"], dt.Rows[i]["RateEquivalent"]);
						existsIds.Add(Id);
					}
				}
			}
			return dtUom;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void ItemUomBind(int ItemId)
	{
		try
		{
			int Id = Conversion.ToInt(CmbPackUom.Value);
			int RUomId = Conversion.ToInt(CmbRateUom.Value);
			int VariantId = Conversion.ToInt(CmbVariantDescription.Value);
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			DataTable FilterItems = new DataTable();
			DataRow[] filteredRows = dtAvailableStockData.Select($"ItemId = {ItemId}");
			FilterItems = (filteredRows.Any() ? filteredRows.CopyToDataTable() : new DataTable());
			DataTable dtPack = GetPackUom(FilterItems);
			if (dtPack.Rows.Count > 0)
			{
				DDL.BindDDL(dtPack, CmbPackUom, "Id", "UOMCode", "UOM", ZeroIndex: false);
				((UltraGridBase)CmbPackUom).DisplayLayout.Bands[0].Columns[1].Width = 193;
				((UltraGridBase)CmbPackUom).Rows[0].Activate();
				if (Id > 0)
				{
					if (dtPack.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbPackUom.Value = Id;
					}
					else
					{
						((Control)(object)CmbPackUom).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbPackUom).Text = string.Empty;
				((UltraGridBase)CmbPackUom).DataSource = null;
			}
			DataTable dtRate = GetRateUom(FilterItems);
			if (dtRate.Rows.Count > 0)
			{
				DDL.BindDDL(dtRate, CmbRateUom, "Id", "UOMCode", "Rate Uom", ZeroIndex: false);
				((UltraGridBase)CmbRateUom).DisplayLayout.Bands[0].Columns[1].Width = 193;
				((UltraGridBase)CmbRateUom).Rows[0].Activate();
				if (RUomId > 0)
				{
					if (dtRate.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == RUomId))
					{
						CmbRateUom.Value = RUomId;
					}
					else
					{
						((Control)(object)CmbRateUom).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbRateUom).Text = string.Empty;
				((UltraGridBase)CmbRateUom).DataSource = null;
			}
			DataTable dtVariant = ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, ItemId);
			if (dtVariant.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtVariant, CmbVariantDescription, "ItemAttributeVarientId", "ItemAttribute", "Modal Description", false);
				((UltraGridBase)CmbVariantDescription).Rows[0].Activate();
				if (VariantId > 0)
				{
					if (dtVariant.AsEnumerable().Any((DataRow row) => row.Field<int>("ItemAttributeVarientId") == VariantId))
					{
						CmbVariantDescription.Value = VariantId;
					}
					else
					{
						((Control)(object)CmbVariantDescription).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbVariantDescription).Text = string.Empty;
				((UltraGridBase)CmbVariantDescription).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void WareHouseFill()
	{
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		try
		{
			int Id = Conversion.ToInt(CmbWarehouseFrom.Value);
			int Id2 = Conversion.ToInt(CmbWarehouseTo.Value);
			int WfFromId = Conversion.ToInt(CmbWareHouseFromDetail.Value);
			int WfToId = Conversion.ToInt(CmbWarehouseToDetail.Value);
			dtWarehouse.Clear();
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			dtWarehouse = WarehousesAllocationToBranch.GetWarehousesAllocatedToBranchByBranchId(obj);
			if (dtWarehouse.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWarehouse, CmbWarehouseFrom, "Id", "WareHouseName", "WareHouse ", true);
				DDL.BindDDLNew(dtWarehouse, CmbWareHouseFromDetail, "Id", "WareHouseName", "WareHouse From", true);
				DDL.BindDDLNew(dtWarehouse, CmbWarehouseTo, "Id", "WareHouseName", "WareHouse ", true);
				DDL.BindDDLNew(dtWarehouse, CmbWarehouseToDetail, "Id", "WareHouseName", "WareHouse To", true);
				((UltraGridBase)CmbWarehouseFrom).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				((UltraGridBase)CmbWarehouseTo).DisplayLayout.AutoFitStyle = (AutoFitStyle)1;
				if (Id > 0)
				{
					if (dtWarehouse.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id))
					{
						CmbWarehouseFrom.Value = Id;
					}
					else
					{
						((Control)(object)CmbWarehouseFrom).Text = string.Empty;
					}
				}
				if (Id2 > 0)
				{
					if (dtWarehouse.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == Id2))
					{
						CmbWarehouseTo.Value = Id2;
					}
					else
					{
						((Control)(object)CmbWarehouseTo).Text = string.Empty;
					}
				}
				if (WfFromId > 0)
				{
					if (dtWarehouse.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == WfFromId))
					{
						CmbWareHouseFromDetail.Value = WfFromId;
					}
					else
					{
						((Control)(object)CmbWareHouseFromDetail).Text = string.Empty;
					}
				}
				if (WfToId > 0)
				{
					if (dtWarehouse.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == WfToId))
					{
						CmbWarehouseToDetail.Value = WfToId;
					}
					else
					{
						((Control)(object)CmbWarehouseToDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((UltraGridBase)CmbWarehouseFrom).DataSource = null;
				((Control)(object)CmbWarehouseFrom).Text = string.Empty;
				((UltraGridBase)CmbWarehouseTo).DataSource = null;
				((Control)(object)CmbWarehouseTo).Text = string.Empty;
				((UltraGridBase)CmbWareHouseFromDetail).DataSource = null;
				((Control)(object)CmbWareHouseFromDetail).Text = string.Empty;
				((UltraGridBase)CmbWarehouseToDetail).DataSource = null;
				((Control)(object)CmbWarehouseToDetail).Text = string.Empty;
			}
		}
		catch (Exception)
		{
			MessageBox.Show("WareHouse Not Found");
		}
	}

	private void CastingTypedtFill(DataTable dt)
	{
		try
		{
			dtCastingType.Clear();
			if (dt.Rows.Count <= 0)
			{
				return;
			}
			HashSet<int> existsIds = new HashSet<int>();
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				int Id = Conversion.ToInt(dt.Rows[i]["CastingTypeId"]);
				if (!existsIds.Contains(Id))
				{
					dtCastingType.Rows.Add(dt.Rows[i]["CastingTypeId"], dt.Rows[i]["CastingType"]);
					existsIds.Add(Id);
				}
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void BindCastingType()
	{
		try
		{
			int DetailId = Conversion.ToInt(CmbCastingType.Value);
			if (dtCastingType.Rows.Count > 0)
			{
				DDL.BindDDL(dtCastingType, CmbCastingType, "Id", "CastingType", "Casting Type", ZeroIndex: true);
				((UltraGridBase)CmbCastingType).DisplayLayout.Bands[0].Columns[1].Width = 350;
				if (DetailId > 0)
				{
					if (dtCastingType.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == DetailId))
					{
						CmbCastingType.Value = DetailId;
					}
					else
					{
						((Control)(object)CmbCastingType).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbCastingType).Text = string.Empty;
				((UltraGridBase)CmbCastingType).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ProductionStageComboFill()
	{
		try
		{
			int ID = Conversion.ToInt(CmbProductionStageFrom.Value);
			int ID2 = Conversion.ToInt(CmbProductionStageTo.Value);
			int WfFromId = Conversion.ToInt(CmbProductionStageFromDetail.Value);
			int WfToId = Conversion.ToInt(CmbProductionStageToDetail.Value);
			dtProductionStage.Rows.Clear();
			dtProductionStage = CommonServices.GetLookupsByTypeId_Engr(3);
			if (dtProductionStage.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtProductionStage, CmbProductionStageFrom, "Id", "LookupName", "Production Stage", true);
				DDL.BindDDLNew(dtProductionStage, CmbProductionStageFromDetail, "Id", "LookupName", "Production Stage", true);
				DDL.BindDDLNew(dtProductionStage, CmbProductionStageTo, "Id", "LookupName", "Production Stage", true);
				DDL.BindDDLNew(dtProductionStage, CmbProductionStageToDetail, "Id", "LookupName", "Production Stage", true);
				if (ID > 0)
				{
					if (dtProductionStage.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbProductionStageFrom.Value = ID;
					}
					else
					{
						((Control)(object)CmbProductionStageFrom).Text = string.Empty;
					}
				}
				if (ID2 > 0)
				{
					if (dtProductionStage.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID2))
					{
						CmbProductionStageTo.Value = ID2;
					}
					else
					{
						((Control)(object)CmbProductionStageTo).Text = string.Empty;
					}
				}
				if (WfFromId > 0)
				{
					if (dtProductionStage.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == WfFromId))
					{
						CmbProductionStageFromDetail.Value = WfFromId;
					}
					else
					{
						((Control)(object)CmbProductionStageFromDetail).Text = string.Empty;
					}
				}
				if (WfToId > 0)
				{
					if (dtProductionStage.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == WfToId))
					{
						CmbProductionStageToDetail.Value = WfToId;
					}
					else
					{
						((Control)(object)CmbProductionStageToDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbProductionStageFrom).Text = string.Empty;
				((UltraGridBase)CmbProductionStageFrom).DataSource = null;
				((Control)(object)CmbProductionStageTo).Text = string.Empty;
				((UltraGridBase)CmbProductionStageTo).DataSource = null;
				((Control)(object)CmbProductionStageFromDetail).Text = string.Empty;
				((UltraGridBase)CmbProductionStageFromDetail).DataSource = null;
				((Control)(object)CmbProductionStageToDetail).Text = string.Empty;
				((UltraGridBase)CmbProductionStageToDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void WorkStationFill()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Expected O, but got Unknown
		try
		{
			dtWorkStation.Clear();
			int ID = Conversion.ToInt(CmbWorkStationFromDetail.Value);
			ProductionWorkStation obj = new ProductionWorkStation();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchId = UserAccount.BranchesId;
			dtWorkStation = ProductionWorkStation.FormHistory(obj);
			if (dtWorkStation.Rows.Count > 0)
			{
				DDL.BindDDLNew(dtWorkStation, CmbWorkStationFromDetail, "Id", "WorkStationName", "Work Station", true);
				if (ID > 0)
				{
					if (dtWorkStation.AsEnumerable().Any((DataRow row) => row.Field<int>("Id") == ID))
					{
						CmbWorkStationFromDetail.Value = ID;
					}
					else
					{
						((Control)(object)CmbWorkStationFromDetail).Text = string.Empty;
					}
				}
			}
			else
			{
				((Control)(object)CmbWorkStationFromDetail).Text = string.Empty;
				((UltraGridBase)CmbWorkStationFromDetail).DataSource = null;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbItem_Leave(object sender, EventArgs e)
	{
		try
		{
			int ItemId = Conversion.ToInt(CmbItem.Value);
			if (((UltraGridBase)CmbItem).ActiveRow != null && ItemId > 0)
			{
				CmbCategory.Value = Conversion.ToInt(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemCategoryId"].Value);
			}
			ItemUomBind(ItemId);
			ItemModalBind(0, ItemId);
			GetAvailableQtyForTextBox();
			txtAvgRate.Text = GetCgsAvgRate().ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbSampleModal_Leave(object sender, EventArgs e)
	{
		try
		{
			int Id = Conversion.ToInt(CmbSampleModal.Value);
			if (((UltraGridBase)CmbSampleModal).ActiveRow != null && Id > 0)
			{
				CmbCategory.Value = Conversion.ToInt(((UltraDropDownBase)CmbSampleModal).SelectedRow.Cells["ItemCategoryId"].Value);
			}
			ItemDetailBind(0, Conversion.ToInt(CmbSampleModal.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCategory_Leave(object sender, EventArgs e)
	{
		try
		{
			ItemDetailBind(Conversion.ToInt(CmbCategory.Value));
			ItemModalBind(Conversion.ToInt(CmbCategory.Value));
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void GetTransferTypeData(int TransferTypeId)
	{
		try
		{
			DataTable dt = WarehouseAndProductionStagesAllocateToStockTransferType.GetWarehouseAndProductionStageDataByTransferTypeId(UserAccount.OrganizationId, UserAccount.CompanyId, TransferTypeId);
			if (dt != null && dt.Rows.Count > 0)
			{
				CmbWarehouseFrom.Value = Conversion.ToInt(dt.Rows[0]["FromWarehouseId"]);
				CmbWarehouseTo.Value = Conversion.ToInt(dt.Rows[0]["ToWarehouseId"]);
				CmbProductionStageFrom.Value = Conversion.ToInt(dt.Rows[0]["FromProductionStageId"]);
				CmbProductionStageTo.Value = Conversion.ToInt(dt.Rows[0]["ToProductionStageId"]);
				CmbWareHouseFromDetail.Value = CmbWarehouseFrom.Value;
				CmbWarehouseToDetail.Value = CmbWarehouseTo.Value;
				CmbProductionStageFromDetail.Value = CmbProductionStageFrom.Value;
				CmbProductionStageToDetail.Value = CmbProductionStageTo.Value;
			}
			else
			{
				((Control)(object)CmbWarehouseFrom).Text = string.Empty;
				((Control)(object)CmbWarehouseTo).Text = string.Empty;
				((Control)(object)CmbProductionStageFrom).Text = string.Empty;
				((Control)(object)CmbProductionStageTo).Text = string.Empty;
				((Control)(object)CmbWareHouseFromDetail).Text = string.Empty;
				((Control)(object)CmbWarehouseToDetail).Text = string.Empty;
				((Control)(object)CmbProductionStageFromDetail).Text = string.Empty;
				((Control)(object)CmbProductionStageToDetail).Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void CmbTransferType_Leave(object sender, EventArgs e)
	{
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Expected O, but got Unknown
		try
		{
			int TransferTypeId = Conversion.ToInt(CmbTransferType.Value);
			if (((UltraGridBase)CmbTransferType).ActiveRow != null && TransferTypeId > 0)
			{
				if (TransferTypeId != Conversion.ToInt(((Control)(object)CmbTransferType).Tag) || Conversion.ToInt(CmbWarehouseFrom.Value) == 0 || dtAvailableStockData.Rows.Count == 0)
				{
					((Control)(object)CmbTransferType).Tag = TransferTypeId;
					GetTransferTypeData(TransferTypeId);
					GridDataTransferTypeId = Conversion.ToInt(CmbTransferType.Value);
					dtAvailableStockData.Clear();
					ReportsParameters obj = new ReportsParameters();
					obj.OrganizationId = UserAccount.OrganizationId;
					obj.CompanyId = UserAccount.CompanyId;
					obj.WarehouseId = Conversion.ToInt(CmbWarehouseFrom.Value);
					obj.ProductionStageId = Conversion.ToInt(CmbProductionStageFrom.Value);
					obj.DocDate = DocDate.Value;
					dtAvailableStockData = InventoryStockEvalautionDetail.GetAvailableQtyByWarehouseAndProductionStage(obj);
					BindCombos();
					if (dtAvailableStockData.Rows.Count == 0)
					{
						table.Clear();
					}
				}
			}
			else if (MessageBox.Show("you are Removing Transfer Type and in that Case Data in Grid will be Reset,are you sure to do that?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				CmbTransferType.Value = GridDataTransferTypeId;
				((Control)(object)CmbTransferType).Tag = GridDataTransferTypeId;
			}
			else
			{
				dtAvailableStockData.Clear();
				table.Clear();
				((Control)(object)CmbTransferType).Tag = 0;
				((Control)(object)CmbWarehouseFrom).Text = string.Empty;
				((Control)(object)CmbWarehouseTo).Text = string.Empty;
				((Control)(object)CmbProductionStageFrom).Text = string.Empty;
				((Control)(object)CmbProductionStageTo).Text = string.Empty;
				((UltraGridBase)CmbCategory).DataSource = null;
				((UltraGridBase)CmbItem).DataSource = null;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbVariantDescription).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
				((UltraGridBase)CmbCastingType).DataSource = null;
				((UltraGridBase)CmbSampleModal).DataSource = null;
				((Control)(object)CmbCategory).Text = string.Empty;
				((Control)(object)CmbSampleModal).Text = string.Empty;
				((Control)(object)CmbItem).Text = string.Empty;
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbVariantDescription).Text = string.Empty;
				((Control)(object)CmbWareHouseFromDetail).Text = string.Empty;
				((Control)(object)CmbWarehouseToDetail).Text = string.Empty;
				((Control)(object)CmbProductionStageFromDetail).Text = string.Empty;
				((Control)(object)CmbProductionStageToDetail).Text = string.Empty;
				((Control)(object)CmbWorkStationFromDetail).Text = string.Empty;
				txtTransferQty.Text = string.Empty;
				txtAvailableQty.Text = string.Empty;
				txtAvgRate.Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				txtAmount.Text = string.Empty;
				((Control)(object)CmbCastingType).Text = string.Empty;
				txtRemarksDetail.Text = string.Empty;
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void BindCombos()
	{
		try
		{
			if (dtAvailableStockData != null && dtAvailableStockData.Rows.Count > 0)
			{
				BindItemCategory(dtAvailableStockData);
				BindBatch(dtAvailableStockData);
				string ItemIds = (from r in dtAvailableStockData.AsEnumerable()
					select r.Field<int>("ItemId")).Distinct().Aggregate("", (string current, int id) => current + id + ",");
				SampleModaldBCall(ItemIds);
				ItemModalBind(Conversion.ToInt(CmbCategory.Value));
				ItemdtFill(dtAvailableStockData);
				ItemDetailBind(Conversion.ToInt(CmbCategory.Value), Conversion.ToInt(CmbSampleModal.Value));
				CastingTypedtFill(dtAvailableStockData);
				BindCastingType();
			}
			else
			{
				((UltraGridBase)CmbCategory).DataSource = null;
				((UltraGridBase)CmbItem).DataSource = null;
				((UltraGridBase)CmbPackUom).DataSource = null;
				((UltraGridBase)CmbVariantDescription).DataSource = null;
				((UltraGridBase)CmbRateUom).DataSource = null;
				((UltraGridBase)CmbCastingType).DataSource = null;
				((UltraGridBase)CmbSampleModal).DataSource = null;
				((Control)(object)CmbCategory).Text = string.Empty;
				((Control)(object)CmbSampleModal).Text = string.Empty;
				((Control)(object)CmbItem).Text = string.Empty;
				((Control)(object)CmbPackUom).Text = string.Empty;
				((Control)(object)CmbVariantDescription).Text = string.Empty;
				((Control)(object)CmbWareHouseFromDetail).Text = string.Empty;
				((Control)(object)CmbWarehouseToDetail).Text = string.Empty;
				((Control)(object)CmbProductionStageFromDetail).Text = string.Empty;
				((Control)(object)CmbProductionStageToDetail).Text = string.Empty;
				((Control)(object)CmbWorkStationFromDetail).Text = string.Empty;
				txtTransferQty.Text = string.Empty;
				txtAvgRate.Text = string.Empty;
				((Control)(object)CmbRateUom).Text = string.Empty;
				txtAmount.Text = string.Empty;
				((Control)(object)CmbCastingType).Text = string.Empty;
				txtRemarksDetail.Text = string.Empty;
			}
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
			if (dtItem.Rows.Count > 0)
			{
				int ItemId = 0;
				if (Conversion.ToInt(CmbItem.Value) > 0)
				{
					ItemId = Conversion.ToInt(CmbItem.Value);
				}
				if (rdbtnItemName.Checked)
				{
					DDL.BindDDL(dtItem, CmbItem, "Id", "ItemName", "Item Name", ZeroIndex: true);
				}
				else
				{
					DDL.BindDDL(dtItem, CmbItem, "Id", "ItemCode", "Item Name", ZeroIndex: true);
				}
				if (ItemId > 0)
				{
					CmbItem.Value = ItemId;
					CmbItem.Focus();
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
		if (txtdocno.Text.Trim() == string.Empty || txtdocno.Text.Trim() == "0")
		{
			MessageBox.Show("DocNo Field is Required");
			txtdocno.Focus();
			return false;
		}
		if (((UltraGridBase)CmbTransferType).ActiveRow == null || Conversion.ToInt(CmbTransferType.Value) == 0)
		{
			MessageBox.Show("TransferType Field is Required");
			CmbTransferType.Focus();
			return false;
		}
		return true;
	}

	private bool FormValidationDetail()
	{
		if (((UltraGridBase)CmbItem).ActiveRow == null || Conversion.ToInt(CmbItem.Value) == 0)
		{
			MessageBox.Show("Item / Modal Field is Required");
			CmbItem.Focus();
			return false;
		}
		if (((UltraGridBase)CmbPackUom).ActiveRow == null || Conversion.ToInt(CmbPackUom.Value) == 0)
		{
			MessageBox.Show("Uom Field is Required");
			CmbPackUom.Focus();
			return false;
		}
		if (((Control)(object)txtBatchNo).Text == null || ((Control)(object)txtBatchNo).Text == string.Empty)
		{
			MessageBox.Show("Batch No Field is Required");
			txtBatchNo.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWareHouseFromDetail).ActiveRow == null || Conversion.ToInt(CmbWareHouseFromDetail.Value) == 0)
		{
			MessageBox.Show("Warehouse From Field is Required");
			CmbWareHouseFromDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbWarehouseToDetail).ActiveRow == null || Conversion.ToInt(CmbWarehouseToDetail.Value) == 0)
		{
			MessageBox.Show("Warehouse To Field is Required");
			CmbWarehouseToDetail.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbWareHouseFromDetail.Value) == Conversion.ToInt(CmbWarehouseToDetail.Value))
		{
			MessageBox.Show("Warehouse From And Warehouse To can't be same");
			CmbWarehouseToDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbProductionStageFromDetail).ActiveRow == null || Conversion.ToInt(CmbProductionStageFromDetail.Value) == 0)
		{
			MessageBox.Show("Production Stage From Field is Required");
			CmbProductionStageFromDetail.Focus();
			return false;
		}
		if (((UltraGridBase)CmbProductionStageToDetail).ActiveRow == null || Conversion.ToInt(CmbProductionStageToDetail.Value) == 0)
		{
			MessageBox.Show("Production Stage To Field is Required");
			CmbProductionStageToDetail.Focus();
			return false;
		}
		if (Conversion.ToInt(CmbTransferType.Value) != 8 && Conversion.ToInt(CmbProductionStageFromDetail.Value) == Conversion.ToInt(CmbProductionStageToDetail.Value))
		{
			MessageBox.Show("Production Stage From And Production Stage To can't be same");
			CmbProductionStageToDetail.Focus();
			return false;
		}
		if (Conversion.ToDouble((object)txtTransferQty.Text) <= 0.0)
		{
			MessageBox.Show("Transfer Qty Field is Required");
			txtTransferQty.Focus();
			return false;
		}
		return true;
	}

	private void btnRefresh_Click(object sender, EventArgs e)
	{
		try
		{
			clsGlobalVariables.GetAllLookUps_Engr = LookUps.FormHistoryWithList();
			BindTransferType(clsGlobalVariables.GetAllLookUps_Engr);
			WareHouseFill();
			ProductionStageComboFill();
			WorkStationFill();
			RejectionReasonTypeDbCall();
			RejectionReasonTypeBind();
			grdComboFill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnnew_Click(object sender, EventArgs e)
	{
		Reset();
	}

	private void btnclose_Click(object sender, EventArgs e)
	{
		Close();
	}

	private void Reset()
	{
		try
		{
			AT = new Attachment();
			Attachment.dtAttachments.Rows.Clear();
			lstRemoveRecord.Clear();
			RecId = 0;
			IsApproved = false;
			AttachmentsValues = "";
			CustomAttachmentsValues = "";
			((Control)(object)CmbWarehouseFrom).Text = string.Empty;
			((Control)(object)CmbWarehouseTo).Text = string.Empty;
			((Control)(object)CmbProductionStageFrom).Text = string.Empty;
			((Control)(object)CmbProductionStageTo).Text = string.Empty;
			txtRemarksHead.Text = string.Empty;
			((Control)(object)CmbTransferType).Enabled = true;
			GridDataTransferTypeId = 0;
			((Control)(object)CmbTransferType).Text = "";
			((Control)(object)CmbTransferType).Tag = 0;
			updateDetailIndex = -1;
			dtAvailableStockData.Clear();
			((UltraGridBase)CmbCategory).DataSource = null;
			((UltraGridBase)CmbItem).DataSource = null;
			((UltraGridBase)CmbPackUom).DataSource = null;
			((UltraGridBase)CmbVariantDescription).DataSource = null;
			((UltraGridBase)CmbRateUom).DataSource = null;
			((UltraGridBase)CmbCastingType).DataSource = null;
			((UltraGridBase)CmbSampleModal).DataSource = null;
			((Control)(object)CmbCategory).Text = string.Empty;
			((Control)(object)CmbSampleModal).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbVariantDescription).Text = string.Empty;
			((Control)(object)CmbWareHouseFromDetail).Text = string.Empty;
			((Control)(object)CmbWarehouseToDetail).Text = string.Empty;
			((Control)(object)CmbProductionStageFromDetail).Text = string.Empty;
			((Control)(object)CmbProductionStageToDetail).Text = string.Empty;
			((Control)(object)CmbWorkStationFromDetail).Text = string.Empty;
			txtTransferQty.Text = string.Empty;
			txtAvailableQty.Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)CmbCastingType).Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			txtTeamWorkerName.Text = string.Empty;
			txtRejectionReasonRemarks.Text = string.Empty;
			btnsave.Visible = true;
			btnupdate.Visible = false;
			btnDelete.Visible = false;
			btnSaveAs.Visible = false;
			DocDate.Focus();
			table.Rows.Clear();
			grd.DataSource = table;
			UpdateDocumentNoUI(DocumentNoDBCall());
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void frmStockTransferManual_FormClosing(object sender, FormClosingEventArgs e)
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

	private void FromResetDetail()
	{
		try
		{
			updateDetailIndex = -1;
			((Control)(object)CmbCategory).Text = string.Empty;
			((Control)(object)CmbItem).Text = string.Empty;
			((Control)(object)CmbPackUom).Text = string.Empty;
			((Control)(object)CmbVariantDescription).Text = string.Empty;
			((Control)(object)CmbWorkStationFromDetail).Text = string.Empty;
			txtTransferQty.Text = string.Empty;
			txtAvailableQty.Text = string.Empty;
			txtAvgRate.Text = string.Empty;
			((Control)(object)CmbRateUom).Text = string.Empty;
			txtAmount.Text = string.Empty;
			((Control)(object)CmbCastingType).Text = string.Empty;
			txtRemarksDetail.Text = string.Empty;
			txtTeamWorkerName.Text = string.Empty;
			txtRejectionReasonRemarks.Text = string.Empty;
			btnplus.Visible = true;
			btnUpdateDetail.Visible = false;
			btnCancelUpdateDetial.Visible = false;
			CmbItem.Focus();
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
			CmbCategory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnCancelUpdateDetial_Leave(object sender, EventArgs e)
	{
		try
		{
			CmbCategory.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void AddRowInGrid()
	{
	}

	private void grdComboFill()
	{
		try
		{
			if (dtWarehouse != null)
			{
				grd.RootTable.Columns["WareHouseFromId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["WareHouseFromId"].HasValueList = true;
				grd.RootTable.Columns["WareHouseFromId"].LimitToList = true;
				grd.RootTable.Columns["WareHouseFromId"].EditType = (EditType)4;
				grd.RootTable.Columns["WareHouseFromId"].ValueList.PopulateValueList((IEnumerable)dtWarehouse.DefaultView, "Id", "WareHouseName");
				grd.RootTable.Columns["WareHouseToId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["WareHouseToId"].HasValueList = true;
				grd.RootTable.Columns["WareHouseToId"].LimitToList = true;
				grd.RootTable.Columns["WareHouseToId"].EditType = (EditType)4;
				grd.RootTable.Columns["WareHouseToId"].ValueList.PopulateValueList((IEnumerable)dtWarehouse.DefaultView, "Id", "WareHouseName");
			}
			if (dtWorkStation != null)
			{
				grd.RootTable.Columns["WorkStationFromId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["WorkStationFromId"].HasValueList = true;
				grd.RootTable.Columns["WorkStationFromId"].LimitToList = true;
				grd.RootTable.Columns["WorkStationFromId"].EditType = (EditType)4;
				grd.RootTable.Columns["WorkStationFromId"].ValueList.PopulateValueList((IEnumerable)dtWorkStation.DefaultView, "Id", "WorkStationName");
			}
			if (dtProductionStage != null)
			{
				grd.RootTable.Columns["ProductionStageFromId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["ProductionStageFromId"].HasValueList = true;
				grd.RootTable.Columns["ProductionStageFromId"].LimitToList = true;
				grd.RootTable.Columns["ProductionStageFromId"].EditType = (EditType)4;
				grd.RootTable.Columns["ProductionStageFromId"].ValueList.PopulateValueList((IEnumerable)dtProductionStage.DefaultView, "Id", "LookupName");
				grd.RootTable.Columns["ProductionStageToId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["ProductionStageToId"].HasValueList = true;
				grd.RootTable.Columns["ProductionStageToId"].LimitToList = true;
				grd.RootTable.Columns["ProductionStageToId"].EditType = (EditType)4;
				grd.RootTable.Columns["ProductionStageToId"].ValueList.PopulateValueList((IEnumerable)dtProductionStage.DefaultView, "Id", "LookupName");
			}
			if (dtRejectionReasonType != null)
			{
				grd.RootTable.Columns["RejectionReasonTypeId"].ColumnType = (ColumnType)0;
				grd.RootTable.Columns["RejectionReasonTypeId"].HasValueList = true;
				grd.RootTable.Columns["RejectionReasonTypeId"].LimitToList = true;
				grd.RootTable.Columns["RejectionReasonTypeId"].EditType = (EditType)4;
				grd.RootTable.Columns["RejectionReasonTypeId"].ValueList.PopulateValueList((IEnumerable)dtRejectionReasonType.DefaultView, "Id", "type");
			}
			grd.RootTable.Columns["WareHouseFromId"].Caption = "WareHouse From";
			grd.RootTable.Columns["WareHouseToId"].Caption = "WareHouse To";
			grd.RootTable.Columns["WorkStationFromId"].Caption = "WorkStation";
			grd.RootTable.Columns["ProductionStageFromId"].Caption = "ProductionStage From";
			grd.RootTable.Columns["ProductionStageToId"].Caption = "ProductionStage To";
			grd.RootTable.Columns["RejectionReasonTypeId"].Caption = "Rejection Reason Type";
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grd.CurrentRow;
			if (r != null && (int)r.RowType == 0 && r.Cells.Count > 0)
			{
				if (e.Column.Key == "Delete")
				{
					DeleteDetailRow(r);
				}
				if (e.Column.Key == "Add")
				{
					AddRowInGrid();
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
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Expected O, but got Unknown
		try
		{
			if (r == null || (int)r.RowType != 0 || r.Cells.Count <= 0)
			{
				return;
			}
			if (Conversion.ToInt(r.Cells["Id"].Value) > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				InvStockTransferDetail vd = new InvStockTransferDetail();
				vd.Id = Conversion.ToInt(r.Cells["Id"].Value);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.ItemUomId = Conversion.ToInt(r.Cells["UomId"].Value);
				vd.ItemVariantId = Conversion.ToInt(r.Cells["VariantId"].Value);
				vd.JobLotId = Conversion.ToInt((object)r.Cells["JobLotId"].Text);
				vd.BatchDescription = Conversion.ToString((object)r.Cells["BatchNo"].Text);
				vd.CastingTypeId = Conversion.ToInt(r.Cells["CastingTypeId"].Value);
				vd.WarehouseFromId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				vd.WarehouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
				vd.ProductionStageFromId = Conversion.ToInt(r.Cells["ProductionStageFromId"].Value);
				vd.ProductionStageToId = Conversion.ToInt(r.Cells["ProductionStageToId"].Value);
				vd.WorkStationFromId = Conversion.ToInt(r.Cells["WorkStationFromId"].Value);
				vd.ItemQty = Conversion.ToDecimal(r.Cells["TransferQty"].Value);
				vd.NetWeight = Conversion.ToDecimal(r.Cells["TransferQty"].Value);
				vd.ItemRate = Conversion.ToDecimal(r.Cells["AvgRate"].Value);
				vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
				vd.ItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				vd.RejectionReasonTypeId = Conversion.ToInt(r.Cells["RejectionReasonTypeId"].Value);
				vd.TeamWorkerName = Conversion.ToString((object)r.Cells["TeamWorkerName"].Text);
				vd.RejectionReasonRemarks = Conversion.ToString((object)r.Cells["RejectionReasonRemarks"].Text);
				vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
				vd.ActionTypeId = 3;
				lstRemoveRecord.Add(vd);
				table.Rows[r.RowIndex].Delete();
				r.Delete();
			}
			else
			{
				r.Delete();
			}
			if (table.Rows.Count == 0)
			{
				AddRowInGrid();
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdSettings()
	{
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Expected O, but got Unknown
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grd.AllowEdit = (InheritableBoolean)1;
			grd.ColumnAutoResize = false;
			grd.RootTable.Columns["Id"].Visible = false;
			grd.RootTable.Columns["ItemId"].Visible = false;
			grd.RootTable.Columns["UomId"].Visible = false;
			grd.RootTable.Columns["VariantId"].Visible = false;
			grd.RootTable.Columns["RateUomId"].Visible = false;
			grd.RootTable.Columns["RateEquivalent"].Visible = false;
			grd.RootTable.Columns["CastingTypeId"].Visible = false;
			grd.RootTable.Columns["JobLotId"].Visible = false;
			grd.RootTable.Columns["WareHouseFromId"].Visible = false;
			grd.RootTable.Columns["WareHouseToId"].Visible = false;
			grd.RootTable.Columns["ProductionStageFromId"].Visible = false;
			grd.RootTable.Columns["ProductionStageToId"].Visible = false;
			grd.RootTable.Columns["TeamWorkerName"].Caption = "Reason Type Description";
			grd.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grd.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
				CommonServices.GridColumnSettings(item, 3);
				item.EditType = (EditType)0;
				item.FilterEditType = (FilterEditType)((!(item.Type == typeof(DateTime))) ? 1 : 6);
			}
			grdComboFill();
			grd.RootTable.Columns["WareHouseFromId"].EditType = (EditType)0;
			grd.RootTable.Columns["WareHouseToId"].EditType = (EditType)0;
			grd.RootTable.Columns["ProductionStageFromId"].EditType = (EditType)0;
			grd.RootTable.Columns["ProductionStageToId"].EditType = (EditType)0;
			grd.RootTable.Columns["JobLotId"].EditType = (EditType)0;
			grd.RootTable.Columns["CastingTypeId"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUomId"].EditType = (EditType)0;
			grd.RootTable.Columns["RateUom"].EditType = (EditType)0;
			grd.RootTable.Columns["RateEquivalent"].EditType = (EditType)0;
			grd.RootTable.Columns["Amount"].EditType = (EditType)0;
			grd.RootTable.Columns["WorkStationFromId"].EditType = (EditType)4;
			grd.RootTable.Columns["TransferQty"].EditType = (EditType)1;
			grd.RootTable.Columns["Remarks"].EditType = (EditType)1;
			grd.RootTable.Columns["AvgRate"].EditType = (EditType)1;
			grd.RootTable.Columns["TeamWorkerName"].EditType = (EditType)1;
			grd.RootTable.Columns["RejectionReasonRemarks"].EditType = (EditType)1;
			grd.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
			grd.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
			grd.RootTable.Columns["BatchNo"].Width = Constants.InventoryConstants.OtherDescription;
			grd.RootTable.Columns["WareHouseFromId"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["WareHouseToId"].Width = Constants.InventoryConstants.WareHouseName;
			grd.RootTable.Columns["ProductionStageFromId"].Width = Constants.InventoryConstants.ProductionStage;
			grd.RootTable.Columns["ProductionStageToId"].Width = Constants.InventoryConstants.ProductionStage;
			grd.RootTable.Columns["WorkStationFromId"].Width = Constants.InventoryConstants.WorkStationName;
			grd.RootTable.Columns["AvailableQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["TransferQty"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.QtyConstant;
			grd.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
			grd.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
			grd.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["RejectionReasonTypeId"].Width = Constants.InventoryConstants.PackingType;
			grd.RootTable.Columns["TeamWorkerName"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns["RejectionReasonRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			grd.RootTable.Columns.Add("Delete");
			grd.RootTable.Columns["Delete"].Key = "Delete";
			grd.RootTable.Columns["Delete"].Caption = "X";
			grd.RootTable.Columns["Delete"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grd.RootTable.Columns["Delete"].ButtonStyle = (ButtonStyle)5;
			grd.RootTable.Columns["Delete"].ButtonText = "X";
			grd.RootTable.Columns["Delete"].Width = 20;
			grd.RootTable.Columns["Delete"].Position = 0;
			grd.FrozenColumns = 2;
			((Control)(object)grd).Refresh();
			ctrlGrdBar1_Load(null, null);
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
			if (FormValidationDetail())
			{
				table.Rows.Add(0, Conversion.ToInt(CmbItem.Value), Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value), Conversion.ToInt(CmbPackUom.Value), Conversion.ToString((object)((Control)(object)CmbPackUom).Text), Conversion.ToInt(CmbVariantDescription.Value), Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text), Conversion.ToInt(txtBatchNo.Value), Conversion.ToString((object)((Control)(object)txtBatchNo).Text), Conversion.ToInt(CmbCastingType.Value), Conversion.ToString((object)((Control)(object)CmbCastingType).Text), Conversion.ToInt(CmbWareHouseFromDetail.Value), Conversion.ToInt(CmbWarehouseTo.Value), Conversion.ToInt(CmbProductionStageFrom.Value), Conversion.ToInt(CmbProductionStageTo.Value), Conversion.ToInt(CmbWorkStationFromDetail.Value), Conversion.ToDouble((object)txtAvailableQty.Text), Conversion.ToDouble((object)txtTransferQty.Text), Conversion.ToDouble((object)txtAvgRate.Text), Conversion.ToInt(CmbRateUom.Value), Conversion.ToString((object)((Control)(object)CmbRateUom).Text), Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells["Equivalent"].Value), Conversion.ToDouble((object)txtAmount.Text), Conversion.ToString((object)txtRemarksDetail.Text), Conversion.ToInt(CmbRejectionReasonType.Value), Conversion.ToString((object)txtTeamWorkerName.Text), Conversion.ToString((object)txtRejectionReasonRemarks.Text));
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FromResetDetail();
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
			if (FormValidationDetail())
			{
				table.Rows[updateDetailIndex]["ItemId"] = Conversion.ToInt(CmbItem.Value);
				table.Rows[updateDetailIndex]["ItemName"] = Conversion.ToString(((UltraDropDownBase)CmbItem).SelectedRow.Cells["ItemName"].Value);
				table.Rows[updateDetailIndex]["UomId"] = Conversion.ToInt(CmbPackUom.Value);
				table.Rows[updateDetailIndex]["Uom"] = Conversion.ToString((object)((Control)(object)CmbPackUom).Text);
				table.Rows[updateDetailIndex]["VariantId"] = Conversion.ToInt(CmbVariantDescription.Value);
				table.Rows[updateDetailIndex]["VariantDescription"] = Conversion.ToString((object)((Control)(object)CmbVariantDescription).Text);
				table.Rows[updateDetailIndex]["BatchNo"] = Conversion.ToString((object)((Control)(object)txtBatchNo).Text);
				table.Rows[updateDetailIndex]["CastingTypeId"] = Conversion.ToInt(CmbCastingType.Value);
				table.Rows[updateDetailIndex]["CastingType"] = Conversion.ToString((object)((Control)(object)CmbCastingType).Text);
				table.Rows[updateDetailIndex]["WareHouseFromId"] = Conversion.ToInt(CmbWareHouseFromDetail.Value);
				table.Rows[updateDetailIndex]["WareHouseToId"] = Conversion.ToInt(CmbWarehouseToDetail.Value);
				table.Rows[updateDetailIndex]["ProductionStageFromId"] = Conversion.ToInt(CmbProductionStageFromDetail.Value);
				table.Rows[updateDetailIndex]["ProductionStageToId"] = Conversion.ToInt(CmbProductionStageToDetail.Value);
				table.Rows[updateDetailIndex]["WorkStationFromId"] = Conversion.ToInt(CmbWorkStationFromDetail.Value);
				table.Rows[updateDetailIndex]["AvailableQty"] = Conversion.ToDouble((object)txtAvailableQty.Text);
				table.Rows[updateDetailIndex]["TransferQty"] = Conversion.ToDouble((object)txtTransferQty.Text);
				table.Rows[updateDetailIndex]["AvgRate"] = Conversion.ToDouble((object)txtAvgRate.Text);
				table.Rows[updateDetailIndex]["RateUomId"] = Conversion.ToInt(CmbRateUom.Value);
				table.Rows[updateDetailIndex]["RateUom"] = Conversion.ToString((object)((Control)(object)CmbRateUom).Text);
				table.Rows[updateDetailIndex]["RateEquivalent"] = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells["Equivalent"].Value);
				table.Rows[updateDetailIndex]["Amount"] = Conversion.ToDouble((object)txtAmount.Text);
				table.Rows[updateDetailIndex]["Remarks"] = txtRemarksDetail.Text;
				table.Rows[updateDetailIndex]["RejectionReasonTypeId"] = Conversion.ToInt(CmbRejectionReasonType.Value);
				table.Rows[updateDetailIndex]["TeamWorkerName"] = txtTeamWorkerName.Text;
				table.Rows[updateDetailIndex]["RejectionReasonRemarks"] = txtRejectionReasonRemarks.Text;
				grd.DataSource = table;
				FromResetDetail();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_DoubleClick(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0 && item.Cells.Count > 0)
			{
				updateDetailIndex = item.RowIndex;
				((Control)(object)CmbCategory).Text = "";
				CmbCategory_Leave(null, null);
				CmbItem.Value = Conversion.ToInt(item.Cells["ItemId"].Value);
				CmbItem_Leave(null, null);
				CmbPackUom.Value = Conversion.ToInt(item.Cells["UomId"].Value);
				CmbVariantDescription.Value = Conversion.ToInt(item.Cells["VariantId"].Value);
				((Control)(object)txtBatchNo).Text = Conversion.ToString(item.Cells["BatchNo"].Value);
				CmbCastingType.Value = Conversion.ToInt(item.Cells["CastingTypeId"].Value);
				CmbWareHouseFromDetail.Value = Conversion.ToInt(item.Cells["WareHouseFromId"].Value);
				CmbWarehouseToDetail.Value = Conversion.ToInt(item.Cells["WareHouseToId"].Value);
				CmbProductionStageFromDetail.Value = Conversion.ToInt(item.Cells["ProductionStageFromId"].Value);
				CmbProductionStageToDetail.Value = Conversion.ToInt(item.Cells["ProductionStageToId"].Value);
				CmbWorkStationFromDetail.Value = Conversion.ToInt(item.Cells["WorkStationFromId"].Value);
				txtTransferQty.Text = Conversion.ToDouble(item.Cells["TransferQty"].Value).ToString("#,##.###");
				txtAvgRate.Text = Conversion.ToDouble(item.Cells["AvgRate"].Value).ToString("#,##.###");
				CmbRateUom.Value = Conversion.ToInt(item.Cells["RateUomId"].Value);
				txtAmount.Text = Conversion.ToDouble(item.Cells["Amount"].Value).ToString("#,##.###");
				txtRemarksDetail.Text = Conversion.ToString(item.Cells["Remarks"].Value);
				CmbRejectionReasonType.Value = Conversion.ToInt(item.Cells["RejectionReasonTypeId"].Value);
				txtTeamWorkerName.Text = Conversion.ToString(item.Cells["TeamWorkerName"].Value);
				txtRejectionReasonRemarks.Text = Conversion.ToString(item.Cells["RejectionReasonRemarks"].Value);
				btnplus.Visible = false;
				btnUpdateDetail.Visible = true;
				btnCancelUpdateDetial.Visible = true;
				CmbItem.Focus();
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
			FromResetDetail();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grd_CellUpdated(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item != null && (int)item.RowType == 0)
			{
				if (e.Column.Key == "TransferQty" || e.Column.Key == "AvgRate")
				{
					double Qty = Conversion.ToDouble(item.Cells["TransferQty"].Value);
					double AvgRate = Conversion.ToDouble(item.Cells["AvgRate"].Value);
					double RateEquivalent = Conversion.ToDouble(item.Cells["RateEquivalent"].Value);
					double Amount = 0.0;
					if (Qty > 0.0 && AvgRate > 0.0 && RateEquivalent > 0.0)
					{
						Amount = Qty / RateEquivalent * AvgRate;
						item.Cells["Amount"].Value = Amount;
					}
					else
					{
						item.Cells["Amount"].Value = 0;
					}
				}
				GetAvailableQtyForGridRow(item);
			}
			grd.UpdateData();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void Insert()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected O, but got Unknown
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Expected O, but got Unknown
		try
		{
			if (!FormValidation())
			{
				return;
			}
			if (grd.GetRows().Count() == 0)
			{
				MessageBox.Show("Grid Record Not Found");
				return;
			}
			InvStockTransferHeader sh = new InvStockTransferHeader();
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Update?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
				{
					return;
				}
				sh.Id = RecId;
			}
			else if (MessageBox.Show("Are you sure to Save?", "Confirm", MessageBoxButtons.YesNo) == DialogResult.No)
			{
				return;
			}
			sh.OrganizationId = UserAccount.OrganizationId;
			sh.CompanyId = UserAccount.CompanyId;
			sh.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			sh.BranchesId = Conversion.ToInt((object)UserAccount.BranchesId);
			sh.ProjectsId = Conversion.ToInt((object)UserAccount.BranchesId);
			sh.DocumentTypeId = 1665;
			sh.EntryUserId = UserAccount.ID;
			sh.ModifyUserId = UserAccount.ID;
			sh.ApprovedUserId = Conversion.ToInt((object)UserAccount.ID);
			sh.EntryDate = DateTime.Now;
			sh.ModifyDate = DateTime.Now;
			sh.ApprovedDate = DateTime.Now;
			sh.IsApproved = false;
			sh.DocDate = Conversion.ToDateTime((object)DocDate.Value);
			sh.DocNo = Conversion.ToInt((object)txtdocno.Text);
			sh.TransferTypeId = Conversion.ToInt(CmbTransferType.Value);
			sh.RemarksHeader = Conversion.ToString((object)txtRemarksHead.Text);
			sh.InvStockTransferDetailList = new List<InvStockTransferDetail>();
			if (lstRemoveRecord.Count > 0 && RecId > 0)
			{
				for (int i = 0; i < lstRemoveRecord.Count; i++)
				{
					sh.InvStockTransferDetailList.Add(lstRemoveRecord[i]);
				}
			}
			GridEXRow[] rows = grd.GetRows();
			foreach (GridEXRow r in rows)
			{
				InvStockTransferDetail vd = new InvStockTransferDetail();
				vd.Id = ((RecId != 0) ? Conversion.ToInt(r.Cells["Id"].Value) : 0);
				vd.ActionTypeId = ((vd.Id <= 0) ? 1 : 2);
				vd.ItemId = Conversion.ToInt(r.Cells["ItemId"].Value);
				vd.ItemUomId = Conversion.ToInt(r.Cells["UomId"].Value);
				vd.ItemVariantId = Conversion.ToInt(r.Cells["VariantId"].Value);
				vd.BatchDescription = Conversion.ToString((object)r.Cells["BatchNo"].Text);
				if (sh.TransferTypeId != 6 && ((btnsave.Enabled && btnsave.Visible) || (btnSaveAs.Enabled && btnSaveAs.Visible)) && Conversion.ToDecimal(r.Cells["TransferQty"].Value) > Conversion.ToDecimal(r.Cells["AvailableQty"].Value))
				{
					throw new Exception($"Item Quantity cannot greater than Available Qty in Detail in Row No: {r.RowIndex + 1}");
				}
				vd.JobLotId = Conversion.ToInt(r.Cells["JobLotId"].Value);
				vd.CastingTypeId = Conversion.ToInt(r.Cells["CastingTypeId"].Value);
				vd.WarehouseFromId = Conversion.ToInt(r.Cells["WareHouseFromId"].Value);
				vd.WarehouseToId = Conversion.ToInt(r.Cells["WareHouseToId"].Value);
				vd.ProductionStageFromId = Conversion.ToInt(r.Cells["ProductionStageFromId"].Value);
				vd.ProductionStageToId = Conversion.ToInt(r.Cells["ProductionStageToId"].Value);
				vd.WorkStationFromId = Conversion.ToInt(r.Cells["WorkStationFromId"].Value);
				vd.ItemQty = Conversion.ToDecimal(r.Cells["TransferQty"].Value);
				vd.NetWeight = Conversion.ToDecimal(r.Cells["TransferQty"].Value);
				vd.ItemRate = Conversion.ToDecimal(r.Cells["AvgRate"].Value);
				vd.RateUomId = Conversion.ToInt(r.Cells["RateUomId"].Value);
				vd.ItemAmount = Conversion.ToDecimal(r.Cells["Amount"].Value);
				vd.RejectionReasonTypeId = Conversion.ToInt(r.Cells["RejectionReasonTypeId"].Value);
				vd.TeamWorkerName = Conversion.ToString((object)r.Cells["TeamWorkerName"].Text);
				vd.RejectionReasonRemarks = Conversion.ToString((object)r.Cells["RejectionReasonRemarks"].Text);
				vd.RemarksDetail = Conversion.ToString((object)r.Cells["Remarks"].Text);
				if (vd.ItemId <= 0)
				{
					throw new Exception($"Item field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.ItemUomId <= 0)
				{
					throw new Exception($"Uom field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (string.IsNullOrEmpty(vd.BatchDescription))
				{
					throw new Exception($"Batch No field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.WarehouseFromId <= 0)
				{
					throw new Exception($"Warehouse From field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.WarehouseToId <= 0)
				{
					throw new Exception($"Warehouse To field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.WarehouseFromId == vd.WarehouseToId)
				{
					throw new Exception($"Warehouse Stage From And To Cannot be same in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.ProductionStageFromId <= 0)
				{
					throw new Exception($"Production Stage From field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.ProductionStageToId <= 0)
				{
					throw new Exception($"Production Stage To field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (sh.TransferTypeId != 8 && vd.ProductionStageFromId == vd.ProductionStageToId)
				{
					throw new Exception($"Production Stage From And To Cannot be same in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.ItemQty <= 0m)
				{
					throw new Exception($"Item Quantity field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.ItemRate <= 0m)
				{
					throw new Exception($"Item Rate field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.RateUomId <= 0)
				{
					throw new Exception($"RateUom field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				if (vd.ItemAmount <= 0m)
				{
					throw new Exception($"Amount field is required in Detail in Row No: {r.RowIndex + 1}");
				}
				sh.InvStockTransferDetailList.Add(vd);
			}
			if (sh.InvStockTransferDetailList.Count == 0)
			{
				throw new Exception("At least enter value/quantity in one of the rows of detail ");
			}
			sh.AttachmentsList = new List<DMSAttachments>();
			sh.DeleteAttachmentsList = new List<DMSAttachments>();
			FormHelper.UpdateAttachmentsForObject<InvStockTransferHeader>(sh, base.Name, AT, "AttachmentsList", "DeleteAttachmentsList");
			sh.AttachmentsValues = AttachmentsValues;
			sh.CustomAttachmentsValues = CustomAttachmentsValues;
			int success = InvStockTransferHeader.Save(sh);
			if (RecId > 0)
			{
				MessageBox.Show("Data Update Successfully....  " + sh.DocNo);
				FormHelper.DeleteAllAttachmentsFromDb(AT, RecId, 1665, base.Name);
			}
			else
			{
				MessageBox.Show("Data Save Successfully....  " + sh.DocNo);
			}
			Reset();
			if (ChkBoxPrintPreview.Checked)
			{
				CommonServices.StockTransferforProductionrejection_Slip_1665(success);
			}
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
			RecId = 0;
			Insert();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnupdate_Click(object sender, EventArgs e)
	{
		try
		{
			if (IsApproved)
			{
				MessageBox.Show("Approved Record Not Update");
			}
			else
			{
				Insert();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void ReadById(int ID)
	{
		try
		{
			Reset();
			RecId = ID;
			InvStockTransferHeader Stock = InvStockTransferHeader.ReadById(ID);
			if (Stock != null)
			{
				btnsave.Visible = false;
				btnupdate.Visible = true;
				btnDelete.Visible = true;
				tabControl1.SelectedIndex = 0;
				((Control)(object)CmbTransferType).Enabled = (btnsave.Enabled && btnsave.Visible) || (btnSaveAs.Enabled && btnSaveAs.Visible);
				DocDate.Value = Conversion.ToDateTime((object)Stock.DocDate);
				txtdocno.Text = Conversion.ToString((object)Stock.DocNo);
				CmbTransferType.Value = Conversion.ToInt((object)Stock.TransferTypeId);
				CmbTransferType_Leave(null, null);
				txtRemarksHead.Text = Conversion.ToString((object)Stock.RemarksHeader);
				IsApproved = Stock.IsApproved;
				table.Clear();
				for (int i = 0; i < Stock.InvStockTransferDetailList.Count; i++)
				{
					table.Rows.Add(Stock.InvStockTransferDetailList[i].Id, Stock.InvStockTransferDetailList[i].ItemId, Stock.InvStockTransferDetailList[i].ItemName, Stock.InvStockTransferDetailList[i].ItemUomId, Stock.InvStockTransferDetailList[i].ItemUom, Stock.InvStockTransferDetailList[i].ItemVariantId, Stock.InvStockTransferDetailList[i].VariantDescription, Stock.InvStockTransferDetailList[i].JobLotId, Stock.InvStockTransferDetailList[i].BatchDescription, Stock.InvStockTransferDetailList[i].CastingTypeId, Stock.InvStockTransferDetailList[i].CastingType, Stock.InvStockTransferDetailList[i].WarehouseFromId, Stock.InvStockTransferDetailList[i].WarehouseToId, Stock.InvStockTransferDetailList[i].ProductionStageFromId, Stock.InvStockTransferDetailList[i].ProductionStageToId, Stock.InvStockTransferDetailList[i].WorkStationFromId, Stock.InvStockTransferDetailList[i].ItemQty, Stock.InvStockTransferDetailList[i].ItemQty, Stock.InvStockTransferDetailList[i].ItemRate, Stock.InvStockTransferDetailList[i].RateUomId, Stock.InvStockTransferDetailList[i].RateUom, Stock.InvStockTransferDetailList[i].RateUomEquivalent, Stock.InvStockTransferDetailList[i].ItemAmount, Stock.InvStockTransferDetailList[i].RemarksDetail, Stock.InvStockTransferDetailList[i].RejectionReasonTypeId, Stock.InvStockTransferDetailList[i].TeamWorkerName, Stock.InvStockTransferDetailList[i].RejectionReasonRemarks);
				}
				if (table.Rows.Count == 0)
				{
					AddRowInGrid();
				}
				grd.DataSource = table;
				grd.RetrieveStructure();
				grdSettings();
				FormHelper.LoadAttachmentsForObject(AT, RecId, base.Name);
				DocDate.Focus();
				return;
			}
			RecId = 0;
			throw new Exception("Record not found");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnDelete_Click(object sender, EventArgs e)
	{
		try
		{
			if (RecId > 0)
			{
				if (MessageBox.Show("Are you sure to Delete?", "Confirm", MessageBoxButtons.YesNo) != DialogResult.No)
				{
					InvStockTransferHeader.DeleteByID(UserAccount.ID, RecId);
					MessageBox.Show("Delete Record Successfully");
					Reset();
				}
			}
			else
			{
				MessageBox.Show("RecId not Found");
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void btnSaveAs_Click(object sender, EventArgs e)
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

	private void txtdocno_Leave(object sender, EventArgs e)
	{
	}

	private void CalculateAmount()
	{
		try
		{
			double qty = Conversion.ToDouble((object)txtTransferQty.Text);
			double Rate = Conversion.ToDouble((object)txtAvgRate.Text);
			double Equivalent = 0.0;
			double Amount = 0.0;
			if (((UltraGridBase)CmbRateUom).ActiveRow != null && Conversion.ToInt(CmbRateUom.Value) > 0)
			{
				Equivalent = Conversion.ToDouble(((UltraDropDownBase)CmbRateUom).SelectedRow.Cells["Equivalent"].Value);
			}
			if (qty > 0.0 && Equivalent > 0.0 && Rate > 0.0)
			{
				Amount = Math.Round(qty / Equivalent * Rate, 2, MidpointRounding.AwayFromZero);
				txtAmount.Text = Amount.ToString("#,##0.##");
			}
			else
			{
				txtAmount.Text = "0";
			}
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private double GetCgsAvgRate()
	{
		try
		{
			double AvgRate = 0.0;
			int ItemId = Conversion.ToInt(CmbItem.Value);
			int warehouseId = Conversion.ToInt(CmbWareHouseFromDetail.Value);
			if (ItemId > 0)
			{
				AvgRate = CommonServices.AvgRateOnlyForCGS(ItemId, DocDate.Value, 1665, RecId, 0, 0, null, warehouseId);
			}
			return AvgRate;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void GetAvailableQtyForTextBox()
	{
		try
		{
			Dictionary<string, string> uiValues = new Dictionary<string, string>
			{
				{
					"ItemId",
					Conversion.ToString(CmbItem.Value)
				},
				{
					"ItemUomId",
					Conversion.ToString(CmbPackUom.Value)
				},
				{
					"JobLotId",
					Conversion.ToString(txtBatchNo.Value)
				},
				{
					"WarehouseId",
					Conversion.ToString(CmbWarehouseFrom.Value)
				},
				{
					"ProductionStageId",
					Conversion.ToString(CmbProductionStageFrom.Value)
				},
				{
					"CastingTypeId",
					Conversion.ToString(CmbCastingType.Value)
				}
			};
			double availableQty = CalculateAvailableQty(uiValues);
			txtAvailableQty.Text = availableQty.ToString("#,##0.##");
		}
		catch (Exception ex)
		{
			MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private double CalculateAvailableQty(Dictionary<string, string> values)
	{
		try
		{
			Dictionary<string, string> filteredValues = values.Where((KeyValuePair<string, string> kv) => !string.IsNullOrEmpty(kv.Value) && Conversion.ToDouble((object)kv.Value) > 0.0).ToDictionary((KeyValuePair<string, string> kv) => kv.Key, (KeyValuePair<string, string> kv) => kv.Value);
			if (filteredValues.Count == 0)
			{
				return 0.0;
			}
			return (from row in dtAvailableStockData.AsEnumerable()
				where filteredValues.All((KeyValuePair<string, string> kv) => row.Table.Columns.Contains(kv.Key) && row[kv.Key]?.ToString() == kv.Value)
				select row).Sum((DataRow row) => row.Field<double?>("AvailableQty").GetValueOrDefault());
		}
		catch (Exception ex)
		{
			throw new Exception("Error in CalculateAvailableQty: " + ex.Message);
		}
	}

	private void GetAvailableQtyForGridRow(GridEXRow r)
	{
		try
		{
			Dictionary<string, string> rowValues = new Dictionary<string, string>
			{
				{
					"ItemId",
					r.Cells["ItemId"].Value?.ToString()
				},
				{
					"ItemUomId",
					r.Cells["UomId"].Value?.ToString()
				},
				{
					"JobLotId",
					r.Cells["JobLotId"].Value?.ToString()
				},
				{
					"WarehouseId",
					Conversion.ToString(CmbWarehouseFrom.Value)
				},
				{
					"ProductionStageId",
					Conversion.ToString(CmbProductionStageFrom.Value)
				},
				{
					"CastingTypeId",
					r.Cells["CastingTypeId"].Value?.ToString()
				}
			};
			double availableQty = CalculateAvailableQty(rowValues);
			r.Cells["AvailableQty"].Value = availableQty;
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	private void txtTransferQty_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtAvgRate_TextChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbRateUom_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			CalculateAmount();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void DocDate_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			txtAvgRate.Text = GetCgsAvgRate().ToString("#,##0.###");
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbVariantDescription_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetAvailableQtyForTextBox();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void txtBatchNo_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableQtyForTextBox();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbCastingType_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			GetAvailableQtyForTextBox();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbPackUom_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableQtyForTextBox();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWorkStationFromDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableQtyForTextBox();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void PurchsaeOrder_KeyDown(object sender, KeyEventArgs e)
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
					DocDate.Focus();
				}
				else
				{
					tabControl1.SelectedIndex = 1;
					fromdate.Focus();
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
				if (e.Control && e.Shift && e.KeyCode == Keys.Delete && formrights.DoHaveCanDelete && btnDelete.Enabled)
				{
					btnDelete_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.S && btnsave.Visible && btnsave.Enabled)
				{
					btnsave_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.U && btnupdate.Visible && btnupdate.Enabled)
				{
					btnupdate_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.P && btnprint.Visible && btnprint.Enabled)
				{
					btnprint_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.N)
				{
					btnnew_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.R)
				{
					btnRefresh_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.F5)
				{
					DocDate.Focus();
				}
				if (e.Control && e.KeyCode == Keys.F10)
				{
					AT.Show();
				}
				if (e.Control && e.KeyCode == Keys.F12 && btnsave.Visible && btnsave.Enabled)
				{
					btnSaveAs_Click(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)grd).Focus();
				}
				if (e.Control)
				{
					_ = e.KeyCode;
					_ = 38;
				}
				if (e.Control && e.KeyCode != Keys.Return)
				{
				}
			}
			else if (tabControl1.SelectedIndex == 1)
			{
				if (e.Control && e.KeyCode == Keys.Return)
				{
					grdhistory_DoubleClick_1(null, null);
				}
				if (e.Control && e.KeyCode == Keys.Down)
				{
					((Control)(object)GridDetailHistory).Focus();
				}
				if (e.Control && e.KeyCode == Keys.S)
				{
					btnshow_Click(null, null);
				}
				else if (e.Control && e.KeyCode == Keys.Up)
				{
					((Control)(object)grdhistory).Focus();
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
			dt.Rows.Add("Ctrl+S", "For Save When on Entry form and For Show Data when on History Form");
			dt.Rows.Add("Ctrl+U", "For Update");
			dt.Rows.Add("Ctrl+Shift+Delete", "For Delete");
			dt.Rows.Add("Ctrl+E", "For Close");
			dt.Rows.Add("Ctrl+R", "For Refresh");
			dt.Rows.Add("Ctrl+N", "For New");
			dt.Rows.Add("Ctrl+P", "For Print");
			dt.Rows.Add("Ctrl+F5", "For Focus on Doc Date");
			dt.Rows.Add("Ctrl+F10", "For Open Attachments");
			dt.Rows.Add("Ctrl+F12", "For Save As");
			dt.Rows.Add("F1", "For Combo Lookup on Current Column of any Focused Grid");
			dt.Rows.Add("Ctrl+T", "For Tab Transfer");
			dt.Rows.Add("Ctrl+alt", "To Show ShortCut Keys Form");
			dt.Rows.Add("Ctrl+D", "For Adding an row in Focused Grid");
			dt.Rows.Add("Ctrl+Delete", "For Deleting an row of Focused Grid");
			dt.Rows.Add("Ctrl+ArrowDown", "For Focus On Detail Grid");
			dt.Rows.Add("Ctrl+ArrowUp", "For Focus On on Doc Date");
			dt.Rows.Add("Ctrl+ArrowRight", "to change focus from one grid to another");
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

	private void grd_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grd.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grd.CurrentColumn == null)
			{
				return;
			}
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grd.CurrentColumn.Key == "Delete")
				{
					DeleteDetailRow(item);
				}
				if (grd.CurrentColumn.Key == "Edit")
				{
					AddRowInGrid();
				}
			}
			if (e.Control && e.KeyCode == Keys.Delete)
			{
				DeleteDetailRow(item);
			}
			if (e.Control && e.KeyCode == Keys.D)
			{
				AddRowInGrid();
			}
			if (e.KeyCode == Keys.F1)
			{
				int Id = 0;
				if (grd.CurrentColumn != null && (grd.CurrentColumn.Key == "VariantId" || grd.CurrentColumn.Key == "VariantDescription"))
				{
					if (Conversion.ToInt(item.Cells["ItemId"].Value) <= 0)
					{
						MessageBox.Show("Please Select an Item First");
						return;
					}
					GrdPopUp grdPopUp = new GrdPopUp(ItemAttributeVarient.GetAllForCombo(UserAccount.OrganizationId, UserAccount.CompanyId, Conversion.ToInt(item.Cells["ItemId"].Value)), "ItemAttributeVarientId", "ItemAttribute");
					grdPopUp.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp.ShowDialog();
					grdPopUp.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp.ReturnId);
					string Name = Conversion.ToString((object)grdPopUp.ReturnName);
					item.Cells["VariantId"].Value = Id;
					item.Cells["VariantDescription"].Value = Name;
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "WorkStationFromId")
				{
					GrdPopUp grdPopUp2 = new GrdPopUp(dtWorkStation, "Id", "WorkStationName");
					grdPopUp2.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp2.ShowDialog();
					grdPopUp2.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp2.ReturnId);
					item.Cells["WorkStationFromId"].Value = Id;
				}
				if (grd.CurrentColumn != null && grd.CurrentColumn.Key == "RejectionReasonTypeId")
				{
					GrdPopUp grdPopUp3 = new GrdPopUp(dtRejectionReasonType, "Id", "type");
					grdPopUp3.StartPosition = FormStartPosition.CenterScreen;
					grdPopUp3.ShowDialog();
					grdPopUp3.BringToFront();
					Id = Conversion.ToInt((object)grdPopUp3.ReturnId);
					item.Cells["RejectionReasonTypeId"].Value = Id;
				}
				GetAvailableQtyForGridRow(item);
				grd.UpdateData();
			}
			((Control)(object)grd).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdHistory_KeyDown(object sender, KeyEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item == null || (int)item.RowType != 0 || grdhistory.CurrentColumn == null)
			{
				return;
			}
			int CurrId = Conversion.ToInt((object)item.Cells["Id"].Value.ToString());
			if (e.Control && e.KeyCode == Keys.Space)
			{
				if (grdhistory.CurrentColumn.Key == "Edit" && formrights.DoHaveUpdateRights)
				{
					RecId = CurrId;
					ReadById(RecId);
				}
				if (grdhistory.CurrentColumn.Key == "Print" && formrights.DoHavePrintRights)
				{
					CommonServices.StockTransferforProductionrejection_Slip_1665(CurrId);
				}
				if (grdhistory.CurrentColumn.Key == "NoOfAttachments")
				{
					CommonServices.GetNoofAttachmentsByRefDocumentTypeID(CurrId, 1665);
				}
			}
			if (e.Control && e.KeyCode == Keys.Return && formrights.DoHaveUpdateRights)
			{
				RecId = CurrId;
				ReadById(RecId);
			}
			if (e.Control && e.KeyCode == Keys.P && formrights.DoHavePrintRights)
			{
				CommonServices.StockTransferforProductionrejection_Slip_1665(CurrId);
			}
			((Control)(object)grdhistory).Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ParameterFill()
	{
		try
		{
			DataTable dt = CommonServices.DateType();
			DDL.BindDDL(dt, cmbperemeter, "Id", "Parameters", "Parameters", ZeroIndex: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void cmbperemeter_ValueChanged(object sender, EventArgs e)
	{
		try
		{
			if (Convert.ToInt32(cmbperemeter.Value) == 1)
			{
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 2)
			{
				fromdate.Value = DateTime.Today.AddDays(-7.0);
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 3)
			{
				DateTime now = DateTime.UtcNow;
				DateTime first = now.Date.AddDays(-(now.Date.Day - 1));
				fromdate.Value = first;
				ToDate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 4)
			{
				int year = DateTime.Now.Year;
				DateTime first2 = new DateTime(year, 1, 1);
				ToDate.Value = first2;
				fromdate.Value = DateTime.Today;
			}
			else if (Convert.ToInt32(cmbperemeter.Value) == 5)
			{
				fromdate.Value = Conversion.ToDateTime((object)clsGlobalVariables.ActiveYr.Start_Period);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (tabControl1.SelectedIndex == 1)
		{
			cmbperemeter.Focus();
		}
		else
		{
			DocDate.Focus();
		}
	}

	private void grdhistory_DoubleClick_1(object sender, EventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				RecId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
				ReadById(RecId);
			}
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
			gridhistoryfill();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void gridhistoryfill()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		try
		{
			ReportsParameters obj = new ReportsParameters();
			obj.OrganizationId = UserAccount.OrganizationId;
			obj.CompanyId = UserAccount.CompanyId;
			obj.BranchesId = UserAccount.BranchesId;
			obj.FinancialYearId = clsGlobalVariables.ActiveYr.Id;
			obj.DocumentTypeId = 1665;
			if (fromdate.Checked)
			{
				obj.FromDate = Conversion.ToDateTime((object)fromdate.Value);
			}
			if (ToDate.Checked)
			{
				obj.ToDate = Conversion.ToDateTime((object)ToDate.Value);
			}
			obj.ToDocNo = Conversion.ToInt((object)txtDocNoTo.Text);
			obj.FromDocNo = Conversion.ToInt((object)txtDocNoFrom.Text);
			obj.CanViewAllRecord = formrights.DoHaveCanViewAllRecordRights;
			if (!obj.CanViewAllRecord)
			{
				obj.EntryUser = UserAccount.ID;
			}
			DataTable dthistoryfill = InvStockTransferHeader.FormHistory(obj);
			if (dthistoryfill.Rows.Count > 0)
			{
				DataTable dtGridHistory = new DataTable();
				dtGridHistory.Columns.Add("Id", typeof(int));
				dtGridHistory.Columns.Add("DocumentTypeId", typeof(int));
				dtGridHistory.Columns.Add("DocNo");
				dtGridHistory.Columns.Add("DocDate", typeof(DateTime));
				dtGridHistory.Columns.Add("TransferType");
				dtGridHistory.Columns.Add("FromWarehouse");
				dtGridHistory.Columns.Add("ToWarehouse");
				dtGridHistory.Columns.Add("ProductionStageFrom");
				dtGridHistory.Columns.Add("ProductionStageTo");
				dtGridHistory.Columns.Add("EntryDate", typeof(DateTime));
				dtGridHistory.Columns.Add("EntryUser", typeof(string));
				dtGridHistory.Columns.Add("ModifyDate", typeof(DateTime));
				dtGridHistory.Columns.Add("ModifyUser");
				dtGridHistory.Columns.Add("RemarksHeader");
				dtGridHistory.Columns.Add("NoOfAttachments");
				dtGridHistory.Clear();
				for (int i = 0; i < dthistoryfill.Rows.Count; i++)
				{
					dtGridHistory.Rows.Add(dthistoryfill.Rows[i]["Id"], dthistoryfill.Rows[i]["DocumentTypeId"], dthistoryfill.Rows[i]["DocNo"], Conversion.ToDateTime(dthistoryfill.Rows[i]["DocDate"]).ToShortDateString(), dthistoryfill.Rows[i]["TransferType"], dthistoryfill.Rows[i]["FromWarehouse"], dthistoryfill.Rows[i]["ToWarehouse"], dthistoryfill.Rows[i]["ProductionStageFrom"], dthistoryfill.Rows[i]["ProductionStageTo"], dthistoryfill.Rows[i]["EntryDate"], dthistoryfill.Rows[i]["EntryUserName"], dthistoryfill.Rows[i]["ModifyDate"], dthistoryfill.Rows[i]["ModifyUserName"], dthistoryfill.Rows[i]["RemarksHeader"], dthistoryfill.Rows[i]["NoOfAttachments"]);
				}
				grdhistory.DataSource = dtGridHistory;
				grdhistory.RetrieveStructure();
				HistoryGridSettings();
			}
			else
			{
				grdhistory.ClearStructure();
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void HistoryGridSettings()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			grdhistory.RootTable.Columns["Id"].Visible = false;
			grdhistory.RootTable.Columns["DocumentTypeId"].Visible = false;
			grdhistory.RootTable.HeaderLines = 2;
			foreach (GridEXColumn item in (JanusCollectionBase)grdhistory.RootTable.Columns)
			{
				item.HeaderAlignment = (TextAlignment)2;
			}
			grdhistory.RootTable.Columns["DocNo"].Width = Constants.InventoryConstants.DocNoConstant;
			grdhistory.RootTable.Columns["DocDate"].Width = Constants.InventoryConstants.DateConstant;
			grdhistory.RootTable.Columns["TransferType"].Width = Constants.InventoryConstants.ItemType;
			grdhistory.RootTable.Columns["FromWarehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdhistory.RootTable.Columns["ToWarehouse"].Width = Constants.InventoryConstants.WareHouseName;
			grdhistory.RootTable.Columns["ProductionStageFrom"].Width = Constants.InventoryConstants.ProductionStage;
			grdhistory.RootTable.Columns["ProductionStageTo"].Width = Constants.InventoryConstants.ProductionStage;
			grdhistory.RootTable.Columns["EntryDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdhistory.RootTable.Columns["EntryUser"].Width = Constants.InventoryConstants.EntryUserConstants;
			grdhistory.RootTable.Columns["ModifyDate"].Width = Constants.InventoryConstants.DateTimeConstantddMMM;
			grdhistory.RootTable.Columns["ModifyUser"].Width = Constants.InventoryConstants.ModifyUserConstants;
			grdhistory.RootTable.Columns["RemarksHeader"].Width = Constants.InventoryConstants.RemarksConstants;
			grdhistory.RootTable.Columns["NoOfAttachments"].Width = Constants.InventoryConstants.NoOfAttachmentsConstants;
			grdhistory.RootTable.Columns["EntryDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["ModifyDate"].FormatString = "dd-MMM-yy hh:mm tt";
			grdhistory.RootTable.Columns["NoOfAttachments"].ColumnType = (ColumnType)5;
			grdhistory.RootTable.Columns.Add("Edit");
			grdhistory.RootTable.Columns["Edit"].Caption = "Edit";
			grdhistory.RootTable.Columns["Edit"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Edit"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Edit"].ButtonText = "Edit";
			grdhistory.RootTable.Columns["Edit"].Width = 40;
			grdhistory.RootTable.Columns["Edit"].Position = 0;
			grdhistory.RootTable.Columns.Add("Print");
			grdhistory.RootTable.Columns["Print"].Caption = "Print";
			grdhistory.RootTable.Columns["Print"].ButtonDisplayMode = (CellButtonDisplayMode)3;
			grdhistory.RootTable.Columns["Print"].ButtonStyle = (ButtonStyle)5;
			grdhistory.RootTable.Columns["Print"].ButtonText = "Print";
			grdhistory.RootTable.Columns["Print"].Width = 40;
			grdhistory.RootTable.Columns["Print"].Position = 1;
			grdhistory.FrozenColumns = 2;
			ctrlGrdBar3_Load(null, null);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.ToString());
		}
	}

	private void grdhistory_LinkClicked(object sender, ColumnActionEventArgs e)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow item = grdhistory.CurrentRow;
			if (item != null && (int)item.RowType == 0 && e.Column.Key == "NoOfAttachments")
			{
				CommonServices.GetNoofAttachmentsByRefDocumentTypeID(Conversion.ToInt(item.Cells["Id"].Value), 1665);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_ColumnButtonClick(object sender, ColumnActionEventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow r = grdhistory.CurrentRow;
			if (r != null && (int)r.RowType == 0)
			{
				if (e.Column.Key == "Edit")
				{
					Reset();
					RecId = Conversion.ToInt((object)r.Cells["Id"].Value.ToString());
					ReadById(RecId);
				}
				if (e.Column.Key == "Print")
				{
					CommonServices.StockTransferforProductionrejection_Slip_1665(Conversion.ToInt((object)r.Cells["Id"].Value.ToString()));
				}
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
			fromdate.Value = DateTime.Now.AddDays(-3.0);
			ToDate.Value = DateTime.Now;
			txtDocNoFrom.Clear();
			txtDocNoTo.Clear();
			grdhistory.DataSource = null;
			GridDetailHistory.DataSource = null;
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void grdhistory_SelectionChanged(object sender, EventArgs e)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0528: Expected O, but got Unknown
		//IL_0528: Unknown result type (might be due to invalid IL or missing references)
		//IL_052f: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			GridEXRow row = grdhistory.CurrentRow;
			if (row == null || (int)row.RowType != 0 || row.Cells.Count <= 0)
			{
				return;
			}
			InvStockTransferHeader Stock = InvStockTransferHeader.ReadById(Conversion.ToInt(row.Cells["Id"].Value));
			if (Stock.InvStockTransferDetailList != null && Stock.InvStockTransferDetailList.Count > 0)
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Id", typeof(int));
				dt.Columns.Add("ItemId", typeof(int));
				dt.Columns.Add("ItemName", typeof(string));
				dt.Columns.Add("UomId", typeof(int));
				dt.Columns.Add("Uom", typeof(string));
				dt.Columns.Add("VariantId", typeof(int));
				dt.Columns.Add("VariantDescription", typeof(string));
				dt.Columns.Add("BatchNo", typeof(string));
				dt.Columns.Add("CastingType", typeof(string));
				dt.Columns.Add("WorkStationFrom", typeof(string));
				dt.Columns.Add("TransferQty", typeof(double));
				dt.Columns.Add("AvgRate", typeof(double));
				dt.Columns.Add("RateUom", typeof(string));
				dt.Columns.Add("Amount", typeof(double));
				dt.Columns.Add("Remarks", typeof(string));
				dt.Columns.Add("RejectionReasonType", typeof(string));
				dt.Columns.Add("TeamWorkerName", typeof(string));
				dt.Columns.Add("RejectionReasonRemarks", typeof(string));
				for (int i = 0; i < Stock.InvStockTransferDetailList.Count; i++)
				{
					dt.Rows.Add(Stock.InvStockTransferDetailList[i].Id, Stock.InvStockTransferDetailList[i].ItemId, Stock.InvStockTransferDetailList[i].ItemName, Stock.InvStockTransferDetailList[i].ItemUomId, Stock.InvStockTransferDetailList[i].ItemUom, Stock.InvStockTransferDetailList[i].ItemVariantId, Stock.InvStockTransferDetailList[i].VariantDescription, Stock.InvStockTransferDetailList[i].BatchDescription, Stock.InvStockTransferDetailList[i].CastingType, Stock.InvStockTransferDetailList[i].WorkStationFrom, Stock.InvStockTransferDetailList[i].ItemQty, Stock.InvStockTransferDetailList[i].ItemRate, Stock.InvStockTransferDetailList[i].RateUom, Stock.InvStockTransferDetailList[i].ItemAmount, Stock.InvStockTransferDetailList[i].RemarksDetail, Stock.InvStockTransferDetailList[i].RejectionReasonType, Stock.InvStockTransferDetailList[i].TeamWorkerName, Stock.InvStockTransferDetailList[i].RejectionReasonRemarks);
				}
				GridDetailHistory.DataSource = dt;
				GridDetailHistory.RetrieveStructure();
				GridDetailHistory.ColumnAutoResize = false;
				GridDetailHistory.RootTable.Columns["Id"].Visible = false;
				GridDetailHistory.RootTable.Columns["ItemId"].Visible = false;
				GridDetailHistory.RootTable.Columns["UomId"].Visible = false;
				GridDetailHistory.RootTable.Columns["VariantId"].Visible = false;
				GridDetailHistory.RootTable.Columns["TeamWorkerName"].Caption = "Reason Type Description";
				GridDetailHistory.RootTable.HeaderLines = 2;
				foreach (GridEXColumn item in (JanusCollectionBase)GridDetailHistory.RootTable.Columns)
				{
					item.HeaderAlignment = (TextAlignment)2;
					CommonServices.GridColumnSettings(item, 3);
					item.EditType = (EditType)0;
					item.FilterEditType = (FilterEditType)((!(item.Type == typeof(DateTime))) ? 1 : 6);
				}
				GridDetailHistory.RootTable.Columns["ItemName"].Width = Constants.InventoryConstants.ItemName;
				GridDetailHistory.RootTable.Columns["Uom"].Width = Constants.InventoryConstants.UomConstant;
				GridDetailHistory.RootTable.Columns["VariantDescription"].Width = Constants.InventoryConstants.VariantEquivalent;
				GridDetailHistory.RootTable.Columns["BatchNo"].Width = Constants.InventoryConstants.OtherDescription;
				GridDetailHistory.RootTable.Columns["CastingType"].Width = Constants.InventoryConstants.CastingType;
				GridDetailHistory.RootTable.Columns["WorkStationFrom"].Width = Constants.InventoryConstants.WorkStationName;
				GridDetailHistory.RootTable.Columns["TransferQty"].Width = Constants.InventoryConstants.QtyConstant;
				GridDetailHistory.RootTable.Columns["AvgRate"].Width = Constants.InventoryConstants.ExRateConstant;
				GridDetailHistory.RootTable.Columns["RateUom"].Width = Constants.InventoryConstants.UomConstant;
				GridDetailHistory.RootTable.Columns["Amount"].Width = Constants.InventoryConstants.AmountConstant;
				GridDetailHistory.RootTable.Columns["Remarks"].Width = Constants.InventoryConstants.RemarksConstants;
				GridDetailHistory.RootTable.Columns["RejectionReasonType"].Width = Constants.InventoryConstants.PackingType;
				GridDetailHistory.RootTable.Columns["TeamWorkerName"].Width = Constants.InventoryConstants.RemarksConstants;
				GridDetailHistory.RootTable.Columns["RejectionReasonRemarks"].Width = Constants.InventoryConstants.RemarksConstants;
			}
			else
			{
				GridDetailHistory.ClearStructure();
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
				GridLayoutService.EnsureGridLayoutMatchesDataTable(base.Name, grd, table);
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void ctrlGrdBar3_Load(object sender, EventArgs e)
	{
		try
		{
			FileStream fs = CommonServices.GetGridLayout(base.Name, ((Control)(object)grdhistory).Name);
			if (fs != null)
			{
				grdhistory.LoadLayoutFile((Stream)fs);
				fs.Close();
				fs.Dispose();
			}
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
			CommonServices.StockTransferforProductionrejection_Slip_1665(RecId);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
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
			MessageBox.Show(ex.Message);
		}
	}

	private void txtDocNoFrom_KeyPress(object sender, KeyPressEventArgs e)
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

	private void txtDocNoTo_KeyPress(object sender, KeyPressEventArgs e)
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

	private void DecimalNumberKeySetting_KeyPress(object sender, KeyPressEventArgs e)
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

	private void BtnLoadStock_Click(object sender, EventArgs e)
	{
		try
		{
			if (((UltraGridBase)CmbTransferType).ActiveRow == null || Conversion.ToInt(CmbTransferType.Value) == 0)
			{
				throw new Exception("Please Select Transfer Type First...");
			}
			LoadSo = new frmLoadAvailableStockEngr(UserAccount);
			LoadSo.dtAvailableStock = dtAvailableStockData;
			LoadSo.ShowDialog();
			LoadSo.BringToFront();
			GetDetailByTransferTypeData(LoadSo.dtLoader);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	public void GetDetailByTransferTypeData(DataTable Stock)
	{
		try
		{
			if (Stock == null || Stock.Rows.Count <= 0)
			{
				return;
			}
			foreach (DataRow d in Stock.Rows)
			{
				table.Rows.Add(0, d["ItemId"], d["ItemName"], d["ItemUomId"], d["Uom"], 0, "", d["JobLotId"], d["BatchNo"], d["CastingTypeId"], d["CastingType"], d["WarehouseId"], Conversion.ToInt(CmbWarehouseTo.Value), d["ProductionStageId"], Conversion.ToInt(CmbProductionStageTo.Value), 0, d["AvailableQty"], d["AvailableQty"], d["AvgRate"], d["RateUomId"], d["RateUom"], d["RateEquivalent"], d["Amount"], "", 0, "", "");
			}
			if (table.Rows.Count == 0)
			{
				AddRowInGrid();
			}
			grd.DataSource = table;
			grd.RetrieveStructure();
			grdSettings();
			DocDate.Focus();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message);
		}
	}

	private void CmbWareHouseFromDetail_Leave(object sender, EventArgs e)
	{
		try
		{
			GetAvailableQtyForTextBox();
			txtAvgRate.Text = GetCgsAvgRate().ToString("#,##0.###");
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
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Expected O, but got Unknown
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Expected O, but got Unknown
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Expected O, but got Unknown
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Expected O, but got Unknown
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Expected O, but got Unknown
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0287: Expected O, but got Unknown
		//IL_0287: Unknown result type (might be due to invalid IL or missing references)
		//IL_028e: Expected O, but got Unknown
		//IL_028e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Expected O, but got Unknown
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Expected O, but got Unknown
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Expected O, but got Unknown
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Expected O, but got Unknown
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Expected O, but got Unknown
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Expected O, but got Unknown
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Expected O, but got Unknown
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Expected O, but got Unknown
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Expected O, but got Unknown
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Expected O, but got Unknown
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Expected O, but got Unknown
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Expected O, but got Unknown
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Expected O, but got Unknown
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Expected O, but got Unknown
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_030c: Expected O, but got Unknown
		//IL_030c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Expected O, but got Unknown
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Expected O, but got Unknown
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0321: Expected O, but got Unknown
		//IL_0321: Unknown result type (might be due to invalid IL or missing references)
		//IL_0328: Expected O, but got Unknown
		//IL_0328: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Expected O, but got Unknown
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0336: Expected O, but got Unknown
		//IL_0336: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Expected O, but got Unknown
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0344: Expected O, but got Unknown
		//IL_0344: Unknown result type (might be due to invalid IL or missing references)
		//IL_034b: Expected O, but got Unknown
		//IL_034b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Expected O, but got Unknown
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Expected O, but got Unknown
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_0360: Expected O, but got Unknown
		//IL_0360: Unknown result type (might be due to invalid IL or missing references)
		//IL_0367: Expected O, but got Unknown
		//IL_0367: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Expected O, but got Unknown
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0375: Expected O, but got Unknown
		//IL_0375: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Expected O, but got Unknown
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Expected O, but got Unknown
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_038a: Expected O, but got Unknown
		//IL_038a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Expected O, but got Unknown
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Expected O, but got Unknown
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Expected O, but got Unknown
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a6: Expected O, but got Unknown
		//IL_03a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b4: Expected O, but got Unknown
		//IL_03b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03bb: Expected O, but got Unknown
		//IL_03bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c2: Expected O, but got Unknown
		//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Expected O, but got Unknown
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d0: Expected O, but got Unknown
		//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d7: Expected O, but got Unknown
		//IL_03d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03de: Expected O, but got Unknown
		//IL_03de: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e5: Expected O, but got Unknown
		//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Expected O, but got Unknown
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f3: Expected O, but got Unknown
		//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Expected O, but got Unknown
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Expected O, but got Unknown
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		//IL_040f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0416: Expected O, but got Unknown
		//IL_0416: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Expected O, but got Unknown
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0424: Expected O, but got Unknown
		//IL_0424: Unknown result type (might be due to invalid IL or missing references)
		//IL_042b: Expected O, but got Unknown
		//IL_042b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0432: Expected O, but got Unknown
		//IL_0432: Unknown result type (might be due to invalid IL or missing references)
		//IL_0439: Expected O, but got Unknown
		//IL_0439: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Expected O, but got Unknown
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Expected O, but got Unknown
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_044e: Expected O, but got Unknown
		//IL_044e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Expected O, but got Unknown
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0463: Expected O, but got Unknown
		//IL_0463: Unknown result type (might be due to invalid IL or missing references)
		//IL_046a: Expected O, but got Unknown
		//IL_046a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0471: Expected O, but got Unknown
		//IL_0471: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Expected O, but got Unknown
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Expected O, but got Unknown
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Expected O, but got Unknown
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Expected O, but got Unknown
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Expected O, but got Unknown
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Expected O, but got Unknown
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a2: Expected O, but got Unknown
		//IL_04a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a9: Expected O, but got Unknown
		//IL_04a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b0: Expected O, but got Unknown
		//IL_04b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Expected O, but got Unknown
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Expected O, but got Unknown
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c5: Expected O, but got Unknown
		//IL_04c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cc: Expected O, but got Unknown
		//IL_04cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d3: Expected O, but got Unknown
		//IL_04d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04da: Expected O, but got Unknown
		//IL_04da: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e1: Expected O, but got Unknown
		//IL_04e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Expected O, but got Unknown
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ef: Expected O, but got Unknown
		//IL_04ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f6: Expected O, but got Unknown
		//IL_04f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Expected O, but got Unknown
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0504: Expected O, but got Unknown
		//IL_0504: Unknown result type (might be due to invalid IL or missing references)
		//IL_050b: Expected O, but got Unknown
		//IL_050b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0512: Expected O, but got Unknown
		//IL_0512: Unknown result type (might be due to invalid IL or missing references)
		//IL_0519: Expected O, but got Unknown
		//IL_0519: Unknown result type (might be due to invalid IL or missing references)
		//IL_0520: Expected O, but got Unknown
		//IL_0520: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Expected O, but got Unknown
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_052e: Expected O, but got Unknown
		//IL_052e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0535: Expected O, but got Unknown
		//IL_0535: Unknown result type (might be due to invalid IL or missing references)
		//IL_053c: Expected O, but got Unknown
		//IL_053c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Expected O, but got Unknown
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Expected O, but got Unknown
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Expected O, but got Unknown
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0558: Expected O, but got Unknown
		//IL_0558: Unknown result type (might be due to invalid IL or missing references)
		//IL_055f: Expected O, but got Unknown
		//IL_055f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Expected O, but got Unknown
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056d: Expected O, but got Unknown
		//IL_056d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Expected O, but got Unknown
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Expected O, but got Unknown
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0582: Expected O, but got Unknown
		//IL_0582: Unknown result type (might be due to invalid IL or missing references)
		//IL_0589: Expected O, but got Unknown
		//IL_0589: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Expected O, but got Unknown
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0597: Expected O, but got Unknown
		//IL_0597: Unknown result type (might be due to invalid IL or missing references)
		//IL_059e: Expected O, but got Unknown
		//IL_059e: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a5: Expected O, but got Unknown
		//IL_05a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ac: Expected O, but got Unknown
		//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b3: Expected O, but got Unknown
		//IL_05b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ba: Expected O, but got Unknown
		//IL_05ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c1: Expected O, but got Unknown
		//IL_05c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c8: Expected O, but got Unknown
		//IL_05c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cf: Expected O, but got Unknown
		//IL_05cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d6: Expected O, but got Unknown
		//IL_05d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05dd: Expected O, but got Unknown
		//IL_05dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e4: Expected O, but got Unknown
		//IL_05e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Expected O, but got Unknown
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f2: Expected O, but got Unknown
		//IL_05f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f9: Expected O, but got Unknown
		//IL_05f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Expected O, but got Unknown
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0607: Expected O, but got Unknown
		//IL_0607: Unknown result type (might be due to invalid IL or missing references)
		//IL_060e: Expected O, but got Unknown
		//IL_060e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0615: Expected O, but got Unknown
		//IL_0615: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Expected O, but got Unknown
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0623: Expected O, but got Unknown
		//IL_0623: Unknown result type (might be due to invalid IL or missing references)
		//IL_062a: Expected O, but got Unknown
		//IL_062a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Expected O, but got Unknown
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0638: Expected O, but got Unknown
		//IL_0638: Unknown result type (might be due to invalid IL or missing references)
		//IL_063f: Expected O, but got Unknown
		//IL_063f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0646: Expected O, but got Unknown
		//IL_0646: Unknown result type (might be due to invalid IL or missing references)
		//IL_064d: Expected O, but got Unknown
		//IL_064d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Expected O, but got Unknown
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_065b: Expected O, but got Unknown
		//IL_065b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0662: Expected O, but got Unknown
		//IL_0662: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Expected O, but got Unknown
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_0670: Expected O, but got Unknown
		//IL_0670: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Expected O, but got Unknown
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Expected O, but got Unknown
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_068c: Expected O, but got Unknown
		//IL_068c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0693: Expected O, but got Unknown
		//IL_0693: Unknown result type (might be due to invalid IL or missing references)
		//IL_069a: Expected O, but got Unknown
		//IL_069a: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Expected O, but got Unknown
		//IL_06ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_06f9: Expected O, but got Unknown
		//IL_0705: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Expected O, but got Unknown
		//IL_0710: Unknown result type (might be due to invalid IL or missing references)
		//IL_071a: Expected O, but got Unknown
		//IL_071b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0725: Expected O, but got Unknown
		//IL_0747: Unknown result type (might be due to invalid IL or missing references)
		//IL_0751: Expected O, but got Unknown
		//IL_0773: Unknown result type (might be due to invalid IL or missing references)
		//IL_077d: Expected O, but got Unknown
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Expected O, but got Unknown
		//IL_079f: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a9: Expected O, but got Unknown
		//IL_07b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_07bf: Expected O, but got Unknown
		//IL_07e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07eb: Expected O, but got Unknown
		//IL_07f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0801: Expected O, but got Unknown
		//IL_080d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0817: Expected O, but got Unknown
		//IL_0891: Unknown result type (might be due to invalid IL or missing references)
		//IL_089b: Expected O, but got Unknown
		//IL_08bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c7: Expected O, but got Unknown
		//IL_08c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d2: Expected O, but got Unknown
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0940: Expected O, but got Unknown
		//IL_0941: Unknown result type (might be due to invalid IL or missing references)
		//IL_094b: Expected O, but got Unknown
		//IL_0957: Unknown result type (might be due to invalid IL or missing references)
		//IL_0961: Expected O, but got Unknown
		//IL_096d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0977: Expected O, but got Unknown
		//IL_0999: Unknown result type (might be due to invalid IL or missing references)
		//IL_09a3: Expected O, but got Unknown
		//IL_0acd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ad7: Expected O, but got Unknown
		//IL_0aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af8: Expected O, but got Unknown
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b50: Expected O, but got Unknown
		//IL_131f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1329: Expected O, but got Unknown
		//IL_1336: Unknown result type (might be due to invalid IL or missing references)
		//IL_1340: Expected O, but got Unknown
		//IL_8d71: Unknown result type (might be due to invalid IL or missing references)
		//IL_8d7b: Expected O, but got Unknown
		//IL_8d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_8da9: Expected O, but got Unknown
		GridEXLayout grd_DesignTimeLayout = new GridEXLayout();
		Appearance appearance34 = new Appearance();
		Appearance appearance67 = new Appearance();
		Appearance appearance78 = new Appearance();
		Appearance appearance89 = new Appearance();
		Appearance appearance101 = new Appearance();
		Appearance appearance112 = new Appearance();
		Appearance appearance123 = new Appearance();
		Appearance appearance134 = new Appearance();
		Appearance appearance145 = new Appearance();
		Appearance appearance147 = new Appearance();
		Appearance appearance148 = new Appearance();
		Appearance appearance149 = new Appearance();
		Appearance appearance13 = new Appearance();
		Appearance appearance14 = new Appearance();
		Appearance appearance15 = new Appearance();
		Appearance appearance16 = new Appearance();
		Appearance appearance17 = new Appearance();
		Appearance appearance18 = new Appearance();
		Appearance appearance19 = new Appearance();
		Appearance appearance20 = new Appearance();
		Appearance appearance41 = new Appearance();
		Appearance appearance59 = new Appearance();
		Appearance appearance60 = new Appearance();
		Appearance appearance61 = new Appearance();
		Appearance appearance232 = new Appearance();
		Appearance appearance233 = new Appearance();
		Appearance appearance234 = new Appearance();
		Appearance appearance3 = new Appearance();
		Appearance appearance4 = new Appearance();
		Appearance appearance5 = new Appearance();
		Appearance appearance6 = new Appearance();
		Appearance appearance7 = new Appearance();
		Appearance appearance8 = new Appearance();
		Appearance appearance9 = new Appearance();
		Appearance appearance10 = new Appearance();
		Appearance appearance11 = new Appearance();
		Appearance appearance21 = new Appearance();
		Appearance appearance22 = new Appearance();
		Appearance appearance24 = new Appearance();
		Appearance appearance49 = new Appearance();
		Appearance appearance50 = new Appearance();
		Appearance appearance51 = new Appearance();
		Appearance appearance52 = new Appearance();
		Appearance appearance53 = new Appearance();
		Appearance appearance55 = new Appearance();
		Appearance appearance56 = new Appearance();
		Appearance appearance57 = new Appearance();
		Appearance appearance58 = new Appearance();
		Appearance appearance120 = new Appearance();
		Appearance appearance121 = new Appearance();
		Appearance appearance122 = new Appearance();
		Appearance appearance124 = new Appearance();
		Appearance appearance125 = new Appearance();
		Appearance appearance126 = new Appearance();
		Appearance appearance127 = new Appearance();
		Appearance appearance128 = new Appearance();
		Appearance appearance129 = new Appearance();
		Appearance appearance130 = new Appearance();
		Appearance appearance131 = new Appearance();
		Appearance appearance132 = new Appearance();
		Appearance appearance80 = new Appearance();
		Appearance appearance81 = new Appearance();
		Appearance appearance82 = new Appearance();
		Appearance appearance83 = new Appearance();
		Appearance appearance84 = new Appearance();
		Appearance appearance85 = new Appearance();
		Appearance appearance86 = new Appearance();
		Appearance appearance87 = new Appearance();
		Appearance appearance88 = new Appearance();
		Appearance appearance90 = new Appearance();
		Appearance appearance91 = new Appearance();
		Appearance appearance92 = new Appearance();
		Appearance appearance93 = new Appearance();
		Appearance appearance94 = new Appearance();
		Appearance appearance95 = new Appearance();
		Appearance appearance96 = new Appearance();
		Appearance appearance97 = new Appearance();
		Appearance appearance98 = new Appearance();
		Appearance appearance99 = new Appearance();
		Appearance appearance102 = new Appearance();
		Appearance appearance103 = new Appearance();
		Appearance appearance104 = new Appearance();
		Appearance appearance105 = new Appearance();
		Appearance appearance106 = new Appearance();
		Appearance appearance107 = new Appearance();
		Appearance appearance108 = new Appearance();
		Appearance appearance109 = new Appearance();
		Appearance appearance110 = new Appearance();
		Appearance appearance111 = new Appearance();
		Appearance appearance113 = new Appearance();
		Appearance appearance114 = new Appearance();
		Appearance appearance115 = new Appearance();
		Appearance appearance116 = new Appearance();
		Appearance appearance117 = new Appearance();
		Appearance appearance118 = new Appearance();
		Appearance appearance119 = new Appearance();
		Appearance appearance1 = new Appearance();
		Appearance appearance100 = new Appearance();
		Appearance appearance164 = new Appearance();
		Appearance appearance175 = new Appearance();
		Appearance appearance186 = new Appearance();
		Appearance appearance197 = new Appearance();
		Appearance appearance208 = new Appearance();
		Appearance appearance219 = new Appearance();
		Appearance appearance230 = new Appearance();
		Appearance appearance2 = new Appearance();
		Appearance appearance12 = new Appearance();
		Appearance appearance23 = new Appearance();
		Appearance appearance133 = new Appearance();
		Appearance appearance135 = new Appearance();
		Appearance appearance136 = new Appearance();
		Appearance appearance137 = new Appearance();
		Appearance appearance138 = new Appearance();
		Appearance appearance139 = new Appearance();
		Appearance appearance140 = new Appearance();
		Appearance appearance141 = new Appearance();
		Appearance appearance142 = new Appearance();
		Appearance appearance143 = new Appearance();
		Appearance appearance144 = new Appearance();
		Appearance appearance146 = new Appearance();
		Appearance appearance151 = new Appearance();
		Appearance appearance152 = new Appearance();
		Appearance appearance153 = new Appearance();
		Appearance appearance154 = new Appearance();
		Appearance appearance156 = new Appearance();
		Appearance appearance157 = new Appearance();
		Appearance appearance158 = new Appearance();
		Appearance appearance159 = new Appearance();
		Appearance appearance160 = new Appearance();
		Appearance appearance161 = new Appearance();
		Appearance appearance162 = new Appearance();
		Appearance appearance163 = new Appearance();
		Appearance appearance235 = new Appearance();
		Appearance appearance236 = new Appearance();
		Appearance appearance237 = new Appearance();
		Appearance appearance238 = new Appearance();
		Appearance appearance239 = new Appearance();
		Appearance appearance240 = new Appearance();
		Appearance appearance212 = new Appearance();
		Appearance appearance213 = new Appearance();
		Appearance appearance214 = new Appearance();
		Appearance appearance215 = new Appearance();
		Appearance appearance216 = new Appearance();
		Appearance appearance217 = new Appearance();
		Appearance appearance42 = new Appearance();
		Appearance appearance43 = new Appearance();
		Appearance appearance54 = new Appearance();
		Appearance appearance62 = new Appearance();
		Appearance appearance44 = new Appearance();
		Appearance appearance45 = new Appearance();
		Appearance appearance46 = new Appearance();
		Appearance appearance47 = new Appearance();
		Appearance appearance48 = new Appearance();
		Appearance appearance63 = new Appearance();
		Appearance appearance64 = new Appearance();
		Appearance appearance65 = new Appearance();
		Appearance appearance66 = new Appearance();
		Appearance appearance68 = new Appearance();
		Appearance appearance69 = new Appearance();
		Appearance appearance70 = new Appearance();
		Appearance appearance71 = new Appearance();
		Appearance appearance72 = new Appearance();
		Appearance appearance73 = new Appearance();
		Appearance appearance74 = new Appearance();
		Appearance appearance75 = new Appearance();
		Appearance appearance76 = new Appearance();
		Appearance appearance77 = new Appearance();
		Appearance appearance79 = new Appearance();
		Appearance appearance25 = new Appearance();
		Appearance appearance26 = new Appearance();
		Appearance appearance27 = new Appearance();
		Appearance appearance28 = new Appearance();
		Appearance appearance29 = new Appearance();
		Appearance appearance30 = new Appearance();
		Appearance appearance31 = new Appearance();
		Appearance appearance32 = new Appearance();
		Appearance appearance33 = new Appearance();
		Appearance appearance35 = new Appearance();
		Appearance appearance36 = new Appearance();
		Appearance appearance37 = new Appearance();
		Appearance appearance150 = new Appearance();
		Appearance appearance155 = new Appearance();
		Appearance appearance165 = new Appearance();
		Appearance appearance166 = new Appearance();
		Appearance appearance167 = new Appearance();
		Appearance appearance168 = new Appearance();
		Appearance appearance169 = new Appearance();
		Appearance appearance170 = new Appearance();
		Appearance appearance171 = new Appearance();
		Appearance appearance172 = new Appearance();
		Appearance appearance173 = new Appearance();
		Appearance appearance174 = new Appearance();
		Appearance appearance222 = new Appearance();
		Appearance appearance223 = new Appearance();
		Appearance appearance224 = new Appearance();
		Appearance appearance228 = new Appearance();
		Appearance appearance229 = new Appearance();
		Appearance appearance231 = new Appearance();
		Appearance appearance218 = new Appearance();
		Appearance appearance220 = new Appearance();
		Appearance appearance221 = new Appearance();
		Appearance appearance176 = new Appearance();
		Appearance appearance177 = new Appearance();
		Appearance appearance178 = new Appearance();
		Appearance appearance179 = new Appearance();
		Appearance appearance180 = new Appearance();
		Appearance appearance181 = new Appearance();
		Appearance appearance182 = new Appearance();
		Appearance appearance183 = new Appearance();
		Appearance appearance184 = new Appearance();
		Appearance appearance199 = new Appearance();
		Appearance appearance200 = new Appearance();
		Appearance appearance201 = new Appearance();
		Appearance appearance202 = new Appearance();
		Appearance appearance203 = new Appearance();
		Appearance appearance204 = new Appearance();
		Appearance appearance185 = new Appearance();
		Appearance appearance187 = new Appearance();
		Appearance appearance188 = new Appearance();
		Appearance appearance189 = new Appearance();
		Appearance appearance190 = new Appearance();
		Appearance appearance191 = new Appearance();
		Appearance appearance192 = new Appearance();
		Appearance appearance193 = new Appearance();
		Appearance appearance194 = new Appearance();
		Appearance appearance195 = new Appearance();
		Appearance appearance196 = new Appearance();
		Appearance appearance198 = new Appearance();
		GridEXLayout grdhistory_DesignTimeLayout = new GridEXLayout();
		GridEXLayout GridDetailHistory_DesignTimeLayout = new GridEXLayout();
		Appearance appearance205 = new Appearance();
		Appearance appearance206 = new Appearance();
		Appearance appearance207 = new Appearance();
		Appearance appearance209 = new Appearance();
		Appearance appearance210 = new Appearance();
		Appearance appearance211 = new Appearance();
		Appearance appearance225 = new Appearance();
		Appearance appearance226 = new Appearance();
		Appearance appearance227 = new Appearance();
		Appearance appearance38 = new Appearance();
		Appearance appearance39 = new Appearance();
		Appearance appearance40 = new Appearance();
		this.panel1 = new System.Windows.Forms.Panel();
		this.tabControl1 = new System.Windows.Forms.TabControl();
		this.tabPage1 = new System.Windows.Forms.TabPage();
		this.panel6 = new System.Windows.Forms.Panel();
		this.panel3 = new System.Windows.Forms.Panel();
		this.tabControl2 = new System.Windows.Forms.TabControl();
		this.tabPage3 = new System.Windows.Forms.TabPage();
		this.grd = new GridEX();
		this.DetailManualEntryPanel = new System.Windows.Forms.Panel();
		this.CmbRejectionReasonType = new UltraCombo();
		this.txtBatchNo = new UltraCombo();
		this.CmbCastingType = new UltraCombo();
		this.label77 = new System.Windows.Forms.Label();
		this.label28 = new System.Windows.Forms.Label();
		this.txtAmount = new System.Windows.Forms.TextBox();
		this.CmbRateUom = new UltraCombo();
		this.label26 = new System.Windows.Forms.Label();
		this.label16 = new System.Windows.Forms.Label();
		this.txtAvgRate = new System.Windows.Forms.TextBox();
		this.CmbWorkStationFromDetail = new UltraCombo();
		this.label10 = new System.Windows.Forms.Label();
		this.CmbProductionStageToDetail = new UltraCombo();
		this.label21 = new System.Windows.Forms.Label();
		this.CmbProductionStageFromDetail = new UltraCombo();
		this.label19 = new System.Windows.Forms.Label();
		this.CmbWarehouseToDetail = new UltraCombo();
		this.label12 = new System.Windows.Forms.Label();
		this.label11 = new System.Windows.Forms.Label();
		this.label18 = new System.Windows.Forms.Label();
		this.CmbSampleModal = new UltraCombo();
		this.label29 = new System.Windows.Forms.Label();
		this.CmbVariantDescription = new UltraCombo();
		this.label5 = new System.Windows.Forms.Label();
		this.CmbCategory = new UltraCombo();
		this.label52 = new System.Windows.Forms.Label();
		this.rdbtnItemCode = new System.Windows.Forms.RadioButton();
		this.rdbtnItemName = new System.Windows.Forms.RadioButton();
		this.btnCancelUpdateDetial = new System.Windows.Forms.Button();
		this.btnplus = new System.Windows.Forms.Button();
		this.label31 = new System.Windows.Forms.Label();
		this.label14 = new System.Windows.Forms.Label();
		this.label22 = new System.Windows.Forms.Label();
		this.txtRejectionReasonRemarks = new System.Windows.Forms.TextBox();
		this.txtTeamWorkerName = new System.Windows.Forms.TextBox();
		this.txtRemarksDetail = new System.Windows.Forms.TextBox();
		this.CmbItem = new UltraCombo();
		this.label36 = new System.Windows.Forms.Label();
		this.label30 = new System.Windows.Forms.Label();
		this.label41 = new System.Windows.Forms.Label();
		this.CmbPackUom = new UltraCombo();
		this.CmbWareHouseFromDetail = new UltraCombo();
		this.label42 = new System.Windows.Forms.Label();
		this.label13 = new System.Windows.Forms.Label();
		this.txtAvailableQty = new System.Windows.Forms.TextBox();
		this.txtTransferQty = new System.Windows.Forms.TextBox();
		this.btnUpdateDetail = new System.Windows.Forms.Button();
		this.panel11 = new System.Windows.Forms.Panel();
		this.ctrlGrdBar5 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel4 = new System.Windows.Forms.Panel();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.CmbWarehouseTo = new UltraCombo();
		this.CmbWarehouseFrom = new UltraCombo();
		this.label3 = new System.Windows.Forms.Label();
		this.CmbProductionStageTo = new UltraCombo();
		this.label2 = new System.Windows.Forms.Label();
		this.CmbProductionStageFrom = new UltraCombo();
		this.label1 = new System.Windows.Forms.Label();
		this.label4 = new System.Windows.Forms.Label();
		this.label6 = new System.Windows.Forms.Label();
		this.CmbTransferType = new UltraCombo();
		this.DocDate = new System.Windows.Forms.DateTimePicker();
		this.txtdocno = new System.Windows.Forms.TextBox();
		this.label17 = new System.Windows.Forms.Label();
		this.label15 = new System.Windows.Forms.Label();
		this.txtRemarksHead = new System.Windows.Forms.TextBox();
		this.panel2 = new System.Windows.Forms.Panel();
		this.ChkBoxPrintPreview = new System.Windows.Forms.CheckBox();
		this.label24 = new System.Windows.Forms.Label();
		this.toolStrip1 = new System.Windows.Forms.ToolStrip();
		this.btnnew = new System.Windows.Forms.ToolStripButton();
		this.btnRefresh = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
		this.btnsave = new System.Windows.Forms.ToolStripButton();
		this.btnSaveAs = new System.Windows.Forms.ToolStripButton();
		this.btnupdate = new System.Windows.Forms.ToolStripButton();
		this.btnDelete = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
		this.btnAttachment = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
		this.btnprint = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
		this.BtnLoadStock = new System.Windows.Forms.ToolStripButton();
		this.btnshortcutkeys = new System.Windows.Forms.ToolStripButton();
		this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
		this.tabPage2 = new System.Windows.Forms.TabPage();
		this.panel7 = new System.Windows.Forms.Panel();
		this.panel9 = new System.Windows.Forms.Panel();
		this.grdhistory = new GridEX();
		this.panel14 = new System.Windows.Forms.Panel();
		this.label27 = new System.Windows.Forms.Label();
		this.GridDetailHistory = new GridEX();
		this.panel15 = new System.Windows.Forms.Panel();
		this.label9 = new System.Windows.Forms.Label();
		this.ctrlGrdBar3 = new grid.User_Conrtols.CtrlGrdBar();
		this.panel8 = new System.Windows.Forms.Panel();
		this.groupBox3 = new System.Windows.Forms.GroupBox();
		this.ToDate = new System.Windows.Forms.DateTimePicker();
		this.fromdate = new System.Windows.Forms.DateTimePicker();
		this.cmbperemeter = new UltraCombo();
		this.btnshow = new System.Windows.Forms.Button();
		this.label20 = new System.Windows.Forms.Label();
		this.label7 = new System.Windows.Forms.Label();
		this.label34 = new System.Windows.Forms.Label();
		this.label8 = new System.Windows.Forms.Label();
		this.label37 = new System.Windows.Forms.Label();
		this.txtDocNoTo = new System.Windows.Forms.TextBox();
		this.txtDocNoFrom = new System.Windows.Forms.TextBox();
		this.bg = new System.Windows.Forms.Label();
		this.panel13 = new System.Windows.Forms.Panel();
		this.label25 = new System.Windows.Forms.Label();
		this.toolStrip3 = new System.Windows.Forms.ToolStrip();
		this.btnNewHistory = new System.Windows.Forms.ToolStripButton();
		this.panel1.SuspendLayout();
		this.tabControl1.SuspendLayout();
		this.tabPage1.SuspendLayout();
		this.panel6.SuspendLayout();
		this.panel3.SuspendLayout();
		this.tabControl2.SuspendLayout();
		this.tabPage3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grd).BeginInit();
		this.DetailManualEntryPanel.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRejectionReasonType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.txtBatchNo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingType).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkStationFromDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageToDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageFromDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseToDetail).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleModal).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariantDescription).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFromDetail).BeginInit();
		this.panel11.SuspendLayout();
		this.panel4.SuspendLayout();
		this.groupBox1.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageTo).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageFrom).BeginInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransferType).BeginInit();
		this.panel2.SuspendLayout();
		this.toolStrip1.SuspendLayout();
		this.tabPage2.SuspendLayout();
		this.panel7.SuspendLayout();
		this.panel9.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).BeginInit();
		this.panel14.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).BeginInit();
		this.panel15.SuspendLayout();
		this.panel8.SuspendLayout();
		this.groupBox3.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).BeginInit();
		this.panel13.SuspendLayout();
		this.toolStrip3.SuspendLayout();
		base.SuspendLayout();
		this.panel1.Controls.Add(this.tabControl1);
		this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel1.Location = new System.Drawing.Point(0, 0);
		this.panel1.Margin = new System.Windows.Forms.Padding(2);
		this.panel1.Name = "panel1";
		this.panel1.Size = new System.Drawing.Size(1089, 681);
		this.panel1.TabIndex = 0;
		this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
		this.tabControl1.Controls.Add(this.tabPage1);
		this.tabControl1.Controls.Add(this.tabPage2);
		this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.tabControl1.Location = new System.Drawing.Point(0, 0);
		this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
		this.tabControl1.Name = "tabControl1";
		this.tabControl1.SelectedIndex = 0;
		this.tabControl1.Size = new System.Drawing.Size(1089, 681);
		this.tabControl1.TabIndex = 0;
		this.tabControl1.SelectedIndexChanged += new System.EventHandler(tabControl1_SelectedIndexChanged);
		this.tabPage1.Controls.Add(this.panel6);
		this.tabPage1.Controls.Add(this.panel4);
		this.tabPage1.Controls.Add(this.panel2);
		this.tabPage1.Location = new System.Drawing.Point(4, 4);
		this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage1.Name = "tabPage1";
		this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage1.Size = new System.Drawing.Size(1081, 652);
		this.tabPage1.TabIndex = 0;
		this.tabPage1.Text = "Form";
		this.tabPage1.UseVisualStyleBackColor = true;
		this.panel6.Controls.Add(this.panel3);
		this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel6.Location = new System.Drawing.Point(2, 152);
		this.panel6.Margin = new System.Windows.Forms.Padding(2);
		this.panel6.Name = "panel6";
		this.panel6.Size = new System.Drawing.Size(1077, 498);
		this.panel6.TabIndex = 4;
		this.panel3.Controls.Add(this.tabControl2);
		this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel3.Location = new System.Drawing.Point(0, 0);
		this.panel3.Name = "panel3";
		this.panel3.Size = new System.Drawing.Size(1077, 498);
		this.panel3.TabIndex = 1;
		this.tabControl2.Controls.Add(this.tabPage3);
		this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
		this.tabControl2.Location = new System.Drawing.Point(0, 0);
		this.tabControl2.Name = "tabControl2";
		this.tabControl2.SelectedIndex = 0;
		this.tabControl2.Size = new System.Drawing.Size(1077, 498);
		this.tabControl2.TabIndex = 0;
		this.tabPage3.Controls.Add((System.Windows.Forms.Control)(object)this.grd);
		this.tabPage3.Controls.Add(this.DetailManualEntryPanel);
		this.tabPage3.Controls.Add(this.panel11);
		this.tabPage3.Font = new System.Drawing.Font("Segoe UI", 10.75f);
		this.tabPage3.Location = new System.Drawing.Point(4, 25);
		this.tabPage3.Name = "tabPage3";
		this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
		this.tabPage3.Size = new System.Drawing.Size(1069, 469);
		this.tabPage3.TabIndex = 0;
		this.tabPage3.Text = "Detail";
		this.tabPage3.UseVisualStyleBackColor = true;
		this.grd.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grd).BackColor = System.Drawing.Color.White;
		this.grd.ColumnAutoResize = true;
		grd_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grd.DesignTimeLayout = grd_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grd).Dock = System.Windows.Forms.DockStyle.Fill;
		((System.Windows.Forms.Control)(object)this.grd).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grd.GroupByBoxVisible = false;
		((System.Windows.Forms.Control)(object)this.grd).Location = new System.Drawing.Point(3, 174);
		((System.Windows.Forms.Control)(object)this.grd).Name = "grd";
		((ScrollableBase)this.grd).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grd).Size = new System.Drawing.Size(1063, 292);
		((System.Windows.Forms.Control)(object)this.grd).TabIndex = 1;
		this.grd.TotalRow = (InheritableBoolean)1;
		this.grd.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grd).VisualStyle = (VisualStyle)4;
		this.grd.CellUpdated += new ColumnActionEventHandler(grd_CellUpdated);
		this.grd.ColumnButtonClick += new ColumnActionEventHandler(grd_ColumnButtonClick);
		((System.Windows.Forms.Control)(object)this.grd).DoubleClick += new System.EventHandler(grd_DoubleClick);
		((System.Windows.Forms.Control)(object)this.grd).KeyDown += new System.Windows.Forms.KeyEventHandler(grd_KeyDown);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRejectionReasonType);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.txtBatchNo);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCastingType);
		this.DetailManualEntryPanel.Controls.Add(this.label77);
		this.DetailManualEntryPanel.Controls.Add(this.label28);
		this.DetailManualEntryPanel.Controls.Add(this.txtAmount);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbRateUom);
		this.DetailManualEntryPanel.Controls.Add(this.label26);
		this.DetailManualEntryPanel.Controls.Add(this.label16);
		this.DetailManualEntryPanel.Controls.Add(this.txtAvgRate);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWorkStationFromDetail);
		this.DetailManualEntryPanel.Controls.Add(this.label10);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionStageToDetail);
		this.DetailManualEntryPanel.Controls.Add(this.label21);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionStageFromDetail);
		this.DetailManualEntryPanel.Controls.Add(this.label19);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseToDetail);
		this.DetailManualEntryPanel.Controls.Add(this.label12);
		this.DetailManualEntryPanel.Controls.Add(this.label11);
		this.DetailManualEntryPanel.Controls.Add(this.label18);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbSampleModal);
		this.DetailManualEntryPanel.Controls.Add(this.label29);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbVariantDescription);
		this.DetailManualEntryPanel.Controls.Add(this.label5);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbCategory);
		this.DetailManualEntryPanel.Controls.Add(this.label52);
		this.DetailManualEntryPanel.Controls.Add(this.rdbtnItemCode);
		this.DetailManualEntryPanel.Controls.Add(this.rdbtnItemName);
		this.DetailManualEntryPanel.Controls.Add(this.btnCancelUpdateDetial);
		this.DetailManualEntryPanel.Controls.Add(this.btnplus);
		this.DetailManualEntryPanel.Controls.Add(this.label31);
		this.DetailManualEntryPanel.Controls.Add(this.label14);
		this.DetailManualEntryPanel.Controls.Add(this.label22);
		this.DetailManualEntryPanel.Controls.Add(this.txtRejectionReasonRemarks);
		this.DetailManualEntryPanel.Controls.Add(this.txtTeamWorkerName);
		this.DetailManualEntryPanel.Controls.Add(this.txtRemarksDetail);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbItem);
		this.DetailManualEntryPanel.Controls.Add(this.label36);
		this.DetailManualEntryPanel.Controls.Add(this.label30);
		this.DetailManualEntryPanel.Controls.Add(this.label41);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbPackUom);
		this.DetailManualEntryPanel.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail);
		this.DetailManualEntryPanel.Controls.Add(this.label42);
		this.DetailManualEntryPanel.Controls.Add(this.label13);
		this.DetailManualEntryPanel.Controls.Add(this.txtAvailableQty);
		this.DetailManualEntryPanel.Controls.Add(this.txtTransferQty);
		this.DetailManualEntryPanel.Controls.Add(this.btnUpdateDetail);
		this.DetailManualEntryPanel.Dock = System.Windows.Forms.DockStyle.Top;
		this.DetailManualEntryPanel.Location = new System.Drawing.Point(3, 27);
		this.DetailManualEntryPanel.Margin = new System.Windows.Forms.Padding(2);
		this.DetailManualEntryPanel.Name = "DetailManualEntryPanel";
		this.DetailManualEntryPanel.Size = new System.Drawing.Size(1063, 147);
		this.DetailManualEntryPanel.TabIndex = 0;
		this.CmbRejectionReasonType.AllowNull = (DefaultableBoolean)1;
		this.CmbRejectionReasonType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRejectionReasonType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRejectionReasonType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance34).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance34).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance34;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance67).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance67).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance67).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance67).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance67;
		((AppearanceBase)appearance78).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance78;
		((SpecialBoxBase)((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance89).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance89).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance89).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance89).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance89;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance101).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance101).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance101;
		((AppearanceBase)appearance112).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance112).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance112;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance123).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance123;
		((AppearanceBase)appearance134).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance134).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance134;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance145).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance145).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance145).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance145).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance145).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance145;
		((AppearanceBase)appearance147).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance147;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance148).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance148).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance148;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance149).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance149;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRejectionReasonType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRejectionReasonType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRejectionReasonType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRejectionReasonType).Location = new System.Drawing.Point(478, 117);
		((System.Windows.Forms.Control)(object)this.CmbRejectionReasonType).Name = "CmbRejectionReasonType";
		((System.Windows.Forms.Control)(object)this.CmbRejectionReasonType).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbRejectionReasonType).TabIndex = 18;
		this.CmbRejectionReasonType.ValueChanged += new System.EventHandler(txtBatchNo_Leave);
		((System.Windows.Forms.Control)(object)this.CmbRejectionReasonType).Leave += new System.EventHandler(txtBatchNo_Leave);
		this.txtBatchNo.AllowNull = (DefaultableBoolean)1;
		this.txtBatchNo.AutoCompleteMode = (AutoCompleteMode)4;
		this.txtBatchNo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.txtBatchNo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance13).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance13).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance13;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance14).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance14).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance14).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance14).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.txtBatchNo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance14;
		((AppearanceBase)appearance15).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance15;
		((SpecialBoxBase)((UltraGridBase)this.txtBatchNo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance16).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance16).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance16).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance16).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance16;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance17).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance17).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance17;
		((AppearanceBase)appearance18).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance18).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance18;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance19).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance19;
		((AppearanceBase)appearance20).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance20).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance20;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance41).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance41).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance41).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance41).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance41).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance41;
		((AppearanceBase)appearance59).TextHAlignAsString = "Left";
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance59;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance60).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance60).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance60;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance61).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance61;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.txtBatchNo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.txtBatchNo).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtBatchNo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.txtBatchNo).Location = new System.Drawing.Point(724, 24);
		((System.Windows.Forms.Control)(object)this.txtBatchNo).Name = "txtBatchNo";
		((System.Windows.Forms.Control)(object)this.txtBatchNo).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.txtBatchNo).TabIndex = 5;
		this.txtBatchNo.ValueChanged += new System.EventHandler(txtBatchNo_Leave);
		((System.Windows.Forms.Control)(object)this.txtBatchNo).Leave += new System.EventHandler(txtBatchNo_Leave);
		this.CmbCastingType.AllowNull = (DefaultableBoolean)1;
		this.CmbCastingType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCastingType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCastingType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance232).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance232).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance232;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance233).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance233).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance233).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance233).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance233;
		((AppearanceBase)appearance234).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance234;
		((SpecialBoxBase)((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance3).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance3).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance3).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance3).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance3;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance4).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance4).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance4;
		((AppearanceBase)appearance5).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance5).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance5;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance6).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance6;
		((AppearanceBase)appearance7).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance7).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance7;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance8).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance8).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance8).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance8).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance8).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance8;
		((AppearanceBase)appearance9).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance9;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance10).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance10).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance10;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance11).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance11;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCastingType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCastingType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Location = new System.Drawing.Point(883, 24);
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Name = "CmbCastingType";
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Size = new System.Drawing.Size(177, 26);
		((System.Windows.Forms.Control)(object)this.CmbCastingType).TabIndex = 6;
		this.CmbCastingType.ValueChanged += new System.EventHandler(CmbCastingType_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbCastingType).Leave += new System.EventHandler(CmbCastingType_ValueChanged);
		this.label77.AutoSize = true;
		this.label77.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label77.Location = new System.Drawing.Point(883, 6);
		this.label77.Name = "label77";
		this.label77.Size = new System.Drawing.Size(74, 15);
		this.label77.TabIndex = 619;
		this.label77.Text = "Casting Type";
		this.label28.AutoSize = true;
		this.label28.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label28.Location = new System.Drawing.Point(80, 100);
		this.label28.Name = "label28";
		this.label28.Size = new System.Drawing.Size(51, 15);
		this.label28.TabIndex = 617;
		this.label28.Text = "Amount";
		this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAmount.Enabled = false;
		this.txtAmount.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAmount.Location = new System.Drawing.Point(80, 120);
		this.txtAmount.Name = "txtAmount";
		this.txtAmount.Size = new System.Drawing.Size(112, 23);
		this.txtAmount.TabIndex = 16;
		this.CmbRateUom.AllowNull = (DefaultableBoolean)1;
		this.CmbRateUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbRateUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbRateUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance21).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance21).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance21;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance22).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance22).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance22).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance22).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance22;
		((AppearanceBase)appearance24).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance24;
		((SpecialBoxBase)((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance49).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance49).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance49).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance49).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance49;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance50).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance50).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance50;
		((AppearanceBase)appearance51).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance51).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance51;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance52).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance52;
		((AppearanceBase)appearance53).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance53).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance53;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance55).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance55).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance55).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance55).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance55).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance55;
		((AppearanceBase)appearance56).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance56;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance57).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance57).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance57;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance58).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance58;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbRateUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbRateUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Location = new System.Drawing.Point(2, 118);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Name = "CmbRateUom";
		((System.Windows.Forms.Control)(object)this.CmbRateUom).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.CmbRateUom).TabIndex = 15;
		this.CmbRateUom.ValueChanged += new System.EventHandler(CmbRateUom_ValueChanged);
		this.label26.AutoSize = true;
		this.label26.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label26.Location = new System.Drawing.Point(2, 100);
		this.label26.Name = "label26";
		this.label26.Size = new System.Drawing.Size(59, 15);
		this.label26.TabIndex = 615;
		this.label26.Text = "Rate Uom";
		this.label16.AutoSize = true;
		this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label16.Location = new System.Drawing.Point(973, 57);
		this.label16.Name = "label16";
		this.label16.Size = new System.Drawing.Size(54, 15);
		this.label16.TabIndex = 613;
		this.label16.Text = "Avg Rate";
		this.txtAvgRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvgRate.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvgRate.Location = new System.Drawing.Point(973, 75);
		this.txtAvgRate.Name = "txtAvgRate";
		this.txtAvgRate.Size = new System.Drawing.Size(87, 23);
		this.txtAvgRate.TabIndex = 14;
		this.txtAvgRate.TextChanged += new System.EventHandler(txtAvgRate_TextChanged);
		this.txtAvgRate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DecimalNumberKeySetting_KeyPress);
		this.CmbWorkStationFromDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbWorkStationFromDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWorkStationFromDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWorkStationFromDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance120).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance120).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance120;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance121).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance121).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance121).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance121).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance121;
		((AppearanceBase)appearance122).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance122;
		((SpecialBoxBase)((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance124).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance124).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance124).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance124).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance124;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance125).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance125).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance125;
		((AppearanceBase)appearance126).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance126).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance126;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance127).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance127;
		((AppearanceBase)appearance128).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance128).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance128;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance129).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance129).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance129).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance129).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance129).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance129;
		((AppearanceBase)appearance130).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance130;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance131).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance131).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance131;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance132).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance132;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWorkStationFromDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWorkStationFromDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWorkStationFromDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWorkStationFromDetail).Location = new System.Drawing.Point(636, 73);
		((System.Windows.Forms.Control)(object)this.CmbWorkStationFromDetail).Name = "CmbWorkStationFromDetail";
		((System.Windows.Forms.Control)(object)this.CmbWorkStationFromDetail).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbWorkStationFromDetail).TabIndex = 11;
		this.CmbWorkStationFromDetail.ValueChanged += new System.EventHandler(CmbWorkStationFromDetail_Leave);
		((System.Windows.Forms.Control)(object)this.CmbWorkStationFromDetail).Leave += new System.EventHandler(CmbWorkStationFromDetail_Leave);
		this.label10.AutoSize = true;
		this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label10.Location = new System.Drawing.Point(636, 54);
		this.label10.Name = "label10";
		this.label10.Size = new System.Drawing.Size(77, 15);
		this.label10.TabIndex = 609;
		this.label10.Text = "Work Station";
		this.CmbProductionStageToDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionStageToDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionStageToDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionStageToDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance80).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance80).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance80;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance81).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance81).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance81).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance81).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance81;
		((AppearanceBase)appearance82).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance82;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance83).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance83).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance83).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance83).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance83;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance84).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance84).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance84;
		((AppearanceBase)appearance85).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance85).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance85;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance86).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance86;
		((AppearanceBase)appearance87).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance87).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance87;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance88).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance88).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance88).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance88).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance88).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance88;
		((AppearanceBase)appearance90).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance90;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance91).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance91).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance91;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance92).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance92;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionStageToDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageToDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageToDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionStageToDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageToDetail).Location = new System.Drawing.Point(478, 73);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageToDetail).Name = "CmbProductionStageToDetail";
		((System.Windows.Forms.Control)(object)this.CmbProductionStageToDetail).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageToDetail).TabIndex = 10;
		this.label21.AutoSize = true;
		this.label21.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label21.Location = new System.Drawing.Point(478, 54);
		this.label21.Name = "label21";
		this.label21.Size = new System.Drawing.Size(115, 15);
		this.label21.TabIndex = 607;
		this.label21.Text = "Production Stage To";
		this.CmbProductionStageFromDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionStageFromDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionStageFromDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionStageFromDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance93).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance93).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance93;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance94).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance94).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance94).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance94).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance94;
		((AppearanceBase)appearance95).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance95;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance96).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance96).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance96).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance96).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance96;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance97).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance97).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance97;
		((AppearanceBase)appearance98).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance98).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance98;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance99).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance99;
		((AppearanceBase)appearance102).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance102).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance102;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance103).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance103).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance103).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance103).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance103).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance103;
		((AppearanceBase)appearance104).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance104;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance105).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance105).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance105;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance106).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance106;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionStageFromDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFromDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFromDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbProductionStageFromDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFromDetail).Location = new System.Drawing.Point(320, 73);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFromDetail).Name = "CmbProductionStageFromDetail";
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFromDetail).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFromDetail).TabIndex = 9;
		this.label19.AutoSize = true;
		this.label19.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label19.Location = new System.Drawing.Point(320, 54);
		this.label19.Name = "label19";
		this.label19.Size = new System.Drawing.Size(130, 15);
		this.label19.TabIndex = 605;
		this.label19.Text = "Production Stage From";
		this.CmbWarehouseToDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseToDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseToDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseToDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance107).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance107).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance107;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance108).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance108).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance108).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance108).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance108;
		((AppearanceBase)appearance109).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance109;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance110).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance110).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance110).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance110).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance110;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance111).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance111).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance111;
		((AppearanceBase)appearance113).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance113).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance113;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance114).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance114;
		((AppearanceBase)appearance115).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance115).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance115;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance116).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance116).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance116).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance116).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance116).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance116;
		((AppearanceBase)appearance117).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance117;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance118).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance118).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance118;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance119).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance119;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseToDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseToDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseToDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWarehouseToDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseToDetail).Location = new System.Drawing.Point(161, 73);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseToDetail).Name = "CmbWarehouseToDetail";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseToDetail).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseToDetail).TabIndex = 8;
		this.label12.AutoSize = true;
		this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label12.Location = new System.Drawing.Point(478, 100);
		this.label12.Name = "label12";
		this.label12.Size = new System.Drawing.Size(125, 15);
		this.label12.TabIndex = 601;
		this.label12.Text = "Rejection Reason Type";
		this.label11.AutoSize = true;
		this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label11.Location = new System.Drawing.Point(161, 54);
		this.label11.Name = "label11";
		this.label11.Size = new System.Drawing.Size(83, 15);
		this.label11.TabIndex = 603;
		this.label11.Text = "Warehouse To";
		this.label18.AutoSize = true;
		this.label18.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label18.Location = new System.Drawing.Point(724, 6);
		this.label18.Name = "label18";
		this.label18.Size = new System.Drawing.Size(56, 15);
		this.label18.TabIndex = 601;
		this.label18.Text = "Batch No";
		this.CmbSampleModal.AllowNull = (DefaultableBoolean)1;
		this.CmbSampleModal.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbSampleModal.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbSampleModal.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance1).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance1).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Appearance = (AppearanceBase)(object)appearance1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance100).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance100).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance100).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance100).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance100;
		((AppearanceBase)appearance164).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance164;
		((SpecialBoxBase)((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance175).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance175).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance175).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance175).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance175;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance186).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance186).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance186;
		((AppearanceBase)appearance197).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance197).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance197;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance208).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance208;
		((AppearanceBase)appearance219).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance219).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance219;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance230).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance230).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance230).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance230).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance230).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance230;
		((AppearanceBase)appearance2).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance2;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance12).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance12).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance12;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance23).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance23;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbSampleModal).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbSampleModal.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Location = new System.Drawing.Point(161, 24);
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Name = "CmbSampleModal";
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Size = new System.Drawing.Size(114, 26);
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).TabIndex = 1;
		((System.Windows.Forms.Control)(object)this.CmbSampleModal).Leave += new System.EventHandler(CmbSampleModal_Leave);
		this.label29.AutoSize = true;
		this.label29.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label29.Location = new System.Drawing.Point(164, 6);
		this.label29.Name = "label29";
		this.label29.Size = new System.Drawing.Size(84, 15);
		this.label29.TabIndex = 599;
		this.label29.Text = "Modal Sample";
		this.CmbVariantDescription.AllowNull = (DefaultableBoolean)1;
		this.CmbVariantDescription.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbVariantDescription.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbVariantDescription.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance133).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance133).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Appearance = (AppearanceBase)(object)appearance133;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance135).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance135).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance135).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance135).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance135;
		((AppearanceBase)appearance136).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance136;
		((SpecialBoxBase)((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance137).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance137).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance137).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance137).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance137;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance138).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance138).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance138;
		((AppearanceBase)appearance139).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance139).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance139;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance140).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance140;
		((AppearanceBase)appearance141).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance141).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance141;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance142).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance142).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance142).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance142).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance142).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance142;
		((AppearanceBase)appearance143).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance143;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance144).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance144).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance144;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance146).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance146;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbVariantDescription).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbVariantDescription.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Location = new System.Drawing.Point(565, 24);
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Name = "CmbVariantDescription";
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).TabIndex = 4;
		this.CmbVariantDescription.ValueChanged += new System.EventHandler(CmbVariantDescription_ValueChanged);
		((System.Windows.Forms.Control)(object)this.CmbVariantDescription).Leave += new System.EventHandler(CmbVariantDescription_ValueChanged);
		this.label5.AutoSize = true;
		this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label5.Location = new System.Drawing.Point(565, 6);
		this.label5.Name = "label5";
		this.label5.Size = new System.Drawing.Size(105, 15);
		this.label5.TabIndex = 599;
		this.label5.Text = "Modal Description";
		this.CmbCategory.AllowNull = (DefaultableBoolean)1;
		this.CmbCategory.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbCategory.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbCategory.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance151).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance151).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Appearance = (AppearanceBase)(object)appearance151;
		((UltraGridBase)this.CmbCategory).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance152).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance152).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance152).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance152).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance152;
		((AppearanceBase)appearance153).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance153;
		((SpecialBoxBase)((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance154).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance154).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance154).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance154).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance154;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance156).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance156).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance156;
		((AppearanceBase)appearance157).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance157).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance157;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance158).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance158;
		((AppearanceBase)appearance159).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance159).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance159;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance160).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance160).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance160).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance160).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance160).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance160;
		((AppearanceBase)appearance161).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance161;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance162).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance162).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance162;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance163).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbCategory).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance163;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbCategory).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbCategory.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Location = new System.Drawing.Point(2, 24);
		((System.Windows.Forms.Control)(object)this.CmbCategory).Name = "CmbCategory";
		((System.Windows.Forms.Control)(object)this.CmbCategory).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbCategory).TabIndex = 0;
		((System.Windows.Forms.Control)(object)this.CmbCategory).Leave += new System.EventHandler(CmbCategory_Leave);
		this.label52.AutoSize = true;
		this.label52.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label52.Location = new System.Drawing.Point(2, 6);
		this.label52.Name = "label52";
		this.label52.Size = new System.Drawing.Size(82, 15);
		this.label52.TabIndex = 597;
		this.label52.Text = "Item Category";
		this.rdbtnItemCode.AutoSize = true;
		this.rdbtnItemCode.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemCode.Location = new System.Drawing.Point(369, 4);
		this.rdbtnItemCode.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemCode.Name = "rdbtnItemCode";
		this.rdbtnItemCode.Size = new System.Drawing.Size(52, 19);
		this.rdbtnItemCode.TabIndex = 595;
		this.rdbtnItemCode.Text = "Code";
		this.rdbtnItemCode.UseVisualStyleBackColor = true;
		this.rdbtnItemName.AutoSize = true;
		this.rdbtnItemName.Checked = true;
		this.rdbtnItemName.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.rdbtnItemName.Location = new System.Drawing.Point(310, 4);
		this.rdbtnItemName.Margin = new System.Windows.Forms.Padding(2);
		this.rdbtnItemName.Name = "rdbtnItemName";
		this.rdbtnItemName.Size = new System.Drawing.Size(57, 19);
		this.rdbtnItemName.TabIndex = 594;
		this.rdbtnItemName.TabStop = true;
		this.rdbtnItemName.Text = "Name";
		this.rdbtnItemName.UseVisualStyleBackColor = true;
		this.rdbtnItemName.CheckedChanged += new System.EventHandler(rdbtnItemName_CheckedChanged);
		this.btnCancelUpdateDetial.BackColor = System.Drawing.Color.Teal;
		this.btnCancelUpdateDetial.FlatAppearance.BorderSize = 0;
		this.btnCancelUpdateDetial.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnCancelUpdateDetial.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnCancelUpdateDetial.ForeColor = System.Drawing.Color.White;
		this.btnCancelUpdateDetial.Location = new System.Drawing.Point(995, 99);
		this.btnCancelUpdateDetial.Name = "btnCancelUpdateDetial";
		this.btnCancelUpdateDetial.Size = new System.Drawing.Size(65, 21);
		this.btnCancelUpdateDetial.TabIndex = 22;
		this.btnCancelUpdateDetial.Text = "Cancel";
		this.btnCancelUpdateDetial.UseVisualStyleBackColor = false;
		this.btnCancelUpdateDetial.Click += new System.EventHandler(btnCancelUpdateDetial_Click);
		this.btnCancelUpdateDetial.Leave += new System.EventHandler(btnCancelUpdateDetial_Leave);
		this.btnplus.BackColor = System.Drawing.Color.Teal;
		this.btnplus.FlatAppearance.BorderSize = 0;
		this.btnplus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnplus.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnplus.ForeColor = System.Drawing.Color.White;
		this.btnplus.Location = new System.Drawing.Point(995, 122);
		this.btnplus.Name = "btnplus";
		this.btnplus.Size = new System.Drawing.Size(37, 23);
		this.btnplus.TabIndex = 21;
		this.btnplus.Text = "+";
		this.btnplus.UseVisualStyleBackColor = false;
		this.btnplus.Click += new System.EventHandler(btnplus_Click);
		this.btnplus.Leave += new System.EventHandler(btnplus_Leave);
		this.label31.AutoSize = true;
		this.label31.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label31.Location = new System.Drawing.Point(795, 100);
		this.label31.Name = "label31";
		this.label31.Size = new System.Drawing.Size(145, 15);
		this.label31.TabIndex = 584;
		this.label31.Text = "Rejection Reason Remarks";
		this.label14.AutoSize = true;
		this.label14.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label14.Location = new System.Drawing.Point(636, 100);
		this.label14.Name = "label14";
		this.label14.Size = new System.Drawing.Size(137, 15);
		this.label14.TabIndex = 584;
		this.label14.Text = "Reason Type Description";
		this.label22.AutoSize = true;
		this.label22.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label22.Location = new System.Drawing.Point(193, 100);
		this.label22.Name = "label22";
		this.label22.Size = new System.Drawing.Size(52, 15);
		this.label22.TabIndex = 584;
		this.label22.Text = "Remarks";
		this.txtRejectionReasonRemarks.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRejectionReasonRemarks.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRejectionReasonRemarks.Location = new System.Drawing.Point(795, 119);
		this.txtRejectionReasonRemarks.Name = "txtRejectionReasonRemarks";
		this.txtRejectionReasonRemarks.Size = new System.Drawing.Size(198, 23);
		this.txtRejectionReasonRemarks.TabIndex = 20;
		this.txtTeamWorkerName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTeamWorkerName.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTeamWorkerName.Location = new System.Drawing.Point(636, 119);
		this.txtTeamWorkerName.Name = "txtTeamWorkerName";
		this.txtTeamWorkerName.Size = new System.Drawing.Size(157, 23);
		this.txtTeamWorkerName.TabIndex = 19;
		this.txtRemarksDetail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksDetail.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtRemarksDetail.Location = new System.Drawing.Point(193, 120);
		this.txtRemarksDetail.Name = "txtRemarksDetail";
		this.txtRemarksDetail.Size = new System.Drawing.Size(284, 23);
		this.txtRemarksDetail.TabIndex = 17;
		this.CmbItem.AllowNull = (DefaultableBoolean)1;
		this.CmbItem.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbItem.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbItem.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance235).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance235).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbItem).DisplayLayout.Appearance = (AppearanceBase)(object)appearance235;
		((UltraGridBase)this.CmbItem).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance236).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance236).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance236).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance236).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance236;
		((AppearanceBase)appearance237).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance237;
		((SpecialBoxBase)((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance238).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance238).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance238).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance238).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbItem).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance238;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbItem).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance239).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance239).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance239;
		((AppearanceBase)appearance240).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance240).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance240;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance212).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance212;
		((AppearanceBase)appearance213).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance213).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance213;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance214).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance214).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance214).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance214).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance214).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance214;
		((AppearanceBase)appearance215).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance215;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance216).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance216).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance216;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance217).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbItem).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance217;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbItem).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbItem).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbItem.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbItem).Location = new System.Drawing.Point(277, 24);
		((System.Windows.Forms.Control)(object)this.CmbItem).Name = "CmbItem";
		((System.Windows.Forms.Control)(object)this.CmbItem).Size = new System.Drawing.Size(208, 26);
		((System.Windows.Forms.Control)(object)this.CmbItem).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbItem).Leave += new System.EventHandler(CmbItem_Leave);
		this.label36.AutoSize = true;
		this.label36.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label36.Location = new System.Drawing.Point(277, 6);
		this.label36.Name = "label36";
		this.label36.Size = new System.Drawing.Size(32, 15);
		this.label36.TabIndex = 8;
		this.label36.Text = "Item";
		this.label30.AutoSize = true;
		this.label30.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label30.Location = new System.Drawing.Point(795, 54);
		this.label30.Name = "label30";
		this.label30.Size = new System.Drawing.Size(77, 15);
		this.label30.TabIndex = 19;
		this.label30.Text = "Available Qty";
		this.label41.AutoSize = true;
		this.label41.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label41.Location = new System.Drawing.Point(884, 54);
		this.label41.Name = "label41";
		this.label41.Size = new System.Drawing.Size(71, 15);
		this.label41.TabIndex = 19;
		this.label41.Text = "Transfer Qty";
		this.CmbPackUom.AllowNull = (DefaultableBoolean)1;
		this.CmbPackUom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbPackUom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbPackUom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance42).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance42).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance42;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance43).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance43).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance43).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance43).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance43;
		((AppearanceBase)appearance54).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance54;
		((SpecialBoxBase)((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance62).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance62).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance62).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance62).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance62;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance44).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance44).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance44;
		((AppearanceBase)appearance45).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance45).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance45;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance46).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance46;
		((AppearanceBase)appearance47).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance47).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance47;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance48).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance48).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance48).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance48).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance48).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance48;
		((AppearanceBase)appearance63).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance63;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance64).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance64).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance64;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance65).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance65;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbPackUom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbPackUom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Location = new System.Drawing.Point(486, 24);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Name = "CmbPackUom";
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Size = new System.Drawing.Size(77, 26);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).TabIndex = 3;
		this.CmbPackUom.ValueChanged += new System.EventHandler(CmbPackUom_Leave);
		((System.Windows.Forms.Control)(object)this.CmbPackUom).Leave += new System.EventHandler(CmbPackUom_Leave);
		this.CmbWareHouseFromDetail.AllowNull = (DefaultableBoolean)1;
		this.CmbWareHouseFromDetail.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWareHouseFromDetail.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWareHouseFromDetail.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance66).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance66).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Appearance = (AppearanceBase)(object)appearance66;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance68).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance68).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance68).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance68).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance68;
		((AppearanceBase)appearance69).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance69;
		((SpecialBoxBase)((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance70).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance70).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance70).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance70).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance70;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance71).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance71).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance71;
		((AppearanceBase)appearance72).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance72).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance72;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance73).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance73;
		((AppearanceBase)appearance74).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance74).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance74;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance75).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance75).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance75).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance75).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance75).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance75;
		((AppearanceBase)appearance76).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance76;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance77).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance77).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance77;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance79).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance79;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWareHouseFromDetail).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.CmbWareHouseFromDetail.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail).Location = new System.Drawing.Point(2, 73);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail).Name = "CmbWareHouseFromDetail";
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail).Size = new System.Drawing.Size(157, 26);
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail).TabIndex = 7;
		((System.Windows.Forms.Control)(object)this.CmbWareHouseFromDetail).Leave += new System.EventHandler(CmbWareHouseFromDetail_Leave);
		this.label42.AutoSize = true;
		this.label42.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label42.Location = new System.Drawing.Point(486, 6);
		this.label42.Name = "label42";
		this.label42.Size = new System.Drawing.Size(35, 15);
		this.label42.TabIndex = 21;
		this.label42.Text = "UOM";
		this.label13.AutoSize = true;
		this.label13.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label13.Location = new System.Drawing.Point(1, 54);
		this.label13.Name = "label13";
		this.label13.Size = new System.Drawing.Size(98, 15);
		this.label13.TabIndex = 25;
		this.label13.Text = "Warehouse From";
		this.txtAvailableQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtAvailableQty.Enabled = false;
		this.txtAvailableQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtAvailableQty.Location = new System.Drawing.Point(795, 75);
		this.txtAvailableQty.Name = "txtAvailableQty";
		this.txtAvailableQty.Size = new System.Drawing.Size(88, 23);
		this.txtAvailableQty.TabIndex = 12;
		this.txtAvailableQty.TextChanged += new System.EventHandler(txtTransferQty_TextChanged);
		this.txtAvailableQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DecimalNumberKeySetting_KeyPress);
		this.txtTransferQty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtTransferQty.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.txtTransferQty.Location = new System.Drawing.Point(884, 75);
		this.txtTransferQty.Name = "txtTransferQty";
		this.txtTransferQty.Size = new System.Drawing.Size(88, 23);
		this.txtTransferQty.TabIndex = 13;
		this.txtTransferQty.TextChanged += new System.EventHandler(txtTransferQty_TextChanged);
		this.txtTransferQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(DecimalNumberKeySetting_KeyPress);
		this.btnUpdateDetail.BackColor = System.Drawing.Color.Teal;
		this.btnUpdateDetail.FlatAppearance.BorderSize = 0;
		this.btnUpdateDetail.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnUpdateDetail.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnUpdateDetail.ForeColor = System.Drawing.Color.White;
		this.btnUpdateDetail.Location = new System.Drawing.Point(995, 122);
		this.btnUpdateDetail.Name = "btnUpdateDetail";
		this.btnUpdateDetail.Size = new System.Drawing.Size(65, 23);
		this.btnUpdateDetail.TabIndex = 21;
		this.btnUpdateDetail.Text = "Update";
		this.btnUpdateDetail.UseVisualStyleBackColor = false;
		this.btnUpdateDetail.Click += new System.EventHandler(btnUpdateDetail_Click);
		this.panel11.BackColor = System.Drawing.Color.Teal;
		this.panel11.Controls.Add(this.ctrlGrdBar5);
		this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel11.Location = new System.Drawing.Point(3, 3);
		this.panel11.Name = "panel11";
		this.panel11.Size = new System.Drawing.Size(1063, 24);
		this.panel11.TabIndex = 2;
		this.ctrlGrdBar5.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar5.FormName = this;
		this.ctrlGrdBar5.Location = new System.Drawing.Point(1020, 2);
		this.ctrlGrdBar5.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
		this.ctrlGrdBar5.MyGrid = this.grd;
		this.ctrlGrdBar5.Name = "ctrlGrdBar5";
		this.ctrlGrdBar5.Size = new System.Drawing.Size(40, 21);
		this.ctrlGrdBar5.TabIndex = 25;
		this.ctrlGrdBar5.Load += new System.EventHandler(ctrlGrdBar1_Load);
		this.panel4.BackColor = System.Drawing.Color.White;
		this.panel4.Controls.Add(this.groupBox1);
		this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel4.Location = new System.Drawing.Point(2, 52);
		this.panel4.Margin = new System.Windows.Forms.Padding(2);
		this.panel4.Name = "panel4";
		this.panel4.Size = new System.Drawing.Size(1077, 100);
		this.panel4.TabIndex = 0;
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseTo);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom);
		this.groupBox1.Controls.Add(this.label3);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionStageTo);
		this.groupBox1.Controls.Add(this.label2);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbProductionStageFrom);
		this.groupBox1.Controls.Add(this.label1);
		this.groupBox1.Controls.Add(this.label4);
		this.groupBox1.Controls.Add(this.label6);
		this.groupBox1.Controls.Add((System.Windows.Forms.Control)(object)this.CmbTransferType);
		this.groupBox1.Controls.Add(this.DocDate);
		this.groupBox1.Controls.Add(this.txtdocno);
		this.groupBox1.Controls.Add(this.label17);
		this.groupBox1.Controls.Add(this.label15);
		this.groupBox1.Controls.Add(this.txtRemarksHead);
		this.groupBox1.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox1.Location = new System.Drawing.Point(4, 1);
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.Size = new System.Drawing.Size(966, 97);
		this.groupBox1.TabIndex = 0;
		this.groupBox1.TabStop = false;
		this.groupBox1.Text = "Main";
		this.CmbWarehouseTo.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance25).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance25).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance25;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance26).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance26).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance26).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance26).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance26;
		((AppearanceBase)appearance27).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance27;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance28).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance28).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance28).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance28).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance28;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance29).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance29).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance29;
		((AppearanceBase)appearance30).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance30).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance30;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance31).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance31;
		((AppearanceBase)appearance32).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance32).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance32;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance33).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance33).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance33).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance33).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance33).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance33;
		((AppearanceBase)appearance35).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance35;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance36).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance36).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance36;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance37).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance37;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouseTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Location = new System.Drawing.Point(380, 40);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Name = "CmbWarehouseTo";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseTo).TabIndex = 5;
		this.CmbWarehouseFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbWarehouseFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbWarehouseFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbWarehouseFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance150).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance150).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance150;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance155).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance155).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance155).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance155).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance155;
		((AppearanceBase)appearance165).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance165;
		((SpecialBoxBase)((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance166).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance166).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance166).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance166).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance166;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance167).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance167).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance167;
		((AppearanceBase)appearance168).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance168).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance168;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance169).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance169;
		((AppearanceBase)appearance170).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance170).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance170;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance171).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance171).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance171).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance171).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance171).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance171;
		((AppearanceBase)appearance172).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance172;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance173).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance173).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance173;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance174).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance174;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbWarehouseFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbWarehouseFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Location = new System.Drawing.Point(380, 12);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Name = "CmbWarehouseFrom";
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbWarehouseFrom).TabIndex = 4;
		this.label3.AutoSize = true;
		this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label3.Location = new System.Drawing.Point(606, 46);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(115, 15);
		this.label3.TabIndex = 28;
		this.label3.Text = "Production Stage To";
		this.CmbProductionStageTo.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionStageTo.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionStageTo.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionStageTo.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance222).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance222).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Appearance = (AppearanceBase)(object)appearance222;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance223).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance223).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance223).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance223).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance223;
		((AppearanceBase)appearance224).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance224;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance228).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance228).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance228).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance228).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance228;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance229).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance229).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance229;
		((AppearanceBase)appearance231).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance231).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance231;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance218).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance218;
		((AppearanceBase)appearance220).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance220).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance220;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance221).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance221).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance221).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance221).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance221).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance221;
		((AppearanceBase)appearance176).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance176;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance177).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance177).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance177;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance178).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance178;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionStageTo).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageTo).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageTo).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbProductionStageTo.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageTo).Location = new System.Drawing.Point(738, 40);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageTo).Name = "CmbProductionStageTo";
		((System.Windows.Forms.Control)(object)this.CmbProductionStageTo).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageTo).TabIndex = 7;
		this.label2.AutoSize = true;
		this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.Location = new System.Drawing.Point(606, 18);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(130, 15);
		this.label2.TabIndex = 26;
		this.label2.Text = "Production Stage From";
		this.CmbProductionStageFrom.AllowNull = (DefaultableBoolean)1;
		this.CmbProductionStageFrom.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbProductionStageFrom.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbProductionStageFrom.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance179).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance179).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Appearance = (AppearanceBase)(object)appearance179;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance180).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance180).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance180).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance180).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance180;
		((AppearanceBase)appearance181).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance181;
		((SpecialBoxBase)((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance182).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance182).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance182).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance182).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance182;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance183).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance183).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance183;
		((AppearanceBase)appearance184).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance184).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance184;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance199).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance199;
		((AppearanceBase)appearance200).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance200).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance200;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance201).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance201).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance201).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance201).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance201).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance201;
		((AppearanceBase)appearance202).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance202;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance203).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance203).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance203;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance204).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance204;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbProductionStageFrom).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFrom).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFrom).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbProductionStageFrom.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFrom).Location = new System.Drawing.Point(738, 12);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFrom).Name = "CmbProductionStageFrom";
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFrom).Size = new System.Drawing.Size(224, 26);
		((System.Windows.Forms.Control)(object)this.CmbProductionStageFrom).TabIndex = 6;
		this.label1.AutoSize = true;
		this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.Location = new System.Drawing.Point(283, 46);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(83, 15);
		this.label1.TabIndex = 24;
		this.label1.Text = "Warehouse To";
		this.label4.AutoSize = true;
		this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label4.Location = new System.Drawing.Point(283, 18);
		this.label4.Name = "label4";
		this.label4.Size = new System.Drawing.Size(98, 15);
		this.label4.TabIndex = 22;
		this.label4.Text = "Warehouse From";
		this.label6.AutoSize = true;
		this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label6.Location = new System.Drawing.Point(6, 46);
		this.label6.Name = "label6";
		this.label6.Size = new System.Drawing.Size(77, 15);
		this.label6.TabIndex = 20;
		this.label6.Text = "Transfer Type";
		this.CmbTransferType.AllowNull = (DefaultableBoolean)1;
		this.CmbTransferType.AutoCompleteMode = (AutoCompleteMode)4;
		this.CmbTransferType.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.CmbTransferType.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance185).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance185).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Appearance = (AppearanceBase)(object)appearance185;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance187).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance187).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance187).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance187).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance187;
		((AppearanceBase)appearance188).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance188;
		((SpecialBoxBase)((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance189).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance189).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance189).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance189).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance189;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance190).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance190).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance190;
		((AppearanceBase)appearance191).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance191).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance191;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance192).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance192;
		((AppearanceBase)appearance193).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance193).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance193;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance194).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance194).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance194).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance194).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance194).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance194;
		((AppearanceBase)appearance195).TextHAlignAsString = "Left";
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance195;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance196).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance196).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance196;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance198).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance198;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.CmbTransferType).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Enabled = false;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Font = new System.Drawing.Font("Verdana", 9.75f);
		this.CmbTransferType.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Location = new System.Drawing.Point(84, 40);
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Name = "CmbTransferType";
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Size = new System.Drawing.Size(195, 26);
		((System.Windows.Forms.Control)(object)this.CmbTransferType).TabIndex = 2;
		((System.Windows.Forms.Control)(object)this.CmbTransferType).Leave += new System.EventHandler(CmbTransferType_Leave);
		this.DocDate.CustomFormat = "dd-MMM-yy";
		this.DocDate.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.DocDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.DocDate.Location = new System.Drawing.Point(162, 14);
		this.DocDate.Name = "DocDate";
		this.DocDate.Size = new System.Drawing.Size(117, 23);
		this.DocDate.TabIndex = 1;
		this.DocDate.ValueChanged += new System.EventHandler(DocDate_ValueChanged);
		this.txtdocno.BackColor = System.Drawing.Color.White;
		this.txtdocno.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtdocno.Enabled = false;
		this.txtdocno.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtdocno.Location = new System.Drawing.Point(84, 14);
		this.txtdocno.Name = "txtdocno";
		this.txtdocno.ReadOnly = true;
		this.txtdocno.Size = new System.Drawing.Size(76, 23);
		this.txtdocno.TabIndex = 0;
		this.txtdocno.Leave += new System.EventHandler(txtdocno_Leave);
		this.label17.AutoSize = true;
		this.label17.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label17.Location = new System.Drawing.Point(6, 18);
		this.label17.Name = "label17";
		this.label17.Size = new System.Drawing.Size(76, 15);
		this.label17.TabIndex = 3;
		this.label17.Text = "Doc No,Date";
		this.label15.AutoSize = true;
		this.label15.BackColor = System.Drawing.Color.Transparent;
		this.label15.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label15.Location = new System.Drawing.Point(6, 73);
		this.label15.Name = "label15";
		this.label15.Size = new System.Drawing.Size(52, 15);
		this.label15.TabIndex = 17;
		this.label15.Text = "Remarks";
		this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.txtRemarksHead.BackColor = System.Drawing.Color.White;
		this.txtRemarksHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtRemarksHead.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtRemarksHead.Location = new System.Drawing.Point(84, 69);
		this.txtRemarksHead.Name = "txtRemarksHead";
		this.txtRemarksHead.Size = new System.Drawing.Size(878, 23);
		this.txtRemarksHead.TabIndex = 3;
		this.panel2.BackColor = System.Drawing.Color.Teal;
		this.panel2.Controls.Add(this.ChkBoxPrintPreview);
		this.panel2.Controls.Add(this.label24);
		this.panel2.Controls.Add(this.toolStrip1);
		this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel2.Location = new System.Drawing.Point(2, 2);
		this.panel2.Margin = new System.Windows.Forms.Padding(2);
		this.panel2.Name = "panel2";
		this.panel2.Size = new System.Drawing.Size(1077, 50);
		this.panel2.TabIndex = 0;
		this.ChkBoxPrintPreview.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ChkBoxPrintPreview.AutoSize = true;
		this.ChkBoxPrintPreview.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.ChkBoxPrintPreview.ForeColor = System.Drawing.Color.White;
		this.ChkBoxPrintPreview.Location = new System.Drawing.Point(988, 29);
		this.ChkBoxPrintPreview.Name = "ChkBoxPrintPreview";
		this.ChkBoxPrintPreview.Size = new System.Drawing.Size(84, 20);
		this.ChkBoxPrintPreview.TabIndex = 6;
		this.ChkBoxPrintPreview.Text = "Preview";
		this.ChkBoxPrintPreview.UseVisualStyleBackColor = true;
		this.label24.AutoSize = true;
		this.label24.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label24.ForeColor = System.Drawing.Color.White;
		this.label24.Location = new System.Drawing.Point(4, 28);
		this.label24.Name = "label24";
		this.label24.Size = new System.Drawing.Size(180, 19);
		this.label24.TabIndex = 35;
		this.label24.Text = "Production Rejection";
		this.toolStrip1.BackColor = System.Drawing.Color.White;
		this.toolStrip1.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[15]
		{
			this.btnnew, this.btnRefresh, this.toolStripSeparator2, this.btnsave, this.btnSaveAs, this.btnupdate, this.btnDelete, this.toolStripSeparator5, this.btnAttachment, this.toolStripSeparator3,
			this.btnprint, this.toolStripSeparator4, this.BtnLoadStock, this.btnshortcutkeys, this.toolStripSeparator1
		});
		this.toolStrip1.Location = new System.Drawing.Point(0, 0);
		this.toolStrip1.Name = "toolStrip1";
		this.toolStrip1.Size = new System.Drawing.Size(1077, 27);
		this.toolStrip1.TabIndex = 0;
		this.toolStrip1.Text = "toolStrip1";
		this.btnnew.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnnew.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnnew.Name = "btnnew";
		this.btnnew.Size = new System.Drawing.Size(59, 24);
		this.btnnew.Text = "&New";
		this.btnnew.Click += new System.EventHandler(btnnew_Click);
		this.btnRefresh.Image = Architecture.WinApp.Properties.Resources.referesf;
		this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnRefresh.Name = "btnRefresh";
		this.btnRefresh.Size = new System.Drawing.Size(80, 24);
		this.btnRefresh.Text = "Refresh";
		this.btnRefresh.Click += new System.EventHandler(btnRefresh_Click);
		this.toolStripSeparator2.Name = "toolStripSeparator2";
		this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
		this.btnsave.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnsave.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnsave.Name = "btnsave";
		this.btnsave.Size = new System.Drawing.Size(64, 24);
		this.btnsave.Text = "&Save";
		this.btnsave.Click += new System.EventHandler(btnsave_Click);
		this.btnSaveAs.Image = Architecture.WinApp.Properties.Resources.icons8_save_16;
		this.btnSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnSaveAs.Name = "btnSaveAs";
		this.btnSaveAs.Size = new System.Drawing.Size(80, 24);
		this.btnSaveAs.Text = "SaveAs";
		this.btnSaveAs.Visible = false;
		this.btnSaveAs.Click += new System.EventHandler(btnSaveAs_Click);
		this.btnupdate.Image = Architecture.WinApp.Properties.Resources.icons8_pencil_16;
		this.btnupdate.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnupdate.Name = "btnupdate";
		this.btnupdate.Size = new System.Drawing.Size(78, 24);
		this.btnupdate.Text = "&Update";
		this.btnupdate.Visible = false;
		this.btnupdate.Click += new System.EventHandler(btnupdate_Click);
		this.btnDelete.Image = Architecture.WinApp.Properties.Resources.delete_bin_2_line;
		this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnDelete.Name = "btnDelete";
		this.btnDelete.Size = new System.Drawing.Size(73, 24);
		this.btnDelete.Text = "Delete";
		this.btnDelete.Visible = false;
		this.btnDelete.Click += new System.EventHandler(btnDelete_Click);
		this.toolStripSeparator5.Name = "toolStripSeparator5";
		this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
		this.btnAttachment.Image = Architecture.WinApp.Properties.Resources.icons8_attach_16;
		this.btnAttachment.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnAttachment.Name = "btnAttachment";
		this.btnAttachment.Size = new System.Drawing.Size(109, 24);
		this.btnAttachment.Text = "Attachment";
		this.btnAttachment.Click += new System.EventHandler(btnAttachment_Click);
		this.toolStripSeparator3.Name = "toolStripSeparator3";
		this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
		this.btnprint.Image = Architecture.WinApp.Properties.Resources.icons8_print_16;
		this.btnprint.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnprint.Name = "btnprint";
		this.btnprint.Size = new System.Drawing.Size(100, 24);
		this.btnprint.Text = "1663-Print";
		this.btnprint.Click += new System.EventHandler(btnprint_Click);
		this.toolStripSeparator4.Name = "toolStripSeparator4";
		this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
		this.BtnLoadStock.Image = Architecture.WinApp.Properties.Resources.download_line;
		this.BtnLoadStock.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.BtnLoadStock.Name = "BtnLoadStock";
		this.BtnLoadStock.Size = new System.Drawing.Size(105, 24);
		this.BtnLoadStock.Text = "Load Stock";
		this.BtnLoadStock.Click += new System.EventHandler(BtnLoadStock_Click);
		this.btnshortcutkeys.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnshortcutkeys.Image = Architecture.WinApp.Properties.Resources._55download;
		this.btnshortcutkeys.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnshortcutkeys.Name = "btnshortcutkeys";
		this.btnshortcutkeys.Size = new System.Drawing.Size(126, 24);
		this.btnshortcutkeys.Text = "ShortCut Keys";
		this.toolStripSeparator1.Name = "toolStripSeparator1";
		this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
		this.tabPage2.Controls.Add(this.panel7);
		this.tabPage2.Location = new System.Drawing.Point(4, 4);
		this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
		this.tabPage2.Name = "tabPage2";
		this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
		this.tabPage2.Size = new System.Drawing.Size(1081, 652);
		this.tabPage2.TabIndex = 1;
		this.tabPage2.Text = "History";
		this.tabPage2.UseVisualStyleBackColor = true;
		this.panel7.Controls.Add(this.panel9);
		this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel7.Location = new System.Drawing.Point(2, 2);
		this.panel7.Margin = new System.Windows.Forms.Padding(2);
		this.panel7.Name = "panel7";
		this.panel7.Size = new System.Drawing.Size(1077, 648);
		this.panel7.TabIndex = 0;
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.grdhistory);
		this.panel9.Controls.Add(this.panel14);
		this.panel9.Controls.Add((System.Windows.Forms.Control)(object)this.GridDetailHistory);
		this.panel9.Controls.Add(this.panel15);
		this.panel9.Controls.Add(this.panel8);
		this.panel9.Controls.Add(this.panel13);
		this.panel9.Controls.Add(this.toolStrip3);
		this.panel9.Dock = System.Windows.Forms.DockStyle.Fill;
		this.panel9.Location = new System.Drawing.Point(0, 0);
		this.panel9.Name = "panel9";
		this.panel9.Size = new System.Drawing.Size(1077, 648);
		this.panel9.TabIndex = 47;
		this.grdhistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.grdhistory).BackColor = System.Drawing.Color.White;
		grdhistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.grdhistory.DesignTimeLayout = grdhistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.grdhistory).Dock = System.Windows.Forms.DockStyle.Fill;
		this.grdhistory.DynamicFiltering = true;
		this.grdhistory.FilterMode = (FilterMode)2;
		this.grdhistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.grdhistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.grdhistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.grdhistory).Location = new System.Drawing.Point(0, 127);
		((System.Windows.Forms.Control)(object)this.grdhistory).Name = "grdhistory";
		((ScrollableBase)this.grdhistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.grdhistory).Size = new System.Drawing.Size(1077, 288);
		((System.Windows.Forms.Control)(object)this.grdhistory).TabIndex = 1;
		this.grdhistory.TotalRow = (InheritableBoolean)1;
		this.grdhistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.grdhistory).VisualStyle = (VisualStyle)4;
		this.grdhistory.ColumnButtonClick += new ColumnActionEventHandler(grdhistory_ColumnButtonClick);
		this.grdhistory.SelectionChanged += new System.EventHandler(grdhistory_SelectionChanged);
		this.grdhistory.LinkClicked += new ColumnActionEventHandler(grdhistory_LinkClicked);
		((System.Windows.Forms.Control)(object)this.grdhistory).DoubleClick += new System.EventHandler(grdhistory_DoubleClick_1);
		((System.Windows.Forms.Control)(object)this.grdhistory).KeyDown += new System.Windows.Forms.KeyEventHandler(grdHistory_KeyDown);
		this.panel14.BackColor = System.Drawing.Color.Teal;
		this.panel14.Controls.Add(this.label27);
		this.panel14.Dock = System.Windows.Forms.DockStyle.Bottom;
		this.panel14.Location = new System.Drawing.Point(0, 415);
		this.panel14.Name = "panel14";
		this.panel14.Size = new System.Drawing.Size(1077, 21);
		this.panel14.TabIndex = 54;
		this.label27.AutoSize = true;
		this.label27.Font = new System.Drawing.Font("Tahoma", 8f, System.Drawing.FontStyle.Bold);
		this.label27.ForeColor = System.Drawing.Color.White;
		this.label27.Location = new System.Drawing.Point(4, 4);
		this.label27.Name = "label27";
		this.label27.Size = new System.Drawing.Size(244, 13);
		this.label27.TabIndex = 37;
		this.label27.Text = "Detail Information Of Above Selected Row";
		this.GridDetailHistory.AllowEdit = (InheritableBoolean)2;
		((ScrollableBase)this.GridDetailHistory).BackColor = System.Drawing.Color.White;
		GridDetailHistory_DesignTimeLayout.LayoutString = "<GridEXLayoutData><RootTable><GroupCondition /></RootTable></GridEXLayoutData>";
		this.GridDetailHistory.DesignTimeLayout = GridDetailHistory_DesignTimeLayout;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Dock = System.Windows.Forms.DockStyle.Bottom;
		this.GridDetailHistory.DynamicFiltering = true;
		this.GridDetailHistory.FilterMode = (FilterMode)2;
		this.GridDetailHistory.FilterRowButtonStyle = (FilterRowButtonStyle)3;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Font = new System.Drawing.Font("Verdana", 8.25f);
		this.GridDetailHistory.GroupByBoxVisible = false;
		this.GridDetailHistory.GroupTotals = (GroupTotals)2;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Location = new System.Drawing.Point(0, 436);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Name = "GridDetailHistory";
		((ScrollableBase)this.GridDetailHistory).RecordNavigator = true;
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).Size = new System.Drawing.Size(1077, 212);
		((System.Windows.Forms.Control)(object)this.GridDetailHistory).TabIndex = 2;
		this.GridDetailHistory.TotalRow = (InheritableBoolean)1;
		this.GridDetailHistory.TotalRowPosition = (TotalRowPosition)1;
		((ScrollableBase)this.GridDetailHistory).VisualStyle = (VisualStyle)4;
		this.panel15.BackColor = System.Drawing.Color.Teal;
		this.panel15.Controls.Add(this.label9);
		this.panel15.Controls.Add(this.ctrlGrdBar3);
		this.panel15.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel15.Location = new System.Drawing.Point(0, 105);
		this.panel15.Name = "panel15";
		this.panel15.Size = new System.Drawing.Size(1077, 22);
		this.panel15.TabIndex = 53;
		this.label9.AutoSize = true;
		this.label9.Font = new System.Drawing.Font("Tahoma", 10f, System.Drawing.FontStyle.Bold);
		this.label9.ForeColor = System.Drawing.Color.White;
		this.label9.Location = new System.Drawing.Point(0, 1);
		this.label9.Name = "label9";
		this.label9.Size = new System.Drawing.Size(119, 17);
		this.label9.TabIndex = 36;
		this.label9.Text = "Filtered Records";
		this.ctrlGrdBar3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.ctrlGrdBar3.FormName = this;
		this.ctrlGrdBar3.Location = new System.Drawing.Point(1034, 0);
		this.ctrlGrdBar3.Margin = new System.Windows.Forms.Padding(4);
		this.ctrlGrdBar3.MyGrid = this.grdhistory;
		this.ctrlGrdBar3.Name = "ctrlGrdBar3";
		this.ctrlGrdBar3.Size = new System.Drawing.Size(37, 21);
		this.ctrlGrdBar3.TabIndex = 23;
		this.ctrlGrdBar3.Load += new System.EventHandler(ctrlGrdBar3_Load);
		this.panel8.Controls.Add(this.groupBox3);
		this.panel8.Controls.Add(this.bg);
		this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel8.Location = new System.Drawing.Point(0, 53);
		this.panel8.Name = "panel8";
		this.panel8.Size = new System.Drawing.Size(1077, 52);
		this.panel8.TabIndex = 0;
		this.groupBox3.Controls.Add(this.ToDate);
		this.groupBox3.Controls.Add(this.fromdate);
		this.groupBox3.Controls.Add((System.Windows.Forms.Control)(object)this.cmbperemeter);
		this.groupBox3.Controls.Add(this.btnshow);
		this.groupBox3.Controls.Add(this.label20);
		this.groupBox3.Controls.Add(this.label7);
		this.groupBox3.Controls.Add(this.label34);
		this.groupBox3.Controls.Add(this.label8);
		this.groupBox3.Controls.Add(this.label37);
		this.groupBox3.Controls.Add(this.txtDocNoTo);
		this.groupBox3.Controls.Add(this.txtDocNoFrom);
		this.groupBox3.Font = new System.Drawing.Font("Tahoma", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.groupBox3.Location = new System.Drawing.Point(2, 2);
		this.groupBox3.Name = "groupBox3";
		this.groupBox3.Size = new System.Drawing.Size(923, 47);
		this.groupBox3.TabIndex = 0;
		this.groupBox3.TabStop = false;
		this.groupBox3.Text = "Main";
		this.ToDate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.ToDate.CustomFormat = "dd-MMM-yy";
		this.ToDate.Font = new System.Drawing.Font("Verdana", 9f);
		this.ToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.ToDate.Location = new System.Drawing.Point(453, 16);
		this.ToDate.Name = "ToDate";
		this.ToDate.ShowCheckBox = true;
		this.ToDate.Size = new System.Drawing.Size(126, 22);
		this.ToDate.TabIndex = 2;
		this.fromdate.CalendarFont = new System.Drawing.Font("Microsoft Sans Serif", 10.8f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.fromdate.CustomFormat = "dd-MMM-yy";
		this.fromdate.Font = new System.Drawing.Font("Verdana", 9f);
		this.fromdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
		this.fromdate.Location = new System.Drawing.Point(277, 16);
		this.fromdate.Name = "fromdate";
		this.fromdate.ShowCheckBox = true;
		this.fromdate.Size = new System.Drawing.Size(126, 22);
		this.fromdate.TabIndex = 1;
		this.cmbperemeter.AllowNull = (DefaultableBoolean)1;
		this.cmbperemeter.AutoCompleteMode = (AutoCompleteMode)4;
		this.cmbperemeter.AutoSuggestFilterMode = (AutoSuggestFilterMode)2;
		this.cmbperemeter.CheckedListSettings.CheckStateMember = "";
		((AppearanceBase)appearance205).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance205).BorderColor = System.Drawing.SystemColors.InactiveCaption;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Appearance = (AppearanceBase)(object)appearance205;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.BorderStyle = (UIElementBorderStyle)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.CaptionVisible = (DefaultableBoolean)2;
		((AppearanceBase)appearance206).BackColor = System.Drawing.SystemColors.ActiveBorder;
		((AppearanceBase)appearance206).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance206).BackGradientStyle = (GradientStyle)2;
		((AppearanceBase)appearance206).BorderColor = System.Drawing.SystemColors.Window;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).Appearance = (AppearanceBase)(object)appearance206;
		((AppearanceBase)appearance207).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.BandLabelAppearance = (AppearanceBase)(object)appearance207;
		((SpecialBoxBase)((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox).BorderStyle = (UIElementBorderStyle)4;
		((AppearanceBase)appearance209).BackColor = System.Drawing.SystemColors.ControlLightLight;
		((AppearanceBase)appearance209).BackColor2 = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance209).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance209).ForeColor = System.Drawing.SystemColors.GrayText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.GroupByBox.PromptAppearance = (AppearanceBase)(object)appearance209;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxColScrollRegions = 1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.MaxRowScrollRegions = 1;
		((AppearanceBase)appearance210).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance210).ForeColor = System.Drawing.SystemColors.ControlText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveCellAppearance = (AppearanceBase)(object)appearance210;
		((AppearanceBase)appearance211).BackColor = System.Drawing.SystemColors.Highlight;
		((AppearanceBase)appearance211).ForeColor = System.Drawing.SystemColors.HighlightText;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.ActiveRowAppearance = (AppearanceBase)(object)appearance211;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleCell = (UIElementBorderStyle)2;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.BorderStyleRow = (UIElementBorderStyle)2;
		((AppearanceBase)appearance225).BackColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CardAreaAppearance = (AppearanceBase)(object)appearance225;
		((AppearanceBase)appearance226).BorderColor = System.Drawing.Color.Silver;
		((AppearanceBase)appearance226).TextTrimming = (TextTrimming)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellAppearance = (AppearanceBase)(object)appearance226;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellClickAction = (CellClickAction)4;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.CellPadding = 0;
		((AppearanceBase)appearance227).BackColor = System.Drawing.SystemColors.Control;
		((AppearanceBase)appearance227).BackColor2 = System.Drawing.SystemColors.ControlDark;
		((AppearanceBase)appearance227).BackGradientAlignment = (GradientAlignment)1;
		((AppearanceBase)appearance227).BackGradientStyle = (GradientStyle)3;
		((AppearanceBase)appearance227).BorderColor = System.Drawing.SystemColors.Window;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.GroupByRowAppearance = (AppearanceBase)(object)appearance227;
		((AppearanceBase)appearance38).TextHAlignAsString = "Left";
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderAppearance = (AppearanceBase)(object)appearance38;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderClickAction = (HeaderClickAction)3;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.HeaderStyle = (HeaderStyle)2;
		((AppearanceBase)appearance39).BackColor = System.Drawing.SystemColors.Window;
		((AppearanceBase)appearance39).BorderColor = System.Drawing.Color.Silver;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowAppearance = (AppearanceBase)(object)appearance39;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.RowSelectors = (DefaultableBoolean)2;
		((AppearanceBase)appearance40).BackColor = System.Drawing.SystemColors.ControlLight;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.Override.TemplateAddRowAppearance = (AppearanceBase)(object)appearance40;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollBounds = (ScrollBounds)0;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ScrollStyle = (ScrollStyle)1;
		((UltraGridBase)this.cmbperemeter).DisplayLayout.ViewStyleBand = (ViewStyleBand)2;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.cmbperemeter.LimitToList = true;
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Location = new System.Drawing.Point(64, 14);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Name = "cmbperemeter";
		((System.Windows.Forms.Control)(object)this.cmbperemeter).Size = new System.Drawing.Size(148, 26);
		((System.Windows.Forms.Control)(object)this.cmbperemeter).TabIndex = 0;
		this.cmbperemeter.ValueChanged += new System.EventHandler(cmbperemeter_ValueChanged);
		this.btnshow.BackColor = System.Drawing.Color.Teal;
		this.btnshow.FlatAppearance.BorderSize = 0;
		this.btnshow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		this.btnshow.Font = new System.Drawing.Font("Verdana", 9f, System.Drawing.FontStyle.Bold);
		this.btnshow.ForeColor = System.Drawing.Color.White;
		this.btnshow.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
		this.btnshow.Location = new System.Drawing.Point(845, 16);
		this.btnshow.Margin = new System.Windows.Forms.Padding(2);
		this.btnshow.Name = "btnshow";
		this.btnshow.Size = new System.Drawing.Size(55, 23);
		this.btnshow.TabIndex = 5;
		this.btnshow.Text = "Show";
		this.btnshow.UseVisualStyleBackColor = false;
		this.btnshow.Click += new System.EventHandler(btnshow_Click);
		this.label20.AutoSize = true;
		this.label20.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label20.Location = new System.Drawing.Point(4, 20);
		this.label20.Name = "label20";
		this.label20.Size = new System.Drawing.Size(60, 15);
		this.label20.TabIndex = 592;
		this.label20.Text = "Date Type";
		this.label7.AutoSize = true;
		this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label7.Location = new System.Drawing.Point(405, 20);
		this.label7.Name = "label7";
		this.label7.Size = new System.Drawing.Size(48, 15);
		this.label7.TabIndex = 12;
		this.label7.Text = "To Date";
		this.label34.AutoSize = true;
		this.label34.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label34.Location = new System.Drawing.Point(214, 20);
		this.label34.Name = "label34";
		this.label34.Size = new System.Drawing.Size(63, 15);
		this.label34.TabIndex = 12;
		this.label34.Text = "From Date";
		this.label8.AutoSize = true;
		this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label8.Location = new System.Drawing.Point(721, 20);
		this.label8.Name = "label8";
		this.label8.Size = new System.Drawing.Size(45, 15);
		this.label8.TabIndex = 28;
		this.label8.Text = "To Doc";
		this.label37.AutoSize = true;
		this.label37.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.label37.Location = new System.Drawing.Point(582, 20);
		this.label37.Name = "label37";
		this.label37.Size = new System.Drawing.Size(60, 15);
		this.label37.TabIndex = 28;
		this.label37.Text = "From Doc";
		this.txtDocNoTo.BackColor = System.Drawing.Color.White;
		this.txtDocNoTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoTo.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNoTo.Location = new System.Drawing.Point(768, 16);
		this.txtDocNoTo.Name = "txtDocNoTo";
		this.txtDocNoTo.ReadOnly = true;
		this.txtDocNoTo.Size = new System.Drawing.Size(75, 23);
		this.txtDocNoTo.TabIndex = 4;
		this.txtDocNoTo.TabStop = false;
		this.txtDocNoTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoTo_KeyPress);
		this.txtDocNoFrom.BackColor = System.Drawing.Color.White;
		this.txtDocNoFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
		this.txtDocNoFrom.Font = new System.Drawing.Font("Verdana", 9.75f);
		this.txtDocNoFrom.Location = new System.Drawing.Point(643, 16);
		this.txtDocNoFrom.Name = "txtDocNoFrom";
		this.txtDocNoFrom.ReadOnly = true;
		this.txtDocNoFrom.Size = new System.Drawing.Size(75, 23);
		this.txtDocNoFrom.TabIndex = 3;
		this.txtDocNoFrom.TabStop = false;
		this.txtDocNoFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(txtDocNoFrom_KeyPress);
		this.bg.AutoSize = true;
		this.bg.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold);
		this.bg.Location = new System.Drawing.Point(6, 11);
		this.bg.Name = "bg";
		this.bg.Size = new System.Drawing.Size(0, 15);
		this.bg.TabIndex = 164;
		this.panel13.BackColor = System.Drawing.Color.Teal;
		this.panel13.Controls.Add(this.label25);
		this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
		this.panel13.Location = new System.Drawing.Point(0, 27);
		this.panel13.Name = "panel13";
		this.panel13.Size = new System.Drawing.Size(1077, 26);
		this.panel13.TabIndex = 49;
		this.label25.AutoSize = true;
		this.label25.Font = new System.Drawing.Font("Tahoma", 12f, System.Drawing.FontStyle.Bold);
		this.label25.ForeColor = System.Drawing.Color.White;
		this.label25.Location = new System.Drawing.Point(3, 3);
		this.label25.Name = "label25";
		this.label25.Size = new System.Drawing.Size(317, 19);
		this.label25.TabIndex = 36;
		this.label25.Text = "Production Move (Stage Shift) History";
		this.toolStrip3.BackColor = System.Drawing.Color.White;
		this.toolStrip3.ImageScalingSize = new System.Drawing.Size(20, 20);
		this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[1] { this.btnNewHistory });
		this.toolStrip3.Location = new System.Drawing.Point(0, 0);
		this.toolStrip3.Name = "toolStrip3";
		this.toolStrip3.Size = new System.Drawing.Size(1077, 27);
		this.toolStrip3.TabIndex = 48;
		this.toolStrip3.Text = "toolStrip3";
		this.btnNewHistory.Font = new System.Drawing.Font("Verdana", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.btnNewHistory.Image = Architecture.WinApp.Properties.Resources.icons8_add_20;
		this.btnNewHistory.ImageTransparentColor = System.Drawing.Color.Magenta;
		this.btnNewHistory.Name = "btnNewHistory";
		this.btnNewHistory.Size = new System.Drawing.Size(59, 24);
		this.btnNewHistory.Text = "&New";
		this.btnNewHistory.Click += new System.EventHandler(btnNewHistory_Click);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		this.BackColor = System.Drawing.Color.White;
		base.ClientSize = new System.Drawing.Size(1089, 681);
		base.Controls.Add(this.panel1);
		base.HelpButton = true;
		base.KeyPreview = true;
		base.Margin = new System.Windows.Forms.Padding(2);
		base.Name = "frmStockTransferForProductionRejection";
		this.Text = "EccountBook ERP";
		base.WindowState = System.Windows.Forms.FormWindowState.Maximized;
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(frmStockTransferManual_FormClosing);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(PurchsaeOrder_KeyDown);
		this.panel1.ResumeLayout(false);
		this.tabControl1.ResumeLayout(false);
		this.tabPage1.ResumeLayout(false);
		this.panel6.ResumeLayout(false);
		this.panel3.ResumeLayout(false);
		this.tabControl2.ResumeLayout(false);
		this.tabPage3.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.grd).EndInit();
		this.DetailManualEntryPanel.ResumeLayout(false);
		this.DetailManualEntryPanel.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbRejectionReasonType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.txtBatchNo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCastingType).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbRateUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWorkStationFromDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageToDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageFromDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseToDetail).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbSampleModal).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbVariantDescription).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbCategory).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbItem).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbPackUom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWareHouseFromDetail).EndInit();
		this.panel11.ResumeLayout(false);
		this.panel4.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		this.groupBox1.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbWarehouseFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageTo).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbProductionStageFrom).EndInit();
		((System.ComponentModel.ISupportInitialize)this.CmbTransferType).EndInit();
		this.panel2.ResumeLayout(false);
		this.panel2.PerformLayout();
		this.toolStrip1.ResumeLayout(false);
		this.toolStrip1.PerformLayout();
		this.tabPage2.ResumeLayout(false);
		this.panel7.ResumeLayout(false);
		this.panel9.ResumeLayout(false);
		this.panel9.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.grdhistory).EndInit();
		this.panel14.ResumeLayout(false);
		this.panel14.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.GridDetailHistory).EndInit();
		this.panel15.ResumeLayout(false);
		this.panel15.PerformLayout();
		this.panel8.ResumeLayout(false);
		this.panel8.PerformLayout();
		this.groupBox3.ResumeLayout(false);
		this.groupBox3.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.cmbperemeter).EndInit();
		this.panel13.ResumeLayout(false);
		this.panel13.PerformLayout();
		this.toolStrip3.ResumeLayout(false);
		this.toolStrip3.PerformLayout();
		base.ResumeLayout(false);
	}
}
